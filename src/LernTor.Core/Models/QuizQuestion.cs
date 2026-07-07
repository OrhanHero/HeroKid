using LernTor.Core.Enums;

namespace LernTor.Core.Models;

public sealed class QuizQuestion
{
    public required string Id { get; init; }
    public required Subject Subject { get; init; }
    public required GradeLevel GradeLevel { get; init; }
    public required string Topic { get; init; }
    public required string Prompt { get; init; }
    public required QuestionType Type { get; init; }

    /// <summary>Antwortoptionen bei MultipleChoice/TrueFalse. Leer bei OpenText.</summary>
    public IReadOnlyList<string> Options { get; init; } = Array.Empty<string>();

    /// <summary>Korrekte Antwort(en). Bei OpenText: akzeptierte Stichworte (case-insensitive, eine reicht).</summary>
    public required IReadOnlyList<string> CorrectAnswers { get; init; }

    /// <summary>Ausführliche Erklärung / Lösungsweg, wird nach Beantwortung gezeigt.</summary>
    public required string Explanation { get; init; }

    /// <summary>Optionales Bild (z.B. für News-Artikel-Fragen).</summary>
    public string? ImageUrl { get; init; }

    public bool CheckAnswer(string givenAnswer)
    {
        if (string.IsNullOrWhiteSpace(givenAnswer))
        {
            return false;
        }

        var normalizedGiven = givenAnswer.Trim();

        return Type switch
        {
            QuestionType.OpenText => CorrectAnswers.Any(correct =>
                normalizedGiven.Contains(correct, StringComparison.OrdinalIgnoreCase)),
            _ => CorrectAnswers.Any(correct =>
                string.Equals(correct, normalizedGiven, StringComparison.OrdinalIgnoreCase))
        };
    }
}
