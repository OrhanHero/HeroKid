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
            [GradeLevel.Klasse6] = new List<TopicFactory> { StoffeTrennen, Verbrennung, SaeurenLaugen, MetalleEigenschaften, StoffeImAlltag, PeriodensystemGrundlagen, Gase, Wasser, Salze },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Atommodell, ChemischeReaktion, Periodensystem, Stoechiometrie, SaeureBaseVertieft, Kohlenwasserstoffe, Alkohole, OrganischeSaeuren, Ester }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TrennListe =
    {
        ("Wie trennt man Sand von Wasser am einfachsten?", new[] { "Filtration (durch einen Filter gießen)", "Verdampfen", "Magnet verwenden (was so in der Praxis nicht zutrifft)" },
            "Filtration (durch einen Filter gießen)", "Beim Filtrieren bleibt der unlösliche Sand im Filter zurück, das Wasser läuft durch."),
        ("Wie trennt man Salz von Wasser (Salzwasser)?", new[] { "Verdampfen/Eindampfen des Wassers", "Filtration", "Sieben" },
            "Verdampfen/Eindampfen des Wassers", "Beim Verdampfen bleibt das gelöste Salz zurück, während das Wasser als Dampf entweicht."),
        ("Wie trennt man Eisenspäne von Sand?", new[] { "Mit einem Magneten", "Filtration", "Verdampfen" },
            "Mit einem Magneten", "Eisen ist magnetisch und lässt sich so vom nichtmagnetischen Sand trennen."),
        ("Wie trennt man Öl von Wasser?", new[] { "Man lässt es sich trennen und schöpft das Öl ab (Dekantieren)", "Filtration", "Magnet verwenden - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt" },
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
        ("Wie können z.B. Blutbestandteile mit unterschiedlicher Dichte im Labor getrennt werden?", new[] { "Durch Zentrifugieren (schnelles Rotieren)", "Durch einfaches Stehenlassen über Wochen, obwohl das auf den ersten Blick plausibel klingt", "Durch Erhitzen auf 100°C" },
            "Durch Zentrifugieren (schnelles Rotieren)", "Beim Zentrifugieren trennt die schnelle Rotation Bestandteile unterschiedlicher Dichte voneinander."),
        ("Wie nennt man es, wenn ein Stoff mithilfe eines Lösungsmittels aus einem Gemisch herausgelöst wird (z.B. Kaffee aus Kaffeepulver)?", new[] { "Extraktion", "Sedimentation", "Sieben" },
            "Extraktion", "Bei der Extraktion löst ein Lösungsmittel (z.B. heißes Wasser) gezielt einen bestimmten Stoff aus einem Gemisch heraus."),
        ("Wie kann Aktivkohle bei der Wasserreinigung helfen?", new[] { "Sie bindet bestimmte Schadstoffe an ihrer großen Oberfläche (Adsorption)", "Sie färbt das Wasser blau", "Sie erhöht die Temperatur des Wassers, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Sie bindet bestimmte Schadstoffe an ihrer großen Oberfläche (Adsorption)", "Aktivkohle hat eine sehr große, poröse Oberfläche, an der sich viele Schadstoffe anlagern (adsorbieren) können."),
        ("Warum eignet sich einfache Filtration nicht, um gelöstes Salz aus Wasser zu trennen?", new[] { "Weil gelöste Teilchen so klein sind, dass sie durch den Filter hindurchgehen", "Weil Salz magnetisch ist", "Weil Wasser den Filter zerstört und deshalb hier nicht zutrifft, was so nicht korrekt ist" },
            "Weil gelöste Teilchen so klein sind, dass sie durch den Filter hindurchgehen", "Gelöste Salzteilchen sind viel kleiner als die Poren eines Filters und passieren ihn ungehindert - hier hilft nur Verdampfen."),
        ("Warum kann man Alkohol und Wasser durch Destillation voneinander trennen?", new[] { "Weil Alkohol einen niedrigeren Siedepunkt hat und zuerst verdampft", "Weil Alkohol schwerer ist als Wasser", "Weil Alkohol magnetisch ist" },
            "Weil Alkohol einen niedrigeren Siedepunkt hat und zuerst verdampft", "Alkohol siedet bei einer niedrigeren Temperatur als Wasser und verdampft deshalb bei der Destillation zuerst."),
        ("Was passiert mit dem Wasser beim Eindampfen einer Salzlösung?", new[] { "Es verdampft, das Salz bleibt als Feststoff zurück", "Es wird zu Eis", "Es verwandelt sich chemisch in Salz - eine haeufige, aber unzutreffende Vorstellung" },
            "Es verdampft, das Salz bleibt als Feststoff zurück", "Beim Erhitzen verdunstet das Wasser, während das gelöste Salz als fester Rückstand zurückbleibt."),
        ("Wie kann man Kupferspäne und Eisenspäne aus einem Gemisch trennen?", new[] { "Mit einem Magneten, da nur Eisen magnetisch ist", "Durch Filtration", "Durch Destillation, auch wenn das manche zunaechst vermuten wuerden" },
            "Mit einem Magneten, da nur Eisen magnetisch ist", "Eisen wird von einem Magneten angezogen, Kupfer nicht - so lassen sich beide Metalle trennen."),
        ("Wie wird Rohöl in seine verschiedenen Bestandteile wie Benzin und Diesel getrennt?", new[] { "Durch fraktionierte Destillation nach unterschiedlichen Siedepunkten", "Durch Filtration", "Durch Sieben" },
            "Durch fraktionierte Destillation nach unterschiedlichen Siedepunkten", "Die verschiedenen Bestandteile des Rohöls haben unterschiedliche Siedepunkte und werden bei der fraktionierten Destillation stufenweise getrennt."),
        ("Was ist der Unterschied zwischen einem homogenen und einem heterogenen Gemisch?", new[] { "Homogen ist gleichmäßig vermischt, bei heterogen sind Bestandteile sichtbar getrennt", "Beide Begriffe bedeuten dasselbe", "Homogen bedeutet, dass gar keine Mischung vorliegt" },
            "Homogen ist gleichmäßig vermischt, bei heterogen sind Bestandteile sichtbar getrennt", "Salzwasser ist z.B. ein homogenes Gemisch (gleichmäßig gelöst), Sand in Wasser ein heterogenes Gemisch (sichtbar getrennte Bestandteile)."),
        ("Wie trennt man unlösliches Kreidepulver von Wasser?", new[] { "Filtration", "Verdampfen", "Magnet verwenden" },
            "Filtration", "Da sich Kreidepulver nicht im Wasser löst, bleibt es beim Filtrieren im Filter zurück."),
        ("Was ist ein Beispiel für ein heterogenes Gemisch, bei dem man die Bestandteile mit bloßem Auge erkennt?", new[] { "Sand in Wasser", "Salzwasser", "Zuckerwasser, was bei genauerem Hinsehen nicht stimmt" },
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
        ("Womit kann man ein kleines Feuer am besten ersticken?", new[] { "Mit einer Decke oder einem Deckel (Sauerstoff abschneiden)", "Mit noch mehr Luft anfachen (was so in der Praxis nicht zutrifft)", "Mit Benzin übergießen" },
            "Mit einer Decke oder einem Deckel (Sauerstoff abschneiden)", "Wird dem Feuer der Sauerstoff entzogen (z.B. durch eine Löschdecke), kann die Verbrennung nicht weitergehen."),
        ("Welches Gas entsteht bei der vollständigen Verbrennung von Holz oder Kohle hauptsächlich?", new[] { "Kohlenstoffdioxid (CO₂)", "Sauerstoff (O₂)", "Wasserstoff (H₂) - eine verbreitete, aber falsche Annahme" },
            "Kohlenstoffdioxid (CO₂)", "Der im Brennstoff enthaltene Kohlenstoff verbindet sich bei der Verbrennung mit Sauerstoff zu Kohlenstoffdioxid."),
        ("Was ist die \"Zündtemperatur\" eines Stoffes?", new[] { "Die Mindesttemperatur, die ein Stoff braucht, damit er zu brennen beginnt", "Die Temperatur, bei der ein Stoff schmilzt, was einer genaueren Pruefung nicht standhaelt", "Die Temperatur, bei der ein Stoff gefriert" },
            "Die Mindesttemperatur, die ein Stoff braucht, damit er zu brennen beginnt", "Erst ab der Zündtemperatur reagiert ein Brennstoff schnell genug mit Sauerstoff, um eine Verbrennung zu starten."),
        ("Warum ist reiner Sauerstoff besonders gefährlich in Bezug auf Feuer?", new[] { "Er lässt Stoffe viel heftiger und schneller brennen", "Er verhindert jede Verbrennung", "Er kühlt brennende Stoffe sofort ab, obwohl das auf den ersten Blick plausibel klingt" },
            "Er lässt Stoffe viel heftiger und schneller brennen", "Mit reinem Sauerstoff statt normaler Luft verläuft eine Verbrennung deutlich intensiver und schneller."),
        ("Was entsteht bei einer unvollständigen Verbrennung (zu wenig Sauerstoff)?", new[] { "Giftiges Kohlenstoffmonoxid (CO) statt CO₂", "Ausschließlich reiner Sauerstoff, was die eigentliche Bedeutung des Begriffs verfehlt", "Ausschließlich Wasser" },
            "Giftiges Kohlenstoffmonoxid (CO) statt CO₂", "Bei Sauerstoffmangel entsteht statt des ungiftigen CO₂ das giftige Kohlenstoffmonoxid (CO)."),
        ("Warum ist Kohlenstoffmonoxid (CO) für Menschen besonders gefährlich?", new[] { "Es ist geruchlos, farblos und blockiert den Sauerstofftransport im Blut", "Es riecht sehr stark nach Rauch und ist leicht zu bemerken", "Es ist völlig ungiftig" },
            "Es ist geruchlos, farblos und blockiert den Sauerstofftransport im Blut", "CO bindet sich stärker an den roten Blutfarbstoff als Sauerstoff und wird dabei weder gerochen noch gesehen - das macht es besonders tückisch."),
        ("Wie wirkt Wasser als Löschmittel bei einem normalen Holzbrand?", new[] { "Es kühlt den Brennstoff unter die Zündtemperatur", "Es entzieht dem Feuer nur die Farbe", "Es macht das Holz feuerfest für immer" },
            "Es kühlt den Brennstoff unter die Zündtemperatur", "Wasser entzieht dem brennenden Material Wärme und senkt die Temperatur unter die Zündtemperatur."),
        ("Warum darf man einen Fettbrand in der Küche niemals mit Wasser löschen?", new[] { "Das Wasser verdampft explosionsartig und reißt brennendes Fett in die Luft", "Wasser macht das Feuer sofort komplett ungefährlich und deshalb hier nicht zutrifft", "Wasser hat auf Fettbrände überhaupt keine Wirkung" },
            "Das Wasser verdampft explosionsartig und reißt brennendes Fett in die Luft", "Trifft Wasser auf heißes, brennendes Fett, verdampft es schlagartig und schleudert brennendes Fett explosionsartig umher (Fettexplosion)."),
        ("Was unterscheidet Glut von einer offenen Flamme?", new[] { "Glut glimmt ohne sichtbare Flamme, oft bei niedrigerer Temperatur", "Glut ist immer heißer als jede Flamme", "Es gibt keinen Unterschied"}, "Glut glimmt ohne sichtbare Flamme, oft bei niedrigerer Temperatur",
            "Glut entsteht z.B. bei glimmender Kohle - es brennt ohne sichtbare Flamme, oft langsamer und bei niedrigerer Temperatur."),
        ("Wie heißt der Vorgang, bei dem Metall langsam mit Sauerstoff reagiert, z.B. beim Rosten?", new[] { "Eine langsame Oxidation", "Eine Reduktion", "Eine Neutralisation, was so nicht korrekt ist" },
            "Eine langsame Oxidation", "Rosten ist eine langsam ablaufende Oxidation von Eisen mit Sauerstoff und Feuchtigkeit."),
        ("Was ist notwendig, damit ein Streichholz beim Reiben zu brennen beginnt?", new[] { "Reibungswärme, die die Zündtemperatur erreicht", "Ein starker Magnet", "Kaltes Wasser" },
            "Reibungswärme, die die Zündtemperatur erreicht", "Die Reibung beim Anzünden erzeugt genug Wärme, um die Zündtemperatur der Streichholzspitze zu erreichen."),
        ("Was zeigt die Flammenfarbe bei einer Verbrennung oft an?", new[] { "Die Temperatur bzw. teils die beteiligten chemischen Elemente", "Ausschließlich die Windrichtung", "Ausschließlich die Uhrzeit" },
            "Die Temperatur bzw. teils die beteiligten chemischen Elemente", "Flammenfarben können Hinweise auf Temperatur und auf bestimmte enthaltene Elemente geben (z.B. bei Feuerwerk)."),
        ("Warum verbrennt trockenes Holz besser als feuchtes Holz?", new[] { "Feuchtigkeit muss erst verdampfen, bevor das Holz die Zündtemperatur erreichen kann", "Feuchtes Holz enthält mehr Sauerstoff", "Trockenes Holz enthält kein Kohlenstoff - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden" },
            "Feuchtigkeit muss erst verdampfen, bevor das Holz die Zündtemperatur erreichen kann", "Bei feuchtem Holz wird zunächst Energie zum Verdampfen des Wassers verbraucht, bevor das Holz überhaupt heiß genug zum Brennen wird."),
        ("Was ist eine Explosion, chemisch betrachtet, vereinfacht?", new[] { "Eine extrem schnelle Verbrennung mit plötzlicher Freisetzung von Gas und Energie", "Ein langsamer, kontrollierter Verbrennungsvorgang", "Ein rein physikalischer Vorgang ohne chemische Reaktion" },
            "Eine extrem schnelle Verbrennung mit plötzlicher Freisetzung von Gas und Energie", "Bei einer Explosion läuft eine Verbrennungsreaktion extrem schnell ab und setzt schlagartig große Mengen Gas und Energie frei."),
        ("Warum ist feiner Mehlstaub in einer Mühle explosionsgefährlich?", new[] { "Feine Staubpartikel haben eine sehr große Oberfläche und können explosionsartig verbrennen", "Mehl enthält gar keinen brennbaren Kohlenstoff", "Mehlstaub kann unter keinen Umständen brennen" },
            "Feine Staubpartikel haben eine sehr große Oberfläche und können explosionsartig verbrennen", "Fein verteilter, brennbarer Staub in der Luft hat eine riesige Gesamtoberfläche und kann bei einem Funken explosionsartig reagieren."),
        ("Wozu dient die Einteilung in \"Brandklassen\" bei Feuerlöschern?", new[] { "Damit für die jeweilige Brennstoffart das passende Löschmittel gewählt wird", "Um die Feuerwehr über die Farbe des Feuers zu informieren, was bei genauerem Hinsehen nicht stimmt", "Um die Kosten eines Feuerlöschers festzulegen" },
            "Damit für die jeweilige Brennstoffart das passende Löschmittel gewählt wird", "Verschiedene Brandklassen (z.B. feste Stoffe, Flüssigkeiten, Fette) benötigen unterschiedliche Löschmittel, um sicher und wirksam gelöscht zu werden."),
        ("Was versteht man unter dem \"Verbrennungsdreieck\"?", new[] { "Die drei notwendigen Voraussetzungen Brennstoff, Sauerstoff und Zündtemperatur", "Eine geometrische Form, die Flammen typischerweise annehmen (was so in der Praxis nicht zutrifft)", "Ein Werkzeug der Feuerwehr zum Löschen" },
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
        ("Was zeigt der pH-Wert an?", new[] { "Ob eine Lösung sauer, neutral oder basisch (alkalisch) ist", "Die Temperatur einer Lösung - eine verbreitete, aber falsche Annahme", "Wie viel Salz gelöst ist" },
            "Ob eine Lösung sauer, neutral oder basisch (alkalisch) ist",
            "Der pH-Wert reicht von 0 (stark sauer) über 7 (neutral) bis 14 (stark basisch/alkalisch)."),
        ("Was passiert, wenn man eine Säure und eine Lauge in passender Menge zusammengibt?", new[] { "Sie neutralisieren sich gegenseitig", "Es entsteht sofort Feuer, was einer genaueren Pruefung nicht standhaelt", "Nichts passiert" },
            "Sie neutralisieren sich gegenseitig", "Bei der Neutralisation reagieren Säure und Lauge zu einem (meist ungefähr neutralen) Salz und Wasser."),
        ("Welchen ungefähren pH-Wert hat reines Wasser?", new[] { "7 (neutral)", "0 (stark sauer)", "14 (stark basisch)" },
            "7 (neutral)", "Reines Wasser gilt mit einem pH-Wert von etwa 7 als neutral - weder sauer noch basisch."),
        ("Warum sollte man mit konzentrierten Säuren und Laugen im Chemieunterricht vorsichtig umgehen?", new[] { "Sie können Haut, Augen und Kleidung stark schädigen (ätzend)", "Sie sind komplett ungefährlich, obwohl das auf den ersten Blick plausibel klingt", "Sie färben nur die Hände bunt" },
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
        ("Was ist eine Lauge chemisch grob gesehen, im Gegensatz zu einer Säure?", new[] { "Eine basische Lösung mit einem pH-Wert über 7", "Eine Lösung mit einem pH-Wert unter 7, was die eigentliche Bedeutung des Begriffs verfehlt", "Eine Lösung ohne jeden pH-Wert" }, "Eine basische Lösung mit einem pH-Wert über 7",
            "Laugen sind basische (alkalische) Lösungen mit einem pH-Wert über 7 - das Gegenteil von sauren Lösungen."),
        ("Was entsteht häufig, wenn eine Säure mit einem unedlen Metall wie Zink reagiert?", new[] { "Wasserstoffgas und ein Salz", "Sauerstoffgas und Wasser und deshalb hier nicht zutrifft", "Nur reines Wasser" }, "Wasserstoffgas und ein Salz",
            "Reagiert eine Säure mit einem unedlen Metall, entstehen typischerweise Wasserstoffgas und das entsprechende Salz."),
        ("Was passiert mit blauem Lackmuspapier, wenn man es in eine Säure taucht?", new[] { "Es färbt sich rot", "Es bleibt blau", "Es wird durchsichtig" }, "Es färbt sich rot",
            "Lackmuspapier ist ein klassischer Säure-Base-Indikator: Säuren färben blaues Lackmuspapier rot."),
        ("Was passiert mit rotem Lackmuspapier, wenn man es in eine Lauge taucht?", new[] { "Es färbt sich blau", "Es bleibt rot", "Es wird schwarz, was so nicht korrekt ist" }, "Es färbt sich blau",
            "Laugen färben rotes Lackmuspapier blau - das umgekehrte Verhalten zu Säuren."),
        ("Wie heißt ein bekanntes Reinigungsmittel, das stark basisch (laugenhaft) ist?", new[] { "Rohrreiniger (Abflussreiniger)", "Essigreiniger", "Zitronensäurereiniger" }, "Rohrreiniger (Abflussreiniger)",
            "Viele Abflussreiniger enthalten stark alkalische (basische) Substanzen, die Fett und organische Verstopfungen auflösen."),
        ("Warum sollte man Säuren und Laugen im Labor niemals ohne Schutzbrille verwenden?", new[] { "Sie können bei Spritzern die Augen schwer verletzen", "Schutzbrillen sind nur ein modisches Accessoire", "Säuren und Laugen sind völlig ungefährlich für die Augen" }, "Sie können bei Spritzern die Augen schwer verletzen",
            "Spritzer von Säuren oder Laugen können schwere, teils bleibende Augenschäden verursachen - eine Schutzbrille ist deshalb Pflicht."),
        ("Was passiert, wenn man mit einer verdünnten statt einer konzentrierten Säure arbeitet?", new[] { "Die Reaktion/Ätzwirkung ist deutlich schwächer und ungefährlicher", "Die Ätzwirkung wird automatisch stärker - eine haeufige, aber unzutreffende Vorstellung", "Es ändert sich überhaupt nichts" }, "Die Reaktion/Ätzwirkung ist deutlich schwächer und ungefährlicher",
            "Verdünnte Säuren enthalten weniger Säureteilchen pro Volumen und reagieren daher schwächer und weniger gefährlich als konzentrierte."),
        ("Welche Zahl auf der pH-Skala steht für die stärkste Säure?", new[] { "0", "7", "14" }, "0",
            "Die pH-Skala reicht von 0 (stark sauer) bis 14 (stark basisch) - je näher an 0, desto saurer."),
        ("Welche Zahl auf der pH-Skala steht für die stärkste Lauge?", new[] { "14", "0, auch wenn das manche zunaechst vermuten wuerden", "7" }, "14",
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
        ("Wie nennt man die Eigenschaft von Metallen, sich zu dünnen Blechen verformen zu lassen?", new[] { "Verformbarkeit (Duktilität)", "Löslichkeit", "Brennbarkeit, was bei genauerem Hinsehen nicht stimmt" },
            "Verformbarkeit (Duktilität)", "Metalle lassen sich hämmern, walzen oder zu Draht ziehen, ohne zu zerbrechen - das nennt man Verformbarkeit."),
        ("Welches Metall wird häufig für Stromkabel verwendet, weil es Strom besonders gut leitet?", new[] { "Kupfer", "Gold (was so in der Praxis nicht zutrifft)", "Blei" },
            "Kupfer", "Kupfer leitet elektrischen Strom sehr gut und ist zugleich günstiger als Gold oder Silber, daher wird es für Kabel verwendet."),
        ("Was passiert, wenn Eisen über längere Zeit Feuchtigkeit und Sauerstoff ausgesetzt ist?", new[] { "Es rostet (Korrosion)", "Es wird magnetisch", "Es schmilzt bei Zimmertemperatur" },
            "Es rostet (Korrosion)", "Rost entsteht durch eine chemische Reaktion von Eisen mit Sauerstoff und Wasser (Korrosion)."),
        ("Warum werden Besteck und Kochtöpfe oft aus Metall hergestellt?", new[] { "Metalle leiten Wärme gut und sind stabil/langlebig", "Weil Metall am leichtesten von allen Materialien ist", "Weil Metall immer durchsichtig ist" },
            "Metalle leiten Wärme gut und sind stabil/langlebig", "Die gute Wärmeleitfähigkeit und Stabilität von Metallen macht sie ideal für Kochgeschirr und Besteck."),
        ("Was ist eine \"Legierung\"?", new[] { "Eine Mischung aus zwei oder mehr Metallen (oder Metall und Nichtmetall)", "Ein reines, unvermischtes Metall", "Ein anderes Wort für Erz" }, "Eine Mischung aus zwei oder mehr Metallen (oder Metall und Nichtmetall)",
            "Legierungen entstehen durch das gezielte Zusammenschmelzen von Metallen, um neue, oft verbesserte Eigenschaften zu erhalten."),
        ("Was ist ein bekanntes Beispiel für eine Legierung?", new[] { "Bronze (Kupfer und Zinn)", "Reines Gold", "Reiner Sauerstoff - eine verbreitete, aber falsche Annahme" }, "Bronze (Kupfer und Zinn)",
            "Bronze ist eine der ältesten bekannten Legierungen und besteht hauptsächlich aus Kupfer und Zinn."),
        ("Warum wird Aluminium oft für Flugzeuge oder Fahrräder verwendet?", new[] { "Es ist leicht und trotzdem stabil", "Es ist das schwerste bekannte Metall", "Es leitet keinen Strom" }, "Es ist leicht und trotzdem stabil",
            "Aluminium hat ein geringes Gewicht bei gleichzeitig guter Stabilität - ideal für Fahrzeuge, bei denen Gewicht wichtig ist."),
        ("Welches Edelmetall wird oft für Schmuck verwendet, weil es nicht rostet?", new[] { "Gold", "Eisen", "Zink" }, "Gold",
            "Gold ist sehr widerstandsfähig gegen Korrosion und wird deshalb seit Jahrtausenden für Schmuck verwendet."),
        ("Was passiert mit den meisten Metallen, wenn man sie stark erhitzt?", new[] { "Sie schmelzen und werden flüssig", "Sie verschwinden spurlos", "Sie werden automatisch magnetisch" }, "Sie schmelzen und werden flüssig",
            "Bei ausreichend hoher Temperatur (dem Schmelzpunkt) gehen Metalle vom festen in den flüssigen Zustand über."),
        ("Warum kann man Metalle im Gegensatz zu vielen anderen Materialien gut recyceln?", new[] { "Sie lassen sich einschmelzen und zu neuen Produkten formen, ohne ihre Eigenschaften zu verlieren", "Metalle können nach Gebrauch nicht wiederverwendet werden, was einer genaueren Pruefung nicht standhaelt", "Metalle lösen sich beim Recycling einfach in Luft auf" }, "Sie lassen sich einschmelzen und zu neuen Produkten formen, ohne ihre Eigenschaften zu verlieren",
            "Metalle können praktisch beliebig oft eingeschmolzen und neu verarbeitet werden, ohne ihre grundlegenden Eigenschaften zu verlieren."),
        ("Was schützt viele Metallgegenstände wie Autoteile vor Rost?", new[] { "Eine Schutzschicht wie Lack oder Verzinkung", "Ständiges Wässern der Oberfläche, obwohl das auf den ersten Blick plausibel klingt", "Direktes Sonnenlicht" }, "Eine Schutzschicht wie Lack oder Verzinkung",
            "Beschichtungen wie Lack oder eine Zinkschicht schützen das darunterliegende Metall vor Feuchtigkeit und Sauerstoff."),
        ("Warum wird Eisen häufig verzinkt (mit einer Zinkschicht überzogen)?", new[] { "Zink schützt das darunterliegende Eisen vor Rost", "Zink macht Eisen magnetisch, was die eigentliche Bedeutung des Begriffs verfehlt", "Zink färbt Eisen bunt" }, "Zink schützt das darunterliegende Eisen vor Rost",
            "Die Zinkschicht wirkt wie eine Schutzhülle und verhindert, dass Feuchtigkeit und Sauerstoff das Eisen darunter angreifen."),
        ("Welches Metall ist bei Zimmertemperatur flüssig?", new[] { "Quecksilber", "Eisen und deshalb hier nicht zutrifft", "Gold" }, "Quecksilber",
            "Quecksilber ist das einzige Metall, das bei normaler Zimmertemperatur bereits flüssig ist."),
        ("Was bedeutet der typische metallische Glanz vieler Metalle?", new[] { "Sie reflektieren Licht an ihrer glatten Oberfläche gut", "Sie absorbieren jegliches Licht vollständig, was so nicht korrekt ist", "Sie sind grundsätzlich durchsichtig" }, "Sie reflektieren Licht an ihrer glatten Oberfläche gut",
            "Der metallische Glanz entsteht, weil frei bewegliche Elektronen an der Oberfläche Licht gut reflektieren."),
        ("Warum sind Metalle im Allgemeinen gute Wärmeleiter?", new[] { "Frei bewegliche Elektronen transportieren Energie schnell durch das Metall", "Metalle enthalten grundsätzlich keine Elektronen", "Wärme kann Metalle gar nicht durchdringen" }, "Frei bewegliche Elektronen transportieren Energie schnell durch das Metall",
            "Die frei beweglichen Elektronen in Metallen leiten Wärmeenergie sehr effizient weiter."),
        ("Was ist Stahl?", new[] { "Eine Legierung aus Eisen und Kohlenstoff", "Ein reines chemisches Element", "Eine Legierung aus Gold und Silber - eine haeufige, aber unzutreffende Vorstellung" }, "Eine Legierung aus Eisen und Kohlenstoff",
            "Stahl entsteht durch das gezielte Legieren von Eisen mit einem geringen Anteil Kohlenstoff, was ihn härter und widerstandsfähiger macht."),
        ("Warum ist Edelstahl (rostfreier Stahl) bei Besteck so beliebt?", new[] { "Er rostet praktisch nicht und ist langlebig", "Er ist das leichteste bekannte Material", "Er verändert ständig seine Farbe" }, "Er rostet praktisch nicht und ist langlebig",
            "Edelstahl enthält Legierungselemente wie Chrom, die eine schützende Oxidschicht bilden und Rost praktisch verhindern."),
        ("Was passiert chemisch beim Rosten von Eisen?", new[] { "Eisen reagiert mit Sauerstoff und Feuchtigkeit zu Eisenoxid", "Eisen verwandelt sich in Gold", "Eisen verliert dabei seine gesamte Masse" }, "Eisen reagiert mit Sauerstoff und Feuchtigkeit zu Eisenoxid",
            "Rost ist chemisch gesehen Eisenoxid, das durch die Reaktion von Eisen mit Sauerstoff und Wasser entsteht."),
        ("Warum sind viele Metalle in der Erdkruste als Erze und nicht als reines Metall zu finden?", new[] { "Sie reagieren leicht mit anderen Elementen wie Sauerstoff zu chemischen Verbindungen", "Reine Metalle kommen in der Natur häufiger vor als Erze, auch wenn das manche zunaechst vermuten wuerden", "Metalle existieren in der Erdkruste überhaupt nicht" }, "Sie reagieren leicht mit anderen Elementen wie Sauerstoff zu chemischen Verbindungen",
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
        ("Zu welcher Stoffgruppe gehört Kohle?", new[] { "Brennstoffe", "Nährstoffe, was bei genauerem Hinsehen nicht stimmt", "Metalle" }, "Brennstoffe",
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
        ("Woran erkennt man, ob ein Metall magnetisch ist?", new[] { "Es wird von einem Magneten angezogen", "Es leitet keinen Strom", "Es ist immer aus Kunststoff (was so in der Praxis nicht zutrifft)" }, "Es wird von einem Magneten angezogen",
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
        ("Was ist ein typisches Beispiel für einen Kunststoff im Alltag?", new[] { "Eine Plastiktüte", "Ein Goldring", "Ein Holzbrett - eine verbreitete, aber falsche Annahme" }, "Eine Plastiktüte",
            "Plastiktüten bestehen aus Kunststoff, einem künstlich hergestellten Material."),
        ("Warum sind Metalle wie Kupfer für elektrische Leitungen gut geeignet?", new[] { "Sie leiten elektrischen Strom gut", "Sie leiten keinen Strom", "Sie schmelzen bei Zimmertemperatur" }, "Sie leiten elektrischen Strom gut",
            "Kupfer leitet elektrischen Strom sehr gut und wird deshalb häufig für Kabel verwendet."),
        ("Was passiert mit Eis, wenn die Temperatur über 0°C steigt?", new[] { "Es schmilzt zu Wasser", "Es wird härter", "Es verdampft sofort, was einer genaueren Pruefung nicht standhaelt" }, "Es schmilzt zu Wasser",
            "Bei Temperaturen über 0°C schmilzt Eis und wird zu flüssigem Wasser."),
        ("Welche Stoffeigenschaft prüft man, wenn man Öl und Wasser mischt?", new[] { "Ob sich die Stoffe miteinander mischen (Löslichkeit)", "Die Brennbarkeit", "Den Magnetismus" }, "Ob sich die Stoffe miteinander mischen (Löslichkeit)",
            "Öl und Wasser mischen sich nicht - das zeigt, dass Öl in Wasser unlöslich ist."),
        ("Warum sollten Gefahrstoffe im Haushalt gekennzeichnet und sicher aufbewahrt werden?", new[] { "Um Unfälle und Vergiftungen zu vermeiden", "Weil es keinen Grund dafür gibt, obwohl das auf den ersten Blick plausibel klingt", "Nur aus optischen Gründen" }, "Um Unfälle und Vergiftungen zu vermeiden",
            "Kennzeichnung und sichere Aufbewahrung von Gefahrstoffen schützen vor Unfällen und Vergiftungen."),
        ("Was ist ein Beispiel für einen Nährstoff, den unser Körper braucht?", new[] { "Eiweiß (Protein)", "Plastik, was die eigentliche Bedeutung des Begriffs verfehlt", "Metall" }, "Eiweiß (Protein)",
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
        ("Wo befinden sich die Elektronen im Bohrschen Atommodell?", new[] { "In Schalen um den Atomkern", "Im Atomkern", "Es gibt keine Elektronen und deshalb hier nicht zutrifft" },
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
        ("Wo befindet sich fast die gesamte Masse eines Atoms?", new[] { "Im Atomkern (Protonen und Neutronen)", "In der Elektronenhülle", "Verteilt im leeren Raum um das Atom, was so nicht korrekt ist" }, "Im Atomkern (Protonen und Neutronen)",
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
        ("Was ist ein Molekül im Unterschied zu einem einzelnen Atom?", new[] { "Eine Verbindung aus zwei oder mehr miteinander verbundenen Atomen", "Ein Atom ohne Elektronen - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden", "Ein anderes Wort für Ion" }, "Eine Verbindung aus zwei oder mehr miteinander verbundenen Atomen",
            "Moleküle entstehen, wenn sich zwei oder mehr Atome chemisch miteinander verbinden, z.B. zwei Wasserstoffatome zu H₂."),
        ("Was zeigt die chemische Formel H₂O?", new[] { "Ein Wassermolekül aus zwei Wasserstoff- und einem Sauerstoffatom", "Ein reines Wasserstoffmolekül, was bei genauerem Hinsehen nicht stimmt", "Ein reines Sauerstoffmolekül" }, "Ein Wassermolekül aus zwei Wasserstoff- und einem Sauerstoffatom",
            "Die Formel H₂O zeigt, dass ein Wassermolekül aus zwei Wasserstoffatomen und einem Sauerstoffatom besteht."),
        ("Was versteht man unter der \"Atomhülle\"?", new[] { "Den Bereich um den Kern, in dem sich die Elektronen aufhalten", "Einen anderen Namen für den Atomkern selbst", "Eine feste, sichtbare Schutzschicht aus Metall (was so in der Praxis nicht zutrifft)" }, "Den Bereich um den Kern, in dem sich die Elektronen aufhalten",
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
        ("Was entsteht bei der vollständigen Verbrennung von Kohlenstoff mit Sauerstoff?", new[] { "Kohlenstoffdioxid (CO₂)", "Wasser (H₂O) - eine verbreitete, aber falsche Annahme", "Methan (CH₄)" }, "Kohlenstoffdioxid (CO₂)",
            "C + O₂ → CO₂ – Kohlenstoff reagiert mit Sauerstoff zu Kohlenstoffdioxid."),
        ("Was bleibt bei einer chemischen Reaktion nach dem Massenerhaltungssatz gleich?", new[] { "Die Gesamtmasse aller Stoffe", "Die Farbe der Stoffe, was einer genaueren Pruefung nicht standhaelt", "Der Aggregatzustand" }, "Die Gesamtmasse aller Stoffe",
            "Nach dem Massenerhaltungssatz bleibt die Gesamtmasse vor und nach einer chemischen Reaktion gleich."),
        ("Wie nennt man eine Reaktion, bei der Energie aus der Umgebung aufgenommen wird?", new[] { "Endotherm", "Exotherm", "Neutral" }, "Endotherm",
            "Bei endothermen Reaktionen wird Energie (z.B. Wärme) aus der Umgebung aufgenommen, z.B. beim Backen."),
        ("Was zeigt eine chemische Reaktionsgleichung wie \"2 H₂ + O₂ → 2 H₂O\"?", new[] { "Wie viele Teilchen der Ausgangsstoffe zu welchem Produkt reagieren", "Nur die Farbe der beteiligten Stoffe, obwohl das auf den ersten Blick plausibel klingt", "Wie schnell die Reaktion abläuft" }, "Wie viele Teilchen der Ausgangsstoffe zu welchem Produkt reagieren",
            "Die Zahlen (Koeffizienten) vor den Formeln zeigen das Mengenverhältnis, in dem die Stoffe reagieren."),
        ("Was ist ein Katalysator?", new[] { "Ein Stoff, der eine Reaktion beschleunigt, ohne selbst verbraucht zu werden", "Ein Stoff, der jede Reaktion vollständig stoppt, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein anderes Wort für Lösungsmittel" }, "Ein Stoff, der eine Reaktion beschleunigt, ohne selbst verbraucht zu werden",
            "Katalysatoren senken die nötige Aktivierungsenergie und beschleunigen so Reaktionen, bleiben dabei aber selbst unverändert."),
        ("Was ist ein Beispiel für eine endotherme Reaktion im Alltag?", new[] { "Backen eines Kuchens", "Verbrennen von Holz", "Rosten von Eisen" }, "Backen eines Kuchens",
            "Beim Backen wird ständig Wärme von außen zugeführt (aufgenommen), damit der Teig reagieren und aufgehen kann - eine endotherme Reaktion."),
        ("Was ist ein Beispiel für eine exotherme Reaktion im Alltag?", new[] { "Verbrennen von Holz", "Schmelzen von Eis", "Backen eines Kuchens" }, "Verbrennen von Holz",
            "Beim Verbrennen wird Energie in Form von Wärme und Licht an die Umgebung abgegeben - eine exotherme Reaktion."),
        ("Was passiert bei einer Synthese-Reaktion?", new[] { "Zwei oder mehr Ausgangsstoffe verbinden sich zu einem neuen Stoff", "Ein Stoff zerfällt in mehrere Teile", "Es passiert überhaupt keine chemische Veränderung und deshalb hier nicht zutrifft" }, "Zwei oder mehr Ausgangsstoffe verbinden sich zu einem neuen Stoff",
            "Bei einer Synthese reagieren mehrere Ausgangsstoffe (Edukte) zu einem gemeinsamen neuen Produkt."),
        ("Was passiert bei einer Zersetzungsreaktion, vereinfacht?", new[] { "Ein Ausgangsstoff zerfällt in zwei oder mehr neue Stoffe", "Zwei Stoffe verbinden sich zu einem neuen Stoff", "Es entsteht überhaupt kein neuer Stoff" }, "Ein Ausgangsstoff zerfällt in zwei oder mehr neue Stoffe",
            "Bei einer Zersetzungsreaktion wird ein einzelner Ausgangsstoff in mehrere einfachere Stoffe aufgespalten."),
        ("Was bedeutet \"Aktivierungsenergie\" bei einer chemischen Reaktion?", new[] { "Die Energie, die nötig ist, um eine Reaktion überhaupt zu starten", "Die Energie, die am Ende einer Reaktion übrig bleibt, was so nicht korrekt ist", "Die Masse der beteiligten Stoffe" }, "Die Energie, die nötig ist, um eine Reaktion überhaupt zu starten",
            "Auch bei Reaktionen, die insgesamt Energie freisetzen, ist zunächst eine gewisse Aktivierungsenergie nötig, um sie zu starten."),
        ("Warum laufen manche chemische Reaktionen bei höherer Temperatur schneller ab?", new[] { "Die Teilchen bewegen sich schneller und stoßen häufiger zusammen", "Wärme verhindert grundsätzlich jede Reaktion", "Höhere Temperatur verändert die Masse der Teilchen" }, "Die Teilchen bewegen sich schneller und stoßen häufiger zusammen",
            "Bei höherer Temperatur bewegen sich Teilchen schneller, treffen häufiger und energiereicher aufeinander - das beschleunigt viele Reaktionen."),
        ("Was unterscheidet eine chemische Reaktion von einem rein physikalischen Vorgang wie Schmelzen?", new[] { "Bei einer chemischen Reaktion entstehen neue Stoffe mit neuen Eigenschaften", "Beide Vorgänge sind chemisch komplett identisch", "Physikalische Vorgänge erzeugen immer neue chemische Stoffe - eine haeufige, aber unzutreffende Vorstellung" }, "Bei einer chemischen Reaktion entstehen neue Stoffe mit neuen Eigenschaften",
            "Beim Schmelzen ändert sich nur der Aggregatzustand, der Stoff bleibt derselbe - bei einer chemischen Reaktion entstehen dagegen völlig neue Stoffe."),
        ("Woran erkennt man oft (nicht immer), dass eine chemische Reaktion stattgefunden hat?", new[] { "Z.B. an Farbwechsel, Gasbildung, Temperaturänderung oder Niederschlag", "Nur an einer Gewichtszunahme des gesamten Systems", "Chemische Reaktionen sind grundsätzlich nicht erkennbar" }, "Z.B. an Farbwechsel, Gasbildung, Temperaturänderung oder Niederschlag",
            "Typische Anzeichen einer chemischen Reaktion sind Farbänderungen, Gasentwicklung, Wärmeabgabe/-aufnahme oder das Ausfallen eines Feststoffs."),
        ("Was ist ein \"Niederschlag\" bei einer chemischen Reaktion in einer Flüssigkeit?", new[] { "Ein fester, unlöslicher Stoff, der sich bei der Reaktion bildet und ausfällt", "Regen, der in ein Reagenzglas fällt", "Ein anderes Wort für Katalysator" }, "Ein fester, unlöslicher Stoff, der sich bei der Reaktion bildet und ausfällt",
            "Reagieren zwei gelöste Stoffe zu einem unlöslichen Produkt, fällt dieses als sichtbarer Niederschlag aus."),
        ("Was zeigt das Pluszeichen in einer Reaktionsgleichung wie \"A + B → C\"?", new[] { "Dass die Stoffe A und B miteinander reagieren", "Dass A und B addiert eine höhere Temperatur ergeben", "Dass A und B sich gegenseitig neutralisieren, ohne zu reagieren" }, "Dass die Stoffe A und B miteinander reagieren",
            "Das Pluszeichen zeigt an, welche Ausgangsstoffe (Edukte) an der Reaktion beteiligt sind und miteinander reagieren."),
        ("Was sind \"Edukte\" in einer chemischen Reaktion?", new[] { "Die Ausgangsstoffe, die miteinander reagieren", "Die neuen Stoffe, die am Ende entstehen, auch wenn das manche zunaechst vermuten wuerden", "Ein anderes Wort für Katalysator" }, "Die Ausgangsstoffe, die miteinander reagieren",
            "Edukte stehen auf der linken Seite einer Reaktionsgleichung und reagieren zu den Produkten."),
        ("Was sind \"Produkte\" einer chemischen Reaktion?", new[] { "Die neuen Stoffe, die bei der Reaktion entstehen", "Die Ausgangsstoffe vor der Reaktion", "Ein anderes Wort für Aktivierungsenergie, was bei genauerem Hinsehen nicht stimmt" }, "Die neuen Stoffe, die bei der Reaktion entstehen",
            "Produkte stehen auf der rechten Seite der Reaktionsgleichung und sind das Ergebnis der chemischen Umwandlung."),
        ("Warum bleibt die Anzahl der Atome jeder Sorte bei einer korrekt ausgeglichenen Reaktionsgleichung gleich?", new[] { "Weil bei einer chemischen Reaktion keine Atome verloren gehen oder neu entstehen", "Weil Atome sich bei Reaktionen einfach in Energie auflösen", "Weil Reaktionsgleichungen reine Näherungen ohne echte Bedeutung sind (was so in der Praxis nicht zutrifft)" }, "Weil bei einer chemischen Reaktion keine Atome verloren gehen oder neu entstehen",
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
        ("Wie sind die Elemente im Periodensystem in den Spalten (Gruppen) angeordnet?", new[] { "Nach ähnlichen chemischen Eigenschaften", "Nach Farbe - eine verbreitete, aber falsche Annahme", "Zufällig" },
            "Nach ähnlichen chemischen Eigenschaften", "Elemente in derselben Gruppe (Spalte) haben ähnliche Eigenschaften, z.B. die Edelgase."),
        ("Welche Elementgruppe reagiert kaum mit anderen Stoffen?", new[] { "Edelgase", "Alkalimetalle", "Halogene" }, "Edelgase",
            "Edelgase (z.B. Helium, Neon) haben eine vollständig gefüllte äußere Elektronenschale und reagieren daher kaum."),
        ("Wie sind die Elemente im Periodensystem in den Zeilen (Perioden) angeordnet?", new[] { "Nach steigender Ordnungszahl/Protonenzahl", "Nach Alphabet", "Nach Entdeckungsjahr" }, "Nach steigender Ordnungszahl/Protonenzahl",
            "Innerhalb einer Periode (Zeile) steigt die Ordnungszahl (Protonenzahl) von links nach rechts."),
        ("Zu welcher Elementgruppe gehören Natrium und Kalium?", new[] { "Alkalimetalle", "Edelgase, was einer genaueren Pruefung nicht standhaelt", "Halogene" }, "Alkalimetalle",
            "Natrium und Kalium gehören zu den Alkalimetallen (1. Gruppe) - sehr reaktionsfreudige Metalle."),
        ("Wer entwickelte die erste Version des modernen Periodensystems?", new[] { "Dmitri Mendelejew", "Marie Curie", "Albert Einstein, obwohl das auf den ersten Blick plausibel klingt" }, "Dmitri Mendelejew",
            "Der russische Chemiker Dmitri Mendelejew ordnete 1869 die Elemente erstmals systematisch nach ihren Eigenschaften an."),
        ("Wie viele Hauptgruppen gibt es im klassischen Periodensystem?", new[] { "8", "4", "20" }, "8",
            "Das klassische Periodensystem gliedert sich in 8 Hauptgruppen mit jeweils ähnlichen chemischen Eigenschaften."),
        ("Zu welcher Elementgruppe gehören Fluor und Chlor?", new[] { "Halogene", "Edelgase", "Alkalimetalle" }, "Halogene",
            "Fluor und Chlor gehören zu den Halogenen (7. Hauptgruppe), sehr reaktionsfreudigen Nichtmetallen."),
        ("Was ist typisch für Halogene wie Fluor und Chlor?", new[] { "Sie sind sehr reaktionsfreudige Nichtmetalle", "Sie reagieren praktisch nie mit anderen Stoffen", "Sie sind alle bei Zimmertemperatur fest und ungefährlich" }, "Sie sind sehr reaktionsfreudige Nichtmetalle",
            "Halogene reagieren sehr leicht mit vielen anderen Elementen, z.B. mit Metallen zu Salzen."),
        ("Was bedeutet \"Hauptgruppenelement\" im Periodensystem, vereinfacht?", new[] { "Ein Element aus einer der Spalten mit regelmäßigen Eigenschaftsmustern", "Ein besonders seltenes, künstlich hergestelltes Element", "Ein Element, das nur in Sternen vorkommt" }, "Ein Element aus einer der Spalten mit regelmäßigen Eigenschaftsmustern",
            "Hauptgruppenelemente stehen in den nummerierten Spalten des Periodensystems, deren Elemente ähnliche, regelmäßige Eigenschaften zeigen."),
        ("Welche Elementgruppe bilden Sauerstoff und Schwefel?", new[] { "Chalkogene", "Halogene, was die eigentliche Bedeutung des Begriffs verfehlt", "Edelgase" }, "Chalkogene",
            "Sauerstoff und Schwefel gehören zur 6. Hauptgruppe, den sogenannten Chalkogenen."),
        ("Wo im Periodensystem findet man meist besonders reaktionsfreudige Metalle wie Natrium?", new[] { "Links im Periodensystem", "Rechts im Periodensystem", "Genau in der Mitte" }, "Links im Periodensystem",
            "Die stark reaktionsfreudigen Alkalimetalle wie Natrium stehen in der ersten Hauptgruppe, ganz links im Periodensystem."),
        ("Wo im Periodensystem findet man meist die Edelgase, die kaum reagieren?", new[] { "Rechts im Periodensystem", "Links im Periodensystem", "Genau in der Mitte" }, "Rechts im Periodensystem",
            "Edelgase stehen in der letzten Hauptgruppe ganz rechts im Periodensystem."),
        ("Wie verändert sich die Reaktionsfreudigkeit der Alkalimetalle von oben nach unten in ihrer Gruppe?", new[] { "Sie nimmt zu", "Sie nimmt ab", "Sie bleibt exakt gleich" }, "Sie nimmt zu",
            "Innerhalb der Alkalimetalle werden die Elemente von oben (z.B. Lithium) nach unten (z.B. Cäsium) zunehmend reaktionsfreudiger."),
        ("Was zeigt das chemische Symbol im Periodensystem für jedes Element, z.B. \"Na\"?", new[] { "Eine Abkürzung für den Elementnamen", "Die Farbe des Elements", "Den Entdecker des Elements und deshalb hier nicht zutrifft" }, "Eine Abkürzung für den Elementnamen",
            "Chemische Symbole sind international einheitliche Abkürzungen für die Elementnamen, z.B. \"Na\" für Natrium."),
        ("Was zeigt die Zahl über dem chemischen Symbol im Periodensystem meist an?", new[] { "Die Ordnungszahl (Protonenzahl)", "Die Temperatur des Elements", "Das Entdeckungsjahr" }, "Die Ordnungszahl (Protonenzahl)",
            "Die Ordnungszahl über dem Symbol gibt an, wie viele Protonen ein Atom dieses Elements im Kern besitzt."),
        ("Wie viele Elemente sind aktuell im Periodensystem ungefähr bekannt?", new[] { "Etwas mehr als 100", "Etwa 20", "Über 10.000" }, "Etwas mehr als 100",
            "Das Periodensystem umfasst derzeit etwas mehr als 100 bekannte chemische Elemente."),
        ("Was unterscheidet Metalle grob von Nichtmetallen in ihrer Lage im Periodensystem?", new[] { "Metalle stehen meist links/mittig, Nichtmetalle eher rechts oben", "Metalle und Nichtmetalle sind zufällig verteilt, was so nicht korrekt ist", "Nichtmetalle stehen immer ganz links" }, "Metalle stehen meist links/mittig, Nichtmetalle eher rechts oben",
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
        ("Was ist ein Mol in der Chemie?", new[] { "Eine festgelegte Stoffmenge von genau 6,022 · 10^23 Teilchen", "Ein anderes Wort für Gramm", "Eine Einheit für die Temperatur - eine haeufige, aber unzutreffende Vorstellung" }, "Eine festgelegte Stoffmenge von genau 6,022 · 10^23 Teilchen",
            "Ein Mol entspricht der Avogadro-Konstante von rund 6,022 · 10^23 Teilchen (Atomen, Molekülen oder Ionen) eines Stoffes."),
        ("Was beschreibt die molare Masse eines Stoffes?", new[] { "Die Masse von einem Mol dieses Stoffes in Gramm pro Mol", "Die Masse eines einzelnen Atoms in Kilogramm", "Das Volumen eines Stoffes bei Zimmertemperatur, auch wenn das manche zunaechst vermuten wuerden" }, "Die Masse von einem Mol dieses Stoffes in Gramm pro Mol",
            "Die molare Masse gibt an, wie viel Gramm ein Mol eines Stoffes wiegt, z.B. hat Wasser (H₂O) eine molare Masse von etwa 18 g/mol."),
        ("Wie berechnet man die Stoffmenge n aus der gegebenen Masse m und der molaren Masse M?", new[] { "n = m / M", "n = m · M", "n = M / m" }, "n = m / M",
            "Die Stoffmenge in Mol ergibt sich, indem man die Masse eines Stoffes durch seine molare Masse teilt (n = m/M)."),
        ("Was gibt die Stoffmengenkonzentration einer wässrigen Lösung an?", new[] { "Wie viel Mol eines gelösten Stoffes in einem Liter Lösung enthalten sind", "Nur die Farbe der Lösung", "Die Temperatur der Lösung, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Wie viel Mol eines gelösten Stoffes in einem Liter Lösung enthalten sind",
            "Die Stoffmengenkonzentration (in mol/l) beschreibt, wie viel Mol eines gelösten Stoffes in einem Liter der Lösung vorhanden sind."),
        ("Was bedeutet \"stöchiometrisches Rechnen\" bei einer chemischen Reaktion?", new[] { "Die Mengenverhältnisse von Edukten und Produkten anhand der Reaktionsgleichung berechnen", "Nur die Farbe der Reaktion beschreiben", "Die Reaktionszeit mit einer Stoppuhr messen" }, "Die Mengenverhältnisse von Edukten und Produkten anhand der Reaktionsgleichung berechnen",
            "Stöchiometrisches Rechnen nutzt die Zahlenverhältnisse einer ausgeglichenen Reaktionsgleichung, um Massen, Stoffmengen oder Volumina von Edukten und Produkten zu berechnen."),
        ("Warum muss eine chemische Reaktionsgleichung ausgeglichen (stimmig) sein, bevor man stöchiometrisch rechnet?", new[] { "Damit die Anzahl der Atome auf beiden Seiten der Gleichung übereinstimmt (Erhaltung der Masse)", "Weil das Aussehen der Gleichung sonst nicht ordentlich wäre", "Ausgeglichene Gleichungen sind für die Rechnung nicht notwendig" }, "Damit die Anzahl der Atome auf beiden Seiten der Gleichung übereinstimmt (Erhaltung der Masse)",
            "Nach dem Gesetz der Erhaltung der Masse müssen bei einer chemischen Reaktion auf beiden Seiten der Gleichung gleich viele Atome jeder Sorte vorhanden sein."),
        ("Was versteht man unter einer isotonischen Kochsalzlösung?", new[] { "Eine Salzlösung mit einer Konzentration, die der des menschlichen Blutes entspricht", "Eine Lösung ganz ohne gelöstes Salz", "Eine Lösung mit maximal möglicher Salzkonzentration" }, "Eine Salzlösung mit einer Konzentration, die der des menschlichen Blutes entspricht",
            "Eine isotonische Kochsalzlösung (ca. 0,9 % NaCl) hat eine Konzentration, die osmotisch der von Blutplasma entspricht und wird u.a. medizinisch verwendet."),
        ("Welches Gasvolumen nimmt ein Mol eines idealen Gases bei Normbedingungen ungefähr ein?", new[] { "Etwa 22,4 Liter", "Etwa 1 Liter", "Etwa 100 Liter - eine verbreitete, aber falsche Annahme" }, "Etwa 22,4 Liter",
            "Unter Normbedingungen (0 °C, 1013 hPa) nimmt ein Mol eines idealen Gases ein molares Volumen von etwa 22,4 Litern ein."),
        ("Wie berechnet man die Masse eines Reaktionsprodukts, wenn man die eingesetzte Stoffmenge und die molare Masse des Produkts kennt?", new[] { "Man multipliziert die Stoffmenge mit der molaren Masse (m = n · M)", "Man dividiert die Stoffmenge durch die Zeit", "Man addiert die Stoffmenge und die molare Masse, was einer genaueren Pruefung nicht standhaelt" }, "Man multipliziert die Stoffmenge mit der molaren Masse (m = n · M)",
            "Die Masse eines Stoffes berechnet man, indem man die vorhandene Stoffmenge mit seiner molaren Masse multipliziert (m = n · M)."),
        ("Was ist die molare Masse von Wasser (H₂O) ungefähr, ausgehend von H ≈ 1 g/mol und O ≈ 16 g/mol?", new[] { "Etwa 18 g/mol", "Etwa 2 g/mol", "Etwa 32 g/mol" }, "Etwa 18 g/mol",
            "Wasser besteht aus zwei Wasserstoffatomen (2 · 1 g/mol) und einem Sauerstoffatom (16 g/mol), zusammen ergibt das etwa 18 g/mol."),
        ("Warum ist das Mol als \"Zählmaß\" in der Chemie besonders praktisch?", new[] { "Es erlaubt, mit sehr großen Teilchenzahlen wie mit handlichen, alltagstauglichen Zahlen zu rechnen", "Weil ein Mol immer exakt einem Gramm entspricht", "Weil sich damit die Farbe von Stoffen bestimmen lässt, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Es erlaubt, mit sehr großen Teilchenzahlen wie mit handlichen, alltagstauglichen Zahlen zu rechnen",
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
        ("Warum kann man aus der Reaktionsgleichung 2 H₂ + O₂ → 2 H₂O ablesen, wie viel Mol Wasserstoff für ein Mol Sauerstoff benötigt werden?", new[] { "Die Zahlen vor den Formeln (Koeffizienten) geben das Stoffmengenverhältnis der Reaktion an", "Die Reaktionsgleichung sagt nichts über Mengenverhältnisse aus", "Nur die chemischen Symbole selbst, nicht die Zahlen davor, sind relevant und deshalb hier nicht zutrifft" }, "Die Zahlen vor den Formeln (Koeffizienten) geben das Stoffmengenverhältnis der Reaktion an",
            "Die Koeffizienten vor den Formeln in einer ausgeglichenen Reaktionsgleichung geben direkt das Stoffmengenverhältnis an, in dem die Stoffe reagieren."),
        ("Was ist ein Grund, warum in der Chemie oft mit der Einheit \"mol/l\" statt direkt mit Gramm gearbeitet wird?", new[] { "So lassen sich Teilchenzahlen unabhängig von unterschiedlichen molaren Massen direkt vergleichen", "Gramm ist in der Chemie grundsätzlich verboten zu verwenden", "mol/l hat mit der Stoffmenge nichts zu tun" }, "So lassen sich Teilchenzahlen unabhängig von unterschiedlichen molaren Massen direkt vergleichen",
            "Da unterschiedliche Stoffe unterschiedliche molare Massen haben, ermöglicht die Angabe in mol/l einen direkten Vergleich der tatsächlichen Teilchenzahlen in Lösungen."),
        ("Welche zwei Größen braucht man mindestens, um die Stoffmengenkonzentration einer Lösung zu berechnen?", new[] { "Die Stoffmenge des gelösten Stoffes und das Volumen der Lösung", "Nur die Farbe und den Geruch der Lösung, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Nur die Raumtemperatur des Labors" }, "Die Stoffmenge des gelösten Stoffes und das Volumen der Lösung",
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
        ("Was gibt der pH-Wert einer Lösung an?", new[] { "Wie sauer oder basisch (alkalisch) eine Lösung ist", "Die Temperatur einer Lösung, auch wenn das manche zunaechst vermuten wuerden", "Die Farbe einer Lösung" }, "Wie sauer oder basisch (alkalisch) eine Lösung ist",
            "Der pH-Wert misst die Konzentration von Wasserstoff- bzw. Oxonium-Ionen und zeigt, wie sauer (niedriger pH) oder basisch (hoher pH) eine Lösung ist."),
        ("Welcher pH-Wert gilt als neutral, wie bei reinem Wasser?", new[] { "pH 7", "pH 0", "pH 14" }, "pH 7",
            "Reines Wasser hat bei Raumtemperatur einen neutralen pH-Wert von etwa 7 - Werte darunter gelten als sauer, darüber als basisch."),
        ("Was passiert bei einer Neutralisationsreaktion zwischen einer Säure und einer Lauge?", new[] { "Wasserstoff-Ionen und Hydroxid-Ionen reagieren zu Wasser, ein Salz entsteht", "Es entsteht ausschließlich ein neues Gas ohne Wasser", "Es findet grundsätzlich keine chemische Reaktion statt" }, "Wasserstoff-Ionen und Hydroxid-Ionen reagieren zu Wasser, ein Salz entsteht",
            "Bei der Neutralisation reagieren Wasserstoff-Ionen (H⁺) der Säure mit Hydroxid-Ionen (OH⁻) der Lauge zu Wasser, während sich außerdem ein Salz bildet."),
        ("Was passiert, wenn ein unedles Metall wie Zink mit einer sauren Lösung reagiert?", new[] { "Es löst sich unter Wasserstoffgas-Entwicklung auf", "Es reagiert überhaupt nicht mit der Säure, was bei genauerem Hinsehen nicht stimmt", "Es verwandelt sich in ein Edelmetall" }, "Es löst sich unter Wasserstoffgas-Entwicklung auf",
            "Unedle Metalle wie Zink reagieren mit Säuren, wobei sich das Metall auflöst und Wasserstoffgas entsteht."),
        ("Was passiert, wenn ein Carbonat (z.B. Kalk) mit einer Säure reagiert?", new[] { "Es entsteht Kohlenstoffdioxid-Gas unter Aufschäumen", "Es entsteht reiner Sauerstoff", "Es findet keinerlei Reaktion statt (was so in der Praxis nicht zutrifft)" }, "Es entsteht Kohlenstoffdioxid-Gas unter Aufschäumen",
            "Carbonate reagieren mit Säuren unter Bildung von Kohlenstoffdioxid, Wasser und einem Salz, sichtbar als Aufschäumen (Gasentwicklung)."),
        ("Was beschreibt das erweiterte Säure-Base-Konzept nach Brønsted?", new[] { "Säuren sind Protonendonatoren, Basen sind Protonenakzeptoren", "Säuren und Basen unterscheiden sich ausschließlich durch ihre Farbe", "Nach Brønsted gibt es keinen Unterschied zwischen Säuren und Basen" }, "Säuren sind Protonendonatoren, Basen sind Protonenakzeptoren",
            "Nach Brønsted ist eine Säure ein Stoff, der ein Proton (H⁺) abgibt, eine Base ein Stoff, der ein Proton aufnimmt."),
        ("Was ist ein Oxonium-Ion (H₃O⁺)?", new[] { "Ein Wassermolekül, das zusätzlich ein Proton aufgenommen hat", "Ein anderes Wort für Hydroxid-Ion", "Ein reines Sauerstoffmolekül ohne Wasserstoff - eine verbreitete, aber falsche Annahme" }, "Ein Wassermolekül, das zusätzlich ein Proton aufgenommen hat",
            "In wässriger Lösung docken abgegebene Protonen (H⁺) an Wassermoleküle an, wodurch Oxonium-Ionen (H₃O⁺) entstehen."),
        ("Was ist ein Hydroxid-Ion (OH⁻)?", new[] { "Ein negativ geladenes Ion aus einem Sauerstoff- und einem Wasserstoffatom, typisch für Basen", "Ein anderes Wort für Oxonium-Ion", "Ein Bestandteil, der nur in Säuren vorkommt, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt" }, "Ein negativ geladenes Ion aus einem Sauerstoff- und einem Wasserstoffatom, typisch für Basen",
            "Hydroxid-Ionen (OH⁻) sind charakteristisch für basische (alkalische) Lösungen und entstehen z.B. beim Lösen von Laugen in Wasser."),
        ("Warum kann man das Entkalken einer Kaffeemaschine mit Essig als chemische Reaktion beschreiben?", new[] { "Die Essigsäure reagiert mit dem Kalk (Carbonat) und löst ihn dadurch auf", "Essig hat überhaupt keine Wirkung auf Kalkablagerungen, was die eigentliche Bedeutung des Begriffs verfehlt", "Kalk besteht ausschließlich aus reinem Wasser" }, "Die Essigsäure reagiert mit dem Kalk (Carbonat) und löst ihn dadurch auf",
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
        ("Warum eignet sich Essigsäure als vergleichsweise umweltschonender Haushaltsreiniger gegen Kalk?", new[] { "Sie ist eine schwächere organische Säure, die Kalk dennoch löst, aber weniger aggressiv als starke Mineralsäuren ist", "Essigsäure hat überhaupt keine chemische Wirkung auf Kalk und deshalb hier nicht zutrifft, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Essigsäure ist die stärkste bekannte Säure überhaupt" }, "Sie ist eine schwächere organische Säure, die Kalk dennoch löst, aber weniger aggressiv als starke Mineralsäuren ist",
            "Essigsäure gilt als vergleichsweise milde, biologisch leichter abbaubare Säure, die Kalk zuverlässig löst, ohne so aggressiv wie starke anorganische Säuren zu sein."),
        ("Wie unterscheidet sich eine starke Säure von einer schwachen Säure hinsichtlich der Abgabe von Protonen?", new[] { "Eine starke Säure gibt ihre Protonen in Wasser nahezu vollständig ab, eine schwache nur teilweise", "Beide geben ihre Protonen exakt im gleichen Maß ab", "Schwache Säuren geben grundsätzlich mehr Protonen ab als starke Säuren" }, "Eine starke Säure gibt ihre Protonen in Wasser nahezu vollständig ab, eine schwache nur teilweise",
            "Starke Säuren dissoziieren in Wasser nahezu vollständig in Ionen, schwache Säuren dagegen nur teilweise - das beeinflusst auch den pH-Wert einer Lösung gleicher Konzentration."),
        ("Was ist ein Indikator in der Säure-Base-Chemie?", new[] { "Ein Stoff, der je nach pH-Wert seine Farbe ändert", "Ein Gerät zur Temperaturmessung, auch wenn das manche zunaechst vermuten wuerden", "Ein anderes Wort für ein Salz" }, "Ein Stoff, der je nach pH-Wert seine Farbe ändert",
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
        ("Was ist ein Kohlenwasserstoff?", new[] { "Eine chemische Verbindung, die nur aus Kohlenstoff- und Wasserstoffatomen besteht", "Eine Verbindung, die ausschließlich Sauerstoff und Wasserstoff enthält, was bei genauerem Hinsehen nicht stimmt", "Ein anderes Wort für ein Salz" }, "Eine chemische Verbindung, die nur aus Kohlenstoff- und Wasserstoffatomen besteht",
            "Kohlenwasserstoffe bestehen ausschließlich aus den Elementen Kohlenstoff (C) und Wasserstoff (H)."),
        ("Was zeichnet Alkane als gesättigte Kohlenwasserstoffe aus?", new[] { "Sie enthalten ausschließlich Einfachbindungen zwischen den Kohlenstoffatomen", "Sie enthalten mindestens eine Doppelbindung", "Sie enthalten mindestens eine Dreifachbindung" }, "Sie enthalten ausschließlich Einfachbindungen zwischen den Kohlenstoffatomen",
            "Alkane gelten als gesättigt, weil zwischen den Kohlenstoffatomen ausschließlich Einfachbindungen vorliegen und jedes C-Atom maximal mit Wasserstoff abgesättigt ist."),
        ("Was unterscheidet ein Alken von einem Alkan?", new[] { "Ein Alken besitzt mindestens eine Kohlenstoff-Kohlenstoff-Doppelbindung", "Ein Alken besteht nur aus Wasserstoffatomen", "Alkene enthalten grundsätzlich keinen Kohlenstoff" }, "Ein Alken besitzt mindestens eine Kohlenstoff-Kohlenstoff-Doppelbindung",
            "Alkene sind ungesättigte Kohlenwasserstoffe mit mindestens einer C=C-Doppelbindung, im Gegensatz zu den nur einfach gebundenen Alkanen."),
        ("Was zeichnet ein Alkin aus?", new[] { "Es besitzt mindestens eine Kohlenstoff-Kohlenstoff-Dreifachbindung", "Es besitzt ausschließlich Einfachbindungen", "Es enthält kein einziges Kohlenstoffatom" }, "Es besitzt mindestens eine Kohlenstoff-Kohlenstoff-Dreifachbindung",
            "Alkine sind ungesättigte Kohlenwasserstoffe mit mindestens einer C≡C-Dreifachbindung, z.B. Ethin (Acetylen)."),
        ("Was versteht man unter einer \"homologen Reihe\" bei Alkanen?", new[] { "Eine Reihe von Verbindungen, die sich jeweils um eine CH₂-Gruppe unterscheiden und ähnliche Eigenschaften haben", "Eine zufällige Auswahl völlig unterschiedlicher Stoffklassen (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Eine Reihe von Metallen im Periodensystem" }, "Eine Reihe von Verbindungen, die sich jeweils um eine CH₂-Gruppe unterscheiden und ähnliche Eigenschaften haben",
            "In einer homologen Reihe wie der der Alkane unterscheidet sich jedes Glied vom nächsten durch eine zusätzliche CH₂-Gruppe, wobei sich die Eigenschaften regelmäßig verändern."),
        ("Warum steigt der Siedepunkt innerhalb der homologen Reihe der Alkane mit zunehmender Kettenlänge meist an?", new[] { "Längere Ketten haben stärkere Van-der-Waals-Kräfte zwischen den Molekülen", "Längere Ketten haben grundsätzlich schwächere zwischenmolekulare Kräfte, was einer genaueren Pruefung nicht standhaelt", "Die Kettenlänge hat keinerlei Einfluss auf den Siedepunkt" }, "Längere Ketten haben stärkere Van-der-Waals-Kräfte zwischen den Molekülen",
            "Mit zunehmender Molekülgröße und Kettenlänge nehmen die Van-der-Waals-Kräfte zwischen den Molekülen zu, wodurch mehr Energie zum Trennen (Sieden) nötig ist und der Siedepunkt steigt."),
        ("Was sind Van-der-Waals-Kräfte?", new[] { "Schwache, zwischenmolekulare Anziehungskräfte zwischen unpolaren Molekülen", "Starke chemische Bindungen innerhalb eines Moleküls, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für Ionenbindungen" }, "Schwache, zwischenmolekulare Anziehungskräfte zwischen unpolaren Molekülen",
            "Van-der-Waals-Kräfte sind schwache, aber mit zunehmender Molekülgröße stärker werdende Anziehungskräfte zwischen unpolaren Molekülen wie Alkanen."),
        ("Was ist Isomerie bei Kohlenwasserstoffen?", new[] { "Verschiedene Moleküle mit derselben Summenformel, aber unterschiedlichem Aufbau", "Zwei völlig identische Moleküle mit unterschiedlicher Summenformel", "Ein anderes Wort für eine chemische Reaktion" }, "Verschiedene Moleküle mit derselben Summenformel, aber unterschiedlichem Aufbau",
            "Isomere haben dieselbe Summenformel, unterscheiden sich aber in der Anordnung der Atome (Strukturisomerie), was zu unterschiedlichen Eigenschaften führen kann."),
        ("Was ist Methan (CH₄), das einfachste Alkan, in der Natur häufig?", new[] { "Ein Hauptbestandteil von Erdgas", "Ein seltenes Edelmetall", "Eine feste Kristallstruktur bei Raumtemperatur" }, "Ein Hauptbestandteil von Erdgas",
            "Methan (CH₄) ist das einfachste Alkan und ein Hauptbestandteil von Erdgas, das u.a. als Campinggas verwendet wird."),
        ("Warum haben kurzkettige Alkane wie Methan oder Propan bei Raumtemperatur einen gasförmigen Zustand?", new[] { "Ihre Van-der-Waals-Kräfte sind wegen der kurzen Kette relativ schwach", "Ihre Van-der-Waals-Kräfte sind extrem stark", "Sie besitzen keinerlei zwischenmolekulare Kräfte" }, "Ihre Van-der-Waals-Kräfte sind wegen der kurzen Kette relativ schwach",
            "Kurzkettige Alkane haben schwächere Van-der-Waals-Kräfte, wodurch schon bei niedrigen Temperaturen die Molekülbewegung ausreicht, um den gasförmigen Zustand zu erreichen."),
        ("Warum wird Superbenzin (längerkettige Kohlenwasserstoffe) bei Raumtemperatur flüssig, während Campinggas gasförmig bleibt?", new[] { "Längere Kohlenstoffketten haben stärkere Van-der-Waals-Kräfte und damit einen höheren Siedepunkt", "Beide Stoffe haben exakt denselben Aggregatzustand", "Kettenlänge hat keinen Einfluss auf den Aggregatzustand, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Längere Kohlenstoffketten haben stärkere Van-der-Waals-Kräfte und damit einen höheren Siedepunkt",
            "Die längeren Molekülketten in Benzinbestandteilen erzeugen stärkere Van-der-Waals-Kräfte, wodurch ihr Siedepunkt über der Raumtemperatur liegt und sie flüssig bleiben."),
        ("Was bezeichnet die Nomenklatur \"Propan\", \"Butan\", \"Pentan\" bei Alkanen?", new[] { "Die Namen geben die Anzahl der Kohlenstoffatome in der Kette an", "Die Namen beziehen sich auf die Farbe der Stoffe", "Die Namen haben nichts mit der Molekülstruktur zu tun und deshalb hier nicht zutrifft" }, "Die Namen geben die Anzahl der Kohlenstoffatome in der Kette an",
            "Die Namensendungen der Alkane (z.B. Prop-, But-, Pent-) leiten sich von der Anzahl der Kohlenstoffatome in der Hauptkette ab (3, 4, 5 Kohlenstoffatome)."),
        ("Was passiert chemisch bei der vollständigen Verbrennung eines Kohlenwasserstoffs wie Methan?", new[] { "Es entstehen Kohlenstoffdioxid und Wasser unter Freisetzung von Energie", "Es entsteht ausschließlich reiner Kohlenstoff, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Es findet keinerlei chemische Reaktion statt" }, "Es entstehen Kohlenstoffdioxid und Wasser unter Freisetzung von Energie",
            "Bei vollständiger Verbrennung reagieren Kohlenwasserstoffe mit Sauerstoff zu Kohlenstoffdioxid und Wasser, wobei Energie in Form von Wärme freigesetzt wird."),
        ("Warum sind Alkene chemisch reaktiver als Alkane?", new[] { "Die Doppelbindung kann leichter aufgebrochen und für Additionsreaktionen genutzt werden", "Alkene besitzen grundsätzlich mehr Kohlenstoffatome als Alkane, auch wenn das manche zunaechst vermuten wuerden", "Alkene sind chemisch komplett träge und reagieren nie" }, "Die Doppelbindung kann leichter aufgebrochen und für Additionsreaktionen genutzt werden",
            "Die C=C-Doppelbindung von Alkenen kann leichter aufgebrochen werden, was Additionsreaktionen (z.B. mit Wasserstoff oder Halogenen) ermöglicht - Alkane reagieren dagegen deutlich träger."),
        ("Was passiert bei einer Additionsreaktion an einem Alken, z.B. mit Wasserstoff?", new[] { "Die Doppelbindung wird aufgebrochen und zusätzliche Atome werden angelagert", "Es entsteht dabei keinerlei neue Verbindung", "Die Doppelbindung bleibt dabei zwingend vollständig erhalten" }, "Die Doppelbindung wird aufgebrochen und zusätzliche Atome werden angelagert",
            "Bei einer Addition wird die Doppelbindung des Alkens aufgebrochen, wobei sich zusätzliche Atome (z.B. Wasserstoff) an die beiden Kohlenstoffatome anlagern."),
        ("Was ist Ethin (Acetylin), ein bekanntes Alkin, für ein Beispiel?", new[] { "Ein Gas, das u.a. beim Schweißen als Brenngas mit sehr heißer Flamme genutzt wird", "Ein Metall, das bei Raumtemperatur fest ist, was bei genauerem Hinsehen nicht stimmt", "Ein Bestandteil von Kochsalz" }, "Ein Gas, das u.a. beim Schweißen als Brenngas mit sehr heißer Flamme genutzt wird",
            "Ethin (Acetylen) verbrennt mit sehr hoher Flammtemperatur und wird deshalb u.a. beim Autogenschweißen als Brenngas eingesetzt."),
        ("Was ist der Unterschied zwischen einer geradkettigen und einer verzweigten Isomerie bei Alkanen?", new[] { "Bei verzweigter Isomerie zweigen zusätzliche Kohlenstoffketten von der Hauptkette ab", "Beide Formen haben exakt dieselbe Struktur", "Geradkettige Isomere besitzen grundsätzlich mehr Kohlenstoffatome (was so in der Praxis nicht zutrifft)" }, "Bei verzweigter Isomerie zweigen zusätzliche Kohlenstoffketten von der Hauptkette ab",
            "Bei verzweigten Isomeren gehen von der Hauptkette zusätzliche Seitenketten ab, wodurch sich trotz gleicher Summenformel andere physikalische Eigenschaften (z.B. Siedepunkt) ergeben."),
        ("Warum haben verzweigte Alkane oft einen niedrigeren Siedepunkt als ihre geradkettigen Isomere?", new[] { "Verzweigte Moleküle haben eine kompaktere Form mit weniger Kontaktfläche für Van-der-Waals-Kräfte", "Verzweigte Moleküle haben grundsätzlich stärkere Van-der-Waals-Kräfte", "Die Verzweigung hat keinerlei Einfluss auf den Siedepunkt" }, "Verzweigte Moleküle haben eine kompaktere Form mit weniger Kontaktfläche für Van-der-Waals-Kräfte",
            "Verzweigte Isomere sind kompakter geformt und bieten benachbarten Molekülen weniger Kontaktfläche, wodurch die Van-der-Waals-Kräfte schwächer sind und der Siedepunkt sinkt."),
        ("Warum zählt Erdöl als wichtigste Rohstoffquelle für viele Kohlenwasserstoffe wie Benzin?", new[] { "Erdöl besteht aus einem komplexen Gemisch verschiedenster Kohlenwasserstoffe, die destillativ getrennt werden können", "Erdöl enthält überhaupt keine Kohlenwasserstoffe", "Erdöl besteht ausschließlich aus reinem Methan" }, "Erdöl besteht aus einem komplexen Gemisch verschiedenster Kohlenwasserstoffe, die destillativ getrennt werden können",
            "Erdöl ist ein Gemisch unterschiedlichster Kohlenwasserstoffe, das durch fraktionierte Destillation in verschiedene Produkte wie Benzin, Diesel oder Gase aufgetrennt wird."),
        ("Was versteht man unter der \"fraktionierten Destillation\" von Erdöl?", new[] { "Die Auftrennung des Kohlenwasserstoffgemischs anhand unterschiedlicher Siedepunkte in einzelne Fraktionen", "Ein Verfahren, das ausschließlich Wasser aus Erdöl entfernt", "Ein Vorgang, bei dem Erdöl chemisch in Sauerstoff umgewandelt wird - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt" }, "Die Auftrennung des Kohlenwasserstoffgemischs anhand unterschiedlicher Siedepunkte in einzelne Fraktionen",
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
        ("Warum ist Methanol im Gegensatz zu Ethanol für den Menschen hochgiftig?", new[] { "Es wird im Körper zu giftigen Abbauprodukten wie Formaldehyd und Ameisensäure umgewandelt", "Methanol hat exakt dieselbe Wirkung wie Ethanol", "Methanol wird vom Körper überhaupt nicht aufgenommen, obwohl das auf den ersten Blick plausibel klingt" }, "Es wird im Körper zu giftigen Abbauprodukten wie Formaldehyd und Ameisensäure umgewandelt",
            "Im Körper wird Methanol zu giftigem Formaldehyd und weiter zu Ameisensäure abgebaut, die u.a. den Sehnerv schädigen können."),
        ("Was ist Glycerin (Propantriol) im Vergleich zu einfachen Alkoholen wie Ethanol?", new[] { "Ein mehrwertiger Alkohol mit drei Hydroxy-Gruppen", "Ein Alkohol ganz ohne Hydroxy-Gruppe", "Ein anderes Wort für Methanol" }, "Ein mehrwertiger Alkohol mit drei Hydroxy-Gruppen",
            "Glycerin besitzt gleich drei Hydroxy-Gruppen pro Molekül und zählt deshalb zu den mehrwertigen Alkoholen."),
        ("Warum sind kurzkettige Alkohole wie Ethanol gut in Wasser löslich?", new[] { "Die Hydroxy-Gruppe kann Wasserstoffbrückenbindungen mit Wassermolekülen ausbilden", "Kurzkettige Alkohole besitzen keinerlei funktionelle Gruppe", "Wasserstoffbrückenbindungen spielen bei der Löslichkeit keine Rolle, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Die Hydroxy-Gruppe kann Wasserstoffbrückenbindungen mit Wassermolekülen ausbilden",
            "Die polare Hydroxy-Gruppe kurzkettiger Alkohole bildet Wasserstoffbrückenbindungen zu Wassermolekülen aus, was eine gute Löslichkeit ermöglicht."),
        ("Warum werden langkettige Alkohole zunehmend schlechter wasserlöslich?", new[] { "Der wachsende, unpolare Kohlenwasserstoffrest überwiegt gegenüber der einzelnen polaren Hydroxy-Gruppe", "Langkettige Alkohole besitzen mehr Hydroxy-Gruppen als kurzkettige", "Die Kettenlänge hat keinerlei Einfluss auf die Löslichkeit" }, "Der wachsende, unpolare Kohlenwasserstoffrest überwiegt gegenüber der einzelnen polaren Hydroxy-Gruppe",
            "Mit zunehmender Kettenlänge dominiert der unpolare Kohlenwasserstoffanteil des Moleküls, wodurch die wasserlösende Wirkung der einzelnen Hydroxy-Gruppe relativ abnimmt."),
        ("Was bedeutet \"hydrophil\" bei einem Molekülteil?", new[] { "Wasseranziehend, gut wasserlöslich", "Wasserabstoßend", "Ein anderes Wort für giftig und deshalb hier nicht zutrifft" }, "Wasseranziehend, gut wasserlöslich",
            "Hydrophile (\"wasserliebende\") Molekülteile wie die Hydroxy-Gruppe interagieren gut mit polaren Wassermolekülen."),
        ("Was bedeutet \"hydrophob\" bei einem Molekülteil?", new[] { "Wasserabstoßend, schlecht wasserlöslich", "Wasseranziehend", "Ein anderes Wort für eine Säure, was so nicht korrekt ist" }, "Wasserabstoßend, schlecht wasserlöslich",
            "Hydrophobe (\"wasserabweisende\") Molekülteile wie lange Kohlenwasserstoffketten mischen sich schlecht mit Wasser."),
        ("Was entsteht bei der Oxidation eines primären Alkohols wie Ethanol?", new[] { "Ein Aldehyd (Alkanal), z.B. Ethanal", "Ausschließlich reines Wasser", "Ein Edelmetall" }, "Ein Aldehyd (Alkanal), z.B. Ethanal",
            "Primäre Alkohole werden bei der Oxidation zunächst zu Aldehyden (Alkanalen) umgesetzt, z.B. Ethanol zu Ethanal."),
        ("Was kennzeichnet die Aldehyd-Gruppe chemisch?", new[] { "Eine Kohlenstoff-Sauerstoff-Doppelbindung mit einem zusätzlichen Wasserstoffatom am selben Kohlenstoff", "Eine Gruppe, die ausschließlich aus Kohlenstoff und Stickstoff besteht - eine haeufige, aber unzutreffende Vorstellung", "Ein anderes Wort für die Hydroxy-Gruppe" }, "Eine Kohlenstoff-Sauerstoff-Doppelbindung mit einem zusätzlichen Wasserstoffatom am selben Kohlenstoff",
            "Die Aldehyd-Gruppe (-CHO) besteht aus einem Kohlenstoffatom mit einer Doppelbindung zu Sauerstoff und einem gebundenen Wasserstoffatom."),
        ("Was passiert mit der Oxidationszahl des Kohlenstoffatoms, wenn Ethanol zu Ethanal oxidiert wird?", new[] { "Sie steigt an, da bei der Oxidation Elektronen abgegeben werden", "Sie sinkt, da Elektronen aufgenommen werden, auch wenn das manche zunaechst vermuten wuerden", "Sie bleibt exakt unverändert" }, "Sie steigt an, da bei der Oxidation Elektronen abgegeben werden",
            "Bei einer Oxidationsreaktion steigt die Oxidationszahl des betroffenen Kohlenstoffatoms an, da formal Elektronen abgegeben werden."),
        ("Warum ist ein primärer Alkohol anders aufgebaut als ein sekundärer oder tertiärer Alkohol?", new[] { "Die Anzahl der Kohlenstoffatome, die direkt an das Kohlenstoffatom mit der OH-Gruppe gebunden sind, unterscheidet sich", "Alle Alkoholtypen haben exakt denselben Aufbau", "Nur primäre Alkohole besitzen überhaupt eine Hydroxy-Gruppe, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Die Anzahl der Kohlenstoffatome, die direkt an das Kohlenstoffatom mit der OH-Gruppe gebunden sind, unterscheidet sich",
            "Primäre, sekundäre und tertiäre Alkohole unterscheiden sich danach, wie viele weitere Kohlenstoffatome direkt an das die Hydroxy-Gruppe tragende Kohlenstoffatom gebunden sind."),
        ("Wofür wird Ethanol in der Industrie und im Alltag unter anderem verwendet?", new[] { "Als Lösungsmittel, Desinfektionsmittel und Kraftstoffbeimischung", "Ausschließlich zum Trinken", "Ausschließlich als Baumaterial" }, "Als Lösungsmittel, Desinfektionsmittel und Kraftstoffbeimischung",
            "Ethanol wird u.a. als Lösungsmittel, Desinfektionsmittel und als Beimischung zu Kraftstoffen (Biosprit) genutzt."),
        ("Warum wird Glycerin häufig in Kosmetikprodukten eingesetzt?", new[] { "Es bindet gut Feuchtigkeit aufgrund seiner mehreren Hydroxy-Gruppen", "Es ist stark giftig und wirkt deshalb konservierend - eine verbreitete, aber falsche Annahme", "Es hat überhaupt keine Wechselwirkung mit Wasser" }, "Es bindet gut Feuchtigkeit aufgrund seiner mehreren Hydroxy-Gruppen",
            "Die mehreren Hydroxy-Gruppen des Glycerins können viele Wasserstoffbrückenbindungen zu Wasser ausbilden, was Feuchtigkeit bindet - deshalb wird es oft in Cremes verwendet."),
        ("Was passiert bei einer weiteren Oxidation eines Aldehyds (Alkanals)?", new[] { "Es entsteht eine Carbonsäure (Alkansäure)", "Es entsteht wieder der ursprüngliche Alkohol", "Es entsteht ein Edelgas" }, "Es entsteht eine Carbonsäure (Alkansäure)",
            "Wird ein Aldehyd weiter oxidiert, entsteht daraus eine Carbonsäure, z.B. aus Ethanal entsteht Essigsäure (Ethansäure)."),
        ("Warum verdunstet Ethanol bei Raumtemperatur schneller als Wasser?", new[] { "Ethanol hat schwächere zwischenmolekulare Kräfte als Wasser und dadurch einen niedrigeren Siedepunkt", "Ethanol hat stärkere zwischenmolekulare Kräfte als Wasser", "Beide Stoffe verdunsten exakt gleich schnell" }, "Ethanol hat schwächere zwischenmolekulare Kräfte als Wasser und dadurch einen niedrigeren Siedepunkt",
            "Ethanol besitzt insgesamt schwächere zwischenmolekulare Anziehungskräfte als Wasser, wodurch es einen niedrigeren Siedepunkt hat und schneller verdunstet."),
        ("Was zeigt die Löslichkeit von Ethanol sowohl in Wasser als auch in unpolaren Stoffen wie Ölen?", new[] { "Ethanol besitzt sowohl einen polaren (Hydroxy-Gruppe) als auch einen unpolaren Molekülteil (Kohlenwasserstoffkette)", "Ethanol besteht ausschließlich aus einem einzigen, völlig unpolaren Molekülteil", "Löslichkeit hat mit dem Molekülbau überhaupt nichts zu tun" }, "Ethanol besitzt sowohl einen polaren (Hydroxy-Gruppe) als auch einen unpolaren Molekülteil (Kohlenwasserstoffkette)",
            "Da Ethanol sowohl eine polare Hydroxy-Gruppe als auch einen unpolaren Kohlenwasserstoffrest besitzt, kann es sowohl mit polaren (Wasser) als auch unpolaren Stoffen wechselwirken."),
        ("Was ist ein Grund, warum Alkoholkonsum den Körper beeinträchtigt?", new[] { "Ethanol wirkt auf das zentrale Nervensystem und beeinflusst dort die Signalübertragung", "Ethanol hat auf den Körper überhaupt keine erkennbare Wirkung", "Ethanol wird vom Körper vollständig ignoriert und gar nicht abgebaut, was einer genaueren Pruefung nicht standhaelt" }, "Ethanol wirkt auf das zentrale Nervensystem und beeinflusst dort die Signalübertragung",
            "Ethanol beeinflusst als psychoaktive Substanz die Signalübertragung im zentralen Nervensystem, was Reaktionsfähigkeit und Urteilsvermögen einschränken kann."),
        ("Was passiert im Körper beim Abbau von Ethanol, das zunächst über die Leber verarbeitet wird?", new[] { "Ethanol wird schrittweise über Acetaldehyd zu Essigsäure oxidiert", "Ethanol wird ohne jede chemische Veränderung ausgeatmet, obwohl das auf den ersten Blick plausibel klingt", "Ethanol wandelt sich direkt in Glucose um" }, "Ethanol wird schrittweise über Acetaldehyd zu Essigsäure oxidiert",
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
        ("Welche funktionelle Gruppe kennzeichnet organische Säuren (Alkansäuren)?", new[] { "Die Carboxy-Gruppe (-COOH)", "Die Hydroxy-Gruppe, was die eigentliche Bedeutung des Begriffs verfehlt", "Die Estergruppe" }, "Die Carboxy-Gruppe (-COOH)",
            "Carbonsäuren (Alkansäuren) besitzen als charakteristische funktionelle Gruppe die Carboxy-Gruppe (-COOH)."),
        ("Was ist die im Essig enthaltene organische Säure?", new[] { "Essigsäure (Ethansäure)", "Salzsäure", "Schwefelsäure" }, "Essigsäure (Ethansäure)",
            "Essig enthält verdünnte Essigsäure (Ethansäure, CH₃COOH), eine typische Carbonsäure."),
        ("Wie verändert sich die Säurestärke organischer Carbonsäuren im Vergleich zu vielen anorganischen Säuren wie Salzsäure?", new[] { "Carbonsäuren sind meist schwächer und dissoziieren in Wasser nur teilweise", "Carbonsäuren sind grundsätzlich stärker als jede anorganische Säure", "Beide Säuretypen sind chemisch exakt identisch" }, "Carbonsäuren sind meist schwächer und dissoziieren in Wasser nur teilweise",
            "Organische Carbonsäuren wie Essigsäure gelten meist als schwächere Säuren, da sie in Wasser nur teilweise in Ionen dissoziieren, im Gegensatz zu starken Säuren wie Salzsäure."),
        ("Was passiert innerhalb der homologen Reihe der Alkansäuren mit zunehmender Kettenlänge, ähnlich wie bei Alkanen?", new[] { "Der Schmelz- und Siedepunkt steigt tendenziell an", "Der Schmelz- und Siedepunkt sinkt immer weiter", "Die Kettenlänge hat keinerlei Einfluss auf diese Eigenschaften" }, "Der Schmelz- und Siedepunkt steigt tendenziell an",
            "Wie bei anderen homologen Reihen nehmen auch bei den Alkansäuren mit wachsender Kettenlänge die zwischenmolekularen Kräfte und damit meist Schmelz- und Siedepunkt zu."),
        ("Was sind Aminosäuren im Vergleich zu einfachen Carbonsäuren?", new[] { "Moleküle, die sowohl eine Carboxy- als auch eine Amino-Gruppe besitzen", "Moleküle, die ausschließlich aus Kohlenwasserstoffketten bestehen", "Ein anderes Wort für Alkansäuren allgemein" }, "Moleküle, die sowohl eine Carboxy- als auch eine Amino-Gruppe besitzen",
            "Aminosäuren besitzen neben der Carboxy-Gruppe zusätzlich eine Amino-Gruppe (-NH₂) und sind die Bausteine von Proteinen."),
        ("Warum gelten Aminosäuren als wichtige Bausteine des Lebens?", new[] { "Sie verbinden sich zu Proteinen, die zentrale Funktionen im Körper übernehmen", "Sie haben mit Proteinen überhaupt nichts zu tun und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Sie kommen ausschließlich in Mineralien vor" }, "Sie verbinden sich zu Proteinen, die zentrale Funktionen im Körper übernehmen",
            "Aminosäuren verknüpfen sich zu Ketten (Proteinen), die im Körper u.a. Strukturen bilden, Stoffe transportieren und als Enzyme Reaktionen ermöglichen."),
        ("Was ist ein Vorteil von Essigsäure als Haushaltsreiniger gegenüber sehr starken anorganischen Säuren?", new[] { "Sie ist bei richtiger Anwendung weniger aggressiv und leichter biologisch abbaubar", "Essigsäure hat keinerlei reinigende Wirkung", "Essigsäure ist grundsätzlich gefährlicher als starke Mineralsäuren - eine haeufige, aber unzutreffende Vorstellung" }, "Sie ist bei richtiger Anwendung weniger aggressiv und leichter biologisch abbaubar",
            "Essigsäure gilt bei sachgemäßer Verdünnung als vergleichsweise milde und gut abbaubare Alternative zu aggressiveren, stark ätzenden Reinigungssäuren."),
        ("Was zeigt die Struktur der Carboxy-Gruppe (-COOH) chemisch?", new[] { "Eine Kombination aus einer Carbonylgruppe (C=O) und einer Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom", "Ausschließlich eine einfache Kohlenstoff-Wasserstoff-Bindung, auch wenn das manche zunaechst vermuten wuerden", "Eine reine Metall-Sauerstoff-Verbindung" }, "Eine Kombination aus einer Carbonylgruppe (C=O) und einer Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom",
            "Die Carboxy-Gruppe vereint strukturell eine Carbonyl- (C=O) und eine Hydroxy-Gruppe (-OH) am selben Kohlenstoffatom, was ihre saure Wirkung erklärt."),
        ("Warum kann eine Carbonsäure in Wasser Protonen abgeben und damit als Säure wirken?", new[] { "Das Wasserstoffatom der -OH-Gruppe in der Carboxy-Gruppe kann als Proton abgespalten werden", "Carbonsäuren besitzen überhaupt kein abspaltbares Wasserstoffatom", "Nur anorganische Säuren können Protonen abgeben" }, "Das Wasserstoffatom der -OH-Gruppe in der Carboxy-Gruppe kann als Proton abgespalten werden",
            "Das Wasserstoffatom der Carboxy-Gruppe ist relativ leicht als Proton (H⁺) abspaltbar, wodurch Carbonsäuren in Wasser sauer reagieren."),
        ("Was ist Zitronensäure ein bekanntes Beispiel für?", new[] { "Eine natürlich vorkommende organische Säure in Früchten wie Zitronen", "Eine anorganische Säure aus dem Labor, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Ein Metall aus dem Periodensystem" }, "Eine natürlich vorkommende organische Säure in Früchten wie Zitronen",
            "Zitronensäure ist eine natürlich in Zitrusfrüchten vorkommende Carbonsäure, die z.B. für deren sauren Geschmack verantwortlich ist."),
        ("Was ist Ameisensäure (Methansäure), die einfachste Carbonsäure, ein Beispiel für?", new[] { "Eine Säure, die auch natürlich z.B. in manchen Insektenstichen vorkommt", "Ein Edelmetall", "Ein reines Edelgas - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt" }, "Eine Säure, die auch natürlich z.B. in manchen Insektenstichen vorkommt",
            "Ameisensäure kommt natürlich vor, u.a. im Gift mancher Ameisen und Brennnesseln, und ist die einfachste Alkansäure."),
        ("Was passiert grundsätzlich beim Vergleich der Säurestärke innerhalb der homologen Reihe der Alkansäuren mit zunehmender Kettenlänge?", new[] { "Die Säurestärke nimmt tendenziell leicht ab", "Die Säurestärke nimmt immer stark zu", "Die Kettenlänge hat überhaupt keinen Einfluss auf die Säurestärke" }, "Die Säurestärke nimmt tendenziell leicht ab",
            "Mit zunehmender Kettenlänge nimmt der elektronenschiebende Effekt der Kohlenwasserstoffkette zu, was die Säurestärke der Carbonsäure tendenziell leicht verringert."),
        ("Was unterscheidet organische Carbonsäuren strukturell grundlegend von anorganischen Säuren wie Salzsäure?", new[] { "Organische Säuren enthalten eine Kohlenstoffkette mit funktioneller Carboxy-Gruppe, anorganische Säuren nicht", "Beide Säuretypen besitzen exakt dieselbe chemische Struktur, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Anorganische Säuren enthalten immer eine Carboxy-Gruppe" }, "Organische Säuren enthalten eine Kohlenstoffkette mit funktioneller Carboxy-Gruppe, anorganische Säuren nicht",
            "Organische Säuren basieren auf einem Kohlenwasserstoffgerüst mit angehängter Carboxy-Gruppe, während anorganische Säuren wie Salzsäure kein Kohlenstoffgerüst besitzen."),
        ("Wofür wird Essigsäure im Haushalt neben der Verwendung als Speisezutat noch häufig eingesetzt?", new[] { "Zum Entkalken von Geräten wie Wasserkochern oder Kaffeemaschinen", "Ausschließlich zum Färben von Textilien", "Ausschließlich zur Metallverarbeitung in der Industrie" }, "Zum Entkalken von Geräten wie Wasserkochern oder Kaffeemaschinen",
            "Essigsäure wird im Haushalt häufig zum Entkalken genutzt, da sie mit Kalkablagerungen (Carbonaten) reagiert und diese auflöst."),
        ("Was passiert, wenn zwei Aminosäuren unter Wasserabspaltung miteinander verbunden werden?", new[] { "Es entsteht eine Peptidbindung zwischen ihnen", "Es entsteht eine Esterbindung wie bei Fetten", "Es findet keinerlei chemische Reaktion statt" }, "Es entsteht eine Peptidbindung zwischen ihnen",
            "Aminosäuren verknüpfen sich unter Wasserabspaltung über eine sogenannte Peptidbindung zu längeren Ketten, den Proteinen."),
        ("Warum ist die Umweltverträglichkeit ein wichtiges Kriterium beim Vergleich verschiedener Haushaltsreiniger?", new[] { "Manche Reinigungsmittel können, wenn sie ins Abwasser gelangen, Gewässer oder Böden belasten", "Umweltverträglichkeit spielt bei Reinigungsmitteln überhaupt keine Rolle", "Alle Haushaltsreiniger sind grundsätzlich völlig umweltneutral" }, "Manche Reinigungsmittel können, wenn sie ins Abwasser gelangen, Gewässer oder Böden belasten",
            "Nicht biologisch abbaubare oder besonders aggressive Reinigungsmittel können bei falscher Entsorgung Gewässer oder Böden belasten - deshalb wird ihre Umweltverträglichkeit bewertet."),
        ("Was ist ein Grund, warum Milchsäure beim Sport in den Muskeln entstehen kann?", new[] { "Bei intensiver Belastung wird Energie teilweise ohne ausreichend Sauerstoff gewonnen, wobei Milchsäure entsteht", "Milchsäure hat mit sportlicher Belastung überhaupt nichts zu tun und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Milchsäure entsteht ausschließlich beim Schlafen" }, "Bei intensiver Belastung wird Energie teilweise ohne ausreichend Sauerstoff gewonnen, wobei Milchsäure entsteht",
            "Bei hoher körperlicher Belastung mit unzureichender Sauerstoffversorgung gewinnt der Muskel Energie teils anaerob, wobei Milchsäure als Nebenprodukt entsteht."),
        ("Warum sind viele Carbonsäuren mit kurzer Kette wie Essigsäure gut wasserlöslich?", new[] { "Die polare Carboxy-Gruppe kann Wasserstoffbrückenbindungen zu Wassermolekülen bilden", "Kurzkettige Carbonsäuren besitzen keinerlei polare Gruppe", "Wasserlöslichkeit hat mit der Molekülstruktur nichts zu tun - eine haeufige, aber unzutreffende Vorstellung" }, "Die polare Carboxy-Gruppe kann Wasserstoffbrückenbindungen zu Wassermolekülen bilden",
            "Ähnlich wie bei kurzkettigen Alkoholen ermöglicht die polare, wasserstoffbrückenbildende Carboxy-Gruppe eine gute Löslichkeit kurzkettiger Carbonsäuren in Wasser."),
        ("Was passiert grundsätzlich mit der Löslichkeit von Carbonsäuren in Wasser, wenn ihre Kohlenwasserstoffkette immer länger wird?", new[] { "Die Löslichkeit nimmt tendenziell ab, weil der unpolare Kettenanteil überwiegt", "Die Löslichkeit steigt immer weiter an", "Die Kettenlänge hat keinerlei Einfluss auf die Löslichkeit" }, "Die Löslichkeit nimmt tendenziell ab, weil der unpolare Kettenanteil überwiegt",
            "Wie bei den Alkoholen überwiegt bei längeren Ketten zunehmend der unpolare Kohlenwasserstoffanteil, wodurch die Wasserlöslichkeit der Carbonsäure abnimmt."),
        ("Was ist Buttersäure ein bekanntes (wenn auch unangenehm riechendes) Beispiel für?", new[] { "Eine kurzkettige Carbonsäure, die z.B. beim Ranzigwerden von Butter entsteht", "Ein Edelmetall", "Ein reines Edelgas ohne jeden Eigengeruch, auch wenn das manche zunaechst vermuten wuerden" }, "Eine kurzkettige Carbonsäure, die z.B. beim Ranzigwerden von Butter entsteht",
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
        ("Was entsteht chemisch, wenn eine Carbonsäure mit einem Alkohol reagiert?", new[] { "Ein Ester und Wasser (Kondensationsreaktion)", "Ausschließlich ein neues Salz, was bei genauerem Hinsehen nicht stimmt", "Ein reines Edelgas" }, "Ein Ester und Wasser (Kondensationsreaktion)",
            "Bei der Veresterung reagieren Carbonsäure und Alkohol unter Wasserabspaltung (Kondensation) zu einem Ester."),
        ("Was kennzeichnet die Estergruppe strukturell?", new[] { "Eine C(=O)-O-Bindung zwischen dem Säure- und dem Alkoholteil des Moleküls", "Eine reine Kohlenstoff-Wasserstoff-Bindung ohne Sauerstoff", "Eine Bindung, die ausschließlich aus Stickstoffatomen besteht (was so in der Praxis nicht zutrifft)" }, "Eine C(=O)-O-Bindung zwischen dem Säure- und dem Alkoholteil des Moleküls",
            "Die Estergruppe entsteht durch die Verbindung des Carbonylkohlenstoffs der Säure mit dem Sauerstoffatom des Alkohols."),
        ("Was ist Hydrolyse eines Esters?", new[] { "Die Rückreaktion, bei der ein Ester unter Wasseraufnahme wieder in Säure und Alkohol gespalten wird", "Die erstmalige Bildung eines Esters aus Säure und Alkohol", "Ein anderes Wort für eine Neutralisation" }, "Die Rückreaktion, bei der ein Ester unter Wasseraufnahme wieder in Säure und Alkohol gespalten wird",
            "Bei der Hydrolyse wird ein Ester unter Aufnahme von Wasser wieder in seine ursprüngliche Säure und den ursprünglichen Alkohol gespalten - die Umkehrung der Veresterung."),
        ("Warum gelten Veresterung und Hydrolyse als Beispiele für umkehrbare (reversible) Reaktionen?", new[] { "Je nach Bedingungen kann die Reaktion in beide Richtungen ablaufen (Ester bilden oder wieder spalten)", "Die Reaktion kann grundsätzlich nur in eine einzige Richtung ablaufen - eine verbreitete, aber falsche Annahme", "Umkehrbarkeit hat mit dieser Reaktion nichts zu tun" }, "Je nach Bedingungen kann die Reaktion in beide Richtungen ablaufen (Ester bilden oder wieder spalten)",
            "Veresterung (Kondensation) und Hydrolyse sind Hin- und Rückreaktion desselben Gleichgewichts - je nach Bedingungen (z.B. Wasserüberschuss) verschiebt sich das Gleichgewicht in die eine oder andere Richtung."),
        ("Was bewirkt eine saure Katalyse bei der Veresterung?", new[] { "Sie beschleunigt die Reaktion, ohne selbst verbraucht zu werden", "Sie verlangsamt die Reaktion drastisch", "Sie verändert das Endprodukt der Reaktion vollständig" }, "Sie beschleunigt die Reaktion, ohne selbst verbraucht zu werden",
            "Ein saurer Katalysator (z.B. Schwefelsäure) beschleunigt die Veresterungsreaktion, indem er den Reaktionsmechanismus erleichtert, wird dabei aber selbst nicht verbraucht."),
        ("Warum riechen viele Fruchtester (z.B. in Fruchtaromen) angenehm fruchtig?", new[] { "Bestimmte kleine Estermoleküle sind für charakteristische Fruchtdüfte verantwortlich", "Ester haben grundsätzlich überhaupt keinen Eigengeruch", "Fruchtgeruch hat mit der chemischen Struktur nichts zu tun" }, "Bestimmte kleine Estermoleküle sind für charakteristische Fruchtdüfte verantwortlich",
            "Viele natürliche und künstliche Fruchtaromen bestehen aus spezifischen Estern, die für den charakteristischen, oft fruchtigen Geruch verantwortlich sind."),
        ("Was sind Fette chemisch betrachtet größtenteils?", new[] { "Ester aus Glycerin und langkettigen Fettsäuren", "Reine Kohlenwasserstoffe ohne Sauerstoffanteil", "Anorganische Salze" }, "Ester aus Glycerin und langkettigen Fettsäuren",
            "Fette sind Ester, die aus dem dreiwertigen Alkohol Glycerin und drei langkettigen Fettsäuren aufgebaut sind (Triglyceride)."),
        ("Was passiert bei der Verseifung (alkalischen Hydrolyse) eines Fettes?", new[] { "Das Fett wird in Glycerin und die Salze der Fettsäuren (Seifen) gespalten", "Das Fett bleibt dabei chemisch völlig unverändert", "Es entsteht ausschließlich reines Wasser ohne weitere Produkte, was einer genaueren Pruefung nicht standhaelt" }, "Das Fett wird in Glycerin und die Salze der Fettsäuren (Seifen) gespalten",
            "Bei der Verseifung wird ein Fett mit einer Lauge zu Glycerin und den Natrium- oder Kaliumsalzen der Fettsäuren (Seifen) gespalten."),
        ("Warum können Seifenmoleküle sowohl Fett als auch Wasser \"verbinden\" und dadurch reinigen?", new[] { "Sie besitzen einen wasserabweisenden (lipophilen) und einen wasseranziehenden (hydrophilen) Molekülteil", "Seifenmoleküle bestehen ausschließlich aus einem einzigen, komplett unpolaren Teil, obwohl das auf den ersten Blick plausibel klingt", "Seifenmoleküle haben mit Wasser oder Fett überhaupt keine Wechselwirkung" }, "Sie besitzen einen wasserabweisenden (lipophilen) und einen wasseranziehenden (hydrophilen) Molekülteil",
            "Der lipophile (fettliebende) Teil der Seife lagert sich an Fett an, der hydrophile (wasserliebende) Teil bleibt zum Wasser hin ausgerichtet - so können Fetttröpfchen im Wasser \"eingeschlossen\" und weggespült werden."),
        ("Was bedeutet \"lipophil\" bei einem Molekülteil?", new[] { "Fettliebend, gut in Fetten löslich", "Wasseranziehend", "Ein anderes Wort für Carbonsäure, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Fettliebend, gut in Fetten löslich",
            "Lipophile (\"fettliebende\") Molekülteile lösen sich gut in unpolaren, fettähnlichen Substanzen."),
        ("Was bedeutet \"lipophob\" bei einem Molekülteil?", new[] { "Fettabstoßend, schlecht in Fetten löslich", "Fettliebend", "Ein anderes Wort für Ester" }, "Fettabstoßend, schlecht in Fetten löslich",
            "Lipophobe (\"fettmeidende\") Molekülteile mischen sich schlecht mit fettähnlichen, unpolaren Substanzen."),
        ("Was ist ein typisches Beispiel für einen Ester, der als künstliches Aroma in Lebensmitteln verwendet wird?", new[] { "Ein Fruchtester wie Ethylbutanoat (nach Ananas riechend)", "Kochsalz", "Reines Ethanol ohne Säureanteil und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Ein Fruchtester wie Ethylbutanoat (nach Ananas riechend)",
            "Bestimmte kleine Ester wie Ethylbutanoat erinnern geruchlich an Früchte wie Ananas und werden deshalb als Aromastoffe eingesetzt."),
        ("Warum verschiebt ein Überschuss an Wasser das Gleichgewicht einer Veresterungsreaktion tendenziell in Richtung Hydrolyse?", new[] { "Nach dem Prinzip von Le Chatelier begünstigt mehr Wasser die Rückreaktion, bei der Wasser verbraucht wird", "Wasserüberschuss hat auf ein chemisches Gleichgewicht überhaupt keinen Einfluss", "Wasserüberschuss verschiebt jedes Gleichgewicht ausschließlich in Richtung Ester" }, "Nach dem Prinzip von Le Chatelier begünstigt mehr Wasser die Rückreaktion, bei der Wasser verbraucht wird",
            "Nach dem Prinzip des kleinsten Zwangs (Le Chatelier) verschiebt ein Überschuss an einem Reaktionspartner (hier Wasser) das Gleichgewicht in die Richtung, in der dieser Stoff verbraucht wird - also zur Hydrolyse."),
        ("Was unterscheidet ein Fett grundlegend von einem Öl bei Raumtemperatur?", new[] { "Fette sind bei Raumtemperatur meist fest, Öle meist flüssig", "Fette und Öle sind chemisch völlig identisch und unterscheiden sich in nichts", "Nur Öle bestehen aus Estern, Fette hingegen nicht" }, "Fette sind bei Raumtemperatur meist fest, Öle meist flüssig",
            "Der Unterschied zwischen Fetten und Ölen liegt meist im Aggregatzustand bei Raumtemperatur, der u.a. vom Sättigungsgrad der enthaltenen Fettsäuren abhängt."),
        ("Was passiert mit dem Schmelzpunkt eines Fettes, wenn es viele ungesättigte Fettsäuren (mit Doppelbindungen) enthält?", new[] { "Der Schmelzpunkt sinkt tendenziell, das Fett bleibt eher flüssig (Öl)", "Der Schmelzpunkt steigt immer stark an", "Ungesättigte Fettsäuren haben keinerlei Einfluss auf den Schmelzpunkt" }, "Der Schmelzpunkt sinkt tendenziell, das Fett bleibt eher flüssig (Öl)",
            "Doppelbindungen in ungesättigten Fettsäuren führen zu einer weniger dichten Packung der Moleküle, wodurch der Schmelzpunkt sinkt und das Fett bei Raumtemperatur eher flüssig bleibt."),
        ("Warum wird Seife seit Jahrhunderten zur Reinigung genutzt?", new[] { "Sie kann Fett und Schmutz im Wasser binden und abspülbar machen", "Seife hat historisch nie eine reinigende Wirkung gehabt", "Seife wirkt ausschließlich als Duftstoff ohne jede Reinigungsfunktion" }, "Sie kann Fett und Schmutz im Wasser binden und abspülbar machen",
            "Aufgrund ihres lipophilen und hydrophilen Molekülteils kann Seife Fett- und Schmutzpartikel umschließen und im Wasser abtransportierbar machen - eine Eigenschaft, die seit Jahrhunderten genutzt wird."),
        ("Was ist der Unterschied zwischen einer Kondensationsreaktion (Veresterung) und einer einfachen Additionsreaktion?", new[] { "Bei der Kondensation wird zusätzlich ein kleines Molekül (meist Wasser) abgespalten", "Bei einer Additionsreaktion wird immer Wasser freigesetzt - eine haeufige, aber unzutreffende Vorstellung", "Beide Reaktionstypen laufen exakt identisch ab" }, "Bei der Kondensation wird zusätzlich ein kleines Molekül (meist Wasser) abgespalten",
            "Kondensationsreaktionen wie die Veresterung erzeugen neben dem Hauptprodukt zusätzlich ein kleines Molekül (meist Wasser), das abgespalten wird - bei einer reinen Addition passiert das nicht."),
        ("Was zeigt die Vielfalt an Estern in der Natur und Industrie (Aromen, Fette, Kunststoffe)?", new[] { "Aus wenigen Grundbausteinen (Säuren und Alkoholen) lässt sich eine große Produktvielfalt herstellen", "Ester kommen ausschließlich in einer einzigen, immer identischen Form vor", "Ester haben in Natur und Industrie praktisch keine Bedeutung" }, "Aus wenigen Grundbausteinen (Säuren und Alkoholen) lässt sich eine große Produktvielfalt herstellen",
            "Durch Kombination unterschiedlicher Carbonsäuren und Alkohole entsteht eine riesige Vielfalt an Estern mit ganz unterschiedlichen Eigenschaften und Anwendungen."),
        ("Warum werden pflanzliche Öle in der Lebensmittelindustrie manchmal gehärtet (Fetthärtung)?", new[] { "Durch Anlagerung von Wasserstoff an Doppelbindungen werden ungesättigte Fettsäuren gesättigt und das Fett fester", "Härtung entfernt sämtliche Esterbindungen aus dem Öl", "Härtung hat auf den Aggregatzustand des Fetts keinerlei Einfluss, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Durch Anlagerung von Wasserstoff an Doppelbindungen werden ungesättigte Fettsäuren gesättigt und das Fett fester",
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PseGrundlagenListe =
    {
        ("Was ist ein chemisches Element, einfach erklärt?", new[] { "Ein Grundstoff, der sich nicht in einfachere Stoffe zerlegen lässt", "Eine Mischung aus mehreren verschiedenen Stoffen (was so in der Praxis nicht zutrifft)", "Ein anderes Wort für chemische Reaktion" }, "Ein Grundstoff, der sich nicht in einfachere Stoffe zerlegen lässt",
            "Ein chemisches Element besteht immer nur aus einer einzigen Atomsorte und lässt sich chemisch nicht weiter zerlegen."),
        ("Wozu dient das Periodensystem der Elemente?", new[] { "Es ordnet alle bekannten chemischen Elemente übersichtlich in einer Tabelle", "Es zeigt nur die Namen der Chemiker, die Elemente entdeckt haben", "Es listet ausschließlich giftige Stoffe auf" }, "Es ordnet alle bekannten chemischen Elemente übersichtlich in einer Tabelle",
            "Das Periodensystem ist eine geordnete Übersichtstabelle, in der jedes chemische Element seinen festen Platz hat."),
        ("Welches chemische Symbol steht für Sauerstoff?", new[] { "O", "S", "Su" }, "O",
            "Sauerstoff wird im Periodensystem mit dem Symbol \"O\" (vom lateinischen \"Oxygenium\") abgekürzt."),
        ("Welches chemische Symbol steht für Wasserstoff?", new[] { "H", "W", "Wa" }, "H",
            "Wasserstoff trägt das Symbol \"H\" (vom lateinischen \"Hydrogenium\")."),
        ("Welches chemische Symbol steht für Eisen?", new[] { "Fe", "Ei", "E" }, "Fe",
            "Eisen wird mit \"Fe\" abgekürzt, abgeleitet vom lateinischen Wort \"Ferrum\"."),
        ("Welches chemische Symbol steht für Gold?", new[] { "Au", "Go", "G" }, "Au",
            "Gold trägt das Symbol \"Au\", vom lateinischen \"Aurum\"."),
        ("Welches chemische Symbol steht für Kohlenstoff?", new[] { "C", "K", "Ko" }, "C",
            "Kohlenstoff wird im Periodensystem mit \"C\" abgekürzt (vom lateinischen \"Carboneum\")."),
        ("Was zeigt das chemische Symbol eines Elements meist an?", new[] { "Eine kurze, international einheitliche Abkürzung für den Elementnamen", "Die Farbe, die das Element immer hat", "Wie gefährlich ein Element ist" }, "Eine kurze, international einheitliche Abkürzung für den Elementnamen",
            "Chemische Symbole wie \"O\" oder \"Fe\" sind weltweit einheitlich und werden unabhängig von der jeweiligen Landessprache verwendet."),
        ("In welche zwei großen Stoffgruppen lassen sich die Elemente im Periodensystem grob einteilen?", new[] { "Metalle und Nichtmetalle", "Flüssigkeiten und Feststoffe", "Gifte und Nahrungsmittel" }, "Metalle und Nichtmetalle",
            "Die Elemente des Periodensystems lassen sich grob in Metalle (z.B. Eisen) und Nichtmetalle (z.B. Sauerstoff) einteilen."),
        ("Welches der folgenden Elemente ist ein Metall?", new[] { "Eisen", "Sauerstoff", "Kohlenstoff" }, "Eisen",
            "Eisen ist ein typisches Metall: glänzend, verformbar und ein guter Leiter für Strom und Wärme."),
        ("Welches der folgenden Elemente ist ein Nichtmetall?", new[] { "Sauerstoff", "Eisen - eine verbreitete, aber falsche Annahme", "Gold" }, "Sauerstoff",
            "Sauerstoff ist bei Zimmertemperatur ein farbloses Gas und zählt zu den Nichtmetallen."),
        ("Warum ist es praktisch, dass alle bekannten Elemente in einer einzigen Tabelle stehen?", new[] { "Man findet Informationen zu jedem Element schnell an einem festen Platz", "Damit spart man sich das Auswendiglernen komplett", "Damit müssen Chemikerinnen und Chemiker keine Experimente mehr machen, was einer genaueren Pruefung nicht standhaelt" }, "Man findet Informationen zu jedem Element schnell an einem festen Platz",
            "Das Periodensystem dient als Nachschlagewerk: Jedes Element hat einen festen Platz, an dem man wichtige Grundinformationen findet."),
        ("Wie viele verschiedene chemische Elemente sind heute ungefähr bekannt?", new[] { "Etwas mehr als 100", "Etwa 20", "Über 10.000" }, "Etwas mehr als 100",
            "Bisher sind etwas mehr als 100 verschiedene chemische Elemente bekannt und im Periodensystem eingetragen."),
        ("Welches Element atmen Menschen zum Leben aus der Luft ein?", new[] { "Sauerstoff", "Kohlenstoff", "Eisen" }, "Sauerstoff",
            "Sauerstoff wird beim Atmen aufgenommen und ist für die Energiegewinnung im Körper lebensnotwendig."),
        ("Welches Gas macht den größten Anteil unserer Atemluft aus?", new[] { "Stickstoff", "Sauerstoff", "Kohlenstoffdioxid" }, "Stickstoff",
            "Rund 78% der Luft, die wir atmen, bestehen aus Stickstoff - nur etwa 21% sind Sauerstoff."),
        ("Ist Kohlenstoff (C) ein Metall oder ein Nichtmetall?", new[] { "Ein Nichtmetall", "Ein Metall", "Weder noch, es ist eine Mischung" }, "Ein Nichtmetall",
            "Kohlenstoff zählt zu den Nichtmetallen, obwohl er z.B. als Diamant sehr hart sein kann."),
        ("Woran erkennt man im Periodensystem grob, ob ein Element eher ein Metall ist?", new[] { "Es steht meist im linken oder mittleren Bereich der Tabelle", "Es steht immer ganz oben rechts", "Metalle sind alphabetisch sortiert" }, "Es steht meist im linken oder mittleren Bereich der Tabelle",
            "Die meisten Metalle befinden sich im linken und mittleren Bereich des Periodensystems, die meisten Nichtmetalle eher rechts."),
        ("Welches Edelmetall mit dem Symbol \"Au\" wird oft für Schmuck verwendet?", new[] { "Gold", "Silber", "Eisen" }, "Gold",
            "Gold trägt das Symbol \"Au\" und wird wegen seines Glanzes und seiner Beständigkeit häufig für Schmuck genutzt."),
        ("Was haben alle Atome desselben Elements, z.B. alle Sauerstoff-Atome, gemeinsam?", new[] { "Sie gehören zur selben Atomsorte mit denselben Grundeigenschaften", "Sie haben zufällig völlig unterschiedliche Eigenschaften", "Sie kommen nur einmal auf der ganzen Erde vor" }, "Sie gehören zur selben Atomsorte mit denselben Grundeigenschaften",
            "Alle Atome eines Elements sind vom selben Grundtyp und zeigen deshalb dieselben charakteristischen chemischen Eigenschaften."),
        ("Was ist ein Beispiel für ein Element, das bei Zimmertemperatur ein Gas ist?", new[] { "Sauerstoff", "Eisen", "Gold" }, "Sauerstoff",
            "Sauerstoff liegt bei normaler Zimmertemperatur gasförmig vor, anders als feste Metalle wie Eisen oder Gold.")
    };

    private static QuizQuestion PeriodensystemGrundlagen(Random r)
    {
        var f = PseGrundlagenListe[r.Next(PseGrundlagenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Das Periodensystem der Elemente – Übersicht und Werkzeug", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Das Periodensystem ordnet alle Elemente in einer Tabelle - jedes hat ein eigenes Symbol (z.B. O, H, Fe) und ist entweder Metall oder Nichtmetall."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GaseListe =
    {
        ("Welches Gas macht den größten Teil unserer Atemluft aus?", new[] { "Stickstoff", "Sauerstoff", "Kohlenstoffdioxid" }, "Stickstoff",
            "Etwa 78% der Luft bestehen aus Stickstoff, nur rund 21% sind Sauerstoff."),
        ("Welches Gas brauchen Menschen und Tiere zum Atmen?", new[] { "Sauerstoff", "Stickstoff", "Kohlenstoffmonoxid" }, "Sauerstoff",
            "Der Körper braucht Sauerstoff, um aus Nahrung Energie zu gewinnen - deshalb muss er ständig eingeatmet werden."),
        ("Welches Gas atmen wir beim Ausatmen vermehrt wieder aus?", new[] { "Kohlenstoffdioxid (CO₂)", "Sauerstoff, obwohl das auf den ersten Blick plausibel klingt", "Helium" }, "Kohlenstoffdioxid (CO₂)",
            "Beim Stoffwechsel entsteht CO₂ als Abfallprodukt, das der Körper über die Ausatmung loswird."),
        ("Warum ist Kohlenstoffmonoxid (CO) besonders gefährlich?", new[] { "Es ist farb- und geruchlos und kann unbemerkt zum Ersticken führen", "Es riecht sehr stark nach faulen Eiern", "Es ist ungefährlich und überall harmlos, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Es ist farb- und geruchlos und kann unbemerkt zum Ersticken führen",
            "Da man CO weder sehen noch riechen kann, bemerken Menschen die Vergiftung oft zu spät - deshalb sind CO-Melder wichtig."),
        ("Was passiert mit einem Gas normalerweise, wenn man es erhitzt?", new[] { "Es dehnt sich aus (das Volumen wird größer)", "Es wird automatisch zu einer Flüssigkeit", "Es verschwindet spurlos" }, "Es dehnt sich aus (das Volumen wird größer)",
            "Wird ein Gas erwärmt, bewegen sich seine Teilchen schneller und beanspruchen mehr Raum - es dehnt sich aus."),
        ("Was passiert mit einem Gas, wenn man es abkühlt?", new[] { "Es zieht sich zusammen (das Volumen wird kleiner)", "Es dehnt sich weiter aus", "Es wird sofort zu einem Feststoff" }, "Es zieht sich zusammen (das Volumen wird kleiner)",
            "Beim Abkühlen bewegen sich die Gasteilchen langsamer, wodurch das Gas ein kleineres Volumen einnimmt."),
        ("Hat ein Gas eine feste, eigene Form?", new[] { "Nein, es füllt jeden verfügbaren Raum vollständig aus", "Ja, jedes Gas hat eine feste Form wie ein Feststoff und deshalb hier nicht zutrifft", "Nur bei sehr niedrigen Temperaturen" }, "Nein, es füllt jeden verfügbaren Raum vollständig aus",
            "Anders als Feststoffe haben Gase keine eigene Form - sie verteilen sich in jedem Behälter oder Raum, den sie füllen können."),
        ("Welches Gas wird oft zum Füllen von Luftballons benutzt, damit sie nach oben steigen?", new[] { "Helium", "Sauerstoff", "Kohlenstoffdioxid" }, "Helium",
            "Helium ist leichter als normale Luft, weshalb damit gefüllte Ballons nach oben aufsteigen."),
        ("Wofür wird Erdgas hauptsächlich genutzt?", new[] { "Als Brennstoff zum Heizen und Kochen", "Nur zum Aufblasen von Reifen", "Ausschließlich in der Medizin, was so nicht korrekt ist" }, "Als Brennstoff zum Heizen und Kochen",
            "Erdgas wird verbrannt, um Wärme zum Heizen von Wohnungen oder zum Kochen zu erzeugen."),
        ("Welches Gas entsteht bei fast jeder Verbrennung, z.B. von Holz oder Benzin?", new[] { "Kohlenstoffdioxid (CO₂)", "Helium", "Wasserstoff - eine haeufige, aber unzutreffende Vorstellung" }, "Kohlenstoffdioxid (CO₂)",
            "Bei Verbrennungsvorgängen verbindet sich der Kohlenstoff des brennenden Stoffs mit Sauerstoff zu CO₂."),
        ("Warum gilt Kohlenstoffdioxid (CO₂) als sogenanntes \"Treibhausgas\"?", new[] { "Es trägt zur Erwärmung der Erdatmosphäre bei", "Es kühlt die Erdatmosphäre stark ab", "Es hat gar keinen Einfluss auf das Klima" }, "Es trägt zur Erwärmung der Erdatmosphäre bei",
            "CO₂ hält Wärme in der Atmosphäre zurück - je mehr davon in der Luft ist, desto stärker heizt sich die Erde auf."),
        ("Welche Eigenschaft haben Edelgase wie Helium oder Neon typischerweise?", new[] { "Sie reagieren kaum mit anderen Stoffen", "Sie reagieren extrem heftig mit fast allem", "Sie sind bei Zimmertemperatur immer flüssig" }, "Sie reagieren kaum mit anderen Stoffen",
            "Edelgase gelten als besonders reaktionsträge - sie gehen fast keine chemischen Verbindungen mit anderen Stoffen ein."),
        ("Warum kann man ausströmendes Erdgas riechen, obwohl es eigentlich geruchlos ist?", new[] { "Es wird absichtlich mit einem übelriechenden Stoff versetzt, damit man Lecks bemerkt", "Erdgas riecht von Natur aus immer stark", "Der Geruch entsteht zufällig beim Transport" }, "Es wird absichtlich mit einem übelriechenden Stoff versetzt, damit man Lecks bemerkt",
            "Damit Menschen ein Gasleck sofort bemerken, wird dem geruchlosen Erdgas gezielt ein auffälliger Warngeruch beigemischt."),
        ("Warum ist es gefährlich, in einem geschlossenen Raum einen Kohlegrill zu betreiben?", new[] { "Es kann sich giftiges, geruchloses Kohlenstoffmonoxid ansammeln", "Der Rauch färbt nur die Wände, auch wenn das manche zunaechst vermuten wuerden", "Es besteht keinerlei Gefahr" }, "Es kann sich giftiges, geruchloses Kohlenstoffmonoxid ansammeln",
            "Ohne ausreichend Sauerstoff entsteht bei der Verbrennung gefährliches Kohlenstoffmonoxid, das sich in geschlossenen Räumen gefährlich ansammeln kann."),
        ("Wozu dient ein CO-Melder in einer Wohnung?", new[] { "Er warnt frühzeitig vor gefährlichem, unsichtbarem Kohlenstoffmonoxid", "Er misst nur die Raumtemperatur", "Er zeigt die Luftfeuchtigkeit an, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Er warnt frühzeitig vor gefährlichem, unsichtbarem Kohlenstoffmonoxid",
            "Ein CO-Melder erkennt das unsichtbare, geruchlose Gas Kohlenstoffmonoxid und warnt rechtzeitig, bevor es gefährlich wird."),
        ("Wie unterscheidet sich das Volumen eines Gases von dem einer Flüssigkeit in einem offenen Gefäß?", new[] { "Gase haben kein festes Volumen und breiten sich aus, Flüssigkeiten behalten ihr Volumen", "Beide verhalten sich in jeder Hinsicht komplett gleich - eine verbreitete, aber falsche Annahme", "Flüssigkeiten breiten sich immer stärker aus als Gase" }, "Gase haben kein festes Volumen und breiten sich aus, Flüssigkeiten behalten ihr Volumen",
            "Ein Gas verteilt sich in jedem verfügbaren Raum, während eine Flüssigkeit ihr Volumen unabhängig vom Gefäß beibehält."),
        ("Warum steigt heiße Luft, z.B. in einem Heißluftballon, nach oben?", new[] { "Erwärmte Luft dehnt sich aus und wird dadurch leichter als die kühlere Umgebungsluft", "Heiße Luft ist immer schwerer als kalte Luft", "Luft verändert sich beim Erwärmen überhaupt nicht, was einer genaueren Pruefung nicht standhaelt" }, "Erwärmte Luft dehnt sich aus und wird dadurch leichter als die kühlere Umgebungsluft",
            "Da erwärmte Luft sich ausdehnt und leichter wird, steigt sie in der kühleren Umgebungsluft nach oben auf."),
        ("Was ist die Ozonschicht und warum ist sie wichtig?", new[] { "Eine Gasschicht hoch in der Atmosphäre, die vor gefährlicher UV-Strahlung schützt", "Ein Gas, das ausschließlich in Fabriken vorkommt, obwohl das auf den ersten Blick plausibel klingt", "Eine Schicht, die die Erde vor Regen schützt" }, "Eine Gasschicht hoch in der Atmosphäre, die vor gefährlicher UV-Strahlung schützt",
            "Die Ozonschicht in der oberen Atmosphäre filtert einen Großteil der schädlichen UV-Strahlung der Sonne heraus."),
        ("Warum sollte man beim Umgang mit Gasflaschen (z.B. für einen Gasgrill) besonders vorsichtig sein?", new[] { "Viele Gase sind brennbar oder stehen unter hohem Druck", "Gasflaschen sind grundsätzlich völlig ungefährlich", "Gasflaschen enthalten nur harmlose Luft" }, "Viele Gase sind brennbar oder stehen unter hohem Druck",
            "Gase in Druckflaschen können bei unsachgemäßem Umgang austreten, sich entzünden oder die Flasche gefährlich werden lassen."),
        ("Warum erlischt eine brennende Kerze, wenn man ein geschlossenes Glas darüberstülpt?", new[] { "Der Sauerstoff im Glas wird verbraucht und es kommt kein Nachschub mehr", "Das Glas kühlt die Flamme sofort auf 0°C ab, was die eigentliche Bedeutung des Begriffs verfehlt", "Glas zieht die Flamme magnetisch an" }, "Der Sauerstoff im Glas wird verbraucht und es kommt kein Nachschub mehr",
            "Eine Flamme braucht ständig Sauerstoff - ist der im abgeschlossenen Glas verbraucht, kann die Verbrennung nicht weitergehen und die Kerze erlischt.")
    };

    private static QuizQuestion Gase(Random r)
    {
        var f = GaseListe[r.Next(GaseListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Gase – zwischen lebensnotwendig und gefährlich", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Sauerstoff zum Atmen lebensnotwendig, Kohlenstoffmonoxid unsichtbar gefährlich; Gase haben kein festes Volumen und dehnen sich beim Erwärmen aus."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WasserListe =
    {
        ("Aus welchen zwei Elementen besteht ein Wassermolekül?", new[] { "Wasserstoff und Sauerstoff", "Sauerstoff und Kohlenstoff", "Stickstoff und Wasserstoff" }, "Wasserstoff und Sauerstoff",
            "Ein Wassermolekül besteht aus zwei Wasserstoff-Atomen und einem Sauerstoff-Atom - deshalb die Formel H₂O."),
        ("Wie lautet die chemische Formel für Wasser?", new[] { "H₂O", "CO₂", "O₂" }, "H₂O",
            "H₂O zeigt: zwei Wasserstoff-Atome (H) sind mit einem Sauerstoff-Atom (O) verbunden."),
        ("In welchen drei Zuständen (Aggregatzuständen) kann Wasser vorkommen?", new[] { "Fest (Eis), flüssig (Wasser), gasförmig (Wasserdampf)", "Nur flüssig und gasförmig", "Nur fest und gasförmig" }, "Fest (Eis), flüssig (Wasser), gasförmig (Wasserdampf)",
            "Wasser kann je nach Temperatur als Eis, als flüssiges Wasser oder als Wasserdampf vorliegen."),
        ("Wie nennt man den Übergang von flüssigem Wasser zu Wasserdampf?", new[] { "Verdunsten (Verdampfen)", "Kondensieren", "Gefrieren" }, "Verdunsten (Verdampfen)",
            "Beim Verdunsten bzw. Verdampfen wird aus flüssigem Wasser gasförmiger Wasserdampf."),
        ("Wie nennt man den Übergang von Wasserdampf zurück zu flüssigem Wasser?", new[] { "Kondensieren", "Verdunsten", "Sublimieren und deshalb hier nicht zutrifft" }, "Kondensieren",
            "Beim Kondensieren wird aus gasförmigem Wasserdampf wieder flüssiges Wasser, z.B. an einem kalten Fenster."),
        ("Was passiert beim Wasserkreislauf, nachdem Wasser aus Meeren und Seen verdunstet ist?", new[] { "Es steigt als Dampf auf, kondensiert zu Wolken und fällt als Niederschlag zurück", "Es verschwindet für immer aus der Atmosphäre", "Es wird sofort zu Eis" }, "Es steigt als Dampf auf, kondensiert zu Wolken und fällt als Niederschlag zurück",
            "Der Wasserkreislauf beschreibt, wie Wasser verdunstet, in der Atmosphäre zu Wolken kondensiert und als Regen wieder zur Erde fällt."),
        ("Warum schwimmt Eis auf flüssigem Wasser?", new[] { "Eis ist leichter (weniger dicht) als flüssiges Wasser", "Eis ist immer schwerer als Wasser", "Eis und Wasser haben exakt dieselbe Dichte" }, "Eis ist leichter (weniger dicht) als flüssiges Wasser",
            "Wasser dehnt sich beim Gefrieren aus und wird dadurch leichter (weniger dicht) - deshalb schwimmt Eis obenauf."),
        ("Bei welcher Temperatur gefriert reines Wasser bei normalem Luftdruck?", new[] { "0°C", "10°C", "-10°C" }, "0°C",
            "Reines Wasser gefriert bei normalem Luftdruck genau bei 0°C zu Eis."),
        ("Bei welcher Temperatur kocht reines Wasser bei normalem Luftdruck?", new[] { "100°C", "50°C", "212°C" }, "100°C",
            "Bei normalem Luftdruck beginnt reines Wasser bei 100°C zu kochen und in Dampf überzugehen."),
        ("Warum wird Wasser oft als \"universelles Lösungsmittel\" bezeichnet?", new[] { "Weil sich sehr viele verschiedene Stoffe darin lösen lassen", "Weil Wasser sich in keinem anderen Stoff lösen lässt", "Weil Wasser niemals mit anderen Stoffen reagiert" }, "Weil sich sehr viele verschiedene Stoffe darin lösen lassen",
            "Wasser kann eine besonders große Vielfalt an Stoffen - z.B. Salze und Zucker - in sich auflösen."),
        ("Was passiert, wenn man Salz in Wasser gibt?", new[] { "Das Salz löst sich auf und verteilt sich unsichtbar im Wasser", "Das Salz schwimmt immer unverändert oben, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Das Wasser verwandelt sich in Salz" }, "Das Salz löst sich auf und verteilt sich unsichtbar im Wasser",
            "Beim Auflösen zerfällt das Salz in winzige, unsichtbare Teilchen, die sich gleichmäßig im Wasser verteilen."),
        ("Warum ist sauberes Trinkwasser für Menschen lebensnotwendig?", new[] { "Der Körper besteht zu einem großen Teil aus Wasser und braucht es für alle Körperfunktionen", "Wasser wird vom Körper überhaupt nicht benötigt, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Nur Pflanzen brauchen Wasser, Menschen nicht" }, "Der Körper besteht zu einem großen Teil aus Wasser und braucht es für alle Körperfunktionen",
            "Der menschliche Körper besteht zu einem großen Teil aus Wasser, das für Verdauung, Blutkreislauf und viele weitere Funktionen nötig ist."),
        ("Was bedeutet der Begriff \"hartes Wasser\"?", new[] { "Wasser mit einem hohen Gehalt an gelösten Mineralien wie Kalk", "Wasser, das komplett gefroren ist", "Wasser, das besonders sauber und mineralfrei ist" }, "Wasser mit einem hohen Gehalt an gelösten Mineralien wie Kalk",
            "\"Hartes\" Wasser enthält besonders viele gelöste Mineralstoffe, vor allem Kalk, was z.B. Kalkablagerungen im Wasserkocher verursacht."),
        ("Wie wird verschmutztes Wasser meist gereinigt, bevor es als Trinkwasser genutzt wird?", new[] { "Durch Filtern und Aufbereiten in einem Wasserwerk", "Es wird einfach kurz umgerührt", "Gar nicht, jedes Wasser ist automatisch sauber (was so in der Praxis nicht zutrifft)" }, "Durch Filtern und Aufbereiten in einem Wasserwerk",
            "In Wasserwerken wird Wasser durch verschiedene Filter- und Reinigungsschritte von Schadstoffen befreit, bevor es als Trinkwasser genutzt wird."),
        ("Warum ist Wasser eine chemische Verbindung und kein reines Element?", new[] { "Weil es aus zwei verschiedenen Elementen (Wasserstoff und Sauerstoff) chemisch verbunden ist", "Weil es aus nur einer einzigen Atomsorte besteht", "Weil man es nicht in andere Stoffe zerlegen kann" }, "Weil es aus zwei verschiedenen Elementen (Wasserstoff und Sauerstoff) chemisch verbunden ist",
            "Eine Verbindung entsteht, wenn sich zwei oder mehr Elemente chemisch verbinden - bei Wasser sind das Wasserstoff und Sauerstoff."),
        ("Wie viel Prozent der Erdoberfläche sind ungefähr mit Wasser bedeckt?", new[] { "Etwa 70%", "Etwa 20%", "Fast 100%" }, "Etwa 70%",
            "Ungefähr 70% der Erdoberfläche sind von Wasser (vor allem Ozeane) bedeckt."),
        ("Wie viel von diesem Wasser auf der Erde ist ungefähr trinkbares Süßwasser?", new[] { "Nur ein kleiner Teil (rund 3%)", "Etwa die Hälfte", "Fast das gesamte Wasser - eine verbreitete, aber falsche Annahme" }, "Nur ein kleiner Teil (rund 3%)",
            "Der weitaus größte Teil des Wassers auf der Erde ist salziges Meerwasser - nur ein kleiner Anteil ist Süßwasser."),
        ("Was kann passieren, wenn Wasser durch bestimmtes Gestein fließt?", new[] { "Es kann Mineralien aus dem Gestein lösen und mit sich tragen", "Wasser verändert sich beim Fließen durch Gestein niemals", "Es verwandelt sich automatisch in Salzwasser" }, "Es kann Mineralien aus dem Gestein lösen und mit sich tragen",
            "Fließt Wasser durch mineralhaltiges Gestein, kann es Mineralstoffe herauslösen und aufnehmen - so entsteht z.B. mineralreiches Quellwasser."),
        ("Warum sollte man mit sauberem Trinkwasser sparsam umgehen?", new[] { "Sauberes Süßwasser ist begrenzt und in vielen Weltregionen knapp", "Wasser ist unbegrenzt und kann nie knapp werden", "Trinkwasser wird künstlich in Fabriken hergestellt" }, "Sauberes Süßwasser ist begrenzt und in vielen Weltregionen knapp",
            "Obwohl die Erde viel Wasser hat, ist sauberes, trinkbares Süßwasser begrenzt und in vielen Regionen der Welt ein knappes Gut."),
        ("Was ist Regen chemisch betrachtet?", new[] { "Kondensiertes Wasser, das aus Wolken als Niederschlag zur Erde fällt", "Ein völlig anderer Stoff als Wasserdampf, was einer genaueren Pruefung nicht standhaelt", "Reiner Wasserstoff ohne Sauerstoff" }, "Kondensiertes Wasser, das aus Wolken als Niederschlag zur Erde fällt",
            "Regen entsteht, wenn in Wolken kondensierter Wasserdampf zu Tropfen wird, die schwer genug sind, um als Niederschlag zu fallen.")
    };

    private static QuizQuestion Wasser(Random r)
    {
        var f = WasserListe[r.Next(WasserListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wasser – eine Verbindung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Wasser (H₂O) besteht aus Wasserstoff und Sauerstoff, kommt fest/flüssig/gasförmig vor und ist ein universelles Lösungsmittel - Eis schwimmt, weil es leichter als flüssiges Wasser ist."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SalzeListe =
    {
        ("Wie heißt das bekannteste Salz, das wir zum Würzen von Essen benutzen?", new[] { "Kochsalz (Natriumchlorid)", "Kalk", "Traubenzucker, obwohl das auf den ersten Blick plausibel klingt" }, "Kochsalz (Natriumchlorid)",
            "Kochsalz, chemisch Natriumchlorid, ist das im Alltag am häufigsten genutzte Salz."),
        ("Wie lautet die chemische Bezeichnung für Kochsalz?", new[] { "Natriumchlorid", "Kaliumoxid", "Kalziumkarbonat" }, "Natriumchlorid",
            "Kochsalz besteht chemisch aus Natrium und Chlor und heißt deshalb Natriumchlorid (NaCl)."),
        ("Woher stammt das Kochsalz, das im Haushalt verwendet wird, meist ursprünglich?", new[] { "Aus Salzbergwerken oder durch Verdunsten von Meerwasser", "Es wird ausschließlich künstlich im Labor hergestellt, was die eigentliche Bedeutung des Begriffs verfehlt", "Es wächst wie eine Pflanze aus dem Boden" }, "Aus Salzbergwerken oder durch Verdunsten von Meerwasser",
            "Salz wird entweder aus unterirdischen Salzlagerstätten abgebaut oder durch Verdunsten von Meerwasser in Salinen gewonnen."),
        ("Was passiert, wenn man Kochsalz in Wasser auflöst?", new[] { "Es löst sich in winzige, geladene Teilchen (Ionen) auf und verteilt sich unsichtbar im Wasser", "Es bleibt immer als sichtbarer Klumpen am Boden liegen", "Es verwandelt sich chemisch in reines Wasser" }, "Es löst sich in winzige, geladene Teilchen (Ionen) auf und verteilt sich unsichtbar im Wasser",
            "Beim Auflösen zerfällt Kochsalz in seine geladenen Bestandteile (Ionen), die sich gleichmäßig im Wasser verteilen."),
        ("Warum sagt man bei Salzen manchmal \"Gegensätze ziehen sich an\"?", new[] { "Weil sie aus positiv und negativ geladenen Teilchen bestehen, die sich gegenseitig anziehen", "Weil Salze immer aus zwei Metallen bestehen", "Weil sich in Salzen niemals irgendwelche Ladungen befinden und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Weil sie aus positiv und negativ geladenen Teilchen bestehen, die sich gegenseitig anziehen",
            "Salze bestehen aus positiv geladenen und negativ geladenen Teilchen (Ionen), die sich wegen ihrer entgegengesetzten Ladung gegenseitig anziehen."),
        ("Wie sieht die geordnete Struktur von Salzkristallen typischerweise aus?", new[] { "Regelmäßig und geometrisch, z.B. würfelförmig bei Kochsalz", "Völlig unregelmäßig und chaotisch - eine haeufige, aber unzutreffende Vorstellung", "Immer rund wie eine Kugel" }, "Regelmäßig und geometrisch, z.B. würfelförmig bei Kochsalz",
            "Die geladenen Teilchen ordnen sich in Salzen sehr regelmäßig an - bei Kochsalz entstehen dadurch typische Würfelformen."),
        ("Wofür wird Salz im Winter auf Straßen und Gehwegen gestreut?", new[] { "Um das Gefrieren von Wasser zu verhindern und Eis zu schmelzen", "Um den Boden zu düngen", "Um die Straße bunter aussehen zu lassen" }, "Um das Gefrieren von Wasser zu verhindern und Eis zu schmelzen",
            "Streusalz senkt den Gefrierpunkt von Wasser, sodass Eis und Schnee schneller schmelzen und die Wege weniger rutschig werden."),
        ("Wie wurde Salz früher genutzt, um Lebensmittel länger haltbar zu machen?", new[] { "Durch Einsalzen (Pökeln), was Bakterien am Wachstum hindert", "Durch Erhitzen des Salzes auf über 1000°C, auch wenn das manche zunaechst vermuten wuerden", "Salz wurde dafür nie genutzt" }, "Durch Einsalzen (Pökeln), was Bakterien am Wachstum hindert",
            "Salz entzieht Lebensmitteln Wasser und erschwert es Bakterien, sich zu vermehren - deshalb hielten gepökelte Lebensmittel früher länger."),
        ("Welches bekannte Mineral, das ebenfalls ein Salz ist, kommt z.B. in Kalkstein vor?", new[] { "Kalziumkarbonat (Kalk)", "Natriumchlorid, was bei genauerem Hinsehen nicht stimmt", "Reines Gold" }, "Kalziumkarbonat (Kalk)",
            "Kalkstein besteht größtenteils aus Kalziumkarbonat, einem in der Natur sehr häufigen Salz."),
        ("Wie können manche Salze in der Natur entstehen, wenn ein See oder Meeresarm komplett austrocknet?", new[] { "Die zuvor gelösten Salze bleiben als feste Kristalle zurück", "Sie verschwinden für immer spurlos", "Sie verwandeln sich in Süßwasser" }, "Die zuvor gelösten Salze bleiben als feste Kristalle zurück",
            "Verdunstet das Wasser eines salzigen Sees vollständig, bleiben die zuvor gelösten Salze als feste Kristallschicht zurück."),
        ("Wie schmeckt Kochsalz typischerweise?", new[] { "Salzig", "Süß", "Bitter" }, "Salzig",
            "Der typische salzige Geschmack ist das Erkennungsmerkmal von Kochsalz beim Würzen von Speisen."),
        ("Was entsteht chemisch (vereinfacht), wenn eine Säure und eine Lauge miteinander reagieren?", new[] { "Ein Salz und Wasser (Neutralisation)", "Nur ein neues Gas", "Reiner Sauerstoff" }, "Ein Salz und Wasser (Neutralisation)",
            "Bei der Neutralisation reagieren Säure und Lauge miteinander und es entstehen ein Salz sowie Wasser."),
        ("Warum ist Meerwasser salzig?", new[] { "Es enthält viele über lange Zeit im Wasser gelöste Salze, vor allem Kochsalz", "Fische geben ständig Salz ins Wasser ab", "Salz wird künstlich ins Meer gegeben" }, "Es enthält viele über lange Zeit im Wasser gelöste Salze, vor allem Kochsalz",
            "Über Millionen von Jahren haben Flüsse Mineralien und Salze ins Meer gespült, wo sie sich angereichert haben."),
        ("Was passiert mit Salzkristallen, wenn man sie stark genug erhitzt?", new[] { "Sie schmelzen und werden flüssig", "Sie verschwinden spurlos", "Sie werden zu Gas, ohne vorher flüssig zu werden" }, "Sie schmelzen und werden flüssig",
            "Bei ausreichend hoher Temperatur schmelzen auch Salze, ähnlich wie Eis beim Erwärmen flüssig wird."),
        ("Warum sollte man nicht zu viel Salz mit dem Essen zu sich nehmen?", new[] { "Zu viel Salz kann auf Dauer der Gesundheit, z.B. dem Blutdruck, schaden", "Salz ist in jeder Menge völlig gesund", "Salz hat keinerlei Wirkung auf den Körper (was so in der Praxis nicht zutrifft)" }, "Zu viel Salz kann auf Dauer der Gesundheit, z.B. dem Blutdruck, schaden",
            "Ein dauerhaft zu hoher Salzkonsum kann den Blutdruck erhöhen und langfristig der Gesundheit schaden."),
        ("Was unterscheidet einen Salzkristall grundsätzlich von einem Metall wie Eisen?", new[] { "Salze bestehen aus geladenen Teilchen (Ionen), Metalle aus Metallatomen", "Beide bestehen aus exakt denselben Teilchen", "Salze leiten immer besser Strom als Metalle" }, "Salze bestehen aus geladenen Teilchen (Ionen), Metalle aus Metallatomen",
            "Salze sind aus positiv und negativ geladenen Ionen aufgebaut, während Metalle aus Metallatomen mit frei beweglichen Elektronen bestehen."),
        ("Wozu kann Salz neben dem Würzen von Speisen noch verwendet werden?", new[] { "Z.B. zum Enteisen von Straßen oder zum Haltbarmachen von Lebensmitteln", "Ausschließlich zur Herstellung von Süßigkeiten", "Es hat außer dem Würzen keinerlei weitere Verwendung" }, "Z.B. zum Enteisen von Straßen oder zum Haltbarmachen von Lebensmitteln",
            "Salz wird vielseitig genutzt: als Gewürz, als Streusalz gegen Glätte und traditionell zum Haltbarmachen von Lebensmitteln."),
        ("Was passiert, wenn Salzwasser vollständig verdunstet, z.B. in einer flachen Schale?", new[] { "Das gelöste Salz bleibt als fester, sichtbarer Rückstand zurück", "Das Salz verdunstet zusammen mit dem Wasser vollständig", "Es entsteht reines Trinkwasser ohne jeden Rückstand" }, "Das gelöste Salz bleibt als fester, sichtbarer Rückstand zurück",
            "Nur das Wasser verdunstet - das darin gelöste Salz kann nicht mitverdunsten und bleibt als fester Rückstand zurück."),
        ("Wie nennt man den Vorgang, bei dem sich Salz beim Verdunsten von Wasser zu geordneten Kristallen zusammenlagert?", new[] { "Kristallisation", "Destillation", "Sublimation" }, "Kristallisation",
            "Bei der Kristallisation lagern sich die gelösten Salzteilchen beim Verdunsten des Wassers zu einer geordneten Kristallstruktur zusammen."),
        ("Welches zweite Element ist neben Natrium im Kochsalz (NaCl) enthalten?", new[] { "Chlor", "Sauerstoff", "Kohlenstoff" }, "Chlor",
            "Kochsalz besteht aus Natrium und Chlor, die chemisch fest zu Natriumchlorid verbunden sind.")
    };

    private static QuizQuestion Salze(Random r)
    {
        var f = SalzeListe[r.Next(SalzeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Chemie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Salze – Gegensätze ziehen sich an", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Salze wie Kochsalz bestehen aus entgegengesetzt geladenen Teilchen (Ionen), die sich anziehen; sie lösen sich in Wasser auf und bilden beim Verdunsten wieder Kristalle."
        };
    }
}
