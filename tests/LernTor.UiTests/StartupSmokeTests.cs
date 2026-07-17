using System.Diagnostics;
using System.IO;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Prozess-Smoke-Test: startet die ECHTE LernTor.exe (inkl. DI-Container, Datenbank-Anlage,
/// App.xaml-Ressourcen, MainWindow) mit LERNTOR_SKIP_LOCK=1 und prüft, dass innerhalb der
/// Wartezeit das Hauptfenster erscheint statt eines Startabsturzes oder Fehlerdialogs. Deckt
/// genau den Pfad ab, den XamlLoadTests nicht abdecken kann: App.OnStartup, DI-Auflösung,
/// EnsureCreated/SchemaUpdater und das MainWindow selbst.
///
/// <para>Hinweis: nutzt die echte DB unter %LOCALAPPDATA%\LernTor des ausführenden Kontos
/// (auf dem CI-Runner wegwerfbar; auf einer Entwickler-Maschine wird die vorhandene DB nur
/// gelesen/ergänzt, nie gelöscht).</para>
/// </summary>
public sealed class StartupSmokeTests
{
    [Fact]
    public void App_startet_und_zeigt_das_Hauptfenster_ohne_Fehlerdialog()
    {
        var exePath = FindAppExe();

        var startInfo = new ProcessStartInfo(exePath) { UseShellExecute = false };
        // Ohne diese Variable würde der Test den CI-Runner (bzw. Entwickler-PC) in den
        // Kiosk-Modus sperren. Nebeneffekt: Fatal-Fehler zeigen im Dev-Modus eine MessageBox
        // ("LernTor - Fehler"/"LernTor - Startfehler") statt still neu zu starten - genau
        // darauf prüft die Schleife unten.
        startInfo.Environment["LERNTOR_SKIP_LOCK"] = "1";

        using var process = Process.Start(startInfo)!;
        try
        {
            var deadline = DateTime.UtcNow.AddSeconds(60);
            while (DateTime.UtcNow < deadline)
            {
                Assert.False(process.HasExited,
                    $"LernTor.exe hat sich beim Start selbst beendet (ExitCode {(process.HasExited ? process.ExitCode : 0)}) " +
                    "- Startabsturz? Fehlerprotokoll unter %LOCALAPPDATA%\\LernTor\\logs prüfen.");

                process.Refresh();
                var title = process.MainWindowTitle;

                Assert.False(title.Contains("Fehler", StringComparison.OrdinalIgnoreCase),
                    $"LernTor zeigt beim Start einen Fehlerdialog: \"{title}\" " +
                    "- Fehlerprotokoll unter %LOCALAPPDATA%\\LernTor\\logs prüfen.");

                if (process.MainWindowHandle != nint.Zero && title.Contains("LernTor", StringComparison.OrdinalIgnoreCase))
                {
                    return; // Hauptfenster ist da - Start erfolgreich.
                }

                Thread.Sleep(500);
            }

            Assert.Fail("Das LernTor-Hauptfenster ist innerhalb von 60s nicht erschienen " +
                        "(Prozess läuft, aber kein Fenster mit Titel \"LernTor\").");
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
