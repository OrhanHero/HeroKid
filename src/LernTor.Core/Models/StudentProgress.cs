using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Persistenter Fortschrittszustand eines Lerntages. Wird nach jedem Schritt gespeichert,
/// damit ein Absturz oder Neustart nicht zu Datenverlust führt.
/// </summary>
public sealed class StudentProgress
{
    public required string ProfileId { get; set; }
    public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public LearningStage CurrentStage { get; set; } = LearningStage.Willkommen;

    /// <summary>
    /// Ob der Pflicht-Leseabschnitt (mind. 5 Minuten, laut vorlesen) heute schon absolviert wurde.
    /// </summary>
    public bool HasCompletedReading { get; set; }

    /// <summary>
    /// Ob der Pflicht-Tippabschnitt (10-Finger-System Lektionen) heute schon absolviert wurde.
    /// </summary>
    public bool HasCompletedTyping { get; set; }

    /// <summary>
    /// Ob der Pflicht-Schreibabschnitt (mind. 5 Minuten, freies Schreiben) heute schon absolviert wurde.
    /// </summary>
    public bool HasCompletedWriting { get; set; }

    public HashSet<string> CompletedNewsArticleIds { get; set; } = new();
    public HashSet<Subject> CompletedExerciseSubjects { get; set; } = new();

    public int FinalQuizAttempts { get; set; }
    public double? LastQuizScore { get; set; }
    public bool IsUnlocked { get; set; }

    /// <summary>
    /// Fachbereiche, die nach nicht bestandenem Quiz gezielt wiederholt werden müssen.
    /// </summary>
    public List<Subject> SubjectsToRetry { get; set; } = new();

    /// <summary>
    /// Heute verdiente Belohnungs-Sterne (Gamification): +2 Lesen, +2 News, +1 je Fach,
    /// +5 bestandenes Abschlussquiz. Zählt nur echte Abschlüsse, keine übersprungenen Fächer.
    /// </summary>
    public int EarnedStarsToday { get; set; }

    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.Now;
}