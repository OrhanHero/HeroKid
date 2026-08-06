using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class TrafficSignCatalog
{
    /// <summary>
    /// Vorschriftzeichen: sie <b>ordnen an</b>. Wer sie missachtet, begeht eine
    /// Ordnungswidrigkeit - anders als bei den Gefahrzeichen, die nur warnen.
    ///
    /// <para>Die Farbe verrät die Richtung: <b>roter</b> Rand = Verbot ("das darfst du nicht"),
    /// <b>blauer</b> Grund = Gebot ("das musst du" oder "das darfst du hier"). Diese eine Regel
    /// erklärt die Hälfte aller runden Zeichen.</para>
    /// </summary>
    private static readonly TrafficSign[] Vorschrift =
    {
        new()
        {
            Number = "201-50", Name = "Andreaskreuz",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Transparent, FillColor = SignColors.Transparent,
            PathData = SignPictograms.Andreaskreuz, PathColor = SignColors.Rot,
            Meaning = "Dem Schienenverkehr Vorrang gewähren. Steht unmittelbar vor jedem Bahnübergang.",
            Hint = "Das einzige Zeichen in Kreuzform. Zug gewinnt immer - er kann weder bremsen noch ausweichen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "205", Name = "Vorfahrt gewähren",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.DreieckSpitzeUnten,
            Meaning = "Vorfahrt gewähren. Anhalten muss man nur, wenn tatsächlich jemand kommt.",
            Hint = "Die Spitze zeigt nach unten - wie ein Daumen: du musst zurückstecken. Die Form ist einzigartig, damit man sie auch von hinten erkennt.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "206", Name = "Halt. Vorfahrt gewähren",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Achteck,
            FillColor = SignColors.Rot, Text = "STOP", TextColor = SignColors.Weiss,
            Meaning = "Anhalten ist Pflicht - auch wenn niemand kommt. Erst an der Haltlinie stehen, dann schauen.",
            Hint = "Der Unterschied zu VZ 205: hier musst du IMMER stehen bleiben, nicht nur wenn jemand kommt.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "208", Name = "Vorrang des Gegenverkehrs",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.PfeilGeradeausSchaft, PathStrokeThickness = 10,
            OverlayPathData = SignPictograms.PfeilNachRechtsSpitze, OverlayColor = SignColors.Rot,
            Meaning = "An einer Engstelle hat der Gegenverkehr Vorrang. Warten, bis frei ist.",
            Hint = "Der rote Pfeil ist der Gegenverkehr - rot heißt hier \"nicht du\".",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "209-20", Name = "Vorgeschriebene Fahrtrichtung rechts",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.AbbiegenRechtsSchaft, PathColor = SignColors.Weiss, PathStrokeThickness = 11,
            OverlayPathData = SignPictograms.AbbiegenRechtsSpitze, OverlayColor = SignColors.Weiss,
            Meaning = "Hier muss nach rechts abgebogen werden. Geradeaus oder links ist verboten.",
            Hint = "Blau mit weißem Pfeil = Gebot: genau da lang, nirgendwo anders hin.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "209-30", Name = "Vorgeschriebene Fahrtrichtung geradeaus",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.PfeilGeradeausSchaft, PathColor = SignColors.Weiss, PathStrokeThickness = 11,
            OverlayPathData = SignPictograms.PfeilGeradeausSpitze, OverlayColor = SignColors.Weiss,
            Meaning = "Es muss geradeaus weitergefahren werden. Abbiegen ist verboten.",
            Hint = "Ein blaues Gebot zwingt zu einer Richtung - es erlaubt sie nicht nur.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "215", Name = "Kreisverkehr",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.KreisverkehrPfeile, PathColor = SignColors.Weiss, PathStrokeThickness = 9,
            OverlayPathData = SignPictograms.KreisverkehrSpitzen, OverlayColor = SignColors.Weiss,
            Meaning = "Es ist im Kreis herum zu fahren, entgegen dem Uhrzeigersinn. Steht fast immer zusammen mit VZ 205.",
            Hint = "Im Kreisverkehr wird beim Hineinfahren NICHT geblinkt, beim Hinausfahren schon.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "220-20", Name = "Einbahnstraße",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.PfeilNachRechtsSchaft, PathColor = SignColors.Weiss, PathStrokeThickness = 12,
            OverlayPathData = SignPictograms.PfeilNachRechtsSpitze, OverlayColor = SignColors.Weiss,
            Meaning = "Die Straße darf nur in Pfeilrichtung befahren werden. Von der anderen Seite steht dort VZ 267.",
            Hint = "Liegendes blaues Rechteck mit Pfeil - nicht mit dem runden Richtungsgebot verwechseln.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "237", Name = "Radweg",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Fahrrad, PathColor = SignColors.Weiss, PathStrokeThickness = 7,
            Meaning = "Radfahrende müssen diesen Weg benutzen; andere Fahrzeuge dürfen ihn nicht befahren.",
            Hint = "Blauer Kreis = Benutzungspflicht. Ohne Schild darf man auch auf der Straße fahren.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "239", Name = "Gehweg",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Fussgaenger, PathColor = SignColors.Weiss,
            Meaning = "Nur für Fußgänger. Radfahren ist hier nicht erlaubt - außer ein Zusatzzeichen gestattet es.",
            Hint = "Kinder bis 8 Jahre MÜSSEN mit dem Rad auf den Gehweg, bis 10 dürfen sie.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "240", Name = "Gemeinsamer Geh- und Radweg",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.FahrradOben, PathColor = SignColors.Weiss, PathStrokeThickness = 6,
            OverlayPathData = SignPictograms.FussgaengerUnten, OverlayColor = SignColors.Weiss,
            Meaning = "Fußgänger und Radfahrende teilen sich einen Weg. Radfahrende müssen Rücksicht nehmen und notfalls Schritt fahren.",
            Hint = "Beide Sinnbilder ohne Trennstrich = gemeinsam. Mit Trennstrich (VZ 241) = jeder auf seiner Seite.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "242.1", Name = "Beginn einer Fußgängerzone",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Fussgaenger, PathColor = SignColors.Weiss,
            Meaning = "Ab hier gehört die Straße den Fußgängern. Fahrzeuge nur, wenn ein Zusatzzeichen es erlaubt.",
            Hint = "Auch Radfahren ist hier verboten, solange kein Zusatzzeichen es ausdrücklich erlaubt.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "250", Name = "Verbot für Fahrzeuge aller Art",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            Meaning = "Kein Fahrzeug darf hier hinein - auch kein Fahrrad. Zu Fuß gehen ist erlaubt.",
            Hint = "Der leere rote Kreis ist das schärfste Fahrverbot: alles, was Räder hat, bleibt draußen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "251", Name = "Verbot für Kraftwagen",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.Pkw,
            Meaning = "Autos und andere mehrspurige Kraftfahrzeuge dürfen nicht hinein. Fahrräder und Motorräder schon.",
            Hint = "Nur das Sinnbild ist verboten - was nicht abgebildet ist, darf.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "253", Name = "Verbot für Kraftfahrzeuge über 3,5 t",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.Lkw,
            Meaning = "LKW über 3,5 t zulässiger Gesamtmasse dürfen nicht durchfahren. PKW und Busse sind ausgenommen.",
            Hint = "Gemeint ist das erlaubte Gesamtgewicht, nicht was gerade geladen ist.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "254", Name = "Verbot für Radverkehr",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.Fahrrad, PathStrokeThickness = 7,
            Meaning = "Radfahren ist verboten. Das Rad zu schieben ist erlaubt - dann gilt man als Fußgänger.",
            Hint = "Roter Kreis = Verbot, blauer Kreis = Radweg. Die Farbe entscheidet, nicht das Fahrrad.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "255", Name = "Verbot für Krafträder",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.Kraftrad, PathStrokeThickness = 7,
            Meaning = "Motorräder, auch mit Beiwagen, sowie Kleinkrafträder und Mofas dürfen nicht durchfahren.",
            Hint = "Gilt auch fürs Mofa - Hauptsache Motor und zwei Räder.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "259", Name = "Verbot für Fußgänger",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.Fussgaenger,
            Meaning = "Zu Fuß gehen ist hier verboten, etwa auf Autobahnen oder in Baustellen.",
            Hint = "Das seltenste der Verbotszeichen - und ausgerechnet das, an dem Leute sterben, wenn sie es ignorieren.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "260", Name = "Verbot für Krafträder und Kraftwagen",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.KraftradKlein, PathStrokeThickness = 6,
            OverlayPathData = SignPictograms.PkwKlein, OverlayColor = SignColors.Schwarz,
            Meaning = "Alle Kraftfahrzeuge müssen draußen bleiben. Fahrräder, Fußgänger und Fuhrwerke dürfen.",
            Hint = "Zwei Sinnbilder in einem Kreis heißt: beide sind gemeint.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "262", Name = "Verbot über angegebener tatsächlicher Masse",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            Text = "5,5 t",
            Meaning = "Fahrzeuge, die tatsächlich schwerer sind als angegeben, dürfen nicht durchfahren.",
            Hint = "Hier zählt das WIRKLICHE Gewicht mit Ladung - anders als bei VZ 253.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "264", Name = "Verbot über angegebener tatsächlicher Breite",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            Text = "2,5 m",
            Meaning = "Fahrzeuge, die breiter sind als angegeben (einschließlich Ladung und Spiegel), dürfen nicht durch.",
            Hint = "Die Ladung zählt mit. Ein breiter Anhänger kann das Fahrzeug zu breit machen.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "265", Name = "Verbot über angegebener tatsächlicher Höhe",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            Text = "3,8 m",
            Meaning = "Fahrzeuge, die höher sind als angegeben, dürfen nicht durchfahren - typisch vor Brücken und Tunneln.",
            Hint = "Das Schild ist die letzte Warnung vor der Brücke. Danach ist das Dach ab.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "267", Name = "Verbot der Einfahrt",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            FillColor = SignColors.Rot,
            PathData = SignPictograms.QuerbalkenBreit, PathColor = SignColors.Weiss,
            Meaning = "Einfahrt verboten. Steht meist am falschen Ende einer Einbahnstraße.",
            Hint = "Roter Kreis mit weißem Balken: du fährst gerade gegen die Richtung. Umdrehen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "272", Name = "Verbot des Wendens",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.WendenSchaft, PathStrokeThickness = 10,
            OverlayPathData = SignPictograms.WendenSpitze, OverlayColor = SignColors.Schwarz,
            Meaning = "Wenden ist verboten. Abbiegen bleibt erlaubt.",
            Hint = "Der U-Pfeil ist durchgestrichen - nicht umdrehen, aber abbiegen darfst du.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "274-50", Name = "Zulässige Höchstgeschwindigkeit",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            Text = "50",
            Meaning = "Schneller als angegeben darf hier nicht gefahren werden. Es ist eine Obergrenze, keine Vorgabe.",
            Hint = "\"Zulässig\" heißt höchstens - bei Regen oder Nebel muss man langsamer fahren, auch wenn 50 dasteht.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "274.1", Name = "Beginn einer Tempo-30-Zone",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Quadrat,
            Text = "30",
            Meaning = "In der ganzen Zone gilt Tempo 30. Innerhalb gilt überall rechts vor links, es gibt keine Vorfahrtstraßen.",
            Hint = "Zone heißt: das Tempo gilt bis zum Ende-Schild, nicht nur bis zur nächsten Kreuzung.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "274.2", Name = "Ende einer Tempo-30-Zone",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Quadrat,
            Text = "30",
            PathData = SignPictograms.SchraegstrichKreuz, PathColor = "#7A7A7A", PathStrokeThickness = 6,
            Meaning = "Ende der Tempo-30-Zone. Ab hier gilt wieder die allgemeine Regel - innerorts also 50.",
            Hint = "Graue Striche darüber heißen bei jedem Zeichen: das hier ist jetzt vorbei.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "276", Name = "Überholverbot für Kraftfahrzeuge aller Art",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            PathData = SignPictograms.UeberholtesFahrzeug, PathColor = "#7A7A7A",
            OverlayPathData = SignPictograms.UeberholendesFahrzeug, OverlayColor = SignColors.Rot,
            Meaning = "Kraftfahrzeuge dürfen einander nicht überholen. Das rote Auto ist das überholende.",
            Hint = "Ein Fahrrad zu überholen bleibt erlaubt - verboten ist nur das Überholen von Kraftfahrzeugen.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "278-50", Name = "Ende der zulässigen Höchstgeschwindigkeit",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = "#7A7A7A", Text = "50", TextColor = "#7A7A7A",
            PathData = SignPictograms.SchraegstrichAbwaerts, PathColor = "#7A7A7A", PathStrokeThickness = 6,
            Meaning = "Die angegebene Begrenzung endet. Es gilt wieder, was allgemein erlaubt ist.",
            Hint = "Grau statt rot und durchgestrichen: die Beschränkung ist aufgehoben - nicht \"jetzt musst du 50 fahren\".",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "282", Name = "Ende sämtlicher Streckenverbote",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            BorderColor = "#7A7A7A",
            PathData = SignPictograms.EndeStreckenverbote, PathColor = "#7A7A7A", PathStrokeThickness = 6,
            Meaning = "Alle streckenbezogenen Geschwindigkeits- und Überholverbote sind aufgehoben.",
            Hint = "Ein Schild räumt alles ab, statt jedes Verbot einzeln zu beenden.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "283", Name = "Absolutes Haltverbot",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            FillColor = SignColors.Blau,
            PathData = SignPictograms.SchraegstrichKreuz, PathColor = SignColors.Rot, PathStrokeThickness = 9,
            Meaning = "Hier darf überhaupt nicht gehalten werden - auch nicht kurz zum Ein- und Aussteigen.",
            Hint = "Zwei Striche = zweimal verboten: weder halten noch parken. Ein Strich (VZ 286) erlaubt kurzes Halten.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "286", Name = "Eingeschränktes Haltverbot",
            Category = TrafficSignCategory.Vorschriftzeichen, Shape = SignShape.Kreis,
            FillColor = SignColors.Blau,
            PathData = SignPictograms.SchraegstrichAbwaerts, PathColor = SignColors.Rot, PathStrokeThickness = 9,
            Meaning = "Parken verboten. Halten bis zu drei Minuten zum Ein- und Aussteigen oder Beladen ist erlaubt.",
            Hint = "Ein Strich = ein Verbot: nur parken. Die Kurzform: aussteigen ja, stehenlassen nein.",
            RelevantForBicycle = false
        }
    };
}
