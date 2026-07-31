using LernTor.Core.Models;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Tagesauswahl der Nachrichtenquellen. Seit der Katalog 44 Quellen umfasst, wird nicht mehr jede
/// aktive Quelle täglich abgerufen - sonst wären das 44 HTTP-Abrufe beim Start und ein News-Teil,
/// der länger dauert als alle Fächer zusammen.
/// </summary>
public sealed class NewsFeedSelectionTests
{
    private static NewsFeedSource Feed(string name, NewsFeedLanguage language) =>
        new(name, $"https://example.invalid/{name}", NewsRegionFocus.Deutschland, language, NewsCategory.Welt);

    private static List<NewsFeedSource> Katalog(int deutsch, int tuerkisch, int englisch)
    {
        var feeds = new List<NewsFeedSource>();
        for (var i = 0; i < deutsch; i++) feeds.Add(Feed($"de{i:00}", NewsFeedLanguage.Deutsch));
        for (var i = 0; i < tuerkisch; i++) feeds.Add(Feed($"tr{i:00}", NewsFeedLanguage.Tuerkisch));
        for (var i = 0; i < englisch; i++) feeds.Add(Feed($"en{i:00}", NewsFeedLanguage.Englisch));
        return feeds;
    }

    [Fact]
    public void Weniger_Quellen_als_gewuenscht_liefert_alle()
    {
        var feeds = Katalog(3, 2, 1);

        var selected = RssNewsService.SelectFeedsForDay(feeds, targetCount: 12, new DateOnly(2026, 8, 3));

        Assert.Equal(feeds.Count, selected.Count);
    }

    [Fact]
    public void Genau_so_viele_Quellen_wie_gewuenscht_werden_ausgewaehlt()
    {
        var selected = RssNewsService.SelectFeedsForDay(Katalog(27, 9, 8), 12, new DateOnly(2026, 8, 3));

        Assert.Equal(12, selected.Count);
    }

    [Fact]
    public void Jede_Sprache_kommt_jeden_Tag_vor()
    {
        // Der halbe Sinn der Sammlung ist die Sprachmischung - ein Tag mit ausschliesslich
        // deutschen Quellen (27 von 44) waere das Gegenteil davon.
        var feeds = Katalog(27, 9, 8);

        for (var offset = 0; offset < 30; offset++)
        {
            var day = new DateOnly(2026, 8, 3).AddDays(offset);
            var selected = RssNewsService.SelectFeedsForDay(feeds, 12, day);

            Assert.Contains(selected, f => f.Language == NewsFeedLanguage.Deutsch);
            Assert.Contains(selected, f => f.Language == NewsFeedLanguage.Tuerkisch);
            Assert.Contains(selected, f => f.Language == NewsFeedLanguage.Englisch);
        }
    }

    [Fact]
    public void Auswahl_ist_innerhalb_eines_Tages_stabil()
    {
        // Beim zweiten Oeffnen der App am selben Tag duerfen nicht ploetzlich andere Nachrichten
        // dastehen - der Fortschritt haengt an den Artikel-IDs.
        var feeds = Katalog(27, 9, 8);
        var day = new DateOnly(2026, 8, 3);

        var first = RssNewsService.SelectFeedsForDay(feeds, 12, day).Select(f => f.Name);
        var second = RssNewsService.SelectFeedsForDay(feeds, 12, day).Select(f => f.Name);

        Assert.Equal(first, second);
    }

    [Fact]
    public void Keine_Quelle_kommt_am_selben_Tag_doppelt_vor()
    {
        var feeds = Katalog(27, 9, 8);

        for (var offset = 0; offset < 40; offset++)
        {
            var selected = RssNewsService.SelectFeedsForDay(feeds, 12, new DateOnly(2026, 8, 3).AddDays(offset));
            Assert.Equal(selected.Count, selected.Select(f => f.Name).Distinct().Count());
        }
    }

    [Fact]
    public void Ueber_mehrere_Tage_kommt_jede_Quelle_vorbei()
    {
        // Sonst waeren die hinteren Quellen im Katalog reine Dekoration.
        var feeds = Katalog(27, 9, 8);
        var seen = new HashSet<string>();

        for (var offset = 0; offset < 21; offset++)
        {
            foreach (var feed in RssNewsService.SelectFeedsForDay(feeds, 12, new DateOnly(2026, 8, 3).AddDays(offset)))
            {
                seen.Add(feed.Name);
            }
        }

        Assert.Equal(feeds.Count, seen.Count);
    }

    [Fact]
    public void Aufeinanderfolgende_Tage_zeigen_ueberwiegend_andere_Quellen()
    {
        var feeds = Katalog(27, 9, 8);
        var day = new DateOnly(2026, 8, 3);

        var heute = RssNewsService.SelectFeedsForDay(feeds, 12, day).Select(f => f.Name).ToHashSet();
        var morgen = RssNewsService.SelectFeedsForDay(feeds, 12, day.AddDays(1)).Select(f => f.Name).ToHashSet();

        heute.IntersectWith(morgen);
        Assert.True(heute.Count <= 4, $"Zu viel Überlappung zwischen zwei Tagen: {heute.Count} von 12.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(20)]
    public void Auswahl_bleibt_fuer_jede_Einstellung_gueltig(int targetCount)
    {
        var feeds = Katalog(27, 9, 8);

        var selected = RssNewsService.SelectFeedsForDay(feeds, targetCount, new DateOnly(2026, 8, 3));

        Assert.Equal(targetCount, selected.Count);
        Assert.Equal(selected.Count, selected.Select(f => f.Name).Distinct().Count());
    }

    [Fact]
    public void Nur_eine_Sprache_im_Katalog_funktioniert_auch()
    {
        // Eltern duerfen alle tuerkischen und englischen Quellen abwaehlen.
        var selected = RssNewsService.SelectFeedsForDay(Katalog(20, 0, 0), 5, new DateOnly(2026, 8, 3));

        Assert.Equal(5, selected.Count);
        Assert.All(selected, f => Assert.Equal(NewsFeedLanguage.Deutsch, f.Language));
    }

    [Fact]
    public void Der_echte_Katalog_ist_gross_und_dreisprachig()
    {
        Assert.True(CuratedNewsFeeds.All.Count >= 44, $"Nur {CuratedNewsFeeds.All.Count} Quellen im Katalog.");

        foreach (var language in new[] { NewsFeedLanguage.Deutsch, NewsFeedLanguage.Tuerkisch, NewsFeedLanguage.Englisch })
        {
            Assert.Contains(CuratedNewsFeeds.All, f => f.Language == language);
        }
    }

    [Fact]
    public void Quellennamen_und_URLs_sind_eindeutig()
    {
        // Der Name ist der Schluessel in AppSettings.DisabledNewsFeeds - ein doppelter Name
        // wuerde zwei Quellen gemeinsam an- und abschalten.
        Assert.Equal(CuratedNewsFeeds.All.Count, CuratedNewsFeeds.All.Select(f => f.Name).Distinct().Count());
        Assert.Equal(CuratedNewsFeeds.All.Count, CuratedNewsFeeds.All.Select(f => f.RssUrl).Distinct().Count());
    }
}
