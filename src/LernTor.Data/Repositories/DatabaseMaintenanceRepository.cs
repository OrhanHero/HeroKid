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
    /// tauchten also die Sterne und Fehler des gelöschten Kindes beim neuen wieder auf. Neue
    /// Tabellen gehören ausnahmslos in diese Liste.</para>
    /// </summary>
    public async Task ResetAllDataAsync(CancellationToken cancellationToken = default)
    {
        await _db.Progress.ExecuteDeleteAsync(cancellationToken);
        await _db.ActivityLog.ExecuteDeleteAsync(cancellationToken);
        await _db.QuizAttempts.ExecuteDeleteAsync(cancellationToken);
        await _db.Settings.ExecuteDeleteAsync(cancellationToken);
        await _db.CustomQuestions.ExecuteDeleteAsync(cancellationToken);
        await _db.ReviewQuestions.ExecuteDeleteAsync(cancellationToken);
        await _db.MasteredPrompts.ExecuteDeleteAsync(cancellationToken);
        await _db.ArchivedArticles.ExecuteDeleteAsync(cancellationToken);
        await _db.RewardRedemptions.ExecuteDeleteAsync(cancellationToken);
        await _db.Rewards.ExecuteDeleteAsync(cancellationToken);
        await _db.TypingLessonProgress.ExecuteDeleteAsync(cancellationToken);
        await _db.CustomReadingTexts.ExecuteDeleteAsync(cancellationToken);
        await _db.VocabularyEntries.ExecuteDeleteAsync(cancellationToken);
        await _db.HomeworkTasks.ExecuteDeleteAsync(cancellationToken);
        await _db.Exams.ExecuteDeleteAsync(cancellationToken);

        // Profile zuletzt: alles andere haengt per ProfileId daran.
        await _db.Profiles.ExecuteDeleteAsync(cancellationToken);
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
        var escapedPath = targetPath.Replace("'", "''");
        await _db.Database.ExecuteSqlRawAsync($"VACUUM INTO '{escapedPath}'", cancellationToken);
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
    public static void ImportBackup(string sourcePath)
    {
        if (!IsSqliteDatabase(sourcePath))
        {
            throw new InvalidDataException(
                "Die gewählte Datei ist keine LernTor-Datenbanksicherung (kein SQLite-Format).");
        }

        SqliteConnection.ClearAllPools();
        File.Copy(sourcePath, LernTorDbContext.GetDefaultDbPath(), overwrite: true);
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
