using System.Text;
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

        var trimmedGiven = givenAnswer.Trim();
        var normalizedGiven = ToLatinKeyboardForm(trimmedGiven);

        return Type switch
        {
            QuestionType.OpenText => CorrectAnswers.Any(correct =>
                trimmedGiven.Contains(correct, StringComparison.OrdinalIgnoreCase) ||
                normalizedGiven.Contains(ToLatinKeyboardForm(correct), StringComparison.Ordinal)),
            _ => CorrectAnswers.Any(correct =>
                string.Equals(correct, trimmedGiven, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ToLatinKeyboardForm(correct), normalizedGiven, StringComparison.Ordinal))
        };
    }

    /// <summary>
    /// Vereinfacht Sonderzeichen (v.a. türkisch: ç ğ ı İ ş, dazu ö ü) auf ihre nächste auf einer
    /// deutschen Tastatur eingebbare Näherung und wandelt in Kleinbuchstaben um. Wird nur als
    /// zusätzlicher Vergleich genutzt (Kinder ohne türkische Tastatur können trotzdem antworten),
    /// die exakte Schreibweise wird weiterhin zuerst geprüft.
    /// </summary>
    private static string ToLatinKeyboardForm(string text)
    {
        var builder = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            builder.Append(ch switch
            {
                'ç' or 'Ç' => 'c',
                'ğ' or 'Ğ' => 'g',
                'ı' or 'İ' => 'i',
                'ş' or 'Ş' => 's',
                'ö' or 'Ö' => 'o',
                'ü' or 'Ü' => 'u',
                _ => ch
            });
        }

        return builder.ToString().ToLowerInvariant().Trim();
    }
}
