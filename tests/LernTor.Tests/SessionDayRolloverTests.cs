using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Wann eine Sitzung zu einem neuen Tag gehört: über Mitternacht bleibt sie beim Starttag, über
/// Nacht stehen gelassen beginnt morgens ein neuer.
/// </summary>
public sealed class SessionDayRolloverTests
{
    private static readonly DateOnly Montag = new(2026, 9, 28);

    [Fact]
    public void Am_selben_Tag_laeuft_die_Sitzung_weiter()
    {
        Assert.False(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 28, 23, 59, 0)));
    }

    [Fact]
    public void Kurz_nach_Mitternacht_gehoert_die_Sitzung_noch_zum_Vortag()
    {
        // Um 23:58 angefangen: bis zum Schluss derselbe Tag - sonst finge die Sitzung
        // um Mitternacht still von vorn an.
        Assert.False(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 29, 0, 5, 0)));
        Assert.False(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 29, 3, 59, 0)));
    }

    [Fact]
    public void Ueber_Nacht_stehen_gelassen_beginnt_morgens_ein_neuer_Tag()
    {
        Assert.True(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 29, 4, 0, 0)));
        Assert.True(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 29, 7, 30, 0)));
    }

    [Fact]
    public void Nach_mehr_als_einem_Tag_immer_ein_neuer_Tag()
    {
        Assert.True(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 30, 0, 30, 0)));
    }

    [Fact]
    public void Eine_zurueckgestellte_Uhr_beginnt_keinen_neuen_Tag()
    {
        Assert.False(SessionDayRollover.ShouldStartNewDay(Montag, new DateTime(2026, 9, 27, 12, 0, 0)));
    }
}
