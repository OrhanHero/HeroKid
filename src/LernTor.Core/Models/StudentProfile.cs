using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Ein Kind-Profil. Mehrere Kinder am selben PC können so getrennt ihren eigenen Fortschritt
/// und ihre eigene Klassenstufe haben, statt sich eine globale Einstellung zu teilen.
/// </summary>
public sealed class StudentProfile
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public int? Age { get; set; }

    /// <summary>Freitext, z.B. "9a" oder "6c" - rein informativ, für die Aufgaben zählt GradeLevel.</summary>
    public string? ClassLabel { get; set; }

    public required GradeLevel GradeLevel { get; set; }

    /// <summary>Vom Kind gewähltes Avatar-Emoji für die Profil-Kachel (bewusst Emoji statt Bilddateien:
    /// keine Assets zu pflegen, rendert auf jedem Windows nativ, kulturneutral wählbar).</summary>
    public string AvatarEmoji { get; set; } = DefaultAvatar;

    /// <summary>Über alle Lerntage gesammelte Belohnungs-Sterne (Gamification, siehe StudentProgress.EarnedStarsToday).</summary>
    public int TotalStars { get; set; }

    /// <summary>
    /// Mindest-Genauigkeit (0.0-1.0) zum Bestehen einer Tipp-Lektion für dieses Profil, von den
    /// Eltern im Eltern-Bereich als Preset (25/50/75/85%) einstellbar - siehe
    /// TypingExerciseService.CheckInput. Default 25% ist bewusst niedrig (Kinder tippen anfangs auf
    /// einer ihnen ungewohnten Tastatur).
    /// </summary>
    public double TypingMinAccuracy { get; set; } = 0.25;

    /// <summary>
    /// Mindest-Trefferquote (0.0-1.0) des ERSTEN Abschlussquiz-Versuchs am Tag, ab der der PC
    /// freigeschaltet wird, von den Eltern als Preset (50/75/85%) einstellbar - siehe
    /// QuizResult.PassThreshold, ProgressGateService.ApplyQuizResult.
    /// </summary>
    public double QuizFirstAttemptThreshold { get; set; } = 0.5;

    /// <summary>
    /// Mindest-Trefferquote (0.0-1.0) des ZWEITEN Abschlussquiz-Versuchs (Wiederholung nach
    /// Nichtbestehen des ersten Versuchs) - von den Eltern als Preset (25/50%) einstellbar. Anders
    /// als früher schaltet der zweite Versuch nicht mehr unabhängig vom Ergebnis frei.
    /// </summary>
    public double QuizRetryThreshold { get; set; } = 0.25;

    /// <summary>
    /// Pflicht-Lesezeit des täglichen Vorlese-Abschnitts in Minuten, von den Eltern als Preset
    /// (2/5/8/10 min) einstellbar - erst nach Ablauf wird "Weiter" nutzbar (ReadingViewModel).
    /// </summary>
    public int ReadingMinutes { get; set; } = DefaultReadingMinutes;

    /// <summary>
    /// Mindest-Lesezeit pro News-Artikel in Sekunden, von den Eltern als Preset (5/10/20/30 s)
    /// einstellbar - solange sie läuft, bleibt die Antwort-Eingabe gesperrt (NewsViewModel).
    /// </summary>
    public int NewsSecondsPerArticle { get; set; } = DefaultNewsSecondsPerArticle;

    /// <summary>
    /// Mindestzeit pro Übungsaufgabe in den Fächern in Sekunden, von den Eltern als Preset
    /// (3/5/10/15 s) einstellbar - verhindert Durchklicken ohne Lesen (ExerciseViewModel).
    /// </summary>
    public int ExerciseSecondsPerQuestion { get; set; } = DefaultExerciseSecondsPerQuestion;

    /// <summary>
    /// Anzahl generierter Übungsaufgaben pro Fach und Tag, von den Eltern als Preset (4/6/8/10)
    /// einstellbar - steuert zusammen mit den aktivierten Fächern die Länge der Tagessession.
    /// </summary>
    public int ExercisesPerSubject { get; set; } = DefaultExercisesPerSubject;

    /// <summary>
    /// Zielgröße des ersten Abschlussquiz am Tag, von den Eltern als Preset (10/15/20/25)
    /// einstellbar - siehe QuizComposer.ComposeFinalQuiz (verteilt über alle aktiven Fächer).
    /// </summary>
    public int QuizQuestionCount { get; set; } = DefaultQuizQuestionCount;

    /// <summary>
    /// Zielgröße des Wiederholungs-Quiz (zweiter Versuch nach Nichtbestehen), von den Eltern als
    /// Preset (10/15/20) einstellbar - gewichtet auf die schwachen Fächer des ersten Versuchs.
    /// </summary>
    public int QuizRetryQuestionCount { get; set; } = DefaultQuizRetryQuestionCount;

    public const int DefaultReadingMinutes = 5;
    public const int DefaultNewsSecondsPerArticle = 10;
    public const int DefaultExerciseSecondsPerQuestion = 5;
    public const int DefaultExercisesPerSubject = 6;
    public const int DefaultQuizQuestionCount = 20;
    public const int DefaultQuizRetryQuestionCount = 15;

    public const string DefaultAvatar = "🧒";

    public static string NewId() => Guid.NewGuid().ToString("N");
}
