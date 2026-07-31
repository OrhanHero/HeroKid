using LernTor.Core.Models;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Rubriken Wissen und Sport. Die Erkennung arbeitet auf TEILZEICHENKETTEN - deshalb sind die
/// Fehltreffer-Tests hier wichtiger als die Treffer-Tests: ein zu kurzes Stichwort verschiebt
/// stillschweigend halbe Tagesausgaben in die falsche Rubrik.
///
/// <para>Eigene Klasse neben <c>NewsCategoryClassifierTests</c> (in KidNewsEnrichmentTests.cs) -
/// die bestehenden Faelle bleiben dort, hier steht nur, was mit den neuen Rubriken dazukam.</para>
/// </summary>
public sealed class NewsCategoryWissenSportTests
{
    private static NewsCategory Rubrik(string title, NewsCategory fallback = NewsCategory.Deutschland) =>
        NewsCategoryClassifier.Classify(title, summary: null, fallback);

    [Theory]
    [InlineData("Bundesliga: Hertha gewinnt gegen Union")]
    [InlineData("Weltmeisterschaft im Handball beginnt")]
    [InlineData("Olympische Spiele: Zeitplan steht")]
    [InlineData("Neuer Trainer für die Nationalmannschaft")]
    public void Sport_wird_erkannt(string title)
    {
        Assert.Equal(NewsCategory.Sport, Rubrik(title));
    }

    [Theory]
    [InlineData("Forschende entschlüsseln das Erbgut der Fledermaus")]
    [InlineData("Neues Teleskop blickt in eine ferne Galaxie")]
    [InlineData("Studie zur Klimaforschung veröffentlicht")]
    [InlineData("Max-Planck-Institut stellt Ergebnisse vor")]
    public void Wissen_wird_erkannt(string title)
    {
        Assert.Equal(NewsCategory.Wissen, Rubrik(title));
    }

    [Theory]
    // "ESA" steckt in "insgesamt" und "Gesamtschule", "EM-" in "System-Update" und
    // "Problem-Bewusstsein", "Genom" in "genommen". Alles alltaegliche deutsche Woerter -
    // genau diese drei Stichwoerter hatte ich zuerst drin.
    [InlineData("Der Bericht ist insgesamt sehr informativ")]
    [InlineData("Die Gesamtschule Neukölln feiert Jubiläum")]
    [InlineData("System-Update für Windows verfügbar")]
    [InlineData("Das Problem-Bewusstsein wächst")]
    [InlineData("Er hat den Ball genommen")]
    [InlineData("Die Abgeordneten haben abgenommen")]
    public void Alltagswoerter_landen_nicht_in_Wissen_oder_Sport(string title)
    {
        var rubrik = Rubrik(title);

        Assert.NotEqual(NewsCategory.Wissen, rubrik);
        Assert.NotEqual(NewsCategory.Sport, rubrik);
    }

    [Fact]
    public void Sport_hat_Vorrang_vor_Wissen()
    {
        // Die "Studie zur Belastung von Fussballprofis" gehoert dorthin, wo Kinder sie suchen.
        Assert.Equal(NewsCategory.Sport, Rubrik("Studie zur Belastung von Fußballprofis"));
    }

    [Fact]
    public void Wissen_hat_Vorrang_vor_Wetter()
    {
        // Sonst ginge Klimaforschung als Wetter durch.
        Assert.Equal(NewsCategory.Wissen, Rubrik("Klimaforschung: Studie zu Hitzewellen"));
    }

    [Fact]
    public void Spiele_bleiben_vor_KI()
    {
        // Bestehende Prioritaet - darf durch die neuen Rubriken nicht kippen.
        Assert.Equal(NewsCategory.Spiele, Rubrik("KI-Gegner in Mario Kart werden schlauer"));
    }

    [Fact]
    public void Ohne_Treffer_entscheidet_die_Quelle()
    {
        Assert.Equal(NewsCategory.Berlin, Rubrik("Neuer Spielplatz eröffnet", NewsCategory.Berlin));
        Assert.Equal(NewsCategory.Tuerkei, Rubrik("Neuer Spielplatz eröffnet", NewsCategory.Tuerkei));
    }

    [Fact]
    public void Jede_Rubrik_hat_ein_eigenes_Emoji()
    {
        // Die Kinder erkennen die Rubriken am Symbol - zwei gleiche waeren verwirrend.
        var emojis = System.Enum.GetValues<NewsCategory>()
            .Select(NewsCategoryClassifier.EmojiFor)
            .ToList();

        Assert.Equal(emojis.Count, emojis.Distinct().Count());
        Assert.DoesNotContain("📰", emojis);
    }

    [Fact]
    public void Neue_Rubriken_haben_ein_Emoji()
    {
        Assert.Equal("🔬", NewsCategoryClassifier.EmojiFor(NewsCategory.Wissen));
        Assert.Equal("⚽", NewsCategoryClassifier.EmojiFor(NewsCategory.Sport));
    }
}
