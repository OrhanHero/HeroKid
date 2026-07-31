using LernTor.News;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Jugendschutz-Prüfung der Nachrichten. Vorher griff überhaupt nur bei Kindern bis neun Jahre
/// ein Filter - für die eigentliche Zielgruppe (10-15) fand keine Prüfung statt.
/// </summary>
public sealed class NewsSuitabilityTests
{
    private static NewsSuitability.Verdict Pruefe(
        string title, NewsFeedLanguage language = NewsFeedLanguage.Deutsch, int? childAge = 12) =>
        NewsSuitability.Evaluate(title, summary: null, language, childAge);

    [Theory]
    [InlineData("Prozess wegen Vergewaltigung beginnt in Berlin")]
    [InlineData("Ermittlungen wegen Kindesmissbrauch")]
    [InlineData("Nach Suizid eines Schülers: Schule sucht Antworten")]
    [InlineData("Bericht über Folter in Gefängnissen")]
    public void Klar_ungeeignete_Themen_sind_hart_gesperrt(string title)
    {
        Assert.True(Pruefe(title).IsBlocked);
    }

    [Theory]
    [InlineData("rape trial opens in London", NewsFeedLanguage.Englisch)]
    [InlineData("Report on child abuse published", NewsFeedLanguage.Englisch)]
    [InlineData("İstanbul'da tecavüz davası başladı", NewsFeedLanguage.Tuerkisch)]
    [InlineData("Cezaevinde işkence iddiası", NewsFeedLanguage.Tuerkisch)]
    public void Sperre_gilt_in_allen_drei_Sprachen(string title, NewsFeedLanguage language)
    {
        Assert.True(Pruefe(title, language).IsBlocked);
    }

    [Fact]
    public void Heikle_Themen_sind_erlaubt_aber_nachrangig()
    {
        // Krieg gehoert zum Rahmenlehrplan - der Artikel darf vorkommen, soll aber nur genommen
        // werden, wenn die Quelle nichts Harmloseres hergibt.
        var verdict = Pruefe("Krieg in der Ukraine: Verhandlungen gehen weiter");

        Assert.False(verdict.IsBlocked);
        Assert.True(verdict.SensitiveHits > 0);
    }

    [Fact]
    public void Harmlose_Nachrichten_sind_unbedenklich()
    {
        var verdict = Pruefe("Neuer Spielplatz in Neukölln eröffnet");

        Assert.False(verdict.IsBlocked);
        Assert.Equal(0, verdict.SensitiveHits);
    }

    [Fact]
    public void Fuer_juengere_Kinder_sind_auch_heikle_Themen_gesperrt()
    {
        Assert.True(Pruefe("Krieg in der Ukraine", childAge: 8).IsBlocked);
        Assert.False(Pruefe("Krieg in der Ukraine", childAge: 12).IsBlocked);
    }

    [Fact]
    public void Ohne_Altersangabe_bleibt_der_News_Teil_nutzbar()
    {
        // Sonst waere der News-Bereich fuer Profile ohne Alter fast leer.
        Assert.False(Pruefe("Krieg in der Ukraine", childAge: null).IsBlocked);
    }

    [Theory]
    [InlineData("Das Wetter war gestern besser als heute")]
    [InlineData("Die Antwort war überraschend")]
    [InlineData("Warschau feiert Stadtfest")]
    [InlineData("Neue Ware im Schulkiosk")]
    public void Deutsches_war_ist_kein_englischer_Krieg(string title)
    {
        // Genau hier waere eine gemeinsame Stichwortliste mit Teilzeichenketten-Suche gescheitert:
        // "war" haette praktisch jeden deutschen Artikel als heikel eingestuft.
        var verdict = Pruefe(title, NewsFeedLanguage.Deutsch);

        Assert.False(verdict.IsBlocked);
        Assert.Equal(0, verdict.SensitiveHits);
    }

    [Theory]
    [InlineData("Total renovierte Turnhalle eröffnet")]
    [InlineData("Nordwind bringt kaltes Wetter")]
    [InlineData("Der Bericht ist informativ")]
    public void Teilwoerter_loesen_keinen_Treffer_aus(string title)
    {
        Assert.Equal(0, Pruefe(title).SensitiveHits);
    }

    [Fact]
    public void Englische_Stichwoerter_greifen_nur_bei_englischen_Quellen()
    {
        var englisch = Pruefe("Ukraine war continues", NewsFeedLanguage.Englisch);
        var deutsch = Pruefe("Ukraine war continues", NewsFeedLanguage.Deutsch);

        Assert.True(englisch.SensitiveHits > 0);
        Assert.Equal(0, deutsch.SensitiveHits);
    }

    [Fact]
    public void Gesperrte_Artikel_sind_immer_nachrangig_einsortiert()
    {
        // SensitiveHits = int.MaxValue sorgt dafuer, dass ein gesperrter Artikel auch dann nicht
        // nach vorne rutscht, falls die Sperre irgendwo versehentlich ignoriert wird.
        Assert.Equal(int.MaxValue, Pruefe("Vergewaltigung vor Gericht").SensitiveHits);
    }

    [Fact]
    public void Leerer_Text_ist_unbedenklich()
    {
        var verdict = NewsSuitability.Evaluate(null, null, NewsFeedLanguage.Deutsch, 12);

        Assert.False(verdict.IsBlocked);
        Assert.Equal(0, verdict.SensitiveHits);
    }
}
