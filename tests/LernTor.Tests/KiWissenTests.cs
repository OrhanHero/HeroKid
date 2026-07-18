using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>KI-Bereich: Lernmodule (KiContentService) und Fragen-Pools (KiWissenGenerator).</summary>
public sealed class KiWissenTests
{
    private readonly KiWissenGenerator _generator = new();

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse9)]
    public void Generate_liefert_die_gewuenschte_Anzahl_mit_Erklaerungen(GradeLevel grade)
    {
        var questions = _generator.Generate(grade, 15, new Random(42));

        Assert.Equal(15, questions.Count);
        Assert.All(questions, q => Assert.Equal(Subject.KiWissen, q.Subject));
        Assert.All(questions, q => Assert.False(string.IsNullOrWhiteSpace(q.Explanation)));
        Assert.All(questions, q => Assert.True(q.CheckAnswer(q.CorrectAnswers[0])));
    }

    [Fact]
    public void Klasse7_faellt_auf_den_Klasse6_Pool_zurueck()
    {
        // KiWissen hat bewusst nur Klasse-6- und Klasse-9-Pools (Grundlagen/vertieft) -
        // Klasse-7-Profile bekommen über die Übergangsregel die Grundlagen.
        var questions = _generator.Generate(GradeLevel.Klasse7, 5, new Random(7));

        Assert.Equal(5, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse6, q.GradeLevel));
    }

    [Fact]
    public void Jede_Frage_hat_mindestens_drei_Antwortoptionen()
    {
        var random = new Random(1);
        var questions = _generator.Generate(GradeLevel.Klasse6, 30, random)
            .Concat(_generator.Generate(GradeLevel.Klasse9, 30, random));

        Assert.All(questions, q => Assert.True(q.Options.Count >= 3, $"Zu wenige Optionen: {q.Prompt}"));
        Assert.All(questions, q => Assert.Contains(q.CorrectAnswers[0], q.Options));
    }

    [Fact]
    public void Lernmodule_sind_vollstaendig_zweisprachig()
    {
        var modules = KiContentService.GetModules();

        Assert.Equal(3, modules.Count);
        Assert.All(modules, m =>
        {
            Assert.False(string.IsNullOrWhiteSpace(m.TitleDe));
            Assert.False(string.IsNullOrWhiteSpace(m.TitleTr));
            Assert.NotEmpty(m.Sections);
            Assert.All(m.Sections, s =>
            {
                Assert.False(string.IsNullOrWhiteSpace(s.HeadingDe));
                Assert.False(string.IsNullOrWhiteSpace(s.HeadingTr));
                Assert.False(string.IsNullOrWhiteSpace(s.BodyDe));
                Assert.False(string.IsNullOrWhiteSpace(s.BodyTr));
            });
        });
    }

    [Fact]
    public void Modul_Ids_sind_eindeutig()
    {
        var ids = KiContentService.GetModules().Select(m => m.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }
}
