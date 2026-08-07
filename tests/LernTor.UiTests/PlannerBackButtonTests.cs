using LernTor.App.Localization;
using LernTor.App.ViewModels;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Der Planer-Zwischenstopp ist seit neuestem auch vom Geschafft-Bildschirm aus erreichbar
/// (Stundenplan, Hausaufgaben, Klausurtermine - genau dann will ein Kind wissen, was morgen
/// ansteht). Dort heisst der grosse Knopf aber NICHT "Zurueck zum Lernen": gelernt wird nichts
/// mehr, er fuehrt zurueck auf den Geschafft-Bildschirm. Die Beschriftung ist das Einzige, was
/// sich unterscheidet - deshalb wird sie hier festgehalten.
/// </summary>
public sealed class PlannerBackButtonTests
{
    private static WelcomeViewModel Build(bool plannerPeek, bool dayIsDone) =>
        new("Emirhan", currentStreak: 0, onContinue: () => { }, onSwitchLanguage: sprache => { },
            isPlannerPeek: plannerPeek, dayIsDone: dayIsDone);

    [Fact]
    public void Waehrend_des_Lerntags_fuehrt_der_Planer_zurueck_zum_Lernen()
    {
        var vm = Build(plannerPeek: true, dayIsDone: false);

        Assert.True(vm.ShowBackToLearning);
        Assert.False(vm.ShowBackToResult);
    }

    [Fact]
    public void Nach_dem_Abschlussquiz_fuehrt_der_Planer_zurueck_zum_Ergebnis()
    {
        var vm = Build(plannerPeek: true, dayIsDone: true);

        Assert.False(vm.ShowBackToLearning);
        Assert.True(vm.ShowBackToResult);
    }

    [Fact]
    public void Auf_der_Startseite_selbst_steht_ueberhaupt_kein_Rueckknopf()
    {
        // Dort ist der Planer ja kein Zwischenstopp, sondern die Seite selbst - der grosse Knopf
        // startet den Lerntag. Zwei Rueckknoepfe gleichzeitig darf es nie geben.
        var vm = Build(plannerPeek: false, dayIsDone: true);

        Assert.False(vm.ShowBackToLearning);
        Assert.False(vm.ShowBackToResult);
    }

    [Fact]
    public void Die_neue_Beschriftung_gibt_es_in_beiden_Sprachen()
    {
        // Ein fehlender Schluessel faellt nicht auf: der Indexer liefert dann "[Welcome_BackToResult]"
        // und genau das stuende dem Kind auf dem Knopf.
        var eintrag = Translations.Map["Welcome_BackToResult"];

        Assert.NotEmpty(eintrag[AppLanguage.Deutsch]);
        Assert.NotEmpty(eintrag[AppLanguage.Tuerkisch]);
    }
}
