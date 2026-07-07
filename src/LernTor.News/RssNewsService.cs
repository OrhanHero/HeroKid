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
    /// und liefert die besten <paramref name="targetCount"/> kindgerecht aufbereiteten Artikel.
    /// Fehlerhafte/nicht erreichbare Feeds werden übersprungen statt die ganze Ladung abzubrechen.
    /// </summary>
    public async Task<IReadOnlyList<NewsArticle>> LoadCuratedArticlesAsync(int targetCount = 8, CancellationToken cancellationToken = default)
    {
        var allRawItems = new List<(SyndicationItem Item, NewsFeedSource Source)>();

        foreach (var source in CuratedNewsFeeds.All)
        {
            try
            {
                var items = await FetchFeedAsync(source, cancellationToken);
                allRawItems.AddRange(items.Select(i => (i, source)));
            }
            catch (Exception)
            {
                // Feed nicht erreichbar oder URL veraltet -> überspringen, restliche Feeds trotzdem laden.
            }
        }

        var ranked = allRawItems
            .OrderByDescending(x => CountPriorityMatches(x.Item.Title?.Text, x.Item.Summary?.Text))
            .ThenByDescending(x => x.Item.PublishDate)
            .Take(targetCount)
            .ToList();

        var articles = new List<NewsArticle>();
        foreach (var (item, source) in ranked)
        {
            articles.Add(BuildArticle(item, source));
        }

        return articles;
    }

    private static int CountPriorityMatches(string? title, string? summary)
    {
        var text = $"{title} {summary}";
        return CuratedNewsFeeds.PriorityKeywords.Count(keyword =>
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
            ComprehensionQuestions = Array.Empty<QuizQuestion>()
        };

        var questions = _questionGenerator.GenerateQuestions(article);

        return article with { ComprehensionQuestions = questions };
    }
}
