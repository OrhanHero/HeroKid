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
        // Sortierung erst nach dem Laden (in-memory): SQLite/EF Core kann ORDER BY auf
        // DateTimeOffset-Spalten nicht serverseitig übersetzen.
        var entities = await _db.ActivityLog
            .Where(a => a.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities.OrderByDescending(a => a.Timestamp).Take(take).ToList();
    }

    public async Task<IReadOnlyList<QuizAttemptEntity>> GetQuizHistoryAsync(string profileId, int take = 50, CancellationToken cancellationToken = default)
    {
        var entities = await _db.QuizAttempts
            .Where(q => q.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities.OrderByDescending(q => q.Timestamp).Take(take).ToList();
    }
}
