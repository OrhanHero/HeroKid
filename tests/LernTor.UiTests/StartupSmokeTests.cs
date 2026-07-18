using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Prozess-Smoke-Test: startet die ECHTE LernTor.exe (inkl. DI-Container, Datenbank-Anlage,
/// App.xaml-Ressourcen, MainWindow) mit LERNTOR_SKIP_LOCK=1. Erfolgskriterium ist der
/// Log-Marker "Hauptfenster angezeigt" im heutigen AppLog - Fenster-Titel/-Handles sind auf dem
/// CI-Runner (nicht-interaktive Session) NICHT zuverlässig auslesbar, das Fehlerprotokoll ist es
/// immer. Zusätzlich schlägt der Test fehl, wenn während des Starts neue ERROR-Zeilen ins
/// Protokoll geschrieben werden (z.B. der Dev-Modus-Fehlerdialog, der den Prozess am Leben
/// hielte). Deckt genau den Pfad ab, den XamlLoadTests nicht abdecken kann: App.OnStartup,
/// DI-Auflösung, EnsureCreated/SchemaUpdater und das MainWindow selbst.
///
/// <para>Hinweis: nutzt die echte DB unter %LOCALAPPDATA%\LernTor des ausführenden Kontos
/// (auf dem CI-Runner wegwerfbar; auf einer Entwickler-Maschine wird die vorhandene DB nur
/// gelesen/ergänzt, nie gelöscht).</para>
/// </summary>
public sealed class StartupSmokeTests
{
    private const string SuccessMarker = "Hauptfenster angezeigt";

    [Fact]
    public void App_startet_bis_zum_Hauptfenster_ohne_Fehler_im_Protokoll()
    {
        var exePath = FindAppExe();
        var logPath = TodayLogPath();
        var baselineLength = File.Exists(logPath) ? new FileInfo(logPath).Length : 0;

        var startInfo = new ProcessStartInfo(exePath) { UseShellExecute = false };
        // Ohne diese Variable würde der Test den CI-Runner (bzw. Entwickler-PC) in den
        // Kiosk-Modus sperren.
        startInfo.Environment["LERNTOR_SKIP_LOCK"] = "1";

        using var process = Process.Start(startInfo)!;
        try
        {
            var deadline = DateTime.UtcNow.AddSeconds(90);
            while (DateTime.UtcNow < deadline)
            {
                var newLogContent = ReadLogSince(logPath, baselineLength);

                Assert.False(newLogContent.Contains("ERROR"),
                    "LernTor hat beim Start Fehler protokolliert:\n" + newLogContent);

                if (newLogContent.Contains(SuccessMarker))
                {
                    return; // Hauptfenster wurde angezeigt, kein Fehler protokolliert - Start ok.
                }

                Assert.False(process.HasExited,
                    $"LernTor.exe hat sich beim Start selbst beendet (ExitCode {(process.HasExited ? process.ExitCode : 0)}). " +
                    $"Protokoll seit Teststart:\n{newLogContent}");

                Thread.Sleep(500);
            }

            Assert.Fail(
                $"Log-Marker \"{SuccessMarker}\" ist innerhalb von 90s nicht im Fehlerprotokoll erschienen. " +
                $"Protokoll seit Teststart:\n{ReadLogSince(logPath, baselineLength)}");
        }
        finally
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // Prozess war schon beendet.
            }
        }
    }

    private static string TodayLogPath() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "LernTor", "logs", $"lerntor-{DateTime.Now:yyyy-MM-dd}.log");

    /// <summary>Liest alles, was seit <paramref name="baselineLength"/> ins Protokoll geschrieben
    /// wurde - mit FileShare.ReadWrite, da die App die Datei parallel beschreibt.</summary>
    private static string ReadLogSince(string logPath, long baselineLength)
    {
        if (!File.Exists(logPath))
        {
            return string.Empty;
        }

        using var stream = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        if (stream.Length <= baselineLength)
        {
            return string.Empty;
        }

        stream.Seek(baselineLength, SeekOrigin.Begin);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Findet die gebaute LernTor.exe relativ zum Test-Ausgabeverzeichnis
    /// (tests/LernTor.UiTests/bin/&lt;Konfiguration&gt;/net8.0-windows/ →
    /// src/LernTor.App/bin/&lt;Konfiguration&gt;/net8.0-windows/LernTor.exe) - dieselbe
    /// Konfiguration, mit der auch die Tests gebaut wurden.
    /// </summary>
    private static string FindAppExe()
    {
        var configuration = AppContext.BaseDirectory.Contains($"{Path.DirectorySeparatorChar}Release{Path.DirectorySeparatorChar}")
            ? "Release"
            : "Debug";

        var exePath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "src", "LernTor.App", "bin", configuration, "net8.0-windows", "LernTor.exe"));

        Assert.True(File.Exists(exePath),
            $"LernTor.exe nicht gefunden unter {exePath} - wurde die Solution in derselben Konfiguration ({configuration}) gebaut?");

        return exePath;
    }
}
