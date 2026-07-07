using LernTor.ContentGen;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

public class QuizComposerTests
{
    private readonly QuizComposer _composer = new();

    [Fact]
    public void ComposeFinalQuiz_WithoutNews_ReturnsFourSubjectsTimesCount()
    {
        var questions = _composer.ComposeFinalQuiz(GradeLevel.Klasse6, new Random(1), newsQuestions: null, perSubjectCount: 5);

        Assert.Equal(20, questions.Count); // 4 Fächer x 5 Fragen
        var subjectsPresent = questions.Select(q => q.Subject).Distinct().ToList();
        Assert.Equal(4, subjectsPresent.Count);
    }

    [Fact]
    public void ComposeFinalQuiz_WithNews_IncludesNewsQuestionsAndIsInTargetRange()
    {
        var newsQuestions = new List<QuizQuestion>
        {
            new()
            {
                Id = "news-1", Subject = Subject.News, GradeLevel = GradeLevel.Klasse6, Topic = "Test",
                Prompt = "Testfrage?", Type = QuestionType.OpenText,
                CorrectAnswers = new[] { "Berlin" }, Explanation = "Weil Berlin die Hauptstadt ist."
            }
        };

        var questions = _composer.ComposeFinalQuiz(GradeLevel.Klasse6, new Random(2), newsQuestions, perSubjectCount: 5);

        Assert.InRange(questions.Count, 20, 25);
        Assert.Contains(questions, q => q.Id == "news-1");
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
