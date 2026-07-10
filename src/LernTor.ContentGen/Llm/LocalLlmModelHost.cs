using System.Net.Http;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using LernTor.ContentGen.TeacherImport;

namespace LernTor.ContentGen.Llm;

/// <summary>
/// Lädt das lokale GGUF-Modell (LLamaSharp) genau einmal und hält Gewichte/Executor im Speicher, statt
/// sie bei jedem Aufruf neu zu laden - das Laden eines Modells kann je nach Größe mehrere Sekunden bis
/// Minuten dauern, was für einen interaktiven Kinder-Chat inakzeptabel wäre. Wird sowohl von
/// <see cref="TeacherImport.LocalLlmQuestionSuggester"/> (Lehrer-Import) als auch vom KI-Lernchat
/// geteilt, damit beide Features nicht unabhängig voneinander dasselbe Modell doppelt im RAM halten.
///
/// <para><b>Automatischer Modell-Download:</b> Ist <see cref="LocalLlmOptions.ModelPath"/> nicht
/// gesetzt, wird beim ersten Gebrauch automatisch ein Standardmodell (siehe <see cref="DefaultModelDownloadUrl"/>)
/// nach <c>%LOCALAPPDATA%\LernTor\models\</c> heruntergeladen - Eltern müssen dafür keine Modelldatei
/// selbst suchen. <b>Die genaue Hugging-Face-URL/Dateiname ist aus dieser Sandbox NICHT verifizierbar</b>
/// (huggingface.co ist von der Entwicklungsumgebung aus blockiert, 403 per curl bestätigt - dieselbe
/// Einschränkung wie zuvor bei docs.cloud.google.com) und muss beim ersten echten Test auf einem
/// Windows-Rechner mit Internetzugang überprüft werden. Schlägt der Download fehl, können Eltern
/// weiterhin manuell eine `.gguf`-Datei herunterladen und über <see cref="LocalLlmOptions.ModelPath"/>
/// (Eltern-Bereich, Datei-Dialog) hinterlegen.</para>
///
/// <para><c>StatelessExecutor</c> erzeugt sich seinen Inferenz-Kontext laut CLR-Metadaten-Analyse der
/// echten LLamaSharp-0.27.0-DLL intern selbst aus den übergebenen <c>IContextParams</c> - ein separat
/// per <c>LLamaWeights.CreateContext(...)</c> erzeugter Kontext wird vom Konstruktor gar nicht
/// entgegengenommen und wäre daher nur unnötig doppelt geladener Speicher.</para>
/// </summary>
public sealed class LocalLlmModelHost : IDisposable
{
    /// <summary>
    /// Qwen2.5-3B-Instruct (Apache-2.0), Q4_K_M-Quantisierung: gutes Verhältnis aus Größe (~2 GB),
    /// Mehrsprachigkeit (u.a. Deutsch) und Anleitungs-/Reasoning-Qualität für CPU-Inferenz auf
    /// gewöhnlicher Familien-Hardware - siehe README für die Modellauswahl-Begründung.
    /// </summary>
    private const string DefaultModelFileName = "qwen2.5-3b-instruct-q4_k_m.gguf";

    private const string DefaultModelDownloadUrl =
        "https://huggingface.co/Qwen/Qwen2.5-3B-Instruct-GGUF/resolve/main/qwen2.5-3b-instruct-q4_k_m.gguf";

    private readonly LocalLlmOptions _options;
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private LLamaWeights? _weights;
    private ILLamaExecutor? _executor;
    private string? _loadedModelPath;

    public LocalLlmModelHost(LocalLlmOptions options, HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
    }

    public async Task<ILLamaExecutor> GetExecutorAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var modelPath = await EnsureModelFileAsync(cancellationToken);

            if (_executor is not null && _loadedModelPath == modelPath)
            {
                return _executor;
            }

            // Modellwechsel im Eltern-Bereich oder Erstladung: alte Gewichte freigeben, bevor neu geladen wird.
            _weights?.Dispose();

            var modelParams = new ModelParams(modelPath)
            {
                ContextSize = _options.ContextSize,
                // Absichtlich reines CPU-Backend (Paket LLamaSharp.Backend.Cpu) - keine CUDA/GPU-Abhängigkeit
                // im Kiosk-Setup, das auf beliebiger Eltern-Hardware laufen muss.
                GpuLayerCount = 0
            };

            _weights = LLamaWeights.LoadFromFile(modelParams);
            _executor = new StatelessExecutor(_weights, modelParams);
            _loadedModelPath = modelPath;

            return _executor;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<string> EnsureModelFileAsync(CancellationToken cancellationToken)
    {
        var configuredPath = _options.ModelPath;
        if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        var defaultPath = GetDefaultModelPath();
        if (!File.Exists(defaultPath))
        {
            await DownloadDefaultModelAsync(defaultPath, cancellationToken);
        }

        return defaultPath;
    }

    private static string GetDefaultModelPath() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "LernTor", "models", DefaultModelFileName);

    private async Task DownloadDefaultModelAsync(string destinationPath, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
        var tempPath = destinationPath + ".download";

        try
        {
            using var response = await _httpClient.GetAsync(
                DefaultModelDownloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using (var httpStream = await response.Content.ReadAsStreamAsync(cancellationToken))
            await using (var fileStream = File.Create(tempPath))
            {
                await httpStream.CopyToAsync(fileStream, cancellationToken);
            }

            File.Move(tempPath, destinationPath, overwrite: true);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            throw new InvalidOperationException(
                "Das KI-Standardmodell konnte nicht automatisch heruntergeladen werden (kein Internet " +
                "beim ersten Gebrauch, oder die Download-Adresse hat sich geändert). Bitte im Eltern-" +
                "Bereich unter 'Automatisches Einlesen' manuell eine .gguf-Modelldatei auswählen.", ex);
        }
    }

    public void Dispose()
    {
        _weights?.Dispose();
        _lock.Dispose();
    }
}
