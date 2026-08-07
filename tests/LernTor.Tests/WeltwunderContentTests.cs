using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Weltwunder - als Geschichte-Thema und als Lesetexte. Kein eigener Bereich: die Liste ist
/// Allgemeinwissen und gehört zur Antike, nicht in eine neue Etappe. Geprüft wird vor allem,
/// dass sie in ALLEN Klassenstufen ankommt und dass die beiden Fakten stimmen, die für diese
/// Familie besonders zählen.
/// </summary>
public sealed class WeltwunderContentTests
{
    private static IReadOnlyList<string> PromptsFuer(GradeLevel stufe)
    {
        var generator = new GeschichteGenerator();
        var zufall = new Random(4711);

        // Grosszuegig ziehen, damit das Thema sicher vorkommt - Themen werden zufaellig gewaehlt.
        return Enumerable.Range(0, 400)
            .SelectMany(_ => generator.Generate(stufe, 1, zufall))
            .Select(frage => frage.Topic)
            .ToList();
    }

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse7)]
    [InlineData(GradeLevel.Klasse9)]
    public void Das_Thema_kommt_in_jeder_Klassenstufe_vor(GradeLevel stufe)
    {
        // Bewusst in allen drei Stufen: die Liste ist mit zwoelf dieselbe wie mit fuenfzehn.
        Assert.Contains(PromptsFuer(stufe), thema => thema.Contains("Weltwunder"));
    }

    [Fact]
    public void Es_gibt_zwei_Lesetexte_zu_den_Weltwundern()
    {
        var titel = ReadingContentProvider.GetAllBuiltIn().Select(t => t.Title).ToList();

        Assert.Contains(titel, t => t.Contains("Weltwunder der Antike"));
        Assert.Contains(titel, t => t.Contains("neuen sieben Weltwunder"));
    }

    [Fact]
    public void Die_Lesetexte_gibt_es_in_allen_drei_Sprachen()
    {
        // Wie jeder andere Lesetext auch - sonst steht ein Kind mit tuerkischer Oberflaeche vor
        // einem deutschen Text.
        var texte = ReadingContentProvider.GetAllBuiltIn().Where(t => t.Title.Contains("Weltwunder")).ToList();

        Assert.Equal(2, texte.Count);
        Assert.All(texte, text =>
        {
            Assert.False(string.IsNullOrWhiteSpace(text.TextDe));
            Assert.False(string.IsNullOrWhiteSpace(text.TextEn));
            Assert.False(string.IsNullOrWhiteSpace(text.TextTr));
        });
    }

    [Fact]
    public void Der_Tuerkei_Bezug_steht_wirklich_drin()
    {
        // Zwei der sieben antiken Weltwunder standen in der heutigen Tuerkei - Ephesos und
        // Halikarnassos, das heutige Bodrum. Fuer diese beiden Kinder ist das der interessanteste
        // Satz des ganzen Themas, und er darf bei einer Ueberarbeitung nicht stillschweigend
        // herausfallen.
        var antike = ReadingContentProvider.GetAllBuiltIn().Single(t => t.Title.Contains("Weltwunder der Antike"));

        Assert.Contains("Ephesos", antike.TextDe);
        Assert.Contains("Bodrum", antike.TextDe);
        Assert.Contains("Bodrum", antike.TextTr);
        Assert.Contains("Bodrum", antike.TextEn);
    }

    [Fact]
    public void Die_Abstimmung_von_2007_wird_nicht_als_UNESCO_Auswahl_dargestellt()
    {
        // Der wichtigste Punkt am zweiten Text: die "neuen sieben Weltwunder" kamen aus einer
        // privaten Abstimmung, nicht von der UNESCO - die hat sich ausdruecklich distanziert.
        // Das falsch darzustellen waere schlimmer, als das Thema gar nicht zu haben.
        var neue = ReadingContentProvider.GetAllBuiltIn().Single(t => t.Title.Contains("neuen sieben Weltwunder"));

        Assert.Contains("UNESCO", neue.TextDe);
        Assert.Contains("privat", neue.TextDe, StringComparison.OrdinalIgnoreCase);
    }
}
