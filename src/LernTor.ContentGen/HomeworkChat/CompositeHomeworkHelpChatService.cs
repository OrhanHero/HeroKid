using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// Leitet <see cref="IHomeworkHelpChatService.AskAsync"/> anhand von
/// <see cref="HomeworkChatProviderOptions.Provider"/> an die von den Eltern gewählte Implementierung
/// weiter (lokales Modell per Standard, oder NotebookLM Enterprise in der Cloud). Das ist die einzige
/// <see cref="IHomeworkHelpChatService"/>-Registrierung in der Dependency-Injection.
/// </summary>
public sealed class CompositeHomeworkHelpChatService : IHomeworkHelpChatService
{
    private readonly HomeworkChatProviderOptions _providerOptions;
    private readonly NotebookLmHomeworkHelpChatService _notebookLm;
    private readonly LocalLlmHomeworkHelpChatService _localLlm;

    public CompositeHomeworkHelpChatService(
        HomeworkChatProviderOptions providerOptions,
        NotebookLmHomeworkHelpChatService notebookLm,
        LocalLlmHomeworkHelpChatService localLlm)
    {
        _providerOptions = providerOptions;
        _notebookLm = notebookLm;
        _localLlm = localLlm;
    }

    public Task<string> AskAsync(
        QuizQuestion question,
        IReadOnlyList<ChatMessage> conversation,
        CancellationToken cancellationToken = default)
    {
        IHomeworkHelpChatService active = _providerOptions.Provider switch
        {
            LlmProvider.NotebookLm => _notebookLm,
            _ => _localLlm
        };

        return active.AskAsync(question, conversation, cancellationToken);
    }
}
