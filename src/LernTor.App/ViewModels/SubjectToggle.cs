using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.Core.Enums;

namespace LernTor.App.ViewModels;

/// <summary>Ein Fach in der "Bereiche deaktivieren"-Liste des Eltern-Bereichs.</summary>
public sealed partial class SubjectToggle : ObservableObject
{
    public Subject Subject { get; }
    public string DisplayName { get; }

    [ObservableProperty]
    private bool isDisabled;

    public SubjectToggle(Subject subject, string displayName, bool isDisabled)
    {
        Subject = subject;
        DisplayName = displayName;
        this.isDisabled = isDisabled;
    }
}
