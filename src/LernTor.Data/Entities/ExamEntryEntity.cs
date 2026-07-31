namespace LernTor.Data.Entities;

/// <summary>
/// Ein Klausurtermin (siehe Core.Models.ExamEntry). Fach, Herkunft und Datum als String -
/// ein spaeteres Umsortieren der Enums soll bestehende Eintraege nicht umdeuten, und das
/// Datum bleibt als "yyyy-MM-dd" zugleich textuell sortierbar (SQLite kennt keinen Datumstyp).
/// </summary>
public sealed class ExamEntryEntity
{
    public string Id { get; set; } = string.Empty;

    public string ProfileId { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Topics { get; set; } = string.Empty;

    /// <summary>Termin im Format "yyyy-MM-dd".</summary>
    public string ExamDate { get; set; } = string.Empty;

    /// <summary>"Eltern" oder "Kind" - entscheidet, ob das Kind den Eintrag aendern darf.</summary>
    public string Author { get; set; } = "Eltern";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
