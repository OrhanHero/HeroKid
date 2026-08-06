namespace LernTor.Core.Enums;

/// <summary>
/// Die fünf Gruppen der Verkehrszeichen nach StVO Anlage 1-3. Die Reihenfolge ist zugleich die
/// empfohlene Lernreihenfolge: Gefahrzeichen zuerst (sie warnen nur und sind am leichtesten zu
/// merken), Zusatzzeichen zuletzt (sie ergeben ohne das Hauptzeichen darüber keinen Sinn).
///
/// <para>Neue Werte gehören ans ENDE - die Auswahl wird als String persistiert
/// (<c>JsonOptions.Default</c>), aber Reihenfolge = Lernreihenfolge, und die soll stabil
/// bleiben.</para>
/// </summary>
public enum TrafficSignCategory
{
    /// <summary>Dreieck mit roter Umrandung: warnt vor einer Gefahr, ordnet nichts an.</summary>
    Gefahrzeichen,

    /// <summary>Rund (Verbot/Gebot) sowie Vorfahrt-Zeichen: ordnet an, ist zu befolgen.</summary>
    Vorschriftzeichen,

    /// <summary>Meist blaue Rechtecke: gibt Hinweise und regelt den Verkehrsablauf.</summary>
    Richtzeichen,

    /// <summary>Baken, Schranken, Leitkegel - sichern und leiten an Baustellen und Gefahrstellen.</summary>
    Verkehrseinrichtung,

    /// <summary>Kleine weiße Schilder unter einem Hauptzeichen: schränken es ein oder erweitern es.</summary>
    Zusatzzeichen
}

/// <summary>Grundform eines Verkehrszeichens - bestimmt, wie es gezeichnet wird.</summary>
public enum SignShape
{
    /// <summary>Gefahrzeichen und VZ 301.</summary>
    DreieckSpitzeOben,

    /// <summary>Nur VZ 205 "Vorfahrt gewähren" - die einzigartige Form ist Absicht: auch von
    /// hinten und bei Schnee erkennbar.</summary>
    DreieckSpitzeUnten,

    /// <summary>Verbots- und Gebotszeichen.</summary>
    Kreis,

    /// <summary>Nur VZ 206 "Halt. Vorfahrt gewähren" - ebenfalls bewusst unverwechselbar.</summary>
    Achteck,

    /// <summary>Auf der Spitze stehendes Quadrat: Vorfahrtstraße (VZ 306/307).</summary>
    Raute,

    /// <summary>Liegendes Rechteck: die meisten Richt- und Zusatzzeichen.</summary>
    Rechteck,

    /// <summary>Stehendes Rechteck bzw. Quadrat.</summary>
    Quadrat
}
