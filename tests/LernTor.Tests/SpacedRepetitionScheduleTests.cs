using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public sealed class SpacedRepetitionScheduleTests
{
    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 30)]
    [InlineData(3, 90)]
    public void Stufen_haben_wachsende_Intervalle(int stage, int expectedDays)
    {
        Assert.Equal(TimeSpan.FromDays(expectedDays), SpacedRepetitionSchedule.IntervalForStage(stage));
    }

    [Fact]
    public void Stufen_oberhalb_der_hoechsten_bleiben_beim_laengsten_Intervall()
    {
        Assert.Equal(TimeSpan.FromDays(90), SpacedRepetitionSchedule.IntervalForStage(7));
    }

    [Fact]
    public void Ungueltige_Stufe_unterhalb_von_1_faellt_auf_das_kuerzeste_Intervall_zurueck()
    {
        Assert.Equal(TimeSpan.FromDays(7), SpacedRepetitionSchedule.IntervalForStage(0));
    }

    [Fact]
    public void NextDueAt_addiert_das_Stufen_Intervall()
    {
        var now = new DateTimeOffset(2026, 7, 17, 12, 0, 0, TimeSpan.Zero);

        Assert.Equal(now.AddDays(30), SpacedRepetitionSchedule.NextDueAt(2, now));
    }
}
