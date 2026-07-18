using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// KI-Bereich: erst die Lernphase (drei Module aus <see cref="KiContentService"/> als Tabs:
/// Was ist KI? / KI im Alltag / Sicher mit KI), dann die Fragenphase ("KI-Checkliste") über ein
/// ganz normales inneres <see cref="ExerciseViewModel"/> - dadurch laufen Fortschritt,
/// Fehler-Kartei, Spaced Repetition und Anti-Durchklick automatisch wie in jedem anderen Fach.
/// Die Lerntexte sind bewusst jederzeit wieder erreichbar ("Zurück zu den Modulen"), erst der
/// Abschluss der Fragen schaltet die nächste Stufe frei.
/// </summary>
public sealed partial class KiBereichViewModel : ObservableObject
{
    public sealed class ModuleTabViewModel
    {
        public required string Id { get; init; }
        public required string Icon { get; init; }
        public required string Title { get; init; }
        public required IReadOnlyList<SectionViewModel> Sections { get; init; }
    }

    public sealed class SectionViewModel
    {
        public required string Heading { get; init; }
        public required string Body { get; init; }
    }

    public ObservableCollection<ModuleTabViewModel> Modules { get; } = new();

    /// <summary>Innere Fragen-Übung (KI-Checkliste); rendert über das vorhandene ExerciseView-Template.</summary>
    public ExerciseViewModel Exercise { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsQuestionPhase))]
    private bool isLearningPhase = true;

    public bool IsQuestionPhase => !IsLearningPhase;

    [ObservableProperty]
    private ModuleTabViewModel? selectedModule;

    public KiBereichViewModel(ExerciseViewModel exercise)
    {
        Exercise = exercise;

        // Sprache einmal beim Aufbau festlegen - der Sprachwechsel passiert auf dem
        // Willkommensbildschirm, also vor dieser Stufe (gleiches Muster wie NewsViewModel).
        var turkish = LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch;
        foreach (var module in KiContentService.GetModules())
        {
            Modules.Add(new ModuleTabViewModel
            {
                Id = module.Id,
                Icon = module.Icon,
                Title = turkish ? module.TitleTr : module.TitleDe,
                Sections = module.Sections
                    .Select(s => new SectionViewModel
                    {
                        Heading = turkish ? s.HeadingTr : s.HeadingDe,
                        Body = turkish ? s.BodyTr : s.BodyDe
                    })
                    .ToList()
            });
        }

        SelectedModule = Modules.FirstOrDefault();
    }

    [RelayCommand]
    private void SelectModule(ModuleTabViewModel module) => SelectedModule = module;

    [RelayCommand]
    private void StartQuestions() => IsLearningPhase = false;

    [RelayCommand]
    private void BackToModules() => IsLearningPhase = true;
}
