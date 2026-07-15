using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Eine Tipp-Übung (Lektion) für das 10-Finger-System.
/// Enthält den anzuzeigenden Text, die Finger-Zuordnung pro Zeichen und Metadaten.
/// </summary>
public sealed class TypingLesson
{
    /// <summary>Eindeutige ID der Lektion (TypingLessonType + Varianten-Index)</summary>
    public required string Id { get; init; }

    /// <summary>Zu welcher Lektionstyp gehört diese Übung</summary>
    public required TypingLessonType LessonType { get; init; }

    /// <summary>Anzeigetitel (z.B. "Lektion 1: Grundreihe – ASDF JKL;")</summary>
    public required string Title { get; init; }

    /// <summary>Kurze Anleitung für das Kind</summary>
    public required string InstructionDe { get; init; }
    public required string InstructionTr { get; init; }

    /// <summary>Der Text, den das Kind abtippen soll (ohne Finger-Markup)</summary>
    public required string TargetText { get; init; }

    /// <summary>
    /// Finger-Zuordnung für jedes Zeichen im TargetText.
    /// Länge = TargetText.Length. Werte: LPinky, LRing, LMiddle, LIndex, RIndex, RMiddle, RRing, RPinky, Thumb (Space).
    /// Wird für virtuelle Tastatur + Finger-Highlighting verwendet.
    /// </summary>
    public required IReadOnlyList<TypingFinger> FingerMapping { get; init; }

    /// <summary>Schwierigkeitsgrad (1-3): 1=Einzelne Buchstaben, 2=Silben/Wörter, 3=Sätze</summary>
    public required int Difficulty { get; init; }

    /// <summary>Geschätzte Dauer in Sekunden (für Fortschrittsanzeige)</summary>
    public required int EstimatedDurationSeconds { get; init; }

    /// <summary>Mindest-Genauigkeit für Freischaltung der nächsten Lektion (Standard 0.85 = 85%)</summary>
    public required double MinimumAccuracy { get; init; }

    /// <summary>Mindest-Anzahl Zeichen die getippt werden müssen (Schutz gegen Abbruch)</summary>
    public required int MinimumCharacters { get; init; }
}

/// <summary>
/// Finger-Zuordnung für 10-Finger-System (deutsche Tastatur QWERTZ).
/// Wird für virtuelle Tastatur-Anzeige + Finger-Highlighting verwendet.
/// </summary>
public enum TypingFinger
{
    None = 0,            // Für Zeichen ohne Finger-Zuordnung (z.B. Leerzeichen → Thumb)
    LPinky = 1,          // Linker kleiner Finger
    LRing = 2,           // Linker Ringfinger
    LMiddle = 3,         // Linker Mittelfinger
    LIndex = 4,          // Linker Zeigefinger
    RIndex = 5,          // Rechter Zeigefinger
    RMiddle = 6,         // Rechter Mittelfinger
    RRing = 7,           // Rechter Ringfinger
    RPinky = 8,          // Rechter kleiner Finger
    Thumb = 9            // Daumen (Leertaste)
}