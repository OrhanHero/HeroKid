using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen;

/// <summary>
/// Stellt das gemischte Abschlussquiz aus allen aktiven Fachbereichen zusammen (Ziel beim ersten
/// Versuch: exakt 20 Fragen, bei einer Wiederholung nach nicht bestandenem Quiz: exakt 15 Fragen -
/// die genaue Zielzahl gibt der Aufrufer über den Parameter <c>targetTotalQuestions</c> vor, siehe
/// <c>MainViewModel.BuildFinalQuizViewModelAsync</c>) und kann gezielt schwache Bereiche für eine
/// Wiederholung nachliefern. News-Verständnisfragen fließen NICHT ins Abschlussquiz ein - sie
/// werden ausschließlich im News-Bereich selbst gestellt.
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
    /// Baut das Abschlussquiz aus allen NICHT deaktivierten Fachbereichen. Die Fragenzahl pro Fach
    /// wird so verteilt, dass in Summe GENAU <paramref name="targetTotalQuestions"/> Fragen
    /// herauskommen (Rest der Ganzzahl-Division auf die ersten Fächer verteilt) - unabhängig davon,
    /// ob gerade 3 oder 12 Fächer aktiv sind.
    /// </summary>
    public IReadOnlyList<QuizQuestion> ComposeFinalQuiz(
        GradeLevel grade,
        Random random,
        IReadOnlySet<Subject>? disabledSubjects = null,
        int targetTotalQuestions = 20,
        IReadOnlySet<string>? recentlySeenPrompts = null)
    {
        disabledSubjects ??= new HashSet<Subject>();
        var activeGenerators = _generators.Where(g => !disabledSubjects.Contains(g.Subject)).ToList();

        var questions = new List<QuizQuestion>();

        if (activeGenerators.Count > 0)
        {
            var baseCount = targetTotalQuestions / activeGenerators.Count;
            var remainder = targetTotalQuestions % activeGenerators.Count;

            for (var i = 0; i < activeGenerators.Count; i++)
            {
                var count = baseCount + (i < remainder ? 1 : 0);
                if (count > 0)
                {
                    questions.AddRange(activeGenerators[i].Generate(grade, count, random, recentlySeenPrompts));
                }
            }
        }

        // Zusätzliche Absicherung: falls trotz der Deduplizierung in den Generatoren irgendwo doch
        // derselbe Fragetext zweimal zusammenkommt, hier ein letztes Mal filtern.
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
        int countPerSubject = 5,
        IReadOnlySet<string>? recentlySeenPrompts = null)
    {
        var questions = new List<QuizQuestion>();

        foreach (var subject in weakSubjects)
        {
            var generator = _generators.FirstOrDefault(g => g.Subject == subject);
            if (generator is not null)
            {
                questions.AddRange(generator.Generate(grade, countPerSubject, random, recentlySeenPrompts));
            }
        }

        return questions;
    }

    public IReadOnlyList<QuizQuestion> GenerateExercises(
        Subject subject, GradeLevel grade, int count, Random random, IReadOnlySet<string>? recentlySeenPrompts = null)
    {
        var generator = _generators.FirstOrDefault(g => g.Subject == subject);
        return generator?.Generate(grade, count, random, recentlySeenPrompts) ?? Array.Empty<QuizQuestion>();
    }
}
