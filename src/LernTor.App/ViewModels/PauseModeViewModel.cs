using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Startbildschirm, solange der Ferien-/Pausenmodus läuft (siehe <see cref="PauseMode"/>).
///
/// <para>Realer Fehler, den dieser Bildschirm behebt: der Pausenmodus übersprang zwar die
/// technische Kiosk-Sperre, die Lernstrecke lief aber unverändert weiter - das Kind landete im
/// Vollbild auf dem Begrüßungsbildschirm und musste bis zum Abschlussquiz durch, um den PC
/// freizubekommen. Von außen war der Modus damit schlicht wirkungslos.</para>
///
/// <para>Zwei Wege, bewusst unterschiedlich gewichtet: <b>PC entsperren</b> beendet LernTor
/// sofort - das ist in den Ferien der Normalfall. <b>Trotzdem üben</b> führt in die gewohnte
/// Profilauswahl, denn freiwilliges Lernen soll die Pause nicht verbieten (so ist der Modus in
/// <c>AppSettings.PauseUntilDate</c> auch beschrieben).</para>
///
/// <para>Der Entsperren-Knopf existiert ausschließlich hier. Er hängt damit an derselben
/// Bedingung wie der Bildschirm selbst - läuft keine Pause, wird dieses ViewModel nie erzeugt und
/// es gibt keinen Knopf, der die tägliche Lernstrecke abkürzen könnte.</para>
/// </summary>
public sealed partial class PauseModeViewModel : ObservableObject
{
    private static readonly CultureInfo German = CultureInfo.GetCultureInfo("de-DE");

    private readonly Action _onUnlockPc;
    private readonly Action _onPracticeAnyway;

    public PauseModeViewModel(DateOnly pauseUntil, DateOnly today, Action onUnlockPc, Action onPracticeAnyway)
    {
        _onUnlockPc = onUnlockPc;
        _onPracticeAnyway = onPracticeAnyway;

        UntilDisplay = pauseUntil.ToDateTime(TimeOnly.MinValue).ToString("dddd, d. MMMM yyyy", German);
        RemainingDays = PauseMode.RemainingDays(pauseUntil, today);
    }

    /// <summary>Das Enddatum ausgeschrieben - Eltern sollen auf einen Blick sehen, ob das
    /// eingetragene Datum wirklich das gemeinte ist.</summary>
    public string UntilDisplay { get; }

    /// <summary>Verbleibende Pausentage einschließlich heute.</summary>
    public int RemainingDays { get; }

    public string RemainingDisplay => RemainingDays == 1
        ? LocalizationService.Instance["Pause_LastDay"]
        : string.Format(LocalizationService.Instance["Pause_RemainingDays"], RemainingDays);

    [RelayCommand]
    private void UnlockPc() => _onUnlockPc();

    [RelayCommand]
    private void PracticeAnyway() => _onPracticeAnyway();
}
