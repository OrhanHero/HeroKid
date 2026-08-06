using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der Fragenkatalog selbst - die Prüfungen, die man am Text einer Frage festmachen kann.
///
/// <para>Ein Katalog von Hand geschriebener Fragen geht nicht am Code kaputt, sondern an
/// Flüchtigkeitsfehlern: ein Index, der auf keine Antwort zeigt, eine doppelte Kennung, ein
/// Verkehrszeichen, das es nicht gibt. Genau das steht hier.</para>
/// </summary>
public sealed class DrivingTheoryCatalogTests
{
    [Fact]
    public void Jede_Frage_hat_eine_eigene_Kennung()
    {
        // Die Kennung ist der Schlüssel des Lernstands. Zwei Fragen mit derselben würden sich
        // gegenseitig als "sitzt" markieren.
        var doppelte = DrivingTheoryCatalog.All
            .GroupBy(frage => frage.Id, StringComparer.Ordinal)
            .Where(gruppe => gruppe.Count() > 1)
            .Select(gruppe => gruppe.Key)
            .ToList();

        Assert.Empty(doppelte);
    }

    [Fact]
    public void Jede_Frage_hat_gueltige_Antworten_und_Loesungen()
    {
        foreach (var frage in DrivingTheoryCatalog.All)
        {
            Assert.True(frage.Options.Count >= 3, $"{frage.Id}: zu wenige Antwortmöglichkeiten");
            Assert.All(frage.Options, option => Assert.False(string.IsNullOrWhiteSpace(option)));

            Assert.NotEmpty(frage.CorrectIndices);
            Assert.Equal(frage.CorrectIndices.Count, frage.CorrectIndices.Distinct().Count());
            Assert.All(frage.CorrectIndices, index =>
                Assert.InRange(index, 0, frage.Options.Count - 1));

            // Alles richtig wäre keine Frage mehr.
            Assert.True(frage.CorrectIndices.Count < frage.Options.Count,
                $"{frage.Id}: alle Antworten sind richtig");

            Assert.False(string.IsNullOrWhiteSpace(frage.Prompt), $"{frage.Id}: keine Frage");
            Assert.False(string.IsNullOrWhiteSpace(frage.Explanation), $"{frage.Id}: keine Begründung");
        }
    }

    [Fact]
    public void Die_Fehlerpunkte_liegen_im_amtlichen_Bereich()
    {
        // Die Prüfung kennt 2, 3, 4 und 5 Fehlerpunkte - andere Werte gibt es nicht.
        Assert.All(DrivingTheoryCatalog.All, frage => Assert.InRange(frage.Points, 2, 5));
    }

    [Fact]
    public void Jedes_Sachgebiet_hat_Fragen()
    {
        // Ein leeres Sachgebiet wäre eine Zeile in der Übersicht, hinter der nichts steckt -
        // und der Schwachstellen-Trainer könnte daraus nichts ziehen.
        foreach (var thema in DrivingTheoryCatalog.Topics)
        {
            Assert.NotEmpty(DrivingTheoryCatalog.ByTopic(thema));
        }
    }

    [Fact]
    public void Es_gibt_Fragen_mit_mehreren_richtigen_Antworten()
    {
        // Der Kern der echten Prüfung. Ein Katalog nur mit Eins-aus-vier würde genau das
        // antrainieren, woran Prüflinge scheitern.
        Assert.True(DrivingTheoryCatalog.All.Count(frage => frage.IsMultipleChoice) >= 5);
    }

    [Fact]
    public void Verweise_auf_Verkehrszeichen_zeigen_auf_vorhandene_Zeichen()
    {
        // Eine Frage mit unbekannter Zeichennummer zeigt in der Ansicht eine leere Fläche -
        // und die Frage wäre ohne das Bild nicht beantwortbar.
        foreach (var frage in DrivingTheoryCatalog.All.Where(f => f.SignNumber is not null))
        {
            Assert.NotNull(TrafficSignCatalog.ByNumber(frage.SignNumber!));
        }
    }

    [Fact]
    public void Die_Fragen_lassen_sich_ueber_ihre_Kennung_finden()
    {
        var erste = DrivingTheoryCatalog.All[0];

        Assert.Equal(erste, DrivingTheoryCatalog.ById(erste.Id));
        Assert.Null(DrivingTheoryCatalog.ById("gibt-es-nicht"));
    }

    [Fact]
    public void Die_laengste_Antwort_ist_nicht_verlaesslich_die_richtige()
    {
        // Diese Codebasis hat es schon einmal erlebt, dass eine Antwortlänge zum Muster wird
        // (siehe scripts/check-answer-length-bias.py). Bei 65 Fragen mit vier Antworten wären
        // 25 % der Zufallswert; die Grenze liegt bewusst bei 60 % und nicht bei 30 %, denn
        // "die längste ist NIE die richtige" wäre das nächste ausnutzbare Muster.
        var treffer = DrivingTheoryCatalog.All.Count(frage =>
        {
            var maxLaenge = frage.Options.Max(option => option.Length);
            var laengste = Enumerable.Range(0, frage.Options.Count)
                .Where(i => frage.Options[i].Length == maxLaenge)
                .ToList();

            return laengste.All(frage.CorrectIndices.Contains);
        });

        var anteil = treffer * 100.0 / DrivingTheoryCatalog.All.Count;

        Assert.True(anteil < 60, $"Die längste Antwort ist in {anteil:0.#} % der Fragen die richtige.");
    }

    [Fact]
    public void Jedes_Sachgebiet_hat_eine_deutsche_Beschriftung()
    {
        foreach (var thema in Enum.GetValues<DrivingTheoryTopic>())
        {
            var beschriftung = DrivingTheoryCatalog.TopicLabel(thema);

            Assert.False(string.IsNullOrWhiteSpace(beschriftung));
            // Ein durchgefallener switch-Zweig gäbe den Enum-Namen zurück - genau das soll
            // auffallen, statt "BefoerderungUndAnhaenger" auf dem Bildschirm zu zeigen.
            Assert.NotEqual(thema.ToString(), beschriftung);
        }
    }
}
