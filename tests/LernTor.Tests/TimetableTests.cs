using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Das Stundenplan-Modell und vor allem die Frage, WELCHER Tag auf der Startseite steht.
/// Die Regel dahinter ist die eigentliche Arbeit: nach der letzten Stunde, am Wochenende, an
/// Feiertagen und in den Ferien ist der heutige Plan nicht die Antwort auf die Frage, die ein
/// Kind hat.
/// </summary>
public sealed class TimetableTests
{
    /// <summary>Ein kleines Raster, das sich leicht im Kopf nachrechnen lässt.</summary>
    private static readonly TimetablePeriod[] Raster =
    {
        new(1, new TimeOnly(8, 0), new TimeOnly(8, 45)),
        new(2, new TimeOnly(8, 50), new TimeOnly(9, 35)),
        new(3, new TimeOnly(9, 55), new TimeOnly(10, 40))
    };

    private static Timetable Plan(params TimetableLesson[] stunden) => new(Raster, stunden);

    private static Timetable Wochenplan() => Plan(
        new TimetableLesson(DayOfWeek.Monday, 1, "Deutsch"),
        new TimetableLesson(DayOfWeek.Monday, 2, "Mathe"),
        new TimetableLesson(DayOfWeek.Monday, 3, "Sport"),
        new TimetableLesson(DayOfWeek.Tuesday, 1, "Englisch"),
        new TimetableLesson(DayOfWeek.Friday, 2, "Musik"));

    [Fact]
    public void Ein_leerer_Plan_bleibt_leer()
    {
        Assert.True(Timetable.Empty.IsEmpty);
        Assert.Empty(Timetable.Empty.Lessons);

        // Das Ausgangsraster steht trotzdem zur Verfuegung - sonst muesste der Eltern-Bereich bei
        // zehn leeren Zeitfeldern anfangen.
        Assert.NotEmpty(Timetable.Empty.Periods);
    }

    [Fact]
    public void Stunden_ohne_Fach_und_ausserhalb_des_Rasters_fallen_weg()
    {
        var plan = Plan(
            new TimetableLesson(DayOfWeek.Monday, 1, "Mathe"),
            new TimetableLesson(DayOfWeek.Monday, 2, "   "),
            new TimetableLesson(DayOfWeek.Monday, 0, "Zu früh"),
            new TimetableLesson(DayOfWeek.Monday, Timetable.MaxPeriod + 1, "Zu spät"));

        Assert.Single(plan.Lessons);
        Assert.Equal("Mathe", plan.Lessons[0].Subject);
    }

    [Fact]
    public void Je_Tag_und_Stunde_bleibt_ein_Eintrag_stehen()
    {
        // Der zuletzt genannte gewinnt: so wirkt ein nachgetragener Korrektur-Eintrag, statt
        // wirkungslos hinter dem alten zu verschwinden.
        var plan = Plan(
            new TimetableLesson(DayOfWeek.Monday, 1, "Falsch"),
            new TimetableLesson(DayOfWeek.Monday, 1, "Richtig"));

        Assert.Single(plan.Lessons);
        Assert.Equal("Richtig", plan.Lessons[0].Subject);
    }

    [Fact]
    public void Lehrkraft_und_Raum_stehen_zusammen_in_einer_Zeile()
    {
        Assert.Equal("Doh · A204", new TimetableLesson(DayOfWeek.Monday, 1, "E", "Doh", "A204").Details);
        Assert.Equal("A204", new TimetableLesson(DayOfWeek.Monday, 1, "E", null, "A204").Details);
        Assert.Equal(string.Empty, new TimetableLesson(DayOfWeek.Monday, 1, "E").Details);
    }

    [Fact]
    public void Das_Zeitraster_sagt_welche_Stunde_gerade_laeuft()
    {
        var stunde = Raster[0];

        Assert.True(stunde.Contains(new TimeOnly(8, 0)));
        Assert.True(stunde.Contains(new TimeOnly(8, 44)));
        // Der Endzeitpunkt gehoert nicht mehr dazu, sonst laufen zwei Stunden zugleich.
        Assert.False(stunde.Contains(new TimeOnly(8, 45)));
        Assert.False(stunde.Contains(new TimeOnly(7, 59)));
    }

    [Fact]
    public void Waehrend_der_zweiten_Stunde_ist_die_zweite_dran_und_die_dritte_als_naechstes()
    {
        // Montag, 14.09.2026, 9:00 Uhr - mitten in der 2. Stunde.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 9, 14, 9, 0, 0));

        Assert.True(stand.IsToday);
        Assert.Equal(DayOfWeek.Monday, stand.Day);
        Assert.Equal(3, stand.Lessons.Count);
        Assert.Equal(2, stand.CurrentPeriod);
        Assert.Equal(3, stand.NextPeriod);
    }

    [Fact]
    public void In_der_Pause_laeuft_keine_Stunde_aber_die_naechste_steht_an()
    {
        // 9:45 Uhr liegt zwischen der 2. (bis 9:35) und der 3. Stunde (ab 9:55).
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 9, 14, 9, 45, 0));

        Assert.True(stand.IsToday);
        Assert.Null(stand.CurrentPeriod);
        Assert.Equal(3, stand.NextPeriod);
    }

    [Fact]
    public void Nach_der_letzten_Stunde_steht_schon_der_naechste_Tag_da()
    {
        // Montag 16:00 Uhr - der heutige Plan interessiert niemanden mehr.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 9, 14, 16, 0, 0));

        Assert.False(stand.IsToday);
        Assert.Equal(DayOfWeek.Tuesday, stand.Day);
        Assert.Single(stand.Lessons);
        Assert.Equal("Englisch", stand.Lessons[0].Subject);
        // An einem Tag, der nicht heute ist, gibt es kein "laeuft gerade".
        Assert.Null(stand.CurrentPeriod);
        Assert.Null(stand.NextPeriod);
    }

    [Fact]
    public void Ein_Tag_ohne_Stunden_wird_uebersprungen()
    {
        // Dienstag nach Schulschluss: Mittwoch und Donnerstag sind im Plan leer, also Freitag.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 9, 15, 18, 0, 0));

        Assert.False(stand.IsToday);
        Assert.Equal(DayOfWeek.Friday, stand.Day);
        Assert.Equal("Musik", stand.Lessons[0].Subject);
    }

    [Fact]
    public void Am_Wochenende_steht_der_Montag_da()
    {
        // Samstag, 19.09.2026.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 9, 19, 11, 0, 0));

        Assert.False(stand.IsToday);
        Assert.Equal(DayOfWeek.Monday, stand.Day);
    }

    [Fact]
    public void Ein_Feiertag_wird_uebersprungen_wie_ein_Wochenendtag()
    {
        // Freitag, 02.10.2026, nach Schulschluss. Samstag ist der Tag der Deutschen Einheit,
        // Sonntag Wochenende - der naechste Unterrichtstag mit Stunden ist Montag, der 05.10.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 10, 2, 17, 0, 0));

        Assert.False(stand.IsToday);
        Assert.Equal(DayOfWeek.Monday, stand.Day);
    }

    [Fact]
    public void In_den_Ferien_zeigt_der_Plan_den_ersten_Schultag_danach()
    {
        // Mitten in den Herbstferien 2026 (19.10.-31.10.). Der 31.10. ist ein Samstag, der 01.11.
        // ein Sonntag - der erste Unterrichtstag danach ist Montag, der 02.11.
        var stand = TimetableToday.Resolve(Wochenplan(), new DateTime(2026, 10, 21, 10, 0, 0));

        Assert.False(stand.IsToday);
        Assert.Equal(DayOfWeek.Monday, stand.Day);
        Assert.Equal(3, stand.Lessons.Count);
    }

    [Fact]
    public void Der_angezeigte_Tag_kennt_sein_Datum_und_seinen_Abstand()
    {
        // Am 07.08.2026 laufen die Sommerferien (09.07.-22.08.). Der erste Schultag danach ist
        // Montag, der 24.08.2026 - siebzehn Tage entfernt. Genau deshalb steht das Datum in der
        // Ueberschrift: "Nächster Schultag · Montag" allein liest jeder als "übermorgen".
        var heute = new DateOnly(2026, 8, 7);
        var stand = TimetableToday.Resolve(Wochenplan(), heute.ToDateTime(new TimeOnly(11, 25)));

        Assert.False(stand.IsToday);
        Assert.Equal(new DateOnly(2026, 8, 24), stand.Date);
        Assert.Equal(DayOfWeek.Monday, stand.Day);
        Assert.Equal(17, stand.DaysAhead(heute));
    }

    [Fact]
    public void Der_Abstand_eines_heutigen_Tages_ist_null()
    {
        var heute = new DateOnly(2026, 9, 14);
        var stand = TimetableToday.Resolve(Wochenplan(), heute.ToDateTime(new TimeOnly(9, 0)));

        Assert.True(stand.IsToday);
        Assert.Equal(heute, stand.Date);
        Assert.Equal(0, stand.DaysAhead(heute));
    }

    [Fact]
    public void Ohne_jede_eingetragene_Stunde_gibt_es_nichts_zu_zeigen()
    {
        var stand = TimetableToday.Resolve(Timetable.Empty, new DateTime(2026, 9, 14, 9, 0, 0));

        Assert.False(stand.HasLessons);
    }

    [Fact]
    public void Eine_Stunde_ohne_Raster_Eintrag_zaehlt_trotzdem_zum_Tag()
    {
        // Die 5. Stunde steht im Plan, im (kleinen) Raster aber nicht. Sie darf deshalb nicht
        // verschwinden - sie bekommt nur keine Uhrzeit angezeigt.
        var plan = Plan(new TimetableLesson(DayOfWeek.Monday, 5, "Sport"));
        var stand = TimetableToday.Resolve(plan, new DateTime(2026, 9, 14, 7, 0, 0));

        Assert.True(stand.IsToday);
        Assert.Single(stand.Lessons);
        Assert.Null(plan.PeriodOf(5));
    }
}
