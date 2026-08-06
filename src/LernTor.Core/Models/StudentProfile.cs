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
    /// Wie viele Nachrichten der News-Abschnitt täglich bringt, von den Eltern als Preset
    /// (6/10/15/20) einstellbar.
    ///
    /// <para>Früher gab es schlicht eine Nachricht pro aktiver Quelle. Das ging auf, solange der
    /// Katalog klein war - seit er 44 Quellen umfasst, wären das 44 Abrufe beim Start und ein
    /// News-Teil, der den ganzen Vormittag füllt. Stattdessen wählt
    /// <c>RssNewsService.SelectFeedsForDay</c> täglich so viele Quellen aus, wie hier eingestellt
    /// sind, und wandert über die Tage durch den Katalog.</para>
    /// </summary>
    public int NewsArticleCount { get; set; } = DefaultNewsArticleCount;

    /// <summary>
    /// Wie streng der Jugendschutz-Filter im News-Bereich arbeitet, von den Eltern einstellbar.
    /// Vorher hing das allein am Alter - eine Entscheidung, die Eltern treffen sollten und nicht
    /// ein Geburtsdatum. Die harte Sperre gilt in jeder Stufe.
    /// </summary>
    public NewsFilterStrictness NewsFilterStrictness { get; set; } = NewsFilterStrictness.Normal;

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

    /// <summary>
    /// Optionaler eigener Zieltext der Eltern für Lektion 6 (Einfache Sätze). Leer = eingebauter
    /// Text. Länge und Bereinigung regelt <see cref="TypingTextOverrides"/>.
    /// </summary>
    public string? CustomTypingSentenceText { get; set; }

    /// <summary>
    /// Optionaler eigener Zieltext der Eltern für die Abschluss-Lektion des Tipptrainers.
    /// Leer = der eingebaute, profil-spezifische Steckbrief-Text.
    /// </summary>
    public string? CustomTypingFinalText { get; set; }

    /// <summary>
    /// Wochenziel in Lerntagen (0 = aus, Standard). Reine Anzeige ohne Druckmechanik - siehe
    /// WeeklyGoalCalculator. Pro Profil, weil ein Zehnjähriger und ein Fünfzehnjähriger
    /// unterschiedlich viel schaffen.
    /// </summary>
    public int WeeklyGoalDays { get; set; }

    /// <summary>
    /// Angehefteter Lesetext (siehe <c>ReadingPiece.Key</c>): steht Tag für Tag als erster Text
    /// im Vorlese-Bereich, bis die Eltern ihn wieder lösen. Pro Profil, weil auch die eigenen
    /// Texte pro Profil hinterlegt werden.
    /// </summary>
    public string? PinnedReadingTextKey { get; set; }

    /// <summary>Die bereinigten Text-Überschreibungen dieses Profils für den Tipptrainer.</summary>
    public TypingTextOverrides TypingTextOverrides =>
        TypingTextOverrides.From(CustomTypingSentenceText, CustomTypingFinalText);

    // --- Führerschein-Bereich (pro Profil, siehe TrafficSignCatalog) ---

    /// <summary>
    /// Ob der Führerschein-Bereich für dieses Kind überhaupt auftaucht. Pro Profil, weil ein
    /// Elfjähriger und ein Fünfzehnjähriger hier unterschiedlich weit sind - der globale
    /// Fächer-Schalter (<c>AppSettings.DisabledSubjects</c>) gilt dagegen für beide Kinder
    /// zugleich. Aus heißt aus, egal welcher der beiden Schalter es sagt.
    /// </summary>
    public bool DrivingAreaEnabled { get; set; } = true;

    /// <summary>
    /// Ob der Erste-Hilfe-Bereich für dieses Kind auftaucht. Wie beim Führerschein pro Profil,
    /// weil er den Tag um eine Etappe verlängert - Eltern sollen das je Kind entscheiden können.
    /// In der Datenbank steht er INVERTIERT (<c>StudentProfileEntity.ErsteHilfeDisabled</c>),
    /// damit vorhandene Profile ihn eingeschaltet bekommen.
    /// </summary>
    public bool ErsteHilfeEnabled { get; set; } = true;

    /// <summary>
    /// Zeichen in der täglichen Challenge, von den Eltern als Preset (3/5/8/10) einstellbar.
    /// Wer gar keine Challenge will, schaltet den Bereich über <see cref="DrivingAreaEnabled"/>
    /// ab - eine Challenge mit null Zeichen wäre nur ein leerer Bildschirm.
    /// </summary>
    public int DrivingChallengeSignCount { get; set; } = DailySignChallengeDefaultCount;

    /// <summary>
    /// Zeichengruppen, die für dieses Kind ausgeblendet sind. Leer = alle fünf Gruppen. Als
    /// String persistiert (<c>JsonOptions.Default</c>), damit ein späteres Umsortieren des Enums
    /// keine gespeicherte Auswahl umdeutet.
    /// </summary>
    public HashSet<TrafficSignCategory> DisabledSignCategories { get; set; } = new();

    public const int DefaultReadingMinutes = 5;

    /// <summary>Fünf Zeichen am Tag - eine Minute, die auch an einem vollen Schultag drin ist.</summary>
    public const int DailySignChallengeDefaultCount = 5;
    public const int DefaultNewsSecondsPerArticle = 10;

    /// <summary>Zwölf Nachrichten am Tag - genug für eine echte Auswahl über drei Sprachen,
    /// ohne dass der News-Teil länger dauert als alle Fächer zusammen.</summary>
    public const int DefaultNewsArticleCount = 12;
    public const int DefaultExerciseSecondsPerQuestion = 5;
    public const int DefaultExercisesPerSubject = 6;
    public const int DefaultQuizQuestionCount = 20;
    public const int DefaultQuizRetryQuestionCount = 15;

    public const string DefaultAvatar = "🧒";

    public static string NewId() => Guid.NewGuid().ToString("N");
}
