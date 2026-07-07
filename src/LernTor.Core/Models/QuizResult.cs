using LernTor.Core.Enums;

namespace LernTor.Core.Models;

public sealed class QuestionOutcome
{
    public required string QuestionId { get; init; }
    public required Subject Subject { get; init; }
    public required string GivenAnswer { get; init; }
    public required bool WasCorrect { get; init; }
}

public sealed class QuizResult
{
    public const double PassThreshold = 0.5;

    public required IReadOnlyList<QuestionOutcome> Outcomes { get; init; }

    public int TotalQuestions => Outcomes.Count;
    public int CorrectCount => Outcomes.Count(o => o.WasCorrect);
    public double ScorePercentage => TotalQuestions == 0 ? 0 : (double)CorrectCount / TotalQuestions;
    public bool Passed => ScorePercentage >= PassThreshold;

    /// <summary>Fachbereiche, in denen die Trefferquote unter 50% lag – für gezielte Wiederholung.</summary>
    public IReadOnlyList<Subject> WeakSubjects => Outcomes
        .GroupBy(o => o.Subject)
        .Where(g => g.Count(o => o.WasCorrect) / (double)g.Count() < PassThreshold)
        .Select(g => g.Key)
        .ToList();
}
