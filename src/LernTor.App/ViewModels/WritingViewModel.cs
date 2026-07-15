using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Pflicht-Schreibabschnitt: zeigt täglich einen Story-Stub (Mix DE/TR/EN), den das Kind 5 Minuten lang fortschreiben soll.
/// Es gibt bewusst KEINE Überspringen-Funktion – stattdessen läuft ein 5-Minuten-Timer herunter, erst danach wird "Weiter" nutzbar.
/// </summary>
public sealed partial class WritingViewModel : ObservableObject
{
    private static readonly TimeSpan MinimumDuration = TimeSpan.FromMinutes(5);

    /// <summary>Tab-Kennungen – als Konstanten statt Enum, da sie direkt aus XAML als CommandParameter-Strings kommen.</summary>
    public const string TabDe = "De";
    public const string TabTr = "Tr";
    public const string TabEn = "En";

    private readonly Action _onCompleted;
    private readonly DispatcherTimer _timer;

    public WritingPrompt Prompt { get; }

    [ObservableProperty]
    private TimeSpan remainingTime = MinimumDuration;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool canContinue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPromptText))]
    private string selectedTab = TabDe;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WordCount))]
    [NotifyPropertyChangedFor(nameof(CharCount))]
    private string writtenText = string.Empty;

    public string CurrentPromptText => SelectedTab switch
    {
        TabTr => Prompt.PromptTr,
        TabEn => Prompt.PromptEn,
        _ => Prompt.PromptDe
    };

    public string RemainingTimeDisplay => $"{(int)RemainingTime.TotalMinutes}:{RemainingTime.Seconds:00}";

    public int WordCount => string.IsNullOrWhiteSpace(WrittenText) ? 0 : WrittenText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

    public int CharCount => WrittenText?.Length ?? 0;

    public WritingViewModel(WritingPrompt prompt, Action onCompleted)
    {
        Prompt = prompt;
        _onCompleted = onCompleted;

        _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();

        // Initialen Tab explizit setzen, damit CurrentPromptText PropertyChanged feuert
        SelectedTab = TabDe;
    }

    private void Tick()
    {
        var next = RemainingTime - TimeSpan.FromSeconds(1);
        RemainingTime = next <= TimeSpan.Zero ? TimeSpan.Zero : next;

        if (RemainingTime <= TimeSpan.Zero)
        {
            _timer.Stop();
            CanContinue = true;
        }
    }

    [RelayCommand]
    private void SelectTab(string tab) => SelectedTab = tab;

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Continue()
    {
        _timer.Stop();
        _onCompleted();
    }

    partial void OnWrittenTextChanged(string value)
    {
        OnPropertyChanged(nameof(WordCount));
        OnPropertyChanged(nameof(CharCount));
    }
}