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
            StreaksEnabled = entity.StreaksEnabled,
            PauseUntilDate = DateOnly.TryParseExact(entity.PauseUntilDate, "yyyy-MM-dd", out var pauseUntil)
                ? pauseUntil
                : null,
            LocalLlmModelPath = entity.LocalLlmModelPath,
            LocalLlmModelKey = entity.LocalLlmModelKey,
            HiddenReadingTextKeys = JsonSerializer.Deserialize<HashSet<string>>(
                Fallback(entity.HiddenReadingTextKeysJson), JsonOptions.Default) ?? new(),
            DisabledNewsFeeds = JsonSerializer.Deserialize<HashSet<string>>(
                Fallback(entity.DisabledNewsFeedsJson), JsonOptions.Default) ?? new()
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
        entity.StreaksEnabled = settings.StreaksEnabled;
        entity.PauseUntilDate = settings.PauseUntilDate?.ToString("yyyy-MM-dd");
        entity.LocalLlmModelPath = settings.LocalLlmModelPath;
        entity.LocalLlmModelKey = settings.LocalLlmModelKey;
        entity.HiddenReadingTextKeysJson = JsonSerializer.Serialize(settings.HiddenReadingTextKeys, JsonOptions.Default);
        entity.DisabledNewsFeedsJson = JsonSerializer.Serialize(settings.DisabledNewsFeeds, JsonOptions.Default);

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Zeilen, die vor diesen Spalten angelegt wurden, bekommen beim additiven Schema-Abgleich
    /// einen leeren String statt "[]" - der wirft beim Deserialisieren. Deshalb hier abfangen.
    /// </summary>
    private static string Fallback(string? json) => string.IsNullOrWhiteSpace(json) ? "[]" : json;
}
