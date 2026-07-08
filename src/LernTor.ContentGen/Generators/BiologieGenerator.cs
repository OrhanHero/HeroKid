using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Biologie nach Berliner Rahmenlehrplan, Klasse 6 (Grundlagen) und Klasse 9 (vertieft).</summary>
public sealed class BiologieGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Biologie;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { MenschlicheOrgane, Fotosynthese, Wirbeltierklassen },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Zellbiologie, Vererbung, Oekosystem }
        };

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
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Der menschliche Körper", Type = QuestionType.MultipleChoice,
            Prompt = $"Was ist die Aufgabe des Organs \"{o.Organ}\"?",
            Options = optionen, CorrectAnswers = new[] { o.Funktion }, Explanation = $"Das {o.Organ} {o.Funktion}.",
            HelpHint = "Überlege, welches Organsystem betroffen ist: Kreislauf (Herz), Atmung (Lunge), Verdauung (Magen) oder Nervensystem (Gehirn)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FotosyntheseListe =
    {
        ("Was brauchen Pflanzen für die Fotosynthese?", new[] { "Licht, Wasser und Kohlenstoffdioxid", "Nur Wasser", "Nur Licht" },
            "Licht, Wasser und Kohlenstoffdioxid",
            "Bei der Fotosynthese wandeln Pflanzen mithilfe von Licht Wasser und CO₂ in Traubenzucker und Sauerstoff um."),
        ("Welches Gas geben Pflanzen bei der Fotosynthese ab?", new[] { "Sauerstoff", "Kohlenstoffdioxid", "Stickstoff" },
            "Sauerstoff", "Pflanzen nehmen CO₂ auf und geben bei der Fotosynthese Sauerstoff (O₂) ab."),
        ("In welchem Teil der Pflanzenzelle findet die Fotosynthese statt?", new[] { "Chloroplasten", "Zellkern", "Mitochondrien" },
            "Chloroplasten", "Die grünen Chloroplasten enthalten Chlorophyll und sind der Ort der Fotosynthese.")
    };

    private static QuizQuestion Fotosynthese(Random r)
    {
        var f = FotosyntheseListe[r.Next(FotosyntheseListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Fotosynthese", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Fotosynthese-Formel (vereinfacht): Licht + Wasser + CO₂ → Traubenzucker + Sauerstoff, in den Chloroplasten."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirbeltiereListe =
    {
        ("Welche Wirbeltierklasse atmet als einzige zeitlebens über Kiemen?", new[] { "Fische", "Amphibien", "Vögel" }, "Fische",
            "Fische atmen mit Kiemen im Wasser gelösten Sauerstoff, im Gegensatz zu Amphibien, die als erwachsene Tiere meist Lungen nutzen."),
        ("Welche Wirbeltierklasse ist wechselwarm UND kann sowohl im Wasser als auch an Land leben?", new[] { "Amphibien (z.B. Frosch)", "Vögel", "Säugetiere" }, "Amphibien (z.B. Frosch)",
            "Amphibien wie Frösche leben als Larve (Kaulquappe) im Wasser und als erwachsenes Tier oft an Land."),
        ("Welches Merkmal haben alle Säugetiere gemeinsam?", new[] { "Sie säugen ihre Jungen mit Milch", "Sie legen alle Eier", "Sie sind wechselwarm" }, "Sie säugen ihre Jungen mit Milch",
            "Namensgebendes Merkmal der Säugetiere ist, dass Muttertiere ihre Jungen mit selbst produzierter Milch säugen.")
    };

    private static QuizQuestion Wirbeltierklassen(Random r)
    {
        var f = WirbeltiereListe[r.Next(WirbeltiereListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wirbeltierklassen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Fische atmen mit Kiemen, Amphibien leben in Wasser und Land, Säugetiere säugen ihre Jungen mit Milch."
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
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Zellbiologie", Type = QuestionType.MultipleChoice,
            Prompt = z.Frage, Options = z.Optionen, CorrectAnswers = new[] { z.Antwort }, Explanation = z.Erklaerung,
            HelpHint = "Zellkern = Erbinformation, Mitochondrium = \"Kraftwerk der Zelle\", Zellwand/Chloroplasten nur bei Pflanzenzellen."
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
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Vererbung (Genetik)", Type = QuestionType.MultipleChoice,
            Prompt = v.Frage, Options = v.Optionen, CorrectAnswers = new[] { v.Antwort }, Explanation = v.Erklaerung,
            HelpHint = "Gene auf der DNA tragen Erbinformationen; ein dominantes Allel setzt sich gegenüber einem rezessiven durch."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] OekosystemListe =
    {
        ("Was bezeichnet man als \"Produzenten\" in einem Ökosystem?", new[] { "Pflanzen, die durch Fotosynthese Energie herstellen", "Tiere, die andere Tiere fressen", "Pilze und Bakterien, die zersetzen" },
            "Pflanzen, die durch Fotosynthese Energie herstellen",
            "Produzenten (Pflanzen) bilden die Grundlage jedes Nahrungsnetzes, da sie mit Sonnenenergie Biomasse aufbauen."),
        ("Welche Rolle haben Destruenten (z.B. Pilze, Bakterien) im Ökosystem?", new[] { "Sie zersetzen abgestorbene Lebewesen und Kreisläufe schließen sich", "Sie produzieren Sauerstoff", "Sie fressen nur Pflanzen" },
            "Sie zersetzen abgestorbene Lebewesen und Kreisläufe schließen sich",
            "Destruenten bauen tote organische Stoffe ab und geben Nährstoffe an den Boden zurück.")
    };

    private static QuizQuestion Oekosystem(Random r)
    {
        var f = OekosystemListe[r.Next(OekosystemListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Ökosysteme", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Produzenten (Pflanzen) stellen Energie her, Konsumenten fressen andere Lebewesen, Destruenten (Pilze/Bakterien) zersetzen."
        };
    }
}
