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
            "Der Übergang von fest zu flüssig heißt Schmelzen (z.B. Eis wird zu Wasser).")
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
            "Kupfer (Metall)", "Metalle wie Kupfer sind gute elektrische Leiter, Holz und Gummi sind Isolatoren.")
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
            "Sie stoßen sich ab", "Gleiche Pole stoßen sich ab, ungleiche Pole (Nord-Süd) ziehen sich an.")
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
            "Beim Fallen wird die Höhenenergie (potenzielle Energie) in Bewegungsenergie (kinetische Energie) umgewandelt.")
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
            "F = m·a beschreibt, wie stark eine Kraft einen Körper abhängig von seiner Masse beschleunigt.")
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
            "Fahrraddynamo/Generator im Kraftwerk", "Fahrraddynamos und Generatoren in Kraftwerken erzeugen Strom durch Bewegung eines Magneten relativ zu einer Spule (Induktion).")
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
