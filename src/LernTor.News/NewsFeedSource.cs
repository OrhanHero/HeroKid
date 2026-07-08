using LernTor.Core.Models;

namespace LernTor.News;

public sealed record NewsFeedSource(string Name, string RssUrl, NewsRegionFocus RegionFocus, bool IsGerman);

/// <summary>
/// Kuratierte, kostenlose RSS-Quellen mit Fokus auf Deutschland/Berlin und Türkei/Istanbul/Samsun/Ünye.
/// Hinweis: RSS-Endpunkte von Nachrichtenseiten ändern sich gelegentlich – bei Fetch-Fehlern bitte die
/// URL beim jeweiligen Anbieter neu prüfen (das Programm überspringt fehlerhafte Feeds automatisch).
/// </summary>
public static class CuratedNewsFeeds
{
    public static readonly IReadOnlyList<NewsFeedSource> All = new[]
    {
        new NewsFeedSource("tagesschau.de", "https://www.tagesschau.de/xml/rss2/", NewsRegionFocus.Deutschland, IsGerman: true),
        new NewsFeedSource("rbb24 Berlin", "https://www.rbb24.de/aktuell/index.xml", NewsRegionFocus.Berlin, IsGerman: true),
        new NewsFeedSource("Tagesspiegel Berlin", "https://www.tagesspiegel.de/contentexport/feed/home", NewsRegionFocus.Berlin, IsGerman: true),
        new NewsFeedSource("Hürriyet", "https://www.hurriyet.com.tr/rss/anasayfa", NewsRegionFocus.Tuerkei, IsGerman: false),
        new NewsFeedSource("Sabah", "https://www.sabah.com.tr/rss/anasayfa.xml", NewsRegionFocus.Tuerkei, IsGerman: false),
    };

    /// <summary>Schlüsselwörter zur Priorisierung von Artikeln nach den gewünschten Regionen.</summary>
    public static readonly IReadOnlyList<string> PriorityKeywords = new[]
    {
        "Berlin", "Deutschland", "Istanbul", "Samsun", "Ünye", "Unye", "Türkei", "Turkiye", "Turkei"
    };
}
