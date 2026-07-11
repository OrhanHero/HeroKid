using LernTor.News;
using Xunit;

namespace LernTor.Tests;

public class BerlinDistrictDetectorTests
{
    [Theory]
    [InlineData("Neue Schwimmhalle in Spandau eröffnet", "Spandau")]
    [InlineData("Streit um Radweg in Kreuzberg", "Friedrichshain-Kreuzberg")]
    [InlineData("Der Wedding bekommt einen neuen Park", "Mitte")]
    [InlineData("Baustelle in Köpenick sorgt für Staus", "Treptow-Köpenick")]
    [InlineData("Fest im Märkischen Viertel", null)] // dekliniert ("Märkischen") - bewusst kein Treffer
    [InlineData("Schule in Marzahn wird saniert", "Marzahn-Hellersdorf")]
    public void Detect_erkennt_Bezirke_und_Ortsteile(string text, string? expected)
    {
        Assert.Equal(expected, BerlinDistrictDetector.Detect(text));
    }

    [Fact]
    public void Detect_Mitte_nur_in_eindeutiger_Form()
    {
        // Das Alltagswort "Mitte" darf nicht anschlagen …
        Assert.Null(BerlinDistrictDetector.Detect("Der Ball liegt in der Mitte des Feldes."));
        // … die eindeutigen Formen schon.
        Assert.Equal("Mitte", BerlinDistrictDetector.Detect("Neues Museum in Berlin-Mitte geplant"));
        Assert.Equal("Mitte", BerlinDistrictDetector.Detect("Der Bezirk Mitte plant neue Spielplätze"));
    }

    [Fact]
    public void Detect_nimmt_den_zuerst_genannten_Ort()
    {
        Assert.Equal("Spandau", BerlinDistrictDetector.Detect("Von Spandau bis Neukölln: neue Buslinien"));
    }

    [Fact]
    public void Detect_ohne_Ortsbezug_liefert_null()
    {
        Assert.Null(BerlinDistrictDetector.Detect("Die Regierung plant ein neues Gesetz."));
        Assert.Null(BerlinDistrictDetector.Detect(null));
    }

    [Fact]
    public void Alle_zwoelf_Bezirke_sind_abgedeckt()
    {
        var districts = BerlinDistrictDetector.PlaceToDistrict.Values.Distinct().ToList();
        Assert.Equal(12, districts.Count);
    }
}
