using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Eine Lektion in der Kursübersicht, mit Stand.</summary>
public sealed class CourseLessonRowViewModel
{
    public CourseLessonRowViewModel(
        DrivingCourseLesson lesson,
        int number,
        CourseLessonProgressEntity? progress,
        bool isRecommended,
        TrafficSign? sample)
    {
        Lesson = lesson;
        Number = number;
        Progress = progress;
        IsRecommended = isRecommended;
        Sample = sample;
    }

    public DrivingCourseLesson Lesson { get; }

    /// <summary>Position im Kurs, ab 1 - die Kinder sollen sehen, dass es eine Reihenfolge gibt.</summary>
    public int Number { get; }

    public CourseLessonProgressEntity? Progress { get; }

    /// <summary>Die erste Lektion, die noch nicht geschafft ist.</summary>
    public bool IsRecommended { get; }

    /// <summary>Ein Zeichen aus der Lektion als Vorschaubild.</summary>
    public TrafficSign? Sample { get; }

    public string Title => $"{Number}. {Lesson.Title}";

    public string Intro => Lesson.Intro;

    public LessonStatus Status => Progress switch
    {
        { PassedAt: not null } => LessonStatus.Geschafft,
        { ReadAt: not null } => LessonStatus.Gelesen,
        _ => LessonStatus.Offen
    };

    public bool IsDone => Status == LessonStatus.Geschafft;

    public bool IsRead => Status == LessonStatus.Gelesen;

    public string StatusDisplay => Status switch
    {
        LessonStatus.Geschafft => LocalizationService.Instance["Fs_LessonDone"],
        LessonStatus.Gelesen => string.Format(
            LocalizationService.Instance["Fs_LessonRead"], Progress?.BestPercent ?? 0),
        _ => LocalizationService.Instance["Fs_LessonOpen"]
    };
}

/// <summary>
/// Der Theorie-Kurs: vierzehn Lektionen, eine je Sachgebiet, in Kursreihenfolge.
///
/// <para><b>Die Reihenfolge ist eine Empfehlung, keine Sperre.</b> Jede Lektion ist jederzeit
/// zu öffnen - wer in der Fahrschule gerade Vorfahrt hat, soll Vorfahrt lesen können, ohne sich
/// vorher durch dreizehn andere zu klicken. Markiert wird trotzdem, wo weiterzumachen wäre.</para>
/// </summary>
public sealed partial class CourseOverviewViewModel : ObservableObject
{
    private readonly Action<DrivingCourseLesson> _onOpenLesson;
    private readonly Action _onBack;

    public CourseOverviewViewModel(
        IReadOnlyList<DrivingCourseLesson> lessons,
        IReadOnlyDictionary<string, CourseLessonProgressEntity> progress,
        Action<DrivingCourseLesson> onOpenLesson,
        Action onBack)
    {
        _onOpenLesson = onOpenLesson;
        _onBack = onBack;

        var empfohlen = lessons.FirstOrDefault(lektion =>
            !progress.TryGetValue(lektion.Id, out var stand) || stand.PassedAt is null);

        for (var i = 0; i < lessons.Count; i++)
        {
            var lektion = lessons[i];
            progress.TryGetValue(lektion.Id, out var stand);

            var beispiel = lektion.SignNumbers
                .Select(TrafficSignCatalog.ByNumber)
                .FirstOrDefault(zeichen => zeichen is not null);

            Lessons.Add(new CourseLessonRowViewModel(
                lektion, i + 1, stand, lektion == empfohlen, beispiel));
        }

        DoneCount = Lessons.Count(zeile => zeile.IsDone);
    }

    public ObservableCollection<CourseLessonRowViewModel> Lessons { get; } = new();

    public int DoneCount { get; }

    public int LessonTotal => Lessons.Count;

    public string OverallDisplay =>
        string.Format(LocalizationService.Instance["Fs_CourseOverall"], DoneCount, LessonTotal);

    public double Fraction => LessonTotal == 0 ? 0 : (double)DoneCount / LessonTotal;

    [RelayCommand]
    private void OpenLesson(CourseLessonRowViewModel? row)
    {
        if (row is not null)
        {
            _onOpenLesson(row.Lesson);
        }
    }

    [RelayCommand]
    private void Back() => _onBack();
}
