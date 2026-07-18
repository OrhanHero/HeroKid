using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.News;

namespace LernTor.App.ViewModels;

/// <summary>
/// Pflicht-News-Teil: Artikel nacheinander lesen und die Verständnisfragen beantworten.
/// Die Marker im Kopf sind klickbar (Sprung-Navigation); "Weiter" führt zum nächsten OFFENEN
/// Artikel, der Abschluss-Button erscheint erst, wenn ALLE Artikel beantwortet sind.
///
/// <para><b>Mindest-Lesezeit pro Artikel:</b> wie bei den Fach-Übungen wird "Weiter" erst frei,
/// wenn die Fragen beantwortet sind UND ein kurzer Countdown abgelaufen ist - die Nachricht soll
/// gelesen und verstanden werden, nicht weggeklickt. Bereits erledigte Artikel (Wiederbesuch
/// über die Marker) haben keinen Countdown.</para>
/// </summary>
public sealed partial class NewsViewModel : ObservableObject
{
    /// <summary>Mindest-Lesezeit pro Artikel in Sekunden (pro Profil im Eltern-Bereich
    /// einstellbar, siehe StudentProfile.NewsSecondsPerArticle).</summary>
    private readonly int _minSecondsPerArticle;

    private readonly IReadOnlyList<NewsArticle> _articles;
    private readonly Action<NewsArticle, QuestionOutcome, QuizQuestion> _onArticleAnswered;
    private readonly Action _onSectionCompleted;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly DispatcherTimer _minTimeTimer;

    /// <summary>Artikel-IDs, deren Fragen bereits vollständig beantwortet wurden - vorbefüllt mit den
    /// Abschlüssen aus einer früheren (z.B. abgestürzten) Session desselben Tages.</summary>
    private readonly HashSet<string> _completedArticleIds;

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private NewsArticle? currentArticle;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoNext))]
    private bool canProceed;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoNext))]
    [NotifyPropertyChangedFor(nameof(ShowLockCountdown))]
    private int lockSecondsRemaining;

    public ObservableCollection<QuestionAnswerViewModel> CurrentQuestions { get; } = new();

    /// <summary>Ein klickbarer Marker je Artikel (erledigt/aktuell/offen) für die Übersichtsleiste
    /// im Kopf - so sieht das Kind auch nach einem Neustart, welche Artikel heute geschafft sind.</summary>
    public ObservableCollection<SessionStepViewModel> ArticleMarkers { get; } = new();

    public int TotalArticles => _articles.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastArticle => CurrentIndex >= _articles.Count - 1;
    public bool HasArticles => _articles.Count > 0;

    /// <summary>"Weiter"/"Abschließen" erst nach Antwort UND abgelaufener Mindest-Lesezeit.</summary>
    public bool CanGoNext => CanProceed && LockSecondsRemaining <= 0;

    /// <summary>Countdown-Hinweis nur zeigen, solange die Mindest-Lesezeit noch läuft.</summary>
    public bool ShowLockCountdown => LockSecondsRemaining > 0;

    /// <summary>True, sobald die Fragen ALLER Artikel beantwortet sind - erst dann erscheint der
    /// Abschluss-Button. Nötig wegen der Marker-Sprung-Navigation: "letzter Artikel erreicht"
    /// heißt nicht mehr automatisch "alles erledigt".</summary>
    public bool AllArticlesCompleted =>
        _articles.Count == 0 || _articles.All(a => _completedArticleIds.Contains(a.Id));

    /// <summary>Steuert die Sichtbarkeit der "Schwierige Wörter"-Box (eine leere Box ohne
    /// Einträge soll gar nicht erst erscheinen).</summary>
    public bool HasExplainedTerms => CurrentArticle?.ExplainedTerms.Count > 0;

    partial void OnCurrentArticleChanged(NewsArticle? value) => OnPropertyChanged(nameof(HasExplainedTerms));

    // --- Wetter-Widget (Berlin, Open-Meteo; null = Abruf fehlgeschlagen → Widget ausgeblendet) ---

    /// <summary>Der kindgerechte Wetterbericht; DE/TR-Texte wählt die Oberfläche über die
    /// Properties darunter anhand der aktuellen App-Sprache (Momentaufnahme beim Anzeigen -
    /// ein Sprachwechsel mitten im News-Teil ist im Kiosk-Ablauf nicht vorgesehen).</summary>
    public KidWeatherReport? Weather { get; }

    public bool HasWeather => Weather is not null;

    public string WeatherTempDisplay => Weather is null
        ? string.Empty
        : $"{Weather.Emoji} {Weather.CurrentTemp}°C";

    public string WeatherRangeDisplay => Weather is null
        ? string.Empty
        : $"↑ {Weather.TodayMax}° / ↓ {Weather.TodayMin}°";

    public string WeatherDescription => Weather is null
        ? string.Empty
        : (LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch
            ? Weather.DescriptionTr
            : Weather.DescriptionDe);

    public string WeatherTip => Weather is null
        ? string.Empty
        : (LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch
            ? Weather.TipTr
            : Weather.TipDe);

    public NewsViewModel(
        IReadOnlyList<NewsArticle> articles,
        HashSet<string> alreadyCompletedIds,
        Action<NewsArticle, QuestionOutcome, QuizQuestion> onArticleAnswered,
        Action onSectionCompleted,
        IHomeworkHelpChatService homeworkChat,
        KidWeatherReport? weather = null,
        int minSecondsPerArticle = StudentProfile.DefaultNewsSecondsPerArticle)
    {
        _minSecondsPerArticle = minSecondsPerArticle > 0 ? minSecondsPerArticle : StudentProfile.DefaultNewsSecondsPerArticle;
        Weather = weather;
        _articles = articles;
        _onArticleAnswered = onArticleAnswered;
        _onSectionCompleted = onSectionCompleted;
        _homeworkChat = homeworkChat;
        _completedArticleIds = new HashSet<string>(alreadyCompletedIds);

        _minTimeTimer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _minTimeTimer.Tick += (_, _) => Tick();

        // Bereits abgeschlossene Artikel dieser Session überspringen (Fortschritt aus Absturz/Neustart).
        while (CurrentIndex < _articles.Count && alreadyCompletedIds.Contains(_articles[CurrentIndex].Id))
        {
            CurrentIndex++;
        }

        LoadCurrentArticle();
    }

    private void Tick()
    {
        if (LockSecondsRemaining > 0)
        {
            LockSecondsRemaining--;
        }

        if (LockSecondsRemaining <= 0)
        {
            _minTimeTimer.Stop();
        }
    }

    /// <summary>Baut die Marker-Leiste neu auf. Erledigt = Fragen vollständig beantwortet (auch aus
    /// einer früheren Session heute), aktuell = gerade angezeigter Artikel.</summary>
    private void RebuildArticleMarkers()
    {
        ArticleMarkers.Clear();
        for (var i = 0; i < _articles.Count; i++)
        {
            ArticleMarkers.Add(new SessionStepViewModel
            {
                Label = (i + 1).ToString(),
                IsDone = _completedArticleIds.Contains(_articles[i].Id),
                IsCurrent = i == CurrentIndex,
                Index = i
            });
        }

        OnPropertyChanged(nameof(AllArticlesCompleted));
    }

    private void LoadCurrentArticle()
    {
        CurrentQuestions.Clear();

        if (CurrentIndex >= _articles.Count)
        {
            CurrentArticle = null;
            CanProceed = true;
            LockSecondsRemaining = 0;
            return;
        }

        CurrentArticle = _articles[CurrentIndex];
        foreach (var question in CurrentArticle.ComprehensionQuestions)
        {
            CurrentQuestions.Add(new QuestionAnswerViewModel(question, _homeworkChat, OnQuestionSubmitted));
        }

        var alreadyCompleted = _completedArticleIds.Contains(CurrentArticle.Id);

        // "Weiter" ist frei, wenn es nichts zu beantworten gibt ODER dieser Artikel schon
        // erledigt ist (Wiederbesuch über Marker - erneutes Beantworten wäre unfair).
        CanProceed = CurrentQuestions.Count == 0 || alreadyCompleted;

        // Mindest-Lesezeit nur für noch offene Artikel; beim Wiederbesuch erledigter Artikel
        // wäre ein erneuter Countdown reine Navigations-Schikane.
        LockSecondsRemaining = alreadyCompleted ? 0 : _minSecondsPerArticle;
        if (LockSecondsRemaining > 0)
        {
            _minTimeTimer.Start();
        }

        // Fragenlose Artikel zählen sofort als erledigt, sonst könnte AllArticlesCompleted
        // (und damit der Abschluss-Button) nie wahr werden.
        if (CurrentQuestions.Count == 0)
        {
            _completedArticleIds.Add(CurrentArticle.Id);
        }

        OnPropertyChanged(nameof(DisplayIndex));
        OnPropertyChanged(nameof(IsLastArticle));
        RebuildArticleMarkers();
    }

    private void OnQuestionSubmitted(QuestionAnswerViewModel answered)
    {
        if (CurrentArticle is null)
        {
            return;
        }

        _onArticleAnswered(CurrentArticle, answered.ToOutcome(), answered.Question);
        CanProceed = CurrentQuestions.All(q => q.IsSubmitted);

        // Marker sofort auf "erledigt" stellen, sobald die letzte Frage des Artikels beantwortet
        // ist - nicht erst beim Klick auf "Nächster Artikel".
        if (CanProceed)
        {
            _completedArticleIds.Add(CurrentArticle.Id);
            RebuildArticleMarkers();
        }
    }

    /// <summary>Springt zum nächsten noch offenen Artikel (hinter dem aktuellen, sonst von vorn) -
    /// durch die Marker-Navigation ist "einfach Index+1" nicht mehr zwingend der nächste offene.</summary>
    [RelayCommand]
    private void NextArticle()
    {
        if (!CanGoNext || AllArticlesCompleted)
        {
            return;
        }

        for (var offset = 1; offset <= _articles.Count; offset++)
        {
            var candidate = (CurrentIndex + offset) % _articles.Count;
            if (!_completedArticleIds.Contains(_articles[candidate].Id))
            {
                CurrentIndex = candidate;
                LoadCurrentArticle();
                return;
            }
        }
    }

    /// <summary>Schließt den News-Bereich ab - Button erscheint erst, wenn ALLE Artikel erledigt
    /// sind, und respektiert die Mindest-Lesezeit des zuletzt beantworteten Artikels.</summary>
    [RelayCommand]
    private void FinishSection()
    {
        if (AllArticlesCompleted && LockSecondsRemaining <= 0)
        {
            _minTimeTimer.Stop();
            _onSectionCompleted();
        }
    }

    /// <summary>Klick auf einen Artikel-Marker: direkt zu diesem Artikel springen.</summary>
    [RelayCommand]
    private void JumpToArticle(SessionStepViewModel marker)
    {
        if (marker.Index < 0 || marker.Index >= _articles.Count || marker.Index == CurrentIndex)
        {
            return;
        }

        CurrentIndex = marker.Index;
        LoadCurrentArticle();
    }
}
