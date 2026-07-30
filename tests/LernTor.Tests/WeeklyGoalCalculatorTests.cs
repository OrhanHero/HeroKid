using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class WeeklyGoalCalculatorTests
{
    // 2026-05-13 ist ein Mittwoch; die Woche läuft von Montag 11. bis Sonntag 17. Mai.
    private static readonly DateOnly Mittwoch = new(2026, 5, 13);

    [Theory]
    [InlineData(2026, 5, 11)] // Montag
    [InlineData(2026, 5, 13)] // Mittwoch
    [InlineData(2026, 5, 17)] // Sonntag
    public void Woche_beginnt_am_Montag(int year, int month, int day)
    {
        // Mit dem Sonntag als Wochenstart wäre "diese Woche" für ein Schulkind eine andere
        // Woche als für seinen Stundenplan.
        Assert.Equal(new DateOnly(2026, 5, 11), WeeklyGoalCalculator.StartOfWeek(new DateOnly(year, month, day)));
    }

    [Fact]
    public void Ohne_Ziel_ist_die_Anzeige_aus()
    {
        var status = WeeklyGoalCalculator.Evaluate(new[] { Mittwoch }, Mittwoch, goal: 0);

        Assert.False(status.IsActive);
        Assert.False(status.IsReached);
    }

    [Fact]
    public void Nur_Tage_der_laufenden_Woche_zaehlen()
    {
        var tage = new[]
        {
            new DateOnly(2026, 5, 10), // Sonntag davor - zählt nicht mehr
            new DateOnly(2026, 5, 11),
            new DateOnly(2026, 5, 12),
            new DateOnly(2026, 5, 18)  // Montag danach - zählt noch nicht
        };

        var status = WeeklyGoalCalculator.Evaluate(tage, Mittwoch, goal: 4);

        Assert.Equal(2, status.LearnedDays);
        Assert.Equal(2, status.Missing);
    }

    [Fact]
    public void Ziel_erreicht_wird_erkannt()
    {
        var tage = new[]
        {
            new DateOnly(2026, 5, 11), new DateOnly(2026, 5, 12), new DateOnly(2026, 5, 13)
        };

        var status = WeeklyGoalCalculator.Evaluate(tage, Mittwoch, goal: 3);

        Assert.True(status.IsReached);
        Assert.Equal(0, status.Missing);
        Assert.Equal(1.0, status.Progress);
    }

    [Fact]
    public void Uebererfuellung_bleibt_bei_hundert_Prozent()
    {
        var tage = Enumerable.Range(11, 5).Select(d => new DateOnly(2026, 5, d)).ToArray();

        var status = WeeklyGoalCalculator.Evaluate(tage, new DateOnly(2026, 5, 15), goal: 3);

        Assert.True(status.IsReached);
        Assert.Equal(1.0, status.Progress);
    }

    [Fact]
    public void Ein_verpasster_Tag_bricht_das_Ziel_nicht()
    {
        // Der entscheidende Unterschied zur Lernserie: wer Montag und Dienstag nicht kann,
        // schafft die vier Tage immer noch.
        var tage = new[] { new DateOnly(2026, 5, 13) };

        var status = WeeklyGoalCalculator.Evaluate(tage, Mittwoch, goal: 4);

        Assert.False(status.IsReached);
        // Mittwoch bis Sonntag sind noch fünf Tage, davon werden drei gebraucht.
        Assert.Equal(5, status.DaysLeft);
        Assert.True(status.IsStillPossible);
    }

    [Fact]
    public void Rechnerisch_unmoegliche_Ziele_werden_als_solche_erkannt()
    {
        // Am Samstag mit 0 Lerntagen sind 7 Tage nicht mehr zu schaffen - dann wird bewusst
        // nichts Mahnendes angezeigt, die Woche ist einfach so gelaufen.
        var status = WeeklyGoalCalculator.Evaluate(Array.Empty<DateOnly>(), new DateOnly(2026, 5, 16), goal: 7);

        Assert.True(status.IsActive);
        Assert.Equal(2, status.DaysLeft);
        Assert.False(status.IsStillPossible);
    }

    [Fact]
    public void Am_Sonntag_bleibt_der_heutige_Tag_erreichbar()
    {
        // Wer sonntags noch lernt, erfüllt den Sonntag - DaysLeft muss ihn also mitzählen.
        var status = WeeklyGoalCalculator.Evaluate(
            new[] { new DateOnly(2026, 5, 11), new DateOnly(2026, 5, 12) },
            new DateOnly(2026, 5, 17),
            goal: 3);

        Assert.Equal(1, status.DaysLeft);
        Assert.True(status.IsStillPossible);
    }

    [Fact]
    public void Aus_ist_ein_zulaessiges_Ziel()
    {
        Assert.Contains(0, WeeklyGoalCalculator.AllowedGoals);
        Assert.All(WeeklyGoalCalculator.AllowedGoals, goal => Assert.InRange(goal, 0, 7));
    }
}
