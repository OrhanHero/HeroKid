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
}
