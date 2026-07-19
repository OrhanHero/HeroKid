using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.ContentGen.Llm;
using LernTor.ContentGen.TeacherImport;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;
using LernTor.Security;
using Microsoft.Win32;

namespace LernTor.App.ViewModels;

public sealed partial class ParentSettingsViewModel : ObservableObject
{
    private readonly SettingsRepository _settingsRepo;
    private readonly ActivityLogRepository _activityLogRepo;
    private readonly StudentProfileRepository _profileRepo;
    private readonly DatabaseMaintenanceRepository _maintenanceRepo;
    private readonly CustomQuestionRepository _customQuestionRepo;
    private readonly KioskLockService _kioskLock;
    private readonly LocalLlmOptions _localLlmOptions;
    private readonly TeacherDocumentImportService _teacherImportService;
    private readonly PiperTtsEngine _piperTts;
    private readonly RewardRepository _rewardRepo;

    private AppSettings _settings = new();

    /// <summary>Alle deaktivierbaren Fachbereiche (News zählt bewusst nicht dazu, ist Pflicht).</summary>
    private static readonly (Subject Subject, string TranslationKey)[] ToggleableSubjects =
    {
        (Subject.Mathematik, "Stage_Mathematik"),
        (Subject.Deutsch, "Stage_Deutsch"),
        (Subject.Tuerkisch, "Stage_Tuerkisch"),
        (Subject.Englisch, "Stage_Englisch"),
        (Subject.Biologie, "Stage_Biologie"),
        (Subject.Chemie, "Stage_Chemie"),
        (Subject.Physik, "Stage_Physik"),
        (Subject.Geschichte, "Stage_Geschichte"),
        (Subject.Gewi, "Stage_Gewi"),
        (Subject.Politik, "Stage_Politik"),
        (Subject.Geo, "Stage_Geo"),
        (Subject.Ethik, "Stage_Ethik"),
        (Subject.Kunst, "Stage_Kunst"),
        (Subject.Musik, "Stage_Musik"),
        (Subject.Itg, "Stage_Itg"),
        (Subject.KiWissen, "Stage_KiWissen"),
        (Subject.Tippen, "Stage_Tippen"),
    };

    /// <summary>Wird vom Aufrufer gesetzt, um beim Öffnen direkt das gerade aktive Kind-Profil vorauszuwählen.</summary>
    public string? PreselectProfileId { get; set; }

    [ObservableProperty]
    private bool isAuthenticated;

    [ObservableProperty]
    private bool isFirstTimeSetup;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private StudentProfile? selectedProfile;

    public ObservableCollection<SubjectToggle> SubjectToggles { get; } = new();
    public ObservableCollection<StudentProfile> Profiles { get; } = new();
    public ObservableCollection<ActivityLogEntity> RecentActivity { get; } = new();
    public ObservableCollection<QuizAttemptEntity> QuizHistory { get; } = new();
    public ObservableCollection<QuizQuestion> CustomQuestions { get; } = new();

    public bool HasNoCustomQuestions => CustomQuestions.Count == 0;

    /// <summary>Alle Fächer, für die eigene Aufgaben angelegt werden können (News hat keine Übungsaufgaben).</summary>
    public IReadOnlyList<Subject> AvailableSubjects { get; } = Enum.GetValues<Subject>().Where(s => s != Subject.News).ToList();
    public IReadOnlyList<GradeLevel> AvailableGradeLevels { get; } = Enum.GetValues<GradeLevel>().ToList();
    public IReadOnlyList<QuestionType> AvailableQuestionTypes { get; } = Enum.GetValues<QuestionType>().ToList();

    [ObservableProperty]
    private Subject newQuestionSubject = Subject.Mathematik;

    [ObservableProperty]
    private GradeLevel newQuestionGrade = GradeLevel.Klasse6;

    [ObservableProperty]
    private QuestionType newQuestionType = QuestionType.OpenText;

    [ObservableProperty]
    private string newQuestionTopic = string.Empty;

    [ObservableProperty]
    private string newQuestionPrompt = string.Empty;

    /// <summary>Bei MultipleChoice/TrueFalse: Antwortoptionen, mit Komma getrennt.</summary>
    [ObservableProperty]
    private string newQuestionOptionsText = string.Empty;

    /// <summary>Akzeptierte Antwort(en), mit Komma getrennt (bei OpenText reicht eine).</summary>
    [ObservableProperty]
    private string newQuestionCorrectAnswersText = string.Empty;

    [ObservableProperty]
    private string newQuestionExplanation = string.Empty;

    [ObservableProperty]
    private string newQuestionHelpHint = string.Empty;

    [ObservableProperty]
    private string customQuestionErrorMessage = string.Empty;

    public bool NewQuestionNeedsOptions => NewQuestionType != QuestionType.OpenText;

    partial void OnNewQuestionTypeChanged(QuestionType value) => OnPropertyChanged(nameof(NewQuestionNeedsOptions));

    // --- Automatisches Einlesen von Lehrer-Unterlagen + KI-Lernchat (lokales LLM, siehe README) ---

    /// <summary>Kuratierte, automatisch herunterladbare Modelle (Dropdown im Eltern-Bereich).</summary>
    public IReadOnlyList<LocalLlmModelInfo> AvailableLlmModels { get; } = LocalLlmModelCatalog.Models;

    [ObservableProperty]
    private LocalLlmModelInfo selectedLlmModel = LocalLlmModelCatalog.Resolve(null);

    [ObservableProperty]
    private string localLlmModelPath = string.Empty;

    [ObservableProperty]
    private Subject importSubject = Subject.Mathematik;

    [ObservableProperty]
    private GradeLevel importGrade = GradeLevel.Klasse6;

    [ObservableProperty]
    private string importFilePath = string.Empty;

    [ObservableProperty]
    private bool isImporting;

    [ObservableProperty]
    private string importErrorMessage = string.Empty;

    public ObservableCollection<EditableDraftViewModel> ImportedDrafts { get; } = new();

    public bool HasNoImportedDrafts => ImportedDrafts.Count == 0;

    // --- Natürliche Vorlesestimmen (Piper, siehe PiperTtsEngine) ---

    [ObservableProperty]
    private bool isPiperInstalled;

    [ObservableProperty]
    private bool isPiperInstalling;

    [ObservableProperty]
    private string piperStatusMessage = string.Empty;

    [ObservableProperty]
    private string piperErrorMessage = string.Empty;

    // --- 🎁 Belohnungen (Sterne einlösen; Kind-Ansicht siehe ResultViewModel) ---

    public ObservableCollection<RewardEntity> Rewards { get; } = new();
    public ObservableCollection<RewardRedemptionEntity> RewardRedemptions { get; } = new();

    public bool HasNoRewards => Rewards.Count == 0;

    [ObservableProperty]
    private string newRewardEmoji = string.Empty;

    [ObservableProperty]
    private string newRewardTitle = string.Empty;

    [ObservableProperty]
    private string newRewardCostText = string.Empty;

    [ObservableProperty]
    private string rewardErrorMessage = string.Empty;

    private async Task ReloadRewardsAsync()
    {
        Rewards.Clear();
        foreach (var reward in await _rewardRepo.GetAllAsync())
        {
            Rewards.Add(reward);
        }

        OnPropertyChanged(nameof(HasNoRewards));
    }

    [RelayCommand]
    private async Task AddRewardAsync()
    {
        RewardErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(NewRewardTitle))
        {
            RewardErrorMessage = LocalizationService.Instance["Parent_Rewards_ErrorTitleMissing"];
            return;
        }

        if (!int.TryParse(NewRewardCostText.Trim(), out var cost) || cost < 1)
        {
            RewardErrorMessage = LocalizationService.Instance["Parent_Rewards_ErrorCostInvalid"];
            return;
        }

        await _rewardRepo.AddAsync(NewRewardEmoji, NewRewardTitle, cost);
        NewRewardEmoji = string.Empty;
        NewRewardTitle = string.Empty;
        NewRewardCostText = string.Empty;
        await ReloadRewardsAsync();
    }

    [RelayCommand]
    private async Task DeleteRewardAsync(RewardEntity reward)
    {
        await _rewardRepo.DeleteAsync(reward.Id);
        await ReloadRewardsAsync();
    }

    // --- Fehlerprotokoll (lokale Log-Dateien, siehe LernTor.Core.Logging.AppLog) ---

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasErrorLogEntries))]
    private string errorLogTail = string.Empty;

    public bool HasErrorLogEntries => !string.IsNullOrWhiteSpace(ErrorLogTail);

    /// <summary>Lädt die letzten Zeilen des heutigen Protokolls neu (beim Öffnen und per Button).</summary>
    [RelayCommand]
    private void RefreshErrorLog() => ErrorLogTail = Core.Logging.AppLog.ReadTodayTail();

    /// <summary>Öffnet den Log-Ordner im Windows-Explorer (für ältere Tage / zum Weitergeben).</summary>
    [RelayCommand]
    private void OpenLogFolder()
    {
        try
        {
            System.IO.Directory.CreateDirectory(Core.Logging.AppLog.LogDirectory);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Core.Logging.AppLog.LogDirectory,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Core.Logging.AppLog.Warn("Eltern-Bereich", $"Log-Ordner konnte nicht geöffnet werden - {ex.Message}");
        }
    }

    public event Action? RequestClose;

    public ParentSettingsViewModel(
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        DatabaseMaintenanceRepository maintenanceRepo,
        CustomQuestionRepository customQuestionRepo,
        KioskLockService kioskLock,
        LocalLlmOptions localLlmOptions,
        TeacherDocumentImportService teacherImportService,
        PiperTtsEngine piperTts,
        RewardRepository rewardRepo)
    {
        _rewardRepo = rewardRepo;
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _profileRepo = profileRepo;
        _maintenanceRepo = maintenanceRepo;
        _customQuestionRepo = customQuestionRepo;
        _kioskLock = kioskLock;
        _localLlmOptions = localLlmOptions;
        _teacherImportService = teacherImportService;
        _piperTts = piperTts;
        IsPiperInstalled = piperTts.IsInstalled;
    }

    /// <summary>Unterdrückt das Sofort-Speichern, während InitializeAsync die Werte aus der DB in
    /// die Properties lädt - sonst würde das Laden selbst sofort wieder (halb befüllt) speichern.</summary>
    private bool _isLoadingSettings;

    public async Task InitializeAsync()
    {
        _settings = await _settingsRepo.LoadAsync();
        IsFirstTimeSetup = string.IsNullOrEmpty(_settings.AdminPasswordHash);

        _isLoadingSettings = true;
        try
        {
            LocalLlmModelPath = _settings.LocalLlmModelPath ?? string.Empty;
            SelectedLlmModel = LocalLlmModelCatalog.Resolve(_settings.LocalLlmModelKey);
            StreaksEnabled = _settings.StreaksEnabled;
            PauseUntil = _settings.PauseUntilDate is { } pauseUntil
                ? pauseUntil.ToDateTime(TimeOnly.MinValue)
                : null;
        }
        finally
        {
            _isLoadingSettings = false;
        }

        ApplyLocalLlmOptions();
        RefreshErrorLog();
        await ReloadRewardsAsync();
    }

    private void ApplyLocalLlmOptions()
    {
        _localLlmOptions.ModelPath = string.IsNullOrWhiteSpace(LocalLlmModelPath) ? null : LocalLlmModelPath;
        _localLlmOptions.ModelKey = SelectedLlmModel.Key;
    }

    /// <summary>
    /// Modell-Auswahl (Dropdown wie eigene Datei) wird SOFORT gespeichert und angewendet, nicht erst
    /// beim "Speichern"-Button: wer das Fenster über "Schließen" verließ, verlor die Auswahl vorher
    /// kommentarlos - genau so als Bug gemeldet.
    /// </summary>
    partial void OnSelectedLlmModelChanged(LocalLlmModelInfo value) => PersistLlmSelection();

    partial void OnLocalLlmModelPathChanged(string value) => PersistLlmSelection();

    private void PersistLlmSelection()
    {
        if (_isLoadingSettings)
        {
            return;
        }

        _settings.LocalLlmModelPath = string.IsNullOrWhiteSpace(LocalLlmModelPath) ? null : LocalLlmModelPath;
        _settings.LocalLlmModelKey = SelectedLlmModel.Key;
        ApplyLocalLlmOptions();
        _ = _settingsRepo.SaveAsync(_settings);
    }

    /// <summary>Einmaliger Download der natürlichen Piper-Vorlesestimmen (~230 MB, siehe
    /// <see cref="PiperTtsEngine"/>). Läuft im Hintergrund weiter, auch wenn das Eltern-Fenster
    /// währenddessen geschlossen wird - beim nächsten Öffnen zeigt IsPiperInstalled den Stand.</summary>
    [RelayCommand]
    private async Task InstallPiperVoicesAsync()
    {
        if (IsPiperInstalling || IsPiperInstalled)
        {
            return;
        }

        IsPiperInstalling = true;
        PiperErrorMessage = string.Empty;
        var progress = new Progress<string>(message => PiperStatusMessage = message);

        try
        {
            await _piperTts.InstallAsync(progress, CancellationToken.None);
            IsPiperInstalled = _piperTts.IsInstalled;
        }
        catch (Exception ex)
        {
            PiperStatusMessage = string.Empty;
            PiperErrorMessage = ex.Message;
        }
        finally
        {
            IsPiperInstalling = false;
        }
    }

    [RelayCommand]
    private async Task LoginAsync(string password)
    {
        ErrorMessage = string.Empty;

        if (IsFirstTimeSetup)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                ErrorMessage = "Das Passwort muss mindestens 4 Zeichen lang sein.";
                return;
            }

            var (hash, salt) = AdminAuthService.HashPassword(password);
            _settings.AdminPasswordHash = hash;
            _settings.AdminPasswordSalt = salt;
            await _settingsRepo.SaveAsync(_settings);
            IsFirstTimeSetup = false;
            await AuthenticateAsync();
            return;
        }

        if (AdminAuthService.Verify(password, _settings.AdminPasswordHash, _settings.AdminPasswordSalt))
        {
            await AuthenticateAsync();
        }
        else
        {
            ErrorMessage = "Falsches Passwort.";
        }
    }

    private async Task AuthenticateAsync()
    {
        IsAuthenticated = true;

        SubjectToggles.Clear();
        foreach (var (subject, translationKey) in ToggleableSubjects)
        {
            SubjectToggles.Add(new SubjectToggle(
                subject,
                LocalizationService.Instance[translationKey],
                _settings.DisabledSubjects.Contains(subject)));
        }

        Profiles.Clear();
        foreach (var profile in await _profileRepo.GetAllAsync())
        {
            Profiles.Add(profile);
        }

        SelectedProfile = Profiles.FirstOrDefault(p => p.Id == PreselectProfileId) ?? Profiles.FirstOrDefault();

        await ReloadCustomQuestionsAsync();
    }

    private async Task ReloadCustomQuestionsAsync()
    {
        CustomQuestions.Clear();
        foreach (var question in await _customQuestionRepo.GetAllAsync())
        {
            CustomQuestions.Add(question);
        }

        OnPropertyChanged(nameof(HasNoCustomQuestions));
    }

    partial void OnSelectedProfileChanged(StudentProfile? value)
    {
        _ = ReloadActivityForSelectedProfileAsync();

        TypingMinAccuracyPercent = PercentFromFraction(value?.TypingMinAccuracy, 25);
        QuizFirstAttemptThresholdPercent = PercentFromFraction(value?.QuizFirstAttemptThreshold, 50);
        QuizRetryThresholdPercent = PercentFromFraction(value?.QuizRetryThreshold, 25);
        ReadingMinutes = value?.ReadingMinutes ?? StudentProfile.DefaultReadingMinutes;
        NewsSecondsPerArticle = value?.NewsSecondsPerArticle ?? StudentProfile.DefaultNewsSecondsPerArticle;
        ExerciseSecondsPerQuestion = value?.ExerciseSecondsPerQuestion ?? StudentProfile.DefaultExerciseSecondsPerQuestion;
        ExercisesPerSubject = value?.ExercisesPerSubject ?? StudentProfile.DefaultExercisesPerSubject;
        QuizQuestionCount = value?.QuizQuestionCount ?? StudentProfile.DefaultQuizQuestionCount;
        QuizRetryQuestionCount = value?.QuizRetryQuestionCount ?? StudentProfile.DefaultQuizRetryQuestionCount;
    }

    private static int PercentFromFraction(double? fraction, int fallbackPercent) =>
        fraction is null ? fallbackPercent : (int)Math.Round(fraction.Value * 100);

    // --- Schwierigkeitsstufen pro Profil (Tipptrainer-Mindestgenauigkeit, Abschlussquiz-Schwellenwerte) ---

    /// <summary>🔥-Lernserie auf dem Willkommensbildschirm anzeigen (global, Standard aus -
    /// bewusst kein Streak-Druck, siehe StreakCalculator). Direkt in _settings gespiegelt, damit
    /// JEDER Speicherpfad (Haupt-Speichern wie LLM-Sofort-Speichern) den aktuellen Wert persistiert.</summary>
    [ObservableProperty]
    private bool streaksEnabled;

    partial void OnStreaksEnabledChanged(bool value) => _settings.StreaksEnabled = value;

    /// <summary>Preset-Werte für die Tipptrainer-Mindestgenauigkeit (siehe TabPillButton-Gruppe im Eltern-Bereich).</summary>
    [ObservableProperty]
    private int typingMinAccuracyPercent = 25;

    /// <summary>Preset-Werte für den 1. Abschlussquiz-Versuch am Tag.</summary>
    [ObservableProperty]
    private int quizFirstAttemptThresholdPercent = 50;

    /// <summary>Preset-Werte für den 2. Abschlussquiz-Versuch (Wiederholung nach Nichtbestehen).</summary>
    [ObservableProperty]
    private int quizRetryThresholdPercent = 25;

    [RelayCommand]
    private void SetTypingMinAccuracy(string percent)
    {
        TypingMinAccuracyPercent = int.TryParse(percent, out var parsed) ? parsed : 25;
    }

    [RelayCommand]
    private void SetQuizFirstAttemptThreshold(string percent)
    {
        QuizFirstAttemptThresholdPercent = int.TryParse(percent, out var parsed) ? parsed : 50;
    }

    [RelayCommand]
    private void SetQuizRetryThreshold(string percent)
    {
        QuizRetryThresholdPercent = int.TryParse(percent, out var parsed) ? parsed : 25;
    }

    // --- Timer pro Profil (Pflicht-Lesezeit, Mindestzeiten News/Übungen) - wie die
    // Schwierigkeitsstufen als Presets, damit kein neuer Build nötig ist, um Zeiten anzupassen. ---

    /// <summary>Pflicht-Lesezeit des Vorlese-Abschnitts in Minuten (Presets 2/5/8/10).</summary>
    [ObservableProperty]
    private int readingMinutes = StudentProfile.DefaultReadingMinutes;

    /// <summary>Mindest-Lesezeit pro News-Artikel in Sekunden (Presets 5/10/20/30).</summary>
    [ObservableProperty]
    private int newsSecondsPerArticle = StudentProfile.DefaultNewsSecondsPerArticle;

    /// <summary>Mindestzeit pro Übungsaufgabe in den Fächern in Sekunden (Presets 3/5/10/15).</summary>
    [ObservableProperty]
    private int exerciseSecondsPerQuestion = StudentProfile.DefaultExerciseSecondsPerQuestion;

    [RelayCommand]
    private void SetReadingMinutes(string minutes)
    {
        ReadingMinutes = int.TryParse(minutes, out var parsed) ? parsed : StudentProfile.DefaultReadingMinutes;
    }

    [RelayCommand]
    private void SetNewsSecondsPerArticle(string seconds)
    {
        NewsSecondsPerArticle = int.TryParse(seconds, out var parsed) ? parsed : StudentProfile.DefaultNewsSecondsPerArticle;
    }

    [RelayCommand]
    private void SetExerciseSecondsPerQuestion(string seconds)
    {
        ExerciseSecondsPerQuestion = int.TryParse(seconds, out var parsed) ? parsed : StudentProfile.DefaultExerciseSecondsPerQuestion;
    }

    // --- Umfang pro Profil (Aufgaben pro Fach, Quiz-Längen) - Presets wie oben. ---

    /// <summary>Generierte Übungsaufgaben pro Fach und Tag (Presets 4/6/8/10).</summary>
    [ObservableProperty]
    private int exercisesPerSubject = StudentProfile.DefaultExercisesPerSubject;

    /// <summary>Fragenzahl des ersten Abschlussquiz (Presets 10/15/20/25).</summary>
    [ObservableProperty]
    private int quizQuestionCount = StudentProfile.DefaultQuizQuestionCount;

    /// <summary>Fragenzahl des Wiederholungs-Quiz (Presets 10/15/20).</summary>
    [ObservableProperty]
    private int quizRetryQuestionCount = StudentProfile.DefaultQuizRetryQuestionCount;

    [RelayCommand]
    private void SetExercisesPerSubject(string count)
    {
        ExercisesPerSubject = int.TryParse(count, out var parsed) ? parsed : StudentProfile.DefaultExercisesPerSubject;
    }

    [RelayCommand]
    private void SetQuizQuestionCount(string count)
    {
        QuizQuestionCount = int.TryParse(count, out var parsed) ? parsed : StudentProfile.DefaultQuizQuestionCount;
    }

    [RelayCommand]
    private void SetQuizRetryQuestionCount(string count)
    {
        QuizRetryQuestionCount = int.TryParse(count, out var parsed) ? parsed : StudentProfile.DefaultQuizRetryQuestionCount;
    }

    // --- Ferien-/Pausenmodus (global): bis einschließlich des Datums keine Kiosk-Sperre. ---

    /// <summary>Enddatum des Ferienmodus als DateTime? (DatePicker-freundlich); direkt in
    /// _settings gespiegelt, damit jeder Speicherpfad den aktuellen Wert persistiert.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPauseActive))]
    private DateTime? pauseUntil;

    /// <summary>Zeigt den Aktiv-Hinweis, solange das gesetzte Datum heute oder in der Zukunft liegt.</summary>
    public bool IsPauseActive => PauseUntil is { } until && until.Date >= DateTime.Today;

    partial void OnPauseUntilChanged(DateTime? value) =>
        _settings.PauseUntilDate = value is { } d ? DateOnly.FromDateTime(d) : null;

    [RelayCommand]
    private void ClearPauseUntil() => PauseUntil = null;

    private async Task ReloadActivityForSelectedProfileAsync()
    {
        RecentActivity.Clear();
        QuizHistory.Clear();
        RewardRedemptions.Clear();
        _reportActivity = Array.Empty<ActivityLogEntity>();

        if (SelectedProfile is null)
        {
            RebuildReport();
            return;
        }

        foreach (var entry in await _activityLogRepo.GetRecentActivityAsync(SelectedProfile.Id))
        {
            RecentActivity.Add(entry);
        }

        foreach (var attempt in await _activityLogRepo.GetQuizHistoryAsync(SelectedProfile.Id))
        {
            QuizHistory.Add(attempt);
        }

        // Eingelöste Belohnungen des gewählten Profils (die Eltern lösen sie in der echten Welt ein).
        foreach (var redemption in await _rewardRepo.GetRedemptionsAsync(SelectedProfile.Id))
        {
            RewardRedemptions.Add(redemption);
        }

        // 30 Tage einmal laden - die 7/30-Tage-Umschaltung filtert danach nur noch in-memory.
        _reportActivity = await _activityLogRepo.GetActivitySinceAsync(SelectedProfile.Id, TimeSpan.FromDays(30));
        RebuildReport();
    }

    // --- Wochen-/Monatsbericht (Stärken/Schwächen je Fach, Lerntage, Quiz-Verlauf) ---

    private IReadOnlyList<ActivityLogEntity> _reportActivity = Array.Empty<ActivityLogEntity>();

    public ObservableCollection<SubjectReportRowViewModel> ReportRows { get; } = new();

    /// <summary>Themen-Heatmap: die schwächsten Einzel-Themen im Zeitraum (max. 10, mindestens
    /// 3 Antworten je Thema als Datenbasis) - zeigt Eltern, dass z.B. genau "Brüche" hakt,
    /// nicht nur pauschal "Mathe".</summary>
    public ObservableCollection<SubjectReportRowViewModel> TopicReportRows { get; } = new();

    [ObservableProperty]
    private bool hasTopicReportData;

    [ObservableProperty]
    private int reportDays = 7;

    [ObservableProperty]
    private string reportLearnedDaysDisplay = string.Empty;

    [ObservableProperty]
    private string reportQuizTrendDisplay = string.Empty;

    [ObservableProperty]
    private bool hasReportData;

    [RelayCommand]
    private void SetReportPeriod(string days)
    {
        ReportDays = int.TryParse(days, out var parsed) ? parsed : 7;
        RebuildReport();
    }

    private void RebuildReport()
    {
        ReportRows.Clear();
        TopicReportRows.Clear();

        var loc = LocalizationService.Instance;
        var cutoff = DateTimeOffset.Now - TimeSpan.FromDays(ReportDays);
        var answers = _reportActivity.Where(a => a.Timestamp >= cutoff).ToList();

        HasReportData = answers.Count > 0;
        if (!HasReportData)
        {
            HasTopicReportData = false;
            ReportLearnedDaysDisplay = string.Empty;
            ReportQuizTrendDisplay = string.Empty;
            return;
        }

        // Schwächste Fächer zuerst - Eltern wollen sehen, wo Unterstützung nötig ist.
        var bySubject = answers
            .GroupBy(a => a.Subject)
            .Select(g => new SubjectReportRowViewModel
            {
                Label = loc[$"Stage_{g.Key}"],
                Correct = g.Count(a => a.WasCorrect),
                Total = g.Count()
            })
            .OrderBy(r => r.Rate)
            .ThenByDescending(r => r.Total);

        foreach (var row in bySubject)
        {
            ReportRows.Add(row);
        }

        // Themen-Heatmap: nur Themen mit genug Datenbasis (>= 3 Antworten), schwächste zuerst,
        // maximal 10 Zeilen - Eltern brauchen die Brennpunkte, keine vollständige Themenliste.
        var weakestTopics = answers
            .GroupBy(a => (a.Subject, a.Topic))
            .Where(g => g.Count() >= 3)
            .Select(g => new SubjectReportRowViewModel
            {
                Label = $"{loc[$"Stage_{g.Key.Subject}"]} · {g.Key.Topic}",
                Correct = g.Count(a => a.WasCorrect),
                Total = g.Count()
            })
            .OrderBy(r => r.Rate)
            .ThenByDescending(r => r.Total)
            .Take(10)
            .ToList();

        HasTopicReportData = weakestTopics.Count > 0;
        foreach (var row in weakestTopics)
        {
            TopicReportRows.Add(row);
        }

        var learnedDays = answers.Select(a => DateOnly.FromDateTime(a.Timestamp.LocalDateTime)).Distinct().Count();
        ReportLearnedDaysDisplay = string.Format(loc["Parent_Report_LearnedDays"], learnedDays, ReportDays);

        // QuizHistory ist neueste-zuerst sortiert; für den Trend chronologisch (älteste → neueste).
        var quizScores = QuizHistory
            .Where(q => q.Timestamp >= cutoff)
            .Reverse()
            .Select(q => $"{q.ScorePercentage:P0}{(q.Passed ? " ✓" : string.Empty)}")
            .ToList();
        ReportQuizTrendDisplay = quizScores.Count == 0
            ? loc["Parent_Report_NoQuiz"]
            : string.Format(loc["Parent_Report_QuizTrend"], string.Join("  →  ", quizScores));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        _settings.DisabledSubjects.Clear();
        foreach (var toggle in SubjectToggles)
        {
            if (toggle.IsDisabled)
            {
                _settings.DisabledSubjects.Add(toggle.Subject);
            }
        }

        _settings.LocalLlmModelPath = string.IsNullOrWhiteSpace(LocalLlmModelPath) ? null : LocalLlmModelPath;
        _settings.LocalLlmModelKey = SelectedLlmModel.Key;
        ApplyLocalLlmOptions();

        await _settingsRepo.SaveAsync(_settings);

        if (SelectedProfile is not null)
        {
            var typingMinAccuracy = TypingMinAccuracyPercent / 100.0;
            var quizFirstAttemptThreshold = QuizFirstAttemptThresholdPercent / 100.0;
            var quizRetryThreshold = QuizRetryThresholdPercent / 100.0;

            await _profileRepo.UpdateSettingsAsync(SelectedProfile.Id, typingMinAccuracy, quizFirstAttemptThreshold, quizRetryThreshold,
                ReadingMinutes, NewsSecondsPerArticle, ExerciseSecondsPerQuestion,
                ExercisesPerSubject, QuizQuestionCount, QuizRetryQuestionCount);

            SelectedProfile.TypingMinAccuracy = typingMinAccuracy;
            SelectedProfile.QuizFirstAttemptThreshold = quizFirstAttemptThreshold;
            SelectedProfile.QuizRetryThreshold = quizRetryThreshold;
            SelectedProfile.ReadingMinutes = ReadingMinutes;
            SelectedProfile.NewsSecondsPerArticle = NewsSecondsPerArticle;
            SelectedProfile.ExerciseSecondsPerQuestion = ExerciseSecondsPerQuestion;
            SelectedProfile.ExercisesPerSubject = ExercisesPerSubject;
            SelectedProfile.QuizQuestionCount = QuizQuestionCount;
            SelectedProfile.QuizRetryQuestionCount = QuizRetryQuestionCount;
        }

        RequestClose?.Invoke();
    }

    /// <summary>Entfernt die eigene Modelldatei wieder - danach gilt erneut das Katalog-Modell.</summary>
    [RelayCommand]
    private void ClearLocalLlmModelFile() => LocalLlmModelPath = string.Empty;

    /// <summary>Öffnet einen Datei-Dialog zur Auswahl einer lokalen GGUF-Modelldatei (LLamaSharp).</summary>
    [RelayCommand]
    private void PickLocalLlmModelFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "GGUF-Modell (*.gguf)|*.gguf",
            Title = "Lokales LLM-Modell auswählen"
        };

        if (dialog.ShowDialog() == true)
        {
            LocalLlmModelPath = dialog.FileName;
        }
    }

    /// <summary>Öffnet einen Datei-Dialog zur Auswahl einer Lehrer-Unterlage (PDF oder Word .docx).</summary>
    [RelayCommand]
    private void PickImportFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Unterrichtsmaterial (*.pdf;*.docx)|*.pdf;*.docx",
            Title = "Lehrer-Unterlage auswählen"
        };

        if (dialog.ShowDialog() == true)
        {
            ImportFilePath = dialog.FileName;
        }
    }

    /// <summary>
    /// Extrahiert Text aus der gewählten Datei und lässt das lokale KI-Modell Fragenentwürfe
    /// vorschlagen. Die Entwürfe werden NICHT automatisch gespeichert - Eltern müssen jeden einzeln
    /// über <see cref="AcceptImportedDraftAsync"/> bestätigen oder über <see cref="DiscardImportedDraft"/>
    /// verwerfen (siehe ExtractedQuestionDraft-Dokumentation: keine Automatik ohne menschliche Kontrolle).
    /// </summary>
    [RelayCommand]
    private async Task RunImportAsync()
    {
        ImportErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(ImportFilePath) || !File.Exists(ImportFilePath))
        {
            ImportErrorMessage = "Bitte zuerst eine gültige Datei auswählen.";
            return;
        }

        IsImporting = true;
        try
        {
            await using var fileStream = File.OpenRead(ImportFilePath);
            var drafts = await _teacherImportService.ImportAsync(fileStream, ImportFilePath, ImportSubject, ImportGrade);

            ImportedDrafts.Clear();
            foreach (var draft in drafts)
            {
                ImportedDrafts.Add(new EditableDraftViewModel(draft));
            }

            OnPropertyChanged(nameof(HasNoImportedDrafts));

            if (drafts.Count == 0)
            {
                ImportErrorMessage = "Die KI hat keine Fragenvorschläge aus diesem Dokument geliefert.";
            }
        }
        catch (Exception ex)
        {
            ImportErrorMessage = ex.Message;
        }
        finally
        {
            IsImporting = false;
        }
    }

    /// <summary>Übernimmt einen geprüften (und ggf. inline korrigierten) Vorschlag als echte eigene
    /// Aufgabe. Bei Validierungsfehlern bleibt die Karte mit Fehlermeldung stehen.</summary>
    [RelayCommand]
    private async Task AcceptImportedDraftAsync(EditableDraftViewModel draft)
    {
        var question = draft.TryBuildQuestion(ImportSubject, ImportGrade);
        if (question is null)
        {
            return;
        }

        await _customQuestionRepo.AddAsync(question);

        ImportedDrafts.Remove(draft);
        OnPropertyChanged(nameof(HasNoImportedDrafts));
        await ReloadCustomQuestionsAsync();
    }

    /// <summary>Verwirft einen Vorschlag, ohne ihn zu speichern.</summary>
    [RelayCommand]
    private void DiscardImportedDraft(EditableDraftViewModel draft)
    {
        ImportedDrafts.Remove(draft);
        OnPropertyChanged(nameof(HasNoImportedDrafts));
    }

    [RelayCommand]
    private void SkipUnlock()
    {
        // Sofort-Freischaltung ist ein reiner Notfall-Override: entsperrt den PC unabhängig
        // davon, ob/welches Kind-Profil gerade aktiv war, ohne dessen Fortschritt zu verändern.
        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Direkter Weg auf dem Login-Bildschirm: Passwort eingeben und sofort entsperren/beenden,
    /// ohne erst in die volle Einstellungsansicht wechseln zu müssen.
    /// </summary>
    [RelayCommand]
    private void UnlockAndExit(string password)
    {
        ErrorMessage = string.Empty;

        if (IsFirstTimeSetup)
        {
            ErrorMessage = "Bitte zuerst über \"Anmelden\" ein Admin-Passwort festlegen.";
            return;
        }

        if (AdminAuthService.Verify(password, _settings.AdminPasswordHash, _settings.AdminPasswordSalt))
        {
            SkipUnlock();
        }
        else
        {
            ErrorMessage = "Falsches Passwort.";
        }
    }

    [RelayCommand]
    private void Close() => RequestClose?.Invoke();

    /// <summary>
    /// Löscht das oben ausgewählte Profil samt Fortschritt/Protokoll/Quiz-Historie - nach
    /// Rückfrage. Das letzte verbleibende Profil ist nicht löschbar (der Kiosk-Ablauf braucht
    /// mindestens ein Profil; wer wirklich alles entfernen will, nutzt "Alle Daten zurücksetzen").
    /// </summary>
    [RelayCommand]
    private async Task DeleteSelectedProfileAsync()
    {
        if (SelectedProfile is null)
        {
            return;
        }

        if (Profiles.Count <= 1)
        {
            System.Windows.MessageBox.Show(
                "Das letzte Profil kann nicht gelöscht werden - die App braucht mindestens ein Profil.",
                "Profil löschen",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            return;
        }

        var confirmed = System.Windows.MessageBox.Show(
            $"Profil \"{SelectedProfile.Name}\" mit allen Fortschritten, Protokollen und der " +
            "Quiz-Historie unwiderruflich löschen?",
            "Profil löschen",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning,
            System.Windows.MessageBoxResult.No);

        if (confirmed != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        var toDelete = SelectedProfile;
        await _profileRepo.DeleteAsync(toDelete.Id);
        Profiles.Remove(toDelete);
        SelectedProfile = Profiles.FirstOrDefault();
    }

    /// <summary>
    /// Löscht unwiderruflich alle Profile, Fortschritte, Aktivitätsprotokolle und Einstellungen.
    /// Vorher ging das nur manuell über das Löschen der lerntor.db-Datei. Erfordert eine explizite
    /// Ja/Nein-Bestätigung, damit ein Klick während der normalen Nutzung nicht versehentlich alles
    /// zurücksetzt.
    /// </summary>
    [RelayCommand]
    private async Task ResetAllDataAsync()
    {
        var confirmed = System.Windows.MessageBox.Show(
            "Wirklich ALLE Profile, Fortschritte und Einstellungen unwiderruflich löschen?\n\nDas kann nicht rückgängig gemacht werden.",
            "Datenbank zurücksetzen",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning,
            System.Windows.MessageBoxResult.No);

        if (confirmed != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        await _maintenanceRepo.ResetAllDataAsync();

        System.Windows.MessageBox.Show(
            "Zurückgesetzt. LernTor wird jetzt beendet - beim nächsten Start werden wieder die Standardprofile angelegt.",
            "Datenbank zurückgesetzt",
            System.Windows.MessageBoxButton.OK,
            System.Windows.MessageBoxImage.Information);

        // Vor jedem beabsichtigten Beenden zuerst entsperren - MainWindow verweigert das
        // Schließen sonst (Schutz gegen den Alt+Tab-"X"-Button, siehe MainWindow_Closing).
        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Exportiert eine konsistente Sicherung der kompletten Datenbank (Profile, Fortschritte,
    /// Sterne, Einstellungen) als einzelne .db-Datei, z.B. auf einen USB-Stick. Die lerntor.db
    /// ist sonst ein Single Point of Failure - Plattendefekt = Monate Fortschritt weg.
    /// </summary>
    [RelayCommand]
    private async Task ExportBackupAsync()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "LernTor-Sicherung (*.db)|*.db",
            FileName = $"lerntor-backup-{DateTime.Now:yyyy-MM-dd}.db",
            Title = "LernTor-Sicherung speichern"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            await _maintenanceRepo.ExportBackupAsync(dialog.FileName);
            System.Windows.MessageBox.Show(
                $"Sicherung gespeichert:\n{dialog.FileName}",
                "Sicherung erstellt",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            LernTor.Core.Logging.AppLog.Error("Parent", "Backup-Export fehlgeschlagen", ex);
            System.Windows.MessageBox.Show(
                $"Sicherung fehlgeschlagen:\n{ex.Message}",
                "Sicherung erstellen",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Ersetzt die aktive Datenbank durch eine zuvor exportierte Sicherung. Beendet die App
    /// danach (wie ResetAllData): der laufende Prozess hätte sonst veraltete Daten im Speicher,
    /// erst der nächste Start lädt die wiederhergestellten Profile/Fortschritte.
    /// </summary>
    [RelayCommand]
    private void ImportBackup()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "LernTor-Sicherung (*.db)|*.db|Alle Dateien (*.*)|*.*",
            Title = "LernTor-Sicherung wiederherstellen"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        var confirmed = System.Windows.MessageBox.Show(
            "Die aktuelle Datenbank wird durch die gewählte Sicherung ersetzt.\n\n" +
            "Alle SEIT der Sicherung entstandenen Fortschritte und Sterne gehen dabei verloren. Fortfahren?",
            "Sicherung wiederherstellen",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning,
            System.Windows.MessageBoxResult.No);

        if (confirmed != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            DatabaseMaintenanceRepository.ImportBackup(dialog.FileName);
        }
        catch (Exception ex)
        {
            LernTor.Core.Logging.AppLog.Error("Parent", "Backup-Import fehlgeschlagen", ex);
            System.Windows.MessageBox.Show(
                $"Wiederherstellung fehlgeschlagen:\n{ex.Message}",
                "Sicherung wiederherstellen",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
            return;
        }

        System.Windows.MessageBox.Show(
            "Wiederhergestellt. LernTor wird jetzt beendet - beim nächsten Start sind die Daten aus der Sicherung aktiv.",
            "Sicherung wiederhergestellt",
            System.Windows.MessageBoxButton.OK,
            System.Windows.MessageBoxImage.Information);

        // Vor jedem beabsichtigten Beenden zuerst entsperren - MainWindow verweigert das
        // Schließen sonst (Schutz gegen den Alt+Tab-"X"-Button, siehe MainWindow_Closing).
        _kioskLock.Unlock();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Legt eine eigene Aufgabe an (z.B. aktuelle Hausaufgabe der Lehrkraft). Ergänzt die
    /// generierten Aufgaben aus LernTor.ContentGen additiv, ersetzt sie nicht.
    /// </summary>
    [RelayCommand]
    private async Task AddCustomQuestionAsync()
    {
        CustomQuestionErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(NewQuestionPrompt))
        {
            CustomQuestionErrorMessage = "Bitte eine Frage/Aufgabenstellung eingeben.";
            return;
        }

        var correctAnswers = SplitCommaSeparated(NewQuestionCorrectAnswersText);
        if (correctAnswers.Count == 0)
        {
            CustomQuestionErrorMessage = "Bitte mindestens eine richtige Antwort eingeben.";
            return;
        }

        var options = NewQuestionNeedsOptions ? SplitCommaSeparated(NewQuestionOptionsText) : Array.Empty<string>();
        if (NewQuestionNeedsOptions && options.Count < 2)
        {
            CustomQuestionErrorMessage = "Bitte mindestens zwei Antwortoptionen eingeben (mit Komma getrennt).";
            return;
        }

        await _customQuestionRepo.AddAsync(new QuizQuestion
        {
            Id = Guid.NewGuid().ToString("N"),
            Subject = NewQuestionSubject,
            GradeLevel = NewQuestionGrade,
            Topic = string.IsNullOrWhiteSpace(NewQuestionTopic) ? "Eigene Aufgabe" : NewQuestionTopic,
            Type = NewQuestionType,
            Prompt = NewQuestionPrompt,
            Options = options,
            CorrectAnswers = correctAnswers,
            Explanation = string.IsNullOrWhiteSpace(NewQuestionExplanation) ? "-" : NewQuestionExplanation,
            HelpHint = string.IsNullOrWhiteSpace(NewQuestionHelpHint) ? null : NewQuestionHelpHint
        });

        NewQuestionTopic = string.Empty;
        NewQuestionPrompt = string.Empty;
        NewQuestionOptionsText = string.Empty;
        NewQuestionCorrectAnswersText = string.Empty;
        NewQuestionExplanation = string.Empty;
        NewQuestionHelpHint = string.Empty;

        await ReloadCustomQuestionsAsync();
    }

    [RelayCommand]
    private async Task DeleteCustomQuestionAsync(QuizQuestion question)
    {
        await _customQuestionRepo.DeleteAsync(question.Id);
        await ReloadCustomQuestionsAsync();
    }

    private static IReadOnlyList<string> SplitCommaSeparated(string text) =>
        text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
