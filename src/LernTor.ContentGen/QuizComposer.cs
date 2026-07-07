using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen;

/// <summary>
/// Stellt das gemischte Abschlussquiz aus allen Fachbereichen zusammen (20-25 Fragen)
/// und kann gezielt schwache Bereiche für eine Wiederholung nachliefern.
/// </summary>
public sealed class QuizComposer
{
    private readonly IReadOnlyList<IExerciseGenerator> _generators;

    public QuizComposer()
        : this(new IExerciseGenerator[] { new MathGenerator(), new GermanGenerator(), new TurkishGenerator(), new ScienceGenerator() })
    {
    }

    public QuizComposer(IReadOnlyList<IExerciseGenerator> generators)
    {
        _generators = generators;
    }

    /// <summary>
    /// Baut das Abschlussquiz: standardmäßig je 4-5 Fragen aus Mathe/Deutsch/Türkisch/NaWi
    /// plus alle mitgegebenen News-Verständnisfragen, insgesamt 20-25 Fragen.
    /// </summary>
    public IReadOnlyList<QuizQuestion> ComposeFinalQuiz(
        GradeLevel grade,
        Random random,
        IReadOnlyList<QuizQuestion>? newsQuestions = null,
        int perSubjectCount = 5)
    {
        var questions = new List<QuizQuestion>();

        if (newsQuestions is not null)
        {
            questions.AddRange(newsQuestions);
        }

        foreach (var generator in _generators)
        {
            questions.AddRange(generator.Generate(grade, perSubjectCount, random));
        }

        // Zusätzliche Absicherung: falls trotz der Deduplizierung in den Generatoren/News
        // irgendwo doch derselbe Fragetext zweimal zusammenkommt, hier ein letztes Mal filtern.
        var distinctQuestions = questions
            .GroupBy(q => q.Prompt)
            .Select(group => group.First())
            .ToList();

        return distinctQuestions.OrderBy(_ => random.Next()).ToList();
    }

    /// <summary>Erzeugt zusätzliche Übungsfragen für gezielt schwache Fachbereiche nach nicht bestandenem Quiz.</summary>
    public IReadOnlyList<QuizQuestion> ComposeRetryExercises(
        IReadOnlyList<Subject> weakSubjects,
        GradeLevel grade,
        Random random,
        int countPerSubject = 5)
    {
        var questions = new List<QuizQuestion>();

        foreach (var subject in weakSubjects)
        {
            var generator = _generators.FirstOrDefault(g => g.Subject == subject);
            if (generator is not null)
            {
                questions.AddRange(generator.Generate(grade, countPerSubject, random));
            }
        }

        return questions;
    }

    public IReadOnlyList<QuizQuestion> GenerateExercises(Subject subject, GradeLevel grade, int count, Random random)
    {
        var generator = _generators.FirstOrDefault(g => g.Subject == subject);
        return generator?.Generate(grade, count, random) ?? Array.Empty<QuizQuestion>();
    }
}
