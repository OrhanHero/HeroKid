using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Repositories;

namespace LernTor.App.ViewModels;

/// <summary>
/// Profilauswahl beim Start als kleines Dashboard: mehrere Kinder am selben PC wählen ihre Kachel
/// (mit selbst gewähltem Avatar und heutigem Fortschrittsring) oder legen ein neues Profil an.
/// </summary>
public sealed partial class ProfileSelectionViewModel : ObservableObject
{
    private readonly StudentProfileRepository _profileRepo;
    private readonly ProgressRepository _progressRepo;
    private readonly Action<StudentProfile> _onProfileSelected;

    public ObservableCollection<ProfileTileViewModel> ProfileTiles { get; } = new();

    /// <summary>Avatare zur Auswahl beim Anlegen - bewusst Emojis (keine Bild-Assets, nativ auf
    /// jedem Windows, kulturneutral). Reihenfolge = Anzeige-Reihenfolge im Picker.</summary>
    public IReadOnlyList<string> AvatarChoices { get; } = new[]
    {
        "🧒", "🚀", "⚽", "🦁", "🦄", "🐱", "🐢", "🦅", "🎮", "🌟", "🤖", "🎨"
    };

    [ObservableProperty]
    private bool isCreatingNewProfile;

    [ObservableProperty]
    private string newProfileName = string.Empty;

    [ObservableProperty]
    private string newProfileAge = string.Empty;

    [ObservableProperty]
    private string newProfileClassLabel = string.Empty;

    [ObservableProperty]
    private bool newProfileIsGrade9;

    [ObservableProperty]
    private string selectedAvatar = StudentProfile.DefaultAvatar;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    /// <summary>Tageszeitabhängige Begrüßung (morgens/nachmittags/abends), in der aktiven Sprache.</summary>
    public string Greeting => DateTime.Now.Hour switch
    {
        < 11 => LocalizationService.Instance["Profile_GreetingMorning"],
        < 17 => LocalizationService.Instance["Profile_GreetingAfternoon"],
        _ => LocalizationService.Instance["Profile_GreetingEvening"]
    };

    public ProfileSelectionViewModel(
        StudentProfileRepository profileRepo,
        ProgressRepository progressRepo,
        Action<StudentProfile> onProfileSelected)
    {
        _profileRepo = profileRepo;
        _progressRepo = progressRepo;
        _onProfileSelected = onProfileSelected;
    }

    public async Task InitializeAsync()
    {
        await _profileRepo.SeedDefaultProfilesIfEmptyAsync();

        ProfileTiles.Clear();
        foreach (var profile in await _profileRepo.GetAllAsync())
        {
            // LoadOrCreateTodayAsync schreibt beim "Anlegen" nichts in die DB (liefert nur ein
            // frisches Objekt) - hier also ein reiner Lese-Peek für den Fortschrittsring.
            var todaysProgress = await _progressRepo.LoadOrCreateTodayAsync(profile.Id);
            ProfileTiles.Add(new ProfileTileViewModel(profile, todaysProgress));
        }
    }

    [RelayCommand]
    private void SelectProfile(ProfileTileViewModel tile) => _onProfileSelected(tile.Profile);

    [RelayCommand]
    private void ShowCreateForm()
    {
        NewProfileName = string.Empty;
        NewProfileAge = string.Empty;
        NewProfileClassLabel = string.Empty;
        NewProfileIsGrade9 = false;
        SelectedAvatar = StudentProfile.DefaultAvatar;
        ErrorMessage = string.Empty;
        IsCreatingNewProfile = true;
    }

    [RelayCommand]
    private void CancelCreateForm() => IsCreatingNewProfile = false;

    [RelayCommand]
    private void PickAvatar(string avatar) => SelectedAvatar = avatar;

    [RelayCommand]
    private async Task CreateProfileAsync()
    {
        if (string.IsNullOrWhiteSpace(NewProfileName))
        {
            ErrorMessage = LocalizationService.Instance["Profile_NameRequired"];
            return;
        }

        int? age = int.TryParse(NewProfileAge, out var parsedAge) ? parsedAge : null;
        var grade = NewProfileIsGrade9 ? GradeLevel.Klasse9 : GradeLevel.Klasse6;
        var classLabel = string.IsNullOrWhiteSpace(NewProfileClassLabel) ? null : NewProfileClassLabel.Trim();

        var profile = await _profileRepo.CreateAsync(NewProfileName.Trim(), age, classLabel, grade, SelectedAvatar);
        IsCreatingNewProfile = false;

        _onProfileSelected(profile);
    }
}
