using System.Globalization;

namespace LernTor.Core.Design;

/// <summary>
/// Kontrastverhältnis zweier Farben nach WCAG 2.2 (relative Leuchtdichte, 1 : 1 bis 21 : 1).
/// </summary>
public static class ColorContrast
{
    /// <summary>Mindestkontrast für normalen Text (WCAG 2.2, 1.4.3, Stufe AA).</summary>
    public const double Text = 4.5;

    /// <summary>Mindestkontrast für große Schrift und grafische Elemente (1.4.3/1.4.11).</summary>
    public const double Graphic = 3.0;

    public static double Ratio(string hexA, string hexB)
    {
        var a = RelativeLuminance(hexA);
        var b = RelativeLuminance(hexB);
        var (hell, dunkel) = a > b ? (a, b) : (b, a);
        return (hell + 0.05) / (dunkel + 0.05);
    }

    public static double RelativeLuminance(string hex)
    {
        var (r, g, b) = Parse(hex);
        return 0.2126 * Linear(r) + 0.7152 * Linear(g) + 0.0722 * Linear(b);
    }

    public static (byte R, byte G, byte B) Parse(string hex)
    {
        var s = hex.TrimStart('#');
        if (s.Length != 6)
        {
            throw new FormatException($"Farbe '{hex}' ist nicht im Format #RRGGBB.");
        }

        return (
            byte.Parse(s[..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(s[2..4], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(s[4..], NumberStyles.HexNumber, CultureInfo.InvariantCulture));
    }

    private static double Linear(byte channel)
    {
        var c = channel / 255.0;
        return c <= 0.03928 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
    }
}
