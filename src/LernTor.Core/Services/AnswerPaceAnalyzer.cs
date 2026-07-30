namespace LernTor.Core.Services;

/// <summary>
/// Wertet aus, wie schnell Aufgaben beantwortet wurden. Zweck ist nicht, Kinder zu bestrafen,
/// sondern Eltern sichtbar zu machen, ob wirklich gelesen und nachgedacht wurde: eine Antwort
/// nach 1,2 Sekunden ist geraten, keine Leistung - im reinen Richtig/Falsch-Bericht sieht man
/// das nicht, weil bei drei Optionen auch Raten in einem Drittel der Fälle "richtig" ergibt.
///
/// <para>Bewusst nur ein Hinweis, keine Sperre: die Mindestzeit pro Aufgabe
/// (<c>StudentProfile.ExerciseSecondsPerQuestion</c>) bremst bereits das Weiterklicken. Wer
/// zusätzlich das Antworten selbst sperrt, bestraft auch das Kind, das die Antwort einfach
/// sofort weiß.</para>
/// </summary>
public static class AnswerPaceAnalyzer
{
    /// <summary>
    /// Ab wie vielen Millisekunden eine Antwort als "durchdacht" zählt. Drei Sekunden reichen
    /// nicht, um eine Frage samt Optionen zu lesen - darunter wurde geraten oder die Antwort war
    /// bereits auswendig bekannt.
    /// </summary>
    public const int GuessThresholdMs = 3000;

    /// <summary>Ergebnis der Auswertung eines Zeitraums.</summary>
    /// <param name="Measured">Antworten mit gemessener Dauer (Altbestand ohne Messung zählt nicht mit).</param>
    /// <param name="Quick">Davon schneller als <see cref="GuessThresholdMs"/>.</param>
    /// <param name="QuickAndWrong">Davon schnell UND falsch - das deutlichste Rate-Signal.</param>
    /// <param name="MedianMs">Median der gemessenen Dauern; robuster als der Mittelwert, den
    /// eine einzelne lange Pause (Kind geht zwischendurch weg) sonst verzerrt.</param>
    public readonly record struct PaceSummary(int Measured, int Quick, int QuickAndWrong, int MedianMs)
    {
        public bool HasData => Measured > 0;

        /// <summary>Anteil der schnellen Antworten (0.0-1.0).</summary>
        public double QuickShare => Measured > 0 ? (double)Quick / Measured : 0;

        /// <summary>
        /// Auffällig ab einem Drittel schneller Antworten - das entspricht ungefähr der Quote,
        /// die reines Raten bei drei Optionen erzeugt.
        /// </summary>
        public bool IsSuspicious => Measured >= 10 && QuickShare >= 0.33;
    }

    /// <summary>
    /// Wertet Antwortdauern aus. Einträge ohne Messung (Dauer 0 oder negativ) werden übersprungen -
    /// das sind Antworten aus der Zeit vor dieser Messung, die sonst als "blitzschnell" gälten.
    /// </summary>
    public static PaceSummary Analyze(IEnumerable<(int DurationMs, bool WasCorrect)> answers)
    {
        var durations = new List<int>();
        int quick = 0;
        int quickAndWrong = 0;

        foreach (var (durationMs, wasCorrect) in answers)
        {
            if (durationMs <= 0) continue;

            durations.Add(durationMs);
            if (durationMs >= GuessThresholdMs) continue;

            quick++;
            if (!wasCorrect) quickAndWrong++;
        }

        if (durations.Count == 0)
        {
            return new PaceSummary(0, 0, 0, 0);
        }

        durations.Sort();
        var median = durations.Count % 2 == 1
            ? durations[durations.Count / 2]
            : (durations[durations.Count / 2 - 1] + durations[durations.Count / 2]) / 2;

        return new PaceSummary(durations.Count, quick, quickAndWrong, median);
    }
}
