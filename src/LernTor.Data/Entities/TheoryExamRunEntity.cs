using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LernTor.Core.Models;

/// <summary>
/// Ein Durchlauf der Prüfungssimulation.
///
/// <para><b>Bestanden/durchgefallen wird nicht gespeichert</b>, sondern aus den Zahlen neu
/// berechnet (<see cref="TheoryExamRules.HasPassed"/>). Ein gespeichertes Häkchen würde alte
/// Läufe nach einer Regeländerung anders bewerten als die Zahlen daneben - und dann stünde in
/// der Historie "bestanden" neben zwölf Fehlerpunkten.</para>
/// </summary>
[Table("TheoryExamRuns")]
public sealed class TheoryExamRunEntity
{
    [Key]
    public required string Id { get; init; }

    [Required]
    [MaxLength(64)]
    public required string ProfileId { get; init; }

    public DateTimeOffset TakenAt { get; set; } = DateTimeOffset.Now;

    /// <summary>Gestellte Fragen - kann unter 30 liegen, wenn der Katalog nicht reicht.</summary>
    public int QuestionCount { get; set; }

    public int WrongPoints { get; set; }

    public int WrongCount { get; set; }

    /// <summary>Falsch beantwortete 5-Punkte-Fragen; zwei davon bedeuten für sich durchgefallen.</summary>
    public int HeavyMistakes { get; set; }

    /// <summary>Das Ergebnis in der Form, in der die Regeln damit rechnen.</summary>
    public TheoryExamResult ToResult() =>
        new(QuestionCount, WrongPoints, WrongCount, HeavyMistakes);
}
