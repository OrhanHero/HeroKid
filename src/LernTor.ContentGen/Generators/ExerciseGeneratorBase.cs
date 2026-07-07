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

    public IReadOnlyList<QuizQuestion> Generate(GradeLevel grade, int count, Random random)
    {
        if (!TopicsByGrade.TryGetValue(grade, out var topics) || topics.Count == 0)
        {
            return Array.Empty<QuizQuestion>();
        }

        var questions = new List<QuizQuestion>(count);
        for (var i = 0; i < count; i++)
        {
            var topic = topics[random.Next(topics.Count)];
            questions.Add(topic(random));
        }

        return questions;
    }

    protected static string NewId() => Guid.NewGuid().ToString("N");
}
