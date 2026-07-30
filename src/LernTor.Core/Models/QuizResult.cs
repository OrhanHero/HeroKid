using LernTor.Core.Enums;

namespace LernTor.Core.Models;

public sealed class QuestionOutcome
{
    public required string QuestionId { get; init; }
    public required Subject Subject { get; init; }
    public required string GivenAnswer { get; init; }
    public required bool WasCorrect { get; init; }

    /// <summary>
    /// Bearbeitungsdauer in Millisekunden (von der Anzeige der Frage bis zum Absenden).
    /// 0 = nicht gemessen. Fließt nur ins Eltern-Protokoll, nicht in die Bewertung -
    /// schnell zu antworten ist kein Fehler, siehe AnswerPaceAnalyzer.
    /// </summary>
    public int DurationMs { get; init; }
}

public sealed class QuizResult
{
    /// <summary>
    /// Mindest-Trefferquote zum Bestehen. Standard 50%, aber pro Profil und Versuch (1./2.) über
    /// StudentProfile.QuizFirstAttemptThreshold/QuizRetryThreshold einstellbar - siehe
    /// ScoringService.BuildResult, MainViewModel.OnFinalQuizCompleted.
    /// </summary>
    public double PassThreshold { get; init; } = 0.5;

    public required IReadOnlyList<QuestionOutcome> Outcomes { get; init; }

    public int TotalQuestions => Outcomes.Count;
    public int CorrectCount => Outcomes.Count(o => o.WasCorrect);
    public double ScorePercentage => TotalQuestions == 0 ? 0 : (double)CorrectCount / TotalQuestions;
    public bool Passed => ScorePercentage >= PassThreshold;

    /// <summary>Fachbereiche, in denen die Trefferquote unter PassThreshold lag – für gezielte Wiederholung.</summary>
    public IReadOnlyList<Subject> WeakSubjects => Outcomes
        .GroupBy(o => o.Subject)
        .Where(g => g.Count(o => o.WasCorrect) / (double)g.Count() < PassThreshold)
        .Select(g => g.Key)
        .ToList();
}
