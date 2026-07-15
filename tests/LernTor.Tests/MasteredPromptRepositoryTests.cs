using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Dauerhafter Prompt-Ausschluss gegen echte SQLite-Temp-Dateien (Muster wie ReviewQuestionRepositoryTests).</summary>
public sealed class MasteredPromptRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-mastered-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static QuizQuestion MathQuestion(string prompt = "Was ist 1/2 + 1/4?") => new()
    {
        Id = "q1",
        Subject = Subject.Mathematik,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Bruchrechnen",
        Prompt = prompt,
        Type = QuestionType.MultipleChoice,
        Options = new[] { "3/4", "2/6", "1/8" },
        CorrectAnswers = new[] { "3/4" },
        Explanation = "1/2 = 2/4, also 2/4 + 1/4 = 3/4."
    };

    [Fact]
    public async Task Richtig_beantwortet_landet_im_dauerhaften_Ausschluss()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordIfCorrectAsync("p1", MathQuestion(), wasCorrect: true);

        var mastered = await repo.GetMasteredPromptsAsync("p1");
        Assert.Contains("Was ist 1/2 + 1/4?", mastered);
    }

    [Fact]
    public async Task Falsch_beantwortet_landet_nicht_im_Ausschluss()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordIfCorrectAsync("p1", MathQuestion(), wasCorrect: false);

        Assert.Empty(await repo.GetMasteredPromptsAsync("p1"));
    }

    [Fact]
    public async Task Doppeltes_richtiges_Beantworten_legt_keinen_zweiten_Eintrag_an()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordIfCorrectAsync("p1", MathQuestion(), wasCorrect: true);
        await repo.RecordIfCorrectAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Single(db.MasteredPrompts.ToList());
    }

    [Fact]
    public async Task Ausschluss_gilt_nur_fuer_das_jeweilige_Profil()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordIfCorrectAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Empty(await repo.GetMasteredPromptsAsync("anderes-profil"));
    }

    [Fact]
    public async Task News_Fragen_werden_nicht_dauerhaft_ausgeschlossen()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        var question = new QuizQuestion
        {
            Id = "n1",
            Subject = Subject.News,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "News",
            Prompt = "Worum ging es im Artikel?",
            Type = QuestionType.OpenText,
            CorrectAnswers = new[] { "Berlin" },
            Explanation = "Stand im Artikel."
        };

        await repo.RecordIfCorrectAsync("p1", question, wasCorrect: true);

        Assert.Empty(db.MasteredPrompts.ToList());
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }
        catch
        {
            // Temp-Datei ggf. noch gesperrt - das Temp-Verzeichnis räumt das OS auf.
        }
    }
}
