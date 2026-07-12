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
                AdjektivSteigerung
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                AktivPassiv,
                Konjunktionen,
                Kommasetzung,
                DassOderDas,
                Wortarten9,
                Textsorten
            }
        };

    private static readonly (string Wort, string Wortart)[] WortartenBeispiele =
    {
        ("Haus", "Nomen"), ("laufen", "Verb"), ("schnell", "Adjektiv"), ("Berlin", "Nomen"),
        ("lachen", "Verb"), ("bunt", "Adjektiv"), ("Schule", "Nomen"), ("singen", "Verb"),
        ("groß", "Adjektiv"), ("Lehrerin", "Nomen"), ("springen", "Verb"), ("freundlich", "Adjektiv")
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
        ("ich fahre", "ich fuhr", "ich bin gefahren", "fahren")
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
        ("Der Vater repariert das Fahrrad.", "Der Vater", "repariert", "das Fahrrad")
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
        ("er kauft ein neues ___ (fahrrad).", "Fahrrad", "Nomen werden immer großgeschrieben, auch nach einem Adjektiv wie \"neues\".")
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
        ("gerne", "lieber", "am liebsten")
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

    private static readonly (string Aktiv, string Passiv)[] AktivPassivBeispiele =
    {
        ("Der Gärtner gießt die Blumen.", "Die Blumen werden von dem Gärtner gegossen."),
        ("Die Firma baut das Haus.", "Das Haus wird von der Firma gebaut."),
        ("Der Lehrer korrigiert die Klassenarbeit.", "Die Klassenarbeit wird von dem Lehrer korrigiert."),
        ("Die Köchin kocht die Suppe.", "Die Suppe wird von der Köchin gekocht."),
        ("Die Schüler lösen die Matheaufgabe.", "Die Matheaufgabe wird von den Schülern gelöst.")
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
        ("Ich nehme einen Schirm mit, ___ es regnen könnte.", "falls", "konditional (Bedingung)")
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
        ("Der Hund bellte___ und lief davon.", "", "Bei zwei durch \"und\" verbundenen Verben mit demselben Subjekt (kein vollständiger Nebensatz) steht meist kein Komma.")
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
        ("Ich glaube, ___ er Recht hat.", "dass")
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
        ("Endlich kam der Bus.", "Endlich", "Adverb")
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
            "Erörterung", "Eine Erörterung wägt Pro- und Contra-Argumente zu einer Streitfrage ab und mündet in ein begründetes eigenes Urteil.")
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
}
