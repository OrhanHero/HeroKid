using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;

namespace LernTor.App.ViewModels;

public sealed partial class WelcomeViewModel : ObservableObject
{
    private readonly Action _onContinue;
    private readonly Action<AppLanguage> _onSwitchLanguage;

    public string ProfileName { get; }

    /// <summary>Aufeinanderfolgende Lerntage (0 = Anzeige aus, siehe StreakCalculator).
    /// MainViewModel übergibt 0, wenn Eltern die Streak-Anzeige nicht eingeschaltet haben.</summary>
    public int CurrentStreak { get; }

    /// <summary>Erst ab 2 Tagen anzeigen - "🔥 1 Tag in Folge" wäre keine Serie.</summary>
    public bool ShowStreak => CurrentStreak >= 2;

    public WelcomeViewModel(string profileName, int currentStreak, Action onContinue, Action<AppLanguage> onSwitchLanguage)
    {
        ProfileName = profileName;
        CurrentStreak = currentStreak;
        _onContinue = onContinue;
        _onSwitchLanguage = onSwitchLanguage;
    }

    [RelayCommand]
    private void Continue() => _onContinue();

    [RelayCommand]
    private void ToggleLanguage()
    {
        var next = Localization.LocalizationService.Instance.CurrentLanguage == AppLanguage.Deutsch
            ? AppLanguage.Tuerkisch
            : AppLanguage.Deutsch;
        _onSwitchLanguage(next);
    }
}
