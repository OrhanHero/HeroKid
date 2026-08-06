using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Ein Abschnitt einer Lektion, fertig für die Anzeige.</summary>
public sealed class CourseSectionViewModel
{
    public CourseSectionViewModel(CourseSection section)
    {
        Heading = section.Heading;
        Body = section.Body;
        Sign = section.SignNumber is { } nummer ? TrafficSignCatalog.ByNumber(nummer) : null;
    }

    public string Heading { get; }

    public string Body { get; }

    /// <summary>Zeichen zum Abschnitt, falls eines dazugehört - sonst null.</summary>
    public TrafficSign? Sign { get; }
}

/// <summary>
/// Eine Kurslektion zum Lesen, mit der Lernstandskontrolle am Ende.
///
/// <para><b>Die Kontrolle ist freiwillig und kann warten.</b> Wer nur lesen will, drückt
/// "Fertig gelesen" und ist raus; die Lektion gilt dann als gelesen, aber nicht als geschafft.
/// Eine erzwungene Prüfung nach jedem Text macht aus einem Nachschlagewerk eine Schulstunde,
/// und dann wird nichts mehr nachgeschlagen.</para>
/// </summary>
public sealed partial class CourseLessonViewModel : ObservableObject
{
    private readonly DrivingCourseLesson _lesson;
    private readonly Action<DrivingCourseLesson> _onStartCheck;
    private readonly Action<DrivingCourseLesson> _onFinishedReading;
    private readonly Action _onBack;

    public CourseLessonViewModel(
        DrivingCourseLesson lesson,
        CourseLessonProgressEntity? progress,
        bool checkPossible,
        Action<DrivingCourseLesson> onStartCheck,
        Action<DrivingCourseLesson> onFinishedReading,
        Action onBack)
    {
        _lesson = lesson;
        _onStartCheck = onStartCheck;
        _onFinishedReading = onFinishedReading;
        _onBack = onBack;

        CheckPossible = checkPossible;
        IsPassed = progress?.PassedAt is not null;
        BestPercent = progress?.BestPercent ?? 0;

        foreach (var abschnitt in lesson.Sections)
        {
            Sections.Add(new CourseSectionViewModel(abschnitt));
        }

        foreach (var merksatz in lesson.KeyPoints)
        {
            KeyPoints.Add(merksatz);
        }
    }

    public string Title => _lesson.Title;

    public string Intro => _lesson.Intro;

    public string TopicLabel => DrivingTheoryCatalog.TopicLabel(_lesson.Topic);

    public ObservableCollection<CourseSectionViewModel> Sections { get; } = new();

    public ObservableCollection<string> KeyPoints { get; } = new();

    /// <summary>Ob es für dieses Sachgebiet überhaupt Fragen gibt. Ein Knopf, der ins Leere
    /// führt, ist schlimmer als kein Knopf.</summary>
    public bool CheckPossible { get; }

    public bool IsPassed { get; }

    public int BestPercent { get; }

    public string StatusDisplay => IsPassed
        ? LocalizationService.Instance["Fs_LessonDone"]
        : BestPercent > 0
            ? string.Format(LocalizationService.Instance["Fs_LessonBest"], BestPercent)
            : string.Empty;

    public bool HasStatus => !string.IsNullOrEmpty(StatusDisplay);

    [RelayCommand]
    private void StartCheck() => _onStartCheck(_lesson);

    [RelayCommand]
    private void FinishReading() => _onFinishedReading(_lesson);

    [RelayCommand]
    private void Back() => _onBack();
}
