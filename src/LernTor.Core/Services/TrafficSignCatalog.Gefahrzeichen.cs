using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class TrafficSignCatalog
{
    /// <summary>
    /// Gefahrzeichen: dreieckig, roter Rand, Spitze oben. Sie <b>warnen</b> nur - sie ordnen
    /// nichts an und verbieten nichts. Genau das ist die Prüfungsfrage, an der die meisten
    /// scheitern: ein Gefahrzeichen schreibt kein Tempo vor, es verlangt "angemessene"
    /// Geschwindigkeit.
    /// </summary>
    private static readonly TrafficSign[] Gefahr =
    {
        new()
        {
            Number = "101", Name = "Gefahrstelle",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Ausrufezeichen,
            Meaning = "Warnt vor einer Gefahr, für die es kein eigenes Zeichen gibt. Ein Zusatzzeichen darunter sagt oft, worum es geht.",
            Hint = "Das Ausrufezeichen ist der Joker unter den Warnschildern: irgendetwas stimmt hier nicht, schau genau hin.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "102", Name = "Kreuzung oder Einmündung mit Vorfahrt von rechts",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Kreuzung,
            Meaning = "Es gilt die Grundregel \"rechts vor links\". Wer von rechts kommt, darf zuerst.",
            Hint = "Kein Vorfahrtschild in Sicht heißt nicht \"ich darf\", sondern \"rechts darf\".",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "103-20", Name = "Kurve (rechts)",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.KurveRechtsSchaft, PathStrokeThickness = 11,
            OverlayPathData = SignPictograms.PfeilspitzeRechts, OverlayColor = SignColors.Schwarz,
            Meaning = "Warnt vor einer Rechtskurve. Vor der Kurve abbremsen, nicht in ihr.",
            Hint = "Bremsen gehört vor die Kurve. Wer mitten drin bremst, verliert den Halt.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "103-10", Name = "Kurve (links)",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.KurveLinksSchaft, PathStrokeThickness = 11,
            OverlayPathData = SignPictograms.PfeilspitzeLinks, OverlayColor = SignColors.Schwarz,
            Meaning = "Warnt vor einer Linkskurve. Vor der Kurve abbremsen, nicht in ihr.",
            Hint = "Links und rechts unterscheiden sich nur in der Pfeilrichtung - genau hinschauen lohnt sich.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "105-10", Name = "Doppelkurve (zunächst links)",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.DoppelkurveSchaft, PathStrokeThickness = 11,
            OverlayPathData = SignPictograms.PfeilspitzeOben, OverlayColor = SignColors.Schwarz,
            Meaning = "Warnt vor zwei aufeinanderfolgenden Kurven, die erste nach links.",
            Hint = "Nach der ersten Kurve ist es nicht vorbei - die zweite kommt sofort.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "108-10", Name = "Gefälle",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            Text = "10%",
            Meaning = "Es geht bergab, hier um 10 %. Der Bremsweg wird länger; im Auto einen kleineren Gang wählen.",
            Hint = "Gefälle heißt bergab - das Fahrzeug wird von selbst schneller.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "110-10", Name = "Steigung",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            Text = "10%",
            Meaning = "Es geht bergauf, hier um 10 %. Langsame Fahrzeuge können den Verkehr aufhalten.",
            Hint = "Steigung heißt bergauf - hier kriechen LKW, überholen lohnt selten.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "112", Name = "Unebene Fahrbahn",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.UnebeneFahrbahn, PathStrokeThickness = 9,
            Meaning = "Warnt vor Löchern, Wellen oder Aufwölbungen in der Fahrbahn.",
            Hint = "Zwei Wellen im Dreieck: die Straße ist buckelig, langsamer fahren.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "114", Name = "Schleuder- oder Rutschgefahr",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Schleudergefahr, PathStrokeThickness = 7,
            Meaning = "Die Fahrbahn kann bei Nässe oder Schmutz besonders rutschig sein.",
            Hint = "Die Schlangenlinie unter dem Auto ist die Spur eines Wagens, der schon gerutscht ist.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "117-20", Name = "Seitenwind",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Seitenwind, PathStrokeThickness = 7,
            Meaning = "Warnt vor starkem Wind von der Seite, oft an Brücken und Waldausgängen.",
            Hint = "Das ist ein Windsack wie am Flugplatz. Lenkrad oder Lenker gut festhalten.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "120", Name = "Verengte Fahrbahn",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.VerengteFahrbahn, PathStrokeThickness = 9,
            Meaning = "Die Fahrbahn wird von beiden Seiten schmaler.",
            Hint = "Beide Linien rücken zusammen - von links UND rechts wird es enger.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "121-10", Name = "Einseitig (rechts) verengte Fahrbahn",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.EinseitigVerengt, PathStrokeThickness = 9,
            Meaning = "Nur die rechte Seite der Fahrbahn wird schmaler.",
            Hint = "Eine Linie bleibt gerade, eine knickt ein - nur diese Seite wird enger.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "123", Name = "Arbeitsstelle",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Arbeitsstelle,
            Meaning = "Warnt vor einer Baustelle: Menschen auf der Fahrbahn, Maschinen, plötzliche Hindernisse.",
            Hint = "Hier arbeiten Menschen direkt neben den Autos. Runter vom Gas.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "124", Name = "Stau",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Stau,
            Meaning = "Warnt vor stockendem Verkehr oder Stauende - die häufigste Ursache für Auffahrunfälle.",
            Hint = "Drei Autos übereinander: es steht. Wer hinten drauffährt, ist fast immer schuld.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "125", Name = "Gegenverkehr",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Gegenverkehr, PathStrokeThickness = 8,
            Meaning = "Warnt vor Gegenverkehr auf einer Straße, die vorher nur in eine Richtung befahren wurde.",
            Hint = "Zwei Pfeile, die aneinander vorbeizeigen - ab hier kommt dir wieder jemand entgegen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "131", Name = "Lichtzeichenanlage",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.AmpelGehaeuse, PathStrokeThickness = 7,
            OverlayPathData = SignPictograms.AmpelLichter, OverlayColor = SignColors.Schwarz,
            Meaning = "Warnt vor einer Ampel, die man sonst zu spät sehen würde - etwa hinter einer Kurve.",
            Hint = "Das Schild kommt, bevor die Ampel zu sehen ist. Fuß runter vom Gas.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "133-10", Name = "Fußgänger",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Fussgaenger,
            Meaning = "Warnt vor Fußgängern auf oder neben der Fahrbahn.",
            Hint = "Achtung: Menschen zu Fuß haben kein Blech um sich herum.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "136-10", Name = "Kinder",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Kinder,
            Meaning = "Warnt vor Kindern, etwa an Schulen und Spielplätzen. Kinder verhalten sich unberechenbar.",
            Hint = "Kinder rennen ohne zu schauen auf die Straße. Hier gilt: mit dem Schlimmsten rechnen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "138-10", Name = "Radverkehr",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Fahrrad, PathStrokeThickness = 7,
            Meaning = "Warnt davor, dass hier Radfahrende die Fahrbahn kreuzen oder auf sie einbiegen.",
            Hint = "Fahrrad im roten Dreieck heißt Warnung. Fahrrad im blauen Kreis heißt Radweg - nicht verwechseln.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "142-10", Name = "Wildwechsel",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Wildwechsel,
            Meaning = "Warnt vor Wildtieren auf der Fahrbahn, vor allem in der Dämmerung.",
            Hint = "Wo ein Reh war, kommen meist noch mehr. Nach dem ersten Tier nicht sofort wieder beschleunigen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "151", Name = "Bahnübergang",
            Category = TrafficSignCategory.Gefahrzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.Bahnuebergang,
            Meaning = "Warnt vor einem Bahnübergang. Der Schienenverkehr hat immer Vorrang.",
            Hint = "Ein Zug braucht bis zum Stehen einen Kilometer. Ausweichen kann er nicht.",
            RelevantForBicycle = true
        }
    };
}
