using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der Berliner Schulkalender. Die Termine sind von Hand eingepflegt - geprüft wird deshalb
/// vor allem, dass sie in sich stimmen und dass sichtbar bleibt, wie weit sie reichen.
/// </summary>
public sealed class SchoolCalendarTests
{
    [Fact]
    public void Alle_eingetragenen_Termine_sind_vorhanden()
    {
        // 21 bis zum Sommer 2027, seit 3.1 dazu 7 Ferientermine 2027/28 und 10 Feiertage 2028.
        Assert.Equal(38, SchoolCalendar.All.Count);
    }

    [Fact]
    public void Die_Eintraege_sind_nach_Beginn_sortiert()
    {
        // Darauf verlassen sich Upcoming/NextVacation/NextHoliday - alle nehmen den ersten
        // passenden Eintrag statt zu sortieren.
        var starts = SchoolCalendar.All.Select(eintrag => eintrag.Start).ToList();

        Assert.Equal(starts.OrderBy(datum => datum), starts);
    }

    [Fact]
    public void Kein_Zeitraum_endet_vor_seinem_Beginn()
    {
        Assert.All(SchoolCalendar.All, eintrag => Assert.True(eintrag.End >= eintrag.Start, eintrag.Name));
    }

    [Fact]
    public void Ferienzeitraeume_ueberlappen_sich_nicht()
    {
        // Feiertage duerfen in Ferien liegen (25.12. in den Weihnachtsferien) - zwei Ferien
        // gleichzeitig waere dagegen ein Tippfehler.
        var ferien = SchoolCalendar.All
            .Where(eintrag => eintrag.Kind != CalendarEntryKind.Feiertag)
            .OrderBy(eintrag => eintrag.Start)
            .ToList();

        for (var i = 0; i + 1 < ferien.Count; i++)
        {
            Assert.True(ferien[i].End < ferien[i + 1].Start,
                $"{ferien[i].Name} überschneidet sich mit {ferien[i + 1].Name}");
        }
    }

    [Fact]
    public void Der_Kalender_sagt_wie_weit_er_reicht()
    {
        // Ohne diese Grenze wuerde die Startseite nach dem letzten Termin stillschweigend
        // nichts mehr anzeigen, und niemand wuesste, dass nachzutragen ist.
        Assert.Equal(new DateOnly(2028, 8, 12), SchoolCalendar.LastVacationDay);
        Assert.Equal(new DateOnly(2028, 12, 26), SchoolCalendar.LastHolidayDay);
    }

    /// <summary>Schuljahr 2027/28 (3.1): Ferien beginnen montags (oder nach Plan an einem
    /// anderen festen Tag) und enden samstags bzw. vor einem Wochenende - ein vertippter Tag
    /// fiele hier auf.</summary>
    [Theory]
    [InlineData(2027, 10, 11, DayOfWeek.Monday, 2027, 10, 23, DayOfWeek.Saturday)]
    [InlineData(2027, 12, 22, DayOfWeek.Wednesday, 2027, 12, 31, DayOfWeek.Friday)]
    [InlineData(2028, 1, 31, DayOfWeek.Monday, 2028, 2, 5, DayOfWeek.Saturday)]
    [InlineData(2028, 4, 10, DayOfWeek.Monday, 2028, 4, 22, DayOfWeek.Saturday)]
    [InlineData(2028, 6, 1, DayOfWeek.Thursday, 2028, 6, 2, DayOfWeek.Friday)]
    [InlineData(2028, 7, 1, DayOfWeek.Saturday, 2028, 8, 12, DayOfWeek.Saturday)]
    public void Ferien_2027_28_stimmen_mit_den_Wochentagen(int vj, int vm, int vt, DayOfWeek vonTag, int bj, int bm, int bt, DayOfWeek bisTag)
    {
        var von = new DateOnly(vj, vm, vt);
        var bis = new DateOnly(bj, bm, bt);
        Assert.Equal(vonTag, von.DayOfWeek);
        Assert.Equal(bisTag, bis.DayOfWeek);
        Assert.Contains(SchoolCalendar.All, e => e.Kind == CalendarEntryKind.Ferien && e.Start == von && e.End == bis);
    }

    [Fact]
    public void Bewegliche_Feiertage_2028_folgen_aus_Ostern()
    {
        // Ostersonntag 2028 nach der Gaußschen Osterformel (Gregorianisch).
        var ostern = Ostersonntag(2028);
        Assert.Equal(new DateOnly(2028, 4, 16), ostern);

        Assert.NotNull(SchoolCalendar.HolidayOn(ostern.AddDays(-2)));   // Karfreitag
        Assert.NotNull(SchoolCalendar.HolidayOn(ostern.AddDays(1)));    // Ostermontag
        Assert.NotNull(SchoolCalendar.HolidayOn(ostern.AddDays(39)));   // Himmelfahrt
        Assert.NotNull(SchoolCalendar.HolidayOn(ostern.AddDays(50)));   // Pfingstmontag
        Assert.Equal(new DateOnly(2028, 8, 14), SchoolCalendar.SchoolYearStart(new DateOnly(2028, 9, 1)));
    }

    private static DateOnly Ostersonntag(int jahr)
    {
        int a = jahr % 19, b = jahr / 100, c = jahr % 100, d = b / 4, e = b % 4;
        int f = (b + 8) / 25, g = (b - f + 1) / 3, h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4, k = c % 4, l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int monat = (h + l - 7 * m + 114) / 31, tag = (h + l - 7 * m + 114) % 31 + 1;
        return new DateOnly(jahr, monat, tag);
    }

    [Theory]
    [InlineData(2026, 7, 9)]      // erster Ferientag
    [InlineData(2026, 8, 6)]
    [InlineData(2026, 8, 22)]     // letzter Ferientag
    public void Die_Sommerferien_2026_umfassen_ihre_Randtage(int jahr, int monat, int tag)
    {
        var ferien = SchoolCalendar.CurrentVacation(new DateOnly(jahr, monat, tag));

        Assert.NotNull(ferien);
        Assert.Equal("Sommerferien", ferien!.Name);
    }

    [Fact]
    public void Am_Schulstart_sind_keine_Ferien_mehr()
    {
        // Der letzte Ferientag ist Samstag, der 22.08.2026; erster Schultag ist Montag, der
        // 24.08.2026. Der Sonntag dazwischen gehoert zu KEINEM der beiden - ein Wochenendtag
        // ist kein Ferientag, auch wenn er sich so anfuehlt. Genau daran ist eine erste Fassung
        // dieses Tests gescheitert, die "der Tag vor Schulstart ist noch Ferien" annahm.
        Assert.NotNull(SchoolCalendar.CurrentVacation(new DateOnly(2026, 8, 22)));
        Assert.Null(SchoolCalendar.CurrentVacation(new DateOnly(2026, 8, 23)));
        Assert.Null(SchoolCalendar.CurrentVacation(SchoolCalendar.SchoolYearStart2026));
    }

    [Fact]
    public void Ferien_schlagen_den_Feiertag_darin()
    {
        // Am 25.12. ist die richtige Auskunft "Weihnachtsferien", nicht "1. Weihnachtsfeiertag".
        var heiligabendPlusEins = new DateOnly(2026, 12, 25);
        var stand = SchoolCalendar.Today(heiligabendPlusEins);

        Assert.Equal("Weihnachtsferien", stand.Vacation?.Name);
        Assert.Equal("1. Weihnachtsfeiertag", stand.Holiday?.Name);
        Assert.Equal("Weihnachtsferien", stand.Headline?.Name);
    }

    [Fact]
    public void Die_Weihnachtsferien_gehen_ueber_den_Jahreswechsel()
    {
        var ferien = SchoolCalendar.All.First(eintrag => eintrag.Name == "Weihnachtsferien");

        Assert.Equal(2026, ferien.Start.Year);
        Assert.Equal(2027, ferien.End.Year);
        Assert.Equal(11, ferien.LengthInDays);
        Assert.NotNull(SchoolCalendar.CurrentVacation(new DateOnly(2027, 1, 1)));
    }

    [Fact]
    public void Restliche_Tage_zaehlen_den_heutigen_mit()
    {
        var ferien = SchoolCalendar.All.First(eintrag => eintrag.Name == "Sommerferien");

        Assert.Equal(17, ferien.RemainingDays(new DateOnly(2026, 8, 6)));
        Assert.Equal(1, ferien.RemainingDays(ferien.End));
        Assert.Equal(0, ferien.RemainingDays(ferien.End.AddDays(1)));
    }

    [Fact]
    public void Ein_Schultag_zeigt_die_naechsten_Termine_statt_eines_laufenden()
    {
        var schultag = new DateOnly(2026, 9, 15);
        var stand = SchoolCalendar.Today(schultag);

        Assert.Null(stand.Vacation);
        Assert.Null(stand.Holiday);
        Assert.False(stand.IsSchoolFree);
        Assert.Equal("Herbstferien", stand.NextVacation?.Name);
        Assert.Equal("Tag der Deutschen Einheit", stand.NextHoliday?.Name);

        // Angezeigt wird, was ZUERST kommt: der Feiertag am 03.10. liegt vor den Herbstferien
        // am 19.10. Die naechsten Ferien anzukuendigen, waehrend vorher ein Feiertag liegt,
        // waere zwar richtig, aber nicht die Antwort auf die Frage, die ein Kind hat.
        Assert.Equal("Tag der Deutschen Einheit", stand.Headline?.Name);
    }

    [Fact]
    public void Feiertage_am_Wochenende_sind_als_solche_erkennbar()
    {
        // 03.10.2026 ist ein Samstag - ein freier Tag ist das fuer die Kinder nicht.
        var einheit = SchoolCalendar.All.First(eintrag => eintrag.Start == new DateOnly(2026, 10, 3));

        Assert.True(einheit.FallsOnWeekend);

        // Der Frauentag 2027 faellt dagegen auf einen Montag.
        var frauentag = SchoolCalendar.All.Single(eintrag => eintrag.Name == "Internationaler Frauentag" && eintrag.Start.Year == 2027);

        Assert.False(frauentag.FallsOnWeekend);
    }

    [Fact]
    public void Hinter_dem_letzten_Termin_gibt_es_nichts_mehr_anzuzeigen()
    {
        var danach = SchoolCalendar.LastHolidayDay.AddDays(1);
        var stand = SchoolCalendar.Today(danach);

        Assert.True(stand.BeyondCoverage);
        Assert.Null(stand.Headline);
    }

    [Fact]
    public void Vergangene_Termine_tauchen_in_der_Vorschau_nicht_auf()
    {
        var stichtag = new DateOnly(2027, 6, 1);

        Assert.All(SchoolCalendar.Upcoming(stichtag, 100),
            eintrag => Assert.True(eintrag.End >= stichtag, eintrag.Name));
    }

    [Fact]
    public void Jeder_Eintrag_hat_einen_Namen()
    {
        Assert.All(SchoolCalendar.All,
            eintrag => Assert.False(string.IsNullOrWhiteSpace(eintrag.Name)));
    }

    [Fact]
    public void Das_Schuljahr_2026_27_beginnt_am_Montag_nach_den_Sommerferien()
    {
        // Sommerferien enden Samstag, 22.08.2026 - erster Schultag ist Montag, 24.08.
        Assert.Equal(SchoolCalendar.SchoolYearStart2026, SchoolCalendar.SchoolYearStart(new DateOnly(2026, 9, 28)));
        Assert.Equal(new DateOnly(2026, 8, 24), SchoolCalendar.SchoolYearStart(new DateOnly(2026, 8, 24)));
    }

    [Fact]
    public void Vor_dem_ersten_bekannten_Schuljahresbeginn_gibt_es_keinen()
    {
        Assert.Null(SchoolCalendar.SchoolYearStart(new DateOnly(2026, 8, 10)));
        Assert.Null(SchoolCalendar.SchoolYearStart(new DateOnly(2026, 8, 23)));
    }

    [Fact]
    public void Der_Hinweis_auf_die_Klassenstufe_gilt_nur_in_den_ersten_vier_Wochen()
    {
        Assert.True(SchoolCalendar.IsEarlySchoolYear(new DateOnly(2026, 8, 24)));
        Assert.True(SchoolCalendar.IsEarlySchoolYear(new DateOnly(2026, 9, 20)));
        Assert.False(SchoolCalendar.IsEarlySchoolYear(new DateOnly(2026, 9, 21)));
        Assert.False(SchoolCalendar.IsEarlySchoolYear(new DateOnly(2026, 8, 20)));
    }
}
