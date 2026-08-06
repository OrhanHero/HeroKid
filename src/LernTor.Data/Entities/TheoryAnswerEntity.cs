using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LernTor.Core.Models;

/// <summary>
/// Lernstand einer einzelnen Theoriefrage pro Profil.
///
/// <para><b>Das Sachgebiet steht bewusst nicht hier drin</b>, obwohl der Schwachstellen-Trainer
/// danach gruppiert: es steht schon im Fragenkatalog und wird von dort geholt. Zweimal gespeichert
/// hieße, dass die beiden Stellen auseinanderlaufen können, sobald eine Frage einmal in ein
/// anderes Sachgebiet umsortiert wird. Fragen, die es im Katalog nicht mehr gibt, fallen dadurch
/// von selbst aus der Auswertung - und das ist richtig so.</para>
/// </summary>
[Table("TheoryAnswers")]
public sealed class TheoryAnswerEntity
{
    /// <summary>Zusammengesetzt aus ProfileId und QuestionId.</summary>
    [Key]
    public required string Id { get; init; }

    [Required]
    [MaxLength(64)]
    public required string ProfileId { get; init; }

    /// <summary>Die Kennung aus dem Katalog, z.B. "vorfahrt-03".</summary>
    [Required]
    [MaxLength(64)]
    public required string QuestionId { get; init; }

    /// <summary>Richtige Antworten in Folge; ab <see cref="Services.TheoryProgress.MasteredStreak"/>
    /// gilt die Frage als gekonnt.</summary>
    public int CorrectStreak { get; set; }

    public int CorrectTotal { get; set; }

    public int WrongTotal { get; set; }

    public DateTimeOffset LastAnsweredAt { get; set; } = DateTimeOffset.Now;
}
