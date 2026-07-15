using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Data.Repositories;

namespace LernTor.App.ViewModels;

/// <summary>Eine Belohnung in der Kind-Ansicht auf dem Freigeschaltet-Bildschirm.</summary>
public sealed class RewardItemViewModel
{
    public required int RewardId { get; init; }
    public required string Display { get; init; }
    public required string CostDisplay { get; init; }
    public required int StarCost { get; init; }
    public required bool CanRedeem { get; init; }
}

public sealed partial class ResultViewModel : ObservableObject
{
    private readonly Action _onRetryRequested;
    private readonly Action _onUnlockConfirmed;
    private readonly RewardRepository? _rewardRepo;
    private readonly string? _profileId;

    public bool Passed { get; }
    public QuizResult? Result { get; }

    public int CorrectCount => Result?.CorrectCount ?? 0;
    public int TotalQuestions => Result?.TotalQuestions ?? 0;
    public double ScorePercentage => Result?.ScorePercentage ?? 1.0;

    /// <summary>Heute verdiente Belohnungs-Sterne (Gamification).</summary>
    public int EarnedStarsToday { get; }

    /// <summary>Gesamtstand - veränderlich, weil das Einlösen einer Belohnung Sterne abzieht.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StarsSummary))]
    private int totalStars;

    public string StarsSummary => string.Format(
        LocalizationService.Instance["Result_StarsSummary"], EarnedStarsToday, TotalStars);

    public bool HasStarsToShow => EarnedStarsToday > 0;

    // --- 🎁 Belohnungen (von den Eltern gepflegt, mit Sternen einlösbar) ---

    public ObservableCollection<RewardItemViewModel> Rewards { get; } = new();

    public bool HasRewards => Rewards.Count > 0;

    public ResultViewModel(
        bool passed,
        QuizResult? result,
        int earnedStarsToday,
        int totalStars,
        Action onRetryRequested,
        Action onUnlockConfirmed,
        RewardRepository? rewardRepo = null,
        string? profileId = null)
    {
        Passed = passed;
        Result = result;
        EarnedStarsToday = earnedStarsToday;
        TotalStars = totalStars;
        _onRetryRequested = onRetryRequested;
        _onUnlockConfirmed = onUnlockConfirmed;
        _rewardRepo = rewardRepo;
        _profileId = profileId;

        // Belohnungen nur auf dem Freigeschaltet-Bildschirm zeigen (nicht beim Nicht-Bestehen -
        // dort soll der Fokus auf dem erneuten Versuch liegen, nicht auf verpassten Belohnungen).
        if (Passed)
        {
            _ = LoadRewardsAsync();
        }
    }

    private async Task LoadRewardsAsync()
    {
        if (_rewardRepo is null || _profileId is null)
        {
            return;
        }

        var rewards = await _rewardRepo.GetAllAsync();
        Rewards.Clear();
        foreach (var reward in rewards)
        {
            var canRedeem = TotalStars >= reward.StarCost;
            Rewards.Add(new RewardItemViewModel
            {
                RewardId = reward.Id,
                Display = $"{reward.Emoji} {reward.Title}",
                // Erreichbar: nur die Kosten zeigen; sonst den Fortschritt ("12 / 20 ⭐").
                CostDisplay = canRedeem ? $"{reward.StarCost} ⭐" : $"{TotalStars} / {reward.StarCost} ⭐",
                StarCost = reward.StarCost,
                CanRedeem = canRedeem
            });
        }

        OnPropertyChanged(nameof(HasRewards));
    }

    [RelayCommand]
    private async Task RedeemRewardAsync(RewardItemViewModel item)
    {
        if (_rewardRepo is null || _profileId is null || !item.CanRedeem)
        {
            return;
        }

        // Rückfrage gegen versehentliches Verklicken - eingelöste Sterne sind weg.
        var confirmed = System.Windows.MessageBox.Show(
            string.Format(LocalizationService.Instance["Result_Rewards_Confirm"], item.Display, item.StarCost),
            LocalizationService.Instance["Result_Rewards_Title"],
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Question,
            System.Windows.MessageBoxResult.No);

        if (confirmed != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        var (success, newTotal) = await _rewardRepo.RedeemAsync(_profileId, item.RewardId);
        if (success)
        {
            TotalStars = newTotal;
            await LoadRewardsAsync();
        }
    }

    [RelayCommand]
    private void RetryWeakSubjects() => _onRetryRequested();

    [RelayCommand]
    private void ConfirmUnlock() => _onUnlockConfirmed();
}
