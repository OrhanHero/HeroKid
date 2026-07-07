using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Repositories;

namespace LernTor.App.ViewModels;

/// <summary>
/// Profilauswahl beim Start: mehrere Kinder am selben PC können so getrennt ihren eigenen
/// Fortschritt und ihre eigene Klassenstufe haben. Neue Profile können direkt hier angelegt werden.
/// </summary>
public sealed partial class ProfileSelectionViewModel : ObservableObject
{
    private readonly StudentProfileRepository _profileRepo;
    private readonly Action<StudentProfile> _onProfileSelected;

    public ObservableCollection<StudentProfile> Profiles { get; } = new();

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
    private string errorMessage = string.Empty;

    public ProfileSelectionViewModel(StudentProfileRepository profileRepo, Action<StudentProfile> onProfileSelected)
    {
        _profileRepo = profileRepo;
        _onProfileSelected = onProfileSelected;
    }

    public async Task InitializeAsync()
    {
        await _profileRepo.SeedDefaultProfilesIfEmptyAsync();

        Profiles.Clear();
        foreach (var profile in await _profileRepo.GetAllAsync())
        {
            Profiles.Add(profile);
        }
    }

    [RelayCommand]
    private void SelectProfile(StudentProfile profile) => _onProfileSelected(profile);

    [RelayCommand]
    private void ShowCreateForm()
    {
        NewProfileName = string.Empty;
        NewProfileAge = string.Empty;
        NewProfileClassLabel = string.Empty;
        NewProfileIsGrade9 = false;
        ErrorMessage = string.Empty;
        IsCreatingNewProfile = true;
    }

    [RelayCommand]
    private void CancelCreateForm() => IsCreatingNewProfile = false;

    [RelayCommand]
    private async Task CreateProfileAsync()
    {
        if (string.IsNullOrWhiteSpace(NewProfileName))
        {
            ErrorMessage = Localization.LocalizationService.Instance["Profile_NameRequired"];
            return;
        }

        int? age = int.TryParse(NewProfileAge, out var parsedAge) ? parsedAge : null;
        var grade = NewProfileIsGrade9 ? GradeLevel.Klasse9 : GradeLevel.Klasse6;
        var classLabel = string.IsNullOrWhiteSpace(NewProfileClassLabel) ? null : NewProfileClassLabel.Trim();

        var profile = await _profileRepo.CreateAsync(NewProfileName.Trim(), age, classLabel, grade);
        Profiles.Add(profile);
        IsCreatingNewProfile = false;

        _onProfileSelected(profile);
    }
}
