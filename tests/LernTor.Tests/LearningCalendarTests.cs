using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>„📅 Deine Lerntage“ (docs/NAECHSTES-LEVEL-3-1.md, Schritt 2).</summary>
public sealed class LearningCalendarTests
{
    // Mittwoch, 30.09.2026. Nur Wochenenden gelten in den meisten Tests als schulfrei, damit die
    // Erwartungen nicht am Berliner Ferienkalender hängen.
    private static readonly DateOnly Heute = new(2026, 9, 30);

    private static bool NurWochenende(DateOnly tag) => tag.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    [Fact]
    public void Zwoelf_Wochen_von_Montag_bis_Sonntag_die_letzte_ist_diese()
    {
        var kalender = LearningCalendar.Build(new HashSet<DateOnly>(), Heute, isSchoolFree: NurWochenende);

        Assert.Equal(12, kalender.Weeks.Count);
        Assert.All(kalender.Weeks, woche =>
        {
            Assert.Equal(DayOfWeek.Monday, woche.Monday.DayOfWeek);
            Assert.Equal(7, woche.Days.Count);
        });
        Assert.Equal(new DateOnly(2026, 9, 28), kalender.Weeks[^1].Monday);
        Assert.Equal(new DateOnly(2026, 7, 13), kalender.Weeks[0].Monday);
    }

    [Fact]
    public void Heute_ist_markiert_und_danach_ist_Zukunft()
    {
        var woche = LearningCalendar.Build(new HashSet<DateOnly>(), Heute, isSchoolFree: NurWochenende).Weeks[^1];

        Assert.Single(woche.Days, tag => tag.IsToday);
        Assert.True(woche.Days[2].IsToday);                                  // Mittwoch
        Assert.All(woche.Days.Skip(3), tag => Assert.Equal(LearningDayKind.Zukunft, tag.Kind));
    }

    [Fact]
    public void Gelernt_geht_vor_schulfrei_und_ein_freier_Tag_ist_nicht_verpasst()
    {
        var gelernt = new HashSet<DateOnly> { new(2026, 9, 26), new(2026, 9, 29) }; // Samstag, Dienstag
        var woche = LearningCalendar.Build(gelernt, Heute, isSchoolFree: NurWochenende).Weeks[^2];

        Assert.Equal(LearningDayKind.Gelernt, woche.Days[5].Kind);    // Sa 26.09., gelernt
        Assert.Equal(LearningDayKind.Schulfrei, woche.Days[6].Kind);  // So 27.09., frei
        Assert.Equal(LearningDayKind.Schultag, woche.Days[0].Kind);   // Mo 21.09., nicht gelernt
    }

    [Fact]
    public void Zaehlt_alle_Lerntage_und_die_der_letzten_vier_Wochen()
    {
        var gelernt = new HashSet<DateOnly>
        {
            new(2025, 12, 1),          // weit vor dem Kalender - zählt insgesamt mit
            new(2026, 9, 2),           // 28 Tage vor heute - außerhalb von vier Wochen
            new(2026, 9, 3),           // 27 Tage vor heute - innerhalb
            Heute,
            new(2026, 10, 5)           // Zukunft (Uhr verstellt?) - zählt nicht
        };

        var kalender = LearningCalendar.Build(gelernt, Heute, isSchoolFree: NurWochenende);

        Assert.Equal(4, kalender.TotalLearningDays);
        Assert.Equal(2, kalender.LearningDaysLastFourWeeks);
    }

    [Fact]
    public void Lerntage_sind_dieselben_wie_bei_den_Abzeichen()
    {
        var antworten = new[]
        {
            new MasteryAnswer(Subject.Mathematik, "A", "1", true, new DateTimeOffset(2026, 9, 29, 10, 0, 0, TimeSpan.FromHours(2))),
            new MasteryAnswer(Subject.Mathematik, "A", "2", false, new DateTimeOffset(2026, 9, 29, 11, 0, 0, TimeSpan.FromHours(2))),
            new MasteryAnswer(Subject.Deutsch, "B", "3", true, new DateTimeOffset(2026, 9, 30, 9, 0, 0, TimeSpan.FromHours(2)))
        };

        var tage = LearningCalendar.LearnedDays(antworten);
        var fakten = AchievementCatalog.FromAnswers(antworten, Array.Empty<TopicMasteryStatus>());

        Assert.Equal(fakten.LearningDays, tage.Count);
    }

    [Fact]
    public void Ohne_Angabe_gelten_Berliner_Ferien_als_schulfrei()
    {
        // Herbstferien 2026: 19.10. bis 31.10. (SchoolCalendar). Der Montag darin ist schulfrei.
        var kalender = LearningCalendar.Build(new HashSet<DateOnly>(), new DateOnly(2026, 10, 21));

        var montag = kalender.Weeks[^1].Days[0];
        Assert.Equal(new DateOnly(2026, 10, 19), montag.Date);
        Assert.Equal(LearningDayKind.Schulfrei, montag.Kind);
    }
}
