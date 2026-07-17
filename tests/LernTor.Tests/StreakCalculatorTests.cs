using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public sealed class StreakCalculatorTests
{
    private static readonly DateOnly Today = new(2026, 7, 17);

    private static DateOnly Day(int daysAgo) => Today.AddDays(-daysAgo);

    [Fact]
    public void No_learning_days_means_zero_streak()
    {
        Assert.Equal(0, StreakCalculator.CurrentStreak(Array.Empty<DateOnly>(), Today));
    }

    [Fact]
    public void Consecutive_days_ending_today_are_counted_including_today()
    {
        var days = new[] { Day(0), Day(1), Day(2) };

        Assert.Equal(3, StreakCalculator.CurrentStreak(days, Today));
    }

    [Fact]
    public void Missing_today_does_not_break_the_streak_yet()
    {
        // Die heutige Session läuft evtl. gerade erst an - gezählt wird dann ab gestern.
        var days = new[] { Day(1), Day(2), Day(3) };

        Assert.Equal(3, StreakCalculator.CurrentStreak(days, Today));
    }

    [Fact]
    public void A_full_missed_day_resets_the_streak()
    {
        // Vorgestern + älter gelernt, gestern UND heute nicht -> Serie gerissen.
        var days = new[] { Day(2), Day(3), Day(4) };

        Assert.Equal(0, StreakCalculator.CurrentStreak(days, Today));
    }

    [Fact]
    public void Gap_in_history_only_counts_the_recent_run()
    {
        var days = new[] { Day(0), Day(1), Day(3), Day(4), Day(5) };

        Assert.Equal(2, StreakCalculator.CurrentStreak(days, Today));
    }

    [Fact]
    public void Duplicate_days_do_not_inflate_the_streak()
    {
        var days = new[] { Day(0), Day(0), Day(1), Day(1) };

        Assert.Equal(2, StreakCalculator.CurrentStreak(days, Today));
    }
}
