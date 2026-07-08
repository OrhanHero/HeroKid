using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen;

/// <summary>
/// Stellt das gemischte Abschlussquiz aus allen aktiven Fachbereichen zusammen (Ziel: 20-25 Fragen)
/// und kann gezielt schwache Bereiche für eine Wiederholung nachliefern.
/// </summary>
public sealed class QuizComposer
{
    private readonly IReadOnlyList<IExerciseGenerator> _generators;

    public QuizComposer()
        : this(new IExerciseGenerator[]
        {
            new MathGenerator(), new GermanGenerator(), new TurkishGenerator(), new EnglischGenerator(),
            new BiologieGenerator(), new ChemieGenerator(), new PhysikGenerator(),
            new GewiGenerator(), new PolitikGenerator(), new GeoGenerator(), new EthikGenerator(), new ItgGenerator()
        })
    {
    }

    public QuizComposer(IReadOnlyList<IExerciseGenerator> generators)
    {
        _generators = generators;
    }

    /// <summary>
    /// Baut das Abschlussquiz aus allen NICHT deaktivierten Fachbereichen plus den mitgegebenen
    /// News-Verständnisfragen. Die Fragenzahl pro Fach wird dynamisch so verteilt, dass insgesamt
    /// ungefähr <paramref name="targetTotalQuestions"/> Fragen herauskommen - unabhängig davon, ob
    /// gerade 3 oder 12 Fächer aktiv sind (bei mehr aktiven Fächern also weniger Fragen je Fach).
    /// </summary>
    public IReadOnlyList<QuizQuestion> ComposeFinalQuiz(
        GradeLevel grade,
        Random random,
        IReadOnlyList<QuizQuestion>? newsQuestions = null,
        IReadOnlySet<Subject>? disabledSubjects = null,
        int targetTotalQuestions = 22)
    {
        disabledSubjects ??= new HashSet<Subject>();
        var activeGenerators = _generators.Where(g => !disabledSubjects.Contains(g.Subject)).ToList();

        var questions = new List<QuizQuestion>();

        // News höchstens etwa ein Drittel des Quiz - sonst würde ein Tag mit vielen Artikeln
        // das Quiz allein schon sprengen.
        var newsPool = (newsQuestions ?? Array.Empty<QuizQuestion>()).OrderBy(_ => random.Next()).ToList();
        var maxNewsQuestions = Math.Min(newsPool.Count, Math.Max(1, targetTotalQuestions / 3));
        questions.AddRange(newsPool.Take(maxNewsQuestions));

        if (activeGenerators.Count > 0)
        {
            var remainingBudget = Math.Max(targetTotalQuestions - questions.Count, activeGenerators.Count);
            var perSubjectCount = Math.Max(1, remainingBudget / activeGenerators.Count);

            foreach (var generator in activeGenerators)
            {
                questions.AddRange(generator.Generate(grade, perSubjectCount, random));
            }
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
