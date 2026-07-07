using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

public class MathGeneratorTests
{
    private readonly MathGenerator _generator = new();

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse9)]
    public void Generate_ReturnsRequestedCount(GradeLevel grade)
    {
        var questions = _generator.Generate(grade, 20, new Random(42));

        Assert.Equal(20, questions.Count);
        Assert.All(questions, q => Assert.False(string.IsNullOrWhiteSpace(q.Explanation)));
        Assert.All(questions, q => Assert.NotEmpty(q.CorrectAnswers));
    }

    [Fact]
    public void GeneratedAnswers_AreVerifiableAsCorrect()
    {
        // Für jede generierte Frage muss die eigene "richtige" Antwort auch als richtig erkannt werden.
        var random = new Random(1234);
        var questions = _generator.Generate(GradeLevel.Klasse6, 50, random)
            .Concat(_generator.Generate(GradeLevel.Klasse9, 50, random));

        foreach (var question in questions)
        {
            var correctAnswer = question.CorrectAnswers[0];
            Assert.True(question.CheckAnswer(correctAnswer), $"Frage '{question.Prompt}' akzeptiert eigene Lösung '{correctAnswer}' nicht.");
        }
    }

    [Fact]
    public void GeneratedAnswers_RejectObviouslyWrongAnswer()
    {
        var random = new Random(999);
        var questions = _generator.Generate(GradeLevel.Klasse9, 30, random);

        foreach (var question in questions)
        {
            Assert.False(question.CheckAnswer("völlig-falsche-antwort-xyz"));
        }
    }

    [Fact]
    public void Generate_UnknownGrade_ReturnsEmpty()
    {
        // Simuliert eine Klassenstufe ohne konfigurierte Themen (defensive Prüfung der Basisklasse).
        var questions = _generator.Generate((GradeLevel)999, 5, new Random());
        Assert.Empty(questions);
    }
}
