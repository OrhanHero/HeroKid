using LernTor.ContentGen.Llm;
using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// NotebookLM-Variante des KI-Lernchats: da NotebookLMs API grundsätzlich dokumentbasiert arbeitet,
/// wird bei JEDER Chat-Nachricht ein neues Wegwerf-Notebook mit dem Aufgabenkontext als Quelle
/// angelegt und danach wieder gelöscht (siehe <see cref="NotebookLmClient.AskAsync"/>) - anders als
/// beim lokalen Modell gibt es hier keinen zwischen Nachrichten offen gehaltenen Zustand. Bei sehr
/// aktiver Nutzung kann das die NotebookLM-Tageskontingente (siehe README, "Nutzungslimits")
/// schneller ausschöpfen als eine Sitzung mit dauerhaftem Notebook - für Familien, die das nicht
/// möchten, ist genau dafür das lokale Modell der Standard-Anbieter.
/// </summary>
public sealed class NotebookLmHomeworkHelpChatService : IHomeworkHelpChatService
{
    private readonly NotebookLmClient _client;

    public NotebookLmHomeworkHelpChatService(NotebookLmClient client)
    {
        _client = client;
    }

    public Task<string> AskAsync(
        QuizQuestion question,
        IReadOnlyList<ChatMessage> conversation,
        CancellationToken cancellationToken = default)
    {
        var contextText = HomeworkChatPromptBuilder.BuildQuestionContext(question);
        var prompt = HomeworkChatPromptBuilder.BuildConversationPrompt(conversation);

        return _client.AskAsync(
            $"LernTor Chat – {question.Subject} {question.GradeLevel}", contextText, prompt, cancellationToken);
    }
}
