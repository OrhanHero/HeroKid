using System.Text.RegularExpressions;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Erzeugt pro Artikel EINE Verständnisfrage ohne externes NLP/LLM: einen Lückentext - ein Satz
/// aus der Zusammenfassung mit ausgeblendetem Schlüsselwort. Wer den Text gelesen hat, erkennt
/// das fehlende Wort sofort; ohne Lesen ist die Frage nicht lösbar. Frühere Fragetypen wurden
/// auf Nutzerwunsch entfernt: die "Nenne ein wichtiges Wort aus der Überschrift"-Frage (mit
/// einem Blick lösbar) und die Rubrik-/Regionsfrage (unnötig, und bei Fehlklassifikation der
/// Rubrik sogar unfair). Gibt die Zusammenfassung keinen Lückentext her (sehr kurzer Text),
/// bleibt der Artikel ohne Frage - die Mindest-Lesezeit gilt trotzdem (siehe NewsViewModel).
/// Deterministisch (kein Random): dieselbe Nachricht ergibt immer dieselbe Frage.
/// </summary>
public sealed class HeuristicComprehensionQuestionGenerator : IComprehensionQuestionGenerator
{
    private static readonly HashSet<string> Stopwords = new(StringComparer.OrdinalIgnoreCase)
    {
        "der", "die", "das", "und", "mit", "für", "aus", "von", "nach", "über", "einen", "eine",
        "ist", "sind", "wird", "werden", "hat", "haben", "auch", "sich", "sein", "ihre", "ihrer",
        "als", "wie", "wegen", "durch", "beim", "zwischen", "diese", "dieser", "dieses", "einem",
        "einer", "nicht", "noch", "schon", "aber", "oder", "wurde", "wurden", "worden", "gegen"
    };

    public IReadOnlyList<QuizQuestion> GenerateQuestions(NewsArticle article)
    {
        var clozeQuestion = BuildClozeQuestion(article);
        return clozeQuestion is null ? Array.Empty<QuizQuestion>() : new[] { clozeQuestion };
    }

    /// <summary>
    /// Lückentext aus dem ersten brauchbaren Satz der Zusammenfassung: das längste inhaltstragende
    /// Wort (≥ 6 Buchstaben, kein Stoppwort) wird durch eine Lücke ersetzt; die Ablenker-Optionen
    /// sind andere Inhaltswörter aus dem restlichen Text. Liefert null, wenn Satz oder Wörter
    /// fehlen (sehr kurze Zusammenfassung) - der Artikel bleibt dann ohne Frage.
    /// </summary>
    private static QuizQuestion? BuildClozeQuestion(NewsArticle article)
    {
        var sentences = Regex.Split(article.SimplifiedSummary, @"(?<=[.!?])\s+")
            .Where(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 4)
            .ToList();

        if (sentences.Count == 0)
        {
            return null;
        }

        var sentence = sentences[0];
        var contentWords = ExtractContentWords(sentence);
        if (contentWords.Count == 0)
        {
            return null;
        }

        var answer = contentWords.OrderByDescending(w => w.Length).First();
        var gapped = Regex.Replace(sentence, $@"\b{Regex.Escape(answer)}\b", "_____");

        // Ablenker aus dem übrigen Text (nicht aus demselben Satz, damit sie nicht direkt
        // daneben stehen); reichen die nicht, ergänzen neutrale Standard-Ablenker.
        var distractors = ExtractContentWords(string.Join(" ", sentences.Skip(1)) + " " + article.Title)
            .Where(w => !string.Equals(w, answer, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(w => w.Length)
            .Take(2)
            .ToList();

        foreach (var fallback in new[] { "Wochenende", "Nachbarschaft" })
        {
            if (distractors.Count >= 2)
            {
                break;
            }
            if (!string.Equals(fallback, answer, StringComparison.OrdinalIgnoreCase))
            {
                distractors.Add(fallback);
            }
        }

        var options = distractors.Append(answer).OrderBy(o => o, StringComparer.OrdinalIgnoreCase).ToList();

        return new QuizQuestion
        {
            Id = $"news-{article.Id}-lueckentext",
            Subject = Subject.News,
            GradeLevel = GradeLevel.Klasse6,
            Topic = $"News: {article.Title}",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Lies den Artikel genau! Welches Wort fehlt in diesem Satz daraus?\n\n\"{gapped}\"",
            Options = options,
            CorrectAnswers = new[] { answer },
            Explanation = $"Der Satz aus dem Artikel lautet: \"{sentence}\"",
            ImageUrl = article.ImageUrl
        };
    }

    private static List<string> ExtractContentWords(string text) => text
        .Split(new[] { ' ', ',', '.', ':', ';', '!', '?', '-', '"', '„', '“', '(', ')' },
            StringSplitOptions.RemoveEmptyEntries)
        .Where(w => w.Length >= 6 && !Stopwords.Contains(w) && w.All(char.IsLetter))
        .ToList();
}
