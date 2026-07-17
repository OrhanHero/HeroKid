using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Gemeisterte Aufgaben mit Spaced-Repetition-Zeitplan gegen echte SQLite-Temp-Dateien
/// (Muster wie ReviewQuestionRepositoryTests).</summary>
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
    public async Task Richtig_beantwortet_ist_zunaechst_ausgeschlossen_mit_Stufe_1_und_7_Tage_Faelligkeit()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Contains("Was ist 1/2 + 1/4?", await repo.GetMasteredPromptsAsync("p1"));

        var entity = db.MasteredPrompts.Single();
        Assert.Equal(1, entity.ReviewStage);
        Assert.NotNull(entity.NextDueAt);
        var daysUntilDue = (entity.NextDueAt!.Value - DateTimeOffset.Now).TotalDays;
        Assert.InRange(daysUntilDue, 6.9, 7.1);
    }

    [Fact]
    public async Task Faellige_Aufgabe_ist_nicht_mehr_ausgeschlossen()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        var entity = db.MasteredPrompts.Single();
        entity.NextDueAt = DateTimeOffset.Now.AddMinutes(-1);
        await db.SaveChangesAsync();

        Assert.Empty(await repo.GetMasteredPromptsAsync("p1"));
    }

    [Fact]
    public async Task Bestandene_Auffrischung_erhoeht_die_Stufe_und_verlaengert_das_Intervall_auf_30_Tage()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        var entity = db.MasteredPrompts.Single();
        Assert.Equal(2, entity.ReviewStage);
        var daysUntilDue = (entity.NextDueAt!.Value - DateTimeOffset.Now).TotalDays;
        Assert.InRange(daysUntilDue, 29.9, 30.1);
    }

    [Fact]
    public async Task Falsch_beantwortete_Auffrischung_loescht_die_Meisterung()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        Assert.Empty(db.MasteredPrompts.ToList());
    }

    [Fact]
    public async Task Falsch_beantwortet_ohne_vorherige_Meisterung_legt_nichts_an()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        Assert.Empty(db.MasteredPrompts.ToList());
    }

    [Fact]
    public async Task Alt_Eintrag_ohne_Faelligkeitsdatum_gilt_als_faellig_und_ist_nicht_ausgeschlossen()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        // Eintrag von vor der Spaced-Repetition-Umstellung: NextDueAt existierte damals nicht.
        db.MasteredPrompts.Add(new LernTor.Data.Entities.MasteredPromptEntity
        {
            ProfileId = "p1",
            Prompt = "Alte Aufgabe",
            Subject = "Mathematik",
            MasteredAt = DateTimeOffset.Now.AddMonths(-6),
            ReviewStage = 0,
            NextDueAt = null
        });
        await db.SaveChangesAsync();

        Assert.Empty(await repo.GetMasteredPromptsAsync("p1"));

        // Wird die alte Aufgabe erneut richtig beantwortet, bekommt sie einen echten Zeitplan.
        await repo.RecordOutcomeAsync("p1", MathQuestion("Alte Aufgabe"), wasCorrect: true);
        var entity = db.MasteredPrompts.Single();
        Assert.Equal(1, entity.ReviewStage);
        Assert.NotNull(entity.NextDueAt);
    }

    [Fact]
    public async Task Doppeltes_richtiges_Beantworten_legt_keinen_zweiten_Eintrag_an()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Single(db.MasteredPrompts.ToList());
    }

    [Fact]
    public async Task Ausschluss_gilt_nur_fuer_das_jeweilige_Profil()
    {
        using var db = CreateContext();
        var repo = new MasteredPromptRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Empty(await repo.GetMasteredPromptsAsync("anderes-profil"));
    }

    [Fact]
    public async Task News_Fragen_werden_nicht_erfasst()
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

        await repo.RecordOutcomeAsync("p1", question, wasCorrect: true);

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
