using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
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

/// <summary>
/// Eine Fach-Zeile in der eigenen Fortschrittsansicht des Kindes.
///
/// <para>Bewusst ohne Ampelfarben und ohne trauriges Gesicht bei einem Rueckschritt: ihn zu
/// verschweigen waere unehrlich, ihn rot zu markieren waere eine Strafe fuer etwas, das jedem
/// passiert. Die Zahlen stehen da, gewertet wird nicht.</para>
/// </summary>
public sealed class SubjectProgressRowViewModel
{
    public SubjectProgressRowViewModel(SubjectProgress progress, string subjectLabel)
    {
        Progress = progress;
        SubjectLabel = subjectLabel;
    }

    public SubjectProgress Progress { get; }

    public string SubjectLabel { get; }

    /// <summary>"vor zwei Wochen 40 % → jetzt 70 %" - der Kern der Sache in einer Zeile.</summary>
    public string ChangeDisplay => $"{Progress.EarlierPercent} % → {Progress.RecentPercent} %";

    public string TrendDisplay => Progress.Trend switch
    {
        ProgressTrend.Verbessert => $"↑ {Progress.Change} Punkte besser",
        ProgressTrend.Schwaecher => $"↓ {Math.Abs(Progress.Change)} Punkte weniger",
        _ => "gleich geblieben"
    };

    /// <summary>Nur Verbesserungen werden hervorgehoben - der Rest bleibt neutral stehen.</summary>
    public bool IsImprovement => Progress.Trend == ProgressTrend.Verbessert;
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

    // --- Tages-Zusammenfassung (siehe MainViewModel.BuildResultViewModelAsync) ---

    /// <summary>Heute beantwortete Aufgaben (Aktivitätsprotokoll seit Mitternacht).</summary>
    public int TodayAnsweredCount { get; }

    /// <summary>Trefferquote der heute beantworteten Aufgaben in Prozent (gerundet).</summary>
    public int TodayCorrectPercent { get; }

    /// <summary>🔥-Lernserie; 0 = ausgeblendet (von den Eltern nicht eingeschaltet oder Serie zu kurz).</summary>
    public int CurrentStreak { get; }

    public bool ShowStreak => CurrentStreak >= 2;

    /// <summary>Nur auf dem Freigeschaltet-Bildschirm - beim Nicht-Bestehen liegt der Fokus auf
    /// dem erneuten Versuch, nicht auf einer Bilanz.</summary>
    public bool HasDailySummary => Passed && TodayAnsweredCount > 0;

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

    // --- Eigene Entwicklung (siehe LearningProgressTracker) ---

    /// <summary>Fach fuer Fach: frueher gegen jetzt. Leer, solange es zu wenig Daten gibt -
    /// lieber nichts zeigen als eine Zahl, die nichts bedeutet.</summary>
    public ObservableCollection<SubjectProgressRowViewModel> SubjectProgressRows { get; } = new();

    /// <summary>Nur auf dem Freigeschaltet-Bildschirm - beim Nicht-Bestehen liegt der Fokus auf
    /// dem erneuten Versuch.</summary>
    public bool HasSubjectProgress => Passed && SubjectProgressRows.Count > 0;

    public ResultViewModel(
        bool passed,
        QuizResult? result,
        int earnedStarsToday,
        int totalStars,
        int todayAnsweredCount,
        int todayCorrectPercent,
        int currentStreak,
        Action onRetryRequested,
        Action onUnlockConfirmed,
        RewardRepository? rewardRepo = null,
        string? profileId = null,
        IReadOnlyList<SubjectProgress>? subjectProgress = null)
    {
        Passed = passed;
        Result = result;
        EarnedStarsToday = earnedStarsToday;
        TotalStars = totalStars;
        TodayAnsweredCount = todayAnsweredCount;
        TodayCorrectPercent = todayCorrectPercent;
        CurrentStreak = currentStreak;
        _onRetryRequested = onRetryRequested;
        _onUnlockConfirmed = onUnlockConfirmed;
        _rewardRepo = rewardRepo;
        _profileId = profileId;

        foreach (var progress in subjectProgress ?? Array.Empty<SubjectProgress>())
        {
            SubjectProgressRows.Add(new SubjectProgressRowViewModel(
                progress, LocalizationService.Instance[$"Stage_{progress.Subject}"]));
        }

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
