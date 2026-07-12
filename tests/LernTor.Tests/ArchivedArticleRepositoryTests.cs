using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>News-Archiv/Offline-Rückfall gegen echte SQLite-Temp-Dateien.</summary>
public sealed class ArchivedArticleRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-archive-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static NewsArticle Article(string id) => new()
    {
        Id = id,
        Title = $"Titel {id}",
        SimplifiedSummary = "Eine kindgerechte Zusammenfassung.",
        SourceName = "tagesschau.de",
        SourceUrl = "https://example.org",
        PublishedAt = DateTimeOffset.Now,
        RegionFocus = NewsRegionFocus.Berlin,
        Category = NewsCategory.Berlin,
        CategoryEmoji = "🐻",
        ReadingMinutes = 2,
        Difficulty = NewsDifficulty.Leicht,
        WhyImportant = "Darum.",
        MeaningForKids = "Deshalb.",
        BerlinDistrict = "Spandau",
        ExplainedTerms = new[] { new ExplainedTerm("Senat", "Die Regierung von Berlin.") },
        ComprehensionQuestions = new[]
        {
            new QuizQuestion
            {
                Id = $"{id}-frage",
                Subject = Subject.News,
                GradeLevel = GradeLevel.Klasse6,
                Topic = "News",
                Prompt = "Welcher Bezirk?",
                Type = QuestionType.MultipleChoice,
                Options = new[] { "Spandau", "Pankow" },
                CorrectAnswers = new[] { "Spandau" },
                Explanation = "Stand im Artikel."
            }
        }
    };

    [Fact]
    public async Task Archiv_stellt_Artikel_samt_Fragen_vollstaendig_wieder_her()
    {
        using var db = CreateContext();
        var repo = new ArchivedArticleRepository(db);

        await repo.ArchiveTodayAsync(new[] { Article("a1"), Article("a2") });
        var restored = await repo.GetLatestArchiveAsync();

        Assert.Equal(2, restored.Count);
        var article = restored.Single(a => a.Id == "a1");
        Assert.Equal(NewsCategory.Berlin, article.Category);
        Assert.Equal("Spandau", article.BerlinDistrict);
        Assert.Single(article.ExplainedTerms);

        var question = Assert.Single(article.ComprehensionQuestions);
        Assert.Equal("Welcher Bezirk?", question.Prompt);
        Assert.True(question.CheckAnswer("Spandau")); // Frage bleibt beantwortbar.
    }

    [Fact]
    public async Task Erneutes_Archivieren_am_selben_Tag_ersetzt_statt_zu_doppeln()
    {
        using var db = CreateContext();
        var repo = new ArchivedArticleRepository(db);

        await repo.ArchiveTodayAsync(new[] { Article("a1") });
        await repo.ArchiveTodayAsync(new[] { Article("a1"), Article("a2") });

        Assert.Equal(2, (await repo.GetLatestArchiveAsync()).Count);
    }

    [Fact]
    public async Task Alte_Staende_werden_beim_Archivieren_entfernt()
    {
        using var db = CreateContext();
        var repo = new ArchivedArticleRepository(db);

        await repo.ArchiveTodayAsync(new[] { Article("alt") });
        foreach (var entity in db.ArchivedArticles.ToList())
        {
            entity.ArchivedDate = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd");
        }
        db.SaveChanges();

        await repo.ArchiveTodayAsync(new[] { Article("neu") });

        Assert.All(db.ArchivedArticles.ToList(),
            e => Assert.Equal(DateTime.Today.ToString("yyyy-MM-dd"), e.ArchivedDate));
    }

    [Fact]
    public async Task Leeres_Archiv_liefert_leere_Liste()
    {
        using var db = CreateContext();
        var repo = new ArchivedArticleRepository(db);

        Assert.Empty(await repo.GetLatestArchiveAsync());
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
