namespace LernTor.Data.Entities;

/// <summary>
/// Das Zeitraster einer Schule, je Profil eine Zeile pro Stunde (siehe
/// Core.Models.TimetablePeriod).
///
/// <para><b>Warum pro Profil und nicht einmal für die App:</b> die beiden Kinder gehen auf
/// verschiedene Schulen mit verschiedenen Anfangs- und Endzeiten. Ein fest eingebautes Raster
/// hätte bei einem der beiden immer danebengelegen - und die Anzeige „läuft gerade“ auf der
/// Startseite wäre damit schlicht falsch.</para>
///
/// <para>Die Zeiten stehen als "HH:mm"-String da: SQLite kennt keinen eigenen Zeittyp, und in
/// dieser Schreibweise ist die Textsortierung zugleich die Zeitsortierung.</para>
/// </summary>
public sealed class TimetablePeriodEntity
{
    public string Id { get; set; } = string.Empty;

    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Stundennummer, 1-basiert.</summary>
    public int Period { get; set; }

    /// <summary>Beginn im Format "HH:mm".</summary>
    public string Start { get; set; } = string.Empty;

    /// <summary>Ende im Format "HH:mm".</summary>
    public string End { get; set; } = string.Empty;
}
