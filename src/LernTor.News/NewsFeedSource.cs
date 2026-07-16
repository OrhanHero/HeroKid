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

        // Berlin: Bezirks- & Parlaments-Pressemitteilungen (direkt von der Verwaltung)
        new NewsFeedSource("Berlin.de Bezirksamt Neukölln", "https://www.berlin.de/presse/pressemitteilungen/index/feed?institutions%5B%5D=Bezirksamt+Neuk%C3%B6lln",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Berlin),
        new NewsFeedSource("Berlin.de Bezirksamt Friedrichshain-Kreuzberg", "https://www.berlin.de/presse/pressemitteilungen/index/feed?institutions%5B%5D=Bezirksamt+Friedrichshain-Kreuzberg",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Berlin),
        new NewsFeedSource("Abgeordnetenhaus Berlin", "https://www.parlament-berlin.de/rss/meldungen",
            NewsRegionFocus.Berlin, IsGerman: true, NewsCategory.Deutschland),

        // Jugendmagazin der BpB (politische Bildung, jugendgerecht)
        new NewsFeedSource("fluter.de", "https://www.fluter.de/rss.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),

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

        // Spiele-Rubrik: echte Gaming-Nachrichten statt nur zufälliger Spiele-Treffer aus
        // allgemeinen Quellen (siehe NewsCategoryClassifier).
        new NewsFeedSource("GameStar", "https://www.gamestar.de/rss/gaming.rss",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Spiele),

        // Offizielle Hersteller-News (DE) – stärken die Spiele-Rubrik unabhängig von GameStar
        new NewsFeedSource("Nintendo.de News", "https://www.nintendo.com/de-de/news.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Spiele),
        new NewsFeedSource("PlayStation Blog DE", "https://blog.de.playstation.com/feed",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Spiele),
        new NewsFeedSource("Xbox News DE", "https://news.xbox.com/de-de/feed/",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Spiele),
        new NewsFeedSource("Steam News", "https://store.steampowered.com/feeds/news.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Spiele),

        // Bundesregierung: offizielle, seriöse Quelle für "was macht eigentlich die Regierung" -
        // kompakt für kurze Meldungen, Pressemitteilungen für ausführlichere Themen.
        new NewsFeedSource("Bundesregierung kompakt", "https://www.bundesregierung.de/service/rss/breg-de/1151242/feed.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),
        new NewsFeedSource("Bundesregierung Pressemitteilungen", "https://www.bundesregierung.de/service/rss/breg-de/1151244/feed.xml",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),

        // BMBFSFJ: Themen wie Bildung, Familie und Jugendschutz betreffen die Zielgruppe direkt.
        new NewsFeedSource("BMBFSFJ", "https://www.bmbfsfj.bund.de/service/rss/bmbfsfj/108854/feed.rss",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Deutschland),

        // IT Boltwise: zusätzliche Technik-/KI-Quelle neben heise, stärkt die KI-Rubrik.
        new NewsFeedSource("IT Boltwise", "https://www.it-boltwise.de/themen/allgemein/feed",
            NewsRegionFocus.Deutschland, IsGerman: true, NewsCategory.Ki),
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
