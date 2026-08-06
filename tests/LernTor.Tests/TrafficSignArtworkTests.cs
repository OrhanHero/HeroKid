using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die aus der amtlichen Uebersicht gewonnenen Original-Zeichnungen.
///
/// <para>Aufgenommen wird nur, was maschinell bestaetigt ist: aeussere Kontur passend zur
/// erwarteten Grundform, erwartete Randfarbe vorhanden. Diese Tests halten fest, dass die
/// Auswahl nicht stillschweigend aufweicht - eine falsche Zuordnung faellt sonst nirgends auf,
/// das Schild rendert ja tadellos, es ist nur das falsche.</para>
/// </summary>
public sealed class TrafficSignArtworkTests
{
    [Fact]
    public void Es_gibt_Originalzeichnungen()
    {
        Assert.True(TrafficSignArtwork.Count >= 40,
            $"Nur {TrafficSignArtwork.Count} Zeichen liegen im Original vor.");
    }

    [Fact]
    public void Jede_Originalnummer_gehoert_zu_einem_Zeichen_im_Katalog()
    {
        // Eine Zeichnung ohne passenden Katalogeintrag waere tote Last - und ein Hinweis
        // darauf, dass eine Nummer umbenannt wurde, ohne die Zeichnung nachzuziehen.
        var verwaist = TrafficSignArtwork.Numbers
            .Where(number => TrafficSignCatalog.ByNumber(number) is null)
            .ToList();

        Assert.Empty(verwaist);
    }

    [Fact]
    public void Zeichen_mit_Originalzeichnung_melden_das_auch()
    {
        foreach (var number in TrafficSignArtwork.Numbers)
        {
            var sign = TrafficSignCatalog.ByNumber(number)!;

            Assert.True(sign.HasOriginalArtwork, $"Zeichen {number} traegt keine Originalzeichnung.");
            Assert.NotEmpty(sign.Artwork!);
        }
    }

    [Fact]
    public void Ohne_Originalzeichnung_bleibt_die_nachgezeichnete_Fassung_stehen()
    {
        // Das ist der Kern der Regel: lieber ein vereinfachtes RICHTIGES Schild als ein
        // originalgetreues falsches. Bei diesen Zeichen kam aus der Vorlage nachweislich das
        // falsche Bild (beim Wendeverbot etwa ein blaues statt eines roten Schildes).
        foreach (var number in new[] { "272", "276", "283", "314" })
        {
            var sign = TrafficSignCatalog.ByNumber(number)!;

            Assert.False(sign.HasOriginalArtwork,
                $"Zeichen {number} war als Fehlzuordnung aussortiert und ist wieder drin.");
            Assert.True(sign.PathData is not null || sign.Text is not null,
                $"Zeichen {number} hat weder Originalzeichnung noch nachgezeichnete Fassung.");
        }
    }

    [Fact]
    public void Jede_Ebene_hat_Pfad_und_Farbe()
    {
        foreach (var number in TrafficSignArtwork.Numbers)
        {
            foreach (var ebene in TrafficSignArtwork.For(number)!)
            {
                Assert.False(string.IsNullOrWhiteSpace(ebene.Path), $"Zeichen {number}: leerer Pfad.");
                Assert.Matches("^#[0-9A-Fa-f]{6}$", ebene.Color);
            }
        }
    }

    [Fact]
    public void Die_Zeichnungen_bleiben_im_Feld_von_0_bis_100()
    {
        // Alles rechnet im selben Feld wie die nachgezeichneten Piktogramme. Wer darueber
        // hinausragt, wird am Rand abgeschnitten - das faellt beim Lesen der Daten nicht auf.
        foreach (var number in TrafficSignArtwork.Numbers)
        {
            foreach (var ebene in TrafficSignArtwork.For(number)!)
            {
                foreach (var wert in Koordinaten(ebene.Path))
                {
                    Assert.InRange(wert, -2.0, 102.0);
                }
            }
        }
    }

    [Fact]
    public void Die_amtlichen_Verkehrsfarben_kommen_vor()
    {
        // Verkehrsrot und Verkehrsblau muessen im Bestand auftauchen - sonst waere beim
        // Auslesen der Vorlage die Farbinformation verlorengegangen.
        var farben = TrafficSignArtwork.Numbers
            .SelectMany(number => TrafficSignArtwork.For(number)!)
            .Select(ebene => ebene.Color.ToUpperInvariant())
            .ToHashSet();

        Assert.Contains("#E3000F", farben);   // RAL 3020 Verkehrsrot
        Assert.Contains("#005DAA", farben);   // RAL 5017 Verkehrsblau
    }

    [Fact]
    public void Zeichen_mit_Zahl_behalten_ihre_Aufschrift()
    {
        // Der Extraktor liest nur ZEICHENPFADE aus der Vorlage, keine Schrift. Bei diesen
        // Zeichen steht die Aussage aber in der Zahl: ohne sie waere VZ 274-50 ein leerer roter
        // Kreis und VZ 108-10 ein leeres Dreieck. Die Zeichenflaeche legt die Aufschrift
        // deshalb ueber die Originalzeichnung - und dieser Test haelt fest, dass sie da ist.
        foreach (var number in new[] { "108-10", "110-10", "274-50" })
        {
            var sign = TrafficSignCatalog.ByNumber(number)!;

            Assert.True(sign.HasOriginalArtwork, $"Zeichen {number} sollte im Original vorliegen.");
            Assert.False(string.IsNullOrWhiteSpace(sign.Text),
                $"Zeichen {number} hat seine Aufschrift verloren - es waere ein leeres Schild.");
        }
    }

    private static IEnumerable<double> Koordinaten(string path)
    {
        foreach (var stueck in path.Split(' ', 'M', 'L', 'Z', StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (var teil in stueck.Split(','))
            {
                if (double.TryParse(teil, System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out var wert))
                {
                    yield return wert;
                }
            }
        }
    }
}
