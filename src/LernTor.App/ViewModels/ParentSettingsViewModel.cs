using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
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
        (Subject.Gewi, "Stage_Gewi"),
        (Subject.Politik, "Stage_Politik"),
        (Subject.Geo, "Stage_Geo"),
        (Subject.Ethik, "Stage_Ethik"),
        (Subject.Itg, "Stage_Itg"),
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

    public event Action? RequestClose;

    public ParentSettingsViewModel(
        SettingsRepository settingsRepo,
        ActivityLogRepository activityLogRepo,
        StudentProfileRepository profileRepo,
        DatabaseMaintenanceRepository maintenanceRepo,
        CustomQuestionRepository customQuestionRepo,
        KioskLockService kioskLock,
        LocalLlmOptions localLlmOptions,
        TeacherDocumentImportService teacherImportService)
    {
        _settingsRepo = settingsRepo;
        _activityLogRepo = activityLogRepo;
        _profileRepo = profileRepo;
        _maintenanceRepo = maintenanceRepo;
        _customQuestionRepo = customQuestionRepo;
        _kioskLock = kioskLock;
        _localLlmOptions = localLlmOptions;
        _teacherImportService = teacherImportService;
    }

    public async Task InitializeAsync()
    {
        _settings = await _settingsRepo.LoadAsync();
        IsFirstTimeSetup = string.IsNullOrEmpty(_settings.AdminPasswordHash);

        LocalLlmModelPath = _settings.LocalLlmModelPath ?? string.Empty;
        SelectedLlmModel = LocalLlmModelCatalog.Resolve(_settings.LocalLlmModelKey);
        ApplyLocalLlmOptions();
    }

    private void ApplyLocalLlmOptions()
    {
        _localLlmOptions.ModelPath = string.IsNullOrWhiteSpace(LocalLlmModelPath) ? null : LocalLlmModelPath;
        _localLlmOptions.ModelKey = SelectedLlmModel.Key;
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
    }

    private async Task ReloadActivityForSelectedProfileAsync()
    {
        RecentActivity.Clear();
        QuizHistory.Clear();
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

        // 30 Tage einmal laden - die 7/30-Tage-Umschaltung filtert danach nur noch in-memory.
        _reportActivity = await _activityLogRepo.GetActivitySinceAsync(SelectedProfile.Id, TimeSpan.FromDays(30));
        RebuildReport();
    }

    // --- Wochen-/Monatsbericht (Stärken/Schwächen je Fach, Lerntage, Quiz-Verlauf) ---

    private IReadOnlyList<ActivityLogEntity> _reportActivity = Array.Empty<ActivityLogEntity>();

    public ObservableCollection<SubjectReportRowViewModel> ReportRows { get; } = new();

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

        var loc = LocalizationService.Instance;
        var cutoff = DateTimeOffset.Now - TimeSpan.FromDays(ReportDays);
        var answers = _reportActivity.Where(a => a.Timestamp >= cutoff).ToList();

        HasReportData = answers.Count > 0;
        if (!HasReportData)
        {
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
        RequestClose?.Invoke();
    }

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
