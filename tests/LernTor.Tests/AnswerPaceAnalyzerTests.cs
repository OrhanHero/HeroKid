using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class AnswerPaceAnalyzerTests
{
    [Fact]
    public void Ohne_Messwerte_gibt_es_keine_Auswertung()
    {
        var summary = AnswerPaceAnalyzer.Analyze(Array.Empty<(int, bool)>());

        Assert.False(summary.HasData);
        Assert.False(summary.IsSuspicious);
    }

    [Fact]
    public void Alt_Eintraege_ohne_Messung_zaehlen_nicht_als_blitzschnell()
    {
        // Antworten aus der Zeit vor dieser Messung stehen mit 0 in der DB. Wuerden sie mitzaehlen,
        // saehe jedes bestehende Profil beim ersten Oeffnen wie ein reiner Rater aus.
        var summary = AnswerPaceAnalyzer.Analyze(new[]
        {
            (0, true), (0, false), (0, true), (5000, true)
        });

        Assert.Equal(1, summary.Measured);
        Assert.Equal(0, summary.Quick);
    }

    [Fact]
    public void Schnelle_Antworten_werden_gezaehlt()
    {
        var summary = AnswerPaceAnalyzer.Analyze(new[]
        {
            (900, false), (1500, true), (2999, false), (3000, true), (8000, true)
        });

        Assert.Equal(5, summary.Measured);
        Assert.Equal(3, summary.Quick);
        // Genau 3000 ms zaehlt bereits als durchdacht - die Grenze ist einschliessend nach oben.
        Assert.Equal(2, summary.QuickAndWrong);
    }

    [Fact]
    public void Median_ist_robust_gegen_eine_lange_Pause()
    {
        // Das Kind geht zwischendurch weg: ein Ausreisser von 20 Minuten darf den Wert nicht kippen.
        var summary = AnswerPaceAnalyzer.Analyze(new[]
        {
            (4000, true), (5000, true), (6000, true), (1_200_000, true)
        });

        Assert.Equal(5500, summary.MedianMs);
    }

    [Fact]
    public void Median_bei_ungerader_Anzahl()
    {
        var summary = AnswerPaceAnalyzer.Analyze(new[] { (9000, true), (4000, true), (5000, true) });

        Assert.Equal(5000, summary.MedianMs);
    }

    [Fact]
    public void Wenige_Antworten_loesen_keine_Warnung_aus()
    {
        // Drei schnelle Antworten sind Zufall, keine Auffaelligkeit - erst ab 10 Messwerten
        // ist die Quote ueberhaupt aussagekraeftig.
        var summary = AnswerPaceAnalyzer.Analyze(Enumerable.Repeat((500, false), 5));

        Assert.True(summary.HasData);
        Assert.False(summary.IsSuspicious);
    }

    [Fact]
    public void Ueberwiegend_schnelle_Antworten_sind_auffaellig()
    {
        var answers = Enumerable.Repeat((500, false), 8)
            .Concat(Enumerable.Repeat((9000, true), 12));

        var summary = AnswerPaceAnalyzer.Analyze(answers);

        Assert.Equal(20, summary.Measured);
        Assert.Equal(8, summary.Quick);
        Assert.Equal(0.4, summary.QuickShare, 3);
        Assert.True(summary.IsSuspicious);
    }

    [Fact]
    public void Gruendliches_Arbeiten_loest_keine_Warnung_aus()
    {
        var summary = AnswerPaceAnalyzer.Analyze(Enumerable.Repeat((12_000, true), 30));

        Assert.True(summary.HasData);
        Assert.Equal(0, summary.Quick);
        Assert.False(summary.IsSuspicious);
    }
}
