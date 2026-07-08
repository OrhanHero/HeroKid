using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Erzeugt pro Artikel zwei einfache Verständnisfragen ohne externes NLP/LLM:
/// 1) Eine Multiple-Choice-Frage zur Herkunft/Region des Artikels (testet, ob der Titel/Kontext erfasst wurde).
/// 2) Eine offene Frage, die ein Schlüsselwort aus der Überschrift abfragt (testet, ob die Überschrift gelesen wurde).
/// </summary>
public sealed class HeuristicComprehensionQuestionGenerator : IComprehensionQuestionGenerator
{
    private static readonly HashSet<string> Stopwords = new(StringComparer.OrdinalIgnoreCase)
    {
        "der", "die", "das", "und", "mit", "für", "aus", "von", "nach", "über", "einen", "eine",
        "ist", "sind", "wird", "werden", "hat", "haben", "auch", "sich", "sein", "ihre", "ihrer",
        "als", "wie", "wegen", "durch", "beim", "beim", "zwischen", "diese", "dieser"
    };

    public IReadOnlyList<QuizQuestion> GenerateQuestions(NewsArticle article)
    {
        var questions = new List<QuizQuestion>
        {
            BuildRegionQuestion(article),
        };

        var keywordQuestion = BuildKeywordQuestion(article);
        if (keywordQuestion is not null)
        {
            questions.Add(keywordQuestion);
        }

        return questions;
    }

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

    private static QuizQuestion? BuildKeywordQuestion(NewsArticle article)
    {
        var isTurkishArticle = article.RegionFocus is NewsRegionFocus.Tuerkei or NewsRegionFocus.Istanbul
            or NewsRegionFocus.Samsun or NewsRegionFocus.Uenye;

        var keywords = article.Title
            .Split(new[] { ' ', ',', '.', ':', '-', '"', '„', '“' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 5 && !Stopwords.Contains(w))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(5)
            .ToList();

        if (keywords.Count == 0)
        {
            return null;
        }

        return new QuizQuestion
        {
            Id = $"news-{article.Id}-keyword",
            Subject = Subject.News,
            GradeLevel = GradeLevel.Klasse6,
            Topic = $"News: {article.Title}",
            Type = QuestionType.OpenText,
            Prompt = $"Nenne ein wichtiges Wort aus der Überschrift: \"{article.Title}\"",
            CorrectAnswers = keywords,
            Explanation = $"Wichtige Wörter aus der Überschrift waren zum Beispiel: {string.Join(", ", keywords)}.",
            ImageUrl = article.ImageUrl,
            RequiresTurkishCharacters = isTurkishArticle
        };
    }
}
