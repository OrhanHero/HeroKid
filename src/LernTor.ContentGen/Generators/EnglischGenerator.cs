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
        ("I usually ___ (walk) to school, but today I ___ (take) the bus.", "walk / am taking", "Gewohnheit = Simple Present (\"usually\"), aktuelle Ausnahme = Present Progressive (\"today\")."),
        ("They ___ (not/like) football, but they ___ (play) it every weekend.", "don't like / play", "Verneinung im Simple Present mit \"don't\" + Grundform; die Gewohnheit (\"every weekend\") bleibt im Simple Present."),
        ("Right now, the children ___ (play) in the garden.", "are playing", "Eine Handlung, die genau JETZT im Moment passiert, steht im Present Progressive (are + -ing)."),
        ("She ___ (read) a book right now.", "is reading", "\"right now\" zeigt eine Handlung im Moment - Present Progressive."),
        ("He ___ (watch) TV every evening.", "watches", "\"every evening\" zeigt eine Gewohnheit - Simple Present (he/she/it + -es)."),
        ("Look! The dog ___ (chase) the cat.", "is chasing", "\"Look!\" signalisiert eine Handlung genau jetzt - Present Progressive."),
        ("We ___ (not/go) to school on Sundays.", "don't go", "Verneinte Gewohnheit im Simple Present mit \"don't\" + Grundform."),
        ("At the moment, they ___ (build) a new house.", "are building", "\"at the moment\" zeigt eine Handlung, die gerade läuft - Present Progressive."),
        ("My sister ___ (play) the piano every day.", "plays", "\"every day\" zeigt eine Gewohnheit - Simple Present."),
        ("Listen! Someone ___ (knock) on the door.", "is knocking", "\"Listen!\" zeigt eine Handlung genau jetzt - Present Progressive."),
        ("I ___ (not/like) coffee, but I ___ (drink) tea now.", "don't like / am drinking", "Gewohnheit (\"don't like\") im Simple Present, aktuelle Handlung (\"now\") im Present Progressive."),
        ("The sun ___ (rise) in the east.", "rises", "Allgemeingültige Tatsachen stehen im Simple Present."),
        ("Right now, it ___ (rain) heavily outside.", "is raining", "\"right now\" zeigt eine Handlung im Moment - Present Progressive."),
        ("She always ___ (arrive) late.", "arrives", "\"always\" zeigt eine Gewohnheit - Simple Present."),
        ("Be quiet! The baby ___ (sleep).", "is sleeping", "Eine Handlung, die gerade jetzt passiert, steht im Present Progressive."),
        ("Water ___ (boil) at 100 degrees Celsius.", "boils", "Naturwissenschaftliche Tatsachen stehen im Simple Present."),
        ("At the moment, I ___ (write) an email.", "am writing", "\"at the moment\" zeigt eine Handlung im Moment - Present Progressive."),
        ("He never ___ (eat) meat.", "eats", "\"never\" zeigt eine Gewohnheit - Simple Present.")
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
        ("man", "men", new[] { "mans", "manes" }),
        ("tooth", "teeth", new[] { "tooths", "teeths" }),
        ("goose", "geese", new[] { "gooses", "geeses" }),
        ("person", "people", new[] { "persons", "peoples" }),
        ("ox", "oxen", new[] { "oxes", "oxs" }),
        ("sheep", "sheep", new[] { "sheeps", "sheepes" }),
        ("fish", "fish", new[] { "fishes", "fishs" }),
        ("leaf", "leaves", new[] { "leafs", "leafes" }),
        ("wife", "wives", new[] { "wifes", "wifen" }),
        ("knife", "knives", new[] { "knifes", "knifen" }),
        ("half", "halves", new[] { "halfs", "halfes" }),
        ("wolf", "wolves", new[] { "wolfs", "wolfes" }),
        ("potato", "potatoes", new[] { "potatos", "potatoies" }),
        ("tomato", "tomatoes", new[] { "tomatos", "tomatoies" }),
        ("die", "dice", new[] { "dies", "dices" }),
        ("deer", "deer", new[] { "deers", "deeres" }),
        ("life", "lives", new[] { "lifes", "lifen" })
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
        ("___ lives next door to you?", new[] { "Who", "What", "Where" }, "Who", "\"Who\" fragt nach einer Person."),
        ("___ do you go to school - by bus or on foot?", new[] { "How", "What", "Who" }, "How", "\"How\" fragt nach der Art und Weise - hier nach dem Verkehrsmittel."),
        ("___ is the museum? - It's next to the park.", new[] { "Where", "When", "Who" }, "Where", "\"Where\" fragt nach einem Ort."),
        ("___ old are you?", new[] { "How", "What", "When" }, "How", "\"How old\" fragt nach dem Alter."),
        ("___ many books do you have?", new[] { "How", "What", "Who" }, "How", "\"How many\" fragt nach einer zählbaren Menge."),
        ("___ does the film start?", new[] { "When", "Who", "What" }, "When", "\"When\" fragt nach einem Zeitpunkt."),
        ("___ is your favourite colour?", new[] { "What", "Who", "Where" }, "What", "\"What\" fragt nach einer Sache, hier nach der Lieblingsfarbe."),
        ("___ is coming to the party?", new[] { "Who", "What", "How" }, "Who", "\"Who\" fragt nach einer Person."),
        ("___ can I find the station?", new[] { "Where", "When", "Why" }, "Where", "\"Where\" fragt nach einem Ort."),
        ("___ don't you like vegetables?", new[] { "Why", "Where", "Who" }, "Why", "\"Why\" fragt nach einem Grund."),
        ("___ is your teacher's name?", new[] { "What", "Who", "How" }, "What", "\"What\" fragt hier nach dem Namen als Begriff."),
        ("___ did you go on holiday?", new[] { "Where", "Why", "Who" }, "Where", "\"Where\" fragt nach einem Ort."),
        ("___ long does the trip take?", new[] { "How", "What", "When" }, "How", "\"How long\" fragt nach einer Dauer."),
        ("___ is your birthday party?", new[] { "When", "Who", "What" }, "When", "\"When\" fragt nach einem Zeitpunkt."),
        ("___ made this beautiful cake?", new[] { "Who", "What", "Why" }, "Who", "\"Who\" fragt nach einer Person."),
        ("___ much does this cost?", new[] { "How", "What", "Why" }, "How", "\"How much\" fragt nach einer Menge/einem Preis."),
        ("___ do you go to bed?", new[] { "When", "Where", "Why" }, "When", "\"When\" fragt nach einem Zeitpunkt."),
        ("___ is your best friend?", new[] { "Who", "What", "How" }, "Who", "\"Who\" fragt nach einer Person.")
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
        ("We ___ (already/finish) our homework, so we can go out now.", "have already finished", "Present Perfect wird genutzt, wenn das Ergebnis einer Handlung für die Gegenwart wichtig ist."),
        ("He ___ (move) to Berlin in 2020.", "moved", "Eine abgeschlossene Handlung mit einem genauen Zeitpunkt in der Vergangenheit (\"in 2020\") steht im Simple Past."),
        ("I ___ (just/finish) my homework.", "have just finished", "\"just\" weist auf eine gerade abgeschlossene Handlung mit Bezug zur Gegenwart hin - deshalb Present Perfect."),
        ("I ___ (see) that movie three times.", "have seen", "Eine Erfahrung ohne genaue Zeitangabe steht im Present Perfect."),
        ("She ___ (graduate) in 2019.", "graduated", "\"in 2019\" ist eine genaue Zeitangabe - Simple Past."),
        ("They ___ (not/visit) Paris yet.", "haven't visited", "\"yet\" (bisher/noch nicht) signalisiert Present Perfect."),
        ("We ___ (live) in Berlin for five years.", "have lived", "\"for\" + Zeitspanne bis jetzt zeigt Present Perfect."),
        ("He ___ (call) me yesterday.", "called", "\"yesterday\" ist eine genaue Zeitangabe - Simple Past."),
        ("___ you ever ___ (be) to Turkey?", "Have...been", "\"ever\" in Fragen nach Erfahrungen zeigt Present Perfect."),
        ("I ___ (lose) my keys and I can't find them now.", "have lost", "Das Ergebnis ist für die Gegenwart wichtig - Present Perfect."),
        ("She ___ (start) her new job last Monday.", "started", "\"last Monday\" ist eine genaue Zeitangabe - Simple Past."),
        ("We ___ (already/eat) dinner.", "have already eaten", "\"already\" weist auf eine abgeschlossene Handlung mit Bezug zur Gegenwart hin - Present Perfect."),
        ("He ___ (write) three books so far.", "has written", "\"so far\" (bisher) zeigt Present Perfect."),
        ("I ___ (meet) her at the party last week.", "met", "\"last week\" ist eine genaue Zeitangabe - Simple Past."),
        ("They ___ (just/arrive) at the airport.", "have just arrived", "\"just\" zeigt eine gerade abgeschlossene Handlung - Present Perfect."),
        ("She ___ (not/finish) her homework yet.", "hasn't finished", "\"yet\" in Verneinungen zeigt Present Perfect."),
        ("We ___ (travel) to Spain in 2018.", "travelled", "\"in 2018\" ist eine genaue Zeitangabe - Simple Past."),
        ("He ___ (never/try) sushi before.", "has never tried", "\"never...before\" bei Erfahrungen zeigt Present Perfect.")
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
        ("If she ___ (have) time, she will call you.", "has", "Der \"if\"-Nebensatz steht im Simple Present, auch wenn es um die Zukunft geht."),
        ("If we ___ (leave) now, we will catch the train.", "leave", "Der \"if\"-Satz des 1. Conditionals steht im Simple Present, auch bei zukünftiger Bedeutung."),
        ("She will be happy if you ___ (come) to the party.", "come", "Nach \"if\" steht im 1. Conditional das Simple Present, im Hauptsatz \"will\" + Grundform."),
        ("If it ___ (snow), we will build a snowman.", "snows", "Der \"if\"-Satz steht im Simple Present, auch bei zukünftiger Bedeutung."),
        ("If you ___ (not/hurry), you will miss the bus.", "don't hurry", "Verneinung im \"if\"-Satz mit \"don't\" + Grundform, Simple Present."),
        ("We will go to the beach if the weather ___ (be) nice.", "is", "Der \"if\"-Satz steht im Simple Present."),
        ("If I ___ (have) enough money, I will buy a new phone.", "have", "Der \"if\"-Satz steht im Simple Present."),
        ("You will feel better if you ___ (get) some rest.", "get", "Der \"if\"-Satz steht im Simple Present."),
        ("If they ___ (win) the match, they will celebrate.", "win", "Der \"if\"-Satz steht im Simple Present."),
        ("She will call you if she ___ (need) help.", "needs", "Bei he/she/it steht im Simple Present ein -s."),
        ("If we ___ (miss) the train, we will take the next one.", "miss", "Der \"if\"-Satz steht im Simple Present."),
        ("I will help you if you ___ (ask) me.", "ask", "Der \"if\"-Satz steht im Simple Present."),
        ("If he ___ (not/study), he will fail the test.", "doesn't study", "Verneinung bei he/she/it mit \"doesn't\" + Grundform."),
        ("They will be happy if it ___ (stop) raining.", "stops", "Bei it steht im Simple Present ein -s."),
        ("If you ___ (eat) too much candy, you will get a stomachache.", "eat", "Der \"if\"-Satz steht im Simple Present."),
        ("We will arrive late if the bus ___ (be) delayed.", "is", "Der \"if\"-Satz steht im Simple Present."),
        ("If she ___ (practise) every day, she will improve quickly.", "practises", "Bei she steht im Simple Present ein -s."),
        ("You will pass the exam if you ___ (revise) carefully.", "revise", "Der \"if\"-Satz steht im Simple Present.")
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
        ("They will announce the results tomorrow.", "The results will be announced tomorrow."),
        ("The teacher checks the homework every day.", "The homework is checked by the teacher every day."),
        ("People speak English all over the world.", "English is spoken all over the world."),
        ("The cat scared the mouse.", "The mouse was scared by the cat."),
        ("The government builds new schools.", "New schools are built by the government."),
        ("Somebody broke the window.", "The window was broken."),
        ("They will publish the book next year.", "The book will be published next year."),
        ("The chef prepares the meal every day.", "The meal is prepared by the chef every day."),
        ("The artist painted this picture.", "This picture was painted by the artist."),
        ("People drink tea in England.", "Tea is drunk in England."),
        ("The company produces thousands of cars.", "Thousands of cars are produced by the company."),
        ("Someone cleaned the classroom yesterday.", "The classroom was cleaned yesterday."),
        ("They will deliver the package tomorrow.", "The package will be delivered tomorrow."),
        ("The teacher explains the rules every lesson.", "The rules are explained by the teacher every lesson."),
        ("Workers built this bridge in 1990.", "This bridge was built in 1990."),
        ("Millions of people watch this show.", "This show is watched by millions of people."),
        ("The dentist checks my teeth twice a year.", "My teeth are checked by the dentist twice a year."),
        ("Someone stole her wallet.", "Her wallet was stolen.")
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
