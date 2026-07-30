using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.Core.Enums;

namespace LernTor.App.ViewModels;

/// <summary>Ein Fach in der "Bereiche deaktivieren"-Liste des Eltern-Bereichs.</summary>
public sealed partial class SubjectToggle : ObservableObject
{
    private readonly Action? _onChanged;

    public Subject Subject { get; }
    public string DisplayName { get; }

    [ObservableProperty]
    private bool isDisabled;

    public SubjectToggle(Subject subject, string displayName, bool isDisabled, Action? onChanged = null)
    {
        Subject = subject;
        DisplayName = displayName;
        this.isDisabled = isDisabled;
        _onChanged = onChanged;
    }

    /// <summary>
    /// Meldet die Änderung an den Eltern-Bereich. Die Fach-Schalter landen erst über "Speichern"
    /// in der Datenbank - ohne diese Meldung galten die Einstellungen als unverändert, die
    /// Rückfrage beim Schließen blieb aus, und ein abgeschaltetes Fach war beim nächsten Start
    /// wieder an.
    /// </summary>
    partial void OnIsDisabledChanged(bool value) => _onChanged?.Invoke();
}
