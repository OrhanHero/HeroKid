using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Physik nach Berliner Rahmenlehrplan, Klasse 6 (Grundlagen) und Klasse 9 (vertieft).</summary>
public sealed class PhysikGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Physik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Aggregatzustaende, Stromkreis, Magnetismus },
            [GradeLevel.Klasse9] = new List<TopicFactory> { OhmschesGesetz, Energieerhaltung, NewtonscheGesetze, MagnetfelderInduktion }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AggregatzustaendeListe =
    {
        ("In welchem Aggregatzustand ist Wasser bei -10°C?", new[] { "Fest (Eis)", "Flüssig", "Gasförmig (Dampf)" }, "Fest (Eis)",
            "Wasser gefriert bei 0°C. Bei -10°C liegt es als festes Eis vor."),
        ("In welchem Aggregatzustand ist Wasser bei 100°C (bei normalem Luftdruck)?", new[] { "Fest (Eis)", "Flüssig", "Gasförmig (Wasserdampf)" }, "Gasförmig (Wasserdampf)",
            "Wasser siedet bei 100°C und geht dann in den gasförmigen Zustand (Wasserdampf) über."),
        ("Wie nennt man den Übergang von fest zu flüssig?", new[] { "Schmelzen", "Verdampfen", "Sublimieren" }, "Schmelzen",
            "Der Übergang von fest zu flüssig heißt Schmelzen (z.B. Eis wird zu Wasser)."),
        ("Wie nennt man den Übergang von flüssig zu gasförmig?", new[] { "Verdampfen (Verdunsten)", "Schmelzen", "Erstarren" }, "Verdampfen (Verdunsten)",
            "Der Übergang von flüssig zu gasförmig heißt Verdampfen (bei Erreichen des Siedepunkts) bzw. Verdunsten (auch darunter, langsamer)."),
        ("Wie nennt man den direkten Übergang von fest zu gasförmig (ohne flüssige Phase)?", new[] { "Sublimation", "Kondensation", "Schmelzen" }, "Sublimation",
            "Bei der Sublimation geht ein Stoff direkt vom festen in den gasförmigen Zustand über, z.B. Trockeneis (gefrorenes CO₂).")
    };

    private static QuizQuestion Aggregatzustaende(Random r)
    {
        var a = AggregatzustaendeListe[r.Next(AggregatzustaendeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Aggregatzustände", Type = QuestionType.MultipleChoice,
            Prompt = a.Frage, Options = a.Optionen, CorrectAnswers = new[] { a.Antwort }, Explanation = a.Erklaerung,
            HelpHint = "Wasser: unter 0°C fest (Eis), 0-100°C flüssig, über 100°C gasförmig (Wasserdampf, bei normalem Luftdruck)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] StromkreisListe =
    {
        ("Was braucht man mindestens für einen einfachen Stromkreis?", new[] { "Batterie, Kabel und Lampe (geschlossener Kreis)", "Nur eine Batterie", "Nur ein Kabel" },
            "Batterie, Kabel und Lampe (geschlossener Kreis)",
            "Ein Stromkreis muss geschlossen sein: Von der Batterie über ein Kabel zur Lampe und zurück zur Batterie."),
        ("Was passiert, wenn der Stromkreis unterbrochen wird (z.B. Schalter offen)?", new[] { "Die Lampe leuchtet nicht mehr", "Die Lampe leuchtet heller", "Nichts ändert sich" },
            "Die Lampe leuchtet nicht mehr",
            "Ist der Stromkreis unterbrochen, kann kein Strom fließen, die Lampe bleibt dunkel."),
        ("Welches Material leitet elektrischen Strom gut?", new[] { "Kupfer (Metall)", "Holz", "Gummi" },
            "Kupfer (Metall)", "Metalle wie Kupfer sind gute elektrische Leiter, Holz und Gummi sind Isolatoren."),
        ("Was zeigt ein Amperemeter im Stromkreis an?", new[] { "Die Stromstärke", "Die Spannung", "Den Widerstand" },
            "Die Stromstärke", "Ein Amperemeter misst die Stromstärke in Ampere und wird in Reihe in den Stromkreis geschaltet."),
        ("Was passiert, wenn man zwei Lampen in Reihe (hintereinander) schaltet und eine Lampe kaputtgeht?", new[] { "Beide Lampen gehen aus", "Nur die kaputte Lampe bleibt dunkel, die andere leuchtet weiter", "Beide Lampen leuchten heller" },
            "Beide Lampen gehen aus", "Bei einer Reihenschaltung ist der Stromkreis unterbrochen, sobald eine Lampe ausfällt - beide gehen aus.")
    };

    private static QuizQuestion Stromkreis(Random r)
    {
        var f = StromkreisListe[r.Next(StromkreisListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Einfacher Stromkreis", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Stromkreis muss geschlossen sein (Batterie → Kabel → Lampe → zurück zur Batterie); Metalle wie Kupfer leiten gut, Holz/Gummi isolieren."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MagnetismusListe =
    {
        ("Welche Metalle werden von einem Magneten angezogen?", new[] { "Eisen, Nickel, Kobalt", "Gold und Silber", "Aluminium und Kupfer" },
            "Eisen, Nickel, Kobalt", "Nur ferromagnetische Metalle wie Eisen, Nickel und Kobalt werden von Magneten angezogen."),
        ("Was passiert, wenn sich zwei gleiche Pole (Nord-Nord) eines Magneten nähern?", new[] { "Sie stoßen sich ab", "Sie ziehen sich an", "Nichts passiert" },
            "Sie stoßen sich ab", "Gleiche Pole stoßen sich ab, ungleiche Pole (Nord-Süd) ziehen sich an."),
        ("Wie viele Pole hat jeder Magnet mindestens?", new[] { "Zwei (Nord- und Südpol)", "Nur einen", "Drei" },
            "Zwei (Nord- und Südpol)", "Jeder Magnet hat einen Nord- und einen Südpol - schneidet man ihn durch, entstehen zwei neue Magnete mit je zwei Polen."),
        ("Wonach richtet sich eine frei bewegliche Kompassnadel aus?", new[] { "Nach dem Erdmagnetfeld - sie zeigt nach Norden", "Nach der Sonne", "Nach der Uhrzeit" },
            "Nach dem Erdmagnetfeld - sie zeigt nach Norden", "Die Erde wirkt selbst wie ein großer Magnet - eine frei bewegliche Kompassnadel richtet sich daran aus."),
        ("Wodurch kann man aus einem Eisennagel vorübergehend einen Magneten machen?", new[] { "Ihn an einen starken Magneten halten/damit bestreichen", "Ihn erhitzen", "Ihn ins Wasser legen" },
            "Ihn an einen starken Magneten halten/damit bestreichen", "Eisen kann durch Kontakt mit einem starken Magneten selbst vorübergehend magnetisch werden.")
    };

    private static QuizQuestion Magnetismus(Random r)
    {
        var f = MagnetismusListe[r.Next(MagnetismusListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Magnetismus", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nur Eisen, Nickel und Kobalt werden von Magneten angezogen; gleiche Pole stoßen sich ab, ungleiche ziehen sich an."
        };
    }

    private static QuizQuestion OhmschesGesetz(Random r)
    {
        int widerstand = new[] { 5, 10, 20, 25, 50 }[r.Next(5)];
        int strom = new[] { 1, 2, 3, 4 }[r.Next(4)];
        int spannung = widerstand * strom;

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Ohmsches Gesetz", Type = QuestionType.OpenText,
            Prompt = $"Ein Widerstand von {widerstand} Ohm wird von einem Strom von {strom} Ampere durchflossen. " +
                     "Wie groß ist die Spannung in Volt? (U = R · I)",
            CorrectAnswers = new[] { spannung.ToString() },
            Explanation = $"Ohmsches Gesetz: U = R · I = {widerstand} Ω · {strom} A = {spannung} V.",
            HelpHint = "Ohmsches Gesetz: Spannung U = Widerstand R · Stromstärke I."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EnergieListe =
    {
        ("Was besagt der Energieerhaltungssatz?", new[] { "Energie kann nicht erzeugt oder vernichtet werden, nur umgewandelt", "Energie nimmt mit der Zeit immer ab", "Nur elektrische Energie bleibt erhalten" },
            "Energie kann nicht erzeugt oder vernichtet werden, nur umgewandelt",
            "In einem geschlossenen System bleibt die Gesamtenergie konstant, sie wandelt sich nur zwischen Formen (z.B. Höhen- in Bewegungsenergie) um."),
        ("Ein Ball fällt aus großer Höhe herunter. In welche Energieform wandelt sich die Höhenenergie dabei um?", new[] { "Bewegungsenergie (kinetische Energie)", "Wärmeenergie allein", "Elektrische Energie" },
            "Bewegungsenergie (kinetische Energie)",
            "Beim Fallen wird die Höhenenergie (potenzielle Energie) in Bewegungsenergie (kinetische Energie) umgewandelt."),
        ("Ein Auto bremst und wird langsamer. Wohin \"verschwindet\" die Bewegungsenergie hauptsächlich?", new[] { "Sie wandelt sich in Wärme (Reibung) um", "Sie verschwindet komplett spurlos", "Sie wird zu Licht" },
            "Sie wandelt sich in Wärme (Reibung) um", "Reibung beim Bremsen wandelt Bewegungsenergie hauptsächlich in Wärmeenergie um - die Bremsen werden warm."),
        ("Was ist eine erneuerbare Energiequelle?", new[] { "Sonne, Wind oder Wasser", "Kohle", "Erdöl" },
            "Sonne, Wind oder Wasser", "Erneuerbare Energien wie Sonne, Wind und Wasser stehen (im Gegensatz zu Kohle/Öl) dauerhaft zur Verfügung."),
        ("Was beschreibt der Begriff \"potenzielle Energie\" (Höhenenergie) am einfachsten?", new[] { "Gespeicherte Energie aufgrund der Lage/Höhe eines Körpers", "Energie, die durch Bewegung entsteht", "Energie aus elektrischem Strom" },
            "Gespeicherte Energie aufgrund der Lage/Höhe eines Körpers", "Ein angehobener Ball hat potenzielle Energie - je höher er ist, desto mehr Energie kann beim Fallen frei werden.")
    };

    private static QuizQuestion Energieerhaltung(Random r)
    {
        var f = EnergieListe[r.Next(EnergieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Energieerhaltung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Energie wird nie vernichtet, nur umgewandelt - beim Fallen wird Höhenenergie zu Bewegungsenergie."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NewtonListe =
    {
        ("Was besagt Newtons erstes Gesetz (Trägheitsgesetz)?", new[] { "Ein Körper bleibt in Ruhe oder gleichförmiger Bewegung, solange keine Kraft wirkt", "Jede Kraft erzeugt sofort Wärme", "Kräfte wirken nur bei Berührung" },
            "Ein Körper bleibt in Ruhe oder gleichförmiger Bewegung, solange keine Kraft wirkt",
            "Das Trägheitsgesetz: Ohne einwirkende Kraft ändert ein Körper seinen Bewegungszustand nicht."),
        ("Newton'sches Gesetz \"Kraft = Masse · Beschleunigung\" (F = m·a) - wie nennt man dieses Gesetz?", new[] { "Grundgesetz der Mechanik (2. Newtonsches Gesetz)", "Trägheitsgesetz", "Wechselwirkungsgesetz" },
            "Grundgesetz der Mechanik (2. Newtonsches Gesetz)",
            "F = m·a beschreibt, wie stark eine Kraft einen Körper abhängig von seiner Masse beschleunigt."),
        ("Was besagt Newtons drittes Gesetz (\"Actio = Reactio\")?", new[] { "Zu jeder Kraft gibt es eine gleich große Gegenkraft", "Kräfte verschwinden nach kurzer Zeit von selbst", "Nur schwere Objekte üben Kräfte aus" },
            "Zu jeder Kraft gibt es eine gleich große Gegenkraft", "Drückt man gegen eine Wand, drückt die Wand mit gleicher Kraft zurück (Wechselwirkungsgesetz)."),
        ("Ein Einkaufswagen wird doppelt so schwer beladen. Bei gleicher Kraft: was passiert laut F = m·a mit der Beschleunigung?", new[] { "Sie wird halb so groß", "Sie wird doppelt so groß", "Sie bleibt gleich" },
            "Sie wird halb so groß", "Bei gleicher Kraft F verringert eine größere Masse m die Beschleunigung a, da F = m·a."),
        ("Warum spürt man beim plötzlichen Anfahren eines Busses einen Ruck nach hinten?", new[] { "Der Körper bleibt durch Trägheit zunächst in Ruhe, während sich der Bus schon bewegt", "Der Bus schiebt einen absichtlich nach hinten", "Es gibt dafür keinen physikalischen Grund" },
            "Der Körper bleibt durch Trägheit zunächst in Ruhe, während sich der Bus schon bewegt", "Das ist das Trägheitsgesetz in Aktion: Der Körper \"will\" seinen Bewegungszustand (Ruhe) zunächst beibehalten.")
    };

    private static QuizQuestion NewtonscheGesetze(Random r)
    {
        var f = NewtonListe[r.Next(NewtonListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Newtonsche Gesetze", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "1. Newtonsches Gesetz (Trägheit): ohne Kraft ändert sich die Bewegung nicht. 2. Gesetz: F = m · a."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MagnetInduktionListe =
    {
        ("Was entsteht, wenn Strom durch eine Spule (Draht in Windungen) fließt?", new[] { "Ein Magnetfeld (Elektromagnet)", "Nur Wärme, kein Magnetfeld", "Ein elektrisches Feld, aber kein Magnetfeld" },
            "Ein Magnetfeld (Elektromagnet)", "Fließt Strom durch eine Spule, entsteht um sie herum ein Magnetfeld - das Prinzip des Elektromagneten."),
        ("Wie nennt man es, wenn durch ein sich änderndes Magnetfeld in einer Spule eine Spannung erzeugt wird?", new[] { "Elektromagnetische Induktion", "Ohmsches Gesetz", "Trägheitsgesetz" },
            "Elektromagnetische Induktion", "Bei der elektromagnetischen Induktion erzeugt ein sich änderndes Magnetfeld (z.B. durch Bewegung) eine Spannung in einer Spule - Grundprinzip von Generatoren."),
        ("In welchem Alltagsgerät wird elektromagnetische Induktion zur Stromerzeugung genutzt?", new[] { "Fahrraddynamo/Generator im Kraftwerk", "Batterie", "Glühlampe" },
            "Fahrraddynamo/Generator im Kraftwerk", "Fahrraddynamos und Generatoren in Kraftwerken erzeugen Strom durch Bewegung eines Magneten relativ zu einer Spule (Induktion)."),
        ("Was passiert mit der induzierten Spannung, wenn sich der Magnet schneller durch die Spule bewegt?", new[] { "Die induzierte Spannung wird größer", "Die induzierte Spannung wird kleiner", "Es ändert sich nichts" },
            "Die induzierte Spannung wird größer", "Je schneller sich das Magnetfeld relativ zur Spule ändert, desto größer ist die induzierte Spannung."),
        ("Wofür wird das Prinzip der elektromagnetischen Induktion außer bei Generatoren noch genutzt?", new[] { "Transformatoren", "Batterien", "Glühlampen" },
            "Transformatoren", "Transformatoren nutzen Induktion, um Wechselspannung auf eine höhere oder niedrigere Spannung umzuwandeln.")
    };

    private static QuizQuestion MagnetfelderInduktion(Random r)
    {
        var f = MagnetInduktionListe[r.Next(MagnetInduktionListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Magnetfelder und elektromagnetische Induktion", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Strom durch eine Spule erzeugt ein Magnetfeld (Elektromagnet); ein sich änderndes Magnetfeld erzeugt umgekehrt Spannung (Induktion, z.B. im Generator)."
        };
    }
}
