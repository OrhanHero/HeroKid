namespace LernTor.Core.Design;

/// <summary>
/// Welche Farbrollen wie viel Kontrast zueinander brauchen - abgeleitet aus der tatsächlichen
/// Verwendung in den Ansichten (Stand 30.09.2026):
/// <list type="bullet">
/// <item>Text steht auf Hintergrund, Karten, Kacheln und Hervorhebungsflächen.</item>
/// <item>Hauptfarbe, Erfolg und Fehler sind Schriftfarbe (auf Hintergrund/Karte) UND Fläche mit
/// Schrift in <c>OnColor</c> darauf - daher beide Richtungen.</item>
/// <item>Fächerfarben und Fokusring sind große Schrift bzw. Grafik (3 : 1).</item>
/// </list>
/// Ein Design, das eine Regel verletzt, lässt die CI rot werden (<c>DesignThemeCatalogTests</c>).
/// </summary>
public static class DesignContrastRules
{
    public sealed record Rule(double Minimum, IReadOnlyList<string> Foreground, IReadOnlyList<string> Background);

    public static IReadOnlyList<Rule> All { get; } = new[]
    {
        new Rule(ColorContrast.Text,
            new[] { "TextPrimary" },
            new[] { "Background", "Surface", "Accent", "TileLavender", "TileSand", "TileMint", "TileRose" }),
        new Rule(ColorContrast.Text,
            new[] { "TextSecondary" },
            new[] { "Background", "Surface" }),
        new Rule(ColorContrast.Text,
            new[] { "OnColor" },
            new[] { "Primary", "PrimaryDark", "Success", "Error" }),
        new Rule(ColorContrast.Text,
            new[] { "Primary", "Success", "Error" },
            new[] { "Background", "Surface" }),
        new Rule(ColorContrast.Graphic,
            new[] { "Math", "German", "Turkish", "Science", "News", "Focus" },
            new[] { "Background", "Surface" }),
    };

    /// <summary>Alle Verstöße eines Designs als lesbarer Text; leer = in Ordnung.</summary>
    public static IReadOnlyList<string> Violations(DesignPalette palette)
    {
        var rollen = palette.Roles;
        var verstoesse = new List<string>();
        foreach (var regel in All)
        {
            foreach (var vorne in regel.Foreground)
            {
                foreach (var hinten in regel.Background)
                {
                    var kontrast = ColorContrast.Ratio(rollen[vorne], rollen[hinten]);
                    if (kontrast < regel.Minimum)
                    {
                        verstoesse.Add($"{vorne} auf {hinten}: {kontrast:0.00} : 1 (mindestens {regel.Minimum:0.0} : 1)");
                    }
                }
            }
        }

        return verstoesse;
    }
}
