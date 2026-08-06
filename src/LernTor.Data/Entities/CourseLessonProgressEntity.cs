using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LernTor.Core.Models;

/// <summary>
/// Lernstand einer Kurslektion pro Profil.
///
/// <para><b>Gelesen und geschafft stehen getrennt</b>, weil es zwei verschiedene Dinge sind. Nur
/// "geschafft" zu speichern hieße, dass eine gelesene Lektion nach einer verpatzten Kontrolle
/// wieder aussieht wie nie geöffnet - und das Kind fängt entnervt von vorn an.</para>
///
/// <para><see cref="BestPercent"/> hält den besten Versuch fest, nicht den letzten: wer eine
/// Kontrolle aus Neugier noch einmal startet und abbricht, soll sich sein Ergebnis nicht
/// verderben können.</para>
/// </summary>
[Table("CourseLessonProgress")]
public sealed class CourseLessonProgressEntity
{
    /// <summary>Zusammengesetzt aus ProfileId und LessonId.</summary>
    [Key]
    public required string Id { get; init; }

    [Required]
    [MaxLength(64)]
    public required string ProfileId { get; init; }

    /// <summary>Die Kennung aus dem Kurskatalog, z.B. "kurs-vorfahrt".</summary>
    [Required]
    [MaxLength(64)]
    public required string LessonId { get; init; }

    /// <summary>Wann die Lektion zuletzt gelesen wurde. Null = noch nie geöffnet.</summary>
    public DateTimeOffset? ReadAt { get; set; }

    /// <summary>Wann die Lernstandskontrolle zuerst bestanden wurde. Null = noch offen.</summary>
    public DateTimeOffset? PassedAt { get; set; }

    /// <summary>Bester Kontroll-Versuch in Prozent.</summary>
    public int BestPercent { get; set; }

    public int Attempts { get; set; }
}
