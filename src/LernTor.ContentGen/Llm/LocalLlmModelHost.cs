using LLama;
using LLama.Abstractions;
using LLama.Common;
using LernTor.ContentGen.TeacherImport;

namespace LernTor.ContentGen.Llm;

/// <summary>
/// Lädt ein lokales GGUF-Modell (LLamaSharp) genau einmal und hält die Gewichte/den Executor im
/// Speicher, statt sie bei jedem Aufruf neu zu laden - das Laden eines Modells kann je nach Größe
/// mehrere Sekunden bis Minuten dauern, was für einen interaktiven Kinder-Chat inakzeptabel wäre.
/// Wird sowohl von <see cref="TeacherImport.LocalLlmQuestionSuggester"/> (Lehrer-Import, einmaliger
/// Vorgang pro Klick) als auch vom KI-Lernchat (mehrere Nachrichten pro Sitzung) geteilt, damit beide
/// Features nicht unabhängig voneinander dasselbe Modell doppelt im RAM halten.
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

    public LocalLlmModelHost(LocalLlmOptions options)
    {
        _options = options;
    }

    public async Task<ILLamaExecutor> GetExecutorAsync(CancellationToken cancellationToken)
    {
        if (!_options.IsConfigured)
        {
            throw new NotSupportedException(
                "Lokales LLM ist nicht konfiguriert (im Eltern-Bereich unter 'Automatisches Einlesen' " +
                "ist keine gültige GGUF-Modelldatei hinterlegt).");
        }

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_executor is not null && _loadedModelPath == _options.ModelPath)
            {
                return _executor;
            }

            // Modellwechsel im Eltern-Bereich oder Erstladung: alte Gewichte freigeben, bevor neu geladen wird.
            _weights?.Dispose();

            var modelParams = new ModelParams(_options.ModelPath!)
            {
                ContextSize = _options.ContextSize,
                // Absichtlich reines CPU-Backend (Paket LLamaSharp.Backend.Cpu) - keine CUDA/GPU-Abhängigkeit
                // im Kiosk-Setup, das auf beliebiger Eltern-Hardware laufen muss.
                GpuLayerCount = 0
            };

            _weights = LLamaWeights.LoadFromFile(modelParams);
            _executor = new StatelessExecutor(_weights, modelParams);
            _loadedModelPath = _options.ModelPath;

            return _executor;
        }
        finally
        {
            _lock.Release();
        }
    }

    public void Dispose()
    {
        _weights?.Dispose();
        _lock.Dispose();
    }
}
