using LernTor.Core.Models;
using LernTor.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LernTor.Data.Repositories;

/// <summary>
/// Lernstand der Theoriefragen pro Profil: welche Frage sitzt, und wie liefen die bisherigen
/// Prüfungssimulationen. Die Regeln stehen in <see cref="TheoryProgress"/> und
/// <see cref="TheoryExamRules"/> (beide Core) - hier wird nur gespeichert und geholt.
/// </summary>
public sealed class TheoryProgressRepository
{
    private readonly LernTorDbContext _db;
    private readonly ILogger<TheoryProgressRepository> _log;

    public TheoryProgressRepository(LernTorDbContext db, ILogger<TheoryProgressRepository> log)
    {
        _db = db;
        _log = log;
    }

    private static string KeyFor(string profileId, string questionId) => $"{profileId}|{questionId}";

    /// <summary>Alle Fragen-Einträge eines Profils, nach Fragen-Kennung.</summary>
    public async Task<IReadOnlyDictionary<string, TheoryAnswerEntity>> GetAnswersAsync(string profileId)
    {
        var entries = await _db.TheoryAnswers
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        return entries.ToDictionary(entry => entry.QuestionId, entry => entry, StringComparer.Ordinal);
    }

    /// <summary>Kennungen der Fragen, die das Kind sicher kann.</summary>
    public async Task<IReadOnlySet<string>> GetMasteredQuestionIdsAsync(string profileId)
    {
        var entries = await _db.TheoryAnswers
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        return entries
            .Where(entry => TheoryProgress.IsMastered(entry.CorrectStreak))
            .Select(entry => entry.QuestionId)
            .ToHashSet(StringComparer.Ordinal);
    }

    /// <summary>
    /// Schreibt eine Antwort fort. Legt den Eintrag beim ersten Mal an.
    /// </summary>
    /// <returns>Ob die Frage durch diese Antwort neu als gekonnt gilt - dafür gibt es einen Stern.</returns>
    public async Task<bool> RecordAnswerAsync(string profileId, string questionId, bool wasCorrect)
    {
        var id = KeyFor(profileId, questionId);
        var entry = await _db.TheoryAnswers.FindAsync(id);

        if (entry is null)
        {
            entry = new TheoryAnswerEntity
            {
                Id = id,
                ProfileId = profileId,
                QuestionId = questionId
            };
            _db.TheoryAnswers.Add(entry);
        }

        var warVorherGekonnt = TheoryProgress.IsMastered(entry.CorrectStreak);

        entry.CorrectStreak = TheoryProgress.NextStreak(entry.CorrectStreak, wasCorrect);
        entry.LastAnsweredAt = DateTimeOffset.Now;

        if (wasCorrect)
        {
            entry.CorrectTotal++;
        }
        else
        {
            entry.WrongTotal++;
        }

        await _db.SaveChangesAsync();

        return !warVorherGekonnt && TheoryProgress.IsMastered(entry.CorrectStreak);
    }

    /// <summary>Hält einen abgeschlossenen Prüfungsdurchlauf fest.</summary>
    public async Task RecordExamAsync(string profileId, TheoryExamResult result)
    {
        _db.TheoryExamRuns.Add(new TheoryExamRunEntity
        {
            Id = Guid.NewGuid().ToString("n"),
            ProfileId = profileId,
            TakenAt = DateTimeOffset.Now,
            QuestionCount = result.Questions,
            WrongPoints = result.WrongPoints,
            WrongCount = result.WrongCount,
            HeavyMistakes = result.HeavyMistakes
        });

        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Die letzten Prüfungsdurchläufe, neueste zuerst.
    ///
    /// <para>Sortiert wird bewusst erst nach <c>ToListAsync</c>: der SQLite-Anbieter von EF Core
    /// kann <c>OrderBy</c> auf einer <see cref="DateTimeOffset"/>-Spalte nicht übersetzen und
    /// wirft das erst beim Ausführen der Abfrage, nicht beim Übersetzen.</para>
    /// </summary>
    public async Task<IReadOnlyList<TheoryExamRunEntity>> GetRecentExamsAsync(string profileId, int count)
    {
        var runs = await _db.TheoryExamRuns
            .Where(run => run.ProfileId == profileId)
            .ToListAsync();

        return runs
            .OrderByDescending(run => run.TakenAt)
            .Take(count)
            .ToList();
    }

    /// <summary>
    /// Setzt den Theorie-Lernstand eines Profils zurück - im Eltern-Bereich, wenn ein Kind von
    /// vorn anfangen möchte. Betrifft nur die Theoriefragen, nicht die Verkehrszeichen.
    /// </summary>
    public async Task ResetAsync(string profileId)
    {
        var answers = await _db.TheoryAnswers
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();
        var runs = await _db.TheoryExamRuns
            .Where(run => run.ProfileId == profileId)
            .ToListAsync();

        _db.TheoryAnswers.RemoveRange(answers);
        _db.TheoryExamRuns.RemoveRange(runs);
        await _db.SaveChangesAsync();

        _log.LogInformation(
            "Theorie-Lernstand für Profil {ProfileId} zurückgesetzt ({Answers} Fragen, {Runs} Prüfungen).",
            profileId, answers.Count, runs.Count);
    }
}
