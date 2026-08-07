using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Bilder zu den Weltwundern. Sie sind KI-generierte Rekonstruktionen - von sechs der sieben
/// Bauwerke weiß niemand, wie sie wirklich aussahen. Geprüft wird deshalb vor allem, dass die
/// Unterschrift IMMER dabei ist und dass kein Bild an der falschen Frage landet.
/// </summary>
public sealed class WeltwunderBilderTests
{
    private static IReadOnlyList<QuizQuestion> VieleFragen()
    {
        var generator = new GeschichteGenerator();
        var zufall = new Random(1234);

        return Enumerable.Range(0, 600)
            .SelectMany(_ => generator.Generate(GradeLevel.Klasse6, 1, zufall))
            .Where(frage => frage.Topic.Contains("Weltwunder"))
            .ToList();
    }

    [Fact]
    public void Kein_Bild_steht_ueber_der_Frage()
    {
        // Das ist der Punkt der Trennung: ein Bild ueber der Frage verraet die Antwort.
        // "Was war der Koloss von Rhodos?" mit einer Bronzestatue daneben ist keine Frage mehr.
        Assert.All(VieleFragen(), frage => Assert.Null(frage.ImageUrl));
    }

    [Fact]
    public void Jedes_Erklaerungsbild_hat_eine_Unterschrift()
    {
        // Ein fotorealistisches Bild ohne den Hinweis "KI-generierte Rekonstruktion" wuerde mehr
        // behaupten, als irgendjemand weiss - und genau das ist der Lehrstoff des Themas.
        Assert.All(VieleFragen(), frage =>
        {
            if (frage.ExplanationImageUrl is not null)
            {
                Assert.False(string.IsNullOrWhiteSpace(frage.ExplanationImageCaption));
            }
        });
    }

    [Theory]
    [InlineData("Was war der Koloss von Rhodos?", "Eine Bronzestatue", "Der Koloss stand am Hafen von Rhodos.")]
    [InlineData("Stand der Koloss von Rhodos über der Hafeneinfahrt?", "Nein", "Eine spätere Legende.")]
    public void Beim_Koloss_sagt_die_Unterschrift_dass_es_die_Legende_ist(
        string frage, string antwort, string erklaerung)
    {
        // Das Bild zeigt ihn mit gespreizten Beinen ueber der Hafeneinfahrt - was eine Frage
        // dieses Themas ausdruecklich verneint. Ohne diesen Hinweis wuerde die App das eine
        // sagen und das Gegenteil zeigen, und fuer ein Kind gewinnt das Bild.
        var (bild, unterschrift) = GeschichteGenerator.WeltwunderBildFuer(frage, antwort, erklaerung);

        Assert.Equal(WonderImages.KolossRhodos, bild);
        Assert.Equal(WonderImages.HinweisLegende, unterschrift);
        Assert.Contains("LEGENDE", unterschrift);
    }

    [Theory]
    // Die Optionen enthalten die anderen Weltwunder als Ablenker - danach darf NICHT gesucht
    // werden. Geprueft wird gegen Frage, Antwort und Erklaerung.
    [InlineData("Welches Weltwunder steht heute noch?", "Die Große Pyramide von Gizeh",
        "Von den sieben ist nur die Große Pyramide von Gizeh übrig.", "pyramide-gizeh")]
    [InlineData("Welches Weltwunder stand in Ephesos?", "Der Tempel der Artemis",
        "Der Artemis-Tempel bei Ephesos.", "artemis-tempel")]
    [InlineData("Wo stand das Mausoleum von Halikarnassos?", "In der Türkei",
        "Halikarnassos ist das heutige Bodrum.", "mausoleum")]
    [InlineData("Was ist nicht nachgewiesen?", "Die Hängenden Gärten von Babylon",
        "Für die Hängenden Gärten gibt es keinen Fund.", "haengende-gaerten")]
    [InlineData("Wofür diente der Pharos von Alexandria?", "Als Leuchtturm",
        "Der Pharos war sehr hoch.", "leuchtturm-alexandria")]
    [InlineData("Was zeigte die Zeusstatue von Olympia?", "Zeus auf einem Thron",
        "Phidias schuf sie um 435 v. Chr.", "zeusstatue")]
    public void Jede_Frage_bekommt_das_Bild_ihres_eigenen_Bauwerks(
        string frage, string antwort, string erklaerung, string erwarteteDatei)
    {
        var (bild, unterschrift) = GeschichteGenerator.WeltwunderBildFuer(frage, antwort, erklaerung);

        Assert.NotNull(bild);
        Assert.Contains(erwarteteDatei, bild);
        Assert.Equal(WonderImages.Hinweis, unterschrift);
    }

    [Theory]
    [InlineData("Wie viele Weltwunder zählt die Liste?", "Sieben", "Die Sieben galt als vollkommene Zahl.")]
    [InlineData("Wer wählte 2007 die neuen sieben aus?", "Eine private Stiftung",
        "Die UNESCO hatte damit nichts zu tun.")]
    public void Fragen_ohne_bestimmtes_Bauwerk_bekommen_kein_Bild(
        string frage, string antwort, string erklaerung)
    {
        // Ein beliebiges Bild danebenzustellen waere Dekoration, nicht Erklaerung.
        var (bild, unterschrift) = GeschichteGenerator.WeltwunderBildFuer(frage, antwort, erklaerung);

        Assert.Null(bild);
        Assert.Null(unterschrift);
    }

    [Fact]
    public void Alle_sieben_Bilder_werden_auch_wirklich_benutzt()
    {
        // Sonst liegt eine 2,4-MB-Datei in der Assembly, die nie jemand sieht - und ein
        // Weltwunder haette kein Bild, ohne dass es auffiele.
        var benutzt = VieleFragen()
            .Select(frage => frage.ExplanationImageUrl)
            .Where(bild => bild is not null)
            .Distinct()
            .ToList();

        var alle = new[]
        {
            WonderImages.PyramideGizeh, WonderImages.HaengendeGaerten, WonderImages.Zeusstatue,
            WonderImages.ArtemisTempel, WonderImages.Mausoleum, WonderImages.KolossRhodos,
            WonderImages.LeuchtturmAlexandria
        };

        Assert.All(alle, bild => Assert.Contains(bild, benutzt));
    }

    [Fact]
    public void Die_Adressen_nennen_die_Assembly_ausdruecklich()
    {
        // Die Kurzform pack://application:,,,/Pfad findet eine eingebettete Ressource nur, wenn
        // die AUSFUEHRENDE Assembly zugleich die ist, in der sie liegt - in den UI-Tests ist das
        // der Testhost. Genau daran sind hier schon einmal alle Verkehrszeichen unbemerkt auf den
        // Rueckfallpfad gerutscht, bei gruenen Tests (siehe CLAUDE.md).
        var alle = new[]
        {
            WonderImages.PyramideGizeh, WonderImages.HaengendeGaerten, WonderImages.Zeusstatue,
            WonderImages.ArtemisTempel, WonderImages.Mausoleum, WonderImages.KolossRhodos,
            WonderImages.LeuchtturmAlexandria
        };

        Assert.All(alle, bild => Assert.StartsWith("pack://application:,,,/LernTor;component/", bild));
        Assert.Equal(alle.Length, alle.Distinct().Count());
    }
}
