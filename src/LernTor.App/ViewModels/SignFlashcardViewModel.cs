using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Karteikarten: Zeichen zeigen, selbst überlegen, umdrehen.
///
/// <para><b>Warum die Selbsteinschätzung hier NICHT in den Lernstand zählt:</b> "Wusste ich"
/// drückt sich jeder gern, und beim Blick auf die Rückseite meint man ohnehin, es gewusst zu
/// haben. Als gekonnt gilt ein Zeichen deshalb nur über das Quiz, wo die Antwort vor der
/// Auflösung feststeht. Die Karteikarten sind zum Lernen da, das Quiz zum Prüfen - dass man
/// sich selbst nicht prüfen kann, ist keine Schwäche der Karten, sondern ihr Zweck.</para>
///
/// <para>"Noch nicht" schiebt die Karte ans Ende des Stapels, statt sie zu verwerfen: einmal
/// nicht gewusst heißt nicht, dass man es heute nicht mehr lernt.</para>
/// </summary>
public sealed partial class SignFlashcardViewModel : ObservableObject
{
    private readonly List<TrafficSign> _stack;
    private readonly Action _onBack;
    private readonly int _startCount;

    public SignFlashcardViewModel(IReadOnlyList<TrafficSign> signs, string title, Action onBack)
    {
        _stack = new List<TrafficSign>(signs);
        _startCount = _stack.Count;
        _onBack = onBack;
        Title = title;

        ShowCurrent();
    }

    public string Title { get; }

    [ObservableProperty]
    private TrafficSign? currentSign;

    /// <summary>Rückseite sichtbar? Solange nicht, steht nur das Bild da.</summary>
    [ObservableProperty]
    private bool isRevealed;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PositionDisplay))]
    private int remaining;

    [ObservableProperty]
    private bool isFinished;

    public string PositionDisplay => $"{_startCount - Remaining + 1} / {_startCount}";

    private void ShowCurrent()
    {
        Remaining = _stack.Count;

        if (_stack.Count == 0)
        {
            CurrentSign = null;
            IsFinished = true;
            return;
        }

        CurrentSign = _stack[0];
        IsRevealed = false;
    }

    [RelayCommand]
    private void Reveal() => IsRevealed = true;

    [RelayCommand]
    private void Known()
    {
        if (_stack.Count == 0)
        {
            return;
        }

        _stack.RemoveAt(0);
        ShowCurrent();
    }

    [RelayCommand]
    private void NotKnown()
    {
        if (_stack.Count <= 1)
        {
            // Die letzte Karte ans Ende zu schieben hieße, sie sofort wieder vorne zu haben -
            // das wäre eine Schleife, aus der nur "Zurück" herausführt.
            _stack.Clear();
            ShowCurrent();
            return;
        }

        var karte = _stack[0];
        _stack.RemoveAt(0);
        _stack.Add(karte);
        ShowCurrent();
    }

    [RelayCommand]
    private void Back() => _onBack();
}
