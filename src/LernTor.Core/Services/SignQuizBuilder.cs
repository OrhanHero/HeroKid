using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Eine Quizfrage zu einem Verkehrszeichen: Bild zeigen, Namen erkennen.</summary>
/// <param name="Sign">Das abgefragte Zeichen - die Ansicht zeichnet es.</param>
/// <param name="Options">Vier Antwortmöglichkeiten in Anzeigereihenfolge.</param>
/// <param name="CorrectIndex">Position der richtigen Antwort in <paramref name="Options"/>.</param>
public readonly record struct SignQuizQuestion(
    TrafficSign Sign,
    IReadOnlyList<string> Options,
    int CorrectIndex)
{
    public string CorrectAnswer => Options[CorrectIndex];

    public bool IsCorrect(int chosenIndex) => chosenIndex == CorrectIndex;
}

/// <summary>
/// Baut aus dem Zeichenkatalog Quizfragen.
///
/// <para><b>Ablenker aus derselben Gruppe.</b> Zu einem Gefahrzeichen stehen andere
/// Gefahrzeichen zur Wahl, nicht "Parken" oder "Autobahn". Sonst wäre die Gruppe schon die
/// halbe Antwort: wer ein rotes Dreieck sieht, könnte alles Blaue ausschließen, ohne das Zeichen
/// zu kennen. Reicht die eigene Gruppe nicht für drei Ablenker, wird aus dem Rest aufgefüllt.</para>
///
/// <para><b>Zur Antwortlängen-Falle</b> (siehe <c>scripts/check-answer-length-bias.py</c>): hier
/// entsteht sie gar nicht erst. Alle vier Optionen sind echte Zeichennamen aus demselben Katalog,
/// und welches davon das richtige ist, entscheidet das gezeigte Bild - die richtige Antwort ist
/// also rein zufällig mal die längste und mal die kürzeste. Ein Muster, das man ausnutzen könnte,
/// gibt es nicht.</para>
/// </summary>
public static class SignQuizBuilder
{
    /// <summary>Anzahl der Antwortmöglichkeiten je Frage.</summary>
    public const int OptionCount = 4;

    /// <summary>
    /// Baut eine Frage zu <paramref name="sign"/>. <paramref name="pool"/> liefert die Ablenker
    /// und muss das Zeichen selbst nicht enthalten.
    /// </summary>
    public static SignQuizQuestion Build(TrafficSign sign, IReadOnlyList<TrafficSign> pool, Random random)
    {
        var kandidaten = pool
            .Where(other => other.Number != sign.Number && other.Name != sign.Name)
            .ToList();

        var gleicheGruppe = kandidaten.Where(other => other.Category == sign.Category).ToList();
        var rest = kandidaten.Where(other => other.Category != sign.Category).ToList();

        var ablenker = Pick(gleicheGruppe, OptionCount - 1, random);
        if (ablenker.Count < OptionCount - 1)
        {
            ablenker.AddRange(Pick(rest, OptionCount - 1 - ablenker.Count, random));
        }

        var options = ablenker.Select(other => other.Name).ToList();
        var position = random.Next(options.Count + 1);
        options.Insert(position, sign.Name);

        return new SignQuizQuestion(sign, options, position);
    }

    /// <summary>Baut Fragen zu mehreren Zeichen, in der übergebenen Reihenfolge.</summary>
    public static IReadOnlyList<SignQuizQuestion> BuildMany(
        IEnumerable<TrafficSign> signs, IReadOnlyList<TrafficSign> pool, Random random) =>
        signs.Select(sign => Build(sign, pool, random)).ToList();

    private static List<TrafficSign> Pick(List<TrafficSign> from, int count, Random random)
    {
        if (count <= 0 || from.Count == 0)
        {
            return new List<TrafficSign>();
        }

        var kopie = new List<TrafficSign>(from);
        var gewaehlt = new List<TrafficSign>();

        while (gewaehlt.Count < count && kopie.Count > 0)
        {
            var index = random.Next(kopie.Count);
            gewaehlt.Add(kopie[index]);
            kopie.RemoveAt(index);
        }

        return gewaehlt;
    }
}
