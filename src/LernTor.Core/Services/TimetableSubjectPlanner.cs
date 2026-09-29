using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Welche Schulfächer ein Kind heute übt - abgeleitet aus seinem Stundenplan.
///
/// <para><b>Die Regel (Entscheidung der Familie, Schuljahr 2026/27):</b> geübt werden die Fächer
/// des NÄCHSTEN Schultags - Vorbereitung statt Nachbereitung. Freitags, am Wochenende und in
/// den Ferien ist das der erste Tag danach, an dem es wirklich Unterricht gibt. Dazu kommt
/// immer Türkisch (<see cref="AlwaysIncluded"/>), auch wenn es auf keinem der beiden Pläne
/// steht. Alle anderen Schulfächer fallen an diesem Tag aus.</para>
///
/// <para><b>Klausuren gehen vor:</b> ein Fach mit anstehender Klausur ist in der Woche davor
/// jeden Tag dabei, auch wenn es am nächsten Schultag nicht auf dem Plan steht.</para>
///
/// <para><b>Nur Schulfächer.</b> Lesen, Tippen, News, KI-Bereich, Führerschein, Erste Hilfe und
/// das Abschlussquiz haben mit dem Stundenplan nichts zu tun und bleiben, wie die Eltern sie
/// eingestellt haben.</para>
///
/// <para><b>NaWi</b> wird nicht zu allen drei Teilfächern auf einmal, sondern wechselt
/// reihum: am ersten NaWi-Tag Biologie, am nächsten Chemie, dann Physik. Gezählt werden dafür
/// die NaWi-Tage laut Wochenplan seit einem festen Stichtag - so liegen auch zwei NaWi-Tage in
/// Folge (Dienstag/Mittwoch) nie auf demselben Fach, und über die Wochen kommt jedes gleich
/// oft dran.</para>
///
/// <para><b>Ohne Stundenplan ändert sich nichts:</b> ist für das Kind kein Plan eingetragen,
/// oder findet sich in den nächsten Wochen kein Schultag mehr (Kalender zu Ende), liefert
/// <see cref="ScheduledSubjects"/> <c>null</c> - dann gelten alle Fächer wie bisher.</para>
/// </summary>
public static class TimetableSubjectPlanner
{
    /// <summary>Fächer, die unabhängig vom Stundenplan jeden Tag dabei sind.</summary>
    public static IReadOnlySet<Subject> AlwaysIncluded { get; } = new HashSet<Subject> { Subject.Tuerkisch };

    /// <summary>So weit wird nach dem nächsten Schultag gesucht - sechs Wochen decken auch die
    /// Sommerferien ab, wie in <see cref="TimetableToday"/>.</summary>
    private const int MaxLookaheadDays = 45;

    /// <summary>Stichtag für die NaWi-Rotation, ein Montag. Nur der Abstand zählt.</summary>
    private static readonly DateOnly RotationsStichtag = new(2024, 1, 1);

    /// <summary>
    /// Der Schultag, für den heute geübt wird: der erste Tag NACH <paramref name="today"/>, der
    /// nicht schulfrei ist und an dem im Plan Stunden stehen. <c>null</c>, wenn es keinen gibt.
    /// </summary>
    public static DateOnly? TargetDay(Timetable plan, DateOnly today) =>
        TargetDay(plan, today, SchoolCalendar.IsSchoolFree);

    /// <summary>Überladung mit eigener Schulfrei-Auskunft - für Tests, die nicht vom Berliner
    /// Ferienkalender abhängen sollen.</summary>
    public static DateOnly? TargetDay(Timetable plan, DateOnly today, Func<DateOnly, bool> isSchoolFree)
    {
        if (plan.IsEmpty)
        {
            return null;
        }

        for (var versatz = 1; versatz <= MaxLookaheadDays; versatz++)
        {
            var tag = today.AddDays(versatz);
            if (!isSchoolFree(tag) && plan.ForDay(tag.DayOfWeek).Count > 0)
            {
                return tag;
            }
        }

        return null;
    }

    /// <summary>
    /// Die Schulfächer, die heute geübt werden - oder <c>null</c>, wenn der Stundenplan nichts
    /// vorgibt (kein Plan, kein nächster Schultag). Enthält nur Schulfächer
    /// (<see cref="SchoolSubjects"/>); über alle anderen Bereiche sagt die Menge nichts.
    /// </summary>
    public static IReadOnlySet<Subject>? ScheduledSubjects(Timetable plan, DateOnly today) =>
        ScheduledSubjects(plan, today, SchoolCalendar.IsSchoolFree);

    /// <summary>Überladung mit eigener Schulfrei-Auskunft - für Tests.</summary>
    public static IReadOnlySet<Subject>? ScheduledSubjects(
        Timetable plan, DateOnly today, Func<DateOnly, bool> isSchoolFree)
    {
        var ziel = TargetDay(plan, today, isSchoolFree);
        return ziel is null ? null : SubjectsOn(plan, ziel.Value);
    }

    /// <summary>
    /// Die Schulfächer eines bestimmten Tages laut Plan, samt <see cref="AlwaysIncluded"/> und
    /// dem NaWi-Teilfach, das an diesem Tag an der Reihe ist.
    /// </summary>
    /// <param name="plan">Der Stundenplan des Kindes.</param>
    /// <param name="day">Der Schultag, für den geübt wird.</param>
    /// <param name="examSubjects">Fächer mit einer anstehenden Klausur. Sie sind IMMER dabei,
    /// auch wenn sie am Zieltag nicht auf dem Plan stehen: sonst würde die Auswahl nach
    /// Stundenplan genau die Klausurvorbereitung wegschneiden, die
    /// <c>ExamEntry.LearningWeight</c> in der Woche davor hochfährt.</param>
    public static IReadOnlySet<Subject> SubjectsOn(
        Timetable plan, DateOnly day, IEnumerable<Subject>? examSubjects = null)
    {
        var faecher = new HashSet<Subject>(AlwaysIncluded);
        faecher.UnionWith(examSubjects ?? Enumerable.Empty<Subject>());

        foreach (var stunde in plan.ForDay(day.DayOfWeek))
        {
            var fach = TimetableSubjectMap.TryMap(stunde.Subject);
            if (fach is not null)
            {
                faecher.Add(fach.Value);
                continue;
            }

            var teilfaecher = TimetableSubjectMap.CombinedSubjects(stunde.Subject);
            if (teilfaecher.Count > 0)
            {
                faecher.Add(teilfaecher[RotationIndex(plan, stunde.Subject, day, teilfaecher.Count)]);
            }
        }

        faecher.RemoveWhere(fach => !SchoolSubjects.IsSchoolSubject(fach));
        return faecher;
    }

    /// <summary>
    /// Der wievielte Tag mit diesem Sammelfach <paramref name="day"/> seit dem Stichtag ist -
    /// modulo der Zahl der Teilfächer. Gezählt wird nach dem Wochenplan (volle Wochen mal
    /// Sammelfach-Tage je Woche, plus die Sammelfach-Tage dieser Woche bis einschließlich
    /// <paramref name="day"/>), nicht nach tatsächlich gehaltenen Stunden: Ferien schieben die
    /// Reihe damit nicht durcheinander, und dasselbe Datum ergibt immer dasselbe Fach - auch nach
    /// einem Neustart der App mitten am Tag.
    /// </summary>
    private static int RotationIndex(Timetable plan, string label, DateOnly day, int anzahl)
    {
        var sammelTage = Timetable.SchoolDays
            .Where(wochentag => plan.ForDay(wochentag).Any(stunde =>
                string.Equals(stunde.Subject.Trim(), label.Trim(), StringComparison.OrdinalIgnoreCase)))
            .Select(WochentagIndex)
            .ToList();

        if (sammelTage.Count == 0)
        {
            return 0;
        }

        var tage = day.DayNumber - RotationsStichtag.DayNumber;
        var wochen = Math.Max(0, tage) / 7;
        var heuteIndex = WochentagIndex(day.DayOfWeek);
        var inDieserWoche = sammelTage.Count(index => index < heuteIndex);

        return (int)((wochen * (long)sammelTage.Count + inDieserWoche) % anzahl);
    }

    /// <summary>Montag = 0 … Sonntag = 6 - <see cref="DayOfWeek"/> fängt beim Sonntag an.</summary>
    private static int WochentagIndex(DayOfWeek tag) => ((int)tag + 6) % 7;
}
