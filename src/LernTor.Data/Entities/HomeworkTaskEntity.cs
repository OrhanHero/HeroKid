namespace LernTor.Data.Entities;

/// <summary>
/// Eine von den Eltern eingetragene Hausaufgabe mit Stichtag (siehe Core.Models.HomeworkTask).
/// Subject als String, nicht als int - ein späteres Umsortieren des Enums soll bestehende
/// Einträge nicht stillschweigend auf ein anderes Fach umdeuten (siehe CLAUDE.md).
/// Das Fälligkeitsdatum ebenfalls als String ("yyyy-MM-dd"): SQLite kennt keinen eigenen
/// Datumstyp, und EF Core kann auf DateOnly/DateTimeOffset-Spalten nicht serverseitig sortieren.
/// </summary>
public sealed class HomeworkTaskEntity
{
    public string Id { get; set; } = string.Empty;

    public string ProfileId { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>Stichtag im Format "yyyy-MM-dd" - so bleibt die Textsortierung zugleich Datumssortierung.</summary>
    public string DueDate { get; set; } = string.Empty;

    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>"Eltern" oder "Kind" - entscheidet, ob das Kind den Eintrag entfernen darf.
    /// Alt-Zeilen bekommen beim additiven Schema-Abgleich einen leeren String und fallen damit
    /// auf "Eltern" zurueck, also auf das bisherige Verhalten.</summary>
    public string Author { get; set; } = "Eltern";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
