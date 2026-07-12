using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Deutsch-Aufgabengenerator: Grammatik, Rechtschreibung, Zeitformen, Satzglieder (Klasse 6)
/// sowie Aktiv/Passiv, Satzgefüge, Kommasetzung (Klasse 9), jeweils nach Berliner Rahmenlehrplan.
/// </summary>
public sealed class GermanGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Deutsch;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                Wortarten,
                Zeitformen,
                Satzglieder,
                GrossKleinschreibung,
                AdjektivSteigerung,
                Satzarten,
                Wortbildung,
                Balladen,
                SachtexteAuswerten,
                MedialeTexte,
                Schreibformen,
                Gespraechsformen
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                AktivPassiv,
                Konjunktionen,
                Kommasetzung,
                DassOderDas,
                Wortarten9,
                Textsorten,
                DramaAufbau,
                Figurencharakterisierung,
                Quellenkritik,
                Filmanalyse,
                RedeUndBewerbung,
                Satzbau,
                Wortbedeutung9
            }
        };

    private static readonly (string Wort, string Wortart)[] WortartenBeispiele =
    {
        ("Haus", "Nomen"), ("laufen", "Verb"), ("schnell", "Adjektiv"), ("Berlin", "Nomen"),
        ("lachen", "Verb"), ("bunt", "Adjektiv"), ("Schule", "Nomen"), ("singen", "Verb"),
        ("groß", "Adjektiv"), ("Lehrerin", "Nomen"), ("springen", "Verb"), ("freundlich", "Adjektiv"),
        ("Tisch", "Nomen"), ("essen", "Verb"), ("traurig", "Adjektiv"), ("Fenster", "Nomen"),
        ("schwimmen", "Verb"), ("leise", "Adjektiv"), ("Straße", "Nomen"), ("tanzen", "Verb")
    };

    private static QuizQuestion Wortarten(Random r)
    {
        var (wort, wortart) = WortartenBeispiele[r.Next(WortartenBeispiele.Length)];
        var alleWortarten = new[] { "Nomen", "Verb", "Adjektiv" };

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Wortarten",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Welche Wortart hat das Wort \"{wort}\"?",
            Options = alleWortarten,
            CorrectAnswers = new[] { wortart },
            Explanation = $"\"{wort}\" ist ein {wortart}. " +
                          (wortart == "Nomen" ? "Nomen (Hauptwörter) benennen Dinge, Lebewesen oder Begriffe und werden großgeschrieben." :
                           wortart == "Verb" ? "Verben (Tätigkeitswörter) beschreiben Handlungen oder Zustände." :
                           "Adjektive (Eigenschaftswörter) beschreiben, wie etwas ist."),
            HelpHint = "Nomen benennen Dinge/Lebewesen (großgeschrieben), Verben beschreiben Handlungen, Adjektive beschreiben Eigenschaften."
        };
    }

    private static readonly (string Praesens, string Praeteritum, string Perfekt, string Infinitiv)[] Verben =
    {
        ("ich gehe", "ich ging", "ich bin gegangen", "gehen"),
        ("ich spiele", "ich spielte", "ich habe gespielt", "spielen"),
        ("ich esse", "ich aß", "ich habe gegessen", "essen"),
        ("ich schreibe", "ich schrieb", "ich habe geschrieben", "schreiben"),
        ("ich fahre", "ich fuhr", "ich bin gefahren", "fahren"),
        ("ich lese", "ich las", "ich habe gelesen", "lesen"),
        ("ich sehe", "ich sah", "ich habe gesehen", "sehen"),
        ("ich komme", "ich kam", "ich bin gekommen", "kommen"),
        ("ich trinke", "ich trank", "ich habe getrunken", "trinken"),
        ("ich schlafe", "ich schlief", "ich habe geschlafen", "schlafen"),
        ("ich singe", "ich sang", "ich habe gesungen", "singen"),
        ("ich helfe", "ich half", "ich habe geholfen", "helfen"),
        ("ich nehme", "ich nahm", "ich habe genommen", "nehmen"),
        ("ich gebe", "ich gab", "ich habe gegeben", "geben"),
        ("ich finde", "ich fand", "ich habe gefunden", "finden"),
        ("ich denke", "ich dachte", "ich habe gedacht", "denken"),
        ("ich bringe", "ich brachte", "ich habe gebracht", "bringen"),
        ("ich springe", "ich sprang", "ich bin gesprungen", "springen"),
        ("ich fliege", "ich flog", "ich bin geflogen", "fliegen"),
        ("ich wasche", "ich wusch", "ich habe gewaschen", "waschen")
    };

    private static QuizQuestion Zeitformen(Random r)
    {
        var v = Verben[r.Next(Verben.Length)];
        var zeitformen = new[] { "Präsens (Gegenwart)", "Präteritum (Vergangenheit)", "Perfekt (vollendete Gegenwart)" };
        int wahl = r.Next(3);
        string frage = wahl switch
        {
            0 => $"\"{v.Praesens}\" – in welcher Zeitform steht dieser Satz?",
            1 => $"\"{v.Praeteritum}\" – in welcher Zeitform steht dieser Satz?",
            _ => $"\"{v.Perfekt}\" – in welcher Zeitform steht dieser Satz?"
        };
        string antwort = zeitformen[wahl];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Zeitformen (Tempus)",
            Type = QuestionType.MultipleChoice,
            Prompt = frage,
            Options = zeitformen,
            CorrectAnswers = new[] { antwort },
            Explanation = $"Grundform (Infinitiv) des Verbs ist \"{v.Infinitiv}\". " +
                          $"Präsens: \"{v.Praesens}\", Präteritum: \"{v.Praeteritum}\", Perfekt: \"{v.Perfekt}\".",
            HelpHint = "Präsens = Gegenwart (\"ich gehe\"), Präteritum = einfache Vergangenheit (\"ich ging\"), Perfekt = \"habe/bin\" + Partizip II (\"ich bin gegangen\")."
        };
    }

    private static readonly (string Satz, string Subjekt, string Praedikat, string Objekt)[] Saetze =
    {
        ("Der Hund jagt den Ball.", "Der Hund", "jagt", "den Ball"),
        ("Die Lehrerin erklärt die Aufgabe.", "Die Lehrerin", "erklärt", "die Aufgabe"),
        ("Mein Bruder liest ein Buch.", "Mein Bruder", "liest", "ein Buch"),
        ("Die Kinder spielen Fußball.", "Die Kinder", "spielen", "Fußball"),
        ("Der Vater repariert das Fahrrad.", "Der Vater", "repariert", "das Fahrrad"),
        ("Die Katze fängt die Maus.", "Die Katze", "fängt", "die Maus"),
        ("Der Schüler schreibt den Aufsatz.", "Der Schüler", "schreibt", "den Aufsatz"),
        ("Die Mutter kocht das Abendessen.", "Die Mutter", "kocht", "das Abendessen"),
        ("Der Junge malt ein Bild.", "Der Junge", "malt", "ein Bild"),
        ("Die Ärztin untersucht den Patienten.", "Die Ärztin", "untersucht", "den Patienten"),
        ("Der Bäcker backt das Brot.", "Der Bäcker", "backt", "das Brot"),
        ("Die Schwester füttert den Hund.", "Die Schwester", "füttert", "den Hund"),
        ("Der Gärtner pflanzt die Blume.", "Der Gärtner", "pflanzt", "die Blume"),
        ("Die Freunde besuchen das Museum.", "Die Freunde", "besuchen", "das Museum"),
        ("Der Pilot fliegt das Flugzeug.", "Der Pilot", "fliegt", "das Flugzeug"),
        ("Die Verkäuferin bedient den Kunden.", "Die Verkäuferin", "bedient", "den Kunden"),
        ("Der Polizist kontrolliert das Auto.", "Der Polizist", "kontrolliert", "das Auto"),
        ("Die Großmutter erzählt die Geschichte.", "Die Großmutter", "erzählt", "die Geschichte"),
        ("Der Trainer coacht die Mannschaft.", "Der Trainer", "coacht", "die Mannschaft"),
        ("Die Journalistin schreibt den Artikel.", "Die Journalistin", "schreibt", "den Artikel")
    };

    private static QuizQuestion Satzglieder(Random r)
    {
        var s = Saetze[r.Next(Saetze.Length)];
        var teile = new[] { "Subjekt", "Prädikat", "Objekt" };
        int wahl = r.Next(3);
        string gesuchtesTeil = teile[wahl];
        string antwort = wahl switch { 0 => s.Subjekt, 1 => s.Praedikat, _ => s.Objekt };

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Satzglieder",
            Type = QuestionType.OpenText,
            Prompt = $"Satz: \"{s.Satz}\" – Wie lautet das {gesuchtesTeil}?",
            CorrectAnswers = new[] { antwort },
            Explanation = $"Subjekt: \"{s.Subjekt}\" (wer/was handelt), " +
                          $"Prädikat: \"{s.Praedikat}\" (die Handlung/das Verb), " +
                          $"Objekt: \"{s.Objekt}\" (worauf sich die Handlung bezieht).",
            HelpHint = "Frag: Wer/was handelt? (Subjekt) Was tut es? (Prädikat/Verb) Worauf bezieht sich die Handlung? (Objekt)"
        };
    }

    private static readonly (string Satz, string Loesung, string Regel)[] GrossKlein =
    {
        ("ich gehe heute ins ___ (kino).", "Kino", "Nomen werden immer großgeschrieben."),
        ("das ist mein ___ (lieblings)buch.", "Lieblingsbuch", "Zusammengesetzte Nomen werden großgeschrieben."),
        ("wir treffen uns am ___ (montag).", "Montag", "Wochentage sind Nomen und werden großgeschrieben."),
        ("sie ist sehr ___ (fleissig).", "fleißig", "Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("er kauft ein neues ___ (fahrrad).", "Fahrrad", "Nomen werden immer großgeschrieben, auch nach einem Adjektiv wie \"neues\"."),
        ("am ___ (dienstag) haben wir sport.", "Dienstag", "Wochentage sind Nomen und werden großgeschrieben."),
        ("das ___ (auto) ist rot.", "Auto", "Nomen werden immer großgeschrieben."),
        ("sie liest ein ___ (spannendes) buch.", "spannendes", "Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("wir fahren im ___ (sommer) ans meer.", "Sommer", "Jahreszeiten sind Nomen und werden großgeschrieben."),
        ("er ist ziemlich ___ (schuechtern).", "schüchtern", "Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("das ___ (schwimmbad) hat heute geöffnet.", "Schwimmbad", "Nomen werden immer großgeschrieben."),
        ("sie spricht sehr ___ (deutlich).", "deutlich", "Adverbien/Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("im ___ (januar) ist es kalt.", "Januar", "Monatsnamen sind Nomen und werden großgeschrieben."),
        ("er hat ein neues ___ (handy) gekauft.", "Handy", "Nomen werden immer großgeschrieben."),
        ("das war wirklich ___ (mutig) von dir.", "mutig", "Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("wir gehen heute ___ (einkaufen).", "einkaufen", "Verben im Infinitiv bleiben klein, auch nach \"gehen\"."),
        ("das ___ (fussballspiel) beginnt um 15 uhr.", "Fußballspiel", "Nomen werden immer großgeschrieben."),
        ("sie ist sehr ___ (kreativ).", "kreativ", "Adjektive werden kleingeschrieben, außer am Satzanfang."),
        ("am ___ (wochenende) fahren wir weg.", "Wochenende", "Nomen werden immer großgeschrieben."),
        ("der film war ___ (langweilig).", "langweilig", "Adjektive werden kleingeschrieben, außer am Satzanfang.")
    };

    private static QuizQuestion GrossKleinschreibung(Random r)
    {
        var g = GrossKlein[r.Next(GrossKlein.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Groß- und Kleinschreibung",
            Type = QuestionType.OpenText,
            Prompt = $"Setze das Wort in Klammern richtig geschrieben ein: \"{g.Satz}\"",
            CorrectAnswers = new[] { g.Loesung },
            Explanation = $"Richtig: \"{g.Loesung}\". Regel: {g.Regel}",
            HelpHint = "Nomen (auch zusammengesetzte) und Wochentage werden großgeschrieben, Adjektive/Verben normalerweise klein."
        };
    }

    private static readonly (string Grund, string Komparativ, string Superlativ)[] Adjektive =
    {
        ("schön", "schöner", "am schönsten"),
        ("groß", "größer", "am größten"),
        ("klein", "kleiner", "am kleinsten"),
        ("gut", "besser", "am besten"),
        ("gerne", "lieber", "am liebsten"),
        ("alt", "älter", "am ältesten"),
        ("jung", "jünger", "am jüngsten"),
        ("lang", "länger", "am längsten"),
        ("kurz", "kürzer", "am kürzesten"),
        ("stark", "stärker", "am stärksten"),
        ("warm", "wärmer", "am wärmsten"),
        ("kalt", "kälter", "am kältesten"),
        ("hoch", "höher", "am höchsten"),
        ("nah", "näher", "am nächsten"),
        ("viel", "mehr", "am meisten"),
        ("dunkel", "dunkler", "am dunkelsten"),
        ("hell", "heller", "am hellsten"),
        ("schnell", "schneller", "am schnellsten"),
        ("leise", "leiser", "am leisesten"),
        ("teuer", "teurer", "am teuersten")
    };

    private static QuizQuestion AdjektivSteigerung(Random r)
    {
        var a = Adjektive[r.Next(Adjektive.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Steigerung von Adjektiven",
            Type = QuestionType.OpenText,
            Prompt = $"Wie lautet der Superlativ von \"{a.Grund}\"? (Form: \"am ___en\")",
            CorrectAnswers = new[] { a.Superlativ },
            Explanation = $"Positiv: {a.Grund}, Komparativ: {a.Komparativ}, Superlativ: {a.Superlativ}.",
            HelpHint = "Steigerungsstufen: Positiv (schön) → Komparativ (schöner) → Superlativ (am schönsten). Manche Wörter steigern unregelmäßig (gut-besser-am besten)."
        };
    }

    private static readonly (string Satz, string Satzart)[] SatzartenBeispiele =
    {
        ("Die Sonne scheint heute schön.", "Aussagesatz"),
        ("Kommst du heute mit ins Kino?", "Fragesatz"),
        ("Mach bitte das Fenster zu!", "Aufforderungssatz"),
        ("Was für ein toller Tag!", "Ausrufesatz"),
        ("Ich gehe morgen zur Schule.", "Aussagesatz"),
        ("Wie spät ist es gerade?", "Fragesatz"),
        ("Räum dein Zimmer auf!", "Aufforderungssatz"),
        ("Wie schön das aussieht!", "Ausrufesatz"),
        ("Der Zug fährt um acht Uhr ab.", "Aussagesatz"),
        ("Hast du meine Tasche gesehen?", "Fragesatz"),
        ("Setz dich bitte hin!", "Aufforderungssatz"),
        ("Das ist ja unglaublich!", "Ausrufesatz"),
        ("Meine Schwester spielt Klavier.", "Aussagesatz"),
        ("Wohin fährst du in den Ferien?", "Fragesatz"),
        ("Pass gut auf dich auf!", "Aufforderungssatz"),
        ("Wie mutig du bist!", "Ausrufesatz"),
        ("Wir essen heute Pizza zum Abendessen.", "Aussagesatz"),
        ("Warum bist du so traurig?", "Fragesatz"),
        ("Hol schnell den Ball!", "Aufforderungssatz"),
        ("Was für ein Glück wir hatten!", "Ausrufesatz")
    };

    private static QuizQuestion Satzarten(Random r)
    {
        var s = SatzartenBeispiele[r.Next(SatzartenBeispiele.Length)];
        var optionen = new[] { "Aussagesatz", "Fragesatz", "Aufforderungssatz", "Ausrufesatz" };

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Satzarten", Type = QuestionType.MultipleChoice,
            Prompt = $"Um welche Satzart handelt es sich? \"{s.Satz}\"",
            Options = optionen, CorrectAnswers = new[] { s.Satzart },
            Explanation = $"\"{s.Satz}\" ist ein {s.Satzart}.",
            HelpHint = "Aussagesatz: Punkt. Fragesatz: Fragezeichen. Aufforderungssatz: Befehl/Bitte + Ausrufezeichen. Ausrufesatz: starkes Gefühl + Ausrufezeichen."
        };
    }

    private static readonly (string Wort, string Art, string Erklaerung)[] WortbildungBeispiele =
    {
        ("Haustür", "Zusammensetzung (aus zwei Wörtern)", "\"Haustür\" setzt sich aus \"Haus\" und \"Tür\" zusammen."),
        ("Lehrerin", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"Lehrerin\" entsteht aus \"Lehrer\" durch die Nachsilbe \"-in\"."),
        ("Fenster", "Einfaches Wort (nicht zusammengesetzt)", "\"Fenster\" lässt sich nicht in kleinere bedeutungstragende Teile zerlegen."),
        ("Sonnenschein", "Zusammensetzung (aus zwei Wörtern)", "\"Sonnenschein\" setzt sich aus \"Sonne\" und \"Schein\" zusammen."),
        ("Freundschaft", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"Freundschaft\" entsteht aus \"Freund\" durch die Nachsilbe \"-schaft\"."),
        ("Tisch", "Einfaches Wort (nicht zusammengesetzt)", "\"Tisch\" ist ein einfaches, nicht zusammengesetztes Wort."),
        ("Fußballspiel", "Zusammensetzung (aus zwei Wörtern)", "\"Fußballspiel\" setzt sich aus \"Fußball\" und \"Spiel\" zusammen."),
        ("Schönheit", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"Schönheit\" entsteht aus \"schön\" durch die Nachsilbe \"-heit\"."),
        ("Baum", "Einfaches Wort (nicht zusammengesetzt)", "\"Baum\" ist ein einfaches, nicht zusammengesetztes Wort."),
        ("Kinderzimmer", "Zusammensetzung (aus zwei Wörtern)", "\"Kinderzimmer\" setzt sich aus \"Kinder\" und \"Zimmer\" zusammen."),
        ("Freiheit", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"Freiheit\" entsteht aus \"frei\" durch die Nachsilbe \"-heit\"."),
        ("Wasser", "Einfaches Wort (nicht zusammengesetzt)", "\"Wasser\" ist ein einfaches, nicht zusammengesetztes Wort."),
        ("Regenschirm", "Zusammensetzung (aus zwei Wörtern)", "\"Regenschirm\" setzt sich aus \"Regen\" und \"Schirm\" zusammen."),
        ("Kindheit", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"Kindheit\" entsteht aus \"Kind\" durch die Nachsilbe \"-heit\"."),
        ("Blume", "Einfaches Wort (nicht zusammengesetzt)", "\"Blume\" ist ein einfaches, nicht zusammengesetztes Wort."),
        ("Schulranzen", "Zusammensetzung (aus zwei Wörtern)", "\"Schulranzen\" setzt sich aus \"Schule\" und \"Ranzen\" zusammen."),
        ("unglücklich", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"unglücklich\" entsteht aus \"glücklich\" durch die Vorsilbe \"un-\"."),
        ("Katze", "Einfaches Wort (nicht zusammengesetzt)", "\"Katze\" ist ein einfaches, nicht zusammengesetztes Wort."),
        ("Handtasche", "Zusammensetzung (aus zwei Wörtern)", "\"Handtasche\" setzt sich aus \"Hand\" und \"Tasche\" zusammen."),
        ("unmöglich", "Ableitung (mit Vorsilbe/Nachsilbe)", "\"unmöglich\" entsteht aus \"möglich\" durch die Vorsilbe \"un-\".")
    };

    private static QuizQuestion Wortbildung(Random r)
    {
        var w = WortbildungBeispiele[r.Next(WortbildungBeispiele.Length)];
        var optionen = new[] { "Zusammensetzung (aus zwei Wörtern)", "Ableitung (mit Vorsilbe/Nachsilbe)", "Einfaches Wort (nicht zusammengesetzt)" };

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wortbildung", Type = QuestionType.MultipleChoice,
            Prompt = $"Wie wird das Wort \"{w.Wort}\" gebildet?",
            Options = optionen, CorrectAnswers = new[] { w.Art },
            Explanation = w.Erklaerung,
            HelpHint = "Zusammensetzung: zwei ganze Wörter verschmelzen. Ableitung: ein Wort bekommt eine Vor- oder Nachsilbe. Einfaches Wort: lässt sich nicht weiter zerlegen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BalladenListe =
    {
        ("Was ist eine Ballade?", new[] { "Ein Gedicht, das eine spannende Geschichte in Versen erzählt", "Ein reiner Sachtext ohne Handlung", "Ein Text ohne jede sprachliche Gestaltung" }, "Ein Gedicht, das eine spannende Geschichte in Versen erzählt",
            "Eine Ballade erzählt eine oft dramatische Geschichte in gebundener, gereimter oder rhythmischer Sprache."),
        ("Welche Textarten mischt eine Ballade typischerweise?", new[] { "Epik, Lyrik und Dramatik", "Nur Sachtexte", "Nur Werbetexte" }, "Epik, Lyrik und Dramatik",
            "Balladen verbinden erzählende (epische), gedichthafte (lyrische) und oft dialogische (dramatische) Elemente."),
        ("Was ist ein Refrain in einer Ballade?", new[] { "Ein wiederkehrender Vers oder eine wiederkehrende Strophe", "Der allererste Satz eines Gedichts", "Eine Fußnote am Textende" }, "Ein wiederkehrender Vers oder eine wiederkehrende Strophe",
            "Ein Refrain kehrt im Verlauf des Gedichts mehrfach wieder, oft unverändert."),
        ("Was zeichnet die Sprache vieler Balladen aus?", new[] { "Bildhafte Sprache und oft ein regelmäßiger Rhythmus/Reim", "Ausschließlich nüchterne Fachbegriffe", "Vollständiges Fehlen von Emotionen" }, "Bildhafte Sprache und oft ein regelmäßiger Rhythmus/Reim",
            "Balladen nutzen oft sprachliche Bilder, Rhythmus und Reime, um Spannung und Stimmung zu erzeugen."),
        ("Warum eignen sich Balladen gut zum lauten Vorlesen?", new[] { "Wegen ihres Rhythmus, ihrer Spannung und oft dialogischer Passagen", "Weil sie immer sehr kurz sind", "Weil sie keinerlei Handlung haben" }, "Wegen ihres Rhythmus, ihrer Spannung und oft dialogischer Passagen",
            "Rhythmus, Spannungsbogen und wörtliche Rede machen Balladen besonders gut zum Vortragen geeignet."),
        ("Was unterscheidet eine Ballade von einem reinen Sachtext?", new[] { "Die Ballade erzählt eine Geschichte in poetischer, oft dramatischer Form", "Ein Sachtext erzählt immer eine spannende Geschichte", "Es gibt keinen Unterschied" }, "Die Ballade erzählt eine Geschichte in poetischer, oft dramatischer Form",
            "Sachtexte informieren neutral, Balladen erzählen eine Geschichte in gebundener, poetischer Sprache."),
        ("Welches Thema behandeln viele klassische Balladen?", new[] { "Dramatische Ereignisse wie Kampf, Schicksal oder übernatürliche Erscheinungen", "Ausschließlich Kochrezepte", "Nur mathematische Formeln" }, "Dramatische Ereignisse wie Kampf, Schicksal oder übernatürliche Erscheinungen",
            "Klassische Balladen greifen oft dramatische, schicksalhafte oder unheimliche Themen auf."),
        ("Was ist eine Strophe in einem Gedicht/einer Ballade?", new[] { "Ein Abschnitt aus mehreren Verszeilen", "Ein einzelner Buchstabe", "Die Überschrift des Gedichts" }, "Ein Abschnitt aus mehreren Verszeilen",
            "Eine Strophe fasst mehrere Verse zu einem Abschnitt zusammen."),
        ("Was ist ein Vers?", new[] { "Eine einzelne Zeile in einem Gedicht", "Ein ganzes Buch", "Ein einzelnes Wort" }, "Eine einzelne Zeile in einem Gedicht",
            "Ein Vers ist eine einzelne Zeile eines Gedichts oder einer Ballade."),
        ("Woran erkennt man häufig einen Reim in einer Ballade?", new[] { "Am gleichen oder ähnlichen Klang der Wörter am Zeilenende", "An der Anzahl der Wörter im Satz", "An der Textlänge" }, "Am gleichen oder ähnlichen Klang der Wörter am Zeilenende",
            "Reime entstehen durch gleich oder ähnlich klingende Wortenden am Ende von Versen."),
        ("Welche Textsorte gehört neben Kurzgeschichten und Romanen zu den literarischen (fiktionalen) Texten?", new[] { "Ballade", "Gebrauchsanweisung", "Steuerbescheid" }, "Ballade",
            "Die Ballade zählt wie Kurzgeschichte und Roman zu den erzählenden, fiktionalen Texten."),
        ("Was bedeutet \"dramatische Elemente\" in einer Ballade?", new[] { "Es gibt z.B. wörtliche Rede/Dialoge wie in einem Theaterstück", "Der Text hat keinerlei Personen", "Der Text besteht nur aus Zahlen" }, "Es gibt z.B. wörtliche Rede/Dialoge wie in einem Theaterstück",
            "Dramatische Elemente zeigen sich z.B. in wörtlicher Rede zwischen Figuren, ähnlich wie in einem Theaterstück."),
        ("Warum liest man Balladen im Deutschunterricht auch laut vor?", new[] { "Um Spannung, Rhythmus und Betonung besser zu erleben", "Weil geschriebene Texte sonst nicht lesbar wären", "Weil es sonst keine Hausaufgaben gäbe" }, "Um Spannung, Rhythmus und Betonung besser zu erleben",
            "Lautes Vorlesen macht Rhythmus, Spannung und Betonung einer Ballade besonders erlebbar."),
        ("Was kennzeichnet ein Kinder- oder Jugendbuch im Vergleich zu einer Ballade?", new[] { "Es erzählt meist in Prosa (Fließtext), nicht in Versen", "Es hat immer Reime", "Es besteht nur aus einem einzigen Satz" }, "Es erzählt meist in Prosa (Fließtext), nicht in Versen",
            "Kinder- und Jugendbücher sind meist in Prosa geschrieben, Balladen dagegen in Versen."),
        ("Was ist wichtig, um den Inhalt eines Kinder- oder Jugendbuchs richtig zu verstehen?", new[] { "Auf Figuren, Handlung und die Entwicklung der Geschichte achten", "Nur die erste Seite lesen", "Nur die Kapitelüberschriften zählen" }, "Auf Figuren, Handlung und die Entwicklung der Geschichte achten",
            "Wer Figuren, Handlung und deren Entwicklung im Blick behält, versteht ein Buch besser."),
        ("Was ist eine typische Aufgabe beim Lesen eines Jugendbuchs im Unterricht?", new[] { "Die Handlung zusammenfassen und Figuren beschreiben", "Das Buch möglichst schnell weglegen", "Nur das Cover bewerten" }, "Die Handlung zusammenfassen und Figuren beschreiben",
            "Im Unterricht wird oft die Handlung zusammengefasst und über Figuren gesprochen."),
        ("Wie nennt man die Hauptfigur einer Geschichte oder Ballade?", new[] { "Protagonist/Hauptfigur", "Antagonist", "Erzähler ohne Rolle" }, "Protagonist/Hauptfigur",
            "Die Hauptfigur einer Geschichte oder Ballade wird auch Protagonist genannt."),
        ("Was ist eine Pointe am Ende einer Ballade oder Geschichte oft?", new[] { "Eine überraschende Wendung oder Erkenntnis", "Eine reine Wiederholung des Anfangs", "Ein Rechtschreibfehler" }, "Eine überraschende Wendung oder Erkenntnis",
            "Eine Pointe ist ein überraschender Schlusseffekt am Ende einer Geschichte oder Ballade."),
        ("Was ist charakteristisch für den Spannungsaufbau vieler Balladen?", new[] { "Er steigert sich bis zu einem dramatischen Höhepunkt", "Er bleibt von Anfang bis Ende völlig gleich", "Er endet immer schon im ersten Vers" }, "Er steigert sich bis zu einem dramatischen Höhepunkt",
            "Viele Balladen bauen die Spannung Strophe für Strophe auf, bis sie in einem Höhepunkt gipfelt."),
        ("Warum werden Balladen oft auch vertont (z.B. als Lied)?", new[] { "Weil ihr Rhythmus und Reim sich gut mit Musik verbinden lassen", "Weil Balladen niemals einen Rhythmus haben", "Weil Musik und Sprache nichts miteinander zu tun haben" }, "Weil ihr Rhythmus und Reim sich gut mit Musik verbinden lassen",
            "Der oft feste Rhythmus und Reim einer Ballade eignet sich gut zur musikalischen Vertonung.")
    };

    private static QuizQuestion Balladen(Random r)
    {
        var f = BalladenListe[r.Next(BalladenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Balladen und Jugendbücher", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Eine Ballade erzählt eine spannende Geschichte in Versen und mischt Epik, Lyrik und Dramatik."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SachtexteListe =
    {
        ("Was ist das Ziel eines Interviews in einer Zeitung?", new[] { "Antworten einer befragten Person zu bestimmten Fragen wiederzugeben", "Nur die eigene Meinung des Journalisten zu zeigen", "Eine erfundene Geschichte zu erzählen" }, "Antworten einer befragten Person zu bestimmten Fragen wiederzugeben",
            "Ein Interview gibt die Antworten einer befragten Person auf gezielte Fragen wieder."),
        ("Woran erkennt man ein Interview im Text?", new[] { "An Frage-Antwort-Struktur, oft mit Namen der Sprecher", "An einer fortlaufenden Erzählung ohne Dialoge", "An mathematischen Formeln" }, "An Frage-Antwort-Struktur, oft mit Namen der Sprecher",
            "Interviews sind meist als Wechsel von Frage und Antwort mit Sprechernamen aufgebaut."),
        ("Was steht typischerweise am Anfang eines Zeitungsartikels?", new[] { "Die wichtigsten Informationen (Wer, was, wann, wo)", "Immer ein Gedicht", "Nur ein Bild ohne Text" }, "Die wichtigsten Informationen (Wer, was, wann, wo)",
            "Zeitungsartikel nennen im ersten Absatz meist die wichtigsten W-Fragen."),
        ("Was zeigt eine Grafik/ein Diagramm in einem Sachtext meistens?", new[] { "Zahlen oder Daten anschaulich, z.B. als Balken oder Kreis", "Eine erfundene Geschichte", "Nur einen einzelnen Buchstaben" }, "Zahlen oder Daten anschaulich, z.B. als Balken oder Kreis",
            "Grafiken stellen Zahlen und Zusammenhänge anschaulich dar, etwa als Balken- oder Kreisdiagramm."),
        ("Warum nutzen Zeitungsartikel oft Zwischenüberschriften?", new[] { "Um den Text übersichtlich in Abschnitte zu gliedern", "Um den Text unleserlich zu machen", "Weil sie gesetzlich vorgeschrieben sind" }, "Um den Text übersichtlich in Abschnitte zu gliedern",
            "Zwischenüberschriften strukturieren lange Artikel und erleichtern die Orientierung."),
        ("Was bedeutet die Abkürzung \"W-Fragen\" beim Auswerten von Sachtexten?", new[] { "Wer, was, wann, wo, warum, wie - zentrale Fragen zum Textinhalt", "Eine Abkürzung für ein Sportspiel", "Ein anderes Wort für Kommasetzung" }, "Wer, was, wann, wo, warum, wie - zentrale Fragen zum Textinhalt",
            "Die W-Fragen helfen, die wichtigsten Informationen eines Sachtexts zu erfassen."),
        ("Wie liest man eine Balkengrafik richtig?", new[] { "Man vergleicht die Höhe/Länge der Balken mit der Skala/Achse", "Man liest nur die Farbe der Balken", "Man zählt nur die Anzahl der Balken, ohne die Werte zu beachten" }, "Man vergleicht die Höhe/Länge der Balken mit der Skala/Achse",
            "Die Werte einer Balkengrafik liest man anhand der Balkenlänge im Vergleich zur Skala ab."),
        ("Was ist eine Schlagzeile?", new[] { "Die auffällige Überschrift eines Zeitungsartikels", "Der letzte Satz eines Artikels", "Ein Bild ohne Text" }, "Die auffällige Überschrift eines Zeitungsartikels",
            "Die Schlagzeile ist die auffällige, oft zugespitzte Überschrift eines Artikels."),
        ("Warum ist es wichtig, beim Lesen eines Interviews zwischen Frage und Antwort zu unterscheiden?", new[] { "Um zu erkennen, welche Aussage von wem stammt", "Um schneller lesen zu können", "Es ist nicht wichtig" }, "Um zu erkennen, welche Aussage von wem stammt",
            "Nur wer Frage und Antwort trennt, kann Aussagen richtig der jeweiligen Person zuordnen."),
        ("Was kann man aus einer Kreisgrafik (Tortendiagramm) ablesen?", new[] { "Wie sich ein Ganzes in verschiedene Anteile aufteilt", "Nur eine einzelne Zahl ohne Zusammenhang", "Nichts, Kreisgrafiken zeigen keine Informationen" }, "Wie sich ein Ganzes in verschiedene Anteile aufteilt",
            "Ein Tortendiagramm zeigt, wie sich ein Ganzes prozentual auf verschiedene Anteile verteilt."),
        ("Was bedeutet \"Quelle\" bei einem Zeitungsartikel oder einer Grafik?", new[] { "Die Angabe, woher die Information oder die Daten stammen", "Der Name des Lesers", "Die Uhrzeit, zu der man liest" }, "Die Angabe, woher die Information oder die Daten stammen",
            "Die Quellenangabe zeigt, woher Informationen oder Daten ursprünglich stammen."),
        ("Warum sollte man beim Auswerten von Sachtexten auf das Erscheinungsdatum achten?", new[] { "Weil Informationen veralten können", "Weil das Datum keine Bedeutung hat", "Weil ältere Texte automatisch falsch sind" }, "Weil Informationen veralten können",
            "Ältere Informationen können durch neuere Entwicklungen überholt sein."),
        ("Was ist der Unterschied zwischen einer offenen und einer geschlossenen Frage in einem Interview?", new[] { "Offene Fragen erlauben freie Antworten, geschlossene Fragen meist nur Ja/Nein", "Es gibt keinen Unterschied", "Geschlossene Fragen sind immer länger" }, "Offene Fragen erlauben freie Antworten, geschlossene Fragen meist nur Ja/Nein",
            "Offene Fragen laden zu ausführlichen Antworten ein, geschlossene Fragen lassen sich meist knapp beantworten."),
        ("Was zeigt die x-Achse in einem Liniendiagramm häufig an?", new[] { "Meist eine Zeitangabe (z.B. Jahre, Monate)", "Immer den Namen des Autors", "Nichts Wichtiges" }, "Meist eine Zeitangabe (z.B. Jahre, Monate)",
            "In vielen Liniendiagrammen zeigt die waagerechte Achse eine zeitliche Entwicklung."),
        ("Wozu dient ein Untertitel unter der Schlagzeile eines Artikels?", new[] { "Er fasst kurz zusammen, worum es im Artikel geht", "Er nennt nur den Preis der Zeitung", "Er ist rein dekorativ ohne Inhalt" }, "Er fasst kurz zusammen, worum es im Artikel geht",
            "Der Untertitel gibt einen kurzen inhaltlichen Vorgeschmack auf den Artikel."),
        ("Was sollte man tun, um die Hauptaussage eines Sachtextes zu finden?", new[] { "Überschrift, Einleitung und Schlusssatz besonders genau lesen", "Nur die Bilder anschauen", "Den Text von hinten nach vorne lesen" }, "Überschrift, Einleitung und Schlusssatz besonders genau lesen",
            "Überschrift, Einleitung und Schluss enthalten oft die wichtigsten Kernaussagen."),
        ("Warum werden in Zeitungsartikeln oft Zitate von Expertinnen und Experten verwendet?", new[] { "Um Aussagen glaubwürdiger und nachprüfbarer zu machen", "Um den Text länger wirken zu lassen", "Weil Zitate gesetzlich Pflicht sind" }, "Um Aussagen glaubwürdiger und nachprüfbarer zu machen",
            "Expertenzitate stützen die Glaubwürdigkeit eines Artikels."),
        ("Was ist ein Vorteil von Grafiken gegenüber reinem Fließtext?", new[] { "Zahlen und Zusammenhänge werden auf einen Blick anschaulich", "Grafiken enthalten immer mehr Informationen als jeder Text", "Grafiken ersetzen jede Erklärung vollständig" }, "Zahlen und Zusammenhänge werden auf einen Blick anschaulich",
            "Grafiken machen Zahlen und Zusammenhänge schnell erfassbar."),
        ("Wie sollte man vorgehen, wenn man ein Interview zusammenfassen soll?", new[] { "Die wichtigsten Antworten in eigenen Worten wiedergeben", "Das ganze Interview wortwörtlich abschreiben", "Nur die Fragen ohne Antworten aufschreiben" }, "Die wichtigsten Antworten in eigenen Worten wiedergeben",
            "Eine gute Zusammenfassung gibt die wichtigsten Inhalte knapp in eigenen Worten wieder."),
        ("Was bedeutet es, eine Grafik \"kritisch zu lesen\"?", new[] { "Zu prüfen, ob die Darstellung die Daten fair und nicht irreführend zeigt", "Die Grafik grundsätzlich zu ignorieren", "Nur die schönste Farbe zu beachten" }, "Zu prüfen, ob die Darstellung die Daten fair und nicht irreführend zeigt",
            "Kritisches Lesen prüft, ob eine Grafik Daten fair oder eher irreführend darstellt.")
    };

    private static QuizQuestion SachtexteAuswerten(Random r)
    {
        var f = SachtexteListe[r.Next(SachtexteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Sach- und Gebrauchstexte auswerten", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bei Interviews auf Frage/Antwort achten, bei Zeitungsartikeln auf die W-Fragen, bei Grafiken auf Achsen/Skalen und Quelle."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedialeTexteListe =
    {
        ("Was ist ein Wiki?", new[] { "Eine Online-Enzyklopädie, die von vielen Menschen gemeinsam bearbeitet werden kann", "Ein gedrucktes Buch, das niemand verändern darf", "Ein Fernsehsender" }, "Eine Online-Enzyklopädie, die von vielen Menschen gemeinsam bearbeitet werden kann",
            "Ein Wiki wie Wikipedia wird gemeinschaftlich von vielen Nutzerinnen und Nutzern geschrieben und bearbeitet."),
        ("Was sollte man bei Informationen aus einem Wiki wie Wikipedia besonders beachten?", new[] { "Ob Quellen angegeben sind und die Informationen überprüfbar sind", "Alles ist automatisch zu 100% richtig", "Wikis dürfen nie zitiert werden" }, "Ob Quellen angegeben sind und die Informationen überprüfbar sind",
            "Da jeder Wiki-Artikel bearbeiten kann, lohnt sich ein Blick auf Quellenangaben und Überprüfbarkeit."),
        ("Was ist ein Online-Lexikon?", new[] { "Ein digitales Nachschlagewerk mit Begriffserklärungen", "Ein Videospiel", "Eine Fernsehshow" }, "Ein digitales Nachschlagewerk mit Begriffserklärungen",
            "Ein Online-Lexikon erklärt digital Begriffe und Sachverhalte, ähnlich einem gedruckten Lexikon."),
        ("Was gehört zu einer formell korrekten E-Mail?", new[] { "Eine passende Anrede, ein klarer Betreff und ein höflicher Schluss", "Nur Emojis ohne Text", "Groß- und Kleinschreibung spielt keine Rolle" }, "Eine passende Anrede, ein klarer Betreff und ein höflicher Schluss",
            "Eine formelle E-Mail braucht wie ein Brief Anrede, Betreff und höflichen Abschluss."),
        ("Wofür steht der \"Betreff\" einer E-Mail?", new[] { "Er fasst kurz zusammen, worum es in der E-Mail geht", "Er nennt das Passwort des Absenders", "Er ist immer leer" }, "Er fasst kurz zusammen, worum es in der E-Mail geht",
            "Der Betreff gibt dem Empfänger auf einen Blick den Inhalt der E-Mail an."),
        ("Was ist typisch für eine Informationssendung im Fernsehen?", new[] { "Sie berichtet sachlich über aktuelle Ereignisse", "Sie erzählt ausschließlich erfundene Geschichten", "Sie besteht nur aus Musik" }, "Sie berichtet sachlich über aktuelle Ereignisse",
            "Informationssendungen wie Nachrichten berichten sachlich über aktuelles Geschehen."),
        ("Was unterscheidet eine TV-Serie oft von einer einzelnen Sendung?", new[] { "Sie erzählt eine fortlaufende Geschichte über mehrere Folgen", "Sie hat immer nur eine einzige Folge", "Sie zeigt niemals Personen" }, "Sie erzählt eine fortlaufende Geschichte über mehrere Folgen",
            "Serien erzählen eine Geschichte, die sich über mehrere Folgen fortsetzt."),
        ("Was bedeutet \"Cliffhanger\" am Ende einer Serienfolge?", new[] { "Eine spannende, ungeklärte Situation, die zum Weiterschauen anregt", "Das endgültige Ende der ganzen Geschichte", "Ein technischer Fehler bei der Übertragung" }, "Eine spannende, ungeklärte Situation, die zum Weiterschauen anregt",
            "Ein Cliffhanger lässt eine Folge an einer spannenden, offenen Stelle enden."),
        ("Warum sollte man bei Online-Lexika wie bei Büchern auf Aktualität achten?", new[] { "Weil sich Informationen mit der Zeit ändern können", "Weil Online-Lexika sich nie ändern", "Aktualität spielt bei Online-Texten keine Rolle" }, "Weil sich Informationen mit der Zeit ändern können",
            "Auch digitale Nachschlagewerke können veraltete Informationen enthalten."),
        ("Was ist ein Vorteil von Wikis gegenüber gedruckten Lexika?", new[] { "Sie können schnell aktualisiert und ergänzt werden", "Sie sind immer fehlerfrei", "Sie können von niemandem verändert werden" }, "Sie können schnell aktualisiert und ergänzt werden",
            "Wikis lassen sich im Gegensatz zu gedruckten Büchern jederzeit aktualisieren."),
        ("Wie sollte die Anrede in einer formellen E-Mail an eine Lehrkraft lauten?", new[] { "Zum Beispiel \"Sehr geehrte Frau ...\" oder \"Sehr geehrter Herr ...\"", "Einfach \"Hey\"", "Gar keine Anrede" }, "Zum Beispiel \"Sehr geehrte Frau ...\" oder \"Sehr geehrter Herr ...\"",
            "Eine formelle Anrede zeigt Respekt gegenüber der Lehrkraft."),
        ("Was gehört an das Ende einer formellen E-Mail?", new[] { "Eine höfliche Grußformel wie \"Mit freundlichen Grüßen\"", "Nur ein Smiley", "Nichts, E-Mails enden ohne Gruß" }, "Eine höfliche Grußformel wie \"Mit freundlichen Grüßen\"",
            "Eine höfliche Grußformel rundet eine formelle E-Mail passend ab."),
        ("Warum sollte man Inhalte aus dem Internet mit mehreren Quellen vergleichen?", new[] { "Um Fehler oder einseitige Darstellungen zu erkennen", "Weil alle Internetquellen sowieso identisch sind", "Das ist nicht nötig" }, "Um Fehler oder einseitige Darstellungen zu erkennen",
            "Der Vergleich mehrerer Quellen hilft, Fehler oder Einseitigkeit zu erkennen."),
        ("Was ist der Unterschied zwischen einer E-Mail und einem klassischen Brief?", new[] { "Die E-Mail wird digital verschickt und kommt meist sehr schnell an", "Es gibt keinerlei Unterschied", "Ein Brief kommt immer schneller an als eine E-Mail" }, "Die E-Mail wird digital verschickt und kommt meist sehr schnell an",
            "E-Mails werden digital versendet und erreichen den Empfänger meist fast sofort."),
        ("Was bedeutet \"Streaming\" bei Fernsehserien?", new[] { "Inhalte werden über das Internet direkt angesehen, ohne sie vorher komplett herunterzuladen", "Ein anderes Wort für Zeitungsartikel", "Ein Begriff aus der Mathematik" }, "Inhalte werden über das Internet direkt angesehen, ohne sie vorher komplett herunterzuladen",
            "Beim Streaming werden Video- oder Audioinhalte direkt über das Internet abgespielt."),
        ("Warum sollte man bei Wikipedia-Artikeln auf die Versionsgeschichte achten können?", new[] { "Um zu sehen, wie und von wem der Artikel verändert wurde", "Weil Versionsgeschichten Pflichtlektüre in der Schule sind", "Weil sie nichts mit dem Inhalt zu tun hat" }, "Um zu sehen, wie und von wem der Artikel verändert wurde",
            "Die Versionsgeschichte zeigt Änderungen am Artikel im Zeitverlauf."),
        ("Was zeichnet eine gute Informationssendung aus?", new[] { "Sachliche, gut recherchierte und verständliche Berichterstattung", "Möglichst viele erfundene Geschichten", "Ausschließlich Werbung" }, "Sachliche, gut recherchierte und verständliche Berichterstattung",
            "Gute Informationssendungen berichten sachlich, gut recherchiert und verständlich."),
        ("Warum ist ein klarer Betreff bei E-Mails im Schulalltag wichtig (z. B. an Lehrkräfte)?", new[] { "Damit der Empfänger sofort erkennt, worum es geht", "Der Betreff hat keinerlei Bedeutung", "Ein Betreff ist nur bei Briefen nötig" }, "Damit der Empfänger sofort erkennt, worum es geht",
            "Ein aussagekräftiger Betreff hilft dem Empfänger, die E-Mail sofort einzuordnen."),
        ("Was können typische Inhalte eines Onlinelexikons sein?", new[] { "Sachliche Erklärungen zu Begriffen, Personen oder Ereignissen", "Nur private Tagebucheinträge", "Ausschließlich Werbeanzeigen" }, "Sachliche Erklärungen zu Begriffen, Personen oder Ereignissen",
            "Onlinelexika erklären Begriffe, Personen und Ereignisse sachlich."),
        ("Warum sollten Kinder und Jugendliche lernen, verschiedene mediale Textformen (Wiki, E-Mail, TV-Sendung) zu unterscheiden?", new[] { "Um Informationen in ihrem jeweiligen Kontext richtig einzuordnen und zu nutzen", "Weil alle Medienformen exakt gleich funktionieren", "Das ist für den Alltag komplett unwichtig" }, "Um Informationen in ihrem jeweiligen Kontext richtig einzuordnen und zu nutzen",
            "Verschiedene mediale Formen haben unterschiedliche Regeln und Zwecke, die man kennen sollte.")
    };

    private static QuizQuestion MedialeTexte(Random r)
    {
        var f = MedialeTexteListe[r.Next(MedialeTexteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Texte in medialer Form (Wiki, E-Mail, TV)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wikis werden gemeinschaftlich geschrieben, E-Mails brauchen Anrede/Betreff/Gruß, TV-Serien erzählen über mehrere Folgen fort."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SchreibformenListe =
    {
        ("Was ist der erste Schritt beim Verfassen eines längeren Textes?", new[] { "Einen Schreibplan mit Gliederung erstellen", "Sofort ohne Vorbereitung drauflosschreiben", "Den Text direkt auswendig lernen" }, "Einen Schreibplan mit Gliederung erstellen",
            "Ein Schreibplan ordnet die eigenen Ideen, bevor man mit dem Schreiben beginnt."),
        ("Was gehört in einen formellen Brief an eine Behörde oder Firma?", new[] { "Anschrift, Datum, Betreff, förmliche Anrede und Grußformel", "Nur ein einzelnes Wort", "Ausschließlich Emojis" }, "Anschrift, Datum, Betreff, förmliche Anrede und Grußformel",
            "Ein formeller Brief folgt festen Regeln: Anschrift, Datum, Betreff, Anrede und Gruß."),
        ("Was zeichnet eine Erzählung aus?", new[] { "Sie schildert ein (oft erfundenes) Geschehen mit Spannungsbogen", "Sie besteht nur aus einer Aufzählung von Fakten", "Sie hat niemals eine Hauptfigur" }, "Sie schildert ein (oft erfundenes) Geschehen mit Spannungsbogen",
            "Eine Erzählung schildert ein Geschehen spannend, oft mit Höhepunkt und Auflösung."),
        ("Was gehört zu einem klassischen Spannungsbogen in einer Erzählung?", new[] { "Einleitung, Hauptteil mit Höhepunkt und Schluss", "Nur ein einziger Satz", "Ausschließlich der Schluss" }, "Einleitung, Hauptteil mit Höhepunkt und Schluss",
            "Ein Spannungsbogen führt von der Einleitung über einen Höhepunkt zum Schluss."),
        ("Was ist ein Bericht (als Schreibform)?", new[] { "Eine sachliche, chronologische Darstellung eines Ereignisses", "Ein frei erfundenes Gedicht", "Eine Liste von Emojis" }, "Eine sachliche, chronologische Darstellung eines Ereignisses",
            "Ein Bericht stellt ein Ereignis sachlich und in zeitlicher Reihenfolge dar."),
        ("Was ist ein Lesetagebuch?", new[] { "Persönliche Notizen und Gedanken zu einem gelesenen Buch", "Ein Kalender ohne Bezug zu Büchern", "Eine Einkaufsliste" }, "Persönliche Notizen und Gedanken zu einem gelesenen Buch",
            "Ein Lesetagebuch hält eigene Gedanken und Eindrücke während des Lesens fest."),
        ("Was kann man in einem Lesetagebuch typischerweise festhalten?", new[] { "Eindrücke zu Figuren, spannende Stellen und eigene Meinungen zum Buch", "Nur die Seitenzahl des Buches", "Ausschließlich das Erscheinungsjahr" }, "Eindrücke zu Figuren, spannende Stellen und eigene Meinungen zum Buch",
            "Ein Lesetagebuch sammelt persönliche Eindrücke, Gedanken und Meinungen zum Gelesenen."),
        ("Was ist ein Parallelgedicht?", new[] { "Ein eigenes Gedicht, das die Struktur/Form eines Vorlagen-Gedichts übernimmt, aber eigene Inhalte hat", "Eine exakte Kopie eines fremden Gedichts ohne Veränderung", "Ein Gedicht ohne jede Struktur" }, "Ein eigenes Gedicht, das die Struktur/Form eines Vorlagen-Gedichts übernimmt, aber eigene Inhalte hat",
            "Ein Parallelgedicht übernimmt Form/Aufbau einer Vorlage, füllt sie aber mit eigenen Ideen."),
        ("Warum hilft ein Schreibplan beim Verfassen eines Aufsatzes?", new[] { "Er ordnet Ideen vorab und verhindert, wichtige Punkte zu vergessen", "Er macht das Schreiben unnötig kompliziert", "Er hat keinerlei Nutzen" }, "Er ordnet Ideen vorab und verhindert, wichtige Punkte zu vergessen",
            "Ein Schreibplan strukturiert Ideen im Voraus und schafft Übersicht beim Schreiben."),
        ("Was gehört zu einer Textgliederung?", new[] { "Eine sinnvolle Einteilung in Einleitung, Hauptteil und Schluss", "Ein zufälliges Durcheinander der Sätze", "Nur eine einzige lange Zeile ohne Absätze" }, "Eine sinnvolle Einteilung in Einleitung, Hauptteil und Schluss",
            "Eine gute Gliederung teilt einen Text sinnvoll in Einleitung, Hauptteil und Schluss ein."),
        ("Was sollte die Einleitung einer Erzählung leisten?", new[] { "Ort, Zeit und Hauptfiguren kurz vorstellen und Interesse wecken", "Bereits das gesamte Ende verraten", "Komplett leer bleiben" }, "Ort, Zeit und Hauptfiguren kurz vorstellen und Interesse wecken",
            "Die Einleitung führt kurz in Ort, Zeit und Figuren ein und weckt Neugier auf die Geschichte."),
        ("Wie sollte man einen formellen Brief beenden?", new[] { "Mit einer höflichen Grußformel wie \"Mit freundlichen Grüßen\"", "Ohne jeden Abschluss", "Mit einem Emoji anstelle von Worten" }, "Mit einer höflichen Grußformel wie \"Mit freundlichen Grüßen\"",
            "Formelle Briefe enden mit einer höflichen Grußformel."),
        ("Was unterscheidet einen Bericht von einer Erzählung?", new[] { "Ein Bericht ist sachlich und neutral, eine Erzählung darf spannend und ausgeschmückt sein", "Es gibt keinen Unterschied", "Berichte sind immer erfunden" }, "Ein Bericht ist sachlich und neutral, eine Erzählung darf spannend und ausgeschmückt sein",
            "Berichte bleiben sachlich und neutral, Erzählungen dürfen spannend und ausgeschmückt sein."),
        ("Warum ist die richtige Zeitform beim Schreiben eines Berichts wichtig?", new[] { "Berichte werden meist im Präteritum (Vergangenheit) sachlich verfasst", "Die Zeitform ist bei Berichten egal", "Berichte müssen immer im Futur geschrieben werden" }, "Berichte werden meist im Präteritum (Vergangenheit) sachlich verfasst",
            "Berichte werden meist sachlich im Präteritum verfasst, da sie über Vergangenes informieren."),
        ("Was ist beim Schreiben eines Parallelgedichts besonders wichtig?", new[] { "Die Form (z.B. Reimschema, Strophenaufbau) der Vorlage zu übernehmen", "Möglichst nichts von der Vorlage zu übernehmen", "Das Gedicht darf keine eigenen Ideen enthalten" }, "Die Form (z.B. Reimschema, Strophenaufbau) der Vorlage zu übernehmen",
            "Ein Parallelgedicht übernimmt die formalen Merkmale der Vorlage, mit eigenen Inhalten gefüllt."),
        ("Was gehört zum Hauptteil einer spannenden Erzählung?", new[] { "Die Handlung mit einem Höhepunkt (Spannungshöhepunkt)", "Nur eine Wiederholung der Einleitung", "Ausschließlich Beschreibungen ohne Handlung" }, "Die Handlung mit einem Höhepunkt (Spannungshöhepunkt)",
            "Im Hauptteil entfaltet sich die Handlung bis zu ihrem Spannungshöhepunkt."),
        ("Wofür kann ein Lesetagebuch beim Verstehen eines Buches hilfreich sein?", new[] { "Es hilft, Gedanken und Fragen zum Buch während des Lesens festzuhalten", "Es ersetzt das eigentliche Lesen des Buches komplett", "Es hat keinerlei Nutzen" }, "Es hilft, Gedanken und Fragen zum Buch während des Lesens festzuhalten",
            "Ein Lesetagebuch begleitet das Lesen und hilft, Gedanken festzuhalten."),
        ("Was ist ein Betreff in einem formellen Brief oder einer E-Mail?", new[] { "Eine kurze Angabe, worum es in dem Schreiben geht", "Der Name des Absenders", "Das Ausstellungsdatum des Personalausweises" }, "Eine kurze Angabe, worum es in dem Schreiben geht",
            "Der Betreff nennt kurz das Thema des Schreibens."),
        ("Warum sollte man vor dem Schreiben einer Erörterung oder Erzählung Stichpunkte sammeln?", new[] { "Um die eigenen Ideen zu ordnen, bevor man den ganzen Text ausformuliert", "Weil Stichpunkte den fertigen Text komplett ersetzen", "Es bringt keinerlei Vorteil" }, "Um die eigenen Ideen zu ordnen, bevor man den ganzen Text ausformuliert",
            "Stichpunkte helfen, Ideen vorab zu sammeln und zu ordnen."),
        ("Was zeichnet einen guten Schluss einer Erzählung aus?", new[] { "Er rundet die Geschichte sinnvoll ab, z.B. durch eine Auflösung der Spannung", "Er darf mitten im Satz einfach abbrechen", "Er sollte möglichst nichts mit der Geschichte zu tun haben" }, "Er rundet die Geschichte sinnvoll ab, z.B. durch eine Auflösung der Spannung",
            "Ein guter Schluss löst die aufgebaute Spannung sinnvoll auf.")
    };

    private static QuizQuestion Schreibformen(Random r)
    {
        var f = SchreibformenListe[r.Next(SchreibformenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Schreibformen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Schreibplan ordnet Ideen vorab. Bericht = sachlich, Erzählung = spannend, Lesetagebuch = persönliche Notizen, Parallelgedicht = eigene Inhalte in vorgegebener Form."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GespraechsformenListe =
    {
        ("Was ist wichtig, wenn man in einer Diskussion die eigene Meinung vertritt?", new[] { "Die Meinung mit nachvollziehbaren Argumenten begründen", "Nur laut werden, ohne Gründe zu nennen", "Andere Meinungen sofort unterbrechen" }, "Die Meinung mit nachvollziehbaren Argumenten begründen",
            "Eine überzeugende Meinung wird mit nachvollziehbaren Argumenten begründet."),
        ("Was gehört zu einer guten Gesprächsregel in einer Diskussion?", new[] { "Andere aussprechen lassen und zuhören", "Immer gleichzeitig mit allen anderen sprechen", "Nur die eigene Meinung gelten lassen" }, "Andere aussprechen lassen und zuhören",
            "Andere ausreden zu lassen und zuzuhören gehört zu fairen Gesprächsregeln."),
        ("Was sollte man vor einem Interview vorbereiten?", new[] { "Passende, klare Fragen zum Thema", "Gar nichts, man improvisiert komplett spontan", "Nur Fragen, die nichts mit dem Thema zu tun haben" }, "Passende, klare Fragen zum Thema",
            "Gute Interviews beginnen mit gut vorbereiteten, passenden Fragen."),
        ("Was ist die Aufgabe der Person, die ein Interview führt?", new[] { "Fragen stellen und aufmerksam zuhören", "Nur selbst reden, ohne Fragen zu stellen", "Die Antworten der befragten Person verändern" }, "Fragen stellen und aufmerksam zuhören",
            "Die interviewende Person stellt Fragen und hört den Antworten aufmerksam zu."),
        ("Was gehört zu einer guten Präsentation?", new[] { "Eine klare Gliederung und verständliche Sprache", "Möglichst viele unwichtige Details ohne Ordnung", "Das Vorlesen eines kompletten Buches ohne eigene Worte" }, "Eine klare Gliederung und verständliche Sprache",
            "Eine gute Präsentation ist klar gegliedert und verständlich formuliert."),
        ("Warum ist Blickkontakt beim Präsentieren vor der Klasse wichtig?", new[] { "Er zeigt Sicherheit und hält die Aufmerksamkeit der Zuhörer", "Er hat keinerlei Einfluss auf die Präsentation", "Er sollte unbedingt vermieden werden" }, "Er zeigt Sicherheit und hält die Aufmerksamkeit der Zuhörer",
            "Blickkontakt wirkt sicher und hält die Zuhörer bei der Sache."),
        ("Was können Karteikarten oder Stichpunkte bei einer Präsentation helfen?", new[] { "Sie helfen als Gedächtnisstütze, ohne den ganzen Text abzulesen", "Sie ersetzen jede Vorbereitung", "Sie dürfen niemals genutzt werden" }, "Sie helfen als Gedächtnisstütze, ohne den ganzen Text abzulesen",
            "Karteikarten dienen als kurze Gedächtnisstütze, statt den ganzen Text abzulesen."),
        ("Was sollte man tun, wenn man in einer Diskussion anderer Meinung ist als jemand anderes?", new[] { "Sachlich widersprechen und eigene Argumente nennen", "Die andere Person beleidigen", "Sofort das Gespräch abbrechen" }, "Sachlich widersprechen und eigene Argumente nennen",
            "Sachliches Widersprechen mit eigenen Argumenten ist fairer und überzeugender."),
        ("Was ist ein Vorteil von offenen Fragen in einem Interview?", new[] { "Sie ermöglichen ausführlichere, persönlichere Antworten", "Sie lassen nur \"Ja\" oder \"Nein\" als Antwort zu", "Sie sind immer schwerer zu beantworten als geschlossene Fragen" }, "Sie ermöglichen ausführlichere, persönlichere Antworten",
            "Offene Fragen laden zu ausführlicheren, persönlicheren Antworten ein."),
        ("Warum sollte man bei einer Präsentation auf eine verständliche Sprache achten?", new[] { "Damit alle Zuhörer den Inhalt gut nachvollziehen können", "Damit möglichst wenige Zuhörer etwas verstehen", "Verständlichkeit spielt keine Rolle" }, "Damit alle Zuhörer den Inhalt gut nachvollziehen können",
            "Verständliche Sprache sorgt dafür, dass alle Zuhörer folgen können."),
        ("Was bedeutet aktives Zuhören in einer Diskussion?", new[] { "Aufmerksam zuhören und auf das Gesagte eingehen", "Nebenbei etwas ganz anderes tun", "Die andere Person grundsätzlich ignorieren" }, "Aufmerksam zuhören und auf das Gesagte eingehen",
            "Aktives Zuhören bedeutet, aufmerksam zu sein und auf das Gesagte einzugehen."),
        ("Was gehört zu einem guten Einstieg in eine Präsentation?", new[] { "Eine kurze, interessante Einführung ins Thema", "Sofort mit den unwichtigsten Details beginnen", "Gar kein Einstieg, direkt mit dem Schluss beginnen" }, "Eine kurze, interessante Einführung ins Thema",
            "Ein guter Einstieg weckt Interesse und führt kurz ins Thema ein."),
        ("Was sollte man nach einem Interview mit den Antworten der befragten Person tun?", new[] { "Sie sinngemäß korrekt wiedergeben, nicht verfälschen", "Die Antworten beliebig verändern", "Die Antworten komplett ignorieren" }, "Sie sinngemäß korrekt wiedergeben, nicht verfälschen",
            "Antworten sollten inhaltlich korrekt und nicht verfälscht wiedergegeben werden."),
        ("Warum kann es sinnvoll sein, in einer Diskussion Gesprächsregeln vorher festzulegen?", new[] { "Damit sich alle an einen respektvollen Ablauf halten", "Damit niemand mehr etwas sagen darf", "Gesprächsregeln sind grundsätzlich überflüssig" }, "Damit sich alle an einen respektvollen Ablauf halten",
            "Feste Gesprächsregeln sichern einen respektvollen, fairen Ablauf."),
        ("Was kann helfen, Lampenfieber vor einer Präsentation zu verringern?", new[] { "Gut vorbereitet sein und vorher üben", "Möglichst gar nicht üben", "Erst kurz vorher mit der Vorbereitung beginnen" }, "Gut vorbereitet sein und vorher üben",
            "Gute Vorbereitung und Üben verringern meist das Lampenfieber."),
        ("Was ist ein Ziel einer moderierten Diskussion in der Klasse?", new[] { "Verschiedene Standpunkte respektvoll auszutauschen", "Nur eine einzige Meinung als richtig durchzusetzen", "Möglichst laut zu schreien" }, "Verschiedene Standpunkte respektvoll auszutauschen",
            "Eine moderierte Diskussion soll unterschiedliche Standpunkte respektvoll zu Wort kommen lassen."),
        ("Was bedeutet es, in einem Interview \"nachzuhaken\"?", new[] { "Bei einer unklaren Antwort gezielt nachzufragen", "Die befragte Person zu unterbrechen und zu beschimpfen", "Sofort das Thema zu wechseln" }, "Bei einer unklaren Antwort gezielt nachzufragen",
            "Nachhaken bedeutet, bei einer unklaren Antwort gezielt nachzufragen."),
        ("Warum ist es hilfreich, eine Präsentation vorher zu üben?", new[] { "Um sicherer im Vortrag zu werden und die Zeit einzuschätzen", "Üben verschlechtert die Präsentation", "Üben ist reine Zeitverschwendung" }, "Um sicherer im Vortrag zu werden und die Zeit einzuschätzen",
            "Üben macht sicherer im Vortrag und hilft, die Redezeit richtig einzuschätzen."),
        ("Was gehört zu einem respektvollen Diskussionsverhalten?", new[] { "Argumente sachlich vorbringen, statt persönlich zu werden", "Persönliche Angriffe gegen andere", "Anderen ständig ins Wort fallen" }, "Argumente sachlich vorbringen, statt persönlich zu werden",
            "Respektvolles Diskutieren bleibt sachlich, statt persönlich zu werden."),
        ("Was ist der Unterschied zwischen einem Interview und einer normalen Diskussion?", new[] { "Beim Interview stellt meist eine Person gezielt Fragen an eine andere", "Es gibt keinen Unterschied", "Bei einem Interview darf niemand antworten" }, "Beim Interview stellt meist eine Person gezielt Fragen an eine andere",
            "Ein Interview folgt meist der Struktur: eine Person fragt gezielt, die andere antwortet.")
    };

    private static QuizQuestion Gespraechsformen(Random r)
    {
        var f = GespraechsformenListe[r.Next(GespraechsformenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Gesprächsformen und Präsentieren", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "In Diskussionen: zuhören und sachlich argumentieren. In Interviews: Fragen vorbereiten und nachhaken. Bei Präsentationen: klare Gliederung und üben."
        };
    }

    private static readonly (string Aktiv, string Passiv)[] AktivPassivBeispiele =
    {
        ("Der Gärtner gießt die Blumen.", "Die Blumen werden von dem Gärtner gegossen."),
        ("Die Firma baut das Haus.", "Das Haus wird von der Firma gebaut."),
        ("Der Lehrer korrigiert die Klassenarbeit.", "Die Klassenarbeit wird von dem Lehrer korrigiert."),
        ("Die Köchin kocht die Suppe.", "Die Suppe wird von der Köchin gekocht."),
        ("Die Schüler lösen die Matheaufgabe.", "Die Matheaufgabe wird von den Schülern gelöst."),
        ("Der Bäcker backt das Brot.", "Das Brot wird von dem Bäcker gebacken."),
        ("Die Mutter wäscht die Wäsche.", "Die Wäsche wird von der Mutter gewaschen."),
        ("Der Elektriker repariert die Lampe.", "Die Lampe wird von dem Elektriker repariert."),
        ("Die Kinder bauen den Turm.", "Der Turm wird von den Kindern gebaut."),
        ("Der Regisseur dreht den Film.", "Der Film wird von dem Regisseur gedreht."),
        ("Die Firma entwickelt die Software.", "Die Software wird von der Firma entwickelt."),
        ("Der Postbote bringt das Paket.", "Das Paket wird von dem Postboten gebracht."),
        ("Die Schülerin löst das Rätsel.", "Das Rätsel wird von der Schülerin gelöst."),
        ("Der Zimmermann baut das Dach.", "Das Dach wird von dem Zimmermann gebaut."),
        ("Die Ärztin behandelt den Patienten.", "Der Patient wird von der Ärztin behandelt."),
        ("Der Fotograf macht das Foto.", "Das Foto wird von dem Fotografen gemacht."),
        ("Die Redaktion veröffentlicht den Artikel.", "Der Artikel wird von der Redaktion veröffentlicht."),
        ("Der Mechaniker wechselt den Reifen.", "Der Reifen wird von dem Mechaniker gewechselt."),
        ("Die Gemeinde organisiert das Fest.", "Das Fest wird von der Gemeinde organisiert."),
        ("Der Autor schreibt das Buch.", "Das Buch wird von dem Autor geschrieben.")
    };

    private static QuizQuestion AktivPassiv(Random r)
    {
        var a = AktivPassivBeispiele[r.Next(AktivPassivBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Aktiv und Passiv",
            Type = QuestionType.OpenText,
            Prompt = $"Wandle den Aktivsatz \"{a.Aktiv}\" in die Passivform (Vorgangspassiv) um.",
            CorrectAnswers = new[] { a.Passiv },
            Explanation = $"Passivform: \"{a.Passiv}\". Beim Passiv rückt das Objekt des Aktivsatzes an die Subjektstelle, " +
                          "das Verb wird mit \"werden\" + Partizip II gebildet.",
            HelpHint = "Passiv-Bildung: Objekt des Aktivsatzes wird zum Subjekt, Verb wird zu \"wird/werden\" + Partizip II, der Handelnde steht optional mit \"von\"."
        };
    }

    private static readonly (string Satz, string Konjunktion, string Typ)[] KonjunktionsBeispiele =
    {
        ("Wir bleiben zu Hause, ___ es regnet.", "weil", "kausal (Grund)"),
        ("Sie lernt viel, ___ sie eine gute Note bekommt.", "damit", "final (Zweck)"),
        ("___ er müde war, ging er trotzdem joggen.", "Obwohl", "konzessiv (Einräumung)"),
        ("Er ruft an, ___ er ankommt.", "sobald", "temporal (Zeit)"),
        ("Ich nehme einen Schirm mit, ___ es regnen könnte.", "falls", "konditional (Bedingung)"),
        ("Ich bleibe zu Hause, ___ ich erkältet bin.", "da", "kausal (Grund)"),
        ("Er übt jeden Tag, ___ er besser wird.", "damit", "final (Zweck)"),
        ("___ es kalt war, zogen wir uns dick an.", "Weil", "kausal (Grund)"),
        ("___ sie müde war, ging sie trotzdem zur Arbeit.", "Obwohl", "konzessiv (Einräumung)"),
        ("Wir warten hier, ___ der Bus kommt.", "bis", "temporal (Zeit)"),
        ("Sie machte das Licht aus, ___ sie das Zimmer verließ.", "bevor", "temporal (Zeit)"),
        ("Er nahm einen Regenschirm mit, ___ es regnen würde.", "falls", "konditional (Bedingung)"),
        ("___ man fleißig übt, verbessert man seine Note.", "Wenn", "konditional (Bedingung)"),
        ("Sie rief an, ___ sie zu Hause ankam.", "sobald", "temporal (Zeit)"),
        ("Er war so müde, ___ er sofort einschlief.", "dass", "konsekutiv (Folge)"),
        ("___ er wenig Zeit hatte, half er trotzdem mit.", "Obwohl", "konzessiv (Einräumung)"),
        ("Ich gehe zum Arzt, ___ ich mich nicht wohlfühle.", "weil", "kausal (Grund)"),
        ("Sie packte ihre Sachen, ___ der Zug angekommen war.", "nachdem", "temporal (Zeit)"),
        ("Er trainiert hart, ___ er die Meisterschaft gewinnt.", "damit", "final (Zweck)"),
        ("___ du Hilfe brauchst, ruf mich an.", "Falls", "konditional (Bedingung)")
    };

    private static QuizQuestion Konjunktionen(Random r)
    {
        var k = KonjunktionsBeispiele[r.Next(KonjunktionsBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Satzgefüge und Konjunktionen",
            Type = QuestionType.OpenText,
            Prompt = $"Setze die passende Konjunktion ein: \"{k.Satz}\"",
            CorrectAnswers = new[] { k.Konjunktion },
            Explanation = $"Richtig: \"{k.Konjunktion}\" – dies ist eine {k.Typ}e Konjunktion, die einen Nebensatz einleitet.",
            HelpHint = "Überlege den Sinnzusammenhang: Grund (weil), Zweck (damit), Einräumung (obwohl), Zeit (sobald)."
        };
    }

    private static readonly (string SatzMitLuecke, string Loesung, string Regel)[] KommaBeispiele =
    {
        ("Nachdem er gegessen hatte___ ging er schlafen.", ",", "Vor Nebensätzen steht immer ein Komma."),
        ("Ich möchte Pizza___ Pasta oder Salat bestellen.", "", "Bei einer einfachen Aufzählung mit \"oder\" steht meist kein Komma."),
        ("Er sagte___ dass er komme.", ",", "Vor \"dass\"-Sätzen steht immer ein Komma."),
        ("Sie kaufte Äpfel___ Birnen und Bananen.", ",", "Bei Aufzählungen ohne Bindewort steht ein Komma."),
        ("Der Hund bellte___ und lief davon.", "", "Bei zwei durch \"und\" verbundenen Verben mit demselben Subjekt (kein vollständiger Nebensatz) steht meist kein Komma."),
        ("Weil er krank war___ blieb er zu Hause.", ",", "Steht ein Nebensatz vor dem Hauptsatz, folgt darauf ein Komma."),
        ("Sie kaufte Milch___ Zucker und Mehl.", ",", "Bei einer Aufzählung von mindestens drei Elementen steht zwischen den ersten beiden ein Komma."),
        ("Ich weiß___ dass du kommst.", ",", "Vor \"dass\"-Sätzen steht immer ein Komma."),
        ("Sie ging ins Bett___ nachdem sie die Zähne geputzt hatte.", ",", "Vor einem nachgestellten Nebensatz (z.B. mit \"nachdem\") steht ein Komma."),
        ("Er aß Nudeln___ oder Reis.", "", "Bei nur zwei durch \"oder\" verbundenen Elementen steht kein Komma."),
        ("Sie fragte mich___ ob ich mitkomme.", ",", "Vor \"ob\"-Sätzen steht wie vor \"dass\"-Sätzen ein Komma."),
        ("Er kaufte Äpfel___ und Birnen.", "", "Bei nur zwei durch \"und\" verbundenen Elementen steht kein Komma."),
        ("Das Haus___ in dem wir wohnen, ist alt.", ",", "Relativsätze werden immer mit Komma abgetrennt."),
        ("Sie rannte schnell___ weil der Bus schon da war.", ",", "Vor einem nachgestellten \"weil\"-Satz steht ein Komma."),
        ("Wir packten Zelt___ Schlafsack und Proviant ein.", ",", "Bei einer Aufzählung von drei Dingen steht zwischen den ersten beiden ein Komma."),
        ("Er wusste___ dass er sich beeilen musste.", ",", "Auch hier gilt: vor \"dass\" steht immer ein Komma."),
        ("Bevor du gehst___ räum bitte dein Zimmer auf.", ",", "Steht ein Nebensatz (z.B. mit \"bevor\") vor dem Hauptsatz, folgt ein Komma."),
        ("Sie mag Äpfel___ Birnen und Trauben.", ",", "Bei einer Aufzählung von mindestens drei Elementen steht zwischen den ersten beiden ein Komma."),
        ("Er wartete___ bis der Regen aufhörte.", ",", "Vor einem nachgestellten \"bis\"-Satz steht ein Komma."),
        ("Sie schrieb einen Brief___ und schickte ihn ab.", "", "Zwei Verben mit demselben Subjekt, verbunden durch \"und\", brauchen kein Komma.")
    };

    private static QuizQuestion Kommasetzung(Random r)
    {
        var k = KommaBeispiele[r.Next(KommaBeispiele.Length)];
        string komma = string.IsNullOrEmpty(k.Loesung) ? "(kein Komma)" : "\",\"";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Kommasetzung",
            Type = QuestionType.OpenText,
            Prompt = $"Gehört an die Lücke ein Komma? Antworte mit \"ja\" oder \"nein\": \"{k.SatzMitLuecke}\"",
            CorrectAnswers = new[] { string.IsNullOrEmpty(k.Loesung) ? "nein" : "ja" },
            Explanation = $"Regel: {k.Regel}",
            HelpHint = "Vor Nebensätzen (z.B. mit \"dass\", \"nachdem\") steht immer ein Komma; bei einfachen Aufzählungen mit \"und\"/\"oder\" meist nicht."
        };
    }

    private static readonly (string Satz, string Loesung)[] DassDas =
    {
        ("Ich weiß, ___ du kommst.", "dass"),
        ("Das Buch, ___ auf dem Tisch liegt, gehört mir.", "das"),
        ("Er hofft, ___ alles gut geht.", "dass"),
        ("Das Auto, ___ dort steht, ist neu.", "das"),
        ("Ich glaube, ___ er Recht hat.", "dass"),
        ("Ich hoffe, ___ es morgen nicht regnet.", "dass"),
        ("Der Stift, ___ auf dem Boden liegt, ist rot.", "das"),
        ("Sie sagte, ___ sie später kommt.", "dass"),
        ("Das Fahrrad, ___ dort steht, gehört meinem Bruder.", "das"),
        ("Wir wissen, ___ die Prüfung schwer wird.", "dass"),
        ("Das Kind, ___ dort spielt, ist meine Nichte.", "das"),
        ("Er behauptet, ___ er alles weiß.", "dass"),
        ("Das Haus, ___ am Ende der Straße steht, ist neu.", "das"),
        ("Ich vermute, ___ sie schon zu Hause ist.", "dass"),
        ("Das Spiel, ___ wir gestern gesehen haben, war spannend.", "das"),
        ("Sie erklärte, ___ der Zug Verspätung hat.", "dass"),
        ("Das Mädchen, ___ neben mir sitzt, heißt Lena.", "das"),
        ("Ich finde, ___ das eine gute Idee ist.", "dass"),
        ("Das Bild, ___ an der Wand hängt, ist von meiner Oma.", "das"),
        ("Er meint, ___ wir früher losfahren sollten.", "dass")
    };

    private static QuizQuestion DassOderDas(Random r)
    {
        var d = DassDas[r.Next(DassDas.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "\"dass\" oder \"das\"",
            Type = QuestionType.OpenText,
            Prompt = $"Setze richtig ein: \"{d.Satz}\"",
            CorrectAnswers = new[] { d.Loesung },
            Explanation = d.Loesung == "dass"
                ? "\"dass\" leitet hier einen Nebensatz ein (Konjunktion) und kann nicht durch \"welches\" ersetzt werden."
                : "\"das\" ist hier ein Relativpronomen/Artikel und kann durch \"welches\" ersetzt werden.",
            HelpHint = "Probe: Lässt sich das Wort durch \"welches\" ersetzen? Dann ist es \"das\" (Artikel/Relativpronomen), sonst \"dass\" (Konjunktion)."
        };
    }

    private static readonly (string Satz, string Wort, string Wortart)[] Wortarten9Beispiele =
    {
        ("Trotzdem kam sie pünktlich.", "Trotzdem", "Adverb"),
        ("Das Buch liegt auf dem Tisch.", "auf", "Präposition"),
        ("Er und sie gehen ins Kino.", "und", "Konjunktion"),
        ("Sie rannte sehr schnell.", "sehr", "Adverb"),
        ("Endlich kam der Bus.", "Endlich", "Adverb"),
        ("Ich lese oft Bücher.", "oft", "Adverb"),
        ("Das Geschenk ist für dich.", "für", "Präposition"),
        ("Er kam, aber sie ging.", "aber", "Konjunktion"),
        ("Das ist meine Tasche.", "meine", "Pronomen"),
        ("Sie wohnt neben der Schule.", "neben", "Präposition"),
        ("Gestern regnete es stark.", "Gestern", "Adverb"),
        ("Ich oder du müssen das machen.", "oder", "Konjunktion"),
        ("Wir treffen uns dort.", "dort", "Adverb"),
        ("Das Auto steht vor dem Haus.", "vor", "Präposition"),
        ("Das ist ihr Buch.", "ihr", "Pronomen"),
        ("Er kommt später nach.", "später", "Adverb"),
        ("Sie lernt, weil sie will.", "weil", "Konjunktion"),
        ("Ich habe das gesehen.", "das", "Pronomen"),
        ("Der Ball liegt unter dem Tisch.", "unter", "Präposition"),
        ("Plötzlich klingelte das Telefon.", "Plötzlich", "Adverb")
    };

    private static QuizQuestion Wortarten9(Random r)
    {
        var w = Wortarten9Beispiele[r.Next(Wortarten9Beispiele.Length)];
        var optionen = new[] { "Adverb", "Präposition", "Konjunktion", "Pronomen" };

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Wortarten (vertieft)",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Satz: \"{w.Satz}\" – Welche Wortart ist \"{w.Wort}\"?",
            Options = optionen,
            CorrectAnswers = new[] { w.Wortart },
            Explanation = $"\"{w.Wort}\" ist hier ein {w.Wortart}.",
            HelpHint = "Adverbien beschreiben näher (wie/wann/wo), Präpositionen stehen vor Nomen (auf, in, mit), Konjunktionen verbinden Sätze/Wörter (und, aber)."
        };
    }

    private static readonly (string Beschreibung, string Textsorte, string Erklaerung)[] TextsortenBeispiele =
    {
        ("Ein Text, der sachlich und aktuell über ein Ereignis berichtet, das der Reporter selbst live miterlebt hat.",
            "Reportage", "Eine Reportage schildert ein selbst erlebtes/beobachtetes Ereignis lebendig und mit persönlichen Eindrücken, bleibt aber überwiegend sachbezogen."),
        ("Ein Text, in dem jemand offen die eigene Meinung zu einem aktuellen Thema äußert und begründet.",
            "Kommentar", "Ein Kommentar ist ein meinungsbetonter Text: Der Autor bezieht klar Stellung und begründet seine Sichtweise."),
        ("Ein kurzer, sachlicher Text, der nur die wichtigsten Fakten zu einem Ereignis nennt (wer, was, wann, wo).",
            "Bericht", "Ein Bericht stellt Fakten neutral und ohne eigene Meinung dar - typisch für Nachrichtentexte."),
        ("Ein Text, in dem eine Privatperson einer Zeitung ihre Meinung zu einem veröffentlichten Artikel mitteilt.",
            "Leserbrief", "Ein Leserbrief ist eine persönliche, meinungsbetonte Reaktion einer Leserin/eines Lesers auf einen veröffentlichten Text."),
        ("Ein Text, der ein Problem von mehreren Seiten beleuchtet und am Ende zu einer begründeten eigenen Position kommt.",
            "Erörterung", "Eine Erörterung wägt Pro- und Contra-Argumente zu einer Streitfrage ab und mündet in ein begründetes eigenes Urteil."),
        ("Ein Text, in dem eine Journalistin von einem selbst besuchten Flüchtlingslager erzählt und ihre eigenen Eindrücke schildert.",
            "Reportage", "Eine Reportage schildert ein selbst erlebtes Ereignis lebendig und mit persönlichen Eindrücken."),
        ("Ein Text, der ein sportliches Großereignis aus der Perspektive einer Reporterin schildert, die live vor Ort war.",
            "Reportage", "Reportagen basieren auf eigenem Erleben vor Ort und schildern Eindrücke lebendig, bleiben aber überwiegend sachbezogen."),
        ("Ein Text, der einen Tag im Leben einer Krankenschwester begleitet und persönliche Eindrücke einfließen lässt.",
            "Reportage", "Eine Reportage begleitet ein Geschehen hautnah und lässt eigene Beobachtungen und Eindrücke einfließen."),
        ("Ein Text, in dem der Autor klar Stellung bezieht, ob eine neue Regel an der Schule sinnvoll ist.",
            "Kommentar", "Ein Kommentar bezieht klar Position zu einem aktuellen Thema und begründet die eigene Meinung."),
        ("Ein Text, der begründet, warum ein bestimmtes Gesetz aus Sicht des Autors falsch ist.",
            "Kommentar", "Kommentare vertreten offen eine Meinung und untermauern sie mit Argumenten."),
        ("Ein Text, der die eigene Meinung zu einem aktuellen politischen Ereignis pointiert darstellt.",
            "Kommentar", "Ein Kommentar ist meinungsbetont und nimmt pointiert Stellung zu einem aktuellen Thema."),
        ("Ein Text, der neutral beschreibt, was bei einem Verkehrsunfall passiert ist, ohne eigene Meinung.",
            "Bericht", "Ein Bericht stellt Fakten neutral und ohne eigene Meinung dar."),
        ("Ein Text, der sachlich die wichtigsten Ergebnisse einer Wahl zusammenfasst.",
            "Bericht", "Berichte fassen Fakten sachlich und ohne persönliche Wertung zusammen."),
        ("Ein Text, der neutral über den Ablauf einer Schulversammlung informiert.",
            "Bericht", "Ein Bericht informiert neutral über die wichtigsten Fakten eines Ereignisses."),
        ("Ein Text, in dem eine Leserin der Zeitung schreibt, warum sie mit einem Zeitungsartikel nicht einverstanden ist.",
            "Leserbrief", "Ein Leserbrief ist eine persönliche, meinungsbetonte Reaktion auf einen veröffentlichten Text."),
        ("Ein Text, in dem jemand seine persönliche Sichtweise zu einem in der Zeitung diskutierten Thema mitteilt.",
            "Leserbrief", "Leserbriefe geben die persönliche Meinung einer Leserin/eines Lesers zu einem veröffentlichten Thema wieder."),
        ("Ein Text, den ein Leser an die Redaktion schickt, um Kritik oder Zustimmung zu einem Artikel auszudrücken.",
            "Leserbrief", "Ein Leserbrief richtet sich direkt an die Redaktion und äußert Kritik oder Zustimmung zu einem Artikel."),
        ("Ein Text, der Argumente für und gegen Schuluniformen sammelt und am Ende eine eigene Meinung begründet.",
            "Erörterung", "Eine Erörterung stellt Pro- und Contra-Argumente gegenüber und mündet in ein begründetes Fazit."),
        ("Ein Text, der die Vor- und Nachteile von Smartphones in der Schule gegenüberstellt und abwägt.",
            "Erörterung", "Erörterungen wägen verschiedene Standpunkte zu einer Streitfrage sachlich gegeneinander ab."),
        ("Ein Text, der verschiedene Standpunkte zu einer Streitfrage darstellt und logisch zu einem begründeten Fazit kommt.",
            "Erörterung", "Am Ende einer Erörterung steht ein logisch begründetes eigenes Urteil, das aus der Abwägung der Argumente folgt.")
    };

    private static QuizQuestion Textsorten(Random r)
    {
        var t = TextsortenBeispiele[r.Next(TextsortenBeispiele.Length)];
        var optionen = new[] { "Reportage", "Kommentar", "Bericht", "Leserbrief", "Erörterung" };

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Textsorten unterscheiden",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Um welche Textsorte handelt es sich? {t.Beschreibung}",
            Options = optionen,
            CorrectAnswers = new[] { t.Textsorte },
            Explanation = t.Erklaerung,
            HelpHint = "Sachlich + Fakten = Bericht, sachlich + selbst erlebt = Reportage, Meinung = Kommentar/Leserbrief, Pro-und-Contra-Abwägung = Erörterung."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DramaAufbauListe =
    {
        ("Wie nennt man den einleitenden Teil eines Dramas, in dem Figuren, Ort und Grundkonflikt vorgestellt werden?", new[] { "Exposition", "Peripetie", "Epilog" }, "Exposition",
            "Die Exposition führt zu Beginn eines Dramas Figuren, Ort und Grundkonflikt ein."),
        ("Wie nennt man den entscheidenden Wendepunkt in der Handlung eines Dramas?", new[] { "Peripetie", "Exposition", "Prolog" }, "Peripetie",
            "Die Peripetie ist der entscheidende Wendepunkt, an dem sich die Handlung dreht."),
        ("Was ist ein Prolog?", new[] { "Ein einleitendes Vorwort oder eine Eröffnungsszene vor der eigentlichen Handlung", "Der allerletzte Satz des Dramas", "Der Höhepunkt der Handlung" }, "Ein einleitendes Vorwort oder eine Eröffnungsszene vor der eigentlichen Handlung",
            "Der Prolog steht vor der eigentlichen Handlung und führt ins Stück ein."),
        ("Was ist ein Epilog?", new[] { "Ein Nachwort oder eine abschließende Szene nach der eigentlichen Handlung", "Die Einleitung eines Dramas", "Der Wendepunkt der Handlung" }, "Ein Nachwort oder eine abschließende Szene nach der eigentlichen Handlung",
            "Der Epilog steht nach der Haupthandlung und rundet das Stück ab."),
        ("Wie nennt man ein Drama mit unglücklichem, oft tödlichem Ausgang?", new[] { "Tragödie", "Komödie", "Prolog" }, "Tragödie",
            "Eine Tragödie endet meist unglücklich, oft mit dem Scheitern oder Tod der Hauptfigur."),
        ("Wie nennt man ein Drama mit heiterem, meist glücklichem Ausgang?", new[] { "Komödie", "Tragödie", "Epilog" }, "Komödie",
            "Eine Komödie endet meist heiter und versöhnlich."),
        ("Was versteht man unter dem \"Höhepunkt\" eines Dramas?", new[] { "Den Moment der größten Spannung, an dem sich die Handlung entscheidet", "Den allerersten Satz des Stücks", "Eine Nebenhandlung ohne Bedeutung" }, "Den Moment der größten Spannung, an dem sich die Handlung entscheidet",
            "Der Höhepunkt bündelt die Spannung und entscheidet über den weiteren Handlungsverlauf."),
        ("In welchem klassischen Aufbau-Schema folgt die Peripetie meist auf den Höhepunkt?", new[] { "Im klassischen Fünf-Akt-Schema (nach Gustav Freytag)", "Nur in modernen Comics", "Niemals in einem Drama" }, "Im klassischen Fünf-Akt-Schema (nach Gustav Freytag)",
            "Das klassische Fünf-Akt-Schema nach Gustav Freytag ordnet Exposition, Höhepunkt und Peripetie in einen festen Ablauf."),
        ("Was passiert typischerweise in der Exposition eines Theaterstücks?", new[] { "Die Ausgangssituation und die wichtigsten Figuren werden vorgestellt", "Das gesamte Stück wird schon vorweggenommen", "Der Vorhang bleibt geschlossen" }, "Die Ausgangssituation und die wichtigsten Figuren werden vorgestellt",
            "Die Exposition stellt zu Beginn die Ausgangssituation und Hauptfiguren vor."),
        ("Wie wirkt sich eine Peripetie meist auf die Handlung eines Dramas aus?", new[] { "Sie führt zu einer entscheidenden Wende, oft zum Schlechteren für die Hauptfigur", "Sie hat keinerlei Einfluss auf die Handlung", "Sie beendet das Stück sofort" }, "Sie führt zu einer entscheidenden Wende, oft zum Schlechteren für die Hauptfigur",
            "Die Peripetie kehrt die Handlung meist entscheidend, oft zum Nachteil der Hauptfigur."),
        ("Welcher Dramentyp endet oft mit dem Tod oder Scheitern der Hauptfigur?", new[] { "Tragödie", "Komödie", "Prolog" }, "Tragödie",
            "Die Tragödie endet klassischerweise mit dem Scheitern oder Tod der Hauptfigur."),
        ("Was unterscheidet eine Komödie inhaltlich von einer Tragödie?", new[] { "Die Komödie hat meist einen versöhnlichen, glücklichen Ausgang", "Beide enden immer exakt gleich", "Eine Komödie hat niemals Figuren" }, "Die Komödie hat meist einen versöhnlichen, glücklichen Ausgang",
            "Anders als die Tragödie endet die Komödie meist versöhnlich und glücklich."),
        ("Wozu dient ein Prolog in einem klassischen Drama oft?", new[] { "Um das Publikum auf das Thema oder die Moral des Stücks einzustimmen", "Um die Handlung komplett zu verschweigen", "Um das Stück sofort zu beenden" }, "Um das Publikum auf das Thema oder die Moral des Stücks einzustimmen",
            "Ein Prolog stimmt das Publikum oft schon auf Thema oder Moral des Stücks ein."),
        ("Was kann ein Epilog am Ende eines Dramas leisten?", new[] { "Eine abschließende Einordnung oder Moral des Geschehens geben", "Die Handlung von vorne beginnen lassen", "Nichts, er hat keine Funktion" }, "Eine abschließende Einordnung oder Moral des Geschehens geben",
            "Ein Epilog ordnet das Geschehen am Ende oft noch einmal ein."),
        ("Wie nennt man den Konflikt, der in der Exposition eines Dramas meist angelegt wird?", new[] { "Den Grundkonflikt der Handlung", "Einen völlig belanglosen Nebensatz", "Das Bühnenbild" }, "Den Grundkonflikt der Handlung",
            "Die Exposition legt bereits den zentralen Konflikt der Handlung an."),
        ("Warum ist der Höhepunkt eines Dramas dramaturgisch wichtig?", new[] { "Er bündelt die Spannung und entscheidet über den weiteren Verlauf", "Er kommt immer ganz am Anfang", "Er hat keinerlei Bedeutung für die Handlung" }, "Er bündelt die Spannung und entscheidet über den weiteren Verlauf",
            "Am Höhepunkt entscheidet sich, wie die Handlung weitergeht."),
        ("Was passiert häufig direkt nach der Peripetie in einem klassischen Drama?", new[] { "Die Handlung bewegt sich auf die Katastrophe bzw. den Ausgang zu", "Das Stück beginnt komplett neu", "Nichts, das Stück endet sofort" }, "Die Handlung bewegt sich auf die Katastrophe bzw. den Ausgang zu",
            "Nach der Peripetie steuert die Handlung meist auf ihren Ausgang (Katastrophe oder Lösung) zu."),
        ("Welche Textgattung gehört zu Prolog, Exposition, Höhepunkt, Peripetie und Epilog als Aufbauelemente?", new[] { "Das Drama (Theaterstück)", "Die Kurzgeschichte", "Der Zeitungsartikel" }, "Das Drama (Theaterstück)",
            "Diese Aufbauelemente sind typisch für den klassischen Dramenaufbau."),
        ("Was ist ein typisches Merkmal des klassischen Fünf-Akt-Aufbaus (nach Gustav Freytag)?", new[] { "Exposition, steigende Handlung, Höhepunkt, fallende Handlung (Peripetie), Katastrophe/Lösung", "Nur ein einziger, langer Akt ohne Struktur", "Zufällige, unzusammenhängende Szenen" }, "Exposition, steigende Handlung, Höhepunkt, fallende Handlung (Peripetie), Katastrophe/Lösung",
            "Der klassische Fünf-Akt-Aufbau folgt einer festen Abfolge von Exposition bis Lösung."),
        ("Warum werden Prolog und Epilog manchmal als \"Rahmen\" des eigentlichen Dramas bezeichnet?", new[] { "Weil sie die Haupthandlung einleiten bzw. abschließend einordnen", "Weil sie mitten in der Haupthandlung stehen", "Weil sie identisch mit dem Höhepunkt sind" }, "Weil sie die Haupthandlung einleiten bzw. abschließend einordnen",
            "Prolog und Epilog rahmen die eigentliche Haupthandlung ein.")
    };

    private static QuizQuestion DramaAufbau(Random r)
    {
        var f = DramaAufbauListe[r.Next(DramaAufbauListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Aufbau eines Dramas", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Klassischer Dramenaufbau: Prolog - Exposition - steigende Handlung - Höhepunkt - Peripetie - Epilog. Tragödie = unglücklicher Ausgang, Komödie = glücklicher Ausgang."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FigurencharakterisierungListe =
    {
        ("Was ist ein Protagonist?", new[] { "Die Hauptfigur einer Geschichte oder eines Dramas", "Der Gegenspieler der Hauptfigur", "Der Erzähler ohne eigene Rolle" }, "Die Hauptfigur einer Geschichte oder eines Dramas",
            "Der Protagonist ist die zentrale Hauptfigur einer Geschichte."),
        ("Was ist ein Antagonist?", new[] { "Der Gegenspieler der Hauptfigur, der ihr Ziele entgegensteht", "Ein anderes Wort für die Hauptfigur", "Der Autor des Textes" }, "Der Gegenspieler der Hauptfigur, der ihr Ziele entgegensteht",
            "Der Antagonist stellt sich den Zielen des Protagonisten entgegen."),
        ("Was versteht man unter einem Klischee in der Literatur?", new[] { "Eine abgenutzte, vereinfachte Vorstellung oder Darstellung", "Eine besonders originelle, neue Idee", "Einen exakten wissenschaftlichen Beweis" }, "Eine abgenutzte, vereinfachte Vorstellung oder Darstellung",
            "Ein Klischee ist eine abgenutzte, oft vereinfachte Darstellung."),
        ("Was ist ein Stereotyp?", new[] { "Ein stark vereinfachtes, oft festes Bild von einer Gruppe von Menschen", "Eine einzigartige, individuelle Figur", "Ein literarisches Fachwort für \"Reim\"" }, "Ein stark vereinfachtes, oft festes Bild von einer Gruppe von Menschen",
            "Ein Stereotyp ist ein stark vereinfachtes, oft festes Bild einer Personengruppe."),
        ("Was ist eine Personifikation?", new[] { "Die Vermenschlichung eines nicht-menschlichen Dings oder Begriffs (z. B. \"der Wind flüstert\")", "Die Beschreibung einer echten Person ohne sprachliche Bilder", "Ein anderes Wort für Reim" }, "Die Vermenschlichung eines nicht-menschlichen Dings oder Begriffs (z. B. \"der Wind flüstert\")",
            "Eine Personifikation gibt nicht-menschlichen Dingen menschliche Eigenschaften."),
        ("Warum untersuchen Leserinnen und Leser die Beziehung zwischen Protagonist und Antagonist?", new[] { "Weil daraus oft der zentrale Konflikt der Geschichte entsteht", "Weil beide Figuren immer identisch handeln", "Weil diese Beziehung literarisch bedeutungslos ist" }, "Weil daraus oft der zentrale Konflikt der Geschichte entsteht",
            "Der Konflikt zwischen Protagonist und Antagonist treibt oft die Handlung an."),
        ("Was ist ein Beispiel für ein literarisches Klischee?", new[] { "Der \"einsame Wolf\", der niemanden braucht", "Eine völlig neu erfundene, einzigartige Figur ohne Vorbild", "Ein exaktes Zitat aus einer wissenschaftlichen Studie" }, "Der \"einsame Wolf\", der niemanden braucht",
            "Der \"einsame Wolf\" ist ein oft wiederholtes, abgenutztes Figurenklischee."),
        ("Warum ist es problematisch, wenn Texte nur mit Stereotypen arbeiten?", new[] { "Weil sie Menschen vereinfachend und oft ungerecht darstellen", "Weil Stereotype immer die Realität exakt widerspiegeln", "Es ist überhaupt nicht problematisch" }, "Weil sie Menschen vereinfachend und oft ungerecht darstellen",
            "Stereotype vereinfachen Menschengruppen oft ungerecht und einseitig."),
        ("Was bewirkt eine Personifikation stilistisch in einem Text?", new[] { "Sie macht abstrakte Dinge lebendiger und anschaulicher", "Sie macht einen Text automatisch unverständlich", "Sie hat keinerlei Wirkung" }, "Sie macht abstrakte Dinge lebendiger und anschaulicher",
            "Personifikationen machen Dinge oder Naturphänomene lebendiger und anschaulicher."),
        ("Woran erkennt man häufig den Antagonisten in einer klassischen Erzählung?", new[] { "Er verfolgt Ziele, die dem Protagonisten entgegenstehen", "Er ist immer identisch mit dem Protagonisten", "Er kommt in der Geschichte nie vor" }, "Er verfolgt Ziele, die dem Protagonisten entgegenstehen",
            "Der Antagonist verfolgt Ziele, die im Widerspruch zu denen des Protagonisten stehen."),
        ("Was bedeutet \"runde Figur\" im Gegensatz zu einer \"flachen Figur\" (Klischeefigur)?", new[] { "Eine runde Figur ist vielschichtig und entwickelt sich, eine flache bleibt eindimensional", "Beide Begriffe bedeuten dasselbe", "Eine runde Figur hat immer eine kreisförmige Form" }, "Eine runde Figur ist vielschichtig und entwickelt sich, eine flache bleibt eindimensional",
            "Runde Figuren sind vielschichtig und entwickeln sich, flache Figuren bleiben eindimensional."),
        ("Was ist ein Beispiel für eine Personifikation in einem Satz?", new[] { "\"Die Sonne lacht vom Himmel.\"", "\"Die Sonne hat eine Temperatur von 5500 Grad.\"", "\"Die Sonne geht im Osten auf.\"" }, "\"Die Sonne lacht vom Himmel.\"",
            "\"Die Sonne lacht\" verleiht der Sonne eine menschliche Eigenschaft - eine Personifikation."),
        ("Warum verwenden Autorinnen und Autoren manchmal bewusst Stereotype?", new[] { "Um schnell ein bekanntes Bild zu erzeugen, das der Leser sofort einordnen kann", "Weil sie sich keine eigenen Figuren ausdenken können", "Stereotype werden nie bewusst eingesetzt" }, "Um schnell ein bekanntes Bild zu erzeugen, das der Leser sofort einordnen kann",
            "Stereotype erzeugen schnell ein bekanntes, sofort einordenbares Bild."),
        ("Was kann die Analyse eines Klischees in einem Text aufzeigen?", new[] { "Wie ein Text bestimmte Rollenbilder wiederholt oder hinterfragt", "Nichts, Klischees haben keine literarische Bedeutung", "Nur die Anzahl der Wörter im Text" }, "Wie ein Text bestimmte Rollenbilder wiederholt oder hinterfragt",
            "Die Analyse von Klischees zeigt, ob ein Text Rollenbilder bestätigt oder hinterfragt."),
        ("Was bedeutet es, wenn ein Protagonist eine \"Entwicklung\" durchmacht?", new[] { "Er verändert sich im Laufe der Geschichte, z. B. in seinen Ansichten oder seinem Verhalten", "Er bleibt von Anfang bis Ende völlig unverändert", "Er verschwindet einfach aus der Geschichte" }, "Er verändert sich im Laufe der Geschichte, z. B. in seinen Ansichten oder seinem Verhalten",
            "Eine Figurenentwicklung zeigt sich in Veränderungen von Ansichten oder Verhalten."),
        ("Was untersucht man bei einer Figurencharakterisierung?", new[] { "Äußeres, Verhalten, Sprache und Beziehungen einer literarischen Figur", "Nur die Anzahl der Buchstaben ihres Namens", "Ausschließlich das Geburtsdatum des Autors" }, "Äußeres, Verhalten, Sprache und Beziehungen einer literarischen Figur",
            "Eine Charakterisierung untersucht Äußeres, Verhalten, Sprache und Beziehungen einer Figur."),
        ("Was ist der Unterschied zwischen einem Klischee und einem Stereotyp?", new[] { "Ein Klischee ist eine abgenutzte Vorstellung allgemein, ein Stereotyp bezieht sich meist auf eine Personengruppe", "Es gibt keinerlei Unterschied", "Ein Klischee bezieht sich nur auf Tiere" }, "Ein Klischee ist eine abgenutzte Vorstellung allgemein, ein Stereotyp bezieht sich meist auf eine Personengruppe",
            "Klischees sind allgemeine abgenutzte Vorstellungen, Stereotype beziehen sich meist auf Personengruppen."),
        ("Welches sprachliche Mittel liegt vor bei \"Der Baum streckt seine Arme zum Himmel\"?", new[] { "Personifikation", "Statistik", "Interview" }, "Personifikation",
            "Der Baum bekommt hier menschliche \"Arme\" - eine Personifikation."),
        ("Warum kann die kritische Betrachtung von Stereotypen in Texten wichtig sein?", new[] { "Um Vorurteile zu erkennen und zu hinterfragen", "Um Vorurteile möglichst zu verstärken", "Weil das literarisch unbedeutend ist" }, "Um Vorurteile zu erkennen und zu hinterfragen",
            "Kritische Betrachtung hilft, Vorurteile in Texten zu erkennen und zu hinterfragen."),
        ("Was ist typischerweise die Rolle des Antagonisten für die Handlung?", new[] { "Er erzeugt Hindernisse und Konflikte für den Protagonisten", "Er sorgt dafür, dass gar kein Konflikt entsteht", "Er hat keinerlei Funktion in der Handlung" }, "Er erzeugt Hindernisse und Konflikte für den Protagonisten",
            "Der Antagonist erzeugt die Hindernisse, an denen sich der Protagonist beweisen muss.")
    };

    private static QuizQuestion Figurencharakterisierung(Random r)
    {
        var f = FigurencharakterisierungListe[r.Next(FigurencharakterisierungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Figurencharakterisierung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Protagonist = Hauptfigur, Antagonist = Gegenspieler. Klischee/Stereotyp = abgenutzte Vorstellungen. Personifikation = Vermenschlichung von Dingen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] QuellenkritikListe =
    {
        ("Was ist eine Behauptung in einem argumentierenden Text?", new[] { "Eine Aussage, die noch begründet werden muss", "Ein bereits wissenschaftlich bewiesener Fakt", "Eine reine Meinung ohne jeden Inhalt" }, "Eine Aussage, die noch begründet werden muss",
            "Eine Behauptung ist eine Aussage, die zunächst durch Argumente und Belege gestützt werden muss."),
        ("Was ist ein Argument?", new[] { "Ein Grund, der eine Behauptung stützt oder widerlegt", "Eine zufällige, unbegründete Aussage", "Ein anderes Wort für Zitat" }, "Ein Grund, der eine Behauptung stützt oder widerlegt",
            "Ein Argument liefert einen nachvollziehbaren Grund für oder gegen eine Behauptung."),
        ("Was ist ein Beleg (Beweis) für ein Argument?", new[] { "Ein konkretes Beispiel, eine Statistik oder ein Zitat, das das Argument stützt", "Eine reine Behauptung ohne Nachweis", "Ein anderes Wort für Schlagzeile" }, "Ein konkretes Beispiel, eine Statistik oder ein Zitat, das das Argument stützt",
            "Ein Beleg untermauert ein Argument mit konkreten, nachprüfbaren Fakten."),
        ("Was bedeutet \"Aktualität\" als Kriterium für eine seriöse Quelle?", new[] { "Wie neu bzw. aktuell die Information ist", "Wie lang der Text insgesamt ist", "Wie viele Bilder der Text enthält" }, "Wie neu bzw. aktuell die Information ist",
            "Aktualität beschreibt, wie neu bzw. auf dem aktuellen Stand eine Information ist."),
        ("Was bedeutet \"Seriosität\" einer Quelle?", new[] { "Wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist", "Wie unterhaltsam ein Text geschrieben ist", "Wie lang die Internetadresse ist" }, "Wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist",
            "Seriosität beschreibt, wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist."),
        ("Was bedeutet \"Ausgewogenheit\" bei der Bewertung einer Quelle?", new[] { "Ob verschiedene Perspektiven fair berücksichtigt werden", "Ob der Text möglichst kurz ist", "Ob nur eine einzige Meinung dargestellt wird" }, "Ob verschiedene Perspektiven fair berücksichtigt werden",
            "Ausgewogenheit bedeutet, dass verschiedene Sichtweisen fair einbezogen werden."),
        ("Warum ist die Unterscheidung zwischen Behauptung, Argument und Beleg wichtig beim Textverständnis?", new[] { "Um die Logik und Überzeugungskraft eines Textes einschätzen zu können", "Weil es keinen Unterschied zwischen den Begriffen gibt", "Weil Texte ohne diese Unterscheidung nicht lesbar wären" }, "Um die Logik und Überzeugungskraft eines Textes einschätzen zu können",
            "Nur wer diese Begriffe unterscheidet, kann die Überzeugungskraft eines Textes richtig einschätzen."),
        ("Woran kann man die Seriosität einer Internetquelle u.a. erkennen?", new[] { "An einem Impressum, Quellenangaben und nachprüfbaren Fakten", "An der Anzahl der Ausrufezeichen", "An besonders reißerischen Überschriften" }, "An einem Impressum, Quellenangaben und nachprüfbaren Fakten",
            "Impressum, Quellenangaben und nachprüfbare Fakten sind Anzeichen für Seriosität."),
        ("Was sollte man tun, wenn ein Text nur Behauptungen ohne Belege liefert?", new[] { "Die Aussagen kritisch hinterfragen und ggf. weitere Quellen prüfen", "Die Behauptungen ungeprüft für wahr halten", "Den Text automatisch als besonders glaubwürdig einstufen" }, "Die Aussagen kritisch hinterfragen und ggf. weitere Quellen prüfen",
            "Unbelegte Behauptungen sollte man kritisch hinterfragen und ggf. anderweitig überprüfen."),
        ("Was ist ein Beispiel für einen guten Beleg in einem argumentierenden Text?", new[] { "Eine überprüfbare Statistik aus einer seriösen Studie", "Eine unbelegte persönliche Vermutung", "Ein frei erfundenes Beispiel" }, "Eine überprüfbare Statistik aus einer seriösen Studie",
            "Überprüfbare Statistiken aus seriösen Studien sind gute Belege."),
        ("Warum sollte man mehrere Quellen zu einem Thema vergleichen?", new[] { "Um einseitige oder falsche Darstellungen zu erkennen", "Weil alle Quellen sowieso immer identisch berichten", "Das ist überflüssig" }, "Um einseitige oder falsche Darstellungen zu erkennen",
            "Der Vergleich mehrerer Quellen deckt einseitige oder falsche Darstellungen auf."),
        ("Was bedeutet es, wenn ein Text \"einseitig\" berichtet?", new[] { "Er zeigt nur eine Perspektive und lässt andere Sichtweisen weg", "Er zeigt alle Seiten eines Themas gleichermaßen", "Er enthält gar keine Meinung" }, "Er zeigt nur eine Perspektive und lässt andere Sichtweisen weg",
            "Einseitige Texte zeigen nur eine Perspektive und lassen andere weg."),
        ("Was ist der Unterschied zwischen einer Tatsache und einer Behauptung?", new[] { "Eine Tatsache ist nachweisbar, eine Behauptung muss erst noch begründet werden", "Beides ist identisch", "Eine Behauptung ist immer wahr" }, "Eine Tatsache ist nachweisbar, eine Behauptung muss erst noch begründet werden",
            "Tatsachen lassen sich nachweisen, Behauptungen müssen erst noch begründet werden."),
        ("Warum ist das Erscheinungsdatum eines Artikels ein wichtiges Qualitätskriterium?", new[] { "Weil veraltete Informationen nicht mehr zutreffen können", "Weil das Datum keinerlei Bedeutung hat", "Weil ältere Artikel automatisch glaubwürdiger sind" }, "Weil veraltete Informationen nicht mehr zutreffen können",
            "Veraltete Informationen können durch neue Entwicklungen überholt sein."),
        ("Was kann ein Hinweis auf eine unseriöse Quelle sein?", new[] { "Fehlende Autorenangabe und fehlende Belege für steile Behauptungen", "Eine ausführliche Literaturliste", "Eine sachliche, zurückhaltende Sprache" }, "Fehlende Autorenangabe und fehlende Belege für steile Behauptungen",
            "Fehlende Autoren- und Quellenangaben bei steilen Behauptungen sind Warnsignale."),
        ("Was bedeutet \"Stützung\" eines Arguments in einer Argumentationskette?", new[] { "Ein zusätzlicher Beleg oder Beispiel, der das Argument untermauert", "Eine völlig unabhängige, neue Behauptung", "Das Gegenteil des Arguments" }, "Ein zusätzlicher Beleg oder Beispiel, der das Argument untermauert",
            "Die Stützung untermauert ein Argument mit einem zusätzlichen Beleg oder Beispiel."),
        ("Was ist eine \"Folgerung\" am Ende einer Argumentationskette?", new[] { "Die logische Schlussfolgerung aus These, Argument und Beleg", "Der allererste Satz eines Textes", "Eine zufällige, unbegründete Behauptung" }, "Die logische Schlussfolgerung aus These, Argument und Beleg",
            "Die Folgerung zieht am Ende die logische Schlussfolgerung aus der gesamten Argumentation."),
        ("Warum sollte man bei wissenschaftlichen Studien auf die Finanzierung/den Auftraggeber achten?", new[] { "Weil Interessenkonflikte die Ergebnisse beeinflussen können", "Die Finanzierung hat niemals einen Einfluss auf Studienergebnisse", "Das ist völlig unwichtig" }, "Weil Interessenkonflikte die Ergebnisse beeinflussen können",
            "Interessenkonflikte durch die Finanzierung können Studienergebnisse beeinflussen."),
        ("Was ist ein Gegenargument?", new[] { "Ein Argument, das eine andere Position oder Sichtweise stützt", "Ein zusätzliches Argument für dieselbe Position", "Ein anderes Wort für Beleg" }, "Ein Argument, das eine andere Position oder Sichtweise stützt",
            "Ein Gegenargument stützt eine andere Position als die eigene."),
        ("Warum sollte man in einer eigenen Argumentation auch Gegenargumente berücksichtigen?", new[] { "Um die eigene Position überzeugender zu machen und sie zu entkräften", "Weil Gegenargumente grundsätzlich ignoriert werden müssen", "Weil das die eigene Position automatisch schwächt" }, "Um die eigene Position überzeugender zu machen und sie zu entkräften",
            "Wer Gegenargumente einbezieht und entkräftet, wirkt überzeugender.")
    };

    private static QuizQuestion Quellenkritik(Random r)
    {
        var f = QuellenkritikListe[r.Next(QuellenkritikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Argumentation und Quellenkritik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Behauptung braucht Begründung, Argument liefert einen Grund, Beleg macht ihn überprüfbar. Seriöse Quellen sind aktuell, überprüfbar und ausgewogen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FilmanalyseListe =
    {
        ("Was ist der \"Plot\" eines Films?", new[] { "Die Handlung bzw. Geschichte des Films", "Die Filmmusik", "Der Name des Regisseurs" }, "Die Handlung bzw. Geschichte des Films",
            "Der Plot bezeichnet die Handlung bzw. Geschichte eines Films."),
        ("Was ist eine \"Rückblende\" (Flashback)?", new[] { "Eine Szene, die ein früheres Ereignis zeigt", "Eine Szene, die in die Zukunft blickt", "Der Abspann des Films" }, "Eine Szene, die ein früheres Ereignis zeigt",
            "Eine Rückblende zeigt ein zeitlich früher liegendes Ereignis."),
        ("Was bedeutet \"Montage\" in der Filmanalyse?", new[] { "Das Zusammenschneiden einzelner Einstellungen zu einer Szene/Sequenz", "Das Anbringen von Filmplakaten", "Die Beleuchtung am Filmset" }, "Das Zusammenschneiden einzelner Einstellungen zu einer Szene/Sequenz",
            "Montage bezeichnet das Zusammenschneiden einzelner Einstellungen zu Szenen."),
        ("Was versteht man unter \"Kamerabewegung\"?", new[] { "Wie sich die Kamera während einer Aufnahme bewegt (z.B. Schwenk, Fahrt)", "Wie sich die Schauspieler bewegen", "Die Lautstärke der Filmmusik" }, "Wie sich die Kamera während einer Aufnahme bewegt (z.B. Schwenk, Fahrt)",
            "Kamerabewegungen wie Schwenk oder Fahrt beschreiben, wie sich die Kamera während der Aufnahme bewegt."),
        ("Was ist ein \"Schnitt\" beim Film?", new[] { "Der Übergang von einer Einstellung/Szene zur nächsten", "Eine Verletzung eines Schauspielers", "Die Kürzung der Gesamtlänge des Kinosaals" }, "Der Übergang von einer Einstellung/Szene zur nächsten",
            "Der Schnitt markiert den Übergang von einer Einstellung zur nächsten."),
        ("Wofür wird eine Rückblende in einem Film häufig genutzt?", new[] { "Um Hintergrundinformationen zu einer Figur oder Handlung zu liefern", "Um die Handlung komplett zu beenden", "Um den Abspann zu verlängern" }, "Um Hintergrundinformationen zu einer Figur oder Handlung zu liefern",
            "Rückblenden liefern oft Hintergrundinformationen zu Figuren oder Handlungssträngen."),
        ("Was kann eine schnelle Schnittfolge (viele kurze Einstellungen) bei Zuschauenden bewirken?", new[] { "Ein Gefühl von Spannung, Tempo oder Hektik", "Automatisch Langeweile", "Gar keine Wirkung" }, "Ein Gefühl von Spannung, Tempo oder Hektik",
            "Schnelle Schnittfolgen erzeugen oft Spannung, Tempo oder Hektik."),
        ("Was bedeutet eine Kamerafahrt auf eine Figur zu (Zoom/Dolly-in)?", new[] { "Sie kann Nähe, Intensität oder wachsende Bedeutung der Figur betonen", "Sie hat rein technische, keine inhaltliche Bedeutung", "Sie bedeutet immer das Ende des Films" }, "Sie kann Nähe, Intensität oder wachsende Bedeutung der Figur betonen",
            "Eine Kamerafahrt auf eine Figur zu kann Nähe oder wachsende Bedeutung betonen."),
        ("Was ist eine \"Einstellung\" im filmischen Sinn?", new[] { "Eine ununterbrochene Kameraaufnahme zwischen zwei Schnitten", "Die Meinung des Regisseurs zum Film", "Ein anderes Wort für Drehbuch" }, "Eine ununterbrochene Kameraaufnahme zwischen zwei Schnitten",
            "Eine Einstellung ist eine ununterbrochene Aufnahme zwischen zwei Schnitten."),
        ("Warum untersucht man bei einer Filmanalyse die Kamerabewegung?", new[] { "Weil sie die Wirkung und Bedeutung einer Szene mitgestaltet", "Weil sie keinerlei Bedeutung für die Aussage des Films hat", "Weil nur die Filmmusik wichtig ist" }, "Weil sie die Wirkung und Bedeutung einer Szene mitgestaltet",
            "Kamerabewegungen prägen die Wirkung und Bedeutung einer Szene mit."),
        ("Was ist eine Parallelmontage?", new[] { "Der abwechselnde Schnitt zwischen zwei gleichzeitig ablaufenden Handlungssträngen", "Die Wiederholung derselben Szene", "Eine Szene ganz ohne Bild" }, "Der abwechselnde Schnitt zwischen zwei gleichzeitig ablaufenden Handlungssträngen",
            "Eine Parallelmontage schneidet zwischen zwei gleichzeitig ablaufenden Handlungssträngen hin und her."),
        ("Was zeigt eine Rückblende im Unterschied zur normalen Filmhandlung?", new[] { "Ein zeitlich früher liegendes Ereignis", "Ein Ereignis, das nie stattgefunden hat", "Immer nur die Zukunft" }, "Ein zeitlich früher liegendes Ereignis",
            "Rückblenden zeigen im Unterschied zur laufenden Handlung ein früheres Ereignis."),
        ("Was bedeutet der Begriff \"Plot Twist\"?", new[] { "Eine überraschende Wendung in der Handlung", "Ein technischer Fehler im Film", "Der Vorspann des Films" }, "Eine überraschende Wendung in der Handlung",
            "Ein Plot Twist ist eine überraschende Wendung in der Filmhandlung."),
        ("Welche Wirkung kann eine sehr langsame Kamerafahrt in einer emotionalen Szene haben?", new[] { "Sie kann die Emotion oder Spannung des Moments verstärken", "Sie macht die Szene automatisch unwichtig", "Sie hat niemals eine Wirkung" }, "Sie kann die Emotion oder Spannung des Moments verstärken",
            "Eine langsame Kamerafahrt kann die Emotion einer Szene verstärken."),
        ("Was untersucht man bei der Analyse des \"Schnitts\" eines Films?", new[] { "Wie und in welchem Rhythmus Einstellungen aneinandergefügt werden", "Nur die Kostüme der Schauspieler", "Ausschließlich die Filmlänge in Minuten" }, "Wie und in welchem Rhythmus Einstellungen aneinandergefügt werden",
            "Die Schnittanalyse untersucht Rhythmus und Art, wie Einstellungen verbunden werden."),
        ("Was kann eine Rückblende inhaltlich zur Charakterisierung einer Figur beitragen?", new[] { "Sie kann Gründe für das heutige Verhalten der Figur zeigen", "Sie hat keinerlei Bezug zur Figur", "Sie zeigt ausschließlich die Zukunft der Figur" }, "Sie kann Gründe für das heutige Verhalten der Figur zeigen",
            "Rückblenden können erklären, warum eine Figur heute so handelt, wie sie handelt."),
        ("Was ist der Unterschied zwischen einem Schwenk und einer Kamerafahrt?", new[] { "Beim Schwenk dreht sich die Kamera an einem festen Punkt, bei der Fahrt bewegt sie sich fort", "Es gibt keinen Unterschied", "Ein Schwenk findet nur bei Nacht statt" }, "Beim Schwenk dreht sich die Kamera an einem festen Punkt, bei der Fahrt bewegt sie sich fort",
            "Beim Schwenk bleibt die Kamera an einem Ort und dreht sich, bei der Fahrt bewegt sie sich fort."),
        ("Warum ist die Montage neben dem eigentlichen Drehen ein zentraler Teil der Filmproduktion?", new[] { "Weil erst durch die Montage die endgültige Erzählstruktur und Wirkung entsteht", "Weil die Montage keinerlei Einfluss auf den fertigen Film hat", "Weil Filme ohne Montage länger wären, aber inhaltlich identisch" }, "Weil erst durch die Montage die endgültige Erzählstruktur und Wirkung entsteht",
            "Erst durch die Montage entsteht die endgültige Erzählstruktur des Films."),
        ("Was ist eine typische Funktion des \"Plots\" in der Filmanalyse?", new[] { "Er beschreibt, was inhaltlich in welcher Reihenfolge passiert", "Er beschreibt nur die verwendete Kameratechnik", "Er hat mit der Handlung nichts zu tun" }, "Er beschreibt, was inhaltlich in welcher Reihenfolge passiert",
            "Der Plot beschreibt den inhaltlichen Ablauf der Filmhandlung."),
        ("Was kann eine ungewöhnliche Kamerabewegung (z.B. eine schiefe Kameraperspektive) beim Publikum bewirken?", new[] { "Ein Gefühl von Unsicherheit oder Bedrohung", "Automatisch positive Gefühle", "Gar keine Wirkung" }, "Ein Gefühl von Unsicherheit oder Bedrohung",
            "Ungewöhnliche Kameraperspektiven können ein Gefühl von Unsicherheit oder Bedrohung erzeugen.")
    };

    private static QuizQuestion Filmanalyse(Random r)
    {
        var f = FilmanalyseListe[r.Next(FilmanalyseListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Filmanalyse", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Plot = Handlung, Rückblende = früheres Ereignis, Montage/Schnitt = Verbinden von Einstellungen, Kamerabewegung = Schwenk/Fahrt/Zoom."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RedeUndBewerbungListe =
    {
        ("Was gehört in einen Lebenslauf typischerweise?", new[] { "Persönliche Daten, Bildungsweg und bisherige Erfahrungen", "Nur ein einziges Foto ohne weitere Angaben", "Ausschließlich Hobbys ohne jede Angabe zur Bildung" }, "Persönliche Daten, Bildungsweg und bisherige Erfahrungen",
            "Ein Lebenslauf listet tabellarisch persönliche Daten, Bildungsweg und Erfahrungen auf."),
        ("Was ist der Zweck eines Bewerbungsschreibens?", new[] { "Zu erklären, warum man für eine Stelle/ein Praktikum geeignet ist", "Eine private Geschichte ganz ohne Bezug zur Bewerbung zu erzählen", "Ausschließlich Preise zu nennen" }, "Zu erklären, warum man für eine Stelle/ein Praktikum geeignet ist",
            "Ein Bewerbungsschreiben begründet, warum man für eine Stelle geeignet ist."),
        ("Was gehört zu einem formellen Anschreiben für eine Bewerbung?", new[] { "Anschrift, Betreff, förmliche Anrede, klarer Aufbau und höflicher Schluss", "Nur Emojis", "Eine informelle Anrede wie unter Freunden" }, "Anschrift, Betreff, förmliche Anrede, klarer Aufbau und höflicher Schluss",
            "Ein formelles Anschreiben folgt festen Regeln wie ein formeller Brief."),
        ("Was versteht man unter der \"Redeeröffnung\" einer Rede?", new[] { "Den einleitenden Teil, der Aufmerksamkeit weckt und ins Thema einführt", "Den letzten Satz der Rede", "Die Verbeugung nach der Rede" }, "Den einleitenden Teil, der Aufmerksamkeit weckt und ins Thema einführt",
            "Die Redeeröffnung weckt zu Beginn Aufmerksamkeit und führt ins Thema ein."),
        ("Was bedeutet \"Redeanlass\"?", new[] { "Der Grund bzw. die Gelegenheit, zu der eine Rede gehalten wird", "Der Name des Redners", "Die Dauer der Rede in Minuten" }, "Der Grund bzw. die Gelegenheit, zu der eine Rede gehalten wird",
            "Der Redeanlass ist der Grund bzw. die Gelegenheit, zu der eine Rede stattfindet."),
        ("Was ist ein Redemanuskript?", new[] { "Der schriftlich ausgearbeitete oder in Stichpunkten vorbereitete Text einer Rede", "Ein anderes Wort für Applaus", "Das Publikum einer Rede" }, "Der schriftlich ausgearbeitete oder in Stichpunkten vorbereitete Text einer Rede",
            "Ein Redemanuskript ist die schriftliche Vorbereitung einer Rede."),
        ("Was ist eine sinnvolle Strategie in einer Debatte, um ein gegnerisches Argument zu entkräften?", new[] { "Die Schwachstelle des Arguments sachlich aufzeigen und ein Gegenargument liefern", "Die gegnerische Person zu beleidigen", "Das Argument einfach zu ignorieren" }, "Die Schwachstelle des Arguments sachlich aufzeigen und ein Gegenargument liefern",
            "Man entkräftet Argumente am besten sachlich, mit einer klaren Gegenargumentation."),
        ("Was bedeutet es, in einer Debatte \"gemeinsame Interessen zu betonen\"?", new[] { "Auf Punkte hinzuweisen, in denen beide Seiten übereinstimmen", "Nur die eigene Position durchzusetzen, ohne Kompromisse", "Alle Gemeinsamkeiten zu leugnen" }, "Auf Punkte hinzuweisen, in denen beide Seiten übereinstimmen",
            "Gemeinsame Interessen zu betonen kann eine Debatte konstruktiver machen."),
        ("Warum sollte man ein Bewerbungsschreiben individuell auf die jeweilige Stelle zuschneiden?", new[] { "Um zu zeigen, dass man sich mit der Stelle konkret auseinandergesetzt hat", "Weil ein einziges Standardschreiben immer am besten wirkt", "Das ist nicht notwendig" }, "Um zu zeigen, dass man sich mit der Stelle konkret auseinandergesetzt hat",
            "Ein individuelles Schreiben zeigt echtes Interesse an der konkreten Stelle."),
        ("Was ist ein \"Podium\" bzw. eine Podiumsdiskussion?", new[] { "Eine öffentliche Diskussion mehrerer eingeladener Personen zu einem Thema", "Ein einzelner Monolog ohne weitere Teilnehmer", "Ein anderes Wort für Lebenslauf" }, "Eine öffentliche Diskussion mehrerer eingeladener Personen zu einem Thema",
            "Eine Podiumsdiskussion versammelt mehrere eingeladene Personen zu einem öffentlichen Austausch."),
        ("Was ist typisch für ein Bewerbungsgespräch?", new[] { "Fragen zu Fähigkeiten, Motivation und bisherigen Erfahrungen", "Es werden ausschließlich private Themen ohne Bezug zur Stelle besprochen", "Es gibt keinerlei Fragen" }, "Fragen zu Fähigkeiten, Motivation und bisherigen Erfahrungen",
            "In Bewerbungsgesprächen geht es meist um Fähigkeiten, Motivation und Erfahrungen."),
        ("Was sollte man in einem Bewerbungsgespräch bei unerwarteten Fragen tun?", new[] { "Ruhig bleiben und überlegt, ehrlich antworten", "Sofort das Gespräch abbrechen", "Die Frage einfach ignorieren" }, "Ruhig bleiben und überlegt, ehrlich antworten",
            "Ruhiges, ehrliches Antworten wirkt auch bei unerwarteten Fragen souverän."),
        ("Was ist eine \"Beschwerde\" als Textform?", new[] { "Ein Text, der ein Problem oder eine Unzufriedenheit sachlich vorbringt und oft eine Lösung fordert", "Ein reines Lob ohne jede Kritik", "Ein Gedicht ohne konkreten Anlass" }, "Ein Text, der ein Problem oder eine Unzufriedenheit sachlich vorbringt und oft eine Lösung fordert",
            "Eine Beschwerde bringt ein Problem sachlich vor und fordert oft eine Lösung."),
        ("Was gehört zu einer guten Debattenstrategie neben starken eigenen Argumenten?", new[] { "Gegenargumente vorwegzunehmen und zu entkräften", "Nur die eigene Meinung laut zu wiederholen", "Alle Gegenargumente zu ignorieren" }, "Gegenargumente vorwegzunehmen und zu entkräften",
            "Wer Gegenargumente vorwegnimmt und entkräftet, überzeugt stärker."),
        ("Warum ist eine klare Struktur (Einleitung, Hauptteil, Schluss) bei einer Rede wichtig?", new[] { "Damit die Zuhörer dem Gedankengang gut folgen können", "Struktur spielt bei Reden keine Rolle", "Damit die Rede möglichst unübersichtlich wirkt" }, "Damit die Zuhörer dem Gedankengang gut folgen können",
            "Eine klare Struktur hilft den Zuhörern, dem Gedankengang zu folgen."),
        ("Was ist der Unterschied zwischen einem Lebenslauf und einem Bewerbungsschreiben?", new[] { "Der Lebenslauf listet Fakten tabellarisch auf, das Anschreiben begründet die Bewerbung in Fließtext", "Beides ist völlig identisch", "Ein Lebenslauf enthält niemals Daten zur Bildung" }, "Der Lebenslauf listet Fakten tabellarisch auf, das Anschreiben begründet die Bewerbung in Fließtext",
            "Der Lebenslauf listet Fakten auf, das Anschreiben begründet die Bewerbung ausführlich."),
        ("Was ist ein sinnvolles Ziel bei einer Stegreifrede (spontane Rede ohne Vorbereitung)?", new[] { "Trotz fehlender Vorbereitung klar und strukturiert zu sprechen", "Möglichst planlos und zusammenhanglos zu reden", "Die Rede sofort abzubrechen" }, "Trotz fehlender Vorbereitung klar und strukturiert zu sprechen",
            "Auch ohne Vorbereitung sollte eine Stegreifrede klar strukturiert bleiben."),
        ("Was bedeutet \"adressatengerecht\" schreiben bei einer Bewerbung?", new[] { "Den Text auf die Erwartungen und Bedürfnisse des Empfängers abzustimmen", "Immer denselben Text unabhängig vom Empfänger zu verwenden", "Nur an sich selbst zu denken" }, "Den Text auf die Erwartungen und Bedürfnisse des Empfängers abzustimmen",
            "Adressatengerechtes Schreiben stimmt den Text auf den jeweiligen Empfänger ab."),
        ("Was kann in einer Debatte helfen, überzeugend zu argumentieren?", new[] { "Klare Belege und eine logisch aufgebaute Argumentationskette", "Möglichst laut zu sprechen, ohne Inhalte", "Nur Behauptungen ohne jede Begründung" }, "Klare Belege und eine logisch aufgebaute Argumentationskette",
            "Klare Belege und eine logische Argumentationskette überzeugen am meisten."),
        ("Warum ist es sinnvoll, sich vor einem wichtigen Gespräch (z.B. Bewerbungsgespräch) auf mögliche Fragen vorzubereiten?", new[] { "Um souveräner und überzeugender antworten zu können", "Vorbereitung verschlechtert die Wirkung", "Es ist grundsätzlich unnötig" }, "Um souveräner und überzeugender antworten zu können",
            "Vorbereitung macht souveräner und überzeugender im Gespräch.")
    };

    private static QuizQuestion RedeUndBewerbung(Random r)
    {
        var f = RedeUndBewerbungListe[r.Next(RedeUndBewerbungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Rede, Debatte und Bewerbung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Lebenslauf = Fakten tabellarisch, Anschreiben = Begründung in Fließtext. Reden brauchen klare Struktur; in Debatten helfen Belege und entkräftete Gegenargumente."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SatzbauListe =
    {
        ("Was ist der Nominalstil?", new[] { "Ein Schreibstil, der viele Substantive/Nominalisierungen statt Verben verwendet", "Ein Schreibstil, der nur aus Verben besteht", "Ein anderes Wort für Reim" }, "Ein Schreibstil, der viele Substantive/Nominalisierungen statt Verben verwendet",
            "Der Nominalstil verwendet auffällig viele Substantive und Nominalisierungen."),
        ("Was ist der Verbalstil?", new[] { "Ein Schreibstil, der handlungsbetont viele Verben nutzt", "Ein Schreibstil ganz ohne Verben", "Ein anderes Wort für Fußnote" }, "Ein Schreibstil, der handlungsbetont viele Verben nutzt",
            "Der Verbalstil ist handlungsbetont und nutzt viele Verben statt Substantivierungen."),
        ("Was ist eine Parataxe?", new[] { "Eine Reihung gleichrangiger Hauptsätze (Satzreihe)", "Ein einzelnes Wort ohne Satzbezug", "Ein Reimschema in Gedichten" }, "Eine Reihung gleichrangiger Hauptsätze (Satzreihe)",
            "Eine Parataxe reiht gleichrangige Hauptsätze aneinander."),
        ("Was ist eine Hypotaxe?", new[] { "Ein Satzgefüge aus Haupt- und untergeordneten Nebensätzen", "Eine Reihung gleichrangiger Hauptsätze", "Ein anderes Wort für Interview" }, "Ein Satzgefüge aus Haupt- und untergeordneten Nebensätzen",
            "Eine Hypotaxe ordnet Nebensätze einem Hauptsatz unter."),
        ("Wofür wird der Konjunktiv II u.a. verwendet?", new[] { "Für Wünsche, Höflichkeit oder irreale Bedingungen (z.B. \"Ich hätte gerne...\")", "Ausschließlich für die einfache Vergangenheit", "Nur für Fragen" }, "Für Wünsche, Höflichkeit oder irreale Bedingungen (z.B. \"Ich hätte gerne...\")",
            "Der Konjunktiv II drückt u.a. Wünsche, Höflichkeit und irreale Bedingungen aus."),
        ("Was ist ein Temporalsatz?", new[] { "Ein Nebensatz, der eine Zeitangabe ausdrückt (z.B. mit \"als\", \"während\")", "Ein Nebensatz, der einen Grund nennt", "Ein Nebensatz ohne jede Funktion" }, "Ein Nebensatz, der eine Zeitangabe ausdrückt (z.B. mit \"als\", \"während\")",
            "Ein Temporalsatz gibt einen zeitlichen Bezug an."),
        ("Was ist ein Kausalsatz?", new[] { "Ein Nebensatz, der einen Grund angibt (z.B. mit \"weil\", \"da\")", "Ein Nebensatz, der eine Zeitangabe macht", "Ein Nebensatz, der eine Bedingung nennt" }, "Ein Nebensatz, der einen Grund angibt (z.B. mit \"weil\", \"da\")",
            "Ein Kausalsatz nennt einen Grund für den Hauptsatz."),
        ("Was ist ein Modalsatz?", new[] { "Ein Nebensatz, der Art und Weise beschreibt (z.B. mit \"indem\", \"ohne dass\")", "Ein Nebensatz, der eine Bedingung ausdrückt", "Ein Nebensatz, der eine Zeitangabe macht" }, "Ein Nebensatz, der Art und Weise beschreibt (z.B. mit \"indem\", \"ohne dass\")",
            "Ein Modalsatz beschreibt Art und Weise einer Handlung."),
        ("Welcher Satzbau ist typisch für einen eher schlichten, gut verständlichen Text?", new[] { "Parataxe (Reihung von Hauptsätzen)", "Hypotaxe (viele verschachtelte Nebensätze)", "Reiner Nominalstil" }, "Parataxe (Reihung von Hauptsätzen)",
            "Kurze, gereihte Hauptsätze (Parataxe) wirken meist schlicht und gut verständlich."),
        ("Welcher Satzbau erlaubt es, komplexere logische Beziehungen (Grund, Zeit, Bedingung) auszudrücken?", new[] { "Hypotaxe", "Reine Parataxe", "Der Imperativ" }, "Hypotaxe",
            "Die Hypotaxe kann komplexe logische Beziehungen wie Grund, Zeit oder Bedingung ausdrücken."),
        ("Warum wird der Nominalstil oft in Fach- oder Verwaltungstexten kritisiert?", new[] { "Weil er Texte oft sperrig und schwer verständlich macht", "Weil er Texte automatisch leichter verständlich macht", "Weil er in Fachtexten verboten ist" }, "Weil er Texte oft sperrig und schwer verständlich macht",
            "Der Nominalstil gilt oft als sperrig und schwerer verständlich als der Verbalstil."),
        ("Was ist ein Beispiel für den Konjunktiv II?", new[] { "\"Ich würde gerne kommen.\"", "\"Ich komme.\"", "\"Ich kam.\"" }, "\"Ich würde gerne kommen.\"",
            "\"Ich würde gerne kommen\" ist eine Konjunktiv-II-Form."),
        ("Wie erkennt man einen Temporalsatz häufig?", new[] { "An einleitenden Wörtern wie \"als\", \"während\", \"bevor\", \"nachdem\"", "An einleitenden Wörtern wie \"weil\", \"da\"", "An einleitenden Wörtern wie \"falls\", \"wenn\"" }, "An einleitenden Wörtern wie \"als\", \"während\", \"bevor\", \"nachdem\"",
            "Temporalsätze werden oft mit \"als\", \"während\", \"bevor\" oder \"nachdem\" eingeleitet."),
        ("Wie erkennt man einen Kausalsatz häufig?", new[] { "An einleitenden Wörtern wie \"weil\", \"da\"", "An einleitenden Wörtern wie \"als\", \"während\"", "An einleitenden Wörtern wie \"damit\"" }, "An einleitenden Wörtern wie \"weil\", \"da\"",
            "Kausalsätze werden oft mit \"weil\" oder \"da\" eingeleitet."),
        ("Was ist ein Beispiel für einen Modalsatz?", new[] { "\"Er lernte, indem er viele Aufgaben übte.\"", "\"Er lernte, weil die Prüfung nahte.\"", "\"Er lernte, als die Prüfung nahte.\"" }, "\"Er lernte, indem er viele Aufgaben übte.\"",
            "\"indem er viele Aufgaben übte\" beschreibt die Art und Weise - ein Modalsatz."),
        ("Warum kann ein Wechsel zwischen Nominal- und Verbalstil einen Text lebendiger machen?", new[] { "Weil er Abwechslung schafft und Handlungen betonen kann", "Weil beide Stile immer identisch wirken", "Weil Nominalstil grundsätzlich verboten ist" }, "Weil er Abwechslung schafft und Handlungen betonen kann",
            "Ein Wechsel zwischen beiden Stilen schafft Abwechslung und kann Handlungen betonen."),
        ("Was bewirkt eine starke Häufung von Hypotaxen (viele Nebensätze) in einem Text?", new[] { "Der Text kann komplexer, aber auch schwerer lesbar werden", "Der Text wird automatisch kürzer", "Der Text hat dann gar keine Nebensätze mehr" }, "Der Text kann komplexer, aber auch schwerer lesbar werden",
            "Viele verschachtelte Nebensätze machen einen Text komplexer, aber oft schwerer lesbar."),
        ("Welche Satzart drückt am ehesten eine Bedingung aus (z.B. mit \"wenn\", \"falls\")?", new[] { "Konditionalsatz", "Temporalsatz", "Kausalsatz" }, "Konditionalsatz",
            "Ein Konditionalsatz drückt mit \"wenn\" oder \"falls\" eine Bedingung aus."),
        ("Was ist ein Vorteil von Parataxe in Reden oder Aufrufen?", new[] { "Kurze, klare Hauptsätze wirken oft eindringlicher und leichter verständlich", "Parataxe macht Texte automatisch komplizierter", "Parataxe wird nie in Reden verwendet" }, "Kurze, klare Hauptsätze wirken oft eindringlicher und leichter verständlich",
            "Kurze, klare Hauptsätze wirken in Reden oft eindringlich und verständlich."),
        ("Warum ist die Unterscheidung von Nebensatzarten (Temporal-, Kausal-, Modalsatz) beim Textverständnis hilfreich?", new[] { "Sie zeigt genau, in welcher logischen Beziehung Haupt- und Nebensatz zueinander stehen", "Sie hat keinerlei Bedeutung für das Verständnis", "Sie verändert nur die Rechtschreibung" }, "Sie zeigt genau, in welcher logischen Beziehung Haupt- und Nebensatz zueinander stehen",
            "Die Nebensatzart zeigt die logische Beziehung (Zeit, Grund, Art) zum Hauptsatz.")
    };

    private static QuizQuestion Satzbau(Random r)
    {
        var f = SatzbauListe[r.Next(SatzbauListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Satzbau und Sprachwissen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nominalstil = viele Substantive, Verbalstil = viele Verben. Parataxe = Satzreihe, Hypotaxe = Satzgefüge. Nebensätze: Temporal (Zeit), Kausal (Grund), Modal (Art und Weise)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] Wortbedeutung9Liste =
    {
        ("Was sind Synonyme?", new[] { "Wörter mit gleicher oder sehr ähnlicher Bedeutung (z.B. \"schön\" und \"hübsch\")", "Wörter mit entgegengesetzter Bedeutung", "Wörter, die gleich klingen, aber unterschiedliche Bedeutung haben" }, "Wörter mit gleicher oder sehr ähnlicher Bedeutung (z.B. \"schön\" und \"hübsch\")",
            "Synonyme sind Wörter mit gleicher oder sehr ähnlicher Bedeutung."),
        ("Was sind Antonyme?", new[] { "Wörter mit entgegengesetzter Bedeutung (z.B. \"heiß\" und \"kalt\")", "Wörter mit gleicher Bedeutung", "Wörter aus einer anderen Sprache" }, "Wörter mit entgegengesetzter Bedeutung (z.B. \"heiß\" und \"kalt\")",
            "Antonyme sind Wörter mit entgegengesetzter Bedeutung."),
        ("Was sind Homonyme?", new[] { "Wörter, die gleich geschrieben oder gesprochen werden, aber unterschiedliche Bedeutungen haben (z.B. \"Bank\")", "Wörter mit identischer Bedeutung", "Wörter, die es nur im Dialekt gibt" }, "Wörter, die gleich geschrieben oder gesprochen werden, aber unterschiedliche Bedeutungen haben (z.B. \"Bank\")",
            "Homonyme klingen oder schreiben sich gleich, bedeuten aber etwas anderes, z.B. \"Bank\" (Sitzmöbel/Geldinstitut)."),
        ("Was ist ein Anglizismus?", new[] { "Ein aus dem Englischen übernommenes Wort (z.B. \"Meeting\", \"Handy\" im übertragenen Sinn)", "Ein rein deutsches, sehr altes Wort", "Ein Wort, das nur im Dialekt vorkommt" }, "Ein aus dem Englischen übernommenes Wort (z.B. \"Meeting\", \"Handy\" im übertragenen Sinn)",
            "Anglizismen sind aus dem Englischen ins Deutsche übernommene Wörter."),
        ("Was versteht man unter Sprachwandel?", new[] { "Die Veränderung einer Sprache über die Zeit (neue Wörter, veränderte Bedeutungen)", "Das komplette Verschwinden einer Sprache über Nacht", "Ein anderes Wort für Rechtschreibfehler" }, "Die Veränderung einer Sprache über die Zeit (neue Wörter, veränderte Bedeutungen)",
            "Sprachwandel beschreibt, wie sich eine Sprache über die Zeit verändert."),
        ("Was ist ein Dialekt?", new[] { "Eine regionale Sprachvariante mit eigenen Aussprache- und Wortbesonderheiten", "Eine komplett eigenständige, fremde Sprache", "Ein anderes Wort für Fachsprache" }, "Eine regionale Sprachvariante mit eigenen Aussprache- und Wortbesonderheiten",
            "Ein Dialekt ist eine regional geprägte Variante einer Sprache."),
        ("Warum verändert sich Sprache über die Zeit?", new[] { "Durch neue Technologien, gesellschaftlichen Wandel und Sprachkontakt", "Sprache verändert sich niemals", "Nur durch Rechtschreibreformen" }, "Durch neue Technologien, gesellschaftlichen Wandel und Sprachkontakt",
            "Neue Technologien, gesellschaftlicher Wandel und Sprachkontakt treiben Sprachwandel voran."),
        ("Was ist ein Beispiel für ein Homonym im Deutschen?", new[] { "\"Die Bank\" (Sitzmöbel oder Geldinstitut)", "\"schnell\" und \"langsam\"", "\"laufen\" und \"rennen\"" }, "\"Die Bank\" (Sitzmöbel oder Geldinstitut)",
            "\"Bank\" kann Sitzmöbel oder Geldinstitut bedeuten - ein klassisches Homonym."),
        ("Was ist ein Beispiel für ein Synonym zu \"schnell\"?", new[] { "\"rasch\"", "\"langsam\"", "\"Bank\"" }, "\"rasch\"",
            "\"rasch\" bedeutet fast dasselbe wie \"schnell\" - ein Synonym."),
        ("Was ist ein Beispiel für ein Antonym zu \"laut\"?", new[] { "\"leise\"", "\"lauter\"", "\"hörbar\"" }, "\"leise\"",
            "\"leise\" ist das Gegenteil von \"laut\" - ein Antonym."),
        ("Warum werden viele Anglizismen besonders in der Jugendsprache und Technik verwendet?", new[] { "Weil englische Begriffe oft international gebräuchlich und kurz sind", "Weil Deutsch keine eigenen Wörter dafür hat", "Weil Anglizismen gesetzlich vorgeschrieben sind" }, "Weil englische Begriffe oft international gebräuchlich und kurz sind",
            "Englische Begriffe sind oft international bekannt und kurz, deshalb werden sie häufig übernommen."),
        ("Was bedeutet \"Sprachvarietät\"?", new[] { "Eine besondere Ausprägung einer Sprache, z.B. ein Dialekt oder eine Fachsprache", "Ein Synonym für \"Rechtschreibfehler\"", "Ein anderes Wort für Interview" }, "Eine besondere Ausprägung einer Sprache, z.B. ein Dialekt oder eine Fachsprache",
            "Eine Sprachvarietät ist eine besondere Ausprägung einer Sprache, z.B. Dialekt oder Fachsprache."),
        ("Warum kann die Verwendung von Dialektausdrücken in einem Text die Wirkung verändern?", new[] { "Sie kann regionale Herkunft oder Vertrautheit ausdrücken", "Dialekte haben keinerlei stilistische Wirkung", "Dialekte machen einen Text automatisch unverständlich" }, "Sie kann regionale Herkunft oder Vertrautheit ausdrücken",
            "Dialektausdrücke können regionale Herkunft oder Vertrautheit vermitteln."),
        ("Was ist ein sprachliches Register?", new[] { "Die Sprachebene, die zur jeweiligen Situation passt (z.B. formell oder umgangssprachlich)", "Ein anderes Wort für Wörterbuch", "Die Anzahl der Wörter in einem Text" }, "Die Sprachebene, die zur jeweiligen Situation passt (z.B. formell oder umgangssprachlich)",
            "Das sprachliche Register beschreibt die zur Situation passende Sprachebene."),
        ("Warum sollte man beim Schreiben eines formellen Textes auf ein passendes sprachliches Register achten?", new[] { "Weil zu saloppe Sprache in formellen Texten unpassend wirken kann", "Das sprachliche Register spielt niemals eine Rolle", "Formelle Texte dürfen nur aus Anglizismen bestehen" }, "Weil zu saloppe Sprache in formellen Texten unpassend wirken kann",
            "In formellen Texten wirkt zu saloppe Sprache unpassend."),
        ("Was zeigt der Vergleich von Zeitungstexten aus verschiedenen Jahrzehnten oft?", new[] { "Wie sich Wortschatz und Ausdrucksweise über die Zeit verändert haben", "Dass sich Sprache niemals verändert", "Dass ältere Texte immer fehlerhaft sind" }, "Wie sich Wortschatz und Ausdrucksweise über die Zeit verändert haben",
            "Solche Vergleiche zeigen anschaulich den Sprachwandel über die Jahrzehnte."),
        ("Was ist ein Beispiel für Sprachwandel durch neue Technik?", new[] { "Das Wort \"googeln\" für die Internetsuche", "Das Wort \"Baum\", das sich seit Jahrhunderten nicht verändert hat", "Das Alphabet, das sich täglich ändert" }, "Das Wort \"googeln\" für die Internetsuche",
            "\"googeln\" ist ein neues Wort, das durch neue Technik entstanden ist."),
        ("Warum ist die Kenntnis von Synonymen beim Schreiben eigener Texte hilfreich?", new[] { "Um Wortwiederholungen zu vermeiden und den Text abwechslungsreicher zu gestalten", "Um den Text absichtlich unverständlich zu machen", "Synonyme sind beim Schreiben nutzlos" }, "Um Wortwiederholungen zu vermeiden und den Text abwechslungsreicher zu gestalten",
            "Synonyme helfen, Wortwiederholungen zu vermeiden und Texte abwechslungsreicher zu machen."),
        ("Was ist der Unterschied zwischen einem Dialekt und der Standardsprache (Hochdeutsch)?", new[] { "Der Dialekt ist regional geprägt, die Standardsprache gilt überregional als Norm", "Es gibt keinen Unterschied", "Dialekte sind immer älter als die Standardsprache" }, "Der Dialekt ist regional geprägt, die Standardsprache gilt überregional als Norm",
            "Dialekte sind regional geprägt, die Standardsprache gilt überregional als einheitliche Norm."),
        ("Warum diskutieren manche Sprachwissenschaftler kritisch über die Zunahme von Anglizismen im Deutschen?", new[] { "Weil manche eine Verdrängung deutscher Begriffe befürchten, andere sehen es als natürlichen Sprachwandel", "Weil es dazu überhaupt keine unterschiedlichen Meinungen gibt", "Weil Anglizismen gesetzlich verboten sind" }, "Weil manche eine Verdrängung deutscher Begriffe befürchten, andere sehen es als natürlichen Sprachwandel",
            "Zur Zunahme von Anglizismen gibt es unterschiedliche Einschätzungen, von Sorge bis Gelassenheit.")
    };

    private static QuizQuestion Wortbedeutung9(Random r)
    {
        var f = Wortbedeutung9Liste[r.Next(Wortbedeutung9Liste.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wortbedeutung und Sprachwandel", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Synonyme = gleiche Bedeutung, Antonyme = Gegenteil, Homonyme = gleiche Form, andere Bedeutung. Sprache wandelt sich durch Technik, Gesellschaft und Sprachkontakt."
        };
    }
}
