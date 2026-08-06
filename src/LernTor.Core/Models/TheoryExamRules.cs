namespace LernTor.Core.Models;

/// <summary>Ergebnis einer Prüfungssimulation.</summary>
/// <param name="Questions">Gestellte Fragen.</param>
/// <param name="WrongPoints">Summe der Fehlerpunkte.</param>
/// <param name="WrongCount">Anzahl falsch beantworteter Fragen.</param>
/// <param name="HeavyMistakes">Falsche Antworten auf 5-Punkte-Fragen.</param>
public readonly record struct TheoryExamResult(
    int Questions,
    int WrongPoints,
    int WrongCount,
    int HeavyMistakes)
{
    public int CorrectCount => Questions - WrongCount;

    /// <summary>Ob bestanden - siehe <see cref="TheoryExamRules.HasPassed"/>.</summary>
    public bool Passed => TheoryExamRules.HasPassed(WrongPoints, HeavyMistakes);

    /// <summary>Warum durchgefallen, in einem Satz. Leer, wenn bestanden.</summary>
    public string FailureReason => Passed
        ? string.Empty
        : HeavyMistakes >= TheoryExamRules.MaxHeavyMistakes
            ? $"Zwei Fragen mit {TheoryExamRules.HeavyQuestionPoints} Fehlerpunkten falsch - das allein reicht zum Durchfallen."
            : $"{WrongPoints} Fehlerpunkte, erlaubt sind höchstens {TheoryExamRules.MaxWrongPoints}.";
}

/// <summary>
/// Die Regeln der amtlichen Theorieprüfung Klasse B - so gebaut, wie sie beim TÜV wirklich läuft.
///
/// <para><b>Warum nicht einfach "80 % richtig":</b> die Prüfung zählt Fehler<i>punkte</i>, nicht
/// Fragen. Eine falsche 5-Punkte-Frage wiegt so viel wie zwei falsche 2-Punkte-Fragen plus eine
/// weitere. Und zwei falsche 5-Punkte-Fragen bedeuten Durchgefallen, selbst wenn sonst alles
/// stimmt - bei 30 Fragen also 28 richtig und trotzdem nicht bestanden. Wer mit einer
/// Prozent-Logik übt, lernt das Falsche.</para>
///
/// <para>Die Zahlen stehen in der Fahrerlaubnis-Prüfungsordnung und sind hier bewusst als
/// benannte Konstanten abgelegt statt im Code verstreut.</para>
/// </summary>
public static class TheoryExamRules
{
    /// <summary>Fragen in der Prüfung Klasse B (20 Grundstoff + 10 klassenspezifisch).</summary>
    public const int QuestionCount = 30;

    /// <summary>Mehr Fehlerpunkte als das = durchgefallen.</summary>
    public const int MaxWrongPoints = 10;

    /// <summary>Fragen dieses Gewichts sind die schweren.</summary>
    public const int HeavyQuestionPoints = 5;

    /// <summary>So viele falsche schwere Fragen führen für sich genommen zum Durchfallen.</summary>
    public const int MaxHeavyMistakes = 2;

    /// <summary>Übliches Gewicht, wenn nichts anderes angegeben ist.</summary>
    public const int DefaultPoints = 3;

    /// <summary>
    /// Bestanden ist, wer höchstens <see cref="MaxWrongPoints"/> Fehlerpunkte hat UND nicht
    /// zwei schwere Fragen verhauen hat. Beide Bedingungen zählen einzeln.
    /// </summary>
    public static bool HasPassed(int wrongPoints, int heavyMistakes) =>
        wrongPoints <= MaxWrongPoints && heavyMistakes < MaxHeavyMistakes;

    /// <summary>
    /// Wertet einen Prüfungsdurchlauf aus.
    /// </summary>
    /// <param name="questions">Die gestellten Fragen.</param>
    /// <param name="wasCorrect">Zu jeder Frage, ob sie richtig beantwortet wurde - gleiche
    /// Reihenfolge wie <paramref name="questions"/>.</param>
    public static TheoryExamResult Evaluate(
        IReadOnlyList<TheoryQuestion> questions,
        IReadOnlyList<bool> wasCorrect)
    {
        var punkte = 0;
        var falsch = 0;
        var schwer = 0;

        for (var i = 0; i < questions.Count && i < wasCorrect.Count; i++)
        {
            if (wasCorrect[i])
            {
                continue;
            }

            falsch++;
            punkte += questions[i].Points;

            if (questions[i].Points >= HeavyQuestionPoints)
            {
                schwer++;
            }
        }

        return new TheoryExamResult(questions.Count, punkte, falsch, schwer);
    }
}
