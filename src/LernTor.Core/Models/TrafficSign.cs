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
/// Ein Verkehrszeichen als reine Daten - Aussehen, Bedeutung und ein Merksatz.
///
/// <para><b>Aussehen:</b> jedes Zeichen liegt als echte Bilddatei vor (PNG aus Wikimedia
/// Commons/vzkat.de, gemeinfrei als amtliches Werk nach § 5 UrhG - siehe
/// <c>docs/FUEHRERSCHEIN.md</c>), eingebunden über die Katalognummer als Dateiname
/// (<c>LernTor.App.Controls.TrafficSignImages</c>). <see cref="PathData"/>/<see cref="Text"/>
/// bleiben als nachgezeichneter Rückfallpfad stehen, falls für eine Nummer je keine Bilddatei
/// mitgeliefert würde - die Zeichenfläche skaliert beides auf ihre tatsächliche Größe, damit
/// dasselbe Zeichen als kleine Karteikarte und als großes Quizbild funktioniert.</para>
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

    /// <summary>Gilt auch schon fürs Fahrrad - für die Kinder heute relevant, nicht erst mit 17.
    /// Steuert die Kennzeichnung 🚲 in der Übersicht.</summary>
    public bool RelevantForBicycle { get; init; }
}
