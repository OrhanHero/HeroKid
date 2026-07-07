namespace LernTor.Core.Models;

public enum NewsRegionFocus
{
    Deutschland,
    Berlin,
    Tuerkei,
    Istanbul,
    Samsun,
    Uenye,
    International
}

public sealed record NewsArticle
{
    public required string Id { get; init; }
    public required string Title { get; init; }

    /// <summary>Kindgerecht vereinfachte Zusammenfassung (einfache Sprache, 10-15 Jahre).</summary>
    public required string SimplifiedSummary { get; init; }

    public string? ImageUrl { get; init; }
    public required string SourceName { get; init; }
    public required string SourceUrl { get; init; }
    public required DateTimeOffset PublishedAt { get; init; }
    public required NewsRegionFocus RegionFocus { get; init; }
    public required IReadOnlyList<QuizQuestion> ComprehensionQuestions { get; init; }
}
