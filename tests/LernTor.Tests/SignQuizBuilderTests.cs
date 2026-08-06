using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Aufbau der Zeichen-Quizfragen.</summary>
public sealed class SignQuizBuilderTests
{
    private static readonly IReadOnlyList<TrafficSign> Pool = TrafficSignCatalog.All;

    [Fact]
    public void Jede_Frage_hat_vier_verschiedene_Antworten()
    {
        var random = new Random(1);

        foreach (var zeichen in Pool)
        {
            var frage = SignQuizBuilder.Build(zeichen, Pool, random);

            Assert.Equal(SignQuizBuilder.OptionCount, frage.Options.Count);
            Assert.Equal(frage.Options.Count, frage.Options.Distinct(StringComparer.Ordinal).Count());
        }
    }

    [Fact]
    public void Die_richtige_Antwort_ist_dabei_und_richtig_markiert()
    {
        var random = new Random(2);

        foreach (var zeichen in Pool)
        {
            var frage = SignQuizBuilder.Build(zeichen, Pool, random);

            Assert.Equal(zeichen.Name, frage.CorrectAnswer);
            Assert.True(frage.IsCorrect(frage.CorrectIndex));
        }
    }

    [Fact]
    public void Die_Ablenker_kommen_aus_derselben_Gruppe()
    {
        // Sonst waere die Form schon die halbe Antwort: wer ein rotes Dreieck sieht, koennte
        // alles Blaue ausschliessen, ohne das Zeichen zu kennen.
        var random = new Random(3);
        var gefahrzeichen = TrafficSignCatalog.ByCategory(TrafficSignCategory.Gefahrzeichen);
        var namenDerGruppe = gefahrzeichen.Select(s => s.Name).ToHashSet(StringComparer.Ordinal);

        foreach (var zeichen in gefahrzeichen)
        {
            var frage = SignQuizBuilder.Build(zeichen, Pool, random);

            Assert.All(frage.Options, option => Assert.Contains(option, namenDerGruppe));
        }
    }

    [Fact]
    public void Reicht_die_eigene_Gruppe_nicht_wird_aufgefuellt()
    {
        // Eine Gruppe mit nur zwei Zeichen ergaebe sonst zwei statt vier Antworten.
        var random = new Random(4);
        var winzigeGruppe = Pool.Where(s => s.Category == TrafficSignCategory.Gefahrzeichen).Take(2).ToList();
        var poolMitLuecke = winzigeGruppe
            .Concat(Pool.Where(s => s.Category == TrafficSignCategory.Richtzeichen))
            .ToList();

        var frage = SignQuizBuilder.Build(winzigeGruppe[0], poolMitLuecke, random);

        Assert.Equal(SignQuizBuilder.OptionCount, frage.Options.Count);
    }

    [Fact]
    public void Die_richtige_Antwort_steht_nicht_immer_an_derselben_Stelle()
    {
        var random = new Random(5);

        var positionen = Pool
            .Select(zeichen => SignQuizBuilder.Build(zeichen, Pool, random).CorrectIndex)
            .Distinct()
            .ToList();

        Assert.Equal(SignQuizBuilder.OptionCount, positionen.Count);
    }

    [Fact]
    public void Die_richtige_Antwort_ist_nicht_systematisch_die_laengste()
    {
        // Genau das Muster, das die Kinder in dieser App schon einmal ausgenutzt haben
        // (siehe scripts/check-answer-length-bias.py). Hier kann es gar nicht entstehen, weil
        // alle vier Optionen echte Zeichennamen aus demselben Katalog sind - der Test haelt
        // fest, dass das so bleibt.
        var random = new Random(6);
        var laengsteWarRichtig = 0;

        foreach (var zeichen in Pool)
        {
            var frage = SignQuizBuilder.Build(zeichen, Pool, random);
            var laengste = frage.Options.OrderByDescending(o => o.Length).First();

            if (string.Equals(laengste, frage.CorrectAnswer, StringComparison.Ordinal))
            {
                laengsteWarRichtig++;
            }
        }

        var anteil = (double)laengsteWarRichtig / Pool.Count;

        Assert.True(anteil < 0.60, $"Die laengste Antwort war in {anteil:P0} der Faelle die richtige.");
    }

    [Fact]
    public void Mehrere_Fragen_behalten_die_uebergebene_Reihenfolge()
    {
        var zeichen = Pool.Take(5).ToList();

        var fragen = SignQuizBuilder.BuildMany(zeichen, Pool, new Random(7));

        Assert.Equal(
            zeichen.Select(s => s.Number),
            fragen.Select(f => f.Sign.Number));
    }
}
