using System.Text;
using LernTor.ContentGen.Llm;
using LernTor.Core.Enums;
using LLama.Common;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// <see cref="ITeacherQuestionSuggester"/>-Implementierung über ein lokal geladenes GGUF-Modell
/// (LLamaSharp/llama.cpp) - komplett lokal, kein Cloud-Konto, keine laufenden Kosten, Dokumenttext
/// verlässt nie den PC. Modell-Laden/Caching übernimmt die geteilte <see cref="LocalLlmModelHost"/>
/// (auch vom KI-Lernchat genutzt), damit das Modell nicht bei jedem Aufruf neu von der Festplatte
/// geladen werden muss.
/// </summary>
public sealed class LocalLlmQuestionSuggester : ITeacherQuestionSuggester
{
    private readonly LocalLlmModelHost _modelHost;

    public LocalLlmQuestionSuggester(LocalLlmModelHost modelHost)
    {
        _modelHost = modelHost;
    }

    /// <summary>
    /// Wie viele Zeichen Dokumenttext höchstens in den Prompt wandern.
    ///
    /// <para>Das Kontextfenster (<c>LocalLlmOptions.ContextSize</c>, Standard 4096 Token) muss
    /// Anweisung, Dokument UND Antwort fassen. Ein mehrseitiges PDF sprengt das um ein Vielfaches -
    /// und weil ein 7B-Modell auf der CPU jedes Prompt-Token einzeln verarbeitet, hing der Import
    /// dann minutenlang bis stundenlang bei "wird eingelesen…", ohne je fertig zu werden (realer
    /// Fund aus dem Familienbetrieb). Rund 6000 Zeichen deutscher Text entsprechen grob 2000-2400
    /// Token und lassen genug Platz für Anweisung und Antwort.</para>
    /// </summary>
    public const int MaxDocumentCharacters = 6000;

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        var executor = await _modelHost.GetExecutorAsync(cancellationToken);

        var usedText = Shorten(documentText);

        // Reihenfolge bewusst: erst das Dokument, dann die Anweisung, dann der angefangene
        // JSON-Anfang. Instruct-Modelle gewichten das ZULETZT Gelesene am stärksten - stand die
        // Anweisung vor einem langen Dokument, ging sie unter und das Modell fasste den Text
        // einfach zusammen.
        var prompt =
            $"### DOKUMENT\n{usedText}\n\n" +
            LlmResponseParser.BuildPrompt(subject, gradeLevel) +
            "\n\n### ANTWORT\n" +
            LlmResponseParser.AnswerPrefill;

        var inferenceParams = new InferenceParams
        {
            // 1024 statt 2048: 6-10 Fragen als JSON brauchen keine 2048 Token, und jedes
            // erzeugte Token kostet auf der CPU spürbar Zeit.
            MaxTokens = 1024,
            // Ohne Stop-Sequenzen schreibt das Modell nach dem JSON munter weiter und läuft in
            // die MaxTokens-Grenze - dieselbe Falle wie im KI-Lernchat. Bewusst NICHT auf "}"
            // stoppen: das würde die Liste nach der ersten Frage abschneiden.
            AntiPrompts = new List<string> { "\n\n###", "\n###", "### DOKUMENT" }
        };

        var answer = new StringBuilder(LlmResponseParser.AnswerPrefill);
        await foreach (var token in executor.InferAsync(prompt, inferenceParams, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            answer.Append(token);
        }

        return LlmResponseParser.ParseDrafts(answer.ToString(), usedText);
    }

    /// <summary>
    /// Kürzt zu lange Dokumente auf <see cref="MaxDocumentCharacters"/>, möglichst an einer
    /// Satzgrenze - ein mitten im Wort abgeschnittener Text bringt das Modell aus dem Tritt.
    /// </summary>
    internal static string Shorten(string documentText)
    {
        var text = (documentText ?? string.Empty).Trim();
        if (text.Length <= MaxDocumentCharacters)
        {
            return text;
        }

        var cut = text[..MaxDocumentCharacters];
        var lastSentenceEnd = cut.LastIndexOfAny(new[] { '.', '!', '?' });

        // Nur an einer Satzgrenze schneiden, wenn dadurch nicht der halbe Text wegfällt.
        return lastSentenceEnd > MaxDocumentCharacters / 2 ? cut[..(lastSentenceEnd + 1)] : cut;
    }
}
