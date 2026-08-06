using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Wann eine Theoriefrage als gekonnt gilt.</summary>
public sealed class TheoryProgressTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(9, true)]
    public void Erst_zweimal_richtig_in_Folge_gilt_als_gekonnt(int streak, bool erwartet)
    {
        Assert.Equal(erwartet, TheoryProgress.IsMastered(streak));
    }

    [Fact]
    public void Ein_Fehler_setzt_die_Serie_auf_null()
    {
        // Nicht "eins weniger": eine gerade verhauene Frage sitzt nicht mehr fast.
        Assert.Equal(0, TheoryProgress.NextStreak(3, wasCorrect: false));
        Assert.Equal(4, TheoryProgress.NextStreak(3, wasCorrect: true));
    }

    [Fact]
    public void Dieselbe_Schwelle_wie_bei_den_Verkehrszeichen()
    {
        // Zwei Bereiche mit zwei verschiedenen Vorstellungen von "sitzt" wären für die Kinder
        // nicht nachvollziehbar - und für die Anzeige "x von y sitzen" schlicht falsch.
        Assert.Equal(TrafficSignProgress.MasteredStreak, TheoryProgress.MasteredStreak);
    }
}
