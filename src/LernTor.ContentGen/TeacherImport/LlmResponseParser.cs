using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Gemeinsamer Prompt-Text und JSON-Antwort-Parser für <see cref="ITeacherQuestionSuggester"/> -
/// jedes LLM bekommt denselben Auftrag, aus einem Dokument Quizfragen als JSON zu erzeugen, und die
/// Antwort wird identisch geparst.
/// </summary>
internal static class LlmResponseParser
{
    /// <summary>Auf Qwen2.5-Instruct zugeschnitten (klare Abschnitte, nummerierte Regeln): das Modell
    /// hält strukturierte Formatvorgaben deutlich zuverlässiger ein als Fließtext-Anweisungen.</summary>
    /// <summary>
    /// Vorgabe der ersten Antwortzeichen ("Prefill"): der Prompt endet mitten im JSON, das Modell
    /// kann also gar nicht anders, als es fortzusetzen. Ohne diesen Kniff hat Qwen2.5-7B den
    /// Auftrag regelmäßig ignoriert und stattdessen eine Zusammenfassung des Dokuments geschrieben
    /// - Ergebnis war die Meldung "Konnte in der LLM-Antwort kein JSON-Objekt finden" (realer Fund
    /// aus dem Familienbetrieb). Der Wert wird beim Parsen wieder vorangestellt.
    /// </summary>
    public const string AnswerPrefill = "{\"questions\":[";

    public static string BuildPrompt(Subject subject, GradeLevel gradeLevel) =>
        "### AUFGABE\n" +
        $"Erstelle aus dem unten angehängten Dokument 6 bis 10 Quizfragen für ein Kind der {gradeLevel} " +
        $"(Berliner Rahmenlehrplan) im Fach {subject}, auf Deutsch.\n\n" +
        "### REGELN\n" +
        "1. Mische die Fragetypen: überwiegend MultipleChoice, dazu einzelne TrueFalse- und OpenText-Fragen.\n" +
        "2. Die Schwierigkeit muss zur Klassenstufe passen - keine Fragen, die nur Erwachsene beantworten können.\n" +
        "3. Bei MultipleChoice: genau 3-4 Optionen, die falschen müssen plausibel klingen (keine Scherzantworten), " +
        "und die richtige Antwort muss wörtlich eine der Optionen sein.\n" +
        "4. 'explanation' erklärt kindgerecht in 1-2 Sätzen, WARUM die Antwort stimmt.\n" +
        "5. 'helpHint' ist ein kleiner Denkanstoß OHNE die Lösung zu verraten (oder null).\n" +
        "6. 'sourceExcerpt' ist ein wörtliches Zitat der Dokumentstelle, auf der die Frage beruht.\n" +
        "7. Frage nur ab, was wirklich im Dokument steht - erfinde keine Fakten dazu.\n\n" +
        "### AUSGABEFORMAT\n" +
        "Antworte AUSSCHLIESSLICH mit einem JSON-Objekt in exakt diesem Format, ohne weiteren Text davor " +
        "oder danach, ohne Markdown-Codeblock:\n" +
        "{\"questions\":[{\"topic\":\"...\",\"prompt\":\"...\",\"type\":\"MultipleChoice|TrueFalse|OpenText\"," +
        "\"options\":[\"...\"],\"correctAnswers\":[\"...\"],\"explanation\":\"...\",\"helpHint\":\"...|null\"," +
        "\"sourceExcerpt\":\"...\"}]}\n" +
        "Bei OpenText-Fragen ist 'options' ein leeres Array.";

    public static IReadOnlyList<ExtractedQuestionDraft> ParseDrafts(string answerText, string documentText)
    {
        var json = ExtractJsonObject(answerText);
        var parsed = JsonSerializer.Deserialize<AnswerDto>(json, SerializerOptions);

        if (parsed?.Questions is null || parsed.Questions.Count == 0)
        {
            return Array.Empty<ExtractedQuestionDraft>();
        }

        return parsed.Questions.Select(q => new ExtractedQuestionDraft
        {
            Topic = q.Topic ?? string.Empty,
            Prompt = q.Prompt ?? string.Empty,
            Type = ParseQuestionType(q.Type),
            Options = q.Options ?? new List<string>(),
            CorrectAnswers = q.CorrectAnswers ?? new List<string>(),
            Explanation = q.Explanation ?? string.Empty,
            HelpHint = q.HelpHint,
            SourceExcerpt = string.IsNullOrWhiteSpace(q.SourceExcerpt) ? Truncate(documentText, 200) : q.SourceExcerpt!
        }).ToList();
    }

    private static QuestionType ParseQuestionType(string? type) =>
        Enum.TryParse<QuestionType>(type, ignoreCase: true, out var parsed) ? parsed : QuestionType.OpenText;

    public static string Truncate(string text, int maxLength) =>
        text.Length <= maxLength ? text : text[..maxLength] + "…";

    /// <summary>
    /// LLM-Antworten enthalten das erwartete JSON-Objekt manchmal zusätzlich in Markdown-Codeblöcken
    /// (```json ... ```) oder mit erklärendem Text davor/danach - dieses Verfahren extrahiert robust
    /// das erste vollständige {...}-Objekt aus der Antwort.
    /// </summary>
    internal static string ExtractJsonObject(string answerText)
    {
        var match = Regex.Match(answerText, @"\{[\s\S]*\}", RegexOptions.None, TimeSpan.FromSeconds(2));
        if (match.Success)
        {
            return match.Value;
        }

        // Abgeschnittene Antwort: das Token-Limit kann mitten in der Fragenliste zuschlagen.
        // Statt alles zu verwerfen, wird bis zur letzten vollständig geschlossenen Frage gekürzt
        // und die Struktur geschlossen - lieber vier brauchbare Vorschläge als eine Fehlermeldung
        // nach mehreren Minuten Rechenzeit.
        var repaired = TryRepairTruncatedArray(answerText);
        if (repaired is not null)
        {
            return repaired;
        }

        throw new InvalidOperationException(
            "Die KI hat keine verwertbaren Fragen geliefert (die Antwort war kein JSON). Häufigster " +
            "Grund: das Dokument enthält kaum durchgehenden Fließtext. Rohantwort: " +
            Truncate(answerText, 400));
    }

    /// <summary>
    /// Repariert eine mittendrin abgebrochene Antwort der Form <c>{"questions":[{...},{...},{unvoll</c>,
    /// indem hinter der letzten vollständig geschlossenen Frage abgeschnitten und die Liste sowie
    /// das Objekt geschlossen werden. Liefert <c>null</c>, wenn nicht einmal eine Frage komplett ist.
    /// </summary>
    private static string? TryRepairTruncatedArray(string answerText)
    {
        var arrayStart = answerText.IndexOf("[", StringComparison.Ordinal);
        if (arrayStart < 0)
        {
            return null;
        }

        // Klammern zählen, um das Ende einer vollständigen Frage zu finden. Zeichenketten werden
        // dabei übersprungen, damit eine geschweifte Klammer IM Fragetext nicht mitzählt.
        int depth = 0, lastComplete = -1;
        bool inString = false, escaped = false;

        for (int i = arrayStart; i < answerText.Length; i++)
        {
            var c = answerText[i];

            if (inString)
            {
                if (escaped) escaped = false;
                else if (c == '\\') escaped = true;
                else if (c == '"') inString = false;
                continue;
            }

            if (c == '"') inString = true;
            else if (c == '{') depth++;
            else if (c == '}' && --depth == 0) lastComplete = i;
        }

        return lastComplete < 0 ? null : answerText[..(lastComplete + 1)] + "]}";
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private sealed class AnswerDto
    {
        [JsonPropertyName("questions")]
        public List<QuestionDto>? Questions { get; set; }
    }

    private sealed class QuestionDto
    {
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }

        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("options")]
        public List<string>? Options { get; set; }

        [JsonPropertyName("correctAnswers")]
        public List<string>? CorrectAnswers { get; set; }

        [JsonPropertyName("explanation")]
        public string? Explanation { get; set; }

        [JsonPropertyName("helpHint")]
        public string? HelpHint { get; set; }

        [JsonPropertyName("sourceExcerpt")]
        public string? SourceExcerpt { get; set; }
    }
}
