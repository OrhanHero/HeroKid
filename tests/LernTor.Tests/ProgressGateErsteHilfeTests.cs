using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Einordnung des Erste-Hilfe-Bereichs in den Tagesablauf.
///
/// <para>Die Reihenfolge der bereits vorhandenen Bereiche darf sich dabei nicht verschieben -
/// deshalb prüft das hier nicht nur, dass die neue Etappe existiert, sondern auch, dass alles
/// andere in derselben Reihenfolge steht wie vorher.</para>
/// </summary>
public sealed class ProgressGateErsteHilfeTests
{
    private readonly ProgressGateService _gate = new();

    [Fact]
    public void Erste_Hilfe_liegt_zwischen_KI_Bereich_und_Fuehrerschein()
    {
        Assert.Equal(LearningStage.ErsteHilfe, _gate.GetNextStage(LearningStage.KiWissen));
        Assert.Equal(LearningStage.Fuehrerschein, _gate.GetNextStage(LearningStage.ErsteHilfe));
    }

    [Fact]
    public void Die_Reihenfolge_der_vorhandenen_Bereiche_bleibt_unveraendert()
    {
        // Die Kette einmal komplett durchlaufen und mit der erwarteten Abfolge vergleichen.
        var erwartet = new[]
        {
            LearningStage.Vorlesen, LearningStage.Tippen, LearningStage.News,
            LearningStage.Mathematik, LearningStage.Deutsch, LearningStage.Tuerkisch,
            LearningStage.Englisch, LearningStage.Biologie, LearningStage.Chemie,
            LearningStage.Physik, LearningStage.Geschichte, LearningStage.Gewi,
            LearningStage.Politik, LearningStage.Geo, LearningStage.Ethik,
            LearningStage.Kunst, LearningStage.Musik, LearningStage.Itg,
            LearningStage.KiWissen, LearningStage.ErsteHilfe, LearningStage.Fuehrerschein,
            LearningStage.Abschlussquiz, LearningStage.Freigeschaltet
        };

        var tatsaechlich = new List<LearningStage>();
        var stufe = LearningStage.Willkommen;

        for (var i = 0; i < erwartet.Length; i++)
        {
            stufe = _gate.GetNextStage(stufe);
            tatsaechlich.Add(stufe);
        }

        Assert.Equal(erwartet, tatsaechlich);
    }

    [Fact]
    public void Die_Enum_Reihenfolge_passt_zur_Ablaufreihenfolge()
    {
        // SessionSteps vergleicht Stufen ORDINAL (stage > LearningStage.Vorlesen), um die
        // Etappenleiste oben zu faerben. Liefe die Enum-Reihenfolge aus dem Tritt, waeren dort
        // Etappen als erledigt markiert, die noch offen sind - ohne dass irgendetwas abstuerzt.
        Assert.True(LearningStage.KiWissen < LearningStage.ErsteHilfe);
        Assert.True(LearningStage.ErsteHilfe < LearningStage.Fuehrerschein);
        Assert.True(LearningStage.Fuehrerschein < LearningStage.Abschlussquiz);
    }

    [Fact]
    public void Die_Etappe_gehoert_zum_Fach_ErsteHilfe()
    {
        Assert.True(LearningStageSubjects.TryGetSubject(LearningStage.ErsteHilfe, out var fach));
        Assert.Equal(Subject.ErsteHilfe, fach);
    }

    [Fact]
    public void Abgeschaltet_darf_die_Etappe_uebersprungen_werden()
    {
        // Ausdruecklich von der VORgaengeretappe aus geprueft. Von Willkommen aus waere das
        // Ergebnis wertlos: dann liegen noch alle anderen Etappen dazwischen, und der Test
        // wuerde bestehen, ohne ueber Erste Hilfe irgendetwas auszusagen.
        var progress = new LernTor.Core.Models.StudentProgress
        {
            ProfileId = "p1",
            CurrentStage = LearningStage.KiWissen
        };

        Assert.True(_gate.CanEnterStage(
            progress, LearningStage.Fuehrerschein, new HashSet<Subject> { Subject.ErsteHilfe }));
    }

    [Fact]
    public void Eingeschaltet_und_unerledigt_blockiert_die_Etappe_den_Weg()
    {
        var progress = new LernTor.Core.Models.StudentProgress
        {
            ProfileId = "p1",
            CurrentStage = LearningStage.KiWissen
        };

        Assert.False(_gate.CanEnterStage(
            progress, LearningStage.Fuehrerschein, new HashSet<Subject>()));
    }
}
