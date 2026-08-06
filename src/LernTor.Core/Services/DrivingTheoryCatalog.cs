using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Die Theoriefragen des Führerschein-Bereichs, nach den amtlichen Sachgebieten gegliedert.
///
/// <para><b>Rechtliches vorweg:</b> der amtliche Fragenkatalog gehört der TÜV|DEKRA arge tp 21
/// und darf hier nicht hinein - kommerzielle Lern-Apps lizenzieren ihn. Diese Fragen sind selbst
/// geschrieben und decken dieselben Sachgebiete aus StVO und StVZO ab. Zum Lernen gleichwertig,
/// wortgleich mit der Prüfung sind sie nicht. Wer wortgleiche Fragen will, kann sie über den
/// Eltern-Import selbst eintragen.</para>
///
/// <para><b>Die Fragen bilden die Prüfung nach, nicht ein Quiz:</b> mehrere Antworten können
/// richtig sein, und jede Frage wiegt 2 bis 5 Fehlerpunkte. Wer mit Ein-aus-vier-Quizzen übt,
/// lernt das Falsche - siehe <see cref="TheoryExamRules"/>.</para>
/// </summary>
public static partial class DrivingTheoryCatalog
{
    /// <summary>
    /// Sachgebiet Verkehrszeichen. Diese Fragen zeigen ein echtes Schild aus dem
    /// <see cref="TrafficSignCatalog"/> - so hängen die beiden Unterbereiche zusammen, statt
    /// nebeneinanderher zu laufen.
    /// </summary>
    private static readonly TheoryQuestion[] Zeichenfragen =
    {
        new()
        {
            Id = "vz-01", Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen, Points = 5,
            SignNumber = "205",
            Prompt = "Was musst du bei diesem Zeichen tun?",
            Options = new[]
            {
                "Vorfahrt gewähren - anhalten nur, wenn jemand kommt",
                "Auf jeden Fall anhalten, auch wenn niemand kommt",
                "Du hast Vorfahrt",
                "Die Geschwindigkeit auf 30 km/h verringern"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "VZ 205 verlangt, Vorfahrt zu gewähren - anhalten muss man nur, wenn tatsächlich jemand kommt. Das Anhalten selbst ordnet erst das Stoppschild (VZ 206) an."
        },
        new()
        {
            Id = "vz-02", Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen, Points = 4,
            SignNumber = "101",
            Prompt = "Welche Grundform haben Gefahrzeichen, und was bedeutet das?",
            Options = new[]
            {
                "Dreieckig mit rotem Rand - sie warnen, ordnen aber nichts an",
                "Dreieckig mit rotem Rand - sie schreiben eine Höchstgeschwindigkeit vor",
                "Rund mit rotem Rand - sie verbieten etwas",
                "Rechteckig und blau - sie geben Hinweise"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Gefahrzeichen warnen nur. Sie schreiben kein Tempo vor, sondern verlangen eine der Lage angemessene Geschwindigkeit - genau daran scheitern viele in der Prüfung."
        },
        new()
        {
            Id = "vz-03", Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen, Points = 4,
            SignNumber = "237",
            Prompt = "Was bedeutet ein rundes blaues Zeichen im Unterschied zu einem runden roten?",
            Options = new[]
            {
                "Blau ordnet etwas an oder erlaubt es, Rot verbietet etwas",
                "Blau ist nur ein Hinweis ohne rechtliche Wirkung",
                "Beide bedeuten dasselbe, die Farbe ist Gewohnheit",
                "Blau gilt nur außerorts, Rot nur innerorts"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Roter Rand = Verbot, blauer Grund = Gebot. Diese eine Regel erklärt die Hälfte aller runden Zeichen - das Fahrrad im blauen Kreis ist ein Radweg, im roten Kreis ein Radfahrverbot."
        },
        new()
        {
            Id = "vz-04", Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen, Points = 3,
            SignNumber = "1020-30",
            Prompt = "Ein Zusatzzeichen \"Anlieger frei\" hängt unter einem Durchfahrtverbot. Wer darf fahren?",
            Options = new[]
            {
                "Wer dort wohnt, arbeitet oder jemanden besucht",
                "Alle, das Verbot ist damit aufgehoben",
                "Nur Bewohner mit gemeldetem Wohnsitz",
                "Niemand, das Zusatzzeichen verschärft das Verbot"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Anlieger ist, wer dort tatsächlich etwas zu tun hat - auch Besuch und Lieferanten. Nur durchzufahren, um abzukürzen, zählt nicht."
        },
        new()
        {
            Id = "vz-05", Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen, Points = 4,
            SignNumber = "306",
            Prompt = "Du fährst auf einer Vorfahrtstraße (gelbe Raute). Wie lange gilt das?",
            Options = new[]
            {
                "An jeder Kreuzung, bis das Ende-Zeichen kommt",
                "Nur an der nächsten Kreuzung",
                "Bis zur nächsten Ampel",
                "Nur innerhalb geschlossener Ortschaften"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die gelbe Raute gilt fortlaufend. Das auf die nächste Kreuzung beschränkte Zeichen ist VZ 301 - ein Dreieck mit Kreuz, das viele damit verwechseln."
        }
    };

    private static readonly Lazy<TheoryQuestion[]> AllQuestions = new(() =>
        // Wie beim Zeichenkatalog: die Teil-Arrays stehen in anderen Dateien derselben
        // partiellen Klasse, und deren Initialisierungsreihenfolge ist nicht festgelegt.
        // Lazy verschiebt das Zusammenbauen auf den ersten Zugriff.
        Grundlagen!.Concat(Verhalten!).Concat(Zeichenfragen!).ToArray());

    public static IReadOnlyList<TheoryQuestion> All => AllQuestions.Value;

    /// <summary>Die Sachgebiete in Kursreihenfolge - genau die Reihenfolge des Enums.</summary>
    public static IReadOnlyList<DrivingTheoryTopic> Topics { get; } =
        Enum.GetValues<DrivingTheoryTopic>();

    public static IReadOnlyList<TheoryQuestion> ByTopic(DrivingTheoryTopic topic) =>
        AllQuestions.Value.Where(frage => frage.Topic == topic).ToList();

    public static TheoryQuestion? ById(string id) =>
        AllQuestions.Value.FirstOrDefault(frage => frage.Id == id);

    public static string TopicLabel(DrivingTheoryTopic topic) => topic switch
    {
        DrivingTheoryTopic.PersoenlicheVoraussetzungen => "Persönliche Voraussetzungen",
        DrivingTheoryTopic.RechtlicheRahmenbedingungen => "Rechtliche Rahmenbedingungen",
        DrivingTheoryTopic.Strassenverkehrssystem => "Straßenverkehrssystem",
        DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen => "Verkehrszeichen",
        DrivingTheoryTopic.VorfahrtUndRegelung => "Vorfahrt und Verkehrsregelung",
        DrivingTheoryTopic.GeschwindigkeitUndAbstand => "Geschwindigkeit und Abstand",
        DrivingTheoryTopic.FahrmanoeverUndUeberholen => "Fahrmanöver und Überholen",
        DrivingTheoryTopic.RuhenderVerkehr => "Halten und Parken",
        DrivingTheoryTopic.AndereVerkehrsteilnehmer => "Andere Verkehrsteilnehmer",
        DrivingTheoryTopic.BesondereSituationen => "Besondere Situationen",
        DrivingTheoryTopic.UnfallUndPanne => "Unfall und Panne",
        DrivingTheoryTopic.UmweltUndSparsamkeit => "Umwelt und Sparsamkeit",
        DrivingTheoryTopic.FahrzeugtechnikUndSicherheit => "Fahrzeugtechnik und Sicherheit",
        DrivingTheoryTopic.BefoerderungUndAnhaenger => "Beförderung und Anhänger",
        _ => topic.ToString()
    };
}
