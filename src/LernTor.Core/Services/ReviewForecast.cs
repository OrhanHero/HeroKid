using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Wie viele Fragen aus der Fehler-Kartei HEUTE wirklich drankommen - die Zahl auf der
/// Startseite („🔁 Heute kommen 3 Fragen aus deiner Fehler-Kartei wieder“).
///
/// <para><b>Warum nicht einfach alle fälligen Einträge zählen:</b> das tat die Startseite bis
/// 29.09.2026. Seit die Fächer des Tages nach dem Stundenplan ausgewählt werden, kommen
/// Wiederholungsfragen aber nur in den Fächern dran, die heute geübt werden - und je Fach höchstens
/// <see cref="PerSubjectCap"/>. Ein Kind mit zwölf offenen Fehlern in fünf Fächern, von denen heute
/// zwei dran sind, las „12“ und sah dann vier. Eine Zahl, die nicht stimmt, lernt man zu
/// ignorieren; dann hilft sie auch an den Tagen nicht, an denen sie stimmt.</para>
///
/// <para>Rein und ohne Datenbank, damit die Regel ohne WPF testbar ist. Dieselbe Obergrenze
/// benutzt <c>MainViewModel.BuildExerciseViewModelAsync</c> beim Zusammenstellen der Aufgaben -
/// beide lesen sie von hier, damit Anzeige und Ablauf nicht auseinanderlaufen.</para>
/// </summary>
public static class ReviewForecast
{
    /// <summary>Höchstens so viele Fehler-Kartei-Fragen je Fach und Tag.</summary>
    public const int PerSubjectCap = 3;

    /// <summary>Anzahl der heute zu erwartenden Wiederholungsfragen über alle Fächer.</summary>
    /// <param name="dueBySubject">Fällige Einträge je Fach (siehe
    /// <c>ReviewQuestionRepository.GetDueCountsBySubjectAsync</c>).</param>
    /// <param name="isPracticedToday">Wird das Fach heute geübt? In der App ist das
    /// <c>!IsSubjectDisabled(fach)</c> - dieselbe Regel, nach der auch die Etappen übersprungen
    /// werden.</param>
    public static int CountForToday(
        IReadOnlyDictionary<Subject, int> dueBySubject,
        Func<Subject, bool> isPracticedToday)
    {
        ArgumentNullException.ThrowIfNull(dueBySubject);
        ArgumentNullException.ThrowIfNull(isPracticedToday);

        return dueBySubject
            .Where(eintrag => eintrag.Value > 0 && isPracticedToday(eintrag.Key))
            .Sum(eintrag => Math.Min(eintrag.Value, PerSubjectCap));
    }
}
