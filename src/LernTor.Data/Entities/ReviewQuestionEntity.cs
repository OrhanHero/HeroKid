namespace LernTor.Data.Entities;

/// <summary>
/// Ein Eintrag der "Fehler-Kartei" (vereinfachtes Spaced-Repetition): eine im Übungsteil falsch
/// beantwortete Aufgabe, die an FOLGETAGEN im selben Fach erneut gestellt wird, bis sie zweimal
/// in Folge richtig beantwortet wurde. Die komplette Aufgabe wird als Schnappschuss gespeichert,
/// weil generierte Aufgaben (zufällige Zahlen, rotierende Pools) am nächsten Tag nicht
/// reproduzierbar wären. Enums (Fach, Klassenstufe, Fragetyp) als Strings - nie numerisch
/// persistieren (siehe JsonOptions.Default-Regel).
/// </summary>
public sealed class ReviewQuestionEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Frage-Id aus <c>QuizQuestion.Id</c> - identifiziert die Aufgabe beim Upsert.</summary>
    public string QuestionId { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string OptionsJson { get; set; } = "[]";
    public string CorrectAnswersJson { get; set; } = "[]";
    public string Explanation { get; set; } = string.Empty;
    public string? HelpHint { get; set; }
    public string? ImageUrl { get; set; }
    public bool RequiresTurkishCharacters { get; set; }

    /// <summary>Wie oft insgesamt falsch beantwortet - sortiert die fälligsten zuerst.</summary>
    public int WrongCount { get; set; }

    /// <summary>Richtige Antworten in Folge; bei 2 wird der Eintrag gelöscht (gelernt).</summary>
    public int CorrectStreak { get; set; }

    /// <summary>Letzte Beantwortung - am selben Tag wird nicht erneut gefragt (der Lerneffekt
    /// entsteht durch den Abstand, nicht durch sofortiges Nachklicken der eben gezeigten Lösung).</summary>
    public DateTimeOffset LastAnsweredAt { get; set; }
}
