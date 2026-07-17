namespace LernTor.Core.Services;

/// <summary>
/// Wiederholungs-Intervalle für gemeisterte Aufgaben (Spaced Repetition, vereinfachtes
/// Anki-/SM-2-Prinzip): Eine richtig beantwortete Aufgabe wird nicht mehr FÜR IMMER
/// ausgeschlossen, sondern kehrt nach wachsenden Abständen zur Auffrischung zurück -
/// Stufe 1 nach 7 Tagen, Stufe 2 nach 30 Tagen, ab Stufe 3 alle 90 Tage. Wird eine fällige
/// Aufgabe erneut richtig beantwortet, steigt die Stufe; wird sie falsch beantwortet, verfällt
/// die Meisterung und die Fehler-Kartei übernimmt wieder (siehe MasteredPromptRepository).
/// Nebeneffekt: gemeisterte Aufgaben füllen den endlichen Fragen-Pool (~20 je Thema) über die
/// Zeit wieder auf, statt ihn dauerhaft zu verkleinern.
/// </summary>
public static class SpacedRepetitionSchedule
{
    private static readonly TimeSpan[] Intervals =
    {
        TimeSpan.FromDays(7),
        TimeSpan.FromDays(30),
        TimeSpan.FromDays(90)
    };

    /// <summary>Abstand bis zur nächsten Auffrischung für eine Meisterungs-Stufe (>= 1).
    /// Stufen oberhalb der höchsten definierten bleiben beim längsten Intervall (90 Tage).</summary>
    public static TimeSpan IntervalForStage(int stage) =>
        Intervals[Math.Clamp(stage, 1, Intervals.Length) - 1];

    public static DateTimeOffset NextDueAt(int stage, DateTimeOffset now) => now + IntervalForStage(stage);
}
