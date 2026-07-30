namespace LernTor.Core.Services;

/// <summary>
/// Liest eine von Eltern eingefügte Vokabelliste ein - eine Zeile je Wortpaar.
///
/// <para>Niemand tippt dreißig Vokabelpaare in einzelne Eingabefelder. Eltern haben die Liste
/// ohnehin schon: abgeschrieben aus dem Vokabelheft oder kopiert aus einer Nachricht der
/// Lehrkraft. Deshalb akzeptiert der Parser bewusst mehrere Trennzeichen, statt ein bestimmtes
/// Format zu verlangen: <c>=</c>, <c>-</c>, <c>–</c>, <c>;</c>, <c>:</c> und Tabulator (beim
/// Einfügen aus einer Tabelle).</para>
///
/// <para>Ein Bindestrich <em>innerhalb</em> eines Wortes darf dabei nicht trennen
/// ("Fußball-Verein = football club"), deshalb trennt der Parser am ersten Trennzeichen, das von
/// Leerzeichen umgeben ist - und nur wenn kein eindeutigeres Zeichen wie <c>=</c> vorkommt.</para>
/// </summary>
public static class VocabularyParser
{
    /// <summary>Eindeutige Trennzeichen: dürfen auch ohne umgebende Leerzeichen trennen.</summary>
    private static readonly char[] StrongSeparators = { '=', '\t', ';' };

    /// <summary>Schwache Trennzeichen: nur mit Leerzeichen ringsum, sonst wären Bindestrich-Wörter kaputt.</summary>
    private static readonly string[] WeakSeparators = { " - ", " – ", " — ", " : " };

    /// <summary>Ein eingelesenes Wortpaar.</summary>
    public readonly record struct Pair(string German, string Foreign);

    /// <summary>
    /// Liest den eingefügten Text ein. Leere Zeilen, Zeilen ohne Trennzeichen und Zeilen mit
    /// leerer Seite werden übersprungen - eine halb abgeschriebene Liste soll nicht die ganze
    /// Eingabe verwerfen. Doppelte Paare (gleiches deutsches Wort) kommen nur einmal zurück.
    /// </summary>
    public static IReadOnlyList<Pair> Parse(string? text)
    {
        var result = new List<Pair>();
        if (string.IsNullOrWhiteSpace(text))
        {
            return result;
        }

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in text.Split('\n'))
        {
            var line = rawLine.Replace('\r', ' ').Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (!TrySplit(line, out var german, out var foreign))
            {
                continue;
            }

            if (german.Length == 0 || foreign.Length == 0)
            {
                continue;
            }

            if (seen.Add(german))
            {
                result.Add(new Pair(german, foreign));
            }
        }

        return result;
    }

    private static bool TrySplit(string line, out string german, out string foreign)
    {
        german = string.Empty;
        foreign = string.Empty;

        var index = line.IndexOfAny(StrongSeparators);
        var separatorLength = 1;

        if (index < 0)
        {
            foreach (var weak in WeakSeparators)
            {
                var weakIndex = line.IndexOf(weak, StringComparison.Ordinal);
                if (weakIndex >= 0 && (index < 0 || weakIndex < index))
                {
                    index = weakIndex;
                    separatorLength = weak.Length;
                }
            }
        }

        if (index < 0)
        {
            return false;
        }

        german = line[..index].Trim();
        foreign = line[(index + separatorLength)..].Trim();
        return true;
    }
}
