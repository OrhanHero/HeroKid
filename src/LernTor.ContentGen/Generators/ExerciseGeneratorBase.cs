using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

public abstract class ExerciseGeneratorBase : IExerciseGenerator
{
    public abstract Subject Subject { get; }

    /// <summary>Ein Themen-Generator: nimmt Zufallszahlen entgegen und liefert eine fertige Frage.</summary>
    protected delegate QuizQuestion TopicFactory(Random random);

    /// <summary>Liste der pro Klassenstufe verfügbaren Themen-Generatoren.</summary>
    protected abstract IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; }

    public IReadOnlyList<QuizQuestion> Generate(GradeLevel grade, int count, Random random, IReadOnlySet<string>? recentlySeenPrompts = null)
    {
        if (!TopicsByGrade.TryGetValue(grade, out var topics) || topics.Count == 0)
        {
            return Array.Empty<QuizQuestion>();
        }

        var questions = new List<QuizQuestion>(count);

        // Vorbelegt mit kürzlich gestellten Fragen (aus dem Aktivitätsprotokoll, siehe
        // ActivityLogRepository.GetRecentPromptsAsync) - dadurch bevorzugt die Auswahl unten
        // frische Fragen, bevor überhaupt erst auf Wiederholungen zurückgegriffen wird.
        var seenPrompts = recentlySeenPrompts is null
            ? new HashSet<string>()
            : new HashSet<string>(recentlySeenPrompts);

        // Themen-Pools sind teils klein (z.B. nur 3-5 feste Beispiele je Thema), daher würde
        // reines Ziehen mit Zurücklegen schnell denselben Fragetext doppelt liefern. Bei einer
        // Kollision wird neu gezogen, bis genug einzigartige Fragen da sind oder der Pool
        // (Sicherheitsgrenze) erschöpft ist.
        var maxAttempts = count * 20;
        for (var attempt = 0; attempt < maxAttempts && questions.Count < count; attempt++)
        {
            var topic = topics[random.Next(topics.Count)];
            var question = topic(random);

            if (seenPrompts.Add(question.Prompt))
            {
                questions.Add(question);
            }
        }

        // Falls der Pool wirklich zu klein für `count` einzigartige Fragen ist, lieber mit
        // (dann zwangsläufig wiederholten) Fragen auffüllen als weniger als angefordert liefern.
        while (questions.Count < count)
        {
            var topic = topics[random.Next(topics.Count)];
            questions.Add(topic(random));
        }

        return questions;
    }

    protected static string NewId() => Guid.NewGuid().ToString("N");
}
