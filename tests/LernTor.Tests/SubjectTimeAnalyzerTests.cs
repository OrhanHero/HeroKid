using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Zeit-/Trefferbilanz je Fach. Der Punkt ist die Unterscheidung, die eine Trefferquote allein
/// nicht hergibt: schnell und falsch heisst geraten, langsam und falsch heisst nicht verstanden.
/// </summary>
public sealed class SubjectTimeAnalyzerTests
{
    private static IEnumerable<SubjectTimeAnalyzer.Entry> Antworten(
        Subject subject, int anzahl, int dauerMs, double trefferquote)
    {
        var richtig = (int)Math.Round(anzahl * trefferquote);
        for (var i = 0; i < anzahl; i++)
        {
            yield return new SubjectTimeAnalyzer.Entry(subject.ToString(), i < richtig, dauerMs);
        }
    }

    private static SubjectTimeStat Fach(IReadOnlyList<SubjectTimeStat> stats, Subject subject) =>
        stats.Single(s => s.Subject == subject);

    [Fact]
    public void Ohne_Daten_kommt_nichts_zurueck()
    {
        Assert.Empty(SubjectTimeAnalyzer.Analyze(Array.Empty<SubjectTimeAnalyzer.Entry>()));
    }

    [Fact]
    public void Unbekannte_Faecher_werden_uebersprungen()
    {
        var entries = new[] { new SubjectTimeAnalyzer.Entry("GibtEsNicht", true, 5000) };

        Assert.Empty(SubjectTimeAnalyzer.Analyze(entries));
    }

    [Fact]
    public void Zeitaufwendigstes_Fach_steht_vorne()
    {
        var entries = Antworten(Subject.Mathematik, 10, 10_000, 0.8)
            .Concat(Antworten(Subject.Deutsch, 10, 2_000, 0.8))
            .ToList();

        Assert.Equal(Subject.Mathematik, SubjectTimeAnalyzer.Analyze(entries)[0].Subject);
    }

    [Fact]
    public void Summe_und_Median_werden_berechnet()
    {
        var entries = Antworten(Subject.Physik, 4, 5_000, 0.5).ToList();

        var stat = Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Physik);

        Assert.Equal(4, stat.Answered);
        Assert.Equal(4, stat.MeasuredCount);
        Assert.Equal(20_000, stat.TotalMs);
        Assert.Equal(5_000, stat.MedianMs);
        Assert.Equal(2, stat.CorrectCount);
        Assert.Equal(0.5, stat.Accuracy);
    }

    [Fact]
    public void Ungemessene_Alt_Zeilen_zaehlen_nicht_in_die_Zeit()
    {
        // Zeilen aus der Zeit vor der Messung haben Dauer 0 - als "blitzschnell" gewertet
        // wuerden sie jedes Fach als Raten erscheinen lassen.
        var entries = Antworten(Subject.Chemie, 5, 8_000, 1.0)
            .Concat(Antworten(Subject.Chemie, 5, 0, 1.0))
            .ToList();

        var stat = Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Chemie);

        Assert.Equal(10, stat.Answered);
        Assert.Equal(5, stat.MeasuredCount);
        Assert.Equal(40_000, stat.TotalMs);
        Assert.Equal(8_000, stat.MedianMs);
    }

    [Fact]
    public void Schnell_und_falsch_heisst_geraten()
    {
        var entries = Antworten(Subject.Mathematik, 10, 1_000, 0.3)
            .Concat(Antworten(Subject.Deutsch, 10, 10_000, 0.9))
            .ToList();

        Assert.Equal(SubjectEffort.VermutlichGeraten,
            Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Mathematik).Effort);
    }

    [Fact]
    public void Langsam_und_falsch_heisst_hier_hakt_es()
    {
        // Der wichtigste Fall: das Kind gibt sich Muehe und kommt trotzdem nicht weiter.
        var entries = Antworten(Subject.Chemie, 10, 20_000, 0.3)
            .Concat(Antworten(Subject.Deutsch, 10, 5_000, 0.9))
            .ToList();

        Assert.Equal(SubjectEffort.HierHaktEs,
            Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Chemie).Effort);
    }

    [Fact]
    public void Langsam_und_richtig_ist_kein_Problem()
    {
        var entries = Antworten(Subject.Physik, 10, 20_000, 0.9)
            .Concat(Antworten(Subject.Deutsch, 10, 5_000, 0.9))
            .ToList();

        Assert.Equal(SubjectEffort.GruendlichAberLangsam,
            Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Physik).Effort);
    }

    [Fact]
    public void Schnell_und_richtig_heisst_sitzt()
    {
        var entries = Antworten(Subject.Englisch, 10, 1_500, 0.95)
            .Concat(Antworten(Subject.Chemie, 10, 15_000, 0.5))
            .ToList();

        Assert.Equal(SubjectEffort.SitztSicher,
            Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Englisch).Effort);
    }

    [Fact]
    public void Zu_wenige_Messungen_ergeben_kein_Urteil()
    {
        var entries = Antworten(Subject.Kunst, 3, 30_000, 0.0)
            .Concat(Antworten(Subject.Deutsch, 20, 5_000, 0.9))
            .ToList();

        Assert.Equal(SubjectEffort.KeineDaten,
            Fach(SubjectTimeAnalyzer.Analyze(entries), Subject.Kunst).Effort);
    }

    [Fact]
    public void Schnell_und_langsam_sind_relativ_zum_Kind()
    {
        // Ein gruendliches Kind braucht ueberall lange - dann ist "lange" der Normalfall und
        // darf nicht jedes Fach als Problem markieren.
        var gruendlich = Antworten(Subject.Mathematik, 10, 25_000, 0.9)
            .Concat(Antworten(Subject.Deutsch, 10, 26_000, 0.9))
            .Concat(Antworten(Subject.Physik, 10, 24_000, 0.9))
            .ToList();

        var stats = SubjectTimeAnalyzer.Analyze(gruendlich);

        Assert.All(stats, stat => Assert.Equal(SubjectEffort.Unauffaellig, stat.Effort));
    }

    [Fact]
    public void Ein_flottes_Kind_wird_nicht_pauschal_als_Rater_eingestuft()
    {
        var flott = Antworten(Subject.Mathematik, 10, 2_000, 0.9)
            .Concat(Antworten(Subject.Deutsch, 10, 2_100, 0.9))
            .Concat(Antworten(Subject.Englisch, 10, 1_900, 0.9))
            .ToList();

        Assert.All(SubjectTimeAnalyzer.Analyze(flott),
            stat => Assert.NotEqual(SubjectEffort.VermutlichGeraten, stat.Effort));
    }

    [Theory]
    [InlineData(4, SubjectEffort.KeineDaten)]
    [InlineData(5, SubjectEffort.HierHaktEs)]
    public void Die_Mindestmenge_entscheidet_ueber_ein_Urteil(int gemessen, SubjectEffort erwartet)
    {
        Assert.Equal(erwartet,
            SubjectTimeAnalyzer.Classify(gemessen, median: 20_000, overallMedian: 10_000, accuracy: 0.2));
    }

    [Fact]
    public void Ohne_jede_Messung_gibt_es_kein_Urteil()
    {
        Assert.Equal(SubjectEffort.KeineDaten,
            SubjectTimeAnalyzer.Classify(measuredCount: 20, median: 0, overallMedian: 0, accuracy: 0.9));
    }
}
