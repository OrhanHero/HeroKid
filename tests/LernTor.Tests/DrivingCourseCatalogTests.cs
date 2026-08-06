using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Der Theorie-Kurs: die Prüfungen, die man am Inhalt festmachen kann.</summary>
public sealed class DrivingCourseCatalogTests
{
    [Fact]
    public void Jedes_Sachgebiet_hat_genau_eine_Lektion()
    {
        // Ein Sachgebiet ohne Lektion wäre eine Lücke im Kurs; zwei Lektionen zum selben Gebiet
        // würden sich dieselben Kontrollfragen teilen und wie ein Fehler wirken.
        var themen = DrivingCourseCatalog.All.Select(lektion => lektion.Topic).ToList();

        Assert.Equal(Enum.GetValues<DrivingTheoryTopic>().Length, themen.Count);
        Assert.Equal(themen.Count, themen.Distinct().Count());

        foreach (var thema in Enum.GetValues<DrivingTheoryTopic>())
        {
            Assert.NotNull(DrivingCourseCatalog.ForTopic(thema));
        }
    }

    [Fact]
    public void Jede_Lektion_hat_eine_eigene_Kennung()
    {
        // Die Kennung ist der Schlüssel des Lernstands.
        var kennungen = DrivingCourseCatalog.All.Select(lektion => lektion.Id).ToList();

        Assert.Equal(kennungen.Count, kennungen.Distinct(StringComparer.Ordinal).Count());
        Assert.All(kennungen, id => Assert.False(string.IsNullOrWhiteSpace(id)));
    }

    [Fact]
    public void Jede_Lektion_hat_Text_Abschnitte_und_Merksaetze()
    {
        foreach (var lektion in DrivingCourseCatalog.All)
        {
            Assert.False(string.IsNullOrWhiteSpace(lektion.Title), $"{lektion.Id}: kein Titel");
            Assert.False(string.IsNullOrWhiteSpace(lektion.Intro), $"{lektion.Id}: keine Einleitung");

            Assert.True(lektion.Sections.Count >= 3, $"{lektion.Id}: zu wenige Abschnitte");
            Assert.True(lektion.KeyPoints.Count >= 2, $"{lektion.Id}: zu wenige Merksätze");

            foreach (var abschnitt in lektion.Sections)
            {
                Assert.False(string.IsNullOrWhiteSpace(abschnitt.Heading));
                // Ein Zweizeiler ist kein Abschnitt - der Kurs soll erklären, nicht auflisten.
                Assert.True(abschnitt.Body.Length > 120,
                    $"{lektion.Id}/{abschnitt.Heading}: Abschnitt zu kurz");
            }

            Assert.All(lektion.KeyPoints, satz => Assert.False(string.IsNullOrWhiteSpace(satz)));
        }
    }

    [Fact]
    public void Verweise_auf_Verkehrszeichen_zeigen_auf_vorhandene_Zeichen()
    {
        // Eine unbekannte Nummer hinterlässt in der Lektion eine leere Fläche neben dem Text.
        foreach (var lektion in DrivingCourseCatalog.All)
        {
            foreach (var nummer in lektion.SignNumbers)
            {
                Assert.True(TrafficSignCatalog.ByNumber(nummer) is not null,
                    $"{lektion.Id}: Zeichen {nummer} gibt es nicht");
            }
        }
    }

    [Fact]
    public void Jede_Lektion_kann_eine_Lernstandskontrolle_stellen()
    {
        // Sonst führt der Knopf "Lernstandskontrolle starten" ins Leere - deshalb prüft die
        // Ansicht das zur Laufzeit, und dieser Test hält fest, dass es nie nötig wird.
        foreach (var lektion in DrivingCourseCatalog.All)
        {
            Assert.NotEmpty(DrivingTheoryCatalog.ByTopic(lektion.Topic));
        }
    }

    [Fact]
    public void Die_Kontrollfragen_kommen_alle_aus_dem_Sachgebiet_der_Lektion()
    {
        var zufall = new Random(23);

        foreach (var lektion in DrivingCourseCatalog.All)
        {
            var fragen = DrivingCourseCatalog.CheckQuestions(lektion, zufall);

            Assert.NotEmpty(fragen);
            Assert.True(fragen.Count <= DrivingCourseRules.MaxCheckQuestions);
            Assert.Equal(fragen.Count, fragen.Select(frage => frage.Id).Distinct().Count());
            Assert.All(fragen, frage => Assert.Equal(lektion.Topic, frage.Topic));
        }
    }

    [Fact]
    public void Die_Kontrolle_ist_beim_zweiten_Anlauf_nicht_dieselbe_Reihenfolge()
    {
        // Sonst prüft ein Wiederholungsversuch nur noch, ob man sich die Reihenfolge gemerkt hat.
        var lektion = DrivingCourseCatalog.ForTopic(DrivingTheoryTopic.VorfahrtUndRegelung)!;
        var zufall = new Random(5);

        var reihenfolgen = new HashSet<string>();
        for (var i = 0; i < 20; i++)
        {
            reihenfolgen.Add(string.Join(",",
                DrivingCourseCatalog.CheckQuestions(lektion, zufall).Select(frage => frage.Id)));
        }

        Assert.True(reihenfolgen.Count > 1);
    }

    [Fact]
    public void Die_Lektionen_lassen_sich_ueber_ihre_Kennung_finden()
    {
        var erste = DrivingCourseCatalog.All[0];

        Assert.Equal(erste, DrivingCourseCatalog.ById(erste.Id));
        Assert.Null(DrivingCourseCatalog.ById("gibt-es-nicht"));
    }

    [Fact]
    public void Die_Kursreihenfolge_folgt_der_Reihenfolge_der_Sachgebiete()
    {
        // Die Enum-Reihenfolge IST die Kursreihenfolge - steht so in DrivingTheoryTopic.
        var erwartet = Enum.GetValues<DrivingTheoryTopic>();
        var tatsaechlich = DrivingCourseCatalog.All.Select(lektion => lektion.Topic).ToArray();

        Assert.Equal(erwartet, tatsaechlich);
    }
}
