using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using LernTor.Core.Enums;
using LernTor.Core.Logging;
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
    private readonly FeedCache _feedCache;

    public RssNewsService(
        HttpClient httpClient,
        ITextSimplifier simplifier,
        IComprehensionQuestionGenerator questionGenerator,
        FeedCache? feedCache = null)
    {
        _httpClient = httpClient;
        _simplifier = simplifier;
        _questionGenerator = questionGenerator;
        _feedCache = feedCache ?? new FeedCache();
    }

    /// <summary>
    /// Lädt aus jedem kuratierten Feed genau den neuesten Artikel und bereitet ihn kindgerecht auf.
    /// Fehlerhafte/nicht erreichbare Feeds werden übersprungen statt die ganze Ladung abzubrechen.
    /// Das Ergebnis ist dadurch bewusst klein und stabil: pro Feed eine News, keine Quoten- oder
    /// Extrasammlungen - einzige Ausnahme ist das tägliche, fest angehängte Finanzwissen-Erklärstück
    /// (siehe <see cref="FinanceKnowledgeArticles"/>), da es dafür keinen verlässlichen RSS-Feed gibt.
    /// </summary>
    /// <param name="childAge">Alter des aktiven Kind-Profils für den automatischen Altersfilter:
    /// bis einschließlich 9 Jahren werden Artikel mit verstörenden Schlüsselwörtern KOMPLETT
    /// ausgefiltert statt nur herabgestuft ("keine Angstmache" gilt für die Jüngsten strikt);
    /// ab 10 bleibt das mildere Herabstufen, weil sonst an nachrichtenschweren Tagen zu wenige
    /// Artikel übrig blieben. null = kein Alter bekannt, Standardverhalten.</param>
    /// <param name="gradeLevel">Klassenstufe (6 oder 9) für altersgerechte Textvereinfachung:
    /// Klasse 6 = stark vereinfacht (kurze Sätze, einfaches Vokabular, Aktiv statt Passiv),
    /// Klasse 9 = mild vereinfacht (normale Satzstruktur, nur schwierigste Wörter ersetzt).</param>
    /// <param name="disabledFeedNames">Von den Eltern im Eltern-Bereich abgeschaltete Quellen
    /// (Name aus <see cref="CuratedNewsFeeds.All"/>). Bleibt nichts übrig, greifen bewusst wieder
    /// alle Quellen: ein leerer News-Bereich wäre schlimmer als eine ignorierte Einstellung.</param>
    public async Task<IReadOnlyList<NewsArticle>> LoadCuratedArticlesAsync(
        int targetCount = 8,
        int? childAge = null,
        GradeLevel gradeLevel = GradeLevel.Klasse6,
        IReadOnlySet<string>? disabledFeedNames = null,
        CancellationToken cancellationToken = default)
    {
        var articles = new List<NewsArticle>();

        var activeFeeds = disabledFeedNames is null || disabledFeedNames.Count == 0
            ? CuratedNewsFeeds.All
            : CuratedNewsFeeds.All.Where(f => !disabledFeedNames.Contains(f.Name)).ToList();

        if (activeFeeds.Count == 0)
        {
            activeFeeds = CuratedNewsFeeds.All;
        }

        // Nicht mehr ALLE aktiven Quellen abrufen: seit der Katalog 44 Quellen umfasst, waeren das
        // 44 HTTP-Abrufe beim Start und 45 Pflichtartikel am Tag. Stattdessen die Tagesauswahl -
        // targetCount war vorher ein toter Parameter.
        var feedsForToday = SelectFeedsForDay(activeFeeds, targetCount, DateOnly.FromDateTime(DateTime.Today));

        foreach (var source in feedsForToday)
        {
            try
            {
                var items = await FetchFeedAsync(source, cancellationToken);

                var latestItem = SelectLatestItem(items, childAge);
                if (latestItem is not null)
                {
                    articles.Add(BuildArticle(latestItem, source, gradeLevel));
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

        // Zu Finanzthemen gibt es selten kindtaugliche Tagesmeldungen in den RSS-Feeds (siehe
        // FinanceKnowledgeArticles) - deshalb hängt sich hier immer EIN rotierendes, kuratiertes
        // Erklärstück an, statt auf einen (unzuverlässigen) Finanz-RSS-Feed zu setzen.
        articles.Add(FinanceKnowledgeArticles.GetForDate(DateOnly.FromDateTime(DateTime.Today), gradeLevel));

        return articles;
    }

    /// <summary>
    /// Wählt die Quellen aus, die an einem bestimmten Tag abgerufen werden.
    ///
    /// <para>Deterministisch aus dem Datum abgeleitet: innerhalb eines Tages ist die Auswahl
    /// stabil (die App darf beim zweiten Öffnen nicht plötzlich andere Nachrichten zeigen), über
    /// mehrere Tage wandert sie durch den ganzen Katalog. Der Startpunkt springt je Sprachgruppe
    /// um die Tagesmenge weiter, damit aufeinanderfolgende Tage möglichst wenig überlappen.</para>
    ///
    /// <para>Die Plätze werden über die Sprachen verteilt statt einfach der Reihe nach vergeben:
    /// sonst bestünde ein Tag leicht nur aus deutschen Quellen (27 von 44), und Türkisch und
    /// Englisch - der halbe Sinn der Sammlung - kämen kaum vor.</para>
    /// </summary>
    internal static IReadOnlyList<NewsFeedSource> SelectFeedsForDay(
        IReadOnlyList<NewsFeedSource> activeFeeds,
        int targetCount,
        DateOnly day)
    {
        if (targetCount <= 0 || activeFeeds.Count <= targetCount)
        {
            return activeFeeds;
        }

        var groups = activeFeeds
            .GroupBy(feed => feed.Language)
            .OrderBy(group => group.Key)
            .Select(group => group.OrderBy(feed => feed.Name, StringComparer.Ordinal).ToList())
            .ToList();

        var quotas = DistributeQuotas(groups.Select(group => group.Count).ToList(), targetCount);

        var selected = new List<NewsFeedSource>(targetCount);
        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var quota = quotas[i];
            if (quota <= 0)
            {
                continue;
            }

            // Schrittweite = Tagesmenge: der Block wandert taeglich um genau seine eigene Laenge
            // weiter, aufeinanderfolgende Tage zeigen also andere Quellen.
            var offset = (int)(((long)day.DayNumber * quota) % group.Count);
            for (var taken = 0; taken < quota; taken++)
            {
                selected.Add(group[(offset + taken) % group.Count]);
            }
        }

        return selected;
    }

    /// <summary>
    /// Verteilt <paramref name="targetCount"/> Plätze auf die Sprachgruppen: erst einer je
    /// Sprache, der Rest proportional zur Gruppengröße (größter Rest zuerst), gedeckelt auf die
    /// tatsächlich vorhandenen Quellen.
    /// </summary>
    private static int[] DistributeQuotas(IReadOnlyList<int> groupSizes, int targetCount)
    {
        var quotas = new int[groupSizes.Count];
        var total = groupSizes.Sum();
        if (total == 0)
        {
            return quotas;
        }

        // Jede Sprache bekommt zuerst genau einen Platz - ein Tag ganz ohne tuerkische oder
        // englische Nachricht waere das Gegenteil dessen, wofuer die Quellen da sind.
        var assigned = 0;
        for (var i = 0; i < quotas.Length && assigned < targetCount; i++, assigned++)
        {
            quotas[i] = 1;
        }

        var rest = targetCount - assigned;
        if (rest > 0)
        {
            var exact = groupSizes.Select(size => rest * (double)size / total).ToArray();
            for (var i = 0; i < quotas.Length; i++)
            {
                quotas[i] += (int)Math.Floor(exact[i]);
            }

            var open = targetCount - quotas.Sum();
            foreach (var index in Enumerable.Range(0, quotas.Length)
                         .OrderByDescending(i => exact[i] - Math.Floor(exact[i]))
                         .ThenBy(i => i))
            {
                if (open <= 0)
                {
                    break;
                }

                quotas[index]++;
                open--;
            }
        }

        // Keine Gruppe darf mehr Plaetze bekommen, als sie Quellen hat.
        for (var i = 0; i < quotas.Length; i++)
        {
            quotas[i] = Math.Min(quotas[i], groupSizes[i]);
        }

        // Was durch die Deckelung frei wurde, geht an Gruppen, die noch Quellen uebrig haben.
        var shortfall = targetCount - quotas.Sum();
        while (shortfall > 0)
        {
            var progressed = false;
            for (var i = 0; i < quotas.Length && shortfall > 0; i++)
            {
                if (quotas[i] >= groupSizes[i])
                {
                    continue;
                }

                quotas[i]++;
                shortfall--;
                progressed = true;
            }

            if (!progressed)
            {
                break;
            }
        }

        return quotas;
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
        byte[] content;
        try
        {
            using var request = CreateFeedRequest(source.RssUrl);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var buffer = new MemoryStream();
            await stream.CopyToAsync(buffer, cancellationToken);
            content = buffer.ToArray();
            _feedCache.Save(source.RssUrl, content);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Offline-Fallback (siehe FeedCache): letzter erfolgreicher Abruf, sofern < 48h alt.
            // OperationCanceledException bleibt ausgenommen - App-Beenden soll nicht heimlich
            // alte News servieren, sondern wirklich abbrechen.
            var cached = _feedCache.TryLoad(source.RssUrl);
            if (cached is null)
            {
                throw;
            }

            AppLog.Warn("News", $"Feed offline, nutze Cache (max. 48h): {source.Name} - {ex.Message}");
            content = cached;
        }

        return ParseFeedContent(content);
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

    /// <summary>
    /// Holt den Fließtext eines Eintrags. Atom-Feeds dürfen <c>summary</c> weglassen und nur
    /// <c>content</c> liefern - genau das tut z.B. heise online. Wurde nur <c>Summary</c>
    /// ausgelesen, blieb der Text dieser Quelle leer, und in der Folge fehlten dem Artikel die
    /// Verständnisfragen (realer Fund aus dem Familienbetrieb). Deshalb wird <c>Content</c> als
    /// gleichwertige Quelle behandelt, und der jeweils längere Text gewinnt: manche Feeds füllen
    /// <c>summary</c> nur mit einer Zeile, während <c>content</c> den ganzen Teaser enthält.
    /// </summary>
    internal static string ExtractRawSummary(SyndicationItem item)
    {
        var summary = item.Summary?.Text ?? string.Empty;
        var content = (item.Content as TextSyndicationContent)?.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(summary))
        {
            return content.Trim();
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return summary.Trim();
        }

        return content.Trim().Length > summary.Trim().Length ? content.Trim() : summary.Trim();
    }

    private NewsArticle BuildArticle(SyndicationItem item, NewsFeedSource source, GradeLevel gradeLevel)
    {
        var title = item.Title?.Text ?? "Ohne Titel";
        var rawSummary = ExtractRawSummary(item);
        var simplified = _simplifier.Simplify(rawSummary, gradeLevel);
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
            WhyImportant = KidNewsMetadata.WhyImportantFor(category, gradeLevel),
            MeaningForKids = KidNewsMetadata.MeaningForKidsFor(category, gradeLevel),
            ExplainedTerms = KidTermGlossary.FindTerms($"{title} {simplified}"),
            // 📍-Bezirks-Chip statt Kartenansicht (siehe BerlinDistrictDetector) - auch für
            // Nicht-Berlin-Quellen geprüft, falls z.B. eine tagesschau-Meldung Spandau betrifft.
            BerlinDistrict = BerlinDistrictDetector.Detect($"{title} {simplified}")
        };

        var questions = _questionGenerator.GenerateQuestions(article);

        return article with { ComprehensionQuestions = questions };
    }
}
