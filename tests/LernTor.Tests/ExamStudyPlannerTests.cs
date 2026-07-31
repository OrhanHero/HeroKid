using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Klausur-Lernplan: was heute drankommt - und was bewusst nicht erfunden wird.</summary>
public sealed class ExamStudyPlannerTests
{
    private static readonly DateOnly Heute = new(2026, 8, 5);

    private static ExamEntry Klausur(int inTagen, string themen = "Bruchrechnen, Prozentrechnung") => new()
    {
        ProfileId = "p1",
        Subject = Subject.Mathematik,
        Title = "Klassenarbeit 2",
        Topics = themen,
        ExamDate = Heute.AddDays(inTagen)
    };

    [Fact]
    public void Themen_werden_an_den_ueblichen_Trennzeichen_zerlegt()
    {
        Assert.Equal(
            new[] { "Bruchrechnen", "Prozentrechnung", "Dreisatz" },
            ExamStudyPlanner.ParseTopics("Bruchrechnen, Prozentrechnung; Dreisatz"));

        Assert.Equal(new[] { "Brüche", "Prozent" }, ExamStudyPlanner.ParseTopics("- Brüche\n- Prozent"));
    }

    [Fact]
    public void Leere_und_doppelte_Themen_fallen_weg()
    {
        Assert.Empty(ExamStudyPlanner.ParseTopics(null));
        Assert.Empty(ExamStudyPlanner.ParseTopics("   "));
        Assert.Empty(ExamStudyPlanner.ParseTopics(" , , "));

        // Gross-/Kleinschreibung soll kein zweites Thema erzeugen.
        Assert.Equal(new[] { "Brüche", "Prozent" }, ExamStudyPlanner.ParseTopics("Brüche, brüche, Prozent"));
    }

    [Fact]
    public void Ausserhalb_des_Vorlaufs_gibt_es_keinen_Plan()
    {
        // Vor dem Vorlauf gibt es nichts zu planen, danach keinen Anlass mehr.
        Assert.Empty(ExamStudyPlanner.BuildPlan(Klausur(ExamEntry.LearningBoostLeadDays + 1), Heute));
        Assert.Empty(ExamStudyPlanner.BuildPlan(Klausur(-1), Heute));
        Assert.Null(ExamStudyPlanner.Today(Klausur(20), Heute));
    }

    [Fact]
    public void Der_Plan_reicht_von_heute_bis_zum_Klausurtag()
    {
        var plan = ExamStudyPlanner.BuildPlan(Klausur(5), Heute);

        Assert.Equal(6, plan.Count);
        Assert.Equal(Heute, plan[0].Date);
        Assert.Equal(5, plan[0].DaysUntilExam);
        Assert.Equal(Heute.AddDays(5), plan[^1].Date);
        Assert.Equal(0, plan[^1].DaysUntilExam);
    }

    [Fact]
    public void Zwei_Themen_wechseln_sich_Tag_fuer_Tag_ab()
    {
        // Genau das ist der Sinn des Vorlaufs: jedes Thema kommt mehrfach dran, statt einmal.
        var tag7 = ExamStudyPlanner.Today(Klausur(7), Heute)!.Value;
        var tag6 = ExamStudyPlanner.Today(Klausur(6), Heute)!.Value;
        var tag5 = ExamStudyPlanner.Today(Klausur(5), Heute)!.Value;

        Assert.Equal(new[] { "Bruchrechnen" }, tag7.Topics);
        Assert.Equal(new[] { "Prozentrechnung" }, tag6.Topics);
        Assert.Equal(new[] { "Bruchrechnen" }, tag5.Topics);
    }

    [Fact]
    public void Derselbe_Tag_liefert_immer_dasselbe_Thema()
    {
        // Sonst stuende beim zweiten Oeffnen der App etwas anderes da.
        var klausur = Klausur(4);

        var erst = ExamStudyPlanner.Today(klausur, Heute)!.Value.Topics;
        var nochmal = ExamStudyPlanner.Today(klausur, Heute)!.Value.Topics;

        Assert.Equal(erst, nochmal);
    }

    [Fact]
    public void Der_Tag_vor_der_Klausur_ist_Wiederholung_ueber_alles()
    {
        // Am Vorabend ein neues Thema anzufangen hilft niemandem.
        var vortag = ExamStudyPlanner.Today(Klausur(1), Heute)!.Value;

        Assert.True(vortag.IsReviewDay);
        Assert.Equal(new[] { "Bruchrechnen", "Prozentrechnung" }, vortag.Topics);
    }

    [Fact]
    public void Viele_Themen_kommen_zu_zweit_dran_und_werden_alle_abgedeckt()
    {
        var klausur = Klausur(0, "A, B, C, D, E, F, G");

        var abgedeckt = Enumerable.Range(2, ExamEntry.LearningBoostLeadDays - 1)
            .SelectMany(tage => ExamStudyPlanner.Today(Klausur(tage, "A, B, C, D, E, F, G"), Heute)!.Value.Topics)
            .Distinct()
            .ToList();

        Assert.All(
            ExamStudyPlanner.ParseTopics(klausur.Topics),
            thema => Assert.Contains(thema, abgedeckt));
    }

    [Fact]
    public void Nie_mehr_als_zwei_Themen_an_einem_Tag()
    {
        // Eine lange Liste ist kein Plan, sondern nur die Themenliste noch einmal.
        var viele = string.Join(", ", Enumerable.Range(1, 20).Select(i => $"Thema{i}"));

        for (var tage = ExamEntry.LearningBoostLeadDays; tage >= 2; tage--)
        {
            var tag = ExamStudyPlanner.Today(Klausur(tage, viele), Heute)!.Value;
            Assert.True(tag.Topics.Count <= ExamStudyPlanner.MaxTopicsPerDay,
                $"{tage} Tage vorher standen {tag.Topics.Count} Themen da.");
        }
    }

    [Fact]
    public void Ohne_eingetragene_Themen_wird_nichts_erfunden()
    {
        // Ein ausgedachter Plan waere schlimmer als keiner - das Kind wuerde ihm glauben.
        var ohneThemen = Klausur(4, string.Empty);

        var tag = ExamStudyPlanner.Today(ohneThemen, Heute)!.Value;
        Assert.Empty(tag.Topics);
        Assert.False(tag.IsReviewDay);

        var hinweis = ExamStudyPlanner.TodayHint(ohneThemen, Heute, "Mathematik");
        Assert.Contains("mehr Mathematik-Aufgaben", hinweis);
    }

    [Fact]
    public void Der_Hinweis_nennt_das_heutige_Thema()
    {
        var hinweis = ExamStudyPlanner.TodayHint(Klausur(7), Heute, "Mathematik");

        Assert.Contains("Bruchrechnen", hinweis);
    }

    [Fact]
    public void Am_Klausurtag_steht_kein_Lernauftrag_mehr_da()
    {
        // Wer heute schreibt, soll nicht noch eine Aufgabenliste vorgesetzt bekommen.
        var hinweis = ExamStudyPlanner.TodayHint(Klausur(0), Heute, "Mathematik");

        Assert.Contains("Viel Erfolg", hinweis);
        Assert.DoesNotContain("Heute dran", hinweis);
    }

    [Fact]
    public void Am_Vortag_weist_der_Hinweis_aufs_Wiederholen_hin()
    {
        var hinweis = ExamStudyPlanner.TodayHint(Klausur(1), Heute, "Mathematik");

        Assert.Contains("Morgen", hinweis);
        Assert.Contains("Bruchrechnen", hinweis);
        Assert.Contains("Prozentrechnung", hinweis);
    }

    [Fact]
    public void Ohne_laufenden_Plan_bleibt_der_Hinweis_leer()
    {
        Assert.Equal(string.Empty, ExamStudyPlanner.TodayHint(Klausur(20), Heute, "Mathematik"));
        Assert.Equal(string.Empty, ExamStudyPlanner.TodayHint(Klausur(-2), Heute, "Mathematik"));
    }

    [Fact]
    public void Der_Plan_laeuft_genau_solange_wie_die_Lern_Gewichtung()
    {
        // Beides muss am selben Tag anspringen, sonst erklaert der Hinweis mehr Aufgaben, die es
        // noch gar nicht gibt - oder umgekehrt.
        for (var tage = 0; tage <= ExamEntry.LearningBoostLeadDays + 2; tage++)
        {
            var klausur = Klausur(tage);
            var hatPlan = ExamStudyPlanner.BuildPlan(klausur, Heute).Count > 0;
            var wirdGewichtet = klausur.LearningWeight(Heute) > 1.0;

            Assert.Equal(wirdGewichtet, hatPlan);
        }
    }
}
