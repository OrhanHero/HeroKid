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
        LearningStage.News,
        LearningStage.Mathematik,
        LearningStage.Deutsch,
        LearningStage.Tuerkisch,
        LearningStage.Englisch,
        LearningStage.Biologie,
        LearningStage.Chemie,
        LearningStage.Physik,
        LearningStage.Gewi,
        LearningStage.Politik,
        LearningStage.Geo,
        LearningStage.Ethik,
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
    /// Schwächen. Die 50%-Hürde (<see cref="QuizResult.PassThreshold"/>) gilt nur beim ersten
    /// Versuch am Tag - ein zweiter Anlauf (<paramref name="isRetryAttempt"/> = true, weil das Kind
    /// die schwachen Fächer bereits wiederholt hat) schaltet danach in jedem Fall frei, unabhängig
    /// vom erzielten Ergebnis.
    /// </summary>
    public void ApplyQuizResult(StudentProgress progress, QuizResult result, bool isRetryAttempt = false)
    {
        progress.FinalQuizAttempts++;
        progress.LastQuizScore = result.ScorePercentage;
        progress.LastUpdatedAt = DateTimeOffset.Now;

        if (result.Passed || isRetryAttempt)
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
