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
            [GradeLevel.Klasse9] = new List<TopicFactory> { Gewaltenteilung, BundestagBundesrat, Wahlsystem }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DemokratieListe =
    {
        ("Was bedeutet \"Demokratie\" ganz einfach erklärt?", new[] { "Herrschaft des Volkes - alle dürfen mitbestimmen", "Herrschaft eines einzelnen Königs", "Herrschaft des Militärs" },
            "Herrschaft des Volkes - alle dürfen mitbestimmen", "\"Demokratie\" kommt aus dem Griechischen und bedeutet \"Volksherrschaft\": Die Bürger bestimmen durch Wahlen mit."),
        ("Was ist ein wichtiges Merkmal einer Demokratie?", new[] { "Freie Wahlen und Meinungsfreiheit", "Nur eine erlaubte Partei", "Keine Gerichte" }, "Freie Wahlen und Meinungsfreiheit",
            "Freie, geheime Wahlen und die Freiheit, seine Meinung zu äußern, gehören zu den Grundpfeilern einer Demokratie.")
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
            "Jeder Berliner Bezirk hat ein eigenes Bezirksamt, das von einem Bezirksbürgermeister bzw. einer Bezirksbürgermeisterin geleitet wird.")
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
            "Die Judikative liegt bei den unabhängigen Gerichten, die Gesetze auslegen und Recht sprechen.")
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
            "Der Bundesrat ist die Vertretung der Bundesländer auf Bundesebene und wirkt bei vielen Gesetzen mit.")
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
            "In Deutschland ist die Wahl laut Grundgesetz u.a. geheim: Niemand darf erfahren, wen man gewählt hat.")
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
}
