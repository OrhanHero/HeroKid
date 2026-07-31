using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Eigene Entwicklung des Kindes: frueher gegen jetzt, Fach fuer Fach.</summary>
public sealed class LearningProgressTrackerTests
{
    private static readonly DateOnly Heute = new(2026, 8, 5);

    /// <summary>Antworten in einem der beiden Fenster: <paramref name="correct"/> von
    /// <paramref name="total"/> richtig.</summary>
    private static IEnumerable<LearningProgressTracker.Answer> Antworten(
        Subject fach, int tageZurueck, int total, int correct) =>
        Enumerable.Range(0, total).Select(i =>
            new LearningProgressTracker.Answer(fach, Heute.AddDays(-tageZurueck), i < correct));

    [Fact]
    public void Eine_Verbesserung_wird_als_solche_erkannt()
    {
        var antworten = Antworten(Subject.Mathematik, 20, 10, 2)      // früher 20 %
            .Concat(Antworten(Subject.Mathematik, 2, 10, 7));         // jetzt 70 %

        var zeile = Assert.Single(LearningProgressTracker.Compare(antworten, Heute));

        Assert.Equal(Subject.Mathematik, zeile.Subject);
        Assert.Equal(20, zeile.EarlierPercent);
        Assert.Equal(70, zeile.RecentPercent);
        Assert.Equal(50, zeile.Change);
        Assert.Equal(ProgressTrend.Verbessert, zeile.Trend);
    }

    [Fact]
    public void Gleichbleibend_wird_nicht_als_Veraenderung_verkauft()
    {
        // "Du bist um 2 Punkte besser geworden" waere eine leere Aussage.
        var antworten = Antworten(Subject.Deutsch, 20, 10, 6)
            .Concat(Antworten(Subject.Deutsch, 2, 10, 6));

        var zeile = Assert.Single(LearningProgressTracker.Compare(antworten, Heute));

        Assert.Equal(0, zeile.Change);
        Assert.Equal(ProgressTrend.Gleich, zeile.Trend);
    }

    [Fact]
    public void Ein_Rueckschritt_wird_gezeigt_statt_verschwiegen()
    {
        // Ihn zu verschweigen waere unehrlich - angeprangert wird er in der Anzeige trotzdem nicht.
        var antworten = Antworten(Subject.Physik, 20, 10, 8)
            .Concat(Antworten(Subject.Physik, 2, 10, 4));

        var zeile = Assert.Single(LearningProgressTracker.Compare(antworten, Heute));

        Assert.Equal(-40, zeile.Change);
        Assert.Equal(ProgressTrend.Schwaecher, zeile.Trend);
    }

    [Theory]
    [InlineData(4, ProgressTrend.Gleich)]
    [InlineData(5, ProgressTrend.Verbessert)]
    [InlineData(-4, ProgressTrend.Gleich)]
    [InlineData(-5, ProgressTrend.Schwaecher)]
    public void Kleine_Schwankungen_gelten_als_gleich(int aenderung, ProgressTrend erwartet)
    {
        var zeile = new SubjectProgress(Subject.Mathematik, 50, 50 + aenderung, 10, 10);

        Assert.Equal(erwartet, zeile.Trend);
    }

    [Fact]
    public void Ohne_genug_Aufgaben_in_beiden_Zeitraeumen_wird_nicht_verglichen()
    {
        // Eine Aussage aus drei Aufgaben ist Zufall, kein Fortschritt.
        var zuWenigJetzt = Antworten(Subject.Physik, 20, 10, 5)
            .Concat(Antworten(Subject.Physik, 2, LearningProgressTracker.MinAnswersPerWindow - 1, 3));
        Assert.Empty(LearningProgressTracker.Compare(zuWenigJetzt, Heute));

        var zuWenigFrueher = Antworten(Subject.Physik, 20, LearningProgressTracker.MinAnswersPerWindow - 1, 2)
            .Concat(Antworten(Subject.Physik, 2, 10, 8));
        Assert.Empty(LearningProgressTracker.Compare(zuWenigFrueher, Heute));
    }

    [Fact]
    public void Ein_neues_Fach_ohne_Vergangenheit_taucht_nicht_auf()
    {
        // Sonst stuende "0 % → 80 %" da, obwohl das Kind vorher nur nichts gemacht hat.
        var nurJetzt = Antworten(Subject.Chemie, 3, 12, 10);

        Assert.Empty(LearningProgressTracker.Compare(nurJetzt, Heute));
    }

    [Fact]
    public void Die_beiden_Fenster_ueberschneiden_sich_nicht_und_lassen_keine_Luecke()
    {
        // Der Tag genau an der Grenze muss zu genau einem Fenster gehoeren.
        var grenzeJung = LearningProgressTracker.WindowDays - 1;   // aeltester Tag im jungen Fenster
        var grenzeAlt = LearningProgressTracker.WindowDays;        // juengster Tag im alten Fenster

        var antworten = Antworten(Subject.Mathematik, grenzeAlt, 10, 0)
            .Concat(Antworten(Subject.Mathematik, grenzeJung, 10, 10));

        var zeile = Assert.Single(LearningProgressTracker.Compare(antworten, Heute));

        Assert.Equal(0, zeile.EarlierPercent);
        Assert.Equal(100, zeile.RecentPercent);
        Assert.Equal(10, zeile.EarlierAnswered);
        Assert.Equal(10, zeile.RecentAnswered);
    }

    [Fact]
    public void Zu_alte_Aufgaben_zaehlen_nicht_mehr_mit()
    {
        var zuAlt = LearningProgressTracker.WindowDays * 2;

        var antworten = Antworten(Subject.Mathematik, zuAlt, 20, 20)
            .Concat(Antworten(Subject.Mathematik, 20, 10, 2))
            .Concat(Antworten(Subject.Mathematik, 2, 10, 7));

        var zeile = Assert.Single(LearningProgressTracker.Compare(antworten, Heute));

        // Der frühere Wert bleibt 20 % - die 20 uralten Volltreffer sind draußen.
        Assert.Equal(20, zeile.EarlierPercent);
        Assert.Equal(10, zeile.EarlierAnswered);
    }

    [Fact]
    public void Die_groesste_Verbesserung_steht_oben()
    {
        // Das Erfreulichste zuerst - gewertet wird trotzdem nichts weg.
        var antworten = Antworten(Subject.Deutsch, 20, 10, 6).Concat(Antworten(Subject.Deutsch, 2, 10, 6))
            .Concat(Antworten(Subject.Mathematik, 20, 10, 2)).Concat(Antworten(Subject.Mathematik, 2, 10, 7))
            .Concat(Antworten(Subject.Physik, 20, 10, 8)).Concat(Antworten(Subject.Physik, 2, 10, 4));

        var reihenfolge = LearningProgressTracker.Compare(antworten, Heute)
            .Select(zeile => zeile.Subject)
            .ToList();

        Assert.Equal(new[] { Subject.Mathematik, Subject.Deutsch, Subject.Physik }, reihenfolge);
    }

    [Fact]
    public void Ohne_Daten_gibt_es_keine_Zeilen()
    {
        Assert.Empty(LearningProgressTracker.Compare(Array.Empty<LearningProgressTracker.Answer>(), Heute));
    }
}
