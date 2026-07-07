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

    public static string NewId() => Guid.NewGuid().ToString("N");
}
