using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Pflicht-Leseabschnitt: zeigt täglich ein Gedicht/wichtiges Werk (Mix DE/TR/EN), das laut
/// vorgelesen werden soll. Es gibt bewusst KEINE Überspringen-Funktion - stattdessen läuft ein
/// 5-Minuten-Timer herunter, erst danach wird "Weiter" nutzbar. Eine Prüfung, ob tatsächlich laut
/// vorgelesen wurde, ist technisch nicht möglich und wird hier auch nicht versucht.
/// </summary>
public sealed partial class ReadingViewModel : ObservableObject
{
    private static readonly TimeSpan MinimumDuration = TimeSpan.FromMinutes(5);

    private readonly Action _onCompleted;
    private readonly DispatcherTimer _timer;

    public ReadingPiece Piece { get; }

    [ObservableProperty]
    private TimeSpan remainingTime = MinimumDuration;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool canContinue;

    public string RemainingTimeDisplay => $"{(int)RemainingTime.TotalMinutes}:{RemainingTime.Seconds:00}";

    public ReadingViewModel(ReadingPiece piece, Action onCompleted)
    {
        Piece = piece;
        _onCompleted = onCompleted;

        _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
    }

    partial void OnRemainingTimeChanged(TimeSpan value) => OnPropertyChanged(nameof(RemainingTimeDisplay));

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

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Continue()
    {
        _timer.Stop();
        _onCompleted();
    }
}
