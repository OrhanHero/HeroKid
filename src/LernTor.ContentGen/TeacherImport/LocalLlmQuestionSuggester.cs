using System.Text;
using LernTor.ContentGen.Llm;
using LernTor.Core.Enums;
using LLama.Common;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// <see cref="ITeacherQuestionSuggester"/>-Implementierung über ein lokal geladenes GGUF-Modell
/// (LLamaSharp/llama.cpp) - Alternative zu <see cref="NotebookLmQuestionSuggester"/> ohne Cloud-Konto,
/// ohne laufende Kosten, Dokumenttext verlässt nie den PC. Modell-Laden/Caching übernimmt die geteilte
/// <see cref="LocalLlmModelHost"/> (auch vom KI-Lernchat genutzt), damit das Modell nicht bei jedem
/// Aufruf neu von der Festplatte geladen werden muss.
/// </summary>
public sealed class LocalLlmQuestionSuggester : ITeacherQuestionSuggester
{
    private readonly LocalLlmModelHost _modelHost;

    public LocalLlmQuestionSuggester(LocalLlmModelHost modelHost)
    {
        _modelHost = modelHost;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        var executor = await _modelHost.GetExecutorAsync(cancellationToken);

        var prompt = LlmResponseParser.BuildPrompt(subject, gradeLevel) + $"\n\nDokument:\n{documentText}";
        var inferenceParams = new InferenceParams
        {
            MaxTokens = 2048
        };

        var answer = new StringBuilder();
        await foreach (var token in executor.InferAsync(prompt, inferenceParams, cancellationToken))
        {
            answer.Append(token);
        }

        return LlmResponseParser.ParseDrafts(answer.ToString(), documentText);
    }
}
