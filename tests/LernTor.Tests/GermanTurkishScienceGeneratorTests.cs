using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>Generische Tests, die für jeden nicht-mathematischen Aufgabengenerator gelten.</summary>
public class GermanTurkishScienceGeneratorTests
{
    public static IEnumerable<object[]> AllGenerators()
    {
        yield return new object[] { new GermanGenerator() };
        yield return new object[] { new TurkishGenerator() };
        yield return new object[] { new EnglischGenerator() };
        yield return new object[] { new BiologieGenerator() };
        yield return new object[] { new ChemieGenerator() };
        yield return new object[] { new PhysikGenerator() };
        yield return new object[] { new GewiGenerator() };
        yield return new object[] { new PolitikGenerator() };
        yield return new object[] { new GeoGenerator() };
        yield return new object[] { new EthikGenerator() };
        yield return new object[] { new ItgGenerator() };
    }

    [Theory]
    [MemberData(nameof(AllGenerators))]
    public void Generate_BothGrades_ProducesValidQuestions(IExerciseGenerator generator)
    {
        var random = new Random(7);

        foreach (var grade in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            var questions = generator.Generate(grade, 10, random);

            Assert.Equal(10, questions.Count);
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

    [Theory]
    [MemberData(nameof(AllGenerators))]
    public void Generate_WithinPoolSize_DoesNotReturnDuplicatePrompts(IExerciseGenerator generator)
    {
        // Regression-Test: bei kleinen Themen-Pools (z.B. Türkisch) durfte reines Ziehen mit
        // Zurücklegen bisher denselben Fragetext mehrfach in einem Quiz liefern.
        var random = new Random(2026);

        foreach (var grade in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            var questions = generator.Generate(grade, 5, random);
            var distinctPrompts = questions.Select(q => q.Prompt).Distinct().Count();

            Assert.Equal(questions.Count, distinctPrompts);
        }
    }
}
