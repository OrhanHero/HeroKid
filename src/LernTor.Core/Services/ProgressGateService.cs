using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Entscheidet, welche Lernstufe als nächstes freigeschaltet wird. Der Kern der
/// "erst abschließen, dann weiter"-Logik.
/// </summary>
public sealed class ProgressGateService
{
    private static readonly LearningStage[] SequentialOrder =
    {
        LearningStage.Willkommen,
        LearningStage.Vorlesen,
        LearningStage.Tippen,
        LearningStage.News,
        LearningStage.Mathematik,
        LearningStage.Deutsch,
        LearningStage.Tuerkisch,
        LearningStage.Englisch,
        LearningStage.Biologie,
        LearningStage.Chemie,
        LearningStage.Physik,
        LearningStage.Geschichte,
        LearningStage.Gewi,
        LearningStage.Politik,
        LearningStage.Geo,
        LearningStage.Ethik,
        LearningStage.Kunst,
        LearningStage.Musik,
        LearningStage.Itg,
        LearningStage.Abschlussquiz,
        LearningStage.Freigeschaltet
    };

    public LearningStage GetNextStage(LearningStage current)
    {
        var index = Array.IndexOf(SequentialOrder, current);
        if (index < 0 || index >= SequentialOrder.Length - 1)
        {
            return SequentialOrder[^1];
        }

        return SequentialOrder[index + 1];
    }

    public bool CanEnterStage(StudentProgress progress, LearningStage targetStage, HashSet<Subject> disabledSubjects)
    {
        var targetIndex = Array.IndexOf(SequentialOrder, targetStage);
        var currentIndex = Array.IndexOf(SequentialOrder, progress.CurrentStage);

        // Man darf nie eine Stufe überspringen, außer sie wurde von den Eltern deaktiviert.
        if (targetIndex <= currentIndex)
        {
            return true;
        }

        for (var i = currentIndex + 1; i < targetIndex; i++)
        {
            var stageBetween = SequentialOrder[i];
            if (!IsStageSatisfied(progress, stageBetween, disabledSubjects))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsStageSatisfied(StudentProgress progress, LearningStage stage, HashSet<Subject> disabledSubjects)
    {
        if (stage == LearningStage.Willkommen || stage == LearningStage.Abschlussquiz)
        {
            return true;
        }

        if (stage == LearningStage.Vorlesen)
        {
            return progress.HasCompletedReading;
        }

        if (stage == LearningStage.Tippen)
        {
            return progress.HasCompletedTyping;
        }

        if (stage == LearningStage.News)
        {
            return progress.CompletedNewsArticleIds.Count > 0;
        }

        if (LearningStageSubjects.TryGetSubject(stage, out var subject))
        {
            return disabledSubjects.Contains(subject) || progress.CompletedExerciseSubjects.Contains(subject);
        }

        return true;
    }

    /// <summary>
    /// Verarbeitet ein Quizergebnis: entweder Freischaltung, oder gezielte Wiederholung der
    /// Schwächen. Der Schwellenwert (<see cref="QuizResult.PassThreshold"/>) unterscheidet sich
    /// bereits im übergebenen <paramref name="result"/> je nach Versuch (1./2., siehe
    /// StudentProfile.QuizFirstAttemptThreshold/QuizRetryThreshold und
    /// MainViewModel.OnFinalQuizCompleted) - hier wird deshalb einheitlich nur noch
    /// <see cref="QuizResult.Passed"/> geprüft.
    /// </summary>
    public void ApplyQuizResult(StudentProgress progress, QuizResult result)
    {
        progress.FinalQuizAttempts++;
        progress.LastQuizScore = result.ScorePercentage;
        progress.LastUpdatedAt = DateTimeOffset.Now;

        if (result.Passed)
        {
            progress.IsUnlocked = true;
            progress.CurrentStage = LearningStage.Freigeschaltet;
            progress.SubjectsToRetry.Clear();
        }
        else
        {
            progress.SubjectsToRetry = result.WeakSubjects.ToList();
            progress.CurrentStage = LearningStage.Abschlussquiz;
        }
    }
}
