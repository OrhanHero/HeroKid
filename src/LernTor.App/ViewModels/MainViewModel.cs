using System.Globalization;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.App.Localization;
using LernTor.ContentGen;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Repositories;
using LernTor.News;
using LernTor.Security;

namespace LernTor.App.ViewModels;

/// <summary>
/// Navigations-Host: zeigt zunächst die Profilauswahl, danach hält es je gewähltem Kind-Profil
/// den Lernfortschritt und wechselt je nach <see cref="LearningStage"/> das angezeigte
/// Kind-ViewModel. Kind-ViewModels melden ihren Abschluss über Callbacks zurück.
/// </summary>
public sealed partial class MainViewModel : ObservableObject
{
    private readonly ProgressGateService _gate;
    private readonly ScoringService _scoring;
    private readonly ProgressRepository _progressRepo;
    private readonly SettingsRepository _settingsRepo;
    private readonly ActivityLogRepository _activityLogRepo;
    private readonly StudentProfileRepository _profileRepo;
    private readonly CustomQuestionRepository _customQuestionRepo;
    private readonly RssNewsService _newsService;
    private readonly QuizComposer _quizComposer;
    private readonly KioskLockService _kioskLock;
    private readonly Random _random = new();

    private readonly List<QuizQuestion> _collectedNewsQuestions = new();
    private readonly DispatcherTimer _clockTimer;

    [ObservableProperty]
    private object? currentViewModel;

    [ObservableProperty]
    private string currentDateTimeDisplay = string.Empty;

    public StudentProfile? CurrentProfile { get; private set; }
    public StudentProgress Progress { get; private set; } = new() { ProfileId = string.Empty };
    public AppSettings Settings { get; private set; } = new();

    public MainViewModel(
        ProgressGateService gate,
        ScoringService scoring,
        ProgressRepository progressRepo,
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        CustomQuestionRepository customQuestionRepo,
        RssNewsService newsService,
        QuizComposer quizComposer,
        KioskLockService kioskLock)
    {
        _gate = gate;
        _scoring = scoring;
        _progressRepo = progressRepo;
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _profileRepo = profileRepo;
        _customQuestionRepo = customQuestionRepo;
        _newsService = newsService;
        _quizComposer = quizComposer;
        _kioskLock = kioskLock;

        // Zeigt Datum/Uhrzeit im Kiosk-Fenster an - nutzt die lokale PC-Systemuhr (DateTime.Now),
        // keine Netzwerkzeit.
        UpdateClock();
        _clockTimer = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
    }

    private void UpdateClock()
    {
        CurrentDateTimeDisplay = DateTime.Now.ToString("dddd, d. MMMM yyyy – HH:mm:ss", CultureInfo.GetCultureInfo("de-DE"));
    }

    public async Task InitializeAsync()
    {
        Settings = await _settingsRepo.LoadAsync();
        LocalizationService.Instance.CurrentLanguage = Settings.DefaultLanguage;

        var profileSelection = new ProfileSelectionViewModel(_profileRepo, OnProfileSelected);
        CurrentViewModel = profileSelection;
        await profileSelection.InitializeAsync();
    }

    private async void OnProfileSelected(StudentProfile profile)
    {
        CurrentProfile = profile;
        Progress = await _progressRepo.LoadOrCreateTodayAsync(profile.Id);
        await NavigateToStageAsync(Progress.CurrentStage);
    }

    private async Task PersistProgressAsync()
    {
        await _progressRepo.SaveAsync(Progress);
    }

    private async Task NavigateToStageAsync(LearningStage stage)
    {
        // Automatisch deaktivierte Fachbereiche überspringen.
        while (TryGetSubjectForStage(stage, out var disabledSubject) && Settings.DisabledSubjects.Contains(disabledSubject))
        {
            Progress.CompletedExerciseSubjects.Add(disabledSubject);
            stage = _gate.GetNextStage(stage);
        }

        Progress.CurrentStage = stage;
        await PersistProgressAsync();

        CurrentViewModel = stage switch
        {
            LearningStage.Willkommen => new WelcomeViewModel(CurrentProfile!.Name, OnWelcomeContinue, SwitchLanguage),
            LearningStage.Vorlesen => BuildReadingViewModel(),
            LearningStage.News => await BuildNewsViewModelAsync(),
            LearningStage.Abschlussquiz => await BuildFinalQuizViewModelAsync(),
            LearningStage.Freigeschaltet => BuildResultViewModel(passed: true, result: null),
            _ when LearningStageSubjects.TryGetSubject(stage, out var subjectForStage) => await BuildExerciseViewModelAsync(subjectForStage),
            _ => CurrentViewModel
        };
    }

    private static bool TryGetSubjectForStage(LearningStage stage, out Subject subject) =>
        LearningStageSubjects.TryGetSubject(stage, out subject);

    private void SwitchLanguage(AppLanguage language)
    {
        LocalizationService.Instance.CurrentLanguage = language;
        Settings.DefaultLanguage = language;
        _ = _settingsRepo.SaveAsync(Settings);
    }

    private async void OnWelcomeContinue()
    {
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.Willkommen));
    }

    private ReadingViewModel BuildReadingViewModel()
    {
        var piece = ReadingContentProvider.GetForDate(DateOnly.FromDateTime(DateTime.Now));
        return new ReadingViewModel(piece, OnReadingCompleted);
    }

    private async void OnReadingCompleted()
    {
        Progress.HasCompletedReading = true;
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.Vorlesen));
    }

    private async Task<NewsViewModel> BuildNewsViewModelAsync()
    {
        var articles = await _newsService.LoadCuratedArticlesAsync();
        return new NewsViewModel(articles, Progress.CompletedNewsArticleIds, OnArticleAnswered, OnNewsSectionCompleted);
    }

    private async void OnArticleAnswered(NewsArticle article, QuestionOutcome outcome, QuizQuestion question)
    {
        Progress.CompletedNewsArticleIds.Add(article.Id);
        await _activityLogRepo.LogAnswerAsync(CurrentProfile!.Id, outcome, question.Topic, question.Prompt);
        await PersistProgressAsync();
    }

    private async void OnNewsSectionCompleted(IReadOnlyList<QuizQuestion> askedQuestions)
    {
        _collectedNewsQuestions.Clear();
        _collectedNewsQuestions.AddRange(askedQuestions);
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.News));
    }

    private async Task<ExerciseViewModel> BuildExerciseViewModelAsync(Subject subject)
    {
        var grade = CurrentProfile!.GradeLevel;
        var generated = _quizComposer.GenerateExercises(subject, grade, 6, _random);
        var custom = await _customQuestionRepo.GetBySubjectAndGradeAsync(subject, grade);
        var questions = generated.Concat(custom).OrderBy(_ => _random.Next()).ToList();
        return new ExerciseViewModel(subject, questions, OnExerciseQuestionAnswered, () => OnExerciseSubjectCompleted(subject));
    }

    private async void OnExerciseQuestionAnswered(Subject subject, QuestionOutcome outcome, QuizQuestion question)
    {
        await _activityLogRepo.LogAnswerAsync(CurrentProfile!.Id, outcome, question.Topic, question.Prompt);
    }

    private async void OnExerciseSubjectCompleted(Subject subject)
    {
        Progress.CompletedExerciseSubjects.Add(subject);
        var currentStage = Progress.CurrentStage;
        await NavigateToStageAsync(_gate.GetNextStage(currentStage));
    }

    private async Task<FinalQuizViewModel> BuildFinalQuizViewModelAsync()
    {
        var grade = CurrentProfile!.GradeLevel;
        var disabledSubjects = Settings.DisabledSubjects;
        var relevantSubjects = Progress.SubjectsToRetry.Count > 0 ? Progress.SubjectsToRetry : null;
        IEnumerable<QuizQuestion> questions;

        if (relevantSubjects is not null)
        {
            questions = _quizComposer.ComposeRetryExercises(relevantSubjects, grade, _random, countPerSubject: 6)
                .Concat(_quizComposer.ComposeFinalQuiz(grade, _random, null, disabledSubjects, targetTotalQuestions: 10));
        }
        else
        {
            questions = _quizComposer.ComposeFinalQuiz(grade, _random, _collectedNewsQuestions, disabledSubjects, targetTotalQuestions: 22);
        }

        // Eigene (von den Eltern eingetragene) Aufgaben ergänzen additiv - unabhängig vom
        // dynamischen Fragenbudget der generierten Fächer.
        var customQuestions = (await _customQuestionRepo.GetByGradeAsync(grade))
            .Where(q => !disabledSubjects.Contains(q.Subject));

        var finalQuestions = questions.Concat(customQuestions).OrderBy(_ => _random.Next()).ToList();

        return new FinalQuizViewModel(finalQuestions, OnFinalQuizCompleted);
    }

    private async void OnFinalQuizCompleted(IReadOnlyList<QuestionOutcome> outcomes)
    {
        var result = _scoring.BuildResult(outcomes);
        await _activityLogRepo.LogQuizAttemptAsync(CurrentProfile!.Id, result);
        _gate.ApplyQuizResult(Progress, result);
        await PersistProgressAsync();

        if (result.Passed)
        {
            _kioskLock.Unlock();
        }

        CurrentViewModel = BuildResultViewModel(result.Passed, result);
    }

    private ResultViewModel BuildResultViewModel(bool passed, QuizResult? result)
    {
        return new ResultViewModel(passed, result, OnRetryWeakSubjectsRequested, OnUnlockConfirmed);
    }

    private async void OnRetryWeakSubjectsRequested()
    {
        await NavigateToStageAsync(LearningStage.Abschlussquiz);
    }

    private void OnUnlockConfirmed()
    {
        System.Windows.Application.Current.Shutdown();
    }
}
