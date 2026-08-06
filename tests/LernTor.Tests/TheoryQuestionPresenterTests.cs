using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Das Mischen der Antworten.
///
/// <para>Im Katalog steht die richtige Antwort bewusst an erster Stelle, damit eine Frage beim
/// Schreiben und Nachlesen sofort zu erfassen ist. Genau so angezeigt wäre sie wertlos - wer
/// immer die erste ankreuzt, hätte volle Punktzahl. Deshalb sind das hier die wichtigsten Tests
/// des ganzen Bereichs.</para>
/// </summary>
public sealed class TheoryQuestionPresenterTests
{
    private static TheoryQuestion Einfach() => new()
    {
        Id = "einfach",
        Topic = DrivingTheoryTopic.VorfahrtUndRegelung,
        Prompt = "Frage",
        Options = new[] { "richtig", "falsch A", "falsch B", "falsch C" },
        CorrectIndices = new[] { 0 },
        Explanation = "weil"
    };

    private static TheoryQuestion Mehrfach() => new()
    {
        Id = "mehrfach",
        Topic = DrivingTheoryTopic.VorfahrtUndRegelung,
        Prompt = "Frage",
        Options = new[] { "richtig 1", "richtig 2", "falsch A", "falsch B" },
        CorrectIndices = new[] { 0, 1 },
        Explanation = "weil"
    };

    [Fact]
    public void Die_richtige_Antwort_landet_nicht_immer_auf_Platz_eins()
    {
        // Der eigentliche Zweck: "immer A ankreuzen" darf nicht funktionieren.
        var zufall = new Random(1);
        var positionen = new HashSet<int>();

        for (var i = 0; i < 50; i++)
        {
            positionen.Add(TheoryQuestionPresenter.Present(Einfach(), zufall).CorrectIndices[0]);
        }

        Assert.True(positionen.Count > 1, "Die richtige Antwort stand immer an derselben Stelle.");
    }

    [Fact]
    public void Die_richtigen_Positionen_wandern_mit()
    {
        var zufall = new Random(7);

        for (var i = 0; i < 200; i++)
        {
            var frage = Mehrfach();
            var gestellt = TheoryQuestionPresenter.Present(frage, zufall);

            // Aus den neuen Positionen müssen wieder genau die richtigen Texte werden.
            var texte = gestellt.CorrectIndices.Select(index => gestellt.Options[index]).ToHashSet();

            Assert.Equal(new HashSet<string> { "richtig 1", "richtig 2" }, texte);
        }
    }

    [Fact]
    public void Es_geht_keine_Antwort_verloren_und_es_kommt_keine_dazu()
    {
        var zufall = new Random(3);
        var frage = Mehrfach();

        var gestellt = TheoryQuestionPresenter.Present(frage, zufall);

        Assert.Equal(frage.Options.OrderBy(text => text, StringComparer.Ordinal),
            gestellt.Options.OrderBy(text => text, StringComparer.Ordinal));
    }

    [Fact]
    public void Die_gemischte_Frage_bewertet_wie_die_urspruengliche()
    {
        var zufall = new Random(11);
        var gestellt = TheoryQuestionPresenter.Present(Mehrfach(), zufall);

        Assert.True(gestellt.IsMultipleChoice);
        Assert.True(gestellt.IsCorrect(gestellt.CorrectIndices));
        Assert.False(gestellt.IsCorrect(gestellt.CorrectIndices.Take(1)));

        var falsche = Enumerable.Range(0, gestellt.Options.Count)
            .Except(gestellt.CorrectIndices)
            .ToList();

        Assert.False(gestellt.IsCorrect(falsche));
    }

    [Fact]
    public void Die_Reihenfolge_der_Fragen_bleibt_erhalten()
    {
        // Gemischt werden die Antworten, nicht die Fragen - die Reihenfolge legt der Aufrufer
        // fest (Prüfung streut über Sachgebiete, der Trainer sortiert nach Schwäche).
        var fragen = new[] { Einfach(), Mehrfach() };

        var gestellt = TheoryQuestionPresenter.PresentAll(fragen, new Random(5));

        Assert.Equal(new[] { "einfach", "mehrfach" },
            gestellt.Select(frage => frage.Question.Id));
    }

    [Fact]
    public void Der_ganze_Katalog_laesst_sich_mischen_ohne_dass_eine_Loesung_verrutscht()
    {
        // Fängt Katalogfehler ab, die erst beim Mischen auffallen - etwa einen Index, der auf
        // eine Antwort zeigt, die es gar nicht gibt.
        var zufall = new Random(99);

        foreach (var frage in DrivingTheoryCatalog.All)
        {
            var gestellt = TheoryQuestionPresenter.Present(frage, zufall);

            var erwartet = frage.CorrectIndices.Select(index => frage.Options[index]).ToHashSet();
            var tatsaechlich = gestellt.CorrectIndices.Select(index => gestellt.Options[index]).ToHashSet();

            Assert.Equal(erwartet, tatsaechlich);
        }
    }
}
