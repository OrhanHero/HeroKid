using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Fortschritt einer Tipp-Lektion pro Profil.
/// Wird in SQLite gespeichert (EF Core).
/// </summary>
[Table("TypingLessonProgress")]
public sealed class TypingLessonProgressEntity
{
    [Key]
    public required string Id { get; init; } // Composite: ProfileId + LessonId

    [Required]
    [MaxLength(64)]
    public required string ProfileId { get; init; }

    [Required]
    [MaxLength(64)]
    public required string LessonId { get; init; }

    /// <summary>Zur welcher Lektionstyp gehört diese Übung</summary>
    public required int LessonType { get; init; } // TypingLessonType as int

    /// <summary>Best-Attempt Genauigkeit (0.0 - 1.0)</summary>
    public double BestAccuracy { get; set; } = 0.0;

    /// <summary>Best-Attempt WPM (Wörter pro Minute)</summary>
    public double BestWpm { get; set; } = 0.0;

    /// <summary>Gesamt getippte Zeichen in allen Versuchen</summary>
    public int TotalCharactersTyped { get; set; } = 0;

    /// <summary>Korrekt getippte Zeichen (Best Attempt)</summary>
    public int CorrectCharacters { get; set; } = 0;

    /// <summary>Anzahl Versuche für diese Lektion</summary>
    public int AttemptCount { get; set; } = 0;

    /// <summary>Ob Lektion als "bestanden" gilt (≥ MinimumAccuracy + MinimumCharacters)</summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>Zeitstempel des letzten Versuchs</summary>
    public DateTimeOffset LastAttemptAt { get; set; } = DateTimeOffset.Now;

    /// <summary>Zeitstempel der ersten Freischaltung (Bestand)</summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>Sterne die für diese Lektion verdient wurden (Gamification)</summary>
    public int StarsEarned { get; set; } = 0;

    /// <summary>Letzte WPM für UI-Anzeige</summary>
    public double LastWpm { get; set; } = 0.0;

    /// <summary>Letzte Genauigkeit für UI-Anzeige</summary>
    public double LastAccuracy { get; set; } = 0.0;
}