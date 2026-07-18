namespace LernTor.Data.Entities;

public sealed class StudentProfileEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? Age { get; set; }
    public string? ClassLabel { get; set; }
    public int GradeLevel { get; set; }
    public string AvatarEmoji { get; set; } = "🧒";
    public int TotalStars { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public double TypingMinAccuracy { get; set; } = 0.25;
    public double QuizFirstAttemptThreshold { get; set; } = 0.5;
    public double QuizRetryThreshold { get; set; } = 0.25;

    // Timer-Einstellungen (pro Profil, siehe StudentProfile). Beim additiven Schema-Update
    // bekommen Alt-Zeilen DEFAULT 0 (SqliteSchemaUpdater) - 0/negativ wird deshalb beim
    // Mapping im Repository als "nicht gesetzt -> Standardwert" interpretiert.
    public int ReadingMinutes { get; set; } = 5;
    public int NewsSecondsPerArticle { get; set; } = 10;
    public int ExerciseSecondsPerQuestion { get; set; } = 5;
    public int ExercisesPerSubject { get; set; } = 6;
    public int QuizQuestionCount { get; set; } = 20;
    public int QuizRetryQuestionCount { get; set; } = 15;
}
