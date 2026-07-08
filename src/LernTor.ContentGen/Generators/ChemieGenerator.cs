using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Chemie nach Berliner Rahmenlehrplan, Klasse 6 (Grundlagen) und Klasse 9 (vertieft).</summary>
public sealed class ChemieGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Chemie;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { StoffeTrennen, Verbrennung, SaeurenLaugen },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Atommodell, ChemischeReaktion, Periodensystem }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TrennListe =
    {
        ("Wie trennt man Sand von Wasser am einfachsten?", new[] { "Filtration (durch einen Filter gießen)", "Verdampfen", "Magnet verwenden" },
            "Filtration (durch einen Filter gießen)", "Beim Filtrieren bleibt der unlösliche Sand im Filter zurück, das Wasser läuft durch."),
        ("Wie trennt man Salz von Wasser (Salzwasser)?", new[] { "Verdampfen/Eindampfen des Wassers", "Filtration", "Sieben" },
            "Verdampfen/Eindampfen des Wassers", "Beim Verdampfen bleibt das gelöste Salz zurück, während das Wasser als Dampf entweicht."),
        ("Wie trennt man Eisenspäne von Sand?", new[] { "Mit einem Magneten", "Filtration", "Verdampfen" },
            "Mit einem Magneten", "Eisen ist magnetisch und lässt sich so vom nichtmagnetischen Sand trennen.")
    };

    private static QuizQuestion StoffeTrennen(Random r)
    {
        var f = TrennListe[r.Next(TrennListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Stoffgemische trennen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Trennmethode richtet sich nach der Eigenschaft: unlöslich → Filtration, gelöst → Verdampfen, magnetisch → Magnet."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] VerbrennungListe =
    {
        ("Was braucht ein Feuer zum Brennen (Verbrennungsdreieck)?", new[] { "Brennstoff, Sauerstoff und Wärme (Zündtemperatur)", "Nur einen Funken", "Nur Wasser" },
            "Brennstoff, Sauerstoff und Wärme (Zündtemperatur)",
            "Ohne Brennstoff, Sauerstoff oder ausreichende Wärme kann kein Feuer entstehen oder brennen bleiben."),
        ("Warum erstickt eine Kerze unter einem Glas?", new[] { "Der Sauerstoff im Glas wird verbraucht", "Das Glas ist zu kalt", "Die Kerze braucht Licht von außen" },
            "Der Sauerstoff im Glas wird verbraucht",
            "Ohne Nachschub an Sauerstoff kann die Verbrennung nicht weitergehen, die Flamme erlischt.")
    };

    private static QuizQuestion Verbrennung(Random r)
    {
        var f = VerbrennungListe[r.Next(VerbrennungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Verbrennung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Verbrennungsdreieck: Brennstoff + Sauerstoff + Zündtemperatur - fehlt eins davon, erlischt das Feuer."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SaeureListe =
    {
        ("Welche Farbe zeigt Rotkohlsaft (als Indikator) bei einer Säure häufig an?", new[] { "Rot/Pink", "Blau/Grün", "Er bleibt violett" },
            "Rot/Pink", "Rotkohlsaft ist ein natürlicher Indikator: Säuren färben ihn rötlich, Laugen eher blau-grün."),
        ("Was zeigt der pH-Wert an?", new[] { "Ob eine Lösung sauer, neutral oder basisch (alkalisch) ist", "Die Temperatur einer Lösung", "Wie viel Salz gelöst ist" },
            "Ob eine Lösung sauer, neutral oder basisch (alkalisch) ist",
            "Der pH-Wert reicht von 0 (stark sauer) über 7 (neutral) bis 14 (stark basisch/alkalisch).")
    };

    private static QuizQuestion SaeurenLaugen(Random r)
    {
        var f = SaeureListe[r.Next(SaeureListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Säuren und Laugen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "pH-Wert: 0 = stark sauer, 7 = neutral, 14 = stark basisch. Rotkohlsaft färbt sich bei Säuren rötlich, bei Laugen blau-grün."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AtomListe =
    {
        ("Welche Ladung hat ein Proton?", new[] { "Positiv", "Negativ", "Neutral" }, "Positiv",
            "Protonen sind positiv geladen, Elektronen negativ, Neutronen neutral (ungeladen)."),
        ("Wo befinden sich die Elektronen im Bohrschen Atommodell?", new[] { "In Schalen um den Atomkern", "Im Atomkern", "Es gibt keine Elektronen" },
            "In Schalen um den Atomkern", "Elektronen umkreisen den Atomkern auf bestimmten Energieschalen."),
        ("Was bestimmt die Ordnungszahl im Periodensystem?", new[] { "Die Anzahl der Protonen", "Die Anzahl der Neutronen", "Die Masse des Atoms" },
            "Die Anzahl der Protonen", "Die Ordnungszahl entspricht der Protonenzahl im Atomkern.")
    };

    private static QuizQuestion Atommodell(Random r)
    {
        var f = AtomListe[r.Next(AtomListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Atommodell", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Proton = positiv, Elektron = negativ, Neutron = neutral. Elektronen umkreisen den Kern in Schalen; Ordnungszahl = Protonenzahl."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ReaktionenListe =
    {
        ("Wie nennt man eine Reaktion, bei der Energie in Form von Wärme frei wird?", new[] { "Exotherm", "Endotherm", "Neutral" }, "Exotherm",
            "Bei exothermen Reaktionen wird Energie (meist als Wärme) an die Umgebung abgegeben, z.B. bei einer Verbrennung."),
        ("Was entsteht bei der vollständigen Verbrennung von Kohlenstoff mit Sauerstoff?", new[] { "Kohlenstoffdioxid (CO₂)", "Wasser (H₂O)", "Methan (CH₄)" }, "Kohlenstoffdioxid (CO₂)",
            "C + O₂ → CO₂ – Kohlenstoff reagiert mit Sauerstoff zu Kohlenstoffdioxid."),
        ("Was bleibt bei einer chemischen Reaktion nach dem Massenerhaltungssatz gleich?", new[] { "Die Gesamtmasse aller Stoffe", "Die Farbe der Stoffe", "Der Aggregatzustand" }, "Die Gesamtmasse aller Stoffe",
            "Nach dem Massenerhaltungssatz bleibt die Gesamtmasse vor und nach einer chemischen Reaktion gleich.")
    };

    private static QuizQuestion ChemischeReaktion(Random r)
    {
        var c = ReaktionenListe[r.Next(ReaktionenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Chemische Reaktionen", Type = QuestionType.MultipleChoice,
            Prompt = c.Frage, Options = c.Optionen, CorrectAnswers = new[] { c.Antwort }, Explanation = c.Erklaerung,
            HelpHint = "Exotherm = Energie wird abgegeben (z.B. Verbrennung). Nach dem Massenerhaltungssatz bleibt die Gesamtmasse bei einer Reaktion gleich."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PseListe =
    {
        ("Wie sind die Elemente im Periodensystem in den Spalten (Gruppen) angeordnet?", new[] { "Nach ähnlichen chemischen Eigenschaften", "Nach Farbe", "Zufällig" },
            "Nach ähnlichen chemischen Eigenschaften", "Elemente in derselben Gruppe (Spalte) haben ähnliche Eigenschaften, z.B. die Edelgase."),
        ("Welche Elementgruppe reagiert kaum mit anderen Stoffen?", new[] { "Edelgase", "Alkalimetalle", "Halogene" }, "Edelgase",
            "Edelgase (z.B. Helium, Neon) haben eine vollständig gefüllte äußere Elektronenschale und reagieren daher kaum.")
    };

    private static QuizQuestion Periodensystem(Random r)
    {
        var f = PseListe[r.Next(PseListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Periodensystem", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Elemente in derselben Spalte (Gruppe) des Periodensystems haben ähnliche Eigenschaften - Edelgase reagieren kaum."
        };
    }
}
