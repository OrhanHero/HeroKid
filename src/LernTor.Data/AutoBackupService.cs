using LernTor.Core.Logging;
using LernTor.Core.Services;
using LernTor.Data.Repositories;

namespace LernTor.Data;

/// <summary>
/// Zieht automatische Sicherungen der lerntor.db in einen eigenen Unterordner und räumt alte
/// wieder weg (Regeln in <see cref="AutoBackupPolicy"/>).
///
/// <para>Jeder Fehler wird protokolliert und geschluckt: eine misslungene Sicherung darf den
/// App-Start niemals verhindern. Ein Kind, das wegen einer vollen Platte nicht lernen kann, wäre
/// ein schlechterer Zustand als eine fehlende Sicherungskopie.</para>
/// </summary>
public sealed class AutoBackupService
{
    private readonly string _backupDirectory;

    public AutoBackupService(string backupDirectory)
    {
        _backupDirectory = backupDirectory;
    }

    public string BackupDirectory => _backupDirectory;

    /// <summary>Alle vorhandenen automatischen Sicherungen, neueste zuerst.</summary>
    public IReadOnlyList<AutoBackupFile> List()
    {
        try
        {
            if (!Directory.Exists(_backupDirectory))
            {
                return Array.Empty<AutoBackupFile>();
            }

            return Directory.EnumerateFiles(_backupDirectory)
                .Select(path => new FileInfo(path))
                .Where(file => AutoBackupPolicy.IsOwnFile(file.Name))
                .Select(file => new AutoBackupFile(file.Name, new DateTimeOffset(file.LastWriteTime)))
                .OrderByDescending(file => file.CreatedAt)
                .ToList();
        }
        catch (IOException ex)
        {
            AppLog.Warn("Sicherung", $"Sicherungsordner nicht lesbar: {ex.Message}");
            return Array.Empty<AutoBackupFile>();
        }
    }

    /// <summary>
    /// Sichert, falls nach den Regeln fällig. Liefert den Dateinamen oder null (nicht fällig oder
    /// fehlgeschlagen).
    /// </summary>
    public async Task<string?> BackupIfDueAsync(
        DatabaseMaintenanceRepository maintenance,
        AutoBackupReason reason,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var existing = List();
        if (!AutoBackupPolicy.IsDue(existing, now, reason))
        {
            return null;
        }

        var fileName = AutoBackupPolicy.FileNameFor(now, reason);

        try
        {
            Directory.CreateDirectory(_backupDirectory);
            await maintenance.ExportBackupAsync(Path.Combine(_backupDirectory, fileName), cancellationToken);
            AppLog.Info("Sicherung", $"Automatische Sicherung angelegt: {fileName} ({reason})");
            return fileName;
        }
        catch (Exception ex)
        {
            // Bewusst jede Ausnahme: volle Platte, gesperrte Datei, fehlende Rechte - nichts davon
            // rechtfertigt, dass die App nicht startet.
            AppLog.Warn("Sicherung", $"Automatische Sicherung fehlgeschlagen ({reason}): {ex.Message}");
            return null;
        }
    }

    /// <summary>Löscht alte Sicherungen nach den Regeln; liefert die Anzahl gelöschter Dateien.</summary>
    public int Prune(int keep = AutoBackupPolicy.KeepCount)
    {
        var deleted = 0;

        foreach (var fileName in AutoBackupPolicy.Obsolete(List(), keep))
        {
            try
            {
                File.Delete(Path.Combine(_backupDirectory, fileName));
                deleted++;
            }
            catch (IOException ex)
            {
                AppLog.Warn("Sicherung", $"Alte Sicherung {fileName} nicht löschbar: {ex.Message}");
            }
        }

        return deleted;
    }
}
