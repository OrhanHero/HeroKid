using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Der Theorie-Kurs: eine Lektion je amtlichem Sachgebiet, in Kursreihenfolge.
///
/// <para><b>Der Kurs bringt keine eigenen Fragen mit.</b> Die Lernstandskontrolle am Ende einer
/// Lektion nimmt Fragen aus <see cref="DrivingTheoryCatalog"/> zum selben Sachgebiet. Ein
/// zweiter Fragensatz wäre doppelte Pflege - und die Kinder würden im Kurs etwas anderes üben
/// als in der Prüfungssimulation.</para>
///
/// <para><b>Alle Texte sind selbst geschrieben</b> und geben StVO und StVZO in eigenen Worten
/// wieder. Der amtliche Fragenkatalog gehört der TÜV|DEKRA arge tp 21 und darf hier nicht
/// hinein - für die Erklärseiten gilt dasselbe wie für die Fragen.</para>
/// </summary>
public static partial class DrivingCourseCatalog
{
    private static readonly Lazy<DrivingCourseLesson[]> AllLessons = new(() =>
        // Wie bei den Zeichen- und Fragenkatalogen: die Teil-Arrays stehen in anderen Dateien
        // derselben partiellen Klasse, und deren Initialisierungsreihenfolge ist nicht
        // festgelegt. Lazy verschiebt das Zusammenbauen auf den ersten Zugriff.
        Grundlagen!.Concat(Praxis!).ToArray());

    public static IReadOnlyList<DrivingCourseLesson> All => AllLessons.Value;

    public static DrivingCourseLesson? ById(string id) =>
        AllLessons.Value.FirstOrDefault(lektion => lektion.Id == id);

    public static DrivingCourseLesson? ForTopic(DrivingTheoryTopic topic) =>
        AllLessons.Value.FirstOrDefault(lektion => lektion.Topic == topic);

    /// <summary>
    /// Die Fragen der Lernstandskontrolle: aus dem Sachgebiet der Lektion, höchstens
    /// <see cref="DrivingCourseRules.MaxCheckQuestions"/> Stück.
    ///
    /// <para>Gemischt wird mit dem übergebenen Zufallsgenerator, damit die Kontrolle beim zweiten
    /// Anlauf nicht Wort für Wort dieselbe ist - sonst prüft sie beim Wiederholen nur noch, ob
    /// man sich die Reihenfolge gemerkt hat.</para>
    /// </summary>
    public static IReadOnlyList<TheoryQuestion> CheckQuestions(DrivingCourseLesson lesson, Random random)
    {
        var fragen = DrivingTheoryCatalog.ByTopic(lesson.Topic).ToList();

        for (var i = fragen.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (fragen[i], fragen[j]) = (fragen[j], fragen[i]);
        }

        return fragen.Take(DrivingCourseRules.MaxCheckQuestions).ToList();
    }
}
