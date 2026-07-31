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

    /// <summary>Legt direkt einen Archiv-Stand fuer ein bestimmtes Datum an.</summary>
    private static void Archiviere(LernTorDbContext db, DateOnly datum, params string[] artikelIds)
    {
        foreach (var id in artikelIds)
        {
            db.ArchivedArticles.Add(new LernTor.Data.Entities.ArchivedArticleEntity
            {
                ArchivedDate = datum.ToString("yyyy-MM-dd"),
                ArticleId = id,
                Title = $"Titel {id}",
                SimplifiedSummary = "Zusammenfassung.",
                SourceName = "tagesschau.de",
                SourceUrl = "https://example.invalid",
                PublishedAt = DateTimeOffset.Now,
                RegionFocus = nameof(NewsRegionFocus.Berlin),
                Category = nameof(NewsCategory.Berlin),
                CategoryEmoji = "🐻",
                ReadingMinutes = 2,
                Difficulty = nameof(NewsDifficulty.Leicht),
                WhyImportant = "Darum.",
                MeaningForKids = "Deshalb."
            });
        }

        db.SaveChanges();
    }

    private static readonly DateOnly Heute = new(2026, 8, 20);

    [Fact]
    public async Task Ohne_Archiv_gibt_es_keinen_Rueckfall()
    {
        using var db = CreateContext();

        var (articles, archivedOn) = await new ArchivedArticleRepository(db).GetOfflineFallbackAsync(Heute);

        Assert.Empty(articles);
        Assert.Null(archivedOn);
    }

    [Fact]
    public async Task Am_ersten_Ausfalltag_kommt_der_juengste_Stand()
    {
        using var db = CreateContext();
        Archiviere(db, Heute.AddDays(-3), "alt");
        Archiviere(db, Heute.AddDays(-2), "mittel");
        Archiviere(db, Heute.AddDays(-1), "gestern");

        var (articles, archivedOn) = await new ArchivedArticleRepository(db).GetOfflineFallbackAsync(Heute);

        Assert.Equal(Heute.AddDays(-1), archivedOn);
        Assert.Equal("gestern", articles.Single().Id);
    }

    [Fact]
    public async Task Bei_laengerem_Ausfall_wandert_die_Auswahl_zurueck()
    {
        // Genau das ging vorher schief: der Rueckfall lieferte jeden Morgen denselben Stand,
        // inklusive derselben Verstaendnisfragen.
        using var db = CreateContext();
        var letzterOnlineTag = Heute.AddDays(-1);
        Archiviere(db, letzterOnlineTag.AddDays(-2), "tag3");
        Archiviere(db, letzterOnlineTag.AddDays(-1), "tag2");
        Archiviere(db, letzterOnlineTag, "tag1");

        var repo = new ArchivedArticleRepository(db);

        Assert.Equal("tag1", (await repo.GetOfflineFallbackAsync(letzterOnlineTag.AddDays(1))).Articles.Single().Id);
        Assert.Equal("tag2", (await repo.GetOfflineFallbackAsync(letzterOnlineTag.AddDays(2))).Articles.Single().Id);
        Assert.Equal("tag3", (await repo.GetOfflineFallbackAsync(letzterOnlineTag.AddDays(3))).Articles.Single().Id);
    }

    [Fact]
    public async Task Ist_das_Archiv_durchlaufen_beginnt_es_von_vorne()
    {
        using var db = CreateContext();
        var letzterOnlineTag = Heute.AddDays(-1);
        Archiviere(db, letzterOnlineTag.AddDays(-1), "tag2");
        Archiviere(db, letzterOnlineTag, "tag1");

        var repo = new ArchivedArticleRepository(db);

        // Zwei Staende, dritter Ausfalltag -> wieder der juengste.
        Assert.Equal("tag1", (await repo.GetOfflineFallbackAsync(letzterOnlineTag.AddDays(3))).Articles.Single().Id);
    }

    [Fact]
    public async Task Die_Auswahl_ist_innerhalb_eines_Tages_stabil()
    {
        // Beim zweiten Oeffnen duerfen nicht andere Artikel dastehen - der Fortschritt haengt
        // an den Artikel-IDs.
        using var db = CreateContext();
        Archiviere(db, Heute.AddDays(-2), "a");
        Archiviere(db, Heute.AddDays(-1), "b");

        var repo = new ArchivedArticleRepository(db);

        var erst = (await repo.GetOfflineFallbackAsync(Heute)).ArchivedOn;
        var zweit = (await repo.GetOfflineFallbackAsync(Heute)).ArchivedOn;

        Assert.Equal(erst, zweit);
    }

    [Fact]
    public async Task Der_heutige_Stand_zaehlt_nicht_als_Rueckfall()
    {
        // Sonst wuerde ein frueherer Rueckfall sich selbst als "aktuellen" Stand anbieten.
        using var db = CreateContext();
        Archiviere(db, Heute, "heute");
        Archiviere(db, Heute.AddDays(-1), "gestern");

        var (articles, _) = await new ArchivedArticleRepository(db).GetOfflineFallbackAsync(Heute);

        Assert.Equal("gestern", articles.Single().Id);
    }

    [Fact]
    public async Task Archivtage_kommen_juengster_zuerst()
    {
        using var db = CreateContext();
        Archiviere(db, Heute.AddDays(-5), "x");
        Archiviere(db, Heute.AddDays(-1), "y");
        Archiviere(db, Heute.AddDays(-3), "z");

        var dates = await new ArchivedArticleRepository(db).GetArchivedDatesAsync();

        Assert.Equal(new[] { Heute.AddDays(-1), Heute.AddDays(-3), Heute.AddDays(-5) }, dates);
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
