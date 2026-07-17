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
            [GradeLevel.Klasse6] = new List<TopicFactory> { MenschlicheOrgane, Fotosynthese, Wirbeltierklassen, PubertaetUndEntwicklung, Zelle, LebensraeumeUndNahrungsketten },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Zellbiologie, Vererbung, Oekosystem, Immunsystem, Nervensystem, SuchtUndSuchtpraevention, Humangenetik, Evolution }
        };

    private static readonly (string Organ, string Funktion, string[] Falsch)[] OrganeListe =
    {
        ("Herz", "pumpt das Blut durch den Körper", new[] { "verdaut die Nahrung", "filtert die Luft beim Atmen" }),
        ("Lunge", "nimmt Sauerstoff auf und gibt Kohlenstoffdioxid ab", new[] { "pumpt das Blut", "produziert Magensäure" }),
        ("Magen", "verdaut die Nahrung mit Magensäure", new[] { "pumpt das Blut", "denkt und steuert den Körper" }),
        ("Gehirn", "steuert und denkt, verarbeitet Sinnesreize", new[] { "verdaut die Nahrung", "pumpt das Blut" }),
        ("Niere", "filtert das Blut und bildet Urin", new[] { "pumpt das Blut", "verarbeitet Sinnesreize" }),
        ("Leber", "entgiftet den Körper und produziert Gallenflüssigkeit", new[] { "nimmt Sauerstoff auf", "steuert den Körper" }),
        ("Haut", "schützt den Körper und reguliert die Temperatur", new[] { "verdaut die Nahrung", "pumpt das Blut" }),
        ("Dünndarm", "nimmt Nährstoffe aus der Nahrung auf", new[] { "filtert das Blut", "denkt und steuert den Körper" }),
        ("Dickdarm", "entzieht dem Nahrungsbrei Wasser und bildet den Stuhl", new[] { "pumpt das Blut", "produziert Hormone" }),
        ("Bauchspeicheldrüse", "produziert Verdauungsenzyme und das Hormon Insulin", new[] { "pumpt das Blut", "filtert die Luft" }),
        ("Milz", "filtert das Blut und ist wichtig für das Immunsystem", new[] { "verdaut die Nahrung", "steuert den Körper" }),
        ("Auge", "nimmt Lichtreize wahr und ermöglicht das Sehen", new[] { "pumpt das Blut", "verdaut die Nahrung" }),
        ("Ohr", "nimmt Schallwellen wahr und ermöglicht das Hören", new[] { "filtert das Blut", "produziert Hormone" }),
        ("Speiseröhre", "transportiert die Nahrung vom Mund zum Magen", new[] { "verdaut die Nahrung mit Säure", "pumpt das Blut" }),
        ("Schilddrüse", "produziert Hormone, die den Stoffwechsel steuern", new[] { "pumpt das Blut", "verdaut die Nahrung" }),
        ("Blase", "speichert den Urin, bevor er ausgeschieden wird", new[] { "filtert das Blut", "pumpt das Blut" }),
        ("Rückenmark", "leitet Nervensignale zwischen Gehirn und Körper weiter", new[] { "verdaut die Nahrung", "pumpt das Blut" }),
        ("Muskeln", "ermöglichen Bewegung durch Zusammenziehen", new[] { "filtern das Blut", "verdauen die Nahrung" }),
        ("Knochen", "stützen den Körper und schützen innere Organe", new[] { "verdauen die Nahrung", "pumpen das Blut" }),
        ("Zunge", "ermöglicht das Schmecken und hilft beim Sprechen", new[] { "pumpt das Blut", "filtert die Luft" })
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
            "Chloroplasten", "Die grünen Chloroplasten enthalten Chlorophyll und sind der Ort der Fotosynthese."),
        ("Welcher Farbstoff in den Chloroplasten fängt das Sonnenlicht für die Fotosynthese ein?", new[] { "Chlorophyll", "Melanin", "Hämoglobin" },
            "Chlorophyll", "Chlorophyll ist der grüne Blattfarbstoff, der Lichtenergie für die Fotosynthese einfängt."),
        ("Welches Produkt der Fotosynthese speichert die Pflanze als Energievorrat?", new[] { "Traubenzucker (Glukose)", "Sauerstoff", "Kohlenstoffdioxid" },
            "Traubenzucker (Glukose)", "Die bei der Fotosynthese gebildete Glukose dient der Pflanze als Energiequelle und Baustoff, Sauerstoff ist nur ein Nebenprodukt."),
        ("Wodurch gelangt Kohlenstoffdioxid in ein Blatt hinein?", new[] { "Durch kleine Öffnungen, die Spaltöffnungen (Stomata)", "Durch die Blattoberfläche direkt ohne Öffnungen", "Durch die Wurzeln" },
            "Durch kleine Öffnungen, die Spaltöffnungen (Stomata)", "Spaltöffnungen an der Blattunterseite regeln den Gasaustausch, u.a. die Aufnahme von CO₂."),
        ("Wodurch nehmen Pflanzenwurzeln Wasser für die Fotosynthese auf?", new[] { "Über feine Wurzelhaare im Boden", "Über die Blattoberfläche", "Über die Blüten" },
            "Über feine Wurzelhaare im Boden", "Wurzelhaare vergrößern die Oberfläche der Wurzel stark und nehmen Wasser und Mineralstoffe aus dem Boden auf."),
        ("Warum sind die meisten Blätter grün?", new[] { "Weil Chlorophyll grünes Licht reflektiert und andere Farben aufnimmt", "Weil Blätter kein Licht absorbieren", "Weil grüne Farbe zufällig entsteht" },
            "Weil Chlorophyll grünes Licht reflektiert und andere Farben aufnimmt", "Chlorophyll absorbiert vor allem rotes und blaues Licht und reflektiert grünes Licht - deshalb erscheinen Blätter grün."),
        ("Wann findet Fotosynthese bei Pflanzen hauptsächlich statt?", new[] { "Tagsüber, wenn Licht vorhanden ist", "Nur nachts", "Nur bei Regen" },
            "Tagsüber, wenn Licht vorhanden ist", "Fotosynthese braucht Lichtenergie und findet deshalb hauptsächlich am Tag statt."),
        ("Was passiert nachts bei Pflanzen in Bezug auf Sauerstoff und Kohlenstoffdioxid (Zellatmung)?", new[] { "Sie atmen wie andere Lebewesen und verbrauchen dabei Sauerstoff", "Sie betreiben nachts noch stärkere Fotosynthese", "Es passiert überhaupt nichts" },
            "Sie atmen wie andere Lebewesen und verbrauchen dabei Sauerstoff", "Ohne Licht findet keine Fotosynthese statt - die Zellatmung (Sauerstoffverbrauch) läuft aber weiterhin, Tag und Nacht."),
        ("Warum ist Fotosynthese für fast alles Leben auf der Erde wichtig?", new[] { "Sie liefert den Sauerstoff, den die meisten Lebewesen zum Atmen brauchen", "Sie hat keinen Einfluss auf andere Lebewesen", "Sie erzeugt nur Wärme" },
            "Sie liefert den Sauerstoff, den die meisten Lebewesen zum Atmen brauchen", "Der bei der Fotosynthese freigesetzte Sauerstoff wird von Menschen und Tieren zum Atmen benötigt."),
        ("Was speichern viele Pflanzen als Reservestoff, der aus Traubenzucker gebildet wird?", new[] { "Stärke", "Sauerstoff", "Kohlenstoffdioxid" },
            "Stärke", "Pflanzen wandeln überschüssige Glukose oft in Stärke um, die sie z.B. in Wurzeln oder Samen speichern."),
        ("Was passiert mit der Fotosyntheserate, wenn eine Pflanze zu wenig Licht bekommt?", new[] { "Sie sinkt deutlich", "Sie steigt automatisch an", "Sie bleibt exakt gleich" },
            "Sie sinkt deutlich", "Ohne ausreichend Licht kann eine Pflanze weniger Energie durch Fotosynthese gewinnen."),
        ("Warum stehen Zimmerpflanzen oft in der Nähe eines Fensters?", new[] { "Damit sie genug Licht für die Fotosynthese bekommen", "Damit sie weniger Wasser brauchen", "Damit sie vor Kohlenstoffdioxid geschützt sind" },
            "Damit sie genug Licht für die Fotosynthese bekommen", "Ausreichend Licht am Fenster ermöglicht der Pflanze eine effektive Fotosynthese."),
        ("Welche Rolle spielt Fotosynthese am Anfang fast jeder Nahrungskette?", new[] { "Pflanzen produzieren als Erste die Energie, die andere Lebewesen weiternutzen", "Fotosynthese hat mit Nahrungsketten nichts zu tun", "Nur Tiere stehen am Anfang jeder Nahrungskette" },
            "Pflanzen produzieren als Erste die Energie, die andere Lebewesen weiternutzen", "Als Produzenten stehen fotosynthetisierende Pflanzen am Beginn fast jeder Nahrungskette."),
        ("Was ist die Grundvoraussetzung, damit Fotosynthese überhaupt stattfinden kann?", new[] { "Licht muss vorhanden sein", "Es muss dunkel sein", "Es darf kein Wasser vorhanden sein" },
            "Licht muss vorhanden sein", "Ohne Lichtenergie kann die Fotosynthese-Reaktion nicht ablaufen."),
        ("Wie verändert sich die Fotosyntheserate normalerweise bei steigender Temperatur (innerhalb eines normalen Bereichs)?", new[] { "Sie steigt zunächst an", "Sie sinkt sofort auf null", "Sie bleibt komplett unverändert" },
            "Sie steigt zunächst an", "Innerhalb eines für die Pflanze verträglichen Bereichs steigert wärmeres Wetter meist die Fotosyntheserate."),
        ("Was liefert der Regenwald durch massenhafte Fotosynthese der Erde?", new[] { "Einen großen Teil des weltweiten Sauerstoffs", "Ausschließlich Trinkwasser", "Nur Nahrung für Insekten" },
            "Einen großen Teil des weltweiten Sauerstoffs", "Regenwälder gelten wegen ihrer enormen Pflanzenmasse als wichtige Sauerstoffquellen der Erde."),
        ("Warum ist Abholzung von Wäldern schlecht für den globalen Sauerstoffhaushalt?", new[] { "Weniger Bäume betreiben weniger Fotosynthese und produzieren weniger Sauerstoff", "Abgeholzte Wälder produzieren automatisch mehr Sauerstoff", "Bäume haben keinen Einfluss auf den Sauerstoffgehalt der Luft" },
            "Weniger Bäume betreiben weniger Fotosynthese und produzieren weniger Sauerstoff", "Werden große Waldflächen abgeholzt, sinkt die Gesamtmenge an Fotosynthese und damit die Sauerstoffproduktion."),
        ("Was ist ein Chloroplast im Vergleich zur ganzen Pflanzenzelle?", new[] { "Ein kleiner Bestandteil der Zelle, der die Fotosynthese durchführt", "Die gesamte Pflanzenzelle selbst", "Ein Bestandteil, der nur bei Tieren vorkommt" },
            "Ein kleiner Bestandteil der Zelle, der die Fotosynthese durchführt", "Chloroplasten sind spezialisierte Zellorganellen innerhalb der Pflanzenzelle, in denen die Fotosynthese abläuft.")
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
            "Namensgebendes Merkmal der Säugetiere ist, dass Muttertiere ihre Jungen mit selbst produzierter Milch säugen."),
        ("Was ist ein typisches Merkmal von Vögeln?", new[] { "Federn und ein Schnabel ohne Zähne", "Kiemen zum Atmen im Wasser", "Eine feuchte, nackte Haut" }, "Federn und ein Schnabel ohne Zähne",
            "Vögel sind die einzige Wirbeltierklasse mit Federn; sie besitzen einen zahnlosen Schnabel statt eines Kiefers mit Zähnen."),
        ("Warum gelten Reptilien wie Eidechsen als wechselwarm?", new[] { "Ihre Körpertemperatur passt sich der Umgebungstemperatur an", "Sie können ihre Temperatur selbst konstant halten", "Sie leben ausschließlich im Wasser" }, "Ihre Körpertemperatur passt sich der Umgebungstemperatur an",
            "Wechselwarme (ektotherme) Tiere wie Reptilien nehmen die Temperatur ihrer Umgebung an, z.B. durch Sonnenbaden."),
        ("Welche Wirbeltierklasse legt typischerweise Eier mit einer festen, ledrigen Schale an Land?", new[] { "Reptilien", "Fische", "Amphibien" }, "Reptilien",
            "Reptilien wie Schildkröten oder Schlangen legen meist Eier mit einer schützenden, ledrigen oder harten Schale an Land ab."),
        ("Wie atmen erwachsene Amphibien zusätzlich zur Haut?", new[] { "Über Lungen", "Über Kiemen", "Über Federn" }, "Über Lungen",
            "Erwachsene Amphibien wie Frösche atmen über Lungen und zusätzlich über ihre feuchte Haut."),
        ("Welche Wirbeltierklasse hat als einzige echte Federn?", new[] { "Vögel", "Reptilien", "Säugetiere" }, "Vögel",
            "Federn sind ein einzigartiges Merkmal der Vögel und kommen bei keiner anderen Wirbeltierklasse vor."),
        ("Welches Tier ist ein Beispiel für ein Reptil?", new[] { "Eidechse", "Frosch", "Spatz" }, "Eidechse",
            "Die Eidechse gehört mit ihrer trockenen, schuppigen Haut zu den Reptilien."),
        ("Wie bringen die meisten Säugetiere ihre Jungen im Gegensatz zu Vögeln oder Reptilien zur Welt?", new[] { "Lebendgebärend, nicht durch Eier", "Immer durch harte Eier", "Immer durch weiche Eier" }, "Lebendgebärend, nicht durch Eier",
            "Die meisten Säugetiere bringen ihre Jungen lebend zur Welt, statt Eier zu legen (Ausnahmen wie das Schnabeltier bestätigen die Regel)."),
        ("Warum können Fische im Gegensatz zu Landtieren nicht lange außerhalb des Wassers überleben?", new[] { "Ihre Kiemen brauchen Wasser, um Sauerstoff aufzunehmen", "Sie brauchen an Land zu viel Licht", "Sie können an Land nicht sehen" }, "Ihre Kiemen brauchen Wasser, um Sauerstoff aufzunehmen",
            "Kiemen funktionieren nur im Wasser - an der Luft trocknen sie aus und können keinen Sauerstoff mehr aufnehmen."),
        ("Was schützt die Haut vieler Reptilien vor Austrocknung, im Gegensatz zur Amphibienhaut?", new[] { "Schuppen", "Federn", "Fell" }, "Schuppen",
            "Die trockenen Schuppen der Reptilien verhindern das Austrocknen der Haut - anders als bei Amphibien, die eine feuchte Haut brauchen."),
        ("Welche Wirbeltierklasse hat eine glatte, meist feuchte Haut ohne Schuppen oder Federn?", new[] { "Amphibien", "Reptilien", "Vögel" }, "Amphibien",
            "Amphibien wie Frösche haben eine glatte, feuchte Haut, über die sie zusätzlich atmen können."),
        ("Wie regulieren Vögel und Säugetiere im Gegensatz zu Reptilien ihre Körpertemperatur?", new[] { "Sie sind gleichwarm und halten die Temperatur konstant", "Sie sind wechselwarm wie Reptilien", "Sie haben überhaupt keine Körpertemperatur" }, "Sie sind gleichwarm und halten die Temperatur konstant",
            "Vögel und Säugetiere sind gleichwarm (endotherm) und halten ihre Körpertemperatur unabhängig von der Umgebung konstant."),
        ("Welches Tier ist ein Beispiel für ein im Wasser lebendes Säugetier?", new[] { "Wal", "Hai", "Forelle" }, "Wal",
            "Der Wal lebt im Wasser, ist aber trotzdem ein Säugetier - er atmet mit Lungen und säugt seine Jungen."),
        ("Warum zählen Wale trotz ihres Lebens im Wasser zu den Säugetieren?", new[] { "Sie säugen ihre Jungen mit Milch und atmen mit Lungen", "Sie haben Kiemen wie Fische", "Sie legen Eier wie Reptilien" }, "Sie säugen ihre Jungen mit Milch und atmen mit Lungen",
            "Die namensgebenden Säugetiermerkmale (Milch säugen, Lungenatmung) entscheiden über die Zuordnung, nicht der Lebensraum."),
        ("Welches Merkmal haben alle Fische zur Fortbewegung im Wasser gemeinsam?", new[] { "Flossen", "Federn", "Beine" }, "Flossen",
            "Flossen ermöglichen Fischen das gezielte Schwimmen und Steuern im Wasser."),
        ("Wie vermehren sich die meisten Fische?", new[] { "Sie legen Eier im Wasser (Laich)", "Sie bringen ausschließlich lebende Junge zur Welt", "Sie vermehren sich gar nicht" }, "Sie legen Eier im Wasser (Laich)",
            "Die meisten Fischarten legen große Mengen an Eiern (Laich) im Wasser ab."),
        ("Was unterscheidet die Haut eines Frosches von der einer Eidechse?", new[] { "Froschhaut ist glatt und feucht, Eidechsenhaut hat trockene Schuppen", "Beide Häute sind exakt identisch", "Froschhaut hat Federn, Eidechsenhaut nicht" }, "Froschhaut ist glatt und feucht, Eidechsenhaut hat trockene Schuppen",
            "Amphibienhaut ist feucht und dient auch der Atmung, Reptilienhaut ist trocken und schuppig zum Schutz vor Austrocknung."),
        ("Welche Wirbeltierklasse hat oft einen zahnlosen Schnabel statt eines Kiefers mit Zähnen?", new[] { "Vögel", "Säugetiere", "Amphibien" }, "Vögel",
            "Vögel besitzen statt eines Kiefers mit Zähnen einen zahnlosen Schnabel, der je nach Art unterschiedlich geformt ist.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PubertaetListe =
    {
        ("Was passiert im Körper während der Pubertät?", new[] { "Hormone verändern Körper und Gefühle", "Es passiert überhaupt nichts", "Nur die Haarfarbe verändert sich" },
            "Hormone verändern Körper und Gefühle", "In der Pubertät steuern Hormone einen Wachstumsschub sowie viele körperliche und seelische Veränderungen."),
        ("Warum kommt es in der Pubertät häufig zu einem Wachstumsschub?", new[] { "Hormone regen das Knochen- und Muskelwachstum an", "Der Körper hört auf zu wachsen", "Es liegt nur an der Ernährung" },
            "Hormone regen das Knochen- und Muskelwachstum an", "Wachstumshormone und Geschlechtshormone sorgen in der Pubertät oft für einen schnellen Anstieg der Körpergröße."),
        ("Warum verändert sich bei manchen Jugendlichen in der Pubertät die Stimme?", new[] { "Der Kehlkopf wächst und die Stimmbänder verändern sich", "Die Zunge wird kürzer", "Die Ohren verändern sich" },
            "Der Kehlkopf wächst und die Stimmbänder verändern sich", "Besonders bei Jungen wächst der Kehlkopf stärker, wodurch die Stimme tiefer wird (Stimmbruch)."),
        ("Warum wird Körperhygiene in der Pubertät besonders wichtig?", new[] { "Schweiß- und Talgdrüsen werden aktiver", "Der Körper braucht plötzlich kein Wasser mehr", "Hygiene spielt keine Rolle mehr" },
            "Schweiß- und Talgdrüsen werden aktiver", "Hormonelle Veränderungen aktivieren Schweiß- und Talgdrüsen stärker, weshalb regelmäßiges Waschen wichtiger wird."),
        ("Was ist eine mögliche Ursache für Hautunreinheiten (z.B. Pickel) in der Pubertät?", new[] { "Vermehrte Talgproduktion durch Hormone", "Zu viel Schlaf", "Zu häufiges Zähneputzen" },
            "Vermehrte Talgproduktion durch Hormone", "Hormonelle Umstellungen können die Talgdrüsen der Haut aktiver machen, was zu Unreinheiten führen kann."),
        ("Warum schwanken die Gefühle bei vielen Jugendlichen in der Pubertät stärker als vorher?", new[] { "Hormone beeinflussen auch die Stimmung", "Das hat nichts mit dem Körper zu tun", "Nur schlechte Laune ist normal" },
            "Hormone beeinflussen auch die Stimmung", "Die hormonellen Veränderungen der Pubertät wirken sich auch auf Stimmung und Gefühlsleben aus - Stimmungsschwankungen sind normal."),
        ("Wie unterschiedlich kann der Zeitpunkt sein, an dem die Pubertät bei verschiedenen Jugendlichen beginnt?", new[] { "Der Beginn kann von Person zu Person deutlich variieren", "Die Pubertät beginnt bei allen exakt am gleichen Tag", "Die Pubertät beginnt immer erst mit 18 Jahren" },
            "Der Beginn kann von Person zu Person deutlich variieren", "Der Zeitpunkt und das Tempo der pubertären Entwicklung sind individuell sehr unterschiedlich - das ist völlig normal."),
        ("Was ist bei körperlichen Veränderungen in der Pubertät bei Klassenkameraden wichtig?", new[] { "Respekt, da jeder Körper sich anders und unterschiedlich schnell verändert", "Andere wegen ihres Körpers auszulachen", "Alle sollen sich exakt gleich entwickeln" },
            "Respekt, da jeder Körper sich anders und unterschiedlich schnell verändert", "Da sich Körper unterschiedlich schnell verändern, ist gegenseitiger Respekt und Rücksichtnahme besonders wichtig."),
        ("An wen können sich Jugendliche wenden, wenn sie Fragen oder Sorgen zur Pubertät haben?", new[] { "An eine Vertrauensperson, z.B. Eltern, Ärztin/Arzt oder Beratungsstelle", "An niemanden, man muss alles allein herausfinden", "Nur an gleichaltrige Fremde im Internet" },
            "An eine Vertrauensperson, z.B. Eltern, Ärztin/Arzt oder Beratungsstelle", "Vertrauenspersonen wie Eltern, Ärztinnen/Ärzte oder Beratungsstellen können bei Fragen zur Pubertät gut weiterhelfen."),
        ("Was bedeutet \"körperliche Selbstbestimmung\"?", new[] { "Jeder Mensch entscheidet selbst über seinen eigenen Körper", "Andere Menschen dürfen über meinen Körper bestimmen", "Nur Erwachsene haben ein Recht auf ihren eigenen Körper" },
            "Jeder Mensch entscheidet selbst über seinen eigenen Körper", "Körperliche Selbstbestimmung bedeutet, dass niemand ohne Zustimmung über den Körper einer anderen Person verfügen darf."),
        ("Wodurch wird bei einem Menschen eine Schwangerschaft ausgelöst?", new[] { "Durch die Befruchtung einer Eizelle durch eine Samenzelle", "Durch besonders langes Schlafen", "Durch das Essen bestimmter Lebensmittel" },
            "Durch die Befruchtung einer Eizelle durch eine Samenzelle", "Eine Schwangerschaft beginnt, wenn eine Samenzelle eine Eizelle befruchtet und sich die befruchtete Eizelle in der Gebärmutter einnistet."),
        ("Wo wächst ein ungeborenes Kind während der Schwangerschaft heran?", new[] { "In der Gebärmutter", "Im Magen", "In der Lunge" },
            "In der Gebärmutter", "Das ungeborene Kind entwickelt sich in der Gebärmutter der Mutter, geschützt und mit Nährstoffen versorgt."),
        ("Wie lange dauert eine menschliche Schwangerschaft ungefähr?", new[] { "Etwa neun Monate", "Etwa ein Monat", "Etwa drei Jahre" },
            "Etwa neun Monate", "Eine menschliche Schwangerschaft dauert im Durchschnitt etwa 40 Wochen, also rund neun Monate."),
        ("Was ist eine Regelblutung (Menstruation)?", new[] { "Die monatliche Abstoßung der Gebärmutterschleimhaut, wenn keine Befruchtung stattfand", "Eine seltene Krankheit", "Ein Zeichen, dass etwas nicht stimmt" },
            "Die monatliche Abstoßung der Gebärmutterschleimhaut, wenn keine Befruchtung stattfand", "Findet keine Befruchtung statt, wird die aufgebaute Gebärmutterschleimhaut monatlich abgestoßen - das ist die Menstruation, ein normaler Vorgang."),
        ("Warum produziert der Körper von Jungen in der Pubertät vermehrt Samenzellen?", new[] { "Die Hoden beginnen mit der Produktion, gesteuert durch Hormone", "Weil sie mehr Sport treiben", "Weil sie älter aussehen wollen" },
            "Die Hoden beginnen mit der Produktion, gesteuert durch Hormone", "Mit Beginn der Pubertät regen Hormone die Hoden dazu an, Samenzellen zu bilden."),
        ("Was bezeichnet man ganz allgemein als Verhütung (Empfängnisverhütung)?", new[] { "Maßnahmen, die eine Schwangerschaft verhindern sollen", "Maßnahmen, die eine Schwangerschaft garantieren", "Ein anderes Wort für Pubertät" },
            "Maßnahmen, die eine Schwangerschaft verhindern sollen", "Verhütungsmittel wie z.B. Kondome sollen verhindern, dass eine Eizelle befruchtet wird oder sich einnistet."),
        ("Warum gilt das Kondom auch als Schutz vor sexuell übertragbaren Krankheiten?", new[] { "Es verhindert den direkten Kontakt von Körperflüssigkeiten", "Es hat damit nichts zu tun", "Es wirkt nur gegen Erkältungen" },
            "Es verhindert den direkten Kontakt von Körperflüssigkeiten", "Kondome verhindern sowohl eine Schwangerschaft als auch die Übertragung mancher Krankheitserreger über Körperflüssigkeiten."),
        ("Wie sehen heute viele verschiedene Familienmodelle in Deutschland aus?", new[] { "Es gibt z.B. Kleinfamilien, Patchworkfamilien, Alleinerziehende oder gleichgeschlechtliche Elternpaare", "Es gibt nur ein einziges richtiges Familienmodell", "Familien bestehen immer aus genau vier Personen" },
            "Es gibt z.B. Kleinfamilien, Patchworkfamilien, Alleinerziehende oder gleichgeschlechtliche Elternpaare", "In einer modernen Gesellschaft gibt es viele unterschiedliche, gleichwertige Familienformen, die respektiert werden sollten."),
        ("Was ist bei der Geburt eines Kindes normalerweise der erste Schritt?", new[] { "Wehen kündigen die Geburt an, dann verlässt das Kind die Gebärmutter über den Geburtskanal", "Das Kind wird sofort operiert", "Die Mutter schläft während der gesamten Geburt" },
            "Wehen kündigen die Geburt an, dann verlässt das Kind die Gebärmutter über den Geburtskanal", "Regelmäßige Wehen leiten die Geburt ein, durch die das Kind über den Geburtskanal zur Welt kommt (bei einem Kaiserschnitt operativ)."),
        ("Warum ist es wichtig zu wissen, dass jeder Mensch sich in der Pubertät in seinem eigenen Tempo entwickelt?", new[] { "Damit niemand sich wegen früherer oder späterer Entwicklung schämen oder unter Druck fühlen muss", "Damit alle möglichst schnell erwachsen wirken", "Damit man andere mit ihrer Entwicklung vergleichen und bewerten kann" },
            "Damit niemand sich wegen früherer oder späterer Entwicklung schämen oder unter Druck fühlen muss", "Unterschiedliche Entwicklungstempi sind normal - Wissen darüber hilft, Scham oder Vergleichsdruck abzubauen und respektvoll miteinander umzugehen.")
    };

    private static QuizQuestion PubertaetUndEntwicklung(Random r)
    {
        var f = PubertaetListe[r.Next(PubertaetListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Pubertät und Entwicklung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Hormone steuern Wachstum, Stimme, Haut und Gefühle - der Zeitpunkt der Pubertät ist bei jedem Menschen unterschiedlich."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ZellbiologieListe =
    {
        ("Welcher Zellbestandteil enthält die Erbinformation (DNA)?", new[] { "Zellkern", "Zellmembran", "Mitochondrium" }, "Zellkern",
            "Der Zellkern enthält die DNA mit der Erbinformation der Zelle."),
        ("Welcher Zellbestandteil wird als \"Kraftwerk der Zelle\" bezeichnet?", new[] { "Mitochondrium", "Zellkern", "Ribosom" }, "Mitochondrium",
            "Mitochondrien produzieren durch Zellatmung Energie (ATP) und werden daher \"Kraftwerk der Zelle\" genannt."),
        ("Was unterscheidet eine Pflanzenzelle von einer Tierzelle?", new[] { "Zellwand und Chloroplasten", "Zellkern", "Zellmembran" }, "Zellwand und Chloroplasten",
            "Pflanzenzellen besitzen zusätzlich eine feste Zellwand und Chloroplasten für die Fotosynthese."),
        ("Welche Aufgabe haben Ribosomen in der Zelle?", new[] { "Sie stellen Proteine (Eiweiße) her", "Sie speichern die Erbinformation", "Sie produzieren Energie durch Zellatmung" }, "Sie stellen Proteine (Eiweiße) her",
            "Ribosomen übersetzen die genetische Information in Proteine - man nennt sie deshalb auch \"Proteinfabriken\" der Zelle."),
        ("Was umschließt jede Zelle nach außen und regelt, was hinein- und hinausgelangt?", new[] { "Die Zellmembran", "Der Zellkern", "Das Mitochondrium" }, "Die Zellmembran",
            "Die Zellmembran umgibt die Zelle und kontrolliert, welche Stoffe hinein- oder herausgelangen können."),
        ("Was ist das Zytoplasma einer Zelle?", new[] { "Die gallertartige Grundsubstanz, in der die Zellorganellen schwimmen", "Ein anderes Wort für Zellkern", "Die äußere Hülle der Zelle" }, "Die gallertartige Grundsubstanz, in der die Zellorganellen schwimmen",
            "Im Zytoplasma befinden sich alle Zellorganellen, eingebettet in eine gallertartige Flüssigkeit."),
        ("Welche Aufgabe hat der Golgi-Apparat in der Zelle?", new[] { "Er verpackt und transportiert Proteine innerhalb und aus der Zelle", "Er speichert die Erbinformation", "Er produziert Energie durch Zellatmung" }, "Er verpackt und transportiert Proteine innerhalb und aus der Zelle",
            "Der Golgi-Apparat modifiziert und verpackt Proteine, damit sie an ihren richtigen Bestimmungsort gelangen."),
        ("Was ist ein Endoplasmatisches Retikulum (vereinfacht)?", new[] { "Ein Netzwerk aus Membranen, das Stoffe innerhalb der Zelle transportiert", "Ein anderes Wort für Zellwand", "Eine Art Antenne der Zelle" }, "Ein Netzwerk aus Membranen, das Stoffe innerhalb der Zelle transportiert",
            "Das Endoplasmatische Retikulum ist ein verzweigtes Membransystem, das am Transport von Stoffen in der Zelle beteiligt ist."),
        ("Welche Struktur speichert bei Pflanzenzellen Wasser und Nährstoffe?", new[] { "Die Vakuole", "Das Mitochondrium", "Der Zellkern" }, "Die Vakuole",
            "Die große Vakuole in Pflanzenzellen speichert Wasser, Nährstoffe und sorgt für die Stabilität (Zellinnendruck) der Zelle."),
        ("Was ist der Unterschied zwischen Pro- und Eukaryoten (vereinfacht)?", new[] { "Eukaryoten haben einen echten Zellkern, Prokaryoten nicht", "Beide haben exakt dieselbe Zellstruktur", "Nur Prokaryoten haben einen Zellkern" }, "Eukaryoten haben einen echten Zellkern, Prokaryoten nicht",
            "Eukaryotische Zellen (z.B. Tier-/Pflanzenzellen) besitzen einen echten, membranumschlossenen Zellkern - prokaryotische Zellen (z.B. Bakterien) nicht."),
        ("Sind Bakterien Beispiele für Prokaryoten oder Eukaryoten?", new[] { "Prokaryoten", "Eukaryoten", "Weder noch" }, "Prokaryoten",
            "Bakterien haben keinen echten Zellkern und zählen deshalb zu den Prokaryoten."),
        ("Was passiert bei der Zellteilung (Mitose)?", new[] { "Eine Zelle teilt sich in zwei identische Tochterzellen", "Zwei Zellen verschmelzen zu einer", "Eine Zelle verschwindet komplett" }, "Eine Zelle teilt sich in zwei identische Tochterzellen",
            "Bei der Mitose entstehen aus einer Mutterzelle zwei genetisch identische Tochterzellen."),
        ("Woraus besteht die DNA hauptsächlich aufgebaut?", new[] { "Aus Nukleotiden", "Aus Proteinen allein", "Aus Zucker allein" }, "Aus Nukleotiden",
            "Die DNA besteht aus einer langen Kette von Nukleotiden, den Grundbausteinen der Erbinformation."),
        ("Wer entdeckte gemeinsam mit James Watson die Doppelhelix-Struktur der DNA?", new[] { "Francis Crick", "Gregor Mendel", "Charles Darwin" }, "Francis Crick",
            "James Watson und Francis Crick beschrieben 1953 die Doppelhelix-Struktur der DNA."),
        ("Was ist ein Gen?", new[] { "Ein Abschnitt der DNA, der die Bauanleitung für ein Merkmal/Protein enthält", "Ein anderes Wort für Zellkern", "Ein Bestandteil der Zellmembran" }, "Ein Abschnitt der DNA, der die Bauanleitung für ein Merkmal/Protein enthält",
            "Gene sind Abschnitte der DNA, die Bauanleitungen für Proteine bzw. Merkmale eines Lebewesens enthalten."),
        ("Welche Zellorganellen enthalten neben dem Zellkern selbst noch eigene DNA?", new[] { "Mitochondrien (und bei Pflanzen zusätzlich Chloroplasten)", "Ribosomen", "Die Zellmembran" }, "Mitochondrien (und bei Pflanzen zusätzlich Chloroplasten)",
            "Mitochondrien und Chloroplasten besitzen eigene, kleine DNA-Moleküle - ein Hinweis auf ihre evolutionäre Herkunft."),
        ("Was passiert, wenn eine tierische Zelle zu viel Wasser aufnimmt?", new[] { "Sie kann anschwellen und im Extremfall platzen", "Sie schrumpft dabei automatisch", "Es passiert überhaupt nichts" }, "Sie kann anschwellen und im Extremfall platzen",
            "Ohne feste Zellwand kann eine tierische Zelle bei zu viel Wasseraufnahme anschwellen und im Extremfall platzen."),
        ("Warum platzen Pflanzenzellen bei zu viel Wasseraufnahme normalerweise nicht?", new[] { "Die feste Zellwand stützt und schützt die Zelle", "Pflanzenzellen nehmen niemals Wasser auf", "Pflanzenzellen haben keine Membran" }, "Die feste Zellwand stützt und schützt die Zelle",
            "Die stabile Zellwand der Pflanzenzelle wirkt einem übermäßigen Anschwellen entgegen."),
        ("Was ist die Aufgabe von Lysosomen in der Zelle?", new[] { "Sie bauen Abfallstoffe und alte Zellbestandteile ab", "Sie speichern die Erbinformation", "Sie produzieren Sauerstoff" }, "Sie bauen Abfallstoffe und alte Zellbestandteile ab",
            "Lysosomen enthalten Enzyme, die Abfallstoffe und beschädigte Zellbestandteile abbauen - eine Art \"Recyclingstation\" der Zelle."),
        ("Wie viele Chromosomen hat eine normale menschliche Körperzelle?", new[] { "46", "23", "100" }, "46",
            "Eine menschliche Körperzelle enthält normalerweise 46 Chromosomen (23 Paare).")
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
            "Ein dominantes Allel bestimmt das äußere Merkmal (Phänotyp), auch wenn nur eine Kopie davon vorhanden ist."),
        ("Was ist der Unterschied zwischen Genotyp und Phänotyp?", new[] { "Genotyp ist die genetische Anlage, Phänotyp das sichtbare Erscheinungsbild", "Beides bedeutet dasselbe", "Genotyp ist nur bei Pflanzen wichtig" },
            "Genotyp ist die genetische Anlage, Phänotyp das sichtbare Erscheinungsbild", "Der Genotyp ist die Erbanlage (DNA), der Phänotyp das daraus resultierende, sichtbare Merkmal eines Lebewesens."),
        ("Wodurch können Mutationen entstehen?", new[] { "Durch zufällige Veränderungen im Erbgut, z.B. durch Strahlung oder Kopierfehler", "Nur durch bewusste Entscheidungen des Lebewesens", "Mutationen gibt es gar nicht" },
            "Durch zufällige Veränderungen im Erbgut, z.B. durch Strahlung oder Kopierfehler", "Mutationen sind zufällige Veränderungen der DNA, die z.B. durch UV-Strahlung oder Fehler bei der Zellteilung entstehen können."),
        ("Was sind Chromosomen?", new[] { "Verpackte, aufgewickelte DNA-Fäden im Zellkern", "Ein anderes Wort für Zellwand", "Kleine Tiere in der Zelle" },
            "Verpackte, aufgewickelte DNA-Fäden im Zellkern", "Chromosomen sind stark aufgewickelte DNA-Stränge, auf denen die Gene liegen; der Mensch hat davon 46 in fast jeder Zelle."),
        ("Was ist ein rezessives Allel?", new[] { "Ein Allel, das nur sichtbar wird, wenn kein dominantes Allel vorhanden ist", "Ein Allel, das sich immer durchsetzt", "Ein Allel, das gar keine Wirkung hat" },
            "Ein Allel, das nur sichtbar wird, wenn kein dominantes Allel vorhanden ist", "Ein rezessives Allel zeigt sich im Phänotyp nur, wenn beide Genkopien rezessiv sind."),
        ("Was bedeutet \"reinerbig\" (homozygot) bei einem Merkmal?", new[] { "Beide Allele für ein Merkmal sind gleich", "Die beiden Allele unterscheiden sich", "Es gibt gar kein zweites Allel" },
            "Beide Allele für ein Merkmal sind gleich", "Reinerbig (homozygot) bedeutet, dass beide Kopien des Gens für ein Merkmal identisch sind."),
        ("Was bedeutet \"mischerbig\" (heterozygot) bei einem Merkmal?", new[] { "Die beiden Allele für ein Merkmal unterscheiden sich", "Beide Allele sind exakt gleich", "Es gibt überhaupt kein Allel" },
            "Die beiden Allele für ein Merkmal unterscheiden sich", "Mischerbig (heterozygot) bedeutet, dass die beiden Genkopien unterschiedliche Ausprägungen tragen, z.B. ein dominantes und ein rezessives Allel."),
        ("Wer gilt als Begründer der modernen Genetik durch seine Kreuzungsversuche?", new[] { "Gregor Mendel", "Charles Darwin", "Louis Pasteur" },
            "Gregor Mendel", "Gregor Mendel entdeckte im 19. Jahrhundert durch systematische Erbsenversuche grundlegende Vererbungsregeln."),
        ("Was untersuchte Gregor Mendel in seinen berühmten Kreuzungsversuchen?", new[] { "Erbsenpflanzen mit unterschiedlichen Merkmalen", "Fruchtfliegen im Labor", "Menschliche Familienstammbäume" },
            "Erbsenpflanzen mit unterschiedlichen Merkmalen", "Mendel kreuzte gezielt Erbsenpflanzen mit unterschiedlichen Merkmalen (z.B. Farbe, Form) und leitete daraus Vererbungsregeln ab."),
        ("Was bestimmt bei Menschen üblicherweise das biologische Geschlecht?", new[] { "Die Geschlechtschromosomen (XX oder XY)", "Die Ernährung der Mutter", "Der Geburtsmonat" },
            "Die Geschlechtschromosomen (XX oder XY)", "Zwei X-Chromosomen führen üblicherweise zu weiblichem, ein X- und ein Y-Chromosom zu männlichem Geschlecht."),
        ("Wie viele Chromosomen erhält ein Kind jeweils von jedem Elternteil?", new[] { "23", "46", "10" },
            "23", "Jeder Elternteil gibt über die Keimzellen (Ei-/Samenzelle) 23 Chromosomen weiter, zusammen ergeben sich 46."),
        ("Was ist eine Erbkrankheit?", new[] { "Eine Krankheit, die durch veränderte Gene von den Eltern weitergegeben werden kann", "Eine Krankheit, die man sich nur durch Ansteckung holen kann", "Eine Krankheit, die ausschließlich im Alter entsteht" },
            "Eine Krankheit, die durch veränderte Gene von den Eltern weitergegeben werden kann", "Erbkrankheiten beruhen auf Veränderungen im Erbgut, die von den Eltern an die Kinder weitergegeben werden können."),
        ("Was zeigt ein Stammbaum in der Genetik?", new[] { "Wie sich ein Merkmal über mehrere Generationen einer Familie vererbt", "Nur den Wohnort einer Familie", "Nur das Alter der Familienmitglieder" },
            "Wie sich ein Merkmal über mehrere Generationen einer Familie vererbt", "Ein genetischer Stammbaum verfolgt das Auftreten eines Merkmals über mehrere Generationen hinweg."),
        ("Was ist der Unterschied zwischen einer Genmutation und einer Chromosomenmutation (vereinfacht)?", new[] { "Genmutation verändert ein einzelnes Gen, Chromosomenmutation die Struktur/Anzahl ganzer Chromosomen", "Beide bedeuten exakt dasselbe", "Genmutationen betreffen immer das ganze Chromosom" },
            "Genmutation verändert ein einzelnes Gen, Chromosomenmutation die Struktur/Anzahl ganzer Chromosomen", "Genmutationen betreffen einzelne DNA-Abschnitte, Chromosomenmutationen größere Bereiche oder ganze Chromosomen."),
        ("Können auch äußere Einflüsse wie UV-Strahlung Mutationen auslösen?", new[] { "Ja", "Nein, niemals", "Nur bei Pflanzen" },
            "Ja", "UV-Strahlung, Röntgenstrahlung oder bestimmte Chemikalien können das Erbgut schädigen und Mutationen auslösen."),
        ("Was passiert bei eineiigen Zwillingen genetisch?", new[] { "Sie haben (fast) identisches Erbgut", "Sie haben komplett unterschiedliches Erbgut", "Sie haben gar kein eigenes Erbgut" },
            "Sie haben (fast) identisches Erbgut", "Eineiige Zwillinge entstehen aus einer befruchteten Eizelle, die sich teilt - ihr Erbgut ist praktisch identisch."),
        ("Was unterscheidet zweieiige Zwillinge genetisch von eineiigen?", new[] { "Sie sind genetisch nicht ähnlicher als normale Geschwister", "Sie haben exakt identisches Erbgut", "Sie haben gar kein Erbgut gemeinsam" },
            "Sie sind genetisch nicht ähnlicher als normale Geschwister", "Zweieiige Zwillinge entstehen aus zwei getrennt befruchteten Eizellen und ähneln sich genetisch wie gewöhnliche Geschwister."),
        ("Wozu dient Gentechnik in der Landwirtschaft, ein Beispiel?", new[] { "Um Pflanzen widerstandsfähiger gegen Schädlinge zu machen", "Um Pflanzen komplett ohne Erbgut wachsen zu lassen", "Um das Wetter zu beeinflussen" },
            "Um Pflanzen widerstandsfähiger gegen Schädlinge zu machen", "Gentechnische Verfahren werden u.a. genutzt, um Nutzpflanzen widerstandsfähiger gegen Schädlinge oder Trockenheit zu machen."),
        ("Was bedeutet es, wenn ein Merkmal \"geschlechtsgebunden\" vererbt wird (z.B. Rot-Grün-Sehschwäche)?", new[] { "Das entsprechende Gen liegt auf einem Geschlechtschromosom", "Das Merkmal hat nichts mit Genen zu tun", "Nur Frauen können dieses Merkmal überhaupt vererben" },
            "Das entsprechende Gen liegt auf einem Geschlechtschromosom", "Geschlechtsgebundene Merkmale wie die Rot-Grün-Sehschwäche werden über Gene auf den Geschlechtschromosomen vererbt, weshalb sie bei Männern und Frauen unterschiedlich häufig auftreten.")
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
            "Destruenten bauen tote organische Stoffe ab und geben Nährstoffe an den Boden zurück."),
        ("Was passiert, wenn in einem Ökosystem ein Raubtier (z.B. der Wolf) ausstirbt?", new[] { "Die Beutetiere können sich stark vermehren und das Gleichgewicht verschiebt sich", "Es ändert sich gar nichts", "Alle Pflanzen sterben sofort ab" },
            "Die Beutetiere können sich stark vermehren und das Gleichgewicht verschiebt sich", "Fehlt ein natürlicher Feind, können sich Beutetierbestände stark vermehren und z.B. die Vegetation übernutzen - das ökologische Gleichgewicht gerät durcheinander."),
        ("Was versteht man unter einer \"Nahrungskette\"?", new[] { "Die Reihenfolge, wer wen als Nahrung nutzt (z.B. Pflanze → Maus → Fuchs)", "Eine Liste von Lebensmitteln im Supermarkt", "Eine Kette aus Metall in der Landwirtschaft" },
            "Die Reihenfolge, wer wen als Nahrung nutzt (z.B. Pflanze → Maus → Fuchs)", "Eine Nahrungskette zeigt, wie Energie von Produzenten über Konsumenten weitergegeben wird, z.B. Pflanze → Maus → Fuchs."),
        ("Was bedeutet \"biologisches Gleichgewicht\" in einem Ökosystem?", new[] { "Die Bestände von Pflanzen und Tieren halten sich über längere Zeit ungefähr die Waage", "Alle Tierarten haben exakt gleich viele Individuen", "Es gibt in einem Ökosystem nur eine einzige Art" },
            "Die Bestände von Pflanzen und Tieren halten sich über längere Zeit ungefähr die Waage", "Im biologischen Gleichgewicht regulieren sich Populationen (z.B. Räuber und Beute) gegenseitig, sodass keine Art dauerhaft überhandnimmt."),
        ("Was ist ein \"Konsument\" in einem Ökosystem?", new[] { "Ein Lebewesen, das andere Lebewesen als Nahrung nutzt", "Ein Lebewesen, das Fotosynthese betreibt", "Ein Lebewesen, das tote Materie zersetzt" },
            "Ein Lebewesen, das andere Lebewesen als Nahrung nutzt", "Konsumenten (Tiere) ernähren sich von Produzenten (Pflanzen) oder anderen Konsumenten."),
        ("Was unterscheidet einen Konsument erster Ordnung von einem Konsument zweiter Ordnung?", new[] { "Erster Ordnung frisst Pflanzen, zweiter Ordnung frisst andere Tiere", "Beide fressen ausschließlich Pflanzen", "Beide fressen ausschließlich Fleisch" },
            "Erster Ordnung frisst Pflanzen, zweiter Ordnung frisst andere Tiere", "Konsumenten erster Ordnung (Pflanzenfresser) stehen zwischen Produzenten und Konsumenten zweiter Ordnung (Fleischfresser) in der Nahrungskette."),
        ("Was ist ein \"Nahrungsnetz\" im Unterschied zu einer einfachen Nahrungskette?", new[] { "Ein Nahrungsnetz zeigt mehrere miteinander verknüpfte Nahrungsketten", "Ein Nahrungsnetz zeigt nur eine einzige Art", "Ein Nahrungsnetz betrifft nur Pflanzen" },
            "Ein Nahrungsnetz zeigt mehrere miteinander verknüpfte Nahrungsketten", "Da die meisten Tiere mehrere Nahrungsquellen haben, verknüpfen sich viele Nahrungsketten zu einem komplexen Nahrungsnetz."),
        ("Was ist ein \"Lebensraum\" (Habitat)?", new[] { "Der Ort, an dem ein Lebewesen normalerweise lebt", "Ein anderes Wort für Nahrungskette", "Eine bestimmte Tierart selbst" },
            "Der Ort, an dem ein Lebewesen normalerweise lebt", "Der Lebensraum bietet einem Lebewesen die notwendigen Bedingungen wie Nahrung, Schutz und Fortpflanzungsmöglichkeiten."),
        ("Was ist eine \"Population\" in der Ökologie?", new[] { "Alle Individuen einer Art in einem bestimmten Gebiet", "Alle Lebewesen aller Arten weltweit", "Ein einzelnes Tier oder eine einzelne Pflanze" },
            "Alle Individuen einer Art in einem bestimmten Gebiet", "Eine Population umfasst alle Angehörigen einer Art, die in einem bestimmten Gebiet zusammenleben und sich fortpflanzen können."),
        ("Was ist eine \"Lebensgemeinschaft\" (Biozönose)?", new[] { "Alle Lebewesen verschiedener Arten, die in einem Gebiet zusammenleben", "Nur eine einzige Tierart in einem Gebiet", "Ein anderes Wort für Nahrungskette" },
            "Alle Lebewesen verschiedener Arten, die in einem Gebiet zusammenleben", "Die Biozönose umfasst alle Pflanzen, Tiere und andere Organismen, die gemeinsam einen Lebensraum bewohnen."),
        ("Was passiert, wenn eine invasive (fremde) Tierart in ein Ökosystem eingeführt wird?", new[] { "Sie kann heimische Arten verdrängen und das Gleichgewicht stören", "Es passiert grundsätzlich nichts", "Sie passt sich automatisch perfekt ein" },
            "Sie kann heimische Arten verdrängen und das Gleichgewicht stören", "Invasive Arten haben oft keine natürlichen Feinde im neuen Gebiet und können heimische Arten verdrängen."),
        ("Warum sind Bienen für viele Ökosysteme besonders wichtig?", new[] { "Sie bestäuben Blüten und ermöglichen so die Fortpflanzung vieler Pflanzen", "Sie zersetzen abgestorbene Pflanzen", "Sie regulieren die Wassermenge im Boden" },
            "Sie bestäuben Blüten und ermöglichen so die Fortpflanzung vieler Pflanzen", "Ohne Bestäuber wie Bienen könnten sich viele Blütenpflanzen nicht fortpflanzen - das hätte weitreichende Folgen für Ökosysteme und Landwirtschaft."),
        ("Was passiert mit der Energie, wenn sie von einer Nahrungskettenstufe zur nächsten weitergegeben wird?", new[] { "Ein großer Teil geht als Wärme verloren, nur ein kleiner Teil wird weitergegeben", "Die gesamte Energie wird vollständig weitergegeben", "Es entsteht dabei zusätzliche neue Energie" },
            "Ein großer Teil geht als Wärme verloren, nur ein kleiner Teil wird weitergegeben", "Bei jedem Schritt der Nahrungskette geht viel Energie als Wärme verloren - deshalb gibt es meist weniger Raubtiere als Beutetiere."),
        ("Was ist eine \"Symbiose\" zwischen zwei Arten?", new[] { "Ein enges Zusammenleben, von dem meist beide Arten profitieren", "Ein Kampf zweier Arten um denselben Lebensraum", "Ein Zustand, bei dem eine Art die andere komplett vernichtet" },
            "Ein enges Zusammenleben, von dem meist beide Arten profitieren", "Bei einer Symbiose profitieren in der Regel beide beteiligten Arten vom engen Zusammenleben."),
        ("Was ist \"Parasitismus\" im Unterschied zur Symbiose?", new[] { "Eine Art (Parasit) profitiert auf Kosten einer anderen Art (Wirt)", "Beide Arten profitieren gleichermaßen", "Keine der beiden Arten hat einen Vor- oder Nachteil" },
            "Eine Art (Parasit) profitiert auf Kosten einer anderen Art (Wirt)", "Beim Parasitismus zieht der Parasit Nutzen aus dem Wirt, während dieser dadurch geschädigt wird."),
        ("Warum sind Feuchtgebiete wie Moore und Sümpfe besonders artenreiche Ökosysteme?", new[] { "Sie bieten vielen spezialisierten Pflanzen und Tieren Lebensraum", "Dort kann grundsätzlich kein Leben existieren", "Sie sind komplett frei von Pflanzen" },
            "Sie bieten vielen spezialisierten Pflanzen und Tieren Lebensraum", "Feuchtgebiete bieten besondere Lebensbedingungen, an die sich viele spezialisierte Arten angepasst haben."),
        ("Was passiert mit einem Gewässer-Ökosystem, wenn zu viele Nährstoffe (z.B. Dünger) hineingelangen?", new[] { "Es kann zu übermäßigem Algenwachstum (Eutrophierung) kommen", "Das Gewässer wird automatisch sauberer", "Es passiert überhaupt nichts" },
            "Es kann zu übermäßigem Algenwachstum (Eutrophierung) kommen", "Übermäßige Nährstoffe können ein starkes Algenwachstum auslösen, das anderen Wasserlebewesen den Sauerstoff entzieht."),
        ("Was ist eine \"ökologische Nische\" einer Art?", new[] { "Die spezifische Rolle und die Ansprüche einer Art in ihrem Lebensraum", "Ein anderes Wort für den geografischen Kontinent einer Art", "Ein spezielles Versteck, das nur nachts genutzt wird" },
            "Die spezifische Rolle und die Ansprüche einer Art in ihrem Lebensraum", "Die ökologische Nische beschreibt, welche Ressourcen eine Art nutzt und welche Rolle sie im Ökosystem einnimmt."),
        ("Warum sind Korallenriffe ökologisch besonders wertvoll?", new[] { "Sie bieten sehr vielen Meereslebewesen Lebensraum und Nahrung", "Sie sind komplett lebensfeindlich für Meereslebewesen", "Sie kommen nur in kalten Meeren vor" },
            "Sie bieten sehr vielen Meereslebewesen Lebensraum und Nahrung", "Korallenriffe zählen zu den artenreichsten Ökosystemen der Erde und beherbergen zahlreiche Fisch- und Wirbellosenarten.")
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ImmunsystemListe =
    {
        ("Was ist der Unterschied zwischen Bakterien und Viren als Krankheitserreger?", new[] { "Bakterien sind eigenständige Zellen, Viren benötigen eine Wirtszelle zur Vermehrung", "Beide sind identisch aufgebaut", "Viren sind größer als Bakterien" }, "Bakterien sind eigenständige Zellen, Viren benötigen eine Wirtszelle zur Vermehrung",
            "Bakterien sind eigenständige, sich selbst vermehrende Zellen, während Viren keinen eigenen Stoffwechsel haben und eine Wirtszelle zur Vermehrung kapern müssen."),
        ("Was versteht man unter der unspezifischen Immunabwehr?", new[] { "Eine allgemeine, angeborene Abwehr gegen viele verschiedene Erreger (z.B. Haut, Fresszellen)", "Eine Abwehr, die sich gezielt gegen einen einzigen bekannten Erreger richtet", "Ein anderes Wort für Impfung" }, "Eine allgemeine, angeborene Abwehr gegen viele verschiedene Erreger (z.B. Haut, Fresszellen)",
            "Die unspezifische (angeborene) Immunabwehr wirkt sofort und allgemein gegen viele Erreger, u.a. durch Haut als Barriere und Fresszellen (Phagozyten)."),
        ("Was kennzeichnet die spezifische Immunabwehr im Unterschied zur unspezifischen?", new[] { "Sie richtet sich gezielt gegen einen bestimmten Erreger und bildet ein Immungedächtnis", "Sie wirkt sofort gegen jeden beliebigen Erreger ohne Unterschied", "Sie hat mit Antikörpern nichts zu tun" }, "Sie richtet sich gezielt gegen einen bestimmten Erreger und bildet ein Immungedächtnis",
            "Die spezifische Immunabwehr erkennt gezielt einen bestimmten Erreger (über Antigene) und bildet passende Antikörper sowie ein Immungedächtnis."),
        ("Was sind Antikörper?", new[] { "Von B-Lymphozyten gebildete Eiweiße, die gezielt an bestimmte Antigene binden", "Ein anderes Wort für Bakterien", "Zellen, die nur bei Viruserkrankungen vorkommen" }, "Von B-Lymphozyten gebildete Eiweiße, die gezielt an bestimmte Antigene binden",
            "Antikörper sind spezifische Eiweißmoleküle, die von B-Lymphozyten produziert werden und passgenau an ein bestimmtes Antigen binden."),
        ("Was ist die Aufgabe von Fresszellen (Phagozyten) im Immunsystem?", new[] { "Sie nehmen Krankheitserreger auf und bauen sie ab", "Sie produzieren ausschließlich Hormone", "Sie speichern die Erbinformation" }, "Sie nehmen Krankheitserreger auf und bauen sie ab",
            "Fresszellen (Phagozyten) gehören zur unspezifischen Abwehr und bauen Krankheitserreger durch Aufnahme (Phagozytose) ab."),
        ("Was passiert bei einer aktiven Immunisierung (Impfung)?", new[] { "Der Körper wird mit abgeschwächten/inaktiven Erregerbestandteilen konfrontiert und bildet selbst Antikörper", "Fertige Antikörper werden direkt von außen zugeführt", "Es passiert gar nichts im Körper" }, "Der Körper wird mit abgeschwächten/inaktiven Erregerbestandteilen konfrontiert und bildet selbst Antikörper",
            "Bei der aktiven Immunisierung regt ein Impfstoff das Immunsystem an, selbst spezifische Antikörper und ein Immungedächtnis zu bilden."),
        ("Was passiert bei einer passiven Immunisierung?", new[] { "Fertige Antikörper werden direkt verabreicht, ohne dass der Körper sie selbst bildet", "Der Körper bildet über Wochen selbst Antikörper", "Es handelt sich um dieselbe Methode wie die aktive Immunisierung" }, "Fertige Antikörper werden direkt verabreicht, ohne dass der Körper sie selbst bildet",
            "Bei der passiven Immunisierung werden fertige Antikörper direkt zugeführt - der Schutz wirkt sofort, hält aber nicht dauerhaft an, da kein eigenes Immungedächtnis entsteht."),
        ("Warum kann man nach einer überstandenen Infektionskrankheit oft nicht noch einmal an derselben Krankheit erkranken?", new[] { "Das Immunsystem hat ein Immungedächtnis gebildet und reagiert beim erneuten Kontakt viel schneller", "Der Körper vergisst jeden Erreger sofort wieder", "Es gibt kein Immungedächtnis beim Menschen" }, "Das Immunsystem hat ein Immungedächtnis gebildet und reagiert beim erneuten Kontakt viel schneller",
            "Gedächtniszellen des Immunsystems ermöglichen bei erneutem Kontakt mit demselben Erreger eine deutlich schnellere und stärkere Abwehrreaktion."),
        ("Welches Virus verursacht die Immunschwächekrankheit AIDS?", new[] { "HIV (Humanes Immundefizienz-Virus)", "Das Grippevirus", "Das Masernvirus" }, "HIV (Humanes Immundefizienz-Virus)",
            "HIV greift gezielt bestimmte Zellen des Immunsystems (T-Helferzellen) an und schwächt dadurch die körpereigene Abwehr - im fortgeschrittenen Stadium spricht man von AIDS."),
        ("Warum gilt die Grippe (Influenza) jedes Jahr aufs Neue als Herausforderung für Impfstoffe?", new[] { "Das Grippevirus verändert sich (mutiert) häufig, sodass neue Impfstoffe nötig werden", "Das Grippevirus verändert sich nie", "Gegen Grippe kann man sich nicht impfen lassen" }, "Das Grippevirus verändert sich (mutiert) häufig, sodass neue Impfstoffe nötig werden",
            "Influenzaviren mutieren häufig, wodurch bestehende Immunität oder ältere Impfstoffe oft nicht mehr vollständig wirksam gegen neue Varianten sind."),
        ("Was ist der gesellschaftliche Nutzen einer hohen Impfquote in der Bevölkerung (\"Herdenimmunität\")?", new[] { "Auch Menschen, die selbst nicht geimpft werden können, werden indirekt geschützt", "Impfungen haben keinerlei gesellschaftlichen Effekt", "Eine hohe Impfquote schadet der Allgemeinheit" }, "Auch Menschen, die selbst nicht geimpft werden können, werden indirekt geschützt",
            "Ist ein großer Teil der Bevölkerung immun, kann sich ein Erreger schlechter verbreiten - das schützt auch Menschen, die z.B. aus medizinischen Gründen nicht geimpft werden können."),
        ("Was ist ein Antigen?", new[] { "Eine Struktur auf einem Erreger, die vom Immunsystem als körperfremd erkannt wird", "Ein anderes Wort für Antikörper", "Eine Körperzelle, die niemals erkannt wird" }, "Eine Struktur auf einem Erreger, die vom Immunsystem als körperfremd erkannt wird",
            "Antigene sind Oberflächenstrukturen von Erregern, die das spezifische Immunsystem als fremd erkennt und gegen die es Antikörper bildet."),
        ("Was sind T-Helferzellen im Immunsystem?", new[] { "Lymphozyten, die andere Immunzellen bei der spezifischen Abwehr koordinieren", "Zellen, die nur Sauerstoff transportieren", "Zellen, die ausschließlich Hormone produzieren" }, "Lymphozyten, die andere Immunzellen bei der spezifischen Abwehr koordinieren",
            "T-Helferzellen koordinieren die spezifische Immunantwort, u.a. indem sie B-Lymphozyten zur Antikörperbildung anregen."),
        ("Warum ist HIV besonders gefährlich für das Immunsystem selbst?", new[] { "Es befällt gezielt T-Helferzellen, die für die Koordination der Abwehr wichtig sind", "Es befällt ausschließlich rote Blutkörperchen", "Es hat keinerlei Wirkung auf das Immunsystem" }, "Es befällt gezielt T-Helferzellen, die für die Koordination der Abwehr wichtig sind",
            "Da HIV T-Helferzellen zerstört, die für die Koordination der spezifischen Abwehr zentral sind, wird das gesamte Immunsystem im Verlauf stark geschwächt."),
        ("Was bedeutet \"Antibiotikaresistenz\" bei Bakterien?", new[] { "Bakterien werden gegen bestimmte Antibiotika unempfindlich", "Bakterien werden durch Antibiotika immer sofort abgetötet", "Antibiotika wirken nur gegen Viren" }, "Bakterien werden gegen bestimmte Antibiotika unempfindlich",
            "Durch Mutationen und natürliche Selektion können Bakterienstämme entstehen, gegen die bestimmte Antibiotika nicht mehr wirken."),
        ("Warum wirken Antibiotika nicht gegen Viruserkrankungen wie Grippe?", new[] { "Antibiotika greifen bakterielle Strukturen an, die Viren nicht besitzen", "Antibiotika wirken gegen alle Krankheitserreger gleichermaßen", "Viren haben dieselbe Zellstruktur wie Bakterien" }, "Antibiotika greifen bakterielle Strukturen an, die Viren nicht besitzen",
            "Antibiotika wirken meist gegen bakterienspezifische Strukturen (z.B. Zellwand) - Viren haben diese Strukturen nicht, weshalb Antibiotika hier wirkungslos sind."),
        ("Was ist eine Autoimmunerkrankung?", new[] { "Das Immunsystem greift fälschlicherweise körpereigenes Gewebe an", "Eine Krankheit, die nur durch Bakterien ausgelöst wird", "Ein anderes Wort für eine erfolgreiche Impfung" }, "Das Immunsystem greift fälschlicherweise körpereigenes Gewebe an",
            "Bei Autoimmunerkrankungen erkennt das Immunsystem fälschlicherweise körpereigenes Gewebe als fremd und greift es an."),
        ("Was zeigen statistische Daten zu Masern in Ländern mit sinkender Impfquote häufig?", new[] { "Die Zahl der Erkrankungsfälle steigt tendenziell wieder an", "Die Erkrankung verschwindet automatisch von selbst", "Die Impfquote hat keinerlei Einfluss auf die Fallzahlen" }, "Die Zahl der Erkrankungsfälle steigt tendenziell wieder an",
            "Sinkt die Impfquote gegen hochansteckende Krankheiten wie Masern, steigen in der Regel auch die gemeldeten Erkrankungsfälle wieder an."),
        ("Was ist der Unterschied zwischen einer Epidemie und einer Pandemie?", new[] { "Eine Pandemie betrifft im Unterschied zur regional begrenzten Epidemie viele Länder oder die ganze Welt", "Beide Begriffe bedeuten exakt dasselbe", "Eine Epidemie ist immer weltweit, eine Pandemie nur lokal" }, "Eine Pandemie betrifft im Unterschied zur regional begrenzten Epidemie viele Länder oder die ganze Welt",
            "Eine Epidemie ist meist regional oder national begrenzt, eine Pandemie breitet sich dagegen über mehrere Länder oder Kontinente aus."),
        ("Warum werden manche Impfstoffe in mehreren Dosen (Auffrischimpfungen) verabreicht?", new[] { "Um einen möglichst starken und langanhaltenden Immunschutz mit stabilem Immungedächtnis aufzubauen", "Weil eine einzige Impfdosis grundsätzlich schädlich wäre", "Weil Impfstoffe sonst gar keine Wirkung zeigen würden" }, "Um einen möglichst starken und langanhaltenden Immunschutz mit stabilem Immungedächtnis aufzubauen",
            "Auffrischimpfungen sollen das Immungedächtnis stärken und einen möglichst zuverlässigen, lang anhaltenden Schutz sicherstellen.")
    };

    private static QuizQuestion Immunsystem(Random r)
    {
        var f = ImmunsystemListe[r.Next(ImmunsystemListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gesundheit und Krankheit (Immunologie)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Unspezifische Abwehr wirkt sofort gegen viele Erreger (z.B. Fresszellen), spezifische Abwehr bildet gezielt Antikörper und ein Immungedächtnis."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NervensystemListe =
    {
        ("Aus welchen Grundbestandteilen besteht eine Nervenzelle (Neuron) hauptsächlich?", new[] { "Zellkörper, Dendriten und Axon", "Nur einem einzigen runden Zellkörper ohne Fortsätze", "Zellwand und Chloroplasten" }, "Zellkörper, Dendriten und Axon",
            "Ein Neuron besteht aus dem Zellkörper mit Zellkern, den Dendriten (Reizaufnahme) und dem Axon (Reizweiterleitung)."),
        ("Was ist eine Synapse?", new[] { "Die Kontaktstelle zwischen zwei Nervenzellen zur Signalübertragung", "Ein anderes Wort für Zellkern", "Ein Sinnesorgan" }, "Die Kontaktstelle zwischen zwei Nervenzellen zur Signalübertragung",
            "An der Synapse wird ein Nervenimpuls meist chemisch über Botenstoffe (Neurotransmitter) von einer Zelle zur nächsten übertragen."),
        ("Was sind Neurotransmitter?", new[] { "Chemische Botenstoffe, die Signale an der Synapse übertragen", "Ein anderes Wort für Hormone der Schilddrüse", "Bestandteile der Zellwand" }, "Chemische Botenstoffe, die Signale an der Synapse übertragen",
            "Neurotransmitter werden am Ende eines Neurons freigesetzt und docken an Rezeptoren der nächsten Zelle an, um das Signal weiterzugeben."),
        ("Welche Aufgabe haben Sinnesorgane grundsätzlich?", new[] { "Sie nehmen Reize aus der Umwelt oder dem Körper auf", "Sie speichern ausschließlich Erbinformationen", "Sie produzieren Verdauungsenzyme" }, "Sie nehmen Reize aus der Umwelt oder dem Körper auf",
            "Sinnesorgane (z.B. Auge, Ohr, Haut) nehmen spezifische Reize auf und wandeln sie in Nervenimpulse um."),
        ("Was gehört zum zentralen Nervensystem (ZNS) des Menschen?", new[] { "Gehirn und Rückenmark", "Nur die Sinnesorgane", "Nur die Muskeln" }, "Gehirn und Rückenmark",
            "Das zentrale Nervensystem besteht aus Gehirn und Rückenmark und verarbeitet und steuert eingehende Informationen."),
        ("Was ist die Aufgabe des vegetativen (autonomen) Nervensystems?", new[] { "Es steuert unbewusste Körperfunktionen wie Herzschlag und Verdauung", "Es steuert ausschließlich bewusste, willkürliche Bewegungen", "Es hat keine erkennbare Funktion" }, "Es steuert unbewusste Körperfunktionen wie Herzschlag und Verdauung",
            "Das vegetative Nervensystem reguliert unwillkürliche Vorgänge wie Herzschlag, Atmung und Verdauung, meist ohne bewusste Kontrolle."),
        ("Was beschreibt eine Reiz-Reaktions-Kette?", new[] { "Den Weg eines Reizes vom Sinnesorgan über das Nervensystem bis zur motorischen Reaktion", "Eine zufällige, unzusammenhängende Abfolge von Zellprozessen", "Einen Vorgang, der nur in Pflanzenzellen stattfindet" }, "Den Weg eines Reizes vom Sinnesorgan über das Nervensystem bis zur motorischen Reaktion",
            "Eine Reiz-Reaktions-Kette beschreibt den Weg von der Reizaufnahme über die Verarbeitung im Nervensystem bis zur ausgelösten Reaktion, z.B. einer Muskelbewegung."),
        ("Was ist ein Reflex, z.B. das Zurückziehen der Hand bei einer heißen Herdplatte?", new[] { "Eine sehr schnelle, unwillkürliche Reaktion, oft über das Rückenmark gesteuert", "Eine langsame, bewusst durchdachte Entscheidung", "Ein Vorgang, der nur im Gehirn abläuft" }, "Eine sehr schnelle, unwillkürliche Reaktion, oft über das Rückenmark gesteuert",
            "Bei manchen Reflexen wird die Reaktion bereits im Rückenmark ausgelöst, bevor das Signal bewusst im Gehirn verarbeitet wird - das ermöglicht besonders schnelles Handeln."),
        ("Wie wird ein elektrisches Signal (Aktionspotenzial) entlang eines Axons weitergeleitet?", new[] { "Durch die Bewegung geladener Teilchen (Ionen) über die Zellmembran", "Durch direkten Kontakt mit dem Gehirn ohne jede Signalübertragung", "Durch chemische Reaktionen im Zellkern" }, "Durch die Bewegung geladener Teilchen (Ionen) über die Zellmembran",
            "Ein- und ausströmende Ionen (v.a. Natrium und Kalium) über die Zellmembran erzeugen das elektrische Signal, das entlang des Axons weiterläuft."),
        ("Was passiert am Ende eines Axons, wenn ein elektrisches Signal die Synapse erreicht?", new[] { "Neurotransmitter werden freigesetzt und wirken auf die nächste Zelle", "Das Signal verschwindet einfach spurlos", "Es entsteht eine neue Nervenzelle" }, "Neurotransmitter werden freigesetzt und wirken auf die nächste Zelle",
            "Am Ende des Axons löst das ankommende Signal die Ausschüttung von Neurotransmittern aus, die auf die nachfolgende Zelle wirken."),
        ("Warum verlaufen manche Nervenimpulse im Körper besonders schnell?", new[] { "Eine isolierende Markscheide (Myelinscheide) um das Axon beschleunigt die Signalweiterleitung", "Weil alle Nervenzellen exakt gleich schnell leiten", "Weil das Signal dabei gar nicht durch Axone verläuft" }, "Eine isolierende Markscheide (Myelinscheide) um das Axon beschleunigt die Signalweiterleitung",
            "Die Myelinscheide isoliert das Axon und ermöglicht eine sprunghafte, dadurch schnellere Signalweiterleitung."),
        ("Welche Rolle spielt das Rückenmark im Nervensystem?", new[] { "Es leitet Signale zwischen Gehirn und Körper weiter und steuert Reflexe", "Es speichert ausschließlich Langzeiterinnerungen", "Es hat mit dem Nervensystem nichts zu tun" }, "Es leitet Signale zwischen Gehirn und Körper weiter und steuert Reflexe",
            "Das Rückenmark verbindet Gehirn und übrigen Körper über Nervenbahnen und kann bestimmte Reflexe eigenständig steuern."),
        ("Was unterscheidet das sympathische vom parasympathischen Nervensystem (beide Teil des vegetativen Nervensystems)?", new[] { "Sympathikus aktiviert den Körper (z.B. bei Stress), Parasympathikus fördert Erholung", "Beide haben exakt dieselbe Funktion", "Nur der Parasympathikus gehört zum vegetativen Nervensystem" }, "Sympathikus aktiviert den Körper (z.B. bei Stress), Parasympathikus fördert Erholung",
            "Der Sympathikus versetzt den Körper in erhöhte Leistungsbereitschaft (z.B. schnellerer Herzschlag), der Parasympathikus fördert Ruhe, Verdauung und Erholung."),
        ("Was passiert, wenn ein Sinnesorgan wie das Auge einen Reiz aufnimmt?", new[] { "Der Reiz wird in ein elektrisches Signal umgewandelt und über Nervenbahnen weitergeleitet", "Der Reiz bleibt ausschließlich im Sinnesorgan selbst", "Es entsteht sofort eine Muskelbewegung ohne jede Signalverarbeitung" }, "Der Reiz wird in ein elektrisches Signal umgewandelt und über Nervenbahnen weitergeleitet",
            "Sinneszellen wandeln physikalische oder chemische Reize (z.B. Licht) in elektrische Signale um, die über Nervenbahnen zum Gehirn geleitet werden."),
        ("Was versteht man unter der Reizschwelle einer Nervenzelle?", new[] { "Die Mindeststärke eines Reizes, ab der ein Nervenimpuls ausgelöst wird", "Die maximale Anzahl an Nervenzellen im Körper", "Ein anderes Wort für Synapse" }, "Die Mindeststärke eines Reizes, ab der ein Nervenimpuls ausgelöst wird",
            "Erst wenn ein Reiz eine bestimmte Reizschwelle überschreitet, wird ein Aktionspotenzial (Nervenimpuls) in der Zelle ausgelöst (Alles-oder-Nichts-Prinzip)."),
        ("Wodurch unterscheiden sich verschiedene Sinnesorgane in der Art der aufgenommenen Reize?", new[] { "Jedes Sinnesorgan ist auf eine bestimmte Reizart spezialisiert, z.B. Auge auf Licht, Ohr auf Schall", "Alle Sinnesorgane nehmen exakt dieselben Reize wahr", "Sinnesorgane nehmen überhaupt keine Reize auf" }, "Jedes Sinnesorgan ist auf eine bestimmte Reizart spezialisiert, z.B. Auge auf Licht, Ohr auf Schall",
            "Sinnesorgane sind auf bestimmte Reizarten spezialisiert: Das Auge reagiert auf Licht, das Ohr auf Schallwellen, die Haut u.a. auf Druck und Temperatur."),
        ("Was passiert bei einer Schädigung des Rückenmarks häufig mit der Signalweiterleitung zu darunterliegenden Körperbereichen?", new[] { "Sie kann teilweise oder vollständig unterbrochen sein, was zu Lähmungen führen kann", "Die Signalweiterleitung wird automatisch verbessert", "Es hat keinerlei Auswirkung auf die Körperfunktionen" }, "Sie kann teilweise oder vollständig unterbrochen sein, was zu Lähmungen führen kann",
            "Da das Rückenmark Signale zwischen Gehirn und Körper weiterleitet, kann eine Schädigung zu teilweisen oder vollständigen Lähmungen unterhalb der Verletzungsstelle führen."),
        ("Warum reagiert der Körper bei Gefahr oft mit erhöhtem Puls und schnellerer Atmung?", new[] { "Der Sympathikus aktiviert den Körper für eine schnelle Reaktion (\"Kampf oder Flucht\")", "Das Nervensystem hat damit nichts zu tun", "Es handelt sich um eine zufällige, ungesteuerte Reaktion" }, "Der Sympathikus aktiviert den Körper für eine schnelle Reaktion (\"Kampf oder Flucht\")",
            "Der Sympathikus versetzt den Körper in Alarmbereitschaft, u.a. durch erhöhten Puls und schnellere Atmung, um schnell reagieren zu können."),
        ("Was passiert grundsätzlich, wenn ein Neurotransmitter an einen Rezeptor der nachfolgenden Nervenzelle andockt?", new[] { "Er kann dort ein neues elektrisches Signal auslösen oder hemmen", "Er zerstört die nachfolgende Zelle vollständig", "Es passiert überhaupt keine Reaktion" }, "Er kann dort ein neues elektrisches Signal auslösen oder hemmen",
            "Je nach Neurotransmitter und Rezeptor kann die Signalübertragung an der Synapse ein neues Signal in der Folgezelle auslösen oder es hemmen."),
        ("Was passiert mit ausgeschütteten Neurotransmittern im synaptischen Spalt, nachdem sie ihre Wirkung entfaltet haben?", new[] { "Sie werden abgebaut oder wiederaufgenommen, damit das Signal nicht dauerhaft anhält", "Sie bleiben für immer im synaptischen Spalt aktiv", "Sie wandeln sich sofort in Antikörper um" }, "Sie werden abgebaut oder wiederaufgenommen, damit das Signal nicht dauerhaft anhält",
            "Enzyme bauen Neurotransmitter ab oder die abgebende Zelle nimmt sie wieder auf, damit die Synapse für ein neues Signal bereit ist.")
    };

    private static QuizQuestion Nervensystem(Random r)
    {
        var f = NervensystemListe[r.Next(NervensystemListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Bau und Funktion des Nervensystems", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Reiz wird vom Sinnesorgan aufgenommen, als elektrisches Signal über Neuronen geleitet und an Synapsen chemisch (Neurotransmitter) weitergegeben."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SuchtListe =
    {
        ("Wie wirken viele Suchtmittel (z.B. Alkohol, bestimmte Drogen) grundlegend auf das Nervensystem?", new[] { "Sie beeinflussen die Signalübertragung an Synapsen, z.B. über Neurotransmitter", "Sie haben überhaupt keine Wirkung auf Nervenzellen", "Sie wirken ausschließlich auf die Verdauung" }, "Sie beeinflussen die Signalübertragung an Synapsen, z.B. über Neurotransmitter",
            "Viele Suchtmittel greifen gezielt in die Signalübertragung an Synapsen ein, indem sie z.B. die Ausschüttung oder Wirkung von Neurotransmittern verändern."),
        ("Was versteht man unter physiologischer (körperlicher) Abhängigkeit?", new[] { "Der Körper hat sich an eine Substanz gewöhnt und reagiert bei Entzug mit körperlichen Symptomen", "Ein rein psychisches Verlangen ohne jede körperliche Reaktion", "Ein Zustand, der niemals bei Suchtmitteln auftritt" }, "Der Körper hat sich an eine Substanz gewöhnt und reagiert bei Entzug mit körperlichen Symptomen",
            "Bei körperlicher Abhängigkeit hat sich der Organismus an eine Substanz angepasst - beim Absetzen treten körperliche Entzugserscheinungen auf."),
        ("Was ist psychische Abhängigkeit?", new[] { "Ein starkes seelisches Verlangen nach einer Substanz oder einem Verhalten", "Ausschließlich eine körperliche Reaktion ohne jedes Verlangen", "Ein anderes Wort für Immunität" }, "Ein starkes seelisches Verlangen nach einer Substanz oder einem Verhalten",
            "Bei psychischer Abhängigkeit besteht ein starkes inneres Verlangen (Craving) nach der Substanz oder dem Verhalten, unabhängig von rein körperlichen Symptomen."),
        ("Wie beeinflusst Alkohol typischerweise die Signalübertragung im Gehirn?", new[] { "Er verstärkt hemmende Signale und verlangsamt dadurch Reaktionen", "Er hat gar keine Wirkung auf das Gehirn", "Er beschleunigt ausschließlich alle Reaktionen des Körpers" }, "Er verstärkt hemmende Signale und verlangsamt dadurch Reaktionen",
            "Alkohol verstärkt u.a. hemmende Neurotransmitter im Gehirn, was Reaktionsfähigkeit, Koordination und Urteilsvermögen verlangsamt und beeinträchtigt."),
        ("Was passiert bei einer sogenannten Toleranzentwicklung gegenüber einer Substanz?", new[] { "Der Körper benötigt zunehmend höhere Dosen für dieselbe Wirkung", "Der Körper reagiert mit der Zeit immer empfindlicher auf kleinste Mengen", "Toleranzentwicklung hat mit Sucht nichts zu tun" }, "Der Körper benötigt zunehmend höhere Dosen für dieselbe Wirkung",
            "Bei Toleranzentwicklung gewöhnt sich der Körper an eine Substanz, sodass zunehmend höhere Dosen für denselben Effekt nötig werden."),
        ("Was ist ein Beispiel für eine Verhaltenssucht ohne eine chemische Substanz?", new[] { "Glücksspielsucht", "Grippe", "Diabetes" }, "Glücksspielsucht",
            "Auch ohne den Konsum einer Substanz kann eine Abhängigkeit entstehen, z.B. bei Glücksspiel- oder Mediennutzungssucht."),
        ("Warum spricht man bei der Entstehung von Sucht von \"Multikausalität\"?", new[] { "Mehrere Ursachen (biologisch, psychologisch, sozial) wirken meist zusammen", "Es gibt immer nur eine einzige, klar bestimmbare Ursache", "Sucht entsteht rein zufällig ohne jeden erkennbaren Grund" }, "Mehrere Ursachen (biologisch, psychologisch, sozial) wirken meist zusammen",
            "Sucht entsteht meist durch ein Zusammenspiel biologischer (z.B. Veranlagung), psychologischer (z.B. Stress) und sozialer (z.B. Gruppendruck) Faktoren."),
        ("Was ist ein Beispiel für eine soziale Ursache, die zu Sucht beitragen kann?", new[] { "Gruppendruck oder das Umfeld von Freunden und Familie", "Nur die genetische Veranlagung allein", "Ausschließlich die chemische Struktur der Substanz" }, "Gruppendruck oder das Umfeld von Freunden und Familie",
            "Soziales Umfeld, Gruppendruck und familiäre Vorbilder können das Risiko für die Entstehung einer Sucht mit beeinflussen."),
        ("Was bedeutet Suchtprävention?", new[] { "Maßnahmen, die das Entstehen einer Sucht von vornherein verhindern sollen", "Maßnahmen, die eine bestehende Sucht verstärken sollen", "Ein anderes Wort für Impfung" }, "Maßnahmen, die das Entstehen einer Sucht von vornherein verhindern sollen",
            "Suchtprävention umfasst Aufklärung, Förderung von Selbstbewusstsein und alternative Bewältigungsstrategien, um Sucht vorzubeugen."),
        ("Warum sind Jugendliche laut Forschung teilweise anfälliger für die Entwicklung von Süchten als Erwachsene?", new[] { "Das Gehirn, insbesondere Bereiche für Impulskontrolle, ist noch in der Entwicklung", "Jugendliche haben ein bereits vollständig ausgereiftes Gehirn", "Es gibt keinerlei Unterschied zwischen Jugendlichen und Erwachsenen" }, "Das Gehirn, insbesondere Bereiche für Impulskontrolle, ist noch in der Entwicklung",
            "Da sich Gehirnbereiche für Impulskontrolle und Entscheidungsfindung bei Jugendlichen noch entwickeln, gelten sie oft als anfälliger für risikoreiches Verhalten und Suchtentwicklung."),
        ("Wie wirken sich manche Suchtmittel auf das Belohnungssystem des Gehirns aus?", new[] { "Sie lösen eine übermäßige Ausschüttung von Botenstoffen wie Dopamin aus", "Sie haben keinerlei Einfluss auf das Belohnungssystem", "Sie blockieren jede Art von Botenstoff vollständig" }, "Sie lösen eine übermäßige Ausschüttung von Botenstoffen wie Dopamin aus",
            "Viele Suchtmittel erhöhen die Ausschüttung von Dopamin im Belohnungssystem des Gehirns, was ein starkes Verlangen nach Wiederholung erzeugen kann."),
        ("Was ist ein möglicher Auslöser für einen Rückfall nach einer erfolgreichen Suchttherapie?", new[] { "Stress oder eine Rückkehr in das alte soziale Umfeld", "Ein Rückfall ist grundsätzlich ausgeschlossen", "Nur körperliche Erkrankungen können einen Rückfall auslösen" }, "Stress oder eine Rückkehr in das alte soziale Umfeld",
            "Belastende Situationen wie Stress oder die Rückkehr in ein Umfeld, das mit dem früheren Konsum verbunden ist, können das Rückfallrisiko erhöhen."),
        ("Was unterscheidet Medikamentenmissbrauch von der bestimmungsgemäßen Einnahme von Medikamenten?", new[] { "Missbrauch bedeutet eine unsachgemäße, oft übermäßige Einnahme entgegen ärztlicher Verordnung", "Beides bedeutet exakt dasselbe", "Medikamente können grundsätzlich nicht missbräuchlich verwendet werden" }, "Missbrauch bedeutet eine unsachgemäße, oft übermäßige Einnahme entgegen ärztlicher Verordnung",
            "Medikamentenmissbrauch liegt vor, wenn Medikamente unsachgemäß, in zu hoher Dosis oder ohne medizinischen Grund eingenommen werden."),
        ("Warum gelten manche Dopingmittel im Sport auch als gesundheitlich riskant für das Nervensystem?", new[] { "Sie können ähnlich wie andere Suchtmittel in die Signalübertragung des Körpers eingreifen", "Dopingmittel haben grundsätzlich keine Nebenwirkungen", "Doping betrifft ausschließlich die Muskulatur, nie das Nervensystem" }, "Sie können ähnlich wie andere Suchtmittel in die Signalübertragung des Körpers eingreifen",
            "Manche Dopingsubstanzen beeinflussen wie andere psychoaktive Stoffe die Signalübertragung im Körper und können gesundheitliche Risiken für das Nervensystem bergen."),
        ("Was kann eine wirksame Strategie zur Suchtprävention bei Jugendlichen sein?", new[] { "Förderung von Selbstwertgefühl, Stressbewältigung und Aufklärung über Risiken", "Das Thema komplett zu verschweigen", "Ausschließlich strenge Verbote ohne jede Aufklärung" }, "Förderung von Selbstwertgefühl, Stressbewältigung und Aufklärung über Risiken",
            "Wirksame Prävention setzt oft auf Stärkung von Selbstwertgefühl und Bewältigungsstrategien kombiniert mit sachlicher Aufklärung über Risiken."),
        ("Was zeigt der Vergleich verschiedener Suchtmittel hinsichtlich ihrer Wirkung auf Synapsen?", new[] { "Unterschiedliche Substanzen beeinflussen die Signalübertragung auf unterschiedliche, spezifische Weise", "Alle Suchtmittel wirken auf exakt identische Weise", "Suchtmittel haben grundsätzlich keinerlei Wirkung auf Synapsen" }, "Unterschiedliche Substanzen beeinflussen die Signalübertragung auf unterschiedliche, spezifische Weise",
            "Verschiedene Suchtmittel greifen auf unterschiedliche Weise in die Signalübertragung an Synapsen ein, z.B. durch Nachahmung, Blockade oder verstärkte Ausschüttung von Neurotransmittern."),
        ("Was ist ein Grund, warum Suchtprävention oft schon in der Schule ansetzt?", new[] { "Frühzeitige Aufklärung kann das Risiko für spätere Suchtentwicklung verringern", "Schule hat mit Suchtprävention grundsätzlich nichts zu tun", "Prävention wirkt ausschließlich bei bereits erwachsenen Menschen" }, "Frühzeitige Aufklärung kann das Risiko für spätere Suchtentwicklung verringern",
            "Da sich Verhaltensmuster und Einstellungen oft schon im Jugendalter entwickeln, gilt frühzeitige, altersgerechte Aufklärung als wichtiger Baustein der Suchtprävention."),
        ("Was ist ein wichtiger Unterschied zwischen gelegentlichem Substanzkonsum und einer Sucht?", new[] { "Bei einer Sucht besteht ein starkes, oft unkontrollierbares Verlangen trotz negativer Folgen", "Beides bedeutet exakt dasselbe", "Gelegentlicher Konsum ist per Definition immer eine Sucht" }, "Bei einer Sucht besteht ein starkes, oft unkontrollierbares Verlangen trotz negativer Folgen",
            "Eine Sucht zeichnet sich durch anhaltendes, oft unkontrollierbares Verlangen trotz erkennbarer negativer Folgen für Gesundheit, Beziehungen oder Alltag aus."),
        ("Warum kann der Konsum von Suchtmitteln während der Pubertät besonders schädlich für die Gehirnentwicklung sein?", new[] { "Das Gehirn befindet sich noch in einer sensiblen Entwicklungsphase", "Das jugendliche Gehirn ist gegen jede äußere Einwirkung völlig unempfindlich", "Suchtmittel wirken bei Jugendlichen grundsätzlich schwächer als bei Erwachsenen" }, "Das Gehirn befindet sich noch in einer sensiblen Entwicklungsphase",
            "Da sich wichtige Gehirnstrukturen bei Jugendlichen noch entwickeln, kann der Konsum von Suchtmitteln diese Entwicklung besonders stark beeinträchtigen."),
        ("Was versteht man unter \"Co-Abhängigkeit\" im Umfeld einer suchtkranken Person?", new[] { "Nahestehende Personen unterstützen unbewusst das Suchtverhalten mit, statt es zu unterbinden", "Ein anderes Wort für die Sucht selbst", "Ein Zustand, der ausschließlich bei der süchtigen Person selbst auftritt" }, "Nahestehende Personen unterstützen unbewusst das Suchtverhalten mit, statt es zu unterbinden",
            "Bei Co-Abhängigkeit decken oder erleichtern nahestehende Personen ungewollt das Suchtverhalten, z.B. durch Beschönigen oder Vertuschen der Folgen.")
    };

    private static QuizQuestion SuchtUndSuchtpraevention(Random r)
    {
        var f = SuchtListe[r.Next(SuchtListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Sucht und Suchtprävention", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Suchtmittel greifen oft in die Signalübertragung an Synapsen ein; Sucht entsteht meist durch ein Zusammenspiel biologischer, psychischer und sozialer Ursachen (Multikausalität)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HumangenetikListe =
    {
        ("Was ist der Unterschied zwischen Mitose und Meiose?", new[] { "Mitose bildet zwei identische Körperzellen, Meiose bildet vier unterschiedliche Keimzellen mit halbem Chromosomensatz", "Beide Prozesse führen zu exakt demselben Ergebnis", "Meiose findet nur in Pflanzenzellen statt" }, "Mitose bildet zwei identische Körperzellen, Meiose bildet vier unterschiedliche Keimzellen mit halbem Chromosomensatz",
            "Die Mitose dient dem Wachstum und erzeugt zwei genetisch identische Tochterzellen, die Meiose erzeugt vier genetisch unterschiedliche Keimzellen mit halbiertem Chromosomensatz."),
        ("Warum ist die Halbierung des Chromosomensatzes bei der Meiose für die Fortpflanzung wichtig?", new[] { "So entsteht bei der Befruchtung wieder der volle, arttypische Chromosomensatz", "Ohne Halbierung könnten sich Zellen nicht teilen", "Die Halbierung hat mit Fortpflanzung nichts zu tun" }, "So entsteht bei der Befruchtung wieder der volle, arttypische Chromosomensatz",
            "Da Ei- und Samenzelle je einen halben Chromosomensatz besitzen, ergibt sich bei ihrer Verschmelzung wieder der volle, arttypische Chromosomensatz."),
        ("Was ist ein Karyogramm?", new[] { "Eine geordnete Darstellung aller Chromosomen eines Menschen zur Untersuchung von Anzahl und Struktur", "Ein anderes Wort für Stammbaum", "Eine Darstellung des Blutkreislaufs" }, "Eine geordnete Darstellung aller Chromosomen eines Menschen zur Untersuchung von Anzahl und Struktur",
            "Ein Karyogramm ordnet die Chromosomen einer Zelle nach Größe und Form und kann so z.B. auf Chromosomenanomalien hinweisen."),
        ("Was zeigt ein Karyogramm bei Trisomie 21 (Down-Syndrom)?", new[] { "Ein zusätzliches, drittes Chromosom 21 statt der üblichen zwei Kopien", "Ein vollständig fehlendes Chromosom 21", "Ein völlig normales Chromosomenbild ohne jede Abweichung" }, "Ein zusätzliches, drittes Chromosom 21 statt der üblichen zwei Kopien",
            "Bei Trisomie 21 liegt Chromosom 21 dreifach statt zweifach vor, was im Karyogramm sichtbar wird."),
        ("Wie wird die Blutgruppe im AB0-System hauptsächlich bestimmt?", new[] { "Durch bestimmte Merkmale (Antigene) auf der Oberfläche der roten Blutkörperchen", "Durch die Augenfarbe eines Menschen", "Durch das Körpergewicht" }, "Durch bestimmte Merkmale (Antigene) auf der Oberfläche der roten Blutkörperchen",
            "Die Blutgruppen A, B, AB und 0 ergeben sich aus bestimmten Antigen-Merkmalen auf der Oberfläche der roten Blutkörperchen, die genetisch festgelegt sind."),
        ("Welche Blutgruppe gilt im AB0-System oft als \"Universalspender\" für Blutkonserven?", new[] { "Blutgruppe 0", "Blutgruppe AB", "Blutgruppe B" }, "Blutgruppe 0",
            "Blutgruppe 0 (insbesondere 0 negativ) kann in Notfällen oft an Empfänger verschiedener Blutgruppen gespendet werden, da ihr die A- und B-Antigene fehlen."),
        ("Wie werden die Allele für die Blutgruppen A, B und 0 im AB0-System grundsätzlich vererbt?", new[] { "A und B sind dominant über 0, A und B zueinander sind kodominant", "Nur die Blutgruppe 0 kann überhaupt vererbt werden", "Blutgruppen werden zufällig, unabhängig von den Eltern, bestimmt" }, "A und B sind dominant über 0, A und B zueinander sind kodominant",
            "Die Allele A und B sind gegenüber 0 dominant, verhalten sich zueinander aber kodominant - bei AB-Genotyp werden deshalb beide Merkmale gleichzeitig ausgeprägt."),
        ("Was ist eine Genmutation?", new[] { "Eine Veränderung in der Basensequenz eines einzelnen Gens", "Eine Veränderung, die ausschließlich das gesamte Chromosom betrifft", "Ein anderes Wort für Zellteilung" }, "Eine Veränderung in der Basensequenz eines einzelnen Gens",
            "Bei einer Genmutation verändert sich die Basensequenz innerhalb eines einzelnen Gens, was sich auf das codierte Protein auswirken kann."),
        ("Was versteht man unter \"modifikatorischer Variabilität\" im Unterschied zu einer Mutation?", new[] { "Merkmalsunterschiede durch Umwelteinflüsse, ohne dass sich die Erbinformation ändert", "Eine dauerhafte Veränderung der DNA-Sequenz", "Ein anderes Wort für eine Chromosomenmutation" }, "Merkmalsunterschiede durch Umwelteinflüsse, ohne dass sich die Erbinformation ändert",
            "Modifikatorische Variabilität entsteht durch Umwelteinflüsse (z.B. Ernährung, Training) auf bereits vorhandene Erbanlagen, ohne dass sich die DNA selbst verändert."),
        ("Was ist ein Beispiel für eine genetisch bedingte Erbkrankheit beim Menschen?", new[] { "Mukoviszidose", "Eine Erkältung", "Ein Sonnenbrand" }, "Mukoviszidose",
            "Mukoviszidose ist eine erblich bedingte Stoffwechselkrankheit, die durch eine Genmutation verursacht wird und Lunge und Verdauungssystem betrifft."),
        ("Was ist die Aufgabe eines Familienstammbaums in der Humangenetik?", new[] { "Er zeigt das Auftreten eines Merkmals über mehrere Generationen und hilft, den Erbgang zu erkennen", "Er zeigt ausschließlich den Wohnort der Familie", "Er hat keinerlei Aussagekraft für Vererbung" }, "Er zeigt das Auftreten eines Merkmals über mehrere Generationen und hilft, den Erbgang zu erkennen",
            "Anhand eines Familienstammbaums lässt sich oft ableiten, ob ein Merkmal dominant, rezessiv oder geschlechtsgebunden vererbt wird."),
        ("Was ist ein zentrales Ziel der genetischen Beratung bei Kinderwunsch?", new[] { "Eltern über mögliche genetisch bedingte Risiken für ihr Kind sachlich zu informieren", "Eltern eine bestimmte Entscheidung vorzuschreiben", "Ausschließlich die Blutgruppe des Kindes vorherzusagen" }, "Eltern über mögliche genetisch bedingte Risiken für ihr Kind sachlich zu informieren",
            "Genetische Beratung informiert werdende Eltern sachlich über mögliche erbliche Risiken, ohne ihnen eine bestimmte Entscheidung vorzuschreiben."),
        ("Was ist pränatale Diagnostik?", new[] { "Untersuchungsmethoden, die schon vor der Geburt Hinweise auf die Gesundheit des Kindes liefern können", "Eine Untersuchung, die erst nach der Geburt durchgeführt wird", "Ein anderes Wort für Impfung" }, "Untersuchungsmethoden, die schon vor der Geburt Hinweise auf die Gesundheit des Kindes liefern können",
            "Pränatale Diagnostik umfasst Untersuchungen während der Schwangerschaft, die z.B. Hinweise auf bestimmte genetisch bedingte Erkrankungen des ungeborenen Kindes geben können."),
        ("Warum wird pränatale Diagnostik auch ethisch kontrovers diskutiert?", new[] { "Ergebnisse können schwierige Entscheidungen über den weiteren Schwangerschaftsverlauf aufwerfen", "Weil sie überhaupt keine Ergebnisse liefert", "Weil sie gesetzlich in jedem Fall verboten ist" }, "Ergebnisse können schwierige Entscheidungen über den weiteren Schwangerschaftsverlauf aufwerfen",
            "Da Untersuchungsergebnisse Eltern vor schwierige ethische Entscheidungen stellen können, wird der Einsatz pränataler Diagnostik gesellschaftlich kontrovers diskutiert."),
        ("Was bestimmt beim Menschen in der Regel das biologische Geschlecht auf Chromosomenebene?", new[] { "Die Kombination der Geschlechtschromosomen (XX oder XY)", "Die Blutgruppe der Mutter", "Der Geburtsmonat des Kindes" }, "Die Kombination der Geschlechtschromosomen (XX oder XY)",
            "Zwei X-Chromosomen führen in der Regel zu weiblichem, ein X- und ein Y-Chromosom zu männlichem biologischem Geschlecht."),
        ("Was ist ein Beispiel für ein geschlechtsgebunden vererbtes Merkmal beim Menschen?", new[] { "Rot-Grün-Sehschwäche", "Augenfarbe im Allgemeinen", "Körpergröße im Allgemeinen" }, "Rot-Grün-Sehschwäche",
            "Das Gen für die Rot-Grün-Sehschwäche liegt auf dem X-Chromosom, weshalb sie bei Männern (nur ein X-Chromosom) häufiger auftritt als bei Frauen."),
        ("Was passiert bei einer Chromosomenmutation im Unterschied zu einer Genmutation?", new[] { "Die Struktur oder Anzahl ganzer Chromosomen verändert sich", "Nur ein einzelnes Basenpaar eines Gens verändert sich", "Es passiert exakt dasselbe wie bei einer Genmutation" }, "Die Struktur oder Anzahl ganzer Chromosomen verändert sich",
            "Chromosomenmutationen betreffen größere Abschnitte oder ganze Chromosomen (z.B. zusätzliche oder fehlende Chromosomen), Genmutationen dagegen einzelne Gene."),
        ("Was kann bei der Auswertung eines Familienstammbaums auf einen rezessiven Erbgang hindeuten?", new[] { "Ein Merkmal tritt nur auf, wenn beide Elternteile das entsprechende Allel tragen, auch wenn sie selbst gesund erscheinen", "Das Merkmal tritt bei jedem einzelnen Nachkommen zwingend auf", "Rezessive Erbgänge lassen sich nie an Stammbäumen erkennen" }, "Ein Merkmal tritt nur auf, wenn beide Elternteile das entsprechende Allel tragen, auch wenn sie selbst gesund erscheinen",
            "Bei rezessiven Erbgängen können äußerlich gesunde Eltern (Träger eines rezessiven Allels) ein betroffenes Kind bekommen, wenn beide das Allel weitergeben."),
        ("Warum ist die Meiose entscheidend für die genetische Vielfalt innerhalb einer Art?", new[] { "Durch Neukombination der Chromosomen entstehen genetisch unterschiedliche Keimzellen", "Die Meiose erzeugt immer genetisch identische Keimzellen", "Genetische Vielfalt entsteht ausschließlich durch Mutationen, nie durch Meiose" }, "Durch Neukombination der Chromosomen entstehen genetisch unterschiedliche Keimzellen",
            "Während der Meiose werden mütterliche und väterliche Chromosomenanteile neu kombiniert, wodurch genetisch unterschiedliche Keimzellen und damit Vielfalt entstehen."),
        ("Was ist eine mögliche Ursache für eine Trisomie wie das Down-Syndrom bei der Keimzellbildung?", new[] { "Eine Fehlverteilung der Chromosomen während der Meiose (Non-Disjunction)", "Eine bewusste Entscheidung der Eltern", "Ein Fehler, der ausschließlich nach der Geburt entstehen kann" }, "Eine Fehlverteilung der Chromosomen während der Meiose (Non-Disjunction)",
            "Trennen sich Chromosomen während der Meiose nicht korrekt (Non-Disjunction), kann eine Keimzelle mit einem zusätzlichen oder fehlenden Chromosom entstehen.")
    };

    private static QuizQuestion Humangenetik(Random r)
    {
        var f = HumangenetikListe[r.Next(HumangenetikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Vererbung beim Menschen (Humangenetik)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Mitose erzeugt identische Körperzellen, Meiose halbierte, unterschiedliche Keimzellen; Karyogramme und Stammbäume helfen, Erbgänge und Chromosomenbesonderheiten zu erkennen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EvolutionListe =
    {
        ("Wer entwickelte die Evolutionstheorie der natürlichen Selektion, die bis heute als grundlegend gilt?", new[] { "Charles Darwin", "Gregor Mendel", "Jean-Baptiste de Lamarck" }, "Charles Darwin",
            "Charles Darwin veröffentlichte 1859 seine Theorie der natürlichen Selektion (\"Die Entstehung der Arten\"), die bis heute die Grundlage der modernen Evolutionsbiologie bildet."),
        ("Was besagte Lamarcks Theorie der \"Vererbung erworbener Eigenschaften\" (vereinfacht)?", new[] { "Im Laufe des Lebens erworbene Merkmale (z.B. durch Training) würden an die Nachkommen weitergegeben", "Nur zufällige Mutationen könnten vererbt werden", "Erworbene Eigenschaften hätten mit Vererbung nichts zu tun" }, "Im Laufe des Lebens erworbene Merkmale (z.B. durch Training) würden an die Nachkommen weitergegeben",
            "Lamarck nahm an, dass z.B. ein durch häufiges Strecken \"verlängerter\" Hals an die nächste Generation weitergegeben würde - diese Annahme gilt heute als widerlegt."),
        ("Warum gilt Lamarcks Theorie heute als widerlegt?", new[] { "Erworbene körperliche Veränderungen verändern nicht die Erbinformation in den Keimzellen", "Weil Darwins Theorie zeitlich vor Lamarcks Theorie entstand", "Weil Lamarck nie irgendeine Theorie aufgestellt hat" }, "Erworbene körperliche Veränderungen verändern nicht die Erbinformation in den Keimzellen",
            "Nach heutigem Wissen verändern im Leben erworbene Eigenschaften (z.B. trainierte Muskeln) nicht die DNA der Keimzellen und werden daher nicht direkt vererbt."),
        ("Was ist der zentrale Mechanismus der Darwinschen Evolutionstheorie?", new[] { "Zufällige Variation der Merkmale kombiniert mit natürlicher Selektion (Auslese) durch die Umwelt", "Ausschließlich der bewusste Wille der Lebewesen, sich anzupassen", "Reiner Zufall ohne jede Auslese durch die Umwelt" }, "Zufällige Variation der Merkmale kombiniert mit natürlicher Selektion (Auslese) durch die Umwelt",
            "Nach Darwin entstehen zufällige Merkmalsunterschiede (Variabilität); Lebewesen mit vorteilhaften Merkmalen überleben und pflanzen sich häufiger erfolgreich fort (Selektion)."),
        ("Was ist ein Fossil?", new[] { "Ein erhaltener Überrest oder Abdruck eines Lebewesens aus früheren Erdzeitaltern", "Ein lebendes Tier, das heute noch existiert", "Ein anderes Wort für Chromosom" }, "Ein erhaltener Überrest oder Abdruck eines Lebewesens aus früheren Erdzeitaltern",
            "Fossilien sind versteinerte Überreste oder Abdrücke von Lebewesen und liefern wichtige Belege für die Evolution und ausgestorbene Arten."),
        ("Was sind homologe Organe (Homologie)?", new[] { "Organe mit demselben evolutionären Ursprung, aber möglicherweise unterschiedlicher Funktion", "Organe mit völlig unterschiedlicher Herkunft, aber zufällig ähnlicher Funktion", "Organe, die bei keiner zwei Arten jemals gleichzeitig vorkommen" }, "Organe mit demselben evolutionären Ursprung, aber möglicherweise unterschiedlicher Funktion",
            "Homologe Organe, z.B. der Vorderarm bei Mensch, Fledermaus und Wal, haben denselben stammesgeschichtlichen Ursprung, auch wenn sie heute unterschiedliche Funktionen (Greifen, Fliegen, Schwimmen) erfüllen."),
        ("Was sind analoge Organe (Analogie) im Unterschied zu homologen Organen?", new[] { "Organe mit ähnlicher Funktion, aber unabhängiger evolutionärer Entstehung", "Organe mit identischem evolutionärem Ursprung", "Ein anderes Wort für Fossilien" }, "Organe mit ähnlicher Funktion, aber unabhängiger evolutionärer Entstehung",
            "Analoge Organe wie die Flügel von Vögeln und Insekten erfüllen eine ähnliche Funktion, sind aber evolutionär unabhängig voneinander entstanden."),
        ("Was sind rudimentäre Organe?", new[] { "Zurückgebildete Organe, die im Laufe der Evolution ihre ursprüngliche Funktion verloren haben", "Organe, die erst kürzlich neu entstanden sind", "Organe, die bei allen Lebewesen exakt gleich stark ausgeprägt sind" }, "Zurückgebildete Organe, die im Laufe der Evolution ihre ursprüngliche Funktion verloren haben",
            "Rudimentäre Organe wie der Blinddarmwurmfortsatz beim Menschen gelten als Überbleibsel von Organen, die bei Vorfahren eine größere Funktion hatten."),
        ("Was zählt neben Mutation und Selektion als weiterer wichtiger Evolutionsfaktor?", new[] { "Isolation (z.B. geografische Trennung von Populationen)", "Ausschließlich das Wetter", "Nur die Körpergröße eines Lebewesens" }, "Isolation (z.B. geografische Trennung von Populationen)",
            "Isolation trennt Populationen räumlich oder genetisch voneinander, wodurch sich getrennte Gruppen im Laufe der Zeit unterschiedlich entwickeln und neue Arten entstehen können."),
        ("Was versteht man unter natürlicher Selektion (Auslese)?", new[] { "Besser angepasste Individuen überleben und pflanzen sich häufiger erfolgreich fort", "Alle Individuen einer Art haben exakt dieselben Überlebenschancen", "Selektion bedeutet, dass sich Lebewesen bewusst für Merkmale entscheiden" }, "Besser angepasste Individuen überleben und pflanzen sich häufiger erfolgreich fort",
            "Individuen mit Merkmalen, die besser an die Umwelt angepasst sind, haben tendenziell höhere Überlebens- und Fortpflanzungschancen - das ist der Kern natürlicher Selektion."),
        ("Wie erklärt die moderne Evolutionsbiologie die Angepasstheit von Organismen an ihre Umwelt?", new[] { "Durch das Zusammenspiel von zufälliger Variabilität und Selektion über viele Generationen", "Durch bewusste Entscheidungen einzelner Lebewesen", "Angepasstheit entsteht ohne jede erkennbare Ursache" }, "Durch das Zusammenspiel von zufälliger Variabilität und Selektion über viele Generationen",
            "Zufällige genetische Variation liefert unterschiedliche Merkmale, die natürliche Selektion \"filtert\" über viele Generationen die vorteilhaften Merkmale heraus."),
        ("Was zeigt der Vergleich von Hominidenschädeln (z.B. Australopithecus, Homo erectus, Homo sapiens) in der Stammesgeschichte des Menschen?", new[] { "Schrittweise Veränderungen von Schädelform und Gehirnvolumen im Laufe der menschlichen Evolution", "Dass sich der menschliche Schädel niemals verändert hat", "Dass alle Hominidenarten exakt gleichzeitig entstanden sind" }, "Schrittweise Veränderungen von Schädelform und Gehirnvolumen im Laufe der menschlichen Evolution",
            "Der Vergleich fossiler Hominidenschädel zeigt allmähliche Veränderungen wie ein wachsendes Gehirnvolumen und andere anatomische Anpassungen im Verlauf der Menschheitsentwicklung."),
        ("Was ist eine Art im biologischen Sinn (vereinfacht)?", new[] { "Eine Gruppe von Lebewesen, die sich untereinander fortpflanzen und fruchtbare Nachkommen zeugen kann", "Jedes einzelne Lebewesen für sich genommen", "Eine rein zufällige Einteilung ohne biologische Grundlage" }, "Eine Gruppe von Lebewesen, die sich untereinander fortpflanzen und fruchtbare Nachkommen zeugen kann",
            "Der biologische Artbegriff definiert eine Art meist über die Fähigkeit, sich untereinander erfolgreich fortzupflanzen und fruchtbare Nachkommen zu bekommen."),
        ("Was ist ein Beispiel für einen direkten Evolutionsbeleg, der in Gesteinsschichten gefunden werden kann?", new[] { "Versteinerte Fossilien ausgestorbener Arten", "Lebende Tiere im heutigen Zoo", "Aktuelle DNA-Proben von heute lebenden Menschen" }, "Versteinerte Fossilien ausgestorbener Arten",
            "In Gesteinsschichten unterschiedlichen Alters finden sich Fossilien, die Rückschlüsse auf die stammesgeschichtliche Entwicklung von Arten ermöglichen."),
        ("Warum gilt die Ähnlichkeit der DNA zwischen Mensch und anderen Primaten als Hinweis auf eine gemeinsame Abstammung?", new[] { "Eine hohe genetische Übereinstimmung deutet auf einen gemeinsamen evolutionären Vorfahren hin", "Genetische Ähnlichkeit hat mit Abstammung nichts zu tun", "DNA-Vergleiche liefern grundsätzlich keine verwertbaren Hinweise" }, "Eine hohe genetische Übereinstimmung deutet auf einen gemeinsamen evolutionären Vorfahren hin",
            "Je ähnlicher sich die DNA zweier Arten ist, desto näher liegt in der Regel ihr gemeinsamer evolutionärer Vorfahre zeitlich zurück."),
        ("Was passiert, wenn eine Population durch eine geografische Barriere (z.B. Gebirge, Insel) von einer anderen Population getrennt wird?", new[] { "Beide Gruppen können sich über viele Generationen genetisch unterschiedlich entwickeln", "Beide Populationen bleiben zwangsläufig für immer genetisch identisch", "Eine geografische Trennung hat keinerlei Einfluss auf die Evolution" }, "Beide Gruppen können sich über viele Generationen genetisch unterschiedlich entwickeln",
            "Durch geografische Isolation können sich getrennte Populationen unabhängig voneinander weiterentwickeln, was langfristig zur Bildung neuer Arten führen kann."),
        ("Was ist ein Beispiel für die Angepasstheit eines Lebewesens an seinen Lebensraum, die sich evolutionsbiologisch erklären lässt?", new[] { "Die Fellfarbe von Eisbären als Anpassung an die schneereiche arktische Umgebung", "Die Farbe eines künstlich gefärbten Spielzeugs", "Ein zufällig ausgewähltes menschengemachtes Bauwerk" }, "Die Fellfarbe von Eisbären als Anpassung an die schneereiche arktische Umgebung",
            "Das weiße Fell von Eisbären gilt als evolutionäre Anpassung, die eine bessere Tarnung in der schneebedeckten arktischen Umgebung ermöglicht."),
        ("Warum betrachten Wissenschaftlerinnen und Wissenschaftler Evolution heute als gut belegte Theorie und nicht als bloße Vermutung?", new[] { "Zahlreiche unabhängige Beweislinien wie Fossilien, Homologien und Genetik stützen sie übereinstimmend", "Es gibt bislang überhaupt keine Belege dafür", "Die Theorie beruht ausschließlich auf einer einzigen Beobachtung" }, "Zahlreiche unabhängige Beweislinien wie Fossilien, Homologien und Genetik stützen sie übereinstimmend",
            "Fossilfunde, vergleichende Anatomie (Homologien), Genetik und direkt beobachtbare Anpassungsprozesse liefern gemeinsam ein sehr stimmiges Bild, das die Evolutionstheorie stark stützt."),
        ("Was unterscheidet die Rolle des Zufalls bei Darwin von der bei Lamarck grundlegend?", new[] { "Bei Darwin entsteht Variation zufällig, bei Lamarck sollten gezielt erworbene Veränderungen vererbt werden", "Bei beiden Theorien spielt Zufall exakt dieselbe Rolle", "Zufall spielt bei keiner der beiden Theorien eine Rolle" }, "Bei Darwin entsteht Variation zufällig, bei Lamarck sollten gezielt erworbene Veränderungen vererbt werden",
            "Darwin ging von zufälliger Variation aus, auf die die Selektion einwirkt; Lamarck nahm dagegen an, dass gezielt im Leben erworbene Anpassungen weitervererbt würden - das gilt heute als widerlegt."),
        ("Was bedeutet \"Koevolution\" zwischen zwei Arten, z.B. einer Blüte und ihrem Bestäuber?", new[] { "Beide Arten beeinflussen im Laufe der Evolution wechselseitig die Entwicklung der jeweils anderen Art", "Nur eine der beiden Arten entwickelt sich weiter, die andere bleibt exakt gleich", "Koevolution beschreibt zwei völlig unabhängige, sich nie beeinflussende Arten" }, "Beide Arten beeinflussen im Laufe der Evolution wechselseitig die Entwicklung der jeweils anderen Art",
            "Bei Koevolution passen sich zwei Arten über viele Generationen wechselseitig aneinander an, z.B. eine Blütenform an die Kopfform ihres Bestäubers.")
    };

    private static QuizQuestion Evolution(Random r)
    {
        var f = EvolutionListe[r.Next(EvolutionListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse9,
            Topic = "Evolution – Theorien und Stammesgeschichte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Darwins Theorie (Variation + Selektion) gilt als bestätigt, Lamarcks Theorie der Vererbung erworbener Eigenschaften als widerlegt; Fossilien, Homologien und Genetik belegen die Evolution."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ZelleListe =
    {
        ("Was ist die kleinste Funktionseinheit aller Lebewesen?", new[] { "Die Zelle", "Das Organ", "Das Molekül" }, "Die Zelle",
            "Die Zelle ist die kleinste Einheit, die alle Merkmale des Lebens zeigen kann - sie ist der Baustein jedes Lebewesens."),
        ("Womit kann man einzelne Zellen sichtbar machen, weil sie mit bloßem Auge nicht erkennbar sind?", new[] { "Mit einem Mikroskop", "Mit einer Lupe allein", "Mit einem Fernglas" }, "Mit einem Mikroskop",
            "Zellen sind so winzig, dass man sie erst mit einem Mikroskop, das stark vergrößert, erkennen kann."),
        ("Wie nennt man Lebewesen, die nur aus einer einzigen Zelle bestehen?", new[] { "Einzeller", "Vielzeller", "Zellhaufen" }, "Einzeller",
            "Einzeller wie Bakterien oder manche Algen bestehen aus nur einer einzigen Zelle, die alle Lebensfunktionen allein erfüllt."),
        ("Wie nennt man Lebewesen wie den Menschen, die aus vielen Zellen bestehen?", new[] { "Vielzeller", "Einzeller", "Urzeller" }, "Vielzeller",
            "Der Mensch besteht aus Billionen Zellen und zählt deshalb zu den Vielzellern."),
        ("Welcher Zellbestandteil umhüllt jede Zelle nach außen und lässt nur bestimmte Stoffe hindurch?", new[] { "Die Zellmembran", "Der Zellkern", "Die Vakuole" }, "Die Zellmembran",
            "Die Zellmembran umschließt die Zelle und regelt, welche Stoffe hinein- oder herausgelangen können."),
        ("Welcher Zellbestandteil enthält die Erbinformation der Zelle?", new[] { "Der Zellkern", "Die Zellmembran", "Das Zytoplasma" }, "Der Zellkern",
            "Im Zellkern liegt die Erbinformation (DNA) gut geschützt gespeichert."),
        ("Wie nennt man die gallertartige Flüssigkeit im Inneren der Zelle, in der die Zellbestandteile liegen?", new[] { "Zytoplasma", "Zellwand", "Zellmembran" }, "Zytoplasma",
            "Das Zytoplasma füllt das Innere der Zelle aus und umgibt die verschiedenen Zellbestandteile."),
        ("Was besitzt eine Pflanzenzelle zusätzlich, das ihr Stabilität und eine feste Form gibt?", new[] { "Eine Zellwand", "Einen zweiten Zellkern", "Ein Skelett" }, "Eine Zellwand",
            "Die feste Zellwand aus Zellulose gibt Pflanzenzellen zusätzlich zur Membran Halt und Form - Tierzellen haben das nicht."),
        ("Welche grünen Zellbestandteile findet man in Pflanzenzellen und die für die Fotosynthese wichtig sind?", new[] { "Chloroplasten", "Mitochondrien", "Ribosomen" }, "Chloroplasten",
            "Chloroplasten enthalten den grünen Farbstoff Chlorophyll und ermöglichen der Pflanzenzelle die Fotosynthese."),
        ("Welcher Zellbestandteil speichert in einer Pflanzenzelle Wasser und Nährstoffe und nimmt oft viel Platz ein?", new[] { "Die Vakuole", "Der Zellkern", "Die Zellwand" }, "Die Vakuole",
            "Die große Vakuole speichert Wasser und Nährstoffe und hilft der Pflanzenzelle, ihre Form stabil zu halten."),
        ("Wer entdeckte 1665 mit einem einfachen Mikroskop als einer der Ersten Zellen, als er Korkgewebe untersuchte?", new[] { "Robert Hooke", "Charles Darwin", "Gregor Mendel" }, "Robert Hooke",
            "Robert Hooke prägte 1665 den Begriff \"Zelle\", nachdem er unter dem Mikroskop die kleinen Kammern im Kork entdeckt hatte."),
        ("Wie viele Zellen hat der menschliche Körper ungefähr (Größenordnung)?", new[] { "Mehrere Billionen", "Genau 100", "Etwa 1.000" }, "Mehrere Billionen",
            "Der menschliche Körper besteht aus mehreren Billionen Zellen, die zusammenarbeiten."),
        ("Was passiert, wenn eine Zelle wächst und sich teilt?", new[] { "Aus einer Zelle entstehen zwei neue Zellen", "Die Zelle verschwindet komplett", "Zwei Zellen verschmelzen zu einer" }, "Aus einer Zelle entstehen zwei neue Zellen",
            "Bei der Zellteilung entstehen aus einer Zelle zwei neue Zellen - so wächst ein Lebewesen und ersetzt verbrauchte Zellen."),
        ("Wozu dient die Zellteilung z.B. beim Wachstum eines Kindes oder bei der Wundheilung?", new[] { "Um neue Zellen für Wachstum und Reparatur zu bilden", "Um alte Zellen zu vernichten", "Sie hat keinen bestimmten Zweck" }, "Um neue Zellen für Wachstum und Reparatur zu bilden",
            "Durch Zellteilung entstehen die neuen Zellen, die der Körper zum Wachsen und zum Heilen von Wunden braucht."),
        ("Was ist der Hauptunterschied zwischen einer Tierzelle und einer Pflanzenzelle?", new[] { "Pflanzenzellen haben zusätzlich Zellwand und Chloroplasten", "Tierzellen haben keinen Zellkern", "Beide Zelltypen sind komplett identisch" }, "Pflanzenzellen haben zusätzlich Zellwand und Chloroplasten",
            "Pflanzenzellen besitzen im Unterschied zu Tierzellen eine feste Zellwand und grüne Chloroplasten für die Fotosynthese."),
        ("Zu welcher Gruppe (Ein- oder Vielzeller) zählt ein Bakterium?", new[] { "Einzeller", "Vielzeller", "Weder noch" }, "Einzeller",
            "Ein Bakterium besteht nur aus einer einzigen Zelle und ist deshalb ein Einzeller."),
        ("Welches einfache Hilfsmittel braucht man mindestens, um eine einzelne Zelle sehen zu können?", new[] { "Ein Mikroskop", "Ein Fernrohr", "Eine Brille" }, "Ein Mikroskop",
            "Nur ein Mikroskop vergrößert stark genug, um einzelne Zellen sichtbar zu machen."),
        ("Was ist ein Gewebe (z.B. Muskelgewebe) im Körper eines Vielzellers?", new[] { "Eine Gruppe gleichartiger Zellen mit gemeinsamer Aufgabe", "Ein anderes Wort für Zellkern", "Ein einzelnes, sehr großes Molekül" }, "Eine Gruppe gleichartiger Zellen mit gemeinsamer Aufgabe",
            "Viele gleichartige Zellen, die zusammen eine bestimmte Aufgabe erfüllen (z.B. Bewegung), bilden ein Gewebe."),
        ("Warum bezeichnet man die Zelle als \"kleinste Funktionseinheit des Lebendigen\"?", new[] { "Weil sie die kleinste Einheit ist, die alle Lebensmerkmale wie Stoffwechsel und Wachstum zeigen kann", "Weil sie das kleinste Molekül im Körper ist", "Weil sie nur bei Pflanzen vorkommt" }, "Weil sie die kleinste Einheit ist, die alle Lebensmerkmale wie Stoffwechsel und Wachstum zeigen kann",
            "Kleiner als eine Zelle gibt es keine Einheit mehr, die selbstständig Stoffwechsel betreiben, wachsen und sich fortpflanzen kann."),
        ("Was versorgt eine Zelle über die Zellmembran hinweg mit Nährstoffen und Sauerstoff?", new[] { "Der Stoffaustausch mit der Umgebung", "Eine eigene kleine Sonne im Zellinneren", "Die Zelle braucht dafür nichts von außen" }, "Der Stoffaustausch mit der Umgebung",
            "Über die Zellmembran nimmt die Zelle Nährstoffe und Sauerstoff aus ihrer Umgebung auf und gibt Abfallstoffe ab.")
    };

    private static QuizQuestion Zelle(Random r)
    {
        var f = ZelleListe[r.Next(ZelleListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Die Zelle", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die Zelle ist die kleinste Funktionseinheit des Lebens. Zellkern (Erbinfo), Zellmembran (Hülle) und Zytoplasma hat jede Zelle; Zellwand, Chloroplasten und Vakuole nur Pflanzenzellen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] LebensraeumeListe =
    {
        ("Was versteht man unter dem \"Lebensraum\" eines Tieres oder einer Pflanze?", new[] { "Den natürlichen Ort, an dem ein Lebewesen alles findet, was es zum Überleben braucht", "Nur den genauen Ort, an dem ein Tier geboren wurde", "Ein Gebiet, das ausschließlich Menschen bewohnen" }, "Den natürlichen Ort, an dem ein Lebewesen alles findet, was es zum Überleben braucht",
            "Ein Lebensraum (z.B. Wald, Teich, Wiese) bietet einem Lebewesen Nahrung, Schutz und die passenden Bedingungen zum Überleben."),
        ("Wie nennt man Lebewesen, die ihre Energie durch Fotosynthese selbst herstellen, z.B. Pflanzen?", new[] { "Produzenten", "Konsumenten", "Destruenten" }, "Produzenten",
            "Produzenten wie grüne Pflanzen erzeugen mithilfe von Sonnenlicht selbst ihre Nahrung und stehen am Anfang jeder Nahrungskette."),
        ("Wie nennt man Lebewesen, die sich von anderen Lebewesen ernähren, weil sie selbst keine Fotosynthese betreiben können?", new[] { "Konsumenten", "Produzenten", "Zersetzer" }, "Konsumenten",
            "Konsumenten (z.B. Tiere) können ihre Nahrung nicht selbst herstellen und fressen deshalb Pflanzen oder andere Tiere."),
        ("Wie nennt man Lebewesen wie Pilze und viele Bakterien, die abgestorbene Pflanzen- und Tierreste zersetzen?", new[] { "Destruenten (Zersetzer)", "Produzenten", "Räuber" }, "Destruenten (Zersetzer)",
            "Destruenten zersetzen tote Organismen und geben deren Nährstoffe zurück in den Boden, wo Pflanzen sie wieder aufnehmen können."),
        ("Was ist eine Nahrungskette?", new[] { "Eine feste Reihenfolge von Lebewesen, die sich gegenseitig fressen", "Eine Kette aus Metall, mit der Tiere gefangen werden", "Ein anderes Wort für Lebensraum" }, "Eine feste Reihenfolge von Lebewesen, die sich gegenseitig fressen",
            "Eine Nahrungskette zeigt, wer wen frisst, und damit, wie Energie von einem Lebewesen zum nächsten weitergegeben wird."),
        ("Womit beginnt fast jede Nahrungskette?", new[] { "Mit einem Produzenten, z.B. einer Pflanze", "Mit einem großen Raubtier", "Mit einem Destruenten" }, "Mit einem Produzenten, z.B. einer Pflanze",
            "Da Pflanzen als Produzenten die Energie der Sonne nutzbar machen, stehen sie fast immer am Anfang einer Nahrungskette."),
        ("Wie nennt man ein Tier, das sich ausschließlich von Pflanzen ernährt?", new[] { "Pflanzenfresser (Herbivore)", "Fleischfresser (Carnivore)", "Allesfresser (Omnivore)" }, "Pflanzenfresser (Herbivore)",
            "Pflanzenfresser wie Rehe oder Kaninchen fressen ausschließlich pflanzliche Nahrung."),
        ("Wie nennt man ein Tier, das sich von anderen Tieren ernährt?", new[] { "Fleischfresser (Carnivore)", "Pflanzenfresser (Herbivore)", "Destruent" }, "Fleischfresser (Carnivore)",
            "Fleischfresser wie der Fuchs jagen und fressen andere Tiere."),
        ("Wie nennt man ein Tier, das sowohl Pflanzen als auch andere Tiere frisst?", new[] { "Allesfresser (Omnivore)", "Reiner Pflanzenfresser", "Destruent" }, "Allesfresser (Omnivore)",
            "Allesfresser wie das Wildschwein oder der Mensch nehmen sowohl pflanzliche als auch tierische Nahrung zu sich."),
        ("Was passiert, wenn in einer Nahrungskette ein Glied - z.B. der Räuber - plötzlich wegfällt?", new[] { "Das Gleichgewicht der ganzen Kette gerät durcheinander", "Es ändert sich überhaupt nichts", "Alle anderen Tiere sterben sofort" }, "Das Gleichgewicht der ganzen Kette gerät durcheinander",
            "Fällt ein Glied einer Nahrungskette weg, z.B. weil ein Räuber verschwindet, vermehren sich Beutetiere oft stark und der Lebensraum gerät aus dem Gleichgewicht."),
        ("Warum ist ein Nahrungsnetz eine realistischere Darstellung als eine einzelne Nahrungskette?", new[] { "Weil die meisten Tiere mehrere verschiedene Nahrungsquellen haben und mehrfach vernetzt sind", "Weil ein Nahrungsnetz nur aus einer einzigen Kette besteht", "Weil im Nahrungsnetz keine Pflanzen vorkommen" }, "Weil die meisten Tiere mehrere verschiedene Nahrungsquellen haben und mehrfach vernetzt sind",
            "In der Natur frisst kaum ein Tier nur eine einzige Beuteart - viele Nahrungsketten verknüpfen sich deshalb zu einem Nahrungsnetz."),
        ("Welches der folgenden ist ein typischer Lebensraum in Mitteleuropa?", new[] { "Wald, Wiese oder Teich", "Nur die Sahara-Wüste", "Nur der Nordpol" }, "Wald, Wiese oder Teich",
            "Wald, Wiese, Teich, Hecke oder Fluss sind typische Lebensräume, die man in Mitteleuropa direkt vor der Haustür findet."),
        ("Warum können bestimmte Tiere und Pflanzen nur in bestimmten Lebensräumen überleben?", new[] { "Weil sie an die dortigen Bedingungen wie Klima, Nahrung und Boden angepasst sind", "Weil sie sich das zufällig ausgesucht haben", "Weil andere Lebensräume ihnen verboten sind" }, "Weil sie an die dortigen Bedingungen wie Klima, Nahrung und Boden angepasst sind",
            "Lebewesen sind im Lauf der Zeit an die Bedingungen ihres Lebensraums angepasst - deshalb überlebt z.B. ein Wasserfrosch nicht in der Wüste."),
        ("Was bedeutet eine \"Räuber-Beute-Beziehung\"?", new[] { "Ein Tier (Räuber) jagt und frisst ein anderes Tier (Beute)", "Zwei Tiere teilen sich freundschaftlich die Nahrung", "Ein Tier zersetzt abgestorbene Pflanzenreste" }, "Ein Tier (Räuber) jagt und frisst ein anderes Tier (Beute)",
            "Bei einer Räuber-Beute-Beziehung, z.B. Fuchs und Hase, jagt der Räuber gezielt die Beute, um sich zu ernähren."),
        ("Was passiert normalerweise mit der Zahl der Beutetiere, wenn es in einem Lebensraum sehr viele Räuber gibt?", new[] { "Sie sinkt, weil mehr Beutetiere gefressen werden", "Sie steigt automatisch stark an", "Sie bleibt exakt gleich" }, "Sie sinkt, weil mehr Beutetiere gefressen werden",
            "Je mehr Räuber es gibt, desto mehr Beutetiere werden gefressen - die Bestände von Räubern und Beute hängen eng zusammen."),
        ("Wie nennt man die Gesamtheit aller miteinander verknüpften Nahrungsketten eines Lebensraums?", new[] { "Nahrungsnetz", "Nahrungspyramide", "Zellverband" }, "Nahrungsnetz",
            "Da viele Nahrungsketten sich überschneiden und verknüpfen, spricht man insgesamt von einem Nahrungsnetz."),
        ("Was passiert mit den Nährstoffen toter Pflanzen und Tiere durch Destruenten wie Pilze und Bodenbakterien?", new[] { "Sie werden zersetzt und gelangen zurück in den Nährstoffkreislauf", "Sie verschwinden spurlos aus dem Lebensraum", "Sie werden ausschließlich von Räubern verwertet" }, "Sie werden zersetzt und gelangen zurück in den Nährstoffkreislauf",
            "Destruenten zersetzen abgestorbenes Material, wodurch die enthaltenen Nährstoffe wieder für Pflanzen im Boden verfügbar werden."),
        ("Warum sind Regenwürmer für einen Lebensraum wie eine Wiese besonders wichtig?", new[] { "Sie zersetzen abgestorbenes Material und lockern dabei den Boden auf", "Sie betreiben als einzige Tiere Fotosynthese", "Sie jagen als Räuber große Beutetiere" }, "Sie zersetzen abgestorbenes Material und lockern dabei den Boden auf",
            "Regenwürmer sind wichtige Destruenten: Sie zersetzen Pflanzenreste und lockern durch ihre Gänge gleichzeitig den Boden auf."),
        ("Warum sind Bienen für viele Lebensräume besonders wichtig?", new[] { "Sie bestäuben Blüten und sichern so die Fortpflanzung vieler Pflanzen", "Sie sind die einzigen Destruenten in der Natur", "Sie stehen am Ende jeder Nahrungskette" }, "Sie bestäuben Blüten und sichern so die Fortpflanzung vieler Pflanzen",
            "Beim Sammeln von Nektar tragen Bienen Blütenstaub von Blüte zu Blüte und ermöglichen so vielen Pflanzen die Fortpflanzung."),
        ("Was passiert mit der nutzbaren Energiemenge, je weiter man in einer Nahrungskette nach oben (zum Fleischfresser) geht?", new[] { "Sie nimmt ab, weil bei jedem Schritt Energie als Wärme verloren geht", "Sie nimmt automatisch zu", "Sie bleibt auf jeder Stufe exakt gleich groß" }, "Sie nimmt ab, weil bei jedem Schritt Energie als Wärme verloren geht",
            "Bei jedem Schritt einer Nahrungskette geht ein großer Teil der Energie als Wärme verloren - deshalb gibt es meist viel mehr Pflanzenfresser als Fleischfresser.")
    };

    private static QuizQuestion LebensraeumeUndNahrungsketten(Random r)
    {
        var f = LebensraeumeListe[r.Next(LebensraeumeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Biologie, GradeLevel = GradeLevel.Klasse6,
            Topic = "Lebensräume und ihre Bewohner (Nahrungsketten)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nahrungsketten laufen meist: Produzent (Pflanze) → Konsument (Pflanzenfresser) → Konsument (Fleischfresser) → Destruenten zersetzen am Ende alles wieder."
        };
    }
}
