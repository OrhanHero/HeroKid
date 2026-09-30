using LernTor.Core.Enums;
using LernTor.Core.Services;

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

    /// <summary>
    /// Korrekte Antwort(en). Bei OpenText: akzeptierte Lösungen, eine reicht. Seit 30.09.2026
    /// muss die Eingabe einer davon entsprechen, statt sie nur zu enthalten - welche Abweichungen
    /// erlaubt sind, steht in <see cref="OpenTextAnswerMatcher"/>.
    /// </summary>
    public required IReadOnlyList<string> CorrectAnswers { get; init; }

    /// <summary>Ausführliche Erklärung / Lösungsweg, wird nach Beantwortung gezeigt.</summary>
    public required string Explanation { get; init; }

    /// <summary>
    /// Optionaler Tipp/Formel-Hinweis, den man sich VOR dem Beantworten anzeigen lassen kann (z.B.
    /// "Prozentwert = Grundwert · Prozentsatz / 100"). Anders als <see cref="Explanation"/> verrät er
    /// nicht die konkrete Lösung dieser Aufgabe, sondern nur den Rechenweg/das Vorgehen.
    /// </summary>
    public string? HelpHint { get; init; }

    /// <summary>Optionales Bild (z.B. für News-Artikel-Fragen). Steht ÜBER der Frage.</summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// Optionales Bild zur ERKLÄRUNG - wird erst nach dem Antworten gezeigt.
    ///
    /// <para>Bewusst getrennt von <see cref="ImageUrl"/>: ein Bild über der Frage kann die
    /// Antwort verraten. „Was war der Koloss von Rhodos?" mit einer Bronzestatue daneben ist
    /// keine Frage mehr. Nach der Antwort ist dasselbe Bild dagegen Belohnung und Merkhilfe.</para>
    /// </summary>
    public string? ExplanationImageUrl { get; init; }

    /// <summary>
    /// Bildunterschrift zu <see cref="ExplanationImageUrl"/> - etwa der Hinweis, dass es sich um
    /// eine KI-generierte Rekonstruktion handelt. Steht je Frage, weil ein Bild auch etwas
    /// zeigen kann, das ausdrücklich NICHT so war (siehe Koloss von Rhodos).
    /// </summary>
    public string? ExplanationImageCaption { get; init; }

    /// <summary>
    /// Zeigt bei offenen Fragen die türkische Sonderzeichen-Hilfe (ç ğ ı İ ş) an, auch wenn
    /// <see cref="Subject"/> nicht Tuerkisch ist - z.B. bei News-Fragen zu türkischsprachigen
    /// Artikeln, deren erwartete Antwort türkische Sonderzeichen enthalten kann.
    /// </summary>
    public bool RequiresTurkishCharacters { get; init; }

    /// <summary>
    /// Offene Antwort nur mit richtiger Groß-/Kleinschreibung richtig - für Aufgaben, bei denen
    /// genau das geübt wird (Deutsch: Groß- und Kleinschreibung). Sonst ist "auto" für "Auto"
    /// ebenso richtig wie für jede andere Aufgabe.
    /// </summary>
    public bool CaseSensitive { get; init; }

    public bool CheckAnswer(string givenAnswer)
    {
        if (string.IsNullOrWhiteSpace(givenAnswer))
        {
            return false;
        }

        var trimmedGiven = givenAnswer.Trim();
        var normalizedGiven = OpenTextAnswerMatcher.Lateinisch(trimmedGiven);

        return Type switch
        {
            // Diktat: Wort-für-Wort-Vergleich statt "enthält" - beim Rechtschreiben ist genau die
            // Schreibweise die Aufgabe, und ein Satz, in dem der erwartete irgendwo vorkommt,
            // wäre kein richtig geschriebenes Diktat.
            QuestionType.Diktat => DictationEvaluator
                .Evaluate(CorrectAnswers.FirstOrDefault(), trimmedGiven).IsPerfect,
            QuestionType.OpenText => CorrectAnswers.Any(correct =>
                OpenTextAnswerMatcher.Matches(correct, trimmedGiven, Prompt, CaseSensitive)),
            _ => CorrectAnswers.Any(correct =>
                string.Equals(correct, trimmedGiven, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(OpenTextAnswerMatcher.Lateinisch(correct), normalizedGiven, StringComparison.Ordinal))
        };
    }
}
