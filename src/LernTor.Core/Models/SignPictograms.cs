namespace LernTor.Core.Models;

/// <summary>
/// Die Piktogramme der Verkehrszeichen als Geometriepfade in einem gedachten Feld von 0..100 in
/// beiden Richtungen. Als benannte Konstanten, weil dieselbe Figur auf mehreren Zeichen sitzt:
/// das Fahrrad auf VZ 138 (Warnung), 237 (Radweg) und 254 (Verbot), der Fußgänger auf 133, 239
/// und 259. Ein Pfad, der an einer Stelle stimmt, stimmt damit überall.
///
/// <para><b>Bewusst vereinfacht.</b> Ziel ist Wiedererkennbarkeit, nicht die millimetergenaue
/// Nachbildung der Verordnungszeichnung: die Kinder sollen "das ist der Fußgänger" denken, nicht
/// die Strichstärke prüfen. Wo die amtliche Figur zu fein für eine Karteikarte wäre (Hirsch,
/// Bauarbeiter), steht eine kräftigere Silhouette.</para>
///
/// <para>Nutzbare Fläche je Grundform - außerhalb ragt das Piktogramm über den Rand:
/// Dreieck (Spitze oben) etwa x 28..72 / y 40..80, Kreis etwa x 22..78 / y 22..78,
/// Rechteck fast vollständig.</para>
/// </summary>
public static class SignPictograms
{
    // ---------------------------------------------------------------- Gefahrzeichen

    /// <summary>Ausrufezeichen (VZ 101 Gefahrstelle).</summary>
    public const string Ausrufezeichen = "M45,34 L55,34 L53,64 L47,64 Z M45,70 L55,70 L55,80 L45,80 Z";

    /// <summary>Gleichschenkliges Kreuz (VZ 102 Kreuzung, Vorfahrt von rechts).</summary>
    public const string Kreuzung = "M46,38 L54,38 L54,55 L72,55 L72,63 L54,63 L54,82 L46,82 L46,63 L28,63 L28,55 L46,55 Z";

    /// <summary>Andreaskreuz (VZ 201) - zwei gekreuzte Balken.</summary>
    public const string Andreaskreuz = "M12,38 L26,24 L88,86 L74,100 Z M74,24 L88,38 L26,100 L12,86 Z";

    /// <summary>Schaft einer Rechtskurve; die Spitze kommt aus <see cref="PfeilspitzeRechts"/>.</summary>
    public const string KurveRechtsSchaft = "M44,84 L44,60 Q44,46 60,46 L72,46";

    public const string PfeilspitzeRechts = "M70,34 L88,46 L70,58 Z";

    /// <summary>Schaft einer Linkskurve.</summary>
    public const string KurveLinksSchaft = "M56,84 L56,60 Q56,46 40,46 L28,46";

    public const string PfeilspitzeLinks = "M30,34 L12,46 L30,58 Z";

    /// <summary>Doppelkurve, zunächst links (VZ 105-10).</summary>
    public const string DoppelkurveSchaft = "M58,84 L58,70 Q58,60 44,58 Q30,56 30,44 L30,34";

    public const string PfeilspitzeOben = "M18,36 L30,18 L42,36 Z";

    /// <summary>Zwei Wellen (VZ 112 Unebene Fahrbahn).</summary>
    public const string UnebeneFahrbahn = "M22,68 Q34,44 46,68 Q58,44 70,68 Q76,80 82,74";

    /// <summary>Auto mit Schleuderspur (VZ 114).</summary>
    public const string Schleudergefahr =
        "M32,50 L40,38 L62,38 L70,50 L70,62 L30,62 Z M20,76 Q38,66 50,78 Q62,90 80,78";

    /// <summary>Windsack am Mast (VZ 117 Seitenwind).</summary>
    public const string Seitenwind = "M26,30 L26,84 M26,36 L74,46 L74,58 L26,66 Z";

    /// <summary>Zwei nach innen laufende Kanten (VZ 120 Verengte Fahrbahn).</summary>
    public const string VerengteFahrbahn = "M26,84 L38,50 L38,32 M74,84 L62,50 L62,32";

    /// <summary>Nur die rechte Kante läuft nach innen (VZ 121-10).</summary>
    public const string EinseitigVerengt = "M34,84 L34,32 M74,84 L60,50 L60,32";

    /// <summary>Bauarbeiter mit Schaufel (VZ 123 Arbeitsstelle).</summary>
    public const string Arbeitsstelle =
        "M44,30 A6,6 0 1 1 56,30 A6,6 0 1 1 44,30 Z " +
        "M42,40 L58,40 L62,64 L54,64 L54,84 L46,84 L46,64 L38,64 Z " +
        "M60,44 L82,70 L76,76 L54,50 Z";

    /// <summary>Drei Fahrzeuge hintereinander (VZ 124 Stau).</summary>
    public const string Stau =
        "M30,26 L70,26 L70,42 L30,42 Z M30,48 L70,48 L70,64 L30,64 Z M30,70 L70,70 L70,86 L30,86 Z";

    /// <summary>Zwei entgegengesetzte Pfeile (VZ 125 Gegenverkehr).</summary>
    public const string Gegenverkehr =
        "M38,30 L38,84 M38,30 L28,44 M38,30 L48,44 M62,84 L62,30 M62,84 L52,70 M62,84 L72,70";

    /// <summary>Ampel-Gehäuse (VZ 131 Lichtzeichenanlage); die Lichter sind eigene Kreise.</summary>
    public const string AmpelGehaeuse = "M38,26 L62,26 L62,86 L38,86 Z";

    public const string AmpelLichter =
        "M50,38 m-7,0 a7,7 0 1 0 14,0 a7,7 0 1 0 -14,0 Z " +
        "M50,56 m-7,0 a7,7 0 1 0 14,0 a7,7 0 1 0 -14,0 Z " +
        "M50,74 m-7,0 a7,7 0 1 0 14,0 a7,7 0 1 0 -14,0 Z";

    /// <summary>Gehender Mensch - VZ 133 (Warnung), 239 (Gehweg), 259 (Verbot).</summary>
    public const string Fussgaenger =
        "M52,16 A7,7 0 1 1 52,30 A7,7 0 1 1 52,16 Z " +
        "M46,32 L58,32 L64,56 L58,58 L54,44 L54,62 L64,88 L56,90 L48,66 L40,88 L32,86 L44,58 L44,40 L38,52 L32,50 Z";

    /// <summary>Erwachsener und Kind an der Hand (VZ 136 Kinder).</summary>
    public const string Kinder =
        "M36,14 A6,6 0 1 1 36,26 A6,6 0 1 1 36,14 Z " +
        "M30,28 L42,28 L48,50 L42,52 L38,42 L38,60 L46,88 L38,90 L32,66 L26,88 L18,86 L28,58 L28,42 L24,50 L18,48 Z " +
        "M68,34 A5,5 0 1 1 68,44 A5,5 0 1 1 68,34 Z " +
        "M63,46 L73,46 L78,62 L73,64 L70,58 L70,68 L76,88 L69,90 L64,72 L59,88 L52,86 L60,66 L60,58 L57,64 L52,62 Z";

    /// <summary>Fahrrad - VZ 138 (Warnung), 237 (Radweg), 254 (Verbot).</summary>
    public const string Fahrrad =
        "M26,66 m-14,0 a14,14 0 1 0 28,0 a14,14 0 1 0 -28,0 Z " +
        "M74,66 m-14,0 a14,14 0 1 0 28,0 a14,14 0 1 0 -28,0 Z " +
        "M26,66 L44,66 L58,40 M44,66 L60,66 L74,66 M58,40 L70,40 M50,40 L64,40 M74,66 L62,44";

    /// <summary>Springender Hirsch (VZ 142 Wildwechsel) - kräftige Silhouette.</summary>
    public const string Wildwechsel =
        "M20,80 L28,62 L34,50 L46,44 L60,44 L72,50 L78,42 L80,30 L76,22 L84,26 L88,18 L88,32 L84,44 L76,56 " +
        "L74,68 L80,84 L72,84 L66,70 L54,72 L46,84 L38,84 L44,68 L34,64 L28,80 Z";

    /// <summary>Dampflok (VZ 151 Bahnübergang).</summary>
    public const string Bahnuebergang =
        "M18,72 L82,72 L82,80 L18,80 Z " +
        "M26,44 L52,44 L52,72 L26,72 Z M52,56 L74,56 L74,72 L52,72 Z " +
        "M32,34 L40,34 L40,44 L32,44 Z " +
        "M30,80 m-6,0 a6,6 0 1 0 12,0 a6,6 0 1 0 -12,0 Z " +
        "M68,80 m-6,0 a6,6 0 1 0 12,0 a6,6 0 1 0 -12,0 Z";

    // ---------------------------------------------------------------- Fahrzeuge

    /// <summary>PKW von der Seite - VZ 251 (Verbot für Kraftwagen), 276 (Überholverbot).</summary>
    public const string Pkw =
        "M14,58 L24,40 L54,40 L68,58 L84,60 L86,74 L14,74 Z " +
        "M28,74 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z " +
        "M72,74 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z";

    /// <summary>LKW von der Seite (VZ 253).</summary>
    public const string Lkw =
        "M8,42 L48,42 L48,74 L8,74 Z M48,52 L68,52 L82,64 L82,74 L48,74 Z " +
        "M24,76 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z " +
        "M70,76 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z";

    /// <summary>Motorrad von der Seite (VZ 255).</summary>
    public const string Kraftrad =
        "M22,70 m-12,0 a12,12 0 1 0 24,0 a12,12 0 1 0 -24,0 Z " +
        "M78,70 m-12,0 a12,12 0 1 0 24,0 a12,12 0 1 0 -24,0 Z " +
        "M22,70 L40,70 L52,48 L66,48 L78,70 M40,70 L58,70 M52,48 L44,36 L56,36";

    /// <summary>PKW von vorn (VZ 331.1 Kraftfahrstraße).</summary>
    public const string PkwFrontal =
        "M18,52 L28,30 L72,30 L82,52 L82,72 L70,72 L70,64 L30,64 L30,72 L18,72 Z";

    // ------------------------------------------------- Zeichen aus ZWEI Sinnbildern
    // Diese drei Zeichen zeigen zwei Figuren NEBEN- bzw. übereinander. Sie bekommen eigene,
    // kleinere Pfade statt der Einzelfiguren: die beiden vollen Sinnbilder übereinandergelegt
    // ergäben nur einen schwarzen Klecks, in dem nichts mehr zu erkennen ist.

    /// <summary>Überholtes Fahrzeug (grau), linke Hälfte von VZ 276.</summary>
    public const string UeberholtesFahrzeug =
        "M6,56 L12,44 L30,44 L38,56 L44,58 L44,68 L6,68 Z " +
        "M15,68 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z " +
        "M36,68 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z";

    /// <summary>Überholendes Fahrzeug (rot), rechte Hälfte von VZ 276.</summary>
    public const string UeberholendesFahrzeug =
        "M54,36 L60,24 L78,24 L86,36 L92,38 L92,48 L54,48 Z " +
        "M63,48 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z " +
        "M84,48 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z";

    /// <summary>Motorrad, verkleinert für die linke Hälfte von VZ 260.</summary>
    public const string KraftradKlein =
        "M14,64 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z " +
        "M42,64 m-8,0 a8,8 0 1 0 16,0 a8,8 0 1 0 -16,0 Z " +
        "M14,64 L26,64 L34,50 L42,50 L42,64 M26,64 L34,64 M34,50 L29,42 L37,42";

    /// <summary>PKW, verkleinert für die rechte Hälfte von VZ 260.</summary>
    public const string PkwKlein =
        "M52,54 L58,42 L76,42 L84,54 L92,55 L92,64 L52,64 Z " +
        "M61,64 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z " +
        "M83,64 m-5,0 a5,5 0 1 0 10,0 a5,5 0 1 0 -10,0 Z";

    /// <summary>Fahrrad in der oberen Hälfte (VZ 240, gemeinsamer Geh- und Radweg).</summary>
    public const string FahrradOben =
        "M26,32 m-10,0 a10,10 0 1 0 20,0 a10,10 0 1 0 -20,0 Z " +
        "M74,32 m-10,0 a10,10 0 1 0 20,0 a10,10 0 1 0 -20,0 Z " +
        "M26,32 L44,32 L54,14 M44,32 L60,32 L74,32 M54,14 L64,14 M74,32 L64,17";

    /// <summary>Fußgänger in der unteren Hälfte (VZ 240).</summary>
    public const string FussgaengerUnten =
        "M52,54 A5,5 0 1 1 52,64 A5,5 0 1 1 52,54 Z " +
        "M47,65 L57,65 L62,82 L57,84 L54,74 L54,86 L61,96 L56,99 L50,88 L44,99 L39,96 L46,86 L46,74 L43,82 L38,80 Z";

    // ---------------------------------------------------------------- Pfeile und Verbote

    public const string PfeilGeradeausSchaft = "M50,84 L50,26";
    public const string PfeilGeradeausSpitze = "M32,34 L50,12 L68,34 Z";

    public const string PfeilNachRechtsSchaft = "M16,50 L74,50";
    public const string PfeilNachRechtsSpitze = "M66,32 L88,50 L66,68 Z";

    public const string PfeilNachLinksSchaft = "M84,50 L26,50";
    public const string PfeilNachLinksSpitze = "M34,32 L12,50 L34,68 Z";

    /// <summary>Abbiegepfeil nach rechts (VZ 209-20): erst geradeaus, dann rechts.</summary>
    public const string AbbiegenRechtsSchaft = "M40,84 L40,44 L70,44";
    public const string AbbiegenRechtsSpitze = "M64,28 L88,44 L64,60 Z";

    /// <summary>Drei Pfeile im Kreis (VZ 215 Kreisverkehr).</summary>
    public const string KreisverkehrPfeile =
        "M50,22 A28,28 0 0 1 74,64 M74,64 A28,28 0 0 1 26,64 M26,64 A28,28 0 0 1 50,22";

    public const string KreisverkehrSpitzen =
        "M68,58 L84,62 L72,74 Z M32,74 L20,62 L36,58 Z M44,26 L50,10 L58,26 Z";

    /// <summary>Wendepfeil (VZ 272 Verbot des Wendens).</summary>
    public const string WendenSchaft = "M66,80 L66,44 A16,16 0 0 0 34,44 L34,66";
    public const string WendenSpitze = "M20,60 L34,80 L48,60 Z";

    /// <summary>Der weiße Querbalken auf VZ 267 (Verbot der Einfahrt).</summary>
    public const string QuerbalkenBreit = "M14,42 L86,42 L86,58 L14,58 Z";

    /// <summary>Der rote Schrägstrich der Haltverbote (VZ 283/286) und Ende-Zeichen.</summary>
    public const string SchraegstrichAbwaerts = "M22,22 L78,78";

    /// <summary>Zwei gekreuzte Schrägstriche (VZ 283 Absolutes Haltverbot).</summary>
    public const string SchraegstrichKreuz = "M22,22 L78,78 M78,22 L22,78";

    /// <summary>Fünf graue Schrägstriche (VZ 282 Ende sämtlicher Streckenverbote).</summary>
    public const string EndeStreckenverbote =
        "M28,18 L18,32 M44,16 L26,44 M58,16 L34,56 M72,20 L44,70 M80,34 L56,80";

    /// <summary>Zebrastreifen mit Fußgänger (VZ 350 Fußgängerüberweg) - nur die Streifen; die
    /// Figur kommt aus <see cref="Fussgaenger"/>.</summary>
    public const string Zebrastreifen =
        "M20,74 L80,74 L80,80 L20,80 Z M24,84 L80,84 L80,90 L24,90 Z";

    // ---------------------------------------------------------------- Richtzeichen

    /// <summary>Vorfahrt-Kreuz mit verdicktem Längsbalken (VZ 301).</summary>
    public const string VorfahrtKreuz =
        "M44,34 L56,34 L56,58 L56,86 L44,86 L44,58 Z M24,52 L76,52 L76,60 L24,60 Z";

    /// <summary>Autobahn-Sinnbild (VZ 330.1): zwei Fahrbahnen mit Brücke.</summary>
    public const string Autobahn =
        "M18,86 L34,30 L44,30 L38,86 Z M82,86 L66,30 L56,30 L62,86 Z " +
        "M46,16 L54,16 L54,44 L46,44 Z M30,16 L70,16 L70,24 L30,24 Z";

    /// <summary>Sackgasse (VZ 357): Stichstraße mit Querbalken am Ende.</summary>
    public const string Sackgasse = "M46,90 L46,34 L54,34 L54,90 Z M26,22 L74,22 L74,34 L26,34 Z";

    // ---------------------------------------------------------------- Verkehrseinrichtungen

    /// <summary>Rot-weiße Schrägstreifen einer Bake/Absperrschranke.</summary>
    public const string BakenStreifen =
        "M10,70 L30,30 L48,30 L28,70 Z M52,70 L72,30 L90,30 L70,70 Z";

    /// <summary>Leitkegel ("Pylone", VZ 610).</summary>
    public const string Leitkegel = "M50,16 L70,80 L30,80 Z M18,80 L82,80 L82,90 L18,90 Z";

    /// <summary>Grünpfeil (VZ 720): Pfeil nach rechts auf schwarzem Grund.</summary>
    public const string GruenpfeilSchaft = "M20,50 L62,50";
    public const string GruenpfeilSpitze = "M56,28 L86,50 L56,72 Z";
}
