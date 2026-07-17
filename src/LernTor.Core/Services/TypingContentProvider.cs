using System.Collections.Generic;
using System.Linq;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Statischer Pool an Tipp-Lektionen für den 10-Finger-Trainer.
/// Jede Lektion hat einen Zieltext, Finger-Mapping und Metadaten.
/// Nur deutsche Sprache und Tastaturlayout (QWERTZ).
/// Lektionen sind sequentiell: Freischaltung der nächsten erst bei ≥85% Genauigkeit.
/// Die Abschluss-Lektion ist profil-spezifisch (siehe GetFinalLessonForProfile).
/// </summary>
public static class TypingContentProvider
{
    private static readonly IReadOnlyList<TypingLesson> Pool = new List<TypingLesson>
    {
        #region Lektion 1: Grundreihe (ASDF JKL;)
        new TypingLesson
        {
            Id = "grundreihe_1",
            LessonType = TypingLessonType.Grundreihe,
            Title = "Lektion 1: Grundreihe – ASDF JKL;",
            InstructionDe = "Lege deine Finger auf die Grundreihe: Links ASDF, Rechts JKL;. Tippe die Buchstaben nacheinander.",
            InstructionTr = "",
            TargetText = "asdf jkl; asdf jkl; asdf jkl;",
            FingerMapping = new List<TypingFinger>
            {
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex,  // a s d f
                TypingFinger.Thumb,                                                                    // space
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky,   // j k l ;
                TypingFinger.Thumb,                                                                    // space
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex,   // a s d f
                TypingFinger.Thumb,                                                                    // space
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky,   // j k l ;
                TypingFinger.Thumb,                                                                    // space
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex,   // a s d f
                TypingFinger.Thumb,                                                                    // space
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky,   // j k l ;
            },
            Difficulty = 1,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 20
        },
        new TypingLesson
        {
            Id = "grundreihe_2",
            LessonType = TypingLessonType.Grundreihe,
            Title = "Lektion 1b: Grundreihe – erste Wörter",
            InstructionDe = "Jetzt bilden die Grundreihe-Buchstaben erste Wörter. Weiter so!",
            InstructionTr = "",
            TargetText = "fad sad dad jag lag had fas jad sal",
            FingerMapping = new List<TypingFinger>
            {
                TypingFinger.LIndex, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // fad
                TypingFinger.LMiddle, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // sad
                TypingFinger.LMiddle, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // dad
                TypingFinger.RIndex, TypingFinger.LPinky, TypingFinger.RMiddle, TypingFinger.Thumb, // jag
                TypingFinger.LRing, TypingFinger.LPinky, TypingFinger.RMiddle, TypingFinger.Thumb, // lag
                TypingFinger.RIndex, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // had
                TypingFinger.LIndex, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // fas
                TypingFinger.RIndex, TypingFinger.LPinky, TypingFinger.LMiddle, TypingFinger.Thumb, // jad
                TypingFinger.LMiddle, TypingFinger.LPinky, TypingFinger.LRing,                    // sal
            },
            Difficulty = 1,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 20
        },
        new TypingLesson
        {
            Id = "grundreihe_3",
            LessonType = TypingLessonType.Grundreihe,
            Title = "Lektion 1c: Grundreihe – Kombinationen",
            InstructionDe = "Mische die linke und rechte Hand. Konzentriere dich auf die richtigen Finger!",
            InstructionTr = "",
            TargetText = "asdf jkl; sad fad jag lag had fas jad sal das glad",
            FingerMapping = BuildFingerMapping("asdf jkl; sad fad jag lag had fas jad sal das glad"),
            Difficulty = 1,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 30
        },
        #endregion

        #region Lektion 2: Oberreihe (QWERTZUIOPÜ+)
        new TypingLesson
        {
            Id = "oberreihe_1",
            LessonType = TypingLessonType.Oberreihe,
            Title = "Lektion 2: Oberreihe – QWERTZUIOPÜ+",
            InstructionDe = "Strecke deine Zeige- und Mittelfinger nach oben: QWERT (links) ZUIOPÜ+ (rechts).",
            InstructionTr = "",
            TargetText = "qwert zuiopü+ qwert zuiopü+ qwert zuiopü+",
            FingerMapping = new List<TypingFinger>
            {
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex, // q w e r t
                TypingFinger.Thumb,                                                                                        // space
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky, TypingFinger.RPinky, // z u i o p ü +
                TypingFinger.Thumb,
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky, TypingFinger.RPinky,
                TypingFinger.Thumb,
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky, TypingFinger.RPinky,
            },
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 30
        },
        new TypingLesson
        {
            Id = "oberreihe_2",
            LessonType = TypingLessonType.Oberreihe,
            Title = "Lektion 2b: Oberreihe – erste Wörter",
            InstructionDe = "Bilde Wörter mit der Oberreihe. Deine Finger lernen den Weg nach oben!",
            InstructionTr = "",
            TargetText = "quer zitronen puppe tropfen gleich quietsch wasser pneu roboter",
            FingerMapping = BuildFingerMapping("quer zitronen puppe tropfen gleich quietsch wasser pneu roboter"),
            Difficulty = 2,
            EstimatedDurationSeconds = 80,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 40
        },
        #endregion

        #region Lektion 3: Unterreihe (YXCVBNM,.-)
        new TypingLesson
        {
            Id = "unterreihe_1",
            LessonType = TypingLessonType.Unterreihe,
            Title = "Lektion 3: Unterreihe – YXCVBNM,.-",
            InstructionDe = "Bewege Zeige- und Mittelfinger nach unten: YXCVB (links) NM,.- (rechts).",
            InstructionTr = "",
            TargetText = "yxcvb nm,.- yxcvb nm,.- yxcvb nm,.-",
            FingerMapping = new List<TypingFinger>
            {
                TypingFinger.RIndex, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex, // y x c v b
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RPinky, TypingFinger.RPinky, // n m , . -
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RPinky, TypingFinger.RPinky,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RPinky, TypingFinger.RPinky,
            },
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 30
        },
        new TypingLesson
        {
            Id = "unterreihe_2",
            LessonType = TypingLessonType.Unterreihe,
            Title = "Lektion 3b: Unterreihe – Wörter",
            InstructionDe = "Wörter mit der Unterreihe. Deine Finger finden den Weg nach unten!",
            InstructionTr = "",
            TargetText = "baby nymphen myrthen xaver cvjm byron zymatisch",
            FingerMapping = BuildFingerMapping("baby nymphen myrthen xaver cvjm byron zymatisch"),
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 30
        },
        #endregion

        #region Lektion 4: Zahlenreihe (1234567890)
        new TypingLesson
        {
            Id = "zahlenreihe_1",
            LessonType = TypingLessonType.Zahlenreihe,
            Title = "Lektion 4: Zahlenreihe – 1234567890",
            InstructionDe = "Strecke alle Finger nach oben zur Zahlenreihe. Jeder Finger hat seine Zahl!",
            InstructionTr = "",
            TargetText = "12345 67890 12345 67890 12345 67890",
            FingerMapping = new List<TypingFinger>
            {
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex, // 1 2 3 4 5
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky, // 6 7 8 9 0
                TypingFinger.Thumb,
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky,
                TypingFinger.Thumb,
                TypingFinger.LPinky, TypingFinger.LRing, TypingFinger.LMiddle, TypingFinger.LIndex, TypingFinger.LIndex,
                TypingFinger.Thumb,
                TypingFinger.RIndex, TypingFinger.RIndex, TypingFinger.RMiddle, TypingFinger.RRing, TypingFinger.RPinky,
            },
            Difficulty = 3,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 25
        },
        new TypingLesson
        {
            Id = "zahlenreihe_2",
            LessonType = TypingLessonType.Zahlenreihe,
            Title = "Lektion 4b: Zahlen – Kombinationen",
            InstructionDe = "Mische Zahlen und Buchstaben. Telefonnummern, Postleitzahlen, Datumsangaben!",
            InstructionTr = "",
            TargetText = "12345 67890 0176 54321 101112 131415 161718 192021",
            FingerMapping = BuildFingerMapping("12345 67890 0176 54321 101112 131415 161718 192021"),
            Difficulty = 3,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 30
        },
        #endregion

        #region Lektion 5: Wörter & Silben (nur DE)
        new TypingLesson
        {
            Id = "woerter_1",
            LessonType = TypingLessonType.WoerterSilben,
            Title = "Lektion 5: Häufige Wörter & Silben",
            InstructionDe = "Tippe die häufigsten deutschen Wörter. Silbe für Silbe – flüssig und genau!",
            InstructionTr = "",
            TargetText = "der die und nicht den ein ich mir mit auf so ist das mich dich was wir sind hab sie es an",
            FingerMapping = BuildFingerMapping("der die und nicht den ein ich mir mit auf so ist das mich dich was wir sind hab sie es an"),
            Difficulty = 2,
            EstimatedDurationSeconds = 80,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 40
        },
        #endregion

        #region Lektion 6: Einfache Sätze
        new TypingLesson
        {
            Id = "saetze_1",
            LessonType = TypingLessonType.Saetze,
            Title = "Lektion 6: Einfache Sätze",
            InstructionDe = "Tippe komplette Sätze mit Satzzeichen. Groß- und Kleinschreibung beachten!",
            InstructionTr = "",
            TargetText = "Ich lerne tippen. Das macht Spaß. Meine Finger sind schnell. Ich übe jeden Tag.",
            FingerMapping = BuildFingerMapping("Ich lerne tippen. Das macht Spaß. Meine Finger sind schnell. Ich übe jeden Tag."),
            Difficulty = 3,
            EstimatedDurationSeconds = 90,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 60
        },
        new TypingLesson
        {
            Id = "saetze_2",
            LessonType = TypingLessonType.Saetze,
            Title = "Lektion 6b: Sätze mit Zahlen & Zeichen",
            InstructionDe = "Sätze mit Zahlen und Sonderzeichen. Alles zusammen – du schaffst das!",
            InstructionTr = "",
            TargetText = "Heute ist der 12.05.2024. Ich bin 12 Jahre alt. Meine Telefonnummer: 0176-54321.",
            FingerMapping = BuildFingerMapping("Heute ist der 12.05.2024. Ich bin 12 Jahre alt. Meine Telefonnummer: 0176-54321."),
            Difficulty = 3,
            EstimatedDurationSeconds = 90,
            MinimumAccuracy = 0.85,
            MinimumCharacters = 60
        },
        #endregion
    };

    // Profil-spezifische Abschluss-Lektionen (werden nicht im Pool geführt, sondern auf Abruf erstellt)
    private static TypingLesson CreateEmirhanFinalLesson() => new TypingLesson
    {
        Id = "abschluss_emirhan",
        LessonType = TypingLessonType.Abschluss,
        Title = "Lektion 7: Abschluss – Emirhans Text",
        InstructionDe = "Der finale Test! Tippe deinen persönlichen Text fehlerfrei und flüssig.",
        InstructionTr = "",
        TargetText = "Hallo, mein Name ist Emirhan Kahraman. Ich wurde am 09.05.2014 geboren. Ich wohne in der Weichselstraße 41 in 12045 Berlin. Meine Telefonnummer ist 0173 2085640. Mein Vater heißt Orhan Kahraman und meine Mutter heißt Zehra Kahraman. Ich gehe auf die Lemgo-Grundschule und bin in der Klasse 6c. Mein Lieblingsessen ist Nudeln mit Sahnesoße und Schnitzel. In meiner Freizeit spiele ich gerne Fußball und treffe mich mit meinen Freunden. Ich finde es wichtig, schnell tippen zu lernen, weil ich oft am Computer arbeite. Heute übe ich das Zehn-Finger-System und möchte immer besser werden. Am Ende der Stunde werde ich mir mein Ergebnis ansehen und mich freuen, wenn ich schneller geworden bin.",
        FingerMapping = BuildFingerMapping("Hallo, mein Name ist Emirhan Kahraman. Ich wurde am 09.05.2014 geboren. Ich wohne in der Weichselstraße 41 in 12045 Berlin. Meine Telefonnummer ist 0173 2085640. Mein Vater heißt Orhan Kahraman und meine Mutter heißt Zehra Kahraman. Ich gehe auf die Lemgo-Grundschule und bin in der Klasse 6c. Mein Lieblingsessen ist Nudeln mit Sahnesoße und Schnitzel. In meiner Freizeit spiele ich gerne Fußball und treffe mich mit meinen Freunden. Ich finde es wichtig, schnell tippen zu lernen, weil ich oft am Computer arbeite. Heute übe ich das Zehn-Finger-System und möchte immer besser werden. Am Ende der Stunde werde ich mir mein Ergebnis ansehen und mich freuen, wenn ich schneller geworden bin."),
        Difficulty = 3,
        EstimatedDurationSeconds = 180,
        MinimumAccuracy = 0.85,
        MinimumCharacters = 100
    };

    private static TypingLesson CreateBatuhanFinalLesson() => new TypingLesson
    {
        Id = "abschluss_batuhan",
        LessonType = TypingLessonType.Abschluss,
        Title = "Lektion 7: Abschluss – Batuhans Text",
        InstructionDe = "Der finale Test! Tippe deinen persönlichen Text fehlerfrei und flüssig.",
        InstructionTr = "",
        TargetText = "Hallo, mein Name ist Batuhan Kahraman. Ich wurde am 16.08.2011 geboren. Ich wohne in der Weichselstraße 41 in 12045 Berlin. Meine Telefonnummer ist 01522 8467854. Mein Vater heißt Orhan Kahraman und meine Mutter heißt Zehra Kahraman. Ich gehe auf das Robert-Koch-Gymnasium und bin in der Klasse 9a. Mein Lieblingsessen ist Pommes und Nuggets. In meiner Freizeit spiele ich gerne Fußball und treffe mich mit meinen Freunden. Ich finde es wichtig, schnell tippen zu lernen, weil ich oft am Computer arbeite. Heute übe ich das Zehn-Finger-System und möchte immer besser werden. Am Ende der Stunde werde ich mir mein Ergebnis ansehen und mich freuen, wenn ich schneller geworden bin.",
        FingerMapping = BuildFingerMapping("Hallo, mein Name ist Batuhan Kahraman. Ich wurde am 16.08.2011 geboren. Ich wohne in der Weichselstraße 41 in 12045 Berlin. Meine Telefonnummer ist 01522 8467854. Mein Vater heißt Orhan Kahraman und meine Mutter heißt Zehra Kahraman. Ich gehe auf das Robert-Koch-Gymnasium und bin in der Klasse 9a. Mein Lieblingsessen ist Pommes und Nuggets. In meiner Freizeit spiele ich gerne Fußball und treffe mich mit meinen Freunden. Ich finde es wichtig, schnell tippen zu lernen, weil ich oft am Computer arbeite. Heute übe ich das Zehn-Finger-System und möchte immer besser werden. Am Ende der Stunde werde ich mir mein Ergebnis ansehen und mich freuen, wenn ich schneller geworden bin."),
        Difficulty = 3,
        EstimatedDurationSeconds = 180,
        MinimumAccuracy = 0.85,
        MinimumCharacters = 100
    };

    public static IReadOnlyList<TypingLesson> GetAllLessons()
    {
        return Pool;
    }

    public static TypingLesson GetLessonById(string id)
    {
        // Erst im Pool suchen
        var lesson = Pool.FirstOrDefault(l => l.Id == id);
        if (lesson != null) return lesson;

        // Profil-spezifische Abschluss-Lektionen
        return id switch
        {
            "abschluss_emirhan" => CreateEmirhanFinalLesson(),
            "abschluss_batuhan" => CreateBatuhanFinalLesson(),
            _ => null!
        };
    }

    public static IReadOnlyList<TypingLesson> GetLessonsByType(TypingLessonType type)
    {
        return Pool.Where(l => l.LessonType == type).ToList();
    }

    /// <summary>
    /// Ermittelt die nächste Lektion in der Sequenz.
    /// Für die Abschluss-Lektion wird der Profil-Name benötigt.
    /// </summary>
    public static TypingLesson? GetNextLesson(string currentLessonId, string? profileName = null)
    {
        var current = Pool.FirstOrDefault(l => l.Id == currentLessonId);
        if (current == null) return Pool.FirstOrDefault();

        var sameTypeLessons = Pool.Where(l => l.LessonType == current.LessonType).OrderBy(l => l.Id).ToList();
        var currentIndex = sameTypeLessons.FindIndex(l => l.Id == currentLessonId);
        if (currentIndex >= 0 && currentIndex + 1 < sameTypeLessons.Count)
        {
            return sameTypeLessons[currentIndex + 1];
        }

        // Nächster Typ
        var nextType = (TypingLessonType)(((int)current.LessonType) + 1);
        var nextLesson = Pool.Where(l => l.LessonType == nextType).OrderBy(l => l.Id).FirstOrDefault();

        // Wenn wir nach saetze_2 kommen (letztes reguläres Level), zur profil-spezifischen Abschluss-Lektion
        if (nextLesson == null && current.LessonType == TypingLessonType.Saetze)
        {
            return GetFinalLessonForProfile(profileName);
        }

        return nextLesson;
    }

    /// <summary>
    /// Gibt die profil-spezifische Abschluss-Lektion zurück.
    /// </summary>
    public static TypingLesson GetFinalLessonForProfile(string? profileName)
    {
        return profileName?.Contains("Emirhan", StringComparison.OrdinalIgnoreCase) == true
            ? CreateEmirhanFinalLesson()
            : CreateBatuhanFinalLesson(); // Default: Batuhan
    }

    /// <summary>
    /// Ermittelt die nächste freigeschaltete Lektion basierend auf dem Fortschritt.
    /// Für die Abschluss-Lektion wird der Profil-Name benötigt.
    /// </summary>
    public static TypingLesson? GetNextUnlockedLesson(IReadOnlySet<string> completedLessonIds, string? profileName = null)
    {
        foreach (var lesson in Pool.OrderBy(l => (int)l.LessonType).ThenBy(l => l.Id))
        {
            if (!completedLessonIds.Contains(lesson.Id))
            {
                return lesson;
            }
        }
        // Alle regulären Lektionen abgeschlossen -> profil-spezifische Abschluss-Lektion
        return GetFinalLessonForProfile(profileName);
    }

    /// <summary>
    /// Erstellt das Finger-Mapping für einen gegebenen Text basierend auf standard 10-Finger-Belegung (QWERTZ).
    /// </summary>
    private static List<TypingFinger> BuildFingerMapping(string text)
    {
        var mapping = new List<TypingFinger>();
        var fingerMap = GetFingerMap();

        foreach (char c in text)
        {
            char lower = char.ToLowerInvariant(c);
            if (fingerMap.TryGetValue(lower, out var finger))
            {
                mapping.Add(finger);
            }
            else
            {
                // Für unbekannte Zeichen (Satzzeichen etc.) den passenden Finger bestimmen
                mapping.Add(GetFingerForChar(c));
            }
        }
        return mapping;
    }

    private static TypingFinger GetFingerForChar(char c)
    {
        return char.ToLowerInvariant(c) switch
        {
            ' ' => TypingFinger.Thumb,
            '\t' => TypingFinger.Thumb,
            '\n' => TypingFinger.Thumb,
            ',' => TypingFinger.RPinky,
            '.' => TypingFinger.RPinky,
            '-' => TypingFinger.RPinky,
            '+' => TypingFinger.RPinky,
            'ü' => TypingFinger.RPinky,
            'ö' => TypingFinger.RRing,
            'ä' => TypingFinger.LPinky,
            'ß' => TypingFinger.RPinky,
            '?' => TypingFinger.RPinky,
            '!' => TypingFinger.RPinky,
            ':' => TypingFinger.RPinky,
            ';' => TypingFinger.RPinky,
            '\'' => TypingFinger.RPinky,
            '"' => TypingFinger.RPinky,
            '(' => TypingFinger.LPinky,
            ')' => TypingFinger.RPinky,
            '[' => TypingFinger.LPinky,
            ']' => TypingFinger.RPinky,
            '{' => TypingFinger.LPinky,
            '}' => TypingFinger.RPinky,
            '<' => TypingFinger.RPinky,
            '>' => TypingFinger.RPinky,
            '/' => TypingFinger.RPinky,
            '\\' => TypingFinger.RPinky,
            '|' => TypingFinger.RPinky,
            '@' => TypingFinger.LIndex, // AltGr+Q -> linker Zeigefinger
            '#' => TypingFinger.RIndex, // AltGr+3 -> rechter Zeigefinger
            _ => TypingFinger.Thumb
        };
    }

    private static Dictionary<char, TypingFinger> GetFingerMap()
    {
        return new Dictionary<char, TypingFinger>
        {
            // Standard German QWERTZ layout:
            // Row 1 (numbers): 1 2 3 4 5 6 7 8 9 0 ß ´
            // Row 2 (upper):   Q W E R T Z U I O P Ü + (dead key)
            // Row 3 (home):    A S D F G H J K L Ö Ä # (Enter)
            // Row 4 (lower):   < Y X C V B N M , . - (Shift)

            // Oberreihe (Row 2 - upper)
            { 'q', TypingFinger.LPinky }, { 'w', TypingFinger.LRing }, { 'e', TypingFinger.LMiddle }, { 'r', TypingFinger.LIndex }, { 't', TypingFinger.LIndex }, { 'z', TypingFinger.LIndex },
            { 'u', TypingFinger.RIndex }, { 'i', TypingFinger.RMiddle }, { 'o', TypingFinger.RRing }, { 'p', TypingFinger.RPinky }, { 'ü', TypingFinger.RPinky }, { '+', TypingFinger.RPinky },

            // Grundreihe / Home row (Row 3)
            { 'a', TypingFinger.LPinky }, { 's', TypingFinger.LRing }, { 'd', TypingFinger.LMiddle }, { 'f', TypingFinger.LIndex }, { 'g', TypingFinger.LIndex }, { 'h', TypingFinger.RIndex },
            { 'j', TypingFinger.RIndex }, { 'k', TypingFinger.RMiddle }, { 'l', TypingFinger.RRing }, { 'ö', TypingFinger.RPinky }, { 'ä', TypingFinger.RPinky }, { '#', TypingFinger.RPinky },

            // Unterreihe / Lower row (Row 4)
            { 'y', TypingFinger.RIndex }, { 'x', TypingFinger.LRing }, { 'c', TypingFinger.LMiddle }, { 'v', TypingFinger.LIndex }, { 'b', TypingFinger.LIndex },
            { 'n', TypingFinger.RIndex }, { 'm', TypingFinger.RMiddle }, { ',', TypingFinger.RPinky }, { '.', TypingFinger.RPinky }, { '-', TypingFinger.RPinky },

            // Numbers (Row 1)
            { '1', TypingFinger.LPinky }, { '2', TypingFinger.LRing }, { '3', TypingFinger.LMiddle }, { '4', TypingFinger.LIndex }, { '5', TypingFinger.LIndex },
            { '6', TypingFinger.RIndex }, { '7', TypingFinger.RIndex }, { '8', TypingFinger.RMiddle }, { '9', TypingFinger.RRing }, { '0', TypingFinger.RPinky },

            // Space
            { ' ', TypingFinger.Thumb }
        };
    }
}