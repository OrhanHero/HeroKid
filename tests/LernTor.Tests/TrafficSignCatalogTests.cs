using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der Verkehrszeichen-Katalog. Ein Zeichen ohne Bild, ohne Bedeutung oder mit doppelter Nummer
/// faellt im Betrieb erst auf, wenn ein Kind davorsitzt - hier faellt es sofort auf.
/// </summary>
public sealed class TrafficSignCatalogTests
{
    [Fact]
    public void Der_Katalog_deckt_alle_fuenf_Gruppen_ab()
    {
        foreach (var category in Enum.GetValues<TrafficSignCategory>())
        {
            Assert.NotEmpty(TrafficSignCatalog.ByCategory(category));
        }
    }

    [Fact]
    public void Zeichennummern_sind_eindeutig()
    {
        // Die Nummer ist der Schluessel des Lernstands - zwei Zeichen mit derselben Nummer
        // wuerden sich gegenseitig als "gekonnt" markieren.
        var doppelte = TrafficSignCatalog.All
            .GroupBy(sign => sign.Number)
            .Where(gruppe => gruppe.Count() > 1)
            .Select(gruppe => gruppe.Key)
            .ToList();

        Assert.Empty(doppelte);
    }

    [Fact]
    public void Zeichennamen_sind_eindeutig()
    {
        // Namen sind die Antwortmoeglichkeiten im Quiz. Zwei gleiche Namen hiessen: zwei
        // richtige Antworten, von denen eine als falsch gewertet wuerde.
        var doppelte = TrafficSignCatalog.All
            .GroupBy(sign => sign.Name, StringComparer.Ordinal)
            .Where(gruppe => gruppe.Count() > 1)
            .Select(gruppe => gruppe.Key)
            .ToList();

        Assert.Empty(doppelte);
    }

    [Fact]
    public void Jedes_Zeichen_hat_Bedeutung_und_Merksatz()
    {
        Assert.All(TrafficSignCatalog.All, sign =>
        {
            Assert.False(string.IsNullOrWhiteSpace(sign.Number));
            Assert.False(string.IsNullOrWhiteSpace(sign.Name));
            Assert.False(string.IsNullOrWhiteSpace(sign.Meaning));
            Assert.False(string.IsNullOrWhiteSpace(sign.Hint));
        });
    }

    [Fact]
    public void Jedes_Zeichen_ist_erkennbar()
    {
        // Entweder Piktogramm oder Aufschrift - sonst waere es eine leere Flaeche. Einzige
        // gewollte Ausnahme: VZ 250, dessen Aussage GERADE der leere rote Kreis ist.
        var leer = TrafficSignCatalog.All
            .Where(sign => string.IsNullOrWhiteSpace(sign.PathData) && string.IsNullOrWhiteSpace(sign.Text))
            .Select(sign => sign.Number)
            .ToList();

        Assert.Equal(new[] { "250" }, leer);
    }

    [Fact]
    public void Ein_gestrichener_Pfad_hat_auch_eine_Strichstaerke()
    {
        // Strichstaerke 0 heisst "fuellen". Ein Pfad, der nur aus Linien besteht (Pfeil, Kurve),
        // waere gefuellt ein schwarzer Klecks - das faellt beim Lesen des Katalogs nicht auf.
        var linienPfade = TrafficSignCatalog.All
            .Where(sign => sign.PathData is not null
                           && !sign.PathData.Contains('Z', StringComparison.OrdinalIgnoreCase)
                           && sign.PathStrokeThickness <= 0)
            .Select(sign => $"{sign.Number} {sign.Name}")
            .ToList();

        Assert.Empty(linienPfade);
    }

    [Fact]
    public void Farben_sind_gueltige_Hex_Angaben()
    {
        Assert.All(TrafficSignCatalog.All, sign =>
        {
            AssertHex(sign.BorderColor, sign.Number);
            AssertHex(sign.FillColor, sign.Number);
            AssertHex(sign.PathColor, sign.Number);
            AssertHex(sign.TextColor, sign.Number);
            AssertHex(sign.OverlayColor, sign.Number);
        });
    }

    [Fact]
    public void Fahrrad_relevante_Zeichen_sind_gekennzeichnet()
    {
        // Fuer die Kinder heute der eigentliche Nutzen - ein Bereich, in dem KEIN Zeichen fuers
        // Fahrrad gilt, waere reine Vorbereitung auf uebermorgen.
        var fuersRad = TrafficSignCatalog.ForBicycle();

        Assert.True(fuersRad.Count >= 30, $"Nur {fuersRad.Count} Zeichen sind als fahrradrelevant markiert.");
        Assert.Contains(fuersRad, sign => sign.Number == "205");
        Assert.Contains(fuersRad, sign => sign.Number == "206");
    }

    [Fact]
    public void Jede_Gruppe_hat_Bezeichnung_und_Erklaerung()
    {
        foreach (var category in TrafficSignCatalog.LearningOrder)
        {
            Assert.False(string.IsNullOrWhiteSpace(TrafficSignCatalog.CategoryLabel(category)));
            Assert.False(string.IsNullOrWhiteSpace(TrafficSignCatalog.CategoryDescription(category)));
        }
    }

    [Fact]
    public void Die_Lernreihenfolge_beginnt_bei_den_Gefahrzeichen()
    {
        // Sie warnen nur und sind am leichtesten zu merken - der richtige Einstieg.
        Assert.Equal(TrafficSignCategory.Gefahrzeichen, TrafficSignCatalog.LearningOrder[0]);
        Assert.Equal(TrafficSignCategory.Zusatzzeichen, TrafficSignCatalog.LearningOrder[^1]);
    }

    [Fact]
    public void Zeichen_lassen_sich_ueber_ihre_Nummer_finden()
    {
        var stopSchild = TrafficSignCatalog.ByNumber("206");

        Assert.NotNull(stopSchild);
        Assert.Equal(SignShape.Achteck, stopSchild!.Shape);
        Assert.Null(TrafficSignCatalog.ByNumber("gibt-es-nicht"));
    }

    private static void AssertHex(string color, string signNumber)
    {
        Assert.True(
            color.StartsWith('#') && (color.Length == 7 || color.Length == 9),
            $"Zeichen {signNumber}: '{color}' ist keine Hex-Farbe (#RRGGBB oder #AARRGGBB).");
    }
}
