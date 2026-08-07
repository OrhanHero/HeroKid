using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der getippte Stundenplan. Wichtiger als jede erkannte Zeile ist, dass eine NICHT erkannte
/// gemeldet wird: eine stillschweigend übersprungene Zeile wäre eine Schulstunde, die im Plan
/// des Kindes fehlt, und das fiele niemandem auf.
/// </summary>
public sealed class TimetableTextParserTests
{
    [Fact]
    public void Eine_Zeile_je_Wochentag_wird_gelesen()
    {
        var ergebnis = TimetableTextParser.Parse(
            "Mo: 1 Deutsch, 2 Mathe, 3 Sport\n" +
            "Di: 1 Musik, 2 Deutsch");

        Assert.Empty(ergebnis.Warnings);
        Assert.Equal(5, ergebnis.Lessons.Count);

        var montag = ergebnis.Lessons.Where(s => s.Day == DayOfWeek.Monday).ToList();
        Assert.Equal(new[] { 1, 2, 3 }, montag.Select(s => s.Period));
        Assert.Equal(new[] { "Deutsch", "Mathe", "Sport" }, montag.Select(s => s.Subject));
    }

    [Theory]
    [InlineData("Mo: 1 Mathe")]
    [InlineData("Mo 1 Mathe")]
    [InlineData("Montag: 1 Mathe")]
    [InlineData("montag: 1. Mathe")]
    [InlineData("MO: 1) Mathe")]
    public void Der_Wochentag_darf_verschieden_geschrieben_werden(string zeile)
    {
        var ergebnis = TimetableTextParser.Parse(zeile);

        var stunde = Assert.Single(ergebnis.Lessons);
        Assert.Equal(DayOfWeek.Monday, stunde.Day);
        Assert.Equal(1, stunde.Period);
        Assert.Equal("Mathe", stunde.Subject);
    }

    [Fact]
    public void Der_Raum_darf_in_Klammern_dahinter_stehen()
    {
        var ergebnis = TimetableTextParser.Parse("Mo: 1 Sport (TH1), 2 Mathe [A012a]");

        Assert.Equal("Sport", ergebnis.Lessons[0].Subject);
        Assert.Equal("TH1", ergebnis.Lessons[0].Room);
        Assert.Equal("Mathe", ergebnis.Lessons[1].Subject);
        Assert.Equal("A012a", ergebnis.Lessons[1].Room);
    }

    [Fact]
    public void Ein_Tag_darf_ueber_mehrere_Zeilen_gehen()
    {
        // Wer einen langen Tag umbricht, soll den Wochentag nicht wiederholen muessen.
        var ergebnis = TimetableTextParser.Parse(
            "Mo:\n" +
            "1 Deutsch\n" +
            "2 Mathe\n" +
            "Di: 1 Sport");

        Assert.Empty(ergebnis.Warnings);
        Assert.Equal(2, ergebnis.Lessons.Count(s => s.Day == DayOfWeek.Monday));
        Assert.Single(ergebnis.Lessons.Where(s => s.Day == DayOfWeek.Tuesday));
    }

    [Fact]
    public void Semikolon_geht_genauso_wie_Komma()
    {
        var ergebnis = TimetableTextParser.Parse("Fr: 1 Deutsch; 2 Mathe");

        Assert.Equal(2, ergebnis.Lessons.Count);
        Assert.All(ergebnis.Lessons, stunde => Assert.Equal(DayOfWeek.Friday, stunde.Day));
    }

    [Fact]
    public void Was_nicht_gedeutet_werden_kann_wird_gemeldet_statt_verschluckt()
    {
        var ergebnis = TimetableTextParser.Parse(
            "Mo: 1 Deutsch, Mathe ohne Stunde, 3 Sport");

        Assert.Equal(2, ergebnis.Lessons.Count);
        var warnung = Assert.Single(ergebnis.Warnings);
        Assert.Contains("Mathe ohne Stunde", warnung);
        Assert.Contains("Zeile 1", warnung);
    }

    [Fact]
    public void Eine_Zeile_ohne_Wochentag_am_Anfang_wird_gemeldet()
    {
        var ergebnis = TimetableTextParser.Parse("1 Deutsch, 2 Mathe");

        Assert.Empty(ergebnis.Lessons);
        Assert.Single(ergebnis.Warnings);
    }

    [Fact]
    public void Stundennummern_ausserhalb_des_Rasters_gelten_nicht()
    {
        var ergebnis = TimetableTextParser.Parse($"Mo: 0 Zu früh, {Timetable.MaxPeriod + 1} Zu spät");

        Assert.Empty(ergebnis.Lessons);
        Assert.Equal(2, ergebnis.Warnings.Count);
    }

    [Fact]
    public void Der_zuletzt_genannte_Eintrag_gewinnt()
    {
        // Ein nachgetragener Korrektur-Eintrag soll wirken, nicht wirkungslos hinten anstehen.
        var ergebnis = TimetableTextParser.Parse("Mo: 1 Falsch\nMo: 1 Richtig");

        var stunde = Assert.Single(ergebnis.Lessons);
        Assert.Equal("Richtig", stunde.Subject);
    }

    [Fact]
    public void Leerer_Text_ergibt_nichts_und_meldet_nichts()
    {
        var ergebnis = TimetableTextParser.Parse("   \n\n  ");

        Assert.Empty(ergebnis.Lessons);
        Assert.Empty(ergebnis.Warnings);
        Assert.False(ergebnis.HasLessons);
    }

    [Fact]
    public void Ein_ganzer_Beispielplan_geht_ohne_Warnung_durch()
    {
        // Nach dem Muster eines echten Berliner Klassenplans: fuenf Tage, Luecken zwischendrin,
        // Faecher, die LernTor als Lernbereich gar nicht kennt (NaWi, GeWi, WPU).
        var ergebnis = TimetableTextParser.Parse(
            "Mo: 2 Mathe, 3 Sport, 4 Sport, 6 Englisch, 7 Englisch, 8 WPU Sport\n" +
            "Di: 1 Musik, 2 Deutsch, 3 Musik, 4 GeWi, 6 NaWi, 7 Englisch, 8 WPU Keyboard\n" +
            "Mi: 1 Mathe, 2 Mathe, 3 GeWi, 4 GeWi, 6 Sport, 7 Mathe Förder\n" +
            "Do: 1 Englisch, 2 Mathe, 3 NaWi, 4 NaWi, 6 Deutsch, 7 Deutsch\n" +
            "Fr: 1 Deutsch, 2 Mathe, 3 NaWi, 4 NaWi, 6 Kunst, 7 Kunst");

        Assert.Empty(ergebnis.Warnings);
        Assert.Equal(31, ergebnis.Lessons.Count);

        // Das Fach bleibt so stehen, wie es auf dem Plan der Schule steht - nicht auf ein
        // LernTor-Fach umgebogen.
        Assert.Contains(ergebnis.Lessons, stunde => stunde.Subject == "WPU Keyboard");
        Assert.Contains(ergebnis.Lessons, stunde => stunde.Subject == "Mathe Förder");
    }

    [Theory]
    [InlineData("Mathe", "Mathe", null)]
    [InlineData("Mathe (A012a)", "Mathe", "A012a")]
    [InlineData("Sport [TH1]", "Sport", "TH1")]
    [InlineData("  Deutsch  ", "Deutsch", null)]
    [InlineData("", "", null)]
    // Ohne Fach davor sind die Klammern kein Raum, sondern das Fach selbst.
    [InlineData("(Sport)", "(Sport)", null)]
    public void Der_Raum_wird_auch_aus_einer_Rasterzelle_abgetrennt(string zelle, string fach, string? raum)
    {
        var (erkanntesFach, erkannterRaum) = TimetableTextParser.SplitRoom(zelle);

        Assert.Equal(fach, erkanntesFach);
        Assert.Equal(raum, erkannterRaum);
    }
}
