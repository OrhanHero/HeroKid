using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// „🏆 Mein Fortschritt“: für jedes geübte Thema die Meisterschaftsstufe (siehe
/// <see cref="TopicMasteryCalculator"/>), nach Fach gruppiert.
///
/// <para>Erreichbar von der Startseite, auch im Planer-Zwischenstopp. Die Ansicht zeigt nur an:
/// sie schaltet nichts frei, überspringt nichts und hält keine Mindestzeit auf - läuft eine
/// Etappe, ist sie schon angehalten, weil der Weg hierher über den Planer führt.</para>
///
/// <para>Bewusst <b>kein Rot</b> für schwache Themen: „vertraut“ ist eine Stufe auf dem Weg,
/// keine Note. Hervorgehoben werden die erreichten Stufen.</para>
/// </summary>
public sealed partial class ProgressOverviewViewModel : ObservableObject
{
    private readonly Action _onBack;

    public ProgressOverviewViewModel(string profileName, IReadOnlyList<TopicMasteryStatus> topics, Action onBack)
    {
        _onBack = onBack;
        ProfileName = profileName;

        var zaehlung = TopicMasteryCalculator.CountByLevel(topics);
        MasteredCount = zaehlung[MasteryLevel.Gemeistert];
        SecureCount = zaehlung[MasteryLevel.Sicher];
        FamiliarCount = zaehlung[MasteryLevel.Vertraut];
        StartedCount = zaehlung[MasteryLevel.Angefangen];

        // Reihenfolge kommt schon sortiert aus dem Calculator (Fach, dann Thema).
        foreach (var fach in topics.GroupBy(thema => thema.Subject))
        {
            Subjects.Add(new SubjectMasteryGroupViewModel(fach.Key, fach.ToList()));
        }
    }

    public string ProfileName { get; }

    public int MasteredCount { get; }

    public int SecureCount { get; }

    public int FamiliarCount { get; }

    public int StartedCount { get; }

    public string SummaryDisplay => string.Format(
        LocalizationService.Instance["Progress_Summary"], MasteredCount, SecureCount, FamiliarCount, StartedCount);

    public ObservableCollection<SubjectMasteryGroupViewModel> Subjects { get; } = new();

    public bool HasTopics => Subjects.Count > 0;

    [RelayCommand]
    private void Back() => _onBack();
}

/// <summary>Eine Fach-Karte in „Mein Fortschritt“.</summary>
public sealed class SubjectMasteryGroupViewModel
{
    public SubjectMasteryGroupViewModel(Subject subject, IReadOnlyList<TopicMasteryStatus> topics)
    {
        Subject = subject;
        var l = LocalizationService.Instance;
        Title = l[$"Stage_{subject}"];

        var sicher = topics.Count(thema => thema.Level >= MasteryLevel.Sicher);
        SummaryDisplay = string.Format(l["Progress_SubjectSummary"], sicher, topics.Count);

        foreach (var thema in topics)
        {
            Topics.Add(new TopicMasteryRowViewModel(thema));
        }
    }

    public Subject Subject { get; }

    public string Title { get; }

    public string SummaryDisplay { get; }

    public ObservableCollection<TopicMasteryRowViewModel> Topics { get; } = new();
}

/// <summary>Eine Themenzeile mit Stufe und „8 von 10 zuletzt richtig“.</summary>
public sealed class TopicMasteryRowViewModel
{
    public TopicMasteryRowViewModel(TopicMasteryStatus status)
    {
        Status = status;
        var l = LocalizationService.Instance;
        LevelDisplay = l[$"Progress_Level_{status.Level}"];
        DetailDisplay = string.Format(l["Progress_TopicDetail"], status.RecentCorrect, status.RecentAnswered);
    }

    public TopicMasteryStatus Status { get; }

    public string Topic => Status.Topic;

    /// <summary>Für die Farbe des Stufen-Etiketts (DataTrigger in der View).</summary>
    public MasteryLevel Level => Status.Level;

    public string LevelDisplay { get; }

    public string DetailDisplay { get; }
}
