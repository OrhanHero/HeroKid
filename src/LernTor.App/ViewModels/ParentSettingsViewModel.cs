using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;
using LernTor.Security;

namespace LernTor.App.ViewModels;

public sealed partial class ParentSettingsViewModel : ObservableObject
{
    private readonly SettingsRepository _settingsRepo;
    private readonly ActivityLogRepository _activityLogRepo;
    private readonly ProgressRepository _progressRepo;
    private readonly KioskLockService _kioskLock;

    private AppSettings _settings = new();

    [ObservableProperty]
    private bool isAuthenticated;

    [ObservableProperty]
    private bool isFirstTimeSetup;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool disableMathematik;

    [ObservableProperty]
    private bool disableDeutsch;

    [ObservableProperty]
    private bool disableTuerkisch;

    [ObservableProperty]
    private bool disableNaturwissenschaften;

    [ObservableProperty]
    private bool isGrade9;

    [ObservableProperty]
    private int timeLimitMinutes;

    public ObservableCollection<ActivityLogEntity> RecentActivity { get; } = new();
    public ObservableCollection<QuizAttemptEntity> QuizHistory { get; } = new();

    public event Action? RequestClose;

    public ParentSettingsViewModel(
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        ProgressRepository progressRepo,
        KioskLockService kioskLock)
    {
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _progressRepo = progressRepo;
        _kioskLock = kioskLock;
    }

    public async Task InitializeAsync()
    {
        _settings = await _settingsRepo.LoadAsync();
        IsFirstTimeSetup = string.IsNullOrEmpty(_settings.AdminPasswordHash);
    }

    [RelayCommand]
    private async Task LoginAsync(string password)
    {
        ErrorMessage = string.Empty;

        if (IsFirstTimeSetup)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                ErrorMessage = "Das Passwort muss mindestens 4 Zeichen lang sein.";
                return;
            }

            var (hash, salt) = AdminAuthService.HashPassword(password);
            _settings.AdminPasswordHash = hash;
            _settings.AdminPasswordSalt = salt;
            await _settingsRepo.SaveAsync(_settings);
            IsFirstTimeSetup = false;
            await AuthenticateAsync();
            return;
        }

        if (AdminAuthService.Verify(password, _settings.AdminPasswordHash, _settings.AdminPasswordSalt))
        {
            await AuthenticateAsync();
        }
        else
        {
            ErrorMessage = "Falsches Passwort.";
        }
    }

    private async Task AuthenticateAsync()
    {
        IsAuthenticated = true;

        DisableMathematik = _settings.DisabledSubjects.Contains(Subject.Mathematik);
        DisableDeutsch = _settings.DisabledSubjects.Contains(Subject.Deutsch);
        DisableTuerkisch = _settings.DisabledSubjects.Contains(Subject.Tuerkisch);
        DisableNaturwissenschaften = _settings.DisabledSubjects.Contains(Subject.Naturwissenschaften);
        IsGrade9 = _settings.StudentGradeLevel == GradeLevel.Klasse9;
        TimeLimitMinutes = _settings.DailyTimeLimitMinutes ?? 0;

        var activity = await _activityLogRepo.GetRecentActivityAsync();
        RecentActivity.Clear();
        foreach (var entry in activity)
        {
            RecentActivity.Add(entry);
        }

        var quizHistory = await _activityLogRepo.GetQuizHistoryAsync();
        QuizHistory.Clear();
        foreach (var attempt in quizHistory)
        {
            QuizHistory.Add(attempt);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _settings.DisabledSubjects.Clear();
        if (DisableMathematik) _settings.DisabledSubjects.Add(Subject.Mathematik);
        if (DisableDeutsch) _settings.DisabledSubjects.Add(Subject.Deutsch);
        if (DisableTuerkisch) _settings.DisabledSubjects.Add(Subject.Tuerkisch);
        if (DisableNaturwissenschaften) _settings.DisabledSubjects.Add(Subject.Naturwissenschaften);

        _settings.StudentGradeLevel = IsGrade9 ? GradeLevel.Klasse9 : GradeLevel.Klasse6;
        _settings.DailyTimeLimitMinutes = TimeLimitMinutes > 0 ? TimeLimitMinutes : null;

        await _settingsRepo.SaveAsync(_settings);
        RequestClose?.Invoke();
    }

    [RelayCommand]
    private async Task SkipUnlockAsync()
    {
        var progress = await _progressRepo.LoadOrCreateTodayAsync();
        progress.IsUnlocked = true;
        progress.CurrentStage = LearningStage.Freigeschaltet;
        await _progressRepo.SaveAsync(progress);

        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    [RelayCommand]
    private void Close() => RequestClose?.Invoke();
}
