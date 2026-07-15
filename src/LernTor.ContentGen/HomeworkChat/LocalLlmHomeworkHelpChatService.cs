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

    /// <summary>
    /// Stoppwörter für die Texterzeugung: der Prompt endet mit "Assistent:", damit das Modell direkt
    /// mit seiner Antwort fortfährt - ohne Stoppwörter setzt es diese Vervollständigung aber einfach
    /// fort und erfindet gleich die nächste(n) Kind-/Assistent-Runde(n) mit dazu (beobachtetes
    /// Verhalten: das Modell gibt ein komplettes Fantasie-Gespräch statt nur einer einzigen Antwort
    /// zurück). "###" stoppt zusätzlich, wenn das Modell die Abschnitts-Marker des Prompts
    /// (### REGELN usw.) nachahmt und eigene "###"-Anweisungszeilen an die Antwort anhängt
    /// (ebenfalls beim Nutzer beobachtet - die Marker erschienen wörtlich im Chat). AntiPrompts
    /// prüfen nur den ERZEUGTEN Text, nicht den Prompt selbst - die ###-Abschnitte im Prompt lösen
    /// den Stopp also nicht aus. Sobald eines dieser Wörter erscheint, bricht LLamaSharp ab.
    /// </summary>
    private static readonly IReadOnlyList<string> StopSequences = new[]
    {
        "\nKind:", "Kind:", "\nAssistent:", "\n###", "###"
    };

    public async Task<string> AskAsync(
        QuizQuestion question,
        IReadOnlyList<ChatMessage> conversation,
        CancellationToken cancellationToken = default)
    {
        var executor = await _modelHost.GetExecutorAsync(cancellationToken);
        var prompt = HomeworkChatPromptBuilder.BuildPrompt(question, conversation);
        var inferenceParams = new InferenceParams
        {
            MaxTokens = 300,
            AntiPrompts = StopSequences
        };

        var answer = new StringBuilder();
        await foreach (var token in executor.InferAsync(prompt, inferenceParams, cancellationToken))
        {
            answer.Append(token);
        }

        return TrimAtFirstStopSequence(answer.ToString());
    }

    /// <summary>
    /// Zusätzliche Absicherung neben <see cref="InferenceParams.AntiPrompts"/>: falls ein Stoppwort
    /// erst nach ein paar weiteren Token erkannt wird (oder LLamaSharp es trotzdem mit ausgibt),
    /// schneidet dies den Text spätestens hier ab, bevor er dem Kind angezeigt wird.
    /// </summary>
    private static string TrimAtFirstStopSequence(string text)
    {
        var trimmed = text.Trim();
        var cutIndex = trimmed.Length;

        foreach (var stopSequence in StopSequences)
        {
            var index = trimmed.IndexOf(stopSequence, StringComparison.Ordinal);
            if (index >= 0 && index < cutIndex)
            {
                cutIndex = index;
            }
        }

        return trimmed[..cutIndex].Trim();
    }
}
