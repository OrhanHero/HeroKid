using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Datenbank-Wartung aus dem Eltern-Bereich heraus: komplettes Zurücksetzen (alle Profile,
/// Fortschritte, Aktivitätsprotokolle, Quiz-Historie und Einstellungen - siehe
/// Bestätigungsdialog in ParentSettingsViewModel.ResetAllDataAsync) sowie Backup-Export/-Import.
/// Die lerntor.db ist ein Single Point of Failure (Plattendefekt oder versehentliches Löschen =
/// alle Fortschritte und Sterne weg) - der Export erzeugt deshalb eine vollständige, konsistente
/// Kopie z.B. auf einen USB-Stick, der Import spielt sie zurück.
/// </summary>
public sealed class DatabaseMaintenanceRepository
{
    private readonly LernTorDbContext _db;

    public DatabaseMaintenanceRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Werkseinstellungen: leert ALLE Tabellen.
    ///
    /// <para>Hier standen lange nur sechs Tabellen, obwohl der Bestätigungsdialog "alle Profile,
    /// Fortschritte, Aktivitätsprotokolle und Einstellungen" verspricht. Belohnungen, eingelöste
    /// Belohnungen, Tipptrainer-Fortschritt, eigene Lesetexte, Vokabeln, Fehler-Kartei,
    /// gemeisterte Aufgaben und das Nachrichten-Archiv blieben stehen - nach dem Zurücksetzen
    /// tauchten also die Sterne und Fehler des gelöschten Kindes beim neuen wieder auf.</para>
    ///
    /// <para><b>Deshalb keine Liste mehr, sondern das Modell selbst.</b> Die Liste wurde nach
    /// dieser Lektion noch einmal vergessen: Führerschein-Fortschritt, Theorie-Antworten,
    /// Theorie-Prüfungen, Kurs-Fortschritt und beide Stundenplan-Tabellen kamen später dazu und
    /// überlebten das Zurücksetzen (gefunden am 28.09.2026). Gelöscht wird jetzt jede Tabelle,
    /// die EF kennt - eine neue Tabelle ist automatisch dabei.</para>
    /// </summary>
    public async Task ResetAllDataAsync(CancellationToken cancellationToken = default)
    {
        var profilTabelle = _db.Model.FindEntityType(typeof(Entities.StudentProfileEntity))?.GetTableName();

        // Profile zuletzt: alles andere haengt per ProfileId daran.
        var tabellen = AllTableNames()
            .OrderBy(tabelle => tabelle == profilTabelle ? 1 : 0)
            .ToList();

        foreach (var tabelle in tabellen)
        {
            // Tabellennamen stammen aus dem EF-Modell, nicht aus einer Eingabe - kein
            // Injektionsweg. Bewusst verkettet statt interpoliert (EF1002).
            var sql = "DELETE FROM \"" + tabelle + "\"";
            await _db.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
    }

    /// <summary>Die Namen aller Tabellen, die das EF-Modell kennt - je Tabelle einmal.</summary>
    public IReadOnlyList<string> AllTableNames() =>
        _db.Model.GetEntityTypes()
            .Select(typ => typ.GetTableName())
            .OfType<string>()
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// Prüft die Datenbankdatei mit SQLites <c>PRAGMA integrity_check</c>. Eine beschädigte
    /// Datei zeigt sich sonst erst als scheinbar zufälliger Absturz irgendwo im Tagesablauf;
    /// hier bekommen die Eltern auf Knopfdruck eine Antwort im Klartext.
    /// </summary>
    public async Task<DatabaseIntegrityResult> CheckIntegrityAsync(CancellationToken cancellationToken = default)
    {
        var connection = _db.Database.GetDbConnection();
        var wasOpen = connection.State == System.Data.ConnectionState.Open;

        if (!wasOpen)
        {
            await connection.OpenAsync(cancellationToken);
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA integrity_check";

            var meldungen = new List<string>();
            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    meldungen.Add(reader.GetString(0));
                }
            }

            return new DatabaseIntegrityResult(meldungen);
        }
        finally
        {
            if (!wasOpen)
            {
                await connection.CloseAsync();
            }
        }
    }

    /// <summary>
    /// Ob überhaupt schon ein Profil angelegt wurde - also ob in dieser Datenbank etwas steckt,
    /// das eine Sicherung wert wäre.
    ///
    /// <para>Bewusst rohes SQL statt <c>_db.Profiles.AnyAsync()</c>: die Frage wird beim App-Start
    /// gestellt, <b>bevor</b> der Schema-Abgleich gelaufen ist. Fehlt der Tabelle dann eine Spalte,
    /// die das aktuelle Modell erwartet, kippt eine EF-Abfrage - ausgerechnet an der Stelle, die
    /// den Datenverlust verhindern soll. Existiert die Tabelle noch gar nicht (frische
    /// Installation), lautet die Antwort schlicht "nein".</para>
    /// </summary>
    public bool HasAnyProfile()
    {
        var connection = _db.Database.GetDbConnection();
        var wasOpen = connection.State == System.Data.ConnectionState.Open;

        if (!wasOpen)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT CASE WHEN EXISTS(SELECT 1 FROM sqlite_master WHERE type='table' AND name='Profiles') " +
                "THEN (SELECT COUNT(*) FROM Profiles) ELSE 0 END";

            return Convert.ToInt32(command.ExecuteScalar() ?? 0) > 0;
        }
        finally
        {
            if (!wasOpen)
            {
                connection.Close();
            }
        }
    }

    /// <summary>
    /// Exportiert die komplette Datenbank als eigenständige .db-Datei. SQLites
    /// <c>VACUUM INTO</c> erzeugt dabei einen KONSISTENTEN Snapshot, auch während die App die
    /// Datenbank offen hat - im Gegensatz zu einem naiven File.Copy, das mitten in einer
    /// Transaktion eine korrupte Kopie ziehen könnte.
    /// </summary>
    public async Task ExportBackupAsync(string targetPath, CancellationToken cancellationToken = default)
    {
        // VACUUM INTO verweigert das Überschreiben einer bestehenden Datei - der Nutzer hat das
        // Überschreiben aber bereits im Speichern-Dialog bestätigt.
        if (File.Exists(targetPath))
        {
            File.Delete(targetPath);
        }

        // Pfad als SQL-Literal: Microsoft.Data.Sqlite kann VACUUM INTO nicht parametrisieren,
        // deshalb klassisches Escaping durch Verdoppeln von Hochkommata.
        // Der Befehl wird VOR dem Aufruf fertig zusammengesetzt: so sieht der EF-Analysator
        // (EF1002) keinen interpolierten String im Aufruf, und die Stelle, an der escaped wird,
        // steht direkt daneben statt versteckt in einer Interpolation.
        var escapedPath = targetPath.Replace("'", "''");
        var vacuumInto = "VACUUM INTO '" + escapedPath + "'";
        await _db.Database.ExecuteSqlRawAsync(vacuumInto, cancellationToken);
    }

    /// <summary>
    /// Ersetzt die aktive Datenbank durch die gewählte Backup-Datei. Statisch und bewusst OHNE
    /// den laufenden DbContext: vor dem Überschreiben werden alle gepoolten SQLite-Verbindungen
    /// geschlossen (sonst hält Windows die Datei gesperrt). Danach MUSS die App beendet/neu
    /// gestartet werden - der laufende Prozess hat sonst veraltete Daten im Speicher. Ein Backup
    /// mit älterem Schema ist unkritisch: der SqliteSchemaUpdater ergänzt fehlende
    /// Tabellen/Spalten beim nächsten Start (additive Updates, siehe docs/BUILD.md).
    /// </summary>
    /// <exception cref="InvalidDataException">Die Datei ist keine SQLite-Datenbank.</exception>
    public static void ImportBackup(string sourcePath) =>
        ImportBackup(sourcePath, LernTorDbContext.GetDefaultDbPath());

    /// <summary>
    /// Wie <see cref="ImportBackup(string)"/>, aber mit ausdrücklichem Ziel - damit der ganze Weg
    /// (Pool schließen, Datei ersetzen, Schema nachziehen) gegen eine Temp-Datei prüfbar ist, statt
    /// nur an der echten lerntor.db der Familie.
    /// </summary>
    public static void ImportBackup(string sourcePath, string targetPath)
    {
        if (!IsSqliteDatabase(sourcePath))
        {
            throw new InvalidDataException(
                "Die gewählte Datei ist keine LernTor-Datenbanksicherung (kein SQLite-Format).");
        }

        SqliteConnection.ClearAllPools();
        File.Copy(sourcePath, targetPath, overwrite: true);
    }

    private static bool IsSqliteDatabase(string path)
    {
        // Jede SQLite-Datei beginnt mit dem festen 16-Byte-Header "SQLite format 3\0".
        var expectedHeader = Encoding.ASCII.GetBytes("SQLite format 3\0");
        using var stream = File.OpenRead(path);
        var actualHeader = new byte[expectedHeader.Length];
        return stream.Read(actualHeader, 0, actualHeader.Length) == expectedHeader.Length &&
               actualHeader.AsSpan().SequenceEqual(expectedHeader);
    }
}

/// <summary>Ergebnis von <c>PRAGMA integrity_check</c>: eine einzige Zeile "ok" heißt gesund,
/// alles andere sind SQLites eigene Fehlermeldungen.</summary>
public sealed record DatabaseIntegrityResult(IReadOnlyList<string> Messages)
{
    public bool IsOk => Messages.Count == 1 && string.Equals(Messages[0], "ok", StringComparison.OrdinalIgnoreCase);
}
