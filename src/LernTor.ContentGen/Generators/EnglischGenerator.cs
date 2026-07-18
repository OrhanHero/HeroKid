using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Englisch als Fremdsprache, Klasse 6 (Grundlagen), Klasse 7 (Aufbau) und Klasse 9 (vertieft).</summary>
public sealed class EnglischGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Englisch;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                SimplePresentVsProgressive, IrregularPlurals, QuestionWords,
                AlltagUndFamilie, SchuleUndGesellschaft, KulturUndTraditionen, NaturUndUmwelt
            },
            [GradeLevel.Klasse7] = new List<TopicFactory>
            {
                SimplePastVsPastProgressive, GoingToUndWillFuture, ComparativeSuperlative,
                SomeAnyMuchMany, FreizeitUndReisen, GrossbritannienLandeskunde
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                SimplePastVsPresentPerfect, FirstConditional, PassiveVoice,
                IdentitaetUndZukunft, GesellschaftUndMedien, UmweltUndNachhaltigkeit,
                AlltagUndKonsum, SchuleUndArbeitswelt, KulturUndHistorischerHintergrund
            }
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AlltagListe =
    {
        ("Wie sagt man auf Englisch \"meine Schwester\"?", new[] { "my sister", "my brother", "my mother" }, "my sister", "\"my sister\" bedeutet \"meine Schwester\"."),
        ("Wie sagt man auf Englisch \"Ich habe ein Hobby\"?", new[] { "I have a hobby", "I has a hobby", "I having a hobby" }, "I have a hobby", "Bei \"I\" wird das Verb \"have\" ohne -s verwendet."),
        ("Wie sagt man auf Englisch \"Taschengeld\"?", new[] { "pocket money", "hand money", "pocket coin" }, "pocket money", "\"pocket money\" ist die englische Bezeichnung für Taschengeld."),
        ("Was bedeutet \"to go shopping\" auf Deutsch?", new[] { "einkaufen gehen", "spazieren gehen", "schlafen gehen" }, "einkaufen gehen", "\"to go shopping\" bedeutet \"einkaufen gehen\"."),
        ("Wie sagt man auf Englisch \"mein Zimmer\"?", new[] { "my room", "my house", "my school" }, "my room", "\"my room\" bedeutet \"mein Zimmer\"."),
        ("Was bedeutet \"the living room\" auf Deutsch?", new[] { "das Wohnzimmer", "das Schlafzimmer", "die Küche" }, "das Wohnzimmer", "\"the living room\" ist das Wohnzimmer."),
        ("Wie sagt man auf Englisch \"Ich wohne in Berlin\"?", new[] { "I live in Berlin", "I lives in Berlin", "I living in Berlin" }, "I live in Berlin", "Bei \"I\" nutzt man \"live\" ohne -s."),
        ("Was bedeutet \"to get dressed\" auf Deutsch?", new[] { "sich anziehen", "sich hinsetzen", "aufstehen" }, "sich anziehen", "\"to get dressed\" bedeutet \"sich anziehen\"."),
        ("Wie sagt man auf Englisch \"Verabredung\" (mit Freunden)?", new[] { "meeting up", "meeting down", "meeting away" }, "meeting up", "\"meeting up\" bedeutet \"sich verabreden/treffen\"."),
        ("Was bedeutet \"neighbourhood\" auf Deutsch?", new[] { "Nachbarschaft", "Nachbar", "Nachtisch" }, "Nachbarschaft", "\"neighbourhood\" bedeutet \"Nachbarschaft\"."),
        ("Wie sagt man auf Englisch \"zu Fuß zur Schule gehen\"?", new[] { "walk to school", "run to school", "drive to school" }, "walk to school", "\"walk to school\" bedeutet \"zu Fuß zur Schule gehen\"."),
        ("Was bedeutet \"means of transport\" auf Deutsch?", new[] { "Verkehrsmittel", "Verkehrsschild", "Fahrschein" }, "Verkehrsmittel", "\"means of transport\" bedeutet \"Verkehrsmittel\"."),
        ("Wie sagt man auf Englisch \"meine Interessen\"?", new[] { "my interests", "my interest", "my interested" }, "my interests", "Im Plural heißt es \"my interests\"."),
        ("Was bedeutet \"to have breakfast\" auf Deutsch?", new[] { "frühstücken", "zu Mittag essen", "zu Abend essen" }, "frühstücken", "\"to have breakfast\" bedeutet \"frühstücken\"."),
        ("Wie sagt man auf Englisch \"Kleidung\"?", new[] { "clothes", "cloth", "clothing shop" }, "clothes", "\"clothes\" ist das allgemeine englische Wort für Kleidung."),
        ("Was bedeutet \"to celebrate\" auf Deutsch?", new[] { "feiern", "kochen", "putzen" }, "feiern", "\"to celebrate\" bedeutet \"feiern\"."),
        ("Wie sagt man auf Englisch \"Tagesablauf\"?", new[] { "daily routine", "daily rule", "day school" }, "daily routine", "\"daily routine\" bedeutet \"Tagesablauf\"."),
        ("Was bedeutet \"household chores\" auf Deutsch?", new[] { "Hausarbeiten", "Hausaufgaben", "Haustiere" }, "Hausarbeiten", "\"household chores\" bedeutet \"Hausarbeiten\"."),
        ("Wie sagt man auf Englisch \"Einkaufen gehen\"?", new[] { "go shopping", "go swimming", "go running" }, "go shopping", "\"go shopping\" bedeutet \"einkaufen gehen\"."),
        ("Was bedeutet \"personality\" auf Deutsch?", new[] { "Persönlichkeit", "Person", "Personal" }, "Persönlichkeit", "\"personality\" bedeutet \"Persönlichkeit\".")
    };

    private static QuizQuestion AlltagUndFamilie(Random r)
    {
        var f = AlltagListe[r.Next(AlltagListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Individuum und Lebenswelt: Alltag und Familie", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Familie, Wohnen und Alltag: my sister, my room, daily routine, pocket money."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SchuleListe =
    {
        ("Wie sagt man auf Englisch \"Unterrichtsfach\"?", new[] { "school subject", "school building", "school bag" }, "school subject", "\"school subject\" bedeutet \"Unterrichtsfach\"."),
        ("Was bedeutet \"classroom\" auf Deutsch?", new[] { "Klassenzimmer", "Klassenlehrer", "Klassenarbeit" }, "Klassenzimmer", "\"classroom\" bedeutet \"Klassenzimmer\"."),
        ("Wie sagt man auf Englisch \"Hausaufgaben machen\"?", new[] { "do homework", "make homework", "write homework" }, "do homework", "Im Englischen sagt man \"do homework\", nicht \"make homework\"."),
        ("Was bedeutet \"timetable\" auf Deutsch?", new[] { "Stundenplan", "Zeitzone", "Kalender" }, "Stundenplan", "\"timetable\" bedeutet \"Stundenplan\"."),
        ("Wie sagt man auf Englisch \"Nationalität\"?", new[] { "nationality", "nation", "national" }, "nationality", "\"nationality\" bedeutet \"Nationalität\"."),
        ("Was bedeutet \"rules\" auf Deutsch?", new[] { "Regeln", "Regale", "Regeln brechen" }, "Regeln", "\"rules\" bedeutet \"Regeln\"."),
        ("Wie sagt man auf Englisch \"Klassenkamerad\"?", new[] { "classmate", "class friend", "school buddy" }, "classmate", "\"classmate\" bedeutet \"Klassenkamerad/in\"."),
        ("Was bedeutet \"break time\" auf Deutsch?", new[] { "Pause", "Unterricht", "Ferien" }, "Pause", "\"break time\" bedeutet \"Pause\"."),
        ("Wie sagt man auf Englisch \"Schulbus\"?", new[] { "school bus", "school car", "school train" }, "school bus", "\"school bus\" bedeutet \"Schulbus\"."),
        ("Was bedeutet \"to raise your hand\" auf Deutsch?", new[] { "sich melden (die Hand heben)", "aufstehen", "sich hinsetzen" }, "sich melden (die Hand heben)", "\"to raise your hand\" bedeutet \"sich melden\"."),
        ("Wie sagt man auf Englisch \"Arbeitsmaterial\"?", new[] { "school supplies", "school food", "school clothes" }, "school supplies", "\"school supplies\" bedeutet \"Arbeitsmaterial/Schulsachen\"."),
        ("Was bedeutet \"to attend school\" auf Deutsch?", new[] { "eine Schule besuchen", "eine Schule bauen", "eine Schule schließen" }, "eine Schule besuchen", "\"to attend school\" bedeutet \"eine Schule besuchen\"."),
        ("Wie sagt man auf Englisch \"Land/Sprache\"?", new[] { "country/language", "city/dialect", "world/accent" }, "country/language", "\"country\" bedeutet Land, \"language\" bedeutet Sprache."),
        ("Was bedeutet \"to get along with someone\" auf Deutsch?", new[] { "gut mit jemandem auskommen", "jemanden ignorieren", "sich mit jemandem streiten" }, "gut mit jemandem auskommen", "\"to get along with someone\" bedeutet \"gut mit jemandem auskommen\"."),
        ("Wie sagt man auf Englisch \"Regeln befolgen\"?", new[] { "follow the rules", "break the rules", "write the rules" }, "follow the rules", "\"follow the rules\" bedeutet \"Regeln befolgen\"."),
        ("Was bedeutet \"headteacher\" auf Deutsch?", new[] { "Schulleiter/in", "Klassenlehrer/in", "Hausmeister/in" }, "Schulleiter/in", "\"headteacher\" bedeutet \"Schulleiter/in\"."),
        ("Wie sagt man auf Englisch \"im Klassenraum\"?", new[] { "in the classroom", "at the classroom", "on the classroom" }, "in the classroom", "Im Englischen nutzt man die Präposition \"in\" für \"im Klassenraum\"."),
        ("Was bedeutet \"to behave well\" auf Deutsch?", new[] { "sich gut benehmen", "sich schlecht benehmen", "sich verstecken" }, "sich gut benehmen", "\"to behave well\" bedeutet \"sich gut benehmen\"."),
        ("Wie sagt man auf Englisch \"das Schulsystem\"?", new[] { "the school system", "the school building", "the school year" }, "the school system", "\"the school system\" bedeutet \"das Schulsystem\"."),
        ("Was bedeutet \"multicultural\" auf Deutsch?", new[] { "multikulturell", "mehrsprachig", "internationale Küche" }, "multikulturell", "\"multicultural\" bedeutet \"multikulturell\".")
    };

    private static QuizQuestion SchuleUndGesellschaft(Random r)
    {
        var f = SchuleListe[r.Next(SchuleListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Gesellschaft: Schule und Zusammenleben", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Schule und Zusammenleben: classroom, timetable, school subject, follow the rules."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KulturListe =
    {
        ("In welcher Stadt steht der Big Ben?", new[] { "London", "New York", "Sydney" }, "London", "Der Big Ben steht in London, am britischen Parlament."),
        ("In welcher Stadt steht die Freiheitsstatue (Statue of Liberty)?", new[] { "New York", "London", "Los Angeles" }, "New York", "Die Freiheitsstatue steht im Hafen von New York."),
        ("Welches Fest feiern viele Menschen in den USA im November mit Truthahn?", new[] { "Thanksgiving", "Halloween", "Christmas" }, "Thanksgiving", "Thanksgiving wird in den USA im November mit einem Truthahn-Essen gefeiert."),
        ("Wie heißt das bekannte Riesenrad in London?", new[] { "The London Eye", "The Big Wheel", "The London Circle" }, "The London Eye", "\"The London Eye\" ist das berühmte Riesenrad an der Themse in London."),
        ("Welches Fest wird am 31. Oktober mit Kürbissen und Kostümen gefeiert?", new[] { "Halloween", "Thanksgiving", "Easter" }, "Halloween", "Halloween wird am 31. Oktober mit Kürbissen und Kostümen gefeiert."),
        ("Wie heißt die Königsfamilie-Residenz in London?", new[] { "Buckingham Palace", "Windsor Castle Street", "London Tower House" }, "Buckingham Palace", "Der Buckingham Palace ist die Residenz der britischen Königsfamilie in London."),
        ("Welche Sprache spricht man hauptsächlich in England?", new[] { "English", "German", "French" }, "English", "In England spricht man hauptsächlich Englisch."),
        ("Wie nennt man das amerikanische Unabhängigkeitsfest am 4. Juli?", new[] { "Independence Day", "Thanksgiving", "Labour Day" }, "Independence Day", "Der 4. Juli ist der amerikanische Unabhängigkeitstag (Independence Day)."),
        ("Welches berühmte Bauwerk aus Stein steht in Südengland (Wiltshire)?", new[] { "Stonehenge", "Big Ben", "Tower Bridge" }, "Stonehenge", "Stonehenge ist eine berühmte prähistorische Steinformation in Südengland."),
        ("Wie heißt die berühmte Brücke über die Themse in London?", new[] { "Tower Bridge", "Golden Gate Bridge", "Brooklyn Bridge" }, "Tower Bridge", "Die Tower Bridge überspannt die Themse in London."),
        ("In welchem Land liegt Hollywood?", new[] { "USA", "England", "Australien" }, "USA", "Hollywood liegt in Los Angeles, USA."),
        ("Welches Fahrzeug ist typisch für London (rot, zweistöckig)?", new[] { "double-decker bus", "yellow taxi", "tram" }, "double-decker bus", "Die roten, zweistöckigen Busse (double-decker buses) sind typisch für London."),
        ("Wie heißt der bekannte Brauch in englischsprachigen Ländern im Dezember?", new[] { "Christmas", "Thanksgiving", "Halloween" }, "Christmas", "Weihnachten (Christmas) wird im Dezember gefeiert."),
        ("Welches Sportereignis ist in England (Wimbledon) besonders bekannt?", new[] { "Tennis", "Baseball", "Eishockey" }, "Tennis", "Wimbledon ist eines der bekanntesten Tennisturniere der Welt."),
        ("Was ist \"fish and chips\"?", new[] { "Ein typisch britisches Gericht aus Fisch und Pommes", "Ein amerikanisches Dessert", "Ein englisches Frühstücksgetränk" }, "Ein typisch britisches Gericht aus Fisch und Pommes", "\"Fish and chips\" ist ein typisch britisches Gericht aus frittiertem Fisch und Pommes."),
        ("Welche berühmte Uhr befindet sich am britischen Parlament?", new[] { "Big Ben", "The London Eye", "Tower Bridge" }, "Big Ben", "Big Ben ist der Name der berühmten Turmuhr am britischen Parlament."),
        ("Wie heißt der amerikanische Nationalfeiertag zu Ehren der Arbeit im September?", new[] { "Labor Day", "Independence Day", "Memorial Day" }, "Labor Day", "Labor Day wird in den USA im September zu Ehren der Arbeit gefeiert."),
        ("In welcher amerikanischen Stadt liegt der Central Park?", new[] { "New York", "Los Angeles", "Chicago" }, "New York", "Der Central Park liegt mitten in New York City."),
        ("Wie nennt man die traditionelle britische Zeremonie mit Wachen vor dem Buckingham Palace?", new[] { "Changing of the Guard", "Royal Wedding", "Queen's Speech" }, "Changing of the Guard", "\"Changing of the Guard\" ist die traditionelle Wachablösung vor dem Buckingham Palace."),
        ("Welches Fest feiert man in englischsprachigen Ländern im Frühling mit bemalten Eiern?", new[] { "Easter", "Halloween", "Thanksgiving" }, "Easter", "Ostern (Easter) wird im Frühling u.a. mit bemalten Eiern gefeiert.")
    };

    private static QuizQuestion KulturUndTraditionen(Random r)
    {
        var f = KulturListe[r.Next(KulturListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kultur und historischer Hintergrund", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bekannte Orte und Feste: London (Big Ben, Tower Bridge), USA (Thanksgiving, Independence Day, Halloween)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NaturUmweltListe =
    {
        ("Wie sagt man auf Englisch \"Umweltschutz\"?", new[] { "environmental protection", "environment building", "nature shop" }, "environmental protection", "\"environmental protection\" bedeutet \"Umweltschutz\"."),
        ("Was bedeutet \"weather\" auf Deutsch?", new[] { "Wetter", "Klima", "Jahreszeit" }, "Wetter", "\"weather\" bedeutet \"Wetter\"."),
        ("Wie sagt man auf Englisch \"Mülltrennung\"?", new[] { "waste separation", "waste collection only", "waste burning" }, "waste separation", "\"waste separation\" bedeutet \"Mülltrennung\"."),
        ("Was bedeutet \"countryside\" auf Deutsch?", new[] { "das Land/die ländliche Gegend", "die Stadt", "das Meer" }, "das Land/die ländliche Gegend", "\"countryside\" bedeutet \"das Land/die ländliche Gegend\"."),
        ("Wie sagt man auf Englisch \"wilde Tiere\"?", new[] { "wild animals", "farm animals", "pet animals" }, "wild animals", "\"wild animals\" bedeutet \"wilde Tiere\"."),
        ("Was bedeutet \"to recycle\" auf Deutsch?", new[] { "recyceln/wiederverwerten", "wegwerfen", "verschmutzen" }, "recyceln/wiederverwerten", "\"to recycle\" bedeutet \"recyceln/wiederverwerten\"."),
        ("Wie sagt man auf Englisch \"Regenwald\"?", new[] { "rainforest", "rain field", "wet forest" }, "rainforest", "\"rainforest\" bedeutet \"Regenwald\"."),
        ("Was bedeutet \"pollution\" auf Deutsch?", new[] { "Verschmutzung", "Sauberkeit", "Frischluft" }, "Verschmutzung", "\"pollution\" bedeutet \"Verschmutzung\"."),
        ("Wie sagt man auf Englisch \"die Natur schützen\"?", new[] { "protect nature", "destroy nature", "ignore nature" }, "protect nature", "\"protect nature\" bedeutet \"die Natur schützen\"."),
        ("Was bedeutet \"plant\" (als Verb) auf Deutsch?", new[] { "pflanzen", "ernten", "gießen" }, "pflanzen", "\"plant\" als Verb bedeutet \"pflanzen\"."),
        ("Wie sagt man auf Englisch \"Klima\"?", new[] { "climate", "weather forecast", "temperature only" }, "climate", "\"climate\" bedeutet \"Klima\"."),
        ("Was bedeutet \"endangered species\" auf Deutsch?", new[] { "bedrohte Tierart", "häufige Tierart", "Haustierart" }, "bedrohte Tierart", "\"endangered species\" bedeutet \"bedrohte Tierart\"."),
        ("Wie sagt man auf Englisch \"erneuerbare Energie\"?", new[] { "renewable energy", "fossil energy", "no energy" }, "renewable energy", "\"renewable energy\" bedeutet \"erneuerbare Energie\"."),
        ("Was bedeutet \"to save water\" auf Deutsch?", new[] { "Wasser sparen", "Wasser verschwenden", "Wasser verschmutzen" }, "Wasser sparen", "\"to save water\" bedeutet \"Wasser sparen\"."),
        ("Wie sagt man auf Englisch \"Baum pflanzen\"?", new[] { "plant a tree", "cut a tree", "climb a tree" }, "plant a tree", "\"plant a tree\" bedeutet \"einen Baum pflanzen\"."),
        ("Was bedeutet \"natural habitat\" auf Deutsch?", new[] { "natürlicher Lebensraum", "Zoo", "Wohnhaus" }, "natürlicher Lebensraum", "\"natural habitat\" bedeutet \"natürlicher Lebensraum\"."),
        ("Wie sagt man auf Englisch \"Ozean\"?", new[] { "ocean", "lake", "river" }, "ocean", "\"ocean\" bedeutet \"Ozean\"."),
        ("Was bedeutet \"drought\" auf Deutsch?", new[] { "Dürre/Trockenheit", "Überschwemmung", "Schneesturm" }, "Dürre/Trockenheit", "\"drought\" bedeutet \"Dürre/Trockenheit\"."),
        ("Wie sagt man auf Englisch \"Plastikmüll\"?", new[] { "plastic waste", "plastic food", "plastic tree" }, "plastic waste", "\"plastic waste\" bedeutet \"Plastikmüll\"."),
        ("Was bedeutet \"to reduce\" (im Umweltkontext) auf Deutsch?", new[] { "verringern/reduzieren", "vergrößern", "verdoppeln" }, "verringern/reduzieren", "\"to reduce\" bedeutet \"verringern/reduzieren\".")
    };

    private static QuizQuestion NaturUndUmwelt(Random r)
    {
        var f = NaturUmweltListe[r.Next(NaturUmweltListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Natur und Umwelt", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Umwelt-Wortschatz: pollution, recycle, protect nature, renewable energy, endangered species."
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] IdentitaetListe =
    {
        ("Wie sagt man auf Englisch \"Zukunftspläne\"?", new[] { "future plans", "past plans", "present plans" }, "future plans", "\"future plans\" bedeutet \"Zukunftspläne\"."),
        ("Was bedeutet \"role model\" auf Deutsch?", new[] { "Vorbild", "Rollenspiel", "Schauspieler" }, "Vorbild", "\"role model\" bedeutet \"Vorbild\"."),
        ("Wie sagt man auf Englisch \"Sucht/Abhängigkeit\"?", new[] { "addiction", "attraction", "attention" }, "addiction", "\"addiction\" bedeutet \"Sucht/Abhängigkeit\"."),
        ("Was bedeutet \"to pursue a career\" auf Deutsch?", new[] { "eine Karriere verfolgen/anstreben", "eine Karriere beenden", "eine Karriere ablehnen" }, "eine Karriere verfolgen/anstreben", "\"to pursue a career\" bedeutet \"eine Karriere verfolgen/anstreben\"."),
        ("Wie sagt man auf Englisch \"Migration\"?", new[] { "migration", "vacation", "population" }, "migration", "\"migration\" bedeutet \"Migration\"."),
        ("Was bedeutet \"identity\" auf Deutsch?", new[] { "Identität", "Ideologie", "Idee" }, "Identität", "\"identity\" bedeutet \"Identität\"."),
        ("Wie sagt man auf Englisch \"Hoffnungen und Träume\"?", new[] { "hopes and dreams", "fears and doubts", "rules and laws" }, "hopes and dreams", "\"hopes and dreams\" bedeutet \"Hoffnungen und Träume\"."),
        ("Was bedeutet \"to overcome a challenge\" auf Deutsch?", new[] { "eine Herausforderung meistern", "eine Herausforderung vermeiden", "eine Herausforderung ignorieren" }, "eine Herausforderung meistern", "\"to overcome a challenge\" bedeutet \"eine Herausforderung meistern\"."),
        ("Wie sagt man auf Englisch \"Lebensentwurf\"?", new[] { "life plan", "life story", "life span" }, "life plan", "\"life plan\" bedeutet \"Lebensentwurf\"."),
        ("Was bedeutet \"peer pressure\" auf Deutsch?", new[] { "Gruppenzwang", "Elterndruck", "Schuldruck" }, "Gruppenzwang", "\"peer pressure\" bedeutet \"Gruppenzwang\"."),
        ("Wie sagt man auf Englisch \"Suchtprävention\"?", new[] { "addiction prevention", "addiction promotion", "addiction cure only" }, "addiction prevention", "\"addiction prevention\" bedeutet \"Suchtprävention\"."),
        ("Was bedeutet \"self-confidence\" auf Deutsch?", new[] { "Selbstvertrauen", "Selbstzweifel", "Selbstkontrolle" }, "Selbstvertrauen", "\"self-confidence\" bedeutet \"Selbstvertrauen\"."),
        ("Wie sagt man auf Englisch \"Generationenkonflikt\"?", new[] { "generation gap", "generation game", "generation growth" }, "generation gap", "\"generation gap\" bedeutet \"Generationenkonflikt\"."),
        ("Was bedeutet \"to set a goal\" auf Deutsch?", new[] { "sich ein Ziel setzen", "ein Ziel vergessen", "ein Ziel ablehnen" }, "sich ein Ziel setzen", "\"to set a goal\" bedeutet \"sich ein Ziel setzen\"."),
        ("Wie sagt man auf Englisch \"Vorurteil\"?", new[] { "prejudice", "preference", "president" }, "prejudice", "\"prejudice\" bedeutet \"Vorurteil\"."),
        ("Was bedeutet \"to struggle with something\" auf Deutsch?", new[] { "mit etwas kämpfen/Schwierigkeiten haben", "etwas problemlos schaffen", "etwas ignorieren" }, "mit etwas kämpfen/Schwierigkeiten haben", "\"to struggle with something\" bedeutet \"mit etwas kämpfen/Schwierigkeiten haben\"."),
        ("Wie sagt man auf Englisch \"Herkunft\"?", new[] { "background/origin", "future", "destination" }, "background/origin", "\"background/origin\" bedeutet \"Herkunft\"."),
        ("Was bedeutet \"to belong somewhere\" auf Deutsch?", new[] { "irgendwo dazugehören", "irgendwo fremd sein", "irgendwohin reisen" }, "irgendwo dazugehören", "\"to belong somewhere\" bedeutet \"irgendwo dazugehören\"."),
        ("Wie sagt man auf Englisch \"berühmte Persönlichkeit\"?", new[] { "celebrity/famous person", "unknown stranger", "ordinary citizen" }, "celebrity/famous person", "\"celebrity/famous person\" bedeutet \"berühmte Persönlichkeit\"."),
        ("Was bedeutet \"to make a decision\" auf Deutsch?", new[] { "eine Entscheidung treffen", "eine Entscheidung vergessen", "eine Entscheidung ablehnen" }, "eine Entscheidung treffen", "\"to make a decision\" bedeutet \"eine Entscheidung treffen\".")
    };

    private static QuizQuestion IdentitaetUndZukunft(Random r)
    {
        var f = IdentitaetListe[r.Next(IdentitaetListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Identität, Lebensentwürfe und Zukunft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Identität und Zukunft: future plans, role model, addiction, peer pressure, self-confidence."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GesellschaftMedienListe =
    {
        ("Wie sagt man auf Englisch \"Cybermobbing\"?", new[] { "cyberbullying", "cybersecurity", "cyberspace" }, "cyberbullying", "\"cyberbullying\" bedeutet \"Cybermobbing\"."),
        ("Was bedeutet \"stereotype\" auf Deutsch?", new[] { "Klischee/Stereotyp", "Statistik", "Strategie" }, "Klischee/Stereotyp", "\"stereotype\" bedeutet \"Klischee/Stereotyp\"."),
        ("Wie sagt man auf Englisch \"kulturelle Vielfalt\"?", new[] { "cultural diversity", "cultural unity", "cultural history" }, "cultural diversity", "\"cultural diversity\" bedeutet \"kulturelle Vielfalt\"."),
        ("Was bedeutet \"to discriminate against someone\" auf Deutsch?", new[] { "jemanden diskriminieren", "jemanden loben", "jemanden unterstützen" }, "jemanden diskriminieren", "\"to discriminate against someone\" bedeutet \"jemanden diskriminieren\"."),
        ("Wie sagt man auf Englisch \"soziale Medien\"?", new[] { "social media", "social class", "social work" }, "social media", "\"social media\" bedeutet \"soziale Medien\"."),
        ("Was bedeutet \"to spread rumours\" auf Deutsch?", new[] { "Gerüchte verbreiten", "Gerüchte widerlegen", "Gerüchte erfinden ohne sie zu teilen" }, "Gerüchte verbreiten", "\"to spread rumours\" bedeutet \"Gerüchte verbreiten\"."),
        ("Wie sagt man auf Englisch \"Meinungsfreiheit\"?", new[] { "freedom of speech", "freedom of movement", "freedom of choice" }, "freedom of speech", "\"freedom of speech\" bedeutet \"Meinungsfreiheit\"."),
        ("Was bedeutet \"to be biased\" auf Deutsch?", new[] { "voreingenommen sein", "neutral sein", "unwissend sein" }, "voreingenommen sein", "\"to be biased\" bedeutet \"voreingenommen sein\"."),
        ("Wie sagt man auf Englisch \"Vielfalt und Toleranz\"?", new[] { "diversity and tolerance", "uniformity and rejection", "similarity and doubt" }, "diversity and tolerance", "\"diversity and tolerance\" bedeutet \"Vielfalt und Toleranz\"."),
        ("Was bedeutet \"fake news\" auf Deutsch?", new[] { "Falschmeldungen/erfundene Nachrichten", "aktuelle Nachrichten", "geprüfte Nachrichten" }, "Falschmeldungen/erfundene Nachrichten", "\"fake news\" bedeutet \"Falschmeldungen/erfundene Nachrichten\"."),
        ("Wie sagt man auf Englisch \"Regeln des Zusammenlebens\"?", new[] { "rules of coexistence", "rules of sports", "rules of grammar" }, "rules of coexistence", "\"rules of coexistence\" bedeutet \"Regeln des Zusammenlebens\"."),
        ("Was bedeutet \"to respect differences\" auf Deutsch?", new[] { "Unterschiede respektieren", "Unterschiede ignorieren", "Unterschiede bekämpfen" }, "Unterschiede respektieren", "\"to respect differences\" bedeutet \"Unterschiede respektieren\"."),
        ("Wie sagt man auf Englisch \"Vorurteile abbauen\"?", new[] { "break down prejudice", "build up prejudice", "hide prejudice" }, "break down prejudice", "\"break down prejudice\" bedeutet \"Vorurteile abbauen\"."),
        ("Was bedeutet \"a multicultural society\" auf Deutsch?", new[] { "eine multikulturelle Gesellschaft", "eine einsprachige Gesellschaft", "eine isolierte Gesellschaft" }, "eine multikulturelle Gesellschaft", "\"a multicultural society\" bedeutet \"eine multikulturelle Gesellschaft\"."),
        ("Wie sagt man auf Englisch \"sich in jemanden hineinversetzen\"?", new[] { "put yourself in someone's shoes", "put someone in a box", "put someone on hold" }, "put yourself in someone's shoes", "\"put yourself in someone's shoes\" bedeutet \"sich in jemanden hineinversetzen\"."),
        ("Was bedeutet \"online privacy\" auf Deutsch?", new[] { "Privatsphäre im Internet", "Öffentlichkeit im Internet", "Werbung im Internet" }, "Privatsphäre im Internet", "\"online privacy\" bedeutet \"Privatsphäre im Internet\"."),
        ("Wie sagt man auf Englisch \"Nachrichtenquelle prüfen\"?", new[] { "check a news source", "ignore a news source", "invent a news source" }, "check a news source", "\"check a news source\" bedeutet \"eine Nachrichtenquelle prüfen\"."),
        ("Was bedeutet \"to bully someone\" auf Deutsch?", new[] { "jemanden mobben/schikanieren", "jemandem helfen", "jemanden loben" }, "jemanden mobben/schikanieren", "\"to bully someone\" bedeutet \"jemanden mobben/schikanieren\"."),
        ("Wie sagt man auf Englisch \"unterschiedliche Perspektiven\"?", new[] { "different perspectives", "same perspective", "no perspective" }, "different perspectives", "\"different perspectives\" bedeutet \"unterschiedliche Perspektiven\"."),
        ("Was bedeutet \"to raise awareness\" auf Deutsch?", new[] { "das Bewusstsein schärfen/aufmerksam machen", "etwas verschweigen", "etwas verbieten" }, "das Bewusstsein schärfen/aufmerksam machen", "\"to raise awareness\" bedeutet \"das Bewusstsein schärfen/aufmerksam machen\".")
    };

    private static QuizQuestion GesellschaftUndMedien(Random r)
    {
        var f = GesellschaftMedienListe[r.Next(GesellschaftMedienListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gesellschaft, Medien und Vielfalt", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Gesellschaft und Medien: cyberbullying, stereotype, fake news, cultural diversity, freedom of speech."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] UmweltNachhaltigkeitListe =
    {
        ("Wie sagt man auf Englisch \"CO2-Fußabdruck\"?", new[] { "carbon footprint", "carbon dioxide only", "footprint size" }, "carbon footprint", "\"carbon footprint\" bedeutet \"CO2-Fußabdruck\"."),
        ("Was bedeutet \"global warming\" auf Deutsch?", new[] { "globale Erwärmung", "globale Abkühlung", "globaler Handel" }, "globale Erwärmung", "\"global warming\" bedeutet \"globale Erwärmung\"."),
        ("Wie sagt man auf Englisch \"nachhaltig\"?", new[] { "sustainable", "unstable", "unavailable" }, "sustainable", "\"sustainable\" bedeutet \"nachhaltig\"."),
        ("Was bedeutet \"greenhouse gases\" auf Deutsch?", new[] { "Treibhausgase", "Frischluft", "Sauerstoff" }, "Treibhausgase", "\"greenhouse gases\" bedeutet \"Treibhausgase\"."),
        ("Wie sagt man auf Englisch \"fossile Brennstoffe\"?", new[] { "fossil fuels", "renewable fuels", "clean fuels" }, "fossil fuels", "\"fossil fuels\" bedeutet \"fossile Brennstoffe\"."),
        ("Was bedeutet \"to reduce emissions\" auf Deutsch?", new[] { "Emissionen verringern", "Emissionen erhöhen", "Emissionen ignorieren" }, "Emissionen verringern", "\"to reduce emissions\" bedeutet \"Emissionen verringern\"."),
        ("Wie sagt man auf Englisch \"erneuerbare Energien\"?", new[] { "renewable energy sources", "non-renewable energy sources", "no energy sources" }, "renewable energy sources", "\"renewable energy sources\" bedeutet \"erneuerbare Energien\"."),
        ("Was bedeutet \"plastic pollution\" auf Deutsch?", new[] { "Plastikverschmutzung", "Plastikherstellung nur", "Plastikrecycling" }, "Plastikverschmutzung", "\"plastic pollution\" bedeutet \"Plastikverschmutzung\"."),
        ("Wie sagt man auf Englisch \"Klimawandel bekämpfen\"?", new[] { "fight climate change", "ignore climate change", "cause climate change" }, "fight climate change", "\"fight climate change\" bedeutet \"den Klimawandel bekämpfen\"."),
        ("Was bedeutet \"to conserve resources\" auf Deutsch?", new[] { "Ressourcen schonen", "Ressourcen verschwenden", "Ressourcen verkaufen" }, "Ressourcen schonen", "\"to conserve resources\" bedeutet \"Ressourcen schonen\"."),
        ("Wie sagt man auf Englisch \"ökologischer Fußabdruck\"?", new[] { "ecological footprint", "economic footprint", "physical footprint" }, "ecological footprint", "\"ecological footprint\" bedeutet \"ökologischer Fußabdruck\"."),
        ("Was bedeutet \"environmental awareness\" auf Deutsch?", new[] { "Umweltbewusstsein", "Umweltverschmutzung", "Umweltzerstörung" }, "Umweltbewusstsein", "\"environmental awareness\" bedeutet \"Umweltbewusstsein\"."),
        ("Wie sagt man auf Englisch \"Meeresspiegelanstieg\"?", new[] { "rising sea levels", "falling sea levels", "sea level only" }, "rising sea levels", "\"rising sea levels\" bedeutet \"Meeresspiegelanstieg\"."),
        ("Was bedeutet \"to go vegan/vegetarian\" auf Deutsch?", new[] { "vegan/vegetarisch leben", "nur Fleisch essen", "nichts essen" }, "vegan/vegetarisch leben", "\"to go vegan/vegetarian\" bedeutet \"vegan/vegetarisch leben\"."),
        ("Wie sagt man auf Englisch \"Naturkatastrophe\"?", new[] { "natural disaster", "natural beauty", "natural resource only" }, "natural disaster", "\"natural disaster\" bedeutet \"Naturkatastrophe\"."),
        ("Was bedeutet \"to protect the environment\" auf Deutsch?", new[] { "die Umwelt schützen", "die Umwelt zerstören", "die Umwelt ignorieren" }, "die Umwelt schützen", "\"to protect the environment\" bedeutet \"die Umwelt schützen\"."),
        ("Wie sagt man auf Englisch \"Umweltverschmutzung verursachen\"?", new[] { "cause pollution", "prevent pollution", "measure pollution only" }, "cause pollution", "\"cause pollution\" bedeutet \"Umweltverschmutzung verursachen\"."),
        ("Was bedeutet \"clean energy\" auf Deutsch?", new[] { "saubere Energie", "schmutzige Energie", "teure Energie" }, "saubere Energie", "\"clean energy\" bedeutet \"saubere Energie\"."),
        ("Wie sagt man auf Englisch \"nachhaltiger Konsum\"?", new[] { "sustainable consumption", "excessive consumption", "no consumption" }, "sustainable consumption", "\"sustainable consumption\" bedeutet \"nachhaltiger Konsum\"."),
        ("Was bedeutet \"to take action against climate change\" auf Deutsch?", new[] { "gegen den Klimawandel aktiv werden", "den Klimawandel ignorieren", "den Klimawandel leugnen" }, "gegen den Klimawandel aktiv werden", "\"to take action against climate change\" bedeutet \"gegen den Klimawandel aktiv werden\".")
    };

    private static QuizQuestion UmweltUndNachhaltigkeit(Random r)
    {
        var f = UmweltNachhaltigkeitListe[r.Next(UmweltNachhaltigkeitListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Umwelt und Nachhaltigkeit", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Umwelt und Nachhaltigkeit: carbon footprint, global warming, renewable energy, sustainable."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KonsumListe =
    {
        ("Wie sagt man auf Englisch \"Verbraucher/Konsument\"?", new[] { "consumer", "container", "commuter" }, "consumer", "\"consumer\" bedeutet \"Verbraucher/Konsument\"."),
        ("Was bedeutet \"advertisement\" auf Deutsch?", new[] { "Werbung/Anzeige", "Abenteuer", "Vereinbarung" }, "Werbung/Anzeige", "\"advertisement\" bedeutet \"Werbung/Anzeige\"."),
        ("Wie sagt man auf Englisch \"Marke\"?", new[] { "brand", "brain", "brand new" }, "brand", "\"brand\" bedeutet \"Marke\"."),
        ("Was bedeutet \"to save money\" auf Deutsch?", new[] { "Geld sparen", "Geld ausgeben", "Geld verlieren" }, "Geld sparen", "\"to save money\" bedeutet \"Geld sparen\"."),
        ("Wie sagt man auf Englisch \"Rabatt\"?", new[] { "discount", "discourse", "discomfort" }, "discount", "\"discount\" bedeutet \"Rabatt\"."),
        ("Was bedeutet \"refund\" auf Deutsch?", new[] { "Rückerstattung", "Rechnung", "Rabatt" }, "Rückerstattung", "\"refund\" bedeutet \"Rückerstattung\"."),
        ("Wie sagt man auf Englisch \"Kassenbon/Quittung\"?", new[] { "receipt", "recipe", "reception" }, "receipt", "\"receipt\" bedeutet \"Kassenbon/Quittung\"."),
        ("Was bedeutet \"warranty\" auf Deutsch?", new[] { "Garantie", "Warnung", "Lagerhaus" }, "Garantie", "\"warranty\" bedeutet \"Garantie\"."),
        ("Wie sagt man auf Englisch \"sich beschweren\"?", new[] { "to complain", "to complete", "to compliment" }, "to complain", "\"to complain\" bedeutet \"sich beschweren\"."),
        ("Was bedeutet \"consumer rights\" auf Deutsch?", new[] { "Verbraucherrechte", "Konsumverbote", "Kaufverträge" }, "Verbraucherrechte", "\"consumer rights\" bedeutet \"Verbraucherrechte\"."),
        ("Wie sagt man auf Englisch \"Abonnement\"?", new[] { "subscription", "description", "prescription" }, "subscription", "\"subscription\" bedeutet \"Abonnement\"."),
        ("Was bedeutet \"to cancel a subscription\" auf Deutsch?", new[] { "ein Abonnement kündigen", "ein Abonnement abschließen", "ein Abonnement verschenken" }, "ein Abonnement kündigen", "\"to cancel a subscription\" bedeutet \"ein Abonnement kündigen\"."),
        ("Wie sagt man auf Englisch \"Online-Einkauf\"?", new[] { "online shopping", "online banking", "online gaming" }, "online shopping", "\"online shopping\" bedeutet \"Online-Einkauf\"."),
        ("Was bedeutet \"delivery\" auf Deutsch?", new[] { "Lieferung", "Entdeckung", "Entwicklung" }, "Lieferung", "\"delivery\" bedeutet \"Lieferung\"."),
        ("Wie sagt man auf Englisch \"einen Artikel zurückgeben\"?", new[] { "to return an item", "to remove an item", "to repeat an item" }, "to return an item", "\"to return an item\" bedeutet \"einen Artikel zurückgeben\"."),
        ("Was bedeutet \"sustainable consumption\" auf Deutsch?", new[] { "nachhaltiger Konsum", "übermäßiger Konsum", "verbotener Konsum" }, "nachhaltiger Konsum", "\"sustainable consumption\" bedeutet \"nachhaltiger Konsum\"."),
        ("Wie sagt man auf Englisch \"Gebraucht-/Second-Hand-Ware\"?", new[] { "second-hand goods", "second chance goods", "second class goods" }, "second-hand goods", "\"second-hand goods\" bedeutet \"Gebraucht-/Second-Hand-Ware\"."),
        ("Was bedeutet \"to spend money\" auf Deutsch?", new[] { "Geld ausgeben", "Geld sparen", "Geld verleihen" }, "Geld ausgeben", "\"to spend money\" bedeutet \"Geld ausgeben\"."),
        ("Wie sagt man auf Englisch \"Budget/Haushaltsplan\"?", new[] { "budget", "burden", "bundle" }, "budget", "\"budget\" bedeutet \"Budget/Haushaltsplan\"."),
        ("Was bedeutet \"misleading advertisement\" auf Deutsch?", new[] { "irreführende Werbung", "ehrliche Werbung", "verbotene Produkte" }, "irreführende Werbung", "\"misleading advertisement\" bedeutet \"irreführende Werbung\".")
    };

    private static QuizQuestion AlltagUndKonsum(Random r)
    {
        var f = KonsumListe[r.Next(KonsumListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Alltag, Konsum und Wohnwelt (Werbung, Verbraucherschutz)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Konsum und Verbraucherschutz: consumer, refund, warranty, subscription, sustainable consumption."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArbeitsweltListe =
    {
        ("Wie sagt man auf Englisch \"Bewerbung\"?", new[] { "job application", "job description", "job interview" }, "job application", "\"job application\" bedeutet \"Bewerbung\"."),
        ("Was bedeutet \"CV\" (curriculum vitae) auf Deutsch?", new[] { "Lebenslauf", "Anschreiben", "Zeugnis" }, "Lebenslauf", "\"CV\" (curriculum vitae) bedeutet \"Lebenslauf\"."),
        ("Wie sagt man auf Englisch \"Anschreiben/Bewerbungsschreiben\"?", new[] { "cover letter", "cover story", "cover page" }, "cover letter", "\"cover letter\" bedeutet \"Anschreiben/Bewerbungsschreiben\"."),
        ("Was bedeutet \"job interview\" auf Deutsch?", new[] { "Vorstellungsgespräch", "Arbeitsvertrag", "Kündigung" }, "Vorstellungsgespräch", "\"job interview\" bedeutet \"Vorstellungsgespräch\"."),
        ("Wie sagt man auf Englisch \"Qualifikation\"?", new[] { "qualification", "quantity", "qualification exam only" }, "qualification", "\"qualification\" bedeutet \"Qualifikation\"."),
        ("Was bedeutet \"apprenticeship\" auf Deutsch?", new[] { "Ausbildung/Lehre", "Praktikum", "Studium" }, "Ausbildung/Lehre", "\"apprenticeship\" bedeutet \"Ausbildung/Lehre\"."),
        ("Wie sagt man auf Englisch \"Praktikum\"?", new[] { "internship", "internal", "interview" }, "internship", "\"internship\" bedeutet \"Praktikum\"."),
        ("Was bedeutet \"employer\" auf Deutsch?", new[] { "Arbeitgeber", "Angestellte/r", "Arbeitsamt" }, "Arbeitgeber", "\"employer\" bedeutet \"Arbeitgeber\"."),
        ("Wie sagt man auf Englisch \"Angestellte/r\"?", new[] { "employee", "employer", "unemployed" }, "employee", "\"employee\" bedeutet \"Angestellte/r\"."),
        ("Was bedeutet \"salary\" auf Deutsch?", new[] { "Gehalt", "Urlaub", "Rente" }, "Gehalt", "\"salary\" bedeutet \"Gehalt\"."),
        ("Wie sagt man auf Englisch \"Teilzeitjob\"?", new[] { "part-time job", "full-time job", "part of a job" }, "part-time job", "\"part-time job\" bedeutet \"Teilzeitjob\"."),
        ("Was bedeutet \"skills\" auf Deutsch?", new[] { "Fähigkeiten/Kenntnisse", "Aufgaben", "Zeugnisse" }, "Fähigkeiten/Kenntnisse", "\"skills\" bedeutet \"Fähigkeiten/Kenntnisse\"."),
        ("Wie sagt man auf Englisch \"sich um eine Stelle bewerben\"?", new[] { "to apply for a job", "to apologize for a job", "to appoint a job" }, "to apply for a job", "\"to apply for a job\" bedeutet \"sich um eine Stelle bewerben\"."),
        ("Was bedeutet \"work experience\" auf Deutsch?", new[] { "Arbeitserfahrung/Praxiserfahrung", "Arbeitszeit", "Arbeitsplatz" }, "Arbeitserfahrung/Praxiserfahrung", "\"work experience\" bedeutet \"Arbeitserfahrung/Praxiserfahrung\"."),
        ("Wie sagt man auf Englisch \"Abgabefrist\"?", new[] { "deadline", "headline", "byline" }, "deadline", "\"deadline\" bedeutet \"Abgabefrist\"."),
        ("Was bedeutet \"vocational training\" auf Deutsch?", new[] { "Berufsausbildung", "Freiwilligenarbeit", "Urlaubsreise" }, "Berufsausbildung", "\"vocational training\" bedeutet \"Berufsausbildung\"."),
        ("Wie sagt man auf Englisch \"Karriere/Laufbahn\"?", new[] { "career", "carrier", "career break only" }, "career", "\"career\" bedeutet \"Karriere/Laufbahn\"."),
        ("Was bedeutet \"to be hired\" auf Deutsch?", new[] { "eingestellt werden", "entlassen werden", "befördert werden" }, "eingestellt werden", "\"to be hired\" bedeutet \"eingestellt werden\"."),
        ("Wie sagt man auf Englisch \"entlassen werden\"?", new[] { "to be fired", "to be hired", "to be tired" }, "to be fired", "\"to be fired\" bedeutet \"entlassen werden\"."),
        ("Was bedeutet \"job vacancy\" auf Deutsch?", new[] { "offene Stelle", "Kündigungsschreiben", "Arbeitsvertrag" }, "offene Stelle", "\"job vacancy\" bedeutet \"offene Stelle\".")
    };

    private static QuizQuestion SchuleUndArbeitswelt(Random r)
    {
        var f = ArbeitsweltListe[r.Next(ArbeitsweltListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Schule, Ausbildung und Arbeitswelt (Bewerbung)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Bewerbung und Arbeitswelt: job application, CV, cover letter, job interview, to apply for a job."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HistorischerHintergrundListe =
    {
        ("Wie sagt man auf Englisch \"Kulturerbe\"?", new[] { "heritage", "inheritance only", "hermitage" }, "heritage", "\"heritage\" bedeutet \"Kulturerbe\"."),
        ("Was bedeutet \"tradition\" auf Deutsch?", new[] { "Tradition/Brauch", "Übersetzung", "Transaktion" }, "Tradition/Brauch", "\"tradition\" bedeutet \"Tradition/Brauch\"."),
        ("Wie sagt man auf Englisch \"historisches Ereignis\"?", new[] { "historical event", "historic building", "history book" }, "historical event", "\"historical event\" bedeutet \"historisches Ereignis\"."),
        ("Was bedeutet \"monument\" auf Deutsch?", new[] { "Denkmal", "Moment", "Dokument" }, "Denkmal", "\"monument\" bedeutet \"Denkmal\"."),
        ("Wie sagt man auf Englisch \"Kolonialismus\"?", new[] { "colonialism", "colony only", "colonel" }, "colonialism", "\"colonialism\" bedeutet \"Kolonialismus\"."),
        ("Was bedeutet \"immigrant\" auf Deutsch?", new[] { "Einwanderer/Immigrant", "Auswanderer", "Reisender" }, "Einwanderer/Immigrant", "\"immigrant\" bedeutet \"Einwanderer/Immigrant\"."),
        ("Wie sagt man auf Englisch \"Vielfalt\"?", new[] { "diversity", "diversion", "division" }, "diversity", "\"diversity\" bedeutet \"Vielfalt\"."),
        ("Was bedeutet \"custom\" auf Deutsch?", new[] { "Brauch/Sitte", "Zoll (Behörde)", "Kunde" }, "Brauch/Sitte", "\"custom\" bedeutet in diesem Zusammenhang \"Brauch/Sitte\" (als \"customs\" auch \"Zoll\")."),
        ("Wie sagt man auf Englisch \"Fest/Festival\"?", new[] { "festival", "fasting", "festivity only" }, "festival", "\"festival\" bedeutet \"Fest/Festival\"."),
        ("Was bedeutet \"folklore\" auf Deutsch?", new[] { "Volkskunde/Folklore", "Wald", "Volksmusik ausschließlich" }, "Volkskunde/Folklore", "\"folklore\" bedeutet \"Volkskunde/Folklore\"."),
        ("Wie sagt man auf Englisch \"Vorfahre\"?", new[] { "ancestor", "descendant", "assistant" }, "ancestor", "\"ancestor\" bedeutet \"Vorfahre\" (das Gegenteil ist \"descendant\", Nachkomme)."),
        ("Was bedeutet \"civil rights movement\" auf Deutsch?", new[] { "Bürgerrechtsbewegung", "Zivildienst", "Bürgerkrieg" }, "Bürgerrechtsbewegung", "\"civil rights movement\" bedeutet \"Bürgerrechtsbewegung\"."),
        ("Wie sagt man auf Englisch \"Weltreich/Imperium\"?", new[] { "empire", "emperor only", "empty" }, "empire", "\"empire\" bedeutet \"Weltreich/Imperium\"."),
        ("Was bedeutet \"independence\" auf Deutsch?", new[] { "Unabhängigkeit", "Abhängigkeit", "Unsicherheit" }, "Unabhängigkeit", "\"independence\" bedeutet \"Unabhängigkeit\"."),
        ("Wie sagt man auf Englisch \"Generation\"?", new[] { "generation", "generator only", "genre" }, "generation", "\"generation\" bedeutet \"Generation\"."),
        ("Was bedeutet \"cultural identity\" auf Deutsch?", new[] { "kulturelle Identität", "kulturelle Vielfalt", "kulturelles Erbe" }, "kulturelle Identität", "\"cultural identity\" bedeutet \"kulturelle Identität\"."),
        ("Wie sagt man auf Englisch \"Museum\"?", new[] { "museum", "monument only", "memorial only" }, "museum", "\"museum\" bedeutet \"Museum\"."),
        ("Was bedeutet \"historical figure\" auf Deutsch?", new[] { "historische Persönlichkeit", "historisches Gebäude", "historische Zahl" }, "historische Persönlichkeit", "\"historical figure\" bedeutet \"historische Persönlichkeit\"."),
        ("Wie sagt man auf Englisch \"Revolution\"?", new[] { "revolution", "resolution", "revelation" }, "revolution", "\"revolution\" bedeutet \"Revolution\"."),
        ("Was bedeutet \"to commemorate\" auf Deutsch?", new[] { "gedenken/erinnern an", "feiern ohne Anlass", "vergessen" }, "gedenken/erinnern an", "\"to commemorate\" bedeutet \"gedenken/erinnern an\".")
    };

    private static QuizQuestion KulturUndHistorischerHintergrund(Random r)
    {
        var f = HistorischerHintergrundListe[r.Next(HistorischerHintergrundListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Kultur und historischer Hintergrund (Klasse-9-Niveau)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wortschatz zu Kultur und Geschichte: heritage, tradition, colonialism, civil rights movement, independence."
        };
    }

    // ----- Klasse 7 -----

    private static readonly (string Satz, string Loesung, string Regel)[] PastProgressiveListe =
    {
        ("While I ___ (do) my homework, the phone rang.", "was doing", "Die längere Hintergrundhandlung steht im Past Progressive (was/were + -ing), die kurze Unterbrechung im Simple Past."),
        ("She ___ (watch) TV when the lights went out.", "was watching", "Laufende Handlung in der Vergangenheit = Past Progressive; das plötzliche Ereignis steht im Simple Past."),
        ("They ___ (play) football when it started to rain.", "were playing", "Die laufende Handlung (Past Progressive) wird vom Regen (Simple Past) unterbrochen."),
        ("Yesterday at 8 o'clock, I ___ (have) breakfast.", "was having", "Eine Handlung, die zu einem bestimmten Zeitpunkt in der Vergangenheit gerade lief, steht im Past Progressive."),
        ("When the teacher came in, the students ___ (talk).", "were talking", "Die Schüler waren mitten im Sprechen (Past Progressive), als der Lehrer hereinkam (Simple Past)."),
        ("He ___ (ride) his bike when he saw the accident.", "was riding", "Laufende Handlung = Past Progressive; das plötzliche Ereignis (\"saw\") = Simple Past."),
        ("Last night, I ___ (finish) my homework and went to bed.", "finished", "Zwei nacheinander abgeschlossene Handlungen stehen beide im Simple Past."),
        ("We ___ (visit) our grandma last weekend.", "visited", "Abgeschlossene Handlung mit Zeitangabe (\"last weekend\") = Simple Past."),
        ("While mum ___ (cook), dad was setting the table.", "was cooking", "Zwei gleichzeitig laufende Handlungen stehen beide im Past Progressive (while = während)."),
        ("I ___ (see) a great film yesterday.", "saw", "Abgeschlossene Handlung mit \"yesterday\" = Simple Past; \"see\" ist unregelmäßig: see-saw-seen."),
        ("The children ___ (sleep) when their parents came home.", "were sleeping", "Die Kinder schliefen gerade (Past Progressive), als die Eltern kamen (Simple Past)."),
        ("She ___ (break) her leg while she was skiing.", "broke", "Das kurze Ereignis steht im Simple Past (\"broke\"), die laufende Handlung im Past Progressive (\"was skiing\")."),
        ("At 6 pm yesterday, we ___ (drive) home.", "were driving", "Zu einem genauen Zeitpunkt in der Vergangenheit lief die Handlung gerade - Past Progressive."),
        ("He ___ (go) to London two years ago.", "went", "Abgeschlossene Handlung mit \"two years ago\" = Simple Past; \"go\" ist unregelmäßig: go-went-gone."),
        ("While they ___ (wait) for the bus, it began to snow.", "were waiting", "Die Wartesituation läuft (Past Progressive), der Schneefall beginnt plötzlich (Simple Past)."),
        ("I ___ (read) a book when someone knocked on the door.", "was reading", "Laufende Handlung (Past Progressive) wird durch das Klopfen (Simple Past) unterbrochen."),
        ("They ___ (buy) a new car last month.", "bought", "Abgeschlossene Handlung mit \"last month\" = Simple Past; \"buy\" ist unregelmäßig: buy-bought-bought."),
        ("What ___ you ___ (do) at 9 o'clock last night?", "were / doing", "Frage nach einer laufenden Handlung zu einem Zeitpunkt in der Vergangenheit - Past Progressive (were you doing)."),
        ("The sun ___ (shine) when we left the house.", "was shining", "Hintergrundbeschreibung (Past Progressive), während das Verlassen des Hauses im Simple Past steht."),
        ("She ___ (write) an email when her computer crashed.", "was writing", "Laufende Handlung = Past Progressive; der Absturz = Simple Past.")
    };

    private static QuizQuestion SimplePastVsPastProgressive(Random r)
    {
        var p = PastProgressiveListe[r.Next(PastProgressiveListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Simple Past vs. Past Progressive", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Form ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "Past Progressive (was/were + -ing) für laufende Hintergrundhandlungen, Simple Past für kurze/abgeschlossene Ereignisse. \"While\" deutet oft auf Past Progressive hin."
        };
    }

    private static readonly (string Satz, string Loesung, string Regel)[] FutureListe =
    {
        ("Look at those clouds! It ___ (rain).", "is going to rain", "Bei einer Vorhersage mit sichtbaren Anzeichen (dunkle Wolken) nutzt man going-to-Future."),
        ("We ___ (visit) our cousins next weekend. It's all planned.", "are going to visit", "Feste Pläne und Absichten stehen im going-to-Future."),
        ("I think it ___ (be) sunny tomorrow.", "will be", "Vermutungen mit \"I think\" stehen im will-Future."),
        ("The phone is ringing. - I ___ (answer) it!", "will answer", "Spontane Entscheidungen im Moment des Sprechens stehen im will-Future."),
        ("She ___ (study) medicine after school. That's her plan.", "is going to study", "Ein fester Zukunftsplan steht im going-to-Future."),
        ("Maybe we ___ (win) the match.", "will win", "Unsichere Vermutungen (\"maybe\", \"perhaps\") stehen im will-Future."),
        ("Careful! You ___ (drop) the glasses!", "are going to drop", "Wenn etwas gleich sichtbar passieren wird, nutzt man going-to-Future."),
        ("I'm sure he ___ (pass) the test.", "will pass", "Vermutungen mit \"I'm sure\" stehen im will-Future."),
        ("They ___ (move) to Hamburg next month. They have already found a flat.", "are going to move", "Ein bereits beschlossener Plan steht im going-to-Future."),
        ("It's cold in here. - I ___ (close) the window.", "will close", "Spontanes Angebot im Moment des Sprechens = will-Future."),
        ("What ___ you ___ (do) in the summer holidays? Any plans?", "are / going to do", "Frage nach Plänen = going-to-Future (are you going to do)."),
        ("He ___ (probably/come) to the party.", "will probably come", "\"probably\" zeigt eine Vermutung - will-Future (will probably + Grundform)."),
        ("Watch out! The dog ___ (bite) you!", "is going to bite", "Sichtbare Anzeichen für ein gleich eintretendes Ereignis = going-to-Future."),
        ("Don't worry, I ___ (help) you with your bags.", "will help", "Spontanes Hilfsangebot = will-Future."),
        ("My parents ___ (buy) a new car. They have already chosen one.", "are going to buy", "Beschlossener Plan (schon ausgesucht) = going-to-Future."),
        ("Perhaps she ___ (call) you later.", "will call", "\"perhaps\" zeigt eine unsichere Vermutung - will-Future."),
        ("Next year, I ___ (learn) to play the guitar. I've already got one.", "am going to learn", "Fester Vorsatz/Plan = going-to-Future (I am going to + Grundform)."),
        ("I promise I ___ (not/tell) anyone.", "won't tell", "Versprechen stehen im will-Future; verneint: won't + Grundform."),
        ("The bus is full. We ___ (not/get) a seat.", "aren't going to get", "Vorhersage aufgrund sichtbarer Anzeichen (voller Bus) = going-to-Future, verneint."),
        ("She's very good at maths. She ___ (probably/become) an engineer.", "will probably become", "Vermutung über die Zukunft mit \"probably\" = will-Future.")
    };

    private static QuizQuestion GoingToUndWillFuture(Random r)
    {
        var p = FutureListe[r.Next(FutureListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "going-to-Future vs. will-Future", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Zukunftsform ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "going-to-Future für Pläne und sichtbare Anzeichen; will-Future für Vermutungen (I think/maybe/probably), spontane Entscheidungen und Versprechen."
        };
    }

    private static readonly (string Satz, string Loesung, string Regel)[] ComparisonListe =
    {
        ("A car is ___ (fast) than a bike.", "faster", "Kurze Adjektive bilden den Komparativ mit -er: fast - faster."),
        ("Mount Everest is ___ (high) mountain in the world.", "the highest", "Der Superlativ kurzer Adjektive: the + Adjektiv + -est (the highest)."),
        ("This book is ___ (interesting) than that one.", "more interesting", "Lange Adjektive (3+ Silben) bilden den Komparativ mit \"more\": more interesting."),
        ("She is ___ (good) player in our team.", "the best", "\"good\" steigert unregelmäßig: good - better - the best."),
        ("Today is ___ (bad) day of the week.", "the worst", "\"bad\" steigert unregelmäßig: bad - worse - the worst."),
        ("My brother is ___ (old) than me.", "older", "Kurze Adjektive: Komparativ mit -er (older than)."),
        ("This is ___ (beautiful) beach I have ever seen.", "the most beautiful", "Lange Adjektive bilden den Superlativ mit \"the most\": the most beautiful."),
        ("Maths is ___ (difficult) than art for me.", "more difficult", "Lange Adjektive: Komparativ mit \"more\" (more difficult than)."),
        ("A blue whale is ___ (big) animal on Earth.", "the biggest", "Bei kurzen Adjektiven mit Endkonsonant wird dieser verdoppelt: big - bigger - the biggest."),
        ("This exercise is ___ (easy) than the last one.", "easier", "Adjektive auf -y: y wird zu i + -er (easy - easier)."),
        ("Winter days are ___ (short) than summer days.", "shorter", "Kurze Adjektive: Komparativ mit -er (shorter than)."),
        ("He is ___ (happy) boy in the class.", "the happiest", "Adjektive auf -y: y wird zu i + -est (the happiest)."),
        ("My bag is ___ (heavy) than yours.", "heavier", "Adjektive auf -y: y wird zu i + -er (heavier than)."),
        ("That was ___ (exciting) film of the year.", "the most exciting", "Lange Adjektive: Superlativ mit \"the most\" (the most exciting)."),
        ("Gold is ___ (expensive) than silver.", "more expensive", "Lange Adjektive: Komparativ mit \"more\" (more expensive than)."),
        ("February is ___ (short) month of the year.", "the shortest", "Superlativ kurzer Adjektive: the + -est (the shortest)."),
        ("Your idea is ___ (good) than mine.", "better", "\"good\" steigert unregelmäßig: good - better - the best."),
        ("This street is ___ (noisy) than ours.", "noisier", "Adjektive auf -y: y wird zu i + -er (noisier than)."),
        ("The weather today is ___ (bad) than yesterday.", "worse", "\"bad\" steigert unregelmäßig: bad - worse - the worst."),
        ("It was ___ (hot) day of the summer.", "the hottest", "Kurze Adjektive mit Endkonsonant: Verdopplung + -est (the hottest).")
    };

    private static QuizQuestion ComparativeSuperlative(Random r)
    {
        var p = ComparisonListe[r.Next(ComparisonListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Steigerung von Adjektiven (Comparison)", Type = QuestionType.OpenText,
            Prompt = $"Setze die richtige Steigerungsform ein: \"{p.Satz}\"",
            CorrectAnswers = new[] { p.Loesung }, Explanation = p.Regel,
            HelpHint = "Kurze Adjektive: -er/-est (mit \"the\" beim Superlativ). Lange Adjektive: more/most. Unregelmäßig: good-better-best, bad-worse-worst."
        };
    }

    private static readonly (string Satz, string[] Optionen, string Antwort, string Erklaerung)[] QuantifierListe =
    {
        ("Is there ___ milk in the fridge?", new[] { "any", "some", "many" }, "any", "In Fragen und Verneinungen nutzt man meist \"any\" (Is there any milk?)."),
        ("I'd like ___ water, please.", new[] { "some", "any", "many" }, "some", "In Aussagesätzen und höflichen Bitten nutzt man \"some\"."),
        ("There aren't ___ apples left.", new[] { "any", "some", "much" }, "any", "In Verneinungen (aren't) steht \"any\"."),
        ("How ___ money do you have?", new[] { "much", "many", "some" }, "much", "\"money\" ist nicht zählbar - deshalb \"how much\"."),
        ("How ___ friends do you have?", new[] { "many", "much", "any" }, "many", "\"friends\" ist zählbar (Plural) - deshalb \"how many\"."),
        ("We don't have ___ time.", new[] { "much", "many", "some" }, "much", "\"time\" ist nicht zählbar - \"much time\"."),
        ("There are ___ books on the shelf.", new[] { "many", "much", "any" }, "many", "\"books\" ist zählbar - \"many books\"."),
        ("Would you like ___ tea?", new[] { "some", "any", "many" }, "some", "Bei Angeboten (Would you like ...?) nutzt man \"some\", obwohl es eine Frage ist."),
        ("She doesn't eat ___ sugar.", new[] { "much", "many", "some" }, "much", "\"sugar\" ist nicht zählbar - verneint: not much sugar."),
        ("Have you got ___ brothers or sisters?", new[] { "any", "some", "much" }, "any", "In normalen Fragen steht \"any\" (Have you got any ...?)."),
        ("There is too ___ noise in here.", new[] { "much", "many", "any" }, "much", "\"noise\" ist nicht zählbar - \"too much noise\"."),
        ("There are too ___ cars in the city.", new[] { "many", "much", "some" }, "many", "\"cars\" ist zählbar - \"too many cars\"."),
        ("I bought ___ new shoes yesterday.", new[] { "some", "any", "much" }, "some", "In positiven Aussagesätzen steht \"some\"."),
        ("He didn't buy ___ bread.", new[] { "any", "some", "many" }, "any", "In Verneinungen (didn't) steht \"any\"."),
        ("How ___ homework do we have?", new[] { "much", "many", "any" }, "much", "\"homework\" ist nicht zählbar - \"how much homework\"."),
        ("There isn't ___ juice left in the bottle.", new[] { "much", "many", "some" }, "much", "\"juice\" ist nicht zählbar - verneint: not much juice."),
        ("___ people enjoy travelling.", new[] { "Many", "Much", "Any" }, "Many", "\"people\" ist zählbar (Plural) - \"many people\"."),
        ("Can I have ___ more rice, please?", new[] { "some", "any", "many" }, "some", "Bei höflichen Bitten (Can I have ...?) nutzt man \"some\"."),
        ("We saw ___ interesting animals at the zoo.", new[] { "some", "any", "much" }, "some", "Positiver Aussagesatz mit zählbarem Nomen - \"some animals\"."),
        ("Do you have ___ questions?", new[] { "any", "some", "much" }, "any", "In normalen Fragen steht \"any\" (Do you have any questions?).")
    };

    private static QuizQuestion SomeAnyMuchMany(Random r)
    {
        var q = QuantifierListe[r.Next(QuantifierListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "some/any und much/many", Type = QuestionType.MultipleChoice,
            Prompt = $"Welches Wort passt in die Lücke? \"{q.Satz}\"",
            Options = q.Optionen, CorrectAnswers = new[] { q.Antwort }, Explanation = q.Erklaerung,
            HelpHint = "some: Aussagesätze, Angebote, Bitten. any: Fragen und Verneinungen. much: nicht zählbar (money, time). many: zählbar (friends, cars)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FreizeitReisenListe =
    {
        ("What do you need to travel to another country?", new[] { "A passport", "A blackboard", "A dishwasher" }, "A passport", "\"passport\" = Reisepass - das wichtigste Dokument für Auslandsreisen."),
        ("Where do you wait for your plane at the airport?", new[] { "At the gate", "In the kitchen", "At the bus stop" }, "At the gate", "Am \"gate\" (Flugsteig) wartet man auf das Boarding."),
        ("What is a \"return ticket\"?", new[] { "A ticket for the journey there and back", "A ticket you give back", "A free ticket" }, "A ticket for the journey there and back", "\"return ticket\" = Hin- und Rückfahrkarte."),
        ("What does \"to go sightseeing\" mean?", new[] { "To visit famous places in a city", "To sleep all day", "To do your homework" }, "To visit famous places in a city", "\"sightseeing\" = Sehenswürdigkeiten besichtigen."),
        ("Where can you stay overnight on holiday?", new[] { "At a hotel or a youth hostel", "At a supermarket", "At a petrol station" }, "At a hotel or a youth hostel", "\"hotel\" und \"youth hostel\" (Jugendherberge) sind typische Unterkünfte."),
        ("What do you call the bags you take on a journey?", new[] { "Luggage", "Lettuce", "Furniture" }, "Luggage", "\"luggage\" = Gepäck (nicht zählbar: much luggage)."),
        ("What does \"to book a room\" mean?", new[] { "To reserve a room in advance", "To read a book in your room", "To paint a room" }, "To reserve a room in advance", "\"to book\" = reservieren/buchen."),
        ("What is a \"timetable\"?", new[] { "A plan that shows when buses or trains leave", "A table made of wood", "A kind of watch" }, "A plan that shows when buses or trains leave", "\"timetable\" = Fahrplan (oder Stundenplan in der Schule)."),
        ("What hobby needs a ball and a racket?", new[] { "Tennis", "Swimming", "Chess" }, "Tennis", "Tennis spielt man mit \"racket\" (Schläger) und Ball."),
        ("What does \"to hang out with friends\" mean?", new[] { "To spend free time with friends", "To hang clothes on a line", "To argue with friends" }, "To spend free time with friends", "\"to hang out\" = Zeit mit Freunden verbringen (Umgangssprache)."),
        ("Where do you buy a train ticket?", new[] { "At the ticket office or a machine", "At the baker's", "At the hairdresser's" }, "At the ticket office or a machine", "\"ticket office\" = Fahrkartenschalter."),
        ("What is a \"journey\"?", new[] { "Travelling from one place to another", "A kind of food", "A newspaper" }, "Travelling from one place to another", "\"journey\" = die Reise/Fahrt."),
        ("What does \"abroad\" mean?", new[] { "In a foreign country", "On a wide street", "In the garden" }, "In a foreign country", "\"abroad\" = im Ausland (to go abroad = ins Ausland gehen)."),
        ("What do you call a person who travels on a plane?", new[] { "A passenger", "A pedestrian", "A goalkeeper" }, "A passenger", "\"passenger\" = Fahrgast/Passagier."),
        ("What is \"a sleeping bag\" used for?", new[] { "Sleeping outdoors or when camping", "Carrying books", "Cooking soup" }, "Sleeping outdoors or when camping", "\"sleeping bag\" = Schlafsack, typisch fürs Camping."),
        ("What does \"to score a goal\" mean?", new[] { "To shoot the ball into the goal", "To lose the game", "To clean the pitch" }, "To shoot the ball into the goal", "\"to score a goal\" = ein Tor schießen."),
        ("What do you call the place where you can swim indoors?", new[] { "A swimming pool", "A car park", "A library" }, "A swimming pool", "\"(indoor) swimming pool\" = Schwimmbad/Hallenbad."),
        ("What is a \"day trip\"?", new[] { "A short journey there and back on the same day", "A trip that takes a month", "A kind of breakfast" }, "A short journey there and back on the same day", "\"day trip\" = Tagesausflug."),
        ("What does \"to miss the bus\" mean?", new[] { "To arrive too late for the bus", "To like the bus very much", "To repair the bus" }, "To arrive too late for the bus", "\"to miss\" = verpassen (I missed the bus = ich habe den Bus verpasst)."),
        ("What do you call free time activities you do regularly?", new[] { "Hobbies", "Homework", "Chores" }, "Hobbies", "\"hobbies\" = Hobbys, regelmäßige Freizeitbeschäftigungen.")
    };

    private static QuizQuestion FreizeitUndReisen(Random r)
    {
        var f = FreizeitReisenListe[r.Next(FreizeitReisenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Freizeit und Reisen (Wortschatz)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Reise-Wortschatz: passport, luggage, return ticket, youth hostel, timetable, journey, abroad, day trip."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] LandeskundeListe =
    {
        ("What is the capital of the United Kingdom?", new[] { "London", "Manchester", "Dublin" }, "London", "London ist die Hauptstadt des Vereinigten Königreichs."),
        ("Which countries make up Great Britain?", new[] { "England, Scotland and Wales", "England, France and Spain", "England, Ireland and Iceland" }, "England, Scotland and Wales", "Großbritannien besteht aus England, Schottland und Wales; das UK umfasst zusätzlich Nordirland."),
        ("What is the river that flows through London?", new[] { "The Thames", "The Rhine", "The Seine" }, "The Thames", "Die Themse (the Thames) fließt durch London."),
        ("What is \"Big Ben\"?", new[] { "The famous bell in the clock tower of the Houses of Parliament", "A famous football player", "A mountain in Scotland" }, "The famous bell in the clock tower of the Houses of Parliament", "\"Big Ben\" ist eigentlich der Name der großen Glocke im Elizabeth Tower."),
        ("What is the London Underground also called?", new[] { "The Tube", "The Pipe", "The Tunnel Train" }, "The Tube", "Die Londoner U-Bahn heißt umgangssprachlich \"the Tube\"."),
        ("What is the flag of the United Kingdom called?", new[] { "The Union Jack", "The Stars and Stripes", "The Maple Leaf" }, "The Union Jack", "Die britische Flagge heißt \"Union Jack\"."),
        ("Who lives in Buckingham Palace?", new[] { "The British King or Queen", "The Prime Minister", "The Mayor of London" }, "The British King or Queen", "Der Buckingham Palace ist die offizielle Londoner Residenz der britischen Monarchie."),
        ("What do the British traditionally drink in the afternoon?", new[] { "Tea", "Hot lemonade", "Iced coffee" }, "Tea", "Der \"afternoon tea\" ist eine bekannte britische Tradition."),
        ("What is a \"double-decker\"?", new[] { "A bus with two floors", "A sandwich with two eggs", "A house with two doors" }, "A bus with two floors", "Die roten Doppeldeckerbusse sind ein Wahrzeichen Londons."),
        ("On which side of the road do people drive in the UK?", new[] { "On the left", "On the right", "In the middle" }, "On the left", "In Großbritannien herrscht Linksverkehr."),
        ("What is the currency of the United Kingdom?", new[] { "Pound sterling", "Euro", "Dollar" }, "Pound sterling", "Im UK zahlt man mit dem britischen Pfund (pound sterling, £)."),
        ("What is \"fish and chips\"?", new[] { "A traditional British dish with fried fish and chips", "A card game", "A famous TV show" }, "A traditional British dish with fried fish and chips", "\"Fish and chips\" ist das wohl bekannteste britische Gericht."),
        ("What is the capital of Scotland?", new[] { "Edinburgh", "Glasgow", "Cardiff" }, "Edinburgh", "Edinburgh ist die Hauptstadt Schottlands, Cardiff die von Wales."),
        ("What is Stonehenge?", new[] { "A prehistoric stone circle in England", "A castle in London", "A Scottish lake" }, "A prehistoric stone circle in England", "Stonehenge ist ein weltberühmter prähistorischer Steinkreis in Südengland."),
        ("What is Loch Ness famous for?", new[] { "The legend of a monster living in the lake", "Its beaches", "A famous football stadium" }, "The legend of a monster living in the lake", "\"Nessie\", das Ungeheuer von Loch Ness, ist eine berühmte schottische Legende."),
        ("What do British pupils usually wear at school?", new[] { "A school uniform", "Sports clothes only", "Whatever they like, there are no rules" }, "A school uniform", "An den meisten britischen Schulen tragen die Schüler Schuluniform."),
        ("What is the \"Tower of London\"?", new[] { "A historic castle where the Crown Jewels are kept", "The tallest skyscraper in Europe", "A football stadium" }, "A historic castle where the Crown Jewels are kept", "Der Tower of London ist eine historische Festung; dort werden die Kronjuwelen aufbewahrt."),
        ("Which sport was invented in England?", new[] { "Football (soccer)", "Basketball", "Ice hockey" }, "Football (soccer)", "Die modernen Fußballregeln entstanden im 19. Jahrhundert in England."),
        ("What is a \"red telephone box\"?", new[] { "A famous British public phone booth", "A post office", "A fire station" }, "A famous British public phone booth", "Die roten Telefonzellen sind ein bekanntes britisches Wahrzeichen."),
        ("What language is spoken in Wales besides English?", new[] { "Welsh", "Dutch", "Gaelic only" }, "Welsh", "In Wales wird neben Englisch auch Walisisch (Welsh) gesprochen.")
    };

    private static QuizQuestion GrossbritannienLandeskunde(Random r)
    {
        var f = LandeskundeListe[r.Next(LandeskundeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Großbritannien (Landeskunde)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Landeskunde UK: London/Thames/Big Ben/Tube, Union Jack, Linksverkehr, pound sterling, Edinburgh, Stonehenge, Schuluniformen."
        };
    }
}
