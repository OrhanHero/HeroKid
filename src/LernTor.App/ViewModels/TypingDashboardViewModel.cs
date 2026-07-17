using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Repositories;

namespace LernTor.App.ViewModels;

/// <summary>
/// Dashboard für den 10-Finger-Tipp-Trainer: Übersicht aller Lektionen, Fortschrittsring, Start-Button.
/// </summary>
public sealed partial class TypingDashboardViewModel : ObservableObject
{
    private readonly TypingProgressRepository _progressRepo;
    private readonly TypingExerciseService _service;
    private readonly Action<string> _onLessonSelected;
    private readonly Action _onContinueToNews;
    private readonly string _profileId;
    private readonly string _profileName;

    public TypingDashboardViewModel(
        string profileId,
        string profileName,
        TypingProgressRepository progressRepo,
        TypingExerciseService service,
        Action<string> onLessonSelected,
        Action onContinueToNews)
    {
        _profileId = profileId;
        _profileName = profileName;
        _progressRepo = progressRepo;
        _service = service;
        _onLessonSelected = onLessonSelected;
        _onContinueToNews = onContinueToNews;
        Lessons = [];
    }

    public IReadOnlyList<TypingLesson> AllLessons { get; } = TypingContentProvider.GetAllLessons();

    [ObservableProperty]
    private IReadOnlyList<TypingLessonViewModel> lessons = [];

    [ObservableProperty]
    private double overallProgress;

    [ObservableProperty]
    private int completedCount;

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    private int earnedStars;

    public bool HasCompletedAll => CompletedCount == TotalCount;

    public async Task InitializeAsync()
    {
        var dashboardData = await _service.GetDashboardDataAsync(_profileId, _profileName);

        // Map LessonState to TypingLessonViewModel
        var lessonVMs = dashboardData.Lessons.Select(ls =>
        {
            var vm = new TypingLessonViewModel(ls.Lesson, null)
            {
                IsCompleted = ls.IsCompleted,
                BestAccuracy = ls.BestAccuracy,
                BestWpm = ls.BestWpm,
                StarsEarned = ls.StarsEarned,
                AttemptCount = ls.AttemptCount
            };
            vm.SetUnlocked(ls.IsUnlocked);
            return vm;
        }).ToList();

        Lessons = lessonVMs;
        TotalCount = dashboardData.TotalLessons;
        CompletedCount = dashboardData.CompletedCount;
        EarnedStars = lessonVMs.Sum(l => l.StarsEarned);
        OverallProgress = TotalCount > 0 ? (double)CompletedCount / TotalCount : 0.0;

        OnPropertyChanged(nameof(HasCompletedAll));
    }

    [RelayCommand]
    private void StartLesson(string lessonId)
    {
        _onLessonSelected(lessonId);
    }

    [RelayCommand]
    private void ContinueToNews()
    {
        _onContinueToNews();
    }
}

/// <summary>
/// ViewModel für eine einzelne Lektion in der Dashboard-Übersicht.
/// </summary>
public sealed partial class TypingLessonViewModel : ObservableObject
{
    public TypingLesson Lesson { get; }

    public TypingLessonViewModel(TypingLesson lesson, TypingLessonProgressEntity? progress)
    {
        Lesson = lesson;
        Id = lesson.Id;
        Title = lesson.Title;
        LessonType = lesson.LessonType;
        IsCompleted = progress?.IsCompleted ?? false;
        BestAccuracy = progress?.BestAccuracy ?? 0.0;
        BestWpm = progress?.BestWpm ?? 0.0;
        StarsEarned = progress?.StarsEarned ?? 0;
        AttemptCount = progress?.AttemptCount ?? 0;
    }

    public string Id { get; }
    public string Title { get; }
    public TypingLessonType LessonType { get; }

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private double bestAccuracy;

    [ObservableProperty]
    private double bestWpm;

    [ObservableProperty]
    private int starsEarned;

    [ObservableProperty]
    private int attemptCount;

    public string StatusIcon => IsCompleted ? "✅" : "🔒";
    public string StatusText => IsCompleted ? "✅ Abgeschlossen" : "🔒 Gesperrt";
    public string AccuracyDisplay => BestAccuracy > 0 ? $"{BestAccuracy:P0}" : "–";
    public string WpmDisplay => BestWpm > 0 ? $"{BestWpm:F0} WPM" : "–";

    public bool IsUnlocked { get; private set; }

    public void SetUnlocked(bool unlocked) => IsUnlocked = unlocked;
}