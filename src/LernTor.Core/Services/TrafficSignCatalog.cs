using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Alle Verkehrszeichen des Führerschein-Bereichs, nach StVO Anlage 1-3.
///
/// <para><b>Rechtliches in einem Satz:</b> die Zeichen stehen in einer Verordnung und sind damit
/// amtliches Werk (§ 5 UrhG) - Form, Farbe und Bedeutung darf jeder nachbauen. Übernommen ist
/// hier nichts: jedes Zeichen ist aus der Verordnungsbeschreibung als Geometrie neu beschrieben
/// (siehe <see cref="SignPictograms"/>), die Erklärtexte sind selbst geschrieben.</para>
///
/// <para><b>Auswahl:</b> 77 Zeichen aus allen fünf Gruppen - die, die in der Theorieprüfung
/// und im Berliner Alltag tatsächlich vorkommen. Die amtliche Übersicht kennt rund 250; darunter
/// sind Zollstellen, NATO-Brückenschilder und Umleitungsplanskizzen, die kein Kind je braucht.
/// Erweitern geht jederzeit: ein Eintrag mehr im passenden Teil, sonst nichts.</para>
/// </summary>
public static partial class TrafficSignCatalog
{
    /// <summary>
    /// Bewusst <see cref="Lazy{T}"/> statt eines direkten Feld-Initialisierers: die Teil-Arrays
    /// stehen in anderen Dateien derselben partiellen Klasse, und für Feld-Initialisierer über
    /// Teildateien hinweg ist die Reihenfolge <b>nicht festgelegt</b>. Ein direkter Initialisierer
    /// konnte also aus noch nicht befüllten Arrays gebaut werden - der Compiler hat genau davor
    /// gewarnt (CS8604). Lazy verschiebt das Zusammenbauen auf den ersten Zugriff; dann ist der
    /// statische Konstruktor durch und alle Teile stehen.
    /// </summary>
    private static readonly Lazy<TrafficSign[]> AllSigns = new(() =>
        // Das "!" ist hier belegt, nicht geraten: die Lambda laeuft erst beim ersten Zugriff
        // auf .Value, also nach dem statischen Konstruktor - da stehen alle Teil-Arrays.
        // Ohne die Freizeichnung meldet der Compiler fuenf CS8604-Warnungen, weil er die
        // Verzoegerung nicht sieht; die wuerden echte Funde im Rauschen untergehen lassen.
        Gefahr!.Concat(Vorschrift!).Concat(Richt!).Concat(Einrichtungen!).Concat(Zusatz!)
            .ToArray());

    /// <summary>Alle Zeichen in Lernreihenfolge (Gefahr → Vorschrift → Richt → Einrichtungen → Zusatz).</summary>
    public static IReadOnlyList<TrafficSign> All => AllSigns.Value;

    /// <summary>Die Kategorien in der empfohlenen Lernreihenfolge - genau die Reihenfolge des Enums.</summary>
    public static IReadOnlyList<TrafficSignCategory> LearningOrder { get; } =
        Enum.GetValues<TrafficSignCategory>();

    public static IReadOnlyList<TrafficSign> ByCategory(TrafficSignCategory category) =>
        AllSigns.Value.Where(sign => sign.Category == category).ToList();

    /// <summary>Zeichen, die schon fürs Fahrrad gelten - für die Kinder heute relevant.</summary>
    public static IReadOnlyList<TrafficSign> ForBicycle() =>
        AllSigns.Value.Where(sign => sign.RelevantForBicycle).ToList();

    public static TrafficSign? ByNumber(string number) =>
        AllSigns.Value.FirstOrDefault(sign => sign.Number == number);

    public static string CategoryLabel(TrafficSignCategory category) => category switch
    {
        TrafficSignCategory.Gefahrzeichen => "Gefahrzeichen",
        TrafficSignCategory.Vorschriftzeichen => "Vorschriftzeichen",
        TrafficSignCategory.Richtzeichen => "Richtzeichen",
        TrafficSignCategory.Verkehrseinrichtung => "Verkehrseinrichtungen",
        TrafficSignCategory.Zusatzzeichen => "Zusatzzeichen",
        _ => category.ToString()
    };

    /// <summary>Ein Satz in Kindersprache, was diese Gruppe überhaupt ist.</summary>
    public static string CategoryDescription(TrafficSignCategory category) => category switch
    {
        TrafficSignCategory.Gefahrzeichen =>
            "Dreieckig mit rotem Rand. Sie warnen nur - verboten oder angeordnet wird damit nichts.",
        TrafficSignCategory.Vorschriftzeichen =>
            "Meist rund. Roter Rand heißt Verbot, blauer Grund heißt Gebot. Diese Zeichen musst du befolgen.",
        TrafficSignCategory.Richtzeichen =>
            "Meist blaue Rechtecke. Sie geben Hinweise und regeln, wer zuerst darf.",
        TrafficSignCategory.Verkehrseinrichtung =>
            "Schranken, Baken und Leitkegel. Sie sichern Baustellen und leiten dich vorbei.",
        TrafficSignCategory.Zusatzzeichen =>
            "Kleine weiße Schilder unter einem anderen Zeichen. Sie verändern das Zeichen darüber - allein bedeuten sie nichts.",
        _ => string.Empty
    };
}
