using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Zeile der Nebeneinander-Ansicht mehrerer Kinder im Eltern-Bericht (siehe
/// <see cref="ProfileComparison"/>). Bewusst ohne Platzierung, Pfeile oder Farbverlauf:
/// die Zahlen stehen nebeneinander, gewertet wird nicht.
/// </summary>
public sealed class ProfileComparisonRowViewModel
{
    public ProfileComparisonRowViewModel(ProfileComparisonRow row)
    {
        Row = row;
    }

    public ProfileComparisonRow Row { get; }

    public string Name => Row.Name;

    public bool HasData => Row.HasData;

    public string LearnedDaysDisplay => Row.HasData ? $"{Row.LearnedDays}" : "–";

    public string AccuracyDisplay => Row.HasData
        ? $"{Row.Accuracy:P0} ({Row.Correct}/{Row.Answered})"
        : "–";

    /// <summary>Lernzeit in der Einheit, in der Eltern denken - Minuten, ab einer Stunde mit Stunde.</summary>
    public string LearningTimeDisplay
    {
        get
        {
            if (!Row.HasData || Row.LearningTimeMs <= 0)
            {
                return "–";
            }

            var time = Row.LearningTime;
            return time.TotalHours >= 1
                ? $"{(int)time.TotalHours} h {time.Minutes} min"
                : $"{(int)time.TotalMinutes} min";
        }
    }

    public string StarsDisplay => $"{Row.TotalStars} ⭐";

    /// <summary>Klartext statt leerer Zeile - "war diese Woche nicht dran" ist eine Information,
    /// keine Lücke.</summary>
    public string EmptyHint => Row.HasData
        ? string.Empty
        : "in diesem Zeitraum keine Aufgaben bearbeitet";
}
