using System.Globalization;
using System.Linq;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.ContentGen;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Entities;
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
    private const int NewsTargetCount = 31;

    private readonly ProgressGateService _gate;
    private readonly ScoringService _scoring;
    private readonly ProgressRepository _progressRepo;
    private readonly TypingProgressRepository _typingProgressRepo;
    private readonly SettingsRepository _settingsRepo;
    private readonly ActivityLogRepository _activityLogRepo;
    private readonly StudentProfileRepository _profileRepo;
    private readonly CustomQuestionRepository _customQuestionRepo;
    private readonly ReviewQuestionRepository _reviewRepo;
    private readonly MasteredPromptRepository _masteredPromptRepo;
    private readonly ArchivedArticleRepository _archiveRepo;
    private readonly RewardRepository _rewardRepo;
    private readonly RssNewsService _newsService;
    private readonly WeatherService _weatherService;
    private readonly QuizComposer _quizComposer;
    private readonly KioskLockService _kioskLock;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly TextToSpeechService _tts;
    private readonly Random _random = new();

    private readonly DispatcherTimer _clockTimer;
    private readonly TypingExerciseService _typingService;

    [ObservableProperty]
    private object? currentViewModel;

    [ObservableProperty]
    private string currentDateTimeDisplay = string.Empty;

    /// <summary>Name des aktuell aktiven Profils, leer solange noch keins gewählt wurde - dient
    /// der dauerhaft sichtbaren Anzeige im Kiosk-Fenster (siehe MainWindow.xaml).</summary>
    [ObservableProperty]
    private string activeProfileName = string.Empty;

    /// <summary>Etappen-Anzeige der heutigen Session oben im Kiosk-Fenster (Lesen → Tippen → Schreiben → News → Fächer → Quiz).
    /// Wird bei jedem Stufenwechsel komplett neu aufgebaut - vier kurzlebige Objekte pro Wechsel, dafür keine Notify-Logik in den Einträgen nötig.</summary>
    public System.Collections.ObjectModel.ObservableCollection<SessionStepViewModel> SessionSteps { get; } = new();

    public StudentProfile? CurrentProfile { get; private set; }
    public StudentProgress Progress { get; private set; } = new() { ProfileId = string.Empty };
    public AppSettings Settings { get; private set; } = new();

    public MainViewModel(
        ProgressGateService gate,
        ScoringService scoring,
        ProgressRepository progressRepo,
        TypingProgressRepository typingProgressRepo,
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        CustomQuestionRepository customQuestionRepo,
        ReviewQuestionRepository reviewRepo,
        MasteredPromptRepository masteredPromptRepo,
        ArchivedArticleRepository archiveRepo,
        RewardRepository rewardRepo,
        RssNewsService newsService,
        WeatherService weatherService,
        QuizComposer quizComposer,
        KioskLockService kioskLock,
        IHomeworkHelpChatService homeworkChat,
        TextToSpeechService tts,
        TypingExerciseService typingService)
    {
        _weatherService = weatherService;
        _gate = gate;
        _scoring = scoring;
        _progressRepo = progressRepo;
        _typingProgressRepo = typingProgressRepo;
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _profileRepo = profileRepo;
        _customQuestionRepo = customQuestionRepo;
        _reviewRepo = reviewRepo;
        _masteredPromptRepo = masteredPromptRepo;
        _archiveRepo = archiveRepo;
        _rewardRepo = rewardRepo;
        _newsService = newsService;
        _quizComposer = quizComposer;
        _kioskLock = kioskLock;
        _homeworkChat = homeworkChat;
        _tts = tts;
        _typingService = typingService;

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

        var profileSelection = new ProfileSelectionViewModel(_profileRepo, _progressRepo, OnProfileSelected);
        CurrentViewModel = profileSelection;
        await profileSelection.InitializeAsync();
    }

    private async void OnProfileSelected(StudentProfile profile)
    {
        CurrentProfile = profile;
        ActiveProfileName = profile.Name;
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
            LearningStage.Tippen => await BuildTypingDashboardViewModelAsync(),
            LearningStage.News => await BuildNewsViewModelAsync(),
            LearningStage.Abschlussquiz => await BuildFinalQuizViewModelAsync(),
            LearningStage.Freigeschaltet => BuildResultViewModel(passed: true, result: null),
            _ when LearningStageSubjects.TryGetSubject(stage, out var subjectForStage) => await BuildExerciseViewModelAsync(subjectForStage),
            _ => CurrentViewModel
        };

        UpdateSessionSteps();
    }

    /// <summary>Baut die fünf Makro-Etappen (Lesen/Tippen/News/Fächer/Quiz) für die Fortschrittsleiste neu auf.</summary>
    private void UpdateSessionSteps()
    {
        var stage = Progress.CurrentStage;
        var loc = LocalizationService.Instance;
        var activeSubjects = LearningStageSubjects.Map.Values
            .Where(s => !Settings.DisabledSubjects.Contains(s)).ToList();
        var doneSubjects = Progress.CompletedExerciseSubjects.Count(activeSubjects.Contains);
        var isSubjectStage = LearningStageSubjects.Map.ContainsKey(stage);

        SessionSteps.Clear();
        SessionSteps.Add(new SessionStepViewModel
        {
            Label = loc["Steps_Reading"],
            IsDone = stage > LearningStage.Vorlesen,
            IsCurrent = stage == LearningStage.Vorlesen
        });
        SessionSteps.Add(new SessionStepViewModel
        {
            Label = loc["Steps_Typing"],
            IsDone = stage > LearningStage.Tippen,
            IsCurrent = stage == LearningStage.Tippen
        });
        SessionSteps.Add(new SessionStepViewModel
        {
            Label = loc["Steps_News"],
            IsDone = stage > LearningStage.News,
            IsCurrent = stage == LearningStage.News
        });
        SessionSteps.Add(new SessionStepViewModel
        {
            Label = $"{loc["Steps_Subjects"]} {doneSubjects}/{activeSubjects.Count}",
            IsDone = stage >= LearningStage.Abschlussquiz,
            IsCurrent = isSubjectStage
        });
        SessionSteps.Add(new SessionStepViewModel
        {
            Label = loc["Steps_Quiz"],
            IsDone = stage == LearningStage.Freigeschaltet,
            IsCurrent = stage == LearningStage.Abschlussquiz
        });
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
        var today = DateOnly.FromDateTime(DateTime.Now);
        var piece = ReadingContentProvider.GetForDate(today);
        var secondPiece = ReadingContentProvider.GetSecondForDate(today);
        return new ReadingViewModel(piece, secondPiece, OnReadingCompleted, _tts);
    }

    /// <summary>
    /// Schreibt Belohnungs-Sterne gut (Gamification): auf den Tages-Fortschritt (für die
    /// Ergebnis-Anzeige) und dauerhaft aufs Profil. Wird nur aus echten Abschluss-Callbacks
    /// aufgerufen - übersprungene (deaktivierte) Fächer laufen an diesen Callbacks vorbei und
    /// bekommen daher korrekt keine Sterne. Der Tages-Fortschritt wird vom jeweils folgenden
    /// NavigateToStageAsync/PersistProgressAsync mitgespeichert.
    /// </summary>
    private async Task AwardStarsAsync(int amount)
    {
        Progress.EarnedStarsToday += amount;
        CurrentProfile!.TotalStars = await _profileRepo.AddStarsAsync(CurrentProfile.Id, amount);
    }

    private async void OnReadingCompleted()
    {
        Progress.HasCompletedReading = true;
        await AwardStarsAsync(2);
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.Vorlesen));
    }

    private async Task<TypingDashboardViewModel> BuildTypingDashboardViewModelAsync()
    {
        var vm = new TypingDashboardViewModel(CurrentProfile!.Id, CurrentProfile!.Name, _typingProgressRepo, _typingService, lessonId =>
        {
            _ = NavigateToTypingExerciseAsync(lessonId);
        }, () => _ = NavigateToStageAsync(_gate.GetNextStage(LearningStage.Tippen)));
        await vm.InitializeAsync();
        return vm;
    }

    private async Task NavigateToTypingExerciseAsync(string lessonId)
    {
        var lesson = TypingContentProvider.GetLessonById(lessonId);
        if (lesson == null) return;

        var exerciseVm = new TypingExerciseViewModel(
            lesson,
            _typingService,
            _typingProgressRepo,
            CurrentProfile!.Id,
            CurrentProfile!.Name,
            lessonId => OnTypingLessonCompleted(lessonId)
        );
        CurrentViewModel = exerciseVm;
    }

    private async void OnTypingLessonCompleted(string? lessonId)
    {
        // If null, stay on dashboard (next lesson is loaded there via InitializeAsync)
        if (lessonId == null)
        {
            var dashboardVm = await BuildTypingDashboardViewModelAsync();
            CurrentViewModel = dashboardVm;
        }
        else
        {
            // Show completion screen
            var lesson = TypingContentProvider.GetLessonById(lessonId);
            if (lesson != null)
            {
                var progress = await _typingProgressRepo.GetProgressAsync(CurrentProfile!.Id);
                progress.TryGetValue(lessonId, out var lessonProgress);
                var result = new TypingResult
                {
                    Accuracy = lessonProgress?.BestAccuracy ?? 0,
                    Wpm = lessonProgress?.BestWpm ?? 0,
                    CorrectCharacters = lessonProgress?.CorrectCharacters ?? 0,
                    TotalCharacters = lessonProgress?.TotalCharactersTyped ?? 0,
                    Passed = lessonProgress?.IsCompleted ?? false,
                    Elapsed = TimeSpan.Zero
                };
                var completeVm = new TypingLessonCompleteViewModel(
                        lesson,
                        result,
                        () =>
                        {
                            var completedLessonIds = progress.Where(kvp => kvp.Value.IsCompleted).Select(kvp => kvp.Key).ToHashSet();
                            var nextLesson = TypingContentProvider.GetNextUnlockedLesson(completedLessonIds, CurrentProfile!.Name);
                            if (nextLesson != null)
                            {
                                _ = NavigateToTypingExerciseAsync(nextLesson.Id);
                            }
                            else
                            {
                                // All lessons done - go back to dashboard which will show completion
                                _ = NavigateToStageAsync(_gate.GetNextStage(LearningStage.Tippen));
                            }
                        },
                        () => _ = NavigateToTypingExerciseAsync(lessonId)
                    );
                CurrentViewModel = completeVm;
            }
        }
    }

    private async void OnTypingCompleted()
    {
        Progress.HasCompletedTyping = true;
        await AwardStarsAsync(2);
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.Tippen));
    }

    private async Task<NewsViewModel> BuildNewsViewModelAsync()
    {
        // Artikel und Wetter parallel laden - das Wetter-Widget ist Beiwerk und darf den
        // News-Start nicht verzögern; bei Fehlschlag liefert der Dienst null (Widget bleibt weg).
        var gradeLevel = CurrentProfile?.GradeLevel ?? GradeLevel.Klasse6;
        var articlesTask = _newsService.LoadCuratedArticlesAsync(targetCount: NewsTargetCount, childAge: CurrentProfile?.Age, gradeLevel: gradeLevel);
        var weatherTask = _weatherService.LoadBerlinWeatherAsync();
        var articles = await articlesTask;
        var weather = await weatherTask;

        // Offline-Rückfall: sind alle Feeds tot, kommt nur das eingebaute Finanzwissen-Stück
        // zurück (Count <= 1). Dann die Artikel des letzten erfolgreichen Tages aus dem Archiv
        // laden - alte News sind besser als ein fast leerer Pflicht-News-Teil. Bei Erfolg wird
        // der heutige Stand umgekehrt archiviert (idempotent, behält ~7 Tage).
        if (articles.Count <= 1)
        {
            var archived = await _archiveRepo.GetLatestArchiveAsync();
            if (archived.Count > 0)
            {
                Core.Logging.AppLog.Warn("News",
                    $"Keine Feeds erreichbar - Rückfall auf {archived.Count} archivierte Artikel des letzten Tages.");
                articles = archived;
            }
        }
        else
        {
            try
            {
                await _archiveRepo.ArchiveTodayAsync(articles);
            }
            catch (Exception ex)
            {
                // Archivieren ist Komfort - ein Fehler darf den News-Start nicht verhindern.
                Core.Logging.AppLog.Warn("News", $"Tages-Archivierung fehlgeschlagen - {ex.Message}");
            }
        }

        return new NewsViewModel(
            articles, Progress.CompletedNewsArticleIds, OnArticleAnswered, OnNewsSectionCompleted,
            _homeworkChat, weather);
    }

    private async void OnArticleAnswered(NewsArticle article, QuestionOutcome outcome, QuizQuestion question)
    {
        Progress.CompletedNewsArticleIds.Add(article.Id);
        await _activityLogRepo.LogAnswerAsync(CurrentProfile!.Id, outcome, question.Topic, question.Prompt);
        await PersistProgressAsync();
    }

    private async void OnNewsSectionCompleted()
    {
        await AwardStarsAsync(2);
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.News));
    }

    /// <summary>
    /// Fragen, die dem aktuellen Profil innerhalb dieses Zeitraums schon gestellt wurden, werden bei
    /// der Auswahl neuer Übungs-/Quizfragen bevorzugt vermieden (siehe ExerciseGeneratorBase.Generate) -
    /// so wiederholen sich die kleinen, fest hinterlegten Themen-Pools nicht ständig identisch.
    /// </summary>
    private static readonly TimeSpan RecentPromptsWindow = TimeSpan.FromDays(21);

    /// <summary>Angestrebte Gesamtfragenzahl des ersten Abschlussquiz-Versuchs am Tag.</summary>
    private const int InitialQuizTargetQuestions = 20;

    /// <summary>Angestrebte Gesamtfragenzahl bei einer Wiederholung nach nicht bestandenem Quiz -
    /// bewusst kleiner als der erste Versuch (Wiederholung soll nicht wie eine zusätzliche Strafe
    /// wirken), aber mit konzentriert vielen Fragen zu den schwachen Fächern.</summary>
    private const int RetryQuizTargetQuestions = 15;

    /// <summary>
    /// Vereint zwei unabhängige Ausschlussgründe für die Aufgabenauswahl: das 21-Tage-Fenster
    /// kürzlich gestellter Fragen (bevorzugt Frische bei den fest hinterlegten Themen-Pools) und
    /// die dauerhaft gemeisterten Prompts (siehe MasteredPromptRepository - einmal richtig
    /// beantwortete Aufgaben tauchen für dieses Profil nie wieder auf). Beide landen im selben
    /// Ausschluss-Set, weil ExerciseGeneratorBase.Generate ohnehin nur "meide diese Prompts, wenn
    /// möglich" kennt, keinen Unterschied zwischen den beiden Gründen macht.
    /// </summary>
    private async Task<IReadOnlySet<string>> BuildExcludedPromptsAsync()
    {
        var recentlySeen = await _activityLogRepo.GetRecentPromptsAsync(CurrentProfile!.Id, RecentPromptsWindow);
        var mastered = await _masteredPromptRepo.GetMasteredPromptsAsync(CurrentProfile!.Id);

        var excluded = new HashSet<string>(recentlySeen);
        excluded.UnionWith(mastered);
        return excluded;
    }

    private async Task<ExerciseViewModel> BuildExerciseViewModelAsync(Subject subject)
    {
        var grade = CurrentProfile!.GradeLevel;
        var excludedPrompts = await BuildExcludedPromptsAsync();
        var generated = _quizComposer.GenerateExercises(subject, grade, 6, _random, excludedPrompts);
        var custom = await _customQuestionRepo.GetBySubjectAndGradeAsync(subject, grade);

        // Fehler-Kartei: an Vortagen falsch beantwortete Aufgaben dieses Fachs kommen ZUERST
        // (mit 🔁-Thema markiert), bis sie zweimal in Folge richtig beantwortet wurden. Zufällige
        // Dubletten aus dem Generator werden über den Aufgabentext aussortiert.
        var review = await _reviewRepo.GetDueQuestionsAsync(CurrentProfile!.Id, subject, maxCount: 3);
        var reviewPrompts = review.Select(r => r.Prompt).ToHashSet();

        var questions = review
            .Concat(generated.Concat(custom)
                .Where(q => !reviewPrompts.Contains(q.Prompt))
                .OrderBy(_ => _random.Next()))
            .ToList();

        return new ExerciseViewModel(subject, questions, OnExerciseQuestionAnswered, () => OnExerciseSubjectCompleted(subject), _homeworkChat);
    }

    private async void OnExerciseQuestionAnswered(Subject subject, QuestionOutcome outcome, QuizQuestion question)
    {
        await _activityLogRepo.LogAnswerAsync(CurrentProfile!.Id, outcome, question.Topic, question.Prompt);
        // Fehler-Kartei pflegen: falsch → aufnehmen/zurücksetzen, richtig → Streak hoch, bei 2 gelernt.
        await _reviewRepo.RecordOutcomeAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
        // Richtig beantwortet → dauerhaft ausschließen, damit dieser Prompt nie wieder vorkommt.
        await _masteredPromptRepo.RecordIfCorrectAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
    }

    private async void OnExerciseSubjectCompleted(Subject subject)
    {
        Progress.CompletedExerciseSubjects.Add(subject);
        await AwardStarsAsync(1);
        var currentStage = Progress.CurrentStage;
        await NavigateToStageAsync(_gate.GetNextStage(currentStage));
    }

    private async Task<FinalQuizViewModel> BuildFinalQuizViewModelAsync()
    {
        var grade = CurrentProfile!.GradeLevel;
        var disabledSubjects = Settings.DisabledSubjects;
        var relevantSubjects = Progress.SubjectsToRetry.Count > 0 ? Progress.SubjectsToRetry : null;
        var excludedPrompts = await BuildExcludedPromptsAsync();
        IEnumerable<QuizQuestion> questions;

        if (relevantSubjects is not null)
        {
            // Schwache Fächer bekommen konzentriert Fragen (mind. 2 je Fach) aus dem
            // 15er-Zielbudget; der Rest füllt ein allgemeines Mini-Quiz über alle aktiven Fächer
            // auf - genau wie beim ersten Versuch passt sich die Fragenzahl je Fach dynamisch an
            // die Anzahl aktiver Fächer an (siehe ComposeFinalQuiz).
            var perWeakSubjectCount = Math.Max(2, RetryQuizTargetQuestions * 2 / 3 / relevantSubjects.Count);
            var retryQuestions = _quizComposer.ComposeRetryExercises(
                relevantSubjects, grade, _random, countPerSubject: perWeakSubjectCount, recentlySeenPrompts: excludedPrompts);

            var topUpTarget = Math.Max(RetryQuizTargetQuestions - retryQuestions.Count, 1);
            var topUpQuestions = _quizComposer.ComposeFinalQuiz(
                grade, _random, disabledSubjects, targetTotalQuestions: topUpTarget, recentlySeenPrompts: excludedPrompts);

            questions = retryQuestions.Concat(topUpQuestions);
        }
        else
        {
            questions = _quizComposer.ComposeFinalQuiz(
                grade, _random, disabledSubjects,
                targetTotalQuestions: InitialQuizTargetQuestions, recentlySeenPrompts: excludedPrompts);
        }

        // Eigene (von den Eltern eingetragene) Aufgaben ergänzen additiv - unabhängig vom
        // dynamischen Fragenbudget der generierten Fächer.
        var customQuestions = (await _customQuestionRepo.GetByGradeAsync(grade))
            .Where(q => !disabledSubjects.Contains(q.Subject));

        var finalQuestions = questions.Concat(customQuestions).OrderBy(_ => _random.Next()).ToList();

        return new FinalQuizViewModel(finalQuestions, OnFinalQuizCompleted, OnFinalQuizQuestionAnswered, _homeworkChat);
    }

    private async void OnFinalQuizQuestionAnswered(QuizQuestion question, QuestionOutcome outcome)
    {
        // Richtig beantwortet → dauerhaft ausschließen, damit dieser Prompt nie wieder vorkommt -
        // auch wenn er nur im Abschlussquiz und nie in einer Übung drankam.
        await _masteredPromptRepo.RecordIfCorrectAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
    }

    private async void OnFinalQuizCompleted(IReadOnlyList<QuestionOutcome> outcomes)
    {
        // Ein zweiter Anlauf (Wiederholung nach nicht bestandenem ersten Versuch) schaltet danach
        // in jedem Fall frei - die 50%-Hürde gilt nur beim allerersten Versuch am Tag. Muss VOR
        // ApplyQuizResult geprüft werden, da dieses Progress.SubjectsToRetry beim Bestehen leert.
        var isRetryAttempt = Progress.SubjectsToRetry.Count > 0;

        var result = _scoring.BuildResult(outcomes);
        await _activityLogRepo.LogQuizAttemptAsync(CurrentProfile!.Id, result);
        _gate.ApplyQuizResult(Progress, result, isRetryAttempt);

        if (Progress.IsUnlocked)
        {
            // Quiz-Sterne vor dem Persistieren gutschreiben, damit EarnedStarsToday mitgespeichert
            // wird. Kann pro Tag nur einmal passieren: nach dem Freischalten steht der Fortschritt
            // auf Freigeschaltet, ein weiterer Quiz-Durchlauf findet heute nicht mehr statt.
            await AwardStarsAsync(5);
        }

        await PersistProgressAsync();
        UpdateSessionSteps();

        if (Progress.IsUnlocked)
        {
            _kioskLock.Unlock();
        }

        CurrentViewModel = BuildResultViewModel(Progress.IsUnlocked, result);
    }

    private ResultViewModel BuildResultViewModel(bool passed, QuizResult? result)
    {
        return new ResultViewModel(
            passed, result, Progress.EarnedStarsToday, CurrentProfile?.TotalStars ?? 0,
            OnRetryWeakSubjectsRequested, OnUnlockConfirmed,
            _rewardRepo, CurrentProfile?.Id);
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