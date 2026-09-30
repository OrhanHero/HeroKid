using System.Text.RegularExpressions;

namespace LernTor.Core.Services;

/// <summary>
/// Lösung und Rechenweg für (x ± b)² - an einer Stelle, weil der Generator und die
/// Fehler-Kartei beide darauf angewiesen sind.
///
/// <para><b>Hintergrund:</b> bis 30.09.2026 rechnete der Mathe-Generator das Mittelglied als
/// 2·a·b mit einem zufälligen a, obwohl die Aufgabe immer (x ± b)² lautete, also a = 1. Bei
/// acht von neun Aufgaben war die hinterlegte Lösung falsch ("(x + 9)² = x² + 36x + 81") - wer
/// richtig rechnete, bekam einen Fehler und die Aufgabe landete mit der falschen Lösung in der
/// Fehler-Kartei. <see cref="TryKorrigieren"/> repariert solche Karteikarten.</para>
/// </summary>
public static class BinomischeFormel
{
    public const string Thema = "Binomische Formeln";

    private static readonly Regex Aufgabe = new(@"\(x\s*(?<vz>[+-])\s*(?<b>\d+)\)²", RegexOptions.CultureInvariant);

    public static (string Aufgabe, string Loesung, string Erklaerung) Quadrat(int b, bool plus)
    {
        var vorzeichen = plus ? "+" : "-";
        var mitte = 2 * b;
        var hinten = b * b;
        return (
            $"(x {vorzeichen} {b})²",
            $"x² {vorzeichen} {mitte}x + {hinten}",
            $"(x{vorzeichen}{b})² = x² {vorzeichen} 2·x·{b} + {b}² = x² {vorzeichen} {mitte}x + {hinten}");
    }

    /// <summary>Liest (x ± b)² aus dem Aufgabentext und liefert die richtige Lösung.</summary>
    public static bool TryKorrigieren(string prompt, out string loesung, out string erklaerung)
    {
        var m = Aufgabe.Match(prompt);
        if (!m.Success || !int.TryParse(m.Groups["b"].Value, out var b))
        {
            loesung = erklaerung = string.Empty;
            return false;
        }

        (_, loesung, erklaerung) = Quadrat(b, m.Groups["vz"].Value == "+");
        return true;
    }
}
