namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Fach-Zeile im Eltern-Bericht: Richtig-Quote als Balken plus "x von y richtig"-Text.
/// Unveränderlich - der Bericht wird bei Profil-/Zeitraumwechsel komplett neu aufgebaut.
/// </summary>
public sealed class SubjectReportRowViewModel
{
    public required string Label { get; init; }
    public required int Correct { get; init; }
    public required int Total { get; init; }

    /// <summary>Richtig-Quote 0..1 (Balkenwert).</summary>
    public double Rate => Total == 0 ? 0 : (double)Correct / Total;

    public string RateDisplay => $"{Rate:P0} ({Correct}/{Total})";

    /// <summary>Ampel-Schwellen für die Balkenfarbe (siehe DataTrigger im XAML).</summary>
    public bool IsStrong => Rate >= 0.75;
    public bool IsWeak => Rate < 0.5;
}
