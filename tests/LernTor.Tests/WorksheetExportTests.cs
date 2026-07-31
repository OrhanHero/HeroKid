using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Übungsblatt: was auf dem Aufgabenblatt steht - und was ausdrücklich nicht.</summary>
public sealed class WorksheetExportTests
{
    private static readonly DateOnly Heute = new(2026, 8, 5);

    private static QuizQuestion Auswahlfrage(string prompt = "Was ist 2 + 2?", string richtig = "4") => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Subject = Subject.Mathematik,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Addition",
        Type = QuestionType.MultipleChoice,
        Prompt = prompt,
        Options = new[] { richtig, "5", "6" },
        CorrectAnswers = new[] { richtig },
        Explanation = "Zwei plus zwei ist vier."
    };

    private static QuizQuestion OffeneFrage() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Subject = Subject.Mathematik,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Division",
        Type = QuestionType.OpenText,
        Prompt = "Berechne 84 : 7",
        CorrectAnswers = new[] { "12" },
        Explanation = "84 geteilt durch 7 ergibt 12."
    };

    private static QuizQuestion Diktatfrage() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Subject = Subject.Deutsch,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Diktat",
        Type = QuestionType.Diktat,
        Prompt = "Der Hund bellt laut im Garten.",
        CorrectAnswers = new[] { "Der Hund bellt laut im Garten." },
        Explanation = "Nomen werden großgeschrieben."
    };

    /// <summary>Das Umbruch-ELEMENT, nicht das blosse Wort: "pagebreak" steht auch in der
    /// CSS-Regel im &lt;head&gt; und damit vor allem anderen.</summary>
    private const string UmbruchElement = "<div class=\"pagebreak\">";

    private static string Blatt(params QuizQuestion[] fragen) =>
        WorksheetExport.ToHtml(fragen, "Mathematik", GradeLevel.Klasse6, Heute, new Random(1));

    [Fact]
    public void Kopfzeile_traegt_Fach_Klasse_Datum_und_ein_Namensfeld()
    {
        var html = Blatt(Auswahlfrage());

        Assert.Contains("Mathematik", html);
        Assert.Contains("Klasse 6", html);
        Assert.Contains("05.08.2026", html);
        Assert.Contains("Name:", html);
    }

    [Fact]
    public void Aufgaben_werden_durchnummeriert()
    {
        var html = Blatt(Auswahlfrage("Erste Frage?"), Auswahlfrage("Zweite Frage?"));

        Assert.Contains("Erste Frage?", html);
        Assert.Contains("Zweite Frage?", html);
        Assert.Contains(">1.<", html);
        Assert.Contains(">2.<", html);
    }

    [Fact]
    public void Loesungen_stehen_hinter_einem_harten_Seitenumbruch()
    {
        // Die wichtigste Eigenschaft des Blattes: stuende die Antwort neben der Aufgabe, waere es
        // wertlos - kein Kind sieht weg, und niemand sollte das von ihm verlangen.
        var html = Blatt(Auswahlfrage());

        var aufgabe = html.IndexOf("Was ist 2 + 2?", StringComparison.Ordinal);
        var umbruch = html.IndexOf(UmbruchElement, StringComparison.Ordinal);
        var loesungen = html.IndexOf("<h1>Lösungen</h1>", StringComparison.Ordinal);

        Assert.True(aufgabe < umbruch, "Die Aufgabe muss vor dem Seitenumbruch stehen.");
        Assert.True(umbruch < loesungen, "Die Lösungen müssen hinter dem Seitenumbruch stehen.");
        Assert.Contains("page-break-before: always", html);
    }

    [Fact]
    public void Offene_Aufgaben_bekommen_Schreiblinien_statt_Optionen()
    {
        var html = Blatt(OffeneFrage());

        Assert.Contains("writeline", html);
        Assert.DoesNotContain("<ol class=\"options\">", html);
    }

    [Fact]
    public void Antwortoptionen_werden_gemischt()
    {
        // Ungemischt waere auf dem ganzen Blatt fast immer "A" richtig - die Generatoren legen die
        // richtige Antwort meist als erstes Element an. Ein Kind durchschaut das sofort.
        var frage = new QuizQuestion
        {
            Id = "q",
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "T",
            Type = QuestionType.MultipleChoice,
            Prompt = "P",
            Options = Enumerable.Range(1, 8).Select(i => $"Option{i}").ToArray(),
            CorrectAnswers = new[] { "Option1" },
            Explanation = "E"
        };

        var reihenfolgen = Enumerable.Range(1, 12)
            .Select(seed => WorksheetExport.ToHtml(
                new[] { frage }, "Mathematik", GradeLevel.Klasse6, Heute, new Random(seed)))
            .Select(html => html.IndexOf("Option1", StringComparison.Ordinal))
            .Distinct()
            .Count();

        Assert.True(reihenfolgen > 1, "Die richtige Antwort stand bei jedem Versuch an derselben Stelle.");
    }

    [Fact]
    public void Der_Loesungsbuchstabe_passt_zur_gedruckten_Reihenfolge()
    {
        // Einmal mischen, fuer Aufgabe UND Loesung verwenden - sonst zeigt das Loesungsblatt
        // einen Buchstaben, der auf dem Aufgabenblatt woanders steht.
        for (var seed = 1; seed <= 20; seed++)
        {
            var html = WorksheetExport.ToHtml(
                new[] { Auswahlfrage() }, "Mathematik", GradeLevel.Klasse6, Heute, new Random(seed));

            var optionen = html
                .Split("<li>")
                .Skip(1)
                .Select(part => part[..part.IndexOf("</li>", StringComparison.Ordinal)])
                .ToList();

            var erwarteterBuchstabe = "ABC"[optionen.IndexOf("4")];
            Assert.Contains($"{erwarteterBuchstabe}) 4", html);
        }
    }

    [Fact]
    public void Diktate_stehen_nicht_auf_dem_Aufgabenblatt()
    {
        // Ein gedrucktes Diktat waere Abschreiben - der Satz DARF nicht dastehen. Auf dem
        // Loesungsblatt ist er dagegen richtig: von dort liest ein Elternteil ihn vor.
        var html = WorksheetExport.ToHtml(
            new[] { Diktatfrage() }, "Deutsch", GradeLevel.Klasse6, Heute, new Random(1));

        var satz = html.IndexOf("Der Hund bellt laut im Garten.", StringComparison.Ordinal);
        var umbruch = html.IndexOf(UmbruchElement, StringComparison.Ordinal);

        Assert.True(satz > umbruch, "Der Diktatsatz darf erst auf dem Lösungsblatt auftauchen.");
        Assert.Contains("Zum Vorlesen", html);
    }

    [Fact]
    public void Ein_Blatt_nur_aus_Diktaten_sagt_das_auch()
    {
        var html = WorksheetExport.ToHtml(
            new[] { Diktatfrage() }, "Deutsch", GradeLevel.Klasse6, Heute, new Random(1));

        Assert.Contains("keine druckbaren Aufgaben", html);
    }

    [Fact]
    public void Freier_Text_wird_maskiert()
    {
        var html = Blatt(Auswahlfrage("Was ist 3 < 5 & 5 > 3?"));

        Assert.Contains("3 &lt; 5 &amp; 5 &gt; 3", html);
    }

    [Fact]
    public void Das_Blatt_laedt_nichts_aus_dem_Netz()
    {
        var html = Blatt(Auswahlfrage(), OffeneFrage());

        Assert.DoesNotContain("http://", html);
        Assert.DoesNotContain("https://", html);
        Assert.DoesNotContain("<script", html);
    }

    [Fact]
    public void Dateiname_enthaelt_keine_verbotenen_Zeichen()
    {
        var name = WorksheetExport.SuggestFileName("Deutsch/Türkisch", Heute);

        Assert.Equal(-1, name.IndexOfAny(Path.GetInvalidFileNameChars()));
        Assert.Equal("Uebungsblatt-Deutsch-Türkisch-2026-08-05.html", name);
        Assert.Equal("Uebungsblatt-2026-08-05.html", WorksheetExport.SuggestFileName(null, Heute));
    }

    [Theory]
    [InlineData(GradeLevel.Klasse6, "6")]
    [InlineData(GradeLevel.Klasse7, "7")]
    [InlineData(GradeLevel.Klasse9, "9")]
    public void Klassenstufe_steht_als_Zahl_auf_dem_Blatt(GradeLevel stufe, string erwartet)
    {
        Assert.Equal(erwartet, WorksheetExport.GradeNumber(stufe));
    }
}
