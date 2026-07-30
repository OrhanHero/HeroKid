using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Ein Vokabelpaar, das die Eltern für ein Kind hinterlegt haben - deutsch plus Fremdsprache.
///
/// <para>Vokabeln haben ihre eigene Wiederholungs-Steuerung statt der von
/// <c>MasteredPromptRepository</c>: Dort wird ein <em>Fragetext</em> gemeistert, hier ein
/// <em>Wortpaar</em>, das in zwei Abfragerichtungen vorkommt. Die Intervalle sind dieselben
/// (<see cref="Services.SpacedRepetitionSchedule"/>), damit sich das Lernen für das Kind gleich
/// anfühlt.</para>
/// </summary>
public sealed class VocabularyEntry
{
    public required string Id { get; init; }

    /// <summary>Fach, in dem die Vokabel abgefragt wird - nur <see cref="Subject.Englisch"/> oder <see cref="Subject.Tuerkisch"/>.</summary>
    public required Subject Subject { get; init; }

    /// <summary>Das deutsche Wort.</summary>
    public required string German { get; init; }

    /// <summary>Das fremdsprachige Wort.</summary>
    public required string Foreign { get; init; }

    /// <summary>Meisterungs-Stufe (0 = noch nie richtig beantwortet).</summary>
    public int Stage { get; init; }

    /// <summary>Wann die Vokabel wieder abgefragt werden soll. <c>null</c> = sofort fällig.</summary>
    public DateTimeOffset? NextDueAt { get; init; }

    /// <summary>Wie oft sie schon falsch beantwortet wurde - steuert die Reihenfolge (schwerste zuerst).</summary>
    public int WrongCount { get; init; }
}
