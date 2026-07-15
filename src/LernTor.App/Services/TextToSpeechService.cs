using System.Globalization;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace LernTor.App.Services;

/// <summary>
/// Vorlesefunktion - komplett offline, passend zum Kein-Cloud-Prinzip der App. Zwei Stufen:
///
/// <para><b>1. Piper (bevorzugt):</b> Sind die natürlichen Piper-Stimmen installiert (einmaliger
/// Download im Eltern-Bereich, siehe <see cref="PiperTtsEngine"/>), wird der Text satzweise per
/// piper.exe zu WAV synthetisiert und über einen WPF-<see cref="MediaPlayer"/> abgespielt.
/// Satzweise deshalb, weil die Synthese eines mehrminütigen Textes am Stück spürbar dauern würde -
/// so beginnt die Wiedergabe nach dem ersten Satz, während der nächste bereits im Hintergrund
/// erzeugt wird (Vorab-Synthese um genau ein Häppchen).</para>
///
/// <para><b>2. SAPI-Rückfall:</b> Ohne installierte Piper-Stimmen (oder wenn Piper zur Laufzeit
/// scheitert) liest die Windows-eigene Sprachausgabe (System.Speech) vor - roboterhaft, aber ohne
/// jeden Download verfügbar. Sprachauswahl per <c>SelectVoiceByHints(..., culture)</c>; eine
/// türkische SAPI-Stimme ist häufig NICHT vorinstalliert - dann wird still mit der Standardstimme
/// weitergelesen statt zu crashen: falsch betonte Vorlesung ist besser als eine Fehlermeldung
/// mitten im Pflicht-Leseteil.</para>
/// </summary>
public sealed class TextToSpeechService : IDisposable
{
    /// <summary>Zielgröße eines Synthese-Häppchens: klein genug für schnellen Wiedergabestart,
    /// groß genug, um nicht für jeden Kurzsatz einen eigenen piper.exe-Prozess zu starten.</summary>
    private const int MaxChunkLength = 280;

    private readonly SpeechSynthesizer _synthesizer = new();
    private readonly MediaPlayer _mediaPlayer = new();
    private readonly PiperTtsEngine _piper;

    private CancellationTokenSource? _piperCts;
    private Prompt? _currentSapiPrompt;

    /// <summary>Läuft bei jedem Speak/Stop hoch; asynchron eintreffende Abschluss-Ereignisse einer
    /// ÄLTEREN Vorlesung (abgebrochene SAPI-Prompts, auslaufende Piper-Pipelines) dürfen den
    /// Sprechzustand der aktuellen nicht mehr umwerfen.</summary>
    private int _speakVersion;

    /// <summary>Wird bei jedem Start/Stopp gefeuert, damit der Vorlesen-Button seinen Zustand
    /// (▶/⏹) nachführen kann. Alle Zustandswechsel passieren auf dem UI-Thread: Speak/Stop werden
    /// aus Commands aufgerufen, die async-Fortsetzungen laufen über den WPF-SynchronizationContext,
    /// und SpeechSynthesizer-Ereignisse kommen über den Kontext des erzeugenden Threads an.</summary>
    public event Action<bool>? SpeakingChanged;

    public bool IsSpeaking { get; private set; }

    public TextToSpeechService(PiperTtsEngine piper)
    {
        _piper = piper;

        _synthesizer.SetOutputToDefaultAudioDevice();
        // Etwas langsamer als Standard (Skala -10..10): kindgerechtes Vorlesetempo.
        _synthesizer.Rate = -1;
        _synthesizer.SpeakCompleted += (_, e) =>
        {
            // Nur der aktuell laufende Prompt darf den Zustand beenden - das Abbrechen eines alten
            // Prompts (SpeakAsyncCancelAll beim Start einer neuen Vorlesung) feuert dasselbe Ereignis.
            if (e.Prompt == _currentSapiPrompt)
            {
                _currentSapiPrompt = null;
                SetSpeaking(false);
            }
        };

        // MediaPlayer startet mit Lautstärke 0.5 - für den Vorlese-Zweck volle Lautstärke.
        _mediaPlayer.Volume = 1.0;

        PiperTtsEngine.CleanupCache();
    }

    /// <summary>Startet das Vorlesen von <paramref name="text"/> in der Sprache
    /// <paramref name="cultureName"/> (z.B. "de-DE", "tr-TR", "en-US"); bricht eine eventuell
    /// laufende Ausgabe vorher ab.</summary>
    public void Speak(string text, string cultureName)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        Stop();
        var version = ++_speakVersion;
        SetSpeaking(true);

        if (_piper.IsInstalled)
        {
            _piperCts = new CancellationTokenSource();
            _ = RunPiperPipelineAsync(text, cultureName, version, _piperCts.Token);
        }
        else
        {
            SpeakWithSapi(text, cultureName);
        }
    }

    public void Stop()
    {
        _speakVersion++;
        _piperCts?.Cancel();
        _piperCts = null;
        _currentSapiPrompt = null;
        _synthesizer.SpeakAsyncCancelAll();
        _mediaPlayer.Stop();
        _mediaPlayer.Close();
        SetSpeaking(false);
    }

    private void SpeakWithSapi(string text, string cultureName)
    {
        try
        {
            _synthesizer.SelectVoiceByHints(
                VoiceGender.NotSet, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo(cultureName));
        }
        catch (Exception)
        {
            // Keine Stimme für diese Sprache installiert - Standardstimme weiterverwenden (s.o.).
            LernTor.Core.Logging.AppLog.Warn(
                "TTS", $"Keine SAPI-Stimme für {cultureName} installiert - Standardstimme wird verwendet");
        }

        _currentSapiPrompt = _synthesizer.SpeakAsync(text);
    }

    private async Task RunPiperPipelineAsync(string text, string cultureName, int version, CancellationToken cancellationToken)
    {
        try
        {
            await SpeakChunksWithPiperAsync(text, cultureName, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return; // Stop() hat den Sprechzustand bereits zurückgesetzt.
        }
        catch (Exception ex)
        {
            // Piper mittendrin gescheitert (Datei beschädigt, Audio-Gerät weg, …): für DIESE
            // Vorlesung auf SAPI zurückfallen, sofern nicht inzwischen etwas Neues gestartet wurde.
            LernTor.Core.Logging.AppLog.Warn("TTS", $"Piper fehlgeschlagen, SAPI-Rückfall - {ex.Message}");
            if (version == _speakVersion)
            {
                SpeakWithSapi(text, cultureName);
            }
            return;
        }

        if (version == _speakVersion)
        {
            SetSpeaking(false);
        }
    }

    private async Task SpeakChunksWithPiperAsync(string text, string cultureName, CancellationToken cancellationToken)
    {
        var chunks = SplitIntoChunks(text);
        Task<string>? nextSynthesis = _piper.SynthesizeToWavAsync(chunks[0], cultureName, cancellationToken);

        try
        {
            for (var i = 0; i < chunks.Count; i++)
            {
                var wavPath = await nextSynthesis!;
                // Nächstes Häppchen schon synthetisieren, WÄHREND das aktuelle abgespielt wird.
                nextSynthesis = i + 1 < chunks.Count
                    ? _piper.SynthesizeToWavAsync(chunks[i + 1], cultureName, cancellationToken)
                    : null;

                try
                {
                    await PlayWavAsync(wavPath, cancellationToken);
                }
                finally
                {
                    PiperTtsEngine.TryDelete(wavPath);
                }
            }
        }
        finally
        {
            // Bei Abbruch/Fehler läuft ggf. noch eine Vorab-Synthese: deren WAV nach Fertigstellung
            // wegräumen (Rest erledigt CleanupCache beim nächsten App-Start).
            nextSynthesis?.ContinueWith(
                t => PiperTtsEngine.TryDelete(t.Result),
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }
    }

    private async Task PlayWavAsync(string wavPath, CancellationToken cancellationToken)
    {
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        EventHandler onEnded = (_, _) => completion.TrySetResult();
        EventHandler<ExceptionEventArgs> onFailed = (_, e) => completion.TrySetException(e.ErrorException);

        _mediaPlayer.MediaEnded += onEnded;
        _mediaPlayer.MediaFailed += onFailed;
        await using var registration = cancellationToken.Register(() => completion.TrySetCanceled(cancellationToken));

        try
        {
            _mediaPlayer.Open(new Uri(wavPath, UriKind.Absolute));
            _mediaPlayer.Play();
            await completion.Task;
        }
        finally
        {
            _mediaPlayer.MediaEnded -= onEnded;
            _mediaPlayer.MediaFailed -= onFailed;
            _mediaPlayer.Stop();
            _mediaPlayer.Close();
        }
    }

    /// <summary>Teilt den Text an Satzenden und fasst Sätze zu Häppchen von maximal
    /// <see cref="MaxChunkLength"/> Zeichen zusammen (ein überlanger Einzelsatz bleibt ganz).</summary>
    private static List<string> SplitIntoChunks(string text)
    {
        var sentences = Regex.Split(text, @"(?<=[.!?…;:])\s+")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        var chunks = new List<string>();
        var current = string.Empty;

        foreach (var sentence in sentences)
        {
            if (current.Length > 0 && current.Length + sentence.Length + 1 > MaxChunkLength)
            {
                chunks.Add(current);
                current = sentence;
            }
            else
            {
                current = current.Length == 0 ? sentence : $"{current} {sentence}";
            }
        }

        if (current.Length > 0)
        {
            chunks.Add(current);
        }

        return chunks.Count > 0 ? chunks : new List<string> { text };
    }

    private void SetSpeaking(bool value)
    {
        if (IsSpeaking == value)
        {
            return;
        }

        IsSpeaking = value;
        SpeakingChanged?.Invoke(value);
    }

    public void Dispose()
    {
        _piperCts?.Cancel();
        _synthesizer.Dispose();
    }
}
