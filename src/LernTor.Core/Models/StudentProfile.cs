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

    public const string DefaultAvatar = "🧒";

    public static string NewId() => Guid.NewGuid().ToString("N");
}
