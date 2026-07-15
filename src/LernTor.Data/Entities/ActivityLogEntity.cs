namespace LernTor.Data.Entities;

/// <summary>Protokoll jeder bearbeiteten Aufgabe/Frage – für Eltern einsehbar im Eltern-Bereich.</summary>
public sealed class ActivityLogEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string GivenAnswer { get; set; } = string.Empty;
    public bool WasCorrect { get; set; }
}

/// <summary>Ein abgeschlossener Abschlussquiz-Versuch – für die Elternübersicht.</summary>
public sealed class QuizAttemptEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectCount { get; set; }
    public double ScorePercentage { get; set; }
    public bool Passed { get; set; }
}
