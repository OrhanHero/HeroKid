namespace LernTor.Core.Enums;

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    OpenText,

    /// <summary>
    /// Diktat: der Satz wird VORGELESEN statt angezeigt, das Kind tippt ihn. Ausgewertet wird
    /// Wort für Wort (siehe <c>DictationEvaluator</c>) - Satzzeichen entscheiden bewusst nicht
    /// mit, Groß-/Kleinschreibung schon. Bewusst am Ende der Aufzählung ergänzt: neue Werte
    /// dazwischen würden bereits gespeicherte Fragen umdeuten.
    /// </summary>
    Diktat
}
