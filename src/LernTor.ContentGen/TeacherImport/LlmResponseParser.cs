using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Gemeinsamer Prompt-Text und JSON-Antwort-Parser für alle <see cref="ITeacherQuestionSuggester"/>-
/// Implementierungen (Cloud- wie lokale LLMs) - jedes LLM bekommt denselben Auftrag, aus einem
/// Dokument Quizfragen als JSON zu erzeugen, und die Antwort wird identisch geparst, egal ob sie von
/// NotebookLM oder einem lokalen Modell kommt.
/// </summary>
internal static class LlmResponseParser
{
    public static string BuildPrompt(Subject subject, GradeLevel gradeLevel) =>
        $"Erstelle aus dem hochgeladenen Dokument bis zu 10 Quizfragen für ein Kind der {gradeLevel} " +
        $"im Fach {subject} auf Deutsch. Antworte AUSSCHLIESSLICH mit einem JSON-Objekt in exakt " +
        "folgendem Format, ohne weiteren Text davor oder danach:\n" +
        "{\"questions\":[{\"topic\":\"...\",\"prompt\":\"...\",\"type\":\"MultipleChoice|TrueFalse|OpenText\"," +
        "\"options\":[\"...\"],\"correctAnswers\":[\"...\"],\"explanation\":\"...\",\"helpHint\":\"...|null\"," +
        "\"sourceExcerpt\":\"...\"}]}\n" +
        "Bei OpenText-Fragen soll 'options' ein leeres Array sein. 'sourceExcerpt' soll die Textstelle " +
        "aus dem Dokument enthalten, auf der die Frage beruht.";

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
