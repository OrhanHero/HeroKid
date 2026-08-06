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

    /// <summary>Anzahl der taeglichen Nachrichten (siehe StudentProfile.NewsArticleCount).</summary>
    public int NewsArticleCount { get; set; } = 12;

    /// <summary>Filterschaerfe als STRING (nicht als int) - ein spaeteres Umsortieren des Enums
    /// soll bestehende Zeilen nicht stillschweigend auf eine andere Stufe umdeuten.</summary>
    public string NewsFilterStrictness { get; set; } = "Normal";
    public int ExerciseSecondsPerQuestion { get; set; } = 5;
    public int ExercisesPerSubject { get; set; } = 6;
    public int QuizQuestionCount { get; set; } = 20;
    public int QuizRetryQuestionCount { get; set; } = 15;

    // Wochenziel in Lerntagen; 0 = aus (auch der Wert, den Alt-Zeilen beim additiven
    // Schema-Update bekommen - passt hier also ohne Sonderbehandlung).
    public int WeeklyGoalDays { get; set; }

    // Angehefteter Lesetext (ReadingPiece.Key); NULL = kein Text angeheftet.
    public string? PinnedReadingTextKey { get; set; }

    // --- Fuehrerschein-Bereich (siehe StudentProfile) ---

    /// <summary>
    /// Bewusst INVERTIERT gespeichert ("disabled" statt "enabled"): der additive Schema-Abgleich
    /// gibt neuen Spalten in bestehenden Zeilen DEFAULT 0. Bei einem Feld "DrivingAreaEnabled"
    /// waere der Bereich damit fuer alle vorhandenen Profile stillschweigend AUS gewesen -
    /// so ist 0 = "nicht abgeschaltet" = an, also der gewuenschte Standard.
    /// </summary>
    public bool DrivingAreaDisabled { get; set; }

    /// <summary>
    /// Erste-Hilfe-Bereich, aus demselben Grund invertiert wie <see cref="DrivingAreaDisabled"/>:
    /// der additive Schema-Abgleich gibt neuen Spalten DEFAULT 0, also ist 0 = "nicht
    /// abgeschaltet" = an. Bei einem Feld "ErsteHilfeEnabled" waere der Bereich fuer alle
    /// vorhandenen Profile stillschweigend AUS gewesen.
    /// </summary>
    public bool ErsteHilfeDisabled { get; set; }

    /// <summary>Zeichen in der taeglichen Challenge; 0 = Alt-Zeile ohne Wert -> Standard.</summary>
    public int DrivingChallengeSignCount { get; set; }

    /// <summary>Ausgeblendete Zeichengruppen als JSON-Array von Namen (nicht Zahlen), damit ein
    /// spaeteres Umsortieren des Enums keine gespeicherte Auswahl umdeutet.</summary>
    public string DisabledSignCategoriesJson { get; set; } = "[]";

    // Eigene Tipptrainer-Texte der Eltern (optional, NULL = eingebauter Text).
    public string? CustomTypingSentenceText { get; set; }
    public string? CustomTypingFinalText { get; set; }
}
