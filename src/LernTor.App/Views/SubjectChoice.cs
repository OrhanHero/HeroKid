using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.Views;

/// <summary>
/// Ein Eintrag in der Fach-Auswahlliste der Eingabefenster für Klausuren und Hausaufgaben.
///
/// <para>Ohne diesen Zwischenschritt zeigte die Liste den nackten Enum-Namen: dort stand
/// <c>Tuerkisch</c> statt „Türkisch“ und <c>Itg</c> statt „Medienbildung (ITG)“. Der
/// <c>SubjectToTitleConverter</c> übersetzt das längst - nur wurde er in diesen beiden Fenstern
/// nie benutzt, weil die ComboBox direkt an die Aufzählung gebunden war.</para>
/// </summary>
/// <param name="Subject">Das gewählte Fach.</param>
/// <param name="Title">Der angezeigte Name in der eingestellten Sprache.</param>
public sealed record SubjectChoice(Subject Subject, string Title)
{
    /// <summary>
    /// Die Fächer, für die es Klausuren und Hausaufgaben gibt - also die echten Schulfächer.
    /// Tipptrainer, KI-Bereich, Führerschein und Erste Hilfe stehen bewusst nicht darin: dafür
    /// gibt die Schule nichts auf, und „Klausur in Führerschein“ ist kein Termin, den es gibt.
    /// </summary>
    public static IReadOnlyList<SubjectChoice> SchoolSubjectList() =>
        SchoolSubjects.All
            .Select(fach => new SubjectChoice(fach, Localization.LocalizationService.Instance[$"Stage_{fach}"]))
            .OrderBy(eintrag => eintrag.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
}
