using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;

namespace LernTor.App.Services;

/// <summary>
/// Natürliche, komplett lokale Vorlesestimmen über Piper (https://github.com/rhasspy/piper):
/// ein kleines neuronales Text-zu-Sprache-Programm, das als eigenständige piper.exe plus je einer
/// Stimmdatei (.onnx) pro Sprache läuft - ohne Cloud, ohne Installation, CPU-schnell. Die
/// Windows-SAPI-Stimmen (System.Speech) klingen roboterhaft und Türkisch fehlt meist komplett;
/// Piper bringt für Deutsch, Türkisch UND Englisch je eine natürlich klingende Stimme mit.
///
/// <para><b>Bewusst KEIN LLM fürs Vorlesen:</b> Sprachmodelle erzeugen Text, keine Audiodaten -
/// für Sprachausgabe ist ein dediziertes TTS-Modell wie Piper das richtige (und viel kleinere)
/// Werkzeug.</para>
///
/// <para><b>Installation auf Abruf:</b> Eltern laden die Stimmen einmalig im Eltern-Bereich
/// herunter (~230 MB gesamt: piper.exe ~25 MB + drei "medium"-Stimmen à ~65 MB) nach
/// <c>%LOCALAPPDATA%\LernTor\piper\</c>. Ist Piper (noch) nicht installiert, liest
/// <see cref="TextToSpeechService"/> automatisch weiter über SAPI vor - das Kind merkt davon
/// nichts außer der Stimmqualität. Der Download nutzt denselben robusten Ansatz wie der
/// KI-Modell-Download (eigener HttpClient ohne 100-Sekunden-Timeout, Temp-Datei + Umbenennen,
/// mehrere Quell-URLs je Datei möglich).</para>
///
/// <para>Die Synthese ruft piper.exe als Unterprozess auf: Text über stdin (UTF-8), fertige
/// WAV-Datei über <c>--output_file</c>. Die passende <c>.onnx.json</c>-Konfiguration liegt neben
/// der Stimmdatei und wird von piper.exe selbst gefunden.</para>
/// </summary>
public sealed class PiperTtsEngine
{
    /// <summary>Eine herunterladbare Piper-Stimme; <paramref name="CulturePrefix"/> ("de", "tr",
    /// "en") ordnet sie der Vorlesesprache zu. Jede Stimme besteht aus Modell (.onnx) und
    /// Konfiguration (.onnx.json), beide müssen nebeneinander liegen.</summary>
    public sealed record PiperVoice(
        string CulturePrefix,
        string DisplayName,
        string ModelFileName,
        IReadOnlyList<string> ModelUrls,
        IReadOnlyList<string> ConfigUrls);

    // Feste Release-Version statt "latest": Piper-Releases ändern gelegentlich das Zip-Layout;
    // eine gepinnte Version hält Download-URL und erwarteten exe-Pfad zusammen konsistent.
    // Hinweis: GitHub-/Hugging-Face-URLs sind aus der Entwicklungsumgebung heraus nicht
    // erreichbar und daher nur gegen die öffentlich dokumentierten Pfade geprüft, nicht per
    // Testdownload - bei einem Fehlschlag beim Nutzer zuerst diese URLs im Browser gegenprüfen.
    private const string PiperZipUrl =
        "https://github.com/rhasspy/piper/releases/download/2023.11.14-2/piper_windows_amd64.zip";

    private const string VoiceBaseUrl = "https://huggingface.co/rhasspy/piper-voices/resolve/v1.0.0";

    /// <summary>Je Vorlesesprache genau eine kuratierte "medium"-Stimme (guter Kompromiss aus
    /// Qualität und Geschwindigkeit auf normaler Eltern-Hardware).</summary>
    public static readonly IReadOnlyList<PiperVoice> Voices = new[]
    {
        CreateVoice("de", "Thorsten (Deutsch)", "de/de_DE/thorsten/medium", "de_DE-thorsten-medium"),
        CreateVoice("tr", "Fahrettin (Türkçe)", "tr/tr_TR/fahrettin/medium", "tr_TR-fahrettin-medium"),
        CreateVoice("en", "Lessac (English)", "en/en_US/lessac/medium", "en_US-lessac-medium"),
    };

    private static PiperVoice CreateVoice(string culturePrefix, string displayName, string repoPath, string baseName) =>
        new(culturePrefix,
            displayName,
            baseName + ".onnx",
            new[] { $"{VoiceBaseUrl}/{repoPath}/{baseName}.onnx" },
            new[] { $"{VoiceBaseUrl}/{repoPath}/{baseName}.onnx.json" });

    /// <summary>Wie beim KI-Modell-Download: der geteilte HttpClient-Standard-Timeout von
    /// 100 Sekunden gilt auch fürs Auslesen des Antwort-Streams und würde die ~65-MB-Stimmdateien
    /// über langsames Internet mittendrin abbrechen.</summary>
    private static readonly HttpClient DownloadClient = CreateDownloadClient();

    private static HttpClient CreateDownloadClient()
    {
        var client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("LernTor/1.0");
        return client;
    }

    private static string RootDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LernTor", "piper");

    // Das offizielle Zip enthält einen Ordner "piper/" mit piper.exe, onnxruntime.dll und
    // espeak-ng-data - es wird unverändert nach RootDirectory entpackt.
    private static string ExecutablePath => Path.Combine(RootDirectory, "piper", "piper.exe");

    private static string VoicesDirectory => Path.Combine(RootDirectory, "voices");

    private static string ModelPath(PiperVoice voice) => Path.Combine(VoicesDirectory, voice.ModelFileName);

    private static string ConfigPath(PiperVoice voice) => Path.Combine(VoicesDirectory, voice.ModelFileName + ".json");

    /// <summary>True, sobald piper.exe und alle drei Stimmen vollständig vorliegen - erst dann
    /// schaltet <see cref="TextToSpeechService"/> von SAPI auf Piper um.</summary>
    public bool IsInstalled =>
        File.Exists(ExecutablePath)
        && Voices.All(v => File.Exists(ModelPath(v)) && File.Exists(ConfigPath(v)));

    /// <summary>
    /// Lädt piper.exe und alle Stimmen herunter (nur fehlende Dateien - ein abgebrochener
    /// Download setzt beim nächsten Versuch dort fort, wo Dateien schon fertig sind).
    /// <paramref name="progress"/> erhält fortlaufend kindgerecht formulierte Statuszeilen für
    /// die Anzeige im Eltern-Bereich.
    /// </summary>
    public async Task InstallAsync(IProgress<string>? progress, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(RootDirectory);
        Directory.CreateDirectory(VoicesDirectory);

        if (!File.Exists(ExecutablePath))
        {
            progress?.Report("Piper-Programm wird heruntergeladen (~25 MB)…");
            var zipPath = Path.Combine(RootDirectory, "piper.zip");
            await DownloadFileAsync(new[] { PiperZipUrl }, zipPath, cancellationToken);

            progress?.Report("Piper-Programm wird entpackt…");
            ZipFile.ExtractToDirectory(zipPath, RootDirectory, overwriteFiles: true);
            File.Delete(zipPath);

            if (!File.Exists(ExecutablePath))
            {
                throw new InvalidOperationException(
                    "Das Piper-Zip wurde entpackt, enthielt aber keine piper.exe am erwarteten Ort - " +
                    "vermutlich hat sich das Archiv-Layout der Piper-Version geändert.");
            }
        }

        foreach (var voice in Voices)
        {
            if (File.Exists(ModelPath(voice)) && File.Exists(ConfigPath(voice)))
            {
                continue;
            }

            progress?.Report($"Stimme \"{voice.DisplayName}\" wird heruntergeladen (~65 MB)…");
            await DownloadFileAsync(voice.ModelUrls, ModelPath(voice), cancellationToken);
            await DownloadFileAsync(voice.ConfigUrls, ConfigPath(voice), cancellationToken);
        }

        progress?.Report("Fertig - die natürlichen Vorlesestimmen sind einsatzbereit.");
    }

    private static async Task DownloadFileAsync(
        IReadOnlyList<string> urls, string destinationPath, CancellationToken cancellationToken)
    {
        var tempPath = destinationPath + ".download";
        Exception? lastError = null;

        foreach (var url in urls)
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
            $"\"{Path.GetFileName(destinationPath)}\" konnte nicht heruntergeladen werden " +
            $"(kein Internet, zu wenig Speicherplatz, oder die Download-Adresse hat sich geändert). " +
            $"Technischer Grund: {lastError?.Message}", lastError);
    }

    /// <summary>Wählt die Stimme zur Vorlesesprache ("de-DE" → Thorsten usw.); Deutsch als
    /// Rückfall, falls eine unbekannte Culture ankommt.</summary>
    private static PiperVoice VoiceForCulture(string cultureName)
    {
        var prefix = cultureName.Split('-')[0].ToLowerInvariant();
        return Voices.FirstOrDefault(v => v.CulturePrefix == prefix) ?? Voices[0];
    }

    /// <summary>
    /// Synthetisiert <paramref name="text"/> zu einer temporären WAV-Datei und liefert deren Pfad;
    /// der Aufrufer spielt sie ab und löscht sie danach. Ein Aufruf pro Satz-Häppchen (statt des
    /// ganzen Textes am Stück) hält die Wartezeit bis zum ersten hörbaren Ton kurz - siehe
    /// <see cref="TextToSpeechService"/>.
    /// </summary>
    public async Task<string> SynthesizeToWavAsync(string text, string cultureName, CancellationToken cancellationToken)
    {
        var voice = VoiceForCulture(cultureName);
        var cacheDir = Path.Combine(RootDirectory, "cache");
        Directory.CreateDirectory(cacheDir);
        var wavPath = Path.Combine(cacheDir, $"{Guid.NewGuid():N}.wav");

        var startInfo = new ProcessStartInfo
        {
            FileName = ExecutablePath,
            WorkingDirectory = Path.GetDirectoryName(ExecutablePath)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            // piper.exe erwartet UTF-8 auf stdin - ohne das werden ä/ö/ü/ç/ş/ğ falsch gelesen.
            StandardInputEncoding = Encoding.UTF8,
        };
        startInfo.ArgumentList.Add("--model");
        startInfo.ArgumentList.Add(ModelPath(voice));
        startInfo.ArgumentList.Add("--config");
        startInfo.ArgumentList.Add(ConfigPath(voice));
        startInfo.ArgumentList.Add("--output_file");
        startInfo.ArgumentList.Add(wavPath);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("piper.exe konnte nicht gestartet werden.");

        // stderr sofort und parallel leerlesen: piper schreibt dort Info-Zeilen; ein voller
        // Pipe-Puffer würde den Prozess sonst blockieren und WaitForExit nie zurückkehren lassen.
        var stderrTask = process.StandardError.ReadToEndAsync();

        try
        {
            // Zeilenumbrüche entfernen: piper behandelt jede stdin-Zeile als eigene Äußerung -
            // ein Satz-Häppchen soll aber genau eine Äußerung sein.
            await process.StandardInput.WriteLineAsync(text.ReplaceLineEndings(" "));
            process.StandardInput.Close();
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            try { process.Kill(entireProcessTree: true); } catch { /* Prozess ist ggf. schon weg. */ }
            TryDelete(wavPath);
            throw;
        }

        if (process.ExitCode != 0 || !File.Exists(wavPath))
        {
            var error = await stderrTask;
            TryDelete(wavPath);
            throw new InvalidOperationException(
                $"piper.exe hat keine Audiodatei erzeugt (ExitCode {process.ExitCode}): {error}");
        }

        return wavPath;
    }

    internal static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Aufräumen von Temp-WAVs darf nie das Vorlesen stören; verwaiste Dateien im
            // cache-Ordner werden beim nächsten App-Start erneut zum Löschen versucht.
        }
    }

    /// <summary>Räumt liegengebliebene Temp-WAVs aus abgestürzten/abgebrochenen Sitzungen weg
    /// (beim App-Start aufgerufen).</summary>
    public static void CleanupCache()
    {
        var cacheDir = Path.Combine(RootDirectory, "cache");
        if (!Directory.Exists(cacheDir))
        {
            return;
        }

        foreach (var file in Directory.EnumerateFiles(cacheDir, "*.wav"))
        {
            TryDelete(file);
        }
    }
}
