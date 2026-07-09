using System.Text;
using LernTor.Core.Enums;
using LLama;
using LLama.Common;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// <see cref="ITeacherQuestionSuggester"/>-Implementierung über ein lokal geladenes GGUF-Modell
/// (LLamaSharp/llama.cpp) - Alternative zu <see cref="NotebookLmQuestionSuggester"/> ohne Cloud-Konto,
/// ohne laufende Kosten, Dokumenttext verlässt nie den PC. Dafür läuft die Inferenz auf der CPU des
/// Kiosk-PCs (kann je nach Modellgröße mehrere Sekunden bis Minuten dauern) und der Elternteil muss die
/// Modelldatei selbst besorgen (nicht Teil des Installers).
///
/// <para>API-Aufrufe gegen die echte kompilierte LLamaSharp-0.27.0-DLL verifiziert (nuget.org-Download,
/// CLR-Metadaten-Inspektion via dnfile): <c>ModelParams(string modelPath)</c>-Konstruktor mit settable
/// <c>ContextSize</c>/<c>GpuLayerCount</c>-Properties, statisches <c>LLamaWeights.LoadFromFile(ModelParams)</c>,
/// instanzseitiges <c>LLamaWeights.CreateContext(ModelParams, ILogger?)</c>,
/// <c>StatelessExecutor(LLamaWeights, ModelParams, ILogger?)</c>-Konstruktor,
/// <c>InferAsync(string, InferenceParams?, CancellationToken)</c> liefert <c>IAsyncEnumerable&lt;string&gt;</c>
/// (bestätigt über den TypeRef auf System.Collections.Generic.IAsyncEnumerable`1 in der DLL). Sowohl
/// <c>LLamaWeights</c> als auch <c>LLamaContext</c> implementieren <c>IDisposable</c>.</para>
/// </summary>
public sealed class LocalLlmQuestionSuggester : ITeacherQuestionSuggester
{
    private readonly LocalLlmOptions _options;

    public LocalLlmQuestionSuggester(LocalLlmOptions options)
    {
        _options = options;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            throw new NotSupportedException(
                "Automatisches Einlesen von Lehrer-Unterlagen (lokales LLM) ist vorbereitet, aber im " +
                "Eltern-Bereich unter 'Automatisches Einlesen' ist noch keine Modelldatei (GGUF) hinterlegt. " +
                "Der manuelle Eigene-Aufgaben-Editor deckt den Kernbedarf in der Zwischenzeit ab.");
        }

        var modelParams = new ModelParams(_options.ModelPath!)
        {
            ContextSize = _options.ContextSize,
            // Absichtlich reines CPU-Backend (Paket LLamaSharp.Backend.Cpu) - keine CUDA/GPU-Abhängigkeit
            // im Kiosk-Setup, das auf beliebiger Eltern-Hardware laufen muss.
            GpuLayerCount = 0
        };

        using var weights = LLamaWeights.LoadFromFile(modelParams);
        using var context = weights.CreateContext(modelParams);
        var executor = new StatelessExecutor(weights, modelParams);

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
