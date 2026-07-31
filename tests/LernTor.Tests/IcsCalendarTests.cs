using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// ICS-Import/-Export der Klausurtermine. Die bewusste Alternative zu einer
/// Google-Calendar-Anbindung: offener Standard, kein Konto, kein Netz, kein Browserfenster in
/// einer Kiosk-App.
/// </summary>
public sealed class IcsCalendarTests
{
    private static ExamEntry Klausur(
        Subject subject = Subject.Mathematik,
        string title = "Klassenarbeit Nr. 2",
        string topics = "",
        int year = 2026, int month = 9, int day = 14) => new()
    {
        ProfileId = "p1",
        Subject = subject,
        Title = title,
        Topics = topics,
        ExamDate = new DateOnly(year, month, day)
    };

    [Fact]
    public void Export_erzeugt_einen_gueltigen_Kalenderrahmen()
    {
        var ics = IcsCalendar.Export(new[] { Klausur() });

        Assert.StartsWith("BEGIN:VCALENDAR", ics);
        Assert.Contains("VERSION:2.0", ics);
        Assert.EndsWith("END:VCALENDAR\r\n", ics);
        Assert.Contains("BEGIN:VEVENT", ics);
        Assert.Contains("END:VEVENT", ics);
    }

    [Fact]
    public void Export_schreibt_ganztaegige_Termine_mit_exklusivem_Ende()
    {
        // DTEND ist bei ganztaegigen Terminen laut RFC 5545 EXKLUSIV - mit demselben Tag zeigen
        // manche Kalender den Termin gar nicht an.
        var ics = IcsCalendar.Export(new[] { Klausur() });

        Assert.Contains("DTSTART;VALUE=DATE:20260914", ics);
        Assert.Contains("DTEND;VALUE=DATE:20260915", ics);
    }

    [Fact]
    public void Export_nennt_Fach_und_Titel()
    {
        Assert.Contains("SUMMARY:Physik: Test Nr. 2",
            IcsCalendar.Export(new[] { Klausur(Subject.Physik, "Test Nr. 2") }));
    }

    [Fact]
    public void Export_kommt_ohne_Titel_aus()
    {
        Assert.Contains("SUMMARY:Chemie-Klausur",
            IcsCalendar.Export(new[] { Klausur(Subject.Chemie, title: "") }));
    }

    [Fact]
    public void Sonderzeichen_werden_maskiert()
    {
        // Komma und Semikolon trennen in ICS Werte - unmaskiert zerfaellt der Titel.
        var ics = IcsCalendar.Export(new[] { Klausur(topics: "Brüche, Prozent; Zinsen") });

        Assert.Contains(@"DESCRIPTION:Brüche\, Prozent\; Zinsen", ics);
    }

    [Fact]
    public void Export_und_Import_sind_zueinander_passend()
    {
        var ics = IcsCalendar.Export(new[] { Klausur(Subject.Biologie, "Arbeit", "Zellen, Organe") });

        var imported = IcsCalendar.Import(ics).Single();

        Assert.Equal(new DateOnly(2026, 9, 14), imported.Date);
        Assert.Equal("Biologie: Arbeit", imported.Title);
        Assert.Equal("Zellen, Organe", imported.Description);
        Assert.Equal(Subject.Biologie, imported.GuessedSubject);
    }

    [Fact]
    public void Import_liest_mehrere_Termine()
    {
        var ics = IcsCalendar.Export(new[]
        {
            Klausur(Subject.Mathematik, "Eins", day: 14),
            Klausur(Subject.Deutsch, "Zwei", day: 21)
        });

        Assert.Equal(2, IcsCalendar.Import(ics).Count);
    }

    [Theory]
    [InlineData("DTSTART;VALUE=DATE:20260914")]
    [InlineData("DTSTART:20260914")]
    [InlineData("DTSTART:20260914T080000")]
    [InlineData("DTSTART;TZID=Europe/Berlin:20260914T080000")]
    public void Import_versteht_die_ueblichen_Datumsformen(string dtstart)
    {
        var ics = $"BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nSUMMARY:Mathearbeit\r\n{dtstart}\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";

        Assert.Equal(new DateOnly(2026, 9, 14), IcsCalendar.Import(ics).Single().Date);
    }

    [Fact]
    public void Import_haengt_gefaltete_Zeilen_wieder_zusammen()
    {
        // Zeilen ueber 75 Zeichen werden nach RFC 5545 umgebrochen und mit einem Leerzeichen
        // fortgesetzt - ohne Zusammenfuegen zerfaellt jeder laengere Titel in Bruchstuecke.
        var ics = "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nSUMMARY:Mathematik Klassenarbeit ueber Bruch\r\n" +
                  " rechnung und Prozente\r\nDTSTART;VALUE=DATE:20260914\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";

        Assert.Equal(
            "Mathematik Klassenarbeit ueber Bruchrechnung und Prozente",
            IcsCalendar.Import(ics).Single().Title);
    }

    [Fact]
    public void Lange_Zeilen_werden_beim_Export_gefaltet_und_ueberstehen_den_Rueckweg()
    {
        var langerTitel = new string('A', 200);
        var ics = IcsCalendar.Export(new[] { Klausur(Subject.Deutsch, langerTitel) });

        Assert.Contains("\r\n ", ics);
        Assert.Equal($"Deutsch: {langerTitel}", IcsCalendar.Import(ics).Single().Title);
    }

    [Theory]
    [InlineData("Mathearbeit", Subject.Mathematik)]
    [InlineData("Klassenarbeit Deutsch", Subject.Deutsch)]
    [InlineData("Vokabeltest Englisch", Subject.Englisch)]
    [InlineData("Erdkunde-Test", Subject.Geo)]
    [InlineData("Informatik Klausur", Subject.Itg)]
    public void Fach_wird_aus_dem_Titel_erraten(string title, Subject erwartet)
    {
        Assert.Equal(erwartet, IcsCalendar.GuessSubject(title));
    }

    [Fact]
    public void Unbekannte_Titel_raten_lieber_gar_nicht()
    {
        // Ein falsch geratenes Fach wuerde still die Uebungsgewichtung verstellen - dann lieber
        // die Eltern beim Uebernehmen waehlen lassen.
        Assert.Null(IcsCalendar.GuessSubject("Wandertag"));
        Assert.Null(IcsCalendar.GuessSubject(""));
    }

    [Fact]
    public void Termine_ohne_Datum_werden_uebersprungen()
    {
        var ics = "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nSUMMARY:Ohne Datum\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";

        Assert.Empty(IcsCalendar.Import(ics));
    }

    [Fact]
    public void Fremde_Eigenschaften_stoeren_nicht()
    {
        // Fremde Kalender liefern eine Menge mit, was hier niemanden interessiert.
        var ics = "BEGIN:VCALENDAR\r\nPRODID:-//Google Inc//Google Calendar//EN\r\nBEGIN:VEVENT\r\n" +
                  "SEQUENCE:3\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nORGANIZER;CN=Schule:mailto:x@y.z\r\n" +
                  "SUMMARY:Physik Klausur\r\nDTSTART;VALUE=DATE:20260914\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";

        var imported = IcsCalendar.Import(ics).Single();

        Assert.Equal("Physik Klausur", imported.Title);
        Assert.Equal(Subject.Physik, imported.GuessedSubject);
    }

    [Fact]
    public void Leere_und_kaputte_Eingaben_werfen_nicht()
    {
        Assert.Empty(IcsCalendar.Import(string.Empty));
        Assert.Empty(IcsCalendar.Import("   "));
        Assert.Empty(IcsCalendar.Import("völliger Unsinn ohne Struktur"));
    }

    [Fact]
    public void Zeilenumbrueche_in_den_Themen_ueberstehen_den_Rueckweg()
    {
        var ics = IcsCalendar.Export(new[] { Klausur(topics: "Zeile 1\nZeile 2") });

        Assert.Contains(@"DESCRIPTION:Zeile 1\nZeile 2", ics);
        Assert.Equal("Zeile 1\nZeile 2", IcsCalendar.Import(ics).Single().Description);
    }
}
