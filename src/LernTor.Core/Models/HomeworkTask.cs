using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Eine von den Eltern eingetragene Hausaufgabe mit Fälligkeitsdatum.
///
/// <para>Bewusst etwas anderes als <c>CustomQuestion</c>: eine eigene Aufgabe dort ist eine
/// Quizfrage, die das Kind in der Übungsrunde beantwortet. Eine Hausaufgabe hier ist eine
/// Erinnerung an etwas, das AUSSERHALB der App zu erledigen ist ("Mathe Seite 42, Nr. 1-5 bis
/// Donnerstag") - sie wird nicht abgefragt, sondern abgehakt.</para>
/// </summary>
public sealed class HomeworkTask
{
    public string Id { get; set; } = NewId();

    /// <summary>Zu welchem Kind die Aufgabe gehört.</summary>
    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Fach - für den Chip in der Anzeige. Kein Pflichtfeld im Sinne von "muss stimmen".</summary>
    public Subject Subject { get; set; } = Subject.Deutsch;

    /// <summary>Was zu tun ist, in den Worten der Eltern.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Bis wann. Datum ohne Uhrzeit - "bis Donnerstag" ist die Einheit, in der Schule denkt.</summary>
    public DateOnly DueDate { get; set; }

    /// <summary>Wann das Kind sie abgehakt hat; <c>null</c> = noch offen.</summary>
    public DateTimeOffset? CompletedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public bool IsCompleted => CompletedAt is not null;

    /// <summary>Überfällig = Stichtag liegt vor heute und die Aufgabe ist noch offen.</summary>
    public bool IsOverdue(DateOnly today) => !IsCompleted && DueDate < today;

    /// <summary>Heute fällig - der Fall, der auf dem Willkommensbildschirm auffallen muss.</summary>
    public bool IsDueToday(DateOnly today) => !IsCompleted && DueDate == today;

    /// <summary>
    /// Wie dringend, als Sortierschlüssel: überfällig zuerst, dann heute, dann nach Datum.
    /// Erledigte Aufgaben landen immer hinten.
    /// </summary>
    public int UrgencyRank(DateOnly today)
    {
        if (IsCompleted) return 3;
        if (DueDate < today) return 0;
        if (DueDate == today) return 1;
        return 2;
    }

    /// <summary>
    /// Wie lange eine erledigte Aufgabe noch angezeigt wird, bevor sie aus der Kind-Ansicht
    /// verschwindet. Ein sofortiges Verschwinden nimmt dem Abhaken das Erfolgserlebnis.
    /// </summary>
    public const int CompletedVisibleDays = 2;

    /// <summary>
    /// Ob die Aufgabe dem Kind noch gezeigt wird. Offene Aufgaben immer; erledigte noch kurz;
    /// längst abgelaufene offene Aufgaben ebenfalls nicht mehr - eine Hausaufgabe von vor drei
    /// Wochen ist keine Erinnerung mehr, sondern nur noch ein Vorwurf.
    /// </summary>
    public bool IsVisibleTo(DateOnly today)
    {
        if (IsCompleted)
        {
            var completedOn = DateOnly.FromDateTime(CompletedAt!.Value.LocalDateTime);
            return completedOn >= today.AddDays(-CompletedVisibleDays);
        }

        return DueDate >= today.AddDays(-StaleAfterDays);
    }

    /// <summary>Nach so vielen Tagen Überfälligkeit verschwindet eine offene Aufgabe aus der Kind-Ansicht
    /// (im Eltern-Bereich bleibt sie sichtbar, bis sie dort gelöscht wird).</summary>
    public const int StaleAfterDays = 14;

    public static string NewId() => Guid.NewGuid().ToString("N");
}
