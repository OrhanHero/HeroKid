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
        LearningStage.News,
        LearningStage.Mathematik,
        LearningStage.Deutsch,
        LearningStage.Tuerkisch,
        LearningStage.Naturwissenschaften,
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
        return stage switch
        {
            LearningStage.Willkommen => true,
            LearningStage.News => progress.CompletedNewsArticleIds.Count > 0,
            LearningStage.Mathematik => disabledSubjects.Contains(Subject.Mathematik)
                || progress.CompletedExerciseSubjects.Contains(Subject.Mathematik),
            LearningStage.Deutsch => disabledSubjects.Contains(Subject.Deutsch)
                || progress.CompletedExerciseSubjects.Contains(Subject.Deutsch),
            LearningStage.Tuerkisch => disabledSubjects.Contains(Subject.Tuerkisch)
                || progress.CompletedExerciseSubjects.Contains(Subject.Tuerkisch),
            LearningStage.Naturwissenschaften => disabledSubjects.Contains(Subject.Naturwissenschaften)
                || progress.CompletedExerciseSubjects.Contains(Subject.Naturwissenschaften),
            LearningStage.Abschlussquiz => true,
            _ => true
        };
    }

    /// <summary>Verarbeitet ein Quizergebnis: entweder Freischaltung, oder gezielte Wiederholung der Schwächen.</summary>
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
