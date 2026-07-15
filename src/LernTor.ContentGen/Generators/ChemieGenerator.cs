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
            [GradeLevel.Klasse6] = new List<TopicFactory> { StoffeTrennen, Verbrennung, SaeurenLaugen, MetalleEigenschaften, StoffeImAlltag },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Atommodell, ChemischeReaktion, Periodensystem, Stoechiometrie, SaeureBaseVertieft, Kohlenwasserstoffe, Alkohole, OrganischeSaeuren, Ester }
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
            "Destillation", "Bei der Destillation verdampft die Flüssigkeit mit dem niedrigeren Siedepunkt zuerst und wird getrennt aufgefangen."),
        ("Wie kann man mehrere Farbstoffe in einem schwarzen Filzstift sichtbar voneinander trennen?", new[] { "Chromatographie", "Filtration", "Magnet verwenden" },
            "Chromatographie", "Bei der Chromatographie wandern die verschiedenen Farbstoffe unterschiedlich schnell mit einem Lösungsmittel, wodurch sie sichtbar getrennt werden."),
        ("Wie nennt man es, wenn sich schwere Feststoffteilchen (z.B. Sand) am Boden eines Wasserglases absetzen?", new[] { "Sedimentation", "Destillation", "Sublimation" },
            "Sedimentation", "Bei der Sedimentation setzen sich schwerere, unlösliche Teilchen durch die Schwerkraft am Boden ab."),
        ("Wie trennt man ein Gemisch aus grobem Kies und feinem Sand am einfachsten?", new[] { "Sieben", "Verdampfen", "Destillation" },
            "Sieben", "Ein Sieb lässt die kleineren Sandkörner durch, während der größere Kies zurückbleibt."),
        ("Wie kann man festes Iod aus einem Gemisch mit anderen Feststoffen trennen (Iod sublimiert leicht)?", new[] { "Erhitzen, sodass das Iod direkt zu Gas sublimiert und andernorts wieder fest wird", "Filtration", "Magnet verwenden" },
            "Erhitzen, sodass das Iod direkt zu Gas sublimiert und andernorts wieder fest wird", "Iod geht beim Erhitzen direkt vom festen in den gasförmigen Zustand über (Sublimation) und lässt sich so von anderen Feststoffen trennen."),
        ("Wie können z.B. Blutbestandteile mit unterschiedlicher Dichte im Labor getrennt werden?", new[] { "Durch Zentrifugieren (schnelles Rotieren)", "Durch einfaches Stehenlassen über Wochen", "Durch Erhitzen auf 100°C" },
            "Durch Zentrifugieren (schnelles Rotieren)", "Beim Zentrifugieren trennt die schnelle Rotation Bestandteile unterschiedlicher Dichte voneinander."),
        ("Wie nennt man es, wenn ein Stoff mithilfe eines Lösungsmittels aus einem Gemisch herausgelöst wird (z.B. Kaffee aus Kaffeepulver)?", new[] { "Extraktion", "Sedimentation", "Sieben" },
            "Extraktion", "Bei der Extraktion löst ein Lösungsmittel (z.B. heißes Wasser) gezielt einen bestimmten Stoff aus einem Gemisch heraus."),
        ("Wie kann Aktivkohle bei der Wasserreinigung helfen?", new[] { "Sie bindet bestimmte Schadstoffe an ihrer großen Oberfläche (Adsorption)", "Sie färbt das Wasser blau", "Sie erhöht die Temperatur des Wassers" },
            "Sie bindet bestimmte Schadstoffe an ihrer großen Oberfläche (Adsorption)", "Aktivkohle hat eine sehr große, poröse Oberfläche, an der sich viele Schadstoffe anlagern (adsorbieren) können."),
        ("Warum eignet sich einfache Filtration nicht, um gelöstes Salz aus Wasser zu trennen?", new[] { "Weil gelöste Teilchen so klein sind, dass sie durch den Filter hindurchgehen", "Weil Salz magnetisch ist", "Weil Wasser den Filter zerstört" },
            "Weil gelöste Teilchen so klein sind, dass sie durch den Filter hindurchgehen", "Gelöste Salzteilchen sind viel kleiner als die Poren eines Filters und passieren ihn ungehindert - hier hilft nur Verdampfen."),
        ("Warum kann man Alkohol und Wasser durch Destillation voneinander trennen?", new[] { "Weil Alkohol einen niedrigeren Siedepunkt hat und zuerst verdampft", "Weil Alkohol schwerer ist als Wasser", "Weil Alkohol magnetisch ist" },
            "Weil Alkohol einen niedrigeren Siedepunkt hat und zuerst verdampft", "Alkohol siedet bei einer niedrigeren Temperatur als Wasser und verdampft deshalb bei der Destillation zuerst."),
        ("Was passiert mit dem Wasser beim Eindampfen einer Salzlösung?", new[] { "Es verdampft, das Salz bleibt als Feststoff zurück", "Es wird zu Eis", "Es verwandelt sich chemisch in Salz" },
            "Es verdampft, das Salz bleibt als Feststoff zurück", "Beim Erhitzen verdunstet das Wasser, während das gelöste Salz als fester Rückstand zurückbleibt."),
        ("Wie kann man Kupferspäne und Eisenspäne aus einem Gemisch trennen?", new[] { "Mit einem Magneten, da nur Eisen magnetisch ist", "Durch Filtration", "Durch Destillation" },
            "Mit einem Magneten, da nur Eisen magnetisch ist", "Eisen wird von einem Magneten angezogen, Kupfer nicht - so lassen sich beide Metalle trennen."),
        ("Wie wird Rohöl in seine verschiedenen Bestandteile wie Benzin und Diesel getrennt?", new[] { "Durch fraktionierte Destillation nach unterschiedlichen Siedepunkten", "Durch Filtration", "Durch Sieben" },
            "Durch fraktionierte Destillation nach unterschiedlichen Siedepunkten", "Die verschiedenen Bestandteile des Rohöls haben unterschiedliche Siedepunkte und werden bei der fraktionierten Destillation stufenweise getrennt."),
        ("Was ist der Unterschied zwischen einem homogenen und einem heterogenen Gemisch?", new[] { "Homogen ist gleichmäßig vermischt, bei heterogen sind Bestandteile sichtbar getrennt", "Beide Begriffe bedeuten dasselbe", "Homogen bedeutet, dass gar keine Mischung vorliegt" },
            "Homogen ist gleichmäßig vermischt, bei heterogen sind Bestandteile sichtbar getrennt", "Salzwasser ist z.B. ein homogenes Gemisch (gleichmäßig gelöst), Sand in Wasser ein heterogenes Gemisch (sichtbar getrennte Bestandteile)."),
        ("Wie trennt man unlösliches Kreidepulver von Wasser?", new[] { "Filtration", "Verdampfen", "Magnet verwenden" },
            "Filtration", "Da sich Kreidepulver nicht im Wasser löst, bleibt es beim Filtrieren im Filter zurück."),
        ("Was ist ein Beispiel für ein heterogenes Gemisch, bei dem man die Bestandteile mit bloßem Auge erkennt?", new[] { "Sand in Wasser", "Salzwasser", "Zuckerwasser" },
            "Sand in Wasser", "Bei Sand in Wasser sind die einzelnen Bestandteile sichtbar getrennt - ein typisches heterogenes Gemisch.")
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
            "Kohlenstoffdioxid (CO₂)", "Der im Brennstoff enthaltene Kohlenstoff verbindet sich bei der Verbrennung mit Sauerstoff zu Kohlenstoffdioxid."),
        ("Was ist die \"Zündtemperatur\" eines Stoffes?", new[] { "Die Mindesttemperatur, die ein Stoff braucht, damit er zu brennen beginnt", "Die Temperatur, bei der ein Stoff schmilzt", "Die Temperatur, bei der ein Stoff gefriert" },
            "Die Mindesttemperatur, die ein Stoff braucht, damit er zu brennen beginnt", "Erst ab der Zündtemperatur reagiert ein Brennstoff schnell genug mit Sauerstoff, um eine Verbrennung zu starten."),
        ("Warum ist reiner Sauerstoff besonders gefährlich in Bezug auf Feuer?", new[] { "Er lässt Stoffe viel heftiger und schneller brennen", "Er verhindert jede Verbrennung", "Er kühlt brennende Stoffe sofort ab" },
            "Er lässt Stoffe viel heftiger und schneller brennen", "Mit reinem Sauerstoff statt normaler Luft verläuft eine Verbrennung deutlich intensiver und schneller."),
        ("Was entsteht bei einer unvollständigen Verbrennung (zu wenig Sauerstoff)?", new[] { "Giftiges Kohlenstoffmonoxid (CO) statt CO₂", "Ausschließlich reiner Sauerstoff", "Ausschließlich Wasser" },
            "Giftiges Kohlenstoffmonoxid (CO) statt CO₂", "Bei Sauerstoffmangel entsteht statt des ungiftigen CO₂ das giftige Kohlenstoffmonoxid (CO)."),
        ("Warum ist Kohlenstoffmonoxid (CO) für Menschen besonders gefährlich?", new[] { "Es ist geruchlos, farblos und blockiert den Sauerstofftransport im Blut", "Es riecht sehr stark nach Rauch und ist leicht zu bemerken", "Es ist völlig ungiftig" },
            "Es ist geruchlos, farblos und blockiert den Sauerstofftransport im Blut", "CO bindet sich stärker an den roten Blutfarbstoff als Sauerstoff und wird dabei weder gerochen noch gesehen - das macht es besonders tückisch."),
        ("Wie wirkt Wasser als Löschmittel bei einem normalen Holzbrand?", new[] { "Es kühlt den Brennstoff unter die Zündtemperatur", "Es entzieht dem Feuer nur die Farbe", "Es macht das Holz feuerfest für immer" },
            "Es kühlt den Brennstoff unter die Zündtemperatur", "Wasser entzieht dem brennenden Material Wärme und senkt die Temperatur unter die Zündtemperatur."),
        ("Warum darf man einen Fettbrand in der Küche niemals mit Wasser löschen?", new[] { "Das Wasser verdampft explosionsartig und reißt brennendes Fett in die Luft", "Wasser macht das Feuer sofort komplett ungefährlich", "Wasser hat auf Fettbrände überhaupt keine Wirkung" },
            "Das Wasser verdampft explosionsartig und reißt brennendes Fett in die Luft", "Trifft Wasser auf heißes, brennendes Fett, verdampft es schlagartig und schleudert brennendes Fett explosionsartig umher (Fettexplosion)."),
        ("Was unterscheidet Glut von einer offenen Flamme?", new[] { "Glut glimmt ohne sichtbare Flamme, oft bei niedrigerer Temperatur", "Glut ist immer heißer als jede Flamme", "Es gibt keinen Unterschied"}, "Glut glimmt ohne sichtbare Flamme, oft bei niedrigerer Temperatur",
            "Glut entsteht z.B. bei glimmender Kohle - es brennt ohne sichtbare Flamme, oft langsamer und bei niedrigerer Temperatur."),
        ("Wie heißt der Vorgang, bei dem Metall langsam mit Sauerstoff reagiert, z.B. beim Rosten?", new[] { "Eine langsame Oxidation", "Eine Reduktion", "Eine Neutralisation" },
            "Eine langsame Oxidation", "Rosten ist eine langsam ablaufende Oxidation von Eisen mit Sauerstoff und Feuchtigkeit."),
        ("Was ist notwendig, damit ein Streichholz beim Reiben zu brennen beginnt?", new[] { "Reibungswärme, die die Zündtemperatur erreicht", "Ein starker Magnet", "Kaltes Wasser" },
            "Reibungswärme, die die Zündtemperatur erreicht", "Die Reibung beim Anzünden erzeugt genug Wärme, um die Zündtemperatur der Streichholzspitze zu erreichen."),
        ("Was zeigt die Flammenfarbe bei einer Verbrennung oft an?", new[] { "Die Temperatur bzw. teils die beteiligten chemischen Elemente", "Ausschließlich die Windrichtung", "Ausschließlich die Uhrzeit" },
            "Die Temperatur bzw. teils die beteiligten chemischen Elemente", "Flammenfarben können Hinweise auf Temperatur und auf bestimmte enthaltene Elemente geben (z.B. bei Feuerwerk)."),
        ("Warum verbrennt trockenes Holz besser als feuchtes Holz?", new[] { "Feuchtigkeit muss erst verdampfen, bevor das Holz die Zündtemperatur erreichen kann", "Feuchtes Holz enthält mehr Sauerstoff", "Trockenes Holz enthält kein Kohlenstoff" },
            "Feuchtigkeit muss erst verdampfen, bevor das Holz die Zündtemperatur erreichen kann", "Bei feuchtem Holz wird zunächst Energie zum Verdampfen des Wassers verbraucht, bevor das Holz überhaupt heiß genug zum Brennen wird."),
        ("Was ist eine Explosion, chemisch betrachtet, vereinfacht?", new[] { "Eine extrem schnelle Verbrennung mit plötzlicher Freisetzung von Gas und Energie", "Ein langsamer, kontrollierter Verbrennungsvorgang", "Ein rein physikalischer Vorgang ohne chemische Reaktion" },
            "Eine extrem schnelle Verbrennung mit plötzlicher Freisetzung von Gas und Energie", "Bei einer Explosion läuft eine Verbrennungsreaktion extrem schnell ab und setzt schlagartig große Mengen Gas und Energie frei."),
        ("Warum ist feiner Mehlstaub in einer Mühle explosionsgefährlich?", new[] { "Feine Staubpartikel haben eine sehr große Oberfläche und können explosionsartig verbrennen", "Mehl enthält gar keinen brennbaren Kohlenstoff", "Mehlstaub kann unter keinen Umständen brennen" },
            "Feine Staubpartikel haben eine sehr große Oberfläche und können explosionsartig verbrennen", "Fein verteilter, brennbarer Staub in der Luft hat eine riesige Gesamtoberfläche und kann bei einem Funken explosionsartig reagieren."),
        ("Wozu dient die Einteilung in \"Brandklassen\" bei Feuerlöschern?", new[] { "Damit für die jeweilige Brennstoffart das passende Löschmittel gewählt wird", "Um die Feuerwehr über die Farbe des Feuers zu informieren", "Um die Kosten eines Feuerlöschers festzulegen" },
            "Damit für die jeweilige Brennstoffart das passende Löschmittel gewählt wird", "Verschiedene Brandklassen (z.B. feste Stoffe, Flüssigkeiten, Fette) benötigen unterschiedliche Löschmittel, um sicher und wirksam gelöscht zu werden."),
        ("Was versteht man unter dem \"Verbrennungsdreieck\"?", new[] { "Die drei notwendigen Voraussetzungen Brennstoff, Sauerstoff und Zündtemperatur", "Eine geometrische Form, die Flammen typischerweise annehmen", "Ein Werkzeug der Feuerwehr zum Löschen" },
            "Die drei notwendigen Voraussetzungen Brennstoff, Sauerstoff und Zündtemperatur", "Das Verbrennungsdreieck veranschaulicht, dass für ein Feuer immer alle drei Voraussetzungen gleichzeitig vorhanden sein müssen.")
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
            "Sie können Haut, Augen und Kleidung stark schädigen (ätzend)", "Konzentrierte Säuren und Laugen sind ätzend und können Verätzungen verursachen - deshalb gelten im Unterricht besondere Schutzmaßnahmen."),
        ("Welche Farbe zeigt Universalindikator-Papier bei einer starken Säure typischerweise an?", new[] { "Rot", "Blau", "Grün" }, "Rot",
            "Starke Säuren mit niedrigem pH-Wert färben Universalindikator-Papier rot."),
        ("Welche Farbe zeigt Universalindikator-Papier bei einer starken Lauge typischerweise an?", new[] { "Blau/Violett", "Rot", "Orange" }, "Blau/Violett",
            "Starke Laugen mit hohem pH-Wert färben Universalindikator-Papier blau bis violett."),
        ("Welche bekannte Säure ist im Magen zur Verdauung enthalten?", new[] { "Salzsäure", "Essigsäure", "Zitronensäure" }, "Salzsäure",
            "Der Magen produziert Salzsäure, die bei der Verdauung hilft und Krankheitserreger abtötet."),
        ("Welche bekannte Säure ist in Essig enthalten?", new[] { "Essigsäure", "Salzsäure", "Schwefelsäure" }, "Essigsäure",
            "Essig enthält Essigsäure, die ihm seinen charakteristischen, sauren Geschmack und Geruch verleiht."),
        ("Welche bekannte Säure ist in Zitronen enthalten?", new[] { "Zitronensäure", "Salzsäure", "Kohlensäure" }, "Zitronensäure",
            "Zitronensäure ist für den typisch sauren Geschmack von Zitrusfrüchten verantwortlich."),
        ("Was ist eine Lauge chemisch grob gesehen, im Gegensatz zu einer Säure?", new[] { "Eine basische Lösung mit einem pH-Wert über 7", "Eine Lösung mit einem pH-Wert unter 7", "Eine Lösung ohne jeden pH-Wert" }, "Eine basische Lösung mit einem pH-Wert über 7",
            "Laugen sind basische (alkalische) Lösungen mit einem pH-Wert über 7 - das Gegenteil von sauren Lösungen."),
        ("Was entsteht häufig, wenn eine Säure mit einem unedlen Metall wie Zink reagiert?", new[] { "Wasserstoffgas und ein Salz", "Sauerstoffgas und Wasser", "Nur reines Wasser" }, "Wasserstoffgas und ein Salz",
            "Reagiert eine Säure mit einem unedlen Metall, entstehen typischerweise Wasserstoffgas und das entsprechende Salz."),
        ("Was passiert mit blauem Lackmuspapier, wenn man es in eine Säure taucht?", new[] { "Es färbt sich rot", "Es bleibt blau", "Es wird durchsichtig" }, "Es färbt sich rot",
            "Lackmuspapier ist ein klassischer Säure-Base-Indikator: Säuren färben blaues Lackmuspapier rot."),
        ("Was passiert mit rotem Lackmuspapier, wenn man es in eine Lauge taucht?", new[] { "Es färbt sich blau", "Es bleibt rot", "Es wird schwarz" }, "Es färbt sich blau",
            "Laugen färben rotes Lackmuspapier blau - das umgekehrte Verhalten zu Säuren."),
        ("Wie heißt ein bekanntes Reinigungsmittel, das stark basisch (laugenhaft) ist?", new[] { "Rohrreiniger (Abflussreiniger)", "Essigreiniger", "Zitronensäurereiniger" }, "Rohrreiniger (Abflussreiniger)",
            "Viele Abflussreiniger enthalten stark alkalische (basische) Substanzen, die Fett und organische Verstopfungen auflösen."),
        ("Warum sollte man Säuren und Laugen im Labor niemals ohne Schutzbrille verwenden?", new[] { "Sie können bei Spritzern die Augen schwer verletzen", "Schutzbrillen sind nur ein modisches Accessoire", "Säuren und Laugen sind völlig ungefährlich für die Augen" }, "Sie können bei Spritzern die Augen schwer verletzen",
            "Spritzer von Säuren oder Laugen können schwere, teils bleibende Augenschäden verursachen - eine Schutzbrille ist deshalb Pflicht."),
        ("Was passiert, wenn man mit einer verdünnten statt einer konzentrierten Säure arbeitet?", new[] { "Die Reaktion/Ätzwirkung ist deutlich schwächer und ungefährlicher", "Die Ätzwirkung wird automatisch stärker", "Es ändert sich überhaupt nichts" }, "Die Reaktion/Ätzwirkung ist deutlich schwächer und ungefährlicher",
            "Verdünnte Säuren enthalten weniger Säureteilchen pro Volumen und reagieren daher schwächer und weniger gefährlich als konzentrierte."),
        ("Welche Zahl auf der pH-Skala steht für die stärkste Säure?", new[] { "0", "7", "14" }, "0",
            "Die pH-Skala reicht von 0 (stark sauer) bis 14 (stark basisch) - je näher an 0, desto saurer."),
        ("Welche Zahl auf der pH-Skala steht für die stärkste Lauge?", new[] { "14", "0", "7" }, "14",
            "Die pH-Skala reicht von 0 (stark sauer) bis 14 (stark basisch) - je näher an 14, desto basischer."),
        ("Was passiert bei einer Neutralisation genau?", new[] { "Säure und Lauge reagieren zu einem Salz und Wasser", "Säure und Lauge verdampfen beide sofort", "Es entsteht ausschließlich reine Säure" }, "Säure und Lauge reagieren zu einem Salz und Wasser",
            "Bei der Neutralisation reagieren die Wasserstoff-Ionen der Säure mit den Hydroxid-Ionen der Lauge zu Wasser, übrig bleibt ein Salz.")
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
            "Metalle leiten Wärme gut und sind stabil/langlebig", "Die gute Wärmeleitfähigkeit und Stabilität von Metallen macht sie ideal für Kochgeschirr und Besteck."),
        ("Was ist eine \"Legierung\"?", new[] { "Eine Mischung aus zwei oder mehr Metallen (oder Metall und Nichtmetall)", "Ein reines, unvermischtes Metall", "Ein anderes Wort für Erz" }, "Eine Mischung aus zwei oder mehr Metallen (oder Metall und Nichtmetall)",
            "Legierungen entstehen durch das gezielte Zusammenschmelzen von Metallen, um neue, oft verbesserte Eigenschaften zu erhalten."),
        ("Was ist ein bekanntes Beispiel für eine Legierung?", new[] { "Bronze (Kupfer und Zinn)", "Reines Gold", "Reiner Sauerstoff" }, "Bronze (Kupfer und Zinn)",
            "Bronze ist eine der ältesten bekannten Legierungen und besteht hauptsächlich aus Kupfer und Zinn."),
        ("Warum wird Aluminium oft für Flugzeuge oder Fahrräder verwendet?", new[] { "Es ist leicht und trotzdem stabil", "Es ist das schwerste bekannte Metall", "Es leitet keinen Strom" }, "Es ist leicht und trotzdem stabil",
            "Aluminium hat ein geringes Gewicht bei gleichzeitig guter Stabilität - ideal für Fahrzeuge, bei denen Gewicht wichtig ist."),
        ("Welches Edelmetall wird oft für Schmuck verwendet, weil es nicht rostet?", new[] { "Gold", "Eisen", "Zink" }, "Gold",
            "Gold ist sehr widerstandsfähig gegen Korrosion und wird deshalb seit Jahrtausenden für Schmuck verwendet."),
        ("Was passiert mit den meisten Metallen, wenn man sie stark erhitzt?", new[] { "Sie schmelzen und werden flüssig", "Sie verschwinden spurlos", "Sie werden automatisch magnetisch" }, "Sie schmelzen und werden flüssig",
            "Bei ausreichend hoher Temperatur (dem Schmelzpunkt) gehen Metalle vom festen in den flüssigen Zustand über."),
        ("Warum kann man Metalle im Gegensatz zu vielen anderen Materialien gut recyceln?", new[] { "Sie lassen sich einschmelzen und zu neuen Produkten formen, ohne ihre Eigenschaften zu verlieren", "Metalle können nach Gebrauch nicht wiederverwendet werden", "Metalle lösen sich beim Recycling einfach in Luft auf" }, "Sie lassen sich einschmelzen und zu neuen Produkten formen, ohne ihre Eigenschaften zu verlieren",
            "Metalle können praktisch beliebig oft eingeschmolzen und neu verarbeitet werden, ohne ihre grundlegenden Eigenschaften zu verlieren."),
        ("Was schützt viele Metallgegenstände wie Autoteile vor Rost?", new[] { "Eine Schutzschicht wie Lack oder Verzinkung", "Ständiges Wässern der Oberfläche", "Direktes Sonnenlicht" }, "Eine Schutzschicht wie Lack oder Verzinkung",
            "Beschichtungen wie Lack oder eine Zinkschicht schützen das darunterliegende Metall vor Feuchtigkeit und Sauerstoff."),
        ("Warum wird Eisen häufig verzinkt (mit einer Zinkschicht überzogen)?", new[] { "Zink schützt das darunterliegende Eisen vor Rost", "Zink macht Eisen magnetisch", "Zink färbt Eisen bunt" }, "Zink schützt das darunterliegende Eisen vor Rost",
            "Die Zinkschicht wirkt wie eine Schutzhülle und verhindert, dass Feuchtigkeit und Sauerstoff das Eisen darunter angreifen."),
        ("Welches Metall ist bei Zimmertemperatur flüssig?", new[] { "Quecksilber", "Eisen", "Gold" }, "Quecksilber",
            "Quecksilber ist das einzige Metall, das bei normaler Zimmertemperatur bereits flüssig ist."),
        ("Was bedeutet der typische metallische Glanz vieler Metalle?", new[] { "Sie reflektieren Licht an ihrer glatten Oberfläche gut", "Sie absorbieren jegliches Licht vollständig", "Sie sind grundsätzlich durchsichtig" }, "Sie reflektieren Licht an ihrer glatten Oberfläche gut",
            "Der metallische Glanz entsteht, weil frei bewegliche Elektronen an der Oberfläche Licht gut reflektieren."),
        ("Warum sind Metalle im Allgemeinen gute Wärmeleiter?", new[] { "Frei bewegliche Elektronen transportieren Energie schnell durch das Metall", "Metalle enthalten grundsätzlich keine Elektronen", "Wärme kann Metalle gar nicht durchdringen" }, "Frei bewegliche Elektronen transportieren Energie schnell durch das Metall",
            "Die frei beweglichen Elektronen in Metallen leiten Wärmeenergie sehr effizient weiter."),
        ("Was ist Stahl?", new[] { "Eine Legierung aus Eisen und Kohlenstoff", "Ein reines chemisches Element", "Eine Legierung aus Gold und Silber" }, "Eine Legierung aus Eisen und Kohlenstoff",
            "Stahl entsteht durch das gezielte Legieren von Eisen mit einem geringen Anteil Kohlenstoff, was ihn härter und widerstandsfähiger macht."),
        ("Warum ist Edelstahl (rostfreier Stahl) bei Besteck so beliebt?", new[] { "Er rostet praktisch nicht und ist langlebig", "Er ist das leichteste bekannte Material", "Er verändert ständig seine Farbe" }, "Er rostet praktisch nicht und ist langlebig",
            "Edelstahl enthält Legierungselemente wie Chrom, die eine schützende Oxidschicht bilden und Rost praktisch verhindern."),
        ("Was passiert chemisch beim Rosten von Eisen?", new[] { "Eisen reagiert mit Sauerstoff und Feuchtigkeit zu Eisenoxid", "Eisen verwandelt sich in Gold", "Eisen verliert dabei seine gesamte Masse" }, "Eisen reagiert mit Sauerstoff und Feuchtigkeit zu Eisenoxid",
            "Rost ist chemisch gesehen Eisenoxid, das durch die Reaktion von Eisen mit Sauerstoff und Wasser entsteht."),
        ("Warum sind viele Metalle in der Erdkruste als Erze und nicht als reines Metall zu finden?", new[] { "Sie reagieren leicht mit anderen Elementen wie Sauerstoff zu chemischen Verbindungen", "Reine Metalle kommen in der Natur häufiger vor als Erze", "Metalle existieren in der Erdkruste überhaupt nicht" }, "Sie reagieren leicht mit anderen Elementen wie Sauerstoff zu chemischen Verbindungen",
            "Die meisten Metalle sind reaktionsfreudig und liegen deshalb meist als chemische Verbindungen (Erze) statt als reines Metall vor.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] StoffeAlltagListe =
    {
        ("Zu welcher Stoffgruppe gehört Kohle?", new[] { "Brennstoffe", "Nährstoffe", "Metalle" }, "Brennstoffe",
            "Kohle wird verbrannt, um Energie zu gewinnen, und zählt daher zu den Brennstoffen."),
        ("Zu welcher Stoffgruppe gehören Kohlenhydrate, Fette und Eiweiße?", new[] { "Nährstoffe", "Brennstoffe", "Gefahrstoffe" }, "Nährstoffe",
            "Kohlenhydrate, Fette und Eiweiße liefern dem Körper Energie und Baustoffe - sie sind Nährstoffe."),
        ("Zu welcher Stoffgruppe gehören Eisen und Aluminium?", new[] { "Metalle", "Kunststoffe", "Brennstoffe" }, "Metalle",
            "Eisen und Aluminium sind typische Metalle."),
        ("Zu welcher Stoffgruppe gehört PET (z.B. bei Plastikflaschen)?", new[] { "Kunststoffe", "Metalle", "Nährstoffe" }, "Kunststoffe",
            "PET ist ein Kunststoff, aus dem u.a. Plastikflaschen hergestellt werden."),
        ("Was ist ein Gefahrstoff?", new[] { "Ein Stoff, der z.B. giftig, ätzend oder leicht entzündlich ist", "Ein Stoff, der immer ungefährlich ist", "Ein Stoff, der nur in der Küche vorkommt" }, "Ein Stoff, der z.B. giftig, ätzend oder leicht entzündlich ist",
            "Gefahrstoffe können z.B. giftig, ätzend oder leicht entzündlich sein und werden entsprechend gekennzeichnet."),
        ("Was bedeutet \"Brennbarkeit\" als Stoffeigenschaft?", new[] { "Wie leicht sich ein Stoff entzünden lässt", "Wie schwer ein Stoff ist", "Wie ein Stoff schmeckt" }, "Wie leicht sich ein Stoff entzünden lässt",
            "Die Brennbarkeit beschreibt, wie leicht ein Stoff Feuer fängt."),
        ("Was versteht man unter der \"Schmelztemperatur\" eines Stoffes?", new[] { "Die Temperatur, bei der ein fester Stoff flüssig wird", "Die Temperatur, bei der ein Stoff gefriert", "Die normale Raumtemperatur" }, "Die Temperatur, bei der ein fester Stoff flüssig wird",
            "Bei der Schmelztemperatur geht ein Stoff vom festen in den flüssigen Zustand über."),
        ("Woran erkennt man, ob ein Metall magnetisch ist?", new[] { "Es wird von einem Magneten angezogen", "Es leitet keinen Strom", "Es ist immer aus Kunststoff" }, "Es wird von einem Magneten angezogen",
            "Magnetische Metalle wie Eisen werden von einem Magneten angezogen."),
        ("Was bedeutet \"Löslichkeit\" eines Stoffes?", new[] { "Ob und wie gut sich ein Stoff in einer Flüssigkeit (z.B. Wasser) auflöst", "Wie schwer ein Stoff ist", "Wie ein Stoff riecht" }, "Ob und wie gut sich ein Stoff in einer Flüssigkeit (z.B. Wasser) auflöst",
            "Löslichkeit beschreibt, wie gut sich ein Stoff in einer Flüssigkeit auflöst."),
        ("Welches Metall wird von einem normalen Magneten angezogen?", new[] { "Eisen", "Aluminium", "Gold" }, "Eisen",
            "Eisen ist magnetisch und wird von einem normalen Magneten angezogen, Aluminium und Gold nicht."),
        ("Ist Zucker in Wasser gut löslich?", new[] { "Ja, Zucker löst sich gut in Wasser", "Nein, Zucker löst sich nie in Wasser", "Zucker verändert die Farbe von Wasser, löst sich aber nicht" }, "Ja, Zucker löst sich gut in Wasser",
            "Zucker löst sich gut in Wasser auf und ist danach im Wasser verteilt."),
        ("Ist Sand in Wasser löslich?", new[] { "Nein, Sand löst sich nicht in Wasser", "Ja, Sand löst sich vollständig in Wasser", "Sand verdampft in Wasser" }, "Nein, Sand löst sich nicht in Wasser",
            "Sand ist unlöslich in Wasser und kann z.B. durch Filtration abgetrennt werden."),
        ("Warum werden brennbare Stoffe wie Benzin als Gefahrstoffe gekennzeichnet?", new[] { "Weil sie leicht Feuer fangen und gefährlich sein können", "Weil sie besonders schwer sind", "Weil sie niemals brennen" }, "Weil sie leicht Feuer fangen und gefährlich sein können",
            "Leicht entzündliche Stoffe wie Benzin werden als Gefahrstoffe gekennzeichnet, um vor Unfällen zu warnen."),
        ("Was ist ein typisches Beispiel für einen Kunststoff im Alltag?", new[] { "Eine Plastiktüte", "Ein Goldring", "Ein Holzbrett" }, "Eine Plastiktüte",
            "Plastiktüten bestehen aus Kunststoff, einem künstlich hergestellten Material."),
        ("Warum sind Metalle wie Kupfer für elektrische Leitungen gut geeignet?", new[] { "Sie leiten elektrischen Strom gut", "Sie leiten keinen Strom", "Sie schmelzen bei Zimmertemperatur" }, "Sie leiten elektrischen Strom gut",
            "Kupfer leitet elektrischen Strom sehr gut und wird deshalb häufig für Kabel verwendet."),
        ("Was passiert mit Eis, wenn die Temperatur über 0°C steigt?", new[] { "Es schmilzt zu Wasser", "Es wird härter", "Es verdampft sofort" }, "Es schmilzt zu Wasser",
            "Bei Temperaturen über 0°C schmilzt Eis und wird zu flüssigem Wasser."),
        ("Welche Stoffeigenschaft prüft man, wenn man Öl und Wasser mischt?", new[] { "Ob sich die Stoffe miteinander mischen (Löslichkeit)", "Die Brennbarkeit", "Den Magnetismus" }, "Ob sich die Stoffe miteinander mischen (Löslichkeit)",
            "Öl und Wasser mischen sich nicht - das zeigt, dass Öl in Wasser unlöslich ist."),
        ("Warum sollten Gefahrstoffe im Haushalt gekennzeichnet und sicher aufbewahrt werden?", new[] { "Um Unfälle und Vergiftungen zu vermeiden", "Weil es keinen Grund dafür gibt", "Nur aus optischen Gründen" }, "Um Unfälle und Vergiftungen zu vermeiden",
            "Kennzeichnung und sichere Aufbewahrung von Gefahrstoffen schützen vor Unfällen und Vergiftungen."),
        ("Was ist ein Beispiel für einen Nährstoff, den unser Körper braucht?", new[] { "Eiweiß (Protein)", "Plastik", "Metall" }, "Eiweiß (Protein)",
            "Eiweiß (Protein) ist ein wichtiger Nährstoff, den unser Körper zum Aufbau von Zellen benötigt."),
        ("Warum ist es wichtig, Stoffe nach ihren Eigenschaften zu ordnen (z.B. Brennstoffe, Metalle, Kunststoffe)?", new[] { "Um sie sicher und sinnvoll im Alltag zu nutzen und zu entsorgen", "Das ist völlig unwichtig", "Nur um sie schöner aussehen zu lassen" }, "Um sie sicher und sinnvoll im Alltag zu nutzen und zu entsorgen",
            "Die Einteilung nach Eigenschaften hilft, Stoffe sicher zu nutzen, zu lagern und richtig zu entsorgen (z.B. Recycling).")
    };

    private static QuizQuestion StoffeImAlltag(Random r)
    {
        var f = StoffeAlltagListe[r.Next(StoffeAlltagListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Stoffe im Alltag", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Stoffgruppen: Brennstoffe (Kohle), Nährstoffe (Eiweiß), Metalle (Eisen), Kunststoffe (PET), Gefahrstoffe (giftig/ätzend/entzündlich)."
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
            "Ein elektrisch geladenes Atom (oder Molekül) durch Elektronenabgabe/-aufnahme", "Gibt ein Atom Elektronen ab oder nimmt welche auf, entsteht ein geladenes Ion (positiv oder negativ)."),
        ("Welche Ladung hat ein Elektron?", new[] { "Negativ", "Positiv", "Neutral" }, "Negativ",
            "Elektronen sind negativ geladen und umkreisen den positiv geladenen Atomkern."),
        ("Welche Ladung hat ein Neutron?", new[] { "Neutral (keine Ladung)", "Positiv", "Negativ" }, "Neutral (keine Ladung)",
            "Neutronen tragen im Gegensatz zu Protonen und Elektronen keine elektrische Ladung."),
        ("Wo befindet sich fast die gesamte Masse eines Atoms?", new[] { "Im Atomkern (Protonen und Neutronen)", "In der Elektronenhülle", "Verteilt im leeren Raum um das Atom" }, "Im Atomkern (Protonen und Neutronen)",
            "Protonen und Neutronen sind viel schwerer als Elektronen, deshalb steckt fast die gesamte Atommasse im winzigen Kern."),
        ("Was ist die Massenzahl eines Atoms?", new[] { "Die Summe aus Protonen- und Neutronenzahl", "Nur die Anzahl der Elektronen", "Die Anzahl der Isotope"}, "Die Summe aus Protonen- und Neutronenzahl",
            "Die Massenzahl ergibt sich aus der Summe von Protonen und Neutronen im Atomkern."),
        ("Wie viele Elektronen hat ein elektrisch neutrales Atom im Vergleich zu Protonen?", new[] { "Genauso viele wie Protonen", "Immer mehr als Protonen", "Immer weniger als Protonen" }, "Genauso viele wie Protonen",
            "In einem elektrisch neutralen Atom gleicht die Anzahl der negativ geladenen Elektronen die Anzahl der positiv geladenen Protonen genau aus."),
        ("Was passiert, wenn ein Atom ein Elektron abgibt?", new[] { "Es wird zu einem positiv geladenen Ion", "Es wird zu einem negativ geladenen Ion", "Es bleibt elektrisch neutral" }, "Es wird zu einem positiv geladenen Ion",
            "Gibt ein Atom ein negativ geladenes Elektron ab, überwiegt die positive Ladung der Protonen - es entsteht ein positives Ion."),
        ("Was passiert, wenn ein Atom ein zusätzliches Elektron aufnimmt?", new[] { "Es wird zu einem negativ geladenen Ion", "Es wird zu einem positiv geladenen Ion", "Es bleibt elektrisch neutral" }, "Es wird zu einem negativ geladenen Ion",
            "Nimmt ein Atom ein zusätzliches Elektron auf, überwiegt die negative Ladung - es entsteht ein negatives Ion."),
        ("Wie viele Elektronen passen maximal auf die erste (innerste) Elektronenschale?", new[] { "2", "8", "18" }, "2",
            "Die erste Elektronenschale kann maximal 2 Elektronen aufnehmen, weiter außen liegende Schalen mehr."),
        ("Wer entwickelte das bekannte Kern-Schalen-Atommodell mit Elektronen auf festen Bahnen?", new[] { "Niels Bohr", "Isaac Newton", "Albert Einstein" }, "Niels Bohr",
            "Niels Bohr entwickelte Anfang des 20. Jahrhunderts das Modell, bei dem Elektronen den Atomkern auf festen Schalen umkreisen."),
        ("Was hält die negativ geladenen Elektronen um den positiv geladenen Kern?", new[] { "Die elektrische Anziehungskraft zwischen positiven und negativen Ladungen", "Die Schwerkraft der Erde", "Ein starker Magnet im Kern" }, "Die elektrische Anziehungskraft zwischen positiven und negativen Ladungen",
            "Entgegengesetzte Ladungen ziehen sich elektrisch an - das hält die Elektronen in der Nähe des positiv geladenen Kerns."),
        ("Was ist ein Molekül im Unterschied zu einem einzelnen Atom?", new[] { "Eine Verbindung aus zwei oder mehr miteinander verbundenen Atomen", "Ein Atom ohne Elektronen", "Ein anderes Wort für Ion" }, "Eine Verbindung aus zwei oder mehr miteinander verbundenen Atomen",
            "Moleküle entstehen, wenn sich zwei oder mehr Atome chemisch miteinander verbinden, z.B. zwei Wasserstoffatome zu H₂."),
        ("Was zeigt die chemische Formel H₂O?", new[] { "Ein Wassermolekül aus zwei Wasserstoff- und einem Sauerstoffatom", "Ein reines Wasserstoffmolekül", "Ein reines Sauerstoffmolekül" }, "Ein Wassermolekül aus zwei Wasserstoff- und einem Sauerstoffatom",
            "Die Formel H₂O zeigt, dass ein Wassermolekül aus zwei Wasserstoffatomen und einem Sauerstoffatom besteht."),
        ("Was versteht man unter der \"Atomhülle\"?", new[] { "Den Bereich um den Kern, in dem sich die Elektronen aufhalten", "Einen anderen Namen für den Atomkern selbst", "Eine feste, sichtbare Schutzschicht aus Metall" }, "Den Bereich um den Kern, in dem sich die Elektronen aufhalten",
            "Die Atomhülle umfasst den Raum um den Kern, in dem sich die Elektronen auf ihren Schalen bewegen."),
        ("Welches besonders leichte Element hat nur ein Proton in seinem Kern?", new[] { "Wasserstoff", "Sauerstoff", "Kohlenstoff" }, "Wasserstoff",
            "Wasserstoff ist mit nur einem Proton (und ohne Neutron im häufigsten Isotop) das leichteste und einfachste Element."),
        ("Was unterscheidet zwei Isotope desselben Elements voneinander?", new[] { "Die Anzahl der Neutronen im Kern", "Die Anzahl der Protonen im Kern", "Die Ladung der Elektronen" }, "Die Anzahl der Neutronen im Kern",
            "Isotope eines Elements haben dieselbe Protonenzahl, aber unterschiedlich viele Neutronen im Kern.")
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
            "Die Zahlen (Koeffizienten) vor den Formeln zeigen das Mengenverhältnis, in dem die Stoffe reagieren."),
        ("Was ist ein Katalysator?", new[] { "Ein Stoff, der eine Reaktion beschleunigt, ohne selbst verbraucht zu werden", "Ein Stoff, der jede Reaktion vollständig stoppt", "Ein anderes Wort für Lösungsmittel" }, "Ein Stoff, der eine Reaktion beschleunigt, ohne selbst verbraucht zu werden",
            "Katalysatoren senken die nötige Aktivierungsenergie und beschleunigen so Reaktionen, bleiben dabei aber selbst unverändert."),
        ("Was ist ein Beispiel für eine endotherme Reaktion im Alltag?", new[] { "Backen eines Kuchens", "Verbrennen von Holz", "Rosten von Eisen" }, "Backen eines Kuchens",
            "Beim Backen wird ständig Wärme von außen zugeführt (aufgenommen), damit der Teig reagieren und aufgehen kann - eine endotherme Reaktion."),
        ("Was ist ein Beispiel für eine exotherme Reaktion im Alltag?", new[] { "Verbrennen von Holz", "Schmelzen von Eis", "Backen eines Kuchens" }, "Verbrennen von Holz",
            "Beim Verbrennen wird Energie in Form von Wärme und Licht an die Umgebung abgegeben - eine exotherme Reaktion."),
        ("Was passiert bei einer Synthese-Reaktion?", new[] { "Zwei oder mehr Ausgangsstoffe verbinden sich zu einem neuen Stoff", "Ein Stoff zerfällt in mehrere Teile", "Es passiert überhaupt keine chemische Veränderung" }, "Zwei oder mehr Ausgangsstoffe verbinden sich zu einem neuen Stoff",
            "Bei einer Synthese reagieren mehrere Ausgangsstoffe (Edukte) zu einem gemeinsamen neuen Produkt."),
        ("Was passiert bei einer Zersetzungsreaktion, vereinfacht?", new[] { "Ein Ausgangsstoff zerfällt in zwei oder mehr neue Stoffe", "Zwei Stoffe verbinden sich zu einem neuen Stoff", "Es entsteht überhaupt kein neuer Stoff" }, "Ein Ausgangsstoff zerfällt in zwei oder mehr neue Stoffe",
            "Bei einer Zersetzungsreaktion wird ein einzelner Ausgangsstoff in mehrere einfachere Stoffe aufgespalten."),
        ("Was bedeutet \"Aktivierungsenergie\" bei einer chemischen Reaktion?", new[] { "Die Energie, die nötig ist, um eine Reaktion überhaupt zu starten", "Die Energie, die am Ende einer Reaktion übrig bleibt", "Die Masse der beteiligten Stoffe" }, "Die Energie, die nötig ist, um eine Reaktion überhaupt zu starten",
            "Auch bei Reaktionen, die insgesamt Energie freisetzen, ist zunächst eine gewisse Aktivierungsenergie nötig, um sie zu starten."),
        ("Warum laufen manche chemische Reaktionen bei höherer Temperatur schneller ab?", new[] { "Die Teilchen bewegen sich schneller und stoßen häufiger zusammen", "Wärme verhindert grundsätzlich jede Reaktion", "Höhere Temperatur verändert die Masse der Teilchen" }, "Die Teilchen bewegen sich schneller und stoßen häufiger zusammen",
            "Bei höherer Temperatur bewegen sich Teilchen schneller, treffen häufiger und energiereicher aufeinander - das beschleunigt viele Reaktionen."),
        ("Was unterscheidet eine chemische Reaktion von einem rein physikalischen Vorgang wie Schmelzen?", new[] { "Bei einer chemischen Reaktion entstehen neue Stoffe mit neuen Eigenschaften", "Beide Vorgänge sind chemisch komplett identisch", "Physikalische Vorgänge erzeugen immer neue chemische Stoffe" }, "Bei einer chemischen Reaktion entstehen neue Stoffe mit neuen Eigenschaften",
            "Beim Schmelzen ändert sich nur der Aggregatzustand, der Stoff bleibt derselbe - bei einer chemischen Reaktion entstehen dagegen völlig neue Stoffe."),
        ("Woran erkennt man oft (nicht immer), dass eine chemische Reaktion stattgefunden hat?", new[] { "Z.B. an Farbwechsel, Gasbildung, Temperaturänderung oder Niederschlag", "Nur an einer Gewichtszunahme des gesamten Systems", "Chemische Reaktionen sind grundsätzlich nicht erkennbar" }, "Z.B. an Farbwechsel, Gasbildung, Temperaturänderung oder Niederschlag",
            "Typische Anzeichen einer chemischen Reaktion sind Farbänderungen, Gasentwicklung, Wärmeabgabe/-aufnahme oder das Ausfallen eines Feststoffs."),
        ("Was ist ein \"Niederschlag\" bei einer chemischen Reaktion in einer Flüssigkeit?", new[] { "Ein fester, unlöslicher Stoff, der sich bei der Reaktion bildet und ausfällt", "Regen, der in ein Reagenzglas fällt", "Ein anderes Wort für Katalysator" }, "Ein fester, unlöslicher Stoff, der sich bei der Reaktion bildet und ausfällt",
            "Reagieren zwei gelöste Stoffe zu einem unlöslichen Produkt, fällt dieses als sichtbarer Niederschlag aus."),
        ("Was zeigt das Pluszeichen in einer Reaktionsgleichung wie \"A + B → C\"?", new[] { "Dass die Stoffe A und B miteinander reagieren", "Dass A und B addiert eine höhere Temperatur ergeben", "Dass A und B sich gegenseitig neutralisieren, ohne zu reagieren" }, "Dass die Stoffe A und B miteinander reagieren",
            "Das Pluszeichen zeigt an, welche Ausgangsstoffe (Edukte) an der Reaktion beteiligt sind und miteinander reagieren."),
        ("Was sind \"Edukte\" in einer chemischen Reaktion?", new[] { "Die Ausgangsstoffe, die miteinander reagieren", "Die neuen Stoffe, die am Ende entstehen", "Ein anderes Wort für Katalysator" }, "Die Ausgangsstoffe, die miteinander reagieren",
            "Edukte stehen auf der linken Seite einer Reaktionsgleichung und reagieren zu den Produkten."),
        ("Was sind \"Produkte\" einer chemischen Reaktion?", new[] { "Die neuen Stoffe, die bei der Reaktion entstehen", "Die Ausgangsstoffe vor der Reaktion", "Ein anderes Wort für Aktivierungsenergie" }, "Die neuen Stoffe, die bei der Reaktion entstehen",
            "Produkte stehen auf der rechten Seite der Reaktionsgleichung und sind das Ergebnis der chemischen Umwandlung."),
        ("Warum bleibt die Anzahl der Atome jeder Sorte bei einer korrekt ausgeglichenen Reaktionsgleichung gleich?", new[] { "Weil bei einer chemischen Reaktion keine Atome verloren gehen oder neu entstehen", "Weil Atome sich bei Reaktionen einfach in Energie auflösen", "Weil Reaktionsgleichungen reine Näherungen ohne echte Bedeutung sind" }, "Weil bei einer chemischen Reaktion keine Atome verloren gehen oder neu entstehen",
            "Der Massenerhaltungssatz gilt auch auf Atomebene: Atome werden bei einer Reaktion nur neu angeordnet, nicht erzeugt oder vernichtet."),
        ("Was zeigt die \"Reaktionsgeschwindigkeit\" bei einer chemischen Reaktion?", new[] { "Wie schnell Edukte zu Produkten umgesetzt werden", "Wie schwer die beteiligten Stoffe sind", "Welche Farbe die Reaktion am Ende hat" }, "Wie schnell Edukte zu Produkten umgesetzt werden",
            "Die Reaktionsgeschwindigkeit beschreibt, wie schnell sich die Ausgangsstoffe in einer bestimmten Zeit in Produkte umwandeln.")
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
            "Der russische Chemiker Dmitri Mendelejew ordnete 1869 die Elemente erstmals systematisch nach ihren Eigenschaften an."),
        ("Wie viele Hauptgruppen gibt es im klassischen Periodensystem?", new[] { "8", "4", "20" }, "8",
            "Das klassische Periodensystem gliedert sich in 8 Hauptgruppen mit jeweils ähnlichen chemischen Eigenschaften."),
        ("Zu welcher Elementgruppe gehören Fluor und Chlor?", new[] { "Halogene", "Edelgase", "Alkalimetalle" }, "Halogene",
            "Fluor und Chlor gehören zu den Halogenen (7. Hauptgruppe), sehr reaktionsfreudigen Nichtmetallen."),
        ("Was ist typisch für Halogene wie Fluor und Chlor?", new[] { "Sie sind sehr reaktionsfreudige Nichtmetalle", "Sie reagieren praktisch nie mit anderen Stoffen", "Sie sind alle bei Zimmertemperatur fest und ungefährlich" }, "Sie sind sehr reaktionsfreudige Nichtmetalle",
            "Halogene reagieren sehr leicht mit vielen anderen Elementen, z.B. mit Metallen zu Salzen."),
        ("Was bedeutet \"Hauptgruppenelement\" im Periodensystem, vereinfacht?", new[] { "Ein Element aus einer der Spalten mit regelmäßigen Eigenschaftsmustern", "Ein besonders seltenes, künstlich hergestelltes Element", "Ein Element, das nur in Sternen vorkommt" }, "Ein Element aus einer der Spalten mit regelmäßigen Eigenschaftsmustern",
            "Hauptgruppenelemente stehen in den nummerierten Spalten des Periodensystems, deren Elemente ähnliche, regelmäßige Eigenschaften zeigen."),
        ("Welche Elementgruppe bilden Sauerstoff und Schwefel?", new[] { "Chalkogene", "Halogene", "Edelgase" }, "Chalkogene",
            "Sauerstoff und Schwefel gehören zur 6. Hauptgruppe, den sogenannten Chalkogenen."),
        ("Wo im Periodensystem findet man meist besonders reaktionsfreudige Metalle wie Natrium?", new[] { "Links im Periodensystem", "Rechts im Periodensystem", "Genau in der Mitte" }, "Links im Periodensystem",
            "Die stark reaktionsfreudigen Alkalimetalle wie Natrium stehen in der ersten Hauptgruppe, ganz links im Periodensystem."),
        ("Wo im Periodensystem findet man meist die Edelgase, die kaum reagieren?", new[] { "Rechts im Periodensystem", "Links im Periodensystem", "Genau in der Mitte" }, "Rechts im Periodensystem",
            "Edelgase stehen in der letzten Hauptgruppe ganz rechts im Periodensystem."),
        ("Wie verändert sich die Reaktionsfreudigkeit der Alkalimetalle von oben nach unten in ihrer Gruppe?", new[] { "Sie nimmt zu", "Sie nimmt ab", "Sie bleibt exakt gleich" }, "Sie nimmt zu",
            "Innerhalb der Alkalimetalle werden die Elemente von oben (z.B. Lithium) nach unten (z.B. Cäsium) zunehmend reaktionsfreudiger."),
        ("Was zeigt das chemische Symbol im Periodensystem für jedes Element, z.B. \"Na\"?", new[] { "Eine Abkürzung für den Elementnamen", "Die Farbe des Elements", "Den Entdecker des Elements" }, "Eine Abkürzung für den Elementnamen",
            "Chemische Symbole sind international einheitliche Abkürzungen für die Elementnamen, z.B. \"Na\" für Natrium."),
        ("Was zeigt die Zahl über dem chemischen Symbol im Periodensystem meist an?", new[] { "Die Ordnungszahl (Protonenzahl)", "Die Temperatur des Elements", "Das Entdeckungsjahr" }, "Die Ordnungszahl (Protonenzahl)",
            "Die Ordnungszahl über dem Symbol gibt an, wie viele Protonen ein Atom dieses Elements im Kern besitzt."),
        ("Wie viele Elemente sind aktuell im Periodensystem ungefähr bekannt?", new[] { "Etwas mehr als 100", "Etwa 20", "Über 10.000" }, "Etwas mehr als 100",
            "Das Periodensystem umfasst derzeit etwas mehr als 100 bekannte chemische Elemente."),
        ("Was unterscheidet Metalle grob von Nichtmetallen in ihrer Lage im Periodensystem?", new[] { "Metalle stehen meist links/mittig, Nichtmetalle eher rechts oben", "Metalle und Nichtmetalle sind zufällig verteilt", "Nichtmetalle stehen immer ganz links" }, "Metalle stehen meist links/mittig, Nichtmetalle eher rechts oben",
            "Die meisten Metalle befinden sich im linken und mittleren Bereich des Periodensystems, Nichtmetalle eher rechts oben."),
        ("Warum ist das Periodensystem für Chemikerinnen und Chemiker so nützlich?", new[] { "Es zeigt auf einen Blick Eigenschaften und Verwandtschaften der Elemente", "Es dient nur zur Dekoration im Klassenzimmer", "Es zeigt ausschließlich die Preise der Elemente" }, "Es zeigt auf einen Blick Eigenschaften und Verwandtschaften der Elemente",
            "Die geordnete Struktur des Periodensystems erlaubt es, Eigenschaften und Reaktionsverhalten von Elementen vorherzusagen."),
        ("Was bedeutete es, dass Mendelejew in seiner ersten Version Lücken für unentdeckte Elemente ließ?", new[] { "Er sagte deren Eigenschaften anhand des Musters im Periodensystem korrekt voraus", "Er hatte schlicht Platz verschwendet, ohne Grund", "Er wollte das Periodensystem absichtlich unvollständig lassen" }, "Er sagte deren Eigenschaften anhand des Musters im Periodensystem korrekt voraus",
            "Mendelejew erkannte Regelmäßigkeiten im Periodensystem und sagte die Eigenschaften noch unentdeckter Elemente erstaunlich genau voraus."),
        ("Zu welcher Elementgruppe gehört Helium?", new[] { "Edelgase", "Alkalimetalle", "Halogene" }, "Edelgase",
            "Helium gehört zur Gruppe der Edelgase und reagiert wie diese kaum mit anderen Stoffen.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] StoechiometrieListe =
    {
        ("Was ist ein Mol in der Chemie?", new[] { "Eine festgelegte Stoffmenge von genau 6,022 · 10^23 Teilchen", "Ein anderes Wort für Gramm", "Eine Einheit für die Temperatur" }, "Eine festgelegte Stoffmenge von genau 6,022 · 10^23 Teilchen",
            "Ein Mol entspricht der Avogadro-Konstante von rund 6,022 · 10^23 Teilchen (Atomen, Molekülen oder Ionen) eines Stoffes."),
        ("Was beschreibt die molare Masse eines Stoffes?", new[] { "Die Masse von einem Mol dieses Stoffes in Gramm pro Mol", "Die Masse eines einzelnen Atoms in Kilogramm", "Das Volumen eines Stoffes bei Zimmertemperatur" }, "Die Masse von einem Mol dieses Stoffes in Gramm pro Mol",
            "Die molare Masse gibt an, wie viel Gramm ein Mol eines Stoffes wiegt, z.B. hat Wasser (H₂O) eine molare Masse von etwa 18 g/mol."),
        ("Wie berechnet man die Stoffmenge n aus der gegebenen Masse m und der molaren Masse M?", new[] { "n = m / M", "n = m · M", "n = M / m" }, "n = m / M",
            "Die Stoffmenge in Mol ergibt sich, indem man die Masse eines Stoffes durch seine molare Masse teilt (n = m/M)."),
        ("Was gibt die Stoffmengenkonzentration einer wässrigen Lösung an?", new[] { "Wie viel Mol eines gelösten Stoffes in einem Liter Lösung enthalten sind", "Nur die Farbe der Lösung", "Die Temperatur der Lösung" }, "Wie viel Mol eines gelösten Stoffes in einem Liter Lösung enthalten sind",
            "Die Stoffmengenkonzentration (in mol/l) beschreibt, wie viel Mol eines gelösten Stoffes in einem Liter der Lösung vorhanden sind."),
        ("Was bedeutet \"stöchiometrisches Rechnen\" bei einer chemischen Reaktion?", new[] { "Die Mengenverhältnisse von Edukten und Produkten anhand der Reaktionsgleichung berechnen", "Nur die Farbe der Reaktion beschreiben", "Die Reaktionszeit mit einer Stoppuhr messen" }, "Die Mengenverhältnisse von Edukten und Produkten anhand der Reaktionsgleichung berechnen",
            "Stöchiometrisches Rechnen nutzt die Zahlenverhältnisse einer ausgeglichenen Reaktionsgleichung, um Massen, Stoffmengen oder Volumina von Edukten und Produkten zu berechnen."),
        ("Warum muss eine chemische Reaktionsgleichung ausgeglichen (stimmig) sein, bevor man stöchiometrisch rechnet?", new[] { "Damit die Anzahl der Atome auf beiden Seiten der Gleichung übereinstimmt (Erhaltung der Masse)", "Weil das Aussehen der Gleichung sonst nicht ordentlich wäre", "Ausgeglichene Gleichungen sind für die Rechnung nicht notwendig" }, "Damit die Anzahl der Atome auf beiden Seiten der Gleichung übereinstimmt (Erhaltung der Masse)",
            "Nach dem Gesetz der Erhaltung der Masse müssen bei einer chemischen Reaktion auf beiden Seiten der Gleichung gleich viele Atome jeder Sorte vorhanden sein."),
        ("Was versteht man unter einer isotonischen Kochsalzlösung?", new[] { "Eine Salzlösung mit einer Konzentration, die der des menschlichen Blutes entspricht", "Eine Lösung ganz ohne gelöstes Salz", "Eine Lösung mit maximal möglicher Salzkonzentration" }, "Eine Salzlösung mit einer Konzentration, die der des menschlichen Blutes entspricht",
            "Eine isotonische Kochsalzlösung (ca. 0,9 % NaCl) hat eine Konzentration, die osmotisch der von Blutplasma entspricht und wird u.a. medizinisch verwendet."),
        ("Welches Gasvolumen nimmt ein Mol eines idealen Gases bei Normbedingungen ungefähr ein?", new[] { "Etwa 22,4 Liter", "Etwa 1 Liter", "Etwa 100 Liter" }, "Etwa 22,4 Liter",
            "Unter Normbedingungen (0 °C, 1013 hPa) nimmt ein Mol eines idealen Gases ein molares Volumen von etwa 22,4 Litern ein."),
        ("Wie berechnet man die Masse eines Reaktionsprodukts, wenn man die eingesetzte Stoffmenge und die molare Masse des Produkts kennt?", new[] { "Man multipliziert die Stoffmenge mit der molaren Masse (m = n · M)", "Man dividiert die Stoffmenge durch die Zeit", "Man addiert die Stoffmenge und die molare Masse" }, "Man multipliziert die Stoffmenge mit der molaren Masse (m = n · M)",
            "Die Masse eines Stoffes berechnet man, indem man die vorhandene Stoffmenge mit seiner molaren Masse multipliziert (m = n · M)."),
        ("Was ist die molare Masse von Wasser (H₂O) ungefähr, ausgehend von H ≈ 1 g/mol und O ≈ 16 g/mol?", new[] { "Etwa 18 g/mol", "Etwa 2 g/mol", "Etwa 32 g/mol" }, "Etwa 18 g/mol",
            "Wasser besteht aus zwei Wasserstoffatomen (2 · 1 g/mol) und einem Sauerstoffatom (16 g/mol), zusammen ergibt das etwa 18 g/mol."),
        ("Warum ist das Mol als \"Zählmaß\" in der Chemie besonders praktisch?", new[] { "Es erlaubt, mit sehr großen Teilchenzahlen wie mit handlichen, alltagstauglichen Zahlen zu rechnen", "Weil ein Mol immer exakt einem Gramm entspricht", "Weil sich damit die Farbe von Stoffen bestimmen lässt" }, "Es erlaubt, mit sehr großen Teilchenzahlen wie mit handlichen, alltagstauglichen Zahlen zu rechnen",
            "Da einzelne Atome und Moleküle extrem klein und zahlreich sind, fasst das Mol riesige Teilchenzahlen in handlichen, gut nutzbaren Werten zusammen."),
        ("Wie verändert sich die Stoffmengenkonzentration einer Lösung, wenn man dieselbe Menge Salz in mehr Wasser löst?", new[] { "Sie sinkt, da dieselbe Stoffmenge auf ein größeres Volumen verteilt wird", "Sie steigt automatisch an", "Sie bleibt exakt unverändert" }, "Sie sinkt, da dieselbe Stoffmenge auf ein größeres Volumen verteilt wird",
            "Da die Konzentration Stoffmenge pro Volumen beschreibt, sinkt sie, wenn dieselbe Stoffmenge in mehr Lösungsmittel verdünnt wird."),
        ("Was bedeutet die Avogadro-Konstante für die Chemie?", new[] { "Sie gibt die Anzahl der Teilchen in einem Mol an", "Sie gibt die Temperatur an, bei der Wasser kocht", "Sie beschreibt nur die Farbe chemischer Verbindungen" }, "Sie gibt die Anzahl der Teilchen in einem Mol an",
            "Die Avogadro-Konstante (ca. 6,022 · 10^23 pro Mol) definiert, wie viele Teilchen in einem Mol eines Stoffes enthalten sind."),
        ("Warum spielt stöchiometrisches Rechnen im Labor eine wichtige praktische Rolle?", new[] { "Es hilft, die richtigen Mengen an Ausgangsstoffen für eine gewünschte Reaktion abzumessen", "Es hat im echten Labor keinerlei praktische Bedeutung", "Es dient ausschließlich der Beschreibung der Reaktionsfarbe" }, "Es hilft, die richtigen Mengen an Ausgangsstoffen für eine gewünschte Reaktion abzumessen",
            "Stöchiometrische Berechnungen ermöglichen es, im Labor exakt die benötigten Mengen an Chemikalien für eine gewünschte Reaktion und Ausbeute einzusetzen."),
        ("Wie verändert sich das Volumen eines Gases bei einer chemischen Reaktion im Vergleich zu seiner Stoffmenge, bei gleichbleibenden Bedingungen (Temperatur, Druck)?", new[] { "Das Volumen ist direkt proportional zur Stoffmenge des Gases", "Volumen und Stoffmenge stehen in keinerlei Zusammenhang", "Das Volumen verringert sich immer, wenn die Stoffmenge steigt" }, "Das Volumen ist direkt proportional zur Stoffmenge des Gases",
            "Bei gleichbleibenden Bedingungen ist das Volumen eines Gases direkt proportional zu seiner Stoffmenge (mehr Mol Gas nehmen mehr Volumen ein)."),
        ("Was ist ein Beispiel für die praktische Anwendung von Stoffmengenkonzentrationen im Alltag?", new[] { "Die Dosierung von Kochsalzlösungen in der Medizin", "Die Bestimmung der Uhrzeit", "Das Wiegen von Gegenständen ohne jeden Bezug zu Lösungen" }, "Die Dosierung von Kochsalzlösungen in der Medizin",
            "Genaue Konzentrationsangaben sind z.B. bei medizinischen Infusionslösungen entscheidend, damit sie im Körper richtig wirken."),
        ("Was passiert mit der Stoffmenge eines Edukts, wenn es bei einer Reaktion vollständig umgesetzt wird?", new[] { "Seine Stoffmenge sinkt auf null, während Produkte entsprechend der Reaktionsgleichung entstehen", "Seine Stoffmenge bleibt exakt unverändert", "Seine Stoffmenge steigt während der Reaktion immer weiter an" }, "Seine Stoffmenge sinkt auf null, während Produkte entsprechend der Reaktionsgleichung entstehen",
            "Wird ein Edukt vollständig verbraucht, sinkt seine Stoffmenge auf null, während gleichzeitig entsprechend den stöchiometrischen Verhältnissen Produkte entstehen."),
        ("Warum kann man aus der Reaktionsgleichung 2 H₂ + O₂ → 2 H₂O ablesen, wie viel Mol Wasserstoff für ein Mol Sauerstoff benötigt werden?", new[] { "Die Zahlen vor den Formeln (Koeffizienten) geben das Stoffmengenverhältnis der Reaktion an", "Die Reaktionsgleichung sagt nichts über Mengenverhältnisse aus", "Nur die chemischen Symbole selbst, nicht die Zahlen davor, sind relevant" }, "Die Zahlen vor den Formeln (Koeffizienten) geben das Stoffmengenverhältnis der Reaktion an",
            "Die Koeffizienten vor den Formeln in einer ausgeglichenen Reaktionsgleichung geben direkt das Stoffmengenverhältnis an, in dem die Stoffe reagieren."),
        ("Was ist ein Grund, warum in der Chemie oft mit der Einheit \"mol/l\" statt direkt mit Gramm gearbeitet wird?", new[] { "So lassen sich Teilchenzahlen unabhängig von unterschiedlichen molaren Massen direkt vergleichen", "Gramm ist in der Chemie grundsätzlich verboten zu verwenden", "mol/l hat mit der Stoffmenge nichts zu tun" }, "So lassen sich Teilchenzahlen unabhängig von unterschiedlichen molaren Massen direkt vergleichen",
            "Da unterschiedliche Stoffe unterschiedliche molare Massen haben, ermöglicht die Angabe in mol/l einen direkten Vergleich der tatsächlichen Teilchenzahlen in Lösungen."),
        ("Welche zwei Größen braucht man mindestens, um die Stoffmengenkonzentration einer Lösung zu berechnen?", new[] { "Die Stoffmenge des gelösten Stoffes und das Volumen der Lösung", "Nur die Farbe und den Geruch der Lösung", "Nur die Raumtemperatur des Labors" }, "Die Stoffmenge des gelösten Stoffes und das Volumen der Lösung",
            "Die Stoffmengenkonzentration berechnet sich als Stoffmenge (in mol) geteilt durch das Volumen der Lösung (in Litern).")
    };

    private static QuizQuestion Stoechiometrie(Random r)
    {
        var f = StoechiometrieListe[r.Next(StoechiometrieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Klare Verhältnisse – Stöchiometrie", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Mol sind 6,022 · 10^23 Teilchen; Stoffmenge n = Masse m / molare Masse M; die Koeffizienten der Reaktionsgleichung geben die Stoffmengenverhältnisse an."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SaeureBaseListe =
    {
        ("Was gibt der pH-Wert einer Lösung an?", new[] { "Wie sauer oder basisch (alkalisch) eine Lösung ist", "Die Temperatur einer Lösung", "Die Farbe einer Lösung" }, "Wie sauer oder basisch (alkalisch) eine Lösung ist",
            "Der pH-Wert misst die Konzentration von Wasserstoff- bzw. Oxonium-Ionen und zeigt, wie sauer (niedriger pH) oder basisch (hoher pH) eine Lösung ist."),
        ("Welcher pH-Wert gilt als neutral, wie bei reinem Wasser?", new[] { "pH 7", "pH 0", "pH 14" }, "pH 7",
            "Reines Wasser hat bei Raumtemperatur einen neutralen pH-Wert von etwa 7 - Werte darunter gelten als sauer, darüber als basisch."),
        ("Was passiert bei einer Neutralisationsreaktion zwischen einer Säure und einer Lauge?", new[] { "Wasserstoff-Ionen und Hydroxid-Ionen reagieren zu Wasser, ein Salz entsteht", "Es entsteht ausschließlich ein neues Gas ohne Wasser", "Es findet grundsätzlich keine chemische Reaktion statt" }, "Wasserstoff-Ionen und Hydroxid-Ionen reagieren zu Wasser, ein Salz entsteht",
            "Bei der Neutralisation reagieren Wasserstoff-Ionen (H⁺) der Säure mit Hydroxid-Ionen (OH⁻) der Lauge zu Wasser, während sich außerdem ein Salz bildet."),
        ("Was passiert, wenn ein unedles Metall wie Zink mit einer sauren Lösung reagiert?", new[] { "Es löst sich unter Wasserstoffgas-Entwicklung auf", "Es reagiert überhaupt nicht mit der Säure", "Es verwandelt sich in ein Edelmetall" }, "Es löst sich unter Wasserstoffgas-Entwicklung auf",
            "Unedle Metalle wie Zink reagieren mit Säuren, wobei sich das Metall auflöst und Wasserstoffgas entsteht."),
        ("Was passiert, wenn ein Carbonat (z.B. Kalk) mit einer Säure reagiert?", new[] { "Es entsteht Kohlenstoffdioxid-Gas unter Aufschäumen", "Es entsteht reiner Sauerstoff", "Es findet keinerlei Reaktion statt" }, "Es entsteht Kohlenstoffdioxid-Gas unter Aufschäumen",
            "Carbonate reagieren mit Säuren unter Bildung von Kohlenstoffdioxid, Wasser und einem Salz, sichtbar als Aufschäumen (Gasentwicklung)."),
        ("Was beschreibt das erweiterte Säure-Base-Konzept nach Brønsted?", new[] { "Säuren sind Protonendonatoren, Basen sind Protonenakzeptoren", "Säuren und Basen unterscheiden sich ausschließlich durch ihre Farbe", "Nach Brønsted gibt es keinen Unterschied zwischen Säuren und Basen" }, "Säuren sind Protonendonatoren, Basen sind Protonenakzeptoren",
            "Nach Brønsted ist eine Säure ein Stoff, der ein Proton (H⁺) abgibt, eine Base ein Stoff, der ein Proton aufnimmt."),
        ("Was ist ein Oxonium-Ion (H₃O⁺)?", new[] { "Ein Wassermolekül, das zusätzlich ein Proton aufgenommen hat", "Ein anderes Wort für Hydroxid-Ion", "Ein reines Sauerstoffmolekül ohne Wasserstoff" }, "Ein Wassermolekül, das zusätzlich ein Proton aufgenommen hat",
            "In wässriger Lösung docken abgegebene Protonen (H⁺) an Wassermoleküle an, wodurch Oxonium-Ionen (H₃O⁺) entstehen."),
        ("Was ist ein Hydroxid-Ion (OH⁻)?", new[] { "Ein negativ geladenes Ion aus einem Sauerstoff- und einem Wasserstoffatom, typisch für Basen", "Ein anderes Wort für Oxonium-Ion", "Ein Bestandteil, der nur in Säuren vorkommt" }, "Ein negativ geladenes Ion aus einem Sauerstoff- und einem Wasserstoffatom, typisch für Basen",
            "Hydroxid-Ionen (OH⁻) sind charakteristisch für basische (alkalische) Lösungen und entstehen z.B. beim Lösen von Laugen in Wasser."),
        ("Warum kann man das Entkalken einer Kaffeemaschine mit Essig als chemische Reaktion beschreiben?", new[] { "Die Essigsäure reagiert mit dem Kalk (Carbonat) und löst ihn dadurch auf", "Essig hat überhaupt keine Wirkung auf Kalkablagerungen", "Kalk besteht ausschließlich aus reinem Wasser" }, "Die Essigsäure reagiert mit dem Kalk (Carbonat) und löst ihn dadurch auf",
            "Essigsäure reagiert mit dem Calciumcarbonat (Kalk) unter Bildung von Kohlenstoffdioxid, Wasser und einem löslichen Salz, wodurch sich der Kalk auflöst."),
        ("Was zeigt ein niedriger pH-Wert (z.B. pH 2) über eine Lösung an?", new[] { "Sie ist stark sauer", "Sie ist stark basisch", "Sie ist absolut neutral" }, "Sie ist stark sauer",
            "Je niedriger der pH-Wert (unter 7), desto saurer ist die Lösung, das heißt desto mehr Wasserstoff-Ionen enthält sie."),
        ("Was zeigt ein hoher pH-Wert (z.B. pH 12) über eine Lösung an?", new[] { "Sie ist stark basisch (alkalisch)", "Sie ist stark sauer", "Sie ist absolut neutral" }, "Sie ist stark basisch (alkalisch)",
            "Je höher der pH-Wert (über 7), desto basischer ist die Lösung, das heißt desto mehr Hydroxid-Ionen enthält sie im Verhältnis."),
        ("Wie kann man den Donator-Akzeptor-Charakter der Neutralisation formulieren?", new[] { "Die Säure gibt ein Proton ab (Donator), die Base nimmt es auf (Akzeptor)", "Beide Reaktionspartner geben gleichzeitig ein Proton ab", "Weder Säure noch Base sind an einem Protonenübergang beteiligt" }, "Die Säure gibt ein Proton ab (Donator), die Base nimmt es auf (Akzeptor)",
            "Nach dem Brønsted-Konzept überträgt die Säure als Protonendonator ein Proton an die Base, die als Protonenakzeptor fungiert."),
        ("Was passiert mit dem pH-Wert, wenn man eine saure Lösung schrittweise mit einer Lauge neutralisiert?", new[] { "Der pH-Wert steigt schrittweise in Richtung 7 (neutral)", "Der pH-Wert sinkt immer weiter Richtung 0", "Der pH-Wert bleibt während der Neutralisation exakt unverändert" }, "Der pH-Wert steigt schrittweise in Richtung 7 (neutral)",
            "Gibt man einer sauren Lösung schrittweise eine Base hinzu, steigt der pH-Wert an, bis am Neutralisationspunkt idealerweise ein pH-Wert von etwa 7 erreicht wird."),
        ("Was entsteht typischerweise bei einer vollständigen Neutralisation neben Wasser?", new[] { "Ein Salz", "Ausschließlich ein neues Gas", "Reiner Sauerstoff" }, "Ein Salz",
            "Bei der Reaktion einer Säure mit einer Base entstehen neben Wasser auch die Ionen eines Salzes, das nach dem Eindampfen der Lösung sichtbar wird."),
        ("Warum eignet sich Essigsäure als vergleichsweise umweltschonender Haushaltsreiniger gegen Kalk?", new[] { "Sie ist eine schwächere organische Säure, die Kalk dennoch löst, aber weniger aggressiv als starke Mineralsäuren ist", "Essigsäure hat überhaupt keine chemische Wirkung auf Kalk", "Essigsäure ist die stärkste bekannte Säure überhaupt" }, "Sie ist eine schwächere organische Säure, die Kalk dennoch löst, aber weniger aggressiv als starke Mineralsäuren ist",
            "Essigsäure gilt als vergleichsweise milde, biologisch leichter abbaubare Säure, die Kalk zuverlässig löst, ohne so aggressiv wie starke anorganische Säuren zu sein."),
        ("Wie unterscheidet sich eine starke Säure von einer schwachen Säure hinsichtlich der Abgabe von Protonen?", new[] { "Eine starke Säure gibt ihre Protonen in Wasser nahezu vollständig ab, eine schwache nur teilweise", "Beide geben ihre Protonen exakt im gleichen Maß ab", "Schwache Säuren geben grundsätzlich mehr Protonen ab als starke Säuren" }, "Eine starke Säure gibt ihre Protonen in Wasser nahezu vollständig ab, eine schwache nur teilweise",
            "Starke Säuren dissoziieren in Wasser nahezu vollständig in Ionen, schwache Säuren dagegen nur teilweise - das beeinflusst auch den pH-Wert einer Lösung gleicher Konzentration."),
        ("Was ist ein Indikator in der Säure-Base-Chemie?", new[] { "Ein Stoff, der je nach pH-Wert seine Farbe ändert", "Ein Gerät zur Temperaturmessung", "Ein anderes Wort für ein Salz" }, "Ein Stoff, der je nach pH-Wert seine Farbe ändert",
            "Indikatoren wie Universalindikator oder Rotkohlsaft ändern ihre Farbe abhängig vom pH-Wert einer Lösung und zeigen so an, ob sie sauer oder basisch ist."),
        ("Warum ist die Reaktion von Metallen mit Säuren unter Wasserstoffentwicklung ein Beispiel für eine Redoxreaktion?", new[] { "Das Metall gibt Elektronen ab (Oxidation), die Wasserstoff-Ionen nehmen Elektronen auf (Reduktion)", "Bei dieser Reaktion werden überhaupt keine Elektronen übertragen", "Nur die Säure verändert sich chemisch, das Metall bleibt unverändert" }, "Das Metall gibt Elektronen ab (Oxidation), die Wasserstoff-Ionen nehmen Elektronen auf (Reduktion)",
            "Beim Auflösen eines unedlen Metalls in Säure gibt das Metall Elektronen ab (wird oxidiert), während Wasserstoff-Ionen diese Elektronen aufnehmen und zu Wasserstoffgas reduziert werden."),
        ("Was zeigt sich auf Teilchenebene, wenn man Kochsalz (NaCl) in Wasser löst, im Vergleich zu einer Säure-Base-Reaktion?", new[] { "Beim Lösen von Kochsalz findet kein Protonenübergang statt, anders als bei Säure-Base-Reaktionen", "Beim Lösen von Kochsalz findet immer ein Protonenübergang wie bei einer Neutralisation statt", "Kochsalz kann sich grundsätzlich nicht in Wasser lösen" }, "Beim Lösen von Kochsalz findet kein Protonenübergang statt, anders als bei Säure-Base-Reaktionen",
            "Das Lösen von Kochsalz ist ein rein physikalischer Vorgang (Dissoziation in Ionen durch Wasser), während bei Säure-Base-Reaktionen tatsächlich Protonen übertragen werden."),
        ("Warum wird beim Umgang mit konzentrierten Säuren und Laugen im Labor besondere Schutzausrüstung getragen?", new[] { "Beide können stark ätzend wirken und Haut, Augen oder Kleidung schädigen", "Säuren und Laugen sind im konzentrierten Zustand völlig ungefährlich", "Schutzausrüstung dient ausschließlich der Optik, nicht dem Schutz" }, "Beide können stark ätzend wirken und Haut, Augen oder Kleidung schädigen",
            "Konzentrierte Säuren und Laugen wirken stark ätzend und können Haut, Augen und Materialien schwer schädigen, weshalb Schutzbrille und Handschuhe Pflicht sind.")
    };

    private static QuizQuestion SaeureBaseVertieft(Random r)
    {
        var f = SaeureBaseListe[r.Next(SaeureBaseListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Säuren und Laugen – echt ätzend", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nach Brønsted ist eine Säure ein Protonendonator, eine Base ein Protonenakzeptor; bei der Neutralisation entstehen aus H⁺ und OH⁻ Wasser und ein Salz."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KohlenwasserstoffeListe =
    {
        ("Was ist ein Kohlenwasserstoff?", new[] { "Eine chemische Verbindung, die nur aus Kohlenstoff- und Wasserstoffatomen besteht", "Eine Verbindung, die ausschließlich Sauerstoff und Wasserstoff enthält", "Ein anderes Wort für ein Salz" }, "Eine chemische Verbindung, die nur aus Kohlenstoff- und Wasserstoffatomen besteht",
            "Kohlenwasserstoffe bestehen ausschließlich aus den Elementen Kohlenstoff (C) und Wasserstoff (H)."),
        ("Was zeichnet Alkane als gesättigte Kohlenwasserstoffe aus?", new[] { "Sie enthalten ausschließlich Einfachbindungen zwischen den Kohlenstoffatomen", "Sie enthalten mindestens eine Doppelbindung", "Sie enthalten mindestens eine Dreifachbindung" }, "Sie enthalten ausschließlich Einfachbindungen zwischen den Kohlenstoffatomen",
            "Alkane gelten als gesättigt, weil zwischen den Kohlenstoffatomen ausschließlich Einfachbindungen vorliegen und jedes C-Atom maximal mit Wasserstoff abgesättigt ist."),
        ("Was unterscheidet ein Alken von einem Alkan?", new[] { "Ein Alken besitzt mindestens eine Kohlenstoff-Kohlenstoff-Doppelbindung", "Ein Alken besteht nur aus Wasserstoffatomen", "Alkene enthalten grundsätzlich keinen Kohlenstoff" }, "Ein Alken besitzt mindestens eine Kohlenstoff-Kohlenstoff-Doppelbindung",
            "Alkene sind ungesättigte Kohlenwasserstoffe mit mindestens einer C=C-Doppelbindung, im Gegensatz zu den nur einfach gebundenen Alkanen."),
        ("Was zeichnet ein Alkin aus?", new[] { "Es besitzt mindestens eine Kohlenstoff-Kohlenstoff-Dreifachbindung", "Es besitzt ausschließlich Einfachbindungen", "Es enthält kein einziges Kohlenstoffatom" }, "Es besitzt mindestens eine Kohlenstoff-Kohlenstoff-Dreifachbindung",
            "Alkine sind ungesättigte Kohlenwasserstoffe mit mindestens einer C≡C-Dreifachbindung, z.B. Ethin (Acetylen)."),
        ("Was versteht man unter einer \"homologen Reihe\" bei Alkanen?", new[] { "Eine Reihe von Verbindungen, die sich jeweils um eine CH₂-Gruppe unterscheiden und ähnliche Eigenschaften haben", "Eine zufällige Auswahl völlig unterschiedlicher Stoffklassen", "Eine Reihe von Metallen im Periodensystem" }, "Eine Reihe von Verbindungen, die sich jeweils um eine CH₂-Gruppe unterscheiden und ähnliche Eigenschaften haben",
            "In einer homologen Reihe wie der der Alkane unterscheidet sich jedes Glied vom nächsten durch eine zusätzliche CH₂-Gruppe, wobei sich die Eigenschaften regelmäßig verändern."),
        ("Warum steigt der Siedepunkt innerhalb der homologen Reihe der Alkane mit zunehmender Kettenlänge meist an?", new[] { "Längere Ketten haben stärkere Van-der-Waals-Kräfte zwischen den Molekülen", "Längere Ketten haben grundsätzlich schwächere zwischenmolekulare Kräfte", "Die Kettenlänge hat keinerlei Einfluss auf den Siedepunkt" }, "Längere Ketten haben stärkere Van-der-Waals-Kräfte zwischen den Molekülen",
            "Mit zunehmender Molekülgröße und Kettenlänge nehmen die Van-der-Waals-Kräfte zwischen den Molekülen zu, wodurch mehr Energie zum Trennen (Sieden) nötig ist und der Siedepunkt steigt."),
        ("Was sind Van-der-Waals-Kräfte?", new[] { "Schwache, zwischenmolekulare Anziehungskräfte zwischen unpolaren Molekülen", "Starke chemische Bindungen innerhalb eines Moleküls", "Ein anderes Wort für Ionenbindungen" }, "Schwache, zwischenmolekulare Anziehungskräfte zwischen unpolaren Molekülen",
            "Van-der-Waals-Kräfte sind schwache, aber mit zunehmender Molekülgröße stärker werdende Anziehungskräfte zwischen unpolaren Molekülen wie Alkanen."),
        ("Was ist Isomerie bei Kohlenwasserstoffen?", new[] { "Verschiedene Moleküle mit derselben Summenformel, aber unterschiedlichem Aufbau", "Zwei völlig identische Moleküle mit unterschiedlicher Summenformel", "Ein anderes Wort für eine chemische Reaktion" }, "Verschiedene Moleküle mit derselben Summenformel, aber unterschiedlichem Aufbau",
            "Isomere haben dieselbe Summenformel, unterscheiden sich aber in der Anordnung der Atome (Strukturisomerie), was zu unterschiedlichen Eigenschaften führen kann."),
        ("Was ist Methan (CH₄), das einfachste Alkan, in der Natur häufig?", new[] { "Ein Hauptbestandteil von Erdgas", "Ein seltenes Edelmetall", "Eine feste Kristallstruktur bei Raumtemperatur" }, "Ein Hauptbestandteil von Erdgas",
            "Methan (CH₄) ist das einfachste Alkan und ein Hauptbestandteil von Erdgas, das u.a. als Campinggas verwendet wird."),
        ("Warum haben kurzkettige Alkane wie Methan oder Propan bei Raumtemperatur einen gasförmigen Zustand?", new[] { "Ihre Van-der-Waals-Kräfte sind wegen der kurzen Kette relativ schwach", "Ihre Van-der-Waals-Kräfte sind extrem stark", "Sie besitzen keinerlei zwischenmolekulare Kräfte" }, "Ihre Van-der-Waals-Kräfte sind wegen der kurzen Kette relativ schwach",
            "Kurzkettige Alkane haben schwächere Van-der-Waals-Kräfte, wodurch schon bei niedrigen Temperaturen die Molekülbewegung ausreicht, um den gasförmigen Zustand zu erreichen."),
        ("Warum wird Superbenzin (längerkettige Kohlenwasserstoffe) bei Raumtemperatur flüssig, während Campinggas gasförmig bleibt?", new[] { "Längere Kohlenstoffketten haben stärkere Van-der-Waals-Kräfte und damit einen höheren Siedepunkt", "Beide Stoffe haben exakt denselben Aggregatzustand", "Kettenlänge hat keinen Einfluss auf den Aggregatzustand" }, "Längere Kohlenstoffketten haben stärkere Van-der-Waals-Kräfte und damit einen höheren Siedepunkt",
            "Die längeren Molekülketten in Benzinbestandteilen erzeugen stärkere Van-der-Waals-Kräfte, wodurch ihr Siedepunkt über der Raumtemperatur liegt und sie flüssig bleiben."),
        ("Was bezeichnet die Nomenklatur \"Propan\", \"Butan\", \"Pentan\" bei Alkanen?", new[] { "Die Namen geben die Anzahl der Kohlenstoffatome in der Kette an", "Die Namen beziehen sich auf die Farbe der Stoffe", "Die Namen haben nichts mit der Molekülstruktur zu tun" }, "Die Namen geben die Anzahl der Kohlenstoffatome in der Kette an",
            "Die Namensendungen der Alkane (z.B. Prop-, But-, Pent-) leiten sich von der Anzahl der Kohlenstoffatome in der Hauptkette ab (3, 4, 5 Kohlenstoffatome)."),
        ("Was passiert chemisch bei der vollständigen Verbrennung eines Kohlenwasserstoffs wie Methan?", new[] { "Es entstehen Kohlenstoffdioxid und Wasser unter Freisetzung von Energie", "Es entsteht ausschließlich reiner Kohlenstoff", "Es findet keinerlei chemische Reaktion statt" }, "Es entstehen Kohlenstoffdioxid und Wasser unter Freisetzung von Energie",
            "Bei vollständiger Verbrennung reagieren Kohlenwasserstoffe mit Sauerstoff zu Kohlenstoffdioxid und Wasser, wobei Energie in Form von Wärme freigesetzt wird."),
        ("Warum sind Alkene chemisch reaktiver als Alkane?", new[] { "Die Doppelbindung kann leichter aufgebrochen und für Additionsreaktionen genutzt werden", "Alkene besitzen grundsätzlich mehr Kohlenstoffatome als Alkane", "Alkene sind chemisch komplett träge und reagieren nie" }, "Die Doppelbindung kann leichter aufgebrochen und für Additionsreaktionen genutzt werden",
            "Die C=C-Doppelbindung von Alkenen kann leichter aufgebrochen werden, was Additionsreaktionen (z.B. mit Wasserstoff oder Halogenen) ermöglicht - Alkane reagieren dagegen deutlich träger."),
        ("Was passiert bei einer Additionsreaktion an einem Alken, z.B. mit Wasserstoff?", new[] { "Die Doppelbindung wird aufgebrochen und zusätzliche Atome werden angelagert", "Es entsteht dabei keinerlei neue Verbindung", "Die Doppelbindung bleibt dabei zwingend vollständig erhalten" }, "Die Doppelbindung wird aufgebrochen und zusätzliche Atome werden angelagert",
            "Bei einer Addition wird die Doppelbindung des Alkens aufgebrochen, wobei sich zusätzliche Atome (z.B. Wasserstoff) an die beiden Kohlenstoffatome anlagern."),
        ("Was ist Ethin (Acetylin), ein bekanntes Alkin, für ein Beispiel?", new[] { "Ein Gas, das u.a. beim Schweißen als Brenngas mit sehr heißer Flamme genutzt wird", "Ein Metall, das bei Raumtemperatur fest ist", "Ein Bestandteil von Kochsalz" }, "Ein Gas, das u.a. beim Schweißen als Brenngas mit sehr heißer Flamme genutzt wird",
            "Ethin (Acetylen) verbrennt mit sehr hoher Flammtemperatur und wird deshalb u.a. beim Autogenschweißen als Brenngas eingesetzt."),
        ("Was ist der Unterschied zwischen einer geradkettigen und einer verzweigten Isomerie bei Alkanen?", new[] { "Bei verzweigter Isomerie zweigen zusätzliche Kohlenstoffketten von der Hauptkette ab", "Beide Formen haben exakt dieselbe Struktur", "Geradkettige Isomere besitzen grundsätzlich mehr Kohlenstoffatome" }, "Bei verzweigter Isomerie zweigen zusätzliche Kohlenstoffketten von der Hauptkette ab",
            "Bei verzweigten Isomeren gehen von der Hauptkette zusätzliche Seitenketten ab, wodurch sich trotz gleicher Summenformel andere physikalische Eigenschaften (z.B. Siedepunkt) ergeben."),
        ("Warum haben verzweigte Alkane oft einen niedrigeren Siedepunkt als ihre geradkettigen Isomere?", new[] { "Verzweigte Moleküle haben eine kompaktere Form mit weniger Kontaktfläche für Van-der-Waals-Kräfte", "Verzweigte Moleküle haben grundsätzlich stärkere Van-der-Waals-Kräfte", "Die Verzweigung hat keinerlei Einfluss auf den Siedepunkt" }, "Verzweigte Moleküle haben eine kompaktere Form mit weniger Kontaktfläche für Van-der-Waals-Kräfte",
            "Verzweigte Isomere sind kompakter geformt und bieten benachbarten Molekülen weniger Kontaktfläche, wodurch die Van-der-Waals-Kräfte schwächer sind und der Siedepunkt sinkt."),
        ("Warum zählt Erdöl als wichtigste Rohstoffquelle für viele Kohlenwasserstoffe wie Benzin?", new[] { "Erdöl besteht aus einem komplexen Gemisch verschiedenster Kohlenwasserstoffe, die destillativ getrennt werden können", "Erdöl enthält überhaupt keine Kohlenwasserstoffe", "Erdöl besteht ausschließlich aus reinem Methan" }, "Erdöl besteht aus einem komplexen Gemisch verschiedenster Kohlenwasserstoffe, die destillativ getrennt werden können",
            "Erdöl ist ein Gemisch unterschiedlichster Kohlenwasserstoffe, das durch fraktionierte Destillation in verschiedene Produkte wie Benzin, Diesel oder Gase aufgetrennt wird."),
        ("Was versteht man unter der \"fraktionierten Destillation\" von Erdöl?", new[] { "Die Auftrennung des Kohlenwasserstoffgemischs anhand unterschiedlicher Siedepunkte in einzelne Fraktionen", "Ein Verfahren, das ausschließlich Wasser aus Erdöl entfernt", "Ein Vorgang, bei dem Erdöl chemisch in Sauerstoff umgewandelt wird" }, "Die Auftrennung des Kohlenwasserstoffgemischs anhand unterschiedlicher Siedepunkte in einzelne Fraktionen",
            "Bei der fraktionierten Destillation wird Erdöl erhitzt und die enthaltenen Kohlenwasserstoffe verdampfen bei unterschiedlichen Temperaturen, wodurch sie in einzelne Fraktionen wie Benzin, Diesel oder Heizöl getrennt werden können.")
    };

    private static QuizQuestion Kohlenwasserstoffe(Random r)
    {
        var f = KohlenwasserstoffeListe[r.Next(KohlenwasserstoffeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Kohlenwasserstoffe – vom Campinggas zum Superbenzin", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Alkane haben nur Einfachbindungen, Alkene mindestens eine Doppel-, Alkine mindestens eine Dreifachbindung; längere Ketten haben stärkere Van-der-Waals-Kräfte und höhere Siedepunkte."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AlkoholeListe =
    {
        ("Was ist die funktionelle Gruppe, die Alkohole (Alkanole) kennzeichnet?", new[] { "Die Hydroxy-Gruppe (-OH)", "Die Carboxy-Gruppe", "Die Estergruppe" }, "Die Hydroxy-Gruppe (-OH)",
            "Alkohole (Alkanole) besitzen als charakteristische funktionelle Gruppe die Hydroxy-Gruppe (-OH), die an ein Kohlenstoffatom gebunden ist."),
        ("Was ist der bekannteste und in Getränken enthaltene Alkohol?", new[] { "Ethanol", "Methanol", "Glycerin" }, "Ethanol",
            "Ethanol ist der Alkohol, der in alkoholischen Getränken enthalten ist und z.B. durch Vergärung von Zucker entsteht."),
        ("Warum wird Methanol umgangssprachlich manchmal als \"Holzgeist\" bezeichnet?", new[] { "Es wurde früher u.a. durch trockene Destillation von Holz gewonnen", "Weil es ausschließlich in Bäumen wächst", "Weil es keinerlei industrielle Bedeutung hat" }, "Es wurde früher u.a. durch trockene Destillation von Holz gewonnen",
            "Methanol wurde historisch unter anderem durch trockene Destillation (Erhitzen ohne Luft) von Holz gewonnen, daher der Name \"Holzgeist\"."),
        ("Warum ist Methanol im Gegensatz zu Ethanol für den Menschen hochgiftig?", new[] { "Es wird im Körper zu giftigen Abbauprodukten wie Formaldehyd und Ameisensäure umgewandelt", "Methanol hat exakt dieselbe Wirkung wie Ethanol", "Methanol wird vom Körper überhaupt nicht aufgenommen" }, "Es wird im Körper zu giftigen Abbauprodukten wie Formaldehyd und Ameisensäure umgewandelt",
            "Im Körper wird Methanol zu giftigem Formaldehyd und weiter zu Ameisensäure abgebaut, die u.a. den Sehnerv schädigen können."),
        ("Was ist Glycerin (Propantriol) im Vergleich zu einfachen Alkoholen wie Ethanol?", new[] { "Ein mehrwertiger Alkohol mit drei Hydroxy-Gruppen", "Ein Alkohol ganz ohne Hydroxy-Gruppe", "Ein anderes Wort für Methanol" }, "Ein mehrwertiger Alkohol mit drei Hydroxy-Gruppen",
            "Glycerin besitzt gleich drei Hydroxy-Gruppen pro Molekül und zählt deshalb zu den mehrwertigen Alkoholen."),
        ("Warum sind kurzkettige Alkohole wie Ethanol gut in Wasser löslich?", new[] { "Die Hydroxy-Gruppe kann Wasserstoffbrückenbindungen mit Wassermolekülen ausbilden", "Kurzkettige Alkohole besitzen keinerlei funktionelle Gruppe", "Wasserstoffbrückenbindungen spielen bei der Löslichkeit keine Rolle" }, "Die Hydroxy-Gruppe kann Wasserstoffbrückenbindungen mit Wassermolekülen ausbilden",
            "Die polare Hydroxy-Gruppe kurzkettiger Alkohole bildet Wasserstoffbrückenbindungen zu Wassermolekülen aus, was eine gute Löslichkeit ermöglicht."),
        ("Warum werden langkettige Alkohole zunehmend schlechter wasserlöslich?", new[] { "Der wachsende, unpolare Kohlenwasserstoffrest überwiegt gegenüber der einzelnen polaren Hydroxy-Gruppe", "Langkettige Alkohole besitzen mehr Hydroxy-Gruppen als kurzkettige", "Die Kettenlänge hat keinerlei Einfluss auf die Löslichkeit" }, "Der wachsende, unpolare Kohlenwasserstoffrest überwiegt gegenüber der einzelnen polaren Hydroxy-Gruppe",
            "Mit zunehmender Kettenlänge dominiert der unpolare Kohlenwasserstoffanteil des Moleküls, wodurch die wasserlösende Wirkung der einzelnen Hydroxy-Gruppe relativ abnimmt."),
        ("Was bedeutet \"hydrophil\" bei einem Molekülteil?", new[] { "Wasseranziehend, gut wasserlöslich", "Wasserabstoßend", "Ein anderes Wort für giftig" }, "Wasseranziehend, gut wasserlöslich",
            "Hydrophile (\"wasserliebende\") Molekülteile wie die Hydroxy-Gruppe interagieren gut mit polaren Wassermolekülen."),
        ("Was bedeutet \"hydrophob\" bei einem Molekülteil?", new[] { "Wasserabstoßend, schlecht wasserlöslich", "Wasseranziehend", "Ein anderes Wort für eine Säure" }, "Wasserabstoßend, schlecht wasserlöslich",
            "Hydrophobe (\"wasserabweisende\") Molekülteile wie lange Kohlenwasserstoffketten mischen sich schlecht mit Wasser."),
        ("Was entsteht bei der Oxidation eines primären Alkohols wie Ethanol?", new[] { "Ein Aldehyd (Alkanal), z.B. Ethanal", "Ausschließlich reines Wasser", "Ein Edelmetall" }, "Ein Aldehyd (Alkanal), z.B. Ethanal",
            "Primäre Alkohole werden bei der Oxidation zunächst zu Aldehyden (Alkanalen) umgesetzt, z.B. Ethanol zu Ethanal."),
        ("Was kennzeichnet die Aldehyd-Gruppe chemisch?", new[] { "Eine Kohlenstoff-Sauerstoff-Doppelbindung mit einem zusätzlichen Wasserstoffatom am selben Kohlenstoff", "Eine Gruppe, die ausschließlich aus Kohlenstoff und Stickstoff besteht", "Ein anderes Wort für die Hydroxy-Gruppe" }, "Eine Kohlenstoff-Sauerstoff-Doppelbindung mit einem zusätzlichen Wasserstoffatom am selben Kohlenstoff",
            "Die Aldehyd-Gruppe (-CHO) besteht aus einem Kohlenstoffatom mit einer Doppelbindung zu Sauerstoff und einem gebundenen Wasserstoffatom."),
        ("Was passiert mit der Oxidationszahl des Kohlenstoffatoms, wenn Ethanol zu Ethanal oxidiert wird?", new[] { "Sie steigt an, da bei der Oxidation Elektronen abgegeben werden", "Sie sinkt, da Elektronen aufgenommen werden", "Sie bleibt exakt unverändert" }, "Sie steigt an, da bei der Oxidation Elektronen abgegeben werden",
            "Bei einer Oxidationsreaktion steigt die Oxidationszahl des betroffenen Kohlenstoffatoms an, da formal Elektronen abgegeben werden."),
        ("Warum ist ein primärer Alkohol anders aufgebaut als ein sekundärer oder tertiärer Alkohol?", new[] { "Die Anzahl der Kohlenstoffatome, die direkt an das Kohlenstoffatom mit der OH-Gruppe gebunden sind, unterscheidet sich", "Alle Alkoholtypen haben exakt denselben Aufbau", "Nur primäre Alkohole besitzen überhaupt eine Hydroxy-Gruppe" }, "Die Anzahl der Kohlenstoffatome, die direkt an das Kohlenstoffatom mit der OH-Gruppe gebunden sind, unterscheidet sich",
            "Primäre, sekundäre und tertiäre Alkohole unterscheiden sich danach, wie viele weitere Kohlenstoffatome direkt an das die Hydroxy-Gruppe tragende Kohlenstoffatom gebunden sind."),
        ("Wofür wird Ethanol in der Industrie und im Alltag unter anderem verwendet?", new[] { "Als Lösungsmittel, Desinfektionsmittel und Kraftstoffbeimischung", "Ausschließlich zum Trinken", "Ausschließlich als Baumaterial" }, "Als Lösungsmittel, Desinfektionsmittel und Kraftstoffbeimischung",
            "Ethanol wird u.a. als Lösungsmittel, Desinfektionsmittel und als Beimischung zu Kraftstoffen (Biosprit) genutzt."),
        ("Warum wird Glycerin häufig in Kosmetikprodukten eingesetzt?", new[] { "Es bindet gut Feuchtigkeit aufgrund seiner mehreren Hydroxy-Gruppen", "Es ist stark giftig und wirkt deshalb konservierend", "Es hat überhaupt keine Wechselwirkung mit Wasser" }, "Es bindet gut Feuchtigkeit aufgrund seiner mehreren Hydroxy-Gruppen",
            "Die mehreren Hydroxy-Gruppen des Glycerins können viele Wasserstoffbrückenbindungen zu Wasser ausbilden, was Feuchtigkeit bindet - deshalb wird es oft in Cremes verwendet."),
        ("Was passiert bei einer weiteren Oxidation eines Aldehyds (Alkanals)?", new[] { "Es entsteht eine Carbonsäure (Alkansäure)", "Es entsteht wieder der ursprüngliche Alkohol", "Es entsteht ein Edelgas" }, "Es entsteht eine Carbonsäure (Alkansäure)",
            "Wird ein Aldehyd weiter oxidiert, entsteht daraus eine Carbonsäure, z.B. aus Ethanal entsteht Essigsäure (Ethansäure)."),
        ("Warum verdunstet Ethanol bei Raumtemperatur schneller als Wasser?", new[] { "Ethanol hat schwächere zwischenmolekulare Kräfte als Wasser und dadurch einen niedrigeren Siedepunkt", "Ethanol hat stärkere zwischenmolekulare Kräfte als Wasser", "Beide Stoffe verdunsten exakt gleich schnell" }, "Ethanol hat schwächere zwischenmolekulare Kräfte als Wasser und dadurch einen niedrigeren Siedepunkt",
            "Ethanol besitzt insgesamt schwächere zwischenmolekulare Anziehungskräfte als Wasser, wodurch es einen niedrigeren Siedepunkt hat und schneller verdunstet."),
        ("Was zeigt die Löslichkeit von Ethanol sowohl in Wasser als auch in unpolaren Stoffen wie Ölen?", new[] { "Ethanol besitzt sowohl einen polaren (Hydroxy-Gruppe) als auch einen unpolaren Molekülteil (Kohlenwasserstoffkette)", "Ethanol besteht ausschließlich aus einem einzigen, völlig unpolaren Molekülteil", "Löslichkeit hat mit dem Molekülbau überhaupt nichts zu tun" }, "Ethanol besitzt sowohl einen polaren (Hydroxy-Gruppe) als auch einen unpolaren Molekülteil (Kohlenwasserstoffkette)",
            "Da Ethanol sowohl eine polare Hydroxy-Gruppe als auch einen unpolaren Kohlenwasserstoffrest besitzt, kann es sowohl mit polaren (Wasser) als auch unpolaren Stoffen wechselwirken."),
        ("Was ist ein Grund, warum Alkoholkonsum den Körper beeinträchtigt?", new[] { "Ethanol wirkt auf das zentrale Nervensystem und beeinflusst dort die Signalübertragung", "Ethanol hat auf den Körper überhaupt keine erkennbare Wirkung", "Ethanol wird vom Körper vollständig ignoriert und gar nicht abgebaut" }, "Ethanol wirkt auf das zentrale Nervensystem und beeinflusst dort die Signalübertragung",
            "Ethanol beeinflusst als psychoaktive Substanz die Signalübertragung im zentralen Nervensystem, was Reaktionsfähigkeit und Urteilsvermögen einschränken kann."),
        ("Was passiert im Körper beim Abbau von Ethanol, das zunächst über die Leber verarbeitet wird?", new[] { "Ethanol wird schrittweise über Acetaldehyd zu Essigsäure oxidiert", "Ethanol wird ohne jede chemische Veränderung ausgeatmet", "Ethanol wandelt sich direkt in Glucose um" }, "Ethanol wird schrittweise über Acetaldehyd zu Essigsäure oxidiert",
            "Die Leber baut Ethanol in mehreren Oxidationsschritten ab: zunächst zu Acetaldehyd, dann weiter zu Essigsäure, die der Körper schließlich verwerten kann.")
    };

    private static QuizQuestion Alkohole(Random r)
    {
        var f = AlkoholeListe[r.Next(AlkoholeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Alkohole – vom Holzgeist zum Glycerin", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Alkohole tragen eine Hydroxy-Gruppe (-OH); kurze Ketten sind gut wasserlöslich (Wasserstoffbrücken), primäre Alkohole werden über Aldehyde zu Carbonsäuren oxidiert."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] OrganischeSaeurenListe =
    {
        ("Welche funktionelle Gruppe kennzeichnet organische Säuren (Alkansäuren)?", new[] { "Die Carboxy-Gruppe (-COOH)", "Die Hydroxy-Gruppe", "Die Estergruppe" }, "Die Carboxy-Gruppe (-COOH)",
            "Carbonsäuren (Alkansäuren) besitzen als charakteristische funktionelle Gruppe die Carboxy-Gruppe (-COOH)."),
        ("Was ist die im Essig enthaltene organische Säure?", new[] { "Essigsäure (Ethansäure)", "Salzsäure", "Schwefelsäure" }, "Essigsäure (Ethansäure)",
            "Essig enthält verdünnte Essigsäure (Ethansäure, CH₃COOH), eine typische Carbonsäure."),
        ("Wie verändert sich die Säurestärke organischer Carbonsäuren im Vergleich zu vielen anorganischen Säuren wie Salzsäure?", new[] { "Carbonsäuren sind meist schwächer und dissoziieren in Wasser nur teilweise", "Carbonsäuren sind grundsätzlich stärker als jede anorganische Säure", "Beide Säuretypen sind chemisch exakt identisch" }, "Carbonsäuren sind meist schwächer und dissoziieren in Wasser nur teilweise",
            "Organische Carbonsäuren wie Essigsäure gelten meist als schwächere Säuren, da sie in Wasser nur teilweise in Ionen dissoziieren, im Gegensatz zu starken Säuren wie Salzsäure."),
        ("Was passiert innerhalb der homologen Reihe der Alkansäuren mit zunehmender Kettenlänge, ähnlich wie bei Alkanen?", new[] { "Der Schmelz- und Siedepunkt steigt tendenziell an", "Der Schmelz- und Siedepunkt sinkt immer weiter", "Die Kettenlänge hat keinerlei Einfluss auf diese Eigenschaften" }, "Der Schmelz- und Siedepunkt steigt tendenziell an",
            "Wie bei anderen homologen Reihen nehmen auch bei den Alkansäuren mit wachsender Kettenlänge die zwischenmolekularen Kräfte und damit meist Schmelz- und Siedepunkt zu."),
        ("Was sind Aminosäuren im Vergleich zu einfachen Carbonsäuren?", new[] { "Moleküle, die sowohl eine Carboxy- als auch eine Amino-Gruppe besitzen", "Moleküle, die ausschließlich aus Kohlenwasserstoffketten bestehen", "Ein anderes Wort für Alkansäuren allgemein" }, "Moleküle, die sowohl eine Carboxy- als auch eine Amino-Gruppe besitzen",
            "Aminosäuren besitzen neben der Carboxy-Gruppe zusätzlich eine Amino-Gruppe (-NH₂) und sind die Bausteine von Proteinen."),
        ("Warum gelten Aminosäuren als wichtige Bausteine des Lebens?", new[] { "Sie verbinden sich zu Proteinen, die zentrale Funktionen im Körper übernehmen", "Sie haben mit Proteinen überhaupt nichts zu tun", "Sie kommen ausschließlich in Mineralien vor" }, "Sie verbinden sich zu Proteinen, die zentrale Funktionen im Körper übernehmen",
            "Aminosäuren verknüpfen sich zu Ketten (Proteinen), die im Körper u.a. Strukturen bilden, Stoffe transportieren und als Enzyme Reaktionen ermöglichen."),
        ("Was ist ein Vorteil von Essigsäure als Haushaltsreiniger gegenüber sehr starken anorganischen Säuren?", new[] { "Sie ist bei richtiger Anwendung weniger aggressiv und leichter biologisch abbaubar", "Essigsäure hat keinerlei reinigende Wirkung", "Essigsäure ist grundsätzlich gefährlicher als starke Mineralsäuren" }, "Sie ist bei richtiger Anwendung weniger aggressiv und leichter biologisch abbaubar",
            "Essigsäure gilt bei sachgemäßer Verdünnung als vergleichsweise milde und gut abbaubare Alternative zu aggressiveren, stark ätzenden Reinigungssäuren."),
        ("Was zeigt die Struktur der Carboxy-Gruppe (-COOH) chemisch?", new[] { "Eine Kombination aus einer Carbonylgruppe (C=O) und einer Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom", "Ausschließlich eine einfache Kohlenstoff-Wasserstoff-Bindung", "Eine reine Metall-Sauerstoff-Verbindung" }, "Eine Kombination aus einer Carbonylgruppe (C=O) und einer Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom",
            "Die Carboxy-Gruppe vereint strukturell eine Carbonyl- (C=O) und eine Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom, was ihre saure Wirkung erklärt."),
        ("Warum kann eine Carbonsäure in Wasser Protonen abgeben und damit als Säure wirken?", new[] { "Das Wasserstoffatom der -OH-Gruppe in der Carboxy-Gruppe kann als Proton abgespalten werden", "Carbonsäuren besitzen überhaupt kein abspaltbares Wasserstoffatom", "Nur anorganische Säuren können Protonen abgeben" }, "Das Wasserstoffatom der -OH-Gruppe in der Carboxy-Gruppe kann als Proton abgespalten werden",
            "Das Wasserstoffatom der Carboxy-Gruppe ist relativ leicht als Proton (H⁺) abspaltbar, wodurch Carbonsäuren in Wasser sauer reagieren."),
        ("Was ist Zitronensäure ein bekanntes Beispiel für?", new[] { "Eine natürlich vorkommende organische Säure in Früchten wie Zitronen", "Eine anorganische Säure aus dem Labor", "Ein Metall aus dem Periodensystem" }, "Eine natürlich vorkommende organische Säure in Früchten wie Zitronen",
            "Zitronensäure ist eine natürlich in Zitrusfrüchten vorkommende Carbonsäure, die z.B. für deren sauren Geschmack verantwortlich ist."),
        ("Was ist Ameisensäure (Methansäure), die einfachste Carbonsäure, ein Beispiel für?", new[] { "Eine Säure, die auch natürlich z.B. in manchen Insektenstichen vorkommt", "Ein Edelmetall", "Ein reines Edelgas" }, "Eine Säure, die auch natürlich z.B. in manchen Insektenstichen vorkommt",
            "Ameisensäure kommt natürlich vor, u.a. im Gift mancher Ameisen und Brennnesseln, und ist die einfachste Alkansäure."),
        ("Was passiert grundsätzlich beim Vergleich der Säurestärke innerhalb der homologen Reihe der Alkansäuren mit zunehmender Kettenlänge?", new[] { "Die Säurestärke nimmt tendenziell leicht ab", "Die Säurestärke nimmt immer stark zu", "Die Kettenlänge hat überhaupt keinen Einfluss auf die Säurestärke" }, "Die Säurestärke nimmt tendenziell leicht ab",
            "Mit zunehmender Kettenlänge nimmt der elektronenschiebende Effekt der Kohlenwasserstoffkette zu, was die Säurestärke der Carbonsäure tendenziell leicht verringert."),
        ("Was unterscheidet organische Carbonsäuren strukturell grundlegend von anorganischen Säuren wie Salzsäure?", new[] { "Organische Säuren enthalten eine Kohlenstoffkette mit funktioneller Carboxy-Gruppe, anorganische Säuren nicht", "Beide Säuretypen besitzen exakt dieselbe chemische Struktur", "Anorganische Säuren enthalten immer eine Carboxy-Gruppe" }, "Organische Säuren enthalten eine Kohlenstoffkette mit funktioneller Carboxy-Gruppe, anorganische Säuren nicht",
            "Organische Säuren basieren auf einem Kohlenwasserstoffgerüst mit angehängter Carboxy-Gruppe, während anorganische Säuren wie Salzsäure kein Kohlenstoffgerüst besitzen."),
        ("Wofür wird Essigsäure im Haushalt neben der Verwendung als Speisezutat noch häufig eingesetzt?", new[] { "Zum Entkalken von Geräten wie Wasserkochern oder Kaffeemaschinen", "Ausschließlich zum Färben von Textilien", "Ausschließlich zur Metallverarbeitung in der Industrie" }, "Zum Entkalken von Geräten wie Wasserkochern oder Kaffeemaschinen",
            "Essigsäure wird im Haushalt häufig zum Entkalken genutzt, da sie mit Kalkablagerungen (Carbonaten) reagiert und diese auflöst."),
        ("Was passiert, wenn zwei Aminosäuren unter Wasserabspaltung miteinander verbunden werden?", new[] { "Es entsteht eine Peptidbindung zwischen ihnen", "Es entsteht eine Esterbindung wie bei Fetten", "Es findet keinerlei chemische Reaktion statt" }, "Es entsteht eine Peptidbindung zwischen ihnen",
            "Aminosäuren verknüpfen sich unter Wasserabspaltung über eine sogenannte Peptidbindung zu längeren Ketten, den Proteinen."),
        ("Warum ist die Umweltverträglichkeit ein wichtiges Kriterium beim Vergleich verschiedener Haushaltsreiniger?", new[] { "Manche Reinigungsmittel können, wenn sie ins Abwasser gelangen, Gewässer oder Böden belasten", "Umweltverträglichkeit spielt bei Reinigungsmitteln überhaupt keine Rolle", "Alle Haushaltsreiniger sind grundsätzlich völlig umweltneutral" }, "Manche Reinigungsmittel können, wenn sie ins Abwasser gelangen, Gewässer oder Böden belasten",
            "Nicht biologisch abbaubare oder besonders aggressive Reinigungsmittel können bei falscher Entsorgung Gewässer oder Böden belasten - deshalb wird ihre Umweltverträglichkeit bewertet."),
        ("Was ist ein Grund, warum Milchsäure beim Sport in den Muskeln entstehen kann?", new[] { "Bei intensiver Belastung wird Energie teilweise ohne ausreichend Sauerstoff gewonnen, wobei Milchsäure entsteht", "Milchsäure hat mit sportlicher Belastung überhaupt nichts zu tun", "Milchsäure entsteht ausschließlich beim Schlafen" }, "Bei intensiver Belastung wird Energie teilweise ohne ausreichend Sauerstoff gewonnen, wobei Milchsäure entsteht",
            "Bei hoher körperlicher Belastung mit unzureichender Sauerstoffversorgung gewinnt der Muskel Energie teils anaerob, wobei Milchsäure als Nebenprodukt entsteht."),
        ("Warum sind viele Carbonsäuren mit kurzer Kette wie Essigsäure gut wasserlöslich?", new[] { "Die polare Carboxy-Gruppe kann Wasserstoffbrückenbindungen zu Wassermolekülen bilden", "Kurzkettige Carbonsäuren besitzen keinerlei polare Gruppe", "Wasserlöslichkeit hat mit der Molekülstruktur nichts zu tun" }, "Die polare Carboxy-Gruppe kann Wasserstoffbrückenbindungen zu Wassermolekülen bilden",
            "Ähnlich wie bei kurzkettigen Alkoholen ermöglicht die polare, wasserstoffbrückenbildende Carboxy-Gruppe eine gute Löslichkeit kurzkettiger Carbonsäuren in Wasser."),
        ("Was passiert grundsätzlich mit der Löslichkeit von Carbonsäuren in Wasser, wenn ihre Kohlenwasserstoffkette immer länger wird?", new[] { "Die Löslichkeit nimmt tendenziell ab, weil der unpolare Kettenanteil überwiegt", "Die Löslichkeit steigt immer weiter an", "Die Kettenlänge hat keinerlei Einfluss auf die Löslichkeit" }, "Die Löslichkeit nimmt tendenziell ab, weil der unpolare Kettenanteil überwiegt",
            "Wie bei den Alkoholen überwiegt bei längeren Ketten zunehmend der unpolare Kohlenwasserstoffanteil, wodurch die Wasserlöslichkeit der Carbonsäure abnimmt."),
        ("Was ist Buttersäure ein bekanntes (wenn auch unangenehm riechendes) Beispiel für?", new[] { "Eine kurzkettige Carbonsäure, die z.B. beim Ranzigwerden von Butter entsteht", "Ein Edelmetall", "Ein reines Edelgas ohne jeden Eigengeruch" }, "Eine kurzkettige Carbonsäure, die z.B. beim Ranzigwerden von Butter entsteht",
            "Buttersäure ist eine kurzkettige Carbonsäure mit unangenehmem Geruch, die u.a. beim Ranzigwerden von Fetten wie Butter freigesetzt wird.")
    };

    private static QuizQuestion OrganischeSaeuren(Random r)
    {
        var f = OrganischeSaeurenListe[r.Next(OrganischeSaeurenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Organische Säuren – Salatsauce, Entkalker & Co", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Carbonsäuren tragen die Carboxy-Gruppe (-COOH) und sind meist schwächere Säuren als anorganische Säuren; Aminosäuren tragen zusätzlich eine Amino-Gruppe und bilden Proteine."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EsterListe =
    {
        ("Was entsteht chemisch, wenn eine Carbonsäure mit einem Alkohol reagiert?", new[] { "Ein Ester und Wasser (Kondensationsreaktion)", "Ausschließlich ein neues Salz", "Ein reines Edelgas" }, "Ein Ester und Wasser (Kondensationsreaktion)",
            "Bei der Veresterung reagieren Carbonsäure und Alkohol unter Wasserabspaltung (Kondensation) zu einem Ester."),
        ("Was kennzeichnet die Estergruppe strukturell?", new[] { "Eine C(=O)-O-Bindung zwischen dem Säure- und dem Alkoholteil des Moleküls", "Eine reine Kohlenstoff-Wasserstoff-Bindung ohne Sauerstoff", "Eine Bindung, die ausschließlich aus Stickstoffatomen besteht" }, "Eine C(=O)-O-Bindung zwischen dem Säure- und dem Alkoholteil des Moleküls",
            "Die Estergruppe entsteht durch die Verbindung des Carbonylkohlenstoffs der Säure mit dem Sauerstoffatom des Alkohols."),
        ("Was ist Hydrolyse eines Esters?", new[] { "Die Rückreaktion, bei der ein Ester unter Wasseraufnahme wieder in Säure und Alkohol gespalten wird", "Die erstmalige Bildung eines Esters aus Säure und Alkohol", "Ein anderes Wort für eine Neutralisation" }, "Die Rückreaktion, bei der ein Ester unter Wasseraufnahme wieder in Säure und Alkohol gespalten wird",
            "Bei der Hydrolyse wird ein Ester unter Aufnahme von Wasser wieder in seine ursprüngliche Säure und den ursprünglichen Alkohol gespalten - die Umkehrung der Veresterung."),
        ("Warum gelten Veresterung und Hydrolyse als Beispiele für umkehrbare (reversible) Reaktionen?", new[] { "Je nach Bedingungen kann die Reaktion in beide Richtungen ablaufen (Ester bilden oder wieder spalten)", "Die Reaktion kann grundsätzlich nur in eine einzige Richtung ablaufen", "Umkehrbarkeit hat mit dieser Reaktion nichts zu tun" }, "Je nach Bedingungen kann die Reaktion in beide Richtungen ablaufen (Ester bilden oder wieder spalten)",
            "Veresterung (Kondensation) und Hydrolyse sind Hin- und Rückreaktion desselben Gleichgewichts - je nach Bedingungen (z.B. Wasserüberschuss) verschiebt sich das Gleichgewicht in die eine oder andere Richtung."),
        ("Was bewirkt eine saure Katalyse bei der Veresterung?", new[] { "Sie beschleunigt die Reaktion, ohne selbst verbraucht zu werden", "Sie verlangsamt die Reaktion drastisch", "Sie verändert das Endprodukt der Reaktion vollständig" }, "Sie beschleunigt die Reaktion, ohne selbst verbraucht zu werden",
            "Ein saurer Katalysator (z.B. Schwefelsäure) beschleunigt die Veresterungsreaktion, indem er den Reaktionsmechanismus erleichtert, wird dabei aber selbst nicht verbraucht."),
        ("Warum riechen viele Fruchtester (z.B. in Fruchtaromen) angenehm fruchtig?", new[] { "Bestimmte kleine Estermoleküle sind für charakteristische Fruchtdüfte verantwortlich", "Ester haben grundsätzlich überhaupt keinen Eigengeruch", "Fruchtgeruch hat mit der chemischen Struktur nichts zu tun" }, "Bestimmte kleine Estermoleküle sind für charakteristische Fruchtdüfte verantwortlich",
            "Viele natürliche und künstliche Fruchtaromen bestehen aus spezifischen Estern, die für den charakteristischen, oft fruchtigen Geruch verantwortlich sind."),
        ("Was sind Fette chemisch betrachtet größtenteils?", new[] { "Ester aus Glycerin und langkettigen Fettsäuren", "Reine Kohlenwasserstoffe ohne Sauerstoffanteil", "Anorganische Salze" }, "Ester aus Glycerin und langkettigen Fettsäuren",
            "Fette sind Ester, die aus dem dreiwertigen Alkohol Glycerin und drei langkettigen Fettsäuren aufgebaut sind (Triglyceride)."),
        ("Was passiert bei der Verseifung (alkalischen Hydrolyse) eines Fettes?", new[] { "Das Fett wird in Glycerin und die Salze der Fettsäuren (Seifen) gespalten", "Das Fett bleibt dabei chemisch völlig unverändert", "Es entsteht ausschließlich reines Wasser ohne weitere Produkte" }, "Das Fett wird in Glycerin und die Salze der Fettsäuren (Seifen) gespalten",
            "Bei der Verseifung wird ein Fett mit einer Lauge zu Glycerin und den Natrium- oder Kaliumsalzen der Fettsäuren (Seifen) gespalten."),
        ("Warum können Seifenmoleküle sowohl Fett als auch Wasser \"verbinden\" und dadurch reinigen?", new[] { "Sie besitzen einen wasserabweisenden (lipophilen) und einen wasseranziehenden (hydrophilen) Molekülteil", "Seifenmoleküle bestehen ausschließlich aus einem einzigen, komplett unpolaren Teil", "Seifenmoleküle haben mit Wasser oder Fett überhaupt keine Wechselwirkung" }, "Sie besitzen einen wasserabweisenden (lipophilen) und einen wasseranziehenden (hydrophilen) Molekülteil",
            "Der lipophile (fettliebende) Teil der Seife lagert sich an Fett an, der hydrophile (wasserliebende) Teil bleibt zum Wasser hin ausgerichtet - so können Fetttröpfchen im Wasser \"eingeschlossen\" und weggespült werden."),
        ("Was bedeutet \"lipophil\" bei einem Molekülteil?", new[] { "Fettliebend, gut in Fetten löslich", "Wasseranziehend", "Ein anderes Wort für Carbonsäure" }, "Fettliebend, gut in Fetten löslich",
            "Lipophile (\"fettliebende\") Molekülteile lösen sich gut in unpolaren, fettähnlichen Substanzen."),
        ("Was bedeutet \"lipophob\" bei einem Molekülteil?", new[] { "Fettabstoßend, schlecht in Fetten löslich", "Fettliebend", "Ein anderes Wort für Ester" }, "Fettabstoßend, schlecht in Fetten löslich",
            "Lipophobe (\"fettmeidende\") Molekülteile mischen sich schlecht mit fettähnlichen, unpolaren Substanzen."),
        ("Was ist ein typisches Beispiel für einen Ester, der als künstliches Aroma in Lebensmitteln verwendet wird?", new[] { "Ein Fruchtester wie Ethylbutanoat (nach Ananas riechend)", "Kochsalz", "Reines Ethanol ohne Säureanteil" }, "Ein Fruchtester wie Ethylbutanoat (nach Ananas riechend)",
            "Bestimmte kleine Ester wie Ethylbutanoat erinnern geruchlich an Früchte wie Ananas und werden deshalb als Aromastoffe eingesetzt."),
        ("Warum verschiebt ein Überschuss an Wasser das Gleichgewicht einer Veresterungsreaktion tendenziell in Richtung Hydrolyse?", new[] { "Nach dem Prinzip von Le Chatelier begünstigt mehr Wasser die Rückreaktion, bei der Wasser verbraucht wird", "Wasserüberschuss hat auf ein chemisches Gleichgewicht überhaupt keinen Einfluss", "Wasserüberschuss verschiebt jedes Gleichgewicht ausschließlich in Richtung Ester" }, "Nach dem Prinzip von Le Chatelier begünstigt mehr Wasser die Rückreaktion, bei der Wasser verbraucht wird",
            "Nach dem Prinzip des kleinsten Zwangs (Le Chatelier) verschiebt ein Überschuss an einem Reaktionspartner (hier Wasser) das Gleichgewicht in die Richtung, in der dieser Stoff verbraucht wird - also zur Hydrolyse."),
        ("Was unterscheidet ein Fett grundlegend von einem Öl bei Raumtemperatur?", new[] { "Fette sind bei Raumtemperatur meist fest, Öle meist flüssig", "Fette und Öle sind chemisch völlig identisch und unterscheiden sich in nichts", "Nur Öle bestehen aus Estern, Fette hingegen nicht" }, "Fette sind bei Raumtemperatur meist fest, Öle meist flüssig",
            "Der Unterschied zwischen Fetten und Ölen liegt meist im Aggregatzustand bei Raumtemperatur, der u.a. vom Sättigungsgrad der enthaltenen Fettsäuren abhängt."),
        ("Was passiert mit dem Schmelzpunkt eines Fettes, wenn es viele ungesättigte Fettsäuren (mit Doppelbindungen) enthält?", new[] { "Der Schmelzpunkt sinkt tendenziell, das Fett bleibt eher flüssig (Öl)", "Der Schmelzpunkt steigt immer stark an", "Ungesättigte Fettsäuren haben keinerlei Einfluss auf den Schmelzpunkt" }, "Der Schmelzpunkt sinkt tendenziell, das Fett bleibt eher flüssig (Öl)",
            "Doppelbindungen in ungesättigten Fettsäuren führen zu einer weniger dichten Packung der Moleküle, wodurch der Schmelzpunkt sinkt und das Fett bei Raumtemperatur eher flüssig bleibt."),
        ("Warum wird Seife seit Jahrhunderten zur Reinigung genutzt?", new[] { "Sie kann Fett und Schmutz im Wasser binden und abspülbar machen", "Seife hat historisch nie eine reinigende Wirkung gehabt", "Seife wirkt ausschließlich als Duftstoff ohne jede Reinigungsfunktion" }, "Sie kann Fett und Schmutz im Wasser binden und abspülbar machen",
            "Aufgrund ihres lipophilen und hydrophilen Molekülteils kann Seife Fett- und Schmutzpartikel umschließen und im Wasser abtransportierbar machen - eine Eigenschaft, die seit Jahrhunderten genutzt wird."),
        ("Was ist der Unterschied zwischen einer Kondensationsreaktion (Veresterung) und einer einfachen Additionsreaktion?", new[] { "Bei der Kondensation wird zusätzlich ein kleines Molekül (meist Wasser) abgespalten", "Bei einer Additionsreaktion wird immer Wasser freigesetzt", "Beide Reaktionstypen laufen exakt identisch ab" }, "Bei der Kondensation wird zusätzlich ein kleines Molekül (meist Wasser) abgespalten",
            "Kondensationsreaktionen wie die Veresterung erzeugen neben dem Hauptprodukt zusätzlich ein kleines Molekül (meist Wasser), das abgespalten wird - bei einer reinen Addition passiert das nicht."),
        ("Was zeigt die Vielfalt an Estern in der Natur und Industrie (Aromen, Fette, Kunststoffe)?", new[] { "Aus wenigen Grundbausteinen (Säuren und Alkoholen) lässt sich eine große Produktvielfalt herstellen", "Ester kommen ausschließlich in einer einzigen, immer identischen Form vor", "Ester haben in Natur und Industrie praktisch keine Bedeutung" }, "Aus wenigen Grundbausteinen (Säuren und Alkoholen) lässt sich eine große Produktvielfalt herstellen",
            "Durch Kombination unterschiedlicher Carbonsäuren und Alkohole entsteht eine riesige Vielfalt an Estern mit ganz unterschiedlichen Eigenschaften und Anwendungen."),
        ("Warum werden pflanzliche Öle in der Lebensmittelindustrie manchmal gehärtet (Fetthärtung)?", new[] { "Durch Anlagerung von Wasserstoff an Doppelbindungen werden ungesättigte Fettsäuren gesättigt und das Fett fester", "Härtung entfernt sämtliche Esterbindungen aus dem Öl", "Härtung hat auf den Aggregatzustand des Fetts keinerlei Einfluss" }, "Durch Anlagerung von Wasserstoff an Doppelbindungen werden ungesättigte Fettsäuren gesättigt und das Fett fester",
            "Bei der Fetthärtung wird Wasserstoff an die Doppelbindungen ungesättigter Fettsäuren addiert, wodurch das Fett gesättigter und bei Raumtemperatur fester wird."),
        ("Was ist ein Grund, warum manche Kunststoffe (Polyester) auf Esterbindungen aufgebaut sind?", new[] { "Viele einzelne Ester-Bausteine lassen sich zu langen, stabilen Kettenmolekülen verknüpfen", "Esterbindungen können grundsätzlich keine langen Ketten bilden", "Polyester enthalten keinerlei Esterbindung" }, "Viele einzelne Ester-Bausteine lassen sich zu langen, stabilen Kettenmolekülen verknüpfen",
            "Polyester entstehen durch die Verknüpfung vieler Säure- und Alkoholbausteine über Esterbindungen zu langen, stabilen Polymerketten, die z.B. für Textilfasern genutzt werden.")
    };

    private static QuizQuestion Ester(Random r)
    {
        var f = EsterListe[r.Next(EsterListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Ester – Vielfalt der Produkte aus Alkoholen und Säuren", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ester entstehen aus Säure + Alkohol unter Wasserabspaltung (Veresterung), Hydrolyse ist die Rückreaktion; Fette sind Ester aus Glycerin und Fettsäuren, Verseifung ergibt Seife."
        };
    }
}
