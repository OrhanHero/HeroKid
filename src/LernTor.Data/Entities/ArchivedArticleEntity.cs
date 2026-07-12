namespace LernTor.Data.Entities;

/// <summary>
/// Tages-Archiv der aufbereiteten News-Artikel (profilübergreifend - die Tagesartikel sind für
/// alle Kinder gleich). Zweck ist der OFFLINE-RÜCKFALL: sind morgens alle Feeds unerreichbar
/// (kein Internet, Anbieter down), liest das Kind die Artikel des letzten erfolgreichen Tages
/// statt vor einem leeren News-Teil zu stehen. Der komplette aufbereitete Zustand wird
/// gespeichert - inklusive der Verständnisfragen als JSON, denn ohne Fragen wäre der
/// Pflicht-News-Teil nicht abschließbar. Enums als Strings (übliche Regel), das Archivdatum als
/// "yyyy-MM-dd"-String, damit SQLite Vergleich/Sortierung serverseitig kann (DateTimeOffset
/// kann es nicht).
/// </summary>
public sealed class ArchivedArticleEntity
{
    public int Id { get; set; }

    /// <summary>Archivdatum als "yyyy-MM-dd" (sortier- und vergleichbar als String).</summary>
    public string ArchivedDate { get; set; } = string.Empty;

    public string ArticleId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string SimplifiedSummary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; }
    public string RegionFocus { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategoryEmoji { get; set; } = string.Empty;
    public int ReadingMinutes { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string WhyImportant { get; set; } = string.Empty;
    public string MeaningForKids { get; set; } = string.Empty;
    public string ExplainedTermsJson { get; set; } = "[]";
    public string? BerlinDistrict { get; set; }
    public string ComprehensionQuestionsJson { get; set; } = "[]";
}
