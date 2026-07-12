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
}
