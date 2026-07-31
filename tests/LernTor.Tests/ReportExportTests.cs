using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Bericht als Datei: Maskierung, Ampelfarben, Dateiname.</summary>
public sealed class ReportExportTests
{
    private static ReportExportDocument Dokument(
        string kind = "Emirhan",
        IReadOnlyList<string>? zusammenfassung = null,
        params ReportExportSection[] abschnitte) =>
        new(kind, "Letzte 7 Tage", new DateTimeOffset(2026, 8, 5, 18, 30, 0, TimeSpan.FromHours(2)),
            zusammenfassung ?? new[] { "📅 An 4 von 7 Tagen gelernt" }, abschnitte);

    [Fact]
    public void Bericht_enthaelt_Kind_Zeitraum_und_Zeilen()
    {
        var html = ReportExport.ToHtml(Dokument(
            abschnitte: new ReportExportSection("Fächer", new[]
            {
                new ReportExportRow("Mathematik", "80 % (8/10)", 0.8)
            })));

        Assert.Contains("Emirhan", html);
        Assert.Contains("Letzte 7 Tage", html);
        Assert.Contains("📅 An 4 von 7 Tagen gelernt", html);
        Assert.Contains("Mathematik", html);
        Assert.Contains("80 % (8/10)", html);
        Assert.Contains("05.08.2026", html);
    }

    [Fact]
    public void Freier_Text_wird_maskiert()
    {
        // Kindernamen und Themenbezeichnungen sind frei eingegeben - ein "&" oder eine spitze
        // Klammer darin darf die Datei nicht zerlegen.
        var html = ReportExport.ToHtml(Dokument(
            kind: "Ali & <b>Ayse</b>",
            abschnitte: new ReportExportSection("Themen", new[]
            {
                new ReportExportRow("Brüche <> Dezimal", "50 % (1/2)", 0.5)
            })));

        Assert.DoesNotContain("<b>Ayse</b>", html);
        Assert.Contains("Ali &amp; &lt;b&gt;Ayse&lt;/b&gt;", html);
        Assert.Contains("Brüche &lt;&gt; Dezimal", html);
    }

    [Theory]
    [InlineData(1.0, "good")]
    [InlineData(0.75, "good")]
    [InlineData(0.74, "ok")]
    [InlineData(0.5, "ok")]
    [InlineData(0.49, "weak")]
    [InlineData(0.0, "weak")]
    public void Ampelfarben_folgen_denselben_Schwellen_wie_die_App(double quote, string erwartet)
    {
        Assert.Equal(erwartet, ReportExport.RateClass(quote));
    }

    [Fact]
    public void Balkenbreite_steht_in_der_Datei_und_bleibt_im_Rahmen()
    {
        var html = ReportExport.ToHtml(Dokument(
            abschnitte: new ReportExportSection("Fächer", new[]
            {
                new ReportExportRow("Mathematik", "80 %", 0.8),
                new ReportExportRow("Kaputt", "?", 1.7)
            })));

        // Punkt statt Komma: eine deutsche Kultur wuerde "width:80,0%" schreiben - ungueltiges CSS.
        Assert.Contains("width:80%", html);
        Assert.Contains("width:100%", html);
        Assert.DoesNotContain("width:170%", html);
    }

    [Fact]
    public void Zeilen_ohne_Quote_bekommen_keinen_Balken()
    {
        var html = ReportExport.ToHtml(Dokument(
            abschnitte: new ReportExportSection("Lernzeit", new[]
            {
                new ReportExportRow("Mathematik", "42 min", Note: "12 s pro Aufgabe")
            })));

        Assert.DoesNotContain("class=\"bar\"", html);
        Assert.Contains("12 s pro Aufgabe", html);
    }

    [Fact]
    public void Leere_Abschnitte_und_leere_Zusammenfassungszeilen_fallen_weg()
    {
        // Eine Ueberschrift ohne Inhalt sieht im Ausdruck nach einem Fehler aus.
        var html = ReportExport.ToHtml(Dokument(
            zusammenfassung: new[] { "Gelernt: viel", string.Empty, "   " },
            abschnitte: new ReportExportSection("Themen", Array.Empty<ReportExportRow>())));

        Assert.DoesNotContain("Themen", html);
        Assert.Equal(1, html.Split("class=\"summary\"").Length - 1);
    }

    [Fact]
    public void Dateiname_traegt_Namen_und_Datum()
    {
        Assert.Equal(
            "LernTor-Bericht-Emirhan-2026-08-05.html",
            ReportExport.SuggestFileName("Emirhan", new DateOnly(2026, 8, 5)));
    }

    [Theory]
    [InlineData("Ali/Ayse")]
    [InlineData("Ali:Ayse")]
    [InlineData("Ali*?Ayse")]
    [InlineData("Ali\\Ayse")]
    public void Dateiname_enthaelt_keine_verbotenen_Zeichen(string kind)
    {
        // Ein Slash im Profilnamen haette den Speichern-Dialog mit einem Pfadfehler abgewiesen.
        var name = ReportExport.SuggestFileName(kind, new DateOnly(2026, 8, 5));

        Assert.Equal(-1, name.IndexOfAny(Path.GetInvalidFileNameChars()));
        Assert.Equal("LernTor-Bericht-Ali-Ayse-2026-08-05.html", name);
    }

    [Fact]
    public void Dateiname_kommt_auch_ohne_brauchbaren_Namen_zustande()
    {
        Assert.Equal("LernTor-Bericht-2026-08-05.html", ReportExport.SuggestFileName("  ", new DateOnly(2026, 8, 5)));
        Assert.Equal("LernTor-Bericht-2026-08-05.html", ReportExport.SuggestFileName(null, new DateOnly(2026, 8, 5)));
        Assert.Equal("LernTor-Bericht-2026-08-05.html", ReportExport.SuggestFileName("???", new DateOnly(2026, 8, 5)));
    }

    [Fact]
    public void Datei_ist_eigenstaendig_und_laedt_nichts_nach()
    {
        // Ein Bericht, der ohne Internet nur halb aussieht, waere wertlos - und Nachladen von
        // aussen hat in einer Familien-App ohnehin nichts zu suchen.
        var html = ReportExport.ToHtml(Dokument(
            abschnitte: new ReportExportSection("Fächer", new[]
            {
                new ReportExportRow("Mathematik", "80 %", 0.8)
            })));

        Assert.DoesNotContain("http://", html);
        Assert.DoesNotContain("https://", html);
        Assert.DoesNotContain("<script", html);
        Assert.Contains("<style>", html);
    }
}
