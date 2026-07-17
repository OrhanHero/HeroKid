using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class ProgressGateServiceTests
{
    private readonly ProgressGateService _gate = new();
    private readonly ScoringService _scoring = new();

    [Fact]
    public void GetNextStage_FollowsSequentialOrder()
    {
        Assert.Equal(LearningStage.Vorlesen, _gate.GetNextStage(LearningStage.Willkommen));
        Assert.Equal(LearningStage.Tippen, _gate.GetNextStage(LearningStage.Vorlesen));
        Assert.Equal(LearningStage.News, _gate.GetNextStage(LearningStage.Tippen));
        Assert.Equal(LearningStage.Mathematik, _gate.GetNextStage(LearningStage.News));
        Assert.Equal(LearningStage.Freigeschaltet, _gate.GetNextStage(LearningStage.Abschlussquiz));
        Assert.Equal(LearningStage.Freigeschaltet, _gate.GetNextStage(LearningStage.Freigeschaltet));
    }

    [Fact]
    public void ApplyQuizResult_PassingScore_UnlocksAndClearsRetryList()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.Abschlussquiz };
        var outcomes = Enumerable.Range(0, 10)
            .Select(i => new QuestionOutcome { QuestionId = $"q{i}", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = i < 6 })
            .ToList();
        var result = _scoring.BuildResult(outcomes);

        _gate.ApplyQuizResult(progress, result);

        Assert.True(progress.IsUnlocked);
        Assert.Equal(LearningStage.Freigeschaltet, progress.CurrentStage);
        Assert.Empty(progress.SubjectsToRetry);
        Assert.Equal(1, progress.FinalQuizAttempts);
    }

    [Fact]
    public void ApplyQuizResult_FailingScore_KeepsLockedAndFillsRetryList()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.Abschlussquiz };
        var outcomes = new[]
        {
            new QuestionOutcome { QuestionId = "q1", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q2", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q3", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q4", Subject = Subject.Deutsch, GivenAnswer = "x", WasCorrect = true },
        };
        var result = _scoring.BuildResult(outcomes);

        _gate.ApplyQuizResult(progress, result);

        Assert.False(progress.IsUnlocked);
        Assert.Contains(Subject.Mathematik, progress.SubjectsToRetry);
        Assert.DoesNotContain(Subject.Deutsch, progress.SubjectsToRetry);
    }

    [Fact]
    public void ApplyQuizResult_CustomLowerThreshold_PassesEvenBelowDefaultThreshold()
    {
        // Simuliert einen 2. Versuch mit einem niedrigeren, vom Profil konfigurierten
        // Schwellenwert (z.B. 25% statt der Standard-50%-Hürde des 1. Versuchs).
        var progress = new StudentProgress
        {
            ProfileId = "test-profile",
            CurrentStage = LearningStage.Abschlussquiz,
            SubjectsToRetry = new List<Subject> { Subject.Mathematik }
        };
        var outcomes = new[]
        {
            new QuestionOutcome { QuestionId = "q1", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = true },
            new QuestionOutcome { QuestionId = "q2", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q3", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q4", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
        };
        var result = _scoring.BuildResult(outcomes, passThreshold: 0.25);

        _gate.ApplyQuizResult(progress, result);

        Assert.True(progress.IsUnlocked);
        Assert.Equal(LearningStage.Freigeschaltet, progress.CurrentStage);
        Assert.Empty(progress.SubjectsToRetry);
    }

    [Fact]
    public void ApplyQuizResult_RetryAttempt_BelowConfiguredRetryThreshold_StaysLockedAndRefillsRetryList()
    {
        // Anders als früher schaltet ein 2. Versuch nicht mehr unabhängig vom Ergebnis frei - auch
        // beim (niedrigeren) Wiederholungs-Schwellenwert muss dieser tatsächlich erreicht werden.
        var progress = new StudentProgress
        {
            ProfileId = "test-profile",
            CurrentStage = LearningStage.Abschlussquiz,
            SubjectsToRetry = new List<Subject> { Subject.Mathematik }
        };
        var outcomes = new[]
        {
            new QuestionOutcome { QuestionId = "q1", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
            new QuestionOutcome { QuestionId = "q2", Subject = Subject.Mathematik, GivenAnswer = "x", WasCorrect = false },
        };
        var result = _scoring.BuildResult(outcomes, passThreshold: 0.25);

        _gate.ApplyQuizResult(progress, result);

        Assert.False(progress.IsUnlocked);
        Assert.Equal(LearningStage.Abschlussquiz, progress.CurrentStage);
        Assert.Contains(Subject.Mathematik, progress.SubjectsToRetry);
    }

    [Fact]
    public void CanEnterStage_BlocksSkippingUncompletedSubject()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.News };
        progress.CompletedNewsArticleIds.Add("a1");

        var canSkipToDeutsch = _gate.CanEnterStage(progress, LearningStage.Deutsch, new HashSet<Subject>());

        Assert.False(canSkipToDeutsch); // Mathematik wurde noch nicht abgeschlossen
    }

    [Fact]
    public void CanEnterStage_BlocksSkippingUnfinishedReadingSection()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.Willkommen, HasCompletedReading = false };

        var canSkipToNews = _gate.CanEnterStage(progress, LearningStage.News, new HashSet<Subject>());

        Assert.False(canSkipToNews); // Vorlesen (mind. 5 Minuten) wurde noch nicht abgeschlossen
    }

    [Fact]
    public void CanEnterStage_AllowsEnteringNewsAfterReadingAndTypingCompleted()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.Willkommen, HasCompletedReading = true, HasCompletedTyping = true };

        var canSkipToNews = _gate.CanEnterStage(progress, LearningStage.News, new HashSet<Subject>());

        Assert.True(canSkipToNews);
    }

    [Fact]
    public void CanEnterStage_AllowsSkippingDisabledSubject()
    {
        var progress = new StudentProgress { ProfileId = "test-profile", CurrentStage = LearningStage.News };
        progress.CompletedNewsArticleIds.Add("a1");

        var disabled = new HashSet<Subject> { Subject.Mathematik };
        var canSkipToDeutsch = _gate.CanEnterStage(progress, LearningStage.Deutsch, disabled);

        Assert.True(canSkipToDeutsch);
    }
}
