using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Welche Bereiche für ein bestimmtes Kind an diesem Tag ausfallen.
///
/// <para><b>Zwei Schalter, beide zählen:</b> der globale Fächer-Schalter im Eltern-Bereich
/// (<c>AppSettings.DisabledSubjects</c>, gilt für alle Kinder) und die profilbezogenen Schalter
/// für Führerschein und Erste Hilfe. Ein Elfjähriger und ein Fünfzehnjähriger sind dort
/// unterschiedlich weit; global ließe sich das nicht abbilden.</para>
///
/// <para><b>Warum das hier steht und nicht im ViewModel:</b> es gab davon zwei Fassungen. Die
/// Etappen-Steuerung kannte beide Schalter, das Abschlussquiz und der Etappen-Zähler nur den
/// globalen. Wer den Führerschein-Bereich für ein Kind abschaltete, bekam trotzdem
/// Führerschein-Fragen im Abschlussquiz - in einem Bereich, den dieses Kind an dem Tag nie
/// gesehen hatte -, und die falschen Antworten drückten es unter die Bestehensschwelle. Eine
/// Regel, die an zwei Stellen steht, ist eine Regel, die auseinanderläuft; in Core ist sie
/// außerdem ohne WPF prüfbar.</para>
/// </summary>
public static class SubjectAvailability
{
    /// <summary>Fällt dieser Bereich für dieses Kind aus?</summary>
    /// <param name="subject">Der Bereich.</param>
    /// <param name="globallyDisabled">Global abgeschaltete Bereiche (alle Kinder).</param>
    /// <param name="drivingAreaEnabled">Profilschalter für den Führerschein-Bereich.</param>
    /// <param name="ersteHilfeEnabled">Profilschalter für Erste Hilfe.</param>
    public static bool IsDisabled(
        Subject subject,
        IReadOnlySet<Subject>? globallyDisabled,
        bool drivingAreaEnabled,
        bool ersteHilfeEnabled)
    {
        if (globallyDisabled is not null && globallyDisabled.Contains(subject))
        {
            return true;
        }

        return subject switch
        {
            Subject.Fuehrerschein => !drivingAreaEnabled,
            Subject.ErsteHilfe => !ersteHilfeEnabled,
            _ => false
        };
    }

    /// <summary>
    /// Dieselbe Auskunft als Menge - für alles, was nicht Bereich für Bereich fragt
    /// (Abschlussquiz, eigene Aufgaben der Eltern).
    ///
    /// <para>Bewusst AUS <see cref="IsDisabled"/> abgeleitet und nicht danebengepflegt: so können
    /// die beiden Formen nicht auseinanderlaufen. Genau das war der Fehler.</para>
    /// </summary>
    public static HashSet<Subject> EffectiveDisabled(
        IReadOnlySet<Subject>? globallyDisabled,
        bool drivingAreaEnabled,
        bool ersteHilfeEnabled) =>
        Enum.GetValues<Subject>()
            .Where(fach => IsDisabled(fach, globallyDisabled, drivingAreaEnabled, ersteHilfeEnabled))
            .ToHashSet();
}
