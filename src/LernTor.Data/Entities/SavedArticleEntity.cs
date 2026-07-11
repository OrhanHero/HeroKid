namespace LernTor.Data.Entities;

/// <summary>
/// Ein vom Kind gemerkter News-Artikel (🔖 Lesezeichen) - der komplette aufbereitete Text wird
/// mitgespeichert, damit gemerkte Artikel auch OHNE Internet wieder lesbar sind (Offline-Lesen).
/// Pro Profil gespeichert; RSS-Artikel selbst werden weiterhin nicht persistiert, nur das, was
/// das Kind aktiv gemerkt hat.
/// </summary>
public sealed class SavedArticleEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Die Feed-Artikel-Id - verhindert doppeltes Merken desselben Artikels.</summary>
    public string ArticleId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string SimplifiedSummary { get; set; } = string.Empty;
    public string SourceName { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string CategoryEmoji { get; set; } = string.Empty;

    /// <summary>Rubrikname als String (JsonStringEnumConverter-Prinzip: kein numerisches Enum in
    /// der DB, damit spätere Enum-Umsortierungen alte Daten nicht stumm uminterpretieren).</summary>
    public string Category { get; set; } = string.Empty;

    public string? BerlinDistrict { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset SavedAt { get; set; }
}
