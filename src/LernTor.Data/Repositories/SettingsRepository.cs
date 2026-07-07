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
            StudentGradeLevel = (GradeLevel)entity.StudentGradeLevel,
            DisabledSubjects = JsonSerializer.Deserialize<HashSet<Subject>>(entity.DisabledSubjectsJson) ?? new(),
            DailyTimeLimitMinutes = entity.DailyTimeLimitMinutes,
            HardLockShellReplacementEnabled = entity.HardLockShellReplacementEnabled
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
        entity.StudentGradeLevel = (int)settings.StudentGradeLevel;
        entity.DisabledSubjectsJson = JsonSerializer.Serialize(settings.DisabledSubjects);
        entity.DailyTimeLimitMinutes = settings.DailyTimeLimitMinutes;
        entity.HardLockShellReplacementEnabled = settings.HardLockShellReplacementEnabled;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
