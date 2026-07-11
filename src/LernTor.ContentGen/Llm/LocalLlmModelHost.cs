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
/// gesetzt, wird beim ersten Gebrauch das im Eltern-Bereich gewählte Katalog-Modell (siehe
/// <see cref="LocalLlmModelCatalog"/>, Standard Qwen2.5-7B-Instruct Q4_K_M) nach
/// <c>%LOCALAPPDATA%\LernTor\models\</c> heruntergeladen - Eltern müssen keine Modelldatei selbst
/// suchen. Jedes Katalog-Modell hat seinen eigenen Dateinamen; ein Modellwechsel lädt also nur beim
/// ersten Mal herunter und wechselt danach zwischen bereits vorhandenen Dateien. Zu URLs und
/// Fallback bei Download-Fehlern siehe <see cref="LocalLlmModelCatalog"/>.</para>
///
/// <para><c>StatelessExecutor</c> erzeugt sich seinen Inferenz-Kontext laut CLR-Metadaten-Analyse der
/// echten LLamaSharp-0.27.0-DLL intern selbst aus den übergebenen <c>IContextParams</c> - ein separat
/// per <c>LLamaWeights.CreateContext(...)</c> erzeugter Kontext wird vom Konstruktor gar nicht
/// entgegengenommen und wäre daher nur unnötig doppelt geladener Speicher.</para>
/// </summary>
public sealed class LocalLlmModelHost : IDisposable
{
    private readonly LocalLlmOptions _options;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private LLamaWeights? _weights;
    private ILLamaExecutor? _executor;
    private string? _loadedModelPath;

    /// <summary>
    /// Eigener HttpClient statt des geteilten App-Clients: dessen Standard-Timeout von 100 Sekunden
    /// gilt bei HttpClient auch für das Auslesen des Antwort-Streams - ein mehrere GB großer
    /// Modell-Download bricht damit über normales Heim-Internet garantiert mittendrin ab (genau so
    /// beim Nutzer passiert). Hier daher unbegrenzt; abbrechbar bleibt der Download über das
    /// CancellationToken. Ein User-Agent ist gesetzt, weil manche CDNs anonyme Clients ablehnen.
    /// </summary>
    private static readonly HttpClient DownloadClient = CreateDownloadClient();

    private static HttpClient CreateDownloadClient()
    {
        var client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("LernTor/1.0");
        return client;
    }

    public LocalLlmModelHost(LocalLlmOptions options)
    {
        _options = options;
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
        // Eigene Modelldatei der Eltern hat Vorrang vor dem Katalog.
        var configuredPath = _options.ModelPath;
        if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        var model = LocalLlmModelCatalog.Resolve(_options.ModelKey);
        var localPath = GetLocalPath(model);
        if (!File.Exists(localPath))
        {
            await DownloadModelAsync(model, localPath, cancellationToken);
        }

        return localPath;
    }

    private static string GetLocalPath(LocalLlmModelInfo model) => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "LernTor", "models", model.FileName);

    private static async Task DownloadModelAsync(LocalLlmModelInfo model, string destinationPath, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
        var tempPath = destinationPath + ".download";
        Exception? lastError = null;

        foreach (var url in model.DownloadUrls)
        {
            try
            {
                using var response = await DownloadClient.GetAsync(
                    url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                response.EnsureSuccessStatusCode();

                await using (var httpStream = await response.Content.ReadAsStreamAsync(cancellationToken))
                await using (var fileStream = File.Create(tempPath))
                {
                    await httpStream.CopyToAsync(fileStream, cancellationToken);
                }

                File.Move(tempPath, destinationPath, overwrite: true);
                return;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                lastError = ex;
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                // Nächste Spiegel-Quelle probieren.
            }
        }

        throw new InvalidOperationException(
            $"Das KI-Modell \"{model.DisplayName}\" (~{model.ApproxSizeGb:0.#} GB) konnte von keiner " +
            "der hinterlegten Quellen heruntergeladen werden (kein Internet, zu wenig Speicherplatz, " +
            "oder die Download-Adressen haben sich geändert). Im Eltern-Bereich unter 'Automatisches " +
            "Einlesen' kann ein anderes Modell gewählt oder manuell eine .gguf-Modelldatei ausgewählt " +
            $"werden. Technischer Grund des letzten Versuchs: {lastError?.Message}", lastError);
    }

    public void Dispose()
    {
        _weights?.Dispose();
        _lock.Dispose();
    }
}
