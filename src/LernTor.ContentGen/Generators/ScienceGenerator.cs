using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Naturwissenschaften-Generator: kombiniert Physik, Chemie und Biologie nach Berliner
/// Rahmenlehrplan NaWi/Chemie/Physik/Biologie, Klasse 6 (Grundlagen) und Klasse 9 (vertieft).
/// </summary>
public sealed class ScienceGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Naturwissenschaften;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                Aggregatzustaende,
                Stromkreis,
                MenschlicheOrgane,
                Fotosynthese,
                Magnetismus
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                Atommodell,
                OhmschesGesetz,
                Zellbiologie,
                ChemischeReaktion,
                Vererbung
            }
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
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Aggregatzustände (Physik)",
            Type = QuestionType.MultipleChoice,
            Prompt = a.Frage,
            Options = a.Optionen,
            CorrectAnswers = new[] { a.Antwort },
            Explanation = a.Erklaerung
        };
    }

    private static QuizQuestion Stromkreis(Random r)
    {
        var fragen = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
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
        var f = fragen[r.Next(fragen.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Einfacher Stromkreis (Physik)",
            Type = QuestionType.MultipleChoice,
            Prompt = f.Frage,
            Options = f.Optionen,
            CorrectAnswers = new[] { f.Antwort },
            Explanation = f.Erklaerung
        };
    }

    private static readonly (string Organ, string Funktion, string[] Falsch)[] OrganeListe =
    {
        ("Herz", "pumpt das Blut durch den Körper", new[] { "verdaut die Nahrung", "filtert die Luft beim Atmen" }),
        ("Lunge", "nimmt Sauerstoff auf und gibt Kohlenstoffdioxid ab", new[] { "pumpt das Blut", "produziert Magensäure" }),
        ("Magen", "verdaut die Nahrung mit Magensäure", new[] { "pumpt das Blut", "denkt und steuert den Körper" }),
        ("Gehirn", "steuert und denkt, verarbeitet Sinnesreize", new[] { "verdaut die Nahrung", "pumpt das Blut" })
    };

    private static QuizQuestion MenschlicheOrgane(Random r)
    {
        var o = OrganeListe[r.Next(OrganeListe.Length)];
        var optionen = new[] { o.Funktion }.Concat(o.Falsch).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Der menschliche Körper (Biologie)",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Was ist die Aufgabe des Organs \"{o.Organ}\"?",
            Options = optionen,
            CorrectAnswers = new[] { o.Funktion },
            Explanation = $"Das {o.Organ} {o.Funktion}."
        };
    }

    private static QuizQuestion Fotosynthese(Random r)
    {
        var fragen = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
        {
            ("Was brauchen Pflanzen für die Fotosynthese?", new[] { "Licht, Wasser und Kohlenstoffdioxid", "Nur Wasser", "Nur Licht" },
                "Licht, Wasser und Kohlenstoffdioxid",
                "Bei der Fotosynthese wandeln Pflanzen mithilfe von Licht Wasser und CO₂ in Traubenzucker und Sauerstoff um."),
            ("Welches Gas geben Pflanzen bei der Fotosynthese ab?", new[] { "Sauerstoff", "Kohlenstoffdioxid", "Stickstoff" },
                "Sauerstoff", "Pflanzen nehmen CO₂ auf und geben bei der Fotosynthese Sauerstoff (O₂) ab."),
            ("In welchem Teil der Pflanzenzelle findet die Fotosynthese statt?", new[] { "Chloroplasten", "Zellkern", "Mitochondrien" },
                "Chloroplasten", "Die grünen Chloroplasten enthalten Chlorophyll und sind der Ort der Fotosynthese.")
        };
        var f = fragen[r.Next(fragen.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Fotosynthese (Biologie)",
            Type = QuestionType.MultipleChoice,
            Prompt = f.Frage,
            Options = f.Optionen,
            CorrectAnswers = new[] { f.Antwort },
            Explanation = f.Erklaerung
        };
    }

    private static QuizQuestion Magnetismus(Random r)
    {
        var fragen = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
        {
            ("Welche Metalle werden von einem Magneten angezogen?", new[] { "Eisen, Nickel, Kobalt", "Gold und Silber", "Aluminium und Kupfer" },
                "Eisen, Nickel, Kobalt", "Nur ferromagnetische Metalle wie Eisen, Nickel und Kobalt werden von Magneten angezogen."),
            ("Was passiert, wenn sich zwei gleiche Pole (Nord-Nord) eines Magneten nähern?", new[] { "Sie stoßen sich ab", "Sie ziehen sich an", "Nichts passiert" },
                "Sie stoßen sich ab", "Gleiche Pole stoßen sich ab, ungleiche Pole (Nord-Süd) ziehen sich an.")
        };
        var f = fragen[r.Next(fragen.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Magnetismus (Physik)",
            Type = QuestionType.MultipleChoice,
            Prompt = f.Frage,
            Options = f.Optionen,
            CorrectAnswers = new[] { f.Antwort },
            Explanation = f.Erklaerung
        };
    }

    private static QuizQuestion Atommodell(Random r)
    {
        var fragen = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
        {
            ("Welche Ladung hat ein Proton?", new[] { "Positiv", "Negativ", "Neutral" }, "Positiv",
                "Protonen sind positiv geladen, Elektronen negativ, Neutronen neutral (ungeladen)."),
            ("Wo befinden sich die Elektronen im Bohrschen Atommodell?", new[] { "In Schalen um den Atomkern", "Im Atomkern", "Es gibt keine Elektronen" },
                "In Schalen um den Atomkern", "Elektronen umkreisen den Atomkern auf bestimmten Energieschalen."),
            ("Was bestimmt die Ordnungszahl im Periodensystem?", new[] { "Die Anzahl der Protonen", "Die Anzahl der Neutronen", "Die Masse des Atoms" },
                "Die Anzahl der Protonen", "Die Ordnungszahl entspricht der Protonenzahl im Atomkern.")
        };
        var f = fragen[r.Next(fragen.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Atommodell (Chemie)",
            Type = QuestionType.MultipleChoice,
            Prompt = f.Frage,
            Options = f.Optionen,
            CorrectAnswers = new[] { f.Antwort },
            Explanation = f.Erklaerung
        };
    }

    private static QuizQuestion OhmschesGesetz(Random r)
    {
        int widerstand = new[] { 5, 10, 20, 25, 50 }[r.Next(5)];
        int strom = new[] { 1, 2, 3, 4 }[r.Next(4)];
        int spannung = widerstand * strom;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Ohmsches Gesetz (Physik)",
            Type = QuestionType.OpenText,
            Prompt = $"Ein Widerstand von {widerstand} Ohm wird von einem Strom von {strom} Ampere durchflossen. " +
                     "Wie groß ist die Spannung in Volt? (U = R · I)",
            CorrectAnswers = new[] { spannung.ToString() },
            Explanation = $"Ohmsches Gesetz: U = R · I = {widerstand} Ω · {strom} A = {spannung} V."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ZellbiologieListe =
    {
        ("Welcher Zellbestandteil enthält die Erbinformation (DNA)?", new[] { "Zellkern", "Zellmembran", "Mitochondrium" }, "Zellkern",
            "Der Zellkern enthält die DNA mit der Erbinformation der Zelle."),
        ("Welcher Zellbestandteil wird als \"Kraftwerk der Zelle\" bezeichnet?", new[] { "Mitochondrium", "Zellkern", "Ribosom" }, "Mitochondrium",
            "Mitochondrien produzieren durch Zellatmung Energie (ATP) und werden daher \"Kraftwerk der Zelle\" genannt."),
        ("Was unterscheidet eine Pflanzenzelle von einer Tierzelle?", new[] { "Zellwand und Chloroplasten", "Zellkern", "Zellmembran" }, "Zellwand und Chloroplasten",
            "Pflanzenzellen besitzen zusätzlich eine feste Zellwand und Chloroplasten für die Fotosynthese.")
    };

    private static QuizQuestion Zellbiologie(Random r)
    {
        var z = ZellbiologieListe[r.Next(ZellbiologieListe.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Zellbiologie",
            Type = QuestionType.MultipleChoice,
            Prompt = z.Frage,
            Options = z.Optionen,
            CorrectAnswers = new[] { z.Antwort },
            Explanation = z.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ReaktionenListe =
    {
        ("Wie nennt man eine Reaktion, bei der Energie in Form von Wärme frei wird?", new[] { "Exotherm", "Endotherm", "Neutral" }, "Exotherm",
            "Bei exothermen Reaktionen wird Energie (meist als Wärme) an die Umgebung abgegeben, z.B. bei einer Verbrennung."),
        ("Was entsteht bei der vollständigen Verbrennung von Kohlenstoff mit Sauerstoff?", new[] { "Kohlenstoffdioxid (CO₂)", "Wasser (H₂O)", "Methan (CH₄)" }, "Kohlenstoffdioxid (CO₂)",
            "C + O₂ → CO₂ – Kohlenstoff reagiert mit Sauerstoff zu Kohlenstoffdioxid."),
        ("Was bleibt bei einer chemischen Reaktion nach dem Gesetz von der Erhaltung der Masse gleich?", new[] { "Die Gesamtmasse aller Stoffe", "Die Farbe der Stoffe", "Der Aggregatzustand" }, "Die Gesamtmasse aller Stoffe",
            "Nach dem Massenerhaltungssatz bleibt die Gesamtmasse vor und nach einer chemischen Reaktion gleich.")
    };

    private static QuizQuestion ChemischeReaktion(Random r)
    {
        var c = ReaktionenListe[r.Next(ReaktionenListe.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Chemische Reaktionen",
            Type = QuestionType.MultipleChoice,
            Prompt = c.Frage,
            Options = c.Optionen,
            CorrectAnswers = new[] { c.Antwort },
            Explanation = c.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] VererbungListe =
    {
        ("Wie werden Merkmale von Eltern an Kinder weitergegeben?", new[] { "Über Gene (DNA)", "Über das Blut allein", "Zufällig, ohne Träger" }, "Über Gene (DNA)",
            "Gene, die auf der DNA liegen, tragen die Erbinformationen und werden von den Eltern an die Kinder weitergegeben."),
        ("Was beschreibt ein dominantes Allel?", new[] { "Es setzt sich gegenüber dem rezessiven Allel durch", "Es wird nie sichtbar", "Es kommt nur bei Tieren vor" },
            "Es setzt sich gegenüber dem rezessiven Allel durch",
            "Ein dominantes Allel bestimmt das äußere Merkmal (Phänotyp), auch wenn nur eine Kopie davon vorhanden ist.")
    };

    private static QuizQuestion Vererbung(Random r)
    {
        var v = VererbungListe[r.Next(VererbungListe.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Naturwissenschaften,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Vererbung (Genetik)",
            Type = QuestionType.MultipleChoice,
            Prompt = v.Frage,
            Options = v.Optionen,
            CorrectAnswers = new[] { v.Antwort },
            Explanation = v.Erklaerung
        };
    }
}
