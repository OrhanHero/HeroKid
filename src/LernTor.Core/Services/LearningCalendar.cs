namespace LernTor.Core.Services;

/// <summary>Wie ein Tag im Lernkalender erscheint.</summary>
public enum LearningDayKind
{
    /// <summary>Mindestens eine Antwort an diesem Tag - auch an einem schulfreien Tag.</summary>
    Gelernt,

    /// <summary>Ein Schultag ohne Antwort.</summary>
    Schultag,

    /// <summary>Wochenende, Feiertag oder Ferien ohne Antwort - kein „verpasster“ Tag.</summary>
    Schulfrei,

    /// <summary>Liegt noch vor uns.</summary>
    Zukunft
}

public sealed record LearningCalendarDay(DateOnly Date, LearningDayKind Kind, bool IsToday);

/// <summary>Eine Woche von Montag bis Sonntag.</summary>
public sealed record LearningCalendarWeek(DateOnly Monday, IReadOnlyList<LearningCalendarDay> Days);

/// <summary>
/// „📅 Deine Lerntage“ in „Mein Fortschritt“ (docs/NAECHSTES-LEVEL-3-1.md, Schritt 2): die
/// letzten Wochen als Kalender, dazu zwei Zahlen.
///
/// <para><b>Bewusst keine Serie.</b> Eine Serie („12 Tage am Stück!“) reißt an einem einzigen
/// verpassten Tag und bestraft damit genau das Kind, das ohnehin einen schlechten Tag hatte.
/// Gezählt werden Tage, die bleiben - dieselbe Regel wie bei den Abzeichen: nichts verfällt.
/// Schulfreie Tage sind hell statt „leer“, sie gelten nicht als verpasst.</para>
///
/// <para>Ein Lerntag ist ein Kalendertag (Ortszeit) mit mindestens einer Antwort - genau wie
/// bei den Abzeichen „Zehn/Fünfzig/Hundert Lerntage“ (<see cref="AchievementCatalog.FromAnswers"/>),
/// damit Kalender und Abzeichen nie verschiedene Zahlen nennen.</para>
/// </summary>
public sealed record LearningCalendar(
    IReadOnlyList<LearningCalendarWeek> Weeks,
    int TotalLearningDays,
    int LearningDaysLastFourWeeks)
{
    public const int DefaultWeeks = 12;

    /// <summary>Die Lerntage aus den Antworten (Ortszeit), wie bei den Abzeichen.</summary>
    public static IReadOnlySet<DateOnly> LearnedDays(IEnumerable<MasteryAnswer> answers) =>
        answers.Select(antwort => DateOnly.FromDateTime(antwort.Timestamp.LocalDateTime)).ToHashSet();

    /// <summary>
    /// Baut den Kalender: <paramref name="weeks"/> Wochen, die letzte ist die von
    /// <paramref name="today"/>. <paramref name="isSchoolFree"/> ist austauschbar für Tests;
    /// ohne Angabe gilt <see cref="SchoolCalendar.IsSchoolFree"/> (Wochenende, Feiertage, Ferien).
    /// </summary>
    public static LearningCalendar Build(
        IReadOnlySet<DateOnly> learnedDays,
        DateOnly today,
        int weeks = DefaultWeeks,
        Func<DateOnly, bool>? isSchoolFree = null)
    {
        ArgumentNullException.ThrowIfNull(learnedDays);
        if (weeks < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(weeks), weeks, "Mindestens eine Woche.");
        }

        var schulfrei = isSchoolFree ?? SchoolCalendar.IsSchoolFree;

        // Montag der aktuellen Woche (DayOfWeek.Sunday = 0, deshalb +6 % 7).
        var diesenMontag = today.AddDays(-(((int)today.DayOfWeek + 6) % 7));
        var ersterMontag = diesenMontag.AddDays(-7 * (weeks - 1));

        var wochen = new List<LearningCalendarWeek>(weeks);
        for (var w = 0; w < weeks; w++)
        {
            var montag = ersterMontag.AddDays(7 * w);
            var tage = new List<LearningCalendarDay>(7);
            for (var d = 0; d < 7; d++)
            {
                var tag = montag.AddDays(d);
                var art = tag > today ? LearningDayKind.Zukunft
                    : learnedDays.Contains(tag) ? LearningDayKind.Gelernt
                    : schulfrei(tag) ? LearningDayKind.Schulfrei
                    : LearningDayKind.Schultag;
                tage.Add(new LearningCalendarDay(tag, art, tag == today));
            }

            wochen.Add(new LearningCalendarWeek(montag, tage));
        }

        var vierWochenBeginn = today.AddDays(-27);
        return new LearningCalendar(
            wochen,
            learnedDays.Count(tag => tag <= today),
            learnedDays.Count(tag => tag >= vierWochenBeginn && tag <= today));
    }
}
