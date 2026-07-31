using System.Text.Json;
using LernTor.Core.Logging;

namespace LernTor.News;

/// <summary>Zustand einer Nachrichtenquelle beim letzten tatsächlichen Abruf.</summary>
/// <param name="FeedName">Name aus <see cref="CuratedNewsFeeds.All"/> - zugleich der Schlüssel.</param>
/// <param name="CheckedAt">Wann zuletzt abgerufen wurde.</param>
/// <param name="IsHealthy">Ob der Abruf einen brauchbaren Artikel geliefert hat.</param>
/// <param name="Error">Kurzer Fehlertext, wenn nicht.</param>
public sealed record FeedHealthEntry(string FeedName, DateTimeOffset CheckedAt, bool IsHealthy, string? Error);

/// <summary>
/// Merkt sich, welche Quellen beim letzten Abruf funktioniert haben - damit im Eltern-Bereich
/// sichtbar wird, was sonst nur im Fehlerprotokoll steht.
///
/// <para>RSS-Endpunkte sterben schleichend: Anbieter strukturieren um, und die App überspringt
/// tote Feeds bewusst geräuschlos, damit ein einzelner Ausfall nicht den Lernablauf des Kindes
/// blockiert. Bei 44 Quellen heißt das aber, dass die Hälfte kaputt sein könnte, ohne dass es
/// jemandem auffällt. Der wöchentliche <c>feed-healthcheck</c>-Lauf auf GitHub prüft zwar alle
/// URLs, aber den sieht nur, wer ins Repository schaut.</para>
///
/// <para>Wichtig für die Anzeige: seit der Tagesrotation wird längst nicht mehr jede Quelle
/// täglich abgerufen. Ein Eintrag kann also mehrere Tage alt sein - deshalb wird der Zeitpunkt
/// mitgespeichert und im Eltern-Bereich mit angezeigt, statt einen alten Stand als aktuell
/// auszugeben.</para>
///
/// <para>Speicherort <c>%LOCALAPPDATA%\LernTor\feedhealth.json</c>. Alle IO-Fehler werden
/// verschluckt und nur protokolliert - eine Diagnose-Hilfe darf den News-Abruf niemals
/// gefährden (gleiche Philosophie wie <see cref="FeedCache"/>).</para>
/// </summary>
public sealed class FeedHealthLog
{
    private readonly string _filePath;
    private readonly object _gate = new();

    public FeedHealthLog()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LernTor", "feedhealth.json"))
    {
    }

    public FeedHealthLog(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>Hält das Ergebnis eines Abrufs fest. Überschreibt den vorherigen Stand der Quelle.</summary>
    public void Record(string feedName, bool isHealthy, string? error = null)
    {
        try
        {
            lock (_gate)
            {
                var entries = LoadInternal();
                entries[feedName] = new FeedHealthEntry(
                    feedName,
                    DateTimeOffset.Now,
                    isHealthy,
                    // Lange .NET-Ausnahmetexte helfen Eltern nicht - der Anfang reicht zur Einordnung.
                    isHealthy ? null : Shorten(error));

                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_filePath, JsonSerializer.Serialize(entries.Values.ToList()));
            }
        }
        catch (Exception ex)
        {
            AppLog.Warn("News", $"Feed-Zustand konnte nicht gespeichert werden - {ex.Message}");
        }
    }

    /// <summary>Alle bekannten Zustände, Schlüssel ist der Quellenname.</summary>
    public IReadOnlyDictionary<string, FeedHealthEntry> LoadAll()
    {
        try
        {
            lock (_gate)
            {
                return LoadInternal();
            }
        }
        catch (Exception ex)
        {
            AppLog.Warn("News", $"Feed-Zustand konnte nicht gelesen werden - {ex.Message}");
            return new Dictionary<string, FeedHealthEntry>();
        }
    }

    private Dictionary<string, FeedHealthEntry> LoadInternal()
    {
        if (!File.Exists(_filePath))
        {
            return new Dictionary<string, FeedHealthEntry>();
        }

        var json = File.ReadAllText(_filePath);
        var entries = JsonSerializer.Deserialize<List<FeedHealthEntry>>(json);

        return entries is null
            ? new Dictionary<string, FeedHealthEntry>()
            : entries
                .Where(e => !string.IsNullOrWhiteSpace(e.FeedName))
                .GroupBy(e => e.FeedName)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(e => e.CheckedAt).First());
    }

    internal static string Shorten(string? error)
    {
        var text = (error ?? "Unbekannter Fehler").Replace('\n', ' ').Replace('\r', ' ').Trim();
        return text.Length <= 160 ? text : text[..160] + "…";
    }
}
