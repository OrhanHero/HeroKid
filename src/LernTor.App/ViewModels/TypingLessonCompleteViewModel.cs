using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Zwischenbildschirm nach Abschluss einer Lektion: Zeigt Ergebnis, Sterne, Button "Weiter".
/// </summary>
public sealed partial class TypingLessonCompleteViewModel : ObservableObject
{
    private readonly Action _onContinue;
    private readonly Action _onRetry;
    private readonly TypingLesson _lesson;
    private readonly TypingResult _result;

    public TypingLessonCompleteViewModel(
        TypingLesson lesson,
        TypingResult result,
        Action onContinue,
        Action onRetry)
    {
        _lesson = lesson;
        _result = result;
        _onContinue = onContinue;
        _onRetry = onRetry;

        StarsEarned = CalculateStars(result.Accuracy, result.Wpm);
    }

    public bool Passed => _result.Passed;
    public double Accuracy => _result.Accuracy;
    public double Wpm => _result.Wpm;
    public int CorrectChars => _result.CorrectCharacters;
    public int TotalChars => _result.TotalCharacters;
    public TimeSpan Elapsed => _result.Elapsed;

    [ObservableProperty]
    private int starsEarned;

    public IEnumerable<int> StarsList => Enumerable.Range(1, StarsEarned);

    [RelayCommand]
    private void Continue() => _onContinue();

    [RelayCommand]
    private void Retry() => _onRetry();

    private int CalculateStars(double accuracy, double wpm)
    {
        int stars = 1;
        if (accuracy >= 0.90) stars++;
        if (accuracy >= 0.97) stars++;
        if (wpm >= 30) stars++;
        if (wpm >= 50) stars++;
        return Math.Min(stars, 5);
    }
}