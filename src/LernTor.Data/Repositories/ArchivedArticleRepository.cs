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
    /// <summary>
    /// So viele Tage bleiben Archiv-Stände erhalten, ältere werden beim Archivieren entfernt.
    ///
    /// <para>Von 7 auf 21 erhöht: sieben Tage decken einen Internetausfall ab, aber keinen
    /// Urlaub. Drei Wochen Vorrat kosten wenige hundert Kilobyte und sind der Unterschied
    /// zwischen "der News-Teil funktioniert auch in der Türkei" und "ab Tag acht sieht das Kind
    /// jeden Morgen dieselben Nachrichten".</para>
    /// </summary>
    private const int RetentionDays = 21;

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

    /// <summary>Alle Tage, für die ein Archiv-Stand vorliegt - jüngster zuerst.</summary>
    public async Task<IReadOnlyList<DateOnly>> GetArchivedDatesAsync(CancellationToken cancellationToken = default)
    {
        var keys = await _db.ArchivedArticles
            .Select(a => a.ArchivedDate)
            .Distinct()
            .ToListAsync(cancellationToken);

        return keys
            .Select(key => DateOnly.TryParseExact(key, "yyyy-MM-dd", out var date) ? date : (DateOnly?)null)
            .Where(date => date is not null)
            .Select(date => date!.Value)
            .OrderByDescending(date => date)
            .ToList();
    }

    /// <summary>
    /// Der Offline-Rückfall: Artikel eines archivierten Tages, wenn heute kein Feed erreichbar ist.
    ///
    /// <para>Bisher kam hier immer der JÜNGSTE Stand zurück. Bei einem Ausfall über mehrere Tage
    /// - Urlaub, Router kaputt - bekam das Kind damit jeden Morgen exakt dieselben Nachrichten
    /// vorgesetzt, inklusive derselben Verständnisfragen. Stattdessen wandert die Auswahl mit
    /// jedem Ausfalltag einen Archiv-Tag weiter zurück und beginnt danach wieder vorne.</para>
    ///
    /// <para>Am ersten Ausfalltag ist das der Stand von gestern (das Frischeste, was es gibt), am
    /// zweiten der von vorgestern, und so fort. Deterministisch aus dem Datum abgeleitet, also
    /// innerhalb eines Tages stabil - beim zweiten Öffnen dürfen nicht plötzlich andere Artikel
    /// dastehen, der Fortschritt hängt an den Artikel-IDs.</para>
    /// </summary>
    /// <returns>Die Artikel und der Tag, von dem sie stammen; leere Liste, wenn nie archiviert wurde.</returns>
    public async Task<(IReadOnlyList<NewsArticle> Articles, DateOnly? ArchivedOn)> GetOfflineFallbackAsync(
        DateOnly today, CancellationToken cancellationToken = default)
    {
        var dates = await GetArchivedDatesAsync(cancellationToken);

        // Der heutige Stand zaehlt nicht: wenn heute nichts geladen werden konnte, gibt es ihn
        // entweder gar nicht, oder er stammt aus einem frueheren Rueckfall.
        var usable = dates.Where(date => date < today).ToList();
        if (usable.Count == 0)
        {
            return (Array.Empty<NewsArticle>(), null);
        }

        // Wie viele Tage der Ausfall schon dauert, gemessen am juengsten Archiv-Stand.
        var daysOffline = today.DayNumber - usable[0].DayNumber - 1;
        var index = ((daysOffline % usable.Count) + usable.Count) % usable.Count;
        var chosen = usable[index];

        var key = chosen.ToString("yyyy-MM-dd");
        var entities = await _db.ArchivedArticles
            .Where(a => a.ArchivedDate == key)
            .ToListAsync(cancellationToken);

        return (entities.Select(ToArticle).ToList(), chosen);
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
