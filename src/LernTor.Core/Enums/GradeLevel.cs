namespace LernTor.Core.Enums;

/// <summary>
/// Klassenstufe eines Profils. Die Werte sind explizit gesetzt, weil sie numerisch in SQLite
/// persistiert werden - so bleibt das Ergänzen weiterer Stufen gefahrlos.
///
/// <para><b>Doppeljahrgänge:</b> Der Berliner Rahmenlehrplan ist in Doppeljahrgangsstufen
/// gegliedert (7/8 und 9/10). Die kuratierten Aufgabenpools folgen dieser Gliederung: Klasse 7
/// deckt inhaltlich 7/8 ab, Klasse 9 deckt 9/10 ab. Für Klasse 8 und 10 gibt es deshalb bewusst
/// keine eigenen Pools - die Übergangsregel in <c>ExerciseGeneratorBase.Generate</c> greift auf
/// die nächstniedrigere vorhandene Stufe zu und trifft damit automatisch den richtigen
/// Doppeljahrgang. Eltern können so die tatsächliche Klasse eintragen, statt auf- oder
/// abzurunden.</para>
/// </summary>
public enum GradeLevel
{
    Klasse6 = 6,

    /// <summary>Klasse 7 - eigener Themenpool, deckt den Doppeljahrgang 7/8 inhaltlich ab.</summary>
    Klasse7 = 7,

    /// <summary>Klasse 8 - nutzt über die Übergangsregel die Klasse-7-Pools (Doppeljahrgang 7/8).</summary>
    Klasse8 = 8,

    /// <summary>Klasse 9 - eigener Themenpool, deckt den Doppeljahrgang 9/10 inhaltlich ab.</summary>
    Klasse9 = 9,

    /// <summary>Klasse 10 - nutzt über die Übergangsregel die Klasse-9-Pools (Doppeljahrgang 9/10),
    /// also den Stoff, der auch zum MSA führt.</summary>
    Klasse10 = 10
}
