using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Geografie/Erdkunde nach Berliner Rahmenlehrplan, Klasse 6 und Klasse 9.</summary>
public sealed class GeoGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Geo;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Kontinente, Klimazonen, Bundeslaender, RisikoraeumeNaturgefahren, MigrationUndBevoelkerung, TropischerRegenwald, ArmutUndReichtumKlasse6 },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Plattentektonik, Klimawandel, Verstaedterung, ArmutReichtum, RessourcenEnergie, LandwirtschaftUndBoden, KlimaschutzInternational, WirtschaftlicheVerflechtung, EuropaWirtschaftsraum }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KontinenteListe =
    {
        ("Wie viele Kontinente gibt es (gängige Zählweise: Afrika, Amerika, Antarktika, Asien, Australien/Ozeanien, Europa)?", new[] { "6", "4", "10" }, "6",
            "In der gängigen Zählweise gibt es 6 Kontinente. (Manche zählen Nord- und Südamerika getrennt = 7.)"),
        ("Welcher Ozean ist der größte der Erde?", new[] { "Pazifischer Ozean", "Atlantischer Ozean", "Indischer Ozean" }, "Pazifischer Ozean",
            "Der Pazifische Ozean ist mit Abstand der flächenmäßig größte Ozean der Erde."),
        ("Auf welchem Kontinent liegt Deutschland?", new[] { "Europa", "Asien", "Afrika" }, "Europa", "Deutschland liegt in Mitteleuropa, auf dem Kontinent Europa."),
        ("Welcher Kontinent ist der flächenmäßig größte der Erde?", new[] { "Asien", "Afrika", "Europa" }, "Asien",
            "Asien ist mit großem Abstand der flächenmäßig größte Kontinent der Erde."),
        ("Wie heißt der Kontinent, der fast vollständig von Eis bedeckt ist?", new[] { "Antarktika", "Australien", "Europa" }, "Antarktika",
            "Antarktika liegt am Südpol und ist fast vollständig von einem dicken Eispanzer bedeckt."),
        ("Welcher Kontinent ist der flächenmäßig kleinste?", new[] { "Australien/Ozeanien", "Europa (was so in der Praxis nicht zutrifft)", "Afrika" }, "Australien/Ozeanien",
            "Australien (mit Ozeanien) ist der flächenmäßig kleinste der bewohnten Kontinente."),
        ("Welcher Ozean ist nach dem Pazifik der zweitgrößte?", new[] { "Atlantischer Ozean", "Indischer Ozean", "Arktischer Ozean" }, "Atlantischer Ozean",
            "Der Atlantische Ozean liegt zwischen Amerika und Europa/Afrika und ist nach dem Pazifik der zweitgrößte Ozean."),
        ("Auf welchem Kontinent liegt Ägypten?", new[] { "Afrika", "Asien", "Europa" }, "Afrika",
            "Ägypten liegt im Nordosten Afrikas, ein kleiner Teil (Sinai-Halbinsel) auf dem asiatischen Kontinent."),
        ("Auf welchem Kontinent liegt Brasilien?", new[] { "Südamerika", "Nordamerika", "Afrika" }, "Südamerika",
            "Brasilien ist das flächenmäßig größte Land Südamerikas."),
        ("Auf welchem Kontinent liegt China?", new[] { "Asien", "Europa", "Afrika" }, "Asien",
            "China liegt in Ostasien und ist das bevölkerungsreichste Land Asiens."),
        ("Wie heißt der längste Fluss der Welt (nach gängiger Zählweise)?", new[] { "Nil", "Rhein", "Donau" }, "Nil",
            "Der Nil in Afrika gilt nach den meisten Messungen als längster Fluss der Welt."),
        ("Wie heißt das höchste Gebirge der Welt?", new[] { "Himalaya", "Alpen", "Anden" }, "Himalaya",
            "Der Himalaya in Asien beherbergt die höchsten Berge der Welt, darunter den Mount Everest."),
        ("Wie heißt der höchste Berg der Welt?", new[] { "Mount Everest", "Kilimandscharo", "Matterhorn" }, "Mount Everest",
            "Der Mount Everest im Himalaya ist mit knapp 8.849 Metern der höchste Berg der Erde."),
        ("Welcher Kontinent hat keine dauerhafte einheimische Bevölkerung, sondern nur Forschungsstationen?", new[] { "Antarktika", "Australien", "Europa" }, "Antarktika",
            "Antarktika wird nicht dauerhaft bewohnt - dort gibt es nur wechselnde Forscherinnen und Forscher in Stationen."),
        ("Welcher Ozean liegt zwischen Amerika und Europa/Afrika?", new[] { "Atlantischer Ozean", "Pazifischer Ozean", "Indischer Ozean" }, "Atlantischer Ozean",
            "Der Atlantische Ozean trennt den amerikanischen Kontinent von Europa und Afrika."),
        ("Welcher Ozean liegt zwischen Afrika, Asien und Australien?", new[] { "Indischer Ozean", "Atlantischer Ozean", "Arktischer Ozean" }, "Indischer Ozean",
            "Der Indische Ozean grenzt an Afrika, Asien und Australien."),
        ("Auf welchem Kontinent liegt der Großteil der Türkei?", new[] { "Asien", "Europa", "Afrika" }, "Asien",
            "Der größte Teil der Türkei liegt in Asien (Anatolien), ein kleinerer Teil in Europa (Thrakien)."),
        ("Wie viele Ozeane gibt es nach gängiger Zählweise?", new[] { "5", "3", "8" }, "5",
            "Üblicherweise werden 5 Ozeane gezählt: Pazifik, Atlantik, Indischer Ozean, Südlicher (Antarktischer) und Arktischer Ozean."),
        ("Welcher Kontinent liegt vollständig auf der Südhalbkugel?", new[] { "Antarktika", "Asien", "Europa" }, "Antarktika",
            "Antarktika liegt komplett südlich des Äquators, rund um den Südpol."),
        ("Wie wird der amerikanische Kontinent oft unterteilt?", new[] { "In Nordamerika und Südamerika", "In Ostamerika und Westamerika", "Er wird nie unterteilt" }, "In Nordamerika und Südamerika",
            "Je nach Zählweise wird der Doppelkontinent Amerika oft in Nordamerika und Südamerika unterteilt.")
    };

    private static QuizQuestion Kontinente(Random r)
    {
        var f = KontinenteListe[r.Next(KontinenteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kontinente und Ozeane", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die gängige Zählweise kennt 6 Kontinente; Deutschland liegt in Europa."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KlimazonenListe =
    {
        ("In welcher Klimazone liegt die Sahara?", new[] { "Tropen/Subtropen (heiße Trockenzone)", "Polare Zone", "Gemäßigte Zone" }, "Tropen/Subtropen (heiße Trockenzone)",
            "Die Sahara liegt in der heißen, trockenen Klimazone (Subtropen), geprägt von Wüste."),
        ("In welcher Klimazone liegt Deutschland?", new[] { "Gemäßigte Zone", "Tropen", "Polare Zone - eine verbreitete, aber falsche Annahme" }, "Gemäßigte Zone",
            "Deutschland liegt in der gemäßigten Klimazone mit vier Jahreszeiten und mildem Klima."),
        ("Wie nennt man die Klimazone rund um den Äquator mit hohen Temperaturen und viel Regen?", new[] { "Tropen", "Polare Zone", "Gemäßigte Zone" }, "Tropen",
            "In den Tropen rund um den Äquator ist es ganzjährig warm bis heiß, oft mit viel Niederschlag (z.B. Regenwald)."),
        ("Was ist typisch für das Klima in der polaren Zone (z.B. Arktis, Antarktis)?", new[] { "Sehr kalt und eisig, kaum Vegetation", "Ganzjährig warm und trocken", "Viel Regen und dichte Wälder, was einer genaueren Pruefung nicht standhaelt" }, "Sehr kalt und eisig, kaum Vegetation",
            "In der polaren Zone herrschen sehr niedrige Temperaturen und viel Eis, wodurch dort kaum Pflanzen wachsen können."),
        ("Was kennzeichnet die gemäßigte Klimazone?", new[] { "Vier Jahreszeiten mit milden Temperaturunterschieden", "Ganzjährig gleichbleibend heiß, obwohl das auf den ersten Blick plausibel klingt", "Ganzjährig Schnee und Eis" }, "Vier Jahreszeiten mit milden Temperaturunterschieden",
            "In der gemäßigten Zone (z.B. Deutschland) wechseln sich Frühling, Sommer, Herbst und Winter mit moderaten Temperaturen ab."),
        ("Was ist typisch für das Klima der Savanne?", new[] { "Wechsel zwischen Regenzeit und Trockenzeit", "Ganzjährig gleichmäßiger Regen", "Ganzjährig Schnee" }, "Wechsel zwischen Regenzeit und Trockenzeit",
            "In der Savanne, meist am Rand der Tropen, wechseln sich eine Regenzeit und eine lange Trockenzeit ab."),
        ("Welches Klima herrscht in der Wüste Sahara?", new[] { "Trockenklima mit sehr wenig Niederschlag", "Feuchtklima mit viel Regen", "Polares Klima mit Eis" }, "Trockenklima mit sehr wenig Niederschlag",
            "Die Sahara ist geprägt von extremer Trockenheit mit nur sehr seltenem, geringem Niederschlag."),
        ("Was versteht man unter dem Äquator?", new[] { "Die gedachte Linie, die die Erde in eine Nord- und eine Südhalbkugel teilt", "Eine Gebirgskette in Südamerika", "Einen Ozean zwischen Afrika und Asien" }, "Die gedachte Linie, die die Erde in eine Nord- und eine Südhalbkugel teilt",
            "Der Äquator ist die gedachte Linie auf halbem Weg zwischen Nord- und Südpol, die die Erde in zwei Hälften teilt."),
        ("Warum ist es am Äquator ganzjährig warm?", new[] { "Weil die Sonne dort das ganze Jahr über fast senkrecht steht", "Weil dort kein Wind weht, was die eigentliche Bedeutung des Begriffs verfehlt", "Weil es dort nie regnet" }, "Weil die Sonne dort das ganze Jahr über fast senkrecht steht",
            "Am Äquator treffen die Sonnenstrahlen ganzjährig nahezu senkrecht auf, wodurch es dort konstant warm bis heiß ist."),
        ("Was beschreibt der Begriff \"Monsun\"?", new[] { "Jahreszeitlich wechselnde Winde, die z.B. in Südasien starke Regenzeiten bringen", "Ein Wüstenwind ohne Regen", "Ein Wirbelsturm, der nur im Winter auftritt und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Jahreszeitlich wechselnde Winde, die z.B. in Südasien starke Regenzeiten bringen",
            "Monsune sind jahreszeitlich wechselnde Winde, die z.B. in Süd- und Ostasien ausgeprägte Regen- und Trockenzeiten verursachen."),
        ("In welcher Klimazone liegt der Amazonas-Regenwald?", new[] { "Tropen (tropisches Regenklima)", "Gemäßigte Zone - eine haeufige, aber unzutreffende Vorstellung", "Polare Zone" }, "Tropen (tropisches Regenklima)",
            "Der Amazonas-Regenwald liegt in den Tropen, geprägt von ganzjährig hohen Temperaturen und viel Niederschlag."),
        ("Was ist typisch für das mediterrane Klima (z.B. Südeuropa)?", new[] { "Trockene, heiße Sommer und milde, regenreiche Winter", "Ganzjährig Schnee und Frost", "Ganzjährig Regen ohne echten Sommer" }, "Trockene, heiße Sommer und milde, regenreiche Winter",
            "Das mediterrane Klima rund ums Mittelmeer zeichnet sich durch trockene, heiße Sommer und milde, feuchtere Winter aus."),
        ("Wie nennt man die Nadelwaldzone der kühl-gemäßigten bis subpolaren Breiten (z.B. in Russland, Kanada)?", new[] { "Taiga (borealer Nadelwald)", "Savanne, auch wenn das manche zunaechst vermuten wuerden", "Steppe" }, "Taiga (borealer Nadelwald)",
            "Die Taiga ist der riesige Nadelwaldgürtel der kühlen, nördlichen Breiten in Nordamerika und Eurasien."),
        ("Was ist die Tundra?", new[] { "Eine baumlose, meist kalte Landschaft nahe der Polarregion mit Dauerfrostboden", "Ein tropischer Regenwald, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Eine heiße Wüste" }, "Eine baumlose, meist kalte Landschaft nahe der Polarregion mit Dauerfrostboden",
            "In der Tundra ist der Boden dauerhaft oder fast dauerhaft gefroren (Permafrost), weshalb dort kaum Bäume wachsen."),
        ("Was passiert mit der Temperatur, je höher man in den Bergen steigt (Höhenklima)?", new[] { "Sie sinkt in der Regel", "Sie steigt immer", "Sie bleibt gleich - eine verbreitete, aber falsche Annahme" }, "Sie sinkt in der Regel",
            "Mit zunehmender Höhe nimmt die Lufttemperatur normalerweise ab - deshalb liegt auf hohen Bergen auch in warmen Regionen Schnee."),
        ("Was kennzeichnet ein Kontinentalklima (z.B. Innerasien) im Gegensatz zu einem Seeklima?", new[] { "Große Temperaturunterschiede zwischen Sommer und Winter", "Ganzjährig milde Temperaturen", "Ganzjährig hohe Luftfeuchtigkeit, was einer genaueren Pruefung nicht standhaelt" }, "Große Temperaturunterschiede zwischen Sommer und Winter",
            "Fernab vom Meer schwanken die Temperaturen zwischen Sommer und Winter stark, da Land sich schneller erwärmt und abkühlt als Wasser."),
        ("Was versteht man unter der Steppe?", new[] { "Eine meist baumlose Graslandschaft in eher trockenen, gemäßigten bis subtropischen Gebieten", "Einen tropischen Regenwald, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein Gletschergebiet" }, "Eine meist baumlose Graslandschaft in eher trockenen, gemäßigten bis subtropischen Gebieten",
            "Steppen sind weite, meist baumlose Graslandschaften mit eher geringem Niederschlag, z.B. in Osteuropa und Zentralasien."),
        ("Warum gibt es an den Polen (Arktis, Antarktis) so wenig Sonneneinstrahlung im Winter?", new[] { "Weil die Sonne dort im Winter kaum oder gar nicht über den Horizont steigt", "Weil dort immer Wolken sind", "Weil die Erde dort schneller rotiert" }, "Weil die Sonne dort im Winter kaum oder gar nicht über den Horizont steigt",
            "Wegen der Neigung der Erdachse steigt die Sonne an den Polen im Winter kaum oder gar nicht auf - es herrscht Polarnacht."),
        ("Welche Klimazone liegt zwischen den Tropen und der gemäßigten Zone?", new[] { "Subtropen", "Polarzone", "Taiga" }, "Subtropen",
            "Die Subtropen liegen als Übergangszone zwischen den heißen Tropen und der gemäßigten Klimazone."),
        ("Was zeigt eine Klimazonenkarte typischerweise?", new[] { "Wie sich Temperatur und Niederschlag je nach Breitengrad weltweit verteilen", "Wo die höchsten Berge liegen", "Wo die meisten Menschen leben" }, "Wie sich Temperatur und Niederschlag je nach Breitengrad weltweit verteilen",
            "Klimazonenkarten zeigen, wie Temperatur und Niederschlag weltweit vor allem vom Breitengrad abhängen.")
    };

    private static QuizQuestion Klimazonen(Random r)
    {
        var f = KlimazonenListe[r.Next(KlimazonenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Klimazonen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Denk an die Lage: Wüstenregionen wie die Sahara sind heiß/trocken, Deutschland liegt in der gemäßigten Zone."
        };
    }

    private static readonly (string Frage, string Antwort, string Erklaerung)[] BundeslaenderListe =
    {
        ("Wie heißt die Hauptstadt von Bayern?", "München", "München ist die Landeshauptstadt des Bundeslandes Bayern."),
        ("Wie heißt die Hauptstadt von Nordrhein-Westfalen?", "Düsseldorf", "Düsseldorf ist die Landeshauptstadt von Nordrhein-Westfalen."),
        ("Berlin ist sowohl eine Stadt als auch ein eigenes Bundesland. Wie nennt man das?", "Stadtstaat", "Berlin (wie auch Hamburg und Bremen) ist ein Stadtstaat: Stadt und Bundesland zugleich."),
        ("Wie heißt die Hauptstadt von Baden-Württemberg?", "Stuttgart", "Stuttgart ist die Landeshauptstadt von Baden-Württemberg."),
        ("Wie heißt die Hauptstadt von Sachsen?", "Dresden", "Dresden ist die Landeshauptstadt des Bundeslandes Sachsen."),
        ("Wie heißt die Hauptstadt von Hessen?", "Wiesbaden", "Wiesbaden ist die Landeshauptstadt von Hessen."),
        ("Wie heißt die Hauptstadt von Niedersachsen?", "Hannover", "Hannover ist die Landeshauptstadt von Niedersachsen."),
        ("Wie heißt die Hauptstadt von Rheinland-Pfalz?", "Mainz", "Mainz ist die Landeshauptstadt von Rheinland-Pfalz."),
        ("Wie heißt die Hauptstadt von Schleswig-Holstein?", "Kiel", "Kiel ist die Landeshauptstadt von Schleswig-Holstein."),
        ("Wie heißt die Hauptstadt von Thüringen?", "Erfurt", "Erfurt ist die Landeshauptstadt von Thüringen."),
        ("Wie heißt die Hauptstadt von Sachsen-Anhalt?", "Magdeburg", "Magdeburg ist die Landeshauptstadt von Sachsen-Anhalt."),
        ("Wie heißt die Hauptstadt von Mecklenburg-Vorpommern?", "Schwerin", "Schwerin ist die Landeshauptstadt von Mecklenburg-Vorpommern."),
        ("Wie heißt die Hauptstadt von Brandenburg?", "Potsdam", "Potsdam ist die Landeshauptstadt von Brandenburg."),
        ("Wie heißt die Hauptstadt des Saarlands?", "Saarbrücken", "Saarbrücken ist die Landeshauptstadt des Saarlands."),
        ("Wie viele Bundesländer hat Deutschland?", "16", "Deutschland besteht aus 16 Bundesländern, darunter die drei Stadtstaaten Berlin, Hamburg und Bremen."),
        ("Wie heißt die Bundeshauptstadt Deutschlands?", "Berlin", "Berlin ist die Hauptstadt der Bundesrepublik Deutschland und zugleich ein eigenes Bundesland."),
        ("Welches Bundesland ist flächenmäßig das größte Deutschlands?", "Bayern", "Bayern ist mit großem Abstand das flächenmäßig größte deutsche Bundesland."),
        ("Welches Bundesland hat die meisten Einwohner?", "Nordrhein-Westfalen", "Nordrhein-Westfalen ist das bevölkerungsreichste deutsche Bundesland."),
        ("In welchem Bundesland liegt die Insel Rügen?", "Mecklenburg-Vorpommern", "Die Ostseeinsel Rügen gehört zum Bundesland Mecklenburg-Vorpommern."),
        ("Welches ist das flächenmäßig kleinste Bundesland Deutschlands?", "Bremen", "Bremen ist mit Abstand das flächenmäßig kleinste deutsche Bundesland und zugleich ein Stadtstaat.")
    };

    private static QuizQuestion Bundeslaender(Random r)
    {
        var f = BundeslaenderListe[r.Next(BundeslaenderListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Deutschland: Bundesländer", Type = QuestionType.OpenText,
            Prompt = f.Frage, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Jedes deutsche Bundesland hat eine eigene Landeshauptstadt - Berlin, Hamburg und Bremen sind gleichzeitig Stadt und Bundesland (Stadtstaaten)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PlattenListe =
    {
        ("Was verursacht die Bewegung der Kontinentalplatten (Plattentektonik)?", new[] { "Strömungen im heißen Erdmantel", "Der Mond und deshalb hier nicht zutrifft", "Der Wind" }, "Strömungen im heißen Erdmantel",
            "Konvektionsströme im zähflüssigen Erdmantel bewegen die Kontinentalplatten langsam gegeneinander."),
        ("Was entsteht häufig an den Grenzen zwischen zwei Kontinentalplatten?", new[] { "Vulkane und Erdbeben", "Wüsten", "Gletscher, was so nicht korrekt ist" }, "Vulkane und Erdbeben",
            "An Plattengrenzen entstehen durch Spannungen und Reibung häufig Erdbeben und Vulkanismus."),
        ("Wie nennt man Gebirge, die durch das Aufeinandertreffen zweier Kontinentalplatten entstehen (z.B. Himalaya)?", new[] { "Faltengebirge", "Flachland - eine haeufige, aber unzutreffende Vorstellung", "Senke" }, "Faltengebirge",
            "Wenn zwei Kontinentalplatten zusammenstoßen, wird die Erdkruste aufgefaltet - so entstehen Faltengebirge wie der Himalaya oder die Alpen."),
        ("Was passiert, wenn sich zwei Kontinentalplatten voneinander wegbewegen (z.B. am mittelozeanischen Rücken)?", new[] { "Es dringt neues Magma nach oben und bildet neue Erdkruste", "Es entsteht sofort eine Wüste, auch wenn das manche zunaechst vermuten wuerden", "Die Erde wird dort kälter" }, "Es dringt neues Magma nach oben und bildet neue Erdkruste",
            "An solchen Plattengrenzen ('divergente Plattengrenzen') steigt Magma auf und erstarrt zu neuer Erdkruste, wodurch der Meeresboden langsam wächst."),
        ("Wie nennt man die riesigen, starren Platten, aus denen die äußere Erdkruste besteht?", new[] { "Kontinentalplatten (tektonische Platten)", "Klimazonen, was bei genauerem Hinsehen nicht stimmt", "Gletscher" }, "Kontinentalplatten (tektonische Platten)",
            "Die Erdkruste ist in mehrere große, starre Kontinentalplatten (tektonische Platten) aufgeteilt, die auf dem zähflüssigen Erdmantel schwimmen."),
        ("Wie hieß der Superkontinent, aus dem sich vor Jahrmillionen alle heutigen Kontinente durch Plattenbewegung entwickelt haben?", new[] { "Pangaea", "Atlantis", "Eurasien" }, "Pangaea",
            "Pangaea war ein Superkontinent, der vor rund 300 Millionen Jahren fast alle heutigen Kontinente vereinte, bevor sie auseinanderdrifteten."),
        ("Wer stellte Anfang des 20. Jahrhunderts die Theorie der Kontinentalverschiebung auf?", new[] { "Alfred Wegener", "Albert Einstein", "Charles Darwin" }, "Alfred Wegener",
            "Der deutsche Wissenschaftler Alfred Wegener entwickelte 1912 die Theorie der Kontinentalverschiebung."),
        ("Welches Indiz nutzte Alfred Wegener u.a. für seine Theorie der Kontinentalverschiebung?", new[] { "Die Küstenlinien von Afrika und Südamerika passen wie Puzzleteile zusammen", "Der Mond verändert seine Umlaufbahn", "Die Ozeane werden jedes Jahr wärmer" }, "Die Küstenlinien von Afrika und Südamerika passen wie Puzzleteile zusammen",
            "Wegener fiel auf, dass die Küstenlinien mancher Kontinente wie Puzzleteile zueinander passen - ein Hinweis darauf, dass sie einst zusammenhingen."),
        ("Wie nennt man eine Zone mit besonders vielen Vulkanen und Erdbeben rund um den Pazifischen Ozean?", new[] { "Pazifischer Feuerring (Ring of Fire)", "Sahelzone", "Polarkreis (was so in der Praxis nicht zutrifft)" }, "Pazifischer Feuerring (Ring of Fire)",
            "Der Pazifische Feuerring umrahmt den Pazifik und ist wegen vieler Plattengrenzen besonders reich an Vulkanen und Erdbeben."),
        ("Womit wird die Stärke eines Erdbebens meist gemessen?", new[] { "Richterskala (bzw. Momenten-Magnituden-Skala)", "Beaufortskala - eine verbreitete, aber falsche Annahme", "Celsius-Skala" }, "Richterskala (bzw. Momenten-Magnituden-Skala)",
            "Die Stärke von Erdbeben wird meist mit der Richterskala oder der moderneren Momenten-Magnituden-Skala angegeben."),
        ("Was passiert bei einer Subduktion, wenn eine ozeanische Platte auf eine Kontinentalplatte trifft?", new[] { "Die schwerere ozeanische Platte taucht unter die Kontinentalplatte ab", "Beide Platten schmelzen sofort komplett", "Es entsteht ein neuer Ozean" }, "Die schwerere ozeanische Platte taucht unter die Kontinentalplatte ab",
            "Bei der Subduktion taucht die dichtere ozeanische Platte unter die leichtere Kontinentalplatte, was oft Vulkane und Erdbeben verursacht."),
        ("Was ist eine Transformstörung wie die San-Andreas-Verwerfung in Kalifornien?", new[] { "Zwei Platten reiben seitlich aneinander vorbei", "Zwei Platten entfernen sich nur voneinander, was einer genaueren Pruefung nicht standhaelt", "Eine Platte schmilzt vollständig" }, "Zwei Platten reiben seitlich aneinander vorbei",
            "An einer Transformstörung gleiten zwei Platten seitlich aneinander vorbei, was starke Erdbeben auslösen kann."),
        ("Aus welchen Schichten besteht die Erde von außen nach innen?", new[] { "Erdkruste, Erdmantel, Erdkern", "Erdkern, Erdkruste, Erdmantel", "Nur Erdkruste und Ozean" }, "Erdkruste, Erdmantel, Erdkern",
            "Die Erde ist von außen nach innen in Erdkruste, Erdmantel und Erdkern gegliedert."),
        ("Was kann durch ein starkes Seebeben (Erdbeben unter dem Meer) ausgelöst werden?", new[] { "Ein Tsunami", "Ein Monsun", "Ein Passat" }, "Ein Tsunami",
            "Ein starkes Seebeben kann eine gewaltige Flutwelle, einen Tsunami, auslösen."),
        ("Auf welcher tektonischen Platte liegt der größte Teil Europas und Asiens?", new[] { "Eurasische Platte", "Pazifische Platte", "Antarktische Platte" }, "Eurasische Platte",
            "Der größte Teil Europas und Asiens liegt auf der Eurasischen Platte."),
        ("Warum gibt es in Japan besonders viele Erdbeben und Vulkane?", new[] { "Weil dort mehrere tektonische Platten aufeinandertreffen", "Weil Japan eine Insel ist", "Weil dort viel Regen fällt" }, "Weil dort mehrere tektonische Platten aufeinandertreffen",
            "Japan liegt an einer Stelle, wo mehrere tektonische Platten aufeinandertreffen, was viele Erdbeben und Vulkane verursacht."),
        ("Was versteht man unter \"Kontinentaldrift\"?", new[] { "Die langsame Wanderung der Kontinente über Jahrmillionen", "Das schnelle Abkühlen der Ozeane", "Den täglichen Wechsel von Ebbe und Flut" }, "Die langsame Wanderung der Kontinente über Jahrmillionen",
            "Kontinentaldrift bezeichnet die sehr langsame Wanderung der Kontinente über Jahrmillionen durch Plattenbewegung."),
        ("Wie schnell bewegen sich Kontinentalplatten ungefähr pro Jahr?", new[] { "Wenige Zentimeter pro Jahr", "Mehrere hundert Kilometer pro Jahr", "Sie bewegen sich gar nicht" }, "Wenige Zentimeter pro Jahr",
            "Kontinentalplatten bewegen sich meist nur wenige Zentimeter pro Jahr - etwa so schnell wie Fingernägel wachsen."),
        ("Was kann bei einem Vulkanausbruch aus dem Erdinneren an die Oberfläche gelangen?", new[] { "Magma (dann Lava genannt), Asche und Gase", "Nur Wasser, obwohl das auf den ersten Blick plausibel klingt", "Nur Sand" }, "Magma (dann Lava genannt), Asche und Gase",
            "Bei einem Vulkanausbruch treten Magma (an der Oberfläche Lava genannt), Asche und Gase aus dem Erdinneren aus."),
        ("Welche zwei Platten treffen an der Westküste Südamerikas (Anden) aufeinander?", new[] { "Die Nazca-Platte und die Südamerikanische Platte", "Die Pazifische Platte und die Nordamerikanische Platte", "Die Eurasische und die Afrikanische Platte" }, "Die Nazca-Platte und die Südamerikanische Platte",
            "An der Westküste Südamerikas taucht die Nazca-Platte unter die Südamerikanische Platte, wodurch die Anden entstanden sind.")
    };

    private static QuizQuestion Plattentektonik(Random r)
    {
        var f = PlattenListe[r.Next(PlattenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Plattentektonik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Konvektionsströme im Erdmantel bewegen die Kontinentalplatten - an ihren Grenzen entstehen oft Erdbeben/Vulkane."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KlimawandelListe =
    {
        ("Welches Gas trägt als Treibhausgas besonders stark zum menschengemachten Klimawandel bei?", new[] { "Kohlenstoffdioxid (CO₂)", "Sauerstoff", "Stickstoff" }, "Kohlenstoffdioxid (CO₂)",
            "CO₂ aus der Verbrennung fossiler Brennstoffe (Kohle, Öl, Gas) ist das wichtigste menschengemachte Treibhausgas."),
        ("Was ist eine Folge des Klimawandels für die Polkappen und Gletscher?", new[] { "Sie schmelzen zunehmend ab", "Sie wachsen stark an", "Es ändert sich nichts" }, "Sie schmelzen zunehmend ab",
            "Durch die globale Erwärmung schmelzen Gletscher und Polkappen zunehmend, was u.a. den Meeresspiegel steigen lässt."),
        ("Wie nennt man den natürlichen Effekt, bei dem Gase in der Atmosphäre Wärme zurückhalten?", new[] { "Treibhauseffekt", "Gezeiteneffekt", "Erosionseffekt" }, "Treibhauseffekt",
            "Der Treibhauseffekt hält Wärme in der Atmosphäre zurück - menschengemachte Treibhausgase verstärken diesen Effekt zusätzlich."),
        ("Was ist eine sinnvolle Maßnahme, um den menschengemachten Klimawandel zu bremsen?", new[] { "Erneuerbare Energien wie Sonne und Wind statt Kohle/Öl nutzen", "Mehr Kohlekraftwerke bauen", "Weniger Bäume pflanzen" }, "Erneuerbare Energien wie Sonne und Wind statt Kohle/Öl nutzen",
            "Der Umstieg von fossilen Brennstoffen auf erneuerbare Energien verringert den CO₂-Ausstoß und hilft, den Klimawandel zu bremsen."),
        ("Warum sind Wälder wichtig im Kampf gegen den Klimawandel?", new[] { "Bäume nehmen CO₂ auf und speichern Kohlenstoff", "Bäume produzieren CO₂", "Wälder haben keinen Einfluss aufs Klima" }, "Bäume nehmen CO₂ auf und speichern Kohlenstoff",
            "Bäume binden bei der Fotosynthese CO₂ und speichern Kohlenstoff im Holz - Abholzung setzt dieses CO₂ wieder frei und verstärkt den Klimawandel."),
        ("Was passiert mit dem globalen Meeresspiegel durch den Klimawandel?", new[] { "Er steigt an", "Er sinkt", "Er bleibt gleich" }, "Er steigt an",
            "Durch schmelzendes Eis und die Ausdehnung des wärmeren Meerwassers steigt der globale Meeresspiegel an."),
        ("Was versteht man unter \"Treibhausgasen\"?", new[] { "Gase wie CO₂ und Methan, die Wärme in der Atmosphäre zurückhalten", "Gase, die die Erde abkühlen", "Nur Sauerstoff und Stickstoff, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Gase wie CO₂ und Methan, die Wärme in der Atmosphäre zurückhalten",
            "Treibhausgase wie CO₂ und Methan halten Wärme in der Atmosphäre zurück und verstärken so die Erderwärmung."),
        ("Welche menschliche Aktivität setzt besonders viel CO₂ frei?", new[] { "Verbrennung fossiler Brennstoffe wie Kohle, Öl und Gas", "Fotosynthese von Pflanzen", "Trinkwasseraufbereitung" }, "Verbrennung fossiler Brennstoffe wie Kohle, Öl und Gas",
            "Die Verbrennung fossiler Brennstoffe in Kraftwerken, Autos und der Industrie ist die Hauptquelle menschengemachter CO₂-Emissionen."),
        ("Was ist eine Folge des Klimawandels für Wetterextreme?", new[] { "Hitzewellen, Dürren und Starkregen nehmen zu", "Es gibt keine Wetterextreme mehr", "Das Wetter wird überall gleich" }, "Hitzewellen, Dürren und Starkregen nehmen zu",
            "Der Klimawandel lässt Wetterextreme wie Hitzewellen, Dürren und Starkregen häufiger und intensiver auftreten."),
        ("Was bedeutet \"CO₂-Fußabdruck\"?", new[] { "Die Menge an Treibhausgasen, die durch das Verhalten einer Person oder eines Landes verursacht wird", "Der Abdruck eines Schuhs im Schnee und deshalb hier nicht zutrifft, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Die Fläche eines Landes" }, "Die Menge an Treibhausgasen, die durch das Verhalten einer Person oder eines Landes verursacht wird",
            "Der CO₂-Fußabdruck misst, wie viele Treibhausgase durch den Lebensstil einer Person, eines Unternehmens oder Landes verursacht werden."),
        ("Warum ist Methan (z.B. aus der Massentierhaltung) klimaschädlich?", new[] { "Es ist ein starkes Treibhausgas, das Wärme in der Atmosphäre zurückhält", "Es kühlt die Atmosphäre ab", "Es hat keinen Einfluss aufs Klima, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Es ist ein starkes Treibhausgas, das Wärme in der Atmosphäre zurückhält",
            "Methan ist ein besonders wirksames Treibhausgas und entsteht u.a. bei der Massentierhaltung und in Mülldeponien."),
        ("Was ist das Ziel des Pariser Klimaabkommens?", new[] { "Die Erderwärmung deutlich unter 2 Grad (möglichst 1,5 Grad) zu begrenzen", "Die Erderwärmung auf 5 Grad zu erhöhen", "Fossile Brennstoffe unbegrenzt zu nutzen" }, "Die Erderwärmung deutlich unter 2 Grad (möglichst 1,5 Grad) zu begrenzen",
            "Im Pariser Klimaabkommen von 2015 einigten sich fast alle Staaten darauf, die Erderwärmung deutlich unter 2, möglichst auf 1,5 Grad zu begrenzen."),
        ("Was können einzelne Menschen tun, um weniger CO₂ zu verursachen?", new[] { "Weniger Auto fahren, mehr Fahrrad/Bus/Bahn nutzen", "Mehr Flugreisen unternehmen (was so in der Praxis nicht zutrifft)", "Mehr Kohle verbrennen" }, "Weniger Auto fahren, mehr Fahrrad/Bus/Bahn nutzen",
            "Wer öfter Fahrrad, Bus oder Bahn statt Auto nutzt, verursacht weniger CO₂-Emissionen."),
        ("Was passiert mit vielen Tier- und Pflanzenarten durch den schnellen Klimawandel?", new[] { "Manche Arten verlieren ihren Lebensraum und sind vom Aussterben bedroht", "Alle Arten profitieren gleichermaßen", "Es gibt keine Auswirkungen auf Tiere und Pflanzen - eine verbreitete, aber falsche Annahme" }, "Manche Arten verlieren ihren Lebensraum und sind vom Aussterben bedroht",
            "Viele Tier- und Pflanzenarten können sich nicht schnell genug an den raschen Klimawandel anpassen und verlieren ihren Lebensraum."),
        ("Was ist ein Unterschied zwischen \"Wetter\" und \"Klima\"?", new[] { "Wetter beschreibt kurzfristige Zustände, Klima den langjährigen Durchschnitt", "Wetter und Klima bedeuten genau dasselbe", "Klima ändert sich nur innerhalb eines Tages" }, "Wetter beschreibt kurzfristige Zustände, Klima den langjährigen Durchschnitt",
            "Wetter ist der kurzfristige Zustand der Atmosphäre, Klima beschreibt das durchschnittliche Wettergeschehen über viele Jahre."),
        ("Warum ist das Abschmelzen der Permafrostböden in der Arktis problematisch fürs Klima?", new[] { "Dabei wird zusätzlich gespeichertes Methan und CO₂ freigesetzt", "Dadurch wird die Erde automatisch kühler", "Permafrostböden haben keinen Einfluss aufs Klima" }, "Dabei wird zusätzlich gespeichertes Methan und CO₂ freigesetzt",
            "Auftauender Permafrost setzt lange gespeichertes Methan und CO₂ frei, was den Klimawandel weiter verstärkt."),
        ("Was bedeutet \"Klimaneutralität\"?", new[] { "Es wird nicht mehr Treibhausgas ausgestoßen, als an anderer Stelle gebunden/eingespart wird", "Es wird gar kein Strom mehr verbraucht, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Es gibt kein Klima mehr" }, "Es wird nicht mehr Treibhausgas ausgestoßen, als an anderer Stelle gebunden/eingespart wird",
            "Klimaneutralität bedeutet, dass ausgestoßene Treibhausgase durch Einsparungen oder Kompensation ausgeglichen werden."),
        ("Welche Energiequelle zählt zu den erneuerbaren Energien?", new[] { "Windkraft", "Braunkohle", "Erdöl" }, "Windkraft",
            "Windkraft ist im Gegensatz zu Kohle und Öl eine erneuerbare, klimafreundlichere Energiequelle."),
        ("Wie beeinflusst der Klimawandel viele Küstenregionen und Inselstaaten?", new[] { "Sie sind durch den steigenden Meeresspiegel zunehmend vom Überfluten bedroht", "Sie werden automatisch größer", "Sie sind überhaupt nicht betroffen, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Sie sind durch den steigenden Meeresspiegel zunehmend vom Überfluten bedroht",
            "Der steigende Meeresspiegel bedroht tief liegende Küstenregionen und Inselstaaten zunehmend mit Überflutung."),
        ("Was zeigen Klimamodelle und Messdaten seit der Industrialisierung?", new[] { "Die globale Durchschnittstemperatur ist deutlich angestiegen", "Die globale Durchschnittstemperatur ist stark gesunken", "Es gab überhaupt keine Veränderung" }, "Die globale Durchschnittstemperatur ist deutlich angestiegen",
            "Seit Beginn der Industrialisierung ist die globale Durchschnittstemperatur nachweislich deutlich angestiegen.")
    };

    private static QuizQuestion Klimawandel(Random r)
    {
        var f = KlimawandelListe[r.Next(KlimawandelListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Klimawandel", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "CO₂ aus fossilen Brennstoffen ist das wichtigste menschengemachte Treibhausgas - es lässt Gletscher/Polkappen schmelzen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] VerstaedterungListe =
    {
        ("Was bedeutet \"Verstädterung\" (Urbanisierung)?", new[] { "Immer mehr Menschen ziehen in Städte", "Immer mehr Menschen ziehen aufs Land", "Städte werden abgerissen" },
            "Immer mehr Menschen ziehen in Städte", "Verstädterung/Urbanisierung beschreibt den weltweiten Trend, dass ein wachsender Anteil der Bevölkerung in Städten lebt."),
        ("Was ist eine typische Herausforderung schnell wachsender Megastädte?", new[] { "Wohnungsmangel und Verkehrsprobleme", "Zu wenig Einwohner", "Zu viel freie Fläche und deshalb hier nicht zutrifft" },
            "Wohnungsmangel und Verkehrsprobleme", "Schnelles Städtewachstum führt oft zu Wohnraumknappheit, Verkehrsstaus und Umweltproblemen."),
        ("Ab wie vielen Einwohnern spricht man üblicherweise von einer \"Megastadt\"?", new[] { "Ab etwa 10 Millionen Einwohnern", "Ab etwa 10.000 Einwohnern", "Ab etwa 100.000 Einwohnern, was so nicht korrekt ist" },
            "Ab etwa 10 Millionen Einwohnern", "Als Megastädte werden meist Ballungsräume mit mehr als 10 Millionen Einwohnern bezeichnet, z.B. Tokio oder Istanbul."),
        ("Warum ziehen viele Menschen vom Land in die Stadt?", new[] { "Wegen mehr Arbeitsplätzen und besserer Infrastruktur", "Weil es in Städten weniger Menschen gibt", "Weil Städte immer billiger sind" },
            "Wegen mehr Arbeitsplätzen und besserer Infrastruktur", "Städte bieten oft mehr Arbeitsplätze, Bildungsangebote und Infrastruktur, was viele Menschen zum Umzug bewegt."),
        ("Was ist ein \"Slum\"?", new[] { "Ein armes, oft provisorisches Wohnviertel am Rand einer schnell wachsenden Stadt", "Ein modernes Einkaufszentrum", "Ein besonders teures Wohnviertel - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden" },
            "Ein armes, oft provisorisches Wohnviertel am Rand einer schnell wachsenden Stadt", "In vielen schnell wachsenden Megastädten entstehen Slums, weil Zuwanderer sich keine regulären Wohnungen leisten können."),
        ("Wie nennt man den Prozess, wenn Menschen vom Land in die Stadt ziehen?", new[] { "Land-Stadt-Wanderung (Binnenmigration)", "Stadtflucht", "Auswanderung, was bei genauerem Hinsehen nicht stimmt" },
            "Land-Stadt-Wanderung (Binnenmigration)", "Wenn Menschen innerhalb eines Landes vom Land in die Stadt ziehen, nennt man das Land-Stadt-Wanderung."),
        ("Was ist ein \"Ballungsraum\"?", new[] { "Ein dicht besiedeltes Gebiet mit einer oder mehreren großen Städten", "Ein dünn besiedeltes ländliches Gebiet", "Eine unbewohnte Wüstenregion" },
            "Ein dicht besiedeltes Gebiet mit einer oder mehreren großen Städten", "Ein Ballungsraum ist ein dicht besiedeltes Gebiet, das durch eine oder mehrere große Städte geprägt ist."),
        ("In welchen Weltregionen wachsen die Städte heute besonders schnell?", new[] { "Vor allem in Asien und Afrika", "Vor allem in der Antarktis", "Nirgendwo, Städte schrumpfen überall" },
            "Vor allem in Asien und Afrika", "Das schnellste Städtewachstum findet heute vor allem in Asien und Afrika statt."),
        ("Was versteht man unter \"Suburbanisierung\"?", new[] { "Menschen ziehen vom Stadtzentrum ins Umland (Vororte)", "Menschen ziehen vom Land direkt in die Innenstadt (was so in der Praxis nicht zutrifft)", "Städte werden komplett aufgegeben" },
            "Menschen ziehen vom Stadtzentrum ins Umland (Vororte)", "Suburbanisierung beschreibt die Abwanderung von Menschen aus der Innenstadt ins städtische Umland."),
        ("Welches Problem entsteht häufig durch schnelles, ungeplantes Städtewachstum?", new[] { "Luftverschmutzung und Verkehrsstaus", "Zu viel unbebaute Fläche", "Zu wenig Einwohner" },
            "Luftverschmutzung und Verkehrsstaus", "Schnell und ungeplant wachsende Städte kämpfen oft mit Luftverschmutzung, Verkehrsstaus und Infrastrukturproblemen."),
        ("Was ist eine \"Metropole\"?", new[] { "Eine sehr große, bedeutende Stadt mit hoher wirtschaftlicher und kultureller Bedeutung", "Ein kleines Dorf", "Eine unbewohnte Insel" },
            "Eine sehr große, bedeutende Stadt mit hoher wirtschaftlicher und kultureller Bedeutung", "Metropolen sind besonders große und bedeutende Städte mit hoher wirtschaftlicher und kultureller Ausstrahlung."),
        ("Warum entstehen in schnell wachsenden Städten oft informelle Siedlungen (z.B. Favelas, Slums)?", new[] { "Weil der offizielle Wohnungsbau nicht mit dem schnellen Bevölkerungswachstum mithält", "Weil zu wenige Menschen in die Stadt ziehen - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Weil es in der Stadt keine Arbeit gibt" },
            "Weil der offizielle Wohnungsbau nicht mit dem schnellen Bevölkerungswachstum mithält", "Wenn der reguläre Wohnungsbau nicht mit dem Bevölkerungswachstum Schritt hält, entstehen oft informelle Siedlungen wie Favelas."),
        ("Was bedeutet \"Landflucht\"?", new[] { "Menschen verlassen ländliche Gebiete, oft wegen fehlender Arbeitsplätze", "Menschen ziehen aus der Stadt aufs Land, obwohl das auf den ersten Blick plausibel klingt", "Bauern bauen mehr Felder an" },
            "Menschen verlassen ländliche Gebiete, oft wegen fehlender Arbeitsplätze", "Landflucht bezeichnet die Abwanderung von Menschen aus ländlichen Gebieten, oft wegen fehlender Arbeitsplätze und Perspektiven."),
        ("Wie wirkt sich Verstädterung häufig auf die Landwirtschaft in ländlichen Gebieten aus?", new[] { "Weniger Arbeitskräfte bleiben auf dem Land, da viele in die Stadt abwandern", "Es gibt danach mehr Bauern als vorher", "Landwirtschaft verschwindet weltweit komplett" },
            "Weniger Arbeitskräfte bleiben auf dem Land, da viele in die Stadt abwandern", "Durch die Abwanderung in die Städte fehlen in vielen ländlichen Regionen Arbeitskräfte für die Landwirtschaft."),
        ("Was ist ein Vorteil des Lebens in einer Großstadt, den viele Zuwanderer suchen?", new[] { "Bessere Chancen auf Arbeit, Bildung und medizinische Versorgung", "Weniger Lärm und Verkehr, was die eigentliche Bedeutung des Begriffs verfehlt", "Mehr freie Naturflächen" },
            "Bessere Chancen auf Arbeit, Bildung und medizinische Versorgung", "Viele Menschen ziehen in Großstädte, weil sie sich dort bessere Chancen auf Arbeit, Bildung und medizinische Versorgung erhoffen."),
        ("Welche Stadt zählt zu den größten Megastädten der Welt?", new[] { "Tokio", "Erfurt", "Kiel" },
            "Tokio", "Der Großraum Tokio zählt mit vielen Millionen Einwohnern zu den größten Megastädten der Welt."),
        ("Was bedeutet \"Gentrifizierung\" in Großstädten?", new[] { "Aufwertung eines Stadtviertels, wodurch Mieten steigen und einkommensschwächere Bewohner verdrängt werden", "Der komplette Abriss einer Stadt und deshalb hier nicht zutrifft, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Das Verbot von Neubauten" },
            "Aufwertung eines Stadtviertels, wodurch Mieten steigen und einkommensschwächere Bewohner verdrängt werden", "Bei der Gentrifizierung wird ein Stadtviertel aufgewertet, wodurch steigende Mieten oft einkommensschwächere Bewohner verdrängen."),
        ("Was ist eine Herausforderung beim Verkehr in schnell wachsenden Megastädten?", new[] { "Überfüllte Straßen und lange Staus", "Zu wenige Autos auf den Straßen", "Keine Verkehrsmittel werden benötigt" },
            "Überfüllte Straßen und lange Staus", "In vielen schnell wachsenden Megastädten führen zu viele Fahrzeuge zu überfüllten Straßen und langen Staus."),
        ("Wie können Städte dem Wohnraummangel durch Verstädterung begegnen?", new[] { "Durch den Bau von bezahlbarem Wohnraum und guter Infrastruktur", "Indem sie keine neuen Wohnungen bauen, auch wenn das manche zunaechst vermuten wuerden", "Indem sie alle Zuwanderer abweisen" },
            "Durch den Bau von bezahlbarem Wohnraum und guter Infrastruktur", "Der gezielte Bau von bezahlbarem Wohnraum und funktionierender Infrastruktur hilft gegen Wohnraummangel in wachsenden Städten."),
        ("Welcher Anteil der Weltbevölkerung lebt heute schätzungsweise in Städten?", new[] { "Über die Hälfte", "Weniger als 5 Prozent", "Niemand" },
            "Über die Hälfte", "Schätzungen zufolge lebt heute weltweit bereits mehr als die Hälfte der Menschen in Städten.")
    };

    private static QuizQuestion Verstaedterung(Random r)
    {
        var f = VerstaedterungListe[r.Next(VerstaedterungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Verstädterung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Verstädterung/Urbanisierung bedeutet: immer mehr Menschen ziehen in Städte - das kann zu Wohnraum- und Verkehrsproblemen führen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArmutReichtumListe =
    {
        ("Was misst der \"Human Development Index\" (HDI) eines Landes ungefähr?", new[] { "Lebensstandard: u.a. Einkommen, Bildung und Lebenserwartung", "Nur die Fläche eines Landes", "Nur die Anzahl der Einwohner, was bei genauerem Hinsehen nicht stimmt" },
            "Lebensstandard: u.a. Einkommen, Bildung und Lebenserwartung", "Der HDI kombiniert Einkommen, Bildungsstand und Lebenserwartung, um den Entwicklungsstand eines Landes zu vergleichen."),
        ("Was ist eine typische Ursache für Armut in vielen Ländern des globalen Südens?", new[] { "Ungleicher Zugang zu Bildung, Wasser und Arbeit", "Zu viel Regen das ganze Jahr über (was so in der Praxis nicht zutrifft)", "Zu wenige Feiertage" },
            "Ungleicher Zugang zu Bildung, Wasser und Arbeit", "Fehlender Zugang zu Bildung, sauberem Wasser, medizinischer Versorgung und fairer Arbeit zählt zu den wichtigsten Armutsursachen."),
        ("Was bedeutet \"Nord-Süd-Gefälle\" in der Geografie?", new[] { "Reichere Industrieländer liegen oft im globalen Norden, ärmere Entwicklungsländer oft im globalen Süden", "Im Süden ist es immer kälter als im Norden", "Alle Länder der Erde sind gleich wohlhabend" },
            "Reichere Industrieländer liegen oft im globalen Norden, ärmere Entwicklungsländer oft im globalen Süden", "Das \"Nord-Süd-Gefälle\" beschreibt vereinfacht, dass wohlhabendere Industrieländer eher im globalen Norden, viele ärmere Entwicklungsländer eher im globalen Süden liegen."),
        ("Was ist ein \"Entwicklungsland\"?", new[] { "Ein Land mit vergleichsweise niedrigem Einkommen, Bildungsstand und Gesundheitsversorgung", "Ein Land, das gerade erst gegründet wurde - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Ein Land ohne eigene Sprache" },
            "Ein Land mit vergleichsweise niedrigem Einkommen, Bildungsstand und Gesundheitsversorgung", "Entwicklungsländer haben im Vergleich zu Industrieländern oft geringeres Einkommen, weniger Bildungschancen und eine schwächere Gesundheitsversorgung."),
        ("Wie kann fairer Handel (Fairtrade) armen Bäuerinnen und Bauern helfen?", new[] { "Durch garantierte, faire Mindestpreise für ihre Produkte", "Indem ihre Produkte billiger verkauft werden müssen", "Indem sie keine Produkte mehr exportieren dürfen" },
            "Durch garantierte, faire Mindestpreise für ihre Produkte", "Fairtrade-Siegel garantieren Erzeugerinnen und Erzeugern faire Mindestpreise, was ihnen ein stabileres Einkommen sichert."),
        ("Was bedeutet \"absolute Armut\"?", new[] { "Wenn grundlegende Bedürfnisse wie Nahrung, Wasser und Wohnraum nicht gedeckt werden können", "Wenn man kein teures Auto besitzt", "Wenn man weniger verdient als der Nachbar" },
            "Wenn grundlegende Bedürfnisse wie Nahrung, Wasser und Wohnraum nicht gedeckt werden können", "Absolute Armut liegt vor, wenn selbst lebensnotwendige Grundbedürfnisse wie Nahrung, Wasser oder Wohnraum nicht gesichert sind."),
        ("Was bedeutet \"relative Armut\"?", new[] { "Wenn jemand deutlich weniger besitzt als der Durchschnitt der Gesellschaft, in der er lebt", "Wenn jemand gar kein Einkommen hat, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Wenn ein Land keine Rohstoffe hat" },
            "Wenn jemand deutlich weniger besitzt als der Durchschnitt der Gesellschaft, in der er lebt", "Relative Armut bemisst sich am Vergleich mit dem Lebensstandard der übrigen Gesellschaft, nicht an absoluten Mindeststandards."),
        ("Was ist ein \"Schwellenland\"?", new[] { "Ein Land auf dem Weg von einem Entwicklungsland zu einem Industrieland", "Ein Land ohne jegliche Wirtschaft", "Ein anderer Name für Industrieland" },
            "Ein Land auf dem Weg von einem Entwicklungsland zu einem Industrieland", "Schwellenländer wie Brasilien oder Indien befinden sich wirtschaftlich zwischen Entwicklungs- und Industrieland."),
        ("Was ist eine typische Ursache für Hunger in manchen Regionen der Welt?", new[] { "Dürren, Kriege und ungerechte Verteilung von Nahrungsmitteln", "Zu viel Regen das ganze Jahr und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Zu viele Supermärkte" },
            "Dürren, Kriege und ungerechte Verteilung von Nahrungsmitteln", "Hunger entsteht oft durch eine Kombination aus Dürren, Kriegen und einer ungerechten Verteilung von Nahrungsmitteln."),
        ("Wie kann Bildung helfen, Armut zu verringern?", new[] { "Bildung eröffnet bessere Chancen auf Arbeit und höheres Einkommen", "Bildung hat keinen Einfluss auf Armut", "Bildung macht Länder automatisch ärmer - eine haeufige, aber unzutreffende Vorstellung" },
            "Bildung eröffnet bessere Chancen auf Arbeit und höheres Einkommen", "Gute Bildung verbessert die Chancen auf qualifizierte Arbeit und ein höheres Einkommen und hilft so, Armut zu verringern."),
        ("Was ist Entwicklungshilfe?", new[] { "Unterstützung reicherer Länder für ärmere Länder, z.B. durch Geld, Wissen oder Projekte", "Ein Handelsverbot zwischen Ländern", "Eine Steuer nur für arme Länder" },
            "Unterstützung reicherer Länder für ärmere Länder, z.B. durch Geld, Wissen oder Projekte", "Entwicklungshilfe umfasst finanzielle, technische oder projektbezogene Unterstützung reicherer für ärmere Länder."),
        ("Was versteht man unter \"Kinderarbeit\" als Folge von Armut?", new[] { "Kinder müssen statt zur Schule zu gehen arbeiten, um zum Familieneinkommen beizutragen", "Kinder arbeiten freiwillig als Hobby", "Kinderarbeit gibt es nur in reichen Ländern" },
            "Kinder müssen statt zur Schule zu gehen arbeiten, um zum Familieneinkommen beizutragen", "In armen Familien müssen Kinder oft statt zur Schule zu gehen arbeiten, um zum Lebensunterhalt der Familie beizutragen."),
        ("Was bedeutet \"ungleiche Einkommensverteilung\" innerhalb eines Landes?", new[] { "Ein kleiner Teil der Bevölkerung besitzt einen sehr großen Teil des Vermögens", "Alle Menschen verdienen exakt gleich viel", "Es gibt in keinem Land Unterschiede" },
            "Ein kleiner Teil der Bevölkerung besitzt einen sehr großen Teil des Vermögens", "Bei ungleicher Einkommensverteilung besitzt ein kleiner, oft sehr reicher Teil der Bevölkerung einen unverhältnismäßig großen Teil des Vermögens."),
        ("Wie wirkt sich fehlender Zugang zu sauberem Trinkwasser auf die Armut aus?", new[] { "Er führt zu Krankheiten und erschwert Bildung und Arbeit", "Er hat keinerlei Auswirkungen", "Er macht ein Land automatisch reicher, auch wenn das manche zunaechst vermuten wuerden" },
            "Er führt zu Krankheiten und erschwert Bildung und Arbeit", "Fehlender Zugang zu sauberem Trinkwasser führt zu Krankheiten und erschwert Schulbesuch und Arbeit, was Armut verfestigt."),
        ("Was sind die \"Sustainable Development Goals\" (SDGs) der Vereinten Nationen?", new[] { "Weltweite Ziele u.a. zur Bekämpfung von Armut und Hunger bis 2030", "Ein Vertrag nur über Klimaschutz", "Ein Handelsabkommen zwischen zwei Ländern, was bei genauerem Hinsehen nicht stimmt" },
            "Weltweite Ziele u.a. zur Bekämpfung von Armut und Hunger bis 2030", "Die 17 SDGs der Vereinten Nationen sollen bis 2030 u.a. Armut und Hunger weltweit deutlich verringern."),
        ("Welche Organisation der Vereinten Nationen kümmert sich besonders um Kinderrechte weltweit?", new[] { "UNICEF", "NATO (was so in der Praxis nicht zutrifft)", "FIFA" },
            "UNICEF", "UNICEF ist das Kinderhilfswerk der Vereinten Nationen und setzt sich weltweit für Kinderrechte ein."),
        ("Was kann internationaler fairer Handel im Vergleich zu herkömmlichem Handel bewirken?", new[] { "Bessere Arbeitsbedingungen und stabilere Einkommen für Produzenten in ärmeren Ländern", "Höhere Gewinne nur für große Konzerne", "Er hat keinerlei Effekt auf die Produzenten - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt" },
            "Bessere Arbeitsbedingungen und stabilere Einkommen für Produzenten in ärmeren Ländern", "Fairer Handel sichert Produzentinnen und Produzenten in ärmeren Ländern oft bessere Arbeitsbedingungen und stabilere Einkommen."),
        ("Was ist ein Mikrokredit?", new[] { "Ein kleiner Kredit, der armen Menschen hilft, ein eigenes kleines Geschäft aufzubauen", "Ein riesiger Kredit nur für Großkonzerne", "Eine Steuer auf Lebensmittel" },
            "Ein kleiner Kredit, der armen Menschen hilft, ein eigenes kleines Geschäft aufzubauen", "Mikrokredite sind kleine Darlehen, die armen Menschen ermöglichen, sich mit einem eigenen kleinen Geschäft selbst zu helfen."),
        ("Welche Rolle spielt Korruption häufig bei der Armutsbekämpfung?", new[] { "Sie kann Hilfsgelder und Ressourcen von den Bedürftigen fernhalten", "Sie hilft immer, Armut schneller zu beseitigen, obwohl das auf den ersten Blick plausibel klingt", "Sie hat keinerlei Einfluss" },
            "Sie kann Hilfsgelder und Ressourcen von den Bedürftigen fernhalten", "Korruption kann dazu führen, dass Hilfsgelder und Ressourcen nicht bei den Menschen ankommen, die sie am meisten brauchen."),
        ("Was bedeutet \"Globalisierung\" im Zusammenhang mit Armut und Reichtum?", new[] { "Die weltweite Vernetzung von Handel und Wirtschaft, die sowohl Chancen als auch Risiken für arme Länder bringt", "Der komplette Rückzug aller Länder von internationalem Handel", "Ein Begriff nur für den Klimawandel" },
            "Die weltweite Vernetzung von Handel und Wirtschaft, die sowohl Chancen als auch Risiken für arme Länder bringt", "Globalisierung vernetzt Wirtschaft und Handel weltweit - das bietet ärmeren Ländern Chancen, birgt aber auch Risiken wie ungleiche Handelsbedingungen.")
    };

    private static QuizQuestion ArmutReichtum(Random r)
    {
        var f = ArmutReichtumListe[r.Next(ArmutReichtumListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Armut und Reichtum weltweit", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Der HDI misst Lebensstandard (Einkommen, Bildung, Lebenserwartung) - Zugang zu Bildung/Wasser/Arbeit ist zentral für Armut/Reichtum."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RessourcenEnergieListe =
    {
        ("Was sind fossile Energieträger?", new[] { "Über Jahrmillionen aus abgestorbenen Lebewesen entstandene Rohstoffe wie Kohle, Erdöl und Erdgas", "Ausschließlich Sonnen- und Windenergie, was die eigentliche Bedeutung des Begriffs verfehlt und deshalb hier nicht zutrifft", "Nur Wasserkraft" }, "Über Jahrmillionen aus abgestorbenen Lebewesen entstandene Rohstoffe wie Kohle, Erdöl und Erdgas",
            "Fossile Energieträger wie Kohle, Erdöl und Erdgas entstanden über Jahrmillionen aus abgestorbenen Pflanzen und Tieren und sind nur endlich vorhanden."),
        ("Warum gelten fossile Energieträger als nicht erneuerbar?", new[] { "Ihre Neubildung dauert Jahrmillionen, viel länger als der menschliche Verbrauch", "Sie bilden sich innerhalb weniger Tage neu, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Sie sind in unbegrenzter Menge vorhanden" }, "Ihre Neubildung dauert Jahrmillionen, viel länger als der menschliche Verbrauch",
            "Da die Neubildung fossiler Rohstoffe Jahrmillionen dauert, gelten sie im menschlichen Zeitmaßstab als endlich und nicht erneuerbar."),
        ("Was sind erneuerbare Energien?", new[] { "Energiequellen wie Sonne, Wind und Wasser, die sich ständig neu erneuern", "Ausschließlich Kohle und Erdöl, auch wenn das manche zunaechst vermuten wuerden", "Nur Kernenergie" }, "Energiequellen wie Sonne, Wind und Wasser, die sich ständig neu erneuern",
            "Erneuerbare Energien wie Sonnen-, Wind- und Wasserkraft stehen im Gegensatz zu fossilen Energieträgern dauerhaft zur Verfügung."),
        ("Was sind \"Seltene Erden\"?", new[] { "Eine Gruppe von Metallen, die u.a. für Smartphones und Elektronik wichtig sind", "Ein anderes Wort für fossile Brennstoffe", "Eine Bezeichnung für besonders fruchtbaren Ackerboden, was bei genauerem Hinsehen nicht stimmt" }, "Eine Gruppe von Metallen, die u.a. für Smartphones und Elektronik wichtig sind",
            "Seltene Erden sind eine Gruppe von Metallen, die trotz des Namens nicht extrem selten, aber schwer abzubauen sind und für moderne Elektronik wie Smartphones gebraucht werden."),
        ("Warum ist der Abbau seltener Erden häufig umstritten?", new[] { "Er verursacht oft erhebliche Umweltbelastungen und schwierige Arbeitsbedingungen", "Er hat überhaupt keine Umweltauswirkungen", "Seltene Erden werden nirgendwo abgebaut" }, "Er verursacht oft erhebliche Umweltbelastungen und schwierige Arbeitsbedingungen",
            "Der Abbau seltener Erden geht oft mit erheblicher Umweltbelastung (z.B. giftige Abfälle) und schwierigen Arbeitsbedingungen in den Förderländern einher."),
        ("Was ist eine \"globale Stoffstromanalyse\" bei einem Produkt wie einem Smartphone?", new[] { "Die Untersuchung, woher die Rohstoffe stammen und wie sie um die Welt transportiert werden", "Eine Analyse, die sich nur mit dem Verkaufspreis befasst (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Ein Begriff, der nur für Lebensmittel verwendet wird" }, "Die Untersuchung, woher die Rohstoffe stammen und wie sie um die Welt transportiert werden",
            "Eine Stoffstromanalyse verfolgt den Weg von Rohstoffen von der Förderung über die Verarbeitung bis zum fertigen Produkt und zeigt globale Lieferketten auf."),
        ("Was ist ein möglicher Grund für Konflikte zwischen Industrienationen und Förderländern beim Abbau fossiler Energieträger?", new[] { "Ungleiche Verteilung von Gewinnen, Umweltschäden und politischer Einfluss auf die Förderregionen", "Es gibt bei fossilen Rohstoffen grundsätzlich keinerlei Konfliktpotenzial, was einer genaueren Pruefung nicht standhaelt", "Förderländer erhalten immer den größten Teil der Gewinne" }, "Ungleiche Verteilung von Gewinnen, Umweltschäden und politischer Einfluss auf die Förderregionen",
            "Beim Abbau fossiler Rohstoffe entstehen häufig Konflikte um die Verteilung der Gewinne, Umweltfolgen vor Ort und politischen Einfluss internationaler Konzerne."),
        ("Was ist Erdgas als Energieträger im Vergleich zu Kohle?", new[] { "Ein fossiler Energieträger, der bei der Verbrennung meist weniger CO₂ freisetzt als Kohle", "Ein erneuerbarer Energieträger wie Windkraft", "Ein Energieträger, der überhaupt keine Emissionen verursacht, obwohl das auf den ersten Blick plausibel klingt" }, "Ein fossiler Energieträger, der bei der Verbrennung meist weniger CO₂ freisetzt als Kohle",
            "Erdgas zählt wie Kohle und Erdöl zu den fossilen Energieträgern, verursacht bei der Verbrennung aber meist weniger CO₂-Emissionen als Kohle."),
        ("Was ist ein Vorteil von Windkraft als Energiequelle?", new[] { "Sie verursacht bei der Stromerzeugung selbst keine CO₂-Emissionen", "Sie ist nur begrenzt oft nutzbar wie fossile Brennstoffe, was die eigentliche Bedeutung des Begriffs verfehlt", "Sie verursacht besonders viel CO₂" }, "Sie verursacht bei der Stromerzeugung selbst keine CO₂-Emissionen",
            "Windkraftanlagen erzeugen Strom, ohne dabei selbst CO₂ auszustoßen, im Gegensatz zur Verbrennung fossiler Energieträger."),
        ("Was ist eine Herausforderung bei der Nutzung von Sonnen- und Windenergie?", new[] { "Ihre Verfügbarkeit schwankt je nach Wetter und Tageszeit", "Sie sind unbegrenzt und konstant zu jeder Zeit verfügbar", "Sie verursachen extrem hohe CO₂-Emissionen" }, "Ihre Verfügbarkeit schwankt je nach Wetter und Tageszeit",
            "Da Sonne und Wind nicht konstant verfügbar sind, braucht es Speicherlösungen oder ein ausgeglichenes Energiesystem, um Schwankungen auszugleichen."),
        ("Was ist eine mögliche Folge starker wirtschaftlicher Abhängigkeit eines Landes von Energieexporten (z.B. Erdöl)?", new[] { "Die wirtschaftliche Lage wird stark von schwankenden Weltmarktpreisen beeinflusst", "Das Land ist von globalen Marktpreisen völlig unabhängig", "Die Wirtschaft profitiert immer gleichmäßig, unabhängig vom Ölpreis und deshalb hier nicht zutrifft" }, "Die wirtschaftliche Lage wird stark von schwankenden Weltmarktpreisen beeinflusst",
            "Länder, deren Wirtschaft stark von Energieexporten abhängt, sind anfällig für Schwankungen der Weltmarktpreise für diese Rohstoffe."),
        ("Was zeigt das Konfliktpotenzial rund um fossile Energieträger häufig zwischen Förderländern und internationalen Konzernen?", new[] { "Ungleiche Verteilung von Gewinnen und Verantwortung für Umweltschäden vor Ort", "Es gibt bei diesem Thema historisch niemals irgendwelche Spannungen", "Konzerne tragen grundsätzlich alle Umweltkosten selbst" }, "Ungleiche Verteilung von Gewinnen und Verantwortung für Umweltschäden vor Ort",
            "Häufig profitieren internationale Konzerne stark von der Rohstoffförderung, während die Umweltfolgen und ein Großteil der Risiken bei der lokalen Bevölkerung verbleiben."),
        ("Warum wird der Ausbau erneuerbarer Energien oft als wichtiger Beitrag zum Klimaschutz gesehen?", new[] { "Sie ersetzen fossile Energieträger und senken so den CO₂-Ausstoß", "Sie erhöhen den CO₂-Ausstoß im Vergleich zu Kohle deutlich, was so nicht korrekt ist", "Sie haben mit dem Klimaschutz nichts zu tun" }, "Sie ersetzen fossile Energieträger und senken so den CO₂-Ausstoß",
            "Da erneuerbare Energien bei der Stromerzeugung kaum CO₂ ausstoßen, senkt ihr Ausbau anstelle fossiler Energieträger die Treibhausgasemissionen."),
        ("Was versteht man unter \"Energiewende\"?", new[] { "Den Umstieg von fossilen und nuklearen Energiequellen hin zu erneuerbaren Energien", "Den kompletten Verzicht auf jegliche Energieerzeugung - eine haeufige, aber unzutreffende Vorstellung", "Eine Umstellung ausschließlich auf Kohlekraft" }, "Den Umstieg von fossilen und nuklearen Energiequellen hin zu erneuerbaren Energien",
            "Die Energiewende bezeichnet den geplanten, langfristigen Umstieg von fossilen und nuklearen Energieträgern auf erneuerbare Energiequellen."),
        ("Welche Rolle spielt Kobalt bei der Produktion von Smartphone- und Elektroauto-Akkus?", new[] { "Es ist ein wichtiger Rohstoff für Batterien, dessen Abbau oft mit schwierigen Arbeitsbedingungen verbunden ist", "Kobalt spielt bei Batterien überhaupt keine Rolle, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Kobalt wird ausschließlich in Europa gefördert" }, "Es ist ein wichtiger Rohstoff für Batterien, dessen Abbau oft mit schwierigen Arbeitsbedingungen verbunden ist",
            "Kobalt wird für viele Batterien benötigt, wird aber oft unter problematischen Arbeitsbedingungen abgebaut, u.a. in der Demokratischen Republik Kongo."),
        ("Was zeigt ein Wirkungsgefüge (Ursache-Folge-Diagramm) beim Thema globaler Rohstoffabbau?", new[] { "Wie wirtschaftliche, soziale und ökologische Faktoren miteinander zusammenhängen", "Ausschließlich die geografische Lage eines Rohstoffvorkommens (was so in der Praxis nicht zutrifft)", "Nur den Verkaufspreis eines Rohstoffs" }, "Wie wirtschaftliche, soziale und ökologische Faktoren miteinander zusammenhängen",
            "Ein Wirkungsgefüge macht sichtbar, wie sich z.B. Rohstoffnachfrage, Umweltfolgen und soziale Bedingungen in Förderregionen gegenseitig beeinflussen."),
        ("Was ist ein Grund, warum Industrienationen stark von importierten Energierohstoffen abhängig sein können?", new[] { "Eigene Vorkommen reichen nicht aus, um den hohen Energiebedarf zu decken", "Industrienationen benötigen grundsätzlich keine Energie", "Industrienationen fördern immer ausreichend eigene Rohstoffe" }, "Eigene Vorkommen reichen nicht aus, um den hohen Energiebedarf zu decken",
            "Viele Industrienationen haben einen so hohen Energiebedarf, dass eigene Rohstoffvorkommen nicht ausreichen und Importe notwendig werden."),
        ("Was kann eine Diversifizierung der Energiequellen (z.B. Mix aus Wind, Sonne, Wasser) für die Versorgungssicherheit eines Landes bedeuten?", new[] { "Weniger Abhängigkeit von einer einzigen Energiequelle oder einem einzigen Lieferland", "Eine größere Abhängigkeit von einem einzigen fossilen Energieträger - eine verbreitete, aber falsche Annahme", "Keinerlei Unterschied zur Nutzung nur einer Energiequelle" }, "Weniger Abhängigkeit von einer einzigen Energiequelle oder einem einzigen Lieferland",
            "Ein diversifizierter Energiemix verringert das Risiko, bei Ausfall oder Preisschwankungen einer einzelnen Energiequelle in Versorgungsschwierigkeiten zu geraten."),
        ("Was zeigt sich, wenn man die weltweite Verteilung von Erdölvorkommen betrachtet?", new[] { "Sie ist geografisch sehr ungleich verteilt, mit großen Vorkommen z.B. im Nahen Osten", "Erdöl ist überall auf der Welt in exakt gleicher Menge vorhanden, was einer genaueren Pruefung nicht standhaelt", "Erdöl kommt ausschließlich in Europa vor" }, "Sie ist geografisch sehr ungleich verteilt, mit großen Vorkommen z.B. im Nahen Osten",
            "Erdölvorkommen sind weltweit sehr ungleich verteilt, mit besonders großen bekannten Reserven u.a. im Nahen Osten."),
        ("Warum wird beim Rohstoffabbau zunehmend auf Recycling (z.B. von Elektroschrott) gesetzt?", new[] { "Um wertvolle Rohstoffe wiederzuverwenden und den Bedarf an neuem Abbau zu senken", "Weil recycelte Rohstoffe grundsätzlich wertlos sind, obwohl das auf den ersten Blick plausibel klingt", "Weil Recycling den Rohstoffbedarf immer erhöht" }, "Um wertvolle Rohstoffe wiederzuverwenden und den Bedarf an neuem Abbau zu senken",
            "Recycling von Elektrogeräten kann wertvolle Rohstoffe wie seltene Erden zurückgewinnen und so den Bedarf an neuem, oft umweltbelastendem Abbau verringern.")
    };

    private static QuizQuestion RessourcenEnergie(Random r)
    {
        var f = RessourcenEnergieListe[r.Next(RessourcenEnergieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Umgang mit Ressourcen: Energie und Rohstoffe", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Fossile Energieträger (Kohle/Öl/Gas) sind endlich und Jahrmillionen alt; erneuerbare Energien (Sonne/Wind/Wasser) erneuern sich ständig - beim Rohstoffabbau (auch seltene Erden) entstehen oft Umwelt- und Verteilungskonflikte."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] LandwirtschaftBodenListe =
    {
        ("Was kennzeichnet die konventionelle Landwirtschaft im Vergleich zum ökologischen Landbau?", new[] { "Sie setzt häufiger chemisch-synthetische Dünger und Pestizide ein", "Sie verzichtet grundsätzlich auf jede Form von Düngung, was die eigentliche Bedeutung des Begriffs verfehlt", "Sie ist gesetzlich in Deutschland verboten" }, "Sie setzt häufiger chemisch-synthetische Dünger und Pestizide ein",
            "Konventionelle Landwirtschaft nutzt häufiger chemisch-synthetische Dünge- und Pflanzenschutzmittel als der ökologische Landbau."),
        ("Was ist ein Grundprinzip des ökologischen Landbaus?", new[] { "Möglichst natürliche Anbaumethoden ohne chemisch-synthetische Pestizide und Dünger", "Ein möglichst hoher Einsatz chemischer Pestizide", "Ausschließlich Anbau in Gewächshäusern" }, "Möglichst natürliche Anbaumethoden ohne chemisch-synthetische Pestizide und Dünger",
            "Der ökologische Landbau verzichtet bewusst auf chemisch-synthetische Pestizide und Düngemittel und setzt stattdessen auf natürliche Anbaumethoden."),
        ("Was bedeutet \"Ressourcenschonung\" in der Landwirtschaft?", new[] { "Böden, Wasser und andere natürliche Ressourcen so nutzen, dass sie langfristig erhalten bleiben", "Möglichst viele Ressourcen auf einmal verbrauchen", "Ressourcen nur einmalig zu nutzen, ohne auf die Zukunft zu achten und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Böden, Wasser und andere natürliche Ressourcen so nutzen, dass sie langfristig erhalten bleiben",
            "Ressourcenschonende Landwirtschaft achtet darauf, Boden, Wasser und andere Ressourcen so zu nutzen, dass sie auch für kommende Generationen erhalten bleiben."),
        ("Was sind biotische Rohstoffe, wie z.B. Holz oder Fisch?", new[] { "Rohstoffe, die aus lebenden Organismen gewonnen werden", "Rohstoffe, die ausschließlich aus Mineralien bestehen", "Ein anderes Wort für fossile Energieträger" }, "Rohstoffe, die aus lebenden Organismen gewonnen werden",
            "Biotische Rohstoffe wie Holz oder Fisch stammen aus lebenden Organismen und können sich - bei nachhaltiger Nutzung - regenerieren."),
        ("Was bedeutet Überfischung der Ozeane?", new[] { "Fischbestände werden schneller entnommen, als sie sich auf natürliche Weise erneuern können", "Es werden zu wenige Fische gefangen", "Fischbestände wachsen dadurch automatisch schneller" }, "Fischbestände werden schneller entnommen, als sie sich auf natürliche Weise erneuern können",
            "Bei Überfischung werden mehr Fische aus dem Meer entnommen, als durch natürliche Fortpflanzung nachwachsen können, wodurch Bestände langfristig schrumpfen."),
        ("Was zeigen Satellitenaufnahmen häufig zum Thema Abholzung?", new[] { "Den Rückgang von Waldflächen über bestimmte Zeiträume hinweg", "Ausschließlich die Bodentemperatur eines Gebiets", "Nur die politischen Grenzen eines Landes" }, "Den Rückgang von Waldflächen über bestimmte Zeiträume hinweg",
            "Satellitenaufnahmen ermöglichen es, den Rückgang von Waldflächen durch Abholzung über die Zeit sichtbar zu machen und zu vergleichen."),
        ("Warum wird eine nachhaltige Forstwirtschaft angestrebt?", new[] { "Damit nicht mehr Holz eingeschlagen wird, als nachwachsen kann", "Damit möglichst schnell der gesamte Wald abgeholzt wird - eine haeufige, aber unzutreffende Vorstellung", "Weil Holz keine begrenzte Ressource ist" }, "Damit nicht mehr Holz eingeschlagen wird, als nachwachsen kann",
            "Nachhaltige Forstwirtschaft sorgt dafür, dass nur so viel Holz genutzt wird, wie durch Aufforstung und natürliches Wachstum nachwächst."),
        ("Was ist ein Nachteil intensiver konventioneller Landwirtschaft für den Boden auf lange Sicht?", new[] { "Der Boden kann durch einseitige Nutzung und Chemieeinsatz an Fruchtbarkeit verlieren", "Der Boden wird dadurch automatisch fruchtbarer", "Intensive Landwirtschaft hat keinerlei Einfluss auf den Boden" }, "Der Boden kann durch einseitige Nutzung und Chemieeinsatz an Fruchtbarkeit verlieren",
            "Einseitige, intensive Bodennutzung und starker Chemieeinsatz können die Bodenqualität und Fruchtbarkeit langfristig verschlechtern."),
        ("Was ist ein Vorteil des ökologischen Landbaus für die Artenvielfalt?", new[] { "Er bietet durch den Verzicht auf chemische Pestizide oft mehr Lebensraum für Insekten und andere Tiere", "Er verringert die Artenvielfalt grundsätzlich", "Er hat keinerlei Auswirkung auf die Artenvielfalt, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Er bietet durch den Verzicht auf chemische Pestizide oft mehr Lebensraum für Insekten und andere Tiere",
            "Da im ökologischen Landbau weniger chemische Pestizide eingesetzt werden, finden Insekten und andere Tiere dort oft bessere Lebensbedingungen."),
        ("Was ist ein Nachteil, den manche dem ökologischen Landbau im Vergleich zur konventionellen Landwirtschaft zuschreiben?", new[] { "Geringere Ernteerträge pro Fläche", "Deutlich höhere Erträge pro Fläche", "Ökologischer Landbau benötigt überhaupt keine Fläche" }, "Geringere Ernteerträge pro Fläche",
            "Ökologischer Landbau erzielt auf derselben Fläche im Schnitt oft geringere Erträge als konventionelle, intensivere Anbaumethoden."),
        ("Was bedeutet \"Monokultur\" für den landwirtschaftlichen Boden?", new[] { "Über lange Zeit wird immer nur eine Pflanzenart angebaut, was den Boden einseitig auslaugen kann", "Es werden auf einer Fläche viele verschiedene Pflanzenarten gemischt (was so in der Praxis nicht zutrifft)", "Auf der Fläche wächst überhaupt keine Pflanze" }, "Über lange Zeit wird immer nur eine Pflanzenart angebaut, was den Boden einseitig auslaugen kann",
            "Wird über lange Zeit stets dieselbe Pflanzenart angebaut (Monokultur), kann das dem Boden einseitig bestimmte Nährstoffe entziehen und ihn auslaugen."),
        ("Was zeigt eine thematische Karte zur globalen Waldbedeckung typischerweise?", new[] { "Wo auf der Welt Waldflächen zu- oder abgenommen haben", "Nur die politischen Ländergrenzen", "Ausschließlich die Bevölkerungsdichte eines Landes - eine verbreitete, aber falsche Annahme" }, "Wo auf der Welt Waldflächen zu- oder abgenommen haben",
            "Thematische Karten zur Waldbedeckung zeigen anschaulich, in welchen Weltregionen Wälder wachsen, stabil bleiben oder durch Abholzung zurückgehen."),
        ("Was ist ein Grund für globale Überfischung mancher Fischbestände?", new[] { "Hohe weltweite Nachfrage kombiniert mit zu intensiver, wenig regulierter Fischerei", "Zu geringe weltweite Nachfrage nach Fisch", "Fischbestände erneuern sich immer schneller als sie gefangen werden, was einer genaueren Pruefung nicht standhaelt" }, "Hohe weltweite Nachfrage kombiniert mit zu intensiver, wenig regulierter Fischerei",
            "Eine Kombination aus hoher globaler Nachfrage und zu intensiver, teils unregulierter Fischerei führt in vielen Meeresregionen zu Überfischung."),
        ("Was können Fangquoten für die Fischerei bewirken?", new[] { "Sie sollen die Menge gefangener Fische begrenzen, um Bestände zu schützen", "Sie erhöhen die gefangene Fischmenge unbegrenzt, obwohl das auf den ersten Blick plausibel klingt", "Sie haben keinerlei Einfluss auf Fischbestände" }, "Sie sollen die Menge gefangener Fische begrenzen, um Bestände zu schützen",
            "Fangquoten legen fest, wie viel Fisch maximal entnommen werden darf, um ein Aussterben oder starkes Schrumpfen der Bestände zu verhindern."),
        ("Was versteht man unter der \"Zukunftsbedeutung\" der Bodennutzung, wenn man ökologischen und konventionellen Landbau vergleicht?", new[] { "Wie nachhaltig eine Anbaumethode langfristig Erträge sichert, ohne den Boden zu schädigen", "Nur den aktuellen Verkaufspreis der Ernte, was die eigentliche Bedeutung des Begriffs verfehlt und deshalb hier nicht zutrifft", "Ausschließlich die Anzahl der Erntehelfer" }, "Wie nachhaltig eine Anbaumethode langfristig Erträge sichert, ohne den Boden zu schädigen",
            "Bei der Zukunftsbedeutung geht es darum, ob eine Anbaumethode auch langfristig, über Generationen hinweg, ausreichende Erträge sichern kann, ohne den Boden zu erschöpfen."),
        ("Was kann übermäßiger Einsatz von Kunstdünger für Gewässer in der Nähe landwirtschaftlicher Flächen bedeuten?", new[] { "Nährstoffe können ins Wasser gelangen und zu übermäßigem Algenwachstum (Eutrophierung) führen", "Kunstdünger hat keinerlei Auswirkung auf nahegelegene Gewässer", "Gewässer werden dadurch automatisch sauberer" }, "Nährstoffe können ins Wasser gelangen und zu übermäßigem Algenwachstum (Eutrophierung) führen",
            "Überschüssiger Dünger kann in Gewässer gelangen und dort ein übermäßiges Algenwachstum (Eutrophierung) auslösen, was anderen Wasserlebewesen schadet."),
        ("Was ist ein Beispiel für nachhaltige Fischerei?", new[] { "Fischfang, der sich an wissenschaftlich ermittelten Fangquoten orientiert", "Fischfang ohne jede Begrenzung der Fangmenge", "Der vollständige Verzicht auf jegliche Fischerei weltweit, was so nicht korrekt ist" }, "Fischfang, der sich an wissenschaftlich ermittelten Fangquoten orientiert",
            "Nachhaltige Fischerei orientiert sich an wissenschaftlich ermittelten Fangquoten, damit sich Fischbestände erholen und erhalten können."),
        ("Was ist ein Vorteil regionaler, saisonaler Landwirtschaftsprodukte gegenüber importierten Produkten hinsichtlich Ressourcenschonung?", new[] { "Kürzere Transportwege sparen häufig Energie und Ressourcen", "Regionale Produkte verbrauchen immer mehr Ressourcen als importierte", "Transportwege spielen für die Ressourcenschonung keine Rolle" }, "Kürzere Transportwege sparen häufig Energie und Ressourcen",
            "Kürzere Transportwege bei regionalen, saisonalen Produkten sparen im Vergleich zu weit importierten Produkten oft Energie und Ressourcen."),
        ("Was zeigt der Vergleich zwischen konventioneller Landwirtschaft und ökologischem Landbau in Bezug auf Pestizideinsatz und Bodenlangzeitgesundheit?", new[] { "Weniger Pestizideinsatz kann langfristig die Bodengesundheit und Artenvielfalt fördern", "Pestizideinsatz hat auf die Bodengesundheit keinerlei Auswirkung - eine haeufige, aber unzutreffende Vorstellung", "Mehr Pestizide verbessern die Bodengesundheit garantiert" }, "Weniger Pestizideinsatz kann langfristig die Bodengesundheit und Artenvielfalt fördern",
            "Studien zeigen häufig, dass geringerer Pestizideinsatz langfristig positive Effekte auf Bodenqualität, Artenvielfalt und Ökosystemgesundheit haben kann."),
        ("Warum ist die Wahl zwischen konventioneller und ökologischer Landwirtschaft auch eine Abwägung zwischen Ertrag und Nachhaltigkeit?", new[] { "Höhere kurzfristige Erträge stehen oft im Spannungsverhältnis zu langfristiger Ressourcenschonung", "Ertrag und Nachhaltigkeit stehen niemals in irgendeinem Zusammenhang, auch wenn das manche zunaechst vermuten wuerden", "Ökologischer Landbau erzielt immer die höheren Erträge" }, "Höhere kurzfristige Erträge stehen oft im Spannungsverhältnis zu langfristiger Ressourcenschonung",
            "Die Entscheidung zwischen den Anbauformen bedeutet oft eine Abwägung zwischen möglichst hohen kurzfristigen Erträgen und langfristiger Schonung von Boden und Ökosystemen.")
    };

    private static QuizQuestion LandwirtschaftUndBoden(Random r)
    {
        var f = LandwirtschaftBodenListe[r.Next(LandwirtschaftBodenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Umgang mit Ressourcen: Landwirtschaft und Boden", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ökologischer Landbau verzichtet auf chemisch-synthetische Pestizide/Dünger und schont Boden/Artenvielfalt, oft aber mit geringeren Erträgen als konventionelle Landwirtschaft; Überfischung und Abholzung gefährden biotische Rohstoffe."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KlimaschutzListe =
    {
        ("Was ist ein Ziel internationaler Klimaschutzmaßnahmen?", new[] { "Den Ausstoß von Treibhausgasen weltweit zu verringern", "Den Ausstoß von Treibhausgasen möglichst zu erhöhen, was bei genauerem Hinsehen nicht stimmt", "Fossile Energieträger unbegrenzt zu fördern" }, "Den Ausstoß von Treibhausgasen weltweit zu verringern",
            "Internationale Klimaschutzmaßnahmen wie das Pariser Abkommen zielen darauf ab, den weltweiten Treibhausgasausstoß deutlich zu senken."),
        ("Was bedeutet \"Anpassung an den Klimawandel\" (Klimaanpassung) im Unterschied zu Klimaschutz?", new[] { "Maßnahmen, um mit bereits unvermeidbaren Folgen des Klimawandels umzugehen (z.B. Deiche, Dürreresistenz)", "Ausschließlich Maßnahmen zur Verringerung des CO₂-Ausstoßes", "Ein anderes Wort für Klimaschutz" }, "Maßnahmen, um mit bereits unvermeidbaren Folgen des Klimawandels umzugehen (z.B. Deiche, Dürreresistenz)",
            "Klimaanpassung zielt darauf ab, sich auf bereits unvermeidbare Folgen des Klimawandels einzustellen, während Klimaschutz die Ursachen (Emissionen) bekämpft."),
        ("Was ist ein typischer Interessenkonflikt beim internationalen Klimaschutz zwischen Industrienationen und Schwellenländern?", new[] { "Industrienationen haben historisch mehr CO₂ ausgestoßen, Schwellenländer wollen sich aber ebenfalls wirtschaftlich entwickeln dürfen", "Es gibt zwischen diesen Ländergruppen niemals unterschiedliche Interessen", "Schwellenländer haben historisch die mit Abstand meisten Emissionen verursacht (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" }, "Industrienationen haben historisch mehr CO₂ ausgestoßen, Schwellenländer wollen sich aber ebenfalls wirtschaftlich entwickeln dürfen",
            "Ein zentraler Streitpunkt ist, dass Industrienationen historisch für einen Großteil der Emissionen verantwortlich sind, während sich Schwellenländer ebenfalls wirtschaftlich entwickeln möchten - oft mithilfe fossiler Energien."),
        ("Was ist eine Aufgabe von Mediation bei internationalen Klimakonflikten?", new[] { "Zwischen unterschiedlichen Interessen (z.B. Industrienationen und Schwellenländern) zu vermitteln", "Ausschließlich eine Konfliktseite zu bevorzugen, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Klimakonflikte vollständig zu ignorieren" }, "Zwischen unterschiedlichen Interessen (z.B. Industrienationen und Schwellenländern) zu vermitteln",
            "Mediation bei Klimakonflikten versucht, zwischen unterschiedlichen nationalen Interessen einen fairen Ausgleich und gemeinsame Lösungen zu finden."),
        ("Was zeigt das Eisbären-Symbol in der medialen Klimawandel-Debatte häufig?", new[] { "Es steht symbolisch für die Bedrohung der Arktis und ihrer Lebewesen durch den Klimawandel", "Es hat mit dem Klimawandel überhaupt nichts zu tun", "Es zeigt ausschließlich wirtschaftliche Aspekte des Klimawandels" }, "Es steht symbolisch für die Bedrohung der Arktis und ihrer Lebewesen durch den Klimawandel",
            "Das Bild des Eisbären auf schmelzendem Eis wird in den Medien oft symbolisch genutzt, um die Bedrohung arktischer Lebensräume durch den Klimawandel zu veranschaulichen."),
        ("Warum kann die mediale Verwendung eines einzelnen Symbols (wie des Eisbären) für die Klimadebatte kritisch hinterfragt werden?", new[] { "Ein einzelnes Symbol kann die Komplexität des globalen Problems stark vereinfachen", "Symbole sind für die mediale Kommunikation grundsätzlich völlig ungeeignet, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein einzelnes Symbol erklärt das gesamte Problem immer vollständig korrekt" }, "Ein einzelnes Symbol kann die Komplexität des globalen Problems stark vereinfachen",
            "Mediale Symbole wie der Eisbär können emotional wirken, laufen aber Gefahr, die vielschichtige globale Klimaproblematik zu stark zu vereinfachen."),
        ("Was ist eine mögliche Klimaschutzmaßnahme auf staatlicher Ebene?", new[] { "Förderung erneuerbarer Energien und CO₂-Bepreisung", "Die unbegrenzte Förderung fossiler Brennstoffe und deshalb hier nicht zutrifft", "Der komplette Verzicht auf jede Klimapolitik" }, "Förderung erneuerbarer Energien und CO₂-Bepreisung",
            "Staaten setzen u.a. auf Förderprogramme für erneuerbare Energien und eine CO₂-Bepreisung, um klimaschädliche Emissionen zu verringern."),
        ("Was bedeutet \"CO₂-Bepreisung\" als klimapolitisches Instrument?", new[] { "Der Ausstoß von CO₂ wird finanziell verteuert, um Anreize zur Emissionsminderung zu schaffen", "CO₂-Ausstoß wird komplett kostenlos gestellt", "Ein anderes Wort für Steuerfreiheit bei fossilen Brennstoffen" }, "Der Ausstoß von CO₂ wird finanziell verteuert, um Anreize zur Emissionsminderung zu schaffen",
            "Bei der CO₂-Bepreisung müssen Unternehmen oder Verbraucher für ausgestoßenes CO₂ zahlen, was einen finanziellen Anreiz zur Emissionsreduktion schafft."),
        ("Was ist ein Beispiel für eine Anpassungsmaßnahme an den Klimawandel in Küstenregionen?", new[] { "Der Bau oder die Erhöhung von Deichen gegen den steigenden Meeresspiegel", "Der vollständige Verzicht auf jeden Küstenschutz", "Das Verbot jeglicher Bebauung in Küstennähe" }, "Der Bau oder die Erhöhung von Deichen gegen den steigenden Meeresspiegel",
            "Der Bau und die Erhöhung von Deichen sind typische Anpassungsmaßnahmen, um Küstenregionen vor einem steigenden Meeresspiegel zu schützen."),
        ("Was für Interessen können bei internationalen Klimaverhandlungen zwischen einem Kohleförderland und einem von Überflutung bedrohten Inselstaat aufeinandertreffen?", new[] { "Wirtschaftliche Interessen am Rohstoffabbau gegenüber der Sorge um das eigene Überleben durch den Klimawandel", "Beide Länder haben in solchen Verhandlungen immer identische Interessen", "Es gibt bei Klimaverhandlungen grundsätzlich keine unterschiedlichen Interessen" }, "Wirtschaftliche Interessen am Rohstoffabbau gegenüber der Sorge um das eigene Überleben durch den Klimawandel",
            "Ein Kohleförderland hat oft wirtschaftliche Interessen am fortgesetzten Rohstoffabbau, während ein von Überflutung bedrohter Inselstaat auf schnellen, ambitionierten Klimaschutz drängt."),
        ("Was ist eine Aufgabe kritischer Medienkompetenz bei der Klimawandel-Berichterstattung?", new[] { "Prüfen, ob eine mediale Darstellung wissenschaftlich fundiert und ausgewogen ist", "Jede mediale Darstellung unhinterfragt zu übernehmen", "Klimawandel-Berichterstattung grundsätzlich zu ignorieren" }, "Prüfen, ob eine mediale Darstellung wissenschaftlich fundiert und ausgewogen ist",
            "Kritische Medienkompetenz bedeutet, mediale Darstellungen zum Klimawandel auf wissenschaftliche Fundiertheit und Ausgewogenheit zu prüfen."),
        ("Was ist ein Beispiel für internationale Klimaschutz-Zusammenarbeit?", new[] { "Das Pariser Klimaabkommen von 2015", "Ein rein nationales Gesetz ohne internationale Beteiligung", "Ein Handelsabkommen ohne jeden Klimabezug" }, "Das Pariser Klimaabkommen von 2015",
            "Das Pariser Klimaabkommen von 2015 ist ein zentrales Beispiel internationaler Zusammenarbeit, in dem sich fast alle Staaten auf gemeinsame Klimaziele einigten."),
        ("Was ist ein Grund, warum Klimaschutzmaßnahmen politisch oft umstritten sind?", new[] { "Sie können kurzfristig wirtschaftliche Kosten verursachen, auch wenn sie langfristig Vorteile bringen", "Klimaschutzmaßnahmen sind politisch völlig unumstritten", "Klimaschutzmaßnahmen verursachen niemals wirtschaftliche Kosten, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Sie können kurzfristig wirtschaftliche Kosten verursachen, auch wenn sie langfristig Vorteile bringen",
            "Da Klimaschutzmaßnahmen oft kurzfristige wirtschaftliche Umstellungskosten mit sich bringen, wird ihr genauer Umfang und ihre Ausgestaltung politisch häufig kontrovers diskutiert."),
        ("Was können Nichtregierungsorganisationen (NGOs) im internationalen Klimaschutz bewirken?", new[] { "Sie können auf Missstände aufmerksam machen und politischen Druck für mehr Klimaschutz aufbauen", "NGOs haben im Klimaschutz keinerlei Bedeutung", "NGOs verhindern grundsätzlich jeden Klimaschutz" }, "Sie können auf Missstände aufmerksam machen und politischen Druck für mehr Klimaschutz aufbauen",
            "NGOs wie Umweltorganisationen setzen sich oft für mehr Klimaschutz ein, indem sie Öffentlichkeit schaffen und politischen Druck auf Regierungen ausüben."),
        ("Was ist ein Beispiel für einen ethischen Rahmen bei internationalen Klimakonflikten?", new[] { "Die Frage, wer die historische Verantwortung für Emissionen trägt und wer die Kosten des Klimaschutzes tragen sollte", "Ausschließlich die Frage nach dem günstigsten Rohstoffpreis, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Ein rein technisches Problem ohne jede ethische Dimension" }, "Die Frage, wer die historische Verantwortung für Emissionen trägt und wer die Kosten des Klimaschutzes tragen sollte",
            "Ein ethischer Rahmen für Klimakonflikte fragt u.a., wer historisch für die meisten Emissionen verantwortlich ist und wie die Kosten des Klimaschutzes gerecht verteilt werden können."),
        ("Was versteht man unter \"Klimagerechtigkeit\"?", new[] { "Die Forderung, dass die Lasten und Folgen des Klimawandels gerecht zwischen reicheren und ärmeren Ländern verteilt werden", "Ein Begriff, der ausschließlich juristische Gerichtsverfahren beschreibt (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Ein anderes Wort für Klimaneutralität" }, "Die Forderung, dass die Lasten und Folgen des Klimawandels gerecht zwischen reicheren und ärmeren Ländern verteilt werden",
            "Klimagerechtigkeit fordert eine faire Verteilung von Verantwortung, Kosten und Folgen des Klimawandels zwischen historisch stark emittierenden und stärker betroffenen, oft ärmeren Ländern."),
        ("Warum sind gerade wirtschaftlich schwächere Länder oft besonders stark von den Folgen des Klimawandels betroffen, obwohl sie historisch wenig dazu beigetragen haben?", new[] { "Sie haben oft weniger finanzielle Mittel für Anpassungsmaßnahmen wie Deiche oder Bewässerungssysteme", "Sie sind grundsätzlich am wenigsten vom Klimawandel betroffen", "Wirtschaftlich schwächere Länder haben historisch die meisten Emissionen verursacht" }, "Sie haben oft weniger finanzielle Mittel für Anpassungsmaßnahmen wie Deiche oder Bewässerungssysteme",
            "Wirtschaftlich schwächeren Ländern fehlen häufig die finanziellen Mittel für aufwendige Anpassungsmaßnahmen, wodurch sie besonders anfällig für Klimafolgen wie Dürren oder Überflutungen sind."),
        ("Was können internationale Klimafonds bewirken?", new[] { "Sie unterstützen finanziell schwächere Länder bei Klimaschutz- und Anpassungsmaßnahmen", "Sie verhindern grundsätzlich jede Form von Klimaschutz", "Sie dienen ausschließlich der Finanzierung fossiler Energieprojekte, was einer genaueren Pruefung nicht standhaelt" }, "Sie unterstützen finanziell schwächere Länder bei Klimaschutz- und Anpassungsmaßnahmen",
            "Internationale Klimafonds sollen ärmere Länder finanziell dabei unterstützen, Klimaschutz- und Anpassungsmaßnahmen umzusetzen, die sie sich sonst kaum leisten könnten."),
        ("Was ist ein Beispiel für eine technologische Klimaschutzmaßnahme in der Industrie?", new[] { "Effizientere, energiesparendere Produktionsverfahren einführen", "Möglichst ineffiziente, energieintensive Verfahren beibehalten", "Technologische Maßnahmen spielen beim Klimaschutz keine Rolle" }, "Effizientere, energiesparendere Produktionsverfahren einführen",
            "Energieeffizientere Produktionsverfahren senken den Energieverbrauch und damit oft auch die Treibhausgasemissionen der Industrie."),
        ("Warum wird bei internationalen Klimaverhandlungen häufig über verbindliche versus freiwillige Klimaziele gestritten?", new[] { "Verbindliche Ziele schaffen mehr Verlässlichkeit, schränken aber die nationale Souveränität stärker ein", "Verbindliche und freiwillige Ziele unterscheiden sich inhaltlich überhaupt nicht, obwohl das auf den ersten Blick plausibel klingt", "Über diese Frage gibt es bei Klimaverhandlungen historisch nie Uneinigkeit" }, "Verbindliche Ziele schaffen mehr Verlässlichkeit, schränken aber die nationale Souveränität stärker ein",
            "Verbindliche Klimaziele gelten als verlässlicher, werden von manchen Staaten aber als stärkerer Eingriff in die eigene politische und wirtschaftliche Souveränität empfunden als freiwillige Zusagen.")
    };

    private static QuizQuestion KlimaschutzInternational(Random r)
    {
        var f = KlimaschutzListe[r.Next(KlimaschutzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Klimaschutz: Internationale Konflikte und Lösungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Klimaschutz bekämpft die Ursachen (Emissionen senken), Klimaanpassung reagiert auf bereits unvermeidbare Folgen; zwischen Industrienationen und Schwellen-/Entwicklungsländern bestehen oft unterschiedliche Interessen und Verantwortungsfragen (Klimagerechtigkeit)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirtschaftsverflechtungListe =
    {
        ("Was bedeutet \"Globalisierung\" in der Wirtschaftsgeografie?", new[] { "Die zunehmende weltweite Vernetzung von Handel, Produktion und Kommunikation", "Der vollständige Rückzug aller Länder aus dem internationalen Handel, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein Begriff, der ausschließlich den Tourismus betrifft" }, "Die zunehmende weltweite Vernetzung von Handel, Produktion und Kommunikation",
            "Globalisierung beschreibt die wachsende weltweite Vernetzung von Wirtschaft, Handel, Kommunikation und Kultur."),
        ("Was ist eine Wertschöpfungskette, z.B. bei einer Jeans?", new[] { "Die Abfolge aller Produktionsschritte von der Rohstoffgewinnung bis zum fertigen Produkt im Laden", "Ein anderes Wort für den Verkaufspreis eines Produkts", "Eine Liste aller Länder, die eine Währung gemeinsam nutzen" }, "Die Abfolge aller Produktionsschritte von der Rohstoffgewinnung bis zum fertigen Produkt im Laden",
            "Eine Wertschöpfungskette zeigt, wie ein Produkt wie eine Jeans von der Baumwollernte über Weberei und Nähen bis zum Verkauf verschiedene Stationen weltweit durchläuft."),
        ("Warum finden viele Produktionsschritte der Textilindustrie heute in Ländern wie Bangladesch oder Vietnam statt?", new[] { "Dort sind die Arbeitskosten meist deutlich niedriger als in Industrieländern", "Weil dort ausschließlich die Baumwolle wächst", "Weil Industrieländer keinerlei Textilien mehr benötigen" }, "Dort sind die Arbeitskosten meist deutlich niedriger als in Industrieländern",
            "Niedrigere Arbeitskosten machen viele asiatische Länder für arbeitsintensive Produktionsschritte wie die Textilherstellung wirtschaftlich attraktiv."),
        ("Was ist ein Kritikpunkt an globalen Wertschöpfungsketten in der Textilindustrie?", new[] { "Teils schlechte Arbeitsbedingungen und niedrige Löhne in den Produktionsländern", "Es gibt an globalen Wertschöpfungsketten grundsätzlich nichts zu kritisieren", "Arbeitsbedingungen sind in jedem Produktionsland automatisch identisch gut" }, "Teils schlechte Arbeitsbedingungen und niedrige Löhne in den Produktionsländern",
            "Globale Wertschöpfungsketten stehen häufig in der Kritik, weil in manchen Produktionsländern niedrige Löhne und schlechte Arbeitsbedingungen bestehen."),
        ("Was ist ein Beispiel für eine Wertschöpfungskette in der Hightech-Industrie?", new[] { "Ein Smartphone, dessen Rohstoffe, Bauteile und Montage über mehrere Länder verteilt sind", "Ein einzelner Apfel vom heimischen Baum", "Ein handgeschriebener Brief" }, "Ein Smartphone, dessen Rohstoffe, Bauteile und Montage über mehrere Länder verteilt sind",
            "Bei einem Smartphone stammen Rohstoffe, Bauteile und die Endmontage oft aus ganz unterschiedlichen Ländern, bevor das fertige Gerät verkauft wird."),
        ("Was versteht man unter \"Herkunftsgebieten\" im Tourismus?", new[] { "Die Regionen, aus denen die meisten Touristinnen und Touristen stammen", "Die Zielorte, die von Touristen besucht werden", "Ein anderes Wort für Reisebüros" }, "Die Regionen, aus denen die meisten Touristinnen und Touristen stammen",
            "Herkunftsgebiete im Tourismus sind die Regionen oder Länder, aus denen die meisten Reisenden stammen, z.B. wohlhabendere Industrieländer."),
        ("Was versteht man unter \"Destinationsgebieten\" im Tourismus?", new[] { "Die beliebten Reiseziele, die von Touristen besucht werden", "Die Herkunftsländer der meisten Reisenden und deshalb hier nicht zutrifft", "Ein anderes Wort für Flughäfen" }, "Die beliebten Reiseziele, die von Touristen besucht werden",
            "Destinationsgebiete sind die Zielregionen des Tourismus, oft mit besonderen landschaftlichen, kulturellen oder klimatischen Anziehungspunkten."),
        ("Was ist eine wirtschaftliche Chance des Massentourismus für ein Zielgebiet?", new[] { "Einnahmen und Arbeitsplätze durch Hotellerie, Gastronomie und Dienstleistungen", "Massentourismus bringt einem Zielgebiet ausschließlich Nachteile", "Massentourismus verhindert grundsätzlich jede wirtschaftliche Entwicklung" }, "Einnahmen und Arbeitsplätze durch Hotellerie, Gastronomie und Dienstleistungen",
            "Massentourismus kann für Zielregionen bedeutende Einnahmen und Arbeitsplätze in Hotellerie, Gastronomie und weiteren Dienstleistungen schaffen."),
        ("Was ist ein sozialer/ökologischer Nachteil von Massentourismus für lokale Destinationen?", new[] { "Überlastung von Infrastruktur, Umwelt und teils Verdrängung der einheimischen Bevölkerung", "Massentourismus hat grundsätzlich keinerlei negative Auswirkungen", "Massentourismus verringert automatisch den Ressourcenverbrauch vor Ort, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Überlastung von Infrastruktur, Umwelt und teils Verdrängung der einheimischen Bevölkerung",
            "Massentourismus kann lokale Infrastruktur und Umwelt überlasten und in manchen Regionen zu steigenden Preisen oder Verdrängung der einheimischen Bevölkerung führen."),
        ("Was ist ein Beispiel für den Wandel lokaler Wirtschaftsstandorte durch Globalisierung?", new[] { "Traditionelle Industriestandorte verlieren an Bedeutung, während neue Standorte (z.B. in Asien) wachsen", "Wirtschaftsstandorte verändern sich durch Globalisierung grundsätzlich nie, auch wenn das manche zunaechst vermuten wuerden", "Alle Wirtschaftsstandorte weltweit entwickeln sich immer exakt identisch" }, "Traditionelle Industriestandorte verlieren an Bedeutung, während neue Standorte (z.B. in Asien) wachsen",
            "Im Zuge der Globalisierung verlagern sich viele Produktionsstandorte, wodurch traditionelle Industrieregionen schrumpfen und neue Wirtschaftszentren, oft in Asien, wachsen können."),
        ("Was ist ein Beispiel für die soziale Dimension globaler Lieferketten in der Hightech-Industrie?", new[] { "Arbeitsbedingungen in Fabriken, die elektronische Bauteile zusammensetzen", "Ausschließlich die Farbe des Verpackungskartons", "Der Name des Herstellerlandes allein, ohne weitere Bedeutung, was bei genauerem Hinsehen nicht stimmt" }, "Arbeitsbedingungen in Fabriken, die elektronische Bauteile zusammensetzen",
            "Die Arbeitsbedingungen in den Fabriken, die z.B. Smartphones zusammenbauen, sind ein zentraler sozialer Aspekt globaler Wertschöpfungsketten."),
        ("Was zeigt eine sachlogisch geordnete Beschreibung einer Wertschöpfungskette (z.B. bei einer Jeans)?", new[] { "Die einzelnen Produktionsschritte in ihrer zeitlichen und logischen Abfolge", "Nur den Endpreis des Produkts im Geschäft", "Ausschließlich die Werbung für das Produkt" }, "Die einzelnen Produktionsschritte in ihrer zeitlichen und logischen Abfolge",
            "Eine sachlogische Darstellung einer Wertschöpfungskette ordnet die Produktionsschritte - z.B. Baumwollanbau, Spinnen, Weben, Färben, Nähen, Transport, Verkauf - in ihrer sinnvollen Reihenfolge an."),
        ("Was ist \"Overtourism\"?", new[] { "Ein Zustand, in dem so viele Touristen an einen Ort kommen, dass Infrastruktur und Lebensqualität der Einheimischen leiden", "Ein anderes Wort für sanften, nachhaltigen Tourismus", "Ein Begriff für besonders wenige Touristen an einem Ort (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" }, "Ein Zustand, in dem so viele Touristen an einen Ort kommen, dass Infrastruktur und Lebensqualität der Einheimischen leiden",
            "Overtourism beschreibt eine Situation, in der die Touristenzahlen so hoch sind, dass sie Infrastruktur, Umwelt und das Leben der lokalen Bevölkerung stark belasten."),
        ("Was kann eine Diversifizierung der Tourismusangebote (z.B. weg von reinem Massentourismus) für eine Destination bewirken?", new[] { "Eine gleichmäßigere Verteilung von Einnahmen und geringere Belastung einzelner Hotspots", "Eine noch stärkere Konzentration auf einen einzigen überlaufenen Ort, was einer genaueren Pruefung nicht standhaelt", "Keinerlei Veränderung gegenüber Massentourismus" }, "Eine gleichmäßigere Verteilung von Einnahmen und geringere Belastung einzelner Hotspots",
            "Werden touristische Angebote diversifiziert, können Einnahmen gleichmäßiger verteilt und einzelne überlastete Orte entlastet werden."),
        ("Was bedeutet \"outsourcen\" von Produktionsschritten in der globalisierten Wirtschaft?", new[] { "Bestimmte Arbeitsschritte werden an externe Firmen oder ins Ausland ausgelagert", "Alle Produktionsschritte werden ausschließlich im eigenen Land durchgeführt, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für den Import von Rohstoffen" }, "Bestimmte Arbeitsschritte werden an externe Firmen oder ins Ausland ausgelagert",
            "Beim Outsourcing lagern Unternehmen bestimmte Produktions- oder Dienstleistungsschritte an externe Firmen aus, oft in Länder mit niedrigeren Kosten."),
        ("Was ist ein möglicher Vorteil für Verbraucherinnen und Verbraucher durch globale Wertschöpfungsketten?", new[] { "Oft niedrigere Preise durch kostengünstigere Produktion in verschiedenen Ländern", "Ausschließlich höhere Preise für alle Produkte", "Globale Wertschöpfungsketten haben für Verbraucher keinerlei Auswirkung, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Oft niedrigere Preise durch kostengünstigere Produktion in verschiedenen Ländern",
            "Durch die Verteilung der Produktion auf Länder mit günstigeren Produktionskosten profitieren Verbraucherinnen und Verbraucher oft von niedrigeren Preisen."),
        ("Was ist ein Beispiel für die Systeme-Kompetenz beim Verständnis globaler Wertschöpfungsketten?", new[] { "Zusammenhänge zwischen Rohstoffgewinnung, Produktion, Transport und Konsum zu erkennen", "Ausschließlich einen einzelnen Produktionsschritt isoliert zu betrachten", "Wertschöpfungsketten grundsätzlich nicht analysieren zu können" }, "Zusammenhänge zwischen Rohstoffgewinnung, Produktion, Transport und Konsum zu erkennen",
            "Systeme-Kompetenz bedeutet, die Zusammenhänge zwischen den verschiedenen Stationen einer Wertschöpfungskette - von der Rohstoffgewinnung bis zum Konsum - zu durchschauen."),
        ("Was ist ein Grund, warum manche Regionen besonders stark vom internationalen Tourismus wirtschaftlich abhängig sind?", new[] { "Der Tourismussektor bietet dort einen Großteil der verfügbaren Arbeitsplätze und Einnahmen", "Tourismus spielt in solchen Regionen wirtschaftlich überhaupt keine Rolle und deshalb hier nicht zutrifft", "Diese Regionen haben zahlreiche andere, ebenso starke Wirtschaftszweige" }, "Der Tourismussektor bietet dort einen Großteil der verfügbaren Arbeitsplätze und Einnahmen",
            "In manchen Regionen mit wenigen anderen Wirtschaftszweigen macht der Tourismus einen Großteil der Arbeitsplätze und wirtschaftlichen Einnahmen aus."),
        ("Was ist eine Folge, wenn touristische Nachfrage plötzlich stark einbricht (z.B. durch eine Krise)?", new[] { "Stark tourismusabhängige Regionen können erhebliche wirtschaftliche Einbußen erleiden", "Stark tourismusabhängige Regionen bleiben davon völlig unberührt", "Die Wirtschaft solcher Regionen wird dadurch automatisch stärker" }, "Stark tourismusabhängige Regionen können erhebliche wirtschaftliche Einbußen erleiden",
            "Bricht die touristische Nachfrage ein, können Regionen, die wirtschaftlich stark vom Tourismus abhängen, erhebliche Einnahme- und Arbeitsplatzverluste erleiden."),
        ("Was ist ein Grund, warum manche Unternehmen ihre Produktion auf mehrere Länder statt nur auf ein einziges verteilen?", new[] { "Um Kosten, Fachwissen und Risiken auf mehrere Standorte zu verteilen", "Um die Produktion bewusst ineffizienter zu gestalten", "Weil dadurch grundsätzlich keinerlei wirtschaftliche Vorteile entstehen" }, "Um Kosten, Fachwissen und Risiken auf mehrere Standorte zu verteilen",
            "Durch die Verteilung der Produktion auf mehrere Länder können Unternehmen von unterschiedlichen Kostenvorteilen, Fachwissen und einer breiteren Risikoverteilung profitieren.")
    };

    private static QuizQuestion WirtschaftlicheVerflechtung(Random r)
    {
        var f = WirtschaftsverflechtungListe[r.Next(WirtschaftsverflechtungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wirtschaftliche Verflechtungen und Globalisierung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Eine Wertschöpfungskette zeigt alle Produktionsstationen eines Produkts über mehrere Länder hinweg; Massentourismus bringt Zielregionen wirtschaftliche Chancen, aber auch soziale/ökologische Belastungen (Overtourism)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EuropaWirtschaftsraumListe =
    {
        ("Was macht die naturräumliche Vielfalt Europas aus?", new[] { "Verschiedene Klimazonen, Landschaften und Gebirge von der Mittelmeerküste bis zur Skandinavischen Halbinsel", "Ganz Europa hat ein einziges, völlig einheitliches Klima", "Europa besteht ausschließlich aus flachem Tiefland" }, "Verschiedene Klimazonen, Landschaften und Gebirge von der Mittelmeerküste bis zur Skandinavischen Halbinsel",
            "Europa vereint sehr unterschiedliche Naturräume, vom mediterranen Süden über die Alpen bis zu den kühlen nordischen Regionen."),
        ("Was bedeutet \"europäische Identität\" im Zusammenhang mit Sprache, Währung und Kultur?", new[] { "Ein Gefühl der Zugehörigkeit zu Europa trotz sprachlicher und kultureller Vielfalt der Mitgliedstaaten", "Dass alle Europäer dieselbe Sprache und Kultur haben", "Ein Begriff, der mit der EU nichts zu tun hat" }, "Ein Gefühl der Zugehörigkeit zu Europa trotz sprachlicher und kultureller Vielfalt der Mitgliedstaaten",
            "Europäische Identität beschreibt ein gemeinsames Zugehörigkeitsgefühl, das trotz unterschiedlicher Sprachen, Währungen und Kulturen der einzelnen Länder besteht."),
        ("Was sind ökonomische Disparitäten innerhalb Europas?", new[] { "Wirtschaftliche Unterschiede zwischen reicheren und ärmeren Regionen oder Ländern Europas", "Ein Zustand, in dem alle europäischen Regionen wirtschaftlich exakt gleich stark sind", "Ein anderes Wort für die gemeinsame Währung Euro" }, "Wirtschaftliche Unterschiede zwischen reicheren und ärmeren Regionen oder Ländern Europas",
            "Ökonomische Disparitäten zeigen sich z.B. in unterschiedlichem Einkommen, Arbeitslosigkeit oder Wirtschaftsleistung zwischen verschiedenen Regionen und Ländern Europas."),
        ("Was zeigt eine thematische Karte zur Arbeitslosigkeit in Europa typischerweise?", new[] { "Regionale Unterschiede in der Arbeitslosenquote zwischen verschiedenen europäischen Regionen", "Ausschließlich die Bevölkerungsdichte, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Nur die Anzahl der Nationalparks" }, "Regionale Unterschiede in der Arbeitslosenquote zwischen verschiedenen europäischen Regionen",
            "Thematische Karten zur Arbeitslosigkeit machen sichtbar, wie stark sich die wirtschaftliche Lage zwischen verschiedenen Regionen Europas unterscheiden kann."),
        ("Was ist ein Beispiel für grenzübergreifende Zusammenarbeit in Europa?", new[] { "Gemeinsame Projekte und Absprachen im Alpenraum oder Ostseeraum zwischen mehreren Ländern", "Länder, die jegliche Zusammenarbeit über Grenzen hinweg strikt ablehnen", "Ein Begriff, der ausschließlich innerhalb eines einzigen Landes gilt" }, "Gemeinsame Projekte und Absprachen im Alpenraum oder Ostseeraum zwischen mehreren Ländern",
            "Regionen wie der Alpenraum oder der Ostseeraum arbeiten oft länderübergreifend zusammen, z.B. bei Umweltschutz, Verkehr oder Wirtschaft."),
        ("Was ist ein Vorteil offener Grenzen innerhalb der EU (Schengen-Raum)?", new[] { "Erleichterter Handel, Reiseverkehr und Zusammenarbeit zwischen den Mitgliedstaaten", "Es gibt dadurch keinerlei Reise- oder Handelsmöglichkeiten mehr", "Offene Grenzen existieren in der EU grundsätzlich nicht" }, "Erleichterter Handel, Reiseverkehr und Zusammenarbeit zwischen den Mitgliedstaaten",
            "Offene Binnengrenzen im Schengen-Raum erleichtern Reisen, Handel und Zusammenarbeit zwischen den beteiligten europäischen Staaten erheblich."),
        ("Was ist eine mögliche Herausforderung, die mit offenen Grenzen einhergehen kann?", new[] { "Nationale Interessen (z.B. bei Grenzkontrollen oder Migration) können mit dem Prinzip offener Grenzen in Konflikt geraten", "Offene Grenzen bringen ausschließlich Vorteile ohne jede Herausforderung, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Offene Grenzen haben mit nationalen Interessen nichts zu tun" }, "Nationale Interessen (z.B. bei Grenzkontrollen oder Migration) können mit dem Prinzip offener Grenzen in Konflikt geraten",
            "Offene Grenzen können in Konflikt mit nationalen Interessen geraten, etwa bei Fragen der Grenzkontrolle, Sicherheit oder Migration."),
        ("Was zeigt der Vergleich der Wirtschaftsleistung zwischen Nord- und Südeuropa häufig?", new[] { "Teils deutliche wirtschaftliche Unterschiede zwischen verschiedenen europäischen Regionen", "Dass ganz Europa wirtschaftlich exakt gleich stark ist", "Dass es in Europa überhaupt keine wirtschaftlichen Unterschiede gibt" }, "Teils deutliche wirtschaftliche Unterschiede zwischen verschiedenen europäischen Regionen",
            "Zwischen verschiedenen Regionen Europas, etwa zwischen Nord- und Südeuropa, bestehen teils deutliche Unterschiede in Wirtschaftskraft und Arbeitslosigkeit."),
        ("Was ist der Alpenraum als Region grenzübergreifender Zusammenarbeit?", new[] { "Ein Gebirgsraum, der sich über mehrere Länder wie Österreich, die Schweiz, Italien und Frankreich erstreckt", "Eine Region, die vollständig innerhalb eines einzigen Landes liegt (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Ein anderes Wort für den Ostseeraum" }, "Ein Gebirgsraum, der sich über mehrere Länder wie Österreich, die Schweiz, Italien und Frankreich erstreckt",
            "Der Alpenraum erstreckt sich über mehrere Länder und erfordert deshalb oft länderübergreifende Absprachen, z.B. bei Verkehr, Tourismus und Umweltschutz."),
        ("Was ist der Ostseeraum als Region grenzübergreifender Zusammenarbeit?", new[] { "Eine Region rund um die Ostsee mit mehreren angrenzenden Ländern, die z.B. beim Umweltschutz zusammenarbeiten", "Ein Gebiet, das nur aus einem einzigen Land besteht", "Ein anderes Wort für den Alpenraum" }, "Eine Region rund um die Ostsee mit mehreren angrenzenden Ländern, die z.B. beim Umweltschutz zusammenarbeiten",
            "Die Ostsee-Anrainerstaaten arbeiten in vielen Bereichen zusammen, u.a. beim gemeinsamen Schutz der Meeresumwelt."),
        ("Was ist ein Vorteil, den Bürgerinnen und Bürger im Alltag durch die EU erfahren können?", new[] { "Erleichtertes Reisen, Studieren und Arbeiten in anderen Mitgliedstaaten", "Ausschließlich zusätzliche bürokratische Hürden ohne jeden Nutzen", "Die EU hat auf den Alltag der Bürgerinnen und Bürger keinerlei Einfluss" }, "Erleichtertes Reisen, Studieren und Arbeiten in anderen Mitgliedstaaten",
            "Durch die EU können Bürgerinnen und Bürger oft einfacher in anderen Mitgliedstaaten reisen, studieren oder arbeiten."),
        ("Was zeigt eine räumliche Orientierung anhand topografischer Karten über naturräumliche Vielfalt Europas?", new[] { "Wo Gebirge, Tiefebenen, Küsten und Flüsse in Europa liegen", "Ausschließlich die Verteilung von Sprachgruppen, was einer genaueren Pruefung nicht standhaelt", "Nur die Grenzen der Eurozone" }, "Wo Gebirge, Tiefebenen, Küsten und Flüsse in Europa liegen",
            "Topografische Karten zeigen die naturräumliche Gliederung Europas - Gebirge wie die Alpen, Tiefebenen, Küstenregionen und große Flüsse."),
        ("Was bedeutet \"nationale Interessen vs. offene Grenzen\" als Diskussionsthema der EU?", new[] { "Die Abwägung zwischen dem Nutzen offener Grenzen und dem Wunsch einzelner Staaten nach eigener Kontrolle", "Ein Thema, das in der EU niemals diskutiert wird", "Ein Begriff ausschließlich für wirtschaftliche Fragen ohne politischen Bezug" }, "Die Abwägung zwischen dem Nutzen offener Grenzen und dem Wunsch einzelner Staaten nach eigener Kontrolle",
            "In der EU wird immer wieder diskutiert, wie der Nutzen offener Grenzen mit dem Wunsch einzelner Mitgliedstaaten nach eigener Kontrolle in Einklang gebracht werden kann."),
        ("Was ist ein Beispiel für soziale Disparitäten innerhalb Europas?", new[] { "Unterschiede beim Zugang zu Bildung, Gesundheitsversorgung oder Einkommen zwischen Regionen", "Ein Zustand, in dem alle Menschen in Europa exakt gleich viel verdienen, obwohl das auf den ersten Blick plausibel klingt", "Ein Begriff, der ausschließlich das Klima betrifft" }, "Unterschiede beim Zugang zu Bildung, Gesundheitsversorgung oder Einkommen zwischen Regionen",
            "Soziale Disparitäten in Europa zeigen sich z.B. in unterschiedlichem Zugang zu Bildung, medizinischer Versorgung oder im Einkommensniveau verschiedener Regionen."),
        ("Was ist ein Beispiel für ökologische Disparitäten innerhalb Europas?", new[] { "Unterschiedlich starke Umweltbelastungen oder unterschiedlicher Ausbau erneuerbarer Energien je nach Region", "Ein Zustand, in dem alle Regionen exakt dieselbe Umweltqualität haben, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein Begriff, der nur für außereuropäische Länder gilt" }, "Unterschiedlich starke Umweltbelastungen oder unterschiedlicher Ausbau erneuerbarer Energien je nach Region",
            "Ökologische Disparitäten zeigen sich z.B. in unterschiedlich hoher Umweltbelastung oder unterschiedlich weit fortgeschrittenem Ausbau erneuerbarer Energien zwischen europäischen Regionen."),
        ("Was können Regionen mit ähnlichen naturräumlichen Herausforderungen (z.B. Alpenraum) durch Zusammenarbeit über Landesgrenzen hinweg erreichen?", new[] { "Gemeinsame Lösungen für Probleme wie Verkehr, Tourismus oder Umweltschutz", "Weniger Handlungsmöglichkeiten als bei rein nationalem Vorgehen", "Zusammenarbeit über Landesgrenzen hinweg bringt grundsätzlich keinerlei Vorteile" }, "Gemeinsame Lösungen für Probleme wie Verkehr, Tourismus oder Umweltschutz",
            "Länderübergreifende Zusammenarbeit in Regionen wie dem Alpenraum ermöglicht oft effektivere, gemeinsame Lösungen für Herausforderungen, die eine einzelne Landesgrenze überschreiten."),
        ("Was ist ein Grund, warum die Angleichung wirtschaftlicher Unterschiede zwischen europäischen Regionen politisch gefördert wird (z.B. durch EU-Förderprogramme)?", new[] { "Um mehr wirtschaftlichen Zusammenhalt und Chancengleichheit innerhalb der EU zu erreichen", "Weil wirtschaftliche Unterschiede in der EU grundsätzlich unerwünscht ignoriert werden sollen", "Weil EU-Förderprogramme ausschließlich reichen Regionen zugutekommen sollen" }, "Um mehr wirtschaftlichen Zusammenhalt und Chancengleichheit innerhalb der EU zu erreichen",
            "EU-Förderprogramme für strukturschwächere Regionen sollen wirtschaftliche Unterschiede verringern und den Zusammenhalt innerhalb der Europäischen Union stärken."),
        ("Was zeigt die naturräumliche und wirtschaftliche Vielfalt Europas insgesamt über die Herausforderungen der europäischen Integration?", new[] { "Gemeinsame europäische Politik muss sehr unterschiedliche Regionen und Interessen berücksichtigen", "Europa ist naturräumlich und wirtschaftlich vollständig einheitlich, weshalb es keine Herausforderungen gibt", "Europäische Integration hat mit regionaler Vielfalt nichts zu tun" }, "Gemeinsame europäische Politik muss sehr unterschiedliche Regionen und Interessen berücksichtigen",
            "Die große naturräumliche und wirtschaftliche Vielfalt Europas macht deutlich, wie anspruchsvoll es ist, gemeinsame europäische Politik zu gestalten, die allen Regionen gerecht wird."),
        ("Was versteht man unter dem Konzept der \"multiperspektivischen\" Diskussion von EU-Chancen und -Herausforderungen?", new[] { "Verschiedene Sichtweisen (z.B. wirtschaftlich, politisch, regional) gleichberechtigt zu berücksichtigen", "Ausschließlich eine einzige, feste Sichtweise als allein gültig zu betrachten und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Ein Begriff, der mit der EU nichts zu tun hat" }, "Verschiedene Sichtweisen (z.B. wirtschaftlich, politisch, regional) gleichberechtigt zu berücksichtigen",
            "Eine multiperspektivische Betrachtung der EU berücksichtigt unterschiedliche Blickwinkel gleichberechtigt, statt nur eine einzelne Sichtweise gelten zu lassen."),
        ("Was ist ein Beispiel für eine naturräumliche Herausforderung, die mehrere europäische Länder gemeinsam betrifft?", new[] { "Der Schutz der Meeresumwelt in einem gemeinsamen Meeresraum wie der Ostsee", "Ein Naturraum, der ausschließlich innerhalb eines einzigen Landes liegt - eine haeufige, aber unzutreffende Vorstellung", "Ein Thema, das keinerlei Länder gemeinsam betrifft" }, "Der Schutz der Meeresumwelt in einem gemeinsamen Meeresraum wie der Ostsee",
            "Gemeinsame Naturräume wie die Ostsee erfordern länderübergreifende Zusammenarbeit, etwa beim Schutz der Meeresumwelt vor Verschmutzung.")
    };

    private static QuizQuestion EuropaWirtschaftsraum(Random r)
    {
        var f = EuropaWirtschaftsraumListe[r.Next(EuropaWirtschaftsraumListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse9,
            Topic = "Europa in der Welt (naturräumliche und wirtschaftliche Vielfalt)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Europa vereint große naturräumliche Vielfalt und wirtschaftliche/soziale/ökologische Disparitäten zwischen Regionen; grenzübergreifende Zusammenarbeit (Alpenraum, Ostseeraum) hilft bei gemeinsamen Herausforderungen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RisikoraeumeListe =
    {
        ("Was versteht man unter einem \"Risikoraum\" in der Geografie?", new[] { "Ein Gebiet, in dem Naturgefahren wie Erdbeben, Vulkane oder Überschwemmungen besonders häufig auftreten", "Ein Gebiet, das komplett frei von jeglichen Gefahren ist", "Ein Gebiet, das nur auf Landkarten existiert" }, "Ein Gebiet, in dem Naturgefahren wie Erdbeben, Vulkane oder Überschwemmungen besonders häufig auftreten",
            "Risikoräume sind Regionen, in denen bestimmte Naturgefahren durch die geografische Lage besonders wahrscheinlich sind."),
        ("Warum leben trotz der Gefahr viele Menschen in der Nähe von Vulkanen?", new[] { "Der Boden dort ist durch Vulkanasche oft sehr fruchtbar", "Vulkane sind für Menschen völlig ungefährlich", "Es gibt dort besonders wenig Naturkatastrophen, auch wenn das manche zunaechst vermuten wuerden" }, "Der Boden dort ist durch Vulkanasche oft sehr fruchtbar",
            "Vulkanasche liefert wertvolle Nährstoffe für den Boden, weshalb Landwirtschaft rund um Vulkane oft sehr ertragreich ist."),
        ("Was ist ein Tsunami?", new[] { "Eine sehr große Meereswelle, meist ausgelöst durch ein Seebeben", "Ein starker, lang anhaltender Regen", "Ein anderes Wort für Vulkanausbruch" }, "Eine sehr große Meereswelle, meist ausgelöst durch ein Seebeben",
            "Ein Tsunami entsteht meist durch ein Erdbeben unter dem Meeresboden, das eine riesige, sich ausbreitende Welle erzeugt."),
        ("Warum sind manche Küstenregionen besonders von Tsunamis bedroht?", new[] { "Sie liegen in der Nähe von Erdbebenzonen unter dem Meer", "Sie liegen besonders weit vom Meer entfernt", "Tsunamis treten nur in Wüstenregionen auf" }, "Sie liegen in der Nähe von Erdbebenzonen unter dem Meer",
            "Küsten in der Nähe untermeerischer Erdbebenzonen, z.B. am Pazifischen Feuerring, sind besonders tsunamigefährdet."),
        ("Was ist ein Hurrikan bzw. Taifun?", new[] { "Ein sehr starker tropischer Wirbelsturm mit hohen Windgeschwindigkeiten und Starkregen", "Ein leichter, angenehmer Sommerwind, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Ein anderes Wort für Erdbeben" }, "Ein sehr starker tropischer Wirbelsturm mit hohen Windgeschwindigkeiten und Starkregen",
            "Hurrikans (Atlantik) und Taifune (Pazifik) sind unterschiedliche Namen für dieselbe Art tropischer Wirbelstürme mit extremen Winden und Regen."),
        ("Warum bauen Menschen in Erdbebengebieten heute oft speziell gesicherte Gebäude?", new[] { "Damit die Gebäude Erdstöße besser aushalten und weniger einstürzen", "Speziell gesicherte Gebäude sind billiger als normale", "Erdbebensichere Bauweise hat keinerlei praktischen Nutzen - eine verbreitete, aber falsche Annahme" }, "Damit die Gebäude Erdstöße besser aushalten und weniger einstürzen",
            "Erdbebensichere Konstruktionen federn die Erschütterungen ab und verringern so das Risiko von Einstürzen und Opfern."),
        ("Was ist ein Deich und wofür wird er gebaut?", new[] { "Ein Schutzwall, der tief liegendes Land vor Überschwemmungen schützt", "Ein anderes Wort für Vulkan", "Eine Art Brücke über einen Fluss, was einer genaueren Pruefung nicht standhaelt" }, "Ein Schutzwall, der tief liegendes Land vor Überschwemmungen schützt",
            "Deiche halten Hochwasser von Flüssen oder Meeren zurück und schützen dahinterliegendes, tief gelegenes Land."),
        ("Warum sind flache Küstenregionen besonders von Überschwemmungen bedroht?", new[] { "Sie liegen oft nur wenig über dem Meeresspiegel", "Sie liegen besonders hoch über dem Meeresspiegel", "Flache Küsten sind grundsätzlich sicherer als steile" }, "Sie liegen oft nur wenig über dem Meeresspiegel",
            "Je geringer der Höhenunterschied zum Meeresspiegel, desto schneller kann ansteigendes Wasser das Land überfluten."),
        ("Was ist ein Frühwarnsystem im Zusammenhang mit Naturgefahren?", new[] { "Ein System, das Menschen frühzeitig vor Gefahren wie Tsunamis warnt", "Ein System, das Naturkatastrophen komplett verhindert, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für Wettervorhersage im Fernsehen" }, "Ein System, das Menschen frühzeitig vor Gefahren wie Tsunamis warnt",
            "Frühwarnsysteme erkennen Anzeichen einer Gefahr frühzeitig und geben Warnungen aus, damit Menschen rechtzeitig in Sicherheit gebracht werden können."),
        ("Warum ist Japan besonders häufig von Erdbeben betroffen?", new[] { "Es liegt an der Grenze mehrerer tektonischer Platten", "Japan liegt mitten in einer sehr stabilen Erdkrustenzone", "Erdbeben in Japan haben nichts mit der geografischen Lage zu tun" }, "Es liegt an der Grenze mehrerer tektonischer Platten",
            "Japan liegt am sogenannten Pazifischen Feuerring, wo mehrere tektonische Platten aufeinandertreffen - das begünstigt häufige Erdbeben."),
        ("Was können Menschen tun, um sich in einem Risikoraum besser auf Naturgefahren vorzubereiten?", new[] { "Notfallpläne erstellen, sich informieren und Frühwarnsysteme nutzen", "Am besten überhaupt nichts unternehmen", "Ausschließlich auf Zufall vertrauen" }, "Notfallpläne erstellen, sich informieren und Frühwarnsysteme nutzen",
            "Gute Vorbereitung - etwa Notfallpläne, Informationskampagnen und funktionierende Frühwarnsysteme - kann Menschenleben retten."),
        ("Was ist eine Lawine?", new[] { "Eine große Menge Schnee, die sich plötzlich einen Hang hinunterbewegt", "Ein anderes Wort für Erdbeben", "Eine besonders starke Meereswelle" }, "Eine große Menge Schnee, die sich plötzlich einen Hang hinunterbewegt",
            "Bei einer Lawine löst sich eine instabile Schneemasse und rutscht mit hoher Geschwindigkeit einen Hang hinab."),
        ("Warum sind Berggebiete im Winter oft von Lawinengefahr betroffen?", new[] { "Instabile Schneemassen können sich an steilen Hängen plötzlich lösen", "In Bergen schneit es grundsätzlich nie", "Lawinen treten ausschließlich im Sommer auf, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Instabile Schneemassen können sich an steilen Hängen plötzlich lösen",
            "Steile Hänge mit viel Schnee sind besonders lawinengefährdet, wenn sich Schneeschichten lösen."),
        ("Was ist eine Dürre?", new[] { "Eine längere Zeit ohne ausreichend Niederschlag, die zu Wassermangel führt", "Ein plötzlicher, sehr starker Regenschauer und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Ein anderes Wort für Überschwemmung" }, "Eine längere Zeit ohne ausreichend Niederschlag, die zu Wassermangel führt",
            "Bei einer Dürre bleibt der erwartete Niederschlag über längere Zeit aus, wodurch Böden und Wasserreserven austrocknen."),
        ("Warum kann eine Dürre für die Landwirtschaft eines Gebiets gefährlich werden?", new[] { "Pflanzen bekommen zu wenig Wasser und Ernten können ausfallen", "Dürren verbessern grundsätzlich die Ernteerträge", "Landwirtschaft ist von Niederschlag völlig unabhängig" }, "Pflanzen bekommen zu wenig Wasser und Ernten können ausfallen",
            "Ohne ausreichend Wasser können Nutzpflanzen nicht richtig wachsen, was zu Ernteausfällen und Nahrungsmittelknappheit führen kann."),
        ("Was passiert häufig, wenn in kurzer Zeit sehr viel Regen fällt (Starkregen)?", new[] { "Es kann zu plötzlichen Überschwemmungen (Sturzfluten) kommen", "Der Boden kann das Wasser immer problemlos sofort aufnehmen", "Starkregen hat grundsätzlich keine Auswirkungen" }, "Es kann zu plötzlichen Überschwemmungen (Sturzfluten) kommen",
            "Fällt in kurzer Zeit sehr viel Regen, kann der Boden das Wasser nicht schnell genug aufnehmen - es kommt zu Sturzfluten."),
        ("Warum ziehen manche Menschen trotz bekannter Naturgefahren nicht aus Risikogebieten weg?", new[] { "Oft aus wirtschaftlichen Gründen, familiären Bindungen oder weil dort ihre Lebensgrundlage liegt", "Weil ihnen die Gefahr grundsätzlich egal ist", "Weil ein Umzug gesetzlich verboten ist" }, "Oft aus wirtschaftlichen Gründen, familiären Bindungen oder weil dort ihre Lebensgrundlage liegt",
            "Arbeit, Familie, Besitz und Heimatverbundenheit halten viele Menschen in Risikogebieten, selbst wenn sie sich der Gefahr bewusst sind."),
        ("Was ist ein Vulkanausbruch?", new[] { "Der Ausbruch von glühender Lava, Gestein und Gasen aus dem Erdinneren", "Ein anderes Wort für Erdbeben", "Ein plötzlicher, sehr starker Sturm - eine haeufige, aber unzutreffende Vorstellung" }, "Der Ausbruch von glühender Lava, Gestein und Gasen aus dem Erdinneren",
            "Bei einem Vulkanausbruch treten geschmolzenes Gestein (Lava), Asche und Gase aus dem Erdinneren an die Oberfläche."),
        ("Wie helfen Katastrophenschutzorganisationen Menschen in Risikoräumen?", new[] { "Sie planen Evakuierungen, leisten Nothilfe und unterstützen beim Wiederaufbau", "Sie verhindern Naturkatastrophen vollständig", "Sie haben keinerlei praktische Aufgabe" }, "Sie planen Evakuierungen, leisten Nothilfe und unterstützen beim Wiederaufbau",
            "Katastrophenschutzorganisationen helfen vor, während und nach einer Katastrophe - von Evakuierungsplänen bis zum Wiederaufbau."),
        ("Was ist ein wichtiger Unterschied zwischen einer Naturgefahr und einer Naturkatastrophe?", new[] { "Eine Naturkatastrophe entsteht erst, wenn eine Naturgefahr Menschen und ihre Lebensgrundlagen tatsächlich schädigt", "Beide Begriffe bedeuten exakt dasselbe", "Eine Naturkatastrophe kann nur auf dem Meer stattfinden, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Eine Naturkatastrophe entsteht erst, wenn eine Naturgefahr Menschen und ihre Lebensgrundlagen tatsächlich schädigt",
            "Eine Naturgefahr wird erst zur Katastrophe, wenn sie tatsächlich Menschen, Siedlungen oder ihre Lebensgrundlagen trifft und schädigt.")
    };

    private static QuizQuestion RisikoraeumeNaturgefahren(Random r)
    {
        var f = RisikoraeumeListe[r.Next(RisikoraeumeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Leben in Risikoräumen (Naturgefahren)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Risikoräume liegen oft an Plattengrenzen (Erdbeben/Vulkane), an Küsten (Tsunami/Sturmflut) oder in Bergregionen (Lawinen) - Frühwarnsysteme und Vorsorge retten Leben."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MigrationBevoelkerungListe =
    {
        ("Was bedeutet \"Migration\" allgemein?", new[] { "Der dauerhafte oder längerfristige Wohnortwechsel von Menschen", "Ein anderes Wort für Urlaubsreise", "Der tägliche Weg zur Arbeit oder Schule" }, "Der dauerhafte oder längerfristige Wohnortwechsel von Menschen",
            "Migration bezeichnet den Umzug von Menschen von einem Ort zu einem anderen für längere Zeit oder dauerhaft."),
        ("Was ist der Unterschied zwischen Auswanderung (Emigration) und Einwanderung (Immigration)?", new[] { "Emigration ist das Verlassen des Heimatlandes, Immigration das Ankommen in einem neuen Land", "Beide Begriffe bedeuten exakt dasselbe", "Emigration betrifft nur kurze Urlaubsreisen" }, "Emigration ist das Verlassen des Heimatlandes, Immigration das Ankommen in einem neuen Land",
            "Aus Sicht des Herkunftslands spricht man von Emigration (Auswandern), aus Sicht des Ziellands von Immigration (Einwandern)."),
        ("Was versteht man unter \"Flucht\" im Unterschied zu freiwilliger Migration?", new[] { "Menschen verlassen ihr Zuhause unfreiwillig, z.B. wegen Krieg, Verfolgung oder Katastrophen", "Flucht bedeutet ausschließlich einen freiwilligen Umzug wegen einer neuen Arbeitsstelle (was so in der Praxis nicht zutrifft)", "Flucht und Migration sind komplett identisch" }, "Menschen verlassen ihr Zuhause unfreiwillig, z.B. wegen Krieg, Verfolgung oder Katastrophen",
            "Anders als bei freiwilliger Migration haben Geflüchtete meist keine echte Wahl, sondern müssen ihre Heimat aus Not verlassen."),
        ("Was ist ein häufiger Grund für Flucht aus einem Land?", new[] { "Krieg oder Verfolgung", "Ein schönes Urlaubsangebot", "Zu viel Freizeit im Heimatland" }, "Krieg oder Verfolgung",
            "Krieg, politische Verfolgung, Gewalt oder Naturkatastrophen zwingen Menschen oft zur Flucht."),
        ("Was bedeutet \"Landflucht\"?", new[] { "Der Wegzug vieler Menschen vom Land in die Städte", "Der Umzug von der Stadt aufs Land", "Ein anderes Wort für Flucht vor Naturkatastrophen" }, "Der Wegzug vieler Menschen vom Land in die Städte",
            "Bei der Landflucht verlassen viele Menschen ländliche Regionen und ziehen in Städte, oft auf der Suche nach besseren Chancen."),
        ("Warum ziehen viele Menschen aus ländlichen Regionen in Städte?", new[] { "Sie erhoffen sich dort bessere Arbeits- und Bildungschancen", "Auf dem Land gibt es grundsätzlich keine Arbeit mehr - eine verbreitete, aber falsche Annahme", "Städte bieten immer ein ruhigeres Leben" }, "Sie erhoffen sich dort bessere Arbeits- und Bildungschancen",
            "Städte bieten oft mehr Arbeitsplätze, Bildungseinrichtungen und Infrastruktur als ländliche Regionen."),
        ("Was ist eine mögliche Folge starker Landflucht für die betroffene ländliche Region?", new[] { "Dörfer können veralten und wirtschaftlich schwächer werden", "Die Region wird automatisch reicher", "Landflucht hat keinerlei Auswirkungen auf die Region" }, "Dörfer können veralten und wirtschaftlich schwächer werden",
            "Ziehen vor allem junge Menschen weg, altert die verbleibende Bevölkerung und die lokale Wirtschaft kann schwächer werden."),
        ("Was ist eine mögliche Folge von starker Zuwanderung in eine Stadt?", new[] { "Die Stadt wächst schnell und braucht mehr Wohnraum und Infrastruktur", "Die Stadt schrumpft automatisch", "Es entsteht kein zusätzlicher Bedarf an Wohnraum, was einer genaueren Pruefung nicht standhaelt" }, "Die Stadt wächst schnell und braucht mehr Wohnraum und Infrastruktur",
            "Starkes Bevölkerungswachstum durch Zuwanderung erfordert mehr Wohnungen, Schulen und Verkehrswege."),
        ("Wer gilt laut den Vereinten Nationen als \"Flüchtling\"?", new[] { "Eine Person, die ihr Land aus begründeter Furcht vor Verfolgung verlassen musste", "Jeder, der für einen Urlaub ins Ausland reist, obwohl das auf den ersten Blick plausibel klingt", "Nur Personen, die per Flugzeug reisen" }, "Eine Person, die ihr Land aus begründeter Furcht vor Verfolgung verlassen musste",
            "Die Genfer Flüchtlingskonvention definiert Flüchtlinge als Menschen mit begründeter Furcht vor Verfolgung, die deshalb ihr Land verlassen mussten."),
        ("Was ist ein Binnenflüchtling im Unterschied zu einem Flüchtling, der ins Ausland flieht?", new[] { "Jemand, der innerhalb des eigenen Landes vertrieben wurde, ohne die Landesgrenze zu überqueren", "Jemand, der ausschließlich innerhalb der eigenen Stadt umzieht, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein anderes Wort für Tourist" }, "Jemand, der innerhalb des eigenen Landes vertrieben wurde, ohne die Landesgrenze zu überqueren",
            "Binnenflüchtlinge (auch: Binnenvertriebene) bleiben innerhalb der Grenzen ihres Heimatlandes, anders als Flüchtlinge, die ins Ausland fliehen."),
        ("Welche Organisation der Vereinten Nationen kümmert sich weltweit besonders um Flüchtlinge?", new[] { "UNHCR (Flüchtlingshilfswerk der Vereinten Nationen)", "Die Weltgesundheitsorganisation (WHO) und deshalb hier nicht zutrifft", "Die Welthandelsorganisation (WTO)" }, "UNHCR (Flüchtlingshilfswerk der Vereinten Nationen)",
            "Das UNHCR setzt sich weltweit für den Schutz und die Unterstützung von Flüchtlingen ein."),
        ("Was ist ein wichtiger Unterschied zwischen einem Migranten und einem Flüchtling?", new[] { "Migranten ziehen meist freiwillig um, Flüchtlinge werden zur Flucht gezwungen", "Beide Begriffe bedeuten exakt dasselbe", "Migranten reisen ausschließlich per Schiff" }, "Migranten ziehen meist freiwillig um, Flüchtlinge werden zur Flucht gezwungen",
            "Der zentrale Unterschied liegt in der Freiwilligkeit: Migranten entscheiden sich meist selbst, Flüchtlinge fliehen aus Not."),
        ("Was können Gründe für freiwillige Migration sein?", new[] { "Zum Beispiel Arbeit, Ausbildung, Familie oder ein besseres Leben", "Ausschließlich Naturkatastrophen", "Ausschließlich Krieg" }, "Zum Beispiel Arbeit, Ausbildung, Familie oder ein besseres Leben",
            "Freiwillige Migration erfolgt oft aus persönlichen oder wirtschaftlichen Gründen wie einer neuen Arbeitsstelle oder Familienzusammenführung."),
        ("Was versteht man unter Integration von Zugewanderten?", new[] { "Das Hineinwachsen in die neue Gesellschaft, z.B. durch Sprache, Arbeit und soziale Kontakte", "Die sofortige Rückkehr ins Heimatland", "Das komplette Ablegen der eigenen Herkunftskultur, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Das Hineinwachsen in die neue Gesellschaft, z.B. durch Sprache, Arbeit und soziale Kontakte",
            "Integration beschreibt den Prozess, in dem Zugewanderte Teil der neuen Gesellschaft werden, z.B. über Sprache, Arbeit und soziale Beziehungen."),
        ("Warum ist Deutschland historisch und aktuell ein Einwanderungsland?", new[] { "Über Jahrzehnte sind viele Menschen aus verschiedenen Gründen nach Deutschland gezogen und geblieben", "Nach Deutschland ist noch nie jemand eingewandert", "Einwanderung nach Deutschland ist erst seit letztem Jahr erlaubt" }, "Über Jahrzehnte sind viele Menschen aus verschiedenen Gründen nach Deutschland gezogen und geblieben",
            "Von den sogenannten Gastarbeitern bis zu heutigen Fachkräften und Geflüchteten ist Deutschland seit Jahrzehnten Ziel von Einwanderung."),
        ("Was ist eine Fluchtroute?", new[] { "Der Weg, den Menschen auf der Flucht aus ihrer Heimat in ein sichereres Land nehmen", "Ein touristischer Wanderweg", "Ein anderes Wort für Autobahn, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Der Weg, den Menschen auf der Flucht aus ihrer Heimat in ein sichereres Land nehmen",
            "Fluchtrouten sind die oft langen und beschwerlichen Wege, die Geflüchtete auf der Suche nach Sicherheit zurücklegen."),
        ("Warum sind Fluchtrouten oft besonders gefährlich?", new[] { "Sie führen häufig durch unsicheres Gelände, über das Meer oder durch von Schleusern kontrollierte Gebiete", "Fluchtrouten sind grundsätzlich völlig gefahrlos", "Sie verlaufen immer über gut ausgebaute Autobahnen" }, "Sie führen häufig durch unsicheres Gelände, über das Meer oder durch von Schleusern kontrollierte Gebiete",
            "Viele Fluchtrouten sind lebensgefährlich, etwa bei Überfahrten in überfüllten Booten oder durch von Schleppern kontrollierte Gebiete."),
        ("Was ist ein Asylverfahren?", new[] { "Ein rechtliches Verfahren, in dem geprüft wird, ob eine Person als Flüchtling anerkannt wird", "Ein anderes Wort für Einbürgerungstest (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Eine Art Reisebuchung" }, "Ein rechtliches Verfahren, in dem geprüft wird, ob eine Person als Flüchtling anerkannt wird",
            "Im Asylverfahren prüfen Behörden, ob eine Person tatsächlich Verfolgung oder Gefahr in ihrer Heimat droht und ihr deshalb Schutz gewährt wird."),
        ("Was passiert demografisch mit einer Region, aus der viele junge Menschen abwandern?", new[] { "Der Altersdurchschnitt der verbleibenden Bevölkerung steigt", "Der Altersdurchschnitt sinkt automatisch", "Es gibt keinerlei demografische Auswirkungen, was einer genaueren Pruefung nicht standhaelt" }, "Der Altersdurchschnitt der verbleibenden Bevölkerung steigt",
            "Wandern vor allem jüngere Menschen ab, bleibt tendenziell eine ältere Bevölkerung zurück, wodurch der Altersdurchschnitt steigt."),
        ("Was sind \"Pull-Faktoren\" bei Migration?", new[] { "Dinge, die Menschen an einem Zielort anziehen, z.B. Arbeit oder Sicherheit", "Dinge, die Menschen von ihrem Herkunftsort abstoßen", "Ein anderes Wort für Fluchtroute" }, "Dinge, die Menschen an einem Zielort anziehen, z.B. Arbeit oder Sicherheit",
            "Pull-Faktoren sind anziehende Gründe eines Ziellandes, etwa bessere Arbeitschancen, Sicherheit oder Bildung, die Menschen zum Umzug bewegen.")
    };

    private static QuizQuestion MigrationUndBevoelkerung(Random r)
    {
        var f = MigrationBevoelkerungListe[r.Next(MigrationBevoelkerungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Migration und Bevölkerung (Flucht, Landflucht)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Migration ist meist freiwillig (Arbeit/Familie), Flucht unfreiwillig (Krieg/Verfolgung); Landflucht beschreibt den Wegzug vom Land in die Stadt, mit Folgen für beide Seiten."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RegenwaldListe =
    {
        ("Wo liegen die meisten tropischen Regenwälder der Erde?", new[] { "In der Nähe des Äquators", "In der Nähe der Pole", "Ausschließlich in Wüstenregionen" }, "In der Nähe des Äquators",
            "Das warme, feuchte Klima nahe dem Äquator bietet ideale Bedingungen für tropische Regenwälder."),
        ("Was ist typisch für das Klima im tropischen Regenwald?", new[] { "Warm und sehr feucht, mit viel Niederschlag das ganze Jahr über", "Kalt und sehr trocken", "Nur im Sommer warm, im Winter eisig, obwohl das auf den ersten Blick plausibel klingt" }, "Warm und sehr feucht, mit viel Niederschlag das ganze Jahr über",
            "Im tropischen Regenwald herrschen ganzjährig hohe Temperaturen und regelmäßig starker Niederschlag."),
        ("Welcher ist der größte zusammenhängende Regenwald der Erde?", new[] { "Der Amazonas-Regenwald", "Der Schwarzwald", "Der Regenwald der Sahara" }, "Der Amazonas-Regenwald",
            "Der Amazonas-Regenwald in Südamerika ist der größte tropische Regenwald der Welt."),
        ("Warum wird der Regenwald manchmal als \"grüne Lunge der Erde\" bezeichnet?", new[] { "Die Pflanzen produzieren durch Fotosynthese große Mengen Sauerstoff", "Regenwälder haben die Form einer Lunge", "Regenwälder verbrauchen ausschließlich Sauerstoff" }, "Die Pflanzen produzieren durch Fotosynthese große Mengen Sauerstoff",
            "Die riesige Pflanzenmasse der Regenwälder produziert durch Fotosynthese enorme Mengen Sauerstoff und bindet viel CO₂."),
        ("Was macht den tropischen Regenwald so artenreich?", new[] { "Warmes, feuchtes Klima bietet ideale Lebensbedingungen für sehr viele Arten", "Das raue Klima lässt nur wenige Arten überleben, was die eigentliche Bedeutung des Begriffs verfehlt", "Regenwälder sind eigentlich recht artenarm" }, "Warmes, feuchtes Klima bietet ideale Lebensbedingungen für sehr viele Arten",
            "Die konstante Wärme und Feuchtigkeit ermöglicht eine außergewöhnlich hohe Vielfalt an Pflanzen- und Tierarten."),
        ("Warum wachsen im Regenwald die Bäume oft besonders hoch?", new[] { "Sie konkurrieren um Sonnenlicht und wachsen deshalb schnell in die Höhe", "Bäume im Regenwald wachsen grundsätzlich nur sehr langsam", "Hohe Bäume brauchen im Regenwald kein Sonnenlicht" }, "Sie konkurrieren um Sonnenlicht und wachsen deshalb schnell in die Höhe",
            "Im dichten Regenwald konkurrieren die Pflanzen stark um Licht, was viele Bäume dazu treibt, besonders hoch zu wachsen."),
        ("Was ist ein Grund für die Abholzung von Regenwaldflächen?", new[] { "Um Platz für Landwirtschaft, Viehzucht oder Holzwirtschaft zu schaffen", "Regenwälder werden nie abgeholzt", "Nur um Wanderwege für Touristen anzulegen" }, "Um Platz für Landwirtschaft, Viehzucht oder Holzwirtschaft zu schaffen",
            "Große Regenwaldflächen werden gerodet, um Platz für Weideflächen, Plantagen oder die Holzgewinnung zu schaffen."),
        ("Warum ist die Abholzung des Regenwalds ein globales, nicht nur ein lokales Problem?", new[] { "Regenwälder binden viel CO₂ und beeinflussen das weltweite Klima", "Abholzung betrifft ausschließlich das jeweilige Land und deshalb hier nicht zutrifft", "Regenwälder haben keinerlei Einfluss auf das Klima" }, "Regenwälder binden viel CO₂ und beeinflussen das weltweite Klima",
            "Da Regenwälder große Mengen CO₂ speichern und den globalen Wasserkreislauf beeinflussen, wirkt sich ihre Abholzung weltweit aus."),
        ("Was passiert mit dem im Regenwald gespeicherten Kohlenstoff, wenn Bäume abgeholzt und verbrannt werden?", new[] { "Er wird als CO₂ in die Atmosphäre freigesetzt", "Er verschwindet spurlos", "Er wandelt sich automatisch in Sauerstoff um, was so nicht korrekt ist" }, "Er wird als CO₂ in die Atmosphäre freigesetzt",
            "Beim Verbrennen oder Verrotten der Bäume wird der zuvor gebundene Kohlenstoff als CO₂ wieder freigesetzt und verstärkt den Treibhauseffekt."),
        ("Warum ist der Boden im Regenwald trotz üppiger Pflanzenwelt oft nährstoffarm?", new[] { "Nährstoffe werden vom dichten Pflanzenbewuchs sehr schnell wieder aufgenommen", "Regenwaldböden enthalten grundsätzlich extrem viele Nährstoffe - eine haeufige, aber unzutreffende Vorstellung", "Es regnet im Regenwald zu selten für nährstoffreiche Böden" }, "Nährstoffe werden vom dichten Pflanzenbewuchs sehr schnell wieder aufgenommen",
            "Anders als man vermuten könnte, sind Regenwaldböden oft nährstoffarm, weil Nährstoffe sofort vom dichten Pflanzenwuchs aufgenommen werden."),
        ("Was passiert häufig mit gerodeten Regenwaldflächen nach einigen Jahren landwirtschaftlicher Nutzung?", new[] { "Der Boden verliert schnell an Fruchtbarkeit und wird unbrauchbar", "Der Boden wird mit der Zeit immer fruchtbarer", "Gerodete Flächen bleiben für immer unverändert fruchtbar, auch wenn das manche zunaechst vermuten wuerden" }, "Der Boden verliert schnell an Fruchtbarkeit und wird unbrauchbar",
            "Ohne den schützenden Pflanzenbewuchs verlieren die nährstoffarmen Böden schnell an Fruchtbarkeit, sodass Flächen oft nach kurzer Zeit aufgegeben werden."),
        ("Was versteht man unter \"Biodiversität\"?", new[] { "Die Vielfalt an unterschiedlichen Lebewesen und Lebensräumen", "Ein anderes Wort für Regenmenge", "Die Anzahl der Flüsse in einer Region" }, "Die Vielfalt an unterschiedlichen Lebewesen und Lebensräumen",
            "Biodiversität beschreibt die Vielfalt des Lebens - von einzelnen Arten bis zu ganzen Ökosystemen."),
        ("Warum gilt der Regenwald als besonders wichtiger Ort für die Biodiversität der Erde?", new[] { "Ein großer Teil aller bekannten Tier- und Pflanzenarten lebt dort", "Im Regenwald leben fast keine unterschiedlichen Arten", "Biodiversität hat mit Regenwäldern nichts zu tun" }, "Ein großer Teil aller bekannten Tier- und Pflanzenarten lebt dort",
            "Obwohl Regenwälder nur einen kleinen Teil der Erdoberfläche bedecken, beherbergen sie einen enorm hohen Anteil aller bekannten Arten."),
        ("Was können Länder und Organisationen tun, um Regenwälder zu schützen?", new[] { "Schutzgebiete einrichten, nachhaltige Nutzung fördern und Abholzung eindämmen", "Regenwälder lassen sich grundsätzlich nicht schützen, was bei genauerem Hinsehen nicht stimmt", "Ausschließlich den kompletten Tourismus verbieten" }, "Schutzgebiete einrichten, nachhaltige Nutzung fördern und Abholzung eindämmen",
            "Naturschutzgebiete, nachhaltige Forstwirtschaft und internationale Abkommen helfen, Regenwälder langfristig zu bewahren."),
        ("Warum betrifft die Regenwald-Abholzung auch den Wasserkreislauf einer Region?", new[] { "Bäume verdunsten viel Wasser, das für Regenbildung wichtig ist", "Bäume haben keinerlei Einfluss auf den Wasserkreislauf (was so in der Praxis nicht zutrifft)", "Abholzung erhöht automatisch den Niederschlag" }, "Bäume verdunsten viel Wasser, das für Regenbildung wichtig ist",
            "Regenwaldbäume verdunsten enorme Wassermengen, die zur Wolken- und Regenbildung beitragen - weniger Bäume bedeuten oft weniger Niederschlag."),
        ("Was ist ein tropisches Vielschichtsystem (Stockwerkbau) im Regenwald?", new[] { "Verschiedene Pflanzen- und Tierschichten vom Boden bis zum Kronendach", "Ein System aus künstlich angelegten Stufen für Touristen - eine verbreitete, aber falsche Annahme", "Ein anderes Wort für Wasserfall" }, "Verschiedene Pflanzen- und Tierschichten vom Boden bis zum Kronendach",
            "Der Regenwald gliedert sich in mehrere übereinanderliegende Schichten (Boden, Strauch-, Kron- und Überschicht) mit jeweils eigenen Lebensbedingungen."),
        ("Warum leben die meisten Tiere des Regenwalds eher in den oberen Baumkronen als am Boden?", new[] { "Dort gibt es mehr Licht, Früchte und Nahrung", "Am Boden herrscht ständig zu viel Licht", "In den Baumkronen gibt es überhaupt keine Nahrung" }, "Dort gibt es mehr Licht, Früchte und Nahrung",
            "Das Kronendach erhält mehr Sonnenlicht und bietet reichlich Früchte und Blätter - deshalb konzentriert sich dort ein Großteil des Tierlebens."),
        ("Was bedeutet \"nachhaltige Nutzung\" eines Regenwalds?", new[] { "Den Wald so zu nutzen, dass er sich erholen kann und langfristig erhalten bleibt", "Den Wald so schnell wie möglich komplett abzuholzen, was einer genaueren Pruefung nicht standhaelt", "Den Wald überhaupt nicht zu betreten" }, "Den Wald so zu nutzen, dass er sich erholen kann und langfristig erhalten bleibt",
            "Nachhaltige Nutzung bedeutet, Ressourcen so zu entnehmen, dass sich der Wald erholen kann und auch künftigen Generationen erhalten bleibt."),
        ("Was ist ein Epiphyt, wie er häufig im Regenwald vorkommt?", new[] { "Eine Pflanze, die auf einer anderen Pflanze wächst, ohne ihr zu schaden", "Ein Fleisch fressendes Raubtier des Regenwalds, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für Regenwaldboden" }, "Eine Pflanze, die auf einer anderen Pflanze wächst, ohne ihr zu schaden",
            "Epiphyten wie viele Orchideen wachsen auf Ästen anderer Bäume, um näher ans Licht zu kommen, ohne ihrem Wirt zu schaden."),
        ("Warum ist traditionelles Wissen indigener Völker über Regenwaldpflanzen für die Forschung wertvoll?", new[] { "Es kann helfen, neue Heilpflanzen und Wirkstoffe für Medikamente zu entdecken", "Indigenes Wissen hat mit moderner Forschung nichts zu tun, was die eigentliche Bedeutung des Begriffs verfehlt", "Es dient ausschließlich touristischen Zwecken" }, "Es kann helfen, neue Heilpflanzen und Wirkstoffe für Medikamente zu entdecken",
            "Indigene Völker kennen oft seit Generationen die heilende Wirkung bestimmter Pflanzen, was der modernen Medizinforschung wertvolle Hinweise liefern kann.")
    };

    private static QuizQuestion TropischerRegenwald(Random r)
    {
        var f = RegenwaldListe[r.Next(RegenwaldListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Vielfalt der Erde (tropischer Regenwald)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Regenwälder sind warm, feucht, extrem artenreich und binden viel CO₂ - trotz üppiger Pflanzenwelt sind ihre Böden oft nährstoffarm, was Abholzung besonders folgenreich macht."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArmutReichtumK6Listen =
    {
        ("Was bedeutet \"Armut\" ganz allgemein?", new[] { "Ein Mangel an Geld, Nahrung, Bildung oder anderen Grundbedürfnissen", "Ein Überfluss an Geld und Besitz", "Ein anderes Wort für Reichtum" }, "Ein Mangel an Geld, Nahrung, Bildung oder anderen Grundbedürfnissen",
            "Armut bedeutet, dass grundlegende Bedürfnisse wie Nahrung, Wohnung oder Bildung nicht ausreichend gedeckt werden können."),
        ("Was bedeutet \"Reichtum\" im Gegensatz dazu?", new[] { "Ein Überfluss an Geld oder Besitz, mit dem viele Bedürfnisse leicht erfüllt werden können", "Ein Mangel an grundlegenden Dingen", "Ein anderes Wort für Armut" }, "Ein Überfluss an Geld oder Besitz, mit dem viele Bedürfnisse leicht erfüllt werden können",
            "Reichtum bedeutet, deutlich mehr Geld oder Besitz zu haben, als für die Grundbedürfnisse nötig wäre."),
        ("Was ist ein Beispiel für ein Grundbedürfnis, das armen Menschen oft fehlt?", new[] { "Sauberes Trinkwasser, ausreichend Nahrung oder eine feste Unterkunft", "Ein zweites Auto", "Ein Urlaub im Ausland und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Sauberes Trinkwasser, ausreichend Nahrung oder eine feste Unterkunft",
            "Grundbedürfnisse wie sauberes Wasser, genug Nahrung und eine feste Unterkunft sind für arme Menschen oft nicht selbstverständlich."),
        ("Warum ist Bildung wichtig, um Armut langfristig zu verringern?", new[] { "Bildung eröffnet bessere Chancen auf gut bezahlte Arbeit", "Bildung hat keinerlei Einfluss auf spätere Chancen - eine haeufige, aber unzutreffende Vorstellung", "Bildung macht Menschen automatisch reich" }, "Bildung eröffnet bessere Chancen auf gut bezahlte Arbeit",
            "Gute Bildung verbessert die Chancen auf qualifizierte, besser bezahlte Arbeit und kann so helfen, Armut zu durchbrechen."),
        ("Was ist ein Unterschied zwischen absoluter und relativer Armut, vereinfacht?", new[] { "Absolute Armut bedeutet, Grundbedürfnisse nicht decken zu können; relative Armut bedeutet, deutlich weniger zu haben als der Durchschnitt", "Beide Begriffe bedeuten exakt dasselbe", "Relative Armut betrifft nur reiche Länder, absolute nur arme Länder, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt" }, "Absolute Armut bedeutet, Grundbedürfnisse nicht decken zu können; relative Armut bedeutet, deutlich weniger zu haben als der Durchschnitt",
            "Absolute Armut bezieht sich auf das nackte Überleben, relative Armut vergleicht den eigenen Lebensstandard mit dem der übrigen Gesellschaft."),
        ("Warum gibt es auch in reichen Ländern wie Deutschland arme Menschen?", new[] { "Nicht alle Menschen haben gleich guten Zugang zu Arbeit, Bildung oder Unterstützung", "In reichen Ländern gibt es per Definition keine armen Menschen (was so in der Praxis nicht zutrifft)", "Armut existiert ausschließlich in Entwicklungsländern" }, "Nicht alle Menschen haben gleich guten Zugang zu Arbeit, Bildung oder Unterstützung",
            "Selbst in wohlhabenden Ländern gibt es Menschen, die aus verschiedenen Gründen keinen ausreichenden Zugang zu Einkommen oder Unterstützung haben."),
        ("Was können Staaten tun, um Armut in ihrem Land zu verringern?", new[] { "Zum Beispiel Sozialleistungen, kostenlose Bildung und Gesundheitsversorgung bereitstellen", "Armut lässt sich durch staatliches Handeln grundsätzlich nicht beeinflussen - eine verbreitete, aber falsche Annahme", "Ausschließlich die Steuern für alle senken" }, "Zum Beispiel Sozialleistungen, kostenlose Bildung und Gesundheitsversorgung bereitstellen",
            "Sozialstaatliche Maßnahmen wie Unterstützungsleistungen, kostenlose Bildung und Gesundheitsversorgung können Armut verringern."),
        ("Was ist ein Beispiel für eine internationale Organisation, die gegen weltweite Armut arbeitet?", new[] { "Die Vereinten Nationen (UN) bzw. Organisationen wie UNICEF", "Die Internationale Fußball-Föderation (FIFA)", "Eine private Fluggesellschaft" }, "Die Vereinten Nationen (UN) bzw. Organisationen wie UNICEF",
            "Organisationen wie die UN oder UNICEF setzen sich weltweit für die Bekämpfung von Armut, besonders bei Kindern, ein."),
        ("Warum sind Kinder in armen Familien oft besonders benachteiligt?", new[] { "Ihnen fehlt oft der Zugang zu guter Bildung, Gesundheitsversorgung und ausreichender Ernährung", "Arme Kinder haben grundsätzlich bessere Chancen als andere Kinder, was einer genaueren Pruefung nicht standhaelt", "Armut hat auf Kinder keinerlei Auswirkung" }, "Ihnen fehlt oft der Zugang zu guter Bildung, Gesundheitsversorgung und ausreichender Ernährung",
            "Fehlende Ressourcen erschweren Kindern aus armen Familien oft den Zugang zu Bildung, Gesundheit und ausreichender Ernährung."),
        ("Was versteht man unter \"Entwicklungsländern\" im Vergleich zu \"Industrieländern\", vereinfacht?", new[] { "Entwicklungsländer haben oft eine schwächere Wirtschaft und weniger ausgebaute Infrastruktur", "Entwicklungsländer sind grundsätzlich reicher als Industrieländer, obwohl das auf den ersten Blick plausibel klingt", "Beide Begriffe bedeuten exakt dasselbe" }, "Entwicklungsländer haben oft eine schwächere Wirtschaft und weniger ausgebaute Infrastruktur",
            "Entwicklungsländer haben im Vergleich zu Industrieländern oft eine schwächere Wirtschaftskraft und geringer ausgebaute Infrastruktur."),
        ("Was ist \"Spendengeld\" und wie kann es gegen Armut helfen?", new[] { "Freiwillig gegebenes Geld, das z.B. für Nahrung, Bildung oder medizinische Hilfe verwendet wird", "Ein anderes Wort für Steuern", "Geld, das nur für Werbung genutzt wird" }, "Freiwillig gegebenes Geld, das z.B. für Nahrung, Bildung oder medizinische Hilfe verwendet wird",
            "Spenden werden häufig für konkrete Hilfsprojekte eingesetzt, z.B. Nahrungsmittelhilfe, Schulbau oder medizinische Versorgung."),
        ("Was ist \"Entwicklungshilfe\"?", new[] { "Unterstützung reicherer Länder oder Organisationen für ärmere Länder", "Ein anderes Wort für Kriegsführung", "Hilfe, die nur innerhalb eines einzelnen Landes stattfindet, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Unterstützung reicherer Länder oder Organisationen für ärmere Länder",
            "Entwicklungshilfe umfasst finanzielle, materielle oder fachliche Unterstützung wohlhabenderer Länder für ärmere Länder."),
        ("Warum ist der Zugang zu sauberem Trinkwasser weltweit ungleich verteilt?", new[] { "Manche Regionen haben zu wenig Wasser oder keine ausreichende Infrastruktur zur Wasseraufbereitung", "Sauberes Trinkwasser ist überall auf der Welt gleich gut verfügbar und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Wasserknappheit betrifft ausschließlich reiche Länder" }, "Manche Regionen haben zu wenig Wasser oder keine ausreichende Infrastruktur zur Wasseraufbereitung",
            "Klimatische Bedingungen und fehlende Infrastruktur führen dazu, dass sauberes Trinkwasser weltweit sehr ungleich verteilt ist."),
        ("Was ist ein Beispiel für eine Ursache von Armut in einem Land?", new[] { "Zum Beispiel Kriege, Naturkatastrophen, schlechte Regierungsführung oder fehlende Bildung", "Zu viel Regen im ganzen Land - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden", "Zu viele Feiertage im Jahr" }, "Zum Beispiel Kriege, Naturkatastrophen, schlechte Regierungsführung oder fehlende Bildung",
            "Armut hat meist mehrere Ursachen gleichzeitig, darunter Konflikte, Katastrophen, schlechte politische Führung und mangelnde Bildung."),
        ("Was kann jede einzelne Person tun, um gegen Armut zu helfen?", new[] { "Zum Beispiel spenden, sich engagieren oder fair gehandelte Produkte kaufen", "Es gibt für einzelne Personen überhaupt keine Möglichkeit zu helfen", "Nur Regierungen können gegen Armut etwas unternehmen" }, "Zum Beispiel spenden, sich engagieren oder fair gehandelte Produkte kaufen",
            "Auch im Kleinen kann jeder Einzelne durch Spenden, ehrenamtliches Engagement oder bewussten Konsum etwas gegen Armut beitragen."),
        ("Was bedeutet \"Fairer Handel\" (Fairtrade) im Zusammenhang mit Armut?", new[] { "Produzenten in ärmeren Ländern erhalten einen faireren, höheren Preis für ihre Waren", "Fairer Handel bedeutet, dass alle Waren kostenlos abgegeben werden", "Fairer Handel hat mit Armutsbekämpfung nichts zu tun" }, "Produzenten in ärmeren Ländern erhalten einen faireren, höheren Preis für ihre Waren",
            "Fairtrade-Produkte garantieren Erzeugern in ärmeren Ländern einen faireren Preis, was ihre Lebensbedingungen verbessern kann."),
        ("Warum kann eine gute Gesundheitsversorgung helfen, Armut zu verringern?", new[] { "Gesunde Menschen können besser arbeiten und für sich selbst sorgen", "Gesundheitsversorgung hat keinerlei Einfluss auf Armut, was bei genauerem Hinsehen nicht stimmt", "Kranke Menschen verdienen automatisch mehr Geld" }, "Gesunde Menschen können besser arbeiten und für sich selbst sorgen",
            "Krankheit kann Menschen an der Arbeit hindern - gute Gesundheitsversorgung hilft, arbeitsfähig zu bleiben und für sich selbst zu sorgen."),
        ("Was versteht man unter dem \"Wohlstandsgefälle\" zwischen verschiedenen Weltregionen?", new[] { "Deutliche Unterschiede im Lebensstandard zwischen reicheren und ärmeren Regionen der Welt", "Alle Weltregionen haben exakt denselben Lebensstandard (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Ein anderes Wort für Klimawandel" }, "Deutliche Unterschiede im Lebensstandard zwischen reicheren und ärmeren Regionen der Welt",
            "Das Wohlstandsgefälle beschreibt die teils großen Unterschiede im Lebensstandard zwischen verschiedenen Weltregionen."),
        ("Was ist ein sinnvolles Ziel im Kampf gegen weltweite Armut, das die Vereinten Nationen verfolgen?", new[] { "Die Verringerung extremer Armut weltweit (z.B. im Rahmen der Nachhaltigkeitsziele/SDGs)", "Die Abschaffung sämtlicher Staatsgrenzen", "Die weltweite Einführung einer einzigen Sprache" }, "Die Verringerung extremer Armut weltweit (z.B. im Rahmen der Nachhaltigkeitsziele/SDGs)",
            "Eines der UN-Nachhaltigkeitsziele (SDGs) ist es, extreme Armut weltweit bis zu einem bestimmten Zieljahr deutlich zu verringern."),
        ("Warum kommt Kinderarbeit in ärmeren Ländern häufiger vor als in reichen Ländern?", new[] { "Arme Familien sind oft auf das zusätzliche Einkommen der Kinder angewiesen", "Kinder arbeiten grundsätzlich lieber als zur Schule zu gehen", "Kinderarbeit kommt ausschließlich in reichen Ländern vor" }, "Arme Familien sind oft auf das zusätzliche Einkommen der Kinder angewiesen",
            "Wenn das Familieneinkommen nicht für die Grundbedürfnisse reicht, müssen in manchen armen Regionen auch Kinder mitarbeiten, statt zur Schule zu gehen.")
    };

    private static QuizQuestion ArmutUndReichtumKlasse6(Random r)
    {
        var f = ArmutReichtumK6Listen[r.Next(ArmutReichtumK6Listen.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Geo, GradeLevel = GradeLevel.Klasse6,
            Topic = "Armut und Reichtum (Klasse-6-Niveau)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Armut bedeutet fehlenden Zugang zu Grundbedürfnissen wie Wasser, Nahrung und Bildung - Bildung, Gesundheitsversorgung und fairer Handel gehören zu den wichtigsten Hebeln dagegen."
        };
    }
}
