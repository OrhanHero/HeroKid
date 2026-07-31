using LernTor.ContentGen.TeacherImport;
using Xunit;

namespace LernTor.Tests;

/// <summary>Lesetext-Import: aus rohem PDF-/Word-Text wird ein lesbarer Text.</summary>
public sealed class ReadingTextImportTests
{
    [Fact]
    public void Harte_Zeilenumbrueche_werden_zu_Absaetzen_zusammengefasst()
    {
        // Genau das macht rohen Extraktionstext unbrauchbar: der Umbruch sitzt mitten im Satz,
        // weil er im PDF nur vom Seitenlayout kommt.
        var cleaned = ReadingTextImport.Clean("Es war einmal ein\nWanderer, der durch\nden Wald ging.");

        Assert.Equal("Es war einmal ein Wanderer, der durch den Wald ging.", cleaned);
    }

    [Fact]
    public void Leerzeile_trennt_weiterhin_zwei_Absaetze()
    {
        Assert.Equal(
            "Erster Absatz.\n\nZweiter Absatz.",
            ReadingTextImport.Clean("Erster\nAbsatz.\n\nZweiter\nAbsatz."));
    }

    [Fact]
    public void Am_Zeilenende_getrennte_Woerter_werden_zusammengesetzt()
    {
        Assert.Equal("Ein Wanderer ging.", ReadingTextImport.Clean("Ein Wan-\nderer ging."));
        Assert.Equal("Ein Wanderer ging.", ReadingTextImport.Clean("Ein Wan- \n  derer ging."));
    }

    [Fact]
    public void Ein_echter_Bindestrich_bleibt_erhalten()
    {
        // Nicht jeder Bindestrich am Zeilenende ist eine Trennung - "Baden-Württemberg" darf nicht
        // zu "BadenWürttemberg" werden, wenn er nicht am Umbruch steht.
        Assert.Equal("Baden-Württemberg ist groß.", ReadingTextImport.Clean("Baden-Württemberg\nist groß."));
    }

    [Theory]
    [InlineData("7")]
    [InlineData("- 7 -")]
    [InlineData("Seite 7")]
    [InlineData("7 von 12")]
    public void Seitenzahlen_fliegen_raus(string zeile)
    {
        var cleaned = ReadingTextImport.Clean($"Erster Absatz.\n{zeile}\nZweiter Absatz.");

        Assert.DoesNotContain(zeile, cleaned);
        Assert.Contains("Erster Absatz.", cleaned);
        Assert.Contains("Zweiter Absatz.", cleaned);
    }

    [Fact]
    public void Eine_Seitenzahl_verschmilzt_die_Absaetze_nicht()
    {
        // Sie einfach zu loeschen haette den Satz davor mit dem danach verklebt.
        Assert.Equal(
            "Erster Absatz.\n\nZweiter Absatz.",
            ReadingTextImport.Clean("Erster Absatz.\n7\nZweiter Absatz."));
    }

    [Fact]
    public void Unsichtbare_Zeichen_verschwinden()
    {
        // Weiches Trennzeichen und Nullbreiten-Leerzeichen stecken massenhaft in PDF-Text.
        Assert.Equal("Wanderer", ReadingTextImport.Clean("Wan\u00adde\u200brer"));
    }

    [Fact]
    public void Kurze_erste_Zeile_wird_zur_Ueberschrift()
    {
        var imported = ReadingTextImport.FromRawText("Der Wanderer\n\nEs war einmal ein Wan-\nderer.");

        Assert.Equal("Der Wanderer", imported.Title);
        Assert.Equal("Es war einmal ein Wanderer.", imported.Body);
        Assert.True(imported.HasText);
    }

    [Fact]
    public void Ein_ganzer_Satz_am_Anfang_ist_keine_Ueberschrift()
    {
        // Im Zweifel bleibt alles im Text - eine faelschlich abgetrennte erste Zeile fehlte sonst.
        var (title, body) = ReadingTextImport.SplitTitle("Das ist schon ein Satz.\n\nDann mehr.");

        Assert.Equal(string.Empty, title);
        Assert.StartsWith("Das ist schon ein Satz.", body);
    }

    [Fact]
    public void Eine_zu_lange_erste_Zeile_ist_keine_Ueberschrift()
    {
        var lang = new string('X', ReadingTextImport.MaxTitleLength + 10);

        var (title, body) = ReadingTextImport.SplitTitle($"{lang}\n\nDann mehr.");

        Assert.Equal(string.Empty, title);
        Assert.Contains(lang, body);
    }

    [Fact]
    public void Kurzer_Text_wird_nicht_gekuerzt()
    {
        var imported = ReadingTextImport.FromRawText("Ein kurzer Text.", maxLength: 4000);

        Assert.False(imported.WasTruncated);
        Assert.Equal("Ein kurzer Text.", imported.Body);
        Assert.Equal(imported.Body.Length, imported.CleanedLength);
    }

    [Fact]
    public void Zu_langer_Text_wird_an_einer_Satzgrenze_gekuerzt()
    {
        var text = new string('A', 100) + ". " + new string('B', 100) + ". " + new string('C', 100);

        var gekuerzt = ReadingTextImport.TruncateAtBoundary(text, 150);

        Assert.EndsWith(".", gekuerzt);
        Assert.Equal(101, gekuerzt.Length);
        Assert.DoesNotContain("B", gekuerzt);
    }

    [Fact]
    public void Gekuerzt_wird_nie_mitten_im_Wort()
    {
        // Fuer ein vorlesendes Kind ist ein abgehacktes Wort das schlechteste Ergebnis.
        var text = string.Join(" ", Enumerable.Repeat("Wanderer", 40));

        var gekuerzt = ReadingTextImport.TruncateAtBoundary(text, 100);

        Assert.True(gekuerzt.Length <= 100);
        Assert.EndsWith("Wanderer", gekuerzt);
    }

    [Fact]
    public void Kuerzung_wird_gemeldet_und_die_Originallaenge_bleibt_sichtbar()
    {
        // Ohne die Meldung haetten Eltern gespeichert, ohne zu merken, dass der Rest fehlt.
        var text = string.Join(" ", Enumerable.Repeat("Wanderer", 40));

        var imported = ReadingTextImport.FromRawText(text, maxLength: 100);

        Assert.True(imported.WasTruncated);
        Assert.True(imported.CleanedLength > imported.Body.Length);
        Assert.Equal(text.Length, imported.CleanedLength);
    }

    [Fact]
    public void Leere_und_nur_aus_Leerzeichen_bestehende_Dateien_ergeben_nichts()
    {
        foreach (var roh in new string?[] { null, string.Empty, "   ", "\n\n \n" })
        {
            var imported = ReadingTextImport.FromRawText(roh);

            Assert.False(imported.HasText);
            Assert.Equal(string.Empty, imported.Title);
            Assert.False(imported.WasTruncated);
        }
    }

    [Fact]
    public void Windows_Zeilenenden_werden_wie_Unix_behandelt()
    {
        Assert.Equal(
            "Erster Absatz.\n\nZweiter Absatz.",
            ReadingTextImport.Clean("Erster\r\nAbsatz.\r\n\r\nZweiter\r\nAbsatz."));
    }
}
