using System.Text.RegularExpressions;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Erzeugt pro Artikel zwei Verständnisfragen ohne externes NLP/LLM - beide so gebaut, dass die
/// NACHRICHT selbst gelesen und verstanden werden muss (die frühere "Nenne ein wichtiges Wort aus
/// der Überschrift"-Frage war mit einem Blick auf die Überschrift lösbar und wurde entfernt):
/// 1) Rubrik-Frage: zu welchem Themenbereich gehört die Nachricht? (Multiple Choice)
/// 2) Lückentext: ein Satz aus der Zusammenfassung mit ausgeblendetem Schlüsselwort - wer den
///    Text gelesen hat, erkennt das fehlende Wort sofort. Rückfall auf eine Regionsfrage, wenn
///    die Zusammenfassung kein geeignetes Wort hergibt.
/// Deterministisch (kein Random): dieselbe Nachricht ergibt immer dieselben Fragen.
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

    /// <summary>Anzeigenamen der Rubriken für die Rubrik-Frage (deutsch - die Fragen des
    /// Generators sind durchgehend deutsch formuliert).</summary>
    private static readonly IReadOnlyDictionary<NewsCategory, string> CategoryNames =
        new Dictionary<NewsCategory, string>
    {
        [NewsCategory.Berlin] = "Berlin",
        [NewsCategory.Deutschland] = "Deutschland",
        [NewsCategory.Welt] = "Welt",
        [NewsCategory.Tuerkei] = "Türkei",
        [NewsCategory.Ki] = "KI & Technik",
        [NewsCategory.Spiele] = "Spiele",
        [NewsCategory.Finanzen] = "Finanzen",
        [NewsCategory.Wetter] = "Wetter",
    };

    public IReadOnlyList<QuizQuestion> GenerateQuestions(NewsArticle article)
    {
        var questions = new List<QuizQuestion> { BuildCategoryQuestion(article) };

        var clozeQuestion = BuildClozeQuestion(article);
        questions.Add(clozeQuestion ?? BuildRegionQuestion(article));

        return questions;
    }

    /// <summary>Rubrik-Frage: verlangt, das THEMA der Nachricht erfasst zu haben. Die beiden
    /// falschen Optionen werden deterministisch aus den übrigen Rubriken gewählt.</summary>
    private static QuizQuestion BuildCategoryQuestion(NewsArticle article)
    {
        var correct = CategoryNames[article.Category];
        var distractors = Enum.GetValues<NewsCategory>()
            .Where(c => c != article.Category)
            .Take(2)
            .Select(c => CategoryNames[c]);

        var options = distractors.Append(correct).OrderBy(o => o, StringComparer.OrdinalIgnoreCase).ToList();

        return new QuizQuestion
        {
            Id = $"news-{article.Id}-rubrik",
            Subject = Subject.News,
            GradeLevel = GradeLevel.Klasse6,
            Topic = $"News: {article.Title}",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Zu welchem Themenbereich gehört diese Nachricht: \"{article.Title}\"?",
            Options = options,
            CorrectAnswers = new[] { correct },
            Explanation = $"Diese Nachricht gehört in die Rubrik {article.CategoryEmoji} {correct}.",
            ImageUrl = article.ImageUrl
        };
    }

    /// <summary>
    /// Lückentext aus dem ersten brauchbaren Satz der Zusammenfassung: das längste inhaltstragende
    /// Wort (≥ 6 Buchstaben, kein Stoppwort) wird durch eine Lücke ersetzt; die Ablenker-Optionen
    /// sind andere Inhaltswörter aus dem restlichen Text. Liefert null, wenn Satz oder Wörter
    /// fehlen - dann greift die Regionsfrage als Rückfall.
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

    /// <summary>Rückfall, wenn kein Lückentext möglich ist (sehr kurze Zusammenfassung).</summary>
    private static QuizQuestion BuildRegionQuestion(NewsArticle article)
    {
        var alleRegionen = new[] { "Deutschland/Berlin", "Türkei" };
        var richtig = article.RegionFocus is NewsRegionFocus.Tuerkei or NewsRegionFocus.Istanbul
            or NewsRegionFocus.Samsun or NewsRegionFocus.Uenye
            ? "Türkei"
            : "Deutschland/Berlin";

        return new QuizQuestion
        {
            Id = $"news-{article.Id}-region",
            Subject = Subject.News,
            GradeLevel = GradeLevel.Klasse6,
            Topic = $"News: {article.Title}",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Artikel \"{article.Title}\" (Quelle: {article.SourceName}) – worum geht es hier hauptsächlich?",
            Options = alleRegionen,
            CorrectAnswers = new[] { richtig },
            Explanation = $"Dieser Artikel von {article.SourceName} behandelt hauptsächlich Themen aus: {richtig}.",
            ImageUrl = article.ImageUrl
        };
    }
}
