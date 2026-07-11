using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Anzeige-Wrapper um ein <see cref="StudentProfile"/> für die Dashboard-Kachel der Profilauswahl:
/// ergänzt das reine Profil um den heutigen Sitzungs-Fortschritt (Fortschrittsring) und den
/// "Heute schon geschafft"-Zustand. Bewusst unveränderlich - die Kacheln werden bei jedem Anzeigen
/// der Profilauswahl frisch aufgebaut.
/// </summary>
public sealed class ProfileTileViewModel
{
    public StudentProfile Profile { get; }

    /// <summary>Heutiger Sitzungs-Fortschritt 0..1 für den Ring (Position im LearningStage-Ablauf).</summary>
    public double ProgressFraction { get; }

    /// <summary>Ob die heutige Lernsession bereits vollständig abgeschlossen (PC freigeschaltet) wurde.</summary>
    public bool IsDoneToday { get; }

    public string Name => Profile.Name;
    public string AvatarEmoji => Profile.AvatarEmoji;

    /// <summary>Gesamtstand der Belohnungs-Sterne, als fertiger Anzeige-Text ("⭐ 42").</summary>
    public string StarsDisplay => $"⭐ {Profile.TotalStars}";
    public bool HasStars => Profile.TotalStars > 0;

    public ProfileTileViewModel(StudentProfile profile, StudentProgress todaysProgress)
    {
        Profile = profile;
        IsDoneToday = todaysProgress.IsUnlocked;
        ProgressFraction = IsDoneToday
            ? 1d
            : (double)todaysProgress.CurrentStage / (double)LearningStage.Freigeschaltet;
    }
}
