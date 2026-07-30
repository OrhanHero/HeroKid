using System.Collections.Generic;
using System.Linq;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Statischer Pool an Tipp-Lektionen für den 10-Finger-Trainer.
/// Jede Lektion hat einen Zieltext, Finger-Mapping und Metadaten.
/// Nur deutsche Sprache und Tastaturlayout (QWERTZ).
/// Lektionen sind sequentiell: Freischaltung der nächsten erst bei ≥35% Genauigkeit.
/// Die Abschluss-Lektion ist profil-spezifisch (siehe GetFinalLessonForProfile).
/// </summary>
public static class TypingContentProvider
{
    private static readonly IReadOnlyList<TypingLesson> Pool = new List<TypingLesson>
    {
        #region Lektion 1: Grundreihe (ASDF JKLÖ)
        new TypingLesson
        {
            Id = "grundreihe_1",
            LessonType = TypingLessonType.Grundreihe,
            Title = "Lektion 1: Grundreihe – ASDF JKLÖ",
            InstructionDe = "Lege deine Finger auf die Grundreihe: Links ASDF, Rechts JKLÖ. Tippe die Buchstaben nacheinander.",
            InstructionTr = "",
            // QWERTZ-Grundreihe ist A S D F G H J K L Ö Ä - das Semikolon der US-Tastatur gibt es
            // hier nicht als eigene Taste (es liegt auf Umschalt+Komma).
            TargetText = "asdf jklö asdf jklö asdf jklö",
            FingerMapping = BuildFingerMapping("asdf jklö asdf jklö asdf jklö"),
            Difficulty = 1,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.35,
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
            FingerMapping = BuildFingerMapping("fad sad dad jag lag had fas jad sal"),
            Difficulty = 1,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.35,
            MinimumCharacters = 20
        },
        new TypingLesson
        {
            Id = "grundreihe_3",
            LessonType = TypingLessonType.Grundreihe,
            Title = "Lektion 1c: Grundreihe – Kombinationen",
            InstructionDe = "Mische die linke und rechte Hand. Konzentriere dich auf die richtigen Finger!",
            InstructionTr = "",
            TargetText = "asdf jklö sad fad jag lag had fas jad sal das glad",
            FingerMapping = BuildFingerMapping("asdf jklö sad fad jag lag had fas jad sal das glad"),
            Difficulty = 1,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.35,
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
            FingerMapping = BuildFingerMapping("qwert zuiopü+ qwert zuiopü+ qwert zuiopü+"),
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.35,
            MinimumCharacters = 30
        },
        new TypingLesson
        {
            Id = "oberreihe_2",
            LessonType = TypingLessonType.Oberreihe,
            Title = "Lektion 2b: Oberreihe – erste Wörter",
            InstructionDe = "Bilde Wörter mit der Oberreihe. Deine Finger lernen den Weg nach oben!",
            InstructionTr = "",
            TargetText = "karotte zitronen puppe tropfen gleich quietsch wasser pinguin roboter",
            FingerMapping = BuildFingerMapping("karotte zitronen puppe tropfen gleich quietsch wasser pinguin roboter"),
            Difficulty = 2,
            EstimatedDurationSeconds = 80,
            MinimumAccuracy = 0.35,
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
            FingerMapping = BuildFingerMapping("yxcvb nm,.- yxcvb nm,.- yxcvb nm,.-"),
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.35,
            MinimumCharacters = 30
        },
        new TypingLesson
        {
            Id = "unterreihe_2",
            LessonType = TypingLessonType.Unterreihe,
            Title = "Lektion 3b: Unterreihe – Wörter",
            InstructionDe = "Wörter mit der Unterreihe. Deine Finger finden den Weg nach unten!",
            InstructionTr = "",
            TargetText = "banane muschel vanille computer boxen mixer typisch gymnastik",
            FingerMapping = BuildFingerMapping("banane muschel vanille computer boxen mixer typisch gymnastik"),
            Difficulty = 2,
            EstimatedDurationSeconds = 70,
            MinimumAccuracy = 0.35,
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
            FingerMapping = BuildFingerMapping("12345 67890 12345 67890 12345 67890"),
            Difficulty = 3,
            EstimatedDurationSeconds = 60,
            MinimumAccuracy = 0.35,
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
            MinimumAccuracy = 0.35,
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
            MinimumAccuracy = 0.35,
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
            MinimumAccuracy = 0.35,
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
        TargetText = "Hallo, ich bin Emirhan (12) aus Berlin. Ich gehe in die 6c der Lemgo-Grundschule. Mein Lieblingsessen: Nudeln mit Sahnesoße.",
        FingerMapping = BuildFingerMapping("Hallo, ich bin Emirhan (12) aus Berlin. Ich gehe in die 6c der Lemgo-Grundschule. Mein Lieblingsessen: Nudeln mit Sahnesoße."),
        Difficulty = 3,
        EstimatedDurationSeconds = 120,
        MinimumAccuracy = 0.35,
        MinimumCharacters = 80
    };

    private static TypingLesson CreateBatuhanFinalLesson() => new TypingLesson
    {
        Id = "abschluss_batuhan",
        LessonType = TypingLessonType.Abschluss,
        Title = "Lektion 7: Abschluss – Batuhans Text",
        InstructionDe = "Der finale Test! Tippe deinen persönlichen Text fehlerfrei und flüssig.",
        InstructionTr = "",
        TargetText = "Hallo, ich bin Batuhan (15) aus Berlin. Ich besuche die 9a am Robert-Koch-Gymnasium. Lieblingsessen: Pommes und Nuggets.",
        FingerMapping = BuildFingerMapping("Hallo, ich bin Batuhan (15) aus Berlin. Ich besuche die 9a am Robert-Koch-Gymnasium. Lieblingsessen: Pommes und Nuggets."),
        Difficulty = 3,
        EstimatedDurationSeconds = 120,
        MinimumAccuracy = 0.35,
        MinimumCharacters = 80
    };

    /// <summary>Id der Lektion, deren Zieltext die Eltern über <see cref="TypingTextOverrides.SentenceText"/> ersetzen können.</summary>
    public const string SentenceLessonId = "saetze_1";

    public static IReadOnlyList<TypingLesson> GetAllLessons(TypingTextOverrides? overrides = null)
    {
        if (overrides is null || overrides.IsEmpty) return Pool;
        return Pool.Select(l => ApplyOverrides(l, overrides)).ToList();
    }

    public static TypingLesson GetLessonById(string id, TypingTextOverrides? overrides = null)
    {
        // Erst im Pool suchen
        var lesson = Pool.FirstOrDefault(l => l.Id == id);
        if (lesson != null) return ApplyOverrides(lesson, overrides);

        // Profil-spezifische Abschluss-Lektionen
        var final = id switch
        {
            "abschluss_emirhan" => CreateEmirhanFinalLesson(),
            "abschluss_batuhan" => CreateBatuhanFinalLesson(),
            _ => null
        };

        return final is null ? null! : ApplyOverrides(final, overrides);
    }

    /// <summary>
    /// Setzt einen von den Eltern hinterlegten Text in die betroffene Lektion ein. Finger-Mapping
    /// und Mindest-Zeichenzahl werden dabei aus dem neuen Text neu abgeleitet - sonst würde eine
    /// kurze eigene Übung an einer Mindest-Zeichenzahl scheitern, die für den alten Text galt.
    /// </summary>
    private static TypingLesson ApplyOverrides(TypingLesson lesson, TypingTextOverrides? overrides)
    {
        if (overrides is null || overrides.IsEmpty) return lesson;

        var replacement = lesson.LessonType == TypingLessonType.Abschluss
            ? overrides.FinalText
            : lesson.Id == SentenceLessonId ? overrides.SentenceText : null;

        if (string.IsNullOrWhiteSpace(replacement) || replacement == lesson.TargetText) return lesson;

        return new TypingLesson
        {
            Id = lesson.Id,
            LessonType = lesson.LessonType,
            Title = lesson.Title,
            InstructionDe = lesson.InstructionDe,
            InstructionTr = lesson.InstructionTr,
            TargetText = replacement,
            FingerMapping = BuildFingerMapping(replacement),
            Difficulty = lesson.Difficulty,
            EstimatedDurationSeconds = lesson.EstimatedDurationSeconds,
            MinimumAccuracy = lesson.MinimumAccuracy,
            MinimumCharacters = Math.Min(lesson.MinimumCharacters, replacement.Length)
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
    public static TypingLesson? GetNextLesson(string currentLessonId, string? profileName = null, TypingTextOverrides? overrides = null)
    {
        var current = Pool.FirstOrDefault(l => l.Id == currentLessonId);
        if (current == null) return ApplyOverrides(Pool.First(), overrides);

        var sameTypeLessons = Pool.Where(l => l.LessonType == current.LessonType).OrderBy(l => l.Id).ToList();
        var currentIndex = sameTypeLessons.FindIndex(l => l.Id == currentLessonId);
        if (currentIndex >= 0 && currentIndex + 1 < sameTypeLessons.Count)
        {
            return ApplyOverrides(sameTypeLessons[currentIndex + 1], overrides);
        }

        // Nächster Typ
        var nextType = (TypingLessonType)(((int)current.LessonType) + 1);
        var nextLesson = Pool.Where(l => l.LessonType == nextType).OrderBy(l => l.Id).FirstOrDefault();

        // Wenn wir nach saetze_2 kommen (letztes reguläres Level), zur profil-spezifischen Abschluss-Lektion
        if (nextLesson == null && current.LessonType == TypingLessonType.Saetze)
        {
            return GetFinalLessonForProfile(profileName, overrides);
        }

        return nextLesson is null ? null : ApplyOverrides(nextLesson, overrides);
    }

    /// <summary>
    /// Gibt die profil-spezifische Abschluss-Lektion zurück.
    /// </summary>
    public static TypingLesson GetFinalLessonForProfile(string? profileName, TypingTextOverrides? overrides = null)
    {
        var lesson = profileName?.Contains("Emirhan", StringComparison.OrdinalIgnoreCase) == true
            ? CreateEmirhanFinalLesson()
            : CreateBatuhanFinalLesson(); // Default: Batuhan
        return ApplyOverrides(lesson, overrides);
    }

    /// <summary>
    /// Ermittelt die nächste freigeschaltete Lektion basierend auf dem Fortschritt.
    /// Für die Abschluss-Lektion wird der Profil-Name benötigt.
    /// </summary>
    public static TypingLesson? GetNextUnlockedLesson(IReadOnlySet<string> completedLessonIds, string? profileName = null, TypingTextOverrides? overrides = null)
    {
        foreach (var lesson in Pool.OrderBy(l => (int)l.LessonType).ThenBy(l => l.Id))
        {
            if (!completedLessonIds.Contains(lesson.Id))
            {
                return ApplyOverrides(lesson, overrides);
            }
        }
        // Alle regulären Lektionen abgeschlossen -> profil-spezifische Abschluss-Lektion
        return GetFinalLessonForProfile(profileName, overrides);
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
            ',' => TypingFinger.RMiddle,
            '.' => TypingFinger.RRing,
            '-' => TypingFinger.RPinky,
            '+' => TypingFinger.RPinky,
            'ü' => TypingFinger.RPinky,
            'ö' => TypingFinger.RPinky,
            'ä' => TypingFinger.RPinky,
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
            { 'q', TypingFinger.LPinky }, { 'w', TypingFinger.LRing }, { 'e', TypingFinger.LMiddle }, { 'r', TypingFinger.LIndex }, { 't', TypingFinger.LIndex }, { 'z', TypingFinger.RIndex },
            { 'u', TypingFinger.RIndex }, { 'i', TypingFinger.RMiddle }, { 'o', TypingFinger.RRing }, { 'p', TypingFinger.RPinky }, { 'ü', TypingFinger.RPinky }, { '+', TypingFinger.RPinky },

            // Grundreihe / Home row (Row 3)
            { 'a', TypingFinger.LPinky }, { 's', TypingFinger.LRing }, { 'd', TypingFinger.LMiddle }, { 'f', TypingFinger.LIndex }, { 'g', TypingFinger.LIndex }, { 'h', TypingFinger.RIndex },
            { 'j', TypingFinger.RIndex }, { 'k', TypingFinger.RMiddle }, { 'l', TypingFinger.RRing }, { 'ö', TypingFinger.RPinky }, { 'ä', TypingFinger.RPinky }, { '#', TypingFinger.RPinky },

            // Unterreihe / Lower row (Row 4)
            // Auf QWERTZ liegt Y links unten (kleiner Finger links) und Z in der Oberreihe rechts -
            // genau umgekehrt zu QWERTY. Komma und Punkt gehören zum rechten Mittel- bzw. Ringfinger.
            { 'y', TypingFinger.LPinky }, { 'x', TypingFinger.LRing }, { 'c', TypingFinger.LMiddle }, { 'v', TypingFinger.LIndex }, { 'b', TypingFinger.LIndex },
            { 'n', TypingFinger.RIndex }, { 'm', TypingFinger.RMiddle }, { ',', TypingFinger.RMiddle }, { '.', TypingFinger.RRing }, { '-', TypingFinger.RPinky },

            // Numbers (Row 1)
            { '1', TypingFinger.LPinky }, { '2', TypingFinger.LRing }, { '3', TypingFinger.LMiddle }, { '4', TypingFinger.LIndex }, { '5', TypingFinger.LIndex },
            { '6', TypingFinger.RIndex }, { '7', TypingFinger.RIndex }, { '8', TypingFinger.RMiddle }, { '9', TypingFinger.RRing }, { '0', TypingFinger.RPinky },

            // Space
            { ' ', TypingFinger.Thumb }
        };
    }
}