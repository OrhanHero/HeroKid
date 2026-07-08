using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Setzt die komplette lokale Datenbank zurück (alle Profile, Fortschritte, Aktivitätsprotokolle,
/// Quiz-Historie und Einstellungen). Vorher musste man dafür manuell die Datei
/// %LOCALAPPDATA%\LernTor\lerntor.db löschen - das ist jetzt aus der App heraus möglich (siehe
/// Bestätigungsdialog in ParentSettingsViewModel.ResetAllDataAsync).
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
}
