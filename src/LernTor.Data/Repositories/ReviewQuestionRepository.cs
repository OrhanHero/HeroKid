using System.Text.Json;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// "Fehler-Kartei" (siehe <see cref="ReviewQuestionEntity"/>): sammelt im Übungsteil falsch
/// beantwortete Aufgaben pro Profil und liefert sie an Folgetagen als Wiederholungsfragen
/// zurück, bis sie zweimal in Folge richtig beantwortet wurden.
/// </summary>
public sealed class ReviewQuestionRepository
{
    /// <summary>So viele richtige Antworten in Folge gelten als "gelernt" → Eintrag wird entfernt.</summary>
    private const int RequiredCorrectStreak = 2;

    private readonly LernTorDbContext _db;

    public ReviewQuestionRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Verbucht eine beantwortete Übungsaufgabe. Falsch → Eintrag anlegen bzw. Streak
    /// zurücksetzen; richtig → Streak erhöhen und bei <see cref="RequiredCorrectStreak"/> den
    /// Eintrag löschen. News-Fragen werden ignoriert: sie beziehen sich auf Tagesartikel, die es
    /// an Folgetagen nicht mehr gibt - eine Wiederholung ohne den Artikel wäre unfair.
    /// </summary>
    public async Task RecordOutcomeAsync(string profileId, QuizQuestion question, bool wasCorrect, CancellationToken cancellationToken = default)
    {
        if (question.Subject == Subject.News)
        {
            return;
        }

        var entity = await _db.ReviewQuestions.FirstOrDefaultAsync(
            r => r.ProfileId == profileId && r.QuestionId == question.Id, cancellationToken);

        if (!wasCorrect)
        {
            if (entity is null)
            {
                entity = new ReviewQuestionEntity
                {
                    ProfileId = profileId,
                    QuestionId = question.Id,
                    Subject = question.Subject.ToString(),
                    GradeLevel = question.GradeLevel.ToString(),
                    Topic = question.Topic,
                    Prompt = question.Prompt,
                    Type = question.Type.ToString(),
                    OptionsJson = JsonSerializer.Serialize(question.Options, JsonOptions.Default),
                    CorrectAnswersJson = JsonSerializer.Serialize(question.CorrectAnswers, JsonOptions.Default),
                    Explanation = question.Explanation,
                    HelpHint = question.HelpHint,
                    ImageUrl = question.ImageUrl,
                    RequiresTurkishCharacters = question.RequiresTurkishCharacters
                };
                _db.ReviewQuestions.Add(entity);
            }

            entity.WrongCount++;
            entity.CorrectStreak = 0;
            entity.LastAnsweredAt = DateTimeOffset.Now;
            await _db.SaveChangesAsync(cancellationToken);
            return;
        }

        if (entity is null)
        {
            return; // Richtig beantwortet und nicht in der Kartei - nichts zu tun.
        }

        entity.CorrectStreak++;
        entity.LastAnsweredAt = DateTimeOffset.Now;

        if (entity.CorrectStreak >= RequiredCorrectStreak)
        {
            _db.ReviewQuestions.Remove(entity); // Gelernt!
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Fällige Wiederholungsfragen eines Fachs: nicht am selben Tag erneut (Datumsvergleich
    /// in-memory - SQLite kann DateTimeOffset nicht serverseitig vergleichen), am häufigsten
    /// falsch beantwortete zuerst. Das Thema erhält ein 🔁-Präfix, damit das Kind sieht,
    /// dass es eine Wiederholung ist.
    /// </summary>
    public async Task<IReadOnlyList<QuizQuestion>> GetDueQuestionsAsync(
        string profileId, Subject subject, int maxCount, CancellationToken cancellationToken = default)
    {
        var subjectName = subject.ToString();
        var entities = await _db.ReviewQuestions
            .Where(r => r.ProfileId == profileId && r.Subject == subjectName)
            .ToListAsync(cancellationToken);

        var today = DateTime.Today;

        return entities
            .Where(r => r.LastAnsweredAt.LocalDateTime.Date < today)
            .OrderByDescending(r => r.WrongCount)
            .ThenBy(r => r.LastAnsweredAt)
            .Take(maxCount)
            .Select(ToQuestion)
            .ToList();
    }

    private static QuizQuestion ToQuestion(ReviewQuestionEntity entity) => new()
    {
        Id = entity.QuestionId,
        Subject = Enum.Parse<Subject>(entity.Subject),
        GradeLevel = Enum.Parse<GradeLevel>(entity.GradeLevel),
        Topic = $"🔁 {entity.Topic}",
        Prompt = entity.Prompt,
        Type = Enum.Parse<QuestionType>(entity.Type),
        Options = JsonSerializer.Deserialize<List<string>>(entity.OptionsJson, JsonOptions.Default) ?? new List<string>(),
        CorrectAnswers = JsonSerializer.Deserialize<List<string>>(entity.CorrectAnswersJson, JsonOptions.Default) ?? new List<string>(),
        Explanation = entity.Explanation,
        HelpHint = entity.HelpHint,
        ImageUrl = entity.ImageUrl,
        RequiresTurkishCharacters = entity.RequiresTurkishCharacters
    };
}
