using System.Text;

namespace LernTor.Core.Logging;

/// <summary>
/// Minimales lokales Fehler-/Ereignisprotokoll: eine Textdatei pro Tag unter
/// <c>%LOCALAPPDATA%\LernTor\logs\lerntor-JJJJ-MM-TT.log</c>, Dateien älter als 14 Tage werden
/// automatisch entfernt. Komplett lokal, keine Telemetrie - nichts verlässt den PC.
///
/// <para><b>Warum ein eigener statischer Logger statt ILogger/Serilog:</b> Er muss aus allen
/// Schichten (Core/News/ContentGen/App) und aus statischen Kontexten (globale Exception-Handler,
/// bewusst still geschluckte Fehler wie übersprungene RSS-Feeds) ohne DI-Verkabelung erreichbar
/// sein - und die App verschluckt viele Fehler ABSICHTLICH, damit der Kiosk-Ablauf nie hängen
/// bleibt; genau diese unsichtbaren Stellen sollen mit einer Zeile nachvollziehbar werden.</para>
///
/// <para>Das Protokollieren selbst darf NIE eine Exception werfen (volle Platte, gesperrte
/// Datei, ...) - ein Logger, der die App crasht, wäre schlimmer als gar keiner. Alle Fehler beim
/// Schreiben werden verschluckt.</para>
/// </summary>
public static class AppLog
{
    private static readonly object WriteLock = new();
    private static bool _cleanupDone;

    /// <summary>Aufbewahrungsdauer der Tagesdateien.</summary>
    private const int RetentionDays = 14;

    public static string LogDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LernTor", "logs");

    private static string TodayLogPath =>
        Path.Combine(LogDirectory, $"lerntor-{DateTime.Now:yyyy-MM-dd}.log");

    public static void Info(string source, string message) => Write("INFO ", source, message, null);

    /// <summary>Für bewusst abgefangene Fehler, die den Ablauf nicht stören sollen (übersprungener
    /// Feed, Wetter nicht ladbar, TTS-Rückfall) - unsichtbar fürs Kind, sichtbar im Protokoll.</summary>
    public static void Warn(string source, string message) => Write("WARN ", source, message, null);

    public static void Error(string source, string message, Exception? exception = null) =>
        Write("ERROR", source, message, exception);

    private static void Write(string level, string source, string message, Exception? exception)
    {
        try
        {
            lock (WriteLock)
            {
                Directory.CreateDirectory(LogDirectory);

                if (!_cleanupDone)
                {
                    _cleanupDone = true;
                    CleanupOldLogs();
                }

                var line = new StringBuilder()
                    .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .Append(" [").Append(level).Append("] ")
                    .Append(source).Append(": ")
                    .Append(message.ReplaceLineEndings(" "));

                if (exception is not null)
                {
                    line.AppendLine().Append("    ").Append(exception.ToString().ReplaceLineEndings(Environment.NewLine + "    "));
                }

                File.AppendAllText(TodayLogPath, line.AppendLine().ToString());
            }
        }
        catch
        {
            // Logging darf die App niemals stören (s. Klassen-Kommentar).
        }
    }

    private static void CleanupOldLogs()
    {
        var cutoff = DateTime.Now.AddDays(-RetentionDays);
        foreach (var file in Directory.EnumerateFiles(LogDirectory, "lerntor-*.log"))
        {
            try
            {
                if (File.GetLastWriteTime(file) < cutoff)
                {
                    File.Delete(file);
                }
            }
            catch
            {
                // Nicht löschbare Alt-Datei: beim nächsten Start erneut versuchen.
            }
        }
    }

    /// <summary>Die letzten <paramref name="maxLines"/> Zeilen des heutigen Protokolls für die
    /// Anzeige im Eltern-Bereich; leerer String, wenn heute (noch) nichts protokolliert wurde.</summary>
    public static string ReadTodayTail(int maxLines = 50)
    {
        try
        {
            lock (WriteLock)
            {
                if (!File.Exists(TodayLogPath))
                {
                    return string.Empty;
                }

                var lines = File.ReadAllLines(TodayLogPath);
                return string.Join(Environment.NewLine, lines.TakeLast(maxLines));
            }
        }
        catch
        {
            return string.Empty;
        }
    }
}
