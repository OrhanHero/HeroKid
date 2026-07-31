using System.Text;
using System.Text.RegularExpressions;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>Ergebnis eines Lesetext-Imports.</summary>
/// <param name="Title">Erratene Überschrift (leer, wenn keine erkennbar war).</param>
/// <param name="Body">Aufbereiteter Fließtext.</param>
/// <param name="CleanedLength">Länge des aufbereiteten Textes VOR dem Kürzen.</param>
/// <param name="WasTruncated">Ob gekürzt werden musste.</param>
public sealed record ImportedReadingText(string Title, string Body, int CleanedLength, bool WasTruncated)
{
    public bool HasText => Body.Length > 0;
}

/// <summary>
/// Macht aus dem Rohtext einer PDF-/Word-Datei einen brauchbaren Lesetext.
///
/// <para>Eltern hatten bisher nur ein leeres Textfeld: einen Auszug aus dem Lesebuch der Schule
/// einzutragen hieß abtippen. Die Textextraktoren für den Lehrer-Import
/// (<see cref="ITeacherDocumentTextExtractor"/>) sind längst da - was fehlte, war das Stück
/// dazwischen, denn <b>roher</b> Extraktionstext taugt nicht als Lesetext: PDF-Seiten kommen mit
/// harten Zeilenumbrüchen mitten im Satz, am Zeilenende getrennte Wörter ("Wan-\nderer"), Seiten-
/// zahlen und Kopfzeilen zwischen den Absätzen. Einem Kind so etwas vorzusetzen wäre schlimmer,
/// als es abzutippen.</para>
///
/// <para>Bewusst rein regelbasiert und ohne LLM: das Modell ist mehrere Gigabyte groß, muss beim
/// ersten Gebrauch heruntergeladen werden und braucht auf einem Familien-PC spürbar Zeit - für
/// "Zeilenumbrüche zusammenfassen" ist das die falsche Größenordnung. Die Eltern sehen den
/// aufbereiteten Text ohnehin im Formular und können ihn vor dem Speichern korrigieren.</para>
/// </summary>
public static class ReadingTextImport
{
    /// <summary>Längenobergrenze wie im Formular für eigene Lesetexte.</summary>
    public const int DefaultMaxLength = 4000;

    /// <summary>Bis zu dieser Länge gilt eine allein stehende erste Zeile als Überschrift.</summary>
    public const int MaxTitleLength = 80;

    /// <summary>Zeilen, die nur aus einer Seitenzahl bestehen ("7", "- 7 -", "Seite 7", "7 von 12").</summary>
    private static readonly Regex PageNumberLine = new(
        @"^\s*(?:[-–—]\s*)?(?:seite\s*|s\.\s*)?\d{1,4}(?:\s*(?:von|/)\s*\d{1,4})?\s*(?:[-–—])?\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    /// <summary>Am Zeilenende getrenntes Wort: "Wan-\nderer" gehört wieder zusammen.</summary>
    private static readonly Regex HyphenatedLineBreak = new(
        @"(\p{Ll})-[^\S\n]*\n[^\S\n]*(\p{Ll})", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    private static readonly Regex HorizontalWhitespace = new(
        @"[^\S\n]+", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    /// <summary>Satzende, an dem sich sauber abschneiden lässt.</summary>
    private static readonly Regex SentenceEnd = new(
        @"[.!?…](?=\s|$)", RegexOptions.Compiled | RegexOptions.RightToLeft, TimeSpan.FromSeconds(1));

    public static ImportedReadingText FromRawText(string? raw, int maxLength = DefaultMaxLength)
    {
        var cleaned = Clean(raw);
        if (cleaned.Length == 0)
        {
            return new ImportedReadingText(string.Empty, string.Empty, 0, WasTruncated: false);
        }

        var (title, body) = SplitTitle(cleaned);
        var cleanedLength = body.Length;
        var truncated = TruncateAtBoundary(body, maxLength);

        return new ImportedReadingText(title, truncated, cleanedLength, truncated.Length < cleanedLength);
    }

    internal static string Clean(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        // Weiches Trennzeichen (U+00AD) und Nullbreiten-Leerzeichen (U+200B) stecken massenhaft
        // in extrahiertem PDF-Text, sind unsichtbar und zerlegen trotzdem Woerter. Geschuetzte
        // und schmale Leerzeichen faengt weiter unten HorizontalWhitespace ab (\s deckt sie in
        // .NET ab, diese beiden hier aber nicht - deshalb explizit).
        var text = raw
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Replace("\u00ad", string.Empty)
            .Replace("\u200b", string.Empty);

        text = HyphenatedLineBreak.Replace(text, "$1$2");

        var paragraphs = new List<string>();
        var current = new StringBuilder();

        foreach (var rawLine in text.Split('\n'))
        {
            var line = HorizontalWhitespace.Replace(rawLine, " ").Trim();

            if (line.Length == 0)
            {
                Flush(paragraphs, current);
                continue;
            }

            // Seitenzahlen/Kopfzeilen trennen im Original zwei Absätze - sie einfach zu löschen
            // würde die beiden zu einem verschmelzen, deshalb zählen sie wie eine Leerzeile.
            if (PageNumberLine.IsMatch(line))
            {
                Flush(paragraphs, current);
                continue;
            }

            if (current.Length > 0)
            {
                current.Append(' ');
            }

            current.Append(line);
        }

        Flush(paragraphs, current);

        return string.Join("\n\n", paragraphs);
    }

    private static void Flush(List<string> paragraphs, StringBuilder current)
    {
        var paragraph = current.ToString().Trim();
        current.Clear();

        if (paragraph.Length > 0)
        {
            paragraphs.Add(paragraph);
        }
    }

    /// <summary>
    /// Trennt eine Überschrift ab, wenn der erste Absatz kurz ist und nicht wie ein Satz endet.
    /// Im Zweifel bleibt alles im Text - eine falsch abgeschnittene erste Zeile fehlt im Lesetext,
    /// eine nicht erkannte Überschrift steht nur an der falschen Stelle.
    /// </summary>
    internal static (string Title, string Body) SplitTitle(string cleaned)
    {
        var parts = cleaned.Split("\n\n", 2, StringSplitOptions.None);
        if (parts.Length < 2)
        {
            return (string.Empty, cleaned);
        }

        var first = parts[0].Trim();
        var endsLikeSentence = first.Length > 0 && ".!?…:;,".Contains(first[^1]);

        return first.Length is > 0 and <= MaxTitleLength && !endsLikeSentence
            ? (first, parts[1].Trim())
            : (string.Empty, cleaned);
    }

    /// <summary>
    /// Kürzt auf die Höchstlänge, bevorzugt an einer Absatz-, sonst an einer Satzgrenze. Mitten im
    /// Wort abzuschneiden wäre für ein vorlesendes Kind das schlechteste Ergebnis.
    /// </summary>
    internal static string TruncateAtBoundary(string text, int maxLength)
    {
        if (maxLength <= 0)
        {
            return string.Empty;
        }

        if (text.Length <= maxLength)
        {
            return text;
        }

        var window = text[..maxLength];

        var paragraphBreak = window.LastIndexOf("\n\n", StringComparison.Ordinal);
        if (paragraphBreak > maxLength / 2)
        {
            return window[..paragraphBreak].TrimEnd();
        }

        var sentence = SentenceEnd.Match(window);
        if (sentence.Success && sentence.Index > maxLength / 2)
        {
            return window[..(sentence.Index + 1)].TrimEnd();
        }

        var space = window.LastIndexOf(' ');
        return (space > 0 ? window[..space] : window).TrimEnd();
    }
}
