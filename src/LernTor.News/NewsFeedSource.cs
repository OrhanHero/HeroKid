using LernTor.Core.Models;

namespace LernTor.News;

public sealed record NewsFeedSource(
    string Name,
    string RssUrl,
    NewsRegionFocus RegionFocus,
    bool IsGerman,
    NewsCategory DefaultCategory);

/// <summary>
/// Kuratierte, kostenlose RSS-Quellen - ausschließlich seriöse Anbieter (öffentlich-rechtliche
/// Sender, Nachrichtenagenturen, etablierte Regionalzeitungen), bewusst KEINE Boulevardquellen.
/// Türkei-Nachrichten kommen von Anadolu Ajansı (staatliche Nachrichtenagentur), TRT Haber und
/// der Deutschen Welle Türkçe statt von Boulevard-Portalen.
///
/// <para>Hinweis: RSS-Endpunkte ändern sich gelegentlich, und aus der Entwicklungs-Sandbox sind
/// Nachrichten-Domains nicht erreichbar - die URLs sind daher nach öffentlicher Dokumentation
/// eingetragen, aber nicht per Testabruf verifiziert. Das Programm überspringt nicht erreichbare
/// Feeds automatisch (siehe RssNewsService), ein einzelner toter Feed bricht nichts.</para>
/// </summary>
public static class CuratedNewsFeeds
{
    public static readonly IReadOnlyList<NewsFeedSource> All = new[]
    {
        // Deutschland (öffentlich-rechtlich/agenturnah)
        new NewsFeedSource("tagesschau.de", "https://www.tagesschau.de/xml/rss2/",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),

        // Welt: Deutsche Welle berichtet auf Deutsch über internationale Themen.
        new NewsFeedSource("Deutsche Welle", "https://rss.dw.com/xml/rss-de-all",
            NewsRegionFocus.International, IsGerman: true, NewsCategory.Welt),

        // Berlin - die wichtigste regionale Rubrik, deshalb gleich drei Quellen.
        new NewsFeedSource("rbb24 Berlin", "https://www.rbb24.de/aktuell/index.xml",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Berlin),
        new NewsFeedSource("Tagesspiegel Berlin", "https://www.tagesspiegel.de/contentexport/feed/home",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Berlin),
        new NewsFeedSource("Berliner Morgenpost", "https://www.morgenpost.de/rss",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Berlin),

        // Türkei (seriöse Agenturen/Sender statt Boulevard)
        new NewsFeedSource("Anadolu Ajansı", "https://www.aa.com.tr/tr/rss/default?cat=guncel",
            NewsRegionFocus.Tuerkei, IsGerman: false, NewsCategory.Tuerkei),
        new NewsFeedSource("TRT Haber", "https://www.trthaber.com/sondakika.rss",
            NewsRegionFocus.Tuerkei, IsGerman: false, NewsCategory.Tuerkei),
        new NewsFeedSource("DW Türkçe", "https://rss.dw.com/xml/rss-tur-all",
            NewsRegionFocus.Tuerkei, IsGerman: false, NewsCategory.Tuerkei),

        // KI/Technik-Quelle: Digital-/KI-Themen sind für Kinder/Jugendliche zunehmend
        // alltagsrelevant; der NewsCategoryClassifier sortiert die Artikel in die KI-Rubrik.
        new NewsFeedSource("heise online", "https://www.heise.de/rss/heise-atom.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),
    };

    /// <summary>Schlüsselwörter zur Priorisierung von Artikeln nach den gewünschten Regionen/Themen.</summary>
    public static readonly IReadOnlyList<string> PriorityKeywords = new[]
    {
        "Berlin", "Deutschland", "Istanbul", "Samsun", "Ünye", "Unye", "Türkei", "Turkiye", "Turkei",
        "Künstliche Intelligenz", "KI", "ChatGPT", "Roboter", "Digital",
        "Nintendo", "Minecraft", "Pokémon", "Schule", "Bildung", "Wissenschaft"
    };

    /// <summary>
    /// Schlüsselwörter für Themen, die für die Zielgruppe (8-16 Jahre) eher ungeeignet/verstörend
    /// sind (Krieg, Gewaltverbrechen, Suizid, ...) - "keine Angstmache". Solche Artikel werden
    /// nicht hart ausgefiltert (an manchen Tagen gäbe es sonst zu wenige Artikel), aber in der
    /// Rangliste deutlich nach unten gestuft, damit harmlosere Artikel bevorzugt ausgewählt werden.
    /// </summary>
    public static readonly IReadOnlyList<string> SensitiveKeywords = new[]
    {
        "Krieg", "Mord", "Amoklauf", "Terror", "Anschlag", "Attentat", "Vergewaltigung",
        "Missbrauch", "Selbstmord", "Suizid", "Leiche", "getötet", "erschossen", "Gewaltverbrechen",
        // Türkischsprachige Entsprechungen für die Türkei-Feeds:
        "savaş", "cinayet", "terör", "saldırı", "tecavüz", "intihar", "öldürüldü"
    };
}
