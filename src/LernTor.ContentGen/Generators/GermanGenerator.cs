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
}
