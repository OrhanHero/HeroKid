using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>Protokolliert bearbeitete Aufgaben und Quiz-Ergebnisse je Kind-Profil für die Elternübersicht.</summary>
public sealed class ActivityLogRepository
{
    private readonly LernTorDbContext _db;

    public ActivityLogRepository(LernTorDbContext db)
    {
        _db = db;
    }

    public async Task LogAnswerAsync(string profileId, QuestionOutcome outcome, string topic, string prompt, CancellationToken cancellationToken = default)
    {
        _db.ActivityLog.Add(new ActivityLogEntity
        {
            ProfileId = profileId,
            Timestamp = DateTimeOffset.Now,
            Subject = outcome.Subject.ToString(),
            Topic = topic,
            QuestionId = outcome.QuestionId,
            Prompt = prompt,
            GivenAnswer = outcome.GivenAnswer,
            WasCorrect = outcome.WasCorrect
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task LogQuizAttemptAsync(string profileId, QuizResult result, CancellationToken cancellationToken = default)
    {
        _db.QuizAttempts.Add(new QuizAttemptEntity
        {
            ProfileId = profileId,
            Timestamp = DateTimeOffset.Now,
            TotalQuestions = result.TotalQuestions,
            CorrectCount = result.CorrectCount,
            ScorePercentage = result.ScorePercentage,
            Passed = result.Passed
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLogEntity>> GetRecentActivityAsync(string profileId, int take = 200, CancellationToken cancellationToken = default)
    {
        return await _db.ActivityLog
            .Where(a => a.ProfileId == profileId)
            .OrderByDescending(a => a.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<QuizAttemptEntity>> GetQuizHistoryAsync(string profileId, int take = 50, CancellationToken cancellationToken = default)
    {
        return await _db.QuizAttempts
            .Where(q => q.ProfileId == profileId)
            .OrderByDescending(q => q.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
