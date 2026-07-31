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
using LernTor.Core.Services;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;
using LernTor.News;
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
    private readonly CustomReadingTextRepository _customReadingRepo;
    private readonly HomeworkTaskRepository _homeworkRepo;
    private readonly ExamEntryRepository _examRepo;
    private readonly FeedHealthLog _feedHealthLog = new();
    private readonly VocabularyRepository _vocabularyRepo;
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

    /// <summary>
    /// Erklärt in Klartext, welches Profil eine eigene Aufgabe überhaupt zu sehen bekommt.
    /// Eigene Aufgaben werden nach Fach UND Klassenstufe gefiltert - eine für Klasse 6
    /// eingetragene Aufgabe taucht bei einem Klasse-9-Kind nie auf. Ohne diesen Hinweis ist das
    /// die häufigste stille Fehlbedienung (Rückmeldung aus dem Familienbetrieb).
    /// </summary>
    public string CustomQuestionProfileHint
    {
        get
        {
            if (Profiles.Count == 0)
            {
                return string.Empty;
            }

            var byGrade = Profiles
                .GroupBy(p => p.GradeLevel)
                .OrderBy(g => (int)g.Key)
                .Select(g => $"{g.Key} → {string.Join(", ", g.Select(p => p.Name))}");

            return "Wer bekommt was: " + string.Join("  |  ", byGrade) +
                   ". Die Klassenstufe der Aufgabe muss zur Stufe des Profils passen.";
        }
    }

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

    /// <summary>Laufender Status während des Imports (Laufzeit + Schritt), damit nicht nur
    /// "wird eingelesen…" dasteht, während das Modell auf der CPU rechnet.</summary>
    [ObservableProperty]
    private string importStatusMessage = string.Empty;

    /// <summary>Erlaubt das Abbrechen eines laufenden Imports - vorher gab es keinen Ausweg.</summary>
    private CancellationTokenSource? _importCancellation;

    private readonly System.Windows.Threading.DispatcherTimer _importTimer = new()
    {
        Interval = TimeSpan.FromSeconds(1)
    };

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

    /// <summary>
    /// Ob seit dem letzten Speichern etwas geaendert wurde, das nur ueber "Speichern" in die
    /// Datenbank kommt (Presets, Zeiten, Fach-Schalter, eigene Tipp-Texte). Einstellungen, die
    /// sofort speichern - Lesetexte, Nachrichtenquellen, Belohnungen, LLM-Auswahl - zaehlen
    /// bewusst NICHT dazu, sonst waere die Rueckfrage beim Schliessen dauernd falsch positiv.
    /// </summary>
    public bool HasUnsavedChanges { get; private set; }

    /// <summary>Unterdrueckt die Aenderungsmarkierung, waehrend die Editor-Felder mit den Werten
    /// des gewaehlten Profils befuellt werden - sonst gaelte schon ein Profilwechsel als Aenderung.</summary>
    private bool _loadingProfileEditor;

    /// <summary>
    /// Markiert die Einstellungen als geaendert - von den betroffenen Eigenschaften aufgerufen.
    /// Beide Ladevorgaenge muessen stumm bleiben: das Befuellen der Editor-Felder beim
    /// Profilwechsel UND das Laden der globalen Einstellungen in InitializeAsync (Lernserie,
    /// Pausenmodus) - sonst stuende die Rueckfrage schon beim blossen Oeffnen an.
    /// </summary>
    private void MarkDirty()
    {
        if (!_loadingProfileEditor && !_isLoadingSettings)
        {
            HasUnsavedChanges = true;
        }
    }

    // Diese Eigenschaften landen erst ueber "Speichern" in der Datenbank - jede Aenderung daran
    // macht die Rueckfrage beim Schliessen noetig.
    partial void OnTypingMinAccuracyPercentChanged(int value) => MarkDirty();
    partial void OnQuizFirstAttemptThresholdPercentChanged(int value) => MarkDirty();
    partial void OnQuizRetryThresholdPercentChanged(int value) => MarkDirty();
    partial void OnReadingMinutesChanged(int value) => MarkDirty();
    partial void OnNewsSecondsPerArticleChanged(int value) => MarkDirty();
    partial void OnNewsFilterStrictnessChanged(NewsFilterStrictness value) => MarkDirty();

    partial void OnNewsArticleCountChanged(int value)
    {
        MarkDirty();

        // Der Hinweistext unter den Quellen nennt die Anzahl - sonst stuende dort eine veraltete
        // Rechnung, sobald die Eltern die Anzahl umstellen.
        UpdateNewsFeedStatus();
    }
    partial void OnExerciseSecondsPerQuestionChanged(int value) => MarkDirty();
    partial void OnExercisesPerSubjectChanged(int value) => MarkDirty();
    partial void OnQuizQuestionCountChanged(int value) => MarkDirty();
    partial void OnQuizRetryQuestionCountChanged(int value) => MarkDirty();
    partial void OnWeeklyGoalDaysChanged(int value) => MarkDirty();
    partial void OnCustomTypingSentenceTextChanged(string value) => MarkDirty();
    partial void OnCustomTypingFinalTextChanged(string value) => MarkDirty();

    public ParentSettingsViewModel(
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        DatabaseMaintenanceRepository maintenanceRepo,
        CustomQuestionRepository customQuestionRepo,
        CustomReadingTextRepository customReadingRepo,
        HomeworkTaskRepository homeworkRepo,
        ExamEntryRepository examRepo,
        VocabularyRepository vocabularyRepo,
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
        _customReadingRepo = customReadingRepo;
        _homeworkRepo = homeworkRepo;
        _examRepo = examRepo;
        _vocabularyRepo = vocabularyRepo;
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
                _settings.DisabledSubjects.Contains(subject),
                MarkDirty));
        }

        Profiles.Clear();
        foreach (var profile in await _profileRepo.GetAllAsync())
        {
            Profiles.Add(profile);
        }

        SelectedProfile = Profiles.FirstOrDefault(p => p.Id == PreselectProfileId) ?? Profiles.FirstOrDefault();
        OnPropertyChanged(nameof(CustomQuestionProfileHint));
        BuildNewsFeedToggles();

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

    /// <summary>Zu welchem Profil die Werte in den Editor-Feldern gerade gehoeren.</summary>
    private StudentProfile? _editorProfile;

    partial void OnSelectedProfileChanged(StudentProfile? value) => _ = HandleProfileSwitchAsync(value);

    /// <summary>
    /// Übernimmt die Werte des gewählten Profils in die Editor-Felder.
    ///
    /// <para>Vorher werden ungespeicherte Änderungen des BISHERIGEN Profils abgefragt: die Felder
    /// werden gleich überschrieben, und weil <see cref="_loadingProfileEditor"/> dabei die
    /// Änderungsmarkierung unterdrückt, wären sie lautlos weg gewesen - ein Profilwechsel zum
    /// Nachschauen hat also stillschweigend die gerade eingestellten Zeiten verworfen.</para>
    /// </summary>
    private async Task HandleProfileSwitchAsync(StudentProfile? value)
    {
        if (HasUnsavedChanges && _editorProfile is not null && !ReferenceEquals(_editorProfile, value))
        {
            var answer = System.Windows.MessageBox.Show(
                $"Für \"{_editorProfile.Name}\" gibt es ungespeicherte Änderungen.\n\n" +
                "Sollen sie gespeichert werden, bevor das Profil gewechselt wird?",
                "Änderungen speichern?",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question,
                System.Windows.MessageBoxResult.Yes);

            if (answer == System.Windows.MessageBoxResult.Yes)
            {
                // Bewusst VOR ApplyProfileToEditor abgewartet: die Editor-Felder werden gleich
                // ueberschrieben, und der geteilte DbContext vertraegt keine parallelen Zugriffe.
                await SaveProfileEditorAsync(_editorProfile);
            }
        }

        // Werte kommen aus der DB, das ist keine Nutzeraenderung.
        _loadingProfileEditor = true;
        try
        {
            ApplyProfileToEditor(value);
        }
        finally
        {
            _loadingProfileEditor = false;
        }

        _editorProfile = value;

        // Die Felder zeigen jetzt exakt den Stand aus der Datenbank.
        HasUnsavedChanges = false;
    }

    /// <summary>
    /// Schreibt die aktuellen Editor-Felder als Einstellungen des übergebenen Profils weg.
    /// Die Werte werden beim Aufruf eingesammelt (Argumente werden vor dem ersten await
    /// ausgewertet), das Ergebnis ist also unabhängig davon, was danach in den Feldern steht.
    /// </summary>
    private Task SaveProfileEditorAsync(StudentProfile profile) =>
        _profileRepo.UpdateSettingsAsync(
            profile.Id,
            TypingMinAccuracyPercent / 100.0,
            QuizFirstAttemptThresholdPercent / 100.0,
            QuizRetryThresholdPercent / 100.0,
            ReadingMinutes,
            NewsSecondsPerArticle,
            ExerciseSecondsPerQuestion,
            ExercisesPerSubject,
            QuizQuestionCount,
            QuizRetryQuestionCount,
            TypingTextOverrides.Sanitize(CustomTypingSentenceText),
            TypingTextOverrides.Sanitize(CustomTypingFinalText),
            WeeklyGoalDays,
            profile.PinnedReadingTextKey,
            NewsArticleCount,
            NewsFilterStrictness);

    private void ApplyProfileToEditor(StudentProfile? value)
    {
        _ = ReloadActivityForSelectedProfileAsync();

        TypingMinAccuracyPercent = PercentFromFraction(value?.TypingMinAccuracy, 25);
        QuizFirstAttemptThresholdPercent = PercentFromFraction(value?.QuizFirstAttemptThreshold, 50);
        QuizRetryThresholdPercent = PercentFromFraction(value?.QuizRetryThreshold, 25);
        ReadingMinutes = value?.ReadingMinutes ?? StudentProfile.DefaultReadingMinutes;
        NewsSecondsPerArticle = value?.NewsSecondsPerArticle ?? StudentProfile.DefaultNewsSecondsPerArticle;
        NewsArticleCount = value?.NewsArticleCount ?? StudentProfile.DefaultNewsArticleCount;
        NewsFilterStrictness = value?.NewsFilterStrictness ?? NewsFilterStrictness.Normal;
        OnPropertyChanged(nameof(NewsFilterStrictnessHint));
        ExerciseSecondsPerQuestion = value?.ExerciseSecondsPerQuestion ?? StudentProfile.DefaultExerciseSecondsPerQuestion;
        ExercisesPerSubject = value?.ExercisesPerSubject ?? StudentProfile.DefaultExercisesPerSubject;
        QuizQuestionCount = value?.QuizQuestionCount ?? StudentProfile.DefaultQuizQuestionCount;
        QuizRetryQuestionCount = value?.QuizRetryQuestionCount ?? StudentProfile.DefaultQuizRetryQuestionCount;
        WeeklyGoalDays = value?.WeeklyGoalDays ?? 0;
        CustomTypingSentenceText = value?.CustomTypingSentenceText ?? string.Empty;
        CustomTypingFinalText = value?.CustomTypingFinalText ?? string.Empty;
        _ = ReloadCustomReadingTextsAsync();
        _ = ReloadVocabularyAsync();
        _ = ReloadHomeworkAsync();
        _ = ReloadExamsAsync();
    }

    private static int PercentFromFraction(double? fraction, int fallbackPercent) =>
        fraction is null ? fallbackPercent : (int)Math.Round(fraction.Value * 100);

    // --- Schwierigkeitsstufen pro Profil (Tipptrainer-Mindestgenauigkeit, Abschlussquiz-Schwellenwerte) ---

    /// <summary>🔥-Lernserie auf dem Willkommensbildschirm anzeigen (global, Standard aus -
    /// bewusst kein Streak-Druck, siehe StreakCalculator). Direkt in _settings gespiegelt, damit
    /// JEDER Speicherpfad (Haupt-Speichern wie LLM-Sofort-Speichern) den aktuellen Wert persistiert.</summary>
    [ObservableProperty]
    private bool streaksEnabled;

    partial void OnStreaksEnabledChanged(bool value)
    {
        _settings.StreaksEnabled = value;
        MarkDirty();
    }

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

    // --- Wochenziel (reine Anzeige ohne Druckmechanik, siehe WeeklyGoalCalculator) ---

    /// <summary>Wochenziel in Lerntagen; 0 = aus (Standard).</summary>
    [ObservableProperty]
    private int weeklyGoalDays;

    [RelayCommand]
    private void SetWeeklyGoal(string days)
    {
        WeeklyGoalDays = int.TryParse(days, out var parsed) ? parsed : 0;
    }

    // --- Eigene Tipptrainer-Texte (nur die beiden letzten Lektionen, siehe TypingTextOverrides) ---

    /// <summary>Maximale Zeichenzahl eines eigenen Tipp-Textes - in der Oberfläche als Hinweis sichtbar.</summary>
    public static int TypingTextMaxLength => TypingTextOverrides.MaxLength;

    /// <summary>Mindestlänge, ab der ein eigener Text überhaupt greift.</summary>
    public static int TypingTextMinLength => TypingTextOverrides.MinLength;

    /// <summary>Eigener Zieltext für Lektion 6 (Einfache Sätze). Leer = eingebauter Text.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CustomTypingSentenceTextCounter))]
    private string customTypingSentenceText = string.Empty;

    /// <summary>Eigener Zieltext für die Abschluss-Lektion. Leer = eingebauter Steckbrief-Text.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CustomTypingFinalTextCounter))]
    private string customTypingFinalText = string.Empty;

    public string CustomTypingSentenceTextCounter => BuildTypingTextCounter(CustomTypingSentenceText);

    public string CustomTypingFinalTextCounter => BuildTypingTextCounter(CustomTypingFinalText);

    /// <summary>
    /// Live-Anzeige unter dem Eingabefeld: aktuelle Länge, Maximum und - falls der Text zu kurz ist -
    /// der Hinweis, dass dann der eingebaute Text stehen bleibt.
    /// </summary>
    private static string BuildTypingTextCounter(string? text)
    {
        var length = (text ?? string.Empty).Trim().Length;
        var baseText = $"{length} / {TypingTextOverrides.MaxLength} Zeichen";

        if (length == 0)
        {
            return baseText + " – leer lassen für den eingebauten Text.";
        }

        if (length < TypingTextOverrides.MinLength)
        {
            return baseText + $" – zu kurz (mindestens {TypingTextOverrides.MinLength}), der eingebaute Text bleibt aktiv.";
        }

        return baseText;
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

    /// <summary>Anzahl der täglichen Nachrichten (Presets 6/10/15/20).</summary>
    [ObservableProperty]
    private int newsArticleCount = StudentProfile.DefaultNewsArticleCount;

    /// <summary>Wie streng der Jugendschutz-Filter arbeitet (Streng/Normal/Locker).</summary>
    [ObservableProperty]
    private NewsFilterStrictness newsFilterStrictness = NewsFilterStrictness.Normal;

    /// <summary>Erklaert die gewaehlte Stufe im Klartext - "Normal" allein sagt niemandem etwas.</summary>
    public string NewsFilterStrictnessHint => NewsFilterStrictness switch
    {
        NewsFilterStrictness.Streng =>
            "Streng: Krieg, Kriminalität und Unfälle kommen gar nicht vor. An nachrichtenschweren Tagen bleiben dadurch spürbar weniger Artikel übrig.",
        NewsFilterStrictness.Locker =>
            "Locker: nur die harte Sperre greift, sonst zählt allein die Aktualität. Für ältere Jugendliche, die Nachrichten bewusst mitverfolgen sollen.",
        _ =>
            "Normal: Krieg, Kriminalität und Unfälle dürfen vorkommen (sie gehören zum Rahmenlehrplan), werden aber nachrangig behandelt - aus einer Quelle gewinnt der unbedenklichste Artikel."
    } + " Sexualisierte Gewalt, Suizid, Folter, Missbrauch und Massaker sind in JEDER Stufe gesperrt."
      + (SelectedProfile?.Age is { } age && age < NewsSuitability.AlwaysStrictBelowAge
            ? $" Hinweis: {SelectedProfile.Name} ist {age} - unter {NewsSuitability.AlwaysStrictBelowAge} gilt immer \"Streng\", unabhängig von dieser Einstellung."
            : string.Empty);

    [RelayCommand]
    private void SetNewsFilterStrictness(string strictness)
    {
        NewsFilterStrictness = Enum.TryParse<NewsFilterStrictness>(strictness, out var parsed)
            ? parsed
            : NewsFilterStrictness.Normal;
        OnPropertyChanged(nameof(NewsFilterStrictnessHint));
    }

    [RelayCommand]
    private void SetReadingMinutes(string minutes)
    {
        ReadingMinutes = int.TryParse(minutes, out var parsed) ? parsed : StudentProfile.DefaultReadingMinutes;
    }

    [RelayCommand]
    private void SetNewsArticleCount(string count)
    {
        NewsArticleCount = int.TryParse(count, out var parsed) ? parsed : StudentProfile.DefaultNewsArticleCount;
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

    partial void OnPauseUntilChanged(DateTime? value)
    {
        _settings.PauseUntilDate = value is { } d ? DateOnly.FromDateTime(d) : null;
        MarkDirty();
    }

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

    /// <summary>Antworttempo im Zeitraum - zeigt, ob gelesen oder geraten wurde (AnswerPaceAnalyzer).</summary>
    [ObservableProperty]
    private string reportPaceDisplay = string.Empty;

    /// <summary>Warnzeile, nur gefüllt wenn auffällig viele Antworten sehr schnell kamen.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPaceWarning))]
    private string reportPaceWarningDisplay = string.Empty;

    public bool HasPaceWarning => !string.IsNullOrEmpty(ReportPaceWarningDisplay);

    /// <summary>Stand des Wochenziels im Bericht (leer, wenn kein Ziel gesetzt ist).</summary>
    [ObservableProperty]
    private string reportWeeklyGoalDisplay = string.Empty;

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
            ReportPaceDisplay = string.Empty;
            ReportPaceWarningDisplay = string.Empty;
            ReportWeeklyGoalDisplay = string.Empty;
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

        // Lernzeit je Fach: die Trefferquote allein sagt nicht, WARUM ein Fach schlecht laeuft.
        // Schnell und falsch heisst geraten, langsam und falsch heisst nicht verstanden - zwei
        // verschiedene Probleme mit zwei verschiedenen Antworten.
        SubjectTimeRows.Clear();
        var timeStats = SubjectTimeAnalyzer.Analyze(answers.Select(a =>
            new SubjectTimeAnalyzer.Entry(a.Subject, a.WasCorrect, a.AnswerDurationMs)));

        foreach (var stat in timeStats)
        {
            SubjectTimeRows.Add(new SubjectTimeRowViewModel(stat, loc[$"Stage_{stat.Subject}"]));
        }

        HasSubjectTimeData = SubjectTimeRows.Count > 0;
        OnPropertyChanged(nameof(SubjectTimeSummary));

        // Antworttempo: im reinen Richtig/Falsch-Bericht ist Raten unsichtbar, weil es bei drei
        // Optionen in einem Drittel der Fälle "richtig" ergibt. Die Dauer macht es sichtbar.
        var pace = AnswerPaceAnalyzer.Analyze(answers.Select(a => (a.AnswerDurationMs, a.WasCorrect)));
        if (pace.HasData)
        {
            ReportPaceDisplay = string.Format(
                loc["Parent_Report_Pace"], pace.Quick, pace.Measured, (pace.MedianMs / 1000.0).ToString("0.#"));
            ReportPaceWarningDisplay = pace.IsSuspicious
                ? string.Format(loc["Parent_Report_PaceSuspicious"], pace.QuickAndWrong)
                : string.Empty;
        }
        else
        {
            ReportPaceDisplay = loc["Parent_Report_PaceNoData"];
            ReportPaceWarningDisplay = string.Empty;
        }

        // Wochenziel: bewusst immer die AKTUELLE Woche, unabhängig vom gewählten Berichtszeitraum -
        // "3 von 4 Tagen" über 30 Tage gerechnet wäre sinnlos.
        var allLearningDays = _reportActivity
            .Select(a => DateOnly.FromDateTime(a.Timestamp.LocalDateTime))
            .ToHashSet();
        var goal = WeeklyGoalCalculator.Evaluate(
            allLearningDays, DateOnly.FromDateTime(DateTime.Today), SelectedProfile?.WeeklyGoalDays ?? 0);
        ReportWeeklyGoalDisplay = goal.IsActive
            ? string.Format(loc["Parent_Report_WeeklyGoal"], goal.LearnedDays, goal.Goal)
            : loc["Parent_Report_WeeklyGoalOff"];

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

            // Die Bereinigung (Umbrüche raus, auf Maximallänge kürzen, zu kurze Texte verwerfen)
            // macht TypingTextOverrides.Sanitize - der Rückgabewert wird auch ins geladene Profil
            // gespiegelt, damit die Oberfläche denselben Wert zeigt, der in der DB steht.
            var sentenceText = TypingTextOverrides.Sanitize(CustomTypingSentenceText);
            var finalText = TypingTextOverrides.Sanitize(CustomTypingFinalText);

            // Bewusst über dieselbe Methode wie der Profilwechsel: als beide Aufrufstellen die
            // 14 Parameter einzeln aufzählten, hatte eine davon den angehefteten Lesetext
            // vergessen - der Parameter ist optional und fiel still auf null zurück.
            await SaveProfileEditorAsync(SelectedProfile);

            SelectedProfile.WeeklyGoalDays = WeeklyGoalDays;
            SelectedProfile.CustomTypingSentenceText = sentenceText;
            SelectedProfile.CustomTypingFinalText = finalText;
            CustomTypingSentenceText = sentenceText ?? string.Empty;
            CustomTypingFinalText = finalText ?? string.Empty;
            SelectedProfile.TypingMinAccuracy = typingMinAccuracy;
            SelectedProfile.QuizFirstAttemptThreshold = quizFirstAttemptThreshold;
            SelectedProfile.QuizRetryThreshold = quizRetryThreshold;
            SelectedProfile.ReadingMinutes = ReadingMinutes;
            SelectedProfile.NewsSecondsPerArticle = NewsSecondsPerArticle;
            SelectedProfile.NewsArticleCount = NewsArticleCount;
            SelectedProfile.NewsFilterStrictness = NewsFilterStrictness;
            SelectedProfile.ExerciseSecondsPerQuestion = ExerciseSecondsPerQuestion;
            SelectedProfile.ExercisesPerSubject = ExercisesPerSubject;
            SelectedProfile.QuizQuestionCount = QuizQuestionCount;
            SelectedProfile.QuizRetryQuestionCount = QuizRetryQuestionCount;
        }

        HasUnsavedChanges = false;
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
        ImportStatusMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(ImportFilePath) || !File.Exists(ImportFilePath))
        {
            ImportErrorMessage = "Bitte zuerst eine gültige Datei auswählen.";
            return;
        }

        _importCancellation?.Dispose();
        _importCancellation = new CancellationTokenSource();
        var token = _importCancellation.Token;

        var started = DateTimeOffset.Now;
        void Tick(object? _, EventArgs __) =>
            ImportStatusMessage =
                $"Die KI liest das Dokument… {(int)(DateTimeOffset.Now - started).TotalSeconds} s. " +
                "Auf einem normalen PC dauert das je nach Modellgröße 1-5 Minuten.";

        _importTimer.Tick += Tick;
        Tick(null, EventArgs.Empty);
        _importTimer.Start();

        IsImporting = true;
        try
        {
            await using var fileStream = File.OpenRead(ImportFilePath);
            var drafts = await _teacherImportService.ImportAsync(
                fileStream, ImportFilePath, ImportSubject, ImportGrade, token);

            ImportedDrafts.Clear();
            foreach (var draft in drafts)
            {
                ImportedDrafts.Add(new EditableDraftViewModel(draft));
            }

            OnPropertyChanged(nameof(HasNoImportedDrafts));

            ImportStatusMessage = drafts.Count == 0
                ? string.Empty
                : $"{drafts.Count} Vorschläge erstellt - bitte unten einzeln prüfen und übernehmen.";

            if (drafts.Count == 0)
            {
                ImportErrorMessage =
                    "Die KI hat keine Fragenvorschläge aus diesem Dokument geliefert. Häufigster Grund: " +
                    "das Dokument enthält kaum durchgehenden Text (z.B. ein eingescanntes Bild ohne " +
                    "Texterkennung). Versuche es mit einer Datei, aus der sich Text markieren und " +
                    "kopieren lässt.";
            }
        }
        catch (OperationCanceledException)
        {
            ImportStatusMessage = string.Empty;
            ImportErrorMessage = "Einlesen abgebrochen.";
        }
        catch (Exception ex)
        {
            ImportStatusMessage = string.Empty;
            ImportErrorMessage = ex.Message;
        }
        finally
        {
            _importTimer.Stop();
            _importTimer.Tick -= Tick;
            IsImporting = false;
        }
    }

    /// <summary>Bricht ein laufendes Einlesen ab.</summary>
    [RelayCommand]
    private void CancelImport() => _importCancellation?.Cancel();

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

    // --- Eigene Lesetexte pro Profil (siehe CustomReadingTextRepository) ---

    /// <summary>Maximale Länge eines eigenen Lesetextes je Sprache - als Hinweis in der Oberfläche.</summary>
    public static int ReadingTextMaxLength => MaxReadingTextLength;

    private const int MaxReadingTextLength = 4000;

    public ObservableCollection<CustomReadingTextEntity> CustomReadingTexts { get; } = new();

    public bool HasNoCustomReadingTexts => CustomReadingTexts.Count == 0;

    [ObservableProperty]
    private string newReadingTitle = string.Empty;

    [ObservableProperty]
    private string newReadingAuthor = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NewReadingCounter))]
    private string newReadingTextDe = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NewReadingCounter))]
    private string newReadingTextTr = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NewReadingCounter))]
    private string newReadingTextEn = string.Empty;

    [ObservableProperty]
    private string customReadingErrorMessage = string.Empty;

    /// <summary>Live-Anzeige der längsten der drei Sprachfassungen gegen das Maximum.</summary>
    public string NewReadingCounter
    {
        get
        {
            var longest = Math.Max(
                NewReadingTextDe.Trim().Length,
                Math.Max(NewReadingTextTr.Trim().Length, NewReadingTextEn.Trim().Length));
            return $"{longest} / {MaxReadingTextLength} Zeichen (längste Sprachfassung)";
        }
    }

    private async Task ReloadCustomReadingTextsAsync()
    {
        CustomReadingTexts.Clear();
        if (SelectedProfile is not null)
        {
            foreach (var text in await _customReadingRepo.GetEntitiesForProfileAsync(SelectedProfile.Id))
            {
                CustomReadingTexts.Add(text);
            }
        }

        OnPropertyChanged(nameof(HasNoCustomReadingTexts));
        await ReloadReadingLibraryAsync();
    }

    // --- Nachrichtenquellen an/aus (global, siehe AppSettings.DisabledNewsFeeds) ---

    /// <summary>Alle kuratierten Quellen mit Ein/Aus-Schalter.</summary>
    public ObservableCollection<NewsFeedToggle> NewsFeedToggles { get; } = new();

    [ObservableProperty]
    private string newsFeedStatus = string.Empty;

    /// <summary>Sprache als Klartext - "Tuerkisch" als Enum-Name will im Eltern-Bereich niemand lesen.</summary>
    private static string LanguageLabel(NewsFeedLanguage language) => language switch
    {
        NewsFeedLanguage.Deutsch => "Deutsch",
        NewsFeedLanguage.Tuerkisch => "Türkisch",
        NewsFeedLanguage.Englisch => "Englisch",
        _ => language.ToString()
    };

    /// <summary>
    /// Zustand einer Quelle als Klartext. Wichtig ist der ZEITPUNKT: seit der Tagesrotation wird
    /// nicht mehr jede Quelle täglich abgerufen, ein Eintrag kann also Tage alt sein. Ihn ohne
    /// Datum als aktuellen Stand auszugeben wäre irreführend.
    /// </summary>
    private static string HealthLabelFor(FeedHealthEntry? entry)
    {
        if (entry is null)
        {
            return "noch nicht abgerufen";
        }

        var days = (DateOnly.FromDateTime(DateTime.Today).DayNumber
                    - DateOnly.FromDateTime(entry.CheckedAt.LocalDateTime).DayNumber);
        var wann = days switch
        {
            <= 0 => "heute",
            1 => "gestern",
            _ => $"vor {days} Tagen"
        };

        return entry.IsHealthy
            ? $"✓ {wann} geladen"
            : $"⚠ {wann} fehlgeschlagen: {entry.Error}";
    }

    private void BuildNewsFeedToggles()
    {
        NewsFeedToggles.Clear();

        var health = _feedHealthLog.LoadAll();

        // Nach Sprache gruppiert statt nach Region: bei 44 Quellen ist "erst alle deutschen,
        // dann alle türkischen, dann alle englischen" die Reihenfolge, in der Eltern suchen.
        foreach (var feed in CuratedNewsFeeds.All
                     .OrderBy(f => f.Language)
                     .ThenBy(f => f.RegionFocus)
                     .ThenBy(f => f.Name))
        {
            health.TryGetValue(feed.Name, out var entry);

            NewsFeedToggles.Add(new NewsFeedToggle(
                feed.Name,
                $"{LanguageLabel(feed.Language)} · {feed.RegionFocus}",
                !_settings.DisabledNewsFeeds.Contains(feed.Name),
                OnNewsFeedToggled,
                HealthLabelFor(entry),
                isUnhealthy: entry is { IsHealthy: false }));
        }

        UpdateNewsFeedStatus();
    }

    private async void OnNewsFeedToggled(NewsFeedToggle toggle)
    {
        if (toggle.IsEnabled)
        {
            _settings.DisabledNewsFeeds.Remove(toggle.Name);
        }
        else
        {
            _settings.DisabledNewsFeeds.Add(toggle.Name);
        }

        await _settingsRepo.SaveAsync(_settings);
        UpdateNewsFeedStatus();
    }

    /// <summary>
    /// Alle Quellen auf einmal an- oder abwaehlen. 22 Haken einzeln zu klicken ist muehselig -
    /// der uebliche Ablauf ist "alle aus, dann drei wieder an".
    /// </summary>
    [RelayCommand]
    private async Task SetAllNewsFeedsAsync(string enable)
    {
        var shouldEnable = enable == "1";

        _settings.DisabledNewsFeeds.Clear();
        foreach (var toggle in NewsFeedToggles)
        {
            // Setzt IsEnabled ohne den Einzel-Callback je Zeile auszuloesen (der wuerde 22-mal
            // speichern) - deshalb wird die Menge hier direkt gepflegt und einmal gespeichert.
            toggle.SetEnabledSilently(shouldEnable);
            if (!shouldEnable)
            {
                _settings.DisabledNewsFeeds.Add(toggle.Name);
            }
        }

        await _settingsRepo.SaveAsync(_settings);
        UpdateNewsFeedStatus();
    }

    private void UpdateNewsFeedStatus()
    {
        var active = NewsFeedToggles.Count(t => t.IsEnabled);
        if (active == 0)
        {
            NewsFeedStatus = "Alle Quellen abgeschaltet - dann greifen wieder alle, ein leerer News-Bereich wäre schlimmer.";
            return;
        }

        // Kaputte Quellen zuerst nennen - das ist die Information, wegen der man hier nachschaut.
        var kaputt = NewsFeedToggles.Count(t => t.IsUnhealthy);
        var warnung = kaputt > 0
            ? $"⚠ {kaputt} Quelle(n) beim letzten Abruf fehlgeschlagen (siehe Hinweis an der Zeile). "
            : string.Empty;

        // Wichtig fuer die Erwartung der Eltern: seit dem grossen Katalog kommt NICHT mehr aus
        // jeder Quelle taeglich eine Nachricht - es wird taeglich eine andere Auswahl gezogen.
        NewsFeedStatus = warnung + (active <= NewsArticleCount
            ? $"{active} von {NewsFeedToggles.Count} Quellen aktiv - bei {NewsArticleCount} Nachrichten am Tag kommt jede aktive Quelle täglich dran."
            : $"{active} von {NewsFeedToggles.Count} Quellen aktiv. Pro Tag werden daraus {NewsArticleCount} Nachrichten " +
              $"ausgewählt (Deutsch, Türkisch und Englisch gemischt) - über etwa {Math.Max(1, (int)Math.Ceiling(active / (double)NewsArticleCount))} Tage " +
              "kommt jede Quelle einmal vorbei.");
    }

    // --- Lesetext-Verwaltung: EINE Liste aus eigenen und eingebauten Texten ---

    /// <summary>Alle Lesetexte des gewählten Profils - eigene zuerst, dann die eingebauten.</summary>
    public ObservableCollection<ReadingTextRowViewModel> ReadingLibrary { get; } = new();

    [ObservableProperty]
    private string readingLibraryFilter = string.Empty;

    [ObservableProperty]
    private bool readingLibraryOnlyOwn;

    [ObservableProperty]
    private string readingLibraryStatus = string.Empty;

    partial void OnReadingLibraryFilterChanged(string value) => _ = ReloadReadingLibraryAsync();

    partial void OnReadingLibraryOnlyOwnChanged(bool value) => _ = ReloadReadingLibraryAsync();

    private async Task ReloadReadingLibraryAsync()
    {
        ReadingLibrary.Clear();
        if (SelectedProfile is null)
        {
            return;
        }

        var own = await _customReadingRepo.GetForProfileAsync(SelectedProfile.Id);
        var all = ReadingLibraryOnlyOwn
            ? own
            : own.Concat(ReadingContentProvider.GetAllBuiltIn()).ToList();

        var filter = ReadingLibraryFilter.Trim();
        if (filter.Length > 0)
        {
            all = all.Where(p =>
                    p.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    p.Author.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        foreach (var piece in all)
        {
            ReadingLibrary.Add(new ReadingTextRowViewModel(
                piece,
                isHidden: _settings.HiddenReadingTextKeys.Contains(piece.Key),
                isPinned: SelectedProfile.PinnedReadingTextKey == piece.Key,
                onVisibilityChanged: OnReadingRowVisibilityChanged));
        }

        UpdateReadingLibraryStatus();
    }

    /// <summary>
    /// Ausblenden wirkt global (siehe AppSettings.HiddenReadingTextKeys) und wird sofort
    /// gespeichert - eine Einstellung, die erst beim Verlassen des Eltern-Bereichs greift,
    /// lädt zum Vergessen ein.
    /// </summary>
    private async void OnReadingRowVisibilityChanged(ReadingTextRowViewModel row)
    {
        if (row.IsVisible)
        {
            _settings.HiddenReadingTextKeys.Remove(row.Key);
        }
        else
        {
            _settings.HiddenReadingTextKeys.Add(row.Key);

            // Ein ausgeblendeter Text darf nicht gleichzeitig angeheftet bleiben.
            if (SelectedProfile?.PinnedReadingTextKey == row.Key)
            {
                await SetPinnedReadingTextAsync(null);
                row.IsPinned = false;
            }
        }

        await _settingsRepo.SaveAsync(_settings);
        UpdateReadingLibraryStatus();
    }

    /// <summary>Heftet einen Text als Tagestext an bzw. löst ihn wieder.</summary>
    [RelayCommand]
    private async Task TogglePinnedReadingTextAsync(ReadingTextRowViewModel row)
    {
        var newKey = row.IsPinned ? null : row.Key;

        foreach (var other in ReadingLibrary)
        {
            other.IsPinned = other.Key == newKey;
        }

        await SetPinnedReadingTextAsync(newKey);
        UpdateReadingLibraryStatus();
    }

    private async Task SetPinnedReadingTextAsync(string? key)
    {
        if (SelectedProfile is null)
        {
            return;
        }

        SelectedProfile.PinnedReadingTextKey = key;
        await _profileRepo.UpdateSettingsAsync(
            SelectedProfile.Id,
            SelectedProfile.TypingMinAccuracy,
            SelectedProfile.QuizFirstAttemptThreshold,
            SelectedProfile.QuizRetryThreshold,
            SelectedProfile.ReadingMinutes,
            SelectedProfile.NewsSecondsPerArticle,
            SelectedProfile.ExerciseSecondsPerQuestion,
            SelectedProfile.ExercisesPerSubject,
            SelectedProfile.QuizQuestionCount,
            SelectedProfile.QuizRetryQuestionCount,
            SelectedProfile.CustomTypingSentenceText,
            SelectedProfile.CustomTypingFinalText,
            SelectedProfile.WeeklyGoalDays,
            key);
    }

    /// <summary>
    /// Alle sichtbaren Zeilen der Lesetext-Liste auf einmal ein- oder ausblenden. Wirkt bewusst
    /// nur auf die aktuell GEFILTERTE Liste - so laesst sich mit der Suche gezielt eine Gruppe
    /// abwaehlen, statt immer alle 63 Texte auf einmal zu treffen.
    /// </summary>
    [RelayCommand]
    private async Task SetAllReadingTextsAsync(string visible)
    {
        var shouldShow = visible == "1";

        foreach (var row in ReadingLibrary)
        {
            row.SetVisibleSilently(shouldShow);
            if (shouldShow)
            {
                _settings.HiddenReadingTextKeys.Remove(row.Key);
            }
            else
            {
                _settings.HiddenReadingTextKeys.Add(row.Key);
                if (SelectedProfile?.PinnedReadingTextKey == row.Key)
                {
                    await SetPinnedReadingTextAsync(null);
                    row.IsPinned = false;
                }
            }
        }

        await _settingsRepo.SaveAsync(_settings);
        UpdateReadingLibraryStatus();
    }

    private void UpdateReadingLibraryStatus()
    {
        var visible = ReadingLibrary.Count(r => r.IsVisible);
        var pinned = ReadingLibrary.FirstOrDefault(r => r.IsPinned);

        ReadingLibraryStatus = pinned is not null
            ? $"{visible} von {ReadingLibrary.Count} Texten aktiv. Angeheftet: \"{pinned.Title}\" - dieser Text steht jeden Tag an erster Stelle, bis du ihn wieder löst."
            : $"{visible} von {ReadingLibrary.Count} Texten aktiv. Kein Text angeheftet - es gilt die normale Tagesrotation (eigene Texte zuerst).";
    }

    /// <summary>
    /// Legt einen eigenen Lesetext für das gewählte Profil an. Mindestens eine Sprachfassung ist
    /// Pflicht - die übrigen bleiben leer und werden in der Leseansicht mit einem Hinweis gefüllt
    /// (siehe ReadingViewModel.FillMissingLanguages).
    /// </summary>
    [RelayCommand]
    private async Task AddCustomReadingTextAsync()
    {
        CustomReadingErrorMessage = string.Empty;

        if (SelectedProfile is null)
        {
            CustomReadingErrorMessage = "Bitte oben ein Profil auswählen.";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewReadingTitle))
        {
            CustomReadingErrorMessage = "Bitte einen Titel eingeben.";
            return;
        }

        var de = NewReadingTextDe.Trim();
        var tr = NewReadingTextTr.Trim();
        var en = NewReadingTextEn.Trim();

        if (de.Length == 0 && tr.Length == 0 && en.Length == 0)
        {
            CustomReadingErrorMessage = "Bitte den Text in mindestens einer Sprache eingeben.";
            return;
        }

        if (de.Length > MaxReadingTextLength || tr.Length > MaxReadingTextLength || en.Length > MaxReadingTextLength)
        {
            CustomReadingErrorMessage = $"Ein Text ist zu lang (max. {MaxReadingTextLength} Zeichen je Sprache).";
            return;
        }

        if (IsEditingReadingText)
        {
            await _customReadingRepo.UpdateAsync(EditingReadingTextId, NewReadingTitle, NewReadingAuthor, de, tr, en);
            EditingReadingTextId = string.Empty;
        }
        else
        {
            await _customReadingRepo.AddAsync(SelectedProfile.Id, NewReadingTitle, NewReadingAuthor, de, tr, en);
        }

        NewReadingTitle = string.Empty;
        NewReadingAuthor = string.Empty;
        NewReadingTextDe = string.Empty;
        NewReadingTextTr = string.Empty;
        NewReadingTextEn = string.Empty;

        await ReloadCustomReadingTextsAsync();
    }

    [RelayCommand]
    private async Task DeleteCustomReadingTextAsync(CustomReadingTextEntity text)
    {
        await _customReadingRepo.DeleteAsync(text.Id);

        // War der gelöschte Text angeheftet, muss die Anheftung mit weg - sonst zeigt der
        // Lesebereich still wieder die normale Rotation, ohne dass klar wäre warum.
        if (SelectedProfile?.PinnedReadingTextKey == $"eigen:{text.Id}")
        {
            await SetPinnedReadingTextAsync(null);
        }

        if (EditingReadingTextId == text.Id)
        {
            CancelEditReadingText();
        }

        await ReloadCustomReadingTextsAsync();
    }

    // --- Bearbeiten eines eigenen Lesetextes ---

    /// <summary>Id des gerade bearbeiteten Textes; leer = das Formular legt einen neuen an.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditingReadingText))]
    private string editingReadingTextId = string.Empty;

    public bool IsEditingReadingText => !string.IsNullOrEmpty(EditingReadingTextId);

    /// <summary>Lädt einen bestehenden Text ins Formular - dasselbe Formular dient dem Anlegen.</summary>
    [RelayCommand]
    private void EditCustomReadingText(CustomReadingTextEntity text)
    {
        EditingReadingTextId = text.Id;
        NewReadingTitle = text.Title;
        NewReadingAuthor = text.Author;
        NewReadingTextDe = text.TextDe;
        NewReadingTextTr = text.TextTr;
        NewReadingTextEn = text.TextEn;
        CustomReadingErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void CancelEditReadingText()
    {
        EditingReadingTextId = string.Empty;
        NewReadingTitle = string.Empty;
        NewReadingAuthor = string.Empty;
        NewReadingTextDe = string.Empty;
        NewReadingTextTr = string.Empty;
        NewReadingTextEn = string.Empty;
        CustomReadingErrorMessage = string.Empty;
    }

    // --- Vokabeltrainer (Englisch/Türkisch, siehe VocabularyRepository) ---

    /// <summary>Nur die beiden Sprachfächer haben Vokabeln.</summary>
    public IReadOnlyList<Subject> VocabularySubjects { get; } = new[] { Subject.Englisch, Subject.Tuerkisch };

    public ObservableCollection<VocabularyEntryEntity> VocabularyEntries { get; } = new();

    public bool HasNoVocabulary => VocabularyEntries.Count == 0;

    [ObservableProperty]
    private Subject vocabularySubject = Subject.Englisch;

    [ObservableProperty]
    private string vocabularyBulkText = string.Empty;

    [ObservableProperty]
    private string vocabularyStatusMessage = string.Empty;

    [ObservableProperty]
    private string vocabularyErrorMessage = string.Empty;

    partial void OnVocabularySubjectChanged(Subject value) => _ = ReloadVocabularyAsync();

    private async Task ReloadVocabularyAsync()
    {
        VocabularyEntries.Clear();
        if (SelectedProfile is not null)
        {
            foreach (var entry in await _vocabularyRepo.GetAllForProfileAsync(SelectedProfile.Id, VocabularySubject))
            {
                VocabularyEntries.Add(entry);
            }
        }

        OnPropertyChanged(nameof(HasNoVocabulary));
    }

    /// <summary>
    /// Liest die eingefügte Vokabelliste ein (eine Zeile je Wortpaar, siehe
    /// <see cref="VocabularyParser"/>). Bereits vorhandene deutsche Wörter werden aktualisiert
    /// statt gedoppelt - eine korrigierte Liste ein zweites Mal einzufügen soll nicht jede Vokabel
    /// verdoppeln.
    /// </summary>
    [RelayCommand]
    private async Task ImportVocabularyAsync()
    {
        VocabularyErrorMessage = string.Empty;
        VocabularyStatusMessage = string.Empty;

        if (SelectedProfile is null)
        {
            VocabularyErrorMessage = "Bitte oben ein Profil auswählen.";
            return;
        }

        var pairs = VocabularyParser.Parse(VocabularyBulkText);
        if (pairs.Count == 0)
        {
            VocabularyErrorMessage =
                "Keine Wortpaare erkannt. Erwartet wird eine Zeile je Vokabel, z.B. \"Haus = house\".";
            return;
        }

        var added = await _vocabularyRepo.AddOrUpdateManyAsync(SelectedProfile.Id, VocabularySubject, pairs);
        var updated = pairs.Count - added;

        VocabularyBulkText = string.Empty;
        VocabularyStatusMessage = updated > 0
            ? $"{added} neue Vokabeln übernommen, {updated} bestehende aktualisiert."
            : $"{added} Vokabeln übernommen.";

        await ReloadVocabularyAsync();
    }

    [RelayCommand]
    private async Task DeleteVocabularyEntryAsync(VocabularyEntryEntity entry)
    {
        await _vocabularyRepo.DeleteAsync(entry.Id);
        await ReloadVocabularyAsync();
    }

    private static IReadOnlyList<string> SplitCommaSeparated(string text) =>
        text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    // --- Hausaufgaben mit Stichtag (pro Profil) ---

    /// <summary>Alle Hausaufgaben des gewählten Profils, dringendste zuerst.</summary>
    public ObservableCollection<HomeworkItemViewModel> Homework { get; } = new();

    [ObservableProperty]
    private string newHomeworkDescription = string.Empty;

    [ObservableProperty]
    private Subject newHomeworkSubject = Subject.Mathematik;

    /// <summary>Stichtag als DateTime? für den DatePicker; Vorgabe ist morgen - der häufigste Fall.</summary>
    [ObservableProperty]
    private DateTime? newHomeworkDueDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private string homeworkStatus = string.Empty;

    public bool HasNoHomework => Homework.Count == 0;

    /// <summary>Fächer für das Auswahlfeld - dieselbe Liste wie bei den eigenen Aufgaben.</summary>
    public IReadOnlyList<Subject> HomeworkSubjects { get; } =
        Enum.GetValues<Subject>().Where(s => s != Subject.News).ToList();

    private async Task ReloadHomeworkAsync()
    {
        Homework.Clear();
        if (SelectedProfile is null)
        {
            OnPropertyChanged(nameof(HasNoHomework));
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        foreach (var task in await _homeworkRepo.GetForProfileAsync(SelectedProfile.Id))
        {
            Homework.Add(new HomeworkItemViewModel(task, today, OnHomeworkCompletedChanged));
        }

        OnPropertyChanged(nameof(HasNoHomework));
        UpdateHomeworkStatus();
    }

    /// <summary>
    /// Abhaken im Eltern-Bereich wird sofort gespeichert - wie in der Kind-Ansicht. Bewusst KEIN
    /// MarkDirty: das ist keine Einstellung, die auf "Speichern" wartet.
    /// </summary>
    private async void OnHomeworkCompletedChanged(HomeworkItemViewModel item)
    {
        await _homeworkRepo.SetCompletedAsync(item.Id, item.IsCompleted);
        UpdateHomeworkStatus();
    }

    [RelayCommand]
    private async Task AddHomeworkAsync()
    {
        if (SelectedProfile is null)
        {
            HomeworkStatus = "Erst ein Profil auswählen.";
            return;
        }

        var description = NewHomeworkDescription.Trim();
        if (description.Length == 0)
        {
            HomeworkStatus = "Bitte eintragen, was zu tun ist.";
            return;
        }

        var dueDate = DateOnly.FromDateTime(NewHomeworkDueDate ?? DateTime.Today);

        await _homeworkRepo.AddAsync(SelectedProfile.Id, NewHomeworkSubject, description, dueDate);

        NewHomeworkDescription = string.Empty;
        NewHomeworkDueDate = DateTime.Today.AddDays(1);
        await ReloadHomeworkAsync();
    }

    [RelayCommand]
    private async Task DeleteHomeworkAsync(HomeworkItemViewModel item)
    {
        await _homeworkRepo.DeleteAsync(item.Id);
        await ReloadHomeworkAsync();
    }

    /// <summary>Räumt erledigte Hausaufgaben ab, die länger als eine Woche abgehakt sind.</summary>
    [RelayCommand]
    private async Task ClearDoneHomeworkAsync()
    {
        var removed = await _homeworkRepo.DeleteCompletedOlderThanAsync(
            DateOnly.FromDateTime(DateTime.Today).AddDays(-7));

        await ReloadHomeworkAsync();
        HomeworkStatus = removed == 0
            ? "Nichts zum Aufräumen - alle erledigten Aufgaben sind noch frisch."
            : $"{removed} erledigte Hausaufgabe(n) entfernt.";
    }

    // --- Klausurkalender (pro Profil) ---

    /// <summary>Alle Klausurtermine des gewählten Profils, nächster zuerst.</summary>
    public ObservableCollection<ExamItemViewModel> Exams { get; } = new();

    [ObservableProperty]
    private string examStatus = string.Empty;

    public bool HasNoExams => Exams.Count == 0;

    private async Task ReloadExamsAsync()
    {
        Exams.Clear();
        if (SelectedProfile is null)
        {
            OnPropertyChanged(nameof(HasNoExams));
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        foreach (var exam in await _examRepo.GetForProfileAsync(SelectedProfile.Id))
        {
            Exams.Add(new ExamItemViewModel(exam, today));
        }

        OnPropertyChanged(nameof(HasNoExams));
        UpdateExamStatus();
    }

    [RelayCommand]
    private async Task AddExamAsync()
    {
        if (SelectedProfile is null)
        {
            ExamStatus = "Erst ein Profil auswählen.";
            return;
        }

        var dialog = new Views.ExamEntryDialog
        {
            Owner = System.Windows.Application.Current.Windows
                .OfType<Views.ParentSettingsWindow>()
                .FirstOrDefault()
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        await _examRepo.AddAsync(
            SelectedProfile.Id,
            dialog.SelectedSubject,
            dialog.EnteredTitle,
            dialog.EnteredTopics,
            dialog.SelectedDate,
            ExamAuthor.Eltern);

        await ReloadExamsAsync();
    }

    /// <summary>Aus dem Eltern-Bereich darf jeder Termin gelöscht werden, auch die des Kindes.</summary>
    [RelayCommand]
    private async Task DeleteExamAsync(ExamItemViewModel item)
    {
        await _examRepo.DeleteAsync(item.Id);
        await ReloadExamsAsync();
    }

    /// <summary>Räumt Termine ab, die länger als 30 Tage vorbei sind.</summary>
    [RelayCommand]
    private async Task ClearPastExamsAsync()
    {
        var removed = await _examRepo.DeletePastOlderThanAsync(
            DateOnly.FromDateTime(DateTime.Today).AddDays(-30));

        await ReloadExamsAsync();
        ExamStatus = removed == 0
            ? "Nichts zum Aufräumen - keine Termine älter als 30 Tage."
            : $"{removed} vergangene(r) Termin(e) entfernt.";
    }

    /// <summary>
    /// Exportiert die Termine des Kindes als .ics - damit die Klausur auch im Familienkalender
    /// auftaucht. Offener Standard statt Cloud-Anbindung: keine Anmeldung, kein Netz.
    /// </summary>
    [RelayCommand]
    private async Task ExportExamsAsync()
    {
        if (SelectedProfile is null || Exams.Count == 0)
        {
            ExamStatus = "Keine Termine zum Exportieren.";
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "Kalenderdatei (*.ics)|*.ics",
            FileName = $"klausuren-{SelectedProfile.Name.Split(' ')[0].ToLowerInvariant()}.ics",
            Title = "Klausurtermine exportieren"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            var exams = await _examRepo.GetForProfileAsync(SelectedProfile.Id);
            await File.WriteAllTextAsync(
                dialog.FileName,
                IcsCalendar.Export(exams, $"LernTor - {SelectedProfile.Name}"));

            ExamStatus = $"{exams.Count} Termin(e) nach {Path.GetFileName(dialog.FileName)} exportiert.";
        }
        catch (Exception ex)
        {
            ExamStatus = $"Export fehlgeschlagen: {ex.Message}";
        }
    }

    /// <summary>
    /// Liest Termine aus einer .ics-Datei (Schulportal, Outlook, Google-Export) und legt sie als
    /// Eltern-Einträge an. Fremde Kalender enthalten meist mehr als nur Klausuren, deshalb wird
    /// vor dem Übernehmen gezeigt, was gefunden wurde - und Termine in der Vergangenheit werden
    /// gar nicht erst angeboten.
    /// </summary>
    [RelayCommand]
    private async Task ImportExamsAsync()
    {
        if (SelectedProfile is null)
        {
            ExamStatus = "Erst ein Profil auswählen.";
            return;
        }

        var dialog = new OpenFileDialog
        {
            Filter = "Kalenderdatei (*.ics)|*.ics|Alle Dateien (*.*)|*.*",
            Title = "Klausurtermine aus Kalenderdatei einlesen"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            var content = await File.ReadAllTextAsync(dialog.FileName);
            var today = DateOnly.FromDateTime(DateTime.Today);

            var found = IcsCalendar.Import(content)
                .Where(e => e.Date >= today)
                .ToList();

            if (found.Count == 0)
            {
                ExamStatus = "In der Datei standen keine künftigen Termine.";
                return;
            }

            var preview = string.Join("\n", found.Take(10).Select(e =>
                $"• {e.Date:dd.MM.yyyy} - {e.Title}" +
                (e.GuessedSubject is { } subject ? $" ({subject})" : " (Fach bitte nachtragen)")));

            var confirmed = System.Windows.MessageBox.Show(
                $"{found.Count} künftige(r) Termin(e) gefunden:\n\n{preview}" +
                (found.Count > 10 ? $"\n… und {found.Count - 10} weitere" : string.Empty) +
                $"\n\nAlle für {SelectedProfile.Name} übernehmen?",
                "Termine übernehmen?",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question,
                System.Windows.MessageBoxResult.Yes);

            if (confirmed != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var entry in found)
            {
                await _examRepo.AddAsync(
                    SelectedProfile.Id,
                    // Nicht geraten heisst Mathematik als Platzhalter - die Eltern sehen den
                    // Eintrag in der Liste und koennen das Fach korrigieren.
                    entry.GuessedSubject ?? Subject.Mathematik,
                    entry.Title,
                    entry.Description,
                    entry.Date,
                    ExamAuthor.Eltern);
            }

            await ReloadExamsAsync();
            var ohneFach = found.Count(e => e.GuessedSubject is null);
            ExamStatus = ohneFach == 0
                ? $"{found.Count} Termin(e) übernommen."
                : $"{found.Count} Termin(e) übernommen - bei {ohneFach} konnte das Fach nicht erraten werden, bitte nachtragen.";
        }
        catch (Exception ex)
        {
            ExamStatus = $"Einlesen fehlgeschlagen: {ex.Message}";
        }
    }

    // --- Lernzeit je Fach (Eltern-Bericht) ---

    /// <summary>Zeitbilanz je Fach, zeitaufwendigstes zuerst.</summary>
    public ObservableCollection<SubjectTimeRowViewModel> SubjectTimeRows { get; } = new();

    [ObservableProperty]
    private bool hasSubjectTimeData;

    /// <summary>Fasst zusammen, worauf die Eltern schauen sollten - oder dass alles unauffaellig ist.</summary>
    public string SubjectTimeSummary
    {
        get
        {
            if (SubjectTimeRows.Count == 0)
            {
                return string.Empty;
            }

            var auffaellig = SubjectTimeRows.Where(r => r.NeedsAttention).ToList();
            if (auffaellig.Count == 0)
            {
                return "Kein Fach fällt aus dem Rahmen.";
            }

            return "Genauer hinschauen bei: " + string.Join(", ", auffaellig.Select(r => r.SubjectLabel));
        }
    }

    private void UpdateExamStatus()
    {
        if (Exams.Count == 0)
        {
            ExamStatus = "Noch keine Klausuren eingetragen. Die Kinder dürfen sie auch selbst eintragen - "
                       + "solche Einträge stehen hier mit \"selbst eingetragen\" markiert.";
            return;
        }

        var boosting = Exams.Count(e => e.IsBoostingLearning);
        ExamStatus = boosting == 0
            ? $"{Exams.Count} Termin(e) eingetragen. Ab einer Woche vor einer Klausur bekommt das Fach automatisch mehr Aufgaben."
            : $"{Exams.Count} Termin(e) eingetragen - in {boosting} Fach/Fächern wird gerade verstärkt geübt.";
    }

    private void UpdateHomeworkStatus()
    {
        if (Homework.Count == 0)
        {
            HomeworkStatus = "Noch keine Hausaufgaben eingetragen. Sie erscheinen beim Kind auf dem Startbildschirm.";
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var offen = Homework.Count(h => !h.IsCompleted);
        var ueberfaellig = Homework.Count(h => h.Task.IsOverdue(today));

        HomeworkStatus = ueberfaellig > 0
            ? $"{offen} offen, davon {ueberfaellig} überfällig."
            : $"{offen} offen, nichts überfällig.";
    }
}
