using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Xml;
using LernTor.Core.Models;

namespace LernTor.News;

public sealed class RssNewsService
{
    private readonly HttpClient _httpClient;
    private readonly ITextSimplifier _simplifier;
    private readonly IComprehensionQuestionGenerator _questionGenerator;

    public RssNewsService(HttpClient httpClient, ITextSimplifier simplifier, IComprehensionQuestionGenerator questionGenerator)
    {
        _httpClient = httpClient;
        _simplifier = simplifier;
        _questionGenerator = questionGenerator;
    }

    /// <summary>
    /// Lädt Artikel aus allen kuratierten Feeds, priorisiert Berlin/Deutschland/Istanbul/Samsun/Ünye
    /// (sowie KI-/Digitalthemen) und liefert die besten <paramref name="targetCount"/> kindgerecht
    /// aufbereiteten Artikel. Artikel mit verstörenden Themen (Krieg, Gewaltverbrechen, ...) werden
    /// nicht hart ausgefiltert, aber in der Rangliste deutlich nach unten gestuft (siehe
    /// CuratedNewsFeeds.SensitiveKeywords), damit harmlosere Artikel bevorzugt ausgewählt werden.
    /// Fehlerhafte/nicht erreichbare Feeds werden übersprungen statt die ganze Ladung abzubrechen.
    /// </summary>
    /// <param name="childAge">Alter des aktiven Kind-Profils für den automatischen Altersfilter:
    /// bis einschließlich 9 Jahren werden Artikel mit verstörenden Schlüsselwörtern KOMPLETT
    /// ausgefiltert statt nur herabgestuft ("keine Angstmache" gilt für die Jüngsten strikt);
    /// ab 10 bleibt das mildere Herabstufen, weil sonst an nachrichtenschweren Tagen zu wenige
    /// Artikel übrig blieben. null = kein Alter bekannt, Standardverhalten.</param>
    public async Task<IReadOnlyList<NewsArticle>> LoadCuratedArticlesAsync(int targetCount = 8, int? childAge = null, CancellationToken cancellationToken = default)
    {
        var allRawItems = new List<(SyndicationItem Item, NewsFeedSource Source)>();

        foreach (var source in CuratedNewsFeeds.All)
        {
            try
            {
                var items = await FetchFeedAsync(source, cancellationToken);
                allRawItems.AddRange(items.Select(i => (i, source)));
            }
            catch (Exception ex)
            {
                // Feed nicht erreichbar oder URL veraltet -> überspringen, restliche Feeds trotzdem
                // laden. Fürs Kind unsichtbar, aber im Fehlerprotokoll nachvollziehbar, WELCHE
                // Quelle tot ist (wichtig zum Pflegen der Feed-URLs).
                LernTor.Core.Logging.AppLog.Warn(
                    "News", $"Feed übersprungen: {source.Name} ({source.RssUrl}) - {ex.Message}");
            }
        }

        // Verschiedene Feeds (oder URL-Varianten desselben Anbieters) liefern gelegentlich exakt
        // dieselbe Meldung - ohne Deduplizierung nach Titel würde derselbe Artikel doppelt
        // hintereinander im News-Bereich (und darüber auch im Abschlussquiz) auftauchen.
        var deduplicated = allRawItems
            .GroupBy(x => NormalizeTitleForDeduplication(x.Item.Title?.Text))
            .Select(group => group.First())
            .ToList();

        // Altersfilter (siehe Parameter-Doku): für die Jüngsten harte Filterung statt Herabstufung.
        if (childAge is <= 9)
        {
            deduplicated = deduplicated
                .Where(x => CountSensitiveMatches(x.Item.Title?.Text, x.Item.Summary?.Text) == 0)
                .ToList();
        }

        // Berlin-Lokalnachrichten sind die wichtigste regionale Rubrik und sollen nicht zufällig
        // untergehen, nur weil sie im allgemeinen Ranking knapp nicht vorne landen - deshalb wird
        // zuerst ein garantiertes Kontingent an aktuellen Berlin-Artikeln reserviert. Danach ein
        // kleineres Türkei-Kontingent ("täglich jugendgerechte Nachrichten aus der Türkei"),
        // erst dann füllt die übliche Prioritäts-Rangliste die restlichen Plätze auf.
        var minBerlinSlots = Math.Max(2, targetCount / 3);
        var berlinArticles = TakeQuota(deduplicated, NewsRegionFocus.Berlin, minBerlinSlots);

        var minTurkeySlots = Math.Min(2, Math.Max(0, targetCount - berlinArticles.Count));
        var turkeyArticles = TakeQuota(deduplicated, NewsRegionFocus.Tuerkei, minTurkeySlots);

        var reserved = berlinArticles.Concat(turkeyArticles).ToList();
        var remainingSlots = Math.Max(0, targetCount - reserved.Count);
        var rankedRemaining = deduplicated
            .Except(reserved)
            .OrderByDescending(x =>
                CountPriorityMatches(x.Item.Title?.Text, x.Item.Summary?.Text) -
                CountSensitiveMatches(x.Item.Title?.Text, x.Item.Summary?.Text))
            .ThenByDescending(x => x.Item.PublishDate)
            .Take(remainingSlots)
            .ToList();

        var ranked = reserved.Concat(rankedRemaining).ToList();

        var articles = new List<NewsArticle>();
        foreach (var (item, source) in ranked)
        {
            articles.Add(BuildArticle(item, source));
        }

        // Finanzen-Rubrik: täglich EIN rotierendes "Finanzwissen"-Erklärstück anhängen (kuratiert,
        // handgeschriebene Fragen - siehe FinanceKnowledgeArticles). Bewusst zusätzlich zum
        // targetCount: das Stück kommt aus keinem Feed und soll keinen Tagesartikel verdrängen.
        articles.Add(FinanceKnowledgeArticles.GetForDate(DateOnly.FromDateTime(DateTime.Today)));

        return articles;
    }

    /// <summary>Reserviert bis zu <paramref name="slots"/> möglichst unverstörende, aktuelle
    /// Artikel einer Region (für die garantierten Berlin-/Türkei-Kontingente).</summary>
    private static List<(SyndicationItem Item, NewsFeedSource Source)> TakeQuota(
        IEnumerable<(SyndicationItem Item, NewsFeedSource Source)> items,
        NewsRegionFocus region,
        int slots) =>
        items
            .Where(x => x.Source.RegionFocus == region)
            .OrderBy(x => CountSensitiveMatches(x.Item.Title?.Text, x.Item.Summary?.Text))
            .ThenByDescending(x => x.Item.PublishDate)
            .Take(slots)
            .ToList();

    private static string NormalizeTitleForDeduplication(string? title) =>
        (title ?? string.Empty).Trim().ToLowerInvariant();

    private static int CountPriorityMatches(string? title, string? summary)
    {
        var text = $"{title} {summary}";
        return CuratedNewsFeeds.PriorityKeywords.Count(keyword =>
            text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    private static int CountSensitiveMatches(string? title, string? summary)
    {
        var text = $"{title} {summary}";
        return CuratedNewsFeeds.SensitiveKeywords.Count(keyword =>
            text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<IReadOnlyList<SyndicationItem>> FetchFeedAsync(NewsFeedSource source, CancellationToken cancellationToken)
    {
        await using var stream = await _httpClient.GetStreamAsync(source.RssUrl, cancellationToken);
        using var reader = XmlReader.Create(stream);
        var feed = SyndicationFeed.Load(reader);
        return feed?.Items.ToList() ?? new List<SyndicationItem>();
    }

    private NewsArticle BuildArticle(SyndicationItem item, NewsFeedSource source)
    {
        var title = item.Title?.Text ?? "Ohne Titel";
        var rawSummary = item.Summary?.Text ?? string.Empty;
        var simplified = _simplifier.Simplify(rawSummary);
        var imageUrl = item.Links.FirstOrDefault(l => l.MediaType?.StartsWith("image") == true)?.Uri.ToString();

        // Kindgerechte Anreicherung (siehe README "News für Kinder"): Rubrik + Emoji,
        // Lesedauer, Schwierigkeitsgrad, "Warum ist das wichtig?"/"Was bedeutet das für dich?"
        // und sofort erklärte schwierige Wörter - alles regelbasiert und offline.
        var category = NewsCategoryClassifier.Classify(title, simplified, source.DefaultCategory);

        var article = new NewsArticle
        {
            Id = item.Id ?? Guid.NewGuid().ToString("N"),
            Title = title,
            SimplifiedSummary = simplified,
            ImageUrl = imageUrl,
            SourceName = source.Name,
            SourceUrl = item.Links.FirstOrDefault()?.Uri.ToString() ?? source.RssUrl,
            PublishedAt = item.PublishDate,
            RegionFocus = source.RegionFocus,
            ComprehensionQuestions = Array.Empty<QuizQuestion>(),
            Category = category,
            CategoryEmoji = NewsCategoryClassifier.EmojiFor(category),
            ReadingMinutes = KidNewsMetadata.ComputeReadingMinutes(title, simplified),
            Difficulty = KidNewsMetadata.ComputeDifficulty(simplified),
            WhyImportant = KidNewsMetadata.WhyImportantFor(category),
            MeaningForKids = KidNewsMetadata.MeaningForKidsFor(category),
            ExplainedTerms = KidTermGlossary.FindTerms($"{title} {simplified}"),
            // 📍-Bezirks-Chip statt Kartenansicht (siehe BerlinDistrictDetector) - auch für
            // Nicht-Berlin-Quellen geprüft, falls z.B. eine tagesschau-Meldung Spandau betrifft.
            BerlinDistrict = BerlinDistrictDetector.Detect($"{title} {simplified}")
        };

        var questions = _questionGenerator.GenerateQuestions(article);

        return article with { ComprehensionQuestions = questions };
    }
}
