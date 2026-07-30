namespace LernTor.Data.Entities;

/// <summary>
/// Ein von den Eltern hinterlegtes Vokabelpaar (siehe <c>VocabularyRepository</c>).
/// Subject als String, damit ein künftiges Umsortieren des Enums bestehende Einträge nicht
/// stillschweigend einem anderen Fach zuordnet - gleiches Muster wie bei den eigenen Aufgaben.
/// </summary>
public sealed class VocabularyEntryEntity
{
    public string Id { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string German { get; set; } = string.Empty;
    public string Foreign { get; set; } = string.Empty;

    /// <summary>Meisterungs-Stufe (0 = noch nie richtig beantwortet), steuert das Wiederholungs-Intervall.</summary>
    public int Stage { get; set; }

    /// <summary>Nächste Fälligkeit; NULL = sofort fällig (frisch eingetragen oder zuletzt falsch).</summary>
    public DateTimeOffset? NextDueAt { get; set; }

    public int WrongCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
