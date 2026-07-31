using System.Text;

namespace LernTor.Core.Services;

/// <summary>Ein Wort im Vergleich zwischen Diktat und Tippergebnis.</summary>
/// <param name="Expected">Das erwartete Wort (leer, wenn das Kind eines zu viel geschrieben hat).</param>
/// <param name="Given">Das geschriebene Wort (leer, wenn eines fehlt).</param>
/// <param name="IsCorrect">Ob beide übereinstimmen.</param>
public readonly record struct DictationWord(string Expected, string Given, bool IsCorrect);

/// <summary>Das Ergebnis eines Diktats.</summary>
/// <param name="Words">Wort-für-Wort-Vergleich in der Reihenfolge des Diktats.</param>
/// <param name="CorrectCount">Richtig geschriebene Wörter.</param>
/// <param name="IsPerfect">Alles richtig - nur dann zählt das Diktat als bestanden.</param>
public sealed record DictationResult(IReadOnlyList<DictationWord> Words, int CorrectCount, bool IsPerfect)
{
    public int WordCount => Words.Count(word => word.Expected.Length > 0);

    public double Accuracy => WordCount > 0 ? (double)CorrectCount / WordCount : 0;
}

/// <summary>
/// Vergleicht ein diktiertes mit dem getippten Satz - Wort für Wort, damit ein Kind sieht,
/// <b>welches</b> Wort falsch war, statt nur "leider falsch".
///
/// <para><b>Satzzeichen entscheiden bewusst nicht über richtig/falsch.</b> Die Sprachausgabe
/// spricht kein Komma, und einem Kind vorzuwerfen, es habe etwas nicht geschrieben, das es nicht
/// hören konnte, wäre schlicht unfair. Ein fehlender Punkt am Satzende macht die Rechtschreibung
/// des Wortes davor nicht falsch.</para>
///
/// <para><b>Groß- und Kleinschreibung entscheidet dagegen sehr wohl.</b> Sie ist im Deutschen
/// regelbasiert und damit ableitbar - "der hund" statt "der Hund" ist genau der Fehler, um den
/// es bei einem Diktat geht. Ohne diese Prüfung bliebe von der deutschen Rechtschreibung
/// wenig übrig.</para>
///
/// <para>Bestanden ist nur ein fehlerfreies Diktat. Das ist streng, aber die Sätze sind kurz
/// (ein Satz, nicht ein Text), die Rückmeldung zeigt genau die falschen Wörter, und die
/// Fehler-Kartei bringt den Satz später wieder - der Effekt ist Wiederholung, keine Strafe.</para>
/// </summary>
public static class DictationEvaluator
{
    /// <summary>Satzzeichen, die am Wortrand ignoriert werden (siehe Klassenkommentar).</summary>
    private const string BoundaryPunctuation = ".,;:!?…\"'„“”‚‘’()[]-–—";

    public static DictationResult Evaluate(string? expectedSentence, string? givenSentence)
    {
        var expected = SplitWords(expectedSentence);
        var given = SplitWords(givenSentence);

        var words = new List<DictationWord>();
        var correct = 0;

        for (var i = 0; i < Math.Max(expected.Count, given.Count); i++)
        {
            var expectedWord = i < expected.Count ? expected[i] : string.Empty;
            var givenWord = i < given.Count ? given[i] : string.Empty;
            var isCorrect = expectedWord.Length > 0 && string.Equals(expectedWord, givenWord, StringComparison.Ordinal);

            if (isCorrect)
            {
                correct++;
            }

            words.Add(new DictationWord(expectedWord, givenWord, isCorrect));
        }

        // Ein zu viel geschriebenes Wort am Ende macht das Diktat falsch, auch wenn alle
        // erwarteten Wörter stimmen - sonst käme "Der Hund bellt laut laut laut" durch.
        var isPerfect = expected.Count > 0 && given.Count == expected.Count && correct == expected.Count;

        return new DictationResult(words, correct, isPerfect);
    }

    /// <summary>Nur die Wörter - Satzzeichen an den Rändern fallen weg, Leerraum wird egalisiert.</summary>
    internal static IReadOnlyList<string> SplitWords(string? sentence)
    {
        if (string.IsNullOrWhiteSpace(sentence))
        {
            return Array.Empty<string>();
        }

        return sentence
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .Select(Strip)
            .Where(word => word.Length > 0)
            .ToList();
    }

    private static string Strip(string word)
    {
        var start = 0;
        var end = word.Length - 1;

        while (start <= end && BoundaryPunctuation.Contains(word[start]))
        {
            start++;
        }

        while (end >= start && BoundaryPunctuation.Contains(word[end]))
        {
            end--;
        }

        return start > end ? string.Empty : word[start..(end + 1)];
    }

    /// <summary>
    /// Die Rückmeldung als ein Satz: falsche Wörter mit dem, was richtig gewesen wäre. Bewusst
    /// nur die Fehler - eine Liste aller zwölf Wörter mit Häkchen wäre unlesbar.
    /// </summary>
    public static string Feedback(DictationResult result)
    {
        if (result.IsPerfect)
        {
            return "Fehlerfrei geschrieben.";
        }

        var wrong = result.Words.Where(word => !word.IsCorrect).ToList();
        if (wrong.Count == 0)
        {
            // Alle erwarteten Wörter stimmen, es steht aber etwas zu viel da.
            return "Alle Wörter stimmen, aber es steht mehr da, als diktiert wurde.";
        }

        var builder = new StringBuilder();
        foreach (var word in wrong.Take(6))
        {
            if (builder.Length > 0)
            {
                builder.Append("  ·  ");
            }

            builder.Append(word.Expected.Length == 0
                ? $"zu viel: \"{word.Given}\""
                : word.Given.Length == 0
                    ? $"fehlt: \"{word.Expected}\""
                    : $"\"{word.Given}\" → \"{word.Expected}\"");
        }

        if (wrong.Count > 6)
        {
            builder.Append($"  ·  und {wrong.Count - 6} weitere");
        }

        return builder.ToString();
    }
}
