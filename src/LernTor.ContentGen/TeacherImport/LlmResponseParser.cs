using System.Text.Json;
using System.Text.Json.Serialization;
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

        AnswerDto? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<AnswerDto>(json, SerializerOptions);
        }
        catch (JsonException ex)
        {
            // Eltern sollen eine Erklärung sehen, keine .NET-Ausnahme: "'0x1B' is an invalid
            // start of a value" hilft niemandem weiter.
            throw new InvalidOperationException(
                "Die KI hat keine verwertbaren Fragen geliefert (die Antwort war fehlerhaftes JSON). " +
                "Bitte noch einmal versuchen - am besten mit einem Dokument, das durchgehenden " +
                "Fließtext enthält. Rohantwort: " + Truncate(answerText, 400), ex);
        }

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
    /// das erste vollständige {...}-Objekt aus der Antwort und repariert notfalls eine mittendrin
    /// abgebrochene Antwort.
    /// </summary>
    internal static string ExtractJsonObject(string answerText)
    {
        var complete = TryExtractBalancedObject(answerText);
        if (complete is not null)
        {
            return complete;
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
    /// Liefert das erste wirklich zu Ende geschriebene <c>{...}</c>-Objekt der Antwort, sonst
    /// <c>null</c>.
    ///
    /// <para>Bewusst ein Klammerzähler und kein gieriger Regex (<c>\{[\s\S]*\}</c>): der fand auch
    /// in einer mittendrin abgebrochenen Antwort noch einen "Treffer" - nämlich bis zur
    /// schließenden Klammer der letzten fertigen Frage - und lieferte damit kaputtes JSON. Die
    /// Reparatur weiter unten kam so nie zum Zug, und der Import scheiterte mit einer
    /// JsonException statt mit den bereits brauchbaren Fragen.</para>
    /// </summary>
    private static string? TryExtractBalancedObject(string answerText)
    {
        var objectStart = answerText.IndexOf('{');
        if (objectStart < 0)
        {
            return null;
        }

        foreach (var end in ObjectEndsAtDepthZero(answerText, objectStart))
        {
            return answerText[objectStart..(end + 1)];
        }

        return null;
    }

    /// <summary>
    /// Repariert eine mittendrin abgebrochene Antwort der Form <c>{"questions":[{...},{...},{unvoll</c>,
    /// indem hinter der letzten vollständig geschlossenen Frage abgeschnitten und die Liste sowie
    /// das Objekt geschlossen werden. Liefert <c>null</c>, wenn nicht einmal eine Frage komplett ist.
    /// </summary>
    private static string? TryRepairTruncatedArray(string answerText)
    {
        var arrayStart = answerText.IndexOf('[');
        if (arrayStart < 0)
        {
            return null;
        }

        var lastComplete = -1;
        foreach (var end in ObjectEndsAtDepthZero(answerText, arrayStart))
        {
            lastComplete = end;
        }

        return lastComplete < 0 ? null : answerText[..(lastComplete + 1)] + "]}";
    }

    /// <summary>
    /// Läuft den Text ab <paramref name="start"/> durch und meldet jede Position, an der ein
    /// <c>{...}</c>-Objekt auf Verschachtelungstiefe 0 geschlossen wird. Zeichenketten werden
    /// übersprungen, damit eine geschweifte Klammer IM Fragetext (etwa "Was bedeutet {1,2,3}?")
    /// nicht mitzählt.
    ///
    /// <para>Beginnt <paramref name="start"/> auf dem <c>{</c> des Gesamtobjekts, ist der erste
    /// Treffer dessen Ende; beginnt er auf dem <c>[</c> der Fragenliste, ist jeder Treffer das
    /// Ende einer vollständigen Frage.</para>
    /// </summary>
    private static IEnumerable<int> ObjectEndsAtDepthZero(string text, int start)
    {
        int depth = 0;
        bool inString = false, escaped = false;

        for (int i = start; i < text.Length; i++)
        {
            var c = text[i];

            if (inString)
            {
                if (escaped) escaped = false;
                else if (c == '\\') escaped = true;
                else if (c == '"') inString = false;
                continue;
            }

            if (c == '"') inString = true;
            else if (c == '{') depth++;
            else if (c == '}' && depth > 0 && --depth == 0) yield return i;
        }
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
