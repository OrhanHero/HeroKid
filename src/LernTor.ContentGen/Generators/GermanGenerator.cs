using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Deutsch-Aufgabengenerator: Grammatik, Rechtschreibung, Zeitformen, Satzglieder (Klasse 6),
/// Konjunktiv/indirekte Rede, Adverbialsätze, Inhaltsangabe, Argumentieren (Klasse 7)
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
            [GradeLevel.Klasse7] = new List<TopicFactory>
            {
                KonjunktivIndirekteRede,
                Adverbialsaetze,
                SprachlicheBilder,
                Inhaltsangabe,
                Argumentieren,
                Kurzgeschichte
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
                Wortbedeutung9,
                Novelle,
                Parabel
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
        ("Was ist eine Ballade?", new[] { "Ein Gedicht, das eine spannende Geschichte in Versen erzählt", "Ein reiner Sachtext ohne Handlung", "Ein Text ohne jede sprachliche Gestaltung (was so in der Praxis nicht zutrifft)" }, "Ein Gedicht, das eine spannende Geschichte in Versen erzählt",
            "Eine Ballade erzählt eine oft dramatische Geschichte in gebundener, gereimter oder rhythmischer Sprache."),
        ("Welche Textarten mischt eine Ballade typischerweise?", new[] { "Epik, Lyrik und Dramatik", "Nur Sachtexte", "Nur Werbetexte - eine verbreitete, aber falsche Annahme" }, "Epik, Lyrik und Dramatik",
            "Balladen verbinden erzählende (epische), gedichthafte (lyrische) und oft dialogische (dramatische) Elemente."),
        ("Was ist ein Refrain in einer Ballade?", new[] { "Ein wiederkehrender Vers oder eine wiederkehrende Strophe", "Der allererste Satz eines Gedichts", "Eine Fußnote am Textende" }, "Ein wiederkehrender Vers oder eine wiederkehrende Strophe",
            "Ein Refrain kehrt im Verlauf des Gedichts mehrfach wieder, oft unverändert."),
        ("Was zeichnet die Sprache vieler Balladen aus?", new[] { "Bildhafte Sprache und oft ein regelmäßiger Rhythmus/Reim", "Ausschließlich nüchterne Fachbegriffe", "Vollständiges Fehlen von Emotionen" }, "Bildhafte Sprache und oft ein regelmäßiger Rhythmus/Reim",
            "Balladen nutzen oft sprachliche Bilder, Rhythmus und Reime, um Spannung und Stimmung zu erzeugen."),
        ("Warum eignen sich Balladen gut zum lauten Vorlesen?", new[] { "Wegen ihres Rhythmus, ihrer Spannung und oft dialogischer Passagen", "Weil sie immer sehr kurz sind", "Weil sie keinerlei Handlung haben" }, "Wegen ihres Rhythmus, ihrer Spannung und oft dialogischer Passagen",
            "Rhythmus, Spannungsbogen und wörtliche Rede machen Balladen besonders gut zum Vortragen geeignet."),
        ("Was unterscheidet eine Ballade von einem reinen Sachtext?", new[] { "Die Ballade erzählt eine Geschichte in poetischer, oft dramatischer Form", "Ein Sachtext erzählt immer eine spannende Geschichte, was einer genaueren Pruefung nicht standhaelt", "Es gibt keinen Unterschied" }, "Die Ballade erzählt eine Geschichte in poetischer, oft dramatischer Form",
            "Sachtexte informieren neutral, Balladen erzählen eine Geschichte in gebundener, poetischer Sprache."),
        ("Welches Thema behandeln viele klassische Balladen?", new[] { "Dramatische Ereignisse wie Kampf, Schicksal oder übernatürliche Erscheinungen", "Ausschließlich Kochrezepte, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Nur mathematische Formeln" }, "Dramatische Ereignisse wie Kampf, Schicksal oder übernatürliche Erscheinungen",
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
        ("Was kennzeichnet ein Kinder- oder Jugendbuch im Vergleich zu einer Ballade?", new[] { "Es erzählt meist in Prosa (Fließtext), nicht in Versen", "Es hat immer Reime", "Es besteht nur aus einem einzigen Satz und deshalb hier nicht zutrifft" }, "Es erzählt meist in Prosa (Fließtext), nicht in Versen",
            "Kinder- und Jugendbücher sind meist in Prosa geschrieben, Balladen dagegen in Versen."),
        ("Was ist wichtig, um den Inhalt eines Kinder- oder Jugendbuchs richtig zu verstehen?", new[] { "Auf Figuren, Handlung und die Entwicklung der Geschichte achten", "Nur die erste Seite lesen", "Nur die Kapitelüberschriften zählen, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Auf Figuren, Handlung und die Entwicklung der Geschichte achten",
            "Wer Figuren, Handlung und deren Entwicklung im Blick behält, versteht ein Buch besser."),
        ("Was ist eine typische Aufgabe beim Lesen eines Jugendbuchs im Unterricht?", new[] { "Die Handlung zusammenfassen und Figuren beschreiben", "Das Buch möglichst schnell weglegen, auch wenn das manche zunaechst vermuten wuerden", "Nur das Cover bewerten" }, "Die Handlung zusammenfassen und Figuren beschreiben",
            "Im Unterricht wird oft die Handlung zusammengefasst und über Figuren gesprochen."),
        ("Wie nennt man die Hauptfigur einer Geschichte oder Ballade?", new[] { "Protagonist/Hauptfigur", "Antagonist", "Erzähler ohne Rolle" }, "Protagonist/Hauptfigur",
            "Die Hauptfigur einer Geschichte oder Ballade wird auch Protagonist genannt."),
        ("Was ist eine Pointe am Ende einer Ballade oder Geschichte oft?", new[] { "Eine überraschende Wendung oder Erkenntnis", "Eine reine Wiederholung des Anfangs, was bei genauerem Hinsehen nicht stimmt", "Ein Rechtschreibfehler" }, "Eine überraschende Wendung oder Erkenntnis",
            "Eine Pointe ist ein überraschender Schlusseffekt am Ende einer Geschichte oder Ballade."),
        ("Was ist charakteristisch für den Spannungsaufbau vieler Balladen?", new[] { "Er steigert sich bis zu einem dramatischen Höhepunkt", "Er bleibt von Anfang bis Ende völlig gleich (was so in der Praxis nicht zutrifft)", "Er endet immer schon im ersten Vers" }, "Er steigert sich bis zu einem dramatischen Höhepunkt",
            "Viele Balladen bauen die Spannung Strophe für Strophe auf, bis sie in einem Höhepunkt gipfelt."),
        ("Warum werden Balladen oft auch vertont (z.B. als Lied)?", new[] { "Weil ihr Rhythmus und Reim sich gut mit Musik verbinden lassen", "Weil Balladen niemals einen Rhythmus haben", "Weil Musik und Sprache nichts miteinander zu tun haben - eine verbreitete, aber falsche Annahme" }, "Weil ihr Rhythmus und Reim sich gut mit Musik verbinden lassen",
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
        ("Was ist das Ziel eines Interviews in einer Zeitung?", new[] { "Antworten einer befragten Person zu bestimmten Fragen wiederzugeben", "Nur die eigene Meinung des Journalisten zu zeigen, was einer genaueren Pruefung nicht standhaelt", "Eine erfundene Geschichte zu erzählen" }, "Antworten einer befragten Person zu bestimmten Fragen wiederzugeben",
            "Ein Interview gibt die Antworten einer befragten Person auf gezielte Fragen wieder."),
        ("Woran erkennt man ein Interview im Text?", new[] { "An Frage-Antwort-Struktur, oft mit Namen der Sprecher", "An einer fortlaufenden Erzählung ohne Dialoge, obwohl das auf den ersten Blick plausibel klingt", "An mathematischen Formeln" }, "An Frage-Antwort-Struktur, oft mit Namen der Sprecher",
            "Interviews sind meist als Wechsel von Frage und Antwort mit Sprechernamen aufgebaut."),
        ("Was steht typischerweise am Anfang eines Zeitungsartikels?", new[] { "Die wichtigsten Informationen (Wer, was, wann, wo)", "Immer ein Gedicht", "Nur ein Bild ohne Text, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Die wichtigsten Informationen (Wer, was, wann, wo)",
            "Zeitungsartikel nennen im ersten Absatz meist die wichtigsten W-Fragen."),
        ("Was zeigt eine Grafik/ein Diagramm in einem Sachtext meistens?", new[] { "Zahlen oder Daten anschaulich, z.B. als Balken oder Kreis", "Eine erfundene Geschichte", "Nur einen einzelnen Buchstaben und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Zahlen oder Daten anschaulich, z.B. als Balken oder Kreis",
            "Grafiken stellen Zahlen und Zusammenhänge anschaulich dar, etwa als Balken- oder Kreisdiagramm."),
        ("Warum nutzen Zeitungsartikel oft Zwischenüberschriften?", new[] { "Um den Text übersichtlich in Abschnitte zu gliedern", "Um den Text unleserlich zu machen", "Weil sie gesetzlich vorgeschrieben sind" }, "Um den Text übersichtlich in Abschnitte zu gliedern",
            "Zwischenüberschriften strukturieren lange Artikel und erleichtern die Orientierung."),
        ("Was bedeutet die Abkürzung \"W-Fragen\" beim Auswerten von Sachtexten?", new[] { "Wer, was, wann, wo, warum, wie - zentrale Fragen zum Textinhalt", "Eine Abkürzung für ein Sportspiel", "Ein anderes Wort für Kommasetzung" }, "Wer, was, wann, wo, warum, wie - zentrale Fragen zum Textinhalt",
            "Die W-Fragen helfen, die wichtigsten Informationen eines Sachtexts zu erfassen."),
        ("Wie liest man eine Balkengrafik richtig?", new[] { "Man vergleicht die Höhe/Länge der Balken mit der Skala/Achse", "Man liest nur die Farbe der Balken", "Man zählt nur die Anzahl der Balken, ohne die Werte zu beachten" }, "Man vergleicht die Höhe/Länge der Balken mit der Skala/Achse",
            "Die Werte einer Balkengrafik liest man anhand der Balkenlänge im Vergleich zur Skala ab."),
        ("Was ist eine Schlagzeile?", new[] { "Die auffällige Überschrift eines Zeitungsartikels", "Der letzte Satz eines Artikels", "Ein Bild ohne Text" }, "Die auffällige Überschrift eines Zeitungsartikels",
            "Die Schlagzeile ist die auffällige, oft zugespitzte Überschrift eines Artikels."),
        ("Warum ist es wichtig, beim Lesen eines Interviews zwischen Frage und Antwort zu unterscheiden?", new[] { "Um zu erkennen, welche Aussage von wem stammt", "Um schneller lesen zu können - eine haeufige, aber unzutreffende Vorstellung", "Es ist nicht wichtig" }, "Um zu erkennen, welche Aussage von wem stammt",
            "Nur wer Frage und Antwort trennt, kann Aussagen richtig der jeweiligen Person zuordnen."),
        ("Was kann man aus einer Kreisgrafik (Tortendiagramm) ablesen?", new[] { "Wie sich ein Ganzes in verschiedene Anteile aufteilt", "Nur eine einzelne Zahl ohne Zusammenhang", "Nichts, Kreisgrafiken zeigen keine Informationen" }, "Wie sich ein Ganzes in verschiedene Anteile aufteilt",
            "Ein Tortendiagramm zeigt, wie sich ein Ganzes prozentual auf verschiedene Anteile verteilt."),
        ("Was bedeutet \"Quelle\" bei einem Zeitungsartikel oder einer Grafik?", new[] { "Die Angabe, woher die Information oder die Daten stammen", "Der Name des Lesers", "Die Uhrzeit, zu der man liest, auch wenn das manche zunaechst vermuten wuerden" }, "Die Angabe, woher die Information oder die Daten stammen",
            "Die Quellenangabe zeigt, woher Informationen oder Daten ursprünglich stammen."),
        ("Warum sollte man beim Auswerten von Sachtexten auf das Erscheinungsdatum achten?", new[] { "Weil Informationen veralten können", "Weil das Datum keine Bedeutung hat", "Weil ältere Texte automatisch falsch sind" }, "Weil Informationen veralten können",
            "Ältere Informationen können durch neuere Entwicklungen überholt sein."),
        ("Was ist der Unterschied zwischen einer offenen und einer geschlossenen Frage in einem Interview?", new[] { "Offene Fragen erlauben freie Antworten, geschlossene Fragen meist nur Ja/Nein", "Es gibt keinen Unterschied", "Geschlossene Fragen sind immer länger" }, "Offene Fragen erlauben freie Antworten, geschlossene Fragen meist nur Ja/Nein",
            "Offene Fragen laden zu ausführlichen Antworten ein, geschlossene Fragen lassen sich meist knapp beantworten."),
        ("Was zeigt die x-Achse in einem Liniendiagramm häufig an?", new[] { "Meist eine Zeitangabe (z.B. Jahre, Monate)", "Immer den Namen des Autors, was bei genauerem Hinsehen nicht stimmt", "Nichts Wichtiges" }, "Meist eine Zeitangabe (z.B. Jahre, Monate)",
            "In vielen Liniendiagrammen zeigt die waagerechte Achse eine zeitliche Entwicklung."),
        ("Wozu dient ein Untertitel unter der Schlagzeile eines Artikels?", new[] { "Er fasst kurz zusammen, worum es im Artikel geht", "Er nennt nur den Preis der Zeitung (was so in der Praxis nicht zutrifft)", "Er ist rein dekorativ ohne Inhalt" }, "Er fasst kurz zusammen, worum es im Artikel geht",
            "Der Untertitel gibt einen kurzen inhaltlichen Vorgeschmack auf den Artikel."),
        ("Was sollte man tun, um die Hauptaussage eines Sachtextes zu finden?", new[] { "Überschrift, Einleitung und Schlusssatz besonders genau lesen", "Nur die Bilder anschauen", "Den Text von hinten nach vorne lesen" }, "Überschrift, Einleitung und Schlusssatz besonders genau lesen",
            "Überschrift, Einleitung und Schluss enthalten oft die wichtigsten Kernaussagen."),
        ("Warum werden in Zeitungsartikeln oft Zitate von Expertinnen und Experten verwendet?", new[] { "Um Aussagen glaubwürdiger und nachprüfbarer zu machen", "Um den Text länger wirken zu lassen", "Weil Zitate gesetzlich Pflicht sind" }, "Um Aussagen glaubwürdiger und nachprüfbarer zu machen",
            "Expertenzitate stützen die Glaubwürdigkeit eines Artikels."),
        ("Was ist ein Vorteil von Grafiken gegenüber reinem Fließtext?", new[] { "Zahlen und Zusammenhänge werden auf einen Blick anschaulich", "Grafiken enthalten immer mehr Informationen als jeder Text", "Grafiken ersetzen jede Erklärung vollständig" }, "Zahlen und Zusammenhänge werden auf einen Blick anschaulich",
            "Grafiken machen Zahlen und Zusammenhänge schnell erfassbar."),
        ("Wie sollte man vorgehen, wenn man ein Interview zusammenfassen soll?", new[] { "Die wichtigsten Antworten in eigenen Worten wiedergeben", "Das ganze Interview wortwörtlich abschreiben - eine verbreitete, aber falsche Annahme", "Nur die Fragen ohne Antworten aufschreiben" }, "Die wichtigsten Antworten in eigenen Worten wiedergeben",
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
        ("Was sollte man bei Informationen aus einem Wiki wie Wikipedia besonders beachten?", new[] { "Ob Quellen angegeben sind und die Informationen überprüfbar sind", "Alles ist automatisch zu 100% richtig, was einer genaueren Pruefung nicht standhaelt", "Wikis dürfen nie zitiert werden" }, "Ob Quellen angegeben sind und die Informationen überprüfbar sind",
            "Da jeder Wiki-Artikel bearbeiten kann, lohnt sich ein Blick auf Quellenangaben und Überprüfbarkeit."),
        ("Was ist ein Online-Lexikon?", new[] { "Ein digitales Nachschlagewerk mit Begriffserklärungen", "Ein Videospiel", "Eine Fernsehshow, obwohl das auf den ersten Blick plausibel klingt" }, "Ein digitales Nachschlagewerk mit Begriffserklärungen",
            "Ein Online-Lexikon erklärt digital Begriffe und Sachverhalte, ähnlich einem gedruckten Lexikon."),
        ("Was gehört zu einer formell korrekten E-Mail?", new[] { "Eine passende Anrede, ein klarer Betreff und ein höflicher Schluss", "Nur Emojis ohne Text", "Groß- und Kleinschreibung spielt keine Rolle, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Eine passende Anrede, ein klarer Betreff und ein höflicher Schluss",
            "Eine formelle E-Mail braucht wie ein Brief Anrede, Betreff und höflichen Abschluss."),
        ("Wofür steht der \"Betreff\" einer E-Mail?", new[] { "Er fasst kurz zusammen, worum es in der E-Mail geht", "Er nennt das Passwort des Absenders", "Er ist immer leer" }, "Er fasst kurz zusammen, worum es in der E-Mail geht",
            "Der Betreff gibt dem Empfänger auf einen Blick den Inhalt der E-Mail an."),
        ("Was ist typisch für eine Informationssendung im Fernsehen?", new[] { "Sie berichtet sachlich über aktuelle Ereignisse", "Sie erzählt ausschließlich erfundene Geschichten", "Sie besteht nur aus Musik" }, "Sie berichtet sachlich über aktuelle Ereignisse",
            "Informationssendungen wie Nachrichten berichten sachlich über aktuelles Geschehen."),
        ("Was unterscheidet eine TV-Serie oft von einer einzelnen Sendung?", new[] { "Sie erzählt eine fortlaufende Geschichte über mehrere Folgen", "Sie hat immer nur eine einzige Folge", "Sie zeigt niemals Personen" }, "Sie erzählt eine fortlaufende Geschichte über mehrere Folgen",
            "Serien erzählen eine Geschichte, die sich über mehrere Folgen fortsetzt."),
        ("Was bedeutet \"Cliffhanger\" am Ende einer Serienfolge?", new[] { "Eine spannende, ungeklärte Situation, die zum Weiterschauen anregt", "Das endgültige Ende der ganzen Geschichte", "Ein technischer Fehler bei der Übertragung und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Eine spannende, ungeklärte Situation, die zum Weiterschauen anregt",
            "Ein Cliffhanger lässt eine Folge an einer spannenden, offenen Stelle enden."),
        ("Warum sollte man bei Online-Lexika wie bei Büchern auf Aktualität achten?", new[] { "Weil sich Informationen mit der Zeit ändern können", "Weil Online-Lexika sich nie ändern", "Aktualität spielt bei Online-Texten keine Rolle - eine haeufige, aber unzutreffende Vorstellung" }, "Weil sich Informationen mit der Zeit ändern können",
            "Auch digitale Nachschlagewerke können veraltete Informationen enthalten."),
        ("Was ist ein Vorteil von Wikis gegenüber gedruckten Lexika?", new[] { "Sie können schnell aktualisiert und ergänzt werden", "Sie sind immer fehlerfrei", "Sie können von niemandem verändert werden, auch wenn das manche zunaechst vermuten wuerden" }, "Sie können schnell aktualisiert und ergänzt werden",
            "Wikis lassen sich im Gegensatz zu gedruckten Büchern jederzeit aktualisieren."),
        ("Wie sollte die Anrede in einer formellen E-Mail an eine Lehrkraft lauten?", new[] { "Zum Beispiel \"Sehr geehrte Frau ...\" oder \"Sehr geehrter Herr ...\"", "Einfach \"Hey\"", "Gar keine Anrede, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Zum Beispiel \"Sehr geehrte Frau ...\" oder \"Sehr geehrter Herr ...\"",
            "Eine formelle Anrede zeigt Respekt gegenüber der Lehrkraft."),
        ("Was gehört an das Ende einer formellen E-Mail?", new[] { "Eine höfliche Grußformel wie \"Mit freundlichen Grüßen\"", "Nur ein Smiley", "Nichts, E-Mails enden ohne Gruß" }, "Eine höfliche Grußformel wie \"Mit freundlichen Grüßen\"",
            "Eine höfliche Grußformel rundet eine formelle E-Mail passend ab."),
        ("Warum sollte man Inhalte aus dem Internet mit mehreren Quellen vergleichen?", new[] { "Um Fehler oder einseitige Darstellungen zu erkennen", "Weil alle Internetquellen sowieso identisch sind - eine verbreitete, aber falsche Annahme", "Das ist nicht nötig" }, "Um Fehler oder einseitige Darstellungen zu erkennen",
            "Der Vergleich mehrerer Quellen hilft, Fehler oder Einseitigkeit zu erkennen."),
        ("Was ist der Unterschied zwischen einer E-Mail und einem klassischen Brief?", new[] { "Die E-Mail wird digital verschickt und kommt meist sehr schnell an", "Es gibt keinerlei Unterschied", "Ein Brief kommt immer schneller an als eine E-Mail, was einer genaueren Pruefung nicht standhaelt" }, "Die E-Mail wird digital verschickt und kommt meist sehr schnell an",
            "E-Mails werden digital versendet und erreichen den Empfänger meist fast sofort."),
        ("Was bedeutet \"Streaming\" bei Fernsehserien?", new[] { "Inhalte werden über das Internet direkt angesehen, ohne sie vorher komplett herunterzuladen", "Ein anderes Wort für Zeitungsartikel, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein Begriff aus der Mathematik" }, "Inhalte werden über das Internet direkt angesehen, ohne sie vorher komplett herunterzuladen",
            "Beim Streaming werden Video- oder Audioinhalte direkt über das Internet abgespielt."),
        ("Warum sollte man bei Wikipedia-Artikeln auf die Versionsgeschichte achten können?", new[] { "Um zu sehen, wie und von wem der Artikel verändert wurde", "Weil Versionsgeschichten Pflichtlektüre in der Schule sind", "Weil sie nichts mit dem Inhalt zu tun hat" }, "Um zu sehen, wie und von wem der Artikel verändert wurde",
            "Die Versionsgeschichte zeigt Änderungen am Artikel im Zeitverlauf."),
        ("Was zeichnet eine gute Informationssendung aus?", new[] { "Sachliche, gut recherchierte und verständliche Berichterstattung", "Möglichst viele erfundene Geschichten", "Ausschließlich Werbung" }, "Sachliche, gut recherchierte und verständliche Berichterstattung",
            "Gute Informationssendungen berichten sachlich, gut recherchiert und verständlich."),
        ("Warum ist ein klarer Betreff bei E-Mails im Schulalltag wichtig (z. B. an Lehrkräfte)?", new[] { "Damit der Empfänger sofort erkennt, worum es geht", "Der Betreff hat keinerlei Bedeutung", "Ein Betreff ist nur bei Briefen nötig und deshalb hier nicht zutrifft" }, "Damit der Empfänger sofort erkennt, worum es geht",
            "Ein aussagekräftiger Betreff hilft dem Empfänger, die E-Mail sofort einzuordnen."),
        ("Was können typische Inhalte eines Onlinelexikons sein?", new[] { "Sachliche Erklärungen zu Begriffen, Personen oder Ereignissen", "Nur private Tagebucheinträge", "Ausschließlich Werbeanzeigen" }, "Sachliche Erklärungen zu Begriffen, Personen oder Ereignissen",
            "Onlinelexika erklären Begriffe, Personen und Ereignisse sachlich."),
        ("Warum sollten Kinder und Jugendliche lernen, verschiedene mediale Textformen (Wiki, E-Mail, TV-Sendung) zu unterscheiden?", new[] { "Um Informationen in ihrem jeweiligen Kontext richtig einzuordnen und zu nutzen", "Weil alle Medienformen exakt gleich funktionieren, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Das ist für den Alltag komplett unwichtig" }, "Um Informationen in ihrem jeweiligen Kontext richtig einzuordnen und zu nutzen",
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
        ("Was gehört in einen formellen Brief an eine Behörde oder Firma?", new[] { "Anschrift, Datum, Betreff, förmliche Anrede und Grußformel", "Nur ein einzelnes Wort, auch wenn das manche zunaechst vermuten wuerden", "Ausschließlich Emojis" }, "Anschrift, Datum, Betreff, förmliche Anrede und Grußformel",
            "Ein formeller Brief folgt festen Regeln: Anschrift, Datum, Betreff, Anrede und Gruß."),
        ("Was zeichnet eine Erzählung aus?", new[] { "Sie schildert ein (oft erfundenes) Geschehen mit Spannungsbogen", "Sie besteht nur aus einer Aufzählung von Fakten, was bei genauerem Hinsehen nicht stimmt", "Sie hat niemals eine Hauptfigur" }, "Sie schildert ein (oft erfundenes) Geschehen mit Spannungsbogen",
            "Eine Erzählung schildert ein Geschehen spannend, oft mit Höhepunkt und Auflösung."),
        ("Was gehört zu einem klassischen Spannungsbogen in einer Erzählung?", new[] { "Einleitung, Hauptteil mit Höhepunkt und Schluss", "Nur ein einziger Satz", "Ausschließlich der Schluss" }, "Einleitung, Hauptteil mit Höhepunkt und Schluss",
            "Ein Spannungsbogen führt von der Einleitung über einen Höhepunkt zum Schluss."),
        ("Was ist ein Bericht (als Schreibform)?", new[] { "Eine sachliche, chronologische Darstellung eines Ereignisses", "Ein frei erfundenes Gedicht", "Eine Liste von Emojis" }, "Eine sachliche, chronologische Darstellung eines Ereignisses",
            "Ein Bericht stellt ein Ereignis sachlich und in zeitlicher Reihenfolge dar."),
        ("Was ist ein Lesetagebuch?", new[] { "Persönliche Notizen und Gedanken zu einem gelesenen Buch", "Ein Kalender ohne Bezug zu Büchern", "Eine Einkaufsliste" }, "Persönliche Notizen und Gedanken zu einem gelesenen Buch",
            "Ein Lesetagebuch hält eigene Gedanken und Eindrücke während des Lesens fest."),
        ("Was kann man in einem Lesetagebuch typischerweise festhalten?", new[] { "Eindrücke zu Figuren, spannende Stellen und eigene Meinungen zum Buch", "Nur die Seitenzahl des Buches", "Ausschließlich das Erscheinungsjahr (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" }, "Eindrücke zu Figuren, spannende Stellen und eigene Meinungen zum Buch",
            "Ein Lesetagebuch sammelt persönliche Eindrücke, Gedanken und Meinungen zum Gelesenen."),
        ("Was ist ein Parallelgedicht?", new[] { "Ein eigenes Gedicht, das die Struktur/Form eines Vorlagen-Gedichts übernimmt, aber eigene Inhalte hat", "Eine exakte Kopie eines fremden Gedichts ohne Veränderung", "Ein Gedicht ohne jede Struktur" }, "Ein eigenes Gedicht, das die Struktur/Form eines Vorlagen-Gedichts übernimmt, aber eigene Inhalte hat",
            "Ein Parallelgedicht übernimmt Form/Aufbau einer Vorlage, füllt sie aber mit eigenen Ideen."),
        ("Warum hilft ein Schreibplan beim Verfassen eines Aufsatzes?", new[] { "Er ordnet Ideen vorab und verhindert, wichtige Punkte zu vergessen", "Er macht das Schreiben unnötig kompliziert", "Er hat keinerlei Nutzen" }, "Er ordnet Ideen vorab und verhindert, wichtige Punkte zu vergessen",
            "Ein Schreibplan strukturiert Ideen im Voraus und schafft Übersicht beim Schreiben."),
        ("Was gehört zu einer Textgliederung?", new[] { "Eine sinnvolle Einteilung in Einleitung, Hauptteil und Schluss", "Ein zufälliges Durcheinander der Sätze", "Nur eine einzige lange Zeile ohne Absätze, was einer genaueren Pruefung nicht standhaelt" }, "Eine sinnvolle Einteilung in Einleitung, Hauptteil und Schluss",
            "Eine gute Gliederung teilt einen Text sinnvoll in Einleitung, Hauptteil und Schluss ein."),
        ("Was sollte die Einleitung einer Erzählung leisten?", new[] { "Ort, Zeit und Hauptfiguren kurz vorstellen und Interesse wecken", "Bereits das gesamte Ende verraten", "Komplett leer bleiben" }, "Ort, Zeit und Hauptfiguren kurz vorstellen und Interesse wecken",
            "Die Einleitung führt kurz in Ort, Zeit und Figuren ein und weckt Neugier auf die Geschichte."),
        ("Wie sollte man einen formellen Brief beenden?", new[] { "Mit einer höflichen Grußformel wie \"Mit freundlichen Grüßen\"", "Ohne jeden Abschluss", "Mit einem Emoji anstelle von Worten, obwohl das auf den ersten Blick plausibel klingt" }, "Mit einer höflichen Grußformel wie \"Mit freundlichen Grüßen\"",
            "Formelle Briefe enden mit einer höflichen Grußformel."),
        ("Was unterscheidet einen Bericht von einer Erzählung?", new[] { "Ein Bericht ist sachlich und neutral, eine Erzählung darf spannend und ausgeschmückt sein", "Es gibt keinen Unterschied", "Berichte sind immer erfunden" }, "Ein Bericht ist sachlich und neutral, eine Erzählung darf spannend und ausgeschmückt sein",
            "Berichte bleiben sachlich und neutral, Erzählungen dürfen spannend und ausgeschmückt sein."),
        ("Warum ist die richtige Zeitform beim Schreiben eines Berichts wichtig?", new[] { "Berichte werden meist im Präteritum (Vergangenheit) sachlich verfasst", "Die Zeitform ist bei Berichten egal", "Berichte müssen immer im Futur geschrieben werden, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Berichte werden meist im Präteritum (Vergangenheit) sachlich verfasst",
            "Berichte werden meist sachlich im Präteritum verfasst, da sie über Vergangenes informieren."),
        ("Was ist beim Schreiben eines Parallelgedichts besonders wichtig?", new[] { "Die Form (z.B. Reimschema, Strophenaufbau) der Vorlage zu übernehmen", "Möglichst nichts von der Vorlage zu übernehmen und deshalb hier nicht zutrifft", "Das Gedicht darf keine eigenen Ideen enthalten" }, "Die Form (z.B. Reimschema, Strophenaufbau) der Vorlage zu übernehmen",
            "Ein Parallelgedicht übernimmt die formalen Merkmale der Vorlage, mit eigenen Inhalten gefüllt."),
        ("Was gehört zum Hauptteil einer spannenden Erzählung?", new[] { "Die Handlung mit einem Höhepunkt (Spannungshöhepunkt)", "Nur eine Wiederholung der Einleitung", "Ausschließlich Beschreibungen ohne Handlung" }, "Die Handlung mit einem Höhepunkt (Spannungshöhepunkt)",
            "Im Hauptteil entfaltet sich die Handlung bis zu ihrem Spannungshöhepunkt."),
        ("Wofür kann ein Lesetagebuch beim Verstehen eines Buches hilfreich sein?", new[] { "Es hilft, Gedanken und Fragen zum Buch während des Lesens festzuhalten", "Es ersetzt das eigentliche Lesen des Buches komplett", "Es hat keinerlei Nutzen" }, "Es hilft, Gedanken und Fragen zum Buch während des Lesens festzuhalten",
            "Ein Lesetagebuch begleitet das Lesen und hilft, Gedanken festzuhalten."),
        ("Was ist ein Betreff in einem formellen Brief oder einer E-Mail?", new[] { "Eine kurze Angabe, worum es in dem Schreiben geht", "Der Name des Absenders", "Das Ausstellungsdatum des Personalausweises, was so nicht korrekt ist" }, "Eine kurze Angabe, worum es in dem Schreiben geht",
            "Der Betreff nennt kurz das Thema des Schreibens."),
        ("Warum sollte man vor dem Schreiben einer Erörterung oder Erzählung Stichpunkte sammeln?", new[] { "Um die eigenen Ideen zu ordnen, bevor man den ganzen Text ausformuliert", "Weil Stichpunkte den fertigen Text komplett ersetzen - eine haeufige, aber unzutreffende Vorstellung", "Es bringt keinerlei Vorteil" }, "Um die eigenen Ideen zu ordnen, bevor man den ganzen Text ausformuliert",
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
        ("Was bedeutet aktives Zuhören in einer Diskussion?", new[] { "Aufmerksam zuhören und auf das Gesagte eingehen", "Nebenbei etwas ganz anderes tun", "Die andere Person grundsätzlich ignorieren, auch wenn das manche zunaechst vermuten wuerden" }, "Aufmerksam zuhören und auf das Gesagte eingehen",
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
        ("Was gehört zu einem respektvollen Diskussionsverhalten?", new[] { "Argumente sachlich vorbringen, statt persönlich zu werden", "Persönliche Angriffe gegen andere, was bei genauerem Hinsehen nicht stimmt", "Anderen ständig ins Wort fallen" }, "Argumente sachlich vorbringen, statt persönlich zu werden",
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
        ("Wie nennt man den einleitenden Teil eines Dramas, in dem Figuren, Ort und Grundkonflikt vorgestellt werden?", new[] { "Exposition", "Peripetie (was so in der Praxis nicht zutrifft)", "Epilog" }, "Exposition",
            "Die Exposition führt zu Beginn eines Dramas Figuren, Ort und Grundkonflikt ein."),
        ("Wie nennt man den entscheidenden Wendepunkt in der Handlung eines Dramas?", new[] { "Peripetie", "Exposition", "Prolog" }, "Peripetie",
            "Die Peripetie ist der entscheidende Wendepunkt, an dem sich die Handlung dreht."),
        ("Was ist ein Prolog?", new[] { "Ein einleitendes Vorwort oder eine Eröffnungsszene vor der eigentlichen Handlung", "Der allerletzte Satz des Dramas", "Der Höhepunkt der Handlung" }, "Ein einleitendes Vorwort oder eine Eröffnungsszene vor der eigentlichen Handlung",
            "Der Prolog steht vor der eigentlichen Handlung und führt ins Stück ein."),
        ("Was ist ein Epilog?", new[] { "Ein Nachwort oder eine abschließende Szene nach der eigentlichen Handlung", "Die Einleitung eines Dramas - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Der Wendepunkt der Handlung" }, "Ein Nachwort oder eine abschließende Szene nach der eigentlichen Handlung",
            "Der Epilog steht nach der Haupthandlung und rundet das Stück ab."),
        ("Wie nennt man ein Drama mit unglücklichem, oft tödlichem Ausgang?", new[] { "Tragödie", "Komödie, obwohl das auf den ersten Blick plausibel klingt", "Prolog" }, "Tragödie",
            "Eine Tragödie endet meist unglücklich, oft mit dem Scheitern oder Tod der Hauptfigur."),
        ("Wie nennt man ein Drama mit heiterem, meist glücklichem Ausgang?", new[] { "Komödie", "Tragödie", "Epilog" }, "Komödie",
            "Eine Komödie endet meist heiter und versöhnlich."),
        ("Was versteht man unter dem \"Höhepunkt\" eines Dramas?", new[] { "Den Moment der größten Spannung, an dem sich die Handlung entscheidet", "Den allerersten Satz des Stücks", "Eine Nebenhandlung ohne Bedeutung, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Den Moment der größten Spannung, an dem sich die Handlung entscheidet",
            "Der Höhepunkt bündelt die Spannung und entscheidet über den weiteren Handlungsverlauf."),
        ("In welchem klassischen Aufbau-Schema folgt die Peripetie meist auf den Höhepunkt?", new[] { "Im klassischen Fünf-Akt-Schema (nach Gustav Freytag)", "Nur in modernen Comics und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Niemals in einem Drama" }, "Im klassischen Fünf-Akt-Schema (nach Gustav Freytag)",
            "Das klassische Fünf-Akt-Schema nach Gustav Freytag ordnet Exposition, Höhepunkt und Peripetie in einen festen Ablauf."),
        ("Was passiert typischerweise in der Exposition eines Theaterstücks?", new[] { "Die Ausgangssituation und die wichtigsten Figuren werden vorgestellt", "Das gesamte Stück wird schon vorweggenommen - eine haeufige, aber unzutreffende Vorstellung", "Der Vorhang bleibt geschlossen" }, "Die Ausgangssituation und die wichtigsten Figuren werden vorgestellt",
            "Die Exposition stellt zu Beginn die Ausgangssituation und Hauptfiguren vor."),
        ("Wie wirkt sich eine Peripetie meist auf die Handlung eines Dramas aus?", new[] { "Sie führt zu einer entscheidenden Wende, oft zum Schlechteren für die Hauptfigur", "Sie hat keinerlei Einfluss auf die Handlung, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Sie beendet das Stück sofort" }, "Sie führt zu einer entscheidenden Wende, oft zum Schlechteren für die Hauptfigur",
            "Die Peripetie kehrt die Handlung meist entscheidend, oft zum Nachteil der Hauptfigur."),
        ("Welcher Dramentyp endet oft mit dem Tod oder Scheitern der Hauptfigur?", new[] { "Tragödie", "Komödie (was so in der Praxis nicht zutrifft)", "Prolog" }, "Tragödie",
            "Die Tragödie endet klassischerweise mit dem Scheitern oder Tod der Hauptfigur."),
        ("Was unterscheidet eine Komödie inhaltlich von einer Tragödie?", new[] { "Die Komödie hat meist einen versöhnlichen, glücklichen Ausgang", "Beide enden immer exakt gleich", "Eine Komödie hat niemals Figuren" }, "Die Komödie hat meist einen versöhnlichen, glücklichen Ausgang",
            "Anders als die Tragödie endet die Komödie meist versöhnlich und glücklich."),
        ("Wozu dient ein Prolog in einem klassischen Drama oft?", new[] { "Um das Publikum auf das Thema oder die Moral des Stücks einzustimmen", "Um die Handlung komplett zu verschweigen - eine verbreitete, aber falsche Annahme", "Um das Stück sofort zu beenden" }, "Um das Publikum auf das Thema oder die Moral des Stücks einzustimmen",
            "Ein Prolog stimmt das Publikum oft schon auf Thema oder Moral des Stücks ein."),
        ("Was kann ein Epilog am Ende eines Dramas leisten?", new[] { "Eine abschließende Einordnung oder Moral des Geschehens geben", "Die Handlung von vorne beginnen lassen", "Nichts, er hat keine Funktion" }, "Eine abschließende Einordnung oder Moral des Geschehens geben",
            "Ein Epilog ordnet das Geschehen am Ende oft noch einmal ein."),
        ("Wie nennt man den Konflikt, der in der Exposition eines Dramas meist angelegt wird?", new[] { "Den Grundkonflikt der Handlung", "Einen völlig belanglosen Nebensatz", "Das Bühnenbild" }, "Den Grundkonflikt der Handlung",
            "Die Exposition legt bereits den zentralen Konflikt der Handlung an."),
        ("Warum ist der Höhepunkt eines Dramas dramaturgisch wichtig?", new[] { "Er bündelt die Spannung und entscheidet über den weiteren Verlauf", "Er kommt immer ganz am Anfang", "Er hat keinerlei Bedeutung für die Handlung" }, "Er bündelt die Spannung und entscheidet über den weiteren Verlauf",
            "Am Höhepunkt entscheidet sich, wie die Handlung weitergeht."),
        ("Was passiert häufig direkt nach der Peripetie in einem klassischen Drama?", new[] { "Die Handlung bewegt sich auf die Katastrophe bzw. den Ausgang zu", "Das Stück beginnt komplett neu, was einer genaueren Pruefung nicht standhaelt", "Nichts, das Stück endet sofort" }, "Die Handlung bewegt sich auf die Katastrophe bzw. den Ausgang zu",
            "Nach der Peripetie steuert die Handlung meist auf ihren Ausgang (Katastrophe oder Lösung) zu."),
        ("Welche Textgattung gehört zu Prolog, Exposition, Höhepunkt, Peripetie und Epilog als Aufbauelemente?", new[] { "Das Drama (Theaterstück)", "Die Kurzgeschichte", "Der Zeitungsartikel, obwohl das auf den ersten Blick plausibel klingt" }, "Das Drama (Theaterstück)",
            "Diese Aufbauelemente sind typisch für den klassischen Dramenaufbau."),
        ("Was ist ein typisches Merkmal des klassischen Fünf-Akt-Aufbaus (nach Gustav Freytag)?", new[] { "Exposition, steigende Handlung, Höhepunkt, fallende Handlung (Peripetie), Katastrophe/Lösung", "Nur ein einziger, langer Akt ohne Struktur", "Zufällige, unzusammenhängende Szenen" }, "Exposition, steigende Handlung, Höhepunkt, fallende Handlung (Peripetie), Katastrophe/Lösung",
            "Der klassische Fünf-Akt-Aufbau folgt einer festen Abfolge von Exposition bis Lösung."),
        ("Warum werden Prolog und Epilog manchmal als \"Rahmen\" des eigentlichen Dramas bezeichnet?", new[] { "Weil sie die Haupthandlung einleiten bzw. abschließend einordnen", "Weil sie mitten in der Haupthandlung stehen, was die eigentliche Bedeutung des Begriffs verfehlt", "Weil sie identisch mit dem Höhepunkt sind" }, "Weil sie die Haupthandlung einleiten bzw. abschließend einordnen",
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
        ("Was versteht man unter einem Klischee in der Literatur?", new[] { "Eine abgenutzte, vereinfachte Vorstellung oder Darstellung", "Eine besonders originelle, neue Idee", "Einen exakten wissenschaftlichen Beweis und deshalb hier nicht zutrifft" }, "Eine abgenutzte, vereinfachte Vorstellung oder Darstellung",
            "Ein Klischee ist eine abgenutzte, oft vereinfachte Darstellung."),
        ("Was ist ein Stereotyp?", new[] { "Ein stark vereinfachtes, oft festes Bild von einer Gruppe von Menschen", "Eine einzigartige, individuelle Figur, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Ein literarisches Fachwort für \"Reim\"" }, "Ein stark vereinfachtes, oft festes Bild von einer Gruppe von Menschen",
            "Ein Stereotyp ist ein stark vereinfachtes, oft festes Bild einer Personengruppe."),
        ("Was ist eine Personifikation?", new[] { "Die Vermenschlichung eines nicht-menschlichen Dings oder Begriffs (z. B. \"der Wind flüstert\")", "Die Beschreibung einer echten Person ohne sprachliche Bilder, auch wenn das manche zunaechst vermuten wuerden", "Ein anderes Wort für Reim" }, "Die Vermenschlichung eines nicht-menschlichen Dings oder Begriffs (z. B. \"der Wind flüstert\")",
            "Eine Personifikation gibt nicht-menschlichen Dingen menschliche Eigenschaften."),
        ("Warum untersuchen Leserinnen und Leser die Beziehung zwischen Protagonist und Antagonist?", new[] { "Weil daraus oft der zentrale Konflikt der Geschichte entsteht", "Weil beide Figuren immer identisch handeln", "Weil diese Beziehung literarisch bedeutungslos ist" }, "Weil daraus oft der zentrale Konflikt der Geschichte entsteht",
            "Der Konflikt zwischen Protagonist und Antagonist treibt oft die Handlung an."),
        ("Was ist ein Beispiel für ein literarisches Klischee?", new[] { "Der \"einsame Wolf\", der niemanden braucht", "Eine völlig neu erfundene, einzigartige Figur ohne Vorbild", "Ein exaktes Zitat aus einer wissenschaftlichen Studie" }, "Der \"einsame Wolf\", der niemanden braucht",
            "Der \"einsame Wolf\" ist ein oft wiederholtes, abgenutztes Figurenklischee."),
        ("Warum ist es problematisch, wenn Texte nur mit Stereotypen arbeiten?", new[] { "Weil sie Menschen vereinfachend und oft ungerecht darstellen", "Weil Stereotype immer die Realität exakt widerspiegeln, was bei genauerem Hinsehen nicht stimmt", "Es ist überhaupt nicht problematisch" }, "Weil sie Menschen vereinfachend und oft ungerecht darstellen",
            "Stereotype vereinfachen Menschengruppen oft ungerecht und einseitig."),
        ("Was bewirkt eine Personifikation stilistisch in einem Text?", new[] { "Sie macht abstrakte Dinge lebendiger und anschaulicher", "Sie macht einen Text automatisch unverständlich (was so in der Praxis nicht zutrifft)", "Sie hat keinerlei Wirkung" }, "Sie macht abstrakte Dinge lebendiger und anschaulicher",
            "Personifikationen machen Dinge oder Naturphänomene lebendiger und anschaulicher."),
        ("Woran erkennt man häufig den Antagonisten in einer klassischen Erzählung?", new[] { "Er verfolgt Ziele, die dem Protagonisten entgegenstehen", "Er ist immer identisch mit dem Protagonisten - eine verbreitete, aber falsche Annahme", "Er kommt in der Geschichte nie vor" }, "Er verfolgt Ziele, die dem Protagonisten entgegenstehen",
            "Der Antagonist verfolgt Ziele, die im Widerspruch zu denen des Protagonisten stehen."),
        ("Was bedeutet \"runde Figur\" im Gegensatz zu einer \"flachen Figur\" (Klischeefigur)?", new[] { "Eine runde Figur ist vielschichtig und entwickelt sich, eine flache bleibt eindimensional", "Beide Begriffe bedeuten dasselbe", "Eine runde Figur hat immer eine kreisförmige Form" }, "Eine runde Figur ist vielschichtig und entwickelt sich, eine flache bleibt eindimensional",
            "Runde Figuren sind vielschichtig und entwickeln sich, flache Figuren bleiben eindimensional."),
        ("Was ist ein Beispiel für eine Personifikation in einem Satz?", new[] { "\"Die Sonne lacht vom Himmel.\"", "\"Die Sonne hat eine Temperatur von 5500 Grad.\"", "\"Die Sonne geht im Osten auf.\"" }, "\"Die Sonne lacht vom Himmel.\"",
            "\"Die Sonne lacht\" verleiht der Sonne eine menschliche Eigenschaft - eine Personifikation."),
        ("Warum verwenden Autorinnen und Autoren manchmal bewusst Stereotype?", new[] { "Um schnell ein bekanntes Bild zu erzeugen, das der Leser sofort einordnen kann", "Weil sie sich keine eigenen Figuren ausdenken können, was einer genaueren Pruefung nicht standhaelt", "Stereotype werden nie bewusst eingesetzt" }, "Um schnell ein bekanntes Bild zu erzeugen, das der Leser sofort einordnen kann",
            "Stereotype erzeugen schnell ein bekanntes, sofort einordenbares Bild."),
        ("Was kann die Analyse eines Klischees in einem Text aufzeigen?", new[] { "Wie ein Text bestimmte Rollenbilder wiederholt oder hinterfragt", "Nichts, Klischees haben keine literarische Bedeutung", "Nur die Anzahl der Wörter im Text" }, "Wie ein Text bestimmte Rollenbilder wiederholt oder hinterfragt",
            "Die Analyse von Klischees zeigt, ob ein Text Rollenbilder bestätigt oder hinterfragt."),
        ("Was bedeutet es, wenn ein Protagonist eine \"Entwicklung\" durchmacht?", new[] { "Er verändert sich im Laufe der Geschichte, z. B. in seinen Ansichten oder seinem Verhalten", "Er bleibt von Anfang bis Ende völlig unverändert", "Er verschwindet einfach aus der Geschichte" }, "Er verändert sich im Laufe der Geschichte, z. B. in seinen Ansichten oder seinem Verhalten",
            "Eine Figurenentwicklung zeigt sich in Veränderungen von Ansichten oder Verhalten."),
        ("Was untersucht man bei einer Figurencharakterisierung?", new[] { "Äußeres, Verhalten, Sprache und Beziehungen einer literarischen Figur", "Nur die Anzahl der Buchstaben ihres Namens", "Ausschließlich das Geburtsdatum des Autors" }, "Äußeres, Verhalten, Sprache und Beziehungen einer literarischen Figur",
            "Eine Charakterisierung untersucht Äußeres, Verhalten, Sprache und Beziehungen einer Figur."),
        ("Was ist der Unterschied zwischen einem Klischee und einem Stereotyp?", new[] { "Ein Klischee ist eine abgenutzte Vorstellung allgemein, ein Stereotyp bezieht sich meist auf eine Personengruppe", "Es gibt keinerlei Unterschied", "Ein Klischee bezieht sich nur auf Tiere, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Ein Klischee ist eine abgenutzte Vorstellung allgemein, ein Stereotyp bezieht sich meist auf eine Personengruppe",
            "Klischees sind allgemeine abgenutzte Vorstellungen, Stereotype beziehen sich meist auf Personengruppen."),
        ("Welches sprachliche Mittel liegt vor bei \"Der Baum streckt seine Arme zum Himmel\"?", new[] { "Personifikation", "Statistik und deshalb hier nicht zutrifft", "Interview" }, "Personifikation",
            "Der Baum bekommt hier menschliche \"Arme\" - eine Personifikation."),
        ("Warum kann die kritische Betrachtung von Stereotypen in Texten wichtig sein?", new[] { "Um Vorurteile zu erkennen und zu hinterfragen", "Um Vorurteile möglichst zu verstärken, was so nicht korrekt ist", "Weil das literarisch unbedeutend ist" }, "Um Vorurteile zu erkennen und zu hinterfragen",
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
        ("Was ist ein Argument?", new[] { "Ein Grund, der eine Behauptung stützt oder widerlegt", "Eine zufällige, unbegründete Aussage - eine haeufige, aber unzutreffende Vorstellung", "Ein anderes Wort für Zitat" }, "Ein Grund, der eine Behauptung stützt oder widerlegt",
            "Ein Argument liefert einen nachvollziehbaren Grund für oder gegen eine Behauptung."),
        ("Was ist ein Beleg (Beweis) für ein Argument?", new[] { "Ein konkretes Beispiel, eine Statistik oder ein Zitat, das das Argument stützt", "Eine reine Behauptung ohne Nachweis, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Ein anderes Wort für Schlagzeile" }, "Ein konkretes Beispiel, eine Statistik oder ein Zitat, das das Argument stützt",
            "Ein Beleg untermauert ein Argument mit konkreten, nachprüfbaren Fakten."),
        ("Was bedeutet \"Aktualität\" als Kriterium für eine seriöse Quelle?", new[] { "Wie neu bzw. aktuell die Information ist", "Wie lang der Text insgesamt ist", "Wie viele Bilder der Text enthält (was so in der Praxis nicht zutrifft)" }, "Wie neu bzw. aktuell die Information ist",
            "Aktualität beschreibt, wie neu bzw. auf dem aktuellen Stand eine Information ist."),
        ("Was bedeutet \"Seriosität\" einer Quelle?", new[] { "Wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist", "Wie unterhaltsam ein Text geschrieben ist", "Wie lang die Internetadresse ist" }, "Wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist",
            "Seriosität beschreibt, wie vertrauenswürdig und sorgfältig recherchiert eine Quelle ist."),
        ("Was bedeutet \"Ausgewogenheit\" bei der Bewertung einer Quelle?", new[] { "Ob verschiedene Perspektiven fair berücksichtigt werden", "Ob der Text möglichst kurz ist", "Ob nur eine einzige Meinung dargestellt wird - eine verbreitete, aber falsche Annahme" }, "Ob verschiedene Perspektiven fair berücksichtigt werden",
            "Ausgewogenheit bedeutet, dass verschiedene Sichtweisen fair einbezogen werden."),
        ("Warum ist die Unterscheidung zwischen Behauptung, Argument und Beleg wichtig beim Textverständnis?", new[] { "Um die Logik und Überzeugungskraft eines Textes einschätzen zu können", "Weil es keinen Unterschied zwischen den Begriffen gibt", "Weil Texte ohne diese Unterscheidung nicht lesbar wären, was einer genaueren Pruefung nicht standhaelt" }, "Um die Logik und Überzeugungskraft eines Textes einschätzen zu können",
            "Nur wer diese Begriffe unterscheidet, kann die Überzeugungskraft eines Textes richtig einschätzen."),
        ("Woran kann man die Seriosität einer Internetquelle u.a. erkennen?", new[] { "An einem Impressum, Quellenangaben und nachprüfbaren Fakten", "An der Anzahl der Ausrufezeichen", "An besonders reißerischen Überschriften" }, "An einem Impressum, Quellenangaben und nachprüfbaren Fakten",
            "Impressum, Quellenangaben und nachprüfbare Fakten sind Anzeichen für Seriosität."),
        ("Was sollte man tun, wenn ein Text nur Behauptungen ohne Belege liefert?", new[] { "Die Aussagen kritisch hinterfragen und ggf. weitere Quellen prüfen", "Die Behauptungen ungeprüft für wahr halten", "Den Text automatisch als besonders glaubwürdig einstufen, obwohl das auf den ersten Blick plausibel klingt" }, "Die Aussagen kritisch hinterfragen und ggf. weitere Quellen prüfen",
            "Unbelegte Behauptungen sollte man kritisch hinterfragen und ggf. anderweitig überprüfen."),
        ("Was ist ein Beispiel für einen guten Beleg in einem argumentierenden Text?", new[] { "Eine überprüfbare Statistik aus einer seriösen Studie", "Eine unbelegte persönliche Vermutung, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein frei erfundenes Beispiel" }, "Eine überprüfbare Statistik aus einer seriösen Studie",
            "Überprüfbare Statistiken aus seriösen Studien sind gute Belege."),
        ("Warum sollte man mehrere Quellen zu einem Thema vergleichen?", new[] { "Um einseitige oder falsche Darstellungen zu erkennen", "Weil alle Quellen sowieso immer identisch berichten", "Das ist überflüssig" }, "Um einseitige oder falsche Darstellungen zu erkennen",
            "Der Vergleich mehrerer Quellen deckt einseitige oder falsche Darstellungen auf."),
        ("Was bedeutet es, wenn ein Text \"einseitig\" berichtet?", new[] { "Er zeigt nur eine Perspektive und lässt andere Sichtweisen weg", "Er zeigt alle Seiten eines Themas gleichermaßen", "Er enthält gar keine Meinung" }, "Er zeigt nur eine Perspektive und lässt andere Sichtweisen weg",
            "Einseitige Texte zeigen nur eine Perspektive und lassen andere weg."),
        ("Was ist der Unterschied zwischen einer Tatsache und einer Behauptung?", new[] { "Eine Tatsache ist nachweisbar, eine Behauptung muss erst noch begründet werden", "Beides ist identisch", "Eine Behauptung ist immer wahr und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Eine Tatsache ist nachweisbar, eine Behauptung muss erst noch begründet werden",
            "Tatsachen lassen sich nachweisen, Behauptungen müssen erst noch begründet werden."),
        ("Warum ist das Erscheinungsdatum eines Artikels ein wichtiges Qualitätskriterium?", new[] { "Weil veraltete Informationen nicht mehr zutreffen können", "Weil das Datum keinerlei Bedeutung hat", "Weil ältere Artikel automatisch glaubwürdiger sind" }, "Weil veraltete Informationen nicht mehr zutreffen können",
            "Veraltete Informationen können durch neue Entwicklungen überholt sein."),
        ("Was kann ein Hinweis auf eine unseriöse Quelle sein?", new[] { "Fehlende Autorenangabe und fehlende Belege für steile Behauptungen", "Eine ausführliche Literaturliste", "Eine sachliche, zurückhaltende Sprache" }, "Fehlende Autorenangabe und fehlende Belege für steile Behauptungen",
            "Fehlende Autoren- und Quellenangaben bei steilen Behauptungen sind Warnsignale."),
        ("Was bedeutet \"Stützung\" eines Arguments in einer Argumentationskette?", new[] { "Ein zusätzlicher Beleg oder Beispiel, der das Argument untermauert", "Eine völlig unabhängige, neue Behauptung - eine haeufige, aber unzutreffende Vorstellung", "Das Gegenteil des Arguments" }, "Ein zusätzlicher Beleg oder Beispiel, der das Argument untermauert",
            "Die Stützung untermauert ein Argument mit einem zusätzlichen Beleg oder Beispiel."),
        ("Was ist eine \"Folgerung\" am Ende einer Argumentationskette?", new[] { "Die logische Schlussfolgerung aus These, Argument und Beleg", "Der allererste Satz eines Textes", "Eine zufällige, unbegründete Behauptung, auch wenn das manche zunaechst vermuten wuerden" }, "Die logische Schlussfolgerung aus These, Argument und Beleg",
            "Die Folgerung zieht am Ende die logische Schlussfolgerung aus der gesamten Argumentation."),
        ("Warum sollte man bei wissenschaftlichen Studien auf die Finanzierung/den Auftraggeber achten?", new[] { "Weil Interessenkonflikte die Ergebnisse beeinflussen können", "Die Finanzierung hat niemals einen Einfluss auf Studienergebnisse", "Das ist völlig unwichtig" }, "Weil Interessenkonflikte die Ergebnisse beeinflussen können",
            "Interessenkonflikte durch die Finanzierung können Studienergebnisse beeinflussen."),
        ("Was ist ein Gegenargument?", new[] { "Ein Argument, das eine andere Position oder Sichtweise stützt", "Ein zusätzliches Argument für dieselbe Position, was bei genauerem Hinsehen nicht stimmt", "Ein anderes Wort für Beleg" }, "Ein Argument, das eine andere Position oder Sichtweise stützt",
            "Ein Gegenargument stützt eine andere Position als die eigene."),
        ("Warum sollte man in einer eigenen Argumentation auch Gegenargumente berücksichtigen?", new[] { "Um die eigene Position überzeugender zu machen und sie zu entkräften", "Weil Gegenargumente grundsätzlich ignoriert werden müssen (was so in der Praxis nicht zutrifft)", "Weil das die eigene Position automatisch schwächt" }, "Um die eigene Position überzeugender zu machen und sie zu entkräften",
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
        ("Was ist eine \"Rückblende\" (Flashback)?", new[] { "Eine Szene, die ein früheres Ereignis zeigt", "Eine Szene, die in die Zukunft blickt - eine verbreitete, aber falsche Annahme", "Der Abspann des Films" }, "Eine Szene, die ein früheres Ereignis zeigt",
            "Eine Rückblende zeigt ein zeitlich früher liegendes Ereignis."),
        ("Was bedeutet \"Montage\" in der Filmanalyse?", new[] { "Das Zusammenschneiden einzelner Einstellungen zu einer Szene/Sequenz", "Das Anbringen von Filmplakaten, was einer genaueren Pruefung nicht standhaelt", "Die Beleuchtung am Filmset" }, "Das Zusammenschneiden einzelner Einstellungen zu einer Szene/Sequenz",
            "Montage bezeichnet das Zusammenschneiden einzelner Einstellungen zu Szenen."),
        ("Was versteht man unter \"Kamerabewegung\"?", new[] { "Wie sich die Kamera während einer Aufnahme bewegt (z.B. Schwenk, Fahrt)", "Wie sich die Schauspieler bewegen, obwohl das auf den ersten Blick plausibel klingt", "Die Lautstärke der Filmmusik" }, "Wie sich die Kamera während einer Aufnahme bewegt (z.B. Schwenk, Fahrt)",
            "Kamerabewegungen wie Schwenk oder Fahrt beschreiben, wie sich die Kamera während der Aufnahme bewegt."),
        ("Was ist ein \"Schnitt\" beim Film?", new[] { "Der Übergang von einer Einstellung/Szene zur nächsten", "Eine Verletzung eines Schauspielers", "Die Kürzung der Gesamtlänge des Kinosaals" }, "Der Übergang von einer Einstellung/Szene zur nächsten",
            "Der Schnitt markiert den Übergang von einer Einstellung zur nächsten."),
        ("Wofür wird eine Rückblende in einem Film häufig genutzt?", new[] { "Um Hintergrundinformationen zu einer Figur oder Handlung zu liefern", "Um die Handlung komplett zu beenden, was die eigentliche Bedeutung des Begriffs verfehlt", "Um den Abspann zu verlängern" }, "Um Hintergrundinformationen zu einer Figur oder Handlung zu liefern",
            "Rückblenden liefern oft Hintergrundinformationen zu Figuren oder Handlungssträngen."),
        ("Was kann eine schnelle Schnittfolge (viele kurze Einstellungen) bei Zuschauenden bewirken?", new[] { "Ein Gefühl von Spannung, Tempo oder Hektik", "Automatisch Langeweile", "Gar keine Wirkung" }, "Ein Gefühl von Spannung, Tempo oder Hektik",
            "Schnelle Schnittfolgen erzeugen oft Spannung, Tempo oder Hektik."),
        ("Was bedeutet eine Kamerafahrt auf eine Figur zu (Zoom/Dolly-in)?", new[] { "Sie kann Nähe, Intensität oder wachsende Bedeutung der Figur betonen", "Sie hat rein technische, keine inhaltliche Bedeutung", "Sie bedeutet immer das Ende des Films" }, "Sie kann Nähe, Intensität oder wachsende Bedeutung der Figur betonen",
            "Eine Kamerafahrt auf eine Figur zu kann Nähe oder wachsende Bedeutung betonen."),
        ("Was ist eine \"Einstellung\" im filmischen Sinn?", new[] { "Eine ununterbrochene Kameraaufnahme zwischen zwei Schnitten", "Die Meinung des Regisseurs zum Film", "Ein anderes Wort für Drehbuch" }, "Eine ununterbrochene Kameraaufnahme zwischen zwei Schnitten",
            "Eine Einstellung ist eine ununterbrochene Aufnahme zwischen zwei Schnitten."),
        ("Warum untersucht man bei einer Filmanalyse die Kamerabewegung?", new[] { "Weil sie die Wirkung und Bedeutung einer Szene mitgestaltet", "Weil sie keinerlei Bedeutung für die Aussage des Films hat", "Weil nur die Filmmusik wichtig ist" }, "Weil sie die Wirkung und Bedeutung einer Szene mitgestaltet",
            "Kamerabewegungen prägen die Wirkung und Bedeutung einer Szene mit."),
        ("Was ist eine Parallelmontage?", new[] { "Der abwechselnde Schnitt zwischen zwei gleichzeitig ablaufenden Handlungssträngen", "Die Wiederholung derselben Szene und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Eine Szene ganz ohne Bild" }, "Der abwechselnde Schnitt zwischen zwei gleichzeitig ablaufenden Handlungssträngen",
            "Eine Parallelmontage schneidet zwischen zwei gleichzeitig ablaufenden Handlungssträngen hin und her."),
        ("Was zeigt eine Rückblende im Unterschied zur normalen Filmhandlung?", new[] { "Ein zeitlich früher liegendes Ereignis", "Ein Ereignis, das nie stattgefunden hat", "Immer nur die Zukunft" }, "Ein zeitlich früher liegendes Ereignis",
            "Rückblenden zeigen im Unterschied zur laufenden Handlung ein früheres Ereignis."),
        ("Was bedeutet der Begriff \"Plot Twist\"?", new[] { "Eine überraschende Wendung in der Handlung", "Ein technischer Fehler im Film", "Der Vorspann des Films" }, "Eine überraschende Wendung in der Handlung",
            "Ein Plot Twist ist eine überraschende Wendung in der Filmhandlung."),
        ("Welche Wirkung kann eine sehr langsame Kamerafahrt in einer emotionalen Szene haben?", new[] { "Sie kann die Emotion oder Spannung des Moments verstärken", "Sie macht die Szene automatisch unwichtig - eine haeufige, aber unzutreffende Vorstellung", "Sie hat niemals eine Wirkung" }, "Sie kann die Emotion oder Spannung des Moments verstärken",
            "Eine langsame Kamerafahrt kann die Emotion einer Szene verstärken."),
        ("Was untersucht man bei der Analyse des \"Schnitts\" eines Films?", new[] { "Wie und in welchem Rhythmus Einstellungen aneinandergefügt werden", "Nur die Kostüme der Schauspieler", "Ausschließlich die Filmlänge in Minuten, auch wenn das manche zunaechst vermuten wuerden" }, "Wie und in welchem Rhythmus Einstellungen aneinandergefügt werden",
            "Die Schnittanalyse untersucht Rhythmus und Art, wie Einstellungen verbunden werden."),
        ("Was kann eine Rückblende inhaltlich zur Charakterisierung einer Figur beitragen?", new[] { "Sie kann Gründe für das heutige Verhalten der Figur zeigen", "Sie hat keinerlei Bezug zur Figur", "Sie zeigt ausschließlich die Zukunft der Figur" }, "Sie kann Gründe für das heutige Verhalten der Figur zeigen",
            "Rückblenden können erklären, warum eine Figur heute so handelt, wie sie handelt."),
        ("Was ist der Unterschied zwischen einem Schwenk und einer Kamerafahrt?", new[] { "Beim Schwenk dreht sich die Kamera an einem festen Punkt, bei der Fahrt bewegt sie sich fort", "Es gibt keinen Unterschied", "Ein Schwenk findet nur bei Nacht statt, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Beim Schwenk dreht sich die Kamera an einem festen Punkt, bei der Fahrt bewegt sie sich fort",
            "Beim Schwenk bleibt die Kamera an einem Ort und dreht sich, bei der Fahrt bewegt sie sich fort."),
        ("Warum ist die Montage neben dem eigentlichen Drehen ein zentraler Teil der Filmproduktion?", new[] { "Weil erst durch die Montage die endgültige Erzählstruktur und Wirkung entsteht", "Weil die Montage keinerlei Einfluss auf den fertigen Film hat", "Weil Filme ohne Montage länger wären, aber inhaltlich identisch" }, "Weil erst durch die Montage die endgültige Erzählstruktur und Wirkung entsteht",
            "Erst durch die Montage entsteht die endgültige Erzählstruktur des Films."),
        ("Was ist eine typische Funktion des \"Plots\" in der Filmanalyse?", new[] { "Er beschreibt, was inhaltlich in welcher Reihenfolge passiert", "Er beschreibt nur die verwendete Kameratechnik - eine verbreitete, aber falsche Annahme", "Er hat mit der Handlung nichts zu tun" }, "Er beschreibt, was inhaltlich in welcher Reihenfolge passiert",
            "Der Plot beschreibt den inhaltlichen Ablauf der Filmhandlung."),
        ("Was kann eine ungewöhnliche Kamerabewegung (z.B. eine schiefe Kameraperspektive) beim Publikum bewirken?", new[] { "Ein Gefühl von Unsicherheit oder Bedrohung", "Automatisch positive Gefühle, was einer genaueren Pruefung nicht standhaelt", "Gar keine Wirkung" }, "Ein Gefühl von Unsicherheit oder Bedrohung",
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
        ("Was gehört in einen Lebenslauf typischerweise?", new[] { "Persönliche Daten, Bildungsweg und bisherige Erfahrungen", "Nur ein einziges Foto ohne weitere Angaben", "Ausschließlich Hobbys ohne jede Angabe zur Bildung, obwohl das auf den ersten Blick plausibel klingt" }, "Persönliche Daten, Bildungsweg und bisherige Erfahrungen",
            "Ein Lebenslauf listet tabellarisch persönliche Daten, Bildungsweg und Erfahrungen auf."),
        ("Was ist der Zweck eines Bewerbungsschreibens?", new[] { "Zu erklären, warum man für eine Stelle/ein Praktikum geeignet ist", "Eine private Geschichte ganz ohne Bezug zur Bewerbung zu erzählen", "Ausschließlich Preise zu nennen" }, "Zu erklären, warum man für eine Stelle/ein Praktikum geeignet ist",
            "Ein Bewerbungsschreiben begründet, warum man für eine Stelle geeignet ist."),
        ("Was gehört zu einem formellen Anschreiben für eine Bewerbung?", new[] { "Anschrift, Betreff, förmliche Anrede, klarer Aufbau und höflicher Schluss", "Nur Emojis", "Eine informelle Anrede wie unter Freunden, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Anschrift, Betreff, förmliche Anrede, klarer Aufbau und höflicher Schluss",
            "Ein formelles Anschreiben folgt festen Regeln wie ein formeller Brief."),
        ("Was versteht man unter der \"Redeeröffnung\" einer Rede?", new[] { "Den einleitenden Teil, der Aufmerksamkeit weckt und ins Thema einführt", "Den letzten Satz der Rede", "Die Verbeugung nach der Rede" }, "Den einleitenden Teil, der Aufmerksamkeit weckt und ins Thema einführt",
            "Die Redeeröffnung weckt zu Beginn Aufmerksamkeit und führt ins Thema ein."),
        ("Was bedeutet \"Redeanlass\"?", new[] { "Der Grund bzw. die Gelegenheit, zu der eine Rede gehalten wird", "Der Name des Redners", "Die Dauer der Rede in Minuten und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Der Grund bzw. die Gelegenheit, zu der eine Rede gehalten wird",
            "Der Redeanlass ist der Grund bzw. die Gelegenheit, zu der eine Rede stattfindet."),
        ("Was ist ein Redemanuskript?", new[] { "Der schriftlich ausgearbeitete oder in Stichpunkten vorbereitete Text einer Rede", "Ein anderes Wort für Applaus - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden", "Das Publikum einer Rede" }, "Der schriftlich ausgearbeitete oder in Stichpunkten vorbereitete Text einer Rede",
            "Ein Redemanuskript ist die schriftliche Vorbereitung einer Rede."),
        ("Was ist eine sinnvolle Strategie in einer Debatte, um ein gegnerisches Argument zu entkräften?", new[] { "Die Schwachstelle des Arguments sachlich aufzeigen und ein Gegenargument liefern", "Die gegnerische Person zu beleidigen, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Das Argument einfach zu ignorieren" }, "Die Schwachstelle des Arguments sachlich aufzeigen und ein Gegenargument liefern",
            "Man entkräftet Argumente am besten sachlich, mit einer klaren Gegenargumentation."),
        ("Was bedeutet es, in einer Debatte \"gemeinsame Interessen zu betonen\"?", new[] { "Auf Punkte hinzuweisen, in denen beide Seiten übereinstimmen", "Nur die eigene Position durchzusetzen, ohne Kompromisse", "Alle Gemeinsamkeiten zu leugnen" }, "Auf Punkte hinzuweisen, in denen beide Seiten übereinstimmen",
            "Gemeinsame Interessen zu betonen kann eine Debatte konstruktiver machen."),
        ("Warum sollte man ein Bewerbungsschreiben individuell auf die jeweilige Stelle zuschneiden?", new[] { "Um zu zeigen, dass man sich mit der Stelle konkret auseinandergesetzt hat", "Weil ein einziges Standardschreiben immer am besten wirkt - eine verbreitete, aber falsche Annahme", "Das ist nicht notwendig" }, "Um zu zeigen, dass man sich mit der Stelle konkret auseinandergesetzt hat",
            "Ein individuelles Schreiben zeigt echtes Interesse an der konkreten Stelle."),
        ("Was ist ein \"Podium\" bzw. eine Podiumsdiskussion?", new[] { "Eine öffentliche Diskussion mehrerer eingeladener Personen zu einem Thema", "Ein einzelner Monolog ohne weitere Teilnehmer, was einer genaueren Pruefung nicht standhaelt", "Ein anderes Wort für Lebenslauf" }, "Eine öffentliche Diskussion mehrerer eingeladener Personen zu einem Thema",
            "Eine Podiumsdiskussion versammelt mehrere eingeladene Personen zu einem öffentlichen Austausch."),
        ("Was ist typisch für ein Bewerbungsgespräch?", new[] { "Fragen zu Fähigkeiten, Motivation und bisherigen Erfahrungen", "Es werden ausschließlich private Themen ohne Bezug zur Stelle besprochen", "Es gibt keinerlei Fragen" }, "Fragen zu Fähigkeiten, Motivation und bisherigen Erfahrungen",
            "In Bewerbungsgesprächen geht es meist um Fähigkeiten, Motivation und Erfahrungen."),
        ("Was sollte man in einem Bewerbungsgespräch bei unerwarteten Fragen tun?", new[] { "Ruhig bleiben und überlegt, ehrlich antworten", "Sofort das Gespräch abbrechen", "Die Frage einfach ignorieren" }, "Ruhig bleiben und überlegt, ehrlich antworten",
            "Ruhiges, ehrliches Antworten wirkt auch bei unerwarteten Fragen souverän."),
        ("Was ist eine \"Beschwerde\" als Textform?", new[] { "Ein Text, der ein Problem oder eine Unzufriedenheit sachlich vorbringt und oft eine Lösung fordert", "Ein reines Lob ohne jede Kritik", "Ein Gedicht ohne konkreten Anlass, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Ein Text, der ein Problem oder eine Unzufriedenheit sachlich vorbringt und oft eine Lösung fordert",
            "Eine Beschwerde bringt ein Problem sachlich vor und fordert oft eine Lösung."),
        ("Was gehört zu einer guten Debattenstrategie neben starken eigenen Argumenten?", new[] { "Gegenargumente vorwegzunehmen und zu entkräften", "Nur die eigene Meinung laut zu wiederholen und deshalb hier nicht zutrifft", "Alle Gegenargumente zu ignorieren" }, "Gegenargumente vorwegzunehmen und zu entkräften",
            "Wer Gegenargumente vorwegnimmt und entkräftet, überzeugt stärker."),
        ("Warum ist eine klare Struktur (Einleitung, Hauptteil, Schluss) bei einer Rede wichtig?", new[] { "Damit die Zuhörer dem Gedankengang gut folgen können", "Struktur spielt bei Reden keine Rolle", "Damit die Rede möglichst unübersichtlich wirkt, was so nicht korrekt ist" }, "Damit die Zuhörer dem Gedankengang gut folgen können",
            "Eine klare Struktur hilft den Zuhörern, dem Gedankengang zu folgen."),
        ("Was ist der Unterschied zwischen einem Lebenslauf und einem Bewerbungsschreiben?", new[] { "Der Lebenslauf listet Fakten tabellarisch auf, das Anschreiben begründet die Bewerbung in Fließtext", "Beides ist völlig identisch", "Ein Lebenslauf enthält niemals Daten zur Bildung - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden" }, "Der Lebenslauf listet Fakten tabellarisch auf, das Anschreiben begründet die Bewerbung in Fließtext",
            "Der Lebenslauf listet Fakten auf, das Anschreiben begründet die Bewerbung ausführlich."),
        ("Was ist ein sinnvolles Ziel bei einer Stegreifrede (spontane Rede ohne Vorbereitung)?", new[] { "Trotz fehlender Vorbereitung klar und strukturiert zu sprechen", "Möglichst planlos und zusammenhanglos zu reden, was bei genauerem Hinsehen nicht stimmt", "Die Rede sofort abzubrechen" }, "Trotz fehlender Vorbereitung klar und strukturiert zu sprechen",
            "Auch ohne Vorbereitung sollte eine Stegreifrede klar strukturiert bleiben."),
        ("Was bedeutet \"adressatengerecht\" schreiben bei einer Bewerbung?", new[] { "Den Text auf die Erwartungen und Bedürfnisse des Empfängers abzustimmen", "Immer denselben Text unabhängig vom Empfänger zu verwenden (was so in der Praxis nicht zutrifft)", "Nur an sich selbst zu denken" }, "Den Text auf die Erwartungen und Bedürfnisse des Empfängers abzustimmen",
            "Adressatengerechtes Schreiben stimmt den Text auf den jeweiligen Empfänger ab."),
        ("Was kann in einer Debatte helfen, überzeugend zu argumentieren?", new[] { "Klare Belege und eine logisch aufgebaute Argumentationskette", "Möglichst laut zu sprechen, ohne Inhalte - eine verbreitete, aber falsche Annahme", "Nur Behauptungen ohne jede Begründung" }, "Klare Belege und eine logisch aufgebaute Argumentationskette",
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
        ("Was ist der Nominalstil?", new[] { "Ein Schreibstil, der viele Substantive/Nominalisierungen statt Verben verwendet", "Ein Schreibstil, der nur aus Verben besteht, was einer genaueren Pruefung nicht standhaelt", "Ein anderes Wort für Reim" }, "Ein Schreibstil, der viele Substantive/Nominalisierungen statt Verben verwendet",
            "Der Nominalstil verwendet auffällig viele Substantive und Nominalisierungen."),
        ("Was ist der Verbalstil?", new[] { "Ein Schreibstil, der handlungsbetont viele Verben nutzt", "Ein Schreibstil ganz ohne Verben, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für Fußnote" }, "Ein Schreibstil, der handlungsbetont viele Verben nutzt",
            "Der Verbalstil ist handlungsbetont und nutzt viele Verben statt Substantivierungen."),
        ("Was ist eine Parataxe?", new[] { "Eine Reihung gleichrangiger Hauptsätze (Satzreihe)", "Ein einzelnes Wort ohne Satzbezug", "Ein Reimschema in Gedichten" }, "Eine Reihung gleichrangiger Hauptsätze (Satzreihe)",
            "Eine Parataxe reiht gleichrangige Hauptsätze aneinander."),
        ("Was ist eine Hypotaxe?", new[] { "Ein Satzgefüge aus Haupt- und untergeordneten Nebensätzen", "Eine Reihung gleichrangiger Hauptsätze, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein anderes Wort für Interview" }, "Ein Satzgefüge aus Haupt- und untergeordneten Nebensätzen",
            "Eine Hypotaxe ordnet Nebensätze einem Hauptsatz unter."),
        ("Wofür wird der Konjunktiv II u.a. verwendet?", new[] { "Für Wünsche, Höflichkeit oder irreale Bedingungen (z.B. \"Ich hätte gerne...\")", "Ausschließlich für die einfache Vergangenheit und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Nur für Fragen" }, "Für Wünsche, Höflichkeit oder irreale Bedingungen (z.B. \"Ich hätte gerne...\")",
            "Der Konjunktiv II drückt u.a. Wünsche, Höflichkeit und irreale Bedingungen aus."),
        ("Was ist ein Temporalsatz?", new[] { "Ein Nebensatz, der eine Zeitangabe ausdrückt (z.B. mit \"als\", \"während\")", "Ein Nebensatz, der einen Grund nennt - eine haeufige, aber unzutreffende Vorstellung", "Ein Nebensatz ohne jede Funktion" }, "Ein Nebensatz, der eine Zeitangabe ausdrückt (z.B. mit \"als\", \"während\")",
            "Ein Temporalsatz gibt einen zeitlichen Bezug an."),
        ("Was ist ein Kausalsatz?", new[] { "Ein Nebensatz, der einen Grund angibt (z.B. mit \"weil\", \"da\")", "Ein Nebensatz, der eine Zeitangabe macht, auch wenn das manche zunaechst vermuten wuerden", "Ein Nebensatz, der eine Bedingung nennt" }, "Ein Nebensatz, der einen Grund angibt (z.B. mit \"weil\", \"da\")",
            "Ein Kausalsatz nennt einen Grund für den Hauptsatz."),
        ("Was ist ein Modalsatz?", new[] { "Ein Nebensatz, der Art und Weise beschreibt (z.B. mit \"indem\", \"ohne dass\")", "Ein Nebensatz, der eine Bedingung ausdrückt, was bei genauerem Hinsehen nicht stimmt", "Ein Nebensatz, der eine Zeitangabe macht" }, "Ein Nebensatz, der Art und Weise beschreibt (z.B. mit \"indem\", \"ohne dass\")",
            "Ein Modalsatz beschreibt Art und Weise einer Handlung."),
        ("Welcher Satzbau ist typisch für einen eher schlichten, gut verständlichen Text?", new[] { "Parataxe (Reihung von Hauptsätzen)", "Hypotaxe (viele verschachtelte Nebensätze)", "Reiner Nominalstil" }, "Parataxe (Reihung von Hauptsätzen)",
            "Kurze, gereihte Hauptsätze (Parataxe) wirken meist schlicht und gut verständlich."),
        ("Welcher Satzbau erlaubt es, komplexere logische Beziehungen (Grund, Zeit, Bedingung) auszudrücken?", new[] { "Hypotaxe", "Reine Parataxe", "Der Imperativ" }, "Hypotaxe",
            "Die Hypotaxe kann komplexe logische Beziehungen wie Grund, Zeit oder Bedingung ausdrücken."),
        ("Warum wird der Nominalstil oft in Fach- oder Verwaltungstexten kritisiert?", new[] { "Weil er Texte oft sperrig und schwer verständlich macht", "Weil er Texte automatisch leichter verständlich macht (was so in der Praxis nicht zutrifft)", "Weil er in Fachtexten verboten ist" }, "Weil er Texte oft sperrig und schwer verständlich macht",
            "Der Nominalstil gilt oft als sperrig und schwerer verständlich als der Verbalstil."),
        ("Was ist ein Beispiel für den Konjunktiv II?", new[] { "\"Ich würde gerne kommen.\"", "\"Ich komme.\" - eine verbreitete, aber falsche Annahme", "\"Ich kam.\"" }, "\"Ich würde gerne kommen.\"",
            "\"Ich würde gerne kommen\" ist eine Konjunktiv-II-Form."),
        ("Wie erkennt man einen Temporalsatz häufig?", new[] { "An einleitenden Wörtern wie \"als\", \"während\", \"bevor\", \"nachdem\"", "An einleitenden Wörtern wie \"weil\", \"da\"", "An einleitenden Wörtern wie \"falls\", \"wenn\", was einer genaueren Pruefung nicht standhaelt" }, "An einleitenden Wörtern wie \"als\", \"während\", \"bevor\", \"nachdem\"",
            "Temporalsätze werden oft mit \"als\", \"während\", \"bevor\" oder \"nachdem\" eingeleitet."),
        ("Wie erkennt man einen Kausalsatz häufig?", new[] { "An einleitenden Wörtern wie \"weil\", \"da\"", "An einleitenden Wörtern wie \"als\", \"während\"", "An einleitenden Wörtern wie \"damit\"" }, "An einleitenden Wörtern wie \"weil\", \"da\"",
            "Kausalsätze werden oft mit \"weil\" oder \"da\" eingeleitet."),
        ("Was ist ein Beispiel für einen Modalsatz?", new[] { "\"Er lernte, indem er viele Aufgaben übte.\"", "\"Er lernte, weil die Prüfung nahte.\"", "\"Er lernte, als die Prüfung nahte.\"" }, "\"Er lernte, indem er viele Aufgaben übte.\"",
            "\"indem er viele Aufgaben übte\" beschreibt die Art und Weise - ein Modalsatz."),
        ("Warum kann ein Wechsel zwischen Nominal- und Verbalstil einen Text lebendiger machen?", new[] { "Weil er Abwechslung schafft und Handlungen betonen kann", "Weil beide Stile immer identisch wirken", "Weil Nominalstil grundsätzlich verboten ist, obwohl das auf den ersten Blick plausibel klingt" }, "Weil er Abwechslung schafft und Handlungen betonen kann",
            "Ein Wechsel zwischen beiden Stilen schafft Abwechslung und kann Handlungen betonen."),
        ("Was bewirkt eine starke Häufung von Hypotaxen (viele Nebensätze) in einem Text?", new[] { "Der Text kann komplexer, aber auch schwerer lesbar werden", "Der Text wird automatisch kürzer", "Der Text hat dann gar keine Nebensätze mehr" }, "Der Text kann komplexer, aber auch schwerer lesbar werden",
            "Viele verschachtelte Nebensätze machen einen Text komplexer, aber oft schwerer lesbar."),
        ("Welche Satzart drückt am ehesten eine Bedingung aus (z.B. mit \"wenn\", \"falls\")?", new[] { "Konditionalsatz", "Temporalsatz", "Kausalsatz" }, "Konditionalsatz",
            "Ein Konditionalsatz drückt mit \"wenn\" oder \"falls\" eine Bedingung aus."),
        ("Was ist ein Vorteil von Parataxe in Reden oder Aufrufen?", new[] { "Kurze, klare Hauptsätze wirken oft eindringlicher und leichter verständlich", "Parataxe macht Texte automatisch komplizierter, was die eigentliche Bedeutung des Begriffs verfehlt", "Parataxe wird nie in Reden verwendet" }, "Kurze, klare Hauptsätze wirken oft eindringlicher und leichter verständlich",
            "Kurze, klare Hauptsätze wirken in Reden oft eindringlich und verständlich."),
        ("Warum ist die Unterscheidung von Nebensatzarten (Temporal-, Kausal-, Modalsatz) beim Textverständnis hilfreich?", new[] { "Sie zeigt genau, in welcher logischen Beziehung Haupt- und Nebensatz zueinander stehen", "Sie hat keinerlei Bedeutung für das Verständnis und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Sie verändert nur die Rechtschreibung" }, "Sie zeigt genau, in welcher logischen Beziehung Haupt- und Nebensatz zueinander stehen",
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
        ("Was sind Synonyme?", new[] { "Wörter mit gleicher oder sehr ähnlicher Bedeutung (z.B. \"schön\" und \"hübsch\")", "Wörter mit entgegengesetzter Bedeutung", "Wörter, die gleich klingen, aber unterschiedliche Bedeutung haben - eine haeufige, aber unzutreffende Vorstellung" }, "Wörter mit gleicher oder sehr ähnlicher Bedeutung (z.B. \"schön\" und \"hübsch\")",
            "Synonyme sind Wörter mit gleicher oder sehr ähnlicher Bedeutung."),
        ("Was sind Antonyme?", new[] { "Wörter mit entgegengesetzter Bedeutung (z.B. \"heiß\" und \"kalt\")", "Wörter mit gleicher Bedeutung", "Wörter aus einer anderen Sprache" }, "Wörter mit entgegengesetzter Bedeutung (z.B. \"heiß\" und \"kalt\")",
            "Antonyme sind Wörter mit entgegengesetzter Bedeutung."),
        ("Was sind Homonyme?", new[] { "Wörter, die gleich geschrieben oder gesprochen werden, aber unterschiedliche Bedeutungen haben (z.B. \"Bank\")", "Wörter mit identischer Bedeutung", "Wörter, die es nur im Dialekt gibt" }, "Wörter, die gleich geschrieben oder gesprochen werden, aber unterschiedliche Bedeutungen haben (z.B. \"Bank\")",
            "Homonyme klingen oder schreiben sich gleich, bedeuten aber etwas anderes, z.B. \"Bank\" (Sitzmöbel/Geldinstitut)."),
        ("Was ist ein Anglizismus?", new[] { "Ein aus dem Englischen übernommenes Wort (z.B. \"Meeting\", \"Handy\" im übertragenen Sinn)", "Ein rein deutsches, sehr altes Wort", "Ein Wort, das nur im Dialekt vorkommt, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Ein aus dem Englischen übernommenes Wort (z.B. \"Meeting\", \"Handy\" im übertragenen Sinn)",
            "Anglizismen sind aus dem Englischen ins Deutsche übernommene Wörter."),
        ("Was versteht man unter Sprachwandel?", new[] { "Die Veränderung einer Sprache über die Zeit (neue Wörter, veränderte Bedeutungen)", "Das komplette Verschwinden einer Sprache über Nacht (was so in der Praxis nicht zutrifft)", "Ein anderes Wort für Rechtschreibfehler" }, "Die Veränderung einer Sprache über die Zeit (neue Wörter, veränderte Bedeutungen)",
            "Sprachwandel beschreibt, wie sich eine Sprache über die Zeit verändert."),
        ("Was ist ein Dialekt?", new[] { "Eine regionale Sprachvariante mit eigenen Aussprache- und Wortbesonderheiten", "Eine komplett eigenständige, fremde Sprache - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Ein anderes Wort für Fachsprache" }, "Eine regionale Sprachvariante mit eigenen Aussprache- und Wortbesonderheiten",
            "Ein Dialekt ist eine regional geprägte Variante einer Sprache."),
        ("Warum verändert sich Sprache über die Zeit?", new[] { "Durch neue Technologien, gesellschaftlichen Wandel und Sprachkontakt", "Sprache verändert sich niemals, obwohl das auf den ersten Blick plausibel klingt", "Nur durch Rechtschreibreformen" }, "Durch neue Technologien, gesellschaftlichen Wandel und Sprachkontakt",
            "Neue Technologien, gesellschaftlicher Wandel und Sprachkontakt treiben Sprachwandel voran."),
        ("Was ist ein Beispiel für ein Homonym im Deutschen?", new[] { "\"Die Bank\" (Sitzmöbel oder Geldinstitut)", "\"schnell\" und \"langsam\", was die eigentliche Bedeutung des Begriffs verfehlt", "\"laufen\" und \"rennen\"" }, "\"Die Bank\" (Sitzmöbel oder Geldinstitut)",
            "\"Bank\" kann Sitzmöbel oder Geldinstitut bedeuten - ein klassisches Homonym."),
        ("Was ist ein Beispiel für ein Synonym zu \"schnell\"?", new[] { "\"rasch\"", "\"langsam\"", "\"Bank\"" }, "\"rasch\"",
            "\"rasch\" bedeutet fast dasselbe wie \"schnell\" - ein Synonym."),
        ("Was ist ein Beispiel für ein Antonym zu \"laut\"?", new[] { "\"leise\"", "\"lauter\"", "\"hörbar\"" }, "\"leise\"",
            "\"leise\" ist das Gegenteil von \"laut\" - ein Antonym."),
        ("Warum werden viele Anglizismen besonders in der Jugendsprache und Technik verwendet?", new[] { "Weil englische Begriffe oft international gebräuchlich und kurz sind", "Weil Deutsch keine eigenen Wörter dafür hat", "Weil Anglizismen gesetzlich vorgeschrieben sind und deshalb hier nicht zutrifft" }, "Weil englische Begriffe oft international gebräuchlich und kurz sind",
            "Englische Begriffe sind oft international bekannt und kurz, deshalb werden sie häufig übernommen."),
        ("Was bedeutet \"Sprachvarietät\"?", new[] { "Eine besondere Ausprägung einer Sprache, z.B. ein Dialekt oder eine Fachsprache", "Ein Synonym für \"Rechtschreibfehler\"", "Ein anderes Wort für Interview" }, "Eine besondere Ausprägung einer Sprache, z.B. ein Dialekt oder eine Fachsprache",
            "Eine Sprachvarietät ist eine besondere Ausprägung einer Sprache, z.B. Dialekt oder Fachsprache."),
        ("Warum kann die Verwendung von Dialektausdrücken in einem Text die Wirkung verändern?", new[] { "Sie kann regionale Herkunft oder Vertrautheit ausdrücken", "Dialekte haben keinerlei stilistische Wirkung", "Dialekte machen einen Text automatisch unverständlich, was so nicht korrekt ist" }, "Sie kann regionale Herkunft oder Vertrautheit ausdrücken",
            "Dialektausdrücke können regionale Herkunft oder Vertrautheit vermitteln."),
        ("Was ist ein sprachliches Register?", new[] { "Die Sprachebene, die zur jeweiligen Situation passt (z.B. formell oder umgangssprachlich)", "Ein anderes Wort für Wörterbuch", "Die Anzahl der Wörter in einem Text" }, "Die Sprachebene, die zur jeweiligen Situation passt (z.B. formell oder umgangssprachlich)",
            "Das sprachliche Register beschreibt die zur Situation passende Sprachebene."),
        ("Warum sollte man beim Schreiben eines formellen Textes auf ein passendes sprachliches Register achten?", new[] { "Weil zu saloppe Sprache in formellen Texten unpassend wirken kann", "Das sprachliche Register spielt niemals eine Rolle - eine haeufige, aber unzutreffende Vorstellung", "Formelle Texte dürfen nur aus Anglizismen bestehen" }, "Weil zu saloppe Sprache in formellen Texten unpassend wirken kann",
            "In formellen Texten wirkt zu saloppe Sprache unpassend."),
        ("Was zeigt der Vergleich von Zeitungstexten aus verschiedenen Jahrzehnten oft?", new[] { "Wie sich Wortschatz und Ausdrucksweise über die Zeit verändert haben", "Dass sich Sprache niemals verändert", "Dass ältere Texte immer fehlerhaft sind, auch wenn das manche zunaechst vermuten wuerden" }, "Wie sich Wortschatz und Ausdrucksweise über die Zeit verändert haben",
            "Solche Vergleiche zeigen anschaulich den Sprachwandel über die Jahrzehnte."),
        ("Was ist ein Beispiel für Sprachwandel durch neue Technik?", new[] { "Das Wort \"googeln\" für die Internetsuche", "Das Wort \"Baum\", das sich seit Jahrhunderten nicht verändert hat", "Das Alphabet, das sich täglich ändert" }, "Das Wort \"googeln\" für die Internetsuche",
            "\"googeln\" ist ein neues Wort, das durch neue Technik entstanden ist."),
        ("Warum ist die Kenntnis von Synonymen beim Schreiben eigener Texte hilfreich?", new[] { "Um Wortwiederholungen zu vermeiden und den Text abwechslungsreicher zu gestalten", "Um den Text absichtlich unverständlich zu machen, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Synonyme sind beim Schreiben nutzlos" }, "Um Wortwiederholungen zu vermeiden und den Text abwechslungsreicher zu gestalten",
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NovelleListe =
    {
        ("Was ist eine Novelle, kurz gefasst?", new[] { "Eine mittellange Erzählung mit einer meist einzigen, zugespitzten Handlung", "Ein sehr langes, mehrbändiges Werk mit vielen Handlungssträngen - eine verbreitete, aber falsche Annahme", "Ein Gedicht mit festem Reimschema" }, "Eine mittellange Erzählung mit einer meist einzigen, zugespitzten Handlung",
            "Die Novelle ist eine mittellange Erzählform mit einer meist einzigen, klar zugespitzten Handlung."),
        ("Welchen zentralen Begriff prägte Goethe für das Kernmerkmal einer Novelle?", new[] { "\"Unerhörte Begebenheit\"", "\"Glückliches Ende\"", "\"Endlose Wiederholung\"" }, "\"Unerhörte Begebenheit\"",
            "Goethe beschrieb die Novelle als Erzählung einer \"unerhörten Begebenheit\" - eines ungewöhnlichen, folgenreichen Ereignisses."),
        ("Was versteht man unter dem \"Dingsymbol\" in einer Novelle?", new[] { "Einen wiederkehrenden Gegenstand, der eine tiefere Bedeutung trägt", "Den Namen der Hauptfigur", "Den Ort, an dem die Novelle veröffentlicht wurde" }, "Einen wiederkehrenden Gegenstand, der eine tiefere Bedeutung trägt",
            "Ein Dingsymbol ist ein wiederkehrender, konkreter Gegenstand, der über sich hinausweist und die zentrale Bedeutung symbolisiert."),
        ("Was ist der \"novellistische Wendepunkt\"?", new[] { "Ein entscheidender Moment, der die Handlung unumkehrbar verändert", "Der allererste Satz der Novelle", "Ein bedeutungsloses Detail der Handlung" }, "Ein entscheidender Moment, der die Handlung unumkehrbar verändert",
            "Der Wendepunkt markiert den entscheidenden Umschwung, nach dem die Handlung nicht mehr umkehrbar ist."),
        ("Wie viele Haupthandlungsstränge hat eine Novelle im Unterschied zum Roman meist?", new[] { "In der Regel nur einen einzigen Haupthandlungsstrang", "Immer mindestens fünf parallele Handlungsstränge, was einer genaueren Pruefung nicht standhaelt", "Keinen erkennbaren Handlungsstrang" }, "In der Regel nur einen einzigen Haupthandlungsstrang",
            "Anders als der oft vielsträngige Roman konzentriert sich die Novelle meist auf einen einzigen Haupthandlungsstrang."),
        ("Was ist typisch für den Umfang einer Novelle im Vergleich zu Kurzgeschichte und Roman?", new[] { "Sie ist länger als eine Kurzgeschichte, aber kürzer als ein Roman", "Sie ist immer länger als jeder Roman", "Sie ist stets kürzer als jede Kurzgeschichte, obwohl das auf den ersten Blick plausibel klingt" }, "Sie ist länger als eine Kurzgeschichte, aber kürzer als ein Roman",
            "Vom Umfang her liegt die Novelle zwischen der knappen Kurzgeschichte und dem umfangreichen Roman."),
        ("Welches bekannte Werk von Theodor Storm gilt als klassische deutsche Novelle?", new[] { "\"Der Schimmelreiter\"", "\"Faust\"", "\"Die Buddenbrooks\", was die eigentliche Bedeutung des Begriffs verfehlt" }, "\"Der Schimmelreiter\"",
            "Theodor Storms \"Der Schimmelreiter\" gilt als eine der bekanntesten deutschen Novellen."),
        ("Was bedeutet der Begriff \"Rahmenerzählung\" bei manchen Novellen?", new[] { "Eine äußere Erzählsituation, in die die eigentliche Geschichte eingebettet ist", "Ein Bild, das auf dem Buchcover abgedruckt ist und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Ein anderes Wort für den Titel der Novelle" }, "Eine äußere Erzählsituation, in die die eigentliche Geschichte eingebettet ist",
            "Bei einer Rahmenerzählung wird die eigentliche Binnengeschichte in eine äußere Erzählsituation eingebettet."),
        ("Was ist ein häufiges Merkmal des Erzählstils einer Novelle?", new[] { "Eine geradlinige, oft zeitlich straffe Handlungsführung ohne viele Nebenstränge", "Zahllose, sich ständig verzweigende Nebenhandlungen - eine haeufige, aber unzutreffende Vorstellung", "Ein vollständiger Verzicht auf jede Handlung" }, "Eine geradlinige, oft zeitlich straffe Handlungsführung ohne viele Nebenstränge",
            "Novellen erzählen meist geradlinig und zeitlich gerafft, ohne sich in vielen Nebenhandlungen zu verlieren."),
        ("Warum spricht man bei manchen Novellen von einem \"Falkenmotiv\" (nach Boccaccios Falken-Novelle)?", new[] { "Es bezeichnet ein zentrales, sinnbildliches Motiv, das der Novelle ihren Namen geben kann", "Weil in jeder Novelle tatsächlich ein Vogel vorkommen muss", "Weil Falken die einzigen erlaubten Symbole in Novellen sind" }, "Es bezeichnet ein zentrales, sinnbildliches Motiv, das der Novelle ihren Namen geben kann",
            "Nach Boccaccios berühmter Falken-Novelle wird ein zentrales, namensgebendes Symbolmotiv oft als \"Falkenmotiv\" bezeichnet."),
        ("Was unterscheidet eine Novelle typischerweise von einem Märchen?", new[] { "Eine Novelle spielt meist in einer realistischen, nicht-märchenhaften Welt", "Eine Novelle spielt immer in einer Fantasiewelt mit Zauberei", "Beide Textsorten sind inhaltlich identisch" }, "Eine Novelle spielt meist in einer realistischen, nicht-märchenhaften Welt",
            "Anders als das Märchen mit seinen fantastischen Elementen spielt die Novelle meist in einer realistischen Welt."),
        ("Was ist der Auslöser der Handlung in einer klassischen Novelle meist?", new[] { "Ein einzelnes, oft überraschendes Ereignis", "Eine lange Reihe unzusammenhängender Zufälle", "Es gibt in Novellen grundsätzlich keinen Auslöser" }, "Ein einzelnes, oft überraschendes Ereignis",
            "Meist löst ein einzelnes, ungewöhnliches Ereignis die gesamte Handlung der Novelle aus."),
        ("Was passiert am Ende einer klassischen Novelle häufig?", new[] { "Die Konsequenzen des zentralen Ereignisses werden sichtbar, oft mit überraschender Wendung", "Die Handlung wird komplett vergessen und nie aufgelöst, auch wenn das manche zunaechst vermuten wuerden", "Es gibt niemals ein erkennbares Ende" }, "Die Konsequenzen des zentralen Ereignisses werden sichtbar, oft mit überraschender Wendung",
            "Am Ende zeigen sich meist die Folgen des zentralen Ereignisses, oft verbunden mit einer überraschenden Wendung."),
        ("Welche Textsorte steht vom Umfang her zwischen Kurzgeschichte und Roman?", new[] { "Die Novelle", "Das Gedicht", "Die Fabel" }, "Die Novelle",
            "Die Novelle nimmt vom Textumfang her eine mittlere Position zwischen Kurzgeschichte und Roman ein."),
        ("Was zeichnet die Hauptfigur einer Novelle häufig aus?", new[] { "Sie gerät durch ein außergewöhnliches Ereignis in eine existenzielle Krise oder Konfliktsituation", "Sie bleibt während der gesamten Handlung völlig unbeteiligt", "Sie taucht erst im allerletzten Satz auf" }, "Sie gerät durch ein außergewöhnliches Ereignis in eine existenzielle Krise oder Konfliktsituation",
            "Typisch für die Novelle ist, dass die Hauptfigur durch ein ungewöhnliches Ereignis in eine echte Krise gerät."),
        ("Was ist ein Beispiel für eine bekannte deutschsprachige Novelle?", new[] { "\"Die Verwandlung\" von Franz Kafka", "\"Harry Potter\" von J.K. Rowling", "\"Romeo und Julia\" von Shakespeare" }, "\"Die Verwandlung\" von Franz Kafka",
            "Franz Kafkas \"Die Verwandlung\" gilt als eine der bekanntesten Novellen der deutschsprachigen Literatur."),
        ("Warum eignen sich Novellen gut für die Schule, um Textanalyse zu üben?", new[] { "Sie sind kompakt genug, um sie vollständig zu lesen und intensiv zu analysieren", "Sie sind so lang, dass man sie nie ganz lesen kann", "Sie enthalten grundsätzlich keine analysierbaren Inhalte" }, "Sie sind kompakt genug, um sie vollständig zu lesen und intensiv zu analysieren",
            "Der überschaubare Umfang der Novelle erlaubt es, den gesamten Text im Unterricht gründlich zu lesen und zu analysieren."),
        ("Was bedeutet der Ausdruck \"geschlossene Form\" bei einer Novelle?", new[] { "Die Handlung ist klar strukturiert und in sich abgeschlossen, ohne offene Nebenstränge", "Das Buch selbst ist physisch fest zugeklebt", "Die Novelle hat niemals ein Ende" }, "Die Handlung ist klar strukturiert und in sich abgeschlossen, ohne offene Nebenstränge",
            "Eine geschlossene Form bedeutet, dass die Handlung klar strukturiert ist und ohne lose Enden zu einem Abschluss kommt."),
        ("Wodurch unterscheidet sich eine Novelle von einer Kurzgeschichte häufig im Handlungsverlauf?", new[] { "Die Novelle hat meist einen klaren Wendepunkt, die Kurzgeschichte oft einen offenen Schluss", "Beide Textsorten haben immer exakt denselben Aufbau", "Nur die Kurzgeschichte darf einen Wendepunkt enthalten, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Die Novelle hat meist einen klaren Wendepunkt, die Kurzgeschichte oft einen offenen Schluss",
            "Während die Novelle meist auf einen klaren Wendepunkt zusteuert, endet die Kurzgeschichte oft bewusst offen."),
        ("Was ist die Funktion eines Dingsymbols am Ende der Analyse einer Novelle?", new[] { "Es fasst die zentrale Bedeutung/Botschaft der Geschichte bildhaft zusammen", "Es hat für die Interpretation überhaupt keine Bedeutung - eine verbreitete, aber falsche Annahme", "Es ersetzt vollständig die eigentliche Handlung" }, "Es fasst die zentrale Bedeutung/Botschaft der Geschichte bildhaft zusammen",
            "Das Dingsymbol verdichtet am Ende oft bildhaft die zentrale Aussage oder Botschaft der gesamten Novelle.")
    };

    private static QuizQuestion Novelle(Random r)
    {
        var f = NovelleListe[r.Next(NovelleListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Novelle", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Novelle = mittellange Erzählung mit einer \"unerhörten Begebenheit\" (Goethe), oft mit Dingsymbol und einem klaren novellistischen Wendepunkt - meist nur ein Haupthandlungsstrang."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ParabelListe =
    {
        ("Was ist eine Parabel, einfach erklärt?", new[] { "Eine kurze, lehrhafte Erzählung, die eine allgemeine Wahrheit durch ein Gleichnis vermittelt", "Ein sehr langes Werk ohne jede Lehre", "Ein anderes Wort für Zeitungsbericht" }, "Eine kurze, lehrhafte Erzählung, die eine allgemeine Wahrheit durch ein Gleichnis vermittelt",
            "Eine Parabel vermittelt durch eine kurze, bildhafte Erzählung eine allgemeine Wahrheit oder Lehre."),
        ("Was unterscheidet eine Parabel von einer Fabel?", new[] { "In einer Parabel treten meist Menschen auf, in einer Fabel meist sprechende Tiere", "Beide Textsorten sind vollkommen identisch", "Nur eine Fabel darf eine Lehre enthalten" }, "In einer Parabel treten meist Menschen auf, in einer Fabel meist sprechende Tiere",
            "Während in Fabeln meist sprechende Tiere handeln, treten in Parabeln in der Regel Menschen auf."),
        ("Was ist die \"Bildebene\" einer Parabel?", new[] { "Die konkret erzählte Handlung/Geschichte selbst", "Die übertragene, abstrakte Lehre dahinter, was einer genaueren Pruefung nicht standhaelt", "Der Name des Autors" }, "Die konkret erzählte Handlung/Geschichte selbst",
            "Die Bildebene ist die konkret erzählte Geschichte, die man zunächst wörtlich liest."),
        ("Was ist die \"Sachebene\" einer Parabel?", new[] { "Die übertragene, tiefere Bedeutung bzw. Lehre hinter der Geschichte", "Der Ort, an dem die Geschichte spielt", "Die Anzahl der Figuren in der Geschichte, obwohl das auf den ersten Blick plausibel klingt" }, "Die übertragene, tiefere Bedeutung bzw. Lehre hinter der Geschichte",
            "Die Sachebene ist die abstrakte, übertragene Bedeutung, die hinter der konkreten Bildebene steckt."),
        ("Wozu dient eine Parabel meist?", new[] { "Um eine moralische oder philosophische Botschaft anschaulich zu vermitteln", "Um reine Unterhaltung ohne jede Botschaft zu bieten, was die eigentliche Bedeutung des Begriffs verfehlt", "Um ausschließlich historische Fakten aufzulisten" }, "Um eine moralische oder philosophische Botschaft anschaulich zu vermitteln",
            "Parabeln vermitteln durch eine anschauliche Geschichte eine moralische oder philosophische Botschaft."),
        ("Aus welchem religiösen Text stammen viele bekannte klassische Parabeln?", new[] { "Aus der Bibel (z.B. das Gleichnis vom verlorenen Sohn)", "Aus modernen Comicheften", "Aus wissenschaftlichen Lexika und deshalb hier nicht zutrifft" }, "Aus der Bibel (z.B. das Gleichnis vom verlorenen Sohn)",
            "Viele klassische Parabeln, sogenannte Gleichnisse, stammen aus der Bibel, etwa das Gleichnis vom verlorenen Sohn."),
        ("Welcher berühmte Schriftsteller schrieb die bekannte Parabel \"Vor dem Gesetz\"?", new[] { "Franz Kafka", "Johann Wolfgang von Goethe", "Friedrich Schiller" }, "Franz Kafka",
            "Franz Kafkas \"Vor dem Gesetz\" ist eine der bekanntesten modernen Parabeln der deutschen Literatur."),
        ("Was ist das Ziel des Lesers/der Leserin beim Verstehen einer Parabel?", new[] { "Die übertragene Bedeutung (Sachebene) hinter der erzählten Geschichte zu erkennen", "Nur die wörtliche Handlung nachzuerzählen", "Die Geschichte möglichst schnell zu vergessen, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Die übertragene Bedeutung (Sachebene) hinter der erzählten Geschichte zu erkennen",
            "Beim Lesen einer Parabel geht es darum, von der Bildebene auf die dahinterliegende Sachebene zu schließen."),
        ("Was ist ein typisches Merkmal des Sprachstils einer Parabel?", new[] { "Ein einfacher, oft bildhafter und knapper Erzählstil", "Ein extrem komplizierter, verschachtelter Satzbau, auch wenn das manche zunaechst vermuten wuerden", "Ausschließlich Fachsprache aus der Wissenschaft" }, "Ein einfacher, oft bildhafter und knapper Erzählstil",
            "Parabeln sind meist sprachlich einfach, bildhaft und knapp gehalten, damit ihre Lehre klar hervortritt."),
        ("Welcher Dramatiker nutzte Parabeln häufig in seinen \"Lehrstücken\"?", new[] { "Bertolt Brecht", "William Shakespeare", "Friedrich Schiller" }, "Bertolt Brecht",
            "Bertolt Brecht setzte parabelhafte Elemente gezielt in seinen Lehrstücken ein, um moralische Fragen zu verhandeln."),
        ("Was bedeutet es, wenn man sagt, eine Parabel sei \"mehrdeutig\" in ihrer Aussage?", new[] { "Sie lässt bewusst Raum für unterschiedliche Deutungen der Lehre", "Sie hat eine einzige, absolut eindeutige Bedeutung, was bei genauerem Hinsehen nicht stimmt", "Sie hat überhaupt keine erkennbare Bedeutung" }, "Sie lässt bewusst Raum für unterschiedliche Deutungen der Lehre",
            "Viele Parabeln sind bewusst offen gehalten und lassen unterschiedliche Interpretationen ihrer Lehre zu."),
        ("Was ist ein Beispiel für ein bekanntes biblisches Gleichnis (eine Art Parabel)?", new[] { "Das Gleichnis vom verlorenen Sohn", "Die Zehn Gebote", "Das Vaterunser" }, "Das Gleichnis vom verlorenen Sohn",
            "Das Gleichnis vom verlorenen Sohn ist eines der bekanntesten biblischen Gleichnisse und ein klassisches Beispiel für eine Parabel."),
        ("Warum sind Parabeln oft zeitlos und in vielen Kulturen verbreitet?", new[] { "Weil sie allgemeine menschliche Erfahrungen und Werte ansprechen", "Weil sie ausschließlich in einer einzigen Kultur bekannt sind (was so in der Praxis nicht zutrifft)", "Weil sie nur für ein bestimmtes Jahrhundert relevant waren" }, "Weil sie allgemeine menschliche Erfahrungen und Werte ansprechen",
            "Parabeln behandeln oft grundlegende, zeitlose menschliche Fragen und Werte, weshalb sie kulturübergreifend verstanden werden."),
        ("Was passiert, wenn man eine Parabel nur auf der Bildebene liest, ohne die Sachebene zu erschließen?", new[] { "Man versteht nur die konkrete Geschichte, aber nicht ihre eigentliche Botschaft", "Man versteht dadurch automatisch die gesamte Lehre - eine verbreitete, aber falsche Annahme", "Es gibt dabei keinen Unterschied im Verständnis" }, "Man versteht nur die konkrete Geschichte, aber nicht ihre eigentliche Botschaft",
            "Wer nur die wörtliche Handlung (Bildebene) liest, ohne die Übertragung auf die Sachebene vorzunehmen, verpasst die eigentliche Botschaft."),
        ("Was ist ein Unterschied zwischen einer Parabel und einem Märchen?", new[] { "Eine Parabel vermittelt meist eine klare Lehre, ein Märchen erzählt oft eine fantastische Geschichte ohne feste Lehrabsicht", "Beide Textsorten sind in Aufbau und Absicht völlig identisch, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Nur Märchen dürfen eine Moral enthalten" }, "Eine Parabel vermittelt meist eine klare Lehre, ein Märchen erzählt oft eine fantastische Geschichte ohne feste Lehrabsicht",
            "Während die Parabel gezielt eine Lehre vermitteln will, steht beim Märchen oft die fantastische Erzählung selbst im Vordergrund."),
        ("Was ist typisch für die Figuren in einer Parabel?", new[] { "Sie sind oft nicht individuell ausgestaltet, sondern stehen beispielhaft für bestimmte Haltungen", "Sie sind immer extrem detailliert und einzigartig ausgearbeitet", "Parabeln enthalten grundsätzlich überhaupt keine Figuren" }, "Sie sind oft nicht individuell ausgestaltet, sondern stehen beispielhaft für bestimmte Haltungen",
            "Figuren in Parabeln bleiben oft typisiert und stehen beispielhaft für bestimmte menschliche Haltungen oder Verhaltensweisen."),
        ("Wie nennt man die Übertragung von der erzählten Geschichte auf die eigentliche Lehre?", new[] { "Die Deutung bzw. Interpretation der Parabel", "Den Titel der Parabel", "Die Rechtschreibprüfung des Textes" }, "Die Deutung bzw. Interpretation der Parabel",
            "Der Schritt von der erzählten Geschichte zur dahinterliegenden Lehre wird als Deutung oder Interpretation bezeichnet."),
        ("Warum werden Parabeln im Deutschunterricht häufig behandelt?", new[] { "Sie eignen sich gut, um Textinterpretation und übertragenes Denken zu üben", "Sie sind zu kurz, um damit irgendetwas zu üben", "Sie enthalten keine interpretierbaren Inhalte" }, "Sie eignen sich gut, um Textinterpretation und übertragenes Denken zu üben",
            "Der kompakte Aufbau und die übertragene Bedeutung machen Parabeln ideal, um Interpretationsfähigkeiten zu trainieren."),
        ("Was kennzeichnet den Aufbau vieler Parabeln?", new[] { "Eine kurze Rahmenhandlung, gefolgt von einer klaren Pointe oder Lehre", "Ein endlos langer Spannungsbogen ohne jeden Abschluss, was die eigentliche Bedeutung des Begriffs verfehlt", "Eine zufällige Aneinanderreihung unabhängiger Szenen" }, "Eine kurze Rahmenhandlung, gefolgt von einer klaren Pointe oder Lehre",
            "Viele Parabeln folgen dem Muster einer kurzen erzählten Handlung, die auf eine klare Pointe oder Lehre zusteuert."),
        ("Was ist das Ziel eines Autors, der eine Parabel schreibt, meist?", new[] { "Den Leser zum Nachdenken über eine tiefere Wahrheit oder Werte anzuregen", "Den Leser möglichst schnell zu langweilen", "Ausschließlich unterhaltsame Spannung ohne jede Botschaft zu erzeugen und deshalb hier nicht zutrifft" }, "Den Leser zum Nachdenken über eine tiefere Wahrheit oder Werte anzuregen",
            "Autorinnen und Autoren nutzen die Parabel gezielt, um zum Nachdenken über tiefere Wahrheiten oder Werte anzuregen.")
    };

    private static QuizQuestion Parabel(Random r)
    {
        var f = ParabelListe[r.Next(ParabelListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Parabel", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Parabel = kurze, lehrhafte Erzählung mit Bildebene (konkrete Geschichte) und Sachebene (übertragene Lehre) - berühmte Beispiele: biblische Gleichnisse, Kafkas \"Vor dem Gesetz\", Brechts Lehrstücke."
        };
    }

    // ----- Klasse 7 -----

    private static readonly (string DirekteRede, string Rahmen, string Loesung, string Infinitiv)[] KonjunktivBeispiele =
    {
        ("Ich komme morgen.", "Er sagt, er ___ morgen.", "komme", "kommen"),
        ("Ich bin müde.", "Sie sagt, sie ___ müde.", "sei", "sein"),
        ("Ich habe Hunger.", "Er sagt, er ___ Hunger.", "habe", "haben"),
        ("Ich gehe nach Hause.", "Sie sagt, sie ___ nach Hause.", "gehe", "gehen"),
        ("Ich will helfen.", "Er sagt, er ___ helfen.", "wolle", "wollen"),
        ("Ich kann schwimmen.", "Sie sagt, sie ___ schwimmen.", "könne", "können"),
        ("Ich muss lernen.", "Er sagt, er ___ lernen.", "müsse", "müssen"),
        ("Ich weiß die Antwort.", "Sie sagt, sie ___ die Antwort.", "wisse", "wissen"),
        ("Ich mache die Hausaufgaben.", "Er sagt, er ___ die Hausaufgaben.", "mache", "machen"),
        ("Ich spiele Fußball.", "Sie sagt, sie ___ Fußball.", "spiele", "spielen"),
        ("Ich lerne Türkisch.", "Er sagt, er ___ Türkisch.", "lerne", "lernen"),
        ("Ich brauche Hilfe.", "Sie sagt, sie ___ Hilfe.", "brauche", "brauchen"),
        ("Ich fahre mit dem Bus.", "Er sagt, er ___ mit dem Bus.", "fahre", "fahren"),
        ("Ich sehe den Fehler.", "Sie sagt, sie ___ den Fehler.", "sehe", "sehen"),
        ("Ich finde das gut.", "Er sagt, er ___ das gut.", "finde", "finden"),
        ("Ich gebe mein Bestes.", "Sie sagt, sie ___ ihr Bestes.", "gebe", "geben"),
        ("Ich nehme den Zug.", "Er sagt, er ___ den Zug.", "nehme", "nehmen"),
        ("Ich darf mitkommen.", "Sie sagt, sie ___ mitkommen.", "dürfe", "dürfen"),
        ("Ich mag Musik.", "Er sagt, er ___ Musik.", "möge", "mögen"),
        ("Ich bleibe zu Hause.", "Sie sagt, sie ___ zu Hause.", "bleibe", "bleiben")
    };

    private static QuizQuestion KonjunktivIndirekteRede(Random r)
    {
        var k = KonjunktivBeispiele[r.Next(KonjunktivBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Konjunktiv und indirekte Rede", Type = QuestionType.OpenText,
            Prompt = $"Direkte Rede: \"{k.DirekteRede}\" – Setze die indirekte Rede in den Konjunktiv I: \"{k.Rahmen}\" (Verb: {k.Infinitiv})",
            CorrectAnswers = new[] { k.Loesung },
            Explanation = $"In der indirekten Rede steht der Konjunktiv I: 3. Person Singular von \"{k.Infinitiv}\" ist \"{k.Loesung}\". " +
                          "Er zeigt, dass eine Aussage nur wiedergegeben wird.",
            HelpHint = "Konjunktiv I bildet man vom Wortstamm + Endung -e (er komme, sie habe); \"sein\" ist unregelmäßig: er/sie sei."
        };
    }

    private static readonly (string Satz, string Typ)[] AdverbialsatzBeispiele =
    {
        ("Ich bleibe zu Hause, weil ich krank bin.", "Kausalsatz (Grund)"),
        ("Da es regnete, fiel das Spiel aus.", "Kausalsatz (Grund)"),
        ("Sie freut sich, weil sie eine Eins geschrieben hat.", "Kausalsatz (Grund)"),
        ("Er kam zu spät, weil der Bus ausfiel.", "Kausalsatz (Grund)"),
        ("Da er müde war, ging er früh schlafen.", "Kausalsatz (Grund)"),
        ("Als ich klein war, wohnte ich in Ankara.", "Temporalsatz (Zeit)"),
        ("Während wir aßen, klingelte das Telefon.", "Temporalsatz (Zeit)"),
        ("Nachdem sie gelernt hatte, sah sie fern.", "Temporalsatz (Zeit)"),
        ("Bevor du gehst, räum bitte auf.", "Temporalsatz (Zeit)"),
        ("Seitdem er trainiert, ist er viel fitter.", "Temporalsatz (Zeit)"),
        ("Wenn es morgen regnet, bleiben wir drinnen.", "Konditionalsatz (Bedingung)"),
        ("Falls du Hilfe brauchst, ruf mich an.", "Konditionalsatz (Bedingung)"),
        ("Wenn du übst, wirst du besser.", "Konditionalsatz (Bedingung)"),
        ("Falls der Zug Verspätung hat, warten wir.", "Konditionalsatz (Bedingung)"),
        ("Wenn ich Zeit habe, komme ich vorbei.", "Konditionalsatz (Bedingung)"),
        ("Ich lerne viel, damit ich die Prüfung bestehe.", "Finalsatz (Zweck/Absicht)"),
        ("Sie spart Geld, damit sie ein Fahrrad kaufen kann.", "Finalsatz (Zweck/Absicht)"),
        ("Er spricht leise, damit das Baby nicht aufwacht.", "Finalsatz (Zweck/Absicht)"),
        ("Wir beeilen uns, damit wir den Bus erreichen.", "Finalsatz (Zweck/Absicht)"),
        ("Sie schreibt alles auf, damit sie nichts vergisst.", "Finalsatz (Zweck/Absicht)")
    };

    private static QuizQuestion Adverbialsaetze(Random r)
    {
        var a = AdverbialsatzBeispiele[r.Next(AdverbialsatzBeispiele.Length)];
        var optionen = new[] { "Kausalsatz (Grund)", "Temporalsatz (Zeit)", "Konditionalsatz (Bedingung)", "Finalsatz (Zweck/Absicht)" };

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Adverbialsätze", Type = QuestionType.MultipleChoice,
            Prompt = $"Welche Art von Nebensatz steckt in diesem Satz? \"{a.Satz}\"",
            Options = optionen, CorrectAnswers = new[] { a.Typ },
            Explanation = $"\"{a.Satz}\" enthält einen {a.Typ}. " +
                          "Kausalsätze (weil/da) nennen einen Grund, Temporalsätze (als/während/nachdem) eine Zeit, " +
                          "Konditionalsätze (wenn/falls) eine Bedingung, Finalsätze (damit) einen Zweck.",
            HelpHint = "Achte auf die Konjunktion: weil/da = Grund, als/während/nachdem = Zeit, wenn/falls = Bedingung, damit = Zweck."
        };
    }

    private static readonly (string Beispiel, string Stilmittel, string Erklaerung)[] SprachlicheBilderBeispiele =
    {
        ("Ihre Augen leuchten wie Sterne.", "Vergleich", "Ein Vergleich verbindet zwei Dinge mit \"wie\" oder \"als\": Augen wie Sterne."),
        ("Er ist stark wie ein Löwe.", "Vergleich", "Das Vergleichswort \"wie\" verbindet die Person mit dem Löwen."),
        ("Sie schwimmt wie ein Fisch.", "Vergleich", "\"wie ein Fisch\" ist ein Vergleich mit Vergleichswort."),
        ("Er kämpft wie ein Held.", "Vergleich", "\"wie ein Held\" - das Vergleichswort \"wie\" macht den Vergleich erkennbar."),
        ("Das Kind ist flink wie ein Wiesel.", "Vergleich", "Vergleich mit \"wie\": flink wie ein Wiesel."),
        ("Er hat ein Herz aus Stein.", "Metapher", "Eine Metapher überträgt die Bedeutung ohne Vergleichswort: Das Herz ist nicht wirklich aus Stein - gemeint ist Gefühlskälte."),
        ("Sie steht am Scheideweg ihres Lebens.", "Metapher", "\"Scheideweg\" ist ein sprachliches Bild für eine wichtige Entscheidung - ohne Vergleichswort."),
        ("Die Schule ist ein Dschungel.", "Metapher", "Direkte Übertragung ohne \"wie\": Die Schule wird als Dschungel bezeichnet."),
        ("Er ertrinkt in Arbeit.", "Metapher", "Niemand ertrinkt wirklich - das Bild überträgt \"zu viel Arbeit\" ohne Vergleichswort."),
        ("Ihre Worte waren Balsam für seine Seele.", "Metapher", "\"Balsam\" steht bildlich für Trost - eine Metapher ohne Vergleichswort."),
        ("Das Feuer der Begeisterung brannte in ihr.", "Metapher", "Begeisterung wird bildlich als Feuer bezeichnet - ohne Vergleichswort."),
        ("Die Sonne lacht vom Himmel.", "Personifikation", "Die Sonne bekommt eine menschliche Eigenschaft (lachen) - das ist eine Personifikation."),
        ("Der Wind heult um das Haus.", "Personifikation", "Der Wind \"heult\" wie ein Lebewesen - eine Personifikation."),
        ("Die Zeit rennt.", "Personifikation", "Die Zeit kann nicht wirklich rennen - sie wird vermenschlicht."),
        ("Die Blätter tanzen im Wind.", "Personifikation", "Blätter \"tanzen\" - eine menschliche Tätigkeit wird auf Dinge übertragen."),
        ("Der Himmel weint.", "Personifikation", "Regen wird als Weinen des Himmels beschrieben - Personifikation."),
        ("Die Angst packte ihn.", "Personifikation", "Die Angst handelt wie eine Person, die zupackt - Personifikation."),
        ("Das alte Haus schläft im Mondlicht.", "Personifikation", "Ein Haus kann nicht schlafen - es wird vermenschlicht."),
        ("Sie ist langsam wie eine Schnecke.", "Vergleich", "Das Vergleichswort \"wie\" zeigt den Vergleich mit der Schnecke."),
        ("Er trägt eine Maske der Freundlichkeit.", "Metapher", "\"Maske\" steht bildlich für vorgetäuschte Freundlichkeit - ohne Vergleichswort.")
    };

    private static QuizQuestion SprachlicheBilder(Random r)
    {
        var s = SprachlicheBilderBeispiele[r.Next(SprachlicheBilderBeispiele.Length)];
        var optionen = new[] { "Vergleich", "Metapher", "Personifikation" };

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Sprachliche Bilder (Stilmittel)", Type = QuestionType.MultipleChoice,
            Prompt = $"Welches sprachliche Bild wird hier verwendet? \"{s.Beispiel}\"",
            Options = optionen, CorrectAnswers = new[] { s.Stilmittel },
            Explanation = s.Erklaerung,
            HelpHint = "Vergleich: mit \"wie\"/\"als\". Metapher: bildliche Übertragung OHNE Vergleichswort. Personifikation: Dinge/Tiere bekommen menschliche Eigenschaften."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] InhaltsangabeListe =
    {
        ("In welcher Zeitform schreibt man eine Inhaltsangabe?", new[] { "Präsens (Gegenwart)", "Präteritum (Vergangenheit)", "Futur (Zukunft)" }, "Präsens (Gegenwart)",
            "Eine Inhaltsangabe steht immer im Präsens, auch wenn der Originaltext in der Vergangenheit erzählt."),
        ("Was gehört in die Einleitung einer Inhaltsangabe?", new[] { "Textsorte, Titel, Autor/in, Erscheinungsjahr und Kernthema", "Die eigene Meinung zum Text", "Alle Einzelheiten der Handlung, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Textsorte, Titel, Autor/in, Erscheinungsjahr und Kernthema",
            "Der Einleitungssatz nennt Textsorte, Titel, Autor/in, ggf. Jahr und das Thema in einem Satz."),
        ("Darf die eigene Meinung in eine Inhaltsangabe?", new[] { "Nein, die Inhaltsangabe bleibt sachlich und neutral", "Ja, in jedem Absatz", "Ja, aber nur in der Einleitung, auch wenn das manche zunaechst vermuten wuerden" }, "Nein, die Inhaltsangabe bleibt sachlich und neutral",
            "Eine Inhaltsangabe gibt nur den Inhalt wieder - Bewertungen und Meinungen gehören nicht hinein."),
        ("Wie geht man mit wörtlicher Rede aus dem Originaltext um?", new[] { "Man wandelt sie in indirekte Rede um", "Man übernimmt sie wörtlich mit Anführungszeichen", "Man lässt alle Gespräche komplett weg" }, "Man wandelt sie in indirekte Rede um",
            "Wörtliche Rede wird in der Inhaltsangabe zu indirekter Rede (oft im Konjunktiv I)."),
        ("Wie lang sollte eine Inhaltsangabe im Vergleich zum Originaltext sein?", new[] { "Deutlich kürzer - nur das Wichtigste", "Genauso lang", "Länger, weil man alles erklärt, was bei genauerem Hinsehen nicht stimmt" }, "Deutlich kürzer - nur das Wichtigste",
            "Die Inhaltsangabe verdichtet den Text auf die wesentlichen Informationen."),
        ("In welcher Reihenfolge gibt man die Handlung wieder?", new[] { "In der zeitlichen Reihenfolge der Ereignisse", "In beliebiger Reihenfolge", "Rückwärts vom Ende zum Anfang (was so in der Praxis nicht zutrifft)" }, "In der zeitlichen Reihenfolge der Ereignisse",
            "Die Handlung wird chronologisch, also in ihrer zeitlichen Abfolge, zusammengefasst."),
        ("Welche sprachliche Ebene ist für eine Inhaltsangabe richtig?", new[] { "Sachliche Standardsprache ohne Umgangssprache", "Lockere Jugendsprache", "Möglichst viele Ausrufe und Gefühle" }, "Sachliche Standardsprache ohne Umgangssprache",
            "Inhaltsangaben werden sachlich und in Standardsprache formuliert."),
        ("Was macht man mit unwichtigen Einzelheiten des Originaltexts?", new[] { "Man lässt sie weg", "Man beschreibt sie besonders genau", "Man erfindet neue dazu" }, "Man lässt sie weg",
            "Nur die wesentlichen Handlungsschritte kommen in die Inhaltsangabe - Details werden weggelassen."),
        ("Aus welcher Perspektive schreibt man die Inhaltsangabe?", new[] { "In der 3. Person (er/sie), nicht in der Ich-Form des Textes", "Immer in der Ich-Form - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "In der Wir-Form" }, "In der 3. Person (er/sie), nicht in der Ich-Form des Textes",
            "Auch bei einem Ich-Erzähler gibt die Inhaltsangabe die Handlung in der 3. Person wieder."),
        ("Was gehört NICHT in eine Inhaltsangabe?", new[] { "Spannungsaufbau und Ausschmückungen wie im Original", "Der Einleitungssatz", "Die wichtigsten Handlungsschritte, obwohl das auf den ersten Blick plausibel klingt" }, "Spannungsaufbau und Ausschmückungen wie im Original",
            "Die Inhaltsangabe erzählt nicht spannend nach, sondern informiert knapp und sachlich."),
        ("Wozu dient eine Inhaltsangabe?", new[] { "Jemand soll den Inhalt eines Textes schnell erfassen können, ohne ihn zu lesen", "Sie soll den Leser unterhalten wie ein Roman, was die eigentliche Bedeutung des Begriffs verfehlt", "Sie soll die Meinung des Verfassers zeigen" }, "Jemand soll den Inhalt eines Textes schnell erfassen können, ohne ihn zu lesen",
            "Eine Inhaltsangabe informiert kurz und sachlich über den Inhalt eines Textes."),
        ("Wie beginnt man den Hauptteil einer Inhaltsangabe sinnvoll?", new[] { "Mit dem ersten wichtigen Handlungsschritt", "Mit der eigenen Bewertung", "Mit dem Schluss der Geschichte und deshalb hier nicht zutrifft" }, "Mit dem ersten wichtigen Handlungsschritt",
            "Nach dem Einleitungssatz folgt der Hauptteil chronologisch mit den wichtigsten Handlungsschritten."),
        ("Welches Verb passt zu einer sachlichen Inhaltsangabe?", new[] { "\"Der Text handelt von ...\"", "\"Ich fand total spannend, dass ...\"", "\"Stell dir vor, was dann passierte!\"" }, "\"Der Text handelt von ...\"",
            "Formulierungen wie \"Der Text handelt von ...\" sind sachlich und typisch für Inhaltsangaben."),
        ("Was ist der Unterschied zwischen Nacherzählung und Inhaltsangabe?", new[] { "Die Nacherzählung erzählt spannend nach, die Inhaltsangabe fasst sachlich zusammen", "Es gibt keinen Unterschied", "Die Inhaltsangabe ist immer länger, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Die Nacherzählung erzählt spannend nach, die Inhaltsangabe fasst sachlich zusammen",
            "Nacherzählung = erzählend/spannend, Inhaltsangabe = sachlich/knapp im Präsens."),
        ("Muss man in der Inhaltsangabe das Ende des Textes nennen?", new[] { "Ja, auch der Schluss der Handlung wird knapp wiedergegeben", "Nein, das Ende bleibt geheim", "Nur, wenn es ein Happy End ist, auch wenn das manche zunaechst vermuten wuerden" }, "Ja, auch der Schluss der Handlung wird knapp wiedergegeben",
            "Anders als ein Klappentext verrät die Inhaltsangabe die komplette Handlung inklusive Schluss."),
        ("Wie zitiert man in einer Inhaltsangabe am besten?", new[] { "Gar nicht oder nur sehr sparsam - man formuliert mit eigenen Worten", "Man übernimmt ganze Absätze wörtlich", "Man kopiert den Originaltext" }, "Gar nicht oder nur sehr sparsam - man formuliert mit eigenen Worten",
            "Die Inhaltsangabe gibt den Inhalt mit eigenen Worten wieder, nicht mit Zitaten."),
        ("Welche Konjunktionen helfen, Handlungsschritte zu verknüpfen?", new[] { "danach, anschließend, schließlich", "und dann, und dann, und dann", "ähm, halt, irgendwie" }, "danach, anschließend, schließlich",
            "Abwechslungsreiche Verknüpfungen wie \"danach\", \"anschließend\", \"schließlich\" verbinden die Schritte."),
        ("Was bedeutet es, einen Text zu \"verdichten\"?", new[] { "Das Wichtigste in wenigen Worten zusammenfassen", "Den Text länger machen", "Den Text auswendig lernen" }, "Das Wichtigste in wenigen Worten zusammenfassen",
            "Verdichten heißt: nur die Kernaussagen behalten, alles Unwichtige weglassen."),
        ("Wofür steht der Begriff \"W-Fragen\" bei der Vorbereitung einer Inhaltsangabe?", new[] { "Wer? Was? Wann? Wo? Warum? Wie?", "Fragen, die mit \"Welche Farbe\" beginnen", "Fragen ohne Antwort" }, "Wer? Was? Wann? Wo? Warum? Wie?",
            "Mit den W-Fragen erfasst man die Kerninformationen eines Textes."),
        ("Was macht einen guten Schlusssatz einer Inhaltsangabe aus?", new[] { "Er rundet die Zusammenfassung knapp ab, ohne Neues zu erzählen", "Er beginnt eine neue Geschichte, was bei genauerem Hinsehen nicht stimmt", "Er enthält die private Meinung" }, "Er rundet die Zusammenfassung knapp ab, ohne Neues zu erzählen",
            "Der Schluss fasst das Ende der Handlung knapp zusammen - ohne Bewertung, ohne neue Inhalte.")
    };

    private static QuizQuestion Inhaltsangabe(Random r)
    {
        var f = InhaltsangabeListe[r.Next(InhaltsangabeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Inhaltsangabe", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Inhaltsangabe: Präsens, sachlich, chronologisch, keine Meinung, wörtliche Rede wird indirekt, deutlich kürzer als das Original."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArgumentierenListe =
    {
        ("Aus welchen drei Bausteinen besteht ein vollständiges Argument?", new[] { "Behauptung (These), Begründung, Beispiel/Beleg", "Überschrift, Bild, Unterschrift (was so in der Praxis nicht zutrifft)", "Frage, Antwort, Punkt" }, "Behauptung (These), Begründung, Beispiel/Beleg",
            "Ein Argument nennt eine These, begründet sie und stützt sie mit einem Beispiel oder Beleg (B-B-B)."),
        ("Was ist eine These?", new[] { "Eine Behauptung, die begründet werden soll", "Ein bewiesener Fakt", "Eine Frage an den Leser - eine verbreitete, aber falsche Annahme" }, "Eine Behauptung, die begründet werden soll",
            "Die These ist die Ausgangsbehauptung einer Argumentation - sie muss erst begründet werden."),
        ("Was macht ein Beispiel in einer Argumentation?", new[] { "Es veranschaulicht und stützt die Begründung", "Es ersetzt die Begründung vollständig", "Es lenkt vom Thema ab" }, "Es veranschaulicht und stützt die Begründung",
            "Beispiele machen eine Begründung anschaulich und überzeugender - ersetzen sie aber nicht."),
        ("Wie ordnet man Argumente in einer Erörterung am wirkungsvollsten an?", new[] { "Vom schwächsten zum stärksten Argument", "Vom stärksten zum schwächsten Argument", "In zufälliger Reihenfolge" }, "Vom schwächsten zum stärksten Argument",
            "Das stärkste Argument kommt zum Schluss, damit es beim Leser am besten in Erinnerung bleibt."),
        ("Was ist ein Gegenargument?", new[] { "Ein Argument, das gegen die eigene Position spricht", "Ein besonders starkes eigenes Argument, was einer genaueren Pruefung nicht standhaelt", "Eine Beleidigung des Gegners" }, "Ein Argument, das gegen die eigene Position spricht",
            "Wer Gegenargumente kennt und entkräftet, argumentiert überzeugender."),
        ("Was bedeutet es, ein Gegenargument zu \"entkräften\"?", new[] { "Zu zeigen, warum es weniger überzeugend ist als die eigenen Argumente", "Es einfach zu ignorieren", "Es lauter zu wiederholen" }, "Zu zeigen, warum es weniger überzeugend ist als die eigenen Argumente",
            "Entkräften heißt, das Gegenargument aufzugreifen und sachlich zu widerlegen oder abzuschwächen."),
        ("Welche Formulierung leitet ein Argument sachlich ein?", new[] { "\"Ein wichtiges Argument dafür ist ...\"", "\"Jeder weiß doch, dass ...\"", "\"Nur Dumme glauben, dass ...\", obwohl das auf den ersten Blick plausibel klingt" }, "\"Ein wichtiges Argument dafür ist ...\"",
            "Sachliche Überleitungen nennen das Argument, ohne andere abzuwerten."),
        ("Was unterscheidet ein Faktenargument von einem Gefühl?", new[] { "Es stützt sich auf überprüfbare Tatsachen oder Zahlen", "Es klingt lauter", "Es ist immer länger" }, "Es stützt sich auf überprüfbare Tatsachen oder Zahlen",
            "Faktenargumente lassen sich mit Daten, Studien oder Tatsachen belegen."),
        ("Was ist ein Autoritätsargument?", new[] { "Man beruft sich auf Fachleute oder anerkannte Quellen", "Man droht dem Gesprächspartner", "Man wiederholt die eigene Meinung" }, "Man beruft sich auf Fachleute oder anerkannte Quellen",
            "Beim Autoritätsargument stützt eine anerkannte Person oder Institution die Aussage."),
        ("Warum sind Beleidigungen in einer Diskussion kein Argument?", new[] { "Sie begründen nichts, sondern greifen nur die Person an", "Sie sind zu kurz", "Sie sind zu schwer auszusprechen, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Sie begründen nichts, sondern greifen nur die Person an",
            "Ein Angriff auf die Person (statt auf die Sache) hat keine Begründungskraft."),
        ("Was gehört in die Einleitung einer Erörterung?", new[] { "Das Thema/die Streitfrage wird vorgestellt und ihre Bedeutung erklärt", "Das stärkste Argument", "Die vollständige Meinung mit allen Begründungen und deshalb hier nicht zutrifft" }, "Das Thema/die Streitfrage wird vorgestellt und ihre Bedeutung erklärt",
            "Die Einleitung führt zur Streitfrage hin - die Argumente folgen im Hauptteil."),
        ("Was gehört in den Schluss einer Erörterung?", new[] { "Ein Fazit mit der eigenen begründeten Position", "Ein völlig neues Argument", "Eine unbegründete Behauptung, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Ein Fazit mit der eigenen begründeten Position",
            "Der Schluss fasst zusammen und zieht ein begründetes Fazit - neue Argumente gehören nicht hinein."),
        ("Was ist eine Pro-und-Kontra-Erörterung?", new[] { "Man wägt Argumente für und gegen eine Streitfrage ab", "Man nennt nur Argumente für die eigene Meinung", "Man beschreibt einen Gegenstand" }, "Man wägt Argumente für und gegen eine Streitfrage ab",
            "Die dialektische (Pro-Kontra-)Erörterung betrachtet beide Seiten und wägt sie ab."),
        ("Welcher Satz ist eine These (und kein Fakt)?", new[] { "\"Hausaufgaben sollten abgeschafft werden.\"", "\"Berlin ist die Hauptstadt Deutschlands.\", auch wenn das manche zunaechst vermuten wuerden", "\"Ein Tag hat 24 Stunden.\"" }, "\"Hausaufgaben sollten abgeschafft werden.\"",
            "Eine These ist eine strittige Behauptung - Fakten wie \"Berlin ist die Hauptstadt\" sind nicht strittig."),
        ("Wie reagiert man in einer Diskussion fair auf eine andere Meinung?", new[] { "Ausreden lassen, zuhören und sachlich antworten", "Unterbrechen und lauter werden, was bei genauerem Hinsehen nicht stimmt", "Das Thema wechseln" }, "Ausreden lassen, zuhören und sachlich antworten",
            "Faires Diskutieren heißt: zuhören, ausreden lassen, auf Argumente eingehen."),
        ("Was ist ein \"Scheinargument\"?", new[] { "Eine Aussage, die wie ein Argument klingt, aber nichts begründet", "Ein besonders gutes Argument", "Ein Argument aus einem Buch" }, "Eine Aussage, die wie ein Argument klingt, aber nichts begründet",
            "Scheinargumente (z.B. \"Das war schon immer so\") begründen nichts und lassen sich leicht entkräften."),
        ("Warum sollte man Argumente mit Beispielen aus dem Alltag stützen?", new[] { "Der Leser kann sie leichter nachvollziehen", "Beispiele ersetzen die Begründung", "Beispiele machen den Text nur länger" }, "Der Leser kann sie leichter nachvollziehen",
            "Alltagsnahe Beispiele machen abstrakte Begründungen konkret und nachvollziehbar."),
        ("Welche Formulierung leitet ein Gegenargument ein?", new[] { "\"Andererseits könnte man einwenden, dass ...\"", "\"Und außerdem ...\"", "\"Wie bereits gesagt ...\"" }, "\"Andererseits könnte man einwenden, dass ...\"",
            "Wendungen wie \"andererseits\" oder \"dagegen spricht\" kündigen die Gegenposition an."),
        ("Was bedeutet \"abwägen\" in einer Erörterung?", new[] { "Pro- und Kontra-Argumente vergleichen und gewichten", "Alle Argumente gleich gut finden", "Die Wörter zählen" }, "Pro- und Kontra-Argumente vergleichen und gewichten",
            "Beim Abwägen prüft man, welche Argumente schwerer wiegen, und begründet so das Fazit."),
        ("Was ist das Ziel einer Argumentation?", new[] { "Andere mit Begründungen von einer Position überzeugen", "Andere überreden, ohne Gründe zu nennen (was so in der Praxis nicht zutrifft)", "Möglichst viele Fremdwörter benutzen" }, "Andere mit Begründungen von einer Position überzeugen",
            "Argumentieren heißt überzeugen mit Gründen - nicht überreden mit Druck oder Tricks.")
    };

    private static QuizQuestion Argumentieren(Random r)
    {
        var f = ArgumentierenListe[r.Next(ArgumentierenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Argumentieren und Erörtern", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Argument = These + Begründung + Beispiel. In der Erörterung: Einleitung mit Streitfrage, Hauptteil mit gewichteten Argumenten, Schluss mit Fazit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KurzgeschichteListe =
    {
        ("Woran erkennt man den typischen Anfang einer Kurzgeschichte?", new[] { "Sie beginnt unvermittelt, mitten im Geschehen", "Sie beginnt mit \"Es war einmal\"", "Sie beginnt mit einer langen Vorstellung aller Figuren" }, "Sie beginnt unvermittelt, mitten im Geschehen",
            "Kurzgeschichten haben einen offenen, unvermittelten Anfang - der Leser wird direkt ins Geschehen geworfen."),
        ("Was ist typisch für das Ende einer Kurzgeschichte?", new[] { "Es bleibt oft offen oder überraschend", "Alles wird bis ins Detail aufgelöst", "Es endet immer mit einer Hochzeit" }, "Es bleibt oft offen oder überraschend",
            "Das offene oder pointierte Ende regt zum Nachdenken über die Geschichte an."),
        ("Wie viele Figuren hat eine Kurzgeschichte meistens?", new[] { "Wenige - oft nur eine oder zwei Hauptfiguren", "Mindestens zehn - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Gar keine" }, "Wenige - oft nur eine oder zwei Hauptfiguren",
            "Wegen der Kürze konzentriert sich die Kurzgeschichte auf wenige Figuren."),
        ("Welchen Zeitraum umfasst die Handlung einer Kurzgeschichte typischerweise?", new[] { "Einen kurzen Ausschnitt, oft nur Minuten oder Stunden", "Mehrere Generationen, obwohl das auf den ersten Blick plausibel klingt", "Immer genau ein Jahr" }, "Einen kurzen Ausschnitt, oft nur Minuten oder Stunden",
            "Kurzgeschichten zeigen einen knappen Ausschnitt aus dem Alltag - keine Lebensgeschichte."),
        ("Welche Themen behandeln Kurzgeschichten häufig?", new[] { "Alltagssituationen und Konflikte gewöhnlicher Menschen", "Nur Fantasiewelten mit Drachen", "Ausschließlich historische Schlachten, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Alltagssituationen und Konflikte gewöhnlicher Menschen",
            "Im Zentrum stehen Alltagsmomente, die für die Figuren eine besondere Bedeutung bekommen."),
        ("Was bedeutet ein \"Wendepunkt\" in einer Kurzgeschichte?", new[] { "Ein Moment, an dem sich die Situation entscheidend ändert", "Der erste Satz", "Eine Fußnote" }, "Ein Moment, an dem sich die Situation entscheidend ändert",
            "Viele Kurzgeschichten steuern auf einen Wendepunkt zu, der die Figur oder Situation verändert."),
        ("Warum sagt man, in Kurzgeschichten sei \"jedes Wort wichtig\"?", new[] { "Wegen der Kürze trägt jedes Detail zur Bedeutung bei", "Weil die Wörter besonders lang sind und deshalb hier nicht zutrifft", "Weil man sie auswendig lernen muss" }, "Wegen der Kürze trägt jedes Detail zur Bedeutung bei",
            "Auf engem Raum hat jedes Detail (Titel, Gegenstände, Wiederholungen) oft eine tiefere Bedeutung."),
        ("Was ist bei der Analyse einer Kurzgeschichte mit \"Leerstellen\" gemeint?", new[] { "Dinge, die der Text nicht erzählt und die der Leser selbst deuten muss", "Leere Seiten im Buch", "Fehlende Satzzeichen" }, "Dinge, die der Text nicht erzählt und die der Leser selbst deuten muss",
            "Leerstellen sind bewusste Auslassungen - der Leser ergänzt sie durch eigene Deutung."),
        ("Aus welcher Erzählperspektive wird eine Geschichte erzählt, wenn ein \"Ich\" von eigenen Erlebnissen berichtet?", new[] { "Ich-Erzähler", "Auktorialer (allwissender) Erzähler", "Es gibt keinen Erzähler" }, "Ich-Erzähler",
            "Der Ich-Erzähler erzählt aus der eigenen, begrenzten Sicht - der Leser weiß nur, was das Ich weiß."),
        ("Was weiß ein auktorialer (allwissender) Erzähler?", new[] { "Alles - auch Gedanken und Gefühle aller Figuren", "Nur, was eine einzige Figur denkt, was so nicht korrekt ist", "Gar nichts" }, "Alles - auch Gedanken und Gefühle aller Figuren",
            "Der auktoriale Erzähler steht über dem Geschehen und kennt alle Figuren von innen."),
        ("Was ist ein personaler Erzähler?", new[] { "Er erzählt in der 3. Person, aber aus der Sicht einer Figur", "Er nennt sich selbst \"ich\"", "Er kommentiert das Geschehen von außen und weiß alles" }, "Er erzählt in der 3. Person, aber aus der Sicht einer Figur",
            "Der personale Erzähler bleibt bei einer Figur (\"er/sie\"), der Leser erlebt nur deren Innenwelt."),
        ("Was untersucht man bei einer Figurencharakterisierung?", new[] { "Äußeres, Verhalten, Sprache und Beziehungen der Figur", "Nur die Haarfarbe", "Die Seitenzahl des Buches - eine haeufige, aber unzutreffende Vorstellung" }, "Äußeres, Verhalten, Sprache und Beziehungen der Figur",
            "Charakterisieren heißt: Merkmale der Figur aus dem Text belegen und deuten."),
        ("Was ist der Unterschied zwischen direkter und indirekter Charakterisierung?", new[] { "Direkt: der Text beschreibt die Figur ausdrücklich; indirekt: man schließt aus ihrem Verhalten", "Direkt: mündlich; indirekt: schriftlich", "Es gibt keinen Unterschied" }, "Direkt: der Text beschreibt die Figur ausdrücklich; indirekt: man schließt aus ihrem Verhalten",
            "Direkte Charakterisierung nennt Eigenschaften ausdrücklich, indirekte zeigt sie durch Handeln und Sprechen."),
        ("Warum ist der Titel einer Kurzgeschichte oft wichtig für die Deutung?", new[] { "Er gibt häufig einen Hinweis auf das zentrale Thema oder Symbol", "Er verrät immer das Ende, auch wenn das manche zunaechst vermuten wuerden", "Er ist nie wichtig" }, "Er gibt häufig einen Hinweis auf das zentrale Thema oder Symbol",
            "Der Titel lenkt den Blick oft auf das Kernmotiv oder die zentrale Aussage der Geschichte."),
        ("Was ist ein Symbol in einer Geschichte?", new[] { "Ein Gegenstand oder Bild, das für etwas Tieferes steht", "Ein Rechtschreibfehler, was bei genauerem Hinsehen nicht stimmt", "Eine Seitenzahl" }, "Ein Gegenstand oder Bild, das für etwas Tieferes steht",
            "Symbole (z.B. Licht, Mauer, Brücke) verweisen über sich hinaus auf eine tiefere Bedeutung."),
        ("Was macht man beim \"Belegen\" einer Aussage über einen Text?", new[] { "Man zitiert die passende Textstelle mit Zeilenangabe", "Man wiederholt die Aussage lauter", "Man fragt eine andere Person" }, "Man zitiert die passende Textstelle mit Zeilenangabe",
            "Deutungen müssen am Text belegt werden - mit Zitat und Zeilenangabe."),
        ("Was bedeutet \"innerer Monolog\" als Erzähltechnik?", new[] { "Die unausgesprochenen Gedanken einer Figur werden direkt wiedergegeben", "Zwei Figuren sprechen miteinander (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Der Autor spricht den Leser an" }, "Die unausgesprochenen Gedanken einer Figur werden direkt wiedergegeben",
            "Beim inneren Monolog liest man die Gedanken der Figur, als würde sie mit sich selbst sprechen."),
        ("Welche Sprache ist typisch für viele moderne Kurzgeschichten?", new[] { "Einfache Alltagssprache, oft mit knappen Sätzen", "Altertümliche Verssprache", "Ausschließlich Fachbegriffe, was einer genaueren Pruefung nicht standhaelt" }, "Einfache Alltagssprache, oft mit knappen Sätzen",
            "Kurzgeschichten nutzen oft nüchterne Alltagssprache, die zur Alltagshandlung passt."),
        ("Warum eignet sich eine Kurzgeschichte gut für den Unterricht?", new[] { "Sie ist in einer Stunde lesbar und bietet viel zu deuten", "Sie hat keine Handlung, obwohl das auf den ersten Blick plausibel klingt", "Sie ist immer lustig" }, "Sie ist in einer Stunde lesbar und bietet viel zu deuten",
            "Die Kürze erlaubt genaues Lesen und Deuten im Unterricht - trotz kleiner Textmenge viel Gehalt."),
        ("Was prüft man zuerst, wenn man eine Kurzgeschichte deuten will?", new[] { "Wer erzählt, was passiert und wo der Wendepunkt liegt", "Wie schwer das Buch ist", "Ob der Autor noch lebt" }, "Wer erzählt, was passiert und wo der Wendepunkt liegt",
            "Erzählperspektive, Handlung und Wendepunkt sind der Schlüssel zur Deutung einer Kurzgeschichte.")
    };

    private static QuizQuestion Kurzgeschichte(Random r)
    {
        var f = KurzgeschichteListe[r.Next(KurzgeschichteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Kurzgeschichten verstehen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Kurzgeschichte: unvermittelter Anfang, offenes Ende, wenige Figuren, Alltagssituation, oft ein Wendepunkt - jedes Detail kann Bedeutung tragen."
        };
    }
}
