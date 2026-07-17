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

public class FinanceKnowledgeArticlesTests
{
    [Fact]
    public void GetForDate_ist_deterministisch_und_rotiert()
    {
        var day1 = new DateOnly(2026, 3, 1);
        var day2 = new DateOnly(2026, 3, 2);

        Assert.Equal(FinanceKnowledgeArticles.GetForDate(day1).Id, FinanceKnowledgeArticles.GetForDate(day1).Id);
        Assert.NotEqual(FinanceKnowledgeArticles.GetForDate(day1).Id, FinanceKnowledgeArticles.GetForDate(day2).Id);
    }

    [Fact]
    public void Jedes_Erklaerstueck_ist_vollstaendig_und_beantwortbar()
    {
        foreach (var piece in FinanceKnowledgeArticles.Pool)
        {
            Assert.StartsWith("lerntor-finanzwissen-", piece.Id);
            Assert.Equal(NewsCategory.Finanzen, piece.Category);
            Assert.False(string.IsNullOrWhiteSpace(piece.Title));
            Assert.True(piece.SimplifiedSummary.Length > 100, $"Text zu kurz: {piece.Id}");
            Assert.NotEmpty(piece.ComprehensionQuestions);

            foreach (var question in piece.ComprehensionQuestions)
            {
                // Die richtige Antwort muss wörtlich unter den Optionen sein und als korrekt gewertet werden.
                Assert.Contains(question.CorrectAnswers[0], question.Options);
                Assert.True(question.CheckAnswer(question.CorrectAnswers[0]));
                Assert.False(string.IsNullOrWhiteSpace(question.Explanation));
            }
        }
    }

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse9)]
    public void GetForDate_ergaenzt_Einordnung_und_Metadaten(GradeLevel gradeLevel)
    {
        var piece = FinanceKnowledgeArticles.GetForDate(new DateOnly(2026, 5, 10), gradeLevel);

        Assert.False(string.IsNullOrWhiteSpace(piece.WhyImportant));
        Assert.False(string.IsNullOrWhiteSpace(piece.MeaningForKids));
        Assert.True(piece.ReadingMinutes >= 1);
    }
}
