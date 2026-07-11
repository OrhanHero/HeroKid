namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Makro-Etappe der heutigen Lernsession für die Fortschrittsleiste oben im Kiosk-Fenster
/// (Lesen → News → Fächer → Quiz). Unveränderlich - die Leiste wird bei jedem Stufenwechsel komplett
/// neu aufgebaut (siehe MainViewModel.UpdateSessionSteps).
/// </summary>
public sealed class SessionStepViewModel
{
    public required string Label { get; init; }
    public bool IsDone { get; init; }
    public bool IsCurrent { get; init; }

    /// <summary>Position des Eintrags (nur von den klickbaren Artikel-Markern im News-Bereich
    /// genutzt, um per Klick zum Artikel zu springen; die Makro-Etappen ignorieren das Feld).</summary>
    public int Index { get; init; }
}
