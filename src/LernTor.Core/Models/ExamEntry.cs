using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Ein Klausur-/Klassenarbeitstermin.
///
/// <para>Bewusst NICHT dieselbe Tabelle wie <see cref="HomeworkTask"/>: eine Hausaufgabe ist eine
/// Checkbox mit Stichtag, eine Klausur ist ein Countdown, den man nicht abhakt - und sie steuert
/// das Lernen in den Tagen davor. Zwei verschiedene Dinge, die nur zufällig beide ein Datum
/// haben.</para>
///
/// <para><b>Kinder dürfen selbst eintragen.</b> Damit daraus kein Schlupfloch wird ("Klausur
/// löschen, dann muss ich nicht üben"), gilt eine Asymmetrie: das Kind darf eigene Einträge
/// anlegen, ändern und löschen, Eltern-Einträge aber nur ansehen. Die Herkunft steht an jedem
/// Eintrag, damit im Eltern-Bereich sichtbar ist, was das Kind selbst gemeldet hat.</para>
/// </summary>
public sealed class ExamEntry
{
    public string Id { get; set; } = NewId();

    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Fach - entscheidet, welche Übungen vor dem Termin hochgewichtet werden.</summary>
    public Subject Subject { get; set; } = Subject.Mathematik;

    /// <summary>Kurzer Titel, z.B. "Klassenarbeit Nr. 2".</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Freitext, was drankommt ("Bruchrechnen, Prozentrechnung") - reine Anzeige.</summary>
    public string Topics { get; set; } = string.Empty;

    public DateOnly ExamDate { get; set; }

    public EntryAuthor Author { get; set; } = EntryAuthor.Eltern;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>Ab so vielen Tagen vor dem Termin wird das Fach in den Übungen hochgewichtet.</summary>
    public const int LearningBoostLeadDays = 7;

    /// <summary>Wie viele Tage nach dem Termin der Eintrag noch angezeigt wird.</summary>
    public const int VisibleAfterDays = 1;

    /// <summary>Tage bis zur Klausur; negativ = vorbei.</summary>
    public int DaysUntil(DateOnly today) => ExamDate.DayNumber - today.DayNumber;

    /// <summary>Steht noch bevor (heute zählt als bevorstehend - da wird sie geschrieben).</summary>
    public bool IsUpcoming(DateOnly today) => DaysUntil(today) >= 0;

    /// <summary>Ob der Eintrag noch angezeigt wird - kurz nach dem Termin verschwindet er.</summary>
    public bool IsVisibleTo(DateOnly today) => DaysUntil(today) >= -VisibleAfterDays;

    /// <summary>
    /// Faktor, mit dem die Aufgabenzahl dieses Fachs multipliziert wird.
    ///
    /// <para>Steigt zum Termin hin an: eine Woche vorher noch sanft, am Vortag am stärksten. Ein
    /// konstanter Aufschlag über die ganze Woche würde entweder zu früh nerven oder zu spät zu
    /// wenig bringen. Nach der Klausur sofort wieder 1.0 - das Fach hat seinen Anlass verloren.</para>
    /// </summary>
    public double LearningWeight(DateOnly today)
    {
        var days = DaysUntil(today);
        if (days < 0 || days > LearningBoostLeadDays)
        {
            return 1.0;
        }

        // 7 Tage vorher -> 1.25, am Vortag und am Tag selbst -> 2.0.
        var closeness = (LearningBoostLeadDays - days) / (double)LearningBoostLeadDays;
        return 1.25 + closeness * 0.75;
    }

    /// <summary>
    /// Countdown in der Sprache, in der Familien darüber reden. Ein Datum allein muss man erst
    /// umrechnen - und genau das Umrechnen ist der Grund, warum Klausuren überrascht werden.
    /// </summary>
    public string CountdownLabel(DateOnly today) => DaysUntil(today) switch
    {
        < -1 => "vorbei",
        -1 => "war gestern",
        0 => "HEUTE",
        1 => "morgen",
        2 => "übermorgen",
        var days => $"in {days} Tagen"
    };

    /// <summary>Ob das Kind diesen Eintrag ändern oder löschen darf.</summary>
    public bool IsEditableByChild => Author == EntryAuthor.Kind;

    public static string NewId() => Guid.NewGuid().ToString("N");
}
