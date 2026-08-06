using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Wann ein Verkehrszeichen als gekonnt gilt.</summary>
public sealed class TrafficSignProgressTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(5, true)]
    public void Erst_zweimal_richtig_in_Folge_gilt_als_gekonnt(int streak, bool erwartet)
    {
        // Bei vier Antwortmoeglichkeiten trifft jeder vierte Rateversuch - zweimal
        // hintereinander zu raten gelingt nur in einem von sechzehn Faellen.
        Assert.Equal(erwartet, TrafficSignProgress.IsMastered(streak));
    }

    [Fact]
    public void Ein_Fehler_setzt_die_Serie_auf_null()
    {
        // Nicht "eins weniger": ein gerade verwechseltes Zeichen sitzt nicht mehr fast.
        Assert.Equal(0, TrafficSignProgress.NextStreak(4, wasCorrect: false));
        Assert.Equal(5, TrafficSignProgress.NextStreak(4, wasCorrect: true));
    }

    [Fact]
    public void Der_Lernstand_wird_je_Gruppe_gezaehlt()
    {
        var pool = TrafficSignCatalog.All;
        var gefahr = TrafficSignCatalog.ByCategory(TrafficSignCategory.Gefahrzeichen);
        var gekonnt = gefahr.Take(3).Select(s => s.Number).ToHashSet();

        var fortschritt = TrafficSignProgress.ByCategory(pool, gekonnt);
        var gefahrZeile = fortschritt.Single(z => z.Category == TrafficSignCategory.Gefahrzeichen);

        Assert.Equal(3, gefahrZeile.Mastered);
        Assert.Equal(gefahr.Count, gefahrZeile.Total);
        Assert.False(gefahrZeile.IsComplete);
    }

    [Fact]
    public void Eine_vollstaendig_gekonnte_Gruppe_ist_als_solche_erkennbar()
    {
        var gefahr = TrafficSignCatalog.ByCategory(TrafficSignCategory.Gefahrzeichen);
        var gekonnt = gefahr.Select(s => s.Number).ToHashSet();

        var zeile = TrafficSignProgress
            .ByCategory(TrafficSignCatalog.All, gekonnt)
            .Single(z => z.Category == TrafficSignCategory.Gefahrzeichen);

        Assert.True(zeile.IsComplete);
        Assert.Equal(100, zeile.Percent);
    }

    [Fact]
    public void Empfohlen_wird_die_erste_noch_offene_Gruppe()
    {
        // Am Anfang sind das die Gefahrzeichen - sie warnen nur und sind am leichtesten.
        var fortschritt = TrafficSignProgress.ByCategory(TrafficSignCatalog.All, new HashSet<string>());

        Assert.Equal(TrafficSignCategory.Gefahrzeichen, TrafficSignProgress.NextRecommended(fortschritt));
    }

    [Fact]
    public void Ist_eine_Gruppe_fertig_rueckt_die_naechste_nach()
    {
        var gekonnt = TrafficSignCatalog
            .ByCategory(TrafficSignCategory.Gefahrzeichen)
            .Select(s => s.Number)
            .ToHashSet();

        var fortschritt = TrafficSignProgress.ByCategory(TrafficSignCatalog.All, gekonnt);

        Assert.Equal(TrafficSignCategory.Vorschriftzeichen, TrafficSignProgress.NextRecommended(fortschritt));
    }

    [Fact]
    public void Ist_alles_gekonnt_gibt_es_keine_Empfehlung_mehr()
    {
        var alles = TrafficSignCatalog.All.Select(s => s.Number).ToHashSet();

        var fortschritt = TrafficSignProgress.ByCategory(TrafficSignCatalog.All, alles);

        Assert.Null(TrafficSignProgress.NextRecommended(fortschritt));
    }

    [Fact]
    public void Abgewaehlte_Gruppen_tauchen_im_Lernstand_nicht_auf()
    {
        // Eltern koennen Gruppen pro Kind ausblenden - dann darf der Balken sie nicht mitzaehlen.
        var nurGefahr = TrafficSignCatalog.ByCategory(TrafficSignCategory.Gefahrzeichen);

        var fortschritt = TrafficSignProgress.ByCategory(nurGefahr, new HashSet<string>());

        Assert.Single(fortschritt);
        Assert.Equal(TrafficSignCategory.Gefahrzeichen, fortschritt[0].Category);
    }

    [Fact]
    public void Eine_leere_Gruppe_ergibt_keinen_Bruch_durch_null()
    {
        var leer = new CategoryProgress(TrafficSignCategory.Zusatzzeichen, 0, 0);

        Assert.Equal(0, leer.Fraction);
        Assert.Equal(0, leer.Percent);
        Assert.False(leer.IsComplete);
    }
}
