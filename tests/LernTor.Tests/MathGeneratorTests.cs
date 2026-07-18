using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

public class MathGeneratorTests
{
    private readonly MathGenerator _generator = new();

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse7)]
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
            .Concat(_generator.Generate(GradeLevel.Klasse7, 50, random))
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
    public void Generate_UnbekannteHoehereStufe_faellt_auf_die_naechstniedrigere_zurueck()
    {
        // Übergangsregel der Basisklasse: eine Stufe ohne eigenen Themenpool nutzt die
        // nächstniedrigere vorhandene Stufe, statt gar nichts zu liefern (siehe
        // ExerciseGeneratorBase.Generate) - hier fällt die Fantasie-Stufe auf Klasse 9 zurück.
        var questions = _generator.Generate((GradeLevel)999, 5, new Random(7));

        Assert.Equal(5, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse9, q.GradeLevel));
    }

    [Fact]
    public void Generate_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = _generator.Generate(GradeLevel.Klasse7, 9, new Random(7));

        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Fach_ohne_Klasse7_Pool_faellt_auf_Klasse6_zurueck()
    {
        // MusikGenerator hat (noch) keinen Klasse-7-Pool - die Übergangsregel liefert dann
        // Klasse-6-Aufgaben (Wiederholung des zuletzt Gelernten) statt das Fach zu überspringen.
        var questions = new MusikGenerator().Generate(GradeLevel.Klasse7, 5, new Random(7));

        Assert.Equal(5, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse6, q.GradeLevel));
    }

    [Fact]
    public void Deutsch_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new GermanGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }
}
