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

    public async Task ResetAllDataAsync(CancellationToken cancellationToken = default)
    {
        await _db.Progress.ExecuteDeleteAsync(cancellationToken);
        await _db.ActivityLog.ExecuteDeleteAsync(cancellationToken);
        await _db.QuizAttempts.ExecuteDeleteAsync(cancellationToken);
        await _db.Settings.ExecuteDeleteAsync(cancellationToken);
        await _db.Profiles.ExecuteDeleteAsync(cancellationToken);
        await _db.CustomQuestions.ExecuteDeleteAsync(cancellationToken);
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
