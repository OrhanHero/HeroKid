using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>Ein Abschnitt einer Kurslektion: kleine Überschrift, wenige Sätze, evtl. ein Zeichen.</summary>
/// <param name="Heading">Die Überschrift des Abschnitts.</param>
/// <param name="Body">Fließtext, bewusst kurz - drei bis fünf Sätze.</param>
/// <param name="SignNumber">Verkehrszeichen zum Abschnitt; die Ansicht zeigt es daneben. Null,
/// wenn der Abschnitt ohne Bild auskommt.</param>
public sealed record CourseSection(string Heading, string Body, string? SignNumber = null);

/// <summary>
/// Eine Lektion des Theorie-Kurses - eine je amtlichem Sachgebiet.
///
/// <para><b>Der Kurs bringt keine eigenen Fragen mit.</b> Die Lernstandskontrolle am Ende einer
/// Lektion zieht die Fragen des zugehörigen Sachgebiets aus <c>DrivingTheoryCatalog</c>. Ein
/// zweiter Fragensatz wäre doppelte Pflege und würde bei jeder Änderung auseinanderlaufen -
/// und die Kinder würden im Kurs etwas anderes üben als in der Prüfungssimulation.</para>
///
/// <para><b>Warum bebilderte Seiten und keine Videos:</b> LernTor ist vollständig offline. Die
/// Zeichenbilder liegen ohnehin schon im Programm, also werden die Erklärseiten damit bebildert.</para>
/// </summary>
public sealed record DrivingCourseLesson
{
    /// <summary>Stabile Kennung, z.B. "kurs-vorfahrt". Schlüssel des Lernstands - nicht ändern.</summary>
    public required string Id { get; init; }

    /// <summary>Das Sachgebiet - bestimmt zugleich, welche Fragen die Kontrolle stellt.</summary>
    public required DrivingTheoryTopic Topic { get; init; }

    public required string Title { get; init; }

    /// <summary>Ein, zwei Sätze: worum es geht und warum es zählt. Steht vor den Abschnitten.</summary>
    public required string Intro { get; init; }

    public required IReadOnlyList<CourseSection> Sections { get; init; }

    /// <summary>Die Merksätze am Ende - das, was hängenbleiben soll, wenn sonst nichts bleibt.</summary>
    public required IReadOnlyList<string> KeyPoints { get; init; }

    /// <summary>Zeichen, die in dieser Lektion vorkommen - für die Vorschau in der Übersicht.</summary>
    public IEnumerable<string> SignNumbers =>
        Sections.Where(abschnitt => abschnitt.SignNumber is not null)
                .Select(abschnitt => abschnitt.SignNumber!);
}
