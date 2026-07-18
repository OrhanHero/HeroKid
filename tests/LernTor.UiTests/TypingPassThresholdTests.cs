using LernTor.App.Services;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Bestehens-Logik des Tipptrainers: maßgeblich ist AUSSCHLIESSLICH die (pro Profil eingestellte)
/// Mindestgenauigkeit. Regressionstest für einen realen Bug: eine Zusatzbedingung
/// ("alle Zeichen korrekt") überschrieb die Eltern-Einstellung faktisch mit 100%.
/// </summary>
public sealed class TypingPassThresholdTests
{
    // CheckInput nutzt das Repository nicht - null! ist hier bewusst in Ordnung.
    private readonly TypingExerciseService _service = new(null!);

    [Fact]
    public void Fehlerhafte_Eingabe_ueber_der_Mindestgenauigkeit_besteht()
    {
        // 8 von 10 Zeichen richtig = 80% - muss bei 25%-Preset bestehen.
        var result = _service.CheckInput("qwertzuiop", "qwertzuiXX", TimeSpan.FromSeconds(30), minAccuracyOverride: 0.25);

        Assert.Equal(0.8, result.Accuracy, precision: 5);
        Assert.True(result.Passed);
    }

    [Fact]
    public void Eingabe_unter_der_Mindestgenauigkeit_besteht_nicht()
    {
        // 2 von 10 Zeichen richtig = 20% - unter dem 25%-Preset.
        var result = _service.CheckInput("qwertzuiop", "qwXXXXXXXX", TimeSpan.FromSeconds(30), minAccuracyOverride: 0.25);

        Assert.False(result.Passed);
    }

    [Fact]
    public void Preset_100_Prozent_verlangt_weiterhin_fehlerfreies_Tippen()
    {
        var perfekt = _service.CheckInput("asdf jklö", "asdf jklö", TimeSpan.FromSeconds(10), minAccuracyOverride: 1.0);
        var einFehler = _service.CheckInput("asdf jklö", "asdf jklX", TimeSpan.FromSeconds(10), minAccuracyOverride: 1.0);

        Assert.True(perfekt.Passed);
        Assert.False(einFehler.Passed);
    }

    [Fact]
    public void Zu_kurze_Eingabe_zaehlt_fehlende_Zeichen_als_Fehler()
    {
        // Nur die Hälfte getippt (alles richtig): Genauigkeit 50% - besteht bei 25%, nicht bei 75%.
        var result = _service.CheckInput("qwertzuiop", "qwert", TimeSpan.FromSeconds(30), minAccuracyOverride: 0.25);

        Assert.Equal(0.5, result.Accuracy, precision: 5);
        Assert.True(result.Passed);
        Assert.False(_service.CheckInput("qwertzuiop", "qwert", TimeSpan.FromSeconds(30), minAccuracyOverride: 0.75).Passed);
    }
}
