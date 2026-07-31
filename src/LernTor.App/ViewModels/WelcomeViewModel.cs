using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

public sealed partial class WelcomeViewModel : ObservableObject
{
    private readonly Action _onContinue;
    private readonly Action<AppLanguage> _onSwitchLanguage;
    private readonly Action? _onAddExam;
    private readonly Action<ExamItemViewModel>? _onDeleteExam;

    /// <summary>Oeffnet die Klausur-Eingabe. Bewusst auch fuer Kinder erreichbar: wer den Termin
    /// selbst eintraegt, nimmt ihn eher ernst als einen, der ihm hingestellt wird.</summary>
    [RelayCommand]
    private void AddExam() => _onAddExam?.Invoke();

    /// <summary>Loescht einen selbst eingetragenen Termin. Eltern-Eintraege sind gesperrt -
    /// die Pruefung sitzt zusaetzlich im Repository, nicht nur an der Oberflaeche.</summary>
    [RelayCommand]
    private void DeleteExam(ExamItemViewModel item) => _onDeleteExam?.Invoke(item);

    public string ProfileName { get; }

    /// <summary>Aufeinanderfolgende Lerntage (0 = Anzeige aus, siehe StreakCalculator).
    /// MainViewModel übergibt 0, wenn Eltern die Streak-Anzeige nicht eingeschaltet haben.</summary>
    public int CurrentStreak { get; }

    /// <summary>Erst ab 2 Tagen anzeigen - "🔥 1 Tag in Folge" wäre keine Serie.</summary>
    public bool ShowStreak => CurrentStreak >= 2;

    /// <summary>
    /// Heute fällige Aufgaben aus der Fehler-Kartei (falsch beantwortete Fragen früherer Tage).
    /// Die Kartei arbeitete bisher unsichtbar im Hintergrund - hier sehen die Kinder, dass ihre
    /// Fehler nicht einfach verschwinden, sondern wiederkommen, bis sie sitzen. Das erzieht
    /// mehr als jede zusätzliche Sperre.
    /// </summary>
    public int DueReviewCount { get; }

    public bool ShowDueReviews => DueReviewCount > 0;

    /// <summary>
    /// Stand des Wochenziels (siehe WeeklyGoalCalculator). Reine Anzeige - ein verfehltes Ziel
    /// hat keine Folgen. Ist kein Ziel gesetzt, bleibt die Zeile aus.
    /// </summary>
    public WeeklyGoalCalculator.WeeklyGoalStatus WeeklyGoal { get; }

    public bool ShowWeeklyGoal => WeeklyGoal.IsActive;

    public bool ShowWeeklyGoalReached => WeeklyGoal.IsReached;

    public bool ShowWeeklyGoalOpen => WeeklyGoal.IsActive && !WeeklyGoal.IsReached;

    public int WeeklyGoalLearnedDays => WeeklyGoal.LearnedDays;

    public int WeeklyGoalTarget => WeeklyGoal.Goal;

    /// <summary>
    /// Von den Eltern eingetragene Hausaufgaben, dringendste zuerst. Bewusst hier auf dem
    /// Willkommensbildschirm und nicht in einer eigenen Etappe: die Aufgabe wird ausserhalb der
    /// App erledigt, die App erinnert nur daran. Leer = die Karte bleibt ganz weg.
    /// </summary>
    public ObservableCollection<HomeworkItemViewModel> Homework { get; } = new();

    public bool ShowHomework => Homework.Count > 0;

    /// <summary>
    /// Anstehende Klausuren mit Countdown. Die Kinder duerfen hier selbst eintragen - deshalb
    /// bekommt der Willkommensbildschirm ein Eingabefeld statt nur einer Anzeige.
    /// </summary>
    public ObservableCollection<ExamItemViewModel> Exams { get; } = new();

    public bool ShowExams => Exams.Count > 0;

    /// <summary>Wie viele davon noch offen sind - fuer die Ueberschrift der Karte.</summary>
    public int OpenHomeworkCount => Homework.Count(h => !h.IsCompleted);

    public bool AllHomeworkDone => Homework.Count > 0 && OpenHomeworkCount == 0;

    /// <summary>Nach dem Abhaken die abgeleiteten Anzeigen neu berechnen lassen.</summary>
    public void RefreshHomeworkCount()
    {
        OnPropertyChanged(nameof(OpenHomeworkCount));
        OnPropertyChanged(nameof(AllHomeworkDone));
    }

    public WelcomeViewModel(
        string profileName,
        int currentStreak,
        Action onContinue,
        Action<AppLanguage> onSwitchLanguage,
        int dueReviewCount = 0,
        WeeklyGoalCalculator.WeeklyGoalStatus weeklyGoal = default,
        IEnumerable<HomeworkItemViewModel>? homework = null,
        IEnumerable<ExamItemViewModel>? exams = null,
        Action? onAddExam = null,
        Action<ExamItemViewModel>? onDeleteExam = null)
    {
        foreach (var item in homework ?? Enumerable.Empty<HomeworkItemViewModel>())
        {
            Homework.Add(item);
        }

        foreach (var exam in exams ?? Enumerable.Empty<ExamItemViewModel>())
        {
            Exams.Add(exam);
        }

        _onAddExam = onAddExam;
        _onDeleteExam = onDeleteExam;

        ProfileName = profileName;
        CurrentStreak = currentStreak;
        DueReviewCount = dueReviewCount;
        WeeklyGoal = weeklyGoal;
        _onContinue = onContinue;
        _onSwitchLanguage = onSwitchLanguage;
    }

    [RelayCommand]
    private void Continue() => _onContinue();

    [RelayCommand]
    private void ToggleLanguage()
    {
        var next = Localization.LocalizationService.Instance.CurrentLanguage == AppLanguage.Deutsch
            ? AppLanguage.Tuerkisch
            : AppLanguage.Deutsch;
        _onSwitchLanguage(next);
    }
}
