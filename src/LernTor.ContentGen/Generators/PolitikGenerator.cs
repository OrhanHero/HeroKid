using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Politik/politische Bildung nach Berliner Rahmenlehrplan, Klasse 6 und Klasse 9.</summary>
public sealed class PolitikGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Politik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Demokratie, BerlinBezirke, Wahlrecht },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Gewaltenteilung, BundestagBundesrat, Wahlsystem, SozialeMarktwirtschaft }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DemokratieListe =
    {
        ("Was bedeutet \"Demokratie\" ganz einfach erklärt?", new[] { "Herrschaft des Volkes - alle dürfen mitbestimmen", "Herrschaft eines einzelnen Königs", "Herrschaft des Militärs" },
            "Herrschaft des Volkes - alle dürfen mitbestimmen", "\"Demokratie\" kommt aus dem Griechischen und bedeutet \"Volksherrschaft\": Die Bürger bestimmen durch Wahlen mit."),
        ("Was ist ein wichtiges Merkmal einer Demokratie?", new[] { "Freie Wahlen und Meinungsfreiheit", "Nur eine erlaubte Partei", "Keine Gerichte" }, "Freie Wahlen und Meinungsfreiheit",
            "Freie, geheime Wahlen und die Freiheit, seine Meinung zu äußern, gehören zu den Grundpfeilern einer Demokratie."),
        ("Was bedeutet \"Gewaltenteilung\" in einer Demokratie ganz einfach?", new[] { "Macht wird auf mehrere unabhängige Institutionen verteilt", "Eine Person hat die gesamte Macht", "Es gibt gar keine Regeln" }, "Macht wird auf mehrere unabhängige Institutionen verteilt",
            "Gewaltenteilung verhindert, dass eine einzelne Person oder Gruppe zu viel Macht bekommt."),
        ("Warum ist eine freie Presse wichtig für eine Demokratie?", new[] { "Sie informiert die Bürger und kontrolliert die Mächtigen", "Sie darf nur Positives über die Regierung schreiben", "Sie ist unwichtig" }, "Sie informiert die Bürger und kontrolliert die Mächtigen",
            "Eine freie Presse berichtet unabhängig und deckt Missstände auf - das nennt man auch \"vierte Gewalt\"."),
        ("Was ist das Gegenteil einer Demokratie, in der eine Person oder Gruppe allein und unkontrolliert herrscht?", new[] { "Diktatur", "Monarchie mit Wahlrecht", "Bundesrepublik" }, "Diktatur",
            "In einer Diktatur trifft eine Person oder kleine Gruppe die Entscheidungen ohne freie Wahlen oder Kontrolle.")
    };

    private static QuizQuestion Demokratie(Random r)
    {
        var f = DemokratieListe[r.Next(DemokratieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Was ist Demokratie?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "\"Demokratie\" = \"Volksherrschaft\" (griechisch) - freie Wahlen und Meinungsfreiheit gehören zu ihren Grundpfeilern."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BezirkeListe =
    {
        ("Wie viele Bezirke hat Berlin?", new[] { "12", "8", "20" }, "12", "Berlin ist in 12 Bezirke aufgeteilt, z.B. Mitte, Pankow, Neukölln oder Spandau."),
        ("Wer leitet einen Berliner Bezirk?", new[] { "Ein Bezirksbürgermeister/eine Bezirksbürgermeisterin", "Der Bundeskanzler", "Ein König" }, "Ein Bezirksbürgermeister/eine Bezirksbürgermeisterin",
            "Jeder Berliner Bezirk hat ein eigenes Bezirksamt, das von einem Bezirksbürgermeister bzw. einer Bezirksbürgermeisterin geleitet wird."),
        ("Wie heißt das Berliner Stadtparlament, das für ganz Berlin zuständig ist?", new[] { "Abgeordnetenhaus", "Bundestag", "Stadtrat" }, "Abgeordnetenhaus",
            "Das Abgeordnetenhaus von Berlin ist das Landesparlament und beschließt Gesetze für ganz Berlin."),
        ("Wer ist der/die Regierungschef/in von ganz Berlin?", new[] { "Regierender Bürgermeister/Regierende Bürgermeisterin", "Bundeskanzler/in", "Bezirksbürgermeister/in" }, "Regierender Bürgermeister/Regierende Bürgermeisterin",
            "Berlin wird von einem/einer Regierenden Bürgermeister/in geleitet - er/sie ist zugleich Regierungschef/in des Bundeslandes Berlin."),
        ("Berlin ist gleichzeitig eine Stadt und ein...?", new[] { "Bundesland", "Bezirk", "Dorf" }, "Bundesland",
            "Berlin ist ein Stadtstaat: Stadt und Bundesland zugleich, so wie Hamburg und Bremen.")
    };

    private static QuizQuestion BerlinBezirke(Random r)
    {
        var f = BezirkeListe[r.Next(BezirkeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Berlin und seine Bezirke", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Berlin hat 12 Bezirke, jeder mit eigenem Bezirksamt und Bezirksbürgermeister/in."
        };
    }

    private static QuizQuestion Wahlrecht(Random r)
    {
        var varianten = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
        {
            ("Ab welchem Alter darf man in Deutschland bei der Bundestagswahl wählen (aktives Wahlrecht)?", new[] { "18 Jahre", "16 Jahre", "21 Jahre" }, "18 Jahre",
                "Bei der Bundestagswahl darf ab 18 Jahren gewählt werden (bei manchen Kommunal-/Landtagswahlen z.T. schon ab 16)."),
            ("Ab welchem Alter darf man sich in Deutschland als Bundestagsabgeordnete/r wählen lassen (passives Wahlrecht)?", new[] { "18 Jahre", "25 Jahre", "30 Jahre" }, "18 Jahre",
                "Wählbar in den Bundestag ist, wer volljährig ist - also ab 18 Jahren, genau wie beim aktiven Wahlrecht."),
            ("Wie oft finden in Deutschland regulär Bundestagswahlen statt?", new[] { "Alle 4 Jahre", "Jedes Jahr", "Alle 10 Jahre" }, "Alle 4 Jahre",
                "Der Bundestag wird regulär alle vier Jahre neu gewählt."),
            ("Was bedeutet \"allgemeines Wahlrecht\"?", new[] { "Grundsätzlich darf jede/r Bürger/in ab einem bestimmten Alter wählen", "Nur bestimmte Berufsgruppen dürfen wählen", "Nur Männer dürfen wählen" }, "Grundsätzlich darf jede/r Bürger/in ab einem bestimmten Alter wählen",
                "\"Allgemein\" bedeutet: Es gibt keine Einschränkungen nach Geschlecht, Herkunft oder Vermögen - nur das Alter zählt."),
            ("Was macht man, wenn man am Wahltag nicht ins Wahllokal gehen kann, aber trotzdem wählen möchte?", new[] { "Briefwahl beantragen", "Man darf dann gar nicht wählen", "Ein Nachbar wählt für einen mit" }, "Briefwahl beantragen",
                "Mit der Briefwahl kann man vor dem Wahltag per Post wählen, wenn man am Wahltag selbst nicht ins Wahllokal kann."),
        };
        var f = varianten[r.Next(varianten.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wahlrecht", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Sowohl aktives (wählen) als auch passives (sich wählen lassen) Wahlrecht zum Bundestag beginnen mit der Volljährigkeit (18 Jahre)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GewaltenteilungListe =
    {
        ("Welche drei Gewalten gibt es nach dem Prinzip der Gewaltenteilung?", new[] { "Legislative, Exekutive, Judikative", "Regierung, Opposition, Volk", "Bund, Land, Gemeinde" },
            "Legislative, Exekutive, Judikative", "Die drei Staatsgewalten sind: Legislative (Gesetzgebung), Exekutive (Ausführung/Regierung) und Judikative (Rechtsprechung/Gerichte)."),
        ("Wer übt die Judikative (rechtsprechende Gewalt) aus?", new[] { "Die Gerichte", "Der Bundestag", "Die Bundesregierung" }, "Die Gerichte",
            "Die Judikative liegt bei den unabhängigen Gerichten, die Gesetze auslegen und Recht sprechen."),
        ("Wer übt in Deutschland die Exekutive (ausführende Gewalt) hauptsächlich aus?", new[] { "Die Regierung und Verwaltung", "Die Gerichte", "Der Bundestag allein" }, "Die Regierung und Verwaltung",
            "Die Exekutive setzt Gesetze um - dazu gehören Bundesregierung, Ministerien und Behörden."),
        ("Warum ist es wichtig, dass die drei Gewalten voneinander unabhängig sind?", new[] { "Damit keine Gewalt zu viel Macht bekommt und sich alle gegenseitig kontrollieren", "Damit alles schneller geht", "Das ist nicht wichtig" }, "Damit keine Gewalt zu viel Macht bekommt und sich alle gegenseitig kontrollieren",
            "Die gegenseitige Kontrolle (\"Checks and Balances\") verhindert Machtmissbrauch."),
        ("Wer beschließt in Deutschland die Gesetze (Legislative)?", new[] { "Bundestag und Bundesrat", "Die Bundesregierung allein", "Die Gerichte" }, "Bundestag und Bundesrat",
            "Gesetze werden vom Bundestag beschlossen, bei vielen Gesetzen wirkt auch der Bundesrat mit.")
    };

    private static QuizQuestion Gewaltenteilung(Random r)
    {
        var f = GewaltenteilungListe[r.Next(GewaltenteilungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gewaltenteilung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die drei Gewalten: Legislative (Gesetzgebung), Exekutive (Regierung/Verwaltung), Judikative (Gerichte)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BundestagListe =
    {
        ("Was ist die Hauptaufgabe des Bundestags?", new[] { "Gesetze beschließen und die Regierung kontrollieren", "Gerichtsurteile fällen", "Die Polizei leiten" },
            "Gesetze beschließen und die Regierung kontrollieren", "Der Bundestag ist das direkt gewählte Parlament Deutschlands und beschließt Bundesgesetze."),
        ("Was vertritt der Bundesrat?", new[] { "Die Interessen der 16 Bundesländer", "Nur die Interessen Berlins", "Die Vereinten Nationen" }, "Die Interessen der 16 Bundesländer",
            "Der Bundesrat ist die Vertretung der Bundesländer auf Bundesebene und wirkt bei vielen Gesetzen mit."),
        ("Wie wird der Bundeskanzler/die Bundeskanzlerin gewählt?", new[] { "Vom Bundestag gewählt", "Direkt vom Volk gewählt", "Vom Bundespräsidenten bestimmt, ohne Wahl" }, "Vom Bundestag gewählt",
            "Anders als z.B. in den USA wählt in Deutschland nicht das Volk direkt, sondern der Bundestag den Bundeskanzler/die Bundeskanzlerin."),
        ("Wo tagt der Deutsche Bundestag?", new[] { "Im Reichstagsgebäude in Berlin", "Im Schloss Bellevue", "Im Roten Rathaus" }, "Im Reichstagsgebäude in Berlin",
            "Der Bundestag tagt im Reichstagsgebäude, das für Besucher teilweise zugänglich ist (u.a. die Kuppel)."),
        ("Was ist eine \"Fraktion\" im Bundestag?", new[] { "Der Zusammenschluss der Abgeordneten einer Partei", "Ein einzelner Abgeordneter", "Ein Gerichtssaal" }, "Der Zusammenschluss der Abgeordneten einer Partei",
            "Abgeordnete derselben Partei bilden im Bundestag eine Fraktion und stimmen oft gemeinsam ab.")
    };

    private static QuizQuestion BundestagBundesrat(Random r)
    {
        var f = BundestagListe[r.Next(BundestagListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Bundestag und Bundesrat", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Der Bundestag ist das direkt gewählte Parlament (Gesetze, Regierungskontrolle); der Bundesrat vertritt die 16 Bundesländer."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WahlsystemListe =
    {
        ("Bei der Bundestagswahl hat jeder Wähler zwei Stimmen. Wofür ist die Zweitstimme wichtig?", new[] { "Sie entscheidet über die Sitzverteilung der Parteien im Bundestag", "Sie wählt nur den Bundeskanzler direkt", "Sie zählt nicht" },
            "Sie entscheidet über die Sitzverteilung der Parteien im Bundestag",
            "Die Zweitstimme (Parteistimme) ist entscheidend für das Kräfteverhältnis der Parteien im Bundestag."),
        ("Wie nennt man eine Wahl, bei der niemand sehen kann, wen man gewählt hat?", new[] { "Geheime Wahl", "Öffentliche Wahl", "Offene Wahl" }, "Geheime Wahl",
            "In Deutschland ist die Wahl laut Grundgesetz u.a. geheim: Niemand darf erfahren, wen man gewählt hat."),
        ("Was bedeutet \"gleiches Wahlrecht\"?", new[] { "Jede Stimme zählt gleich viel", "Nur Reiche haben mehr Stimmen", "Nur Ältere haben mehr Stimmen" }, "Jede Stimme zählt gleich viel",
            "\"Gleich\" bedeutet: Die Stimme jeder wahlberechtigten Person hat dasselbe Gewicht, unabhängig von Einkommen oder Status."),
        ("Was bedeutet die \"Fünf-Prozent-Hürde\" bei der Bundestagswahl?", new[] { "Eine Partei braucht mindestens 5% der Stimmen, um in den Bundestag einzuziehen", "Jede Partei bekommt automatisch 5% der Sitze", "Nur 5 Parteien dürfen antreten" }, "Eine Partei braucht mindestens 5% der Stimmen, um in den Bundestag einzuziehen",
            "Die 5%-Hürde soll eine zu starke Zersplitterung des Parlaments in viele Kleinstparteien verhindern."),
        ("Was ist die Erststimme bei der Bundestagswahl?", new[] { "Sie wählt die/den Kandidat/in des eigenen Wahlkreises direkt", "Sie entscheidet über die Sitzverteilung der Parteien", "Sie wählt den Bundeskanzler direkt" }, "Sie wählt die/den Kandidat/in des eigenen Wahlkreises direkt",
            "Mit der Erststimme wird eine Person direkt im eigenen Wahlkreis gewählt, mit der Zweitstimme eine Partei.")
    };

    private static QuizQuestion Wahlsystem(Random r)
    {
        var f = WahlsystemListe[r.Next(WahlsystemListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wahlsystem", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bei der Bundestagswahl entscheidet die Zweitstimme über die Sitzverteilung der Parteien; Wahlen in Deutschland sind u.a. geheim."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SozialeMarktwirtschaftListe =
    {
        ("Was ist das Grundprinzip der Sozialen Marktwirtschaft in Deutschland?", new[] { "Freier Markt und Wettbewerb, kombiniert mit sozialer Absicherung durch den Staat", "Der Staat bestimmt alle Preise und Löhne", "Es gibt keinerlei staatliche Regeln für die Wirtschaft" },
            "Freier Markt und Wettbewerb, kombiniert mit sozialer Absicherung durch den Staat",
            "Die Soziale Marktwirtschaft verbindet freien Wettbewerb (Angebot und Nachfrage) mit sozialem Ausgleich, z.B. durch Sozialversicherungen und Mindestlohn."),
        ("Welche Aufgabe hat der Staat in der Sozialen Marktwirtschaft?", new[] { "Er setzt Regeln (z.B. gegen Monopole) und sichert soziale Absicherung ab", "Er verbietet jeglichen privaten Handel", "Er hat gar keine Aufgabe in der Wirtschaft" },
            "Er setzt Regeln (z.B. gegen Monopole) und sichert soziale Absicherung ab",
            "Der Staat greift regelnd ein (z.B. Kartellrecht gegen Monopole) und sorgt für ein soziales Netz (Kranken-, Renten-, Arbeitslosenversicherung)."),
        ("Was gehört in Deutschland zum sozialen Sicherungssystem?", new[] { "Kranken-, Renten- und Arbeitslosenversicherung", "Nur die private Altersvorsorge", "Ausschließlich Steuern auf Lebensmittel" },
            "Kranken-, Renten- und Arbeitslosenversicherung", "Die gesetzlichen Sozialversicherungen (u.a. Kranken-, Renten-, Arbeitslosen- und Pflegeversicherung) bilden das soziale Sicherungsnetz."),
        ("Was ist der Mindestlohn?", new[] { "Der niedrigste erlaubte Stundenlohn in Deutschland", "Das höchste erlaubte Gehalt", "Eine freiwillige Zahlung von Firmen" },
            "Der niedrigste erlaubte Stundenlohn in Deutschland", "Der gesetzliche Mindestlohn legt fest, wie viel pro Stunde mindestens gezahlt werden muss."),
        ("Was passiert bei starkem Wettbewerb zwischen Firmen normalerweise mit den Preisen?", new[] { "Sie tendieren eher dazu, niedrig zu bleiben", "Sie steigen automatisch stark", "Wettbewerb hat keinen Einfluss auf Preise" },
            "Sie tendieren eher dazu, niedrig zu bleiben", "Konkurrieren mehrere Anbieter um Kunden, versuchen sie oft, mit guten Preisen oder Qualität zu überzeugen.")
    };

    private static QuizQuestion SozialeMarktwirtschaft(Random r)
    {
        var f = SozialeMarktwirtschaftListe[r.Next(SozialeMarktwirtschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Soziale Marktwirtschaft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Soziale Marktwirtschaft = freier Wettbewerb + staatliche Regeln/soziale Absicherung (Sozialversicherungen)."
        };
    }
}
