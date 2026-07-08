using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Englisch als Fremdsprache, Klasse 6 (Grundlagen) und Klasse 9 (vertieft).</summary>
public sealed class EnglischGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Englisch;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { SimplePresentVsProgressive, IrregularPlurals, QuestionWords },
            [GradeLevel.Klasse9] = new List<TopicFactory> { SimplePastVsPresentPerfect, FirstConditional, PassiveVoice }
        };

    private static readonly (string Satz, string Loesung, string Regel)[] PresentListe =
    {
        ("Look! She ___ (run) to the bus.", "is running", "Bei einer Handlung, die gerade jetzt passiert, nutzt man Present Progressive (is/are + -ing)."),
        ("Every morning, he ___ (brush) his teeth.", "brushes", "Bei Gewohnheiten/regelmäßigen Handlungen nutzt man Simple Present (bei he/she/it + -s)."),
        ("I usually ___ (walk) to school, but today I ___ (take) the bus.", "walk / am taking", "Gewohnheit = Simple Present (\"usually\"), aktuelle Ausnahme = Present Progressive (\"today\").")
    };

    private static QuizQuestion SimplePresentVsProgressive(Random r)
    {
        var p = PresentListe[r.Next(PresentListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Simple Present vs. Present Progressive", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Form ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "Simple Present für Gewohnheiten/Routinen (he/she/it + -s); Present Progressive (is/are + -ing) für Handlungen gerade jetzt."
        };
    }

    private static readonly (string Singular, string Plural, string[] Falsch)[] PluralListe =
    {
        ("child", "children", new[] { "childs", "childes" }),
        ("mouse", "mice", new[] { "mouses", "mices" }),
        ("foot", "feet", new[] { "foots", "feets" }),
        ("man", "men", new[] { "mans", "manes" })
    };

    private static QuizQuestion IrregularPlurals(Random r)
    {
        var p = PluralListe[r.Next(PluralListe.Length)];
        var optionen = new[] { p.Plural }.Concat(p.Falsch).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Unregelmäßige Pluralformen", Type = QuestionType.MultipleChoice,
            Prompt = $"What is the plural of \"{p.Singular}\"?",
            Options = optionen, CorrectAnswers = new[] { p.Plural },
            Explanation = $"Die Pluralform von \"{p.Singular}\" ist unregelmäßig: \"{p.Plural}\" (kein einfaches -s).",
            HelpHint = "Manche englische Nomen bilden den Plural nicht mit -s, sondern ändern die Wortform komplett (child-children, mouse-mice, foot-feet, man-men)."
        };
    }

    private static readonly (string Satz, string[] Optionen, string Antwort, string Erklaerung)[] WhListe =
    {
        ("___ is your birthday?", new[] { "When", "Who", "Why" }, "When", "\"When\" fragt nach einem Zeitpunkt - passend für ein Datum wie einen Geburtstag."),
        ("___ is the capital of Germany?", new[] { "What", "Who", "When" }, "What", "\"What\" fragt nach einer Sache/einem Ort/Begriff, hier nach dem Namen der Hauptstadt."),
        ("___ lives next door to you?", new[] { "Who", "What", "Where" }, "Who", "\"Who\" fragt nach einer Person.")
    };

    private static QuizQuestion QuestionWords(Random r)
    {
        var f = WhListe[r.Next(WhListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Question Words", Type = QuestionType.MultipleChoice,
            Prompt = f.Satz, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "When = Zeitpunkt, Who = Person, What = Sache/Begriff, Where = Ort, Why = Grund."
        };
    }

    private static readonly (string Satz, string Loesung, string Regel)[] PastPerfectListe =
    {
        ("I ___ (visit) London last year.", "visited", "Eine abgeschlossene Handlung mit genauer Zeitangabe in der Vergangenheit (\"last year\") steht im Simple Past."),
        ("She ___ (never/be) to the USA. (up to now)", "has never been", "Eine Erfahrung ohne genaue Zeitangabe, mit Bezug zur Gegenwart, steht im Present Perfect (has/have + Partizip II)."),
        ("We ___ (already/finish) our homework, so we can go out now.", "have already finished", "Present Perfect wird genutzt, wenn das Ergebnis einer Handlung für die Gegenwart wichtig ist.")
    };

    private static QuizQuestion SimplePastVsPresentPerfect(Random r)
    {
        var p = PastPerfectListe[r.Next(PastPerfectListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Simple Past vs. Present Perfect", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Form ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "Simple Past bei genauer Zeitangabe in der Vergangenheit (\"last year\"); Present Perfect (has/have + Partizip II) bei Erfahrungen/Ergebnissen ohne genaue Zeit."
        };
    }

    private static readonly (string Satz, string Loesung, string Regel)[] ConditionalListe =
    {
        ("If it ___ (rain) tomorrow, we will stay at home.", "rains", "Conditional Type 1: If + Simple Present, ... will + Grundform (für eine realistische zukünftige Möglichkeit)."),
        ("If you study hard, you ___ (pass) the exam.", "will pass", "Im Hauptsatz des 1. Conditionals steht \"will\" + Grundform des Verbs."),
        ("If she ___ (have) time, she will call you.", "has", "Der \"if\"-Nebensatz steht im Simple Present, auch wenn es um die Zukunft geht.")
    };

    private static QuizQuestion FirstConditional(Random r)
    {
        var p = ConditionalListe[r.Next(ConditionalListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Conditional Sentences (Type 1)", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Form ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "1. Conditional: If + Simple Present, ... will + Grundform. Der \"if\"-Satz bleibt im Simple Present, auch wenn es um die Zukunft geht."
        };
    }

    private static readonly (string Aktiv, string Passiv)[] PassivListe =
    {
        ("The company builds many cars.", "Many cars are built by the company."),
        ("Someone stole my bike yesterday.", "My bike was stolen yesterday."),
        ("They will announce the results tomorrow.", "The results will be announced tomorrow.")
    };

    private static QuizQuestion PassiveVoice(Random r)
    {
        var p = PassivListe[r.Next(PassivListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Passive Voice", Type = QuestionType.OpenText,
            Prompt = $"Wandle in die Passiv-Form (Passive Voice) um: \"{p.Aktiv}\"",
            CorrectAnswers = new[] { p.Passiv },
            Explanation = $"Passiv-Form: \"{p.Passiv}\". Bildung: Objekt des Aktivsatzes wird zum Subjekt, Verb als \"be\" + Partizip II.",
            HelpHint = "Passive Voice: Objekt des Aktivsatzes wird zum Subjekt, Verb = Form von \"to be\" + Partizip II (3. Form), der Handelnde steht optional mit \"by\"."
        };
    }
}
