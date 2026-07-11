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
    private static string ExtractJsonObject(string answerText)
    {
        var match = Regex.Match(answerText, @"\{[\s\S]*\}", RegexOptions.None, TimeSpan.FromSeconds(2));
        if (!match.Success)
        {
            throw new InvalidOperationException(
                "Konnte in der LLM-Antwort kein JSON-Objekt finden. Rohantwort: " + Truncate(answerText, 500));
        }

        return match.Value;
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
