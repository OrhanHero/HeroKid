using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Fächerauswahl nach Stundenplan: geübt wird, was am nächsten Schultag dran ist, plus Türkisch;
/// NaWi wechselt reihum. Geprüft an den echten Plänen der beiden Kinder für 2026/27 - genau
/// in der Schreibweise, in der sie im Eltern-Bereich eingefügt werden
/// (docs/STUNDENPLAENE-2026-27.md).
/// </summary>
public sealed class TimetableSubjectPlannerTests
{
    /// <summary>Klasse 9a. ".F" auf dem Untis-Plan ist die geteilte Türkisch/Französisch-Gruppe;
    /// Batuhan ist in der türkischen.</summary>
    internal const string BatuhanText = """
        Mo: 1 Ch (A301), 2 Ch (A301), 3 De (A303), 4 Ku (O003), 5 Ku (O003), 6 Türkisch, 7 Ge (A201)
        Di: 1 Türkisch, 2 Ma (A012), 3 De (A304), 4 E (A204), 5 PB (A201), 6 Wge, 7 Wge
        Mi: 1 Ph (A109), 2 Ph (A109), 3 Türkisch, 4 E (A102), 5 Ma (A012), 6 Bi (A209), 7 Bi (A209)
        Do: 1 Sp (TH1), 2 Sp (TH1), 3 Geo (A202), 4 De (A304), 5 De (A304), 6 Et (A102)
        Fr: 1 Wge, 2 Geo (A202), 3 E (A204), 4 Ge (A103), 5 Sp (TH1), 6 Ma (A012), 7 Ma (A012)
        """;

    /// <summary>Klasse 6c.</summary>
    internal const string EmirhanText = """
        Mo: 1 GeWi, 2 GeWi, 3 En, 4 En, 6 Sport, 7 Sport, 8 Orchester, 9 Orchester
        Di: 1 NaWi, 2 NaWi, 3 Ma, 4 Mu, 6 De, 7 De, 8 MUBet INS
        Mi: 1 Ku, 2 Ku, 3 Ma, 4 Mu, 6 NaWi, 7 NaWi, 8 WPU/JüM/Pop-Chor, 9 WPU/Pop-Chor
        Do: 1 Ma, 2 Ma, 3 Klassenrat, 4 GeWi, 6 Sport, 7 En, 8 Religion
        Fr: 2 En, 3 Ma, 4 De, 6 De, 7 En, 8 MUBet, 9 MUBet, 10 MUBet
        """;

    /// <summary>Montag, 28.09.2026.</summary>
    private static readonly DateOnly Montag = new(2026, 9, 28);

    private static readonly DateOnly Freitag = new(2026, 10, 2);

    /// <summary>Nur das Wochenende ist frei - unabhängig vom Berliner Ferienkalender.</summary>
    private static bool NurWochenende(DateOnly tag) =>
        tag.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private static List<Subject> Sortiert(params Subject[] faecher) => faecher.OrderBy(fach => fach).ToList();

    private static Timetable Plan(string text)
    {
        var ergebnis = TimetableTextParser.Parse(text);
        Assert.Empty(ergebnis.Warnings);
        return new Timetable(lessons: ergebnis.Lessons);
    }

    [Fact]
    public void Beide_Stundenplaene_lassen_sich_ohne_Warnung_einlesen()
    {
        Assert.Equal(34, Plan(BatuhanText).Lessons.Count);
        Assert.Equal(38, Plan(EmirhanText).Lessons.Count);
    }

    [Fact]
    public void Am_Montag_uebt_Batuhan_die_Faecher_vom_Dienstag()
    {
        var faecher = TimetableSubjectPlanner.ScheduledSubjects(Plan(BatuhanText), Montag, NurWochenende);

        Assert.NotNull(faecher);
        Assert.Equal(
            Sortiert(Subject.Tuerkisch, Subject.Mathematik, Subject.Deutsch, Subject.Englisch, Subject.Politik),
            Sortiert(faecher!.ToArray()));
    }

    [Fact]
    public void Am_Freitag_wird_fuer_Montag_geuebt()
    {
        var plan = Plan(BatuhanText);

        Assert.Equal(new DateOnly(2026, 10, 5), TimetableSubjectPlanner.TargetDay(plan, Freitag, NurWochenende));
        Assert.Equal(
            Sortiert(Subject.Chemie, Subject.Deutsch, Subject.Kunst, Subject.Tuerkisch, Subject.Geschichte),
            Sortiert(TimetableSubjectPlanner.ScheduledSubjects(plan, Freitag, NurWochenende)!.ToArray()));
    }

    [Fact]
    public void Am_Wochenende_wird_ebenfalls_fuer_Montag_geuebt()
    {
        var samstag = new DateOnly(2026, 10, 3);
        var sonntag = new DateOnly(2026, 10, 4);

        Assert.Equal(new DateOnly(2026, 10, 5), TimetableSubjectPlanner.TargetDay(Plan(BatuhanText), samstag, NurWochenende));
        Assert.Equal(new DateOnly(2026, 10, 5), TimetableSubjectPlanner.TargetDay(Plan(BatuhanText), sonntag, NurWochenende));
    }

    [Fact]
    public void Ein_schulfreier_Tag_wird_uebersprungen()
    {
        // Dienstag frei (z. B. Feiertag) - dann ist der Mittwoch der naechste Schultag.
        var dienstag = new DateOnly(2026, 9, 29);

        var ziel = TimetableSubjectPlanner.TargetDay(
            Plan(BatuhanText), Montag, tag => NurWochenende(tag) || tag == dienstag);

        Assert.Equal(new DateOnly(2026, 9, 30), ziel);
    }

    [Fact]
    public void Tuerkisch_ist_immer_dabei_auch_ohne_Tuerkischstunde()
    {
        // Emirhan hat kein Tuerkisch in der Schule - geuebt wird es trotzdem.
        var faecher = TimetableSubjectPlanner.ScheduledSubjects(Plan(EmirhanText), Montag, NurWochenende);

        Assert.NotNull(faecher);
        Assert.Contains(Subject.Tuerkisch, faecher!);
    }

    [Fact]
    public void Fuer_Emirhans_Dienstag_Mathe_Musik_Deutsch_Tuerkisch_und_ein_NaWi_Fach()
    {
        var faecher = TimetableSubjectPlanner.ScheduledSubjects(Plan(EmirhanText), Montag, NurWochenende)!;

        Assert.Contains(Subject.Mathematik, faecher);
        Assert.Contains(Subject.Musik, faecher);
        Assert.Contains(Subject.Deutsch, faecher);
        Assert.Contains(Subject.Tuerkisch, faecher);

        var nawi = faecher.Intersect(new[] { Subject.Biologie, Subject.Chemie, Subject.Physik }).ToList();
        Assert.Single(nawi);

        // Sport und MUBet sind keine LernTor-Faecher; Englisch hat Emirhan dienstags nicht.
        Assert.Equal(5, faecher.Count);
    }

    [Fact]
    public void NaWi_wechselt_reihum_und_nie_zweimal_hintereinander_dasselbe()
    {
        var plan = Plan(EmirhanText);

        // Die NaWi-Tage dreier Wochen: Dienstag und Mittwoch.
        var nawiTage = Enumerable.Range(0, 3)
            .SelectMany(woche => new[]
            {
                new DateOnly(2026, 9, 29).AddDays(7 * woche),
                new DateOnly(2026, 9, 30).AddDays(7 * woche)
            })
            .ToList();

        var faecher = nawiTage
            .Select(tag => TimetableSubjectPlanner.SubjectsOn(plan, tag)
                .Single(fach => fach is Subject.Biologie or Subject.Chemie or Subject.Physik))
            .ToList();

        for (var i = 1; i < faecher.Count; i++)
        {
            Assert.NotEqual(faecher[i - 1], faecher[i]);
        }

        Assert.Equal(2, faecher.Count(fach => fach == Subject.Biologie));
        Assert.Equal(2, faecher.Count(fach => fach == Subject.Chemie));
        Assert.Equal(2, faecher.Count(fach => fach == Subject.Physik));
    }

    [Fact]
    public void Dasselbe_Datum_ergibt_immer_dasselbe_NaWi_Fach()
    {
        // Ein Neustart der App mitten am Tag darf das Fach nicht wechseln.
        var plan = Plan(EmirhanText);
        var dienstag = new DateOnly(2026, 9, 29);

        Assert.Equal(
            Sortiert(TimetableSubjectPlanner.SubjectsOn(plan, dienstag).ToArray()),
            Sortiert(TimetableSubjectPlanner.SubjectsOn(plan, dienstag).ToArray()));
    }

    [Fact]
    public void Ein_Klausurfach_ist_dabei_auch_wenn_es_nicht_auf_dem_Plan_steht()
    {
        var dienstag = new DateOnly(2026, 9, 29);

        var faecher = TimetableSubjectPlanner.SubjectsOn(Plan(BatuhanText), dienstag, new[] { Subject.Physik });

        Assert.Contains(Subject.Physik, faecher);
    }

    [Fact]
    public void Ohne_Stundenplan_gibt_es_keine_Vorgabe()
    {
        Assert.Null(TimetableSubjectPlanner.ScheduledSubjects(Timetable.Empty, Montag, NurWochenende));
        Assert.Null(TimetableSubjectPlanner.TargetDay(Timetable.Empty, Montag, NurWochenende));
    }

    [Fact]
    public void Ohne_Schultag_in_Sicht_gibt_es_keine_Vorgabe()
    {
        // Kalender zu Ende: alles schulfrei - dann gelten wieder alle Faecher, statt dass nur
        // noch Tuerkisch uebrig bleibt.
        Assert.Null(TimetableSubjectPlanner.ScheduledSubjects(Plan(BatuhanText), Montag, _tag => true));
    }

    [Fact]
    public void Die_Auswahl_enthaelt_nur_Schulfaecher()
    {
        var faecher = TimetableSubjectPlanner.SubjectsOn(
            Plan(BatuhanText), new DateOnly(2026, 9, 29), new[] { Subject.Fuehrerschein });

        Assert.All(faecher, fach => Assert.True(SchoolSubjects.IsSchoolSubject(fach)));
    }

    [Fact]
    public void Nicht_geplante_Schulfaecher_fallen_aus_die_eigenen_Bereiche_nicht()
    {
        var geplant = new HashSet<Subject> { Subject.Mathematik, Subject.Tuerkisch };

        var aus = SubjectAvailability.EffectiveDisabled(new HashSet<Subject>(), true, true, geplant);

        Assert.DoesNotContain(Subject.Mathematik, aus);
        Assert.DoesNotContain(Subject.Tuerkisch, aus);
        Assert.Contains(Subject.Physik, aus);
        Assert.Contains(Subject.Musik, aus);

        // Tippen, News, KI, Fuehrerschein, Erste Hilfe haben mit dem Stundenplan nichts zu tun.
        Assert.All(SchoolSubjects.NonSchool, bereich => Assert.DoesNotContain(bereich, aus));
    }

    [Fact]
    public void Ohne_Vorgabe_bleibt_alles_wie_bisher()
    {
        var mitNull = SubjectAvailability.EffectiveDisabled(new HashSet<Subject>(), true, true, null);
        var ohne = SubjectAvailability.EffectiveDisabled(new HashSet<Subject>(), true, true);

        Assert.Empty(mitNull);
        Assert.Empty(ohne);
    }

    [Fact]
    public void Der_globale_Schalter_gewinnt_auch_gegen_den_Stundenplan()
    {
        var global = new HashSet<Subject> { Subject.Tuerkisch };
        var geplant = new HashSet<Subject> { Subject.Tuerkisch, Subject.Mathematik };

        Assert.True(SubjectAvailability.IsDisabled(Subject.Tuerkisch, global, true, true, geplant));
    }
}
