using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;

namespace LernTor.App.ViewModels;

public sealed partial class WelcomeViewModel : ObservableObject
{
    private readonly Action _onContinue;
    private readonly Action<AppLanguage> _onSwitchLanguage;

    public WelcomeViewModel(Action onContinue, Action<AppLanguage> onSwitchLanguage)
    {
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
