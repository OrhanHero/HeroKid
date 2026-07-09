using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Leitet <see cref="ITeacherQuestionSuggester.SuggestQuestionsAsync"/> anhand von
/// <see cref="TeacherImportProviderOptions.Provider"/> an die von den Eltern gewählte Implementierung
/// weiter (NotebookLM Enterprise in der Cloud, oder ein lokal geladenes GGUF-Modell). Das ist die
/// einzige <see cref="ITeacherQuestionSuggester"/>-Registrierung in der Dependency-Injection - die
/// beiden konkreten Implementierungen werden nur hierüber aufgerufen.
/// </summary>
public sealed class CompositeTeacherQuestionSuggester : ITeacherQuestionSuggester
{
    private readonly TeacherImportProviderOptions _providerOptions;
    private readonly NotebookLmQuestionSuggester _notebookLm;
    private readonly LocalLlmQuestionSuggester _localLlm;

    public CompositeTeacherQuestionSuggester(
        TeacherImportProviderOptions providerOptions,
        NotebookLmQuestionSuggester notebookLm,
        LocalLlmQuestionSuggester localLlm)
    {
        _providerOptions = providerOptions;
        _notebookLm = notebookLm;
        _localLlm = localLlm;
    }

    public Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        ITeacherQuestionSuggester active = _providerOptions.Provider switch
        {
            LlmProvider.LocalLlm => _localLlm,
            _ => _notebookLm
        };

        return active.SuggestQuestionsAsync(documentText, subject, gradeLevel, cancellationToken);
    }
}
