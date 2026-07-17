using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

public class WeatherServiceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(45)]
    [InlineData(61)]
    [InlineData(75)]
    [InlineData(82)]
    [InlineData(95)]
    [InlineData(1234)] // Unbekannter Code → neutraler Rückfall statt Crash
    public void DescribeWeatherCode_liefert_immer_Emoji_und_beide_Sprachen(int code)
    {
        var (emoji, de, tr) = WeatherService.DescribeWeatherCode(code);

        Assert.False(string.IsNullOrWhiteSpace(emoji));
        Assert.False(string.IsNullOrWhiteSpace(de));
        Assert.False(string.IsNullOrWhiteSpace(tr));
    }

    [Fact]
    public void BuildKidTip_warnt_bei_hoher_Regenwahrscheinlichkeit()
    {
        var (de, _) = WeatherService.BuildKidTip(todayMax: 20, precipitationProbability: 80);
        Assert.Contains("Regen", de);
    }

    [Fact]
    public void BuildKidTip_erinnert_bei_Hitze_ans_Trinken()
    {
        var (de, _) = WeatherService.BuildKidTip(todayMax: 32, precipitationProbability: 10);
        Assert.Contains("trinken", de, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildKidTip_raet_bei_Kaelte_zu_warmer_Kleidung()
    {
        var (de, _) = WeatherService.BuildKidTip(todayMax: 2, precipitationProbability: 10);
        Assert.Contains("warm", de, StringComparison.OrdinalIgnoreCase);
    }
}
