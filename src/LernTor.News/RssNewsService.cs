using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using LernTor.Core.Models;

namespace LernTor.News;

public sealed class RssNewsService
{
    private static readonly XmlReaderSettings FeedReaderSettings = new()
    {
        IgnoreWhitespace = true,
        DtdProcessing = DtdProcessing.Parse,
        XmlResolver = null
    };

    private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

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
    /// Lädt aus jedem kuratierten Feed genau den neuesten Artikel und bereitet ihn kindgerecht auf.
    /// Fehlerhafte/nicht erreichbare Feeds werden übersprungen statt die ganze Ladung abzubrechen.
    /// Das Ergebnis ist dadurch bewusst klein und stabil: pro Feed eine News, keine Quoten-, Extra-
    /// oder Zusatzsammlungen mehr.
    /// </summary>
    /// <param name="childAge">Alter des aktiven Kind-Profils für den automatischen Altersfilter:
    /// bis einschließlich 9 Jahren werden Artikel mit verstörenden Schlüsselwörtern KOMPLETT
    /// ausgefiltert statt nur herabgestuft ("keine Angstmache" gilt für die Jüngsten strikt);
    /// ab 10 bleibt das mildere Herabstufen, weil sonst an nachrichtenschweren Tagen zu wenige
    /// Artikel übrig blieben. null = kein Alter bekannt, Standardverhalten.</param>
    public async Task<IReadOnlyList<NewsArticle>> LoadCuratedArticlesAsync(int targetCount = 8, int? childAge = null, CancellationToken cancellationToken = default)
    {
        var articles = new List<NewsArticle>();

        foreach (var source in CuratedNewsFeeds.All)
        {
            try
            {
                var items = await FetchFeedAsync(source, cancellationToken);

                var latestItem = SelectLatestItem(items, childAge);
                if (latestItem is not null)
                {
                    articles.Add(BuildArticle(latestItem, source));
                }
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

        return articles;
    }

    private static SyndicationItem? SelectLatestItem(IReadOnlyList<SyndicationItem> items, int? childAge)
    {
        var ordered = items.OrderByDescending(item => item.PublishDate).ToList();
        if (childAge is <= 9)
        {
            ordered = ordered
                .Where(item => CountSensitiveMatches(item.Title?.Text, item.Summary?.Text) == 0)
                .ToList();
        }

        return ordered.FirstOrDefault();
    }

    private static int CountSensitiveMatches(string? title, string? summary)
    {
        var text = $"{title} {summary}";
        return CuratedNewsFeeds.SensitiveKeywords.Count(keyword =>
            text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<IReadOnlyList<SyndicationItem>> FetchFeedAsync(NewsFeedSource source, CancellationToken cancellationToken)
    {
        using var request = CreateFeedRequest(source.RssUrl);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer, cancellationToken);
        return ParseFeedContent(buffer.ToArray());
    }

    internal static HttpRequestMessage CreateFeedRequest(string rssUrl)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, rssUrl);
        request.Headers.UserAgent.ParseAdd(DefaultUserAgent);
        request.Headers.Accept.ParseAdd("application/rss+xml");
        request.Headers.Accept.ParseAdd("application/atom+xml");
        request.Headers.Accept.ParseAdd("application/xml");
        request.Headers.Accept.ParseAdd("text/xml");

        return request;
    }

    internal static IReadOnlyList<SyndicationItem> ParseFeedContent(byte[] content)
    {
        using var standardStream = new MemoryStream(content, writable: false);
        using var reader = XmlReader.Create(standardStream, FeedReaderSettings);

        try
        {
            var feed = SyndicationFeed.Load(reader);
            var items = feed?.Items.ToList() ?? [];
            if (items.Count > 0)
            {
                return items;
            }
        }
        catch (XmlException)
        {
        }
        catch (FormatException)
        {
        }

        try
        {
            using var rdfStream = new MemoryStream(content, writable: false);
            var document = XDocument.Load(rdfStream, LoadOptions.None);
            var root = document.Root;
            if (root is not null && IsRdfFeed(root))
            {
                return ParseRdfDocument(document);
            }
        }
        catch (XmlException)
        {
        }
        catch (FormatException)
        {
        }

        return [];
    }

    internal static IReadOnlyList<SyndicationItem> ParseRdfFallback(Stream stream)
    {
        var document = XDocument.Load(stream, LoadOptions.None);
        return ParseRdfDocument(document);
    }

    private static bool IsRdfFeed(XElement root) =>
        root.Name.LocalName.Equals("RDF", StringComparison.OrdinalIgnoreCase) ||
        root.Name.NamespaceName.Equals("http://www.w3.org/1999/02/22-rdf-syntax-ns#", StringComparison.OrdinalIgnoreCase);

    private static IReadOnlyList<SyndicationItem> ParseRdfDocument(XDocument document)
    {
        var items = new List<SyndicationItem>();
        var root = document.Root;
        if (root is null)
        {
            return items;
        }

        foreach (var itemElement in root.Elements().Where(e => e.Name.LocalName.Equals("item", StringComparison.OrdinalIgnoreCase)))
        {
            var item = new SyndicationItem();
            var title = GetElementValue(itemElement, "title");
            var link = GetElementValue(itemElement, "link");
            var description = GetElementValue(itemElement, "description", "encoded", "content");
            var guid = GetElementValue(itemElement, "guid", "id");
            var pubDate = GetElementValue(itemElement, "pubDate", "date", "created");

            if (!string.IsNullOrWhiteSpace(title))
            {
                item.Title = new TextSyndicationContent(title);
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                item.Summary = new TextSyndicationContent(description);
                item.Content = new TextSyndicationContent(description);
            }

            if (Uri.TryCreate(link, UriKind.Absolute, out var linkUri))
            {
                item.Links.Add(SyndicationLink.CreateAlternateLink(linkUri));
            }

            if (!string.IsNullOrWhiteSpace(guid))
            {
                item.Id = guid;
            }
            else if (!string.IsNullOrWhiteSpace(link))
            {
                item.Id = link;
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                item.Id = title;
            }

            if (DateTimeOffset.TryParse(pubDate, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out var publishedAt))
            {
                item.PublishDate = publishedAt;
            }

            items.Add(item);
        }

        return items;
    }

    private static string GetElementValue(XElement itemElement, params string[] localNames)
    {
        foreach (var localName in localNames)
        {
            var matchedElement = itemElement.Elements().FirstOrDefault(e => e.Name.LocalName.Equals(localName, StringComparison.OrdinalIgnoreCase));
            if (matchedElement is not null && !string.IsNullOrWhiteSpace(matchedElement.Value))
            {
                return matchedElement.Value.Trim();
            }
        }

        return string.Empty;
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
