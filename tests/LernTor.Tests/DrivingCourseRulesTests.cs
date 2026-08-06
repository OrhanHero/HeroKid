using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Wann eine Lernstandskontrolle bestanden ist.</summary>
public sealed class DrivingCourseRulesTests
{
    [Theory]
    [InlineData(0, 5, 0)]
    [InlineData(3, 5, 60)]
    [InlineData(4, 5, 80)]
    [InlineData(5, 5, 100)]
    [InlineData(2, 3, 67)]
    [InlineData(0, 0, 0)]      // ohne Fragen keine Aussage, aber auch keine Division durch null
    public void Die_Trefferquote_wird_gerundet(int richtig, int gesamt, int erwartet)
    {
        Assert.Equal(erwartet, DrivingCourseRules.Percent(richtig, gesamt));
    }

    [Theory]
    [InlineData(3, 5, false)]   // 60 %
    [InlineData(4, 5, true)]    // 80 %
    [InlineData(7, 10, true)]   // genau 70 % zählt noch als bestanden
    [InlineData(2, 3, false)]   // 67 %
    [InlineData(0, 0, false)]
    public void Ab_siebzig_Prozent_ist_bestanden(int richtig, int gesamt, bool erwartet)
    {
        Assert.Equal(erwartet, DrivingCourseRules.HasPassed(richtig, gesamt));
    }

    [Fact]
    public void Urteil_und_angezeigte_Zahl_koennen_nicht_auseinanderfallen()
    {
        // HasPassed rechnet über Percent - sonst könnte auf dem Bildschirm "70 %" stehen,
        // während die Lektion als nicht bestanden gilt.
        for (var gesamt = 1; gesamt <= DrivingCourseRules.MaxCheckQuestions; gesamt++)
        {
            for (var richtig = 0; richtig <= gesamt; richtig++)
            {
                var angezeigt = DrivingCourseRules.Percent(richtig, gesamt);

                Assert.Equal(angezeigt >= DrivingCourseRules.PassPercent,
                    DrivingCourseRules.HasPassed(richtig, gesamt));
            }
        }
    }
}
