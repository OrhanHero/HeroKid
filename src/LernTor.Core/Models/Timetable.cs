using LernTor.Core.Services;

namespace LernTor.Core.Models;

/// <summary>
/// Eine Schulstunde im Stundenplan eines Kindes.
///
/// <para><b>Das Fach steht als freier Text drin, nicht als <c>Subject</c>-Enum.</b> Auf einem
/// echten Stundenplan stehen NaWi, GeWi, Sport, WPU Spanisch, Klassenrat, JÜM - Dinge, die
/// LernTor als Lernbereich gar nicht kennt und auch nicht kennen soll. Ein Enum würde all das
/// entweder verschlucken oder auf ein falsches Fach abbilden. Angezeigt wird deshalb immer
/// genau das, was auf dem Plan der Schule steht; die Zuordnung zu einem LernTor-Fach
/// (<see cref="TimetableSubjectMap"/>) dient nur dem Symbol davor und darf danebenliegen, ohne
/// dass etwas kaputtgeht.</para>
/// </summary>
/// <param name="Day">Wochentag - in der Praxis Montag bis Freitag.</param>
/// <param name="Period">Stunde, 1-basiert wie auf dem Plan.</param>
/// <param name="Subject">Fach, so wie es auf dem Stundenplan steht.</param>
/// <param name="Teacher">Kürzel der Lehrkraft, falls auf dem Plan vermerkt.</param>
/// <param name="Room">Raum, falls auf dem Plan vermerkt.</param>
public sealed record TimetableLesson(
    DayOfWeek Day,
    int Period,
    string Subject,
    string? Teacher = null,
    string? Room = null)
{
    /// <summary>"A204" bzw. "Doh · A204" - was neben dem Fach in kleiner Schrift steht.</summary>
    public string Details
    {
        get
        {
            var teilstuecke = new List<string>(2);
            if (!string.IsNullOrWhiteSpace(Teacher))
            {
                teilstuecke.Add(Teacher!.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Room))
            {
                teilstuecke.Add(Room!.Trim());
            }

            return string.Join(" · ", teilstuecke);
        }
    }
}

/// <summary>
/// Eine Stunde im Zeitraster einer Schule: wann sie beginnt und wann sie endet.
///
/// <para>Das Raster gehört zum PROFIL, nicht zur App: die beiden Kinder gehen auf verschiedene
/// Schulen und haben verschiedene Anfangs- und Endzeiten. Ein fest eingebautes Raster hätte
/// bei einem der beiden immer danebengelegen.</para>
/// </summary>
/// <param name="Number">Stundennummer, 1-basiert.</param>
/// <param name="Start">Beginn.</param>
/// <param name="End">Ende.</param>
public sealed record TimetablePeriod(int Number, TimeOnly Start, TimeOnly End)
{
    /// <summary>"8:00 – 8:45".</summary>
    public string Range => $"{Start:HH\\:mm} – {End:HH\\:mm}";

    /// <summary>Läuft diese Stunde gerade? Der Endzeitpunkt zählt nicht mehr dazu.</summary>
    public bool Contains(TimeOnly time) => time >= Start && time < End;
}

/// <summary>
/// Der Stundenplan eines Kindes: das Zeitraster seiner Schule und die eingetragenen Stunden.
///
/// <para>Rein lokal und von Hand gepflegt (Eltern-Bereich). Es gibt bewusst KEINEN Importeur,
/// der PDF-Stundenpläne selbst zu deuten versucht: die Pläne der beiden Schulen sehen völlig
/// verschieden aus - der eine ein Untis-Ausdruck als Scan mit OCR-Text, der andere eine
/// Word-Tabelle - und ein Stundenplan, der still eine falsche Stunde anzeigt, ist schlechter
/// als gar keiner. Eingetragen wird im Raster, angezeigt wird, was eingetragen wurde.</para>
/// </summary>
public sealed class Timetable
{
    /// <summary>Montag bis Freitag - die Tage, für die ein Plan angelegt wird.</summary>
    public static readonly IReadOnlyList<DayOfWeek> SchoolDays = new[]
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
    };

    /// <summary>Höchste Stundennummer, die das Raster annehmen darf. Zehn Stunden decken auch
    /// einen langen Ganztag ab; darüber hinaus ist eher ein Tippfehler als ein Schultag.</summary>
    public const int MaxPeriod = 10;

    /// <summary>
    /// Ein Ausgangsraster für ein neu angelegtes Profil - bewusst nur ein Startwert, den die
    /// Eltern überschreiben. Es orientiert sich an einem üblichen Berliner 45-Minuten-Takt mit
    /// zwei großen Pausen; die tatsächlichen Zeiten stehen auf dem Plan der jeweiligen Schule.
    /// </summary>
    public static IReadOnlyList<TimetablePeriod> DefaultPeriods { get; } = new[]
    {
        Raster(1, 8, 0, 8, 45),
        Raster(2, 8, 50, 9, 35),
        Raster(3, 9, 55, 10, 40),
        Raster(4, 10, 45, 11, 30),
        Raster(5, 11, 50, 12, 35),
        Raster(6, 12, 40, 13, 25),
        Raster(7, 13, 45, 14, 30),
        Raster(8, 14, 35, 15, 20),
        Raster(9, 15, 25, 16, 10),
        Raster(10, 16, 15, 17, 0)
    };

    private static TimetablePeriod Raster(int nummer, int vonStunde, int vonMinute, int bisStunde, int bisMinute) =>
        new(nummer, new TimeOnly(vonStunde, vonMinute), new TimeOnly(bisStunde, bisMinute));

    public Timetable(IEnumerable<TimetablePeriod>? periods = null, IEnumerable<TimetableLesson>? lessons = null)
    {
        Periods = (periods ?? DefaultPeriods)
            .Where(stunde => stunde.Number >= 1 && stunde.Number <= MaxPeriod)
            .GroupBy(stunde => stunde.Number)
            .Select(gruppe => gruppe.First())
            .OrderBy(stunde => stunde.Number)
            .ToList();

        // Je Tag und Stunde bleibt genau ein Eintrag stehen - so sieht das Eingaberaster im
        // Eltern-Bereich aus, und so wird es angezeigt. Steht auf dem Plan der Schule in einer
        // Stunde ein geteilter Kurs, gehoert er als "F / T" in dieselbe Zelle; zwei
        // uebereinanderliegende Eintraege haetten auf der Startseite nur verwirrt.
        Lessons = (lessons ?? Enumerable.Empty<TimetableLesson>())
            .Where(stunde => stunde.Period >= 1 && stunde.Period <= MaxPeriod)
            .Where(stunde => !string.IsNullOrWhiteSpace(stunde.Subject))
            .GroupBy(stunde => (stunde.Day, stunde.Period))
            .Select(gruppe => gruppe.Last())
            .OrderBy(stunde => (int)stunde.Day)
            .ThenBy(stunde => stunde.Period)
            .ToList();
    }

    /// <summary>Ein leerer Plan mit dem Ausgangsraster - für Profile, bei denen noch nichts
    /// eingetragen ist.</summary>
    public static Timetable Empty { get; } = new();

    public IReadOnlyList<TimetablePeriod> Periods { get; }

    public IReadOnlyList<TimetableLesson> Lessons { get; }

    /// <summary>Kein Eintrag - dann bleibt die Kachel auf der Startseite weg, statt leer
    /// dazustehen.</summary>
    public bool IsEmpty => Lessons.Count == 0;

    public IReadOnlyList<TimetableLesson> ForDay(DayOfWeek day) =>
        Lessons.Where(stunde => stunde.Day == day).OrderBy(stunde => stunde.Period).ToList();

    public TimetablePeriod? PeriodOf(int number) =>
        Periods.FirstOrDefault(stunde => stunde.Number == number);

    /// <summary>Die höchste belegte Stunde - so viele Zeilen braucht das Eingaberaster
    /// mindestens.</summary>
    public int LastUsedPeriod => Lessons.Count == 0 ? 0 : Lessons.Max(stunde => stunde.Period);
}

/// <summary>
/// Welcher Schultag dem Kind gerade gezeigt wird, und wo an diesem Tag gerade die Uhr steht.
/// </summary>
/// <param name="Date">Das Datum des angezeigten Tages.</param>
/// <param name="IsToday">Ist das der heutige Tag? Sonst der nächste Schultag.</param>
/// <param name="Lessons">Die Stunden dieses Tages, aufsteigend.</param>
/// <param name="CurrentPeriod">Stunde, die gerade läuft - nur an einem heutigen Schultag.</param>
/// <param name="NextPeriod">Nächste Stunde des heutigen Tages, die noch kommt.</param>
public readonly record struct TimetableDay(
    DateOnly Date,
    bool IsToday,
    IReadOnlyList<TimetableLesson> Lessons,
    int? CurrentPeriod,
    int? NextPeriod)
{
    public DayOfWeek Day => Date.DayOfWeek;

    public bool HasLessons => Lessons.Count > 0;

    /// <summary>
    /// Wie viele Tage der angezeigte Tag entfernt ist. Mitten in den Sommerferien liegt der
    /// nächste Schultag über zwei Wochen weg - dann reicht "Montag" als Auskunft nicht, weil
    /// jeder erst einmal an übermorgen denkt.
    /// </summary>
    public int DaysAhead(DateOnly from) => Math.Max(0, Date.DayNumber - from.DayNumber);
}

/// <summary>
/// Entscheidet, WELCHER Tag des Stundenplans auf der Startseite steht. Bewusst in Core und nicht
/// in einer Ansicht: die Regel ist prüfbar, und sie ist es wert, geprüft zu werden.
///
/// <para>Nach Schulschluss, am Wochenende, an Feiertagen und in den Ferien ist der heutige Plan
/// nicht die Antwort auf die Frage, die ein Kind hat. Gezeigt wird dann der nächste Tag, an dem
/// es wirklich Unterricht gibt - liegt der hinter den Ferien, eben erst danach.</para>
/// </summary>
public static class TimetableToday
{
    /// <summary>So weit wird nach einem nächsten Schultag gesucht. Sechs Wochen decken auch die
    /// Sommerferien ab; findet sich bis dahin nichts, ist der Kalender zu Ende und die Kachel
    /// bleibt weg.</summary>
    private const int MaxLookaheadDays = 45;

    public static TimetableDay Resolve(Timetable plan, DateTime now) =>
        Resolve(plan, now, IstSchulfrei);

    /// <summary>Überladung mit eigener Schulfrei-Auskunft - für Tests, die nicht vom Berliner
    /// Ferienkalender abhängen sollen.</summary>
    public static TimetableDay Resolve(Timetable plan, DateTime now, Func<DateOnly, bool> isSchoolFree)
    {
        var heute = DateOnly.FromDateTime(now);
        var jetzt = TimeOnly.FromDateTime(now);

        // Heute, aber nur solange auch noch etwas kommt: nach der letzten Stunde interessiert der
        // heutige Plan niemanden mehr - dann steht schon morgen an.
        if (!isSchoolFree(heute))
        {
            var heutigeStunden = plan.ForDay(heute.DayOfWeek);
            if (heutigeStunden.Count > 0 && !SchultagVorbei(plan, heutigeStunden, jetzt))
            {
                return new TimetableDay(
                    heute,
                    true,
                    heutigeStunden,
                    LaufendeStunde(plan, heutigeStunden, jetzt),
                    NaechsteStunde(plan, heutigeStunden, jetzt));
            }
        }

        for (var versatz = 1; versatz <= MaxLookaheadDays; versatz++)
        {
            var tag = heute.AddDays(versatz);
            if (isSchoolFree(tag))
            {
                continue;
            }

            var stunden = plan.ForDay(tag.DayOfWeek);
            if (stunden.Count > 0)
            {
                return new TimetableDay(tag, false, stunden, null, null);
            }
        }

        return new TimetableDay(heute, true, Array.Empty<TimetableLesson>(), null, null);
    }

    /// <summary>Wochenende, Feiertag oder Ferien - an all dem findet kein Unterricht statt.</summary>
    private static bool IstSchulfrei(DateOnly tag) =>
        tag.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ||
        SchoolCalendar.CurrentVacation(tag) is not null ||
        SchoolCalendar.HolidayOn(tag) is not null;

    /// <summary>
    /// Ist der Schultag schon herum? Nur dann, wenn wir das auch WISSEN: steht zu keiner Stunde
    /// des Tages eine Uhrzeit im Raster, gilt der Tag als laufend.
    ///
    /// <para>Andernfalls wäre ein Plan ohne eingetragene Zeiten dauerhaft unsichtbar - die
    /// Startseite hätte auf einen Stundenplan, der ordentlich dasteht, immer den Tag danach
    /// gezeigt.</para>
    /// </summary>
    private static bool SchultagVorbei(Timetable plan, IReadOnlyList<TimetableLesson> stunden, TimeOnly jetzt)
    {
        var enden = stunden
            .Select(stunde => plan.PeriodOf(stunde.Period))
            .Where(raster => raster is not null)
            .Select(raster => raster!.End)
            .ToList();

        return enden.Count > 0 && jetzt >= enden.Max();
    }

    private static int? LaufendeStunde(Timetable plan, IReadOnlyList<TimetableLesson> stunden, TimeOnly jetzt) =>
        stunden
            .Select(stunde => plan.PeriodOf(stunde.Period))
            .FirstOrDefault(raster => raster is not null && raster.Contains(jetzt))
            ?.Number;

    private static int? NaechsteStunde(Timetable plan, IReadOnlyList<TimetableLesson> stunden, TimeOnly jetzt) =>
        stunden
            .Select(stunde => plan.PeriodOf(stunde.Period))
            .Where(raster => raster is not null && raster.Start > jetzt)
            .OrderBy(raster => raster!.Start)
            .FirstOrDefault()
            ?.Number;
}
