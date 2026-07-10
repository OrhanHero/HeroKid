using System.Text;
using LernTor.ContentGen.Llm;
using LernTor.Core.Models;
using LLama.Common;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// KI-Lernchat: lokal geladenes GGUF-Modell (LLamaSharp), keine Cloud, kein Konto - die Aufgabe/das
/// Gespräch des Kindes verlässt nie den PC. Nutzt den geteilten <see cref="LocalLlmModelHost"/>, der
/// das Modell nur beim ersten Gebrauch lädt (und bei Bedarf automatisch herunterlädt) und danach für
/// weitere Nachrichten (auch aus dem Lehrer-Import) im Speicher hält.
/// </summary>
public sealed class LocalLlmHomeworkHelpChatService : IHomeworkHelpChatService
{
    private readonly LocalLlmModelHost _modelHost;

    public LocalLlmHomeworkHelpChatService(LocalLlmModelHost modelHost)
    {
        _modelHost = modelHost;
    }

    public async Task<string> AskAsync(
        QuizQuestion question,
        IReadOnlyList<ChatMessage> conversation,
        CancellationToken cancellationToken = default)
    {
        var executor = await _modelHost.GetExecutorAsync(cancellationToken);
        var prompt = HomeworkChatPromptBuilder.BuildPrompt(question, conversation);
        var inferenceParams = new InferenceParams
        {
            MaxTokens = 300
        };

        var answer = new StringBuilder();
        await foreach (var token in executor.InferAsync(prompt, inferenceParams, cancellationToken))
        {
            answer.Append(token);
        }

        return answer.ToString().Trim();
    }
}
