using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class TopicMasteryCalculatorTests
{
    private static readonly DateTimeOffset Start = new(2026, 9, 1, 15, 0, 0, TimeSpan.FromHours(2));

    /// <summary>Antworten in zeitlicher Reihenfolge; jede bekommt einen eigenen Fragetext.</summary>
    private static List<MasteryAnswer> Antworten(
        string thema, params bool[] ergebnisse) =>
        ergebnisse
            .Select((richtig, i) => new MasteryAnswer(
                Subject.Mathematik, thema, $"{thema} Frage {i}", richtig, Start.AddHours(i)))
            .ToList();

    private static TopicMasteryStatus Einziges(IEnumerable<MasteryAnswer> antworten, params string[] wiederholtBestanden) =>
        Assert.Single(TopicMasteryCalculator.Calculate(antworten, wiederholtBestanden.ToHashSet()));

    [Fact]
    public void Unter_fuenf_Antworten_ist_ein_Thema_nur_angefangen()
    {
        var stand = Einziges(Antworten("Bruchrechnen", true, true, true, true));

        Assert.Equal(MasteryLevel.Angefangen, stand.Level);
        Assert.Equal(4, stand.Answered);
    }

    [Fact]
    public void Unter_siebzig_Prozent_ist_vertraut()
    {
        var stand = Einziges(Antworten("Bruchrechnen", true, false, true, false, true, false));

        Assert.Equal(MasteryLevel.Vertraut, stand.Level);
    }

    [Fact]
    public void Ab_siebzig_Prozent_ist_sicher()
    {
        var stand = Einziges(Antworten("Bruchrechnen", true, true, true, false, true, true, true, false, true, true));

        Assert.Equal(0.8, stand.RecentRate, 3);
        Assert.Equal(MasteryLevel.Sicher, stand.Level);
    }

    [Fact]
    public void Hundert_Prozent_ohne_bestandene_Wiederholung_ist_nur_sicher()
    {
        // Alles an einem Nachmittag richtig beweist, dass es frisch ist - nicht, dass es bleibt.
        var stand = Einziges(Antworten("Bruchrechnen", true, true, true, true, true, true));

        Assert.Equal(MasteryLevel.Sicher, stand.Level);
    }

    [Fact]
    public void Gemeistert_braucht_hohe_Quote_und_zwei_bestandene_Wiederholungen()
    {
        var antworten = Antworten("Bruchrechnen", true, true, true, true, true, true);

        Assert.Equal(MasteryLevel.Sicher, Einziges(antworten, "Bruchrechnen Frage 0").Level);
        Assert.Equal(MasteryLevel.Gemeistert, Einziges(antworten, "Bruchrechnen Frage 0", "Bruchrechnen Frage 3").Level);
    }

    [Fact]
    public void Nur_die_juengsten_zehn_Antworten_zaehlen()
    {
        // Zehn falsche im September, dann zehn richtige: wer es jetzt kann, soll das sehen.
        var ergebnisse = Enumerable.Repeat(false, 10).Concat(Enumerable.Repeat(true, 10)).ToArray();

        var stand = Einziges(Antworten("Bruchrechnen", ergebnisse));

        Assert.Equal(20, stand.Answered);
        Assert.Equal(10, stand.RecentAnswered);
        Assert.Equal(10, stand.RecentCorrect);
        Assert.Equal(MasteryLevel.Sicher, stand.Level);
    }

    [Fact]
    public void Stufe_geht_zurueck_wenn_zuletzt_viel_falsch_war()
    {
        var ergebnisse = Enumerable.Repeat(true, 10).Concat(Enumerable.Repeat(false, 6)).ToArray();

        Assert.Equal(MasteryLevel.Vertraut, Einziges(Antworten("Bruchrechnen", ergebnisse)).Level);
    }

    [Fact]
    public void Fehler_Kartei_Praefix_gehoert_zum_selben_Thema()
    {
        var antworten = Antworten("Bruchrechnen", true, true, true);
        antworten.Add(new MasteryAnswer(Subject.Mathematik, "🔁 Bruchrechnen", "x", true, Start.AddDays(2)));
        antworten.Add(new MasteryAnswer(Subject.Mathematik, "🔁 Bruchrechnen", "y", true, Start.AddDays(3)));

        var stand = Einziges(antworten);

        Assert.Equal("Bruchrechnen", stand.Topic);
        Assert.Equal(5, stand.Answered);
    }

    [Fact]
    public void News_Tippen_und_Fuehrerschein_zaehlen_nicht()
    {
        var antworten = new[]
        {
            new MasteryAnswer(Subject.News, "Politik", "a", true, Start),
            new MasteryAnswer(Subject.Tippen, "Grundreihe", "b", true, Start),
            new MasteryAnswer(Subject.Fuehrerschein, "Vorfahrt", "c", true, Start),
            new MasteryAnswer(Subject.ErsteHilfe, "Notruf", "d", true, Start),
        };

        var stand = Einziges(antworten);

        Assert.Equal(Subject.ErsteHilfe, stand.Subject);
    }

    [Fact]
    public void Gleicher_Themenname_in_zwei_Faechern_bleibt_getrennt()
    {
        var antworten = new[]
        {
            new MasteryAnswer(Subject.Deutsch, "Wortschatz", "a", true, Start),
            new MasteryAnswer(Subject.Englisch, "Wortschatz", "b", true, Start),
        };

        var themen = TopicMasteryCalculator.Calculate(antworten, new HashSet<string>());

        Assert.Equal(new[] { Subject.Deutsch, Subject.Englisch }, themen.Select(t => t.Subject).ToArray());
    }

    [Fact]
    public void Leere_Themen_werden_uebergangen_und_Zaehlung_je_Stufe_ist_vollstaendig()
    {
        var antworten = Antworten("Bruchrechnen", true, true, true, true, true);
        antworten.Add(new MasteryAnswer(Subject.Mathematik, "   ", "leer", true, Start));

        var themen = TopicMasteryCalculator.Calculate(antworten, new HashSet<string>());
        var zaehlung = TopicMasteryCalculator.CountByLevel(themen);

        Assert.Single(themen);
        Assert.Equal(4, zaehlung.Count);
        Assert.Equal(1, zaehlung[MasteryLevel.Sicher]);
        Assert.Equal(0, zaehlung[MasteryLevel.Gemeistert]);
    }
}
