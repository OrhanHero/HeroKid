namespace LernTor.Core.Services;

/// <summary>Was für ein Eintrag im Schulkalender steht.</summary>
public enum CalendarEntryKind
{
    /// <summary>Schulferien - mehrere Tage am Stück.</summary>
    Ferien,

    /// <summary>Gesetzlicher Feiertag in Berlin.</summary>
    Feiertag,

    /// <summary>Einzelner unterrichtsfreier Tag (Brückentag o.ä.).</summary>
    UnterrichtsfreierTag
}

/// <summary>Ein Zeitraum im Schulkalender. Ein einzelner Tag hat Start == End.</summary>
/// <param name="Start">Erster Tag, einschließlich.</param>
/// <param name="End">Letzter Tag, einschließlich.</param>
/// <param name="Name">Anzeigename, z.B. "Sommerferien".</param>
/// <param name="Kind">Ferien, Feiertag oder unterrichtsfreier Tag.</param>
public sealed record SchoolCalendarEntry(DateOnly Start, DateOnly End, string Name, CalendarEntryKind Kind)
{
    public bool Contains(DateOnly day) => day >= Start && day <= End;

    public bool IsSingleDay => Start == End;

    /// <summary>Länge in Tagen, beide Enden eingeschlossen.</summary>
    public int LengthInDays => End.DayNumber - Start.DayNumber + 1;

    /// <summary>
    /// Verbleibende Tage ab <paramref name="from"/>, den heutigen mitgezählt. Null, wenn der
    /// Zeitraum vorbei ist.
    /// </summary>
    public int RemainingDays(DateOnly from) =>
        from > End ? 0 : End.DayNumber - Math.Max(from.DayNumber, Start.DayNumber) + 1;

    /// <summary>Tage bis zum Beginn. 0, wenn er schon begonnen hat.</summary>
    public int DaysUntilStart(DateOnly from) => Math.Max(0, Start.DayNumber - from.DayNumber);

    /// <summary>
    /// Ein Feiertag, der auf Samstag oder Sonntag fällt, bringt keinen freien Tag. Das gehört
    /// dazugesagt, statt einen Tag anzukündigen, an dem ohnehin niemand Schule hat.
    /// </summary>
    public bool FallsOnWeekend =>
        IsSingleDay &&
        Start.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}

/// <summary>
/// Schulferien und gesetzliche Feiertage in Berlin.
///
/// <para><b>Fest einkompiliert, nicht aus dem Netz geholt</b> - dasselbe Muster wie bei den
/// Lerninhalten. Die App ist ein Kiosk ohne verlässliche Netzverbindung, und ein Kalender, der
/// beim Start hängt oder leer bleibt, wäre schlimmer als gar keiner. Termine ändern sich
/// höchstens einmal im Jahr; sie einzupflegen ist ein normaler Code-Review.</para>
///
/// <para><b>Die Daten enden bewusst sichtbar.</b> Statt nach dem letzten Eintrag stillschweigend
/// nichts mehr anzuzeigen, sagen <see cref="LastVacationDay"/> und <see cref="LastHolidayDay"/>,
/// wie weit der Kalender reicht - der Eltern-Bereich zeigt das an, damit rechtzeitig nachgetragen
/// wird. Ein leerer Kalender, der wie ein voller aussieht, ist die schlechtere Antwort.</para>
///
/// <para>Vergangene Termine stehen absichtlich nicht drin: der Kalender soll zeigen, was kommt.</para>
/// </summary>
public static class SchoolCalendar
{
    private static readonly SchoolCalendarEntry[] Eintraege =
    {
        // ---------- Schuljahr 2025/26 ----------
        Zeitraum(2026, 7, 9, 2026, 8, 22, "Sommerferien", CalendarEntryKind.Ferien),

        // ---------- Feiertage 2026 ----------
        Tag(2026, 10, 3, "Tag der Deutschen Einheit"),
        Tag(2026, 12, 25, "1. Weihnachtsfeiertag"),
        Tag(2026, 12, 26, "2. Weihnachtsfeiertag"),

        // ---------- Schuljahr 2026/27 ----------
        Zeitraum(2026, 10, 19, 2026, 10, 31, "Herbstferien", CalendarEntryKind.Ferien),
        Zeitraum(2026, 12, 23, 2027, 1, 2, "Weihnachtsferien", CalendarEntryKind.Ferien),
        Zeitraum(2027, 2, 1, 2027, 2, 6, "Winterferien", CalendarEntryKind.Ferien),
        Zeitraum(2027, 3, 22, 2027, 4, 2, "Osterferien", CalendarEntryKind.Ferien),
        Zeitraum(2027, 5, 7, 2027, 5, 7, "Unterrichtsfreier Tag", CalendarEntryKind.UnterrichtsfreierTag),
        Zeitraum(2027, 5, 18, 2027, 5, 19, "Pfingstferien", CalendarEntryKind.Ferien),
        Zeitraum(2027, 7, 1, 2027, 8, 14, "Sommerferien", CalendarEntryKind.Ferien),

        // ---------- Feiertage 2027 ----------
        Tag(2027, 1, 1, "Neujahr"),
        Tag(2027, 3, 8, "Internationaler Frauentag"),
        Tag(2027, 3, 26, "Karfreitag"),
        Tag(2027, 3, 29, "Ostermontag"),
        Tag(2027, 5, 1, "Tag der Arbeit"),
        Tag(2027, 5, 6, "Christi Himmelfahrt"),
        Tag(2027, 5, 17, "Pfingstmontag"),
        Tag(2027, 10, 3, "Tag der Deutschen Einheit"),
        Tag(2027, 12, 25, "1. Weihnachtsfeiertag"),
        Tag(2027, 12, 26, "2. Weihnachtsfeiertag")
    };

    private static SchoolCalendarEntry Tag(int jahr, int monat, int tag, string name) =>
        new(new DateOnly(jahr, monat, tag), new DateOnly(jahr, monat, tag), name, CalendarEntryKind.Feiertag);

    private static SchoolCalendarEntry Zeitraum(
        int vonJahr, int vonMonat, int vonTag,
        int bisJahr, int bisMonat, int bisTag,
        string name, CalendarEntryKind art) =>
        new(new DateOnly(vonJahr, vonMonat, vonTag), new DateOnly(bisJahr, bisMonat, bisTag), name, art);

    /// <summary>Alle Einträge, nach Beginn sortiert.</summary>
    public static IReadOnlyList<SchoolCalendarEntry> All { get; } =
        Eintraege.OrderBy(eintrag => eintrag.Start).ThenBy(eintrag => eintrag.End).ToList();

    /// <summary>Erster Schultag nach den Sommerferien 2026 - Beginn des Schuljahres 2026/27.</summary>
    public static DateOnly SchoolYearStart2026 { get; } = new(2026, 8, 24);

    /// <summary>Bis hierhin sind Ferien eingetragen.</summary>
    public static DateOnly LastVacationDay { get; } =
        All.Where(eintrag => eintrag.Kind != CalendarEntryKind.Feiertag).Max(eintrag => eintrag.End);

    /// <summary>Bis hierhin sind Feiertage eingetragen.</summary>
    public static DateOnly LastHolidayDay { get; } =
        All.Where(eintrag => eintrag.Kind == CalendarEntryKind.Feiertag).Max(eintrag => eintrag.End);

    /// <summary>Alle Einträge, die auf diesen Tag fallen - Feiertage können in Ferien liegen.</summary>
    public static IReadOnlyList<SchoolCalendarEntry> On(DateOnly day) =>
        All.Where(eintrag => eintrag.Contains(day)).ToList();

    /// <summary>
    /// Die Ferien, in denen dieser Tag liegt. Null, wenn Schule ist.
    ///
    /// <para>Ferien schlagen einen Feiertag: am 25.12. ist die richtige Auskunft
    /// "Weihnachtsferien", nicht "1. Weihnachtsfeiertag" - der Feiertag steht ohnehin
    /// mittendrin.</para>
    /// </summary>
    public static SchoolCalendarEntry? CurrentVacation(DateOnly day) =>
        All.FirstOrDefault(eintrag =>
            eintrag.Kind != CalendarEntryKind.Feiertag && eintrag.Contains(day));

    /// <summary>Der Feiertag an diesem Tag, falls einer ist.</summary>
    public static SchoolCalendarEntry? HolidayOn(DateOnly day) =>
        All.FirstOrDefault(eintrag =>
            eintrag.Kind == CalendarEntryKind.Feiertag && eintrag.Contains(day));

    /// <summary>Die nächsten Einträge ab diesem Tag - laufende zuerst, dann kommende.</summary>
    public static IReadOnlyList<SchoolCalendarEntry> Upcoming(DateOnly day, int count) =>
        All.Where(eintrag => eintrag.End >= day).Take(count).ToList();

    /// <summary>Die nächsten Ferien, die noch nicht begonnen haben.</summary>
    public static SchoolCalendarEntry? NextVacation(DateOnly day) =>
        All.FirstOrDefault(eintrag =>
            eintrag.Kind != CalendarEntryKind.Feiertag && eintrag.Start > day);

    /// <summary>Der nächste Feiertag, der noch nicht war.</summary>
    public static SchoolCalendarEntry? NextHoliday(DateOnly day) =>
        All.FirstOrDefault(eintrag =>
            eintrag.Kind == CalendarEntryKind.Feiertag && eintrag.Start > day);

    /// <summary>
    /// Alles, was für die Anzeige eines Tages gebraucht wird - damit die Entscheidung, WAS
    /// gezeigt wird, in Core liegt und prüfbar ist statt in einer Ansicht.
    /// </summary>
    public static CalendarToday Today(DateOnly day) => new(
        CurrentVacation(day),
        HolidayOn(day),
        NextVacation(day),
        NextHoliday(day),
        day > LastVacationDay && day > LastHolidayDay);
}

/// <summary>Der Kalenderstand eines Tages.</summary>
/// <param name="Vacation">Laufende Ferien, sonst null.</param>
/// <param name="Holiday">Feiertag heute, sonst null.</param>
/// <param name="NextVacation">Nächste Ferien, die noch nicht begonnen haben.</param>
/// <param name="NextHoliday">Nächster Feiertag.</param>
/// <param name="BeyondCoverage">Der Tag liegt hinter allen eingetragenen Terminen - dann gibt es
/// nichts anzuzeigen, und das ist ein Hinweis an die Eltern, nicht an das Kind.</param>
public readonly record struct CalendarToday(
    SchoolCalendarEntry? Vacation,
    SchoolCalendarEntry? Holiday,
    SchoolCalendarEntry? NextVacation,
    SchoolCalendarEntry? NextHoliday,
    bool BeyondCoverage)
{
    /// <summary>Heute ist schulfrei - Ferien oder Feiertag.</summary>
    public bool IsSchoolFree => Vacation is not null || Holiday is not null;

    /// <summary>
    /// Was oben in der Kachel steht.
    ///
    /// <para>Läuft heute etwas, gilt das - Ferien vor Feiertag, weil am 25.12. "Weihnachtsferien"
    /// die nützlichere Auskunft ist. Sonst kommt der Termin dran, der als NÄCHSTES eintritt, egal
    /// welcher Art: die nächsten Ferien anzuzeigen, während in drei Tagen ein Feiertag ist, wäre
    /// zwar richtig, aber nicht die Antwort auf die Frage, die ein Kind hat.</para>
    /// </summary>
    public SchoolCalendarEntry? Headline
    {
        get
        {
            if (Vacation is not null)
            {
                return Vacation;
            }

            if (Holiday is not null)
            {
                return Holiday;
            }

            if (NextVacation is null)
            {
                return NextHoliday;
            }

            return NextHoliday is not null && NextHoliday.Start < NextVacation.Start
                ? NextHoliday
                : NextVacation;
        }
    }
}
