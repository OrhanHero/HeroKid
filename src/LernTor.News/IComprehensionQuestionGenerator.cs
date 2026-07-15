using LernTor.Core.Models;

namespace LernTor.News;

public interface IComprehensionQuestionGenerator
{
    /// <summary>Erzeugt 1-2 kurze Verständnisfragen zu einem Artikel.</summary>
    IReadOnlyList<QuizQuestion> GenerateQuestions(NewsArticle article);
}
