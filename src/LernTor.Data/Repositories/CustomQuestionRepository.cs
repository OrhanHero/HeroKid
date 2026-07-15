using System.Text.Json;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Verwaltet die von den Eltern selbst eingetragenen Aufgaben (siehe CustomQuestionEntity). Diese
/// werden im Eltern-Bereich gepflegt und ergänzen die generierten Aufgaben aus LernTor.ContentGen
/// additiv - LernTor.ContentGen weiß nichts von dieser Repository (Core bleibt einzige gemeinsame
/// Abhängigkeit), das Zusammenführen beider Quellen passiert erst in LernTor.App (MainViewModel).
/// </summary>
public sealed class CustomQuestionRepository
{
    private readonly LernTorDbContext _db;

    public CustomQuestionRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>Alle eigenen Aufgaben, neueste zuerst - für die Verwaltungsliste im Eltern-Bereich.</summary>
    public async Task<IReadOnlyList<QuizQuestion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Sortierung erst nach dem Laden (in-memory): SQLite/EF Core kann ORDER BY auf
        // DateTimeOffset-Spalten nicht serverseitig übersetzen.
        var entities = await _db.CustomQuestions.ToListAsync(cancellationToken);
        return entities.OrderByDescending(e => e.CreatedAt).Select(ToModel).ToList();
    }

    /// <summary>Eigene Aufgaben zu einem bestimmten Fach/einer Klassenstufe - für Übungsaufgaben.</summary>
    public async Task<IReadOnlyList<QuizQuestion>> GetBySubjectAndGradeAsync(
        Subject subject, GradeLevel grade, CancellationToken cancellationToken = default)
    {
        var subjectValue = subject.ToString();
        var gradeValue = grade.ToString();
        var entities = await _db.CustomQuestions
            .Where(e => e.Subject == subjectValue && e.GradeLevel == gradeValue)
            .ToListAsync(cancellationToken);
        return entities.Select(ToModel).ToList();
    }

    /// <summary>Eigene Aufgaben zu einer Klassenstufe über alle Fächer - für das Abschlussquiz.</summary>
    public async Task<IReadOnlyList<QuizQuestion>> GetByGradeAsync(GradeLevel grade, CancellationToken cancellationToken = default)
    {
        var gradeValue = grade.ToString();
        var entities = await _db.CustomQuestions.Where(e => e.GradeLevel == gradeValue).ToListAsync(cancellationToken);
        return entities.Select(ToModel).ToList();
    }

    public async Task AddAsync(QuizQuestion question, CancellationToken cancellationToken = default)
    {
        _db.CustomQuestions.Add(new CustomQuestionEntity
        {
            Id = string.IsNullOrEmpty(question.Id) ? Guid.NewGuid().ToString("N") : question.Id,
            Subject = question.Subject.ToString(),
            GradeLevel = question.GradeLevel.ToString(),
            Topic = question.Topic,
            Type = question.Type.ToString(),
            Prompt = question.Prompt,
            OptionsJson = JsonSerializer.Serialize(question.Options),
            CorrectAnswersJson = JsonSerializer.Serialize(question.CorrectAnswers),
            Explanation = question.Explanation,
            HelpHint = question.HelpHint,
            CreatedAt = DateTimeOffset.Now
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CustomQuestions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is not null)
        {
            _db.CustomQuestions.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    private static QuizQuestion ToModel(CustomQuestionEntity entity) => new()
    {
        Id = entity.Id,
        Subject = Enum.Parse<Subject>(entity.Subject),
        GradeLevel = Enum.Parse<GradeLevel>(entity.GradeLevel),
        Topic = entity.Topic,
        Type = Enum.Parse<QuestionType>(entity.Type),
        Prompt = entity.Prompt,
        Options = JsonSerializer.Deserialize<string[]>(entity.OptionsJson) ?? Array.Empty<string>(),
        CorrectAnswers = JsonSerializer.Deserialize<string[]>(entity.CorrectAnswersJson) ?? Array.Empty<string>(),
        Explanation = entity.Explanation,
        HelpHint = entity.HelpHint
    };
}
