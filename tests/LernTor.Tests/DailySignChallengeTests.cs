using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die taegliche Verkehrszeichen-Challenge. Der Kern ist, dass sie sich NICHT neu wuerfeln
/// laesst - sonst startet ein Kind so lange neu, bis leichte Zeichen kommen.
/// </summary>
public sealed class DailySignChallengeTests
{
    private static readonly DateOnly Heute = new(2026, 8, 6);
    private static readonly IReadOnlyList<TrafficSign> Pool = TrafficSignCatalog.All;
    private static readonly IReadOnlySet<string> NichtsGekonnt = new HashSet<string>();

    [Fact]
    public void Derselbe_Tag_liefert_immer_dieselben_Zeichen()
    {
        // Sonst waere ein Neustart der App eine Neuauslosung.
        var ersterAufruf = DailySignChallenge.ForDay(Pool, "kind-1", Heute, NichtsGekonnt);
        var zweiterAufruf = DailySignChallenge.ForDay(Pool, "kind-1", Heute, NichtsGekonnt);

        Assert.Equal(
            ersterAufruf.Select(s => s.Number),
            zweiterAufruf.Select(s => s.Number));
    }

    [Fact]
    public void Ein_anderer_Tag_liefert_andere_Zeichen()
    {
        var heute = DailySignChallenge.ForDay(Pool, "kind-1", Heute, NichtsGekonnt);
        var morgen = DailySignChallenge.ForDay(Pool, "kind-1", Heute.AddDays(1), NichtsGekonnt);

        Assert.NotEqual(
            heute.Select(s => s.Number).ToList(),
            morgen.Select(s => s.Number).ToList());
    }

    [Fact]
    public void Geschwister_bekommen_am_selben_Tag_verschiedene_Zeichen()
    {
        // Sonst waere Abschreiben der schnellste Weg durch die Challenge.
        var emirhan = DailySignChallenge.ForDay(Pool, "profil-emirhan", Heute, NichtsGekonnt);
        var batuhan = DailySignChallenge.ForDay(Pool, "profil-batuhan", Heute, NichtsGekonnt);

        Assert.NotEqual(
            emirhan.Select(s => s.Number).ToList(),
            batuhan.Select(s => s.Number).ToList());
    }

    [Fact]
    public void Es_kommen_genau_fuenf_Zeichen()
    {
        var zeichen = DailySignChallenge.ForDay(Pool, "kind-1", Heute, NichtsGekonnt);

        Assert.Equal(DailySignChallenge.SignsPerDay, zeichen.Count);
        Assert.Equal(zeichen.Count, zeichen.Select(s => s.Number).Distinct().Count());
    }

    [Fact]
    public void Noch_nicht_Gekonntes_kommt_zuerst()
    {
        // Alles gekonnt ausser drei Zeichen -> genau diese drei muessen dabei sein.
        var offen = new[] { "101", "205", "274-50" };
        var gekonnt = Pool.Select(s => s.Number).Where(n => !offen.Contains(n)).ToHashSet();

        var zeichen = DailySignChallenge.ForDay(Pool, "kind-1", Heute, gekonnt);

        foreach (var nummer in offen)
        {
            Assert.Contains(zeichen, s => s.Number == nummer);
        }
    }

    [Fact]
    public void Ist_alles_gekonnt_wird_trotzdem_wiederholt()
    {
        // Ein leerer Bildschirm waere die schlechteste Belohnung fuers Durchlernen.
        var alles = Pool.Select(s => s.Number).ToHashSet();

        var zeichen = DailySignChallenge.ForDay(Pool, "kind-1", Heute, alles);

        Assert.Equal(DailySignChallenge.SignsPerDay, zeichen.Count);
    }

    [Fact]
    public void Ein_kleiner_Pool_wird_nicht_mit_Wiederholungen_aufgefuellt()
    {
        // Lieber drei echte Zeichen als zweimal dasselbe - Eltern koennen Gruppen abwaehlen.
        var klein = Pool.Take(3).ToList();

        var zeichen = DailySignChallenge.ForDay(klein, "kind-1", Heute, NichtsGekonnt);

        Assert.Equal(3, zeichen.Count);
        Assert.Equal(3, zeichen.Select(s => s.Number).Distinct().Count());
    }

    [Fact]
    public void Ein_leerer_Pool_liefert_nichts_statt_zu_werfen()
    {
        Assert.Empty(DailySignChallenge.ForDay(Array.Empty<TrafficSign>(), "kind-1", Heute, NichtsGekonnt));
    }
}
