using System.Globalization;
using System.Linq;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly ProgressGateService _gate;
    private readonly ScoringService _scoring;
    private readonly ProgressRepository _progressRepo;
    private readonly TypingProgressRepository _typingProgressRepo;
    private readonly SettingsRepository _settingsRepo;
    private readonly ActivityLogRepository _activityLogRepo;
    private readonly StudentProfileRepository _profileRepo;
    private readonly CustomQuestionRepository _customQuestionRepo;
    private readonly CustomReadingTextRepository _customReadingRepo;
    private readonly VocabularyRepository _vocabularyRepo;
    private readonly ReviewQuestionRepository _reviewRepo;
    private readonly MasteredPromptRepository _masteredPromptRepo;
    private readonly ArchivedArticleRepository _archiveRepo;
    private readonly HomeworkTaskRepository _homeworkRepo;
    private readonly ExamEntryRepository _examRepo;
    private readonly TrafficSignProgressRepository _signProgressRepo;
    private readonly TheoryProgressRepository _theoryRepo;
    private readonly CourseProgressRepository _courseRepo;
    private readonly TimetableRepository _timetableRepo;
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
        CustomReadingTextRepository customReadingRepo,
        VocabularyRepository vocabularyRepo,
        ReviewQuestionRepository reviewRepo,
        MasteredPromptRepository masteredPromptRepo,
        ArchivedArticleRepository archiveRepo,
        HomeworkTaskRepository homeworkRepo,
        ExamEntryRepository examRepo,
        TrafficSignProgressRepository signProgressRepo,
        TheoryProgressRepository theoryRepo,
        CourseProgressRepository courseRepo,
        TimetableRepository timetableRepo,
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
        _customReadingRepo = customReadingRepo;
        _vocabularyRepo = vocabularyRepo;
        _reviewRepo = reviewRepo;
        _masteredPromptRepo = masteredPromptRepo;
        _archiveRepo = archiveRepo;
        _homeworkRepo = homeworkRepo;
        _examRepo = examRepo;
        _signProgressRepo = signProgressRepo;
        _theoryRepo = theoryRepo;
        _courseRepo = courseRepo;
        _timetableRepo = timetableRepo;
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

    /// <summary>
    /// Hinweis, dass die Kiosk-Sperre gerade NICHT aktiv ist - leer, wenn gesperrt.
    ///
    /// <para><b>Warum das sichtbar sein muss:</b> ohne Sperre lässt sich das Fenster ganz normal
    /// schließen, auch über das „X" in der Alt+Tab-Vorschau (<c>MainWindow.Closing</c> blockiert
    /// nur, solange <c>IsLocked</c> gilt - und das ist richtig so). Nur SAH man der App das nicht
    /// an: gesperrt und ungesperrt sehen identisch aus. Ein Elternteil hält den PC dann für
    /// gesichert, obwohl er es nicht ist - und falsches Vertrauen ist schlimmer als gar keine
    /// Sperre. Genau diese Verwechslung ist beim Testen aufgetreten.</para>
    /// </summary>
    public string KioskSkipNotice =>
        _kioskLock.SkipReason.Length == 0
            ? string.Empty
            : $"🔓 Kiosk-Sperre ist AUS: {_kioskLock.SkipReason}. Das Fenster lässt sich normal schließen.";

    private void UpdateClock()
    {
        CurrentDateTimeDisplay = DateTime.Now.ToString("dddd, d. MMMM yyyy – HH:mm:ss", CultureInfo.GetCultureInfo("de-DE"));
    }

    public async Task InitializeAsync()
    {
        Settings = await _settingsRepo.LoadAsync();
        LocalizationService.Instance.CurrentLanguage = Settings.DefaultLanguage;

        // Ferien-/Pausenmodus vor allem anderen: sonst landet das Kind trotz laufender Pause in
        // der gewohnten Lernstrecke und muss bis zum Abschlussquiz durch, um den PC zu bekommen -
        // genau der Fehler, den dieser Zweig behebt. Die Kiosk-Sperre wurde in diesem Fall schon
        // beim Start uebersprungen (siehe App.xaml.cs), nur die Oberfläche wusste nichts davon.
        if (PauseMode.IsActive(Settings.PauseUntilDate, DateOnly.FromDateTime(DateTime.Today)))
        {
            ShowPauseScreen();
            return;
        }

        await ShowProfileSelectionAsync();
    }

    private void ShowPauseScreen()
    {
        // ActiveProfileName leeren: die Etappen-Anzeige oben ("Lesen → Tippen → …") wuerde sonst
        // nach einem "Trotzdem ueben" und einem spaeteren Zurueck weiter mitlaufen, obwohl gerade
        // gar keine Lernstrecke aktiv ist.
        ActiveProfileName = string.Empty;
        SessionSteps.Clear();

        CurrentViewModel = new PauseModeViewModel(
            Settings.PauseUntilDate!.Value,
            DateOnly.FromDateTime(DateTime.Today),
            OnPauseUnlockRequested,
            () => _ = ShowProfileSelectionAsync());
    }

    private async Task ShowProfileSelectionAsync()
    {
        var profileSelection = new ProfileSelectionViewModel(_profileRepo, _progressRepo, OnProfileSelected);
        CurrentViewModel = profileSelection;
        await profileSelection.InitializeAsync();
    }

    /// <summary>
    /// "PC entsperren" auf dem Ferien-Bildschirm: LernTor beendet sich, der Rechner gehoert dem
    /// Kind. <see cref="KioskLockService.Unlock"/> laeuft vorher, obwohl im Pausenmodus gar nicht
    /// gesperrt wurde - <c>MainWindow.Closing</c> bricht jedes Schliessen ab, solange
    /// <c>IsLocked</c> wahr ist, und diese Reihenfolge ist die Regel fuer JEDEN Beendigungsweg.
    /// </summary>
    private void OnPauseUnlockRequested()
    {
        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    private async void OnProfileSelected(StudentProfile profile)
    {
        CurrentProfile = profile;
        ActiveProfileName = profile.Name;
        Progress = await _progressRepo.LoadOrCreateTodayAsync(profile.Id);
        await NavigateToStageAsync(Progress.CurrentStage);
    }

    /// <summary>
    /// Liest die globalen Einstellungen und das aktive Profil nach dem Schließen des
    /// Eltern-Bereichs neu aus der Datenbank.
    ///
    /// <para>Der Eltern-Bereich arbeitet auf eigenen Kopien (eigenes <c>AppSettings</c>, eigene
    /// <c>StudentProfile</c>-Objekte aus <c>GetAllAsync</c>). Ohne dieses Nachladen lief die
    /// bereits gestartete Sitzung mit den Werten vom App-Start weiter - eine im Eltern-Bereich
    /// geänderte Lesezeit oder ein abgeschaltetes Fach wirkte erst nach einem Neustart der App,
    /// obwohl in der Datenbank längst der neue Wert stand. Das sah von außen exakt so aus, als
    /// wäre gar nicht gespeichert worden.</para>
    ///
    /// <para>Die gerade sichtbare Etappe behält bewusst ihre Werte (ein laufender Lese-Timer wird
    /// nicht mitten im Lauf umgestellt) - ab der nächsten Etappe gelten die neuen.</para>
    /// </summary>
    public async Task ReloadSettingsAndProfileAsync()
    {
        Settings = await _settingsRepo.LoadAsync();

        // Eltern haben die Pause im Eltern-Bereich gerade beendet: der Ferien-Bildschirm samt
        // Entsperren-Knopf muss sofort weg, sonst koennte das Kind den PC weiterhin freigeben.
        if (CurrentViewModel is PauseModeViewModel
            && !PauseMode.IsActive(Settings.PauseUntilDate, DateOnly.FromDateTime(DateTime.Today)))
        {
            await ShowProfileSelectionAsync();
            return;
        }

        // Umgekehrt: Pause gerade eingetragen, aber noch kein Kind am Lernen - dann direkt auf den
        // Ferien-Bildschirm. Eine bereits laufende Lernstrecke wird bewusst NICHT unterbrochen;
        // die Pause greift dort ab dem naechsten Start.
        if (CurrentViewModel is ProfileSelectionViewModel
            && PauseMode.IsActive(Settings.PauseUntilDate, DateOnly.FromDateTime(DateTime.Today)))
        {
            ShowPauseScreen();
            return;
        }

        if (CurrentProfile is null)
        {
            return;
        }

        var refreshed = (await _profileRepo.GetAllAsync()).FirstOrDefault(p => p.Id == CurrentProfile.Id);
        if (refreshed is not null)
        {
            CurrentProfile = refreshed;
            ActiveProfileName = refreshed.Name;
        }
    }

    private async Task PersistProgressAsync()
    {
        await _progressRepo.SaveAsync(Progress);
    }

    private async Task NavigateToStageAsync(LearningStage stage)
    {
        // Automatisch deaktivierte Fachbereiche überspringen.
        while (TryGetSubjectForStage(stage, out var disabledSubject) && IsSubjectDisabled(disabledSubject))
        {
            Progress.CompletedExerciseSubjects.Add(disabledSubject);
            stage = _gate.GetNextStage(stage);
        }

        Progress.CurrentStage = stage;
        await PersistProgressAsync();

        CurrentViewModel = stage switch
        {
            LearningStage.Willkommen => await BuildWelcomeViewModelAsync(),
            LearningStage.Vorlesen => await BuildReadingViewModelAsync(),
            LearningStage.Tippen => await BuildTypingDashboardViewModelAsync(),
            LearningStage.News => await BuildNewsViewModelAsync(),
            LearningStage.Fuehrerschein => await BuildDrivingDashboardViewModelAsync(),
            LearningStage.Abschlussquiz => await BuildFinalQuizViewModelAsync(),
            LearningStage.Freigeschaltet => await BuildResultViewModelAsync(passed: true, result: null),
            // KI-Bereich: Lernmodule + Checkliste in eigener View; die Fragen laufen darin als
            // ganz normales ExerciseViewModel (Fortschritt/Fehler-Kartei/Spaced Repetition inklusive).
            LearningStage.KiWissen => new KiBereichViewModel(await BuildExerciseViewModelAsync(Subject.KiWissen)),
            _ when LearningStageSubjects.TryGetSubject(stage, out var subjectForStage) => await BuildExerciseViewModelAsync(subjectForStage),
            _ => CurrentViewModel
        };

        UpdateSessionSteps();
    }

    /// <summary>
    /// Willkommensbildschirm inkl. optionaler 🔥-Lernserie: nur berechnet, wenn Eltern die
    /// Streak-Anzeige eingeschaltet haben (Standard aus - bewusst kein Streak-Druck), sonst 0
    /// (WelcomeViewModel blendet die Zeile dann aus).
    /// </summary>
    /// <param name="plannerPeek">True, wenn die Ansicht als Zwischenstopp aus einer laufenden
    /// Etappe heraus gezeigt wird - dann führt der große Knopf zurück in die Etappe statt in den
    /// Tagesablauf.</param>
    private async Task<WelcomeViewModel> BuildWelcomeViewModelAsync(bool plannerPeek = false)
    {
        var streak = 0;
        if (Settings.StreaksEnabled)
        {
            var learningDays = await _activityLogRepo.GetLearningDaysAsync(CurrentProfile!.Id);
            streak = StreakCalculator.CurrentStreak(learningDays, DateOnly.FromDateTime(DateTime.Today));
        }

        // Fehler-Kartei sichtbar machen: die Kinder sollen wissen, dass falsch beantwortete
        // Fragen wiederkommen, bevor sie in die Fächer gehen - nicht erst, wenn sie dort auftauchen.
        var dueReviews = await _reviewRepo.GetDueCountAsync(CurrentProfile!.Id);

        // Wochenziel (0 = aus): reine Anzeige, siehe WeeklyGoalCalculator.
        var weeklyGoal = WeeklyGoalCalculator.Evaluate(
            await _activityLogRepo.GetLearningDaysAsync(CurrentProfile!.Id),
            DateOnly.FromDateTime(DateTime.Today),
            CurrentProfile!.WeeklyGoalDays);

        // Hausaufgaben der Eltern: nur die aktuell relevanten (offene bis zwei Wochen nach
        // Stichtag, frisch abgehakte noch kurz) - siehe HomeworkTask.IsVisibleTo.
        var today = DateOnly.FromDateTime(DateTime.Today);
        var homework = (await _homeworkRepo.GetVisibleForProfileAsync(CurrentProfile!.Id, today))
            .Select(task => new HomeworkItemViewModel(task, today, OnHomeworkCompletedChanged))
            .ToList();

        // Klausuren mit Countdown. Die Kinder duerfen selbst eintragen - wer den Termin selbst
        // eintraegt, nimmt ihn eher ernst als einen, der ihm hingestellt wird.
        var exams = (await _examRepo.GetVisibleForProfileAsync(CurrentProfile!.Id, today))
            .Select(exam => new ExamItemViewModel(
                exam, today, LocalizationService.Instance[$"Stage_{exam.Subject}"]))
            .ToList();

        // Stundenplan des Kindes - links auf der Startseite. Pro Profil, samt eigenem Zeitraster:
        // die beiden gehen auf verschiedene Schulen mit verschiedenen Anfangs- und Endzeiten.
        var timetable = await _timetableRepo.GetForProfileAsync(CurrentProfile!.Id);

        return new WelcomeViewModel(
            CurrentProfile!.Name, streak,
            // new Action(...) ausdruecklich: zwei Methodengruppen in einem ?: haben keinen
            // eigenen Typ, und darauf zu bauen, dass der Zieltyp es rettet, hat diese Codebasis
            // schon einen CI-Durchlauf gekostet.
            plannerPeek ? new Action(ClosePlanner) : OnWelcomeContinue,
            SwitchLanguage, dueReviews, weeklyGoal,
            homework, exams, OnAddExamRequested, OnDeleteExamRequested,
            OnAddHomeworkRequested, OnDeleteHomeworkRequested,
            plannerPeek, today, timetable,
            // Nur für die Beschriftung des Rück-Knopfes: vom Geschafft-Bildschirm aus geht es
            // nicht "zurück zum Lernen", sondern zurück zum Ergebnis.
            dayIsDone: Progress.CurrentStage == LearningStage.Freigeschaltet);
    }

    /// <summary>
    /// Hausaufgaben-Eingabe aus der Kind-Ansicht. Der Eintrag wird als vom KIND angelegt vermerkt -
    /// nur solche darf es spaeter auch wieder loeschen. Ein Stichtag in der Vergangenheit ist
    /// hier ausdruecklich erlaubt: eine vergessene Hausaufgabe nachzutragen ist ein normaler Fall.
    /// </summary>
    private async void OnAddHomeworkRequested()
    {
        var dialog = new Views.HomeworkEntryDialog
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        await _homeworkRepo.AddAsync(
            CurrentProfile!.Id,
            dialog.SelectedSubject,
            dialog.EnteredDescription,
            dialog.SelectedDate,
            EntryAuthor.Kind);

        CurrentViewModel = await BuildWelcomeViewModelAsync();
    }

    /// <summary>
    /// Loeschen aus der Kind-Ansicht. Eltern-Eintraege bleiben stehen - ABHAKEN darf das Kind sie
    /// jederzeit, das ist ja der Sinn; nur wegraeumen, was es nicht erledigt hat, waere ein
    /// Schlupfloch. Die Pruefung sitzt im Repository, nicht nur an der Oberflaeche.
    /// </summary>
    private async void OnDeleteHomeworkRequested(HomeworkItemViewModel item)
    {
        if (!await _homeworkRepo.DeleteAsChildAsync(item.Id))
        {
            System.Windows.MessageBox.Show(
                "Diese Hausaufgabe haben deine Eltern eingetragen - die kannst du nur abhaken, nicht löschen.",
                "Nicht möglich",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            return;
        }

        CurrentViewModel = await BuildWelcomeViewModelAsync();
    }

    /// <summary>
    /// Klausur-Eingabe aus der Kind-Ansicht. Der Eintrag wird als vom KIND angelegt vermerkt -
    /// nur solche darf das Kind spaeter auch wieder loeschen.
    /// </summary>
    private async void OnAddExamRequested()
    {
        var dialog = new Views.ExamEntryDialog
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        await _examRepo.AddAsync(
            CurrentProfile!.Id,
            dialog.SelectedSubject,
            dialog.EnteredTitle,
            dialog.EnteredTopics,
            dialog.SelectedDate,
            EntryAuthor.Kind);

        // Neu aufbauen, damit Countdown und Reihenfolge sofort stimmen.
        CurrentViewModel = await BuildWelcomeViewModelAsync();
    }

    /// <summary>
    /// Loeschen aus der Kind-Ansicht. Eltern-Eintraege bleiben unangetastet - die Pruefung sitzt
    /// im Repository (DeleteAsChildAsync), nicht nur an der Oberflaeche, damit sie nicht an einem
    /// vergessenen Sichtbarkeits-Flag haengt.
    /// </summary>
    private async void OnDeleteExamRequested(ExamItemViewModel item)
    {
        var confirmed = System.Windows.MessageBox.Show(
            $"Termin \"{item.Title}\" wirklich löschen?",
            "Klausur löschen",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Question,
            System.Windows.MessageBoxResult.No);

        if (confirmed != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        if (!await _examRepo.DeleteAsChildAsync(item.Id))
        {
            System.Windows.MessageBox.Show(
                "Diesen Termin haben deine Eltern eingetragen - den kannst du nicht löschen.",
                "Nicht möglich",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            return;
        }

        CurrentViewModel = await BuildWelcomeViewModelAsync();
    }

    /// <summary>
    /// Abhaken wird sofort gespeichert. Eine Hausaufgabe, die man abhakt und die beim naechsten
    /// Start wieder offen dasteht, waere schlimmer als gar keine Erinnerung.
    /// </summary>
    private async void OnHomeworkCompletedChanged(HomeworkItemViewModel item)
    {
        await _homeworkRepo.SetCompletedAsync(item.Id, item.IsCompleted);

        if (CurrentViewModel is WelcomeViewModel welcome)
        {
            welcome.RefreshHomeworkCount();
        }
    }

    /// <summary>Baut die fünf Makro-Etappen (Lesen/Tippen/News/Fächer/Quiz) für die Fortschrittsleiste neu auf.</summary>
    private void UpdateSessionSteps()
    {
        var stage = Progress.CurrentStage;
        var loc = LocalizationService.Instance;
        // Ueber IsSubjectDisabled, nicht ueber Settings.DisabledSubjects allein: sonst zaehlen
        // profilweise abgeschaltete Bereiche (Fuehrerschein, Erste Hilfe) als offen mit, und der
        // Zaehler "Faecher 3/9" erreicht nie sein eigenes Ziel.
        var activeSubjects = LearningStageSubjects.Map.Values
            .Where(s => !IsSubjectDisabled(s)).ToList();
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

    /// <summary>
    /// Ob ein Bereich für die heutige Sitzung ausfällt. Die Regel selbst steht in
    /// <see cref="SubjectAvailability"/> (Core) - dort ist sie ohne WPF prüfbar, und sie hat
    /// genau EINE Fassung. Vorher gab es zwei, und die eine kannte die Profilschalter nicht.
    /// </summary>
    private bool IsSubjectDisabled(Subject subject) =>
        SubjectAvailability.IsDisabled(
            subject,
            Settings.DisabledSubjects,
            CurrentProfile?.DrivingAreaEnabled ?? true,
            CurrentProfile?.ErsteHilfeEnabled ?? true);

    /// <summary>
    /// Dieselbe Auskunft wie <see cref="IsSubjectDisabled"/>, nur als Menge - fuer alles, was
    /// nicht Fach fuer Fach fragt (Abschlussquiz, eigene Aufgaben der Eltern).
    ///
    /// <para><b>Bewusst AUS der Abfrage abgeleitet statt daneben gepflegt.</b> Vorher gab es zwei
    /// Wahrheiten: <c>IsSubjectDisabled</c> kannte beide Schalter, das Abschlussquiz und der
    /// Etappen-Zaehler nur den globalen. Wer den Fuehrerschein-Bereich fuer ein Kind abschaltete,
    /// bekam trotzdem Fuehrerschein-Fragen im Abschlussquiz - in einem Bereich, den dieses Kind an
    /// dem Tag nie gesehen hatte -, und der Zaehler "Faecher 3/9" erreichte nie sein eigenes Ziel.
    /// Als abgeleitete Menge koennen die beiden nicht mehr auseinanderlaufen.</para>
    /// </summary>
    private HashSet<Subject> EffectiveDisabledSubjects() =>
        SubjectAvailability.EffectiveDisabled(
            Settings.DisabledSubjects,
            CurrentProfile?.DrivingAreaEnabled ?? true,
            CurrentProfile?.ErsteHilfeEnabled ?? true);

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

    // ================= Planer-Zwischenstopp (Hausaufgaben und Klausuren) =================

    /// <summary>
    /// Die Etappe, die beim Öffnen des Planers lief. <b>Dieselbe Instanz</b> - sie wird beim
    /// Zurückgehen unverändert wieder eingesetzt, damit eine halb beantwortete Aufgabe, ein
    /// angefangener Text oder die Position im Zeichen-Quiz erhalten bleiben. Ein Neuaufbau
    /// würde den Fortschritt der Etappe stillschweigend verwerfen.
    /// </summary>
    private object? _stashedViewModel;

    public bool IsPlannerOpen => _stashedViewModel is not null;

    /// <summary>
    /// Ob der Planer-Knopf gerade sichtbar ist. Nicht ohne Profil (Profilauswahl,
    /// Ferien-Bildschirm), nicht auf der Startseite selbst - dort steht der Planer ja schon -
    /// und nicht, während er offen ist.
    ///
    /// <para><b>Auf dem Geschafft-Bildschirm ausdrücklich schon.</b> Er war lange
    /// ausgenommen, weil dort der Tag zu Ende ist - aber genau dann will ein Kind nachsehen, was
    /// morgen ansteht: Stundenplan, Hausaufgaben, Klausurtermine. Ohne den Knopf blieb nur der
    /// Weg über "PC jetzt benutzen", und damit beendet sich LernTor - der Plan war weg. Der
    /// Planer schaltet nichts frei und überspringt nichts, er zeigt nur an; nach dem
    /// Freischalten gibt es ohnehin nichts mehr zu umgehen.</para>
    /// </summary>
    public bool CanOpenPlanner =>
        CurrentProfile is not null
        && !IsPlannerOpen
        && Progress.CurrentStage is not LearningStage.Willkommen;

    partial void OnCurrentViewModelChanged(object? value)
    {
        OnPropertyChanged(nameof(CanOpenPlanner));
        OnPropertyChanged(nameof(IsPlannerOpen));
    }

    /// <summary>
    /// Zeigt Hausaufgaben und Klausuren, ohne die laufende Etappe zu verlieren.
    ///
    /// <para><b>Die Mindestzeit-Uhr der Etappe wird angehalten</b> (siehe
    /// <see cref="IPausableStage"/>). Liefe sie weiter, wäre der Planer der bequemste Weg, eine
    /// Mindestverweildauer abzusitzen - aufmachen, warten, zurück.</para>
    ///
    /// <para>Die Lernpflicht bleibt unangetastet: der Planer schaltet nichts frei und überspringt
    /// nichts, er zeigt nur an. Zurück geht es genau dorthin, wo das Kind war.</para>
    /// </summary>
    [RelayCommand]
    private async Task OpenPlannerAsync()
    {
        if (!CanOpenPlanner)
        {
            return;
        }

        if (CurrentViewModel is IPausableStage laufend)
        {
            laufend.PauseStage();
        }

        _stashedViewModel = CurrentViewModel;

        // Frisch aufgebaut, damit heute abgehakte Hausaufgaben und neu eingetragene Klausuren
        // auch wirklich zu sehen sind.
        CurrentViewModel = await BuildWelcomeViewModelAsync(plannerPeek: true);
    }

    private void ClosePlanner()
    {
        if (_stashedViewModel is null)
        {
            return;
        }

        var zurueck = _stashedViewModel;
        _stashedViewModel = null;
        CurrentViewModel = zurueck;

        if (zurueck is IPausableStage laufend)
        {
            laufend.ResumeStage();
        }
    }

    // ================= Führerschein Klasse B (siehe TrafficSignCatalog) =================

    /// <summary>Wie viele Zeichen der heutigen Challenge richtig waren - nur für die Anzeige
    /// direkt danach. Nach einem Neustart unbekannt; dann steht statt der Zahl nur, dass die
    /// Challenge erledigt ist (eine gespeicherte "0 von 5" wäre schlicht falsch).</summary>
    private int? _drivingChallengeCorrectToday;

    /// <summary>
    /// Die für dieses Kind freigegebenen Zeichen: der Katalog ohne die Gruppen, die im
    /// Eltern-Bereich für dieses Profil ausgeblendet sind.
    /// </summary>
    private IReadOnlyList<TrafficSign> DrivingSignPool()
    {
        var ausgeblendet = CurrentProfile?.DisabledSignCategories ?? new HashSet<TrafficSignCategory>();

        var pool = TrafficSignCatalog.All.Where(sign => !ausgeblendet.Contains(sign.Category)).ToList();

        // Alle Gruppen abgewählt wäre ein leerer Bereich, aus dem man nicht mehr herauskommt -
        // dann lieber den vollen Katalog als gar nichts.
        return pool.Count > 0 ? pool : TrafficSignCatalog.All;
    }

    private async Task<DrivingDashboardViewModel> BuildDrivingDashboardViewModelAsync()
    {
        var gekonnt = await _signProgressRepo.GetMasteredNumbersAsync(CurrentProfile!.Id);
        var theorieGekonnt = await _theoryRepo.GetMasteredQuestionIdsAsync(CurrentProfile!.Id);
        var kursStand = await _courseRepo.GetAllAsync(CurrentProfile!.Id);

        return new DrivingDashboardViewModel(
            DrivingSignPool(),
            gekonnt,
            CurrentProfile!.DrivingChallengeSignCount,
            Progress.CompletedExerciseSubjects.Contains(Subject.Fuehrerschein),
            _drivingChallengeCorrectToday,
            () => _ = StartDrivingChallengeAsync(),
            category => _ = StartSignFlashcardsAsync(category),
            category => _ = StartSignQuizAsync(category),
            DrivingTheoryCatalog.All.Count(frage => theorieGekonnt.Contains(frage.Id)),
            DrivingTheoryCatalog.All.Count,
            () => _ = ShowTheoryHubAsync(),
            kursStand.Values.Count(eintrag => eintrag.PassedAt is not null),
            DrivingCourseCatalog.All.Count,
            () => _ = ShowCourseOverviewAsync(),
            OnDrivingCompleted);
    }

    /// <summary>
    /// Die tägliche Challenge: feste Zeichen des Tages (siehe <see cref="DailySignChallenge"/>),
    /// als Quiz. Nach dem Durchlauf gilt der Bereich für heute als erledigt - unabhängig davon,
    /// wie viele richtig waren. Der Pflichtteil ist das Üben, nicht das Können.
    /// </summary>
    private async Task StartDrivingChallengeAsync()
    {
        var pool = DrivingSignPool();
        var gekonnt = await _signProgressRepo.GetMasteredNumbersAsync(CurrentProfile!.Id);

        var zeichen = DailySignChallenge.ForDay(
            pool, CurrentProfile!.Id, DateOnly.FromDateTime(DateTime.Today), gekonnt);

        zeichen = zeichen.Take(Math.Max(1, CurrentProfile!.DrivingChallengeSignCount)).ToList();

        var fragen = SignQuizBuilder.BuildMany(zeichen, pool, _random);

        CurrentViewModel = new SignQuizViewModel(
            fragen,
            LocalizationService.Instance["Fs_Challenge"],
            OnSignAnsweredAsync,
            // Der zweite Parameter (Gesamtzahl) wird hier nicht gebraucht, bekommt aber einen
            // Namen: hiesse er "_", waere "_ =" darunter eine Zuweisung AN diesen Parameter
            // statt ein Verwerfen - genau daran ist der Build einmal gescheitert.
            (richtig, gesamt) => _ = OnDrivingChallengeFinishedAsync(richtig),
            () => _ = ShowDrivingDashboardAsync());
    }

    private async Task OnDrivingChallengeFinishedAsync(int correctCount)
    {
        _drivingChallengeCorrectToday = correctCount;
        Progress.CompletedExerciseSubjects.Add(Subject.Fuehrerschein);

        // Ein Stern je richtig erkanntem Zeichen - dieselbe Währung wie überall sonst.
        if (correctCount > 0)
        {
            await AwardStarsAsync(correctCount);
        }

        await PersistProgressAsync();
    }

    private async Task StartSignQuizAsync(TrafficSignCategory category)
    {
        var pool = DrivingSignPool();
        var zeichen = pool.Where(sign => sign.Category == category).ToList();

        var fragen = SignQuizBuilder.BuildMany(Shuffle(zeichen), pool, _random);

        CurrentViewModel = new SignQuizViewModel(
            fragen,
            TrafficSignCatalog.CategoryLabel(category),
            OnSignAnsweredAsync,
            (_, _) => { },
            () => _ = ShowDrivingDashboardAsync());

        await Task.CompletedTask;
    }

    private async Task StartSignFlashcardsAsync(TrafficSignCategory category)
    {
        var zeichen = DrivingSignPool().Where(sign => sign.Category == category).ToList();

        CurrentViewModel = new SignFlashcardViewModel(
            Shuffle(zeichen),
            TrafficSignCatalog.CategoryLabel(category),
            () => _ = ShowDrivingDashboardAsync());

        await Task.CompletedTask;
    }

    /// <summary>
    /// Jede Quiz-Antwort schreibt den Lernstand fort. Ein neu gekonntes Zeichen ist einen Stern
    /// wert - aber nur beim ersten Mal, sonst könnte man dasselbe Zeichen endlos für Sterne
    /// wiederholen.
    /// </summary>
    private async Task OnSignAnsweredAsync(TrafficSign sign, bool wasCorrect)
    {
        var neuGekonnt = await _signProgressRepo.RecordAnswerAsync(CurrentProfile!.Id, sign.Number, wasCorrect);

        if (neuGekonnt)
        {
            await AwardStarsAsync(1);
        }
    }

    private async Task ShowDrivingDashboardAsync()
    {
        CurrentViewModel = await BuildDrivingDashboardViewModelAsync();
    }

    private async void OnDrivingCompleted()
    {
        Progress.CompletedExerciseSubjects.Add(Subject.Fuehrerschein);
        await PersistProgressAsync();
        await NavigateToStageAsync(_gate.GetNextStage(LearningStage.Fuehrerschein));
    }

    private List<TrafficSign> Shuffle(List<TrafficSign> signs)
    {
        for (var i = signs.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (signs[i], signs[j]) = (signs[j], signs[i]);
        }

        return signs;
    }

    // ----------------- Unterbereich Theoriefragen (siehe DrivingTheoryCatalog) -----------------

    /// <summary>Fragen im Schwachstellen-Training. Genug für eine Viertelstunde, nicht mehr -
    /// ein Trainer, der wie eine zweite Prüfung wirkt, wird nicht angefasst.</summary>
    private const int WeakSpotQuestionCount = 10;

    /// <summary>
    /// Trefferquote je Sachgebiet - Grundlage des Schwachstellen-Trainers.
    ///
    /// <para><b>Gemessen wird der Jetzt-Zustand, nicht der Durchschnitt der Historie:</b> eine
    /// Frage zählt als richtig, wenn sie <i>gerade</i> sitzt (siehe <see cref="TheoryProgress"/>).
    /// Über alle je gegebenen Antworten zu mitteln würde ein Sachgebiet noch monatelang als
    /// Schwachstelle führen, nachdem das Kind es längst kann.</para>
    ///
    /// <para>Fragen, die es im Katalog nicht mehr gibt, fallen still heraus - ihr Sachgebiet
    /// steht nur dort und nicht in der Datenbank.</para>
    /// </summary>
    private async Task<IReadOnlyList<TopicMastery>> BuildTheoryMasteryAsync()
    {
        var antworten = await _theoryRepo.GetAnswersAsync(CurrentProfile!.Id);

        var paare = new List<(DrivingTheoryTopic Topic, bool WasCorrect)>();

        foreach (var eintrag in antworten.Values)
        {
            if (DrivingTheoryCatalog.ById(eintrag.QuestionId) is { } frage)
            {
                paare.Add((frage.Topic, TheoryProgress.IsMastered(eintrag.CorrectStreak)));
            }
        }

        return TheoryExamComposer.BuildMastery(paare);
    }

    private async Task ShowTheoryHubAsync()
    {
        var gekonnt = await _theoryRepo.GetMasteredQuestionIdsAsync(CurrentProfile!.Id);
        var staerken = await BuildTheoryMasteryAsync();
        var letzte = await _theoryRepo.GetRecentExamsAsync(CurrentProfile!.Id, 1);

        CurrentViewModel = new TheoryHubViewModel(
            DrivingTheoryCatalog.All,
            gekonnt,
            staerken,
            letzte.FirstOrDefault(),
            topic => _ = StartTheoryTopicAsync(topic),
            () => _ = StartTheoryExamAsync(),
            () => _ = StartTheoryWeakSpotsAsync(),
            () => _ = ShowDrivingDashboardAsync());
    }

    /// <summary>
    /// Übungsrunde zu einem Sachgebiet: alle Fragen daraus, noch nicht sitzende zuerst. Wer ein
    /// Gebiet gezielt anwählt, will daran arbeiten - dann soll nicht zuerst das kommen, was
    /// ohnehin schon sitzt.
    /// </summary>
    private async Task StartTheoryTopicAsync(DrivingTheoryTopic topic)
    {
        var gekonnt = await _theoryRepo.GetMasteredQuestionIdsAsync(CurrentProfile!.Id);

        var fragen = ShuffleQuestions(DrivingTheoryCatalog.ByTopic(topic).ToList())
            .OrderBy(frage => gekonnt.Contains(frage.Id) ? 1 : 0)
            .ToList();

        StartTheoryRun(fragen, DrivingTheoryCatalog.TopicLabel(topic), TheoryRunMode.Uebung);
    }

    private async Task StartTheoryWeakSpotsAsync()
    {
        var staerken = await BuildTheoryMasteryAsync();

        var fragen = TheoryExamComposer.ComposeWeakSpotSet(
            DrivingTheoryCatalog.All, staerken, WeakSpotQuestionCount, _random);

        StartTheoryRun(
            fragen.ToList(),
            LocalizationService.Instance["Fs_WeakSpots"],
            TheoryRunMode.Uebung);
    }

    private async Task StartTheoryExamAsync()
    {
        var fragen = TheoryExamComposer.ComposeExam(DrivingTheoryCatalog.All, _random);

        StartTheoryRun(
            fragen.ToList(),
            LocalizationService.Instance["Fs_Exam"],
            TheoryRunMode.Pruefung);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Baut die Ansicht für einen Fragensatz. Ein leerer Satz führt zurück zur Übersicht statt in
    /// eine Ansicht ohne Fragen - sonst riefe der Durchlauf sein Ende schon im Konstruktor auf,
    /// und die Zuweisung danach würde die Ergebnisansicht gleich wieder überschreiben.
    /// </summary>
    private void StartTheoryRun(IReadOnlyList<TheoryQuestion> questions, string title, TheoryRunMode mode)
    {
        if (questions.Count == 0)
        {
            _ = ShowTheoryHubAsync();
            return;
        }

        var gestellt = TheoryQuestionPresenter.PresentAll(questions, _random);

        CurrentViewModel = new TheoryQuestionViewModel(
            gestellt,
            title,
            mode,
            OnTheoryAnsweredAsync,
            records => _ = OnTheoryRunFinishedAsync(mode, records),
            () => _ = ShowTheoryHubAsync());
    }

    /// <summary>
    /// Jede Antwort schreibt den Lernstand fort. Eine neu sitzende Frage ist einen Stern wert -
    /// aber nur beim ersten Mal, sonst ließe sich dieselbe Frage endlos für Sterne wiederholen.
    /// </summary>
    private async Task OnTheoryAnsweredAsync(TheoryQuestion question, bool wasCorrect)
    {
        var neuGekonnt = await _theoryRepo.RecordAnswerAsync(CurrentProfile!.Id, question.Id, wasCorrect);

        if (neuGekonnt)
        {
            await AwardStarsAsync(1);
        }
    }

    private async Task OnTheoryRunFinishedAsync(
        TheoryRunMode mode, IReadOnlyList<TheoryAnswerRecord> records)
    {
        if (mode != TheoryRunMode.Pruefung)
        {
            await ShowTheoryHubAsync();
            return;
        }

        var ergebnis = TheoryExamRules.Evaluate(
            records.Select(antwort => antwort.Question.Question).ToList(),
            records.Select(antwort => antwort.WasCorrect).ToList());

        await _theoryRepo.RecordExamAsync(CurrentProfile!.Id, ergebnis);

        if (ergebnis.Passed)
        {
            await AwardStarsAsync(5);
        }

        CurrentViewModel = new TheoryResultViewModel(
            ergebnis,
            records,
            () => _ = StartTheoryExamAsync(),
            () => _ = ShowTheoryHubAsync());
    }

    // ----------------- Unterbereich Theorie-Kurs (siehe DrivingCourseCatalog) -----------------

    private async Task ShowCourseOverviewAsync()
    {
        var stand = await _courseRepo.GetAllAsync(CurrentProfile!.Id);

        CurrentViewModel = new CourseOverviewViewModel(
            DrivingCourseCatalog.All,
            stand,
            lesson => _ = ShowCourseLessonAsync(lesson),
            () => _ = ShowDrivingDashboardAsync());
    }

    private async Task ShowCourseLessonAsync(DrivingCourseLesson lesson)
    {
        var stand = await _courseRepo.GetAllAsync(CurrentProfile!.Id);
        stand.TryGetValue(lesson.Id, out var eintrag);

        CurrentViewModel = new CourseLessonViewModel(
            lesson,
            eintrag,
            DrivingTheoryCatalog.ByTopic(lesson.Topic).Count > 0,
            gelesen => _ = StartLessonCheckAsync(gelesen),
            gelesen => _ = OnLessonReadAsync(gelesen),
            () => _ = ShowCourseOverviewAsync());
    }

    private async Task OnLessonReadAsync(DrivingCourseLesson lesson)
    {
        await _courseRepo.MarkReadAsync(CurrentProfile!.Id, lesson.Id);
        await ShowCourseOverviewAsync();
    }

    /// <summary>
    /// Die Lernstandskontrolle am Ende einer Lektion. Sie läuft im Übungs-Modus, also mit
    /// sofortiger Auflösung - sie soll zeigen, was hängengeblieben ist, und nicht wie eine
    /// zweite Prüfung wirken.
    ///
    /// <para>Die Antworten zählen zugleich in den Theorie-Lernstand: es sind dieselben Fragen aus
    /// demselben Katalog. Sie hier nicht mitzuzählen hieße, dass eine im Kurs gemeisterte Frage
    /// im Schwachstellen-Trainer weiter als ungekonnt gilt.</para>
    /// </summary>
    private async Task StartLessonCheckAsync(DrivingCourseLesson lesson)
    {
        var fragen = DrivingCourseCatalog.CheckQuestions(lesson, _random);

        if (fragen.Count == 0)
        {
            await ShowCourseLessonAsync(lesson);
            return;
        }

        var gestellt = TheoryQuestionPresenter.PresentAll(fragen, _random);

        CurrentViewModel = new TheoryQuestionViewModel(
            gestellt,
            string.Format(LocalizationService.Instance["Fs_CheckTitle"], lesson.Title),
            TheoryRunMode.Uebung,
            OnTheoryAnsweredAsync,
            records => _ = OnLessonCheckFinishedAsync(lesson, records),
            () => _ = ShowCourseLessonAsync(lesson));
    }

    private async Task OnLessonCheckFinishedAsync(
        DrivingCourseLesson lesson, IReadOnlyList<TheoryAnswerRecord> records)
    {
        var richtig = records.Count(antwort => antwort.WasCorrect);

        var neuGeschafft = await _courseRepo.RecordCheckAsync(
            CurrentProfile!.Id, lesson.Id, richtig, records.Count);

        if (neuGeschafft)
        {
            await AwardStarsAsync(3);
        }

        await ShowCourseOverviewAsync();
    }

    private List<TheoryQuestion> ShuffleQuestions(List<TheoryQuestion> questions)
    {
        for (var i = questions.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (questions[i], questions[j]) = (questions[j], questions[i]);
        }

        return questions;
    }

    private async Task<ReadingViewModel> BuildReadingViewModelAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        // Eigene Eltern-Texte belegen, sofern vorhanden, den ersten der beiden Tagesplätze -
        // siehe ReadingContentProvider.GetPairForDate.
        var customPieces = await _customReadingRepo.GetForProfileAsync(CurrentProfile!.Id);
        var (piece, secondPiece) = ReadingContentProvider.GetPairForDate(
            today,
            customPieces,
            Settings.HiddenReadingTextKeys,
            CurrentProfile!.PinnedReadingTextKey);
        return new ReadingViewModel(piece, secondPiece, OnReadingCompleted, _tts, CurrentProfile!.ReadingMinutes);
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
        }, () => _ = NavigateToStageAsync(_gate.GetNextStage(LearningStage.Tippen)),
            CurrentProfile!.TypingTextOverrides);
        await vm.InitializeAsync();
        return vm;
    }

    private async Task NavigateToTypingExerciseAsync(string lessonId)
    {
        var overrides = CurrentProfile!.TypingTextOverrides;
        var lesson = TypingContentProvider.GetLessonById(lessonId, overrides);
        if (lesson == null) return;

        var exerciseVm = new TypingExerciseViewModel(
            lesson,
            _typingService,
            _typingProgressRepo,
            CurrentProfile!.Id,
            CurrentProfile!.Name,
            CurrentProfile!.TypingMinAccuracy,
            lessonId => OnTypingLessonCompleted(lessonId),
            overrides
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
            var lesson = TypingContentProvider.GetLessonById(lessonId, CurrentProfile!.TypingTextOverrides);
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
                            var nextLesson = TypingContentProvider.GetNextUnlockedLesson(completedLessonIds, CurrentProfile!.Name, CurrentProfile!.TypingTextOverrides);
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
        var articlesTask = _newsService.LoadCuratedArticlesAsync(
            // Wie viele Nachrichten der Tag bringt, entscheiden die Eltern pro Profil - vorher
            // stand hier eine feste Konstante, die der Dienst ohnehin ignoriert hat.
            targetCount: CurrentProfile?.NewsArticleCount ?? StudentProfile.DefaultNewsArticleCount,
            childAge: CurrentProfile?.Age,
            gradeLevel: gradeLevel,
            disabledFeedNames: Settings.DisabledNewsFeeds,
            filterStrictness: CurrentProfile?.NewsFilterStrictness ?? NewsFilterStrictness.Normal);
        var weatherTask = _weatherService.LoadBerlinWeatherAsync();
        var articles = await articlesTask;
        var weather = await weatherTask;

        // Offline-Rückfall: sind alle Feeds tot, kommt nur das eingebaute Finanzwissen-Stück
        // zurück (Count <= 1). Dann Artikel aus dem Archiv laden - alte News sind besser als ein
        // fast leerer Pflicht-News-Teil. Bei Erfolg wird der heutige Stand umgekehrt archiviert
        // (idempotent, behält 21 Tage).
        //
        // Welcher Archiv-Tag genommen wird, wandert mit der Dauer des Ausfalls: am ersten Tag
        // der jüngste Stand, am zweiten der davor. Sonst bekäme das Kind bei einem längeren
        // Ausfall - Urlaub, Router kaputt - jeden Morgen exakt dieselben Nachrichten inklusive
        // derselben Verständnisfragen.
        if (articles.Count <= 1)
        {
            var (archived, archivedOn) = await _archiveRepo.GetOfflineFallbackAsync(
                DateOnly.FromDateTime(DateTime.Today));

            if (archived.Count > 0)
            {
                Core.Logging.AppLog.Warn("News",
                    $"Keine Feeds erreichbar - Rückfall auf {archived.Count} archivierte Artikel " +
                    $"vom {archivedOn:dd.MM.yyyy}.");
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
            _homeworkChat, weather, CurrentProfile!.NewsSecondsPerArticle);
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

    // Die Fragenzahlen (Übungen pro Fach, Quiz-Erstversuch, Quiz-Wiederholung) sind seit der
    // Eltern-Bereich-Erweiterung pro Profil konfigurierbar - siehe
    // StudentProfile.ExercisesPerSubject/QuizQuestionCount/QuizRetryQuestionCount
    // (Standard 6/20/15; die Wiederholung ist bewusst kleiner als der erste Versuch, damit sie
    // nicht wie eine zusätzliche Strafe wirkt).

    /// <summary>
    /// Vereint zwei unabhängige Ausschlussgründe für die Aufgabenauswahl: das 21-Tage-Fenster
    /// kürzlich gestellter Fragen (bevorzugt Frische bei den fest hinterlegten Themen-Pools) und
    /// die gemeisterten, noch nicht wieder fälligen Prompts (siehe MasteredPromptRepository -
    /// Spaced Repetition: richtig beantwortete Aufgaben pausieren 7/30/90 Tage und kehren dann
    /// zur Auffrischung zurück). Beide landen im selben Ausschluss-Set, weil
    /// ExerciseGeneratorBase.Generate ohnehin nur "meide diese Prompts, wenn möglich" kennt,
    /// keinen Unterschied zwischen den beiden Gründen macht.
    /// </summary>
    private async Task<IReadOnlySet<string>> BuildExcludedPromptsAsync()
    {
        var recentlySeen = await _activityLogRepo.GetRecentPromptsAsync(CurrentProfile!.Id, RecentPromptsWindow);
        var mastered = await _masteredPromptRepo.GetMasteredPromptsAsync(CurrentProfile!.Id);

        var excluded = new HashSet<string>(recentlySeen);
        excluded.UnionWith(mastered);
        return excluded;
    }

    /// <summary>
    /// Gewichte für die adaptive Übungsauswahl: Trefferquote je Thema aus den letzten 30 Tagen
    /// Aktivitätsprotokoll dieses Fachs - schwache Themen (viele Fehler) werden bis zu 3x so
    /// häufig gezogen (siehe AdaptiveTopicWeighting), damit automatisch dort geübt wird, wo es
    /// hakt. Themen mit zu wenig Datenbasis bleiben neutral.
    /// </summary>
    private async Task<IReadOnlyDictionary<string, double>> BuildTopicWeightsAsync(Subject subject)
    {
        var recentActivity = await _activityLogRepo.GetActivitySinceAsync(CurrentProfile!.Id, TimeSpan.FromDays(30));

        var topicStats = recentActivity
            .Where(a => a.Subject == subject.ToString())
            .GroupBy(a => a.Topic)
            .Select(g => (Topic: g.Key, Attempts: g.Count(), Correct: g.Count(a => a.WasCorrect)));

        return AdaptiveTopicWeighting.ComputeWeights(topicStats);
    }

    /// <summary>
    /// Wie viele Aufgaben dieses Fach heute bekommt. Steht eine Klausur an, sind es mehr - ab
    /// einer Woche vorher sanft steigend bis zum Doppelten am Vortag (siehe
    /// ExamEntry.LearningWeight). Genau dafür liegen die Termine lokal: mit einem Cloud-Kalender
    /// müsste die App bei jedem Start online gehen, um zu wissen, was sie üben soll.
    /// </summary>
    private async Task<int> ExerciseCountForSubjectAsync(Subject subject)
    {
        var baseCount = CurrentProfile!.ExercisesPerSubject;

        var weights = await _examRepo.GetLearningWeightsAsync(
            CurrentProfile!.Id, DateOnly.FromDateTime(DateTime.Today));

        if (!weights.TryGetValue(subject, out var weight) || weight <= 1.0)
        {
            return baseCount;
        }

        // Nach oben begrenzt: eine Klausurwoche soll den Tag straffen, nicht sprengen.
        return Math.Min((int)Math.Round(baseCount * weight), baseCount + 8);
    }

    private async Task<ExerciseViewModel> BuildExerciseViewModelAsync(Subject subject)
    {
        var grade = CurrentProfile!.GradeLevel;
        var excludedPrompts = await BuildExcludedPromptsAsync();
        var topicWeights = await BuildTopicWeightsAsync(subject);
        var exerciseCount = await ExerciseCountForSubjectAsync(subject);
        var generated = _quizComposer.GenerateExercises(subject, grade, exerciseCount, _random, excludedPrompts, topicWeights);
        var custom = await _customQuestionRepo.GetBySubjectAndGradeAsync(subject, grade);

        // Fehler-Kartei: an Vortagen falsch beantwortete Aufgaben dieses Fachs kommen ZUERST
        // (mit 🔁-Thema markiert), bis sie zweimal in Folge richtig beantwortet wurden. Zufällige
        // Dubletten aus dem Generator werden über den Aufgabentext aussortiert.
        var review = await _reviewRepo.GetDueQuestionsAsync(CurrentProfile!.Id, subject, maxCount: 3);
        var reviewPrompts = review.Select(r => r.Prompt).ToHashSet();

        // Vokabeln (nur Englisch/Türkisch, nur wenn Eltern welche hinterlegt haben): sie ersetzen
        // bis zur Hälfte der generierten Aufgaben, statt den Tag zu verlängern. Vokabeln sind der
        // Kern dieser beiden Fächer - ohne sie übt das Kind Grammatikregeln, aber keinen Wortschatz.
        var vocabulary = await BuildVocabularyQuestionsAsync(subject, grade);

        // Reihenfolge nach Verbindlichkeit, nicht nach Zufall:
        //   1. Fehler-Kartei  - Wiederholung hat Vorrang vor Neuem
        //   2. Vokabeln       - von den Eltern gepflegte Wortlisten
        //   3. eigene Aufgaben - was Eltern eintragen ODER aus einem Dokument übernehmen, MUSS
        //      auch drankommen. Sie standen vorher hinter den generierten Aufgaben und wurden
        //      deshalb vom Take() abgeschnitten - eingetragene Hausaufgaben tauchten im
        //      schlechtesten Fall nie auf (Regression aus dem Vokabel-Commit).
        //   4. generierte Aufgaben füllen den Rest auf.
        var customForToday = custom.Where(q => !reviewPrompts.Contains(q.Prompt)).ToList();
        var alreadyPlanned = vocabulary.Count + customForToday.Count;
        var generatedFill = generated
            .Where(q => !reviewPrompts.Contains(q.Prompt))
            .Take(Math.Max(0, CurrentProfile!.ExercisesPerSubject - alreadyPlanned))
            .ToList();

        var questions = review
            .Concat(vocabulary)
            // Eigene und generierte Aufgaben gemischt, damit die eigenen nicht immer am selben
            // Platz stehen - abgeschnitten wird aber nur noch bei den generierten.
            .Concat(customForToday.Concat(generatedFill).OrderBy(_ => _random.Next()))
            .ToList();

        // Sprachausgabe wird durchgereicht, weil Deutsch-Uebungen Diktate enthalten koennen -
        // dort wird der Satz vorgelesen statt angezeigt (siehe QuestionType.Diktat).
        return new ExerciseViewModel(subject, questions, OnExerciseQuestionAnswered, () => OnExerciseSubjectCompleted(subject), _homeworkChat,
            _tts, CurrentProfile!.ExerciseSecondsPerQuestion);
    }

    /// <summary>
    /// Fällige Vokabeln des Fachs als Aufgaben. Höchstens die Hälfte der Tagesaufgaben, damit die
    /// generierten Grammatik-/Textaufgaben nicht komplett verdrängt werden.
    /// </summary>
    private async Task<IReadOnlyList<QuizQuestion>> BuildVocabularyQuestionsAsync(Subject subject, GradeLevel grade)
    {
        if (subject != Subject.Englisch && subject != Subject.Tuerkisch)
        {
            return Array.Empty<QuizQuestion>();
        }

        var maxVocabulary = Math.Max(1, CurrentProfile!.ExercisesPerSubject / 2);
        var due = await _vocabularyRepo.GetDueAsync(CurrentProfile!.Id, subject, maxVocabulary);
        if (due.Count == 0)
        {
            return Array.Empty<QuizQuestion>();
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        return due.Select(entry => VocabularyQuestionFactory.Create(entry, grade, today)).ToList();
    }

    private async void OnExerciseQuestionAnswered(Subject subject, QuestionOutcome outcome, QuizQuestion question)
    {
        await _activityLogRepo.LogAnswerAsync(CurrentProfile!.Id, outcome, question.Topic, question.Prompt);

        // Vokabeln haben ihre eigene Wiederholungs-Steuerung (das Wortpaar bleibt dauerhaft im
        // Bestand) und laufen deshalb NICHT über Fehler-Kartei und Meisterungs-Tabelle - dort
        // würde ein Wortpaar nach zweimal richtig ganz verschwinden.
        if (question.Id.StartsWith("vokabel-", StringComparison.Ordinal))
        {
            await _vocabularyRepo.RecordOutcomeAsync(
                CurrentProfile!.Id, question.Id["vokabel-".Length..], outcome.WasCorrect);
            return;
        }

        // Fehler-Kartei pflegen: falsch → aufnehmen/zurücksetzen, richtig → Streak hoch, bei 2 gelernt.
        await _reviewRepo.RecordOutcomeAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
        // Spaced Repetition: richtig → gemeistert (pausiert 7/30/90 Tage, kehrt zur Auffrischung
        // zurück), falsch → Meisterung verfällt und die Fehler-Kartei übernimmt.
        await _masteredPromptRepo.RecordOutcomeAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
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
        // EffectiveDisabledSubjects statt Settings.DisabledSubjects: sonst fragt das
        // Abschlussquiz Faecher ab, die fuer DIESES Kind abgeschaltet sind und die es folglich
        // nie geuebt hat - und die falschen Antworten druecken es unter die Bestehensschwelle.
        var disabledSubjects = EffectiveDisabledSubjects();
        var relevantSubjects = Progress.SubjectsToRetry.Count > 0 ? Progress.SubjectsToRetry : null;
        var excludedPrompts = await BuildExcludedPromptsAsync();
        IEnumerable<QuizQuestion> questions;

        if (relevantSubjects is not null)
        {
            // Schwache Fächer bekommen konzentriert Fragen (mind. 2 je Fach) aus dem
            // 15er-Zielbudget; der Rest füllt ein allgemeines Mini-Quiz über alle aktiven Fächer
            // auf - genau wie beim ersten Versuch passt sich die Fragenzahl je Fach dynamisch an
            // die Anzahl aktiver Fächer an (siehe ComposeFinalQuiz).
            var retryTarget = CurrentProfile!.QuizRetryQuestionCount;
            var perWeakSubjectCount = Math.Max(2, retryTarget * 2 / 3 / relevantSubjects.Count);
            var retryQuestions = _quizComposer.ComposeRetryExercises(
                relevantSubjects, grade, _random, countPerSubject: perWeakSubjectCount, recentlySeenPrompts: excludedPrompts);

            var topUpTarget = Math.Max(retryTarget - retryQuestions.Count, 1);
            var topUpQuestions = _quizComposer.ComposeFinalQuiz(
                grade, _random, disabledSubjects, targetTotalQuestions: topUpTarget, recentlySeenPrompts: excludedPrompts);

            questions = retryQuestions.Concat(topUpQuestions);
        }
        else
        {
            questions = _quizComposer.ComposeFinalQuiz(
                grade, _random, disabledSubjects,
                targetTotalQuestions: CurrentProfile!.QuizQuestionCount, recentlySeenPrompts: excludedPrompts);
        }

        // Eigene (von den Eltern eingetragene) Aufgaben ergänzen additiv - unabhängig vom
        // dynamischen Fragenbudget der generierten Fächer.
        var customQuestions = (await _customQuestionRepo.GetByGradeAsync(grade))
            .Where(q => !disabledSubjects.Contains(q.Subject));

        var finalQuestions = questions.Concat(customQuestions).OrderBy(_ => _random.Next()).ToList();

        return new FinalQuizViewModel(finalQuestions, OnFinalQuizCompleted, OnFinalQuizQuestionAnswered, _homeworkChat, _tts);
    }

    private async void OnFinalQuizQuestionAnswered(QuizQuestion question, QuestionOutcome outcome)
    {
        // Spaced Repetition auch fürs Abschlussquiz: richtig → gemeistert (pausiert 7/30/90 Tage),
        // falsch → Meisterung verfällt - auch wenn der Prompt nur im Quiz und nie in einer Übung drankam.
        await _masteredPromptRepo.RecordOutcomeAsync(CurrentProfile!.Id, question, outcome.WasCorrect);
    }

    private async void OnFinalQuizCompleted(IReadOnlyList<QuestionOutcome> outcomes)
    {
        // Welcher Schwellenwert gilt, hängt davon ab, ob das hier der erste oder ein zweiter Anlauf
        // (Wiederholung nach nicht bestandenem ersten Versuch) ist - beide sind von den Eltern pro
        // Profil einstellbar (StudentProfile.QuizFirstAttemptThreshold/QuizRetryThreshold). Muss VOR
        // ApplyQuizResult geprüft werden, da dieses Progress.SubjectsToRetry beim Bestehen leert.
        var isRetryAttempt = Progress.SubjectsToRetry.Count > 0;
        var passThreshold = isRetryAttempt ? CurrentProfile!.QuizRetryThreshold : CurrentProfile!.QuizFirstAttemptThreshold;

        var result = _scoring.BuildResult(outcomes, passThreshold);
        await _activityLogRepo.LogQuizAttemptAsync(CurrentProfile!.Id, result);
        _gate.ApplyQuizResult(Progress, result);

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

        CurrentViewModel = await BuildResultViewModelAsync(Progress.IsUnlocked, result);
    }

    /// <summary>
    /// Ergebnis-/Freigeschaltet-Bildschirm inkl. Tages-Zusammenfassung: heute beantwortete
    /// Aufgaben + Trefferquote aus dem Aktivitätsprotokoll (seit Mitternacht) und - falls von den
    /// Eltern eingeschaltet - die 🔥-Lernserie. Der Moment der Freischaltung ist der emotionale
    /// Höhepunkt des Ablaufs; hier soll das Kind SEHEN, was es heute geschafft hat.
    /// </summary>
    private async Task<ResultViewModel> BuildResultViewModelAsync(bool passed, QuizResult? result)
    {
        var todayAnswered = 0;
        var todayCorrectPercent = 0;
        var streak = 0;
        IReadOnlyList<SubjectProgress> subjectProgress = Array.Empty<SubjectProgress>();

        if (CurrentProfile is not null)
        {
            var sinceMidnight = DateTime.Now - DateTime.Today;
            var todayActivity = await _activityLogRepo.GetActivitySinceAsync(CurrentProfile.Id, sinceMidnight);
            todayAnswered = todayActivity.Count;
            if (todayAnswered > 0)
            {
                todayCorrectPercent = (int)Math.Round(100.0 * todayActivity.Count(a => a.WasCorrect) / todayAnswered);
            }

            if (Settings.StreaksEnabled)
            {
                var learningDays = await _activityLogRepo.GetLearningDaysAsync(CurrentProfile.Id);
                streak = StreakCalculator.CurrentStreak(learningDays, DateOnly.FromDateTime(DateTime.Today));
            }

            // Eigene Entwicklung: die Kinder sehen sonst nur eine Sternezahl - die sagt nichts
            // darueber, ob sie besser geworden sind. Zwei Vierzehn-Tage-Fenster aus dem
            // vorhandenen Protokoll, kein Vergleich mit dem Geschwisterkind.
            var progressWindow = TimeSpan.FromDays(LearningProgressTracker.WindowDays * 2);
            var progressActivity = await _activityLogRepo.GetActivitySinceAsync(CurrentProfile.Id, progressWindow);

            subjectProgress = LearningProgressTracker.Compare(
                progressActivity
                    .Where(entry => Enum.TryParse<Subject>(entry.Subject, out _))
                    .Select(entry => new LearningProgressTracker.Answer(
                        Enum.Parse<Subject>(entry.Subject),
                        DateOnly.FromDateTime(entry.Timestamp.LocalDateTime),
                        entry.WasCorrect)),
                DateOnly.FromDateTime(DateTime.Today));
        }

        return new ResultViewModel(
            passed, result, Progress.EarnedStarsToday, CurrentProfile?.TotalStars ?? 0,
            todayAnswered, todayCorrectPercent, streak,
            OnRetryWeakSubjectsRequested, OnUnlockConfirmed,
            _rewardRepo, CurrentProfile?.Id,
            subjectProgress);
    }

    private async void OnRetryWeakSubjectsRequested()
    {
        await NavigateToStageAsync(LearningStage.Abschlussquiz);
    }

    private void OnUnlockConfirmed()
    {
        // _kioskLock.Unlock() ist zu diesem Zeitpunkt bereits gelaufen (siehe oben, sobald
        // Progress.IsUnlocked wahr wird) - MainWindow verweigert das Schließen sonst
        // (Schutz gegen den Alt+Tab-"X"-Button, siehe MainWindow_Closing).
        System.Windows.Application.Current.Shutdown();
    }
}