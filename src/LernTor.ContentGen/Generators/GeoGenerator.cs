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
            [GradeLevel.Klasse6] = new List<TopicFactory> { Kontinente, Klimazonen, Bundeslaender },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Plattentektonik, Klimawandel, Verstaedterung, ArmutReichtum }
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
        ("Welcher Kontinent ist der flächenmäßig kleinste?", new[] { "Australien/Ozeanien", "Europa", "Afrika" }, "Australien/Ozeanien",
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
        ("In welcher Klimazone liegt Deutschland?", new[] { "Gemäßigte Zone", "Tropen", "Polare Zone" }, "Gemäßigte Zone",
            "Deutschland liegt in der gemäßigten Klimazone mit vier Jahreszeiten und mildem Klima."),
        ("Wie nennt man die Klimazone rund um den Äquator mit hohen Temperaturen und viel Regen?", new[] { "Tropen", "Polare Zone", "Gemäßigte Zone" }, "Tropen",
            "In den Tropen rund um den Äquator ist es ganzjährig warm bis heiß, oft mit viel Niederschlag (z.B. Regenwald)."),
        ("Was ist typisch für das Klima in der polaren Zone (z.B. Arktis, Antarktis)?", new[] { "Sehr kalt und eisig, kaum Vegetation", "Ganzjährig warm und trocken", "Viel Regen und dichte Wälder" }, "Sehr kalt und eisig, kaum Vegetation",
            "In der polaren Zone herrschen sehr niedrige Temperaturen und viel Eis, wodurch dort kaum Pflanzen wachsen können."),
        ("Was kennzeichnet die gemäßigte Klimazone?", new[] { "Vier Jahreszeiten mit milden Temperaturunterschieden", "Ganzjährig gleichbleibend heiß", "Ganzjährig Schnee und Eis" }, "Vier Jahreszeiten mit milden Temperaturunterschieden",
            "In der gemäßigten Zone (z.B. Deutschland) wechseln sich Frühling, Sommer, Herbst und Winter mit moderaten Temperaturen ab."),
        ("Was ist typisch für das Klima der Savanne?", new[] { "Wechsel zwischen Regenzeit und Trockenzeit", "Ganzjährig gleichmäßiger Regen", "Ganzjährig Schnee" }, "Wechsel zwischen Regenzeit und Trockenzeit",
            "In der Savanne, meist am Rand der Tropen, wechseln sich eine Regenzeit und eine lange Trockenzeit ab."),
        ("Welches Klima herrscht in der Wüste Sahara?", new[] { "Trockenklima mit sehr wenig Niederschlag", "Feuchtklima mit viel Regen", "Polares Klima mit Eis" }, "Trockenklima mit sehr wenig Niederschlag",
            "Die Sahara ist geprägt von extremer Trockenheit mit nur sehr seltenem, geringem Niederschlag."),
        ("Was versteht man unter dem Äquator?", new[] { "Die gedachte Linie, die die Erde in eine Nord- und eine Südhalbkugel teilt", "Eine Gebirgskette in Südamerika", "Einen Ozean zwischen Afrika und Asien" }, "Die gedachte Linie, die die Erde in eine Nord- und eine Südhalbkugel teilt",
            "Der Äquator ist die gedachte Linie auf halbem Weg zwischen Nord- und Südpol, die die Erde in zwei Hälften teilt."),
        ("Warum ist es am Äquator ganzjährig warm?", new[] { "Weil die Sonne dort das ganze Jahr über fast senkrecht steht", "Weil dort kein Wind weht", "Weil es dort nie regnet" }, "Weil die Sonne dort das ganze Jahr über fast senkrecht steht",
            "Am Äquator treffen die Sonnenstrahlen ganzjährig nahezu senkrecht auf, wodurch es dort konstant warm bis heiß ist."),
        ("Was beschreibt der Begriff \"Monsun\"?", new[] { "Jahreszeitlich wechselnde Winde, die z.B. in Südasien starke Regenzeiten bringen", "Ein Wüstenwind ohne Regen", "Ein Wirbelsturm, der nur im Winter auftritt" }, "Jahreszeitlich wechselnde Winde, die z.B. in Südasien starke Regenzeiten bringen",
            "Monsune sind jahreszeitlich wechselnde Winde, die z.B. in Süd- und Ostasien ausgeprägte Regen- und Trockenzeiten verursachen."),
        ("In welcher Klimazone liegt der Amazonas-Regenwald?", new[] { "Tropen (tropisches Regenklima)", "Gemäßigte Zone", "Polare Zone" }, "Tropen (tropisches Regenklima)",
            "Der Amazonas-Regenwald liegt in den Tropen, geprägt von ganzjährig hohen Temperaturen und viel Niederschlag."),
        ("Was ist typisch für das mediterrane Klima (z.B. Südeuropa)?", new[] { "Trockene, heiße Sommer und milde, regenreiche Winter", "Ganzjährig Schnee und Frost", "Ganzjährig Regen ohne echten Sommer" }, "Trockene, heiße Sommer und milde, regenreiche Winter",
            "Das mediterrane Klima rund ums Mittelmeer zeichnet sich durch trockene, heiße Sommer und milde, feuchtere Winter aus."),
        ("Wie nennt man die Nadelwaldzone der kühl-gemäßigten bis subpolaren Breiten (z.B. in Russland, Kanada)?", new[] { "Taiga (borealer Nadelwald)", "Savanne", "Steppe" }, "Taiga (borealer Nadelwald)",
            "Die Taiga ist der riesige Nadelwaldgürtel der kühlen, nördlichen Breiten in Nordamerika und Eurasien."),
        ("Was ist die Tundra?", new[] { "Eine baumlose, meist kalte Landschaft nahe der Polarregion mit Dauerfrostboden", "Ein tropischer Regenwald", "Eine heiße Wüste" }, "Eine baumlose, meist kalte Landschaft nahe der Polarregion mit Dauerfrostboden",
            "In der Tundra ist der Boden dauerhaft oder fast dauerhaft gefroren (Permafrost), weshalb dort kaum Bäume wachsen."),
        ("Was passiert mit der Temperatur, je höher man in den Bergen steigt (Höhenklima)?", new[] { "Sie sinkt in der Regel", "Sie steigt immer", "Sie bleibt gleich" }, "Sie sinkt in der Regel",
            "Mit zunehmender Höhe nimmt die Lufttemperatur normalerweise ab - deshalb liegt auf hohen Bergen auch in warmen Regionen Schnee."),
        ("Was kennzeichnet ein Kontinentalklima (z.B. Innerasien) im Gegensatz zu einem Seeklima?", new[] { "Große Temperaturunterschiede zwischen Sommer und Winter", "Ganzjährig milde Temperaturen", "Ganzjährig hohe Luftfeuchtigkeit" }, "Große Temperaturunterschiede zwischen Sommer und Winter",
            "Fernab vom Meer schwanken die Temperaturen zwischen Sommer und Winter stark, da Land sich schneller erwärmt und abkühlt als Wasser."),
        ("Was versteht man unter der Steppe?", new[] { "Eine meist baumlose Graslandschaft in eher trockenen, gemäßigten bis subtropischen Gebieten", "Einen tropischen Regenwald", "Ein Gletschergebiet" }, "Eine meist baumlose Graslandschaft in eher trockenen, gemäßigten bis subtropischen Gebieten",
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
        ("Was verursacht die Bewegung der Kontinentalplatten (Plattentektonik)?", new[] { "Strömungen im heißen Erdmantel", "Der Mond", "Der Wind" }, "Strömungen im heißen Erdmantel",
            "Konvektionsströme im zähflüssigen Erdmantel bewegen die Kontinentalplatten langsam gegeneinander."),
        ("Was entsteht häufig an den Grenzen zwischen zwei Kontinentalplatten?", new[] { "Vulkane und Erdbeben", "Wüsten", "Gletscher" }, "Vulkane und Erdbeben",
            "An Plattengrenzen entstehen durch Spannungen und Reibung häufig Erdbeben und Vulkanismus."),
        ("Wie nennt man Gebirge, die durch das Aufeinandertreffen zweier Kontinentalplatten entstehen (z.B. Himalaya)?", new[] { "Faltengebirge", "Flachland", "Senke" }, "Faltengebirge",
            "Wenn zwei Kontinentalplatten zusammenstoßen, wird die Erdkruste aufgefaltet - so entstehen Faltengebirge wie der Himalaya oder die Alpen."),
        ("Was passiert, wenn sich zwei Kontinentalplatten voneinander wegbewegen (z.B. am mittelozeanischen Rücken)?", new[] { "Es dringt neues Magma nach oben und bildet neue Erdkruste", "Es entsteht sofort eine Wüste", "Die Erde wird dort kälter" }, "Es dringt neues Magma nach oben und bildet neue Erdkruste",
            "An solchen Plattengrenzen ('divergente Plattengrenzen') steigt Magma auf und erstarrt zu neuer Erdkruste, wodurch der Meeresboden langsam wächst."),
        ("Wie nennt man die riesigen, starren Platten, aus denen die äußere Erdkruste besteht?", new[] { "Kontinentalplatten (tektonische Platten)", "Klimazonen", "Gletscher" }, "Kontinentalplatten (tektonische Platten)",
            "Die Erdkruste ist in mehrere große, starre Kontinentalplatten (tektonische Platten) aufgeteilt, die auf dem zähflüssigen Erdmantel schwimmen."),
        ("Wie hieß der Superkontinent, aus dem sich vor Jahrmillionen alle heutigen Kontinente durch Plattenbewegung entwickelt haben?", new[] { "Pangaea", "Atlantis", "Eurasien" }, "Pangaea",
            "Pangaea war ein Superkontinent, der vor rund 300 Millionen Jahren fast alle heutigen Kontinente vereinte, bevor sie auseinanderdrifteten."),
        ("Wer stellte Anfang des 20. Jahrhunderts die Theorie der Kontinentalverschiebung auf?", new[] { "Alfred Wegener", "Albert Einstein", "Charles Darwin" }, "Alfred Wegener",
            "Der deutsche Wissenschaftler Alfred Wegener entwickelte 1912 die Theorie der Kontinentalverschiebung."),
        ("Welches Indiz nutzte Alfred Wegener u.a. für seine Theorie der Kontinentalverschiebung?", new[] { "Die Küstenlinien von Afrika und Südamerika passen wie Puzzleteile zusammen", "Der Mond verändert seine Umlaufbahn", "Die Ozeane werden jedes Jahr wärmer" }, "Die Küstenlinien von Afrika und Südamerika passen wie Puzzleteile zusammen",
            "Wegener fiel auf, dass die Küstenlinien mancher Kontinente wie Puzzleteile zueinander passen - ein Hinweis darauf, dass sie einst zusammenhingen."),
        ("Wie nennt man eine Zone mit besonders vielen Vulkanen und Erdbeben rund um den Pazifischen Ozean?", new[] { "Pazifischer Feuerring (Ring of Fire)", "Sahelzone", "Polarkreis" }, "Pazifischer Feuerring (Ring of Fire)",
            "Der Pazifische Feuerring umrahmt den Pazifik und ist wegen vieler Plattengrenzen besonders reich an Vulkanen und Erdbeben."),
        ("Womit wird die Stärke eines Erdbebens meist gemessen?", new[] { "Richterskala (bzw. Momenten-Magnituden-Skala)", "Beaufortskala", "Celsius-Skala" }, "Richterskala (bzw. Momenten-Magnituden-Skala)",
            "Die Stärke von Erdbeben wird meist mit der Richterskala oder der moderneren Momenten-Magnituden-Skala angegeben."),
        ("Was passiert bei einer Subduktion, wenn eine ozeanische Platte auf eine Kontinentalplatte trifft?", new[] { "Die schwerere ozeanische Platte taucht unter die Kontinentalplatte ab", "Beide Platten schmelzen sofort komplett", "Es entsteht ein neuer Ozean" }, "Die schwerere ozeanische Platte taucht unter die Kontinentalplatte ab",
            "Bei der Subduktion taucht die dichtere ozeanische Platte unter die leichtere Kontinentalplatte, was oft Vulkane und Erdbeben verursacht."),
        ("Was ist eine Transformstörung wie die San-Andreas-Verwerfung in Kalifornien?", new[] { "Zwei Platten reiben seitlich aneinander vorbei", "Zwei Platten entfernen sich nur voneinander", "Eine Platte schmilzt vollständig" }, "Zwei Platten reiben seitlich aneinander vorbei",
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
        ("Was kann bei einem Vulkanausbruch aus dem Erdinneren an die Oberfläche gelangen?", new[] { "Magma (dann Lava genannt), Asche und Gase", "Nur Wasser", "Nur Sand" }, "Magma (dann Lava genannt), Asche und Gase",
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
        ("Was versteht man unter \"Treibhausgasen\"?", new[] { "Gase wie CO₂ und Methan, die Wärme in der Atmosphäre zurückhalten", "Gase, die die Erde abkühlen", "Nur Sauerstoff und Stickstoff" }, "Gase wie CO₂ und Methan, die Wärme in der Atmosphäre zurückhalten",
            "Treibhausgase wie CO₂ und Methan halten Wärme in der Atmosphäre zurück und verstärken so die Erderwärmung."),
        ("Welche menschliche Aktivität setzt besonders viel CO₂ frei?", new[] { "Verbrennung fossiler Brennstoffe wie Kohle, Öl und Gas", "Fotosynthese von Pflanzen", "Trinkwasseraufbereitung" }, "Verbrennung fossiler Brennstoffe wie Kohle, Öl und Gas",
            "Die Verbrennung fossiler Brennstoffe in Kraftwerken, Autos und der Industrie ist die Hauptquelle menschengemachter CO₂-Emissionen."),
        ("Was ist eine Folge des Klimawandels für Wetterextreme?", new[] { "Hitzewellen, Dürren und Starkregen nehmen zu", "Es gibt keine Wetterextreme mehr", "Das Wetter wird überall gleich" }, "Hitzewellen, Dürren und Starkregen nehmen zu",
            "Der Klimawandel lässt Wetterextreme wie Hitzewellen, Dürren und Starkregen häufiger und intensiver auftreten."),
        ("Was bedeutet \"CO₂-Fußabdruck\"?", new[] { "Die Menge an Treibhausgasen, die durch das Verhalten einer Person oder eines Landes verursacht wird", "Der Abdruck eines Schuhs im Schnee", "Die Fläche eines Landes" }, "Die Menge an Treibhausgasen, die durch das Verhalten einer Person oder eines Landes verursacht wird",
            "Der CO₂-Fußabdruck misst, wie viele Treibhausgase durch den Lebensstil einer Person, eines Unternehmens oder Landes verursacht werden."),
        ("Warum ist Methan (z.B. aus der Massentierhaltung) klimaschädlich?", new[] { "Es ist ein starkes Treibhausgas, das Wärme in der Atmosphäre zurückhält", "Es kühlt die Atmosphäre ab", "Es hat keinen Einfluss aufs Klima" }, "Es ist ein starkes Treibhausgas, das Wärme in der Atmosphäre zurückhält",
            "Methan ist ein besonders wirksames Treibhausgas und entsteht u.a. bei der Massentierhaltung und in Mülldeponien."),
        ("Was ist das Ziel des Pariser Klimaabkommens?", new[] { "Die Erderwärmung deutlich unter 2 Grad (möglichst 1,5 Grad) zu begrenzen", "Die Erderwärmung auf 5 Grad zu erhöhen", "Fossile Brennstoffe unbegrenzt zu nutzen" }, "Die Erderwärmung deutlich unter 2 Grad (möglichst 1,5 Grad) zu begrenzen",
            "Im Pariser Klimaabkommen von 2015 einigten sich fast alle Staaten darauf, die Erderwärmung deutlich unter 2, möglichst auf 1,5 Grad zu begrenzen."),
        ("Was können einzelne Menschen tun, um weniger CO₂ zu verursachen?", new[] { "Weniger Auto fahren, mehr Fahrrad/Bus/Bahn nutzen", "Mehr Flugreisen unternehmen", "Mehr Kohle verbrennen" }, "Weniger Auto fahren, mehr Fahrrad/Bus/Bahn nutzen",
            "Wer öfter Fahrrad, Bus oder Bahn statt Auto nutzt, verursacht weniger CO₂-Emissionen."),
        ("Was passiert mit vielen Tier- und Pflanzenarten durch den schnellen Klimawandel?", new[] { "Manche Arten verlieren ihren Lebensraum und sind vom Aussterben bedroht", "Alle Arten profitieren gleichermaßen", "Es gibt keine Auswirkungen auf Tiere und Pflanzen" }, "Manche Arten verlieren ihren Lebensraum und sind vom Aussterben bedroht",
            "Viele Tier- und Pflanzenarten können sich nicht schnell genug an den raschen Klimawandel anpassen und verlieren ihren Lebensraum."),
        ("Was ist ein Unterschied zwischen \"Wetter\" und \"Klima\"?", new[] { "Wetter beschreibt kurzfristige Zustände, Klima den langjährigen Durchschnitt", "Wetter und Klima bedeuten genau dasselbe", "Klima ändert sich nur innerhalb eines Tages" }, "Wetter beschreibt kurzfristige Zustände, Klima den langjährigen Durchschnitt",
            "Wetter ist der kurzfristige Zustand der Atmosphäre, Klima beschreibt das durchschnittliche Wettergeschehen über viele Jahre."),
        ("Warum ist das Abschmelzen der Permafrostböden in der Arktis problematisch fürs Klima?", new[] { "Dabei wird zusätzlich gespeichertes Methan und CO₂ freigesetzt", "Dadurch wird die Erde automatisch kühler", "Permafrostböden haben keinen Einfluss aufs Klima" }, "Dabei wird zusätzlich gespeichertes Methan und CO₂ freigesetzt",
            "Auftauender Permafrost setzt lange gespeichertes Methan und CO₂ frei, was den Klimawandel weiter verstärkt."),
        ("Was bedeutet \"Klimaneutralität\"?", new[] { "Es wird nicht mehr Treibhausgas ausgestoßen, als an anderer Stelle gebunden/eingespart wird", "Es wird gar kein Strom mehr verbraucht", "Es gibt kein Klima mehr" }, "Es wird nicht mehr Treibhausgas ausgestoßen, als an anderer Stelle gebunden/eingespart wird",
            "Klimaneutralität bedeutet, dass ausgestoßene Treibhausgase durch Einsparungen oder Kompensation ausgeglichen werden."),
        ("Welche Energiequelle zählt zu den erneuerbaren Energien?", new[] { "Windkraft", "Braunkohle", "Erdöl" }, "Windkraft",
            "Windkraft ist im Gegensatz zu Kohle und Öl eine erneuerbare, klimafreundlichere Energiequelle."),
        ("Wie beeinflusst der Klimawandel viele Küstenregionen und Inselstaaten?", new[] { "Sie sind durch den steigenden Meeresspiegel zunehmend vom Überfluten bedroht", "Sie werden automatisch größer", "Sie sind überhaupt nicht betroffen" }, "Sie sind durch den steigenden Meeresspiegel zunehmend vom Überfluten bedroht",
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
        ("Was ist eine typische Herausforderung schnell wachsender Megastädte?", new[] { "Wohnungsmangel und Verkehrsprobleme", "Zu wenig Einwohner", "Zu viel freie Fläche" },
            "Wohnungsmangel und Verkehrsprobleme", "Schnelles Städtewachstum führt oft zu Wohnraumknappheit, Verkehrsstaus und Umweltproblemen."),
        ("Ab wie vielen Einwohnern spricht man üblicherweise von einer \"Megastadt\"?", new[] { "Ab etwa 10 Millionen Einwohnern", "Ab etwa 10.000 Einwohnern", "Ab etwa 100.000 Einwohnern" },
            "Ab etwa 10 Millionen Einwohnern", "Als Megastädte werden meist Ballungsräume mit mehr als 10 Millionen Einwohnern bezeichnet, z.B. Tokio oder Istanbul."),
        ("Warum ziehen viele Menschen vom Land in die Stadt?", new[] { "Wegen mehr Arbeitsplätzen und besserer Infrastruktur", "Weil es in Städten weniger Menschen gibt", "Weil Städte immer billiger sind" },
            "Wegen mehr Arbeitsplätzen und besserer Infrastruktur", "Städte bieten oft mehr Arbeitsplätze, Bildungsangebote und Infrastruktur, was viele Menschen zum Umzug bewegt."),
        ("Was ist ein \"Slum\"?", new[] { "Ein armes, oft provisorisches Wohnviertel am Rand einer schnell wachsenden Stadt", "Ein modernes Einkaufszentrum", "Ein besonders teures Wohnviertel" },
            "Ein armes, oft provisorisches Wohnviertel am Rand einer schnell wachsenden Stadt", "In vielen schnell wachsenden Megastädten entstehen Slums, weil Zuwanderer sich keine regulären Wohnungen leisten können."),
        ("Wie nennt man den Prozess, wenn Menschen vom Land in die Stadt ziehen?", new[] { "Land-Stadt-Wanderung (Binnenmigration)", "Stadtflucht", "Auswanderung" },
            "Land-Stadt-Wanderung (Binnenmigration)", "Wenn Menschen innerhalb eines Landes vom Land in die Stadt ziehen, nennt man das Land-Stadt-Wanderung."),
        ("Was ist ein \"Ballungsraum\"?", new[] { "Ein dicht besiedeltes Gebiet mit einer oder mehreren großen Städten", "Ein dünn besiedeltes ländliches Gebiet", "Eine unbewohnte Wüstenregion" },
            "Ein dicht besiedeltes Gebiet mit einer oder mehreren großen Städten", "Ein Ballungsraum ist ein dicht besiedeltes Gebiet, das durch eine oder mehrere große Städte geprägt ist."),
        ("In welchen Weltregionen wachsen die Städte heute besonders schnell?", new[] { "Vor allem in Asien und Afrika", "Vor allem in der Antarktis", "Nirgendwo, Städte schrumpfen überall" },
            "Vor allem in Asien und Afrika", "Das schnellste Städtewachstum findet heute vor allem in Asien und Afrika statt."),
        ("Was versteht man unter \"Suburbanisierung\"?", new[] { "Menschen ziehen vom Stadtzentrum ins Umland (Vororte)", "Menschen ziehen vom Land direkt in die Innenstadt", "Städte werden komplett aufgegeben" },
            "Menschen ziehen vom Stadtzentrum ins Umland (Vororte)", "Suburbanisierung beschreibt die Abwanderung von Menschen aus der Innenstadt ins städtische Umland."),
        ("Welches Problem entsteht häufig durch schnelles, ungeplantes Städtewachstum?", new[] { "Luftverschmutzung und Verkehrsstaus", "Zu viel unbebaute Fläche", "Zu wenig Einwohner" },
            "Luftverschmutzung und Verkehrsstaus", "Schnell und ungeplant wachsende Städte kämpfen oft mit Luftverschmutzung, Verkehrsstaus und Infrastrukturproblemen."),
        ("Was ist eine \"Metropole\"?", new[] { "Eine sehr große, bedeutende Stadt mit hoher wirtschaftlicher und kultureller Bedeutung", "Ein kleines Dorf", "Eine unbewohnte Insel" },
            "Eine sehr große, bedeutende Stadt mit hoher wirtschaftlicher und kultureller Bedeutung", "Metropolen sind besonders große und bedeutende Städte mit hoher wirtschaftlicher und kultureller Ausstrahlung."),
        ("Warum entstehen in schnell wachsenden Städten oft informelle Siedlungen (z.B. Favelas, Slums)?", new[] { "Weil der offizielle Wohnungsbau nicht mit dem schnellen Bevölkerungswachstum mithält", "Weil zu wenige Menschen in die Stadt ziehen", "Weil es in der Stadt keine Arbeit gibt" },
            "Weil der offizielle Wohnungsbau nicht mit dem schnellen Bevölkerungswachstum mithält", "Wenn der reguläre Wohnungsbau nicht mit dem Bevölkerungswachstum Schritt hält, entstehen oft informelle Siedlungen wie Favelas."),
        ("Was bedeutet \"Landflucht\"?", new[] { "Menschen verlassen ländliche Gebiete, oft wegen fehlender Arbeitsplätze", "Menschen ziehen aus der Stadt aufs Land", "Bauern bauen mehr Felder an" },
            "Menschen verlassen ländliche Gebiete, oft wegen fehlender Arbeitsplätze", "Landflucht bezeichnet die Abwanderung von Menschen aus ländlichen Gebieten, oft wegen fehlender Arbeitsplätze und Perspektiven."),
        ("Wie wirkt sich Verstädterung häufig auf die Landwirtschaft in ländlichen Gebieten aus?", new[] { "Weniger Arbeitskräfte bleiben auf dem Land, da viele in die Stadt abwandern", "Es gibt danach mehr Bauern als vorher", "Landwirtschaft verschwindet weltweit komplett" },
            "Weniger Arbeitskräfte bleiben auf dem Land, da viele in die Stadt abwandern", "Durch die Abwanderung in die Städte fehlen in vielen ländlichen Regionen Arbeitskräfte für die Landwirtschaft."),
        ("Was ist ein Vorteil des Lebens in einer Großstadt, den viele Zuwanderer suchen?", new[] { "Bessere Chancen auf Arbeit, Bildung und medizinische Versorgung", "Weniger Lärm und Verkehr", "Mehr freie Naturflächen" },
            "Bessere Chancen auf Arbeit, Bildung und medizinische Versorgung", "Viele Menschen ziehen in Großstädte, weil sie sich dort bessere Chancen auf Arbeit, Bildung und medizinische Versorgung erhoffen."),
        ("Welche Stadt zählt zu den größten Megastädten der Welt?", new[] { "Tokio", "Erfurt", "Kiel" },
            "Tokio", "Der Großraum Tokio zählt mit vielen Millionen Einwohnern zu den größten Megastädten der Welt."),
        ("Was bedeutet \"Gentrifizierung\" in Großstädten?", new[] { "Aufwertung eines Stadtviertels, wodurch Mieten steigen und einkommensschwächere Bewohner verdrängt werden", "Der komplette Abriss einer Stadt", "Das Verbot von Neubauten" },
            "Aufwertung eines Stadtviertels, wodurch Mieten steigen und einkommensschwächere Bewohner verdrängt werden", "Bei der Gentrifizierung wird ein Stadtviertel aufgewertet, wodurch steigende Mieten oft einkommensschwächere Bewohner verdrängen."),
        ("Was ist eine Herausforderung beim Verkehr in schnell wachsenden Megastädten?", new[] { "Überfüllte Straßen und lange Staus", "Zu wenige Autos auf den Straßen", "Keine Verkehrsmittel werden benötigt" },
            "Überfüllte Straßen und lange Staus", "In vielen schnell wachsenden Megastädten führen zu viele Fahrzeuge zu überfüllten Straßen und langen Staus."),
        ("Wie können Städte dem Wohnraummangel durch Verstädterung begegnen?", new[] { "Durch den Bau von bezahlbarem Wohnraum und guter Infrastruktur", "Indem sie keine neuen Wohnungen bauen", "Indem sie alle Zuwanderer abweisen" },
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
        ("Was misst der \"Human Development Index\" (HDI) eines Landes ungefähr?", new[] { "Lebensstandard: u.a. Einkommen, Bildung und Lebenserwartung", "Nur die Fläche eines Landes", "Nur die Anzahl der Einwohner" },
            "Lebensstandard: u.a. Einkommen, Bildung und Lebenserwartung", "Der HDI kombiniert Einkommen, Bildungsstand und Lebenserwartung, um den Entwicklungsstand eines Landes zu vergleichen."),
        ("Was ist eine typische Ursache für Armut in vielen Ländern des globalen Südens?", new[] { "Ungleicher Zugang zu Bildung, Wasser und Arbeit", "Zu viel Regen das ganze Jahr über", "Zu wenige Feiertage" },
            "Ungleicher Zugang zu Bildung, Wasser und Arbeit", "Fehlender Zugang zu Bildung, sauberem Wasser, medizinischer Versorgung und fairer Arbeit zählt zu den wichtigsten Armutsursachen."),
        ("Was bedeutet \"Nord-Süd-Gefälle\" in der Geografie?", new[] { "Reichere Industrieländer liegen oft im globalen Norden, ärmere Entwicklungsländer oft im globalen Süden", "Im Süden ist es immer kälter als im Norden", "Alle Länder der Erde sind gleich wohlhabend" },
            "Reichere Industrieländer liegen oft im globalen Norden, ärmere Entwicklungsländer oft im globalen Süden", "Das \"Nord-Süd-Gefälle\" beschreibt vereinfacht, dass wohlhabendere Industrieländer eher im globalen Norden, viele ärmere Entwicklungsländer eher im globalen Süden liegen."),
        ("Was ist ein \"Entwicklungsland\"?", new[] { "Ein Land mit vergleichsweise niedrigem Einkommen, Bildungsstand und Gesundheitsversorgung", "Ein Land, das gerade erst gegründet wurde", "Ein Land ohne eigene Sprache" },
            "Ein Land mit vergleichsweise niedrigem Einkommen, Bildungsstand und Gesundheitsversorgung", "Entwicklungsländer haben im Vergleich zu Industrieländern oft geringeres Einkommen, weniger Bildungschancen und eine schwächere Gesundheitsversorgung."),
        ("Wie kann fairer Handel (Fairtrade) armen Bäuerinnen und Bauern helfen?", new[] { "Durch garantierte, faire Mindestpreise für ihre Produkte", "Indem ihre Produkte billiger verkauft werden müssen", "Indem sie keine Produkte mehr exportieren dürfen" },
            "Durch garantierte, faire Mindestpreise für ihre Produkte", "Fairtrade-Siegel garantieren Erzeugerinnen und Erzeugern faire Mindestpreise, was ihnen ein stabileres Einkommen sichert."),
        ("Was bedeutet \"absolute Armut\"?", new[] { "Wenn grundlegende Bedürfnisse wie Nahrung, Wasser und Wohnraum nicht gedeckt werden können", "Wenn man kein teures Auto besitzt", "Wenn man weniger verdient als der Nachbar" },
            "Wenn grundlegende Bedürfnisse wie Nahrung, Wasser und Wohnraum nicht gedeckt werden können", "Absolute Armut liegt vor, wenn selbst lebensnotwendige Grundbedürfnisse wie Nahrung, Wasser oder Wohnraum nicht gesichert sind."),
        ("Was bedeutet \"relative Armut\"?", new[] { "Wenn jemand deutlich weniger besitzt als der Durchschnitt der Gesellschaft, in der er lebt", "Wenn jemand gar kein Einkommen hat", "Wenn ein Land keine Rohstoffe hat" },
            "Wenn jemand deutlich weniger besitzt als der Durchschnitt der Gesellschaft, in der er lebt", "Relative Armut bemisst sich am Vergleich mit dem Lebensstandard der übrigen Gesellschaft, nicht an absoluten Mindeststandards."),
        ("Was ist ein \"Schwellenland\"?", new[] { "Ein Land auf dem Weg von einem Entwicklungsland zu einem Industrieland", "Ein Land ohne jegliche Wirtschaft", "Ein anderer Name für Industrieland" },
            "Ein Land auf dem Weg von einem Entwicklungsland zu einem Industrieland", "Schwellenländer wie Brasilien oder Indien befinden sich wirtschaftlich zwischen Entwicklungs- und Industrieland."),
        ("Was ist eine typische Ursache für Hunger in manchen Regionen der Welt?", new[] { "Dürren, Kriege und ungerechte Verteilung von Nahrungsmitteln", "Zu viel Regen das ganze Jahr", "Zu viele Supermärkte" },
            "Dürren, Kriege und ungerechte Verteilung von Nahrungsmitteln", "Hunger entsteht oft durch eine Kombination aus Dürren, Kriegen und einer ungerechten Verteilung von Nahrungsmitteln."),
        ("Wie kann Bildung helfen, Armut zu verringern?", new[] { "Bildung eröffnet bessere Chancen auf Arbeit und höheres Einkommen", "Bildung hat keinen Einfluss auf Armut", "Bildung macht Länder automatisch ärmer" },
            "Bildung eröffnet bessere Chancen auf Arbeit und höheres Einkommen", "Gute Bildung verbessert die Chancen auf qualifizierte Arbeit und ein höheres Einkommen und hilft so, Armut zu verringern."),
        ("Was ist Entwicklungshilfe?", new[] { "Unterstützung reicherer Länder für ärmere Länder, z.B. durch Geld, Wissen oder Projekte", "Ein Handelsverbot zwischen Ländern", "Eine Steuer nur für arme Länder" },
            "Unterstützung reicherer Länder für ärmere Länder, z.B. durch Geld, Wissen oder Projekte", "Entwicklungshilfe umfasst finanzielle, technische oder projektbezogene Unterstützung reicherer für ärmere Länder."),
        ("Was versteht man unter \"Kinderarbeit\" als Folge von Armut?", new[] { "Kinder müssen statt zur Schule zu gehen arbeiten, um zum Familieneinkommen beizutragen", "Kinder arbeiten freiwillig als Hobby", "Kinderarbeit gibt es nur in reichen Ländern" },
            "Kinder müssen statt zur Schule zu gehen arbeiten, um zum Familieneinkommen beizutragen", "In armen Familien müssen Kinder oft statt zur Schule zu gehen arbeiten, um zum Lebensunterhalt der Familie beizutragen."),
        ("Was bedeutet \"ungleiche Einkommensverteilung\" innerhalb eines Landes?", new[] { "Ein kleiner Teil der Bevölkerung besitzt einen sehr großen Teil des Vermögens", "Alle Menschen verdienen exakt gleich viel", "Es gibt in keinem Land Unterschiede" },
            "Ein kleiner Teil der Bevölkerung besitzt einen sehr großen Teil des Vermögens", "Bei ungleicher Einkommensverteilung besitzt ein kleiner, oft sehr reicher Teil der Bevölkerung einen unverhältnismäßig großen Teil des Vermögens."),
        ("Wie wirkt sich fehlender Zugang zu sauberem Trinkwasser auf die Armut aus?", new[] { "Er führt zu Krankheiten und erschwert Bildung und Arbeit", "Er hat keinerlei Auswirkungen", "Er macht ein Land automatisch reicher" },
            "Er führt zu Krankheiten und erschwert Bildung und Arbeit", "Fehlender Zugang zu sauberem Trinkwasser führt zu Krankheiten und erschwert Schulbesuch und Arbeit, was Armut verfestigt."),
        ("Was sind die \"Sustainable Development Goals\" (SDGs) der Vereinten Nationen?", new[] { "Weltweite Ziele u.a. zur Bekämpfung von Armut und Hunger bis 2030", "Ein Vertrag nur über Klimaschutz", "Ein Handelsabkommen zwischen zwei Ländern" },
            "Weltweite Ziele u.a. zur Bekämpfung von Armut und Hunger bis 2030", "Die 17 SDGs der Vereinten Nationen sollen bis 2030 u.a. Armut und Hunger weltweit deutlich verringern."),
        ("Welche Organisation der Vereinten Nationen kümmert sich besonders um Kinderrechte weltweit?", new[] { "UNICEF", "NATO", "FIFA" },
            "UNICEF", "UNICEF ist das Kinderhilfswerk der Vereinten Nationen und setzt sich weltweit für Kinderrechte ein."),
        ("Was kann internationaler fairer Handel im Vergleich zu herkömmlichem Handel bewirken?", new[] { "Bessere Arbeitsbedingungen und stabilere Einkommen für Produzenten in ärmeren Ländern", "Höhere Gewinne nur für große Konzerne", "Er hat keinerlei Effekt auf die Produzenten" },
            "Bessere Arbeitsbedingungen und stabilere Einkommen für Produzenten in ärmeren Ländern", "Fairer Handel sichert Produzentinnen und Produzenten in ärmeren Ländern oft bessere Arbeitsbedingungen und stabilere Einkommen."),
        ("Was ist ein Mikrokredit?", new[] { "Ein kleiner Kredit, der armen Menschen hilft, ein eigenes kleines Geschäft aufzubauen", "Ein riesiger Kredit nur für Großkonzerne", "Eine Steuer auf Lebensmittel" },
            "Ein kleiner Kredit, der armen Menschen hilft, ein eigenes kleines Geschäft aufzubauen", "Mikrokredite sind kleine Darlehen, die armen Menschen ermöglichen, sich mit einem eigenen kleinen Geschäft selbst zu helfen."),
        ("Welche Rolle spielt Korruption häufig bei der Armutsbekämpfung?", new[] { "Sie kann Hilfsgelder und Ressourcen von den Bedürftigen fernhalten", "Sie hilft immer, Armut schneller zu beseitigen", "Sie hat keinerlei Einfluss" },
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
}
