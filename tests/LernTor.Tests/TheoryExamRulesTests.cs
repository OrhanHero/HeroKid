using LernTor.Core.Enums;
using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

/// <summary>Die Regeln der amtlichen Theorieprüfung Klasse B.</summary>
public sealed class TheoryExamRulesTests
{
    private static TheoryQuestion Frage(int points) => new()
    {
        Id = $"f{points}-{Guid.NewGuid():n}",
        Topic = DrivingTheoryTopic.VorfahrtUndRegelung,
        Prompt = "Frage",
        Options = new[] { "a", "b", "c", "d" },
        CorrectIndices = new[] { 0 },
        Explanation = "weil",
        Points = points
    };

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(10, 0, true)]     // genau die Grenze zählt noch als bestanden
    [InlineData(11, 0, false)]
    [InlineData(10, 1, true)]
    [InlineData(0, 2, false)]     // zwei schwere Fehler allein reichen zum Durchfallen
    public void Bestanden_ist_wer_beide_Bedingungen_erfuellt(int punkte, int schwer, bool erwartet)
    {
        Assert.Equal(erwartet, TheoryExamRules.HasPassed(punkte, schwer));
    }

    [Fact]
    public void Zwei_schwere_Fehler_lassen_durchfallen_obwohl_28_von_30_richtig_sind()
    {
        // Der Fall, an dem eine Prozent-Logik scheitert: 93 % richtig und trotzdem nicht
        // bestanden. Genau deshalb rechnet die App in Fehlerpunkten.
        var fragen = new List<TheoryQuestion>();
        var richtig = new List<bool>();

        fragen.Add(Frage(5));
        richtig.Add(false);
        fragen.Add(Frage(5));
        richtig.Add(false);

        for (var i = 0; i < 28; i++)
        {
            fragen.Add(Frage(4));
            richtig.Add(true);
        }

        var ergebnis = TheoryExamRules.Evaluate(fragen, richtig);

        Assert.Equal(28, ergebnis.CorrectCount);
        Assert.Equal(10, ergebnis.WrongPoints);       // genau an der Punktgrenze
        Assert.Equal(2, ergebnis.HeavyMistakes);
        Assert.False(ergebnis.Passed);
        Assert.Contains("Fehlerpunkten", ergebnis.FailureReason);
    }

    [Fact]
    public void Fehlerpunkte_werden_nach_Gewicht_der_Frage_summiert()
    {
        var fragen = new List<TheoryQuestion> { Frage(2), Frage(3), Frage(4) };
        var ergebnis = TheoryExamRules.Evaluate(fragen, new[] { false, true, false });

        Assert.Equal(6, ergebnis.WrongPoints);        // 2 + 4, die richtige 3er zählt nicht
        Assert.Equal(2, ergebnis.WrongCount);
        Assert.Equal(0, ergebnis.HeavyMistakes);      // erst 5 Punkte gelten als schwer
        Assert.Equal(1, ergebnis.CorrectCount);
    }

    [Fact]
    public void Wer_besteht_bekommt_keine_Begruendung_untergeschoben()
    {
        var fragen = new List<TheoryQuestion> { Frage(4) };
        var ergebnis = TheoryExamRules.Evaluate(fragen, new[] { true });

        Assert.True(ergebnis.Passed);
        Assert.Equal(string.Empty, ergebnis.FailureReason);
    }

    [Fact]
    public void Eine_Antwort_zaehlt_nur_wenn_sie_vollstaendig_stimmt()
    {
        // Teilpunkte gibt es in der Prüfung nicht - das ist der häufigste Irrtum bei
        // Mehrfachfragen.
        var frage = new TheoryQuestion
        {
            Id = "mehrfach",
            Topic = DrivingTheoryTopic.VorfahrtUndRegelung,
            Prompt = "Frage",
            Options = new[] { "a", "b", "c", "d" },
            CorrectIndices = new[] { 0, 2 },
            Explanation = "weil"
        };

        Assert.True(frage.IsCorrect(new[] { 0, 2 }));
        Assert.True(frage.IsCorrect(new[] { 2, 0 }));        // Reihenfolge ist egal
        Assert.False(frage.IsCorrect(new[] { 0 }));          // eine vergessen
        Assert.False(frage.IsCorrect(new[] { 0, 1, 2 }));    // eine zu viel
        Assert.False(frage.IsCorrect(Array.Empty<int>()));
    }
}
