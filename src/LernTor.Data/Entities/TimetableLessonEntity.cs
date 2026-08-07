namespace LernTor.Data.Entities;

/// <summary>
/// Eine Schulstunde im Stundenplan eines Profils (siehe Core.Models.TimetableLesson).
///
/// <para><c>Day</c> steht als String ("Monday") in der Datenbank, nicht als Zahl - dasselbe
/// Muster wie bei <c>Subject</c>: eine umsortierte Aufzählung soll bestehende Zeilen nicht
/// stillschweigend auf einen anderen Wochentag umdeuten (siehe CLAUDE.md).</para>
///
/// <para>Das Fach ist freier Text, kein Fach-Kürzel aus dem <c>Subject</c>-Enum: auf einem
/// echten Stundenplan stehen NaWi, Sport, WPU Spanisch oder Klassenrat, und die kennt LernTor
/// als Lernbereich nicht.</para>
/// </summary>
public sealed class TimetableLessonEntity
{
    public string Id { get; set; } = string.Empty;

    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Wochentag als Name des <see cref="System.DayOfWeek"/>-Werts, z.B. "Monday".</summary>
    public string Day { get; set; } = string.Empty;

    /// <summary>Stunde, 1-basiert wie auf dem Plan.</summary>
    public int Period { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string? Teacher { get; set; }

    public string? Room { get; set; }
}
