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
            [GradeLevel.Klasse6] = new List<TopicFactory> { StoffeTrennen, Verbrennung, SaeurenLaugen, MetalleEigenschaften },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Atommodell, ChemischeReaktion, Periodensystem }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TrennListe =
    {
        ("Wie trennt man Sand von Wasser am einfachsten?", new[] { "Filtration (durch einen Filter gießen)", "Verdampfen", "Magnet verwenden" },
            "Filtration (durch einen Filter gießen)", "Beim Filtrieren bleibt der unlösliche Sand im Filter zurück, das Wasser läuft durch."),
        ("Wie trennt man Salz von Wasser (Salzwasser)?", new[] { "Verdampfen/Eindampfen des Wassers", "Filtration", "Sieben" },
            "Verdampfen/Eindampfen des Wassers", "Beim Verdampfen bleibt das gelöste Salz zurück, während das Wasser als Dampf entweicht."),
        ("Wie trennt man Eisenspäne von Sand?", new[] { "Mit einem Magneten", "Filtration", "Verdampfen" },
            "Mit einem Magneten", "Eisen ist magnetisch und lässt sich so vom nichtmagnetischen Sand trennen."),
        ("Wie trennt man Öl von Wasser?", new[] { "Man lässt es sich trennen und schöpft das Öl ab (Dekantieren)", "Filtration", "Magnet verwenden" },
            "Man lässt es sich trennen und schöpft das Öl ab (Dekantieren)", "Öl und Wasser mischen sich nicht - sie trennen sich von selbst in Schichten, das Öl kann oben abgeschöpft werden."),
        ("Wie trennt man ein Gemisch aus zwei Flüssigkeiten mit unterschiedlichem Siedepunkt (z.B. Alkohol und Wasser)?", new[] { "Destillation", "Filtration", "Magnet verwenden" },
            "Destillation", "Bei der Destillation verdampft die Flüssigkeit mit dem niedrigeren Siedepunkt zuerst und wird getrennt aufgefangen.")
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
            "Ohne Nachschub an Sauerstoff kann die Verbrennung nicht weitergehen, die Flamme erlischt."),
        ("Was für eine Art von Reaktion ist eine Verbrennung?", new[] { "Eine Reaktion mit Sauerstoff (Oxidation)", "Ein rein physikalischer Vorgang ohne neue Stoffe", "Eine Reaktion ohne jeglichen Stoffwechsel" },
            "Eine Reaktion mit Sauerstoff (Oxidation)", "Bei einer Verbrennung reagiert ein Stoff mit Sauerstoff - das ist eine chemische Reaktion (Oxidation), es entstehen neue Stoffe."),
        ("Womit kann man ein kleines Feuer am besten ersticken?", new[] { "Mit einer Decke oder einem Deckel (Sauerstoff abschneiden)", "Mit noch mehr Luft anfachen", "Mit Benzin übergießen" },
            "Mit einer Decke oder einem Deckel (Sauerstoff abschneiden)", "Wird dem Feuer der Sauerstoff entzogen (z.B. durch eine Löschdecke), kann die Verbrennung nicht weitergehen."),
        ("Welches Gas entsteht bei der vollständigen Verbrennung von Holz oder Kohle hauptsächlich?", new[] { "Kohlenstoffdioxid (CO₂)", "Sauerstoff (O₂)", "Wasserstoff (H₂)" },
            "Kohlenstoffdioxid (CO₂)", "Der im Brennstoff enthaltene Kohlenstoff verbindet sich bei der Verbrennung mit Sauerstoff zu Kohlenstoffdioxid.")
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
            "Der pH-Wert reicht von 0 (stark sauer) über 7 (neutral) bis 14 (stark basisch/alkalisch)."),
        ("Was passiert, wenn man eine Säure und eine Lauge in passender Menge zusammengibt?", new[] { "Sie neutralisieren sich gegenseitig", "Es entsteht sofort Feuer", "Nichts passiert" },
            "Sie neutralisieren sich gegenseitig", "Bei der Neutralisation reagieren Säure und Lauge zu einem (meist ungefähr neutralen) Salz und Wasser."),
        ("Welchen ungefähren pH-Wert hat reines Wasser?", new[] { "7 (neutral)", "0 (stark sauer)", "14 (stark basisch)" },
            "7 (neutral)", "Reines Wasser gilt mit einem pH-Wert von etwa 7 als neutral - weder sauer noch basisch."),
        ("Warum sollte man mit konzentrierten Säuren und Laugen im Chemieunterricht vorsichtig umgehen?", new[] { "Sie können Haut, Augen und Kleidung stark schädigen (ätzend)", "Sie sind komplett ungefährlich", "Sie färben nur die Hände bunt" },
            "Sie können Haut, Augen und Kleidung stark schädigen (ätzend)", "Konzentrierte Säuren und Laugen sind ätzend und können Verätzungen verursachen - deshalb gelten im Unterricht besondere Schutzmaßnahmen.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MetalleListe =
    {
        ("Welche Eigenschaft haben (fast) alle Metalle gemeinsam?", new[] { "Sie leiten elektrischen Strom gut", "Sie sind durchsichtig", "Sie lösen sich immer in Wasser" },
            "Sie leiten elektrischen Strom gut", "Metalle haben frei bewegliche Elektronen, die elektrischen Strom und Wärme gut leiten."),
        ("Wie nennt man die Eigenschaft von Metallen, sich zu dünnen Blechen verformen zu lassen?", new[] { "Verformbarkeit (Duktilität)", "Löslichkeit", "Brennbarkeit" },
            "Verformbarkeit (Duktilität)", "Metalle lassen sich hämmern, walzen oder zu Draht ziehen, ohne zu zerbrechen - das nennt man Verformbarkeit."),
        ("Welches Metall wird häufig für Stromkabel verwendet, weil es Strom besonders gut leitet?", new[] { "Kupfer", "Gold", "Blei" },
            "Kupfer", "Kupfer leitet elektrischen Strom sehr gut und ist zugleich günstiger als Gold oder Silber, daher wird es für Kabel verwendet."),
        ("Was passiert, wenn Eisen über längere Zeit Feuchtigkeit und Sauerstoff ausgesetzt ist?", new[] { "Es rostet (Korrosion)", "Es wird magnetisch", "Es schmilzt bei Zimmertemperatur" },
            "Es rostet (Korrosion)", "Rost entsteht durch eine chemische Reaktion von Eisen mit Sauerstoff und Wasser (Korrosion)."),
        ("Warum werden Besteck und Kochtöpfe oft aus Metall hergestellt?", new[] { "Metalle leiten Wärme gut und sind stabil/langlebig", "Weil Metall am leichtesten von allen Materialien ist", "Weil Metall immer durchsichtig ist" },
            "Metalle leiten Wärme gut und sind stabil/langlebig", "Die gute Wärmeleitfähigkeit und Stabilität von Metallen macht sie ideal für Kochgeschirr und Besteck.")
    };

    private static QuizQuestion MetalleEigenschaften(Random r)
    {
        var f = MetalleListe[r.Next(MetalleListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Metalle und ihre Eigenschaften", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Typische Metalleigenschaften: leiten Strom/Wärme gut, glänzen, sind verformbar (Duktilität) - manche rosten (korrodieren) bei Feuchtigkeit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AtomListe =
    {
        ("Welche Ladung hat ein Proton?", new[] { "Positiv", "Negativ", "Neutral" }, "Positiv",
            "Protonen sind positiv geladen, Elektronen negativ, Neutronen neutral (ungeladen)."),
        ("Wo befinden sich die Elektronen im Bohrschen Atommodell?", new[] { "In Schalen um den Atomkern", "Im Atomkern", "Es gibt keine Elektronen" },
            "In Schalen um den Atomkern", "Elektronen umkreisen den Atomkern auf bestimmten Energieschalen."),
        ("Was bestimmt die Ordnungszahl im Periodensystem?", new[] { "Die Anzahl der Protonen", "Die Anzahl der Neutronen", "Die Masse des Atoms" },
            "Die Anzahl der Protonen", "Die Ordnungszahl entspricht der Protonenzahl im Atomkern."),
        ("Wie nennt man Atome desselben Elements mit unterschiedlicher Neutronenzahl?", new[] { "Isotope", "Ionen", "Moleküle" },
            "Isotope", "Isotope eines Elements haben gleich viele Protonen, aber unterschiedlich viele Neutronen."),
        ("Was ist ein Ion?", new[] { "Ein elektrisch geladenes Atom (oder Molekül) durch Elektronenabgabe/-aufnahme", "Ein Atom ohne Elektronen", "Ein anderes Wort für Molekül" },
            "Ein elektrisch geladenes Atom (oder Molekül) durch Elektronenabgabe/-aufnahme", "Gibt ein Atom Elektronen ab oder nimmt welche auf, entsteht ein geladenes Ion (positiv oder negativ).")
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
            "Nach dem Massenerhaltungssatz bleibt die Gesamtmasse vor und nach einer chemischen Reaktion gleich."),
        ("Wie nennt man eine Reaktion, bei der Energie aus der Umgebung aufgenommen wird?", new[] { "Endotherm", "Exotherm", "Neutral" }, "Endotherm",
            "Bei endothermen Reaktionen wird Energie (z.B. Wärme) aus der Umgebung aufgenommen, z.B. beim Backen."),
        ("Was zeigt eine chemische Reaktionsgleichung wie \"2 H₂ + O₂ → 2 H₂O\"?", new[] { "Wie viele Teilchen der Ausgangsstoffe zu welchem Produkt reagieren", "Nur die Farbe der beteiligten Stoffe", "Wie schnell die Reaktion abläuft" }, "Wie viele Teilchen der Ausgangsstoffe zu welchem Produkt reagieren",
            "Die Zahlen (Koeffizienten) vor den Formeln zeigen das Mengenverhältnis, in dem die Stoffe reagieren.")
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
            "Edelgase (z.B. Helium, Neon) haben eine vollständig gefüllte äußere Elektronenschale und reagieren daher kaum."),
        ("Wie sind die Elemente im Periodensystem in den Zeilen (Perioden) angeordnet?", new[] { "Nach steigender Ordnungszahl/Protonenzahl", "Nach Alphabet", "Nach Entdeckungsjahr" }, "Nach steigender Ordnungszahl/Protonenzahl",
            "Innerhalb einer Periode (Zeile) steigt die Ordnungszahl (Protonenzahl) von links nach rechts."),
        ("Zu welcher Elementgruppe gehören Natrium und Kalium?", new[] { "Alkalimetalle", "Edelgase", "Halogene" }, "Alkalimetalle",
            "Natrium und Kalium gehören zu den Alkalimetallen (1. Gruppe) - sehr reaktionsfreudige Metalle."),
        ("Wer entwickelte die erste Version des modernen Periodensystems?", new[] { "Dmitri Mendelejew", "Marie Curie", "Albert Einstein" }, "Dmitri Mendelejew",
            "Der russische Chemiker Dmitri Mendelejew ordnete 1869 die Elemente erstmals systematisch nach ihren Eigenschaften an.")
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
