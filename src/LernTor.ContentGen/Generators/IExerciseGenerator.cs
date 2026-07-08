using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

public interface IExerciseGenerator
{
    Subject Subject { get; }

    /// <summary>
    /// Erzeugt `count` zufällig parametrisierte Aufgaben für die angegebene Klassenstufe.
    /// <paramref name="recentlySeenPrompts"/> (optional) enthält Fragetexte, die kürzlich schon
    /// gestellt wurden - diese werden bevorzugt vermieden, solange der Themen-Pool das hergibt.
    /// </summary>
    IReadOnlyList<QuizQuestion> Generate(GradeLevel grade, int count, Random random, IReadOnlySet<string>? recentlySeenPrompts = null);
}
