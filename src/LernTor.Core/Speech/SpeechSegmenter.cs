using System.Text.RegularExpressions;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Speech;

/// <summary>
/// Zerlegt eine Frage in Abschnitte, die jeweils in EINER Sprache vorgelesen werden
/// (docs/NAECHSTES-LEVEL-3-1.md, Schritt 1).
///
/// <para>Warum überhaupt zerlegen: Englisch- und Türkischfragen sind gemischt - „Setze die
/// richtige Form ein: "She ___ to school every day."“. Liest die deutsche Stimme den englischen
/// Satz, lernt das Kind eine falsche Aussprache; liest die englische Stimme die deutsche
/// Anweisung, versteht es die Aufgabe nicht. Die Regeln:</para>
/// <list type="bullet">
/// <item>Zitate gehören zur Sprache des Fachs - außer die Frage verlangt ausdrücklich eine
/// Übersetzung IN diese Sprache („Wie sagt man auf Englisch "Umweltschutz"?“): dann ist das
/// Zitat Deutsch.</item>
/// <item>Der übrige Text wird nach typischen Wörtern eingeordnet; ohne Zeichen gilt bei Englisch
/// Deutsch (die Anweisungen sind deutsch), bei Türkisch Türkisch (die Aufgaben sind es).</item>
/// <item>Antwortmöglichkeiten und Lösung stehen in der Antwortsprache (siehe
/// <see cref="AnswerLanguage"/>) - einzelne Wörter einzuordnen wäre Raten: „Big Ben“ enthält das
/// türkische „ben“, „Her cumartesi“ das englische „her“.</item>
/// </list>
/// <para>
///
/// <para>Bewusst vorsichtig: ohne klares Zeichen bleibt es bei der Rückfallsprache. Eine
/// Fehleinschätzung klingt schlimmstenfalls falsch, die Frage bleibt lösbar - sie steht ja da.</para>
/// </summary>
public static class SpeechSegmenter
{
    /// <summary>Ein Zitat in geraden oder typografischen Anführungszeichen. Einfache Hochkommas
    /// zählen nicht: im Englischen („don't“) und Türkischen („-di'li“) sind das Apostrophe.</summary>
    private static readonly Regex Zitat = new(@"[""„“«]([^""„“”«»]+)[""“”»]", RegexOptions.Compiled);

    private static readonly Regex Wort = new(@"\p{L}+", RegexOptions.Compiled);

    /// <summary>„(Beispiel: kapı -> kapılar)“ - das Beispiel steht in der Fachsprache und wird
    /// wie ein Zitat behandelt, nur „Beispiel“ bleibt deutsch.</summary>
    private static readonly Regex Beispiel = new(@"\(Beispiel:\s*([^)]*)\)", RegexOptions.Compiled);

    /// <summary>Antwortwörter bei Wahr/Falsch-Fragen: sie stehen fest in einer Sprache, egal in
    /// welchem Fach.</summary>
    private static readonly Dictionary<string, SpeechLanguage> FesteAntwortwoerter = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Wahr"] = SpeechLanguage.Deutsch, ["Falsch"] = SpeechLanguage.Deutsch, ["Richtig"] = SpeechLanguage.Deutsch,
        ["Ja"] = SpeechLanguage.Deutsch, ["Nein"] = SpeechLanguage.Deutsch, ["Stimmt"] = SpeechLanguage.Deutsch,
        ["Stimmt nicht"] = SpeechLanguage.Deutsch,
        ["True"] = SpeechLanguage.Englisch, ["False"] = SpeechLanguage.Englisch,
        ["Yes"] = SpeechLanguage.Englisch, ["No"] = SpeechLanguage.Englisch,
        ["Doğru"] = SpeechLanguage.Tuerkisch, ["Yanlış"] = SpeechLanguage.Tuerkisch,
        ["Evet"] = SpeechLanguage.Tuerkisch, ["Hayır"] = SpeechLanguage.Tuerkisch
    };

    // Nur Wörter, die in den anderen beiden Sprachen nicht vorkommen. "was", "will", "in",
    // "an", "on" (türkisch: zehn), "at" (türkisch: Pferd), "o", "de" fehlen deshalb absichtlich.
    private static readonly HashSet<string> DeutscheWoerter = new(StringComparer.Ordinal)
    {
        "der", "die", "das", "und", "ist", "sind", "ein", "eine", "einen", "einem", "einer", "welche",
        "welcher", "welches", "welchen", "wie", "wer", "wo", "warum", "setze", "wähle", "ergänze",
        "bilde", "übersetze", "schreibe", "richtige", "richtigen", "form", "satz", "wort", "passt",
        "lücke", "heißt", "auf", "deutsch", "englisch", "türkisch", "beispiel", "nicht", "mit", "von",
        "zu", "im", "dem", "den", "des", "für", "oder", "aus", "bei", "nach", "wahr", "falsch",
        "richtig", "stimmt", "ja", "nein", "wandle", "um", "verb", "zeitform", "gegenteil", "mehrzahl",
        "bedeutet", "bedeutung", "sich", "wird", "werden", "hat", "haben", "kein", "keine", "gibt"
    };

    private static readonly HashSet<string> EnglischeWoerter = new(StringComparer.Ordinal)
    {
        "the", "is", "are", "were", "what", "which", "who", "how", "why", "where", "do", "does", "did",
        "you", "your", "he", "she", "it", "they", "we", "of", "to", "and", "or", "this", "that", "these",
        "those", "have", "has", "would", "can", "could", "should", "must", "my", "his", "her", "their",
        "our", "for", "with", "every", "not", "be", "been", "true", "false", "yes", "no", "i", "me",
        "am", "there", "if", "than", "because", "when", "yesterday", "tomorrow", "always", "never"
    };

    private static readonly HashSet<string> TuerkischeWoerter = new(StringComparer.Ordinal)
    {
        "bir", "ve", "bu", "şu", "ne", "nedir", "hangisi", "hangisidir", "hangi", "kelimesinin",
        "kelime", "kelimenin", "cümlede", "cümlenin", "cümle", "için", "ile", "gibi", "mi", "mı", "mu",
        "mü", "değil", "var", "yok", "olan", "doğru", "yanlış", "yaz", "yazınız", "ekle", "eki", "ekini",
        "fiil", "fiilinin", "zaman", "hâlini", "halini", "anlamı", "anlamlısı", "kaç", "kim", "nerede",
        "neden", "nasıl", "ben", "sen", "biz", "siz", "onlar", "da", "evet", "hayır", "çoğul", "hâli",
        "almancası", "türkçesi", "sözcük", "sözcüğü", "sözcüğün", "aşağıdaki", "hangisinde", "vardır"
    };

    /// <summary>Die Frage und - bei Auswahlfragen - die Antwortmöglichkeiten in der angezeigten
    /// Reihenfolge.</summary>
    public static IReadOnlyList<SpeechSegment> ForQuestion(QuizQuestion question, IReadOnlyList<string> displayedOptions)
    {
        var fach = SpeechLanguages.ForQuestion(question);
        var abschnitte = new List<SpeechSegment>(Split(question.Prompt, fach));

        var antwortSprache = AnswerLanguage(question.Prompt, fach);
        foreach (var option in displayedOptions)
        {
            // Satzende anhängen: so macht jede Stimme zwischen den Möglichkeiten eine Pause.
            Add(abschnitte, MitSatzende(option), AntwortSprache(option, antwortSprache));
        }

        return Merge(abschnitte);
    }

    /// <summary>Die (erste) richtige Antwort - zum Anhören nach dem Antworten.</summary>
    public static IReadOnlyList<SpeechSegment> ForSolution(QuizQuestion question)
    {
        var loesung = question.CorrectAnswers.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(loesung))
        {
            return Array.Empty<SpeechSegment>();
        }

        var fach = SpeechLanguages.ForQuestion(question);
        var abschnitte = new List<SpeechSegment>();
        Add(abschnitte, loesung, AntwortSprache(loesung, AnswerLanguage(question.Prompt, fach)));
        return abschnitte;
    }

    /// <summary>
    /// Zerlegt einen Text. Bei Deutsch als Fachsprache bleibt er ein Stück; bei
    /// Englisch/Türkisch gehört Zitiertes zur Fachsprache und der Rest wird nach typischen
    /// Wörtern eingeordnet.
    /// </summary>
    public static IReadOnlyList<SpeechSegment> Split(string text, SpeechLanguage subjectLanguage)
    {
        var abschnitte = new List<SpeechSegment>();
        if (string.IsNullOrWhiteSpace(text))
        {
            return abschnitte;
        }

        if (subjectLanguage == SpeechLanguage.Deutsch)
        {
            // Deutsch bleibt Deutsch: Zitate in deutschen Aufgaben sind Beispiele, Begriffe oder
            // Rechenausdrücke, keine fremdsprachigen Sätze.
            Add(abschnitte, text, SpeechLanguage.Deutsch);
            return Merge(abschnitte);
        }

        text = Beispiel.Replace(text, treffer => $"Beispiel: \"{treffer.Groups[1].Value}\"");

        // Verlangt die Frage eine Übersetzung IN die Fachsprache, ist das Zitat die deutsche
        // Vorlage; sonst ist es der Satz in der Fachsprache, um den es geht.
        var zitatSprache = ExplicitTarget(text) == subjectLanguage ? SpeechLanguage.Deutsch : subjectLanguage;
        var rueckfall = subjectLanguage == SpeechLanguage.Tuerkisch ? SpeechLanguage.Tuerkisch : SpeechLanguage.Deutsch;

        var position = 0;
        foreach (Match zitat in Zitat.Matches(text))
        {
            var davor = text[position..zitat.Index];
            Add(abschnitte, davor, Classify(davor, rueckfall));
            Add(abschnitte, zitat.Groups[1].Value, zitatSprache);
            position = zitat.Index + zitat.Length;
        }

        var rest = text[position..];
        Add(abschnitte, rest, Classify(rest, rueckfall));
        return Merge(abschnitte);
    }

    /// <summary>
    /// Ordnet einen Text einer Sprache zu: Punkte für typische Wörter und für Buchstaben, die
    /// es nur im Türkischen (ı ş ğ) oder nur im Deutschen (ä ß) gibt. Ohne jedes Zeichen - und
    /// bei Gleichstand mit der Rückfallsprache - gilt <paramref name="fallback"/>.
    /// </summary>
    public static SpeechLanguage Classify(string text, SpeechLanguage fallback)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return fallback;
        }

        int deutsch = 0, englisch = 0, tuerkisch = 0;

        // "İ" würde invariant zu "i̇" (mit Kombinationspunkt) - vorher ersetzen.
        var klein = text.Replace('İ', 'i').ToLowerInvariant();
        foreach (Match treffer in Wort.Matches(klein))
        {
            var wort = treffer.Value;
            if (DeutscheWoerter.Contains(wort)) deutsch += 2;
            if (EnglischeWoerter.Contains(wort)) englisch += 2;
            if (TuerkischeWoerter.Contains(wort)) tuerkisch += 2;
            if (wort.IndexOfAny(['ı', 'ş', 'ğ']) >= 0) tuerkisch += 2;
            if (wort.IndexOfAny(['ä', 'ß']) >= 0) deutsch += 1;
        }

        if (text.Contains('İ'))
        {
            tuerkisch += 2;
        }

        var bester = Math.Max(deutsch, Math.Max(englisch, tuerkisch));
        if (bester == 0 || Punkte(fallback) == bester)
        {
            return fallback;
        }

        return deutsch == bester ? SpeechLanguage.Deutsch
            : englisch == bester ? SpeechLanguage.Englisch
            : SpeechLanguage.Tuerkisch;

        int Punkte(SpeechLanguage sprache) => sprache switch
        {
            SpeechLanguage.Englisch => englisch,
            SpeechLanguage.Tuerkisch => tuerkisch,
            _ => deutsch
        };
    }

    /// <summary>
    /// In welcher Sprache die Antworten stehen. Grundsätzlich in der Fachsprache - außer die
    /// Frage verlangt ausdrücklich eine andere („kelimesinin Almancası hangisidir?“ hat deutsche
    /// Antworten, „Wie heißt … auf Englisch?“ englische).
    /// </summary>
    public static SpeechLanguage AnswerLanguage(string prompt, SpeechLanguage subjectLanguage)
    {
        return subjectLanguage == SpeechLanguage.Deutsch
            ? SpeechLanguage.Deutsch
            : ExplicitTarget(prompt) ?? subjectLanguage;
    }

    /// <summary>
    /// Die Sprache, in die die Frage ausdrücklich übersetzen lässt („Almancası“, „auf Englisch“),
    /// sonst null. Gesucht wird nur AUSSERHALB von Zitaten: ein Lesetext wie „Murat Türkçe ve
    /// Almanca konuşuyor“ verlangt keine Übersetzung.
    /// </summary>
    public static SpeechLanguage? ExplicitTarget(string prompt)
    {
        var klein = Zitat.Replace(prompt, " ").Replace('İ', 'i').ToLowerInvariant();

        if (klein.Contains("almanca") || klein.Contains("auf deutsch") || klein.Contains("ins deutsche"))
        {
            return SpeechLanguage.Deutsch;
        }

        if (klein.Contains("auf englisch") || klein.Contains("ins englische") || klein.Contains("in english")
            || klein.Contains("ingilizce"))
        {
            return SpeechLanguage.Englisch;
        }

        if (klein.Contains("türkçe") || klein.Contains("auf türkisch") || klein.Contains("ins türkische"))
        {
            return SpeechLanguage.Tuerkisch;
        }

        return null;
    }

    private static SpeechLanguage AntwortSprache(string antwort, SpeechLanguage antwortSprache) =>
        FesteAntwortwoerter.TryGetValue(antwort.Trim().TrimEnd('.', '!'), out var fest) ? fest : antwortSprache;

    /// <summary>
    /// Macht Zeichen sprechbar, die eine Stimme sonst buchstabiert oder falsch deutet: Lücken
    /// („___“) werden zur Pause, Pfeile zur Pause, und im Deutschen werden Rechenzeichen zu
    /// Wörtern („3/4“ läse eine Windows-Stimme sonst als Datum).
    /// </summary>
    public static string Speakable(string text, SpeechLanguage language)
    {
        var s = Regex.Replace(text, "_{2,}", " … ");
        s = s.Replace("->", ", ").Replace("→", ", ");
        s = Regex.Replace(s, "[\"„“”«»]", string.Empty);

        if (language == SpeechLanguage.Deutsch)
        {
            s = Regex.Replace(s, @"(\d)\s*/\s*(\d)", "$1 durch $2");
            s = Regex.Replace(s, @"(\d) : (\d)", "$1 geteilt durch $2");
            s = Regex.Replace(s, @"(\d):(\d{3,})", "$1 zu $2");
            // Minus nur zwischen Zahlen oder einzelnen Buchstaben (Variablen): " - " zwischen
            // zwei Wörtern ist im Deutschen ein Gedankenstrich.
            s = Regex.Replace(s, @"(?<=[\d)²³]|\b\p{L})\s[-−]\s(?=[\d(]|\p{L}\b)", " minus ");
            s = Regex.Replace(s, @"(?<![\p{L}\d])[-−](?=\d)", "minus ");
            s = s.Replace("·", " mal ").Replace("×", " mal ").Replace(" * ", " mal ")
                .Replace("+", " plus ").Replace("=", " gleich ").Replace("≈", " ungefähr ")
                .Replace("²", " hoch 2").Replace("³", " hoch 3").Replace("√", " Wurzel aus ")
                .Replace("π", " Pi ").Replace("%", " Prozent").Replace("°", " Grad");
        }

        return Regex.Replace(s, @"\s+", " ").Trim();
    }

    private static void Add(List<SpeechSegment> abschnitte, string text, SpeechLanguage sprache)
    {
        var sprechbar = Speakable(text, sprache);

        // Reine Satzzeichen ("?", ": ") tragen nichts bei - eine Stimme würde sie höchstens
        // buchstabieren.
        if (!sprechbar.Any(char.IsLetterOrDigit))
        {
            return;
        }

        abschnitte.Add(new SpeechSegment(sprechbar, sprache));
    }

    /// <summary>Benachbarte Abschnitte derselben Sprache zusammenfassen: weniger Stimmwechsel,
    /// natürlichere Satzmelodie.</summary>
    private static List<SpeechSegment> Merge(List<SpeechSegment> abschnitte)
    {
        var ergebnis = new List<SpeechSegment>();
        foreach (var abschnitt in abschnitte)
        {
            if (ergebnis.Count > 0 && ergebnis[^1].Language == abschnitt.Language)
            {
                ergebnis[^1] = ergebnis[^1] with { Text = $"{ergebnis[^1].Text} {abschnitt.Text}" };
            }
            else
            {
                ergebnis.Add(abschnitt);
            }
        }

        return ergebnis;
    }

    private static string MitSatzende(string text)
    {
        var getrimmt = text.TrimEnd();
        return getrimmt.Length > 0 && ".!?…".Contains(getrimmt[^1]) ? getrimmt : getrimmt + ".";
    }
}
