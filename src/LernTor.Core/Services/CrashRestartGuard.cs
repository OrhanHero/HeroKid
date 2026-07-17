using LernTor.Core.Logging;

namespace LernTor.Core.Services;

/// <summary>
/// Schutz gegen Absturz-Endlosschleifen beim automatischen Neustart im Kiosk-Modus (siehe
/// App.xaml.cs): Vor jedem automatischen Neustart wird der Zeitpunkt in einer kleinen Textdatei
/// (<c>%LOCALAPPDATA%\LernTor\crash-restarts.txt</c>, ein ISO-Zeitstempel pro Zeile) vermerkt.
/// Liegen bereits <see cref="MaxRestarts"/> Neustarts innerhalb von <see cref="Window"/> zurück,
/// wird KEIN weiterer Neustart erlaubt - dann greift das Soft-Lock-Prinzip der App (Absturz =>
/// Desktop bleibt erreichbar) statt einer Endlosschleife aus Starten und Sofort-Crashen, z.B.
/// bei einer korrupten Datenbank oder einem Fehler direkt im Startpfad.
///
/// <para>Die Datei muss den Prozess-Neustart überleben (In-Memory-Zähler gehen beim Neustart
/// verloren) - daher eine Datei statt eines statischen Felds. Bei IO-Fehlern (volle Platte,
/// gesperrte Datei) wird der Neustart sicherheitshalber VERWEIGERT: lieber einmal zum Desktop
/// durchfallen als eine unbegrenzte, nicht mehr nachvollziehbare Neustart-Schleife riskieren.</para>
/// </summary>
public sealed class CrashRestartGuard
{
    public const int MaxRestarts = 3;
    public static readonly TimeSpan Window = TimeSpan.FromMinutes(10);

    private readonly string _stateFilePath;

    public CrashRestartGuard()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LernTor", "crash-restarts.txt"))
    {
    }

    /// <summary>Testkonstruktor mit eigener Zustandsdatei (z.B. im Temp-Ordner).</summary>
    public CrashRestartGuard(string stateFilePath)
    {
        _stateFilePath = stateFilePath;
    }

    /// <summary>
    /// Registriert einen bevorstehenden automatischen Neustart. true = Neustart erlaubt (und
    /// vermerkt), false = Crash-Schleife erkannt oder Zustandsdatei nicht schreibbar - der
    /// Aufrufer darf dann NICHT neu starten.
    /// </summary>
    /// <param name="now">Nur für Tests: "Jetzt"-Zeitpunkt für die Fensterprüfung.</param>
    public bool TryRegisterRestart(DateTimeOffset? now = null)
    {
        var currentTime = now ?? DateTimeOffset.UtcNow;

        try
        {
            var recentRestarts = new List<DateTimeOffset>();
            if (File.Exists(_stateFilePath))
            {
                foreach (var line in File.ReadAllLines(_stateFilePath))
                {
                    // Nicht parsebare Zeilen (halb geschriebene Datei, Altformat) einfach
                    // überspringen statt den Guard lahmzulegen.
                    if (DateTimeOffset.TryParse(line, out var timestamp) &&
                        currentTime - timestamp <= Window)
                    {
                        recentRestarts.Add(timestamp);
                    }
                }
            }

            if (recentRestarts.Count >= MaxRestarts)
            {
                return false;
            }

            recentRestarts.Add(currentTime);
            Directory.CreateDirectory(Path.GetDirectoryName(_stateFilePath)!);
            File.WriteAllLines(_stateFilePath, recentRestarts.Select(t => t.ToString("O")));
            return true;
        }
        catch (Exception ex)
        {
            AppLog.Warn("App", $"CrashRestartGuard: Zustandsdatei nicht nutzbar, Neustart wird verweigert - {ex.Message}");
            return false;
        }
    }
}
