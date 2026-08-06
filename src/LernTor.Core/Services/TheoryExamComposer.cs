using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Wie gut ein Sachgebiet sitzt - Grundlage des Schwachstellen-Trainers.</summary>
/// <param name="Topic">Das Sachgebiet.</param>
/// <param name="Answered">Beantwortete Fragen daraus.</param>
/// <param name="Correct">Davon richtig.</param>
public readonly record struct TopicMastery(DrivingTheoryTopic Topic, int Answered, int Correct)
{
    public int Percent => Answered == 0 ? 0 : (int)Math.Round(Correct * 100.0 / Answered);

    /// <summary>Genug Antworten, um überhaupt etwas sagen zu können.</summary>
    public bool IsMeaningful => Answered >= TheoryExamComposer.MinAnswersForMastery;

    /// <summary>Ein Sachgebiet gilt als Schwachstelle, wenn es belastbar gemessen und schwach ist.</summary>
    public bool IsWeak => IsMeaningful && Percent < TheoryExamComposer.WeakBelowPercent;
}

/// <summary>
/// Stellt Prüfungssimulationen und Übungssätze aus dem Fragenkatalog zusammen.
///
/// <para><b>Die Simulation streut über alle Sachgebiete</b>, statt zufällig aus dem Topf zu
/// ziehen. Sonst kämen an einem Tag zwölf Vorfahrt-Fragen und am nächsten keine - die echte
/// Prüfung mischt, und wer nur einen Ausschnitt übt, hält sich für weiter, als er ist.</para>
///
/// <para><b>Der Schwachstellen-Trainer</b> zieht bevorzugt aus Sachgebieten, in denen die
/// Trefferquote niedrig ist. Er braucht aber Belege: unter <see cref="MinAnswersForMastery"/>
/// Antworten wird ein Sachgebiet nicht bewertet, denn zwei falsche Antworten machen es noch
/// nicht zur Schwachstelle.</para>
/// </summary>
public static class TheoryExamComposer
{
    /// <summary>So viele Antworten braucht ein Sachgebiet, bevor es als schwach gelten kann.</summary>
    public const int MinAnswersForMastery = 4;

    /// <summary>Unter dieser Trefferquote gilt ein Sachgebiet als Schwachstelle.</summary>
    public const int WeakBelowPercent = 70;

    /// <summary>
    /// Eine Prüfungssimulation: <see cref="TheoryExamRules.QuestionCount"/> Fragen, über die
    /// Sachgebiete gestreut. Reicht der Katalog nicht, kommen eben weniger - lieber eine kurze
    /// ehrliche Prüfung als eine mit Wiederholungen.
    /// </summary>
    public static IReadOnlyList<TheoryQuestion> ComposeExam(
        IReadOnlyList<TheoryQuestion> pool, Random random)
    {
        if (pool.Count == 0)
        {
            return Array.Empty<TheoryQuestion>();
        }

        // Reihum durch die Sachgebiete, aus jedem der Reihe nach eine Frage. Das verteilt
        // gleichmäßig, auch wenn die Sachgebiete unterschiedlich viele Fragen haben.
        var nachThema = pool
            .GroupBy(frage => frage.Topic)
            .OrderBy(gruppe => (int)gruppe.Key)
            .Select(gruppe => Shuffle(gruppe.ToList(), random))
            .ToList();

        var gewaehlt = new List<TheoryQuestion>();
        var index = 0;

        while (gewaehlt.Count < TheoryExamRules.QuestionCount)
        {
            var etwasGenommen = false;

            foreach (var thema in nachThema)
            {
                if (index >= thema.Count)
                {
                    continue;
                }

                gewaehlt.Add(thema[index]);
                etwasGenommen = true;

                if (gewaehlt.Count == TheoryExamRules.QuestionCount)
                {
                    break;
                }
            }

            if (!etwasGenommen)
            {
                break;      // Katalog erschöpft
            }

            index++;
        }

        return Shuffle(gewaehlt, random);
    }

    /// <summary>
    /// Übungssatz für den Schwachstellen-Trainer: bevorzugt aus schwachen Sachgebieten, aufgefüllt
    /// aus dem Rest. Gibt es noch keine Schwachstellen (weil zu wenig geübt wurde), kommt ein
    /// gemischter Satz - besser als eine leere Anzeige.
    /// </summary>
    public static IReadOnlyList<TheoryQuestion> ComposeWeakSpotSet(
        IReadOnlyList<TheoryQuestion> pool,
        IReadOnlyList<TopicMastery> mastery,
        int count,
        Random random)
    {
        if (pool.Count == 0 || count <= 0)
        {
            return Array.Empty<TheoryQuestion>();
        }

        var schwach = mastery.Where(m => m.IsWeak).Select(m => m.Topic).ToHashSet();

        var ausSchwach = Shuffle(pool.Where(f => schwach.Contains(f.Topic)).ToList(), random);
        var rest = Shuffle(pool.Where(f => !schwach.Contains(f.Topic)).ToList(), random);

        return ausSchwach.Concat(rest).Take(count).ToList();
    }

    /// <summary>
    /// Trefferquote je Sachgebiet, schwächstes zuerst. Sachgebiete ohne Antworten fallen weg -
    /// "0 %" bei null Versuchen wäre eine Aussage über nichts.
    /// </summary>
    public static IReadOnlyList<TopicMastery> BuildMastery(
        IEnumerable<(DrivingTheoryTopic Topic, bool WasCorrect)> answers)
    {
        return answers
            .GroupBy(a => a.Topic)
            .Select(g => new TopicMastery(g.Key, g.Count(), g.Count(a => a.WasCorrect)))
            .OrderBy(m => m.Percent)
            .ThenBy(m => m.Topic.ToString(), StringComparer.Ordinal)
            .ToList();
    }

    private static List<T> Shuffle<T>(List<T> items, Random random)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }

        return items;
    }
}
