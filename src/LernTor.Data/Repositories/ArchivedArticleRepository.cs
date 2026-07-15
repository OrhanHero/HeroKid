using System.Text.Json;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Tages-Archiv der News-Artikel für den Offline-Rückfall (siehe <see cref="ArchivedArticleEntity"/>).
/// </summary>
public sealed class ArchivedArticleRepository
{
    /// <summary>So viele Tage bleiben Archiv-Stände erhalten, ältere werden beim Archivieren entfernt.</summary>
    private const int RetentionDays = 7;

    private readonly LernTorDbContext _db;

    public ArchivedArticleRepository(LernTorDbContext db)
    {
        _db = db;
    }

    private static string DateKey(DateTime date) => date.ToString("yyyy-MM-dd");

    /// <summary>
    /// Archiviert die heutigen Artikel. Idempotent: ein zweiter Aufruf am selben Tag (z.B. nach
    /// App-Neustart) ersetzt den heutigen Stand, statt ihn zu doppeln. Räumt zugleich Stände
    /// älter als <see cref="RetentionDays"/> Tage ab.
    /// </summary>
    public async Task ArchiveTodayAsync(IReadOnlyList<NewsArticle> articles, CancellationToken cancellationToken = default)
    {
        var today = DateKey(DateTime.Today);
        var cutoff = DateKey(DateTime.Today.AddDays(-RetentionDays));

        _db.ArchivedArticles.RemoveRange(
            _db.ArchivedArticles.Where(a => a.ArchivedDate == today || string.Compare(a.ArchivedDate, cutoff) < 0));

        foreach (var article in articles)
        {
            _db.ArchivedArticles.Add(new ArchivedArticleEntity
            {
                ArchivedDate = today,
                ArticleId = article.Id,
                Title = article.Title,
                SimplifiedSummary = article.SimplifiedSummary,
                ImageUrl = article.ImageUrl,
                SourceName = article.SourceName,
                SourceUrl = article.SourceUrl,
                PublishedAt = article.PublishedAt,
                RegionFocus = article.RegionFocus.ToString(),
                Category = article.Category.ToString(),
                CategoryEmoji = article.CategoryEmoji,
                ReadingMinutes = article.ReadingMinutes,
                Difficulty = article.Difficulty.ToString(),
                WhyImportant = article.WhyImportant,
                MeaningForKids = article.MeaningForKids,
                ExplainedTermsJson = JsonSerializer.Serialize(article.ExplainedTerms, JsonOptions.Default),
                BerlinDistrict = article.BerlinDistrict,
                ComprehensionQuestionsJson = JsonSerializer.Serialize(article.ComprehensionQuestions, JsonOptions.Default)
            });
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Liefert den jüngsten archivierten Tages-Stand (leer, wenn noch nie archiviert
    /// wurde - z.B. Erstinstallation ohne Internet).</summary>
    public async Task<IReadOnlyList<NewsArticle>> GetLatestArchiveAsync(CancellationToken cancellationToken = default)
    {
        var latestDate = await _db.ArchivedArticles
            .OrderByDescending(a => a.ArchivedDate)
            .Select(a => a.ArchivedDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (latestDate is null)
        {
            return Array.Empty<NewsArticle>();
        }

        var entities = await _db.ArchivedArticles
            .Where(a => a.ArchivedDate == latestDate)
            .ToListAsync(cancellationToken);

        return entities.Select(ToArticle).ToList();
    }

    private static NewsArticle ToArticle(ArchivedArticleEntity entity) => new()
    {
        Id = entity.ArticleId,
        Title = entity.Title,
        SimplifiedSummary = entity.SimplifiedSummary,
        ImageUrl = entity.ImageUrl,
        SourceName = entity.SourceName,
        SourceUrl = entity.SourceUrl,
        PublishedAt = entity.PublishedAt,
        RegionFocus = Enum.Parse<NewsRegionFocus>(entity.RegionFocus),
        Category = Enum.Parse<NewsCategory>(entity.Category),
        CategoryEmoji = entity.CategoryEmoji,
        ReadingMinutes = entity.ReadingMinutes,
        Difficulty = Enum.Parse<NewsDifficulty>(entity.Difficulty),
        WhyImportant = entity.WhyImportant,
        MeaningForKids = entity.MeaningForKids,
        ExplainedTerms = JsonSerializer.Deserialize<List<ExplainedTerm>>(entity.ExplainedTermsJson, JsonOptions.Default)
                         ?? new List<ExplainedTerm>(),
        BerlinDistrict = entity.BerlinDistrict,
        ComprehensionQuestions = JsonSerializer.Deserialize<List<QuizQuestion>>(entity.ComprehensionQuestionsJson, JsonOptions.Default)
                                 ?? new List<QuizQuestion>()
    };
}
