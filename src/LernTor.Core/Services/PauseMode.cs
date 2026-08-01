namespace LernTor.Core.Services;

/// <summary>
/// Ferien-/Pausenmodus: bis einschließlich des im Eltern-Bereich gesetzten Datums läuft LernTor
/// ohne Zwang. Danach schaltet sich alles von selbst wieder ein - Eltern müssen an nichts denken.
///
/// <para><b>Warum das hier in Core liegt und nicht als Einzeiler in App:</b> genau als Einzeiler
/// stand es vorher an einer einzigen Stelle (dem Kiosk-Lock beim Start) und wurde an der zweiten,
/// mindestens ebenso wichtigen Stelle - der Lernstrecke selbst - schlicht vergessen. Das Ergebnis
/// war ein Ferienmodus, der im Eltern-Bereich grün leuchtete, während das Kind trotzdem bis zum
/// Abschlussquiz durchmusste. Als benannte Regel mit Tests kann dieselbe Frage jetzt überall
/// gleich beantwortet werden.</para>
///
/// <para><b>Systemuhr:</b> die Prüfung glaubt der lokalen PC-Uhr. Sie vorzustellen beendet die
/// Pause früher (nicht später), zurückzustellen könnte eine abgelaufene Pause wiederbeleben. Für
/// den Zweck - Ferien - ist das hinnehmbar; ein Kind, das die Systemuhr verstellen kann, hat den
/// Rechner ohnehin schon in der Hand.</para>
/// </summary>
public static class PauseMode
{
    /// <summary>
    /// Ob die Pause an <paramref name="today"/> gilt. Der letzte Tag zählt <b>einschließlich</b>
    /// dazu: wer den 23.08. einträgt, meint "bis zum 23. sind Ferien", nicht "bis zum 22.".
    /// </summary>
    public static bool IsActive(DateOnly? pauseUntil, DateOnly today) =>
        pauseUntil is { } until && today <= until;

    /// <summary>
    /// Verbleibende Pausentage einschließlich heute; 0 wenn keine Pause läuft. Am letzten Tag
    /// steht also 1 - "heute ist der letzte Tag" statt "noch 0 Tage".
    /// </summary>
    public static int RemainingDays(DateOnly? pauseUntil, DateOnly today) =>
        IsActive(pauseUntil, today) ? pauseUntil!.Value.DayNumber - today.DayNumber + 1 : 0;
}
