using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

public class GermanTurkishScienceGeneratorTests
{
    public static IEnumerable<object[]> AllGenerators()
    {
        yield return new object[] { new GermanGenerator() };
        yield return new object[] { new TurkishGenerator() };
        yield return new object[] { new ScienceGenerator() };
    }

    [Theory]
    [MemberData(nameof(AllGenerators))]
    public void Generate_BothGrades_ProducesValidQuestions(IExerciseGenerator generator)
    {
        var random = new Random(7);

        foreach (var grade in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            var questions = generator.Generate(grade, 15, random);

            Assert.Equal(15, questions.Count);
            foreach (var question in questions)
            {
                Assert.Equal(generator.Subject, question.Subject);
                Assert.NotEmpty(question.CorrectAnswers);
                Assert.False(string.IsNullOrWhiteSpace(question.Explanation));
                Assert.True(question.CheckAnswer(question.CorrectAnswers[0]));

                if (question.Type == QuestionType.MultipleChoice)
                {
                    Assert.NotEmpty(question.Options);
                    Assert.Contains(question.CorrectAnswers[0], question.Options);
                }
            }
        }
    }
}
