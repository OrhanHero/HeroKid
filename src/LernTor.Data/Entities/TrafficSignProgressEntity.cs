using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LernTor.Core.Models;

/// <summary>
/// Lernstand eines Verkehrszeichens pro Profil.
///
/// <para>Ein Zeichen gilt als gekonnt, wenn es <b>zweimal hintereinander</b> richtig erkannt
/// wurde - dieselbe Regel wie in der Fehler-Kartei (<c>ReviewQuestionRepository</c>), damit die
/// Kinder nicht zwei verschiedene Vorstellungen von "sitzt" lernen. Ein Fehler setzt die Serie
/// auf null: einmal richtig raten macht ein Zeichen nicht gekonnt.</para>
/// </summary>
[Table("TrafficSignProgress")]
public sealed class TrafficSignProgressEntity
{
    /// <summary>Zusammengesetzt aus ProfileId und SignNumber.</summary>
    [Key]
    public required string Id { get; init; }

    [Required]
    [MaxLength(64)]
    public required string ProfileId { get; init; }

    /// <summary>Amtliche Zeichennummer, z.B. "274-50".</summary>
    [Required]
    [MaxLength(32)]
    public required string SignNumber { get; init; }

    /// <summary>Richtige Antworten in Folge; ab <see cref="TrafficSignProgress.MasteredStreak"/> gilt das Zeichen als gekonnt.</summary>
    public int CorrectStreak { get; set; }

    public int CorrectTotal { get; set; }

    public int WrongTotal { get; set; }

    public DateTimeOffset LastSeenAt { get; set; } = DateTimeOffset.Now;
}
