using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
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
    private readonly StudentProfileRepository _profileRepo;
    private readonly KioskLockService _kioskLock;

    private AppSettings _settings = new();

    /// <summary>Alle deaktivierbaren Fachbereiche (News zählt bewusst nicht dazu, ist Pflicht).</summary>
    private static readonly (Subject Subject, string TranslationKey)[] ToggleableSubjects =
    {
        (Subject.Mathematik, "Stage_Mathematik"),
        (Subject.Deutsch, "Stage_Deutsch"),
        (Subject.Tuerkisch, "Stage_Tuerkisch"),
        (Subject.Englisch, "Stage_Englisch"),
        (Subject.Biologie, "Stage_Biologie"),
        (Subject.Chemie, "Stage_Chemie"),
        (Subject.Physik, "Stage_Physik"),
        (Subject.Gewi, "Stage_Gewi"),
        (Subject.Politik, "Stage_Politik"),
        (Subject.Geo, "Stage_Geo"),
        (Subject.Ethik, "Stage_Ethik"),
        (Subject.Itg, "Stage_Itg"),
    };

    /// <summary>Wird vom Aufrufer gesetzt, um beim Öffnen direkt das gerade aktive Kind-Profil vorauszuwählen.</summary>
    public string? PreselectProfileId { get; set; }

    [ObservableProperty]
    private bool isAuthenticated;

    [ObservableProperty]
    private bool isFirstTimeSetup;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private int timeLimitMinutes;

    [ObservableProperty]
    private StudentProfile? selectedProfile;

    public ObservableCollection<SubjectToggle> SubjectToggles { get; } = new();
    public ObservableCollection<StudentProfile> Profiles { get; } = new();
    public ObservableCollection<ActivityLogEntity> RecentActivity { get; } = new();
    public ObservableCollection<QuizAttemptEntity> QuizHistory { get; } = new();

    public event Action? RequestClose;

    public ParentSettingsViewModel(
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        KioskLockService kioskLock)
    {
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _profileRepo = profileRepo;
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

        SubjectToggles.Clear();
        foreach (var (subject, translationKey) in ToggleableSubjects)
        {
            SubjectToggles.Add(new SubjectToggle(
                subject,
                LocalizationService.Instance[translationKey],
                _settings.DisabledSubjects.Contains(subject)));
        }

        TimeLimitMinutes = _settings.DailyTimeLimitMinutes ?? 0;

        Profiles.Clear();
        foreach (var profile in await _profileRepo.GetAllAsync())
        {
            Profiles.Add(profile);
        }

        SelectedProfile = Profiles.FirstOrDefault(p => p.Id == PreselectProfileId) ?? Profiles.FirstOrDefault();
    }

    partial void OnSelectedProfileChanged(StudentProfile? value)
    {
        _ = ReloadActivityForSelectedProfileAsync();
    }

    private async Task ReloadActivityForSelectedProfileAsync()
    {
        RecentActivity.Clear();
        QuizHistory.Clear();

        if (SelectedProfile is null)
        {
            return;
        }

        foreach (var entry in await _activityLogRepo.GetRecentActivityAsync(SelectedProfile.Id))
        {
            RecentActivity.Add(entry);
        }

        foreach (var attempt in await _activityLogRepo.GetQuizHistoryAsync(SelectedProfile.Id))
        {
            QuizHistory.Add(attempt);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _settings.DisabledSubjects.Clear();
        foreach (var toggle in SubjectToggles)
        {
            if (toggle.IsDisabled)
            {
                _settings.DisabledSubjects.Add(toggle.Subject);
            }
        }

        _settings.DailyTimeLimitMinutes = TimeLimitMinutes > 0 ? TimeLimitMinutes : null;

        await _settingsRepo.SaveAsync(_settings);
        RequestClose?.Invoke();
    }

    [RelayCommand]
    private void SkipUnlock()
    {
        // Sofort-Freischaltung ist ein reiner Notfall-Override: entsperrt den PC unabhängig
        // davon, ob/welches Kind-Profil gerade aktiv war, ohne dessen Fortschritt zu verändern.
        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Direkter Weg auf dem Login-Bildschirm: Passwort eingeben und sofort entsperren/beenden,
    /// ohne erst in die volle Einstellungsansicht wechseln zu müssen.
    /// </summary>
    [RelayCommand]
    private void UnlockAndExit(string password)
    {
        ErrorMessage = string.Empty;

        if (IsFirstTimeSetup)
        {
            ErrorMessage = "Bitte zuerst über \"Anmelden\" ein Admin-Passwort festlegen.";
            return;
        }

        if (AdminAuthService.Verify(password, _settings.AdminPasswordHash, _settings.AdminPasswordSalt))
        {
            SkipUnlock();
        }
        else
        {
            ErrorMessage = "Falsches Passwort.";
        }
    }

    [RelayCommand]
    private void Close() => RequestClose?.Invoke();
}
