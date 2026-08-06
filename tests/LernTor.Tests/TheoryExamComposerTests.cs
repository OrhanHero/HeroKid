using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Prüfungssimulation und Schwachstellen-Trainer.</summary>
public sealed class TheoryExamComposerTests
{
    private static readonly Random Zufall = new(4711);

    [Fact]
    public void Die_Pruefung_stellt_dreissig_verschiedene_Fragen()
    {
        var pruefung = TheoryExamComposer.ComposeExam(DrivingTheoryCatalog.All, Zufall);

        Assert.Equal(TheoryExamRules.QuestionCount, pruefung.Count);
        Assert.Equal(pruefung.Count, pruefung.Select(frage => frage.Id).Distinct().Count());
    }

    [Fact]
    public void Die_Pruefung_streut_ueber_alle_Sachgebiete()
    {
        // Ohne Streuung kämen an einem Tag zwölf Vorfahrt-Fragen und am nächsten keine - wer
        // nur einen Ausschnitt übt, hält sich für weiter, als er ist.
        var pruefung = TheoryExamComposer.ComposeExam(DrivingTheoryCatalog.All, Zufall);
        var themen = pruefung.Select(frage => frage.Topic).Distinct().ToList();

        Assert.Equal(DrivingTheoryCatalog.Topics.Count, themen.Count);
    }

    [Fact]
    public void Ein_zu_kleiner_Katalog_gibt_lieber_weniger_Fragen_als_Wiederholungen()
    {
        var klein = DrivingTheoryCatalog.All.Take(7).ToList();

        var pruefung = TheoryExamComposer.ComposeExam(klein, Zufall);

        Assert.Equal(7, pruefung.Count);
        Assert.Equal(7, pruefung.Select(frage => frage.Id).Distinct().Count());
    }

    [Fact]
    public void Ein_leerer_Katalog_gibt_eine_leere_Pruefung_statt_einer_Ausnahme()
    {
        Assert.Empty(TheoryExamComposer.ComposeExam(Array.Empty<TheoryQuestion>(), Zufall));
    }

    [Fact]
    public void Der_Trainer_zieht_zuerst_aus_schwachen_Sachgebieten()
    {
        var schwach = DrivingTheoryTopic.VorfahrtUndRegelung;
        var staerken = new[]
        {
            new TopicMastery(schwach, Answered: 5, Correct: 1),                       // 20 %
            new TopicMastery(DrivingTheoryTopic.UmweltUndSparsamkeit, 5, 5)           // 100 %
        };

        var satz = TheoryExamComposer.ComposeWeakSpotSet(DrivingTheoryCatalog.All, staerken, 4, Zufall);

        Assert.Equal(4, satz.Count);
        Assert.All(satz, frage => Assert.Equal(schwach, frage.Topic));
    }

    [Fact]
    public void Ohne_belegte_Schwachstellen_kommt_ein_gemischter_Satz()
    {
        // Zwei Antworten machen noch keine Schwachstelle - unter MinAnswersForMastery wird
        // nicht bewertet, sonst würde der Trainer sich Schwächen ausdenken.
        var zuWenig = new[] { new TopicMastery(DrivingTheoryTopic.VorfahrtUndRegelung, 2, 0) };

        Assert.False(zuWenig[0].IsMeaningful);
        Assert.False(zuWenig[0].IsWeak);

        var satz = TheoryExamComposer.ComposeWeakSpotSet(DrivingTheoryCatalog.All, zuWenig, 10, Zufall);

        Assert.Equal(10, satz.Count);
        Assert.True(satz.Select(frage => frage.Topic).Distinct().Count() > 1);
    }

    [Fact]
    public void Der_Trainer_fuellt_aus_dem_Rest_auf_wenn_das_schwache_Gebiet_zu_klein_ist()
    {
        var schwach = DrivingTheoryTopic.RuhenderVerkehr;          // nur wenige Fragen im Katalog
        var vorhanden = DrivingTheoryCatalog.ByTopic(schwach).Count;
        var staerken = new[] { new TopicMastery(schwach, 5, 0) };

        var satz = TheoryExamComposer.ComposeWeakSpotSet(
            DrivingTheoryCatalog.All, staerken, vorhanden + 3, Zufall);

        Assert.Equal(vorhanden + 3, satz.Count);
        Assert.Equal(vorhanden, satz.Count(frage => frage.Topic == schwach));
    }

    [Fact]
    public void Die_Trefferquote_wird_je_Sachgebiet_gezaehlt_schwaechstes_zuerst()
    {
        var antworten = new List<(DrivingTheoryTopic, bool)>
        {
            (DrivingTheoryTopic.VorfahrtUndRegelung, true),
            (DrivingTheoryTopic.VorfahrtUndRegelung, false),
            (DrivingTheoryTopic.VorfahrtUndRegelung, false),
            (DrivingTheoryTopic.VorfahrtUndRegelung, false),
            (DrivingTheoryTopic.UmweltUndSparsamkeit, true),
            (DrivingTheoryTopic.UmweltUndSparsamkeit, true)
        };

        var staerken = TheoryExamComposer.BuildMastery(antworten);

        Assert.Equal(2, staerken.Count);
        Assert.Equal(DrivingTheoryTopic.VorfahrtUndRegelung, staerken[0].Topic);
        Assert.Equal(25, staerken[0].Percent);
        Assert.True(staerken[0].IsWeak);

        Assert.Equal(100, staerken[1].Percent);
        // Zwei Antworten reichen nicht für ein Urteil - auch nicht für ein gutes.
        Assert.False(staerken[1].IsMeaningful);
        Assert.False(staerken[1].IsWeak);
    }

    [Fact]
    public void Sachgebiete_ohne_Antworten_tauchen_gar_nicht_auf()
    {
        // "0 %" bei null Versuchen wäre eine Aussage über nichts.
        var staerken = TheoryExamComposer.BuildMastery(
            new[] { (DrivingTheoryTopic.UnfallUndPanne, true) });

        Assert.Single(staerken);
        Assert.Equal(DrivingTheoryTopic.UnfallUndPanne, staerken[0].Topic);
    }
}
