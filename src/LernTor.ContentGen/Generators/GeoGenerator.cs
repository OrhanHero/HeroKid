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
            "Antarktika liegt am Südpol und ist fast vollständig von einem dicken Eispanzer bedeckt.")
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
            "In der gemäßigten Zone (z.B. Deutschland) wechseln sich Frühling, Sommer, Herbst und Winter mit moderaten Temperaturen ab.")
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
        ("Wie heißt die Hauptstadt von Sachsen?", "Dresden", "Dresden ist die Landeshauptstadt des Bundeslandes Sachsen.")
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
            "Die Erdkruste ist in mehrere große, starre Kontinentalplatten (tektonische Platten) aufgeteilt, die auf dem zähflüssigen Erdmantel schwimmen.")
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
            "Bäume binden bei der Fotosynthese CO₂ und speichern Kohlenstoff im Holz - Abholzung setzt dieses CO₂ wieder frei und verstärkt den Klimawandel.")
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
            "Ein armes, oft provisorisches Wohnviertel am Rand einer schnell wachsenden Stadt", "In vielen schnell wachsenden Megastädten entstehen Slums, weil Zuwanderer sich keine regulären Wohnungen leisten können.")
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
            "Durch garantierte, faire Mindestpreise für ihre Produkte", "Fairtrade-Siegel garantieren Erzeugerinnen und Erzeugern faire Mindestpreise, was ihnen ein stabileres Einkommen sichert.")
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
