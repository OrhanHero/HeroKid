using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Sprache einer Nachrichtenquelle. Vorher stand hier ein <c>bool IsGerman</c> - der reichte,
/// solange es nur deutsche und türkische Quellen gab, hätte aber jede englische Quelle im
/// Eltern-Bereich als "Türkisch" beschriftet.
/// </summary>
public enum NewsFeedLanguage
{
    Deutsch,
    Tuerkisch,
    Englisch
}

public sealed record NewsFeedSource(
    string Name,
    string RssUrl,
    NewsRegionFocus RegionFocus,
    NewsFeedLanguage Language,
    NewsCategory DefaultCategory);

/// <summary>
/// Kuratierte, kostenlose RSS-Quellen - ausschließlich seriöse Anbieter (öffentlich-rechtliche
/// Sender, Nachrichtenagenturen, Forschungseinrichtungen, etablierte Regionalzeitungen), bewusst
/// KEINE Boulevardquellen. Türkei-Nachrichten kommen von Anadolu Ajansı (staatliche
/// Nachrichtenagentur), TRT Haber, BBC Türkçe und der Deutschen Welle Türkçe statt von
/// Boulevard-Portalen.
///
/// <para>Die drei Sprachen sind Absicht: Deutsch als Schulsprache, Türkisch als Familiensprache,
/// Englisch als Fremdsprache ab Klasse 6 - eine englische Nachricht am Tag ist Lesetraining, das
/// sich nicht nach Übung anfühlt. Die Eltern wählen im Eltern-Bereich aus, welche Quellen
/// überhaupt vorkommen (siehe AppSettings.DisabledNewsFeeds).</para>
///
/// <para>Hinweis: RSS-Endpunkte ändern sich gelegentlich, und aus der Entwicklungs-Sandbox sind
/// Nachrichten-Domains nicht erreichbar - die URLs sind daher nach öffentlicher Dokumentation
/// eingetragen und per <c>scripts/check-feeds.py</c> (wöchentlicher GitHub-Actions-Lauf) geprüft,
/// nicht beim Schreiben des Codes. Das Programm überspringt nicht erreichbare Feeds automatisch
/// (siehe RssNewsService), ein einzelner toter Feed bricht nichts.</para>
/// </summary>
public static class CuratedNewsFeeds
{
    public static readonly IReadOnlyList<NewsFeedSource> All = new[]
    {
        // ---------------------------------------------------------------- Deutsch: Deutschland
        new NewsFeedSource("tagesschau.de", "https://www.tagesschau.de/xml/rss2/",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),
        new NewsFeedSource("Deutschlandfunk Nachrichten", "https://www.deutschlandfunk.de/nachrichten-100.rss",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // Kindernachrichten des ZDF - die einzige Quelle, die von sich aus für die Zielgruppe
        // schreibt; entsprechend selten muss der Vereinfacher hier eingreifen.
        new NewsFeedSource("ZDF logo! Kindernachrichten", "https://www.zdf.de/rss/zdf/kinder/logo",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // Welt: Deutsche Welle berichtet auf Deutsch über internationale Themen.
        new NewsFeedSource("Deutsche Welle", "https://rss.dw.com/xml/rss-de-all",
            NewsRegionFocus.International, NewsFeedLanguage.Deutsch, NewsCategory.Welt),

        // Berlin - die wichtigste regionale Rubrik, deshalb gleich drei Quellen.
        new NewsFeedSource("rbb24 Berlin", "https://www.rbb24.de/aktuell/index.xml",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),
        new NewsFeedSource("Tagesspiegel Berlin", "https://www.tagesspiegel.de/contentexport/feed/home",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),
        new NewsFeedSource("Berliner Morgenpost", "https://www.morgenpost.de/rss",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),

        // Berlin: Bezirks- & Parlaments-Pressemitteilungen (direkt von der Verwaltung)
        new NewsFeedSource("Berlin.de Bezirksamt Neukölln", "https://www.berlin.de/presse/pressemitteilungen/index/feed?institutions%5B%5D=Bezirksamt+Neuk%C3%B6lln",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),
        new NewsFeedSource("Berlin.de Bezirksamt Friedrichshain-Kreuzberg", "https://www.berlin.de/presse/pressemitteilungen/index/feed?institutions%5B%5D=Bezirksamt+Friedrichshain-Kreuzberg",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),
        new NewsFeedSource("Berlin.de Bezirksamt Mitte", "https://www.berlin.de/presse/pressemitteilungen/index/feed?institutions%5B%5D=Bezirksamt+Mitte",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Berlin),
        new NewsFeedSource("Abgeordnetenhaus Berlin", "https://www.parlament-berlin.de/rss/meldungen",
            NewsRegionFocus.Berlin, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // Jugendmagazin der BpB (politische Bildung, jugendgerecht)
        new NewsFeedSource("fluter.de", "https://www.fluter.de/rss.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // Wissenschaft auf Deutsch: erklärt Zusammenhänge, statt nur Ereignisse zu melden -
        // dankbares Material für Verständnisfragen.
        new NewsFeedSource("Spektrum.de", "https://www.spektrum.de/alias/rss/spektrum-de-rss-feed/996406",
            NewsRegionFocus.International, NewsFeedLanguage.Deutsch, NewsCategory.Wissen),
        new NewsFeedSource("MDR Wissen", "https://www.mdr.de/wissen/rss-feed-wissen-100.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Wissen),
        new NewsFeedSource("Max-Planck-Gesellschaft", "https://www.mpg.de/rss/pressReleases",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Wissen),
        new NewsFeedSource("Umweltbundesamt", "https://www.umweltbundesamt.de/rss/pressemitteilungen",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // Sport: die einzige Rubrik, die vorher gar nicht vorkam, obwohl sie für die Altersgruppe
        // oft der Einstieg ins Nachrichtenlesen überhaupt ist.
        new NewsFeedSource("Sportschau", "https://www.sportschau.de/index~rss2.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Sport),

        // KI/Technik-Quelle: Digital-/KI-Themen sind für Kinder/Jugendliche zunehmend
        // alltagsrelevant; der NewsCategoryClassifier sortiert die Artikel in die KI-Rubrik.
        new NewsFeedSource("heise online", "https://www.heise.de/rss/heise-atom.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),
        new NewsFeedSource("IT Boltwise", "https://www.it-boltwise.de/themen/allgemein/feed",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Ki),

        // Spiele-Rubrik: echte Gaming-Nachrichten statt nur zufälliger Spiele-Treffer aus
        // allgemeinen Quellen (siehe NewsCategoryClassifier).
        new NewsFeedSource("GameStar", "https://www.gamestar.de/rss/gaming.rss",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Spiele),

        // Offizielle Hersteller-News (DE) – stärken die Spiele-Rubrik unabhängig von GameStar
        new NewsFeedSource("Nintendo.de News", "https://www.nintendo.com/de-de/news.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Spiele),
        new NewsFeedSource("PlayStation Blog DE", "https://blog.de.playstation.com/feed",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Spiele),
        new NewsFeedSource("Xbox News DE", "https://news.xbox.com/de-de/feed/",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Spiele),
        new NewsFeedSource("Steam News", "https://store.steampowered.com/feeds/news.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Spiele),

        // Bundesregierung: offizielle, seriöse Quelle für "was macht eigentlich die Regierung" -
        // kompakt für kurze Meldungen, Pressemitteilungen für ausführlichere Themen.
        new NewsFeedSource("Bundesregierung kompakt", "https://www.bundesregierung.de/service/rss/breg-de/1151242/feed.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),
        new NewsFeedSource("Bundesregierung Pressemitteilungen", "https://www.bundesregierung.de/service/rss/breg-de/1151244/feed.xml",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // BMBFSFJ: Themen wie Bildung, Familie und Jugendschutz betreffen die Zielgruppe direkt.
        new NewsFeedSource("BMBFSFJ", "https://www.bmbfsfj.bund.de/service/rss/bmbfsfj/108854/feed.rss",
            NewsRegionFocus.Deutschland, NewsFeedLanguage.Deutsch, NewsCategory.Deutschland),

        // ------------------------------------------------------------------- Türkisch: Türkei
        new NewsFeedSource("Anadolu Ajansı", "https://www.aa.com.tr/tr/rss/default?cat=guncel",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("Anadolu Ajansı Bilim-Teknoloji", "https://www.aa.com.tr/tr/rss/default?cat=bilim-teknoloji",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("Anadolu Ajansı Spor", "https://www.aa.com.tr/tr/rss/default?cat=spor",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Sport),
        new NewsFeedSource("TRT Haber", "https://www.trthaber.com/sondakika.rss",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("TRT Haber Bilim ve Teknoloji", "https://www.trthaber.com/bilim_teknoloji.rss",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("TRT Haber Eğitim", "https://www.trthaber.com/egitim.rss",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("DW Türkçe", "https://rss.dw.com/xml/rss-tur-all",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("BBC News Türkçe", "https://feeds.bbci.co.uk/turkce/rss.xml",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),
        new NewsFeedSource("Euronews Türkçe", "https://tr.euronews.com/rss",
            NewsRegionFocus.Tuerkei, NewsFeedLanguage.Tuerkisch, NewsCategory.Tuerkei),

        // ------------------------------------------------------- Englisch: Fremdsprachentraining
        // BBC Newsround ist die englische Entsprechung zu logo!: Nachrichten, die für Kinder
        // geschrieben sind - der sanfteste Einstieg ins englische Lesen.
        new NewsFeedSource("BBC Newsround", "https://feeds.bbci.co.uk/newsround/rss.xml",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Welt),
        new NewsFeedSource("BBC News World", "https://feeds.bbci.co.uk/news/world/rss.xml",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Welt),
        new NewsFeedSource("BBC Science & Environment", "https://feeds.bbci.co.uk/news/science_and_environment/rss.xml",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Wissen),
        new NewsFeedSource("DW English", "https://rss.dw.com/xml/rss-en-all",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Welt),
        new NewsFeedSource("Euronews English", "https://www.euronews.com/rss",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Welt),
        new NewsFeedSource("NASA Breaking News", "https://www.nasa.gov/rss/dyn/breaking_news.rss",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Wissen),
        new NewsFeedSource("ESA Space News", "https://www.esa.int/rssfeed/Our_Activities/Space_News",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Wissen),
        new NewsFeedSource("ScienceDaily", "https://www.sciencedaily.com/rss/all.xml",
            NewsRegionFocus.International, NewsFeedLanguage.Englisch, NewsCategory.Wissen),
    };

    /// <summary>Schlüsselwörter zur Priorisierung von Artikeln nach den gewünschten Regionen/Themen.</summary>
    public static readonly IReadOnlyList<string> PriorityKeywords = new[]
    {
        "Berlin", "Deutschland", "Istanbul", "Samsun", "Ünye", "Unye", "Türkei", "Turkiye", "Turkei",
        "Künstliche Intelligenz", "KI", "ChatGPT", "Roboter", "Digital",
        "Nintendo", "Minecraft", "Pokémon", "Schule", "Bildung", "Wissenschaft"
    };

    // Die Stichwortlisten für den Jugendschutz stehen in NewsSuitability - dort nach Sprache
    // getrennt und auf ganze Wörter geprüft. Eine gemeinsame Liste mit Teilzeichenketten-Suche
    // war nicht haltbar: das englische "war" hätte in jedem deutschen Satz mit "war" gegriffen.
}
