using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

public sealed partial class ResultViewModel : ObservableObject
{
    private readonly Action _onRetryRequested;
    private readonly Action _onUnlockConfirmed;

    public bool Passed { get; }
    public QuizResult? Result { get; }

    public int CorrectCount => Result?.CorrectCount ?? 0;
    public int TotalQuestions => Result?.TotalQuestions ?? 0;
    public double ScorePercentage => Result?.ScorePercentage ?? 1.0;

    /// <summary>Heute verdiente + insgesamt gesammelte Belohnungs-Sterne (Gamification).</summary>
    public int EarnedStarsToday { get; }
    public int TotalStars { get; }

    public string StarsSummary => string.Format(
        LocalizationService.Instance["Result_StarsSummary"], EarnedStarsToday, TotalStars);

    public bool HasStarsToShow => EarnedStarsToday > 0;

    public ResultViewModel(
        bool passed,
        QuizResult? result,
        int earnedStarsToday,
        int totalStars,
        Action onRetryRequested,
        Action onUnlockConfirmed)
    {
        Passed = passed;
        Result = result;
        EarnedStarsToday = earnedStarsToday;
        TotalStars = totalStars;
        _onRetryRequested = onRetryRequested;
        _onUnlockConfirmed = onUnlockConfirmed;
    }

    [RelayCommand]
    private void RetryWeakSubjects() => _onRetryRequested();

    [RelayCommand]
    private void ConfirmUnlock() => _onUnlockConfirmed();
}
