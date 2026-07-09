using LernTor.ContentGen.Llm;
using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// <see cref="ITeacherQuestionSuggester"/>-Implementierung gegen die NotebookLM-Enterprise-API
/// (Google Cloud, Teil von Gemini Enterprise / Discovery Engine). Die eigentliche HTTP-/Notebook-
/// Lebenszyklus-Logik (inkl. der Verifizierungs-Historie gegen die vom Nutzer bereitgestellte PDF-
/// Dokumentation) steckt in der geteilten <see cref="NotebookLmClient"/>-Klasse (siehe dort für
/// Details) - diese Klasse baut daraus nur noch den fach-/dokumentspezifischen Prompt und parst die
/// Antwort in <see cref="ExtractedQuestionDraft"/>-Vorschläge.
/// </summary>
public sealed class NotebookLmQuestionSuggester : ITeacherQuestionSuggester
{
    private readonly NotebookLmClient _client;

    public NotebookLmQuestionSuggester(NotebookLmClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        var prompt = LlmResponseParser.BuildPrompt(subject, gradeLevel);
        var answerText = await _client.AskAsync(
            $"LernTor Import – {subject} {gradeLevel}", documentText, prompt, cancellationToken);

        return LlmResponseParser.ParseDrafts(answerText, documentText);
    }
}
