using LernTor.ContentGen;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

public class QuizComposerTests
{
    private readonly QuizComposer _composer = new();

    [Fact]
    public void ComposeFinalQuiz_WithoutDisabledSubjects_IncludesEveryDefaultGenerator()
    {
        var questions = _composer.ComposeFinalQuiz(GradeLevel.Klasse6, new Random(1));

        var subjectsPresent = questions.Select(q => q.Subject).Distinct().ToList();

        // Standard-Composer deckt 14 Fächer ab (alle außer News); jedes bekommt mindestens 1 Frage.
        Assert.Equal(14, subjectsPresent.Count);
        Assert.True(questions.Count > 0);
    }

    [Fact]
    public void ComposeFinalQuiz_WithoutDisabledSubjects_ReturnsExactTargetCount()
    {
        var questions = _composer.ComposeFinalQuiz(GradeLevel.Klasse6, new Random(1), targetTotalQuestions: 20);

        Assert.Equal(20, questions.Count);
    }

    [Fact]
    public void ComposeFinalQuiz_WithDisabledSubjects_ExcludesThem()
    {
        var enabled = new HashSet<Subject> { Subject.Mathematik, Subject.Deutsch, Subject.Tuerkisch, Subject.Englisch };
        var disabled = Enum.GetValues<Subject>()
            .Where(s => s != Subject.News && !enabled.Contains(s))
            .ToHashSet();

        var questions = _composer.ComposeFinalQuiz(GradeLevel.Klasse6, new Random(1), disabled, targetTotalQuestions: 20);

        var subjectsPresent = questions.Select(q => q.Subject).Distinct().ToList();
        Assert.Equal(4, subjectsPresent.Count);
        Assert.All(subjectsPresent, s => Assert.Contains(s, enabled));
        Assert.Equal(20, questions.Count);
    }

    [Fact]
    public void ComposeRetryExercises_OnlyGeneratesForRequestedSubjects()
    {
        var weakSubjects = new List<Subject> { Subject.Mathematik, Subject.Tuerkisch };

        var questions = _composer.ComposeRetryExercises(weakSubjects, GradeLevel.Klasse9, new Random(3), countPerSubject: 4);

        Assert.Equal(8, questions.Count);
        Assert.All(questions, q => Assert.Contains(q.Subject, weakSubjects));
    }
}
