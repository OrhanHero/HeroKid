using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Persistenter Fortschrittszustand eines Lerntages. Wird nach jedem Schritt gespeichert,
/// damit ein Absturz oder Neustart nicht zu Datenverlust führt.
/// </summary>
public sealed class StudentProgress
{
    public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public LearningStage CurrentStage { get; set; } = LearningStage.Willkommen;

    public HashSet<string> CompletedNewsArticleIds { get; set; } = new();
    public HashSet<Subject> CompletedExerciseSubjects { get; set; } = new();

    public int FinalQuizAttempts { get; set; }
    public double? LastQuizScore { get; set; }
    public bool IsUnlocked { get; set; }

    /// <summary>Fachbereiche, die nach nicht bestandenem Quiz gezielt wiederholt werden müssen.</summary>
    public List<Subject> SubjectsToRetry { get; set; } = new();

    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.Now;
}
