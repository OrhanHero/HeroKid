using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Gesellschaftswissenschaften (Gewi) - kombiniert Geschichte/Erdkunde/Politik-Grundlagen,
/// wie im Berliner Rahmenlehrplan für die Grundschule/frühe Sekundarstufe üblich.
/// </summary>
public sealed class GewiGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Gewi;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Epochen, Himmelsrichtungen, Kinderrechte, Ernaehrung },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Grundgesetz, Wirtschaftskreislauf, MedienGesellschaft }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EpochenListe =
    {
        ("Welche Epoche kommt zuerst: Steinzeit oder Antike?", new[] { "Steinzeit", "Antike", "Beide gleichzeitig" }, "Steinzeit",
            "Die Steinzeit (vor ca. 2,5 Mio. bis 3000 v. Chr.) liegt zeitlich vor der Antike (ab ca. 3000 v. Chr., z.B. Ägypten, Griechenland, Rom)."),
        ("Welche Epoche folgt zeitlich auf die Antike?", new[] { "Mittelalter", "Steinzeit", "Neuzeit" }, "Mittelalter",
            "Auf die Antike (endet ca. 500 n. Chr.) folgt das Mittelalter (ca. 500-1500 n. Chr.), danach die Neuzeit."),
        ("In welcher Zeit lebten die Menschen hauptsächlich als Jäger und Sammler?", new[] { "Steinzeit", "Mittelalter", "Neuzeit" }, "Steinzeit",
            "In der Steinzeit ernährten sich die Menschen zunächst vom Jagen und Sammeln, bevor Ackerbau/Viehzucht entstanden."),
        ("Welche Hochkultur der Antike baute die Pyramiden?", new[] { "Die alten Ägypter", "Die Römer", "Die Wikinger" }, "Die alten Ägypter",
            "Die Pyramiden von Gizeh wurden im alten Ägypten als Grabmäler für Pharaonen errichtet."),
        ("Wodurch endet in Europa das Mittelalter und beginnt die Neuzeit (ungefähr)?", new[] { "Entdeckung Amerikas / Erfindung des Buchdrucks (ca. 1500)", "Ende der Steinzeit", "Der Zweite Weltkrieg" }, "Entdeckung Amerikas / Erfindung des Buchdrucks (ca. 1500)",
            "Ereignisse wie die Entdeckung Amerikas 1492 und der Buchdruck (Gutenberg) markieren um 1500 den Übergang vom Mittelalter zur Neuzeit.")
    };

    private static QuizQuestion Epochen(Random r)
    {
        var f = EpochenListe[r.Next(EpochenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Geschichtliche Epochen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Reihenfolge der Epochen: Steinzeit → Antike → Mittelalter → Neuzeit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HimmelsrichtungListe =
    {
        ("Welche Himmelsrichtung liegt der Sonne im Zenit (Mittag) am nächsten, wenn man in Deutschland nach ihr schaut?", new[] { "Süden", "Norden", "Westen" }, "Süden",
            "In Deutschland (Nordhalbkugel) steht die Sonne mittags im Süden am höchsten."),
        ("Wo geht die Sonne auf?", new[] { "Im Osten", "Im Westen", "Im Norden" }, "Im Osten",
            "Die Sonne geht im Osten auf und im Westen unter (Erdrotation)."),
        ("Auf einer Landkarte zeigt der obere Rand normalerweise wohin?", new[] { "Norden", "Süden", "Osten" }, "Norden",
            "Landkarten sind meist genordet: oben ist Norden, unten Süden, rechts Osten, links Westen."),
        ("Welches Hilfsmittel zeigt dir zuverlässig die Himmelsrichtung Norden an, egal wo du bist?", new[] { "Ein Kompass", "Eine Uhr", "Ein Lineal" }, "Ein Kompass",
            "Ein Kompass richtet seine Nadel nach dem Erdmagnetfeld aus und zeigt so immer zuverlässig nach Norden."),
        ("Wenn du mit dem Rücken nach Norden stehst, in welche Richtung schaust du dann?", new[] { "Süden", "Osten", "Westen" }, "Süden",
            "Steht man mit dem Rücken nach Norden, blickt man automatisch in die genau entgegengesetzte Richtung: Süden.")
    };

    private static QuizQuestion Himmelsrichtungen(Random r)
    {
        var f = HimmelsrichtungListe[r.Next(HimmelsrichtungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kartenkunde und Himmelsrichtungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Auf genordeten Karten gilt: oben Norden, unten Süden, rechts Osten, links Westen. In Deutschland steht die Mittagssonne im Süden."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KinderrechteListe =
    {
        ("Was ist ein Grundrecht von Kindern laut UN-Kinderrechtskonvention?", new[] { "Das Recht auf Bildung", "Das Recht, den ganzen Tag zu spielen ohne Schule", "Das Recht, alles zu entscheiden" },
            "Das Recht auf Bildung", "Die UN-Kinderrechtskonvention garantiert u.a. das Recht auf Bildung, Schutz und Mitsprache."),
        ("Wer hat die Kinderrechte in der UN-Kinderrechtskonvention festgelegt?", new[] { "Die Vereinten Nationen (UN)", "Nur Deutschland", "Die Schule" }, "Die Vereinten Nationen (UN)",
            "Die UN-Kinderrechtskonvention von 1989 wurde von den Vereinten Nationen beschlossen und gilt in fast allen Ländern."),
        ("Welches Recht schützt Kinder laut Kinderrechtskonvention vor Gewalt und Ausbeutung?", new[] { "Das Recht auf Schutz", "Das Recht auf ein eigenes Auto", "Das Recht auf unbegrenztes Fernsehen" }, "Das Recht auf Schutz",
            "Das Recht auf Schutz bewahrt Kinder u.a. vor Gewalt, Vernachlässigung und Ausbeutung (z.B. Kinderarbeit)."),
        ("Was bedeutet das Recht auf Mitsprache (Partizipation) für Kinder?", new[] { "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern", "Kinder entscheiden allein über alles in der Familie", "Kinder müssen zu allem schweigen" }, "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern",
            "Das Recht auf Mitsprache bedeutet, dass die Meinung von Kindern bei Entscheidungen, die sie betreffen, gehört und berücksichtigt werden soll."),
        ("Was garantiert Kindern das Recht auf Bildung laut Kinderrechtskonvention?", new[] { "Zugang zu Schule und Ausbildung, unabhängig von Herkunft", "Nur Kindern aus reichen Familien Schulbesuch", "Freiwilliger Schulbesuch nur für Jungen" }, "Zugang zu Schule und Ausbildung, unabhängig von Herkunft",
            "Das Recht auf Bildung soll allen Kindern unabhängig von Herkunft, Geschlecht oder Vermögen der Eltern den Zugang zu Schule ermöglichen.")
    };

    private static QuizQuestion Kinderrechte(Random r)
    {
        var f = KinderrechteListe[r.Next(KinderrechteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kinderrechte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die UN-Kinderrechtskonvention (1989) wurde von den Vereinten Nationen beschlossen und schützt u.a. Bildung, Schutz und Mitsprache von Kindern."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ErnaehrungListe =
    {
        ("Warum werden in der Landwirtschaft heute oft Maschinen statt Handarbeit eingesetzt?", new[] { "Um mehr Menschen mit weniger Aufwand zu ernähren (höherer Ertrag)", "Weil es das Gesetz vorschreibt", "Weil Maschinen billiger sind als ein einziger Sack Saatgut" },
            "Um mehr Menschen mit weniger Aufwand zu ernähren (höherer Ertrag)", "Moderne Landwirtschaft (Maschinen, Dünger) erhöht den Ertrag pro Fläche, damit mehr Menschen ernährt werden können."),
        ("Was bedeutet \"Verbraucherschutz\" beim Thema Ernährung?", new[] { "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Lebensmittel möglichst teuer machen", "Nur ausländische Lebensmittel verbieten" },
            "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Verbraucherschutz sorgt z.B. durch Kontrollen und Kennzeichnungspflichten dafür, dass Lebensmittel sicher und ehrlich beworben sind."),
        ("Was versteht man unter \"Überfluss und Mangel\" bei der weltweiten Ernährung?", new[] { "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Überall auf der Welt gibt es gleich viel zu essen", "Mangel bedeutet, dass Essen schlecht schmeckt" },
            "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Weltweit ist Nahrung ungleich verteilt: In manchen Ländern werden Lebensmittel verschwendet, in anderen herrscht Hunger."),
        ("Was bedeutet \"regionale und saisonale\" Ernährung?", new[] { "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Nur Lebensmittel aus dem Ausland essen", "Jeden Tag genau dasselbe essen" },
            "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Regionale und saisonale Ernährung bedeutet, Obst und Gemüse aus der Nähe zu essen, wenn sie gerade natürlich reifen - das spart oft Transportwege."),
        ("Warum ist Lebensmittelverschwendung ein Problem?", new[] { "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Weil dadurch mehr Menschen satt werden", "Weil weggeworfenes Essen automatisch neue Pflanzen wachsen lässt" },
            "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Für die Herstellung von Lebensmitteln werden Wasser, Fläche und Energie verbraucht - wird das Essen weggeworfen, war das alles umsonst.")
    };

    private static QuizQuestion Ernaehrung(Random r)
    {
        var f = ErnaehrungListe[r.Next(ErnaehrungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Ernährung – wie werden Menschen satt?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Denk an Landwirtschaft (Ertrag steigern), Verbraucherschutz (sichere Lebensmittel) und die ungleiche Verteilung von Nahrung weltweit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GrundgesetzListe =
    {
        ("Was ist das Grundgesetz?", new[] { "Die Verfassung der Bundesrepublik Deutschland", "Ein einfaches Gesetz zum Straßenverkehr", "Eine Regel nur für Berlin" },
            "Die Verfassung der Bundesrepublik Deutschland", "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen."),
        ("Welcher Artikel des Grundgesetzes garantiert die Würde des Menschen?", new[] { "Artikel 1", "Artikel 20", "Artikel 100" }, "Artikel 1",
            "Artikel 1 des Grundgesetzes lautet: \"Die Würde des Menschen ist unantastbar.\""),
        ("Seit wann gilt das Grundgesetz für ganz Deutschland (nach der Wiedervereinigung)?", new[] { "Seit 1990", "Seit 1949", "Seit 1945" }, "Seit 1990",
            "Das Grundgesetz galt ab 1949 zunächst nur für die Bundesrepublik (Westdeutschland) und gilt seit der Wiedervereinigung 1990 für ganz Deutschland."),
        ("Was bedeutet \"Gewaltenteilung\" im Grundgesetz?", new[] { "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt", "Nur eine Person entscheidet über alles", "Es gibt gar keine Regeln"}, "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt",
            "Die Gewaltenteilung verteilt die Macht auf Legislative (Parlament), Exekutive (Regierung) und Judikative (Gerichte), damit keine zu mächtig wird."),
        ("Was sind laut Grundgesetz die tragenden Prinzipien des deutschen Staates?", new[] { "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat", "Ein Kaiser entscheidet allein", "Es gibt gar keine festen Prinzipien" }, "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat",
            "Artikel 20 GG legt fest, dass Deutschland ein demokratischer, sozialer Rechtsstaat und Bundesstaat ist.")
    };

    private static QuizQuestion Grundgesetz(Random r)
    {
        var f = GrundgesetzListe[r.Next(GrundgesetzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Grundgesetz", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen; Artikel 1 schützt die Menschenwürde."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirtschaftListe =
    {
        ("Was beschreibt der einfache Wirtschaftskreislauf zwischen Haushalten und Unternehmen?", new[] { "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür", "Nur der Staat verteilt Geld", "Unternehmen bekommen nichts von Haushalten" },
            "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür",
            "Im einfachen Wirtschaftskreislauf tauschen private Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld (Lohn/Erlöse)."),
        ("Was passiert bei Inflation?", new[] { "Die Preise steigen allgemein, Geld verliert an Wert", "Die Preise sinken dauerhaft", "Nichts ändert sich" },
            "Die Preise steigen allgemein, Geld verliert an Wert", "Bei Inflation steigt das allgemeine Preisniveau, wodurch man sich für denselben Geldbetrag weniger leisten kann."),
        ("Welche Rolle spielt der Staat zusätzlich im erweiterten Wirtschaftskreislauf?", new[] { "Er erhebt Steuern und zahlt z.B. für Schulen und Straßen", "Er hat gar keine Rolle", "Er verbietet jeden Handel" },
            "Er erhebt Steuern und zahlt z.B. für Schulen und Straßen", "Der Staat nimmt Steuern von Haushalten/Unternehmen ein und finanziert damit öffentliche Aufgaben wie Schulen, Straßen und Krankenhäuser."),
        ("Was bedeutet \"Angebot und Nachfrage\" für die Preisbildung?", new[] { "Ist etwas knapp und stark gefragt, steigt meist der Preis", "Preise werden immer zufällig festgelegt", "Angebot und Nachfrage haben keinen Einfluss auf Preise" },
            "Ist etwas knapp und stark gefragt, steigt meist der Preis", "Sind viele Menschen an einem knappen Produkt interessiert (hohe Nachfrage, geringes Angebot), steigt in der Marktwirtschaft meist der Preis."),
        ("Was ist der Unterschied zwischen Import und Export?", new[] { "Import: Waren einführen, Export: Waren ausführen", "Beides bedeutet dasselbe", "Import bedeutet, im eigenen Land zu produzieren" },
            "Import: Waren einführen, Export: Waren ausführen", "Importe sind Waren, die aus dem Ausland eingeführt werden, Exporte sind Waren, die ins Ausland verkauft werden.")
    };

    private static QuizQuestion Wirtschaftskreislauf(Random r)
    {
        var f = WirtschaftListe[r.Next(WirtschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wirtschaftskreislauf", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Im einfachen Wirtschaftskreislauf tauschen Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedienListe =
    {
        ("Was ist eine wichtige Aufgabe von Medien in einer Demokratie?", new[] { "Die Öffentlichkeit informieren und die Regierung kontrollieren", "Nur Werbung zu zeigen", "Immer nur eine Meinung zu vertreten" },
            "Die Öffentlichkeit informieren und die Regierung kontrollieren",
            "Medien werden oft als \"vierte Gewalt\" bezeichnet, weil sie die Öffentlichkeit informieren und Politik/Wirtschaft kritisch beobachten."),
        ("Was sollte man tun, bevor man eine Nachricht aus dem Internet weiterverbreitet?", new[] { "Die Quelle und Fakten prüfen", "Sofort teilen, ohne nachzudenken", "Nur die Überschrift lesen" },
            "Die Quelle und Fakten prüfen", "Um Falschmeldungen (Fake News) nicht zu verbreiten, sollte man Quelle und Fakten kritisch prüfen."),
        ("Was unterscheidet einen seriösen Nachrichtensender von einer unseriösen Quelle?", new[] { "Er nennt Quellen und trennt Meinung von Fakten", "Er hat die reißerischste Überschrift", "Er postet am schnellsten"}, "Er nennt Quellen und trennt Meinung von Fakten",
            "Seriöser Journalismus recherchiert sorgfältig, nennt Quellen und kennzeichnet klar, was Fakten und was Meinung ist."),
        ("Warum nennt man Medien manchmal die \"vierte Gewalt\" im Staat?", new[] { "Weil sie Politik und Wirtschaft öffentlich kontrollieren", "Weil sie über Gesetze abstimmen dürfen", "Weil sie Gerichtsurteile fällen" }, "Weil sie Politik und Wirtschaft öffentlich kontrollieren",
            "Neben Legislative, Exekutive und Judikative gelten Medien als \"vierte Gewalt\", weil sie durch Berichterstattung Machtmissbrauch aufdecken können."),
        ("Was unterscheidet soziale Medien von klassischen Nachrichtenmedien?", new[] { "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen", "Soziale Medien werden immer von Journalisten geprüft", "Es gibt keinen Unterschied" }, "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen",
            "Anders als bei klassischen Medien mit Redaktion kann bei sozialen Medien jede Person Inhalte posten - das erhöht das Risiko ungeprüfter Falschinformationen.")
    };

    private static QuizQuestion MedienGesellschaft(Random r)
    {
        var f = MedienListe[r.Next(MedienListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Medien und Gesellschaft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Medien gelten als \"vierte Gewalt\": sie informieren und kontrollieren Politik - deshalb vor dem Teilen immer Quelle und Fakten prüfen."
        };
    }
}
