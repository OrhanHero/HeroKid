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
            [GradeLevel.Klasse9] = new List<TopicFactory> { Plattentektonik, Klimawandel, Verstaedterung }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KontinenteListe =
    {
        ("Wie viele Kontinente gibt es (gängige Zählweise: Afrika, Amerika, Antarktika, Asien, Australien/Ozeanien, Europa)?", new[] { "6", "4", "10" }, "6",
            "In der gängigen Zählweise gibt es 6 Kontinente. (Manche zählen Nord- und Südamerika getrennt = 7.)"),
        ("Welcher Ozean ist der größte der Erde?", new[] { "Pazifischer Ozean", "Atlantischer Ozean", "Indischer Ozean" }, "Pazifischer Ozean",
            "Der Pazifische Ozean ist mit Abstand der flächenmäßig größte Ozean der Erde."),
        ("Auf welchem Kontinent liegt Deutschland?", new[] { "Europa", "Asien", "Afrika" }, "Europa", "Deutschland liegt in Mitteleuropa, auf dem Kontinent Europa.")
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
            "Deutschland liegt in der gemäßigten Klimazone mit vier Jahreszeiten und mildem Klima.")
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
        ("Berlin ist sowohl eine Stadt als auch ein eigenes Bundesland. Wie nennt man das?", "Stadtstaat", "Berlin (wie auch Hamburg und Bremen) ist ein Stadtstaat: Stadt und Bundesland zugleich.")
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
            "An Plattengrenzen entstehen durch Spannungen und Reibung häufig Erdbeben und Vulkanismus.")
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
            "Durch die globale Erwärmung schmelzen Gletscher und Polkappen zunehmend, was u.a. den Meeresspiegel steigen lässt.")
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
            "Wohnungsmangel und Verkehrsprobleme", "Schnelles Städtewachstum führt oft zu Wohnraumknappheit, Verkehrsstaus und Umweltproblemen.")
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
}
