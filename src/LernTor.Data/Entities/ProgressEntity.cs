namespace LernTor.Data.Entities;

public sealed class ProgressEntity
{
    public int Id { get; set; }
    public DateOnly SessionDate { get; set; }
    public string CurrentStage { get; set; } = string.Empty;

    /// <summary>JSON-serialisierte Liste abgeschlossener News-Artikel-IDs.</summary>
    public string CompletedNewsArticleIdsJson { get; set; } = "[]";

    /// <summary>JSON-serialisierte Liste abgeschlossener Fachbereiche.</summary>
    public string CompletedSubjectsJson { get; set; } = "[]";

    public int FinalQuizAttempts { get; set; }
    public double? LastQuizScore { get; set; }
    public bool IsUnlocked { get; set; }

    /// <summary>JSON-serialisierte Liste der zu wiederholenden Fachbereiche.</summary>
    public string SubjectsToRetryJson { get; set; } = "[]";

    public DateTimeOffset LastUpdatedAt { get; set; }
}
