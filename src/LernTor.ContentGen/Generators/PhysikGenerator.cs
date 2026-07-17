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
            [GradeLevel.Klasse6] = new List<TopicFactory> { Aggregatzustaende, Stromkreis, Magnetismus, MessenUndSinne, OptikUndWeltraum, BewegungUndBionik, WaermeausdehnungKoerper, WechselwirkungUndKraft, MechanischeEnergieUndArbeit, ThermischeEnergieUndWaerme },
            [GradeLevel.Klasse9] = new List<TopicFactory> { OhmschesGesetz, Energieerhaltung, NewtonscheGesetze, MagnetfelderInduktion, Kinematik, RadioaktivitaetUndKernphysik, SchwingungenWellenOptik }
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
            "Bei der Sublimation geht ein Stoff direkt vom festen in den gasförmigen Zustand über, z.B. Trockeneis (gefrorenes CO₂)."),
        ("Wie nennt man den Übergang von gasförmig zu flüssig?", new[] { "Kondensation", "Sublimation", "Erstarren" }, "Kondensation",
            "Bei der Kondensation wird aus Gas (z.B. Wasserdampf) wieder Flüssigkeit, z.B. wenn sich eine kalte Scheibe beschlägt."),
        ("Wie nennt man den Übergang von flüssig zu fest?", new[] { "Erstarren (Gefrieren)", "Schmelzen", "Verdampfen" }, "Erstarren (Gefrieren)",
            "Beim Erstarren (Gefrieren) wird aus einer Flüssigkeit ein fester Stoff, z.B. wenn Wasser zu Eis wird."),
        ("Wie nennt man den Übergang von gasförmig direkt zu fest (das Gegenteil der Sublimation)?", new[] { "Resublimation", "Kondensation", "Erstarren" }, "Resublimation",
            "Bei der Resublimation geht ein Gas direkt in den festen Zustand über, z.B. bildet sich so Reif auf kalten Oberflächen."),
        ("Wie bewegen sich Teilchen im festen Aggregatzustand im Vergleich zum gasförmigen Zustand?", new[] { "Sie schwingen nur um feste Positionen, im Gas bewegen sie sich frei", "Sie bewegen sich in beiden Zuständen exakt gleich", "Im festen Zustand bewegen sie sich am schnellsten" }, "Sie schwingen nur um feste Positionen, im Gas bewegen sie sich frei",
            "Im festen Zustand sind Teilchen in einem festen Gitter angeordnet und schwingen nur, im Gas bewegen sie sich frei und ungeordnet."),
        ("Warum nimmt ein Gas immer die Form seines Behälters an?", new[] { "Weil sich die Teilchen frei und ungeordnet bewegen können", "Weil Gas-Teilchen sich gar nicht bewegen", "Weil Gas immer eine feste Form behält" }, "Weil sich die Teilchen frei und ungeordnet bewegen können",
            "Gasteilchen haben große Abstände zueinander und bewegen sich frei - deshalb füllen sie jeden verfügbaren Raum vollständig aus."),
        ("Warum sinkt der Siedepunkt von Wasser in großer Höhe, z.B. auf einem hohen Berg?", new[] { "Weil der Luftdruck dort niedriger ist", "Weil es dort immer kälter ist als am Meer", "Weil Wasser in der Höhe eine andere chemische Formel hat" }, "Weil der Luftdruck dort niedriger ist",
            "Mit sinkendem Luftdruck sinkt auch der Siedepunkt von Wasser - in großer Höhe kocht Wasser schon bei weniger als 100°C."),
        ("Warum kocht Wasser in einem Schnellkochtopf bei einer höheren Temperatur als 100°C?", new[] { "Weil der Druck im geschlossenen Topf erhöht ist", "Weil ein Schnellkochtopf Wasser chemisch verändert", "Weil im Topf weniger Wasser als sonst verwendet wird" }, "Weil der Druck im geschlossenen Topf erhöht ist",
            "Der erhöhte Druck im Schnellkochtopf lässt Wasser erst bei einer höheren Temperatur sieden, wodurch Essen schneller gart."),
        ("Was ist der Unterschied zwischen Verdampfen und Verdunsten?", new[] { "Verdampfen passiert am Siedepunkt, Verdunsten schon bei niedrigeren Temperaturen", "Beide Begriffe bedeuten exakt dasselbe", "Verdunsten passiert nur bei Metallen" }, "Verdampfen passiert am Siedepunkt, Verdunsten schon bei niedrigeren Temperaturen",
            "Verdampfen findet beim Erreichen des Siedepunkts statt, während Verdunsten (z.B. eine trocknende Pfütze) auch bei niedrigeren Temperaturen langsam abläuft."),
        ("Was ist Trockeneis?", new[] { "Gefrorenes Kohlenstoffdioxid (CO₂), das direkt zu Gas sublimiert", "Gewöhnliches, sehr kaltes Wassereis", "Ein anderes Wort für Schnee" }, "Gefrorenes Kohlenstoffdioxid (CO₂), das direkt zu Gas sublimiert",
            "Trockeneis besteht aus festem CO₂ und geht bei Raumtemperatur direkt in den gasförmigen Zustand über, ohne zu schmelzen."),
        ("Was passiert, wenn warme, feuchte Luft auf eine kalte Fensterscheibe trifft?", new[] { "Der Wasserdampf kondensiert zu kleinen Wassertröpfchen (die Scheibe beschlägt)", "Die Scheibe wird automatisch wärmer", "Der Wasserdampf verschwindet komplett" }, "Der Wasserdampf kondensiert zu kleinen Wassertröpfchen (die Scheibe beschlägt)",
            "Trifft feuchte, warme Luft auf eine kalte Oberfläche, kondensiert der enthaltene Wasserdampf zu sichtbaren Tröpfchen."),
        ("Warum schwimmt Eis auf flüssigem Wasser (eine Besonderheit des Wassers)?", new[] { "Eis hat eine geringere Dichte als flüssiges Wasser", "Eis ist schwerer als flüssiges Wasser", "Eis und Wasser haben immer die exakt gleiche Dichte" }, "Eis hat eine geringere Dichte als flüssiges Wasser",
            "Anders als bei den meisten Stoffen dehnt sich Wasser beim Gefrieren aus - dadurch ist Eis leichter (weniger dicht) als flüssiges Wasser."),
        ("Bei welcher Temperatur liegt der Schmelzpunkt von Wasser bei normalem Luftdruck?", new[] { "0°C", "50°C", "-10°C" }, "0°C",
            "Bei normalem Luftdruck schmilzt Eis bei genau 0°C zu flüssigem Wasser."),
        ("Was passiert mit der Temperatur eines Stoffes während des Schmelzens, obwohl weiter Wärme zugeführt wird?", new[] { "Sie bleibt konstant, bis der Stoff komplett geschmolzen ist", "Sie steigt sofort stark an", "Sie sinkt während des Schmelzens" }, "Sie bleibt konstant, bis der Stoff komplett geschmolzen ist",
            "Während des Schmelzens wird die zugeführte Energie für den Zustandswechsel verwendet - die Temperatur bleibt bis zum vollständigen Schmelzen konstant."),
        ("Was ist Tau, der morgens manchmal auf Gras zu sehen ist?", new[] { "Kondensiertes Wasser aus der Luftfeuchtigkeit", "Gefrorener Regen", "Ein anderes Wort für Nebel" }, "Kondensiertes Wasser aus der Luftfeuchtigkeit",
            "Kühlt sich die Luft nachts ab, kondensiert der enthaltene Wasserdampf auf kühlen Oberflächen wie Gras - so entsteht Tau."),
        ("Wie nennt man einen vierten Aggregatzustand, der bei extrem hohen Temperaturen entsteht, z.B. in der Sonne?", new[] { "Plasma", "Kristall", "Gel" }, "Plasma",
            "Bei sehr hohen Temperaturen werden Atome ionisiert - dieser vierte Aggregatzustand heißt Plasma und kommt z.B. in Sternen vor.")
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
            "Beide Lampen gehen aus", "Bei einer Reihenschaltung ist der Stromkreis unterbrochen, sobald eine Lampe ausfällt - beide gehen aus."),
        ("Was passiert, wenn man zwei Lampen parallel (nebeneinander) schaltet und eine Lampe kaputtgeht?", new[] { "Die andere Lampe leuchtet weiter", "Beide Lampen gehen sofort aus", "Beide Lampen werden heller" },
            "Die andere Lampe leuchtet weiter", "Bei einer Parallelschaltung hat jede Lampe ihren eigenen Stromweg - fällt eine aus, bleibt der Kreis der anderen intakt."),
        ("Was ist ein \"Kurzschluss\"?", new[] { "Ein ungewollter, sehr niedriger Widerstand im Stromkreis mit sehr hohem Stromfluss", "Ein besonders kurzes Kabel", "Ein Stromkreis mit sehr geringer Spannung" }, "Ein ungewollter, sehr niedriger Widerstand im Stromkreis mit sehr hohem Stromfluss",
            "Bei einem Kurzschluss fließt der Strom über einen Weg mit sehr geringem Widerstand - das kann gefährlich viel Strom und Hitze erzeugen."),
        ("Wofür ist eine Sicherung in einem Stromkreis da?", new[] { "Sie unterbricht den Stromkreis automatisch bei zu hoher Stromstärke", "Sie macht Lampen automatisch heller", "Sie speichert zusätzliche Energie" }, "Sie unterbricht den Stromkreis automatisch bei zu hoher Stromstärke",
            "Eine Sicherung schützt vor Überlastung und Kurzschluss, indem sie den Stromkreis rechtzeitig unterbricht."),
        ("Was misst ein Voltmeter im Stromkreis?", new[] { "Die elektrische Spannung", "Die Stromstärke", "Den Widerstand" }, "Die elektrische Spannung",
            "Ein Voltmeter misst die Spannung zwischen zwei Punkten eines Stromkreises in Volt."),
        ("Wie wird ein Voltmeter im Unterschied zum Amperemeter in einen Stromkreis geschaltet?", new[] { "Parallel zum zu messenden Bauteil", "Immer in Reihe zum gesamten Kreis", "Es wird gar nicht in den Kreis eingebunden" }, "Parallel zum zu messenden Bauteil",
            "Während das Amperemeter in Reihe geschaltet wird, misst das Voltmeter parallel zum jeweiligen Bauteil."),
        ("Warum sollte man elektrische Geräte nicht mit nassen Händen berühren?", new[] { "Wasser leitet Strom und erhöht die Gefahr eines Stromschlags", "Wasser macht Strom komplett ungefährlich", "Nasse Hände verhindern jeden Stromfluss" }, "Wasser leitet Strom und erhöht die Gefahr eines Stromschlags",
            "Wasser leitet elektrischen Strom besser als trockene Haut - das erhöht das Risiko eines gefährlichen Stromschlags erheblich."),
        ("Was ist der Unterschied zwischen einem elektrischen Leiter und einem Isolator?", new[] { "Ein Leiter lässt Strom fließen, ein Isolator blockiert ihn weitgehend", "Beide leiten Strom exakt gleich gut", "Ein Isolator leitet Strom besser als ein Leiter" }, "Ein Leiter lässt Strom fließen, ein Isolator blockiert ihn weitgehend",
            "Leiter wie Metalle lassen elektrischen Strom gut fließen, Isolatoren wie Gummi oder Kunststoff verhindern den Stromfluss weitgehend."),
        ("Welche Spannung hat eine normale Steckdose in Deutschland?", new[] { "230 Volt", "12 Volt", "1000 Volt" }, "230 Volt",
            "Das deutsche Stromnetz liefert an haushaltsüblichen Steckdosen eine Wechselspannung von 230 Volt."),
        ("Was ist ein \"Schalter\" in einem Stromkreis?", new[] { "Ein Bauteil, mit dem man den Stromkreis öffnen oder schließen kann", "Ein Bauteil, das immer Strom erzeugt", "Ein anderes Wort für Batterie" }, "Ein Bauteil, mit dem man den Stromkreis öffnen oder schließen kann",
            "Ein Schalter unterbricht oder schließt gezielt den Stromkreis, z.B. um eine Lampe an- oder auszuschalten."),
        ("Was bedeutet \"Wechselstrom\" im Unterschied zu \"Gleichstrom\"?", new[] { "Wechselstrom ändert regelmäßig seine Richtung, Gleichstrom fließt immer in eine Richtung", "Beide Stromarten sind komplett identisch", "Wechselstrom fließt nur einmal und stoppt dann" }, "Wechselstrom ändert regelmäßig seine Richtung, Gleichstrom fließt immer in eine Richtung",
            "Im Hausstromnetz fließt Wechselstrom, der seine Fließrichtung regelmäßig ändert; Batterien liefern dagegen Gleichstrom."),
        ("Welche Stromart liefert eine normale Batterie?", new[] { "Gleichstrom", "Wechselstrom", "Gar keinen Strom" }, "Gleichstrom",
            "Batterien liefern Gleichstrom, der stets in dieselbe Richtung fließt."),
        ("Warum können Vögel gefahrlos auf einer einzelnen Hochspannungsleitung sitzen?", new[] { "Weil durch ihren Körper kein geschlossener Stromkreis mit Potenzialunterschied entsteht", "Weil Vögel von Natur aus gegen Strom immun sind", "Weil Hochspannungsleitungen keinen echten Strom führen" }, "Weil durch ihren Körper kein geschlossener Stromkreis mit Potenzialunterschied entsteht",
            "Solange der Vogel nur eine Leitung berührt und keinen zweiten Kontaktpunkt mit anderem Potenzial hat, fließt kein Strom durch seinen Körper."),
        ("Was ist ein \"Widerstand\" als Bauteil in einem Stromkreis?", new[] { "Ein Bauteil, das den Stromfluss gezielt begrenzt", "Ein Bauteil, das den Strom immer verstärkt", "Ein anderes Wort für Schalter" }, "Ein Bauteil, das den Stromfluss gezielt begrenzt",
            "Widerstände werden gezielt eingebaut, um die Stromstärke in einem Stromkreis zu begrenzen oder zu steuern."),
        ("Was passiert mit der Helligkeit einer Lampe, wenn man bei gleicher Spannung den Widerstand im Stromkreis erhöht?", new[] { "Die Lampe leuchtet schwächer, weil weniger Strom fließt", "Die Lampe leuchtet automatisch heller", "Die Helligkeit bleibt exakt gleich" }, "Die Lampe leuchtet schwächer, weil weniger Strom fließt",
            "Ein größerer Widerstand verringert bei gleicher Spannung die Stromstärke - die Lampe leuchtet dadurch schwächer."),
        ("Welchen Vorteil haben LEDs gegenüber klassischen Glühlampen?", new[] { "Sie verbrauchen deutlich weniger Energie bei ähnlicher Helligkeit", "Sie sind grundsätzlich viel teurer im Betrieb", "Sie funktionieren nur mit Gleichstrom aus Batterien" }, "Sie verbrauchen deutlich weniger Energie bei ähnlicher Helligkeit",
            "LEDs wandeln elektrische Energie effizienter in Licht um als klassische Glühlampen und verbrauchen dadurch deutlich weniger Energie.")
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
            "Ihn an einen starken Magneten halten/damit bestreichen", "Eisen kann durch Kontakt mit einem starken Magneten selbst vorübergehend magnetisch werden."),
        ("Was ist ein Elektromagnet?", new[] { "Ein Magnet, der nur wirkt, wenn Strom durch eine Spule fließt", "Ein Magnet, der niemals seine Wirkung verliert", "Ein Magnet aus reinem Gold" }, "Ein Magnet, der nur wirkt, wenn Strom durch eine Spule fließt",
            "Ein Elektromagnet erzeugt sein Magnetfeld nur, solange Strom durch die Spule um einen Eisenkern fließt."),
        ("Was ist der Unterschied zwischen einem Dauermagneten und einem Elektromagneten?", new[] { "Ein Dauermagnet ist immer magnetisch, ein Elektromagnet nur bei Stromfluss", "Beide sind exakt identisch", "Ein Dauermagnet funktioniert nur mit Strom" }, "Ein Dauermagnet ist immer magnetisch, ein Elektromagnet nur bei Stromfluss",
            "Dauermagnete behalten ihre magnetische Wirkung dauerhaft, während Elektromagnete nur bei fließendem Strom magnetisch sind."),
        ("Wie nennt man die unsichtbaren Linien, die den Verlauf eines Magnetfelds beschreiben?", new[] { "Feldlinien", "Isolatoren", "Widerstände" }, "Feldlinien",
            "Magnetische Feldlinien zeigen anschaulich Richtung und Stärke eines Magnetfelds - sichtbar machen kann man sie z.B. mit Eisenspänen."),
        ("Wo ist das Magnetfeld eines Stabmagneten am stärksten?", new[] { "An den Polen (Enden)", "Genau in der Mitte", "Überall exakt gleich stark" }, "An den Polen (Enden)",
            "An den Polen eines Stabmagneten treten die Feldlinien am dichtesten aus - dort ist das Magnetfeld am stärksten."),
        ("Ist der geografische Nordpol der Erde exakt identisch mit dem magnetischen Nordpol?", new[] { "Nein, sie liegen an leicht unterschiedlichen Stellen", "Ja, sie sind exakt derselbe Punkt", "Es gibt gar keinen magnetischen Nordpol" }, "Nein, sie liegen an leicht unterschiedlichen Stellen",
            "Der magnetische Nordpol weicht vom geografischen Nordpol ab und verschiebt sich sogar langsam über die Zeit."),
        ("Wodurch kann ein Magnet seine magnetische Wirkung verlieren?", new[] { "Durch starke Erhitzung oder starke Stöße", "Durch Berührung mit Papier", "Durch das Betrachten bei Tageslicht" }, "Durch starke Erhitzung oder starke Stöße",
            "Starke Hitze oder heftige Erschütterungen können die geordnete Ausrichtung der magnetischen Bereiche im Material zerstören."),
        ("In welchem medizinischen Gerät werden sehr starke Magnetfelder genutzt, um Bilder vom Körperinneren zu erzeugen?", new[] { "MRT (Magnetresonanztomograph)", "Blutdruckmessgerät", "Stethoskop" }, "MRT (Magnetresonanztomograph)",
            "Ein MRT nutzt starke Magnetfelder und Radiowellen, um detaillierte Bilder aus dem Körperinneren zu erzeugen, ganz ohne Röntgenstrahlung."),
        ("Wie funktionieren Lautsprecher grundsätzlich (vereinfacht)?", new[] { "Ein Elektromagnet bewegt mithilfe eines wechselnden Magnetfelds eine Membran", "Sie erzeugen Ton allein durch Luftdruckänderung ohne Magnete", "Sie funktionieren komplett ohne Strom" }, "Ein Elektromagnet bewegt mithilfe eines wechselnden Magnetfelds eine Membran",
            "In Lautsprechern lässt ein sich änderndes Magnetfeld eine Spule und die daran befestigte Membran schwingen - so entsteht Schall."),
        ("Was passiert mit der Anziehungskraft eines Magneten, je weiter man sich von ihm entfernt?", new[] { "Sie wird schwächer", "Sie wird stärker", "Sie bleibt exakt gleich" }, "Sie wird schwächer",
            "Die magnetische Anziehungskraft nimmt mit zunehmendem Abstand vom Magneten deutlich ab."),
        ("Warum werden Aluminium und Kupfer NICHT von einem gewöhnlichen Magneten angezogen?", new[] { "Sie sind keine ferromagnetischen Metalle", "Sie sind zu schwer für Magnete", "Magnete wirken nur bei Edelmetallen" }, "Sie sind keine ferromagnetischen Metalle",
            "Nur ferromagnetische Metalle wie Eisen, Nickel und Kobalt werden von gewöhnlichen Magneten deutlich angezogen."),
        ("Was nutzen Magnetschwebebahnen (Maglev-Züge), um über der Schiene zu schweben?", new[] { "Starke Magnetfelder zur Abstoßung/Anziehung", "Herkömmliche Räder aus Gummi", "Druckluft unter dem Zug" }, "Starke Magnetfelder zur Abstoßung/Anziehung",
            "Magnetschwebebahnen nutzen starke Elektromagnete, um den Zug berührungslos über der Schiene schweben zu lassen."),
        ("Was passiert, wenn man einen Stabmagneten genau in der Mitte durchsägt?", new[] { "Es entstehen zwei neue, vollständige Magnete mit je einem Nord- und Südpol", "Es entsteht ein Magnet ganz ohne Pole", "Der Magnetismus verschwindet komplett" }, "Es entstehen zwei neue, vollständige Magnete mit je einem Nord- und Südpol",
            "Magnete lassen sich nicht in einzelne Pole trennen - jedes Teilstück wird wieder ein vollständiger Magnet mit zwei Polen."),
        ("Was ist eine Kompassnadel im Grunde?", new[] { "Ein kleiner, frei beweglicher Magnet", "Ein Stück unmagnetisches Metall", "Ein elektronischer Sensor ohne Magnet" }, "Ein kleiner, frei beweglicher Magnet",
            "Die Kompassnadel ist selbst ein kleiner Magnet, der sich frei drehen kann und sich am Erdmagnetfeld ausrichtet."),
        ("Wofür wird Magnetismus bei alten Kassetten oder Magnetstreifen von Karten genutzt?", new[] { "Zum Speichern von Informationen", "Zum Erzeugen von Licht", "Zum Kühlen von Geräten" }, "Zum Speichern von Informationen",
            "Magnetische Materialien können Informationen speichern, indem winzige magnetische Bereiche gezielt ausgerichtet werden."),
        ("Was passiert, wenn man Eisenspäne in die Nähe eines Magneten streut?", new[] { "Sie ordnen sich sichtbar entlang der Feldlinien an", "Sie bleiben komplett regellos liegen", "Sie werden vom Magneten abgestoßen" }, "Sie ordnen sich sichtbar entlang der Feldlinien an",
            "Eisenspäne richten sich entlang der unsichtbaren Feldlinien aus und machen so den Verlauf des Magnetfelds sichtbar.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MessenSinneListe =
    {
        ("Was misst ein Thermometer?", new[] { "Die Temperatur", "Das Gewicht", "Die Länge" }, "Die Temperatur",
            "Ein Thermometer misst die Temperatur, meist in Grad Celsius."),
        ("Was misst eine Waage?", new[] { "Das Gewicht/die Masse", "Die Temperatur", "Die Zeit" }, "Das Gewicht/die Masse",
            "Eine Waage misst das Gewicht bzw. die Masse eines Gegenstands."),
        ("In welcher Einheit wird die Temperatur meist angegeben?", new[] { "Grad Celsius (°C)", "Kilogramm (kg)", "Liter (l)" }, "Grad Celsius (°C)",
            "Temperatur wird im Alltag meist in Grad Celsius angegeben."),
        ("In welcher Einheit wird eine Flüssigkeitsmenge oft angegeben?", new[] { "Liter (l)", "Kilogramm (kg)", "Meter (m)" }, "Liter (l)",
            "Flüssigkeitsmengen werden meist in Litern angegeben."),
        ("In welcher Einheit wird das Gewicht oft angegeben?", new[] { "Kilogramm (kg)", "Grad Celsius (°C)", "Liter (l)" }, "Kilogramm (kg)",
            "Das Gewicht bzw. die Masse wird meist in Kilogramm angegeben."),
        ("Was ist der Unterschied zwischen subjektivem Empfinden und objektivem Messen der Temperatur?", new[] { "Empfinden ist persönlich unterschiedlich, ein Thermometer zeigt einen genauen Messwert", "Beides ist immer exakt gleich", "Nur das Empfinden ist wissenschaftlich genau" }, "Empfinden ist persönlich unterschiedlich, ein Thermometer zeigt einen genauen Messwert",
            "Das Temperaturempfinden ist subjektiv und kann von Person zu Person unterschiedlich sein, ein Thermometer liefert einen objektiven Messwert."),
        ("Warum kann sich Wasser mit derselben Temperatur für zwei Personen unterschiedlich warm anfühlen?", new[] { "Weil das Temperaturempfinden subjektiv ist", "Weil die Temperatur sich ständig ändert", "Weil Wasser nie eine feste Temperatur hat" }, "Weil das Temperaturempfinden subjektiv ist",
            "Das persönliche Wärmeempfinden ist subjektiv, auch wenn die tatsächliche Temperatur objektiv gleich ist."),
        ("Welches Sinnesorgan nutzen wir zum Sehen?", new[] { "Die Augen", "Die Ohren", "Die Nase" }, "Die Augen",
            "Mit den Augen nehmen wir visuelle Reize wahr."),
        ("Welches Sinnesorgan nutzen wir zum Hören?", new[] { "Die Ohren", "Die Augen", "Die Zunge" }, "Die Ohren",
            "Mit den Ohren nehmen wir akustische Reize (Geräusche) wahr."),
        ("Welches Sinnesorgan nutzen wir zum Riechen?", new[] { "Die Nase", "Die Haut", "Die Ohren" }, "Die Nase",
            "Mit der Nase nehmen wir Gerüche wahr."),
        ("Welches Sinnesorgan nutzen wir zum Schmecken?", new[] { "Die Zunge", "Die Nase", "Die Augen" }, "Die Zunge",
            "Mit der Zunge nehmen wir Geschmacksreize wahr."),
        ("Welches Sinnesorgan nutzen wir zum Fühlen (Tasten)?", new[] { "Die Haut", "Die Augen", "Die Zunge" }, "Die Haut",
            "Mit der Haut nehmen wir Berührung, Druck und Temperatur wahr."),
        ("Warum sind Messgeräte wie Thermometer oder Waagen wichtig?", new[] { "Sie liefern genaue, überprüfbare Werte statt nur persönlicher Einschätzungen", "Sie sind überflüssig, da Sinne immer genau genug sind", "Sie funktionieren nur bei bestimmten Personen" }, "Sie liefern genaue, überprüfbare Werte statt nur persönlicher Einschätzungen",
            "Messgeräte liefern objektive, überprüfbare Werte, während Sinneswahrnehmung subjektiv und ungenau sein kann."),
        ("Wie liest man ein Flüssigkeitsthermometer richtig ab?", new[] { "Auf Augenhöhe, an der Oberkante der Flüssigkeitssäule", "Von oben herabschauend", "Die Zahl spielt keine Rolle" }, "Auf Augenhöhe, an der Oberkante der Flüssigkeitssäule",
            "Ein Flüssigkeitsthermometer liest man auf Augenhöhe an der Oberkante der Flüssigkeitssäule ab, um Ablesefehler zu vermeiden."),
        ("Was zeigt eine Küchenwaage typischerweise an?", new[] { "Das Gewicht von Zutaten in Gramm oder Kilogramm", "Die Temperatur des Ofens", "Die Kochzeit" }, "Das Gewicht von Zutaten in Gramm oder Kilogramm",
            "Eine Küchenwaage zeigt das Gewicht von Zutaten an, meist in Gramm oder Kilogramm."),
        ("Warum ist eine einheitliche Maßeinheit (z.B. Meter, Kilogramm) wichtig?", new[] { "Damit Messwerte überall vergleichbar sind", "Damit jeder eine eigene Einheit erfinden kann", "Einheiten sind nicht wichtig" }, "Damit Messwerte überall vergleichbar sind",
            "Einheitliche Maßeinheiten sorgen dafür, dass Messwerte überall verstanden und verglichen werden können."),
        ("Was passiert mit der Quecksilber- oder Alkoholsäule in einem Thermometer bei steigender Temperatur?", new[] { "Sie dehnt sich aus und steigt", "Sie schrumpft", "Sie bleibt immer gleich" }, "Sie dehnt sich aus und steigt",
            "Bei steigender Temperatur dehnt sich die Flüssigkeit im Thermometer aus und steigt in der Röhre."),
        ("Was unterscheidet ein digitales Thermometer von einem klassischen Flüssigkeitsthermometer?", new[] { "Es zeigt die Temperatur als Zahl auf einem Display an", "Es misst überhaupt keine Temperatur", "Es funktioniert nur unter Wasser" }, "Es zeigt die Temperatur als Zahl auf einem Display an",
            "Digitale Thermometer zeigen den gemessenen Wert direkt als Zahl auf einem Display an."),
        ("Warum kann man sich bei der Einschätzung von Gewicht allein mit der Hand leicht täuschen?", new[] { "Weil das Gefühl für Gewicht subjektiv und ungenau ist", "Weil die Hand immer exakt genau wiegt", "Weil Gewicht sich ständig verändert" }, "Weil das Gefühl für Gewicht subjektiv und ungenau ist",
            "Das Gewichtsgefühl mit der Hand ist subjektiv - eine Waage liefert einen genauen, objektiven Wert."),
        ("Was ist ein Vorteil des genauen Messens gegenüber dem bloßen Schätzen im Alltag?", new[] { "Genauere, verlässlichere und vergleichbare Ergebnisse", "Schätzen ist immer genauer als Messen", "Es gibt keinen Unterschied" }, "Genauere, verlässlichere und vergleichbare Ergebnisse",
            "Genaues Messen liefert verlässlichere und vergleichbare Ergebnisse als bloßes Schätzen.")
    };

    private static QuizQuestion MessenUndSinne(Random r)
    {
        var f = MessenSinneListe[r.Next(MessenSinneListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Von den Sinnen zum Messen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Thermometer misst Temperatur (°C), Waage misst Gewicht (kg). Sinneswahrnehmung ist subjektiv, Messgeräte liefern objektive Werte."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] OptikWeltraumListe =
    {
        ("Was befindet sich im Zentrum unseres Sonnensystems?", new[] { "Die Sonne", "Die Erde", "Der Mond" }, "Die Sonne",
            "Die Sonne steht im Zentrum unseres Sonnensystems, alle Planeten kreisen um sie."),
        ("Wie viele Planeten hat unser Sonnensystem (nach aktueller Definition)?", new[] { "8", "9", "12" }, "8",
            "Nach der aktuellen Definition (seit 2006) hat unser Sonnensystem 8 Planeten, Pluto zählt seitdem als Zwergplanet."),
        ("Wie entstehen die Jahreszeiten auf der Erde?", new[] { "Durch die geneigte Erdachse und den Umlauf der Erde um die Sonne", "Durch den Mond", "Durch das Wetter allein" }, "Durch die geneigte Erdachse und den Umlauf der Erde um die Sonne",
            "Die geneigte Erdachse sorgt beim Umlauf um die Sonne dafür, dass Nord- und Südhalbkugel wechselnd mehr oder weniger Sonnenlicht erhalten."),
        ("Was ist eine Sonnenfinsternis?", new[] { "Der Mond schiebt sich zwischen Erde und Sonne", "Die Erde schiebt sich zwischen Sonne und Mond", "Die Sonne verschwindet für immer" }, "Der Mond schiebt sich zwischen Erde und Sonne",
            "Bei einer Sonnenfinsternis schiebt sich der Mond zwischen Erde und Sonne und verdeckt sie teilweise oder ganz."),
        ("Was ist eine Mondfinsternis?", new[] { "Die Erde schiebt sich zwischen Sonne und Mond", "Der Mond schiebt sich zwischen Erde und Sonne", "Der Mond verschwindet für immer" }, "Die Erde schiebt sich zwischen Sonne und Mond",
            "Bei einer Mondfinsternis schiebt sich die Erde zwischen Sonne und Mond, wodurch der Mond im Erdschatten liegt."),
        ("Wofür nutzt man eine Lupe?", new[] { "Um kleine Dinge vergrößert zu sehen", "Um weit entfernte Dinge näher zu sehen", "Um im Dunkeln zu sehen" }, "Um kleine Dinge vergrößert zu sehen",
            "Eine Lupe vergrößert kleine, nahe Dinge."),
        ("Wofür nutzt man ein Fernglas?", new[] { "Um weit entfernte Dinge näher/größer zu sehen", "Um sehr kleine Dinge zu vergrößern", "Um Temperaturen zu messen" }, "Um weit entfernte Dinge näher/größer zu sehen",
            "Ein Fernglas holt weit entfernte Objekte optisch näher heran."),
        ("Wofür nutzt man ein Mikroskop?", new[] { "Um sehr kleine Dinge (z.B. Zellen) stark vergrößert zu sehen", "Um weit entfernte Sterne zu sehen", "Um Gewicht zu messen" }, "Um sehr kleine Dinge (z.B. Zellen) stark vergrößert zu sehen",
            "Ein Mikroskop vergrößert sehr kleine Dinge wie Zellen so stark, dass sie sichtbar werden."),
        ("Was ist ein Schatten?", new[] { "Ein dunkler Bereich, der entsteht, wenn Licht von einem Gegenstand blockiert wird", "Ein besonders heller Lichtstrahl", "Eine Art Spiegel" }, "Ein dunkler Bereich, der entsteht, wenn Licht von einem Gegenstand blockiert wird",
            "Ein Schatten entsteht, wenn ein Gegenstand Licht blockiert und dahinter ein dunkler Bereich bleibt."),
        ("Was passiert mit Licht, wenn es auf eine glatte, glänzende Oberfläche trifft?", new[] { "Es wird reflektiert (zurückgeworfen)", "Es verschwindet komplett", "Es wird immer absorbiert" }, "Es wird reflektiert (zurückgeworfen)",
            "Glatte, glänzende Oberflächen reflektieren Licht, wie z.B. ein Spiegel."),
        ("Was ist ein Kristall (im Mikrokosmos betrachtet)?", new[] { "Ein Stoff mit regelmäßig angeordneten kleinsten Teilchen", "Eine Flüssigkeit ohne feste Form", "Ein Gas" }, "Ein Stoff mit regelmäßig angeordneten kleinsten Teilchen",
            "In einem Kristall sind die kleinsten Teilchen des Stoffes regelmäßig und geordnet angeordnet."),
        ("Was kann man mit einem Mikroskop bei Pflanzen betrachten?", new[] { "Pflanzenzellen", "Ganze Planeten", "Sterne" }, "Pflanzenzellen",
            "Mit einem Mikroskop lassen sich einzelne Pflanzenzellen sichtbar machen."),
        ("Wie lange braucht der Mond ungefähr, um einmal um die Erde zu kreisen?", new[] { "Etwa einen Monat", "Etwa einen Tag", "Etwa ein Jahr" }, "Etwa einen Monat",
            "Der Mond umkreist die Erde einmal in etwa einem Monat (ca. 27-29 Tagen)."),
        ("Wie lange braucht die Erde, um einmal um die Sonne zu kreisen?", new[] { "Etwa ein Jahr", "Etwa einen Monat", "Etwa einen Tag" }, "Etwa ein Jahr",
            "Die Erde umkreist die Sonne einmal in etwa einem Jahr (365 Tagen)."),
        ("Warum sehen wir den Mond manchmal als Sichel und manchmal als volle Scheibe?", new[] { "Wegen der unterschiedlichen Mondphasen, je nach Stellung zur Sonne", "Weil sich der Mond ständig verformt", "Weil der Mond manchmal verschwindet" }, "Wegen der unterschiedlichen Mondphasen, je nach Stellung zur Sonne",
            "Je nachdem, wie Sonne, Erde und Mond zueinander stehen, sehen wir unterschiedlich viel der beleuchteten Mondseite - das sind die Mondphasen."),
        ("Was unterscheidet den Makrokosmos vom Mikrokosmos?", new[] { "Der Makrokosmos umfasst sehr Großes (z.B. das Weltall), der Mikrokosmos sehr Kleines (z.B. Zellen)", "Beide Begriffe bedeuten dasselbe", "Mikrokosmos beschreibt nur das Weltall" }, "Der Makrokosmos umfasst sehr Großes (z.B. das Weltall), der Mikrokosmos sehr Kleines (z.B. Zellen)",
            "Makrokosmos bezeichnet die Welt des sehr Großen (z.B. Weltall), Mikrokosmos die Welt des sehr Kleinen (z.B. Zellen, Kristalle)."),
        ("Warum kann man mit bloßem Auge keine einzelnen Zellen erkennen?", new[] { "Weil sie viel zu klein sind", "Weil Zellen unsichtbar sind, auch mit Mikroskop", "Weil Zellen sich zu schnell bewegen" }, "Weil sie viel zu klein sind",
            "Zellen sind so klein, dass man sie ohne Vergrößerung (z.B. durch ein Mikroskop) nicht erkennen kann."),
        ("Was passiert, wenn Licht auf einen Spiegel trifft?", new[] { "Es wird fast vollständig reflektiert", "Es wird komplett absorbiert", "Es verschwindet" }, "Es wird fast vollständig reflektiert",
            "Ein Spiegel reflektiert einfallendes Licht fast vollständig."),
        ("Warum wird ein Fernglas oft von zwei kleinen Fernrohren (Doppelfernrohr) gebildet?", new[] { "Damit man mit beiden Augen gleichzeitig und räumlich sehen kann", "Damit es schwerer wird", "Das hat keinen besonderen Grund" }, "Damit man mit beiden Augen gleichzeitig und räumlich sehen kann",
            "Zwei Fernrohre ermöglichen räumliches (beidäugiges) Sehen, wie man es auch ohne Hilfsmittel gewohnt ist."),
        ("Was zeigt uns die Betrachtung des Sonnensystems über unseren Platz im Weltall?", new[] { "Die Erde ist einer von mehreren Planeten, die die Sonne umkreisen", "Die Erde ist das Zentrum des gesamten Universums", "Die Erde ist der einzige Himmelskörper im Weltall" }, "Die Erde ist einer von mehreren Planeten, die die Sonne umkreisen",
            "Die Erde ist einer von acht Planeten, die die Sonne umkreisen - nicht das Zentrum des Universums.")
    };

    private static QuizQuestion OptikUndWeltraum(Random r)
    {
        var f = OptikWeltraumListe[r.Next(OptikWeltraumListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Welt des Großen – Welt des Kleinen (Optik und Weltraum)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Lupe vergrößert Kleines aus der Nähe, Fernglas holt Entferntes heran, Mikroskop zeigt Zellen. Die Erde umkreist die Sonne, der Mond die Erde."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BewegungBionikListe =
    {
        ("Was bedeutet \"Bionik\"?", new[] { "Die Übertragung von Vorbildern aus der Natur auf Technik", "Ein anderes Wort für Biologie", "Eine Art von Chemie" }, "Die Übertragung von Vorbildern aus der Natur auf Technik",
            "Bionik überträgt Prinzipien und Vorbilder aus der Natur auf technische Erfindungen."),
        ("Welches Vorbild aus der Natur inspirierte den Bau von Flugzeugen?", new[] { "Der Vogelflug", "Das Schwimmen von Fischen", "Das Laufen von Tieren" }, "Der Vogelflug",
            "Der Vogelflug diente als Vorbild für die Entwicklung von Flugzeugen und Tragflächen."),
        ("Was hilft Fischen, sich besonders schnell und mühelos durchs Wasser zu bewegen?", new[] { "Ihre stromlinienförmige Körperform", "Ihre eckige Körperform", "Ihr Fell" }, "Ihre stromlinienförmige Körperform",
            "Die stromlinienförmige Körperform verringert den Wasserwiderstand beim Schwimmen."),
        ("Was versteht man unter \"Stromlinienform\"?", new[] { "Eine besonders windschlüpfrige, den Widerstand verringernde Form", "Eine besonders eckige, raue Form", "Eine Form nur bei Vögeln" }, "Eine besonders windschlüpfrige, den Widerstand verringernde Form",
            "Eine stromlinienförmige Form verringert den Luft- oder Wasserwiderstand bei Bewegung."),
        ("Wie beeinflusst Reibung die Geschwindigkeit eines bewegten Körpers?", new[] { "Reibung bremst die Bewegung", "Reibung beschleunigt die Bewegung immer", "Reibung hat keinerlei Einfluss" }, "Reibung bremst die Bewegung",
            "Reibung wirkt einer Bewegung entgegen und bremst sie ab."),
        ("Warum sind Autos und Flugzeuge oft stromlinienförmig gebaut?", new[] { "Um den Luftwiderstand zu verringern und Energie zu sparen", "Um mehr Luftwiderstand zu erzeugen", "Aus rein ästhetischen Gründen ohne technischen Nutzen" }, "Um den Luftwiderstand zu verringern und Energie zu sparen",
            "Eine stromlinienförmige Bauweise verringert den Luftwiderstand und spart dadurch Energie."),
        ("Welche Bewegungsart nutzen Vögel hauptsächlich?", new[] { "Fliegen", "Schwimmen", "Kriechen" }, "Fliegen",
            "Vögel bewegen sich hauptsächlich fliegend fort."),
        ("Welche Bewegungsart nutzen Fische hauptsächlich?", new[] { "Schwimmen", "Fliegen", "Springen" }, "Schwimmen",
            "Fische bewegen sich hauptsächlich schwimmend fort."),
        ("Was haben Flugzeugtragflächen und Vogelflügel gemeinsam?", new[] { "Beide erzeugen Auftrieb durch ihre besondere Form", "Beide haben Federn", "Beide funktionieren völlig unterschiedlich, ohne jede Gemeinsamkeit" }, "Beide erzeugen Auftrieb durch ihre besondere Form",
            "Sowohl Vogelflügel als auch Flugzeugtragflächen sind so geformt, dass sie Auftrieb erzeugen."),
        ("Was bedeutet \"Auftrieb\" beim Fliegen?", new[] { "Die Kraft, die einen Körper in der Luft nach oben trägt", "Die Kraft, die einen Körper nach unten zieht", "Ein anderes Wort für Reibung" }, "Die Kraft, die einen Körper in der Luft nach oben trägt",
            "Auftrieb ist die Kraft, die einen Körper in der Luft (oder im Wasser) nach oben trägt."),
        ("Warum haben viele schnell schwimmende Tiere (z.B. Delfine) eine glatte Haut?", new[] { "Um den Wasserwiderstand beim Schwimmen zu verringern", "Um mehr Widerstand zu erzeugen", "Aus rein optischen Gründen" }, "Um den Wasserwiderstand beim Schwimmen zu verringern",
            "Eine glatte Haut verringert den Wasserwiderstand und ermöglicht schnelleres Schwimmen."),
        ("Was ist ein Beispiel für Bionik im Alltag?", new[] { "Der Klettverschluss, inspiriert von Kletten-Pflanzensamen", "Ein normaler Reißverschluss ohne Vorbild", "Ein Radiergummi" }, "Der Klettverschluss, inspiriert von Kletten-Pflanzensamen",
            "Der Klettverschluss wurde nach dem Vorbild von Klettensamen entwickelt, die sich an Fell und Kleidung festhaken."),
        ("Wie bewegen sich die meisten Landtiere fort?", new[] { "Durch Laufen mit Beinen", "Durch Fliegen", "Durch Schwimmen mit Flossen" }, "Durch Laufen mit Beinen",
            "Die meisten Landtiere bewegen sich laufend mit Beinen fort."),
        ("Warum ist die Fortbewegung im Wasser für viele Tiere anstrengender als an Land?", new[] { "Wasser hat einen höheren Widerstand als Luft", "Wasser hat keinerlei Widerstand", "Luft hat einen höheren Widerstand als Wasser" }, "Wasser hat einen höheren Widerstand als Luft",
            "Wasser ist dichter als Luft und setzt der Bewegung deshalb einen größeren Widerstand entgegen."),
        ("Was passiert mit der Geschwindigkeit eines Radfahrers durch Gegenwind?", new[] { "Sie wird durch den erhöhten Luftwiderstand verringert", "Sie wird automatisch erhöht", "Gegenwind hat keinen Einfluss" }, "Sie wird durch den erhöhten Luftwiderstand verringert",
            "Gegenwind erhöht den Luftwiderstand und verringert dadurch die Geschwindigkeit."),
        ("Wie hilft eine gebückte Haltung Radfahrenden bei hohen Geschwindigkeiten?", new[] { "Sie verringert den Luftwiderstand", "Sie erhöht den Luftwiderstand", "Sie hat keinerlei Effekt" }, "Sie verringert den Luftwiderstand",
            "Eine gebückte, stromlinienförmigere Haltung verringert den Luftwiderstand."),
        ("Was zeigt der Vergleich zwischen Vogelflug und Flugzeug in der Bionik?", new[] { "Wie Technik von natürlichen Vorbildern lernen kann", "Dass Natur und Technik nichts gemeinsam haben", "Dass Vögel wie Flugzeuge aus Metall bestehen" }, "Wie Technik von natürlichen Vorbildern lernen kann",
            "Der Vergleich zeigt, wie technische Erfindungen von natürlichen Vorbildern lernen und sie nachahmen können."),
        ("Warum sind haifischhaut-inspirierte Schwimmanzüge für den Leistungssport entwickelt worden?", new[] { "Um den Wasserwiderstand für Schwimmer zu verringern", "Um Schwimmer langsamer zu machen", "Nur aus modischen Gründen" }, "Um den Wasserwiderstand für Schwimmer zu verringern",
            "Die Struktur der Haifischhaut diente als Vorbild, um den Wasserwiderstand von Schwimmanzügen zu verringern."),
        ("Was versteht man unter Reibungskraft?", new[] { "Eine Kraft, die einer Bewegung entgegenwirkt", "Eine Kraft, die eine Bewegung immer beschleunigt", "Eine Kraft, die nur im Wasser existiert" }, "Eine Kraft, die einer Bewegung entgegenwirkt",
            "Reibungskraft wirkt einer Bewegung entgegen und bremst sie."),
        ("Warum ist die Erforschung der Natur für Erfinder und Ingenieure oft hilfreich (Bionik)?", new[] { "Die Natur hat über Jahrmillionen effiziente Lösungen für Bewegung und Form entwickelt", "Die Natur bietet keinerlei nützliche Vorbilder", "Nur Zufall führt zu technischen Erfindungen" }, "Die Natur hat über Jahrmillionen effiziente Lösungen für Bewegung und Form entwickelt",
            "Die Natur hat durch Evolution über Jahrmillionen sehr effiziente Lösungen entwickelt, von denen Technik lernen kann.")
    };

    private static QuizQuestion BewegungUndBionik(Random r)
    {
        var f = BewegungBionikListe[r.Next(BewegungBionikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Bewegung zu Wasser, zu Lande und in der Luft (Bionik)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bionik überträgt Vorbilder aus der Natur auf Technik. Stromlinienform und glatte Oberflächen verringern den Widerstand bei Bewegung."
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
            "Gespeicherte Energie aufgrund der Lage/Höhe eines Körpers", "Ein angehobener Ball hat potenzielle Energie - je höher er ist, desto mehr Energie kann beim Fallen frei werden."),
        ("Welche Energieform ist in einer Batterie gespeichert?", new[] { "Chemische Energie", "Kernenergie", "Bewegungsenergie" }, "Chemische Energie",
            "Batterien speichern Energie in chemischer Form und wandeln sie bei Bedarf in elektrische Energie um."),
        ("In welche Energieformen wird chemische Energie beim Verbrennen von Holz hauptsächlich umgewandelt?", new[] { "Wärmeenergie und Lichtenergie", "Nur in elektrische Energie", "Nur in Bewegungsenergie" }, "Wärmeenergie und Lichtenergie",
            "Beim Verbrennen wird die im Holz gespeicherte chemische Energie vor allem in Wärme und Licht umgewandelt."),
        ("Was misst die physikalische Einheit \"Joule\"?", new[] { "Energie", "Zeit", "Temperatur" }, "Energie",
            "Joule ist die SI-Einheit für Energie (bzw. Arbeit) - egal ob Bewegungs-, Höhen- oder Wärmeenergie."),
        ("Was misst die physikalische Einheit \"Watt\"?", new[] { "Leistung (Energie pro Zeit)", "Nur die Masse eines Körpers", "Nur die Länge eines Kabels" }, "Leistung (Energie pro Zeit)",
            "Watt gibt an, wie viel Energie pro Zeiteinheit umgesetzt wird - also die Leistung."),
        ("Was ist der Unterschied zwischen Energie und Leistung (vereinfacht)?", new[] { "Leistung gibt an, wie schnell Energie umgesetzt wird", "Beide Begriffe bedeuten exakt dasselbe", "Energie wird in Sekunden gemessen, Leistung in Metern" }, "Leistung gibt an, wie schnell Energie umgesetzt wird",
            "Energie beschreibt die Gesamtmenge, Leistung beschreibt, wie schnell diese Energiemenge umgesetzt oder übertragen wird."),
        ("Was bedeutet der \"Wirkungsgrad\" einer Maschine?", new[] { "Der Anteil der eingesetzten Energie, der tatsächlich nutzbar umgesetzt wird", "Die maximale Geschwindigkeit einer Maschine", "Die Farbe, in der eine Maschine lackiert ist" }, "Der Anteil der eingesetzten Energie, der tatsächlich nutzbar umgesetzt wird",
            "Ein Teil der eingesetzten Energie geht z.B. als Wärme durch Reibung verloren - der Wirkungsgrad zeigt, wie viel tatsächlich nutzbar bleibt."),
        ("Warum gelten fossile Energieträger wie Kohle und Öl als nicht erneuerbar?", new[] { "Sie brauchen Millionen Jahre zur Entstehung und sind nur begrenzt vorhanden", "Sie entstehen jeden Tag neu", "Sie sind unbegrenzt verfügbar" }, "Sie brauchen Millionen Jahre zur Entstehung und sind nur begrenzt vorhanden",
            "Fossile Energieträger entstanden über Jahrmillionen aus abgestorbenen Organismen und werden schneller verbraucht, als sie neu entstehen können."),
        ("Wie wandelt eine Solarzelle Sonnenlicht um?", new[] { "Sie wandelt Lichtenergie direkt in elektrische Energie um", "Sie speichert Sonnenlicht als Wärme in Wasser", "Sie erzeugt aus Sonnenlicht Bewegungsenergie in einem Motor" }, "Sie wandelt Lichtenergie direkt in elektrische Energie um",
            "Solarzellen nutzen den photoelektrischen Effekt, um Lichtenergie direkt in elektrische Energie umzuwandeln."),
        ("Wie erzeugt eine Windkraftanlage elektrische Energie?", new[] { "Wind dreht die Rotorblätter, die einen Generator antreiben", "Sie speichert Wind direkt als elektrische Ladung", "Sie wandelt Sonnenlicht in Strom um" }, "Wind dreht die Rotorblätter, die einen Generator antreiben",
            "Die Bewegungsenergie des Windes dreht die Rotorblätter, die über einen Generator elektrische Energie erzeugen."),
        ("Was passiert bei einem (reibungsfrei gedachten) schwingenden Pendel in Bezug auf Energie?", new[] { "Es wandelt ständig zwischen Höhenenergie und Bewegungsenergie um", "Die Energie verschwindet bei jeder Schwingung ein Stück", "Es entsteht laufend neue zusätzliche Energie" }, "Es wandelt ständig zwischen Höhenenergie und Bewegungsenergie um",
            "Am höchsten Punkt hat das Pendel die meiste Höhenenergie, im tiefsten Punkt die meiste Bewegungsenergie - die Gesamtenergie bleibt dabei erhalten."),
        ("Warum kommt ein echtes Pendel nach einiger Zeit zur Ruhe, obwohl Energie eigentlich erhalten bleibt?", new[] { "Reibung (Luftwiderstand, Aufhängung) wandelt Bewegungsenergie in Wärme um", "Die Energieerhaltung gilt für Pendel nicht", "Das Pendel verliert einfach Masse" }, "Reibung (Luftwiderstand, Aufhängung) wandelt Bewegungsenergie in Wärme um",
            "Die Energie geht nicht verloren, sondern wandelt sich durch Reibung in nicht mehr nutzbare Wärmeenergie um."),
        ("Welche Energieform steckt hauptsächlich in Lebensmitteln, die wir essen?", new[] { "Chemische Energie", "Elektrische Energie", "Kernenergie" }, "Chemische Energie",
            "Der Körper gewinnt aus der chemischen Energie der Nahrung die Energie für Bewegung, Wärme und alle Lebensvorgänge."),
        ("In welche Energieform wandeln Muskeln chemische Energie beim Bewegen hauptsächlich um?", new[] { "Bewegungsenergie und Wärme", "Ausschließlich Lichtenergie", "Ausschließlich elektrische Energie" }, "Bewegungsenergie und Wärme",
            "Muskeln wandeln die chemische Energie aus der Nahrung überwiegend in Bewegung und Wärme um - deshalb wird uns beim Sport warm."),
        ("In welcher Einheit wird der Stromverbrauch eines Haushalts meist abgerechnet?", new[] { "Kilowattstunde (kWh)", "Grad Celsius", "Meter pro Sekunde" }, "Kilowattstunde (kWh)",
            "Der Stromverbrauch wird üblicherweise in Kilowattstunden gemessen und auf der Stromrechnung abgerechnet."),
        ("Was speichert eine wiederaufladbare Batterie (Akku) beim Laden?", new[] { "Elektrische Energie wird in chemische Energie umgewandelt und gespeichert", "Sie speichert direkt Bewegungsenergie", "Sie speichert Licht als Farbe" }, "Elektrische Energie wird in chemische Energie umgewandelt und gespeichert",
            "Beim Laden wandelt ein Akku zugeführte elektrische Energie in chemische Energie um, die er später wieder als Strom abgeben kann.")
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
            "Der Körper bleibt durch Trägheit zunächst in Ruhe, während sich der Bus schon bewegt", "Das ist das Trägheitsgesetz in Aktion: Der Körper \"will\" seinen Bewegungszustand (Ruhe) zunächst beibehalten."),
        ("In welcher Einheit wird Kraft gemessen?", new[] { "Newton (N)", "Joule (J)", "Volt (V)" }, "Newton (N)",
            "Die Einheit der Kraft ist nach Isaac Newton benannt und wird mit dem Buchstaben N abgekürzt."),
        ("Was ist der Unterschied zwischen Masse und Gewichtskraft?", new[] { "Masse ist die Menge an Materie, Gewichtskraft ist die Kraft, mit der die Erde daran zieht", "Beide Begriffe bedeuten exakt dasselbe", "Masse gibt es nur im Weltall, Gewichtskraft nur auf der Erde" }, "Masse ist die Menge an Materie, Gewichtskraft ist die Kraft, mit der die Erde daran zieht",
            "Die Masse eines Körpers bleibt überall gleich, seine Gewichtskraft hängt dagegen von der jeweiligen Gravitation ab."),
        ("Warum wiegt ein Astronaut auf dem Mond weniger als auf der Erde, obwohl seine Masse gleich bleibt?", new[] { "Die Anziehungskraft (Gravitation) des Mondes ist schwächer als die der Erde", "Der Astronaut verliert auf dem Mond tatsächlich Masse", "Auf dem Mond gibt es überhaupt keine Schwerkraft" }, "Die Anziehungskraft (Gravitation) des Mondes ist schwächer als die der Erde",
            "Die Gewichtskraft hängt von der Gravitation ab - der Mond zieht mit weniger Kraft, deshalb wiegt derselbe Körper dort weniger."),
        ("Was passiert im Vakuum (ganz ohne Luftwiderstand), wenn eine Feder und ein Hammer gleichzeitig fallen gelassen werden?", new[] { "Sie fallen gleich schnell", "Der Hammer fällt deutlich schneller", "Die Feder fällt deutlich schneller" }, "Sie fallen gleich schnell",
            "Ohne Luftwiderstand fallen alle Körper unabhängig von ihrer Masse gleich schnell - ein berühmtes Experiment wurde sogar auf dem Mond gezeigt."),
        ("Was ist die Reibungskraft?", new[] { "Eine Kraft, die der Bewegung zwischen zwei sich berührenden Oberflächen entgegenwirkt", "Eine Kraft, die Bewegung immer beschleunigt", "Eine Kraft, die nur bei Magneten auftritt" }, "Eine Kraft, die der Bewegung zwischen zwei sich berührenden Oberflächen entgegenwirkt",
            "Reibung bremst Bewegung zwischen zwei Oberflächen ab und wandelt Bewegungsenergie in Wärme um."),
        ("Warum ist es schwerer, einen vollen Einkaufswagen anzuschieben als einen leeren?", new[] { "Ein voller Wagen hat mehr Masse und damit mehr Trägheit", "Volle Wagen haben grundsätzlich kaputte Räder", "Es ist tatsächlich genau gleich schwer" }, "Ein voller Wagen hat mehr Masse und damit mehr Trägheit",
            "Nach F = m·a braucht ein Körper mit größerer Masse bei gleicher gewünschter Beschleunigung eine größere Kraft."),
        ("Wie funktioniert der Antrieb einer Rakete nach Newtons drittem Gesetz?", new[] { "Sie stößt Gas nach hinten aus und wird dadurch nach vorne beschleunigt", "Sie wird von der Erde einfach nach oben gezogen", "Sie fliegt nur wegen des Winds"}, "Sie stößt Gas nach hinten aus und wird dadurch nach vorne beschleunigt",
            "Die Rakete stößt Verbrennungsgase mit hoher Kraft nach hinten aus - die Gegenkraft (Rückstoß) beschleunigt die Rakete nach vorne."),
        ("Warum sind Anschnallgurte im Auto wichtig, wenn ein Auto abrupt bremst?", new[] { "Der Körper würde sich wegen der Trägheit sonst weiter nach vorne bewegen", "Gurte machen das Auto automatisch schneller", "Ohne Gurt bleibt der Körper sofort stehen" }, "Der Körper würde sich wegen der Trägheit sonst weiter nach vorne bewegen",
            "Bremst das Auto, will der Körper laut Trägheitsgesetz seine Bewegung fortsetzen - der Gurt hält ihn zurück und verhindert Verletzungen."),
        ("Was passiert mit der Bewegungsrichtung eines Balls, wenn keine Kraft (z.B. Reibung) auf ihn wirkt?", new[] { "Er bewegt sich geradeaus mit gleichbleibender Geschwindigkeit weiter", "Er bleibt sofort automatisch stehen", "Er ändert von selbst seine Richtung" }, "Er bewegt sich geradeaus mit gleichbleibender Geschwindigkeit weiter",
            "Ohne einwirkende Kraft bewegt sich ein Körper laut Trägheitsgesetz geradlinig mit konstanter Geschwindigkeit weiter."),
        ("Wie nennt man die Kraft, mit der die Erde jeden Körper nach unten zieht?", new[] { "Gravitationskraft (Erdanziehungskraft)", "Reibungskraft", "Magnetkraft" }, "Gravitationskraft (Erdanziehungskraft)",
            "Die Gravitationskraft der Erde zieht alle Körper in Richtung Erdmittelpunkt - dadurch fallen losgelassene Gegenstände nach unten."),
        ("Was gilt für die Fallbeschleunigung eines Gegenstands ohne Luftwiderstand, unabhängig von seiner Masse?", new[] { "Sie ist für alle Massen gleich groß (ca. 9,81 m/s²)", "Schwerere Gegenstände fallen dabei immer schneller", "Leichtere Gegenstände fallen dabei immer schneller" }, "Sie ist für alle Massen gleich groß (ca. 9,81 m/s²)",
            "Ohne Luftwiderstand erfahren alle Körper auf der Erde dieselbe Fallbeschleunigung, unabhängig von ihrer Masse."),
        ("Wenn du beim Schwimmen das Wasser mit den Armen nach hinten drückst - was passiert mit dir laut Newtons drittem Gesetz?", new[] { "Du wirst durch die Gegenkraft des Wassers nach vorne geschoben", "Es passiert überhaupt nichts", "Du wirst automatisch nach hinten gezogen" }, "Du wirst durch die Gegenkraft des Wassers nach vorne geschoben",
            "Die Kraft, mit der du das Wasser nach hinten drückst, erzeugt eine gleich große Gegenkraft, die dich nach vorne treibt."),
        ("Was passiert, wenn beim Gehen dein Fuß nach hinten gegen den Boden drückt?", new[] { "Der Boden drückt mit gleicher Kraft zurück und schiebt dich nach vorne", "Der Boden gibt komplett ohne Gegenkraft nach", "Du bleibst dadurch automatisch stehen" }, "Der Boden drückt mit gleicher Kraft zurück und schiebt dich nach vorne",
            "Auch beim Gehen wirkt Newtons drittes Gesetz: Die Kraft deines Fußes gegen den Boden erzeugt eine Gegenkraft, die dich vorwärtsbringt."),
        ("Wer zeigte schon vor Newton, dass unterschiedlich schwere Gegenstände (ohne Luftwiderstand) gleich schnell fallen?", new[] { "Galileo Galilei", "Albert Einstein", "Marie Curie" }, "Galileo Galilei",
            "Galileo Galilei untersuchte im 16./17. Jahrhundert das freie Fallen von Körpern und legte damit den Grundstein für Newtons spätere Gesetze."),
        ("Was beschreibt der Begriff \"Trägheit\" eines Körpers?", new[] { "Seinen Widerstand gegen eine Änderung seines Bewegungszustands", "Seine Fähigkeit, sich von selbst schneller zu bewegen", "Seine Farbe und Oberflächenbeschaffenheit" }, "Seinen Widerstand gegen eine Änderung seines Bewegungszustands",
            "Trägheit beschreibt, wie stark ein Körper seinem aktuellen Bewegungszustand (Ruhe oder gleichförmige Bewegung) \"treu bleibt\", bis eine Kraft ihn ändert.")
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
            "Transformatoren", "Transformatoren nutzen Induktion, um Wechselspannung auf eine höhere oder niedrigere Spannung umzuwandeln."),
        ("Was macht ein Transformator mit Wechselspannung?", new[] { "Er wandelt sie auf eine höhere oder niedrigere Spannung um", "Er wandelt sie in Gleichspannung um, ohne die Höhe zu ändern", "Er speichert sie dauerhaft" }, "Er wandelt sie auf eine höhere oder niedrigere Spannung um",
            "Transformatoren nutzen zwei gekoppelte Spulen, um Wechselspannung je nach Windungszahl-Verhältnis herauf- oder herunterzusetzen."),
        ("Warum funktioniert ein Transformator nur mit Wechselstrom und nicht mit Gleichstrom?", new[] { "Nur ein sich änderndes Magnetfeld kann eine Spannung induzieren", "Gleichstrom ist grundsätzlich zu schwach dafür", "Transformatoren funktionieren mit beiden Stromarten exakt gleich" }, "Nur ein sich änderndes Magnetfeld kann eine Spannung induzieren",
            "Ein konstantes Magnetfeld (wie bei Gleichstrom) induziert keine Spannung - nur ein sich veränderndes Magnetfeld tut das."),
        ("Was ist ein Induktionsherd?", new[] { "Ein Herd, der Kochtöpfe direkt durch ein wechselndes Magnetfeld erhitzt", "Ein Herd, der nur mit offener Flamme funktioniert", "Ein Herd ohne jede elektrische Komponente" }, "Ein Herd, der Kochtöpfe direkt durch ein wechselndes Magnetfeld erhitzt",
            "Ein Induktionsherd erzeugt ein wechselndes Magnetfeld, das direkt im Topfboden Wirbelströme und damit Wärme erzeugt."),
        ("Warum funktioniert ein Induktionsherd nur mit bestimmten, magnetischen Kochtöpfen?", new[] { "Nur ferromagnetische Töpfe reagieren stark genug auf das Magnetfeld", "Alle Materialien funktionieren dabei exakt gleich gut", "Induktionsherde brauchen gar keine Töpfe" }, "Nur ferromagnetische Töpfe reagieren stark genug auf das Magnetfeld",
            "Töpfe aus ferromagnetischem Material (z.B. Eisen, bestimmter Edelstahl) lassen sich durch das Magnetfeld effektiv erhitzen - Aluminium oder Kupfer allein nicht."),
        ("Wie funktioniert kabelloses (induktives) Laden eines Smartphones grundsätzlich?", new[] { "Ein wechselndes Magnetfeld der Ladestation induziert Strom in einer Spule im Handy", "Das Handy lädt sich durch reine Sonneneinstrahlung auf", "Es wird ein unsichtbares Kabel durch die Luft genutzt" }, "Ein wechselndes Magnetfeld der Ladestation induziert Strom in einer Spule im Handy",
            "Induktives Laden nutzt zwei Spulen (in Ladestation und Handy) und ein wechselndes Magnetfeld, um Energie ohne Kabel zu übertragen."),
        ("Was ist der Unterschied zwischen einem Generator und einem Elektromotor (vereinfacht)?", new[] { "Ein Generator wandelt Bewegung in Strom um, ein Motor wandelt Strom in Bewegung um", "Beide Geräte sind technisch exakt identisch verwendet", "Ein Generator braucht immer Strom als Eingabe" }, "Ein Generator wandelt Bewegung in Strom um, ein Motor wandelt Strom in Bewegung um",
            "Generator und Elektromotor nutzen dasselbe physikalische Prinzip, nur in umgekehrter Richtung der Energieumwandlung."),
        ("Was treibt in einem Kraftwerk typischerweise den Generator zur Stromerzeugung an?", new[] { "Zum Beispiel eine Dampf- oder Wasserturbine", "Ausschließlich menschliche Muskelkraft", "Ein einfacher Fahrraddynamo" }, "Zum Beispiel eine Dampf- oder Wasserturbine",
            "Dampf (z.B. aus erhitztem Wasser) oder fließendes Wasser treibt Turbinen an, die wiederum den Generator drehen."),
        ("Was ist notwendig, damit durch Induktion überhaupt eine Spannung entsteht?", new[] { "Eine relative Bewegung zwischen Magnet und Spule bzw. ein sich änderndes Magnetfeld", "Ein völlig ruhender Magnet neben einer ruhenden Spule", "Nur ein einzelner, unbewegter Draht" }, "Eine relative Bewegung zwischen Magnet und Spule bzw. ein sich änderndes Magnetfeld",
            "Ohne Änderung des Magnetfelds relativ zur Spule entsteht keine Induktionsspannung."),
        ("Was passiert mit der induzierten Spannung, wenn man die Anzahl der Windungen einer Spule erhöht?", new[] { "Die induzierte Spannung wird größer", "Die induzierte Spannung wird kleiner", "Es ändert sich nichts an der Spannung" }, "Die induzierte Spannung wird größer",
            "Mehr Windungen bedeuten, dass sich das Magnetfeld öfter mit dem Draht verkettet - das erhöht die induzierte Spannung."),
        ("Was sind \"Wirbelströme\", die z.B. beim Bremsen mancher Züge genutzt werden?", new[] { "Durch ein sich änderndes Magnetfeld in einem Leiter induzierte, kreisförmige Ströme", "Ein anderes Wort für Blitze", "Strömungen von Wasser in Flüssen" }, "Durch ein sich änderndes Magnetfeld in einem Leiter induzierte, kreisförmige Ströme",
            "Wirbelströme entstehen in leitenden Materialien durch ein sich änderndes Magnetfeld und können bremsend wirken."),
        ("Wofür können Wirbelströme technisch genutzt werden?", new[] { "Zum berührungslosen Bremsen, z.B. bei manchen Zügen oder Achterbahnen", "Zum Erzeugen von Licht in Glühlampen", "Zum Speichern von Wasser" }, "Zum berührungslosen Bremsen, z.B. bei manchen Zügen oder Achterbahnen",
            "Wirbelstrombremsen nutzen die bremsende Wirkung induzierter Ströme, um berührungslos und verschleißarm zu bremsen."),
        ("Was passiert, wenn ein Magnet in Ruhe direkt neben einer ebenfalls stillstehenden Spule liegt?", new[] { "Es wird keine Spannung induziert, da sich das Magnetfeld nicht ändert", "Es wird eine sehr hohe Spannung induziert", "Die Spule beginnt sich von selbst zu drehen" }, "Es wird keine Spannung induziert, da sich das Magnetfeld nicht ändert",
            "Ohne Änderung des Magnetfelds (z.B. durch Bewegung) entsteht keine Induktionsspannung."),
        ("Wer entdeckte im 19. Jahrhundert das Prinzip der elektromagnetischen Induktion?", new[] { "Michael Faraday", "Isaac Newton", "Albert Einstein" }, "Michael Faraday",
            "Michael Faraday entdeckte 1831 die elektromagnetische Induktion - die Grundlage für Generatoren und Transformatoren."),
        ("Warum ist die Erfindung des Generators durch Induktion so bedeutend für die moderne Stromversorgung?", new[] { "Sie ermöglicht die effiziente Umwandlung von Bewegungsenergie in nutzbaren elektrischen Strom im großen Maßstab", "Sie hat gar keine praktische Bedeutung", "Sie funktioniert nur in kleinen Laboren" }, "Sie ermöglicht die effiziente Umwandlung von Bewegungsenergie in nutzbaren elektrischen Strom im großen Maßstab",
            "Fast die gesamte weltweite Stromerzeugung in Kraftwerken basiert bis heute auf dem Prinzip der elektromagnetischen Induktion."),
        ("In welchem Alltagsgerät wird Induktion genutzt, um z.B. elektrische Zahnbürsten kabellos aufzuladen?", new[] { "In induktiven Ladestationen", "In gewöhnlichen Batterien", "In herkömmlichen Steckdosenkabeln" }, "In induktiven Ladestationen",
            "Viele elektrische Zahnbürsten werden über eine induktive Ladestation ohne direkten Kabelkontakt aufgeladen.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KinematikListe =
    {
        ("Was kennzeichnet eine gleichförmige Bewegung?", new[] { "Die Geschwindigkeit bleibt über die Zeit konstant", "Die Geschwindigkeit nimmt ständig zu", "Der Körper bleibt die ganze Zeit in Ruhe" }, "Die Geschwindigkeit bleibt über die Zeit konstant",
            "Bei einer gleichförmigen Bewegung legt ein Körper in gleichen Zeitabständen immer gleich große Strecken zurück - die Geschwindigkeit ist konstant."),
        ("Was kennzeichnet eine gleichmäßig beschleunigte Bewegung?", new[] { "Die Geschwindigkeit ändert sich pro Zeiteinheit immer um denselben Betrag", "Die Geschwindigkeit bleibt während der gesamten Bewegung konstant", "Der Körper bewegt sich überhaupt nicht" }, "Die Geschwindigkeit ändert sich pro Zeiteinheit immer um denselben Betrag",
            "Bei gleichmäßig beschleunigter Bewegung ändert sich die Geschwindigkeit in gleichen Zeitabständen immer um denselben Betrag - die Beschleunigung ist konstant."),
        ("Was ist der freie Fall ein Beispiel für?", new[] { "Eine gleichmäßig beschleunigte Bewegung durch die Erdanziehungskraft", "Eine gleichförmige Bewegung mit konstanter Geschwindigkeit", "Eine Bewegung völlig ohne jede Beschleunigung" }, "Eine gleichmäßig beschleunigte Bewegung durch die Erdanziehungskraft",
            "Beim freien Fall wirkt die Erdanziehungskraft mit annähernd konstanter Fallbeschleunigung, wodurch die Geschwindigkeit gleichmäßig zunimmt."),
        ("Wie groß ist die Fallbeschleunigung g auf der Erde ungefähr?", new[] { "Etwa 9,81 m/s²", "Etwa 1 m/s²", "Etwa 100 m/s²" }, "Etwa 9,81 m/s²",
            "Die Fallbeschleunigung auf der Erdoberfläche beträgt näherungsweise 9,81 m/s² und ist für alle fallenden Körper (ohne Luftwiderstand) annähernd gleich groß."),
        ("Was passiert bei einem waagerechten Wurf mit der horizontalen und vertikalen Bewegung eines Körpers?", new[] { "Beide Bewegungen laufen unabhängig voneinander gleichzeitig ab (Überlagerung)", "Nur die horizontale Bewegung findet überhaupt statt", "Nur die vertikale Bewegung findet überhaupt statt" }, "Beide Bewegungen laufen unabhängig voneinander gleichzeitig ab (Überlagerung)",
            "Beim waagerechten Wurf überlagern sich eine gleichförmige horizontale Bewegung und eine gleichmäßig beschleunigte vertikale Fallbewegung unabhängig voneinander."),
        ("Was ist der Unterschied zwischen Momentangeschwindigkeit und Durchschnittsgeschwindigkeit?", new[] { "Momentangeschwindigkeit gilt für einen einzelnen Zeitpunkt, Durchschnittsgeschwindigkeit über ein ganzes Zeitintervall", "Beide Begriffe bedeuten exakt dasselbe", "Durchschnittsgeschwindigkeit gilt nur für einen einzigen Zeitpunkt" }, "Momentangeschwindigkeit gilt für einen einzelnen Zeitpunkt, Durchschnittsgeschwindigkeit über ein ganzes Zeitintervall",
            "Die Momentangeschwindigkeit beschreibt die Geschwindigkeit in einem bestimmten Augenblick, die Durchschnittsgeschwindigkeit den zurückgelegten Weg geteilt durch die gesamte Zeit eines Abschnitts."),
        ("Was zeigt die Steigung in einem s(t)-Diagramm (Weg-Zeit-Diagramm)?", new[] { "Die Geschwindigkeit des Körpers zu diesem Zeitpunkt", "Die Beschleunigung des Körpers", "Die zurückgelegte Gesamtstrecke unabhängig von der Zeit" }, "Die Geschwindigkeit des Körpers zu diesem Zeitpunkt",
            "In einem Weg-Zeit-Diagramm entspricht die Steigung der Kurve an einer Stelle der momentanen Geschwindigkeit des Körpers."),
        ("Was zeigt die Steigung in einem v(t)-Diagramm (Geschwindigkeit-Zeit-Diagramm)?", new[] { "Die Beschleunigung des Körpers zu diesem Zeitpunkt", "Die zurückgelegte Strecke direkt als Zahl", "Die Momentangeschwindigkeit selbst" }, "Die Beschleunigung des Körpers zu diesem Zeitpunkt",
            "In einem Geschwindigkeit-Zeit-Diagramm gibt die Steigung der Kurve die Beschleunigung an - eine waagerechte Linie bedeutet konstante Geschwindigkeit (keine Beschleunigung)."),
        ("Was zeigt die Fläche unter der Kurve in einem v(t)-Diagramm?", new[] { "Den in diesem Zeitraum zurückgelegten Weg", "Die Beschleunigung des Körpers", "Die Masse des bewegten Körpers" }, "Den in diesem Zeitraum zurückgelegten Weg",
            "Die Fläche unter der Kurve eines Geschwindigkeit-Zeit-Diagramms entspricht dem in diesem Zeitintervall zurückgelegten Weg."),
        ("Was ist der Reaktionsweg beim Bremsen eines Fahrzeugs?", new[] { "Die Strecke, die das Fahrzeug zurücklegt, bevor die Fahrerin oder der Fahrer überhaupt zu bremsen beginnt", "Die Strecke, die das Fahrzeug erst nach vollständigem Stillstand zurücklegt", "Ein anderes Wort für den gesamten Anhalteweg" }, "Die Strecke, die das Fahrzeug zurücklegt, bevor die Fahrerin oder der Fahrer überhaupt zu bremsen beginnt",
            "Der Reaktionsweg ist die Strecke, die während der Reaktionszeit (Erkennen der Gefahr bis Bremsbeginn) noch mit unveränderter Geschwindigkeit zurückgelegt wird."),
        ("Was ist der Bremsweg eines Fahrzeugs?", new[] { "Die Strecke, die das Fahrzeug ab Beginn des Bremsvorgangs bis zum Stillstand zurücklegt", "Die Strecke während der reinen Reaktionszeit", "Ein anderes Wort für die Gesamtgeschwindigkeit" }, "Die Strecke, die das Fahrzeug ab Beginn des Bremsvorgangs bis zum Stillstand zurücklegt",
            "Der Bremsweg beginnt, sobald tatsächlich gebremst wird, und endet, wenn das Fahrzeug zum Stehen kommt."),
        ("Wie setzt sich der Anhalteweg eines Fahrzeugs zusammen?", new[] { "Aus der Summe von Reaktionsweg und Bremsweg", "Ausschließlich aus dem Bremsweg allein", "Ausschließlich aus dem Reaktionsweg allein" }, "Aus der Summe von Reaktionsweg und Bremsweg",
            "Der gesamte Anhalteweg ergibt sich, indem man den während der Reaktionszeit zurückgelegten Reaktionsweg zum eigentlichen Bremsweg addiert."),
        ("Was passiert mit dem Bremsweg eines Fahrzeugs ungefähr, wenn sich die Geschwindigkeit verdoppelt (bei gleicher Bremsverzögerung)?", new[] { "Der Bremsweg vervierfacht sich näherungsweise", "Der Bremsweg verdoppelt sich exakt proportional", "Der Bremsweg bleibt komplett unverändert" }, "Der Bremsweg vervierfacht sich näherungsweise",
            "Da der Bremsweg quadratisch von der Geschwindigkeit abhängt, führt eine Verdopplung der Geschwindigkeit näherungsweise zu einer Vervierfachung des Bremswegs."),
        ("Warum ist die Reaktionszeit beim Autofahren ein wichtiger Sicherheitsfaktor?", new[] { "Während der Reaktionszeit legt das Fahrzeug bereits ungebremst eine Strecke zurück", "Die Reaktionszeit hat keinerlei Einfluss auf den Anhalteweg", "Während der Reaktionszeit steht das Fahrzeug bereits vollständig still" }, "Während der Reaktionszeit legt das Fahrzeug bereits ungebremst eine Strecke zurück",
            "Bevor überhaupt gebremst wird, legt das Fahrzeug während der Reaktionszeit noch mit ursprünglicher Geschwindigkeit eine relevante Strecke zurück, die den Gesamtanhalteweg vergrößert."),
        ("Wie lässt sich die zurückgelegte Strecke bei gleichmäßig beschleunigter Bewegung aus einem Zeit-Beschleunigungs-Zusammenhang berechnen (vereinfachtes Prinzip)?", new[] { "Über eine quadratische Beziehung zwischen zurückgelegtem Weg und vergangener Zeit", "Über eine rein lineare Beziehung wie bei der gleichförmigen Bewegung", "Weg und Zeit stehen bei beschleunigter Bewegung in keinerlei Beziehung" }, "Über eine quadratische Beziehung zwischen zurückgelegtem Weg und vergangener Zeit",
            "Bei gleichmäßig beschleunigter Bewegung (wie beim freien Fall) wächst der zurückgelegte Weg quadratisch mit der Zeit, nicht linear wie bei der gleichförmigen Bewegung."),
        ("Was bedeutet es, wenn ein v(t)-Diagramm eine horizontale (waagerechte) Linie zeigt?", new[] { "Der Körper bewegt sich mit konstanter Geschwindigkeit (keine Beschleunigung)", "Der Körper wird gleichmäßig immer schneller", "Der Körper befindet sich in völligem Stillstand" }, "Der Körper bewegt sich mit konstanter Geschwindigkeit (keine Beschleunigung)",
            "Eine waagerechte Linie im Geschwindigkeit-Zeit-Diagramm zeigt an, dass sich die Geschwindigkeit nicht ändert - es liegt also keine Beschleunigung vor."),
        ("Was bedeutet es, wenn ein v(t)-Diagramm eine Linie mit konstanter, positiver Steigung zeigt?", new[] { "Der Körper wird gleichmäßig beschleunigt (konstante Beschleunigung)", "Der Körper bewegt sich mit konstanter Geschwindigkeit", "Der Körper bewegt sich überhaupt nicht" }, "Der Körper wird gleichmäßig beschleunigt (konstante Beschleunigung)",
            "Eine Gerade mit konstanter Steigung im v(t)-Diagramm zeigt eine gleichmäßig beschleunigte Bewegung mit konstanter Beschleunigung an."),
        ("Warum ist der waagerechte Wurf ein gutes Beispiel für die Überlagerung zweier unabhängiger Bewegungen?", new[] { "Die gleichförmige horizontale Bewegung und die beschleunigte vertikale Fallbewegung beeinflussen sich gegenseitig nicht", "Horizontale und vertikale Bewegung sind beim waagerechten Wurf komplett identisch", "Beim waagerechten Wurf findet überhaupt keine horizontale Bewegung statt" }, "Die gleichförmige horizontale Bewegung und die beschleunigte vertikale Fallbewegung beeinflussen sich gegenseitig nicht",
            "Horizontale (gleichförmige) und vertikale (beschleunigte) Teilbewegung eines waagerechten Wurfs verlaufen physikalisch unabhängig voneinander und lassen sich getrennt berechnen."),
        ("Was passiert mit der Fallgeschwindigkeit eines Körpers beim freien Fall ohne Luftwiderstand mit der Zeit?", new[] { "Sie nimmt gleichmäßig, linear mit der Zeit zu", "Sie bleibt während des gesamten Falls konstant", "Sie nimmt mit der Zeit immer weiter ab" }, "Sie nimmt gleichmäßig, linear mit der Zeit zu",
            "Beim freien Fall (ohne Luftwiderstand) nimmt die Geschwindigkeit durch die konstante Fallbeschleunigung linear mit der Zeit zu."),
        ("Warum fallen eine Feder und ein Stein im Vakuum (ohne Luftwiderstand) gleich schnell?", new[] { "Ohne Luftwiderstand hängt die Fallbeschleunigung nicht von der Masse des Körpers ab", "Schwerere Körper fallen im Vakuum immer automatisch schneller", "Federn fallen im Vakuum grundsätzlich gar nicht" }, "Ohne Luftwiderstand hängt die Fallbeschleunigung nicht von der Masse des Körpers ab",
            "Ohne den bremsenden Effekt des Luftwiderstands erfahren alle Körper unabhängig von ihrer Masse dieselbe Fallbeschleunigung und fallen deshalb gleich schnell.")
    };

    private static QuizQuestion Kinematik(Random r)
    {
        var f = KinematikListe[r.Next(KinematikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gleichförmige und beschleunigte Bewegungen (Kinematik)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bei gleichförmiger Bewegung bleibt die Geschwindigkeit konstant, bei gleichmäßig beschleunigter Bewegung ändert sie sich stetig; der Anhalteweg ist Reaktionsweg plus Bremsweg."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RadioaktivitaetListe =
    {
        ("Was beschreibt das Kern-Hülle-Modell eines Atoms?", new[] { "Ein kleiner, positiv geladener Atomkern ist von einer Hülle aus Elektronen umgeben", "Ein Atom besteht nur aus einer einzigen, homogenen Masse", "Der Atomkern ist größer als die gesamte Elektronenhülle" }, "Ein kleiner, positiv geladener Atomkern ist von einer Hülle aus Elektronen umgeben",
            "Im Kern-Hülle-Modell konzentriert sich fast die gesamte Masse eines Atoms im winzigen, positiv geladenen Kern, umgeben von einer vergleichsweise großen Elektronenhülle."),
        ("Was sind Isotope eines chemischen Elements?", new[] { "Atome desselben Elements mit gleicher Protonenzahl, aber unterschiedlicher Neutronenzahl", "Atome völlig unterschiedlicher chemischer Elemente", "Ein anderes Wort für Elektronen" }, "Atome desselben Elements mit gleicher Protonenzahl, aber unterschiedlicher Neutronenzahl",
            "Isotope eines Elements besitzen dieselbe Anzahl an Protonen (und damit dieselben chemischen Eigenschaften), unterscheiden sich aber in der Anzahl der Neutronen."),
        ("Was ist Alphastrahlung?", new[] { "Strahlung aus Heliumkernen (2 Protonen, 2 Neutronen)", "Eine reine Form von Licht ohne jede Masse", "Strahlung, die ausschließlich aus Elektronen besteht" }, "Strahlung aus Heliumkernen (2 Protonen, 2 Neutronen)",
            "Alphastrahlung besteht aus Alphateilchen, die aus jeweils zwei Protonen und zwei Neutronen bestehen - identisch mit einem Heliumkern."),
        ("Was ist Betastrahlung?", new[] { "Strahlung aus schnellen Elektronen (oder Positronen), die beim radioaktiven Zerfall entstehen", "Strahlung, die ausschließlich aus Heliumkernen besteht", "Eine besonders energiereiche Form elektromagnetischer Strahlung ohne Masse" }, "Strahlung aus schnellen Elektronen (oder Positronen), die beim radioaktiven Zerfall entstehen",
            "Betastrahlung besteht aus schnellen Elektronen (Beta-Minus-Zerfall) oder Positronen (Beta-Plus-Zerfall), die beim radioaktiven Zerfall aus dem Atomkern ausgesendet werden."),
        ("Was ist Gammastrahlung?", new[] { "Energiereiche elektromagnetische Strahlung ohne Masse und Ladung", "Strahlung, die ausschließlich aus geladenen Heliumkernen besteht", "Eine Form von Strahlung, die nur aus Elektronen besteht" }, "Energiereiche elektromagnetische Strahlung ohne Masse und Ladung",
            "Gammastrahlung ist elektromagnetische Strahlung sehr hoher Energie, besitzt im Gegensatz zu Alpha- und Betastrahlung keine Masse und keine elektrische Ladung."),
        ("Welche der drei Strahlungsarten (Alpha, Beta, Gamma) hat das größte Durchdringungsvermögen?", new[] { "Gammastrahlung", "Alphastrahlung", "Betastrahlung" }, "Gammastrahlung",
            "Gammastrahlung durchdringt Materie am stärksten und benötigt z.B. dicke Blei- oder Betonschichten zur Abschirmung, während Alphastrahlung schon von Papier gestoppt wird."),
        ("Was versteht man unter dem Ionisierungsvermögen radioaktiver Strahlung?", new[] { "Die Fähigkeit der Strahlung, Atome oder Moleküle zu ionisieren, also Elektronen aus ihnen herauszuschlagen", "Die Fähigkeit der Strahlung, Materie ausschließlich zu erwärmen", "Ein Maß für die Farbe der Strahlung" }, "Die Fähigkeit der Strahlung, Atome oder Moleküle zu ionisieren, also Elektronen aus ihnen herauszuschlagen",
            "Ionisierende Strahlung kann Elektronen aus Atomen oder Molekülen herausschlagen, was z.B. biologisches Gewebe schädigen kann."),
        ("Was ist die Halbwertszeit eines radioaktiven Isotops?", new[] { "Die Zeit, in der die Hälfte der ursprünglich vorhandenen radioaktiven Atomkerne zerfallen ist", "Die Zeit, in der alle radioaktiven Atomkerne vollständig zerfallen sind", "Ein anderes Wort für die Reaktionszeit einer chemischen Reaktion" }, "Die Zeit, in der die Hälfte der ursprünglich vorhandenen radioaktiven Atomkerne zerfallen ist",
            "Nach einer Halbwertszeit ist statistisch gesehen die Hälfte der ursprünglich vorhandenen radioaktiven Atomkerne einer Probe zerfallen."),
        ("Was passiert mit der Aktivität einer radioaktiven Probe nach jeweils einer weiteren Halbwertszeit?", new[] { "Sie halbiert sich erneut gegenüber dem vorherigen Wert", "Sie verdoppelt sich jedes Mal", "Sie bleibt exakt konstant" }, "Sie halbiert sich erneut gegenüber dem vorherigen Wert",
            "Nach jeder weiteren Halbwertszeit sinkt die Aktivität einer radioaktiven Probe erneut auf die Hälfte ihres vorherigen Werts (exponentieller Zerfall)."),
        ("Was passiert bei einer Kernspaltung?", new[] { "Ein schwerer Atomkern wird in zwei oder mehr leichtere Kerne gespalten, wobei Energie freigesetzt wird", "Zwei leichte Atomkerne verschmelzen zu einem schweren Kern", "Es passiert überhaupt keine Veränderung am Atomkern" }, "Ein schwerer Atomkern wird in zwei oder mehr leichtere Kerne gespalten, wobei Energie freigesetzt wird",
            "Bei der Kernspaltung, wie sie z.B. in Kernkraftwerken genutzt wird, wird ein schwerer Atomkern (z.B. Uran) in leichtere Kerne gespalten und dabei Energie frei."),
        ("Was passiert bei einer Kernfusion?", new[] { "Zwei leichte Atomkerne verschmelzen zu einem schwereren Kern, wobei Energie freigesetzt wird", "Ein schwerer Atomkern wird in leichtere Kerne gespalten", "Es findet keinerlei Veränderung am Atomkern statt" }, "Zwei leichte Atomkerne verschmelzen zu einem schwereren Kern, wobei Energie freigesetzt wird",
            "Bei der Kernfusion, wie sie z.B. in der Sonne abläuft, verschmelzen leichte Atomkerne (z.B. Wasserstoff) zu schwereren Kernen und setzen dabei enorme Energie frei."),
        ("Welche Schutzmaßnahmen werden typischerweise gegen radioaktive Strahlung eingesetzt?", new[] { "Abstand halten, Abschirmung (z.B. Blei) und möglichst kurze Aufenthaltsdauer", "Es gibt grundsätzlich keine wirksamen Schutzmaßnahmen", "Radioaktive Strahlung erfordert überhaupt keine besonderen Vorsichtsmaßnahmen" }, "Abstand halten, Abschirmung (z.B. Blei) und möglichst kurze Aufenthaltsdauer",
            "Die drei wichtigsten Schutzprinzipien gegen ionisierende Strahlung sind ausreichend Abstand, geeignete Abschirmung und eine möglichst kurze Expositionszeit."),
        ("Warum ist die Endlagerung von radioaktivem Abfall gesellschaftlich ein umstrittenes Thema?", new[] { "Manche radioaktiven Stoffe bleiben über sehr lange Zeiträume gefährlich, was hohe Anforderungen an die Sicherheit der Lagerung stellt", "Radioaktiver Abfall verliert seine Gefährlichkeit bereits nach wenigen Minuten", "Endlagerung hat mit gesellschaftlichen Fragen überhaupt nichts zu tun" }, "Manche radioaktiven Stoffe bleiben über sehr lange Zeiträume gefährlich, was hohe Anforderungen an die Sicherheit der Lagerung stellt",
            "Da manche radioaktiven Isotope extrem lange Halbwertszeiten haben, muss ihre Lagerung über sehr lange Zeiträume sicher vor Umwelteinflüssen und menschlichem Zugriff geschützt sein."),
        ("Was ist ein Beispiel für eine biologische Wirkung ionisierender Strahlung auf Organismen?", new[] { "Schädigung von Zellen und der DNA, was Krebs auslösen kann", "Ionisierende Strahlung hat auf lebende Organismen überhaupt keine Wirkung", "Ionisierende Strahlung stärkt ausschließlich das Immunsystem" }, "Schädigung von Zellen und der DNA, was Krebs auslösen kann",
            "Ionisierende Strahlung kann Zellstrukturen und die DNA schädigen, was in höheren Dosen das Risiko für Krebserkrankungen erhöhen kann."),
        ("Was zeigt eine vollständige Zerfallsgleichung für einen radioaktiven Zerfall?", new[] { "Das Ausgangsisotop, die Art der Strahlung sowie das entstehende Tochterisotop", "Ausschließlich die Farbe der Strahlung", "Nur die Halbwertszeit ohne jede weitere Information" }, "Das Ausgangsisotop, die Art der Strahlung sowie das entstehende Tochterisotop",
            "Eine vollständige Zerfallsgleichung zeigt das Mutterisotop, die abgegebene Strahlungsart (z.B. Alpha- oder Betateilchen) und das dabei entstehende Tochterisotop."),
        ("Was passiert mit der Massenzahl eines Atomkerns bei einem Alphazerfall?", new[] { "Sie verringert sich um 4", "Sie bleibt exakt unverändert", "Sie erhöht sich um 4" }, "Sie verringert sich um 4",
            "Da beim Alphazerfall ein Heliumkern (2 Protonen, 2 Neutronen, Massenzahl 4) abgegeben wird, sinkt die Massenzahl des verbleibenden Kerns um 4."),
        ("Was passiert mit der Ordnungszahl (Protonenzahl) eines Atomkerns bei einem Betazerfall (Beta-Minus)?", new[] { "Sie erhöht sich um 1, da sich ein Neutron in ein Proton umwandelt", "Sie verringert sich um 1", "Sie bleibt exakt unverändert" }, "Sie erhöht sich um 1, da sich ein Neutron in ein Proton umwandelt",
            "Beim Beta-Minus-Zerfall wandelt sich im Kern ein Neutron in ein Proton um, wodurch sich die Ordnungszahl (Protonenzahl) um eins erhöht und ein Elektron ausgesandt wird."),
        ("Warum wird bei Röntgenaufnahmen im medizinischen Bereich versucht, die Strahlendosis so gering wie möglich zu halten?", new[] { "Um das Risiko schädlicher Wirkungen der ionisierenden Strahlung auf den Körper zu minimieren", "Weil Röntgenstrahlung völlig ungefährlich ist und keinerlei Vorsicht erfordert", "Weil eine geringe Dosis automatisch bessere Bildqualität liefert" }, "Um das Risiko schädlicher Wirkungen der ionisierenden Strahlung auf den Körper zu minimieren",
            "Da Röntgenstrahlung ionisierend wirkt und Gewebe schädigen kann, wird im medizinischen Alltag versucht, die Strahlendosis nach dem Prinzip \"so wenig wie möglich, so viel wie nötig\" zu minimieren."),
        ("Was nutzt ein Kernkraftwerk zur Stromerzeugung grundlegend?", new[] { "Die bei der Kernspaltung freigesetzte Energie erzeugt Wärme, die letztlich Turbinen antreibt", "Es nutzt ausschließlich die direkte Umwandlung von Licht in Strom", "Kernkraftwerke erzeugen Strom ohne jede Wärmeentwicklung" }, "Die bei der Kernspaltung freigesetzte Energie erzeugt Wärme, die letztlich Turbinen antreibt",
            "Die Energie aus der Kernspaltung erhitzt Wasser zu Dampf, der wie in anderen Kraftwerken Turbinen und damit Generatoren zur Stromerzeugung antreibt."),
        ("Warum wird bei der Kernfusion in der Sonne so enorm viel Energie freigesetzt?", new[] { "Ein winziger Teil der verschmelzenden Kernmasse wird gemäß E=mc² in sehr große Energiemengen umgewandelt", "Bei der Fusion wird überhaupt keine Energie freigesetzt", "Die Sonne nutzt zur Energiegewinnung ausschließlich chemische Verbrennung" }, "Ein winziger Teil der verschmelzenden Kernmasse wird gemäß E=mc² in sehr große Energiemengen umgewandelt",
            "Bei der Kernfusion in der Sonne wird ein kleiner Massendefekt in gewaltige Energiemengen umgewandelt, was nach Einsteins Formel E=mc² auch kleinste Massen in große Energiebeträge übersetzt.")
    };

    private static QuizQuestion RadioaktivitaetUndKernphysik(Random r)
    {
        var f = RadioaktivitaetListe[r.Next(RadioaktivitaetListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Radioaktivität und Kernphysik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Alpha-, Beta- und Gammastrahlung unterscheiden sich in Zusammensetzung und Durchdringungsvermögen; nach jeder Halbwertszeit halbiert sich die Aktivität einer radioaktiven Probe."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SchwingungenWellenOptikListe =
    {
        ("Was ist die Amplitude einer harmonischen Schwingung?", new[] { "Die maximale Auslenkung aus der Ruhelage", "Die Zeit für eine vollständige Schwingung", "Die Anzahl der Schwingungen pro Sekunde" }, "Die maximale Auslenkung aus der Ruhelage",
            "Die Amplitude gibt an, wie weit sich ein schwingendes System maximal von seiner Ruhelage entfernt."),
        ("Was ist die Frequenz einer Schwingung?", new[] { "Die Anzahl der Schwingungen pro Sekunde", "Die maximale Auslenkung aus der Ruhelage", "Die gesamte zurückgelegte Strecke eines Pendels" }, "Die Anzahl der Schwingungen pro Sekunde",
            "Die Frequenz (in Hertz) gibt an, wie viele vollständige Schwingungen pro Sekunde stattfinden."),
        ("Was ist die Periodendauer einer Schwingung?", new[] { "Die Zeit für eine vollständige Schwingung", "Die maximale Geschwindigkeit während der Schwingung", "Ein anderes Wort für die Amplitude" }, "Die Zeit für eine vollständige Schwingung",
            "Die Periodendauer ist die Zeit, die ein schwingendes System für eine vollständige Schwingung (Hin- und Rückweg) benötigt."),
        ("Wie hängen Frequenz und Periodendauer einer Schwingung zusammen?", new[] { "Sie sind zueinander reziprok: Frequenz = 1 / Periodendauer", "Sie sind immer exakt gleich groß", "Sie stehen in keinerlei mathematischem Zusammenhang" }, "Sie sind zueinander reziprok: Frequenz = 1 / Periodendauer",
            "Frequenz und Periodendauer sind Kehrwerte voneinander: Je kürzer die Periodendauer, desto höher die Frequenz."),
        ("Was ist Resonanz bei einem schwingungsfähigen System?", new[] { "Eine besonders starke Schwingungsantwort, wenn die Anregungsfrequenz der Eigenfrequenz des Systems entspricht", "Ein Zustand, in dem ein System überhaupt nicht mehr schwingt", "Ein anderes Wort für die Amplitude einer Schwingung" }, "Eine besonders starke Schwingungsantwort, wenn die Anregungsfrequenz der Eigenfrequenz des Systems entspricht",
            "Resonanz tritt auf, wenn ein System mit einer Frequenz angeregt wird, die seiner Eigenfrequenz entspricht - die Schwingungsamplitude kann dabei stark ansteigen."),
        ("Was ist Interferenz bei mechanischen Wellen?", new[] { "Die Überlagerung zweier oder mehrerer Wellen, die sich verstärken oder abschwächen können", "Ein anderes Wort für die Reflexion einer Welle", "Ein Vorgang, bei dem Wellen völlig verschwinden" }, "Die Überlagerung zweier oder mehrerer Wellen, die sich verstärken oder abschwächen können",
            "Bei der Interferenz überlagern sich Wellen, wodurch sich ihre Amplituden je nach Phasenlage verstärken (konstruktive Interferenz) oder abschwächen (destruktive Interferenz) können."),
        ("Was ist Beugung bei Wellen?", new[] { "Die Ablenkung einer Welle beim Passieren eines Hindernisses oder einer engen Öffnung", "Ein anderes Wort für die Amplitude einer Welle", "Ein Vorgang, der ausschließlich bei Lichtwellen auftritt" }, "Die Ablenkung einer Welle beim Passieren eines Hindernisses oder einer engen Öffnung",
            "Beugung beschreibt, wie sich Wellen beim Durchqueren von Öffnungen oder um Hindernisse herum ausbreiten und dabei von der geradlinigen Ausbreitung abweichen."),
        ("Was passiert bei der Reflexion einer Welle an einer Grenzfläche?", new[] { "Die Welle wird zumindest teilweise zurückgeworfen", "Die Welle wird vollständig absorbiert und verschwindet", "Die Welle ändert dabei niemals ihre Richtung" }, "Die Welle wird zumindest teilweise zurückgeworfen",
            "Trifft eine Welle auf eine Grenzfläche zwischen zwei Medien, wird zumindest ein Teil der Welle reflektiert (zurückgeworfen)."),
        ("Wie groß ist die Lichtgeschwindigkeit im Vakuum ungefähr?", new[] { "Etwa 300.000 km/s", "Etwa 300 km/s", "Etwa 3.000.000 km/s" }, "Etwa 300.000 km/s",
            "Die Lichtgeschwindigkeit im Vakuum beträgt etwa 300.000 Kilometer pro Sekunde (genauer: 299.792 km/s) und gilt als Naturkonstante."),
        ("Was besagt das Reflexionsgesetz beim Auftreffen von Licht auf einen Spiegel?", new[] { "Einfallswinkel und Ausfallswinkel sind zueinander gleich groß", "Der Ausfallswinkel ist immer doppelt so groß wie der Einfallswinkel", "Licht wird an einem Spiegel niemals reflektiert" }, "Einfallswinkel und Ausfallswinkel sind zueinander gleich groß",
            "Nach dem Reflexionsgesetz gilt: Der Einfallswinkel ist stets gleich dem Ausfallswinkel, jeweils gemessen zum Lot der spiegelnden Fläche."),
        ("Was beschreibt das Brechungsgesetz beim Übergang von Licht zwischen zwei unterschiedlich dichten Medien?", new[] { "Licht ändert beim Übergang zwischen unterschiedlichen Medien seine Ausbreitungsrichtung", "Licht behält beim Mediumwechsel immer exakt seine ursprüngliche Richtung bei", "Licht kann niemals von einem Medium in ein anderes übertreten" }, "Licht ändert beim Übergang zwischen unterschiedlichen Medien seine Ausbreitungsrichtung",
            "Beim Übergang von einem optisch dünneren in ein optisch dichteres Medium (oder umgekehrt) wird Licht gebrochen, das heißt, es ändert an der Grenzfläche seine Ausbreitungsrichtung."),
        ("Was ist Totalreflexion?", new[] { "Ein Effekt, bei dem Licht an einer Grenzfläche vollständig reflektiert wird, statt ins andere Medium überzutreten", "Ein Effekt, bei dem Licht ausschließlich gebrochen, aber niemals reflektiert wird", "Ein anderes Wort für die normale, teilweise Reflexion von Licht" }, "Ein Effekt, bei dem Licht an einer Grenzfläche vollständig reflektiert wird, statt ins andere Medium überzutreten",
            "Trifft Licht beim Übergang vom optisch dichteren ins dünnere Medium in einem ausreichend flachen Winkel auf die Grenzfläche, wird es vollständig reflektiert (Totalreflexion) - ein Prinzip, das z.B. in Glasfaserkabeln genutzt wird."),
        ("Was beschreibt die Linsengleichung bei einer Sammellinse?", new[] { "Den mathematischen Zusammenhang zwischen Gegenstandsweite, Bildweite und Brennweite", "Ausschließlich die Farbe des entstehenden Bildes", "Den Zusammenhang zwischen der Masse und dem Gewicht einer Linse" }, "Den mathematischen Zusammenhang zwischen Gegenstandsweite, Bildweite und Brennweite",
            "Die Linsengleichung verknüpft Gegenstandsweite, Bildweite und Brennweite einer Linse und ermöglicht die Berechnung, wo ein scharfes Bild entsteht."),
        ("Wie entsteht ein scharfes, reelles Bild bei einer Sammellinse, z.B. in einem Fotoapparat?", new[] { "Lichtstrahlen eines Gegenstands werden von der Linse gebündelt und treffen sich in der Bildebene", "Lichtstrahlen werden von der Linse vollständig absorbiert", "Ein scharfes Bild entsteht unabhängig von der Position der Linse immer automatisch" }, "Lichtstrahlen eines Gegenstands werden von der Linse gebündelt und treffen sich in der Bildebene",
            "Eine Sammellinse bündelt die von einem Objektpunkt ausgehenden Lichtstrahlen so, dass sie sich in der Bildebene wieder treffen und dort ein scharfes, reelles Bild erzeugen."),
        ("Wie funktioniert die Bildentstehung im menschlichen Auge grundlegend?", new[] { "Die Augenlinse bündelt einfallendes Licht und erzeugt auf der Netzhaut ein scharfes Bild", "Das Auge benötigt für die Bildentstehung überhaupt keine Linse", "Bilder entstehen im Auge ausschließlich durch Reflexion, nie durch Brechung" }, "Die Augenlinse bündelt einfallendes Licht und erzeugt auf der Netzhaut ein scharfes Bild",
            "Ähnlich wie bei einer Kamera bündelt die Augenlinse einfallendes Licht, sodass auf der lichtempfindlichen Netzhaut ein scharfes, aber auf dem Kopf stehendes Bild entsteht, das das Gehirn später korrigiert wahrnimmt."),
        ("Was zeigt das Farbspektrum, wenn weißes Licht durch ein Prisma fällt?", new[] { "Weißes Licht setzt sich aus verschiedenen Farben (Wellenlängen) zusammen, die unterschiedlich stark gebrochen werden", "Weißes Licht besteht aus nur einer einzigen, festen Wellenlänge", "Ein Prisma verändert die Farbe des Lichts überhaupt nicht" }, "Weißes Licht setzt sich aus verschiedenen Farben (Wellenlängen) zusammen, die unterschiedlich stark gebrochen werden",
            "Da unterschiedliche Wellenlängen (Farben) beim Durchgang durch ein Prisma unterschiedlich stark gebrochen werden, spaltet sich weißes Licht in sein Farbspektrum auf."),
        ("Wie berechnet man näherungsweise die Periodendauer eines Fadenpendels bei kleinen Auslenkungen?", new[] { "Sie hängt von der Pendellänge und der Fallbeschleunigung ab, nicht aber von der Pendelmasse", "Sie hängt ausschließlich von der Masse des Pendelkörpers ab", "Sie ist bei jedem Fadenpendel exakt identisch, unabhängig von dessen Länge" }, "Sie hängt von der Pendellänge und der Fallbeschleunigung ab, nicht aber von der Pendelmasse",
            "Die Periodendauer eines Fadenpendels bei kleinen Auslenkungen hängt von der Pendellänge und der Fallbeschleunigung ab, ist aber näherungsweise unabhängig von der Masse des Pendelkörpers."),
        ("Was passiert mit der Periodendauer eines Fadenpendels, wenn man die Pendellänge deutlich vergrößert?", new[] { "Die Periodendauer wird länger", "Die Periodendauer wird automatisch kürzer", "Die Periodendauer bleibt exakt unverändert" }, "Die Periodendauer wird länger",
            "Ein längeres Pendel schwingt langsamer, das heißt, seine Periodendauer nimmt mit zunehmender Pendellänge zu."),
        ("Was ist ein Federschwinger und wovon hängt seine Periodendauer hauptsächlich ab?", new[] { "Eine an einer Feder schwingende Masse, deren Periodendauer von Federhärte und Masse abhängt", "Ein System, das ausschließlich von der Farbe der Feder abhängt", "Ein Federschwinger hat immer exakt dieselbe Periodendauer wie ein Fadenpendel gleicher Länge" }, "Eine an einer Feder schwingende Masse, deren Periodendauer von Federhärte und Masse abhängt",
            "Bei einem Federschwinger bestimmen die Federkonstante (Härte der Feder) und die schwingende Masse gemeinsam die Periodendauer der Schwingung."),
        ("Warum kann Resonanz bei Brücken oder Bauwerken technisch gefährlich werden?", new[] { "Wird ein Bauwerk genau mit seiner Eigenfrequenz angeregt, können sich Schwingungen gefährlich stark aufschaukeln", "Resonanz hat auf Bauwerke grundsätzlich überhaupt keine Auswirkung", "Bauwerke besitzen grundsätzlich keine Eigenfrequenz" }, "Wird ein Bauwerk genau mit seiner Eigenfrequenz angeregt, können sich Schwingungen gefährlich stark aufschaukeln",
            "Trifft eine äußere Anregung (z.B. Wind oder gleichmäßige Schritte) genau die Eigenfrequenz eines Bauwerks, kann sich die Schwingungsamplitude durch Resonanz gefährlich aufschaukeln - deshalb werden Brücken entsprechend konstruiert.")
    };

    private static QuizQuestion SchwingungenWellenOptik(Random r)
    {
        var f = SchwingungenWellenOptikListe[r.Next(SchwingungenWellenOptikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Schwingungen, Wellen und optische Geräte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Amplitude ist die maximale Auslenkung, Frequenz und Periodendauer sind zueinander reziprok; beim Licht gilt Einfallswinkel = Ausfallswinkel (Reflexion) und die Linsengleichung für Sammellinsen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WaermeausdehnungListe =
    {
        ("Was passiert mit den meisten Stoffen, wenn man sie erhitzt?", new[] { "Sie dehnen sich aus", "Sie ziehen sich zusammen", "Sie verändern sich überhaupt nicht" }, "Sie dehnen sich aus",
            "Die meisten Stoffe dehnen sich beim Erwärmen aus, weil sich ihre Teilchen schneller bewegen und mehr Platz brauchen."),
        ("Was passiert mit den meisten Stoffen, wenn man sie abkühlt?", new[] { "Sie ziehen sich zusammen", "Sie dehnen sich weiter aus", "Sie werden sofort flüssig" }, "Sie ziehen sich zusammen",
            "Beim Abkühlen bewegen sich die Teilchen langsamer und rücken enger zusammen - der Stoff zieht sich zusammen."),
        ("Warum haben Brücken oft spezielle Dehnungsfugen?", new[] { "Damit sich das Material bei Hitze ausdehnen kann, ohne zu brechen", "Damit die Brücke leichter wird", "Aus rein optischen Gründen" }, "Damit sich das Material bei Hitze ausdehnen kann, ohne zu brechen",
            "Ohne Dehnungsfugen könnte sich das Brückenmaterial bei großer Hitze nicht ausdehnen und würde sich verformen oder brechen."),
        ("Warum lässt sich ein festsitzender Metalldeckel oft leichter öffnen, wenn man ihn kurz unter warmes Wasser hält?", new[] { "Das Metall dehnt sich durch die Wärme leicht aus und löst sich vom Glas", "Warmes Wasser macht das Glas weicher", "Wärme hat auf Metall überhaupt keine Wirkung" }, "Das Metall dehnt sich durch die Wärme leicht aus und löst sich vom Glas",
            "Metall dehnt sich bei Wärme stärker aus als Glas, wodurch der Deckel etwas größer wird und sich leichter abdrehen lässt."),
        ("Wie funktioniert ein einfaches Flüssigkeitsthermometer?", new[] { "Die Flüssigkeit dehnt sich bei Wärme aus und steigt im dünnen Röhrchen", "Es misst Temperatur über einen eingebauten Bildschirm", "Die Flüssigkeit verschwindet bei Wärme komplett" }, "Die Flüssigkeit dehnt sich bei Wärme aus und steigt im dünnen Röhrchen",
            "Die Flüssigkeit im Thermometer (z.B. gefärbter Alkohol) dehnt sich bei steigender Temperatur aus und steigt im dünnen Röhrchen sichtbar an."),
        ("Warum hatten Eisenbahnschienen früher kleine Lücken zwischen den einzelnen Schienenstücken?", new[] { "Damit sich die Schienen bei Hitze ausdehnen können, ohne sich zu verbiegen", "Damit der Zug dort lauter fährt", "Aus Versehen bei der Herstellung" }, "Damit sich die Schienen bei Hitze ausdehnen können, ohne sich zu verbiegen",
            "Ohne diese Lücken könnten sich die Schienen bei großer Hitze nicht ausdehnen und würden sich gefährlich verbiegen."),
        ("Was passiert mit der Luft in einem Ballon, wenn man ihn längere Zeit in die pralle Sonne legt?", new[] { "Die Luft dehnt sich aus und der Ballon kann größer werden oder platzen", "Die Luft zieht sich zusammen und der Ballon schrumpft", "Es passiert überhaupt nichts" }, "Die Luft dehnt sich aus und der Ballon kann größer werden oder platzen",
            "Die erwärmte Luft im Ballon dehnt sich aus und erhöht den Innendruck, wodurch der Ballon größer wird oder sogar platzen kann."),
        ("Welcher Aggregatzustand (fest, flüssig oder gasförmig) dehnt sich bei Erwärmung meist am stärksten aus?", new[] { "Gasförmig", "Fest", "Flüssig" }, "Gasförmig",
            "Gase haben die beweglichsten Teilchen und reagieren deshalb am stärksten mit Ausdehnung auf Erwärmung."),
        ("Warum ist Wasser bei der Wärmeausdehnung in der Nähe des Gefrierpunkts eine Ausnahme?", new[] { "Wasser dehnt sich beim Gefrieren aus, statt sich zusammenzuziehen", "Wasser verhält sich exakt wie jeder andere Stoff", "Wasser zieht sich beim Erwärmen immer zusammen" }, "Wasser dehnt sich beim Gefrieren aus, statt sich zusammenzuziehen",
            "Anders als die meisten Stoffe dehnt sich Wasser beim Gefrieren zu Eis aus - deshalb schwimmt Eis auf flüssigem Wasser."),
        ("Warum können Wasserleitungen im Winter platzen, wenn das Wasser darin gefriert?", new[] { "Das gefrierende Wasser dehnt sich aus und übt hohen Druck auf die Leitung aus", "Gefrierendes Wasser zieht sich stark zusammen und reißt die Leitung ein", "Kälte macht Metallrohre grundsätzlich instabil" }, "Das gefrierende Wasser dehnt sich aus und übt hohen Druck auf die Leitung aus",
            "Da sich Wasser beim Gefrieren ausdehnt, entsteht in einer vollständig gefüllten Leitung ein hoher Druck, der sie zum Platzen bringen kann."),
        ("Warum lässt man Stromleitungen zwischen zwei Masten oft etwas durchhängen?", new[] { "Damit sie sich bei Kälte zusammenziehen können, ohne zu reißen", "Damit sie besser aussehen", "Damit Vögel bequemer darauf sitzen können" }, "Damit sie sich bei Kälte zusammenziehen können, ohne zu reißen",
            "Ziehen sich die Leitungen bei Kälte zusammen, brauchen sie etwas Spielraum (Durchhang), damit sie dabei nicht reißen."),
        ("Wie nennt man das Prinzip, dass sich Stoffe bei Erwärmung ausdehnen und bei Abkühlung zusammenziehen?", new[] { "Thermische Ausdehnung (Wärmeausdehnung)", "Reibung", "Sublimation" }, "Thermische Ausdehnung (Wärmeausdehnung)",
            "Die Volumenänderung eines Stoffs mit der Temperatur nennt man thermische Ausdehnung bzw. Wärmeausdehnung."),
        ("Warum krümmt sich ein Bimetallstreifen (aus zwei verschiedenen Metallen), wenn er erwärmt wird?", new[] { "Weil sich die zwei Metalle unterschiedlich stark ausdehnen", "Weil sich beide Metalle exakt gleich stark ausdehnen", "Weil Metalle sich bei Wärme überhaupt nicht verändern" }, "Weil sich die zwei Metalle unterschiedlich stark ausdehnen",
            "Da die beiden Metalle unterschiedlich stark auf Wärme reagieren, dehnt sich eine Seite stärker aus als die andere - der Streifen krümmt sich."),
        ("Wo wird ein Bimetallstreifen im Alltag häufig eingesetzt?", new[] { "In einfachen Thermostaten/Reglern, die auf Temperaturänderung reagieren", "Ausschließlich in Flugzeugtriebwerken", "In gewöhnlichen Fensterscheiben" }, "In einfachen Thermostaten/Reglern, die auf Temperaturänderung reagieren",
            "Die temperaturabhängige Krümmung des Bimetallstreifens wird genutzt, um z.B. einen Stromkreis bei bestimmter Temperatur zu schließen oder zu öffnen."),
        ("Was passiert mit dem Druck einer eingeschlossenen Gasmenge, wenn man sie stark erwärmt, ohne dass Gas entweichen kann?", new[] { "Der Druck steigt", "Der Druck sinkt", "Der Druck bleibt exakt gleich" }, "Der Druck steigt",
            "Erwärmtes Gas will sich ausdehnen - kann es das nicht, weil es eingeschlossen ist, steigt stattdessen der Druck an."),
        ("Warum sollte man eine geschlossene Getränkedose nicht in der prallen Sonne oder im heißen Auto liegen lassen?", new[] { "Das Gas darin dehnt sich aus und der Druck in der Dose kann gefährlich ansteigen", "Getränkedosen werden durch Sonnenlicht chemisch instabil", "Es besteht dabei überhaupt keine Gefahr" }, "Das Gas darin dehnt sich aus und der Druck in der Dose kann gefährlich ansteigen",
            "Die Erwärmung lässt das Gas in der geschlossenen Dose sich ausdehnen wollen - der steigende Innendruck kann die Dose im Extremfall zum Platzen bringen."),
        ("Was zeigt ein Fieberthermometer an, wenn sich die Flüssigkeit darin weiter ausdehnt?", new[] { "Eine höhere Temperatur", "Eine niedrigere Temperatur", "Es zeigt gar keine Temperatur an" }, "Eine höhere Temperatur",
            "Je mehr sich die Thermometerflüssigkeit ausdehnt und im Röhrchen aufsteigt, desto höher ist die gemessene Temperatur."),
        ("Warum werden Brücken und Gleise regelmäßig auf ihre Dehnungsfugen kontrolliert?", new[] { "Damit die Wärmeausdehnung nicht zu Schäden oder Verformungen führt", "Weil Dehnungsfugen rein dekorativ sind", "Weil sie sich sonst von selbst auflösen würden" }, "Damit die Wärmeausdehnung nicht zu Schäden oder Verformungen führt",
            "Funktionierende Dehnungsfugen sind wichtig, damit temperaturbedingte Ausdehnung nicht zu Rissen oder Verformungen an Bauwerken führt."),
        ("Was passiert grundsätzlich mit den Teilchen eines Stoffs, wenn er erwärmt wird?", new[] { "Sie bewegen sich schneller und brauchen dadurch mehr Platz", "Sie bewegen sich langsamer und rücken enger zusammen", "Sie verändern ihre Bewegung überhaupt nicht" }, "Sie bewegen sich schneller und brauchen dadurch mehr Platz",
            "Wärme versetzt die Teilchen eines Stoffs in schnellere Bewegung - dadurch beanspruchen sie im Mittel mehr Raum, der Stoff dehnt sich aus."),
        ("Was passiert mit dem Loch eines Metallrings, wenn der ganze Ring erhitzt wird?", new[] { "Das Loch wird größer, wie der Rest des Rings", "Das Loch wird kleiner, obwohl der Ring wächst", "Das Loch verändert sich nie" }, "Das Loch wird größer, wie der Rest des Rings",
            "Da sich das gesamte Metall gleichmäßig ausdehnt, wird auch die Öffnung im Ring größer - ein häufiger Denkfehler nimmt fälschlich an, sie würde kleiner.")
    };

    private static QuizQuestion WaermeausdehnungKoerper(Random r)
    {
        var f = WaermeausdehnungListe[r.Next(WaermeausdehnungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Thermisches Verhalten von Körpern (Wärmeausdehnung)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Fast alle Stoffe dehnen sich beim Erwärmen aus und ziehen sich beim Abkühlen zusammen - Wasser ist rund um den Gefrierpunkt eine bekannte Ausnahme."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WechselwirkungKraftListe =
    {
        ("Was versteht man unter einer \"Kraft\" in der Physik, einfach erklärt?", new[] { "Etwas, das einen Körper in Bewegung setzen, abbremsen oder verformen kann", "Ein anderes Wort für Energie", "Ein Gerät zum Messen von Temperatur" }, "Etwas, das einen Körper in Bewegung setzen, abbremsen oder verformen kann",
            "Eine Kraft kann einen Körper beschleunigen, abbremsen, ablenken oder verformen."),
        ("Was ist die Schwerkraft (Gravitation)?", new[] { "Die Kraft, mit der die Erde alle Gegenstände nach unten zieht", "Eine Kraft, die nur bei Metallen wirkt", "Eine Kraft, die Gegenstände nach oben drückt" }, "Die Kraft, mit der die Erde alle Gegenstände nach unten zieht",
            "Die Schwerkraft der Erde zieht alle Gegenstände in Richtung Erdmittelpunkt, weshalb sie zu Boden fallen."),
        ("Was passiert, wenn zwei gleich starke Kräfte in genau entgegengesetzte Richtungen an einem Körper ziehen?", new[] { "Sie heben sich gegenseitig auf, der Körper bleibt in Ruhe", "Der Körper bewegt sich doppelt so schnell", "Der Körper verschwindet" }, "Sie heben sich gegenseitig auf, der Körper bleibt in Ruhe",
            "Sind zwei entgegengesetzte Kräfte gleich groß, gleichen sie sich vollständig aus - der Körper bewegt sich nicht (Gleichgewicht)."),
        ("Was ist Reibung?", new[] { "Eine Kraft, die der Bewegung zweier sich berührender Oberflächen entgegenwirkt", "Eine Kraft, die Bewegung immer beschleunigt", "Ein anderes Wort für Schwerkraft" }, "Eine Kraft, die der Bewegung zweier sich berührender Oberflächen entgegenwirkt",
            "Reibung bremst die Bewegung zweier Oberflächen, die sich berühren, ab - je rauer die Oberflächen, desto größer die Reibung."),
        ("Warum ist es auf Eis schwerer zu laufen als auf normalem Asphalt?", new[] { "Weil Eis viel weniger Reibung bietet als Asphalt", "Weil Eis viel mehr Reibung bietet als Asphalt", "Weil Eis magnetisch ist" }, "Weil Eis viel weniger Reibung bietet als Asphalt",
            "Die glatte Eisoberfläche bietet den Schuhsohlen kaum Reibung, wodurch man leicht ausrutscht."),
        ("Was ist ein Hebel, einfach erklärt?", new[] { "Eine starre Stange, mit der man mit weniger Kraft eine größere Last bewegen kann", "Ein Gerät, das ausschließlich elektrischen Strom misst", "Ein anderes Wort für Reibung" }, "Eine starre Stange, mit der man mit weniger Kraft eine größere Last bewegen kann",
            "Ein Hebel verstärkt die eingesetzte Kraft, sodass man mit weniger Kraftaufwand eine schwerere Last bewegen kann."),
        ("Warum kann man mit einer langen Brechstange leichter einen schweren Stein bewegen als mit einer kurzen?", new[] { "Ein längerer Hebelarm verstärkt die Wirkung der eingesetzten Kraft", "Eine lange Stange ist automatisch leichter als eine kurze", "Die Länge einer Stange hat keinerlei Einfluss" }, "Ein längerer Hebelarm verstärkt die Wirkung der eingesetzten Kraft",
            "Je länger der Hebelarm, desto größer die Wirkung derselben eingesetzten Kraft am anderen Ende."),
        ("Was besagt das Wechselwirkungsprinzip (\"Kraft = Gegenkraft\") einfach erklärt?", new[] { "Übt ein Körper eine Kraft auf einen anderen aus, wirkt eine gleich große Gegenkraft zurück", "Kräfte wirken immer nur in eine einzige Richtung", "Nur schwere Körper üben Kräfte auf andere aus" }, "Übt ein Körper eine Kraft auf einen anderen aus, wirkt eine gleich große Gegenkraft zurück",
            "Zu jeder Kraft gibt es eine gleich große Gegenkraft in die entgegengesetzte Richtung - deshalb spürt man beim Drücken gegen eine Wand auch einen Gegendruck."),
        ("Warum spürt man beim Anschieben einer Wand selbst eine Gegenkraft?", new[] { "Die Wand drückt mit derselben Kraft zurück, mit der man selbst gedrückt hat", "Die Wand bewegt sich unbemerkt ein kleines Stück", "Es handelt sich um reine Einbildung" }, "Die Wand drückt mit derselben Kraft zurück, mit der man selbst gedrückt hat",
            "Nach dem Wechselwirkungsprinzip übt die Wand exakt dieselbe Kraft in die Gegenrichtung aus, die man selbst auf sie ausübt."),
        ("Wie wirkt sich eine größere Kraft normalerweise auf die Beschleunigung eines Körpers aus?", new[] { "Eine größere Kraft beschleunigt einen Körper stärker", "Eine größere Kraft verlangsamt den Körper immer", "Kraft hat keinen Einfluss auf die Beschleunigung" }, "Eine größere Kraft beschleunigt einen Körper stärker",
            "Je größer die auf einen Körper wirkende Kraft, desto stärker wird er beschleunigt."),
        ("Was ist eine Federwaage und wie funktioniert sie?", new[] { "Ein Messgerät, das die wirkende Kraft an der Dehnung einer Feder misst", "Ein Gerät, das ausschließlich Temperatur misst", "Ein Werkzeug zum Schneiden von Metall" }, "Ein Messgerät, das die wirkende Kraft an der Dehnung einer Feder misst",
            "Je stärker die Kraft (z.B. das Gewicht eines Gegenstands), desto weiter dehnt sich die Feder der Federwaage - daran liest man die Kraft ab."),
        ("Warum braucht man beim Ziehen eines schweren Schlittens über Schnee weniger Kraft als über Kies?", new[] { "Schnee bietet weniger Reibung als Kies", "Schnee bietet mehr Reibung als Kies", "Kies ist grundsätzlich schwerer als Schnee" }, "Schnee bietet weniger Reibung als Kies",
            "Die glattere Schneeoberfläche erzeugt weniger Reibungswiderstand als grober, unebener Kies."),
        ("Was passiert mit einem ruhenden Gegenstand, auf den keine Kraft wirkt?", new[] { "Er bleibt in Ruhe", "Er beginnt sich von selbst zu bewegen", "Er verschwindet" }, "Er bleibt in Ruhe",
            "Ohne einwirkende Kraft ändert ein Körper seinen Bewegungszustand nicht - ein ruhender Körper bleibt in Ruhe (Trägheit)."),
        ("Was ist der Unterschied zwischen einer Zugkraft und einer Druckkraft?", new[] { "Eine Zugkraft zieht an einem Körper, eine Druckkraft drückt gegen ihn", "Beide bezeichnen exakt dasselbe", "Zugkraft wirkt nur bei Flüssigkeiten" }, "Eine Zugkraft zieht an einem Körper, eine Druckkraft drückt gegen ihn",
            "Eine Zugkraft wirkt ziehend (z.B. an einem Seil), eine Druckkraft wirkt drückend (z.B. beim Zusammenpressen einer Feder)."),
        ("Warum nutzt ein Fahrrad Kette und Zahnräder statt die Räder direkt anzutreiben?", new[] { "Sie übertragen und verändern die Kraft effizient auf die Räder", "Kette und Zahnräder haben nur eine dekorative Funktion", "Ohne Kette könnte das Fahrrad gar nicht rollen" }, "Sie übertragen und verändern die Kraft effizient auf die Räder",
            "Kette und Zahnräder übertragen die Tretkraft effizient auf das Hinterrad und ermöglichen durch unterschiedliche Übersetzungen leichteres oder schnelleres Fahren."),
        ("Was passiert mit der Reibung, wenn man eine Oberfläche mit Öl schmiert?", new[] { "Die Reibung wird deutlich kleiner", "Die Reibung wird deutlich größer", "Öl hat keinerlei Einfluss auf Reibung" }, "Die Reibung wird deutlich kleiner",
            "Öl verringert den direkten Kontakt zwischen zwei Oberflächen und senkt dadurch die Reibung erheblich."),
        ("Warum ist eine Flaschenzug-Konstruktion nützlich, um schwere Lasten zu heben?", new[] { "Sie verteilt die nötige Kraft auf mehrere Seilstränge und macht das Heben leichter", "Sie macht die Last selbst leichter", "Sie funktioniert nur bei sehr leichten Gegenständen" }, "Sie verteilt die nötige Kraft auf mehrere Seilstränge und macht das Heben leichter",
            "Ein Flaschenzug verteilt das Gewicht der Last auf mehrere Seilabschnitte, wodurch pro Seilzug weniger Kraft nötig ist."),
        ("Was zeigt die physikalische Einheit Newton (N) an?", new[] { "Die Größe einer Kraft", "Eine Temperatur", "Eine Zeitspanne" }, "Die Größe einer Kraft",
            "Newton ist die physikalische Einheit für Kraft, benannt nach dem Physiker Isaac Newton."),
        ("Warum braucht ein Auto auf nasser Straße einen längeren Bremsweg als auf trockener Straße?", new[] { "Nasse Straßen bieten weniger Reibung zwischen Reifen und Straße", "Nasse Straßen bieten mehr Reibung als trockene", "Regen hat keinen Einfluss auf den Bremsweg" }, "Nasse Straßen bieten weniger Reibung zwischen Reifen und Straße",
            "Der Wasserfilm auf nasser Fahrbahn verringert die Reibung zwischen Reifen und Straße, wodurch das Auto weniger stark gebremst wird."),
        ("Warum ist Fallschirmspringen mit geöffnetem Schirm sicherer als im freien Fall?", new[] { "Der Luftwiderstand wirkt als Gegenkraft und bremst die Fallgeschwindigkeit stark ab", "Der Schirm hat keinerlei Einfluss auf die Fallgeschwindigkeit", "Der Schirm beschleunigt den Fall zusätzlich" }, "Der Luftwiderstand wirkt als Gegenkraft und bremst die Fallgeschwindigkeit stark ab",
            "Die große Fläche des geöffneten Schirms erzeugt viel Luftwiderstand, der als Gegenkraft zur Schwerkraft wirkt und die Fallgeschwindigkeit deutlich verringert.")
    };

    private static QuizQuestion WechselwirkungUndKraft(Random r)
    {
        var f = WechselwirkungKraftListe[r.Next(WechselwirkungKraftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wechselwirkung und Kraft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Kräfte können beschleunigen, abbremsen oder verformen; zu jeder Kraft gehört eine gleich große Gegenkraft (Wechselwirkung), Reibung bremst Bewegung ab."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MechanischeEnergieListe =
    {
        ("Was versteht man unter \"Energie\" in der Physik ganz allgemein?", new[] { "Die Fähigkeit, Arbeit zu verrichten bzw. etwas zu bewegen oder zu verändern", "Ein anderes Wort für Kraft", "Eine Einheit für Temperatur" }, "Die Fähigkeit, Arbeit zu verrichten bzw. etwas zu bewegen oder zu verändern",
            "Energie beschreibt die Fähigkeit eines Systems, Arbeit zu verrichten, also z.B. etwas zu bewegen, zu erwärmen oder zu verformen."),
        ("Was ist Bewegungsenergie (kinetische Energie)?", new[] { "Die Energie, die ein sich bewegender Körper besitzt", "Die Energie eines Körpers aufgrund seiner Höhe", "Eine andere Bezeichnung für Wärme" }, "Die Energie, die ein sich bewegender Körper besitzt",
            "Jeder bewegte Körper besitzt Bewegungsenergie - je schneller er sich bewegt, desto mehr davon."),
        ("Was ist Lageenergie (potenzielle Energie)?", new[] { "Die Energie, die ein Körper aufgrund seiner Höhe/Lage besitzt", "Die Energie eines sich bewegenden Körpers", "Eine andere Bezeichnung für Reibung" }, "Die Energie, die ein Körper aufgrund seiner Höhe/Lage besitzt",
            "Ein Körper in großer Höhe besitzt Lageenergie, weil er beim Herunterfallen Arbeit verrichten könnte."),
        ("Ein Ball liegt oben auf einem Turm - welche Energieform besitzt er hauptsächlich?", new[] { "Lageenergie (potenzielle Energie)", "Bewegungsenergie", "Wärmeenergie" }, "Lageenergie (potenzielle Energie)",
            "Solange der Ball ruht, aber in großer Höhe liegt, besitzt er vor allem Lageenergie."),
        ("Was passiert mit der Lageenergie eines fallenden Balls, während er nach unten fällt?", new[] { "Sie wandelt sich zunehmend in Bewegungsenergie um", "Sie verschwindet vollständig", "Sie wird automatisch größer" }, "Sie wandelt sich zunehmend in Bewegungsenergie um",
            "Beim Fallen nimmt die Höhe (und damit die Lageenergie) ab, während die Geschwindigkeit (und damit die Bewegungsenergie) zunimmt."),
        ("Was versteht man unter \"mechanischer Arbeit\" in der Physik, vereinfacht?", new[] { "Man verrichtet Arbeit, wenn eine Kraft einen Körper über eine Strecke bewegt", "Arbeit bedeutet ausschließlich geistige Anstrengung", "Arbeit ist ein anderes Wort für Reibung" }, "Man verrichtet Arbeit, wenn eine Kraft einen Körper über eine Strecke bewegt",
            "Physikalische Arbeit entsteht, wenn eine Kraft auf einen Körper wirkt und ihn dabei über eine Strecke bewegt."),
        ("Wann verrichtet man laut Physik KEINE mechanische Arbeit, obwohl man sich anstrengt (z.B. beim Halten einer schweren Tasche ohne Bewegung)?", new[] { "Wenn sich der Körper trotz Kraftaufwand nicht bewegt", "Immer, wenn man eine schwere Last hält", "Nur, wenn man sich sehr langsam bewegt" }, "Wenn sich der Körper trotz Kraftaufwand nicht bewegt",
            "Physikalisch zählt nur Arbeit, bei der sich der Körper über eine Strecke bewegt - reines Halten ohne Bewegung verrichtet keine mechanische Arbeit."),
        ("Was passiert grundsätzlich mit Energie, wenn sie von einer Form in eine andere umgewandelt wird?", new[] { "Sie geht nicht verloren, sondern wandelt sich nur um", "Sie verschwindet vollständig", "Es entsteht automatisch mehr Energie" }, "Sie geht nicht verloren, sondern wandelt sich nur um",
            "Nach dem Energieerhaltungssatz geht Energie nicht verloren - sie wechselt lediglich ihre Form, z.B. von Lage- zu Bewegungsenergie."),
        ("Wozu kann man eine schiefe Ebene (Rampe) nutzen, um schwere Lasten hochzubringen?", new[] { "Um mit weniger Kraft, aber über einen längeren Weg, dieselbe Höhe zu erreichen", "Um die Last automatisch leichter zu machen", "Rampen haben keinerlei praktischen Nutzen" }, "Um mit weniger Kraft, aber über einen längeren Weg, dieselbe Höhe zu erreichen",
            "Eine Rampe verlängert den Weg nach oben, wodurch pro Meter weniger Kraft nötig ist, um dieselbe Höhe zu erreichen."),
        ("Was passiert mit der Energie eines Gegenstands, der nach einem Sturz auf dem Boden zur Ruhe kommt?", new[] { "Sie wandelt sich z.B. in Wärme und Schallenergie um", "Sie verschwindet vollständig aus dem Universum", "Sie bleibt unverändert als Bewegungsenergie erhalten" }, "Sie wandelt sich z.B. in Wärme und Schallenergie um",
            "Beim Aufprall wird die Bewegungsenergie in andere Energieformen wie Wärme (durch Verformung/Reibung) und Schall (der Aufprallknall) umgewandelt."),
        ("Warum braucht man mehr mechanische Arbeit, um einen schweren Koffer eine Treppe hochzutragen als einen leichten?", new[] { "Ein schwererer Koffer erfordert mehr Kraft für dieselbe Höhe", "Schwere Koffer benötigen grundsätzlich weniger Arbeit", "Das Gewicht des Koffers spielt dabei keine Rolle" }, "Ein schwererer Koffer erfordert mehr Kraft für dieselbe Höhe",
            "Da Arbeit von der aufgewendeten Kraft abhängt, braucht ein schwererer Koffer bei gleicher Höhe mehr Kraft und damit mehr Arbeit."),
        ("Was passiert mit der Bewegungsenergie eines rollenden Balls, wenn er durch Reibung langsamer wird?", new[] { "Sie wandelt sich in Wärmeenergie um", "Sie wird automatisch größer", "Sie verschwindet spurlos aus dem Universum" }, "Sie wandelt sich in Wärmeenergie um",
            "Reibung wandelt einen Teil der Bewegungsenergie in Wärme um, wodurch der Ball nach und nach langsamer wird."),
        ("Was ist ein einfaches Beispiel für Lageenergie im Alltag?", new[] { "Wasser in einem hoch gelegenen Stausee", "Ein rollender Ball auf ebener Straße", "Ein sich erwärmender Kochtopf" }, "Wasser in einem hoch gelegenen Stausee",
            "Das in großer Höhe gespeicherte Wasser eines Stausees besitzt Lageenergie, die beim Herabfließen genutzt werden kann."),
        ("Warum kann in einem Wasserkraftwerk aus der Lageenergie des Wassers Strom erzeugt werden?", new[] { "Das herabfließende Wasser treibt Turbinen an, die Energie in Strom umwandeln", "Wasser erzeugt Strom durch seine Farbe", "Lageenergie kann nicht in Strom umgewandelt werden" }, "Das herabfließende Wasser treibt Turbinen an, die Energie in Strom umwandeln",
            "Das von oben herabstürzende Wasser wandelt seine Lage- in Bewegungsenergie um, die Turbinen antreibt, welche wiederum elektrischen Strom erzeugen."),
        ("Wie verändert sich die Bewegungsenergie eines Autos, wenn es schneller fährt?", new[] { "Sie nimmt deutlich zu", "Sie bleibt exakt gleich", "Sie nimmt ab" }, "Sie nimmt deutlich zu",
            "Je schneller sich ein Körper bewegt, desto größer ist seine Bewegungsenergie."),
        ("Was passiert mit der gesamten mechanischen Energie eines fallenden Balls ohne Luftwiderstand, näherungsweise?", new[] { "Sie bleibt insgesamt ungefähr gleich, wechselt nur die Form", "Sie steigt immer weiter an", "Sie sinkt kontinuierlich auf null" }, "Sie bleibt insgesamt ungefähr gleich, wechselt nur die Form",
            "Ohne Luftwiderstand bleibt die Summe aus Lage- und Bewegungsenergie beim Fallen näherungsweise konstant, nur ihr Anteil verschiebt sich."),
        ("Warum wird beim Bremsen eines Autos Energie in Wärme umgewandelt?", new[] { "Reibung zwischen Bremsen und Rädern wandelt Bewegungsenergie in Wärmeenergie um", "Die Bremsen erzeugen dabei elektrischen Strom", "Beim Bremsen entsteht keinerlei Energieumwandlung" }, "Reibung zwischen Bremsen und Rädern wandelt Bewegungsenergie in Wärmeenergie um",
            "Die Bremsen erzeugen durch Reibung Wärme, wodurch die Bewegungsenergie des Autos abgebaut wird und es langsamer wird."),
        ("Wofür ist ein Trampolin ein gutes Beispiel in Bezug auf Energieumwandlung?", new[] { "Umwandlung von Bewegungsenergie in gespannte Energie des Sprungtuchs und wieder zurück", "Umwandlung von Lageenergie in elektrischen Strom", "Ein Trampolin hat mit Energie nichts zu tun" }, "Umwandlung von Bewegungsenergie in gespannte Energie des Sprungtuchs und wieder zurück",
            "Beim Aufkommen wird die Bewegungsenergie im gespannten Sprungtuch gespeichert und beim Zurückschnellen wieder in Bewegungsenergie umgewandelt."),
        ("Was zeigt die physikalische Einheit Joule (J) an?", new[] { "Eine Menge an Energie oder Arbeit", "Eine Temperatur", "Eine Kraft" }, "Eine Menge an Energie oder Arbeit",
            "Joule ist die physikalische Einheit für Energie und Arbeit, benannt nach dem Physiker James Prescott Joule."),
        ("Was passiert mit der gespannten (elastischen) Energie einer zurückgezogenen Steinschleuder, wenn man sie loslässt?", new[] { "Sie wandelt sich in Bewegungsenergie des Geschosses um", "Sie verschwindet spurlos", "Sie wandelt sich in Lageenergie um, ohne dass sich etwas bewegt" }, "Sie wandelt sich in Bewegungsenergie des Geschosses um",
            "Die im gespannten Gummiband gespeicherte elastische Energie wird beim Loslassen in Bewegungsenergie des abgeschossenen Gegenstands umgewandelt.")
    };

    private static QuizQuestion MechanischeEnergieUndArbeit(Random r)
    {
        var f = MechanischeEnergieListe[r.Next(MechanischeEnergieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Mechanische Energie und Arbeit", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Lageenergie (Höhe) und Bewegungsenergie (Geschwindigkeit) wandeln sich ständig ineinander um; Energie geht nie verloren, sondern wechselt nur die Form (auch in Wärme durch Reibung)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ThermischeEnergieListe =
    {
        ("Was ist Wärme in der Physik, einfach erklärt?", new[] { "Eine Form von Energie, die von einem wärmeren zu einem kälteren Körper übergeht", "Ein anderes Wort für Temperatur", "Eine Kraft, die Körper anzieht" }, "Eine Form von Energie, die von einem wärmeren zu einem kälteren Körper übergeht",
            "Wärme ist Energie, die immer vom wärmeren zum kälteren Körper fließt, bis ein Temperaturausgleich erreicht ist."),
        ("In welche Richtung fließt Wärme immer von selbst?", new[] { "Vom wärmeren zum kälteren Körper", "Vom kälteren zum wärmeren Körper", "Wärme fließt nie von selbst" }, "Vom wärmeren zum kälteren Körper",
            "Ohne äußere Einwirkung (z.B. einen Kühlschrank) fließt Wärme immer nur vom wärmeren zum kälteren Körper."),
        ("Wie nennt man die Wärmeübertragung durch direkten Kontakt zweier Körper, z.B. eine heiße Herdplatte, die einen Topf erwärmt?", new[] { "Wärmeleitung", "Wärmestrahlung", "Wärmeströmung" }, "Wärmeleitung",
            "Bei der Wärmeleitung wird Wärme durch direkten Kontakt von einem Körper auf den anderen übertragen."),
        ("Wie nennt man die Wärmeübertragung durch Strömung, z.B. wenn warme Luft in einem Raum aufsteigt?", new[] { "Wärmeströmung (Konvektion)", "Wärmeleitung", "Sublimation" }, "Wärmeströmung (Konvektion)",
            "Bei der Konvektion transportiert eine strömende Flüssigkeit oder ein strömendes Gas die Wärme mit sich - warme Luft steigt dabei nach oben."),
        ("Wie nennt man die Wärmeübertragung ganz ohne Materie, z.B. die Wärme der Sonne, die durchs Weltall zur Erde gelangt?", new[] { "Wärmestrahlung", "Wärmeleitung", "Wärmeströmung" }, "Wärmestrahlung",
            "Wärmestrahlung braucht kein Trägermedium und funktioniert deshalb auch durch das Vakuum des Weltalls, z.B. bei der Sonnenwärme."),
        ("Warum eignen sich Metalle gut, um Wärme schnell weiterzuleiten, z.B. bei einem Kochlöffel?", new[] { "Metalle sind gute Wärmeleiter", "Metalle sind besonders schlechte Wärmeleiter", "Metalle haben keinerlei Bezug zu Wärme" }, "Metalle sind gute Wärmeleiter",
            "Metalle leiten Wärme sehr effizient weiter - deshalb wird ein Metalllöffel im heißen Topf schnell selbst heiß."),
        ("Warum benutzt man beim Kochen oft Griffe aus Holz oder Kunststoff statt aus Metall?", new[] { "Holz und Kunststoff sind schlechte Wärmeleiter und werden nicht so heiß", "Holz und Kunststoff leiten Wärme besser als Metall", "Aus rein optischen Gründen" }, "Holz und Kunststoff sind schlechte Wärmeleiter und werden nicht so heiß",
            "Da Holz und Kunststoff Wärme schlecht leiten, bleiben Griffe daraus auch bei heißem Topf angenehm anfassbar."),
        ("Warum hält eine Daunenjacke im Winter warm, obwohl sie selbst keine Wärme erzeugt?", new[] { "Die eingeschlossene Luft zwischen den Federn ist ein schlechter Wärmeleiter und hält Körperwärme zurück", "Die Daunenjacke erzeugt selbst chemisch Wärme", "Daunenjacken funktionieren nur bei Sonnenschein" }, "Die eingeschlossene Luft zwischen den Federn ist ein schlechter Wärmeleiter und hält Körperwärme zurück",
            "Die vielen kleinen Luftpolster zwischen den Daunenfedern isolieren gut und verhindern, dass die eigene Körperwärme nach außen entweicht."),
        ("Was passiert mit der Temperatur zweier unterschiedlich warmer Körper, die lange in Kontakt bleiben?", new[] { "Sie gleichen sich mit der Zeit an (Temperaturausgleich)", "Der Unterschied wird immer größer", "Die Temperaturen ändern sich überhaupt nicht" }, "Sie gleichen sich mit der Zeit an (Temperaturausgleich)",
            "Wärme fließt so lange vom wärmeren zum kälteren Körper, bis beide dieselbe Temperatur erreicht haben."),
        ("Was misst ein Thermometer?", new[] { "Die Temperatur", "Die Kraft eines Gegenstands", "Die Masse eines Gegenstands" }, "Die Temperatur",
            "Ein Thermometer zeigt an, wie warm oder kalt etwas gerade ist - also die Temperatur."),
        ("Was ist der Unterschied zwischen Wärme und Temperatur, vereinfacht?", new[] { "Wärme ist übertragene Energie, Temperatur zeigt an, wie warm/kalt etwas gerade ist", "Beide Begriffe bedeuten exakt dasselbe", "Temperatur ist eine Kraft, Wärme eine Strecke" }, "Wärme ist übertragene Energie, Temperatur zeigt an, wie warm/kalt etwas gerade ist",
            "Wärme beschreibt eine Energiemenge, die übertragen wird, während Temperatur den momentanen Zustand (wie warm etwas ist) angibt."),
        ("Warum kühlt eine Tasse heißer Tee mit der Zeit von selbst ab?", new[] { "Sie gibt Wärme an die kühlere Umgebungsluft ab, bis ein Ausgleich erreicht ist", "Die Tasse zieht selbst Kälte aus der Luft an", "Tee kühlt grundsätzlich nie von selbst ab" }, "Sie gibt Wärme an die kühlere Umgebungsluft ab, bis ein Ausgleich erreicht ist",
            "Da die Umgebungsluft kälter ist als der Tee, fließt Wärme vom Tee zur Luft, bis sich die Temperaturen angeglichen haben."),
        ("Warum friert man an einem kalten, windigen Tag stärker als bei gleicher Temperatur ohne Wind?", new[] { "Der Wind trägt die erwärmte Luftschicht um den Körper schneller weg", "Wind erhöht die tatsächliche Lufttemperatur", "Wind hat auf das Kälteempfinden keinerlei Einfluss" }, "Der Wind trägt die erwärmte Luftschicht um den Körper schneller weg",
            "Wind entfernt ständig die durch den Körper erwärmte Luftschicht, wodurch man schneller Wärme verliert und stärker friert (Windchill-Effekt)."),
        ("Wie funktioniert eine Thermoskanne, um Getränke lange warm zu halten?", new[] { "Sie verhindert weitgehend die Wärmeübertragung nach außen, z.B. durch ein Vakuum", "Sie erzeugt selbst zusätzliche Wärme", "Sie funktioniert nur bei kalten Getränken" }, "Sie verhindert weitgehend die Wärmeübertragung nach außen, z.B. durch ein Vakuum",
            "Das Vakuum bzw. die isolierenden Schichten einer Thermoskanne verhindern weitgehend Wärmeleitung, -strömung und -strahlung nach außen."),
        ("Was passiert mit den Teilchen eines Stoffs, wenn er Wärme aufnimmt?", new[] { "Sie bewegen sich schneller", "Sie bewegen sich langsamer", "Sie hören auf, sich zu bewegen" }, "Sie bewegen sich schneller",
            "Wärmeaufnahme erhöht die Bewegungsenergie der Teilchen - sie bewegen sich schneller, was sich als höhere Temperatur zeigt."),
        ("Warum fühlt sich Metall im selben Raum oft kälter an als Holz, obwohl beide dieselbe Raumtemperatur haben?", new[] { "Metall leitet Wärme von der Hand schneller ab als Holz", "Metall hat tatsächlich immer eine niedrigere Temperatur", "Es ist reine Einbildung ohne physikalischen Grund" }, "Metall leitet Wärme von der Hand schneller ab als Holz",
            "Da Metall Wärme sehr gut leitet, entzieht es der Hand schneller Wärme als Holz - das fühlt sich kälter an, obwohl beide gleich temperiert sind."),
        ("Wie nennt man das Gerät, das die Umgebungstemperatur in einem Raum automatisch regelt?", new[] { "Thermostat", "Federwaage", "Bimetallstreifen" }, "Thermostat",
            "Ein Thermostat misst die Raumtemperatur und regelt automatisch die Heizung, um eine gewünschte Temperatur zu halten."),
        ("Warum werden Häuser oft mit Dämmmaterial an Wänden und Dach ausgestattet?", new[] { "Um den Wärmeverlust nach außen im Winter zu verringern", "Um das Haus schwerer zu machen", "Dämmmaterial hat keinerlei Effekt auf Wärme" }, "Um den Wärmeverlust nach außen im Winter zu verringern",
            "Eine gute Dämmung verringert die Wärmeleitung nach außen, sodass im Winter weniger Heizenergie verloren geht."),
        ("Was passiert mit Eiswürfeln in einem Getränk bei Zimmertemperatur?", new[] { "Sie nehmen Wärme aus dem Getränk und der Umgebung auf und schmelzen dabei", "Sie geben selbst Wärme an das Getränk ab", "Sie bleiben unbegrenzt lange fest" }, "Sie nehmen Wärme aus dem Getränk und der Umgebung auf und schmelzen dabei",
            "Da das Getränk wärmer ist als die Eiswürfel, fließt Wärme zu den Eiswürfeln, die dadurch schmelzen und gleichzeitig das Getränk abkühlen."),
        ("Warum beschlägt ein kaltes Fensterglas im Winter von innen, wenn warme, feuchte Raumluft dagegen strömt?", new[] { "Die Luft kühlt am kalten Glas ab und der Wasserdampf kondensiert zu sichtbaren Tröpfchen", "Das Glas selbst produziert Wassertropfen", "Kalte Fenster stoßen grundsätzlich Wasser ab" }, "Die Luft kühlt am kalten Glas ab und der Wasserdampf kondensiert zu sichtbaren Tröpfchen",
            "Trifft warme, feuchte Luft auf die kalte Scheibe, kühlt sie dort stark ab - der enthaltene Wasserdampf kondensiert und wird als Beschlag sichtbar.")
    };

    private static QuizQuestion ThermischeEnergieUndWaerme(Random r)
    {
        var f = ThermischeEnergieListe[r.Next(ThermischeEnergieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Physik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Thermische Energie und Wärme", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wärme fließt immer vom wärmeren zum kälteren Körper - über Leitung (Kontakt), Strömung (Konvektion) oder Strahlung (ohne Materie); gute Wärmeleiter wie Metall fühlen sich kälter/heißer an als Isolatoren."
        };
    }
}
