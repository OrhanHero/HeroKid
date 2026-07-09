using System.Text.Json;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

public sealed class SettingsRepository
{
    private readonly LernTorDbContext _db;

    public SettingsRepository(LernTorDbContext db)
    {
        _db = db;
    }

    public async Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _db.Settings.FirstOrDefaultAsync(s => s.Id == 1, cancellationToken);
        if (entity is null)
        {
            return new AppSettings();
        }

        return new AppSettings
        {
            AdminPasswordHash = entity.AdminPasswordHash,
            AdminPasswordSalt = entity.AdminPasswordSalt,
            DefaultLanguage = Enum.Parse<AppLanguage>(entity.DefaultLanguage),
            DisabledSubjects = JsonSerializer.Deserialize<HashSet<Subject>>(entity.DisabledSubjectsJson, JsonOptions.Default) ?? new(),
            HardLockShellReplacementEnabled = entity.HardLockShellReplacementEnabled,
            NotebookLmProjectId = entity.NotebookLmProjectId,
            NotebookLmLocation = entity.NotebookLmLocation,
            NotebookLmServiceAccountKeyPath = entity.NotebookLmServiceAccountKeyPath,
            TeacherImportProvider = Enum.TryParse<LlmProvider>(entity.TeacherImportProvider, out var importProvider)
                ? importProvider
                : LlmProvider.NotebookLm,
            LocalLlmModelPath = entity.LocalLlmModelPath,
            HomeworkChatProvider = Enum.TryParse<LlmProvider>(entity.HomeworkChatProvider, out var chatProvider)
                ? chatProvider
                : LlmProvider.LocalLlm
        };
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Settings.FirstOrDefaultAsync(s => s.Id == 1, cancellationToken);
        if (entity is null)
        {
            entity = new SettingsEntity { Id = 1 };
            _db.Settings.Add(entity);
        }

        entity.AdminPasswordHash = settings.AdminPasswordHash;
        entity.AdminPasswordSalt = settings.AdminPasswordSalt;
        entity.DefaultLanguage = settings.DefaultLanguage.ToString();
        entity.DisabledSubjectsJson = JsonSerializer.Serialize(settings.DisabledSubjects, JsonOptions.Default);
        entity.HardLockShellReplacementEnabled = settings.HardLockShellReplacementEnabled;
        entity.NotebookLmProjectId = settings.NotebookLmProjectId;
        entity.NotebookLmLocation = settings.NotebookLmLocation;
        entity.NotebookLmServiceAccountKeyPath = settings.NotebookLmServiceAccountKeyPath;
        entity.TeacherImportProvider = settings.TeacherImportProvider.ToString();
        entity.LocalLlmModelPath = settings.LocalLlmModelPath;
        entity.HomeworkChatProvider = settings.HomeworkChatProvider.ToString();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
