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
        // KI/Technik-Quelle: erweitert die News um Digital-/KI-Themen, die für Kinder/Jugendliche
        // zunehmend alltagsrelevant sind (siehe auch die "KI"/"Künstliche Intelligenz"-Schlüsselwörter
        // unten, die Artikel zu diesem Thema auch in den anderen Feeds nach oben priorisieren).
        new NewsFeedSource("heise online", "https://www.heise.de/rss/heise-atom.xml", NewsRegionFocus.Deutschland, IsGerman: true),
    };

    /// <summary>Schlüsselwörter zur Priorisierung von Artikeln nach den gewünschten Regionen/Themen.</summary>
    public static readonly IReadOnlyList<string> PriorityKeywords = new[]
    {
        "Berlin", "Deutschland", "Istanbul", "Samsun", "Ünye", "Unye", "Türkei", "Turkiye", "Turkei",
        "Künstliche Intelligenz", "KI", "ChatGPT", "Roboter", "Digital"
    };

    /// <summary>
    /// Schlüsselwörter für Themen, die für die Zielgruppe (10-15 Jahre) eher ungeeignet/verstörend
    /// sind (Krieg, Gewaltverbrechen, Suizid, ...). Solche Artikel werden nicht hart ausgefiltert
    /// (an manchen Tagen gäbe es sonst zu wenige Artikel), aber in der Rangliste deutlich nach unten
    /// gestuft, damit harmlosere Artikel bevorzugt ausgewählt werden.
    /// </summary>
    public static readonly IReadOnlyList<string> SensitiveKeywords = new[]
    {
        "Krieg", "Mord", "Amoklauf", "Terror", "Anschlag", "Attentat", "Vergewaltigung",
        "Missbrauch", "Selbstmord", "Suizid", "Leiche", "getötet", "erschossen", "Gewaltverbrechen"
    };
}
