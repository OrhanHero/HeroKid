using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

public interface IExerciseGenerator
{
    Subject Subject { get; }

    /// <summary>Erzeugt `count` zufällig parametrisierte Aufgaben für die angegebene Klassenstufe.</summary>
    IReadOnlyList<QuizQuestion> Generate(GradeLevel grade, int count, Random random);
}
