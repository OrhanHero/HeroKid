using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Sicherung und Wiederherstellung im Ganzen, gegen echte SQLite-Dateien.
///
/// <para>Bis hierhin prüfte <c>AutoBackupPolicyTests</c> nur Dateinamen, Aufbewahrung und
/// Fingerabdruck. Kein Test schrieb eine Sicherung und las sie zurück - ausgerechnet der Weg, der
/// die Daten der Kinder bewegt (Verbindungs-Pool schließen, Datei ersetzen, Schema nachziehen),
/// war ungeprüft. Er fällt sonst erst in dem Moment auf, in dem man ihn braucht.</para>
/// </summary>
public sealed class BackupRestoreTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-restore-{Guid.NewGuid():N}.db");
    private readonly string _backupPath = Path.Combine(Path.GetTempPath(), $"lerntor-sicherung-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext(bool ensureCreated = true)
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        if (ensureCreated)
        {
            db.Database.EnsureCreated();
        }

        return db;
    }

    private static long Zeilen(LernTorDbContext db, string tabelle)
    {
        var verbindung = db.Database.GetDbConnection();
        if (verbindung.State != System.Data.ConnectionState.Open)
        {
            verbindung.Open();
        }

        using var befehl = verbindung.CreateCommand();
        befehl.CommandText = "SELECT COUNT(*) FROM \"" + tabelle + "\"";
        return Convert.ToInt64(befehl.ExecuteScalar() ?? 0L);
    }

    /// <summary>Legt ein Kind mit Einstellungen, Stundenplan, Tagesfortschritt und
    /// Führerschein-Fortschritt an - aus möglichst vielen Tabellen etwas.</summary>
    private static async Task<string> KindAnlegenAsync(LernTorDbContext db)
    {
        var profile = new StudentProfileRepository(db);
        var kind = await profile.CreateAsync("Emirhan", 11, "6c", GradeLevel.Klasse6, "🦊");
        await profile.SetTimetableSubjectsEnabledAsync(kind.Id, false);

        await new TimetableRepository(db).ReplaceAsync(
            kind.Id,
            Timetable.DefaultPeriods,
            new[]
            {
                new TimetableLesson(DayOfWeek.Monday, 1, "GeWi"),
                new TimetableLesson(DayOfWeek.Tuesday, 3, "Ma", "YaDu")
            });

        var fortschritt = new ProgressRepository(db);
        var heute = await fortschritt.LoadOrCreateTodayAsync(kind.Id);
        heute.HasCompletedReading = true;
        await fortschritt.SaveAsync(heute);

        await new TrafficSignProgressRepository(db, NullLogger<TrafficSignProgressRepository>.Instance)
            .RecordAnswerAsync(kind.Id, "206", wasCorrect: true);

        return kind.Id;
    }

    [Fact]
    public async Task Sichern_zerstoeren_wiederherstellen_bringt_alles_zurueck()
    {
        string kindId;
        using (var db = CreateContext())
        {
            kindId = await KindAnlegenAsync(db);

            var wartung = new DatabaseMaintenanceRepository(db);
            await wartung.ExportBackupAsync(_backupPath);

            // Zerstören: alles weg, wie nach einem versehentlichen Zurücksetzen.
            await wartung.ResetAllDataAsync();
            Assert.Empty(await new StudentProfileRepository(db).GetAllAsync());
        }

        Assert.True(File.Exists(_backupPath));
        Assert.True(new FileInfo(_backupPath).Length > 0);

        DatabaseMaintenanceRepository.ImportBackup(_backupPath, _dbPath);

        using (var db = CreateContext(ensureCreated: false))
        {
            // Wie beim echten Neustart nach der Wiederherstellung.
            SqliteSchemaUpdater.Update(db);

            var kind = Assert.Single(await new StudentProfileRepository(db).GetAllAsync());
            Assert.Equal(kindId, kind.Id);
            Assert.Equal("Emirhan", kind.Name);
            Assert.Equal(GradeLevel.Klasse6, kind.GradeLevel);
            Assert.False(kind.TimetableSubjectsEnabled);

            var plan = await new TimetableRepository(db).GetForProfileAsync(kindId);
            Assert.Equal(2, plan.Lessons.Count);
            Assert.Contains(plan.Lessons, stunde => stunde.Subject == "Ma" && stunde.Teacher == "YaDu");

            var heute = await new ProgressRepository(db).LoadOrCreateTodayAsync(kindId);
            Assert.True(heute.HasCompletedReading);

            var schilder = await new TrafficSignProgressRepository(db, NullLogger<TrafficSignProgressRepository>.Instance)
                .GetAllAsync(kindId);
            Assert.True(schilder.ContainsKey("206"));
        }
    }

    [Fact]
    public async Task Eine_Sicherung_mit_aelterem_Schema_wird_beim_Einspielen_nachgezogen()
    {
        // Sicherung von "vor ein paar Updates": ohne die Stundenplan-Tabellen und ohne die
        // Spalte fuer die Faecherauswahl nach Stundenplan.
        using (var db = CreateContext())
        {
            await new StudentProfileRepository(db).CreateAsync("Batuhan", 14, "9a", GradeLevel.Klasse9, "🦁");
            db.Database.ExecuteSqlRaw("DROP TABLE \"TimetableLessons\"");
            db.Database.ExecuteSqlRaw("DROP TABLE \"TimetablePeriods\"");
            db.Database.ExecuteSqlRaw("ALTER TABLE \"Profiles\" DROP COLUMN \"TimetableSubjectsDisabled\"");

            await new DatabaseMaintenanceRepository(db).ExportBackupAsync(_backupPath);
            await db.Database.ExecuteSqlRawAsync("DELETE FROM \"Profiles\"");
        }

        DatabaseMaintenanceRepository.ImportBackup(_backupPath, _dbPath);

        using (var db = CreateContext(ensureCreated: false))
        {
            var ergaenzt = SqliteSchemaUpdater.Update(db);
            Assert.NotEmpty(ergaenzt);

            var kind = Assert.Single(await new StudentProfileRepository(db).GetAllAsync());
            Assert.Equal("Batuhan", kind.Name);

            // Neue Spalte mit DEFAULT 0 = "nicht abgeschaltet" = an - dafuer ist sie invertiert.
            Assert.True(kind.TimetableSubjectsEnabled);

            // Und die nachgezogenen Tabellen sind benutzbar.
            var stundenplan = new TimetableRepository(db);
            await stundenplan.ReplaceAsync(kind.Id, Timetable.DefaultPeriods, new[] { new TimetableLesson(DayOfWeek.Monday, 1, "Ch") });
            Assert.Single((await stundenplan.GetForProfileAsync(kind.Id)).Lessons);
        }
    }

    [Fact]
    public async Task Zuruecksetzen_leert_wirklich_jede_Tabelle()
    {
        // Die Liste der zu leerenden Tabellen wurde zweimal vergessen nachzupflegen. Seit sie
        // aus dem EF-Modell kommt, prueft dieser Test jede einzelne Tabelle.
        using var db = CreateContext();
        await KindAnlegenAsync(db);
        var wartung = new DatabaseMaintenanceRepository(db);

        Assert.Contains("TimetableLessons", wartung.AllTableNames());
        Assert.True(Zeilen(db, "TimetableLessons") > 0);

        await wartung.ResetAllDataAsync();

        Assert.All(wartung.AllTableNames(), tabelle => Assert.Equal(0L, Zeilen(db, tabelle)));
    }

    [Fact]
    public async Task Eine_gesunde_Datenbank_besteht_die_Pruefung()
    {
        using var db = CreateContext();
        await KindAnlegenAsync(db);

        var ergebnis = await new DatabaseMaintenanceRepository(db).CheckIntegrityAsync();

        Assert.True(ergebnis.IsOk);
    }

    [Fact]
    public void Fehlermeldungen_von_SQLite_gelten_nicht_als_gesund()
    {
        Assert.False(new DatabaseIntegrityResult(new[] { "*** in database main ***", "Page 3 is never used" }).IsOk);
        Assert.False(new DatabaseIntegrityResult(Array.Empty<string>()).IsOk);
    }

    [Fact]
    public void Eine_Datei_die_keine_Datenbank_ist_wird_abgelehnt()
    {
        File.WriteAllText(_backupPath, "Das ist keine Sicherung, sondern ein Einkaufszettel.");

        Assert.Throws<InvalidDataException>(() => DatabaseMaintenanceRepository.ImportBackup(_backupPath, _dbPath));
        Assert.False(File.Exists(_dbPath));
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();

        foreach (var pfad in new[] { _dbPath, _backupPath })
        {
            try
            {
                if (File.Exists(pfad))
                {
                    File.Delete(pfad);
                }
            }
            catch
            {
                // Temp-Datei ggf. noch gesperrt (Verbindungs-Pool) - das Temp-Verzeichnis räumt das OS auf.
            }
        }
    }
}
