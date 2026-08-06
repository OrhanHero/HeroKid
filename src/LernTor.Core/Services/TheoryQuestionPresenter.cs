using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Eine Theoriefrage, wie sie dem Kind gezeigt wird - mit gemischten Antworten.</summary>
/// <param name="Question">Die zugrunde liegende Frage.</param>
/// <param name="Options">Antworten in Anzeigereihenfolge.</param>
/// <param name="CorrectIndices">Positionen der richtigen Antworten in dieser Reihenfolge.</param>
public readonly record struct PresentedQuestion(
    TheoryQuestion Question,
    IReadOnlyList<string> Options,
    IReadOnlyList<int> CorrectIndices)
{
    public bool IsMultipleChoice => CorrectIndices.Count > 1;

    /// <summary>Vollständig richtig: alle richtigen angekreuzt, keine falsche dazu.</summary>
    public bool IsCorrect(IEnumerable<int> chosen)
    {
        var gewaehlt = chosen.ToHashSet();
        return gewaehlt.Count == CorrectIndices.Count && CorrectIndices.All(gewaehlt.Contains);
    }

    public IEnumerable<string> CorrectAnswers => CorrectIndices.Select(i => Options[i]);
}

/// <summary>
/// Mischt die Antwortmöglichkeiten, bevor eine Frage angezeigt wird.
///
/// <para><b>Warum das sein muss:</b> im Katalog steht die richtige Antwort bewusst an erster
/// Stelle - so ist eine Frage beim Schreiben und beim späteren Nachlesen sofort zu erfassen.
/// Genau so angezeigt wäre sie aber wertlos: wer immer die erste ankreuzt, hätte volle
/// Punktzahl, ohne ein Wort verstanden zu haben. Diese Codebasis hat schon einmal erlebt, dass
/// Kinder ein solches Muster finden und ausnutzen - dort war es "die längste Antwort ist die
/// richtige" (siehe <c>scripts/check-answer-length-bias.py</c>).</para>
///
/// <para>Gemischt wird an EINER Stelle, damit keine Ansicht es vergessen kann. Wer eine Frage
/// anzeigt, geht durch <see cref="Present"/> - anders kommt man an die Antworten nicht heran,
/// ohne es zu merken.</para>
/// </summary>
public static class TheoryQuestionPresenter
{
    /// <summary>Mischt die Antworten einer Frage und zieht die richtigen Positionen mit.</summary>
    public static PresentedQuestion Present(TheoryQuestion question, Random random)
    {
        var reihenfolge = Enumerable.Range(0, question.Options.Count).ToList();

        for (var i = reihenfolge.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (reihenfolge[i], reihenfolge[j]) = (reihenfolge[j], reihenfolge[i]);
        }

        var optionen = reihenfolge.Select(alt => question.Options[alt]).ToList();

        // Neue Position jeder richtigen Antwort: wo ist ihr alter Index gelandet?
        var richtige = question.CorrectIndices
            .Select(alt => reihenfolge.IndexOf(alt))
            .OrderBy(index => index)
            .ToList();

        return new PresentedQuestion(question, optionen, richtige);
    }

    /// <summary>Mischt mehrere Fragen, Reihenfolge der Fragen bleibt erhalten.</summary>
    public static IReadOnlyList<PresentedQuestion> PresentAll(
        IEnumerable<TheoryQuestion> questions, Random random) =>
        questions.Select(frage => Present(frage, random)).ToList();
}
