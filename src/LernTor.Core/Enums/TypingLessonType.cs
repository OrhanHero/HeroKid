namespace LernTor.Core.Enums;

/// <summary>
/// Lektionstypen des 10-Finger-Tipp-Trainers in streng sequentieller Reihenfolge.
/// Freischaltung der nächsten Lektion erst bei ≥85% Genauigkeit in der aktuellen Lektion.
/// </summary>
public enum TypingLessonType
{
    /// <summary>Grundreihe: ASDF JKL; + erste Wörter</summary>
    Grundreihe = 1,

    /// <summary>Oberreihe: QWERTZUIOPÜ+ + Wörter</summary>
    Oberreihe = 2,

    /// <summary>Unterreihe: YXCVBNM,.- + Wörter</summary>
    Unterreihe = 3,

    /// <summary>Zahlenreihe: 1234567890 + Kombinationen</summary>
    Zahlenreihe = 4,

    /// <summary>Häufige Wörter & Silben (DE + TR)</summary>
    WoerterSilben = 5,

    /// <summary>Einfache Sätze mit Satzzeichen</summary>
    Saetze = 6,

    /// <summary>Abschluss-Text: Kombiniert alle Reihen</summary>
    Abschluss = 7
}