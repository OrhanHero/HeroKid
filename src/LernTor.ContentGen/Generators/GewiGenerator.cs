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
            [GradeLevel.Klasse6] = new List<TopicFactory> { Epochen, Himmelsrichtungen, Kinderrechte },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Grundgesetz, Wirtschaftskreislauf, MedienGesellschaft }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EpochenListe =
    {
        ("Welche Epoche kommt zuerst: Steinzeit oder Antike?", new[] { "Steinzeit", "Antike", "Beide gleichzeitig" }, "Steinzeit",
            "Die Steinzeit (vor ca. 2,5 Mio. bis 3000 v. Chr.) liegt zeitlich vor der Antike (ab ca. 3000 v. Chr., z.B. Ägypten, Griechenland, Rom)."),
        ("Welche Epoche folgt zeitlich auf die Antike?", new[] { "Mittelalter", "Steinzeit", "Neuzeit" }, "Mittelalter",
            "Auf die Antike (endet ca. 500 n. Chr.) folgt das Mittelalter (ca. 500-1500 n. Chr.), danach die Neuzeit."),
        ("In welcher Zeit lebten die Menschen hauptsächlich als Jäger und Sammler?", new[] { "Steinzeit", "Mittelalter", "Neuzeit" }, "Steinzeit",
            "In der Steinzeit ernährten sich die Menschen zunächst vom Jagen und Sammeln, bevor Ackerbau/Viehzucht entstanden.")
    };

    private static QuizQuestion Epochen(Random r)
    {
        var f = EpochenListe[r.Next(EpochenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Geschichtliche Epochen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HimmelsrichtungListe =
    {
        ("Welche Himmelsrichtung liegt der Sonne im Zenit (Mittag) am nächsten, wenn man in Deutschland nach ihr schaut?", new[] { "Süden", "Norden", "Westen" }, "Süden",
            "In Deutschland (Nordhalbkugel) steht die Sonne mittags im Süden am höchsten."),
        ("Wo geht die Sonne auf?", new[] { "Im Osten", "Im Westen", "Im Norden" }, "Im Osten",
            "Die Sonne geht im Osten auf und im Westen unter (Erdrotation)."),
        ("Auf einer Landkarte zeigt der obere Rand normalerweise wohin?", new[] { "Norden", "Süden", "Osten" }, "Norden",
            "Landkarten sind meist genordet: oben ist Norden, unten Süden, rechts Osten, links Westen.")
    };

    private static QuizQuestion Himmelsrichtungen(Random r)
    {
        var f = HimmelsrichtungListe[r.Next(HimmelsrichtungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kartenkunde und Himmelsrichtungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KinderrechteListe =
    {
        ("Was ist ein Grundrecht von Kindern laut UN-Kinderrechtskonvention?", new[] { "Das Recht auf Bildung", "Das Recht, den ganzen Tag zu spielen ohne Schule", "Das Recht, alles zu entscheiden" },
            "Das Recht auf Bildung", "Die UN-Kinderrechtskonvention garantiert u.a. das Recht auf Bildung, Schutz und Mitsprache."),
        ("Wer hat die Kinderrechte in der UN-Kinderrechtskonvention festgelegt?", new[] { "Die Vereinten Nationen (UN)", "Nur Deutschland", "Die Schule" }, "Die Vereinten Nationen (UN)",
            "Die UN-Kinderrechtskonvention von 1989 wurde von den Vereinten Nationen beschlossen und gilt in fast allen Ländern.")
    };

    private static QuizQuestion Kinderrechte(Random r)
    {
        var f = KinderrechteListe[r.Next(KinderrechteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kinderrechte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GrundgesetzListe =
    {
        ("Was ist das Grundgesetz?", new[] { "Die Verfassung der Bundesrepublik Deutschland", "Ein einfaches Gesetz zum Straßenverkehr", "Eine Regel nur für Berlin" },
            "Die Verfassung der Bundesrepublik Deutschland", "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen."),
        ("Welcher Artikel des Grundgesetzes garantiert die Würde des Menschen?", new[] { "Artikel 1", "Artikel 20", "Artikel 100" }, "Artikel 1",
            "Artikel 1 des Grundgesetzes lautet: \"Die Würde des Menschen ist unantastbar.\"")
    };

    private static QuizQuestion Grundgesetz(Random r)
    {
        var f = GrundgesetzListe[r.Next(GrundgesetzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Grundgesetz", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirtschaftListe =
    {
        ("Was beschreibt der einfache Wirtschaftskreislauf zwischen Haushalten und Unternehmen?", new[] { "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür", "Nur der Staat verteilt Geld", "Unternehmen bekommen nichts von Haushalten" },
            "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür",
            "Im einfachen Wirtschaftskreislauf tauschen private Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld (Lohn/Erlöse)."),
        ("Was passiert bei Inflation?", new[] { "Die Preise steigen allgemein, Geld verliert an Wert", "Die Preise sinken dauerhaft", "Nichts ändert sich" },
            "Die Preise steigen allgemein, Geld verliert an Wert", "Bei Inflation steigt das allgemeine Preisniveau, wodurch man sich für denselben Geldbetrag weniger leisten kann.")
    };

    private static QuizQuestion Wirtschaftskreislauf(Random r)
    {
        var f = WirtschaftListe[r.Next(WirtschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wirtschaftskreislauf", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedienListe =
    {
        ("Was ist eine wichtige Aufgabe von Medien in einer Demokratie?", new[] { "Die Öffentlichkeit informieren und die Regierung kontrollieren", "Nur Werbung zu zeigen", "Immer nur eine Meinung zu vertreten" },
            "Die Öffentlichkeit informieren und die Regierung kontrollieren",
            "Medien werden oft als \"vierte Gewalt\" bezeichnet, weil sie die Öffentlichkeit informieren und Politik/Wirtschaft kritisch beobachten."),
        ("Was sollte man tun, bevor man eine Nachricht aus dem Internet weiterverbreitet?", new[] { "Die Quelle und Fakten prüfen", "Sofort teilen, ohne nachzudenken", "Nur die Überschrift lesen" },
            "Die Quelle und Fakten prüfen", "Um Falschmeldungen (Fake News) nicht zu verbreiten, sollte man Quelle und Fakten kritisch prüfen.")
    };

    private static QuizQuestion MedienGesellschaft(Random r)
    {
        var f = MedienListe[r.Next(MedienListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Medien und Gesellschaft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }
}
