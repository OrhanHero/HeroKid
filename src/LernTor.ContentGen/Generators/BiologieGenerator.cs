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
            [GradeLevel.Klasse6] = new List<TopicFactory> { MenschlicheOrgane, Fotosynthese, Wirbeltierklassen, PubertaetUndEntwicklung },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Zellbiologie, Vererbung, Oekosystem }
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
}
