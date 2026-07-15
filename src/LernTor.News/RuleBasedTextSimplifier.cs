using System.Text.RegularExpressions;

namespace LernTor.News;

/// <summary>
/// Einfache regelbasierte Vereinfachung: HTML entfernen, auf kurze Sätze kürzen,
/// einige gängige komplexe Wörter durch kindgerechtere Begriffe ersetzen.
/// Kein Ersatz für echtes NLP, aber lauffähig ohne externe Abhängigkeiten.
/// </summary>
public sealed partial class RuleBasedTextSimplifier : ITextSimplifier
{
    private const int MaxSentences = 3;
    private const int MaxTotalLength = 500;

    private static readonly Dictionary<string, string> Vereinfachungen = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Koalition"] = "Bündnis mehrerer Parteien",
        ["Verhandlungen"] = "Gespräche",
        ["Diplomaten"] = "Vertreter der Regierung",
        ["Infrastruktur"] = "Straßen, Brücken und Gebäude",
        ["Bevölkerung"] = "Menschen, die dort leben",
        ["signifikant"] = "deutlich",
        ["implementieren"] = "einführen",
        ["Ministerium"] = "Behörde der Regierung",
    };

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex(@"(?<=[.!?])\s+")]
    private static partial Regex SentenceSplitRegex();

    public string Simplify(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
        {
            return "Zu diesem Artikel gibt es noch keine Zusammenfassung.";
        }

        var withoutHtml = HtmlTagRegex().Replace(rawText, " ").Trim();
        var normalizedWhitespace = Regex.Replace(withoutHtml, @"\s+", " ");

        var sentences = SentenceSplitRegex()
            .Split(normalizedWhitespace)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Take(MaxSentences)
            .ToList();

        var simplified = string.Join(" ", sentences);

        foreach (var (complex, simple) in Vereinfachungen)
        {
            simplified = Regex.Replace(simplified, Regex.Escape(complex), simple, RegexOptions.IgnoreCase);
        }

        if (simplified.Length > MaxTotalLength)
        {
            simplified = simplified[..MaxTotalLength].TrimEnd() + "…";
        }

        return simplified;
    }
}
