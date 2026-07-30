namespace LernTor.Core.Services;

/// <summary>
/// Wochenziel: an wie vielen Tagen dieser Woche das Kind gelernt hat, gemessen an einem von den
/// Eltern gesetzten Ziel (z.B. "4 Tage diese Woche").
///
/// <para>Bewusst dieselbe Haltung wie beim <see cref="StreakCalculator"/>: reine Anzeige, keine
/// Druckmechanik. Ein verfehltes Ziel hat keine Folgen - kein Sternverlust, keine Sperre, keine
/// Mahnung. Der Unterschied zur Serie ist die Nachsicht: eine Serie zerbricht an einem einzigen
/// verpassten Tag, ein Wochenziel nicht. Wer am Montag und Dienstag nicht kann, schafft die vier
/// Tage immer noch.</para>
///
/// <para>Die Woche beginnt am Montag (DIN 1355 / ISO 8601, in Deutschland und der Türkei üblich) -
/// mit dem Sonntag als Wochenstart wäre "diese Woche" für ein Schulkind eine andere Woche als für
/// seinen Stundenplan.</para>
/// </summary>
public static class WeeklyGoalCalculator
{
    /// <summary>Zulässige Ziele, die Eltern einstellen können. 0 = Wochenziel aus.</summary>
    public static readonly IReadOnlyList<int> AllowedGoals = new[] { 0, 3, 4, 5, 6, 7 };

    /// <summary>Ergebnis für die Anzeige.</summary>
    /// <param name="Goal">Gesetztes Ziel in Tagen (0 = keins).</param>
    /// <param name="LearnedDays">Tage dieser Woche mit mindestens einer beantworteten Aufgabe.</param>
    /// <param name="DaysLeft">Verbleibende Tage der Woche einschließlich heute.</param>
    public readonly record struct WeeklyGoalStatus(int Goal, int LearnedDays, int DaysLeft)
    {
        public bool IsActive => Goal > 0;

        public bool IsReached => IsActive && LearnedDays >= Goal;

        /// <summary>Wie viele Tage noch fehlen (nie negativ).</summary>
        public int Missing => Math.Max(0, Goal - LearnedDays);

        /// <summary>
        /// Ob das Ziel rechnerisch noch erreichbar ist. Ist es das nicht mehr, wird bewusst
        /// nichts Mahnendes angezeigt - die Woche ist dann einfach so gelaufen.
        /// </summary>
        public bool IsStillPossible => IsActive && Missing <= DaysLeft;

        /// <summary>Fortschritt 0.0-1.0 für einen Balken.</summary>
        public double Progress => IsActive ? Math.Clamp((double)LearnedDays / Goal, 0, 1) : 0;
    }

    /// <summary>Montag der Woche, in der <paramref name="date"/> liegt.</summary>
    public static DateOnly StartOfWeek(DateOnly date)
    {
        // DayOfWeek zählt Sonntag=0; für einen Montagsstart muss der Sonntag 6 Tage zurückgehen.
        int offset = ((int)date.DayOfWeek + 6) % 7;
        return date.AddDays(-offset);
    }

    public static WeeklyGoalStatus Evaluate(IEnumerable<DateOnly> learningDays, DateOnly today, int goal)
    {
        if (goal <= 0)
        {
            return new WeeklyGoalStatus(0, 0, 0);
        }

        var start = StartOfWeek(today);
        var end = start.AddDays(6);

        var days = learningDays as IReadOnlySet<DateOnly> ?? learningDays.ToHashSet();
        var learned = days.Count(d => d >= start && d <= end);

        // Einschließlich heute: wer heute noch lernt, erfüllt den heutigen Tag.
        var daysLeft = end.DayNumber - today.DayNumber + 1;

        return new WeeklyGoalStatus(goal, learned, Math.Max(0, daysLeft));
    }
}
