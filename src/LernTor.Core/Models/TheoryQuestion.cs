using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Eine Theoriefrage.
///
/// <para><b>Mehrere Antworten können richtig sein</b> - das ist keine Spielerei, sondern der
/// Kern der echten Prüfung. Wer nur eine von zwei richtigen ankreuzt, hat die Frage falsch.
/// Genau daran scheitern Prüflinge, die mit Ein-aus-vier-Quizzen geübt haben, und deshalb ist
/// es hier von Anfang an so gebaut.</para>
///
/// <para><b>Fehlerpunkte statt Häkchen:</b> jede Frage wiegt 2, 3, 4 oder 5 Punkte, je nach
/// Gefährlichkeit des Irrtums. Eine falsch beantwortete 5-Punkte-Frage wiegt schwerer als zwei
/// falsche 2-Punkte-Fragen - auch das ist amtlich so und ändert, worauf man beim Üben achtet.</para>
/// </summary>
public sealed record TheoryQuestion
{
    /// <summary>Stabile Kennung, z.B. "vorfahrt-01". Schlüssel des Lernstands - nicht ändern.</summary>
    public required string Id { get; init; }

    public required DrivingTheoryTopic Topic { get; init; }

    public required string Prompt { get; init; }

    /// <summary>Antwortmöglichkeiten in fester Reihenfolge.</summary>
    public required IReadOnlyList<string> Options { get; init; }

    /// <summary>Positionen der richtigen Antworten - mindestens eine, oft mehrere.</summary>
    public required IReadOnlyList<int> CorrectIndices { get; init; }

    /// <summary>Warum das so ist. Erscheint nach der Antwort, nie vorher.</summary>
    public required string Explanation { get; init; }

    /// <summary>Fehlerpunkte bei falscher Antwort (2/3/4/5).</summary>
    public int Points { get; init; } = TheoryExamRules.DefaultPoints;

    /// <summary>Nummer eines Verkehrszeichens, das zur Frage gehört; die Ansicht zeichnet es dann
    /// mit. Null, wenn die Frage ohne Bild auskommt.</summary>
    public string? SignNumber { get; init; }

    /// <summary>Mehr als eine richtige Antwort - die Ansicht zeigt dann Kästchen statt Knöpfe.</summary>
    public bool IsMultipleChoice => CorrectIndices.Count > 1;

    /// <summary>
    /// Eine Antwort zählt nur, wenn sie <b>vollständig</b> stimmt: alle richtigen angekreuzt und
    /// keine falsche dazu. Teilpunkte gibt es in der Prüfung nicht.
    /// </summary>
    public bool IsCorrect(IEnumerable<int> chosen)
    {
        var gewaehlt = chosen.ToHashSet();
        return gewaehlt.Count == CorrectIndices.Count && CorrectIndices.All(gewaehlt.Contains);
    }

    /// <summary>Die richtigen Antworten als Text - für die Auflösung nach der Frage.</summary>
    public IEnumerable<string> CorrectAnswers => CorrectIndices.Select(i => Options[i]);
}
