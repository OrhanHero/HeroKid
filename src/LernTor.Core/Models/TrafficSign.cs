using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Die amtlichen Farben der deutschen Verkehrszeichen (RAL-Verkehrsfarben, hier als Hex für die
/// Darstellung). Als Konstanten statt als lose Strings, damit ein Tippfehler im Katalog nicht
/// erst auf dem Bildschirm auffällt.
/// </summary>
public static class SignColors
{
    /// <summary>RAL 3020 Verkehrsrot.</summary>
    public const string Rot = "#C1121C";

    /// <summary>RAL 5017 Verkehrsblau.</summary>
    public const string Blau = "#004B93";

    /// <summary>RAL 1023 Verkehrsgelb.</summary>
    public const string Gelb = "#FFCC00";

    /// <summary>RAL 6024 Verkehrsgrün.</summary>
    public const string Gruen = "#007A33";

    public const string Weiss = "#FFFFFF";

    public const string Schwarz = "#1A1A1A";

    /// <summary>Für Zeichen ohne sichtbaren Rand (z.B. Zusatzzeichen auf weißem Grund).</summary>
    public const string Transparent = "#00000000";
}

/// <summary>
/// Eine Ebene einer Original-Zeichnung: ein Pfad, seine Farbe, gefüllt oder als Linie.
/// </summary>
/// <param name="Path">Geometriepfad im Feld 0..100.</param>
/// <param name="Color">Farbe aus der Vorlage (RAL-Verkehrsfarben).</param>
/// <param name="Filled">Gefüllt, sonst als dünne Linie gezeichnet.</param>
public sealed record SignArtworkLayer(string Path, string Color, bool Filled);

/// <summary>
/// Ein Verkehrszeichen als reine Daten - Aussehen, Bedeutung und ein Merksatz.
///
/// <para><b>Warum selbst gezeichnet statt Bilddateien:</b> LernTor ist vollständig offline, es
/// kann nichts nachladen. Die Zeichen selbst stehen in StVO Anlage 1-3 und sind als amtliches
/// Werk gemeinfrei (§ 5 UrhG) - ihre Form und Farbe darf jeder nachbauen. Layout und Grafiken
/// einer fremden Broschüre dagegen nicht, deshalb ist hier nichts übernommen, sondern jedes
/// Zeichen aus der Verordnungsbeschreibung als Geometrie beschrieben.</para>
///
/// <para><see cref="PathData"/> ist ein WPF-Geometriepfad in einem gedachten Feld von
/// 0..100 in beiden Richtungen. Die Zeichenfläche skaliert ihn auf ihre tatsächliche Größe,
/// damit dasselbe Zeichen als kleine Karteikarte und als großes Quizbild funktioniert.</para>
/// </summary>
public sealed record TrafficSign
{
    /// <summary>Amtliche Nummer nach StVO, z.B. "101" oder "274-50". Zugleich der Schlüssel für
    /// den Lernfortschritt - deshalb stabil halten.</summary>
    public required string Number { get; init; }

    /// <summary>Amtliche Bezeichnung, z.B. "Vorfahrt gewähren".</summary>
    public required string Name { get; init; }

    public required TrafficSignCategory Category { get; init; }

    public required SignShape Shape { get; init; }

    public string BorderColor { get; init; } = SignColors.Rot;

    public string FillColor { get; init; } = SignColors.Weiss;

    /// <summary>Aufschrift, wenn das Zeichen aus Text/Zahlen besteht ("50", "STOP", "3,8 m").</summary>
    public string? Text { get; init; }

    public string TextColor { get; init; } = SignColors.Schwarz;

    /// <summary>Piktogramm als Geometriepfad im 0..100-Feld; null, wenn das Zeichen nur aus
    /// Text besteht oder bewusst leer ist (VZ 250).</summary>
    public string? PathData { get; init; }

    public string PathColor { get; init; } = SignColors.Schwarz;

    /// <summary>0 = der Pfad wird gefüllt; &gt; 0 = er wird als Linie dieser Stärke gezeichnet.
    /// Pfeile, Kurven und Umrisse (Fahrrad, Ampel) sind als Linie mit zwei Kommandos beschrieben,
    /// wo eine Füllfläche ein Dutzend Stützpunkte bräuchte.</summary>
    public double PathStrokeThickness { get; init; }

    /// <summary>Ein zweiter Pfad in eigener Farbe und eigener Strichstärke - für Zeichen aus zwei
    /// Teilen: der rote Balken über dem blauen Grund beim Haltverbot, oder die gefüllte Pfeilspitze
    /// am Ende eines gestrichenen Pfeilschafts.</summary>
    public string? OverlayPathData { get; init; }

    public string OverlayColor { get; init; } = SignColors.Rot;

    public double OverlayStrokeThickness { get; init; }

    /// <summary>Was das Zeichen anordnet oder ankündigt - die Prüfungsantwort.</summary>
    public required string Meaning { get; init; }

    /// <summary>Merksatz/Eselsbrücke in Kindersprache. Erscheint erst NACH der Antwort, damit er
    /// beim Abfragen nicht die Lösung verrät.</summary>
    public required string Hint { get; init; }

    /// <summary>
    /// Die Original-Zeichnung aus der amtlichen Übersicht, falls vorhanden. Ist sie gesetzt,
    /// wird <b>nur</b> sie gezeichnet - Grundform, Aufschrift und Piktogramm bleiben außen vor,
    /// denn das Original bringt Rand, Fläche und Sinnbild bereits mit.
    ///
    /// <para>Nicht jedes Zeichen hat eine: aufgenommen wird nur, was maschinell bestätigt ist
    /// (siehe <see cref="TrafficSignArtwork"/>). Der Rest behält seine nachgezeichnete Fassung,
    /// lieber ein vereinfachtes richtiges Schild als ein originalgetreues falsches.</para>
    /// </summary>
    public IReadOnlyList<SignArtworkLayer>? Artwork { get; init; }

    /// <summary>Ob dieses Zeichen im Original vorliegt - für die Anzeige und für Tests.</summary>
    public bool HasOriginalArtwork => Artwork is { Count: > 0 };

    /// <summary>Gilt auch schon fürs Fahrrad - für die Kinder heute relevant, nicht erst mit 17.
    /// Steuert die Kennzeichnung 🚲 in der Übersicht.</summary>
    public bool RelevantForBicycle { get; init; }
}
