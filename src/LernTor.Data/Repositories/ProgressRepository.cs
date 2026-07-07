using System.Text.Json;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Lädt/speichert den Fortschritt des aktuellen Lerntages. Wird nach jedem Schritt aufgerufen,
/// damit ein Absturz oder Neustart des PCs keinen Fortschritt verliert.
/// </summary>
public sealed class ProgressRepository
{
    private readonly LernTorDbContext _db;

    public ProgressRepository(LernTorDbContext db)
    {
        _db = db;
    }

    public async Task<StudentProgress> LoadOrCreateTodayAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var entity = await _db.Progress
            .FirstOrDefaultAsync(p => p.SessionDate == today, cancellationToken);

        if (entity is null)
        {
            return new StudentProgress { SessionDate = today };
        }

        return new StudentProgress
        {
            SessionDate = entity.SessionDate,
            CurrentStage = Enum.Parse<LearningStage>(entity.CurrentStage),
            CompletedNewsArticleIds = JsonSerializer.Deserialize<HashSet<string>>(entity.CompletedNewsArticleIdsJson) ?? new(),
            CompletedExerciseSubjects = JsonSerializer.Deserialize<HashSet<Subject>>(entity.CompletedSubjectsJson) ?? new(),
            FinalQuizAttempts = entity.FinalQuizAttempts,
            LastQuizScore = entity.LastQuizScore,
            IsUnlocked = entity.IsUnlocked,
            SubjectsToRetry = JsonSerializer.Deserialize<List<Subject>>(entity.SubjectsToRetryJson) ?? new(),
            LastUpdatedAt = entity.LastUpdatedAt
        };
    }

    public async Task SaveAsync(StudentProgress progress, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Progress
            .FirstOrDefaultAsync(p => p.SessionDate == progress.SessionDate, cancellationToken);

        if (entity is null)
        {
            entity = new ProgressEntity { SessionDate = progress.SessionDate };
            _db.Progress.Add(entity);
        }

        entity.CurrentStage = progress.CurrentStage.ToString();
        entity.CompletedNewsArticleIdsJson = JsonSerializer.Serialize(progress.CompletedNewsArticleIds);
        entity.CompletedSubjectsJson = JsonSerializer.Serialize(progress.CompletedExerciseSubjects);
        entity.FinalQuizAttempts = progress.FinalQuizAttempts;
        entity.LastQuizScore = progress.LastQuizScore;
        entity.IsUnlocked = progress.IsUnlocked;
        entity.SubjectsToRetryJson = JsonSerializer.Serialize(progress.SubjectsToRetry);
        entity.LastUpdatedAt = DateTimeOffset.Now;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
