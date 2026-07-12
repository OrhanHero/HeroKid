using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Fehler-Kartei-Logik gegen echte SQLite-Temp-Dateien (Muster wie SqliteSchemaUpdaterTests).</summary>
public sealed class ReviewQuestionRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-review-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static QuizQuestion MathQuestion(string id = "q1") => new()
    {
        Id = id,
        Subject = Subject.Mathematik,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Bruchrechnen",
        Prompt = "Was ist 1/2 + 1/4?",
        Type = QuestionType.MultipleChoice,
        Options = new[] { "3/4", "2/6", "1/8" },
        CorrectAnswers = new[] { "3/4" },
        Explanation = "1/2 = 2/4, also 2/4 + 1/4 = 3/4."
    };

    [Fact]
    public async Task Falsche_Antwort_legt_Eintrag_an_aber_nicht_am_selben_Tag_faellig()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        Assert.Single(db.ReviewQuestions.ToList());
        // Heute beantwortet → heute nicht erneut fällig (Abstand ist der Lerneffekt).
        Assert.Empty(await repo.GetDueQuestionsAsync("p1", Subject.Mathematik, 3));
    }

    [Fact]
    public async Task Am_Folgetag_faellig_mit_Wiederholungs_Markierung_und_intakter_Aufgabe()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        Backdate(db, days: 1);
        var due = await repo.GetDueQuestionsAsync("p1", Subject.Mathematik, 3);

        var question = Assert.Single(due);
        Assert.StartsWith("🔁", question.Topic);
        Assert.Equal("Was ist 1/2 + 1/4?", question.Prompt);
        Assert.Equal(new[] { "3/4", "2/6", "1/8" }, question.Options);
        Assert.True(question.CheckAnswer("3/4")); // Der Schnappschuss bleibt beantwortbar.

        // Anderes Fach oder anderes Profil: nichts fällig.
        Assert.Empty(await repo.GetDueQuestionsAsync("p1", Subject.Deutsch, 3));
        Assert.Empty(await repo.GetDueQuestionsAsync("anderes-profil", Subject.Mathematik, 3));
    }

    [Fact]
    public async Task Zweimal_richtig_in_Folge_entfernt_den_Eintrag()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);
        Assert.Single(db.ReviewQuestions.ToList()); // Streak 1: bleibt.

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);
        Assert.Empty(db.ReviewQuestions.ToList()); // Streak 2: gelernt, gelöscht.
    }

    [Fact]
    public async Task Erneut_falsch_setzt_den_Streak_zurueck_und_zaehlt_WrongCount_hoch()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);
        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: false);

        var entity = db.ReviewQuestions.Single();
        Assert.Equal(0, entity.CorrectStreak);
        Assert.Equal(2, entity.WrongCount);
    }

    [Fact]
    public async Task Richtige_Antwort_ohne_Kartei_Eintrag_tut_nichts()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);

        await repo.RecordOutcomeAsync("p1", MathQuestion(), wasCorrect: true);

        Assert.Empty(db.ReviewQuestions.ToList());
    }

    [Fact]
    public async Task News_Fragen_landen_nicht_in_der_Kartei()
    {
        using var db = CreateContext();
        var repo = new ReviewQuestionRepository(db);

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

        await repo.RecordOutcomeAsync("p1", question, wasCorrect: false);

        Assert.Empty(db.ReviewQuestions.ToList());
    }

    /// <summary>Setzt LastAnsweredAt aller Einträge um <paramref name="days"/> Tage zurück -
    /// simuliert den Folgetag, ohne echte Wartezeit.</summary>
    private static void Backdate(LernTorDbContext db, int days)
    {
        foreach (var entity in db.ReviewQuestions.ToList())
        {
            entity.LastAnsweredAt = entity.LastAnsweredAt.AddDays(-days);
        }

        db.SaveChanges();
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
