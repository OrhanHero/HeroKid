using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Ein Kind-Profil. Mehrere Kinder am selben PC können so getrennt ihren eigenen Fortschritt
/// und ihre eigene Klassenstufe haben, statt sich eine globale Einstellung zu teilen.
/// </summary>
public sealed class StudentProfile
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public int? Age { get; set; }

    /// <summary>Freitext, z.B. "9a" oder "6c" - rein informativ, für die Aufgaben zählt GradeLevel.</summary>
    public string? ClassLabel { get; set; }

    public required GradeLevel GradeLevel { get; set; }

    /// <summary>Vom Kind gewähltes Avatar-Emoji für die Profil-Kachel (bewusst Emoji statt Bilddateien:
    /// keine Assets zu pflegen, rendert auf jedem Windows nativ, kulturneutral wählbar).</summary>
    public string AvatarEmoji { get; set; } = DefaultAvatar;

    /// <summary>Über alle Lerntage gesammelte Belohnungs-Sterne (Gamification, siehe StudentProgress.EarnedStarsToday).</summary>
    public int TotalStars { get; set; }

    /// <summary>
    /// Mindest-Genauigkeit (0.0-1.0) zum Bestehen einer Tipp-Lektion für dieses Profil, von den
    /// Eltern im Eltern-Bereich als Preset (25/50/75/85%) einstellbar - siehe
    /// TypingExerciseService.CheckInput. Default 25% ist bewusst niedrig (Kinder tippen anfangs auf
    /// einer ihnen ungewohnten Tastatur).
    /// </summary>
    public double TypingMinAccuracy { get; set; } = 0.25;

    /// <summary>
    /// Mindest-Trefferquote (0.0-1.0) des ERSTEN Abschlussquiz-Versuchs am Tag, ab der der PC
    /// freigeschaltet wird, von den Eltern als Preset (50/75/85%) einstellbar - siehe
    /// QuizResult.PassThreshold, ProgressGateService.ApplyQuizResult.
    /// </summary>
    public double QuizFirstAttemptThreshold { get; set; } = 0.5;

    /// <summary>
    /// Mindest-Trefferquote (0.0-1.0) des ZWEITEN Abschlussquiz-Versuchs (Wiederholung nach
    /// Nichtbestehen des ersten Versuchs) - von den Eltern als Preset (25/50%) einstellbar. Anders
    /// als früher schaltet der zweite Versuch nicht mehr unabhängig vom Ergebnis frei.
    /// </summary>
    public double QuizRetryThreshold { get; set; } = 0.25;

    public const string DefaultAvatar = "🧒";

    public static string NewId() => Guid.NewGuid().ToString("N");
}
