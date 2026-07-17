namespace LernTor.Core.Services;

/// <summary>
/// Berechnet die 🔥-Lernserie (aufeinanderfolgende Lerntage) aus den Kalendertagen, an denen ein
/// Profil mindestens eine Aufgabe beantwortet hat (ActivityLogRepository.GetLearningDaysAsync).
/// Bewusst OHNE Druckmechanik (siehe Design-Entscheidung im Status-Report): die Serie wird nur
/// ANGEZEIGT (und nur wenn Eltern sie per <c>AppSettings.StreaksEnabled</c> eingeschaltet haben,
/// Standard aus) - es gibt keine Strafen, keine Erinnerungen und keinen Verlust von Belohnungen
/// bei verpassten Tagen, anders als z.B. bei Duolingo-Streaks.
/// </summary>
public static class StreakCalculator
{
    /// <summary>
    /// Anzahl der aufeinanderfolgenden Lerntage bis heute. Der HEUTIGE Tag zählt mit, sobald an
    /// ihm gelernt wurde - fehlt er noch (die heutige Session läuft ja evtl. gerade erst an),
    /// bricht das die Serie NICHT, sondern es wird ab gestern rückwärts gezählt. Erst ein ganzer
    /// verpasster Kalendertag setzt die Serie auf 0 zurück.
    /// </summary>
    public static int CurrentStreak(IEnumerable<DateOnly> learningDays, DateOnly today)
    {
        var days = learningDays as IReadOnlySet<DateOnly> ?? learningDays.ToHashSet();

        var start = days.Contains(today) ? today : today.AddDays(-1);
        var streak = 0;
        for (var day = start; days.Contains(day); day = day.AddDays(-1))
        {
            streak++;
        }

        return streak;
    }
}
