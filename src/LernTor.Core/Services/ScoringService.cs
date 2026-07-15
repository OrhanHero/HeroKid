using LernTor.Core.Models;

namespace LernTor.Core.Services;

public sealed class ScoringService
{
    public QuestionOutcome Evaluate(QuizQuestion question, string givenAnswer)
    {
        return new QuestionOutcome
        {
            QuestionId = question.Id,
            Subject = question.Subject,
            GivenAnswer = givenAnswer,
            WasCorrect = question.CheckAnswer(givenAnswer)
        };
    }

    public QuizResult BuildResult(IEnumerable<QuestionOutcome> outcomes)
    {
        return new QuizResult { Outcomes = outcomes.ToList() };
    }
}
