using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Ein Sachgebiet in der Theorie-Übersicht, mit Lernstand.</summary>
public sealed class TheoryTopicRowViewModel
{
    public TheoryTopicRowViewModel(
        DrivingTheoryTopic topic, int questionCount, int mastered, TopicMastery? mastery)
    {
        Topic = topic;
        QuestionCount = questionCount;
        Mastered = mastered;
        Mastery = mastery;
    }

    public DrivingTheoryTopic Topic { get; }

    public int QuestionCount { get; }

    /// <summary>Fragen dieses Gebiets, die gerade sitzen.</summary>
    public int Mastered { get; }

    public TopicMastery? Mastery { get; }

    public string Title => DrivingTheoryCatalog.TopicLabel(Topic);

    public double Fraction => QuestionCount == 0 ? 0 : (double)Mastered / QuestionCount;

    public bool IsComplete => QuestionCount > 0 && Mastered == QuestionCount;

    /// <summary>Nur ein belastbar gemessenes Gebiet gilt als Schwachstelle - siehe
    /// <see cref="TheoryExamComposer.MinAnswersForMastery"/>.</summary>
    public bool IsWeak => Mastery is { IsWeak: true };

    public string ProgressDisplay => IsComplete
        ? LocalizationService.Instance["Fs_TheoryTopicComplete"]
        : string.Format(LocalizationService.Instance["Fs_TheoryTopicProgress"], Mastered, QuestionCount);
}

/// <summary>
/// Der Unterbereich Theoriefragen: die vierzehn amtlichen Sachgebiete, die Prüfungssimulation
/// und der Schwachstellen-Trainer.
///
/// <para><b>Der Schwachstellen-Trainer steht oben, nicht unten.</b> Wer selbst wählt, übt das,
/// was ohnehin schon sitzt - das fühlt sich gut an und bringt nichts. Die Auswahl nach
/// Sachgebiet bleibt trotzdem da, weil ein Fahrschul-Thema der Woche sonst nicht gezielt geübt
/// werden könnte.</para>
/// </summary>
public sealed partial class TheoryHubViewModel : ObservableObject
{
    private readonly Action<DrivingTheoryTopic> _onPracticeTopic;
    private readonly Action _onStartExam;
    private readonly Action _onStartWeakSpots;
    private readonly Action _onBack;

    public TheoryHubViewModel(
        IReadOnlyList<TheoryQuestion> pool,
        IReadOnlySet<string> masteredQuestionIds,
        IReadOnlyList<TopicMastery> mastery,
        TheoryExamRunEntity? lastExam,
        Action<DrivingTheoryTopic> onPracticeTopic,
        Action onStartExam,
        Action onStartWeakSpots,
        Action onBack)
    {
        _onPracticeTopic = onPracticeTopic;
        _onStartExam = onStartExam;
        _onStartWeakSpots = onStartWeakSpots;
        _onBack = onBack;

        LastExam = lastExam;

        var nachThema = mastery.ToDictionary(eintrag => eintrag.Topic);

        foreach (var thema in DrivingTheoryCatalog.Topics)
        {
            var fragen = pool.Where(frage => frage.Topic == thema).ToList();

            if (fragen.Count == 0)
            {
                continue;
            }

            Topics.Add(new TheoryTopicRowViewModel(
                thema,
                fragen.Count,
                fragen.Count(frage => masteredQuestionIds.Contains(frage.Id)),
                nachThema.TryGetValue(thema, out var wert) ? (TopicMastery?)wert : null));
        }

        QuestionTotal = pool.Count;
        MasteredTotal = pool.Count(frage => masteredQuestionIds.Contains(frage.Id));
        WeakTopicCount = Topics.Count(row => row.IsWeak);
        ExamPossible = pool.Count > 0;
    }

    public ObservableCollection<TheoryTopicRowViewModel> Topics { get; } = new();

    public int QuestionTotal { get; }

    public int MasteredTotal { get; }

    public int WeakTopicCount { get; }

    public bool ExamPossible { get; }

    public TheoryExamRunEntity? LastExam { get; }

    public string OverallDisplay => string.Format(
        LocalizationService.Instance["Fs_TheoryOverall"], MasteredTotal, QuestionTotal);

    /// <summary>
    /// Die Prüfung stellt <see cref="TheoryExamRules.QuestionCount"/> Fragen - solange der
    /// Katalog so viele hergibt. Reicht er nicht, steht die echte Zahl dran statt einer, die
    /// gleich nicht stimmt.
    /// </summary>
    public string ExamHint => string.Format(
        LocalizationService.Instance["Fs_ExamHint"],
        Math.Min(TheoryExamRules.QuestionCount, QuestionTotal),
        TheoryExamRules.MaxWrongPoints);

    /// <summary>
    /// Solange zu wenig geübt wurde, gibt es keine belegten Schwachstellen. Das steht dann auch
    /// so da - der Trainer schickt dann einen gemischten Satz, statt sich Schwächen auszudenken.
    /// </summary>
    public string WeakSpotHint => WeakTopicCount > 0
        ? string.Format(LocalizationService.Instance["Fs_WeakSpotsFound"], WeakTopicCount)
        : LocalizationService.Instance["Fs_WeakSpotsNone"];

    public bool HasLastExam => LastExam is not null;

    public string LastExamDisplay
    {
        get
        {
            if (LastExam is null)
            {
                return string.Empty;
            }

            var ergebnis = LastExam.ToResult();
            var stand = ergebnis.Passed
                ? LocalizationService.Instance["Fs_ExamPassed"]
                : LocalizationService.Instance["Fs_ExamFailed"];

            return string.Format(
                LocalizationService.Instance["Fs_LastExam"],
                stand, ergebnis.WrongPoints, LastExam.TakenAt.ToLocalTime().ToString("dd.MM.yyyy"));
        }
    }

    [RelayCommand]
    private void PracticeTopic(TheoryTopicRowViewModel? row)
    {
        if (row is not null)
        {
            _onPracticeTopic(row.Topic);
        }
    }

    [RelayCommand]
    private void StartExam() => _onStartExam();

    [RelayCommand]
    private void StartWeakSpots() => _onStartWeakSpots();

    [RelayCommand]
    private void Back() => _onBack();
}
