using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Welche Einträge des <see cref="Subject"/>-Enums wirklich Schulfächer sind - also solche, in
/// denen es eine Klausur und eine Hausaufgabe geben kann.
///
/// <para>Nicht jeder Bereich der App ist ein Schulfach. Der Tipptrainer, der KI-Bereich, der
/// Führerschein und Erste Hilfe sind LernTor-eigene Übungsbereiche; für sie stellt die Schule
/// weder Hausaufgaben noch schreibt jemand darin eine Klausur. In der Auswahlliste des
/// Klausur-Dialogs standen sie trotzdem - "Klausur in Führerschein" ist kein Termin, den es
/// gibt, und eine Liste voller Möglichkeiten, die keine sind, macht das Eintragen langsamer,
/// nicht schneller.</para>
///
/// <para><b>Definiert über die Ausnahmen, nicht über eine Aufzählung.</b> Ein neues Schulfach
/// ist damit automatisch dabei - nur ein neuer LernTor-EIGENER Bereich braucht hier einen
/// Eintrag. Andersherum wäre es eine weitere Liste, die man beim Anlegen eines Fachs zu
/// aktualisieren vergisst.</para>
/// </summary>
public static class SchoolSubjects
{
    /// <summary>
    /// Bereiche, die kein Schulfach sind. <see cref="Subject.News"/> ist die Nachrichtenrunde,
    /// <see cref="Subject.Tippen"/> der Tipptrainer, <see cref="Subject.KiWissen"/>,
    /// <see cref="Subject.Fuehrerschein"/> und <see cref="Subject.ErsteHilfe"/> sind eigene
    /// Übungsbereiche ohne Rahmenlehrplan-Bezug.
    /// </summary>
    public static IReadOnlySet<Subject> NonSchool { get; } = new HashSet<Subject>
    {
        Subject.News,
        Subject.Tippen,
        Subject.KiWissen,
        Subject.Fuehrerschein,
        Subject.ErsteHilfe
    };

    /// <summary>Alle echten Schulfächer, in der Reihenfolge des Enums.</summary>
    public static IReadOnlyList<Subject> All { get; } =
        Enum.GetValues<Subject>().Where(fach => !NonSchool.Contains(fach)).ToList();

    public static bool IsSchoolSubject(Subject subject) => !NonSchool.Contains(subject);
}
