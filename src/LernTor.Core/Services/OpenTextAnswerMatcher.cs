using System.Text;
using System.Text.RegularExpressions;

namespace LernTor.Core.Services;

/// <summary>
/// Prüft eine frei eingetippte Antwort gegen eine erwartete Lösung.
///
/// <para><b>Warum nicht mehr "enthält":</b> bis 30.09.2026 galt eine offene Antwort als richtig,
/// sobald die Lösung irgendwo darin vorkam. Das hatte drei echte Folgen: "25" war richtig, wenn
/// "5" gesucht war (und "-5", "50", "15"); wer "gelse gelir geliyor" eintippte, hatte jede
/// Zeitform-Aufgabe richtig; und "kitabımız" (unser Buch) zählte für "kitabım" (mein Buch).
/// Jetzt muss die Antwort der Lösung entsprechen - mit genau den Abweichungen, die kein Raten
/// erlauben:</para>
/// <list type="bullet">
/// <item>Groß-/Kleinschreibung (außer bei <c>caseSensitive</c>), Leerzeichen, Anführungszeichen,
/// Satzzeichen am Ende, typografische Apostrophe, <c>^2</c> statt <c>²</c>, <c>36pi</c> statt
/// <c>36π</c>;</item>
/// <item>türkische Sonderzeichen ohne türkische Tastatur (ç→c, ğ→g, ı→i, ş→s, ö→o, ü→u);</item>
/// <item>bei Zahlen: eine Einheit dahinter ("42 €", "320 cm³") und ein "x =" davor, bei
/// Dezimalzahlen auch das Komma statt des Punkts - Ziffern außerhalb der Zahl nie;</item>
/// <item>ein Artikel oder "to" davor ("der Hund", "to swim") - außer die Lösung ist selbst ein
/// Artikel;</item>
/// <item>der ganze Lückensatz statt nur der Lücke - aber nur genau dieser Satz.</item>
/// </list>
/// </summary>
public static class OpenTextAnswerMatcher
{
    private static readonly HashSet<string> Begleitwoerter = new(StringComparer.Ordinal)
    {
        "der", "die", "das", "den", "dem", "des", "ein", "eine", "einen", "einem", "einer",
        "the", "a", "an", "to"
    };

    private static readonly Regex Zahl = new(@"^[+-]?\d+(?:[.,/]\d+)?π?$", RegexOptions.CultureInvariant);

    private static readonly Regex ZahlMitEinheit = new(
        @"^(?:[a-z]\s*=\s*)?(?<zahl>[+-]?\d+(?:[.,/]\d+)?\s*π?)\s*(?<einheit>\D{0,24})$",
        RegexOptions.CultureInvariant);

    private static readonly Regex Lueckensatz = new("\"(?<satz>[^\"]*___[^\"]*)\"", RegexOptions.CultureInvariant);
    private static readonly Regex LueckeMitHinweis = new(@"_{3,}\s*(?:\([^)]*\))?", RegexOptions.CultureInvariant);
    private static readonly Regex Leerraum = new(@"\s+", RegexOptions.CultureInvariant);
    private static readonly Regex PiAlsWort = new(@"(?<=\d)\s*pi\b", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static bool Matches(string correct, string given, string? prompt = null, bool caseSensitive = false)
    {
        if (string.IsNullOrWhiteSpace(correct) || string.IsNullOrWhiteSpace(given))
        {
            return false;
        }

        var erwartet = Normalize(correct);
        var antwort = Normalize(given);

        if (Gleich(erwartet, antwort, caseSensitive) || Gleich(Kompakt(erwartet), Kompakt(antwort), caseSensitive))
        {
            return true;
        }

        if (!caseSensitive && Wortfolge(antwort).SequenceEqual(Wortfolge(erwartet)))
        {
            return true;
        }

        if (IstZahl(erwartet))
        {
            return ZahlStimmt(erwartet, antwort);
        }

        return MitBegleitwort(erwartet, antwort, caseSensitive)
            || (!caseSensitive && prompt is not null && GanzerLueckensatz(erwartet, antwort, prompt));
    }

    /// <summary>Vereinheitlicht alles, was nichts über die Richtigkeit aussagt.</summary>
    internal static string Normalize(string text)
    {
        var s = text.Normalize(NormalizationForm.FormC)
            .Replace('’', '\'').Replace('‘', '\'').Replace('`', '\'').Replace('´', '\'')
            .Replace('„', '"').Replace('“', '"').Replace('”', '"').Replace('«', '"').Replace('»', '"')
            .Replace("^2", "²").Replace("^3", "³");
        s = PiAlsWort.Replace(s, "π");
        s = Leerraum.Replace(s, " ").Trim();
        s = s.Trim('"', '\'', ' ');
        s = s.TrimEnd('.', '!', '?', ' ');
        return s;
    }

    private static bool Gleich(string a, string b, bool caseSensitive)
    {
        if (caseSensitive)
        {
            return string.Equals(a, b, StringComparison.Ordinal);
        }

        return string.Equals(a, b, StringComparison.OrdinalIgnoreCase)
            || string.Equals(Lateinisch(a), Lateinisch(b), StringComparison.Ordinal);
    }

    /// <summary>Ohne Leerzeichen und Apostrophe: "x = 3, y = -6", "( 5 , 1 )", "cant she".</summary>
    private static string Kompakt(string text) => Leerraum.Replace(text, "").Replace("'", "");

    /// <summary>
    /// Wörter ohne Satzzeichen, klein, in lateinischer Form: "walk, am taking" = "walk / am
    /// taking". Nur für Antworten mit Buchstaben - bei "3/4" wäre "3 4" sonst auch richtig.
    /// </summary>
    private static IReadOnlyList<string> Wortfolge(string text)
    {
        if (!text.Any(char.IsLetter))
        {
            return new[] { "\u0000" + text };
        }

        return Lateinisch(text)
            .Split(new[] { ' ', ',', ';', ':', '/', '(', ')', '"', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    private static bool IstZahl(string text) => Zahl.IsMatch(Leerraum.Replace(text, ""));

    /// <summary>
    /// "42 €", "320 cm³", "x = 9", "99 Grad" - die Zahl selbst muss exakt stimmen, dahinter darf
    /// nur Text ohne Ziffern stehen. So bleibt "25" falsch, wenn "5" gesucht ist.
    /// </summary>
    private static bool ZahlStimmt(string erwartet, string antwort)
    {
        var m = ZahlMitEinheit.Match(antwort.ToLowerInvariant());
        if (!m.Success)
        {
            return false;
        }

        var zahl = Leerraum.Replace(m.Groups["zahl"].Value, "");
        var soll = Leerraum.Replace(erwartet, "").ToLowerInvariant();
        if (zahl == soll)
        {
            return true;
        }

        // 0,5 statt 0.5 - nur wenn die Lösung eine Dezimalzahl mit Punkt ist. "-2,5" als zwei
        // Lösungen einer quadratischen Gleichung bleibt so von "-2.5" unterscheidbar.
        return soll.Contains('.') && !soll.Contains('/') && zahl.Replace(',', '.') == soll && !zahl.Contains('.');
    }

    private static bool MitBegleitwort(string erwartet, string antwort, bool caseSensitive)
    {
        var leer = antwort.IndexOf(' ');
        if (leer <= 0)
        {
            return false;
        }

        // Ist die Lösung selbst ein Artikel ("der Tisch" -> "der"), wäre "die der" sonst ein
        // Treffer - genau das Raten, das hier ausgeschlossen werden soll.
        if (Begleitwoerter.Contains(erwartet.ToLowerInvariant()))
        {
            return false;
        }

        var erstes = antwort[..leer].ToLowerInvariant();
        return Begleitwoerter.Contains(erstes) && Gleich(erwartet, antwort[(leer + 1)..], caseSensitive);
    }

    /// <summary>
    /// Das Kind hat den ganzen Satz aus der Aufgabe abgeschrieben und die Lücke gefüllt. Das ist
    /// richtig - aber nur, wenn die Wörter genau dem gefüllten Satz entsprechen. Bei mehreren
    /// Lücken ist die Lösung mit " / " getrennt ("walk / am taking").
    /// </summary>
    private static bool GanzerLueckensatz(string erwartet, string antwort, string prompt)
    {
        var m = Lueckensatz.Match(prompt);
        if (!m.Success)
        {
            return false;
        }

        var satz = m.Groups["satz"].Value;
        var luecken = LueckeMitHinweis.Matches(satz).Count;
        var teile = erwartet.Split(" / ");
        if (teile.Length != luecken)
        {
            teile = new[] { erwartet };
            if (luecken != 1)
            {
                return false;
            }
        }

        var i = 0;
        var gefuellt = LueckeMitHinweis.Replace(satz, treffer => teile[i++] + " ");
        return Wortfolge(Normalize(gefuellt)).SequenceEqual(Wortfolge(antwort));
    }

    /// <summary>
    /// Näherung für Kinder ohne türkische Tastatur, klein geschrieben. Wird nur zusätzlich zum
    /// genauen Vergleich genutzt.
    /// </summary>
    internal static string Lateinisch(string text)
    {
        var builder = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            builder.Append(ch switch
            {
                'ç' or 'Ç' => 'c',
                'ğ' or 'Ğ' => 'g',
                'ı' or 'İ' => 'i',
                'ş' or 'Ş' => 's',
                'ö' or 'Ö' => 'o',
                'ü' or 'Ü' => 'u',
                _ => char.ToLowerInvariant(ch)
            });
        }

        return builder.ToString().Trim();
    }
}
