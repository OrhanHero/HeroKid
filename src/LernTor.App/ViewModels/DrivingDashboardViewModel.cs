using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Eine Zeichengruppe in der Übersicht, mit Lernstand.</summary>
public sealed class SignCategoryRowViewModel
{
    public SignCategoryRowViewModel(CategoryProgress progress, bool isRecommended, TrafficSign? sample)
    {
        Progress = progress;
        IsRecommended = isRecommended;
        Sample = sample;
    }

    public CategoryProgress Progress { get; }

    /// <summary>Ein Zeichen der Gruppe als Vorschaubild - sagt mehr als der Gruppenname.</summary>
    public TrafficSign? Sample { get; }

    /// <summary>Die erste Gruppe, die noch nicht vollständig sitzt.</summary>
    public bool IsRecommended { get; }

    public TrafficSignCategory Category => Progress.Category;

    public string Title => TrafficSignCatalog.CategoryLabel(Progress.Category);

    public string Description => TrafficSignCatalog.CategoryDescription(Progress.Category);

    public string ProgressDisplay => Progress.IsComplete
        ? LocalizationService.Instance["Fs_Complete"]
        : string.Format(LocalizationService.Instance["Fs_Progress"], Progress.Mastered, Progress.Total);

    public double Fraction => Progress.Fraction;

    public bool IsComplete => Progress.IsComplete;
}

/// <summary>
/// Der Einstieg in den Führerschein-Bereich: drei Unterbereiche und die tägliche Challenge.
///
/// <para><b>Nur die Challenge ist Pflicht</b> - fünf Zeichen, eine Minute. Alles andere ist
/// freiwillig und jederzeit erreichbar. Ein Bereich, der wie eine zweite Schule wirkt, wird
/// nicht benutzt; einer, der jeden Tag eine Minute kostet, schon.</para>
///
/// <para>Die Theoriefragen sind der zweite Unterbereich und von hier aus erreichbar. Der
/// Theorie-Kurs ist sichtbar, aber noch nicht befüllt - bewusst sichtbar statt versteckt, damit
/// die Kinder sehen, wohin der Bereich geht. Der Zustand steht dran, damit niemand vergeblich
/// klickt.</para>
/// </summary>
public sealed partial class DrivingDashboardViewModel : ObservableObject
{
    private readonly Action _onStartChallenge;
    private readonly Action<TrafficSignCategory> _onStartFlashcards;
    private readonly Action<TrafficSignCategory> _onStartQuiz;
    private readonly Action _onOpenTheory;
    private readonly Action _onContinue;

    public DrivingDashboardViewModel(
        IReadOnlyList<TrafficSign> pool,
        IReadOnlySet<string> masteredNumbers,
        int challengeSignCount,
        bool challengeDoneToday,
        int? challengeCorrectToday,
        Action onStartChallenge,
        Action<TrafficSignCategory> onStartFlashcards,
        Action<TrafficSignCategory> onStartQuiz,
        int theoryMastered,
        int theoryTotal,
        Action onOpenTheory,
        Action onContinue)
    {
        _onStartChallenge = onStartChallenge;
        _onStartFlashcards = onStartFlashcards;
        _onStartQuiz = onStartQuiz;
        _onOpenTheory = onOpenTheory;
        _onContinue = onContinue;

        ChallengeSignCount = challengeSignCount;
        ChallengeDoneToday = challengeDoneToday;
        ChallengeCorrectToday = challengeCorrectToday;
        TheoryMastered = theoryMastered;
        TheoryTotal = theoryTotal;

        var fortschritt = TrafficSignProgress.ByCategory(pool, masteredNumbers);
        var empfohlen = TrafficSignProgress.NextRecommended(fortschritt);

        foreach (var eintrag in fortschritt)
        {
            var beispiel = pool.FirstOrDefault(sign =>
                sign.Category == eintrag.Category && !masteredNumbers.Contains(sign.Number))
                ?? pool.FirstOrDefault(sign => sign.Category == eintrag.Category);

            Categories.Add(new SignCategoryRowViewModel(eintrag, eintrag.Category == empfohlen, beispiel));
        }

        MasteredTotal = pool.Count(sign => masteredNumbers.Contains(sign.Number));
        SignTotal = pool.Count;
    }

    public ObservableCollection<SignCategoryRowViewModel> Categories { get; } = new();

    public int ChallengeSignCount { get; }

    public bool ChallengeDoneToday { get; }

    public int? ChallengeCorrectToday { get; }

    public int MasteredTotal { get; }

    public int SignTotal { get; }

    /// <summary>Theoriefragen, die gerade sitzen.</summary>
    public int TheoryMastered { get; }

    public int TheoryTotal { get; }

    public string TheoryProgressDisplay => string.Format(
        LocalizationService.Instance["Fs_TheoryOverall"], TheoryMastered, TheoryTotal);

    public string OverallDisplay =>
        string.Format(LocalizationService.Instance["Fs_Progress"], MasteredTotal, SignTotal);

    /// <summary>
    /// Nach einem Neustart ist der Zaehler unbekannt (er lebt nur in der laufenden Sitzung) -
    /// dann steht nur, dass die Challenge erledigt ist. "0 von 5 richtig" anzuzeigen, weil man
    /// die Zahl nicht mehr weiss, waere schlicht gelogen.
    /// </summary>
    public string ChallengeHint => ChallengeDoneToday
        ? ChallengeCorrectToday is { } richtig
            ? string.Format(LocalizationService.Instance["Fs_ChallengeDone"], richtig, ChallengeSignCount)
            : LocalizationService.Instance["Fs_ChallengeDoneSimple"]
        : string.Format(LocalizationService.Instance["Fs_ChallengeHint"], ChallengeSignCount);

    /// <summary>Weiter zur nächsten Etappe darf man erst nach der Challenge - sie ist der
    /// Pflichtteil dieses Bereichs.</summary>
    public bool CanContinue => ChallengeDoneToday;

    [RelayCommand]
    private void StartChallenge() => _onStartChallenge();

    [RelayCommand]
    private void StartFlashcards(SignCategoryRowViewModel? row)
    {
        if (row is not null)
        {
            _onStartFlashcards(row.Category);
        }
    }

    [RelayCommand]
    private void StartQuiz(SignCategoryRowViewModel? row)
    {
        if (row is not null)
        {
            _onStartQuiz(row.Category);
        }
    }

    [RelayCommand]
    private void OpenTheory() => _onOpenTheory();

    [RelayCommand]
    private void Continue() => _onContinue();
}
