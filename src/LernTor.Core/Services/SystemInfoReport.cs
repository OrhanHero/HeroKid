using System.Globalization;

namespace LernTor.Core.Services;

/// <summary>Was die Systeminfo im Eltern-Bereich anzeigt - gesammelt von der App.</summary>
public sealed record SystemInfoSnapshot(
    string? InformationalVersion,
    string RuntimeVersion,
    string OsDescription,
    long? DatabaseBytes,
    DateTimeOffset? LastAutoBackup,
    int ProfileCount,
    string DataDirectory);

/// <summary>
/// Formatiert die Systeminfo für den Eltern-Bereich: App-Version, .NET-Version, Windows,
/// Datenbankgröße, letzte automatische Sicherung. Das sind die ersten Fragen bei jedem Problem -
/// und ohne diese Anzeige muss man sie jemandem am Telefon aus dem Explorer vorlesen lassen.
/// Rein und ohne Dateizugriff, damit das Format testbar ist; die App sammelt die Werte.
/// </summary>
public static class SystemInfoReport
{
    private static readonly CultureInfo Deutsch = CultureInfo.GetCultureInfo("de-DE");

    public static IReadOnlyList<string> Lines(SystemInfoSnapshot info)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new[]
        {
            $"LernTor {AppVersion(info.InformationalVersion)}",
            $".NET {info.RuntimeVersion} · {info.OsDescription}",
            $"Datenbank: {(info.DatabaseBytes is { } bytes ? FormatBytes(bytes) : "nicht gefunden")} · {info.ProfileCount} Profil(e)",
            $"Letzte automatische Sicherung: {(info.LastAutoBackup is { } zeit ? zeit.LocalDateTime.ToString("dd.MM.yyyy HH:mm", Deutsch) : "noch keine")}",
            $"Datenordner: {info.DataDirectory}",
        };
    }

    /// <summary>
    /// „2.0.0 (245a544)“ aus „2.0.0+245a544b3c…“ - das SDK hängt den Git-Stand an die
    /// Versionsangabe an. Sieben Zeichen reichen, um den Commit wiederzufinden.
    /// </summary>
    public static string AppVersion(string? informationalVersion)
    {
        if (string.IsNullOrWhiteSpace(informationalVersion))
        {
            return "(unbekannt)";
        }

        var teile = informationalVersion.Split('+', 2);
        if (teile.Length == 1 || teile[1].Length == 0)
        {
            return teile[0];
        }

        var stand = teile[1].Length > 7 ? teile[1][..7] : teile[1];
        return $"{teile[0]} ({stand})";
    }

    /// <summary>„812 KB“, „3,2 MB“ - deutsche Schreibweise, eine Nachkommastelle ab MB.</summary>
    public static string FormatBytes(long bytes)
    {
        if (bytes < 0)
        {
            bytes = 0;
        }

        return bytes switch
        {
            < 1024 => $"{bytes} B",
            < 1024 * 1024 => $"{Math.Round(bytes / 1024.0).ToString("0", Deutsch)} KB",
            < 1024L * 1024 * 1024 => $"{(bytes / (1024.0 * 1024)).ToString("0.0", Deutsch)} MB",
            _ => $"{(bytes / (1024.0 * 1024 * 1024)).ToString("0.0", Deutsch)} GB",
        };
    }
}
