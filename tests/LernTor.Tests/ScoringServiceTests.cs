using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class ScoringServiceTests
{
    private readonly ScoringService _scoring = new();

    private static QuizQuestion MakeQuestion(string id, params string[] correctAnswers) => new()
    {
        Id = id,
        Subject = Subject.Mathematik,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Test",
        Prompt = "1+1?",
        Type = QuestionType.OpenText,
        CorrectAnswers = correctAnswers,
        Explanation = "1+1=2"
    };

    [Fact]
    public void Evaluate_CorrectAnswer_MarksOutcomeCorrect()
    {
        var question = MakeQuestion("q1", "2");
        var outcome = _scoring.Evaluate(question, "2");

        Assert.True(outcome.WasCorrect);
        Assert.Equal(Subject.Mathematik, outcome.Subject);
    }

    [Fact]
    public void Evaluate_WrongAnswer_MarksOutcomeIncorrect()
    {
        var question = MakeQuestion("q1", "2");
        var outcome = _scoring.Evaluate(question, "3");

        Assert.False(outcome.WasCorrect);
    }

    [Theory]
    [InlineData(5, 10, 0.5, true)]
    [InlineData(4, 10, 0.4, false)]
    [InlineData(10, 10, 1.0, true)]
    [InlineData(0, 10, 0.0, false)]
    public void BuildResult_ComputesScoreAndPassThreshold(int correct, int total, double expectedScore, bool expectedPassed)
    {
        var outcomes = Enumerable.Range(0, total)
            .Select(i => new QuestionOutcome
            {
                QuestionId = $"q{i}",
                Subject = Subject.Mathematik,
                GivenAnswer = "x",
                WasCorrect = i < correct
            });

        var result = _scoring.BuildResult(outcomes);

        Assert.Equal(expectedScore, result.ScorePercentage, precision: 5);
        Assert.Equal(expectedPassed, result.Passed);
    }
}
