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

        Assert.Equal(5, modules.Count);
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

    [Theory]
    [InlineData("ki-richtig-nutzen")]
    [InlineData("ki-ist-kein-leben")]
    public void Arbeitsweise_und_Grenzen_sind_eigene_Module(string modulId)
    {
        // Die beiden Module tragen die Kernbotschaft des KI-Bereichs: KI ist ein Werkzeug, das man
        // richtig bedienen muss - und kein Ersatz für echte Menschen. Sie dürfen nicht wegfallen.
        Assert.Contains(KiContentService.GetModules(), m => m.Id == modulId);
    }

    [Fact]
    public void Grenzen_Modul_nennt_die_Nummer_gegen_Kummer()
    {
        // Ein Kind in Not soll nicht erst suchen müssen - die Nummer steht im Lerntext selbst.
        var modul = KiContentService.GetModules().Single(m => m.Id == "ki-ist-kein-leben");

        Assert.Contains(modul.Sections, s => s.BodyDe.Contains("116 111") && s.BodyTr.Contains("116 111"));
    }

    [Fact]
    public void Beide_Klassenstufen_pruefen_den_verantwortlichen_Umgang()
    {
        // Lerntexte allein reichen nicht - die Themen müssen auch abgefragt werden, sonst
        // klickt das Kind sie durch und behält nichts.
        var random = new Random(4711);
        var klasse6 = _generator.Generate(GradeLevel.Klasse6, 60, random);
        var klasse9 = _generator.Generate(GradeLevel.Klasse9, 60, random);

        Assert.Contains(klasse6, q => q.Topic == "KI richtig nutzen");
        Assert.Contains(klasse9, q => q.Topic == "Wo KI nicht hingehört");
    }
}
