using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.News;

namespace LernTor.App.ViewModels;

public sealed partial class NewsViewModel : ObservableObject
{
    private readonly IReadOnlyList<NewsArticle> _articles;
    private readonly Action<NewsArticle, QuestionOutcome, QuizQuestion> _onArticleAnswered;
    private readonly Action<IReadOnlyList<QuizQuestion>> _onSectionCompleted;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly List<QuizQuestion> _allAskedQuestions = new();

    /// <summary>Artikel-IDs, deren Fragen bereits vollständig beantwortet wurden - vorbefüllt mit den
    /// Abschlüssen aus einer früheren (z.B. abgestürzten) Session desselben Tages.</summary>
    private readonly HashSet<string> _completedArticleIds;

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private NewsArticle? currentArticle;

    [ObservableProperty]
    private bool canProceed;

    public ObservableCollection<QuestionAnswerViewModel> CurrentQuestions { get; } = new();

    /// <summary>Ein Marker je Artikel (erledigt/aktuell/offen) für die Übersichtsleiste im Kopf -
    /// so sieht das Kind auch nach einem Neustart, welche Artikel heute schon geschafft sind.</summary>
    public ObservableCollection<SessionStepViewModel> ArticleMarkers { get; } = new();

    public int TotalArticles => _articles.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastArticle => CurrentIndex >= _articles.Count - 1;
    public bool HasArticles => _articles.Count > 0;

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
        Action<IReadOnlyList<QuizQuestion>> onSectionCompleted,
        IHomeworkHelpChatService homeworkChat,
        KidWeatherReport? weather = null)
    {
        Weather = weather;
        _articles = articles;
        _onArticleAnswered = onArticleAnswered;
        _onSectionCompleted = onSectionCompleted;
        _homeworkChat = homeworkChat;
        _completedArticleIds = new HashSet<string>(alreadyCompletedIds);

        // Bereits an einem Vortag abgeschlossene Artikel dieser Session überspringen (Fortschritt aus Absturz/Neustart).
        while (CurrentIndex < _articles.Count && alreadyCompletedIds.Contains(_articles[CurrentIndex].Id))
        {
            CurrentIndex++;
        }

        LoadCurrentArticle();
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
                IsDone = _completedArticleIds.Contains(_articles[i].Id) || i < CurrentIndex,
                IsCurrent = i == CurrentIndex
            });
        }
    }

    private void LoadCurrentArticle()
    {
        CurrentQuestions.Clear();

        if (CurrentIndex >= _articles.Count)
        {
            CurrentArticle = null;
            CanProceed = true;
            return;
        }

        CurrentArticle = _articles[CurrentIndex];
        foreach (var question in CurrentArticle.ComprehensionQuestions)
        {
            _allAskedQuestions.Add(question);
            CurrentQuestions.Add(new QuestionAnswerViewModel(question, _homeworkChat, OnQuestionSubmitted));
        }

        CanProceed = CurrentQuestions.Count == 0;
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

    [RelayCommand]
    private void NextArticle()
    {
        if (!CanProceed)
        {
            return;
        }

        if (IsLastArticle)
        {
            _onSectionCompleted(_allAskedQuestions);
            return;
        }

        CurrentIndex++;
        LoadCurrentArticle();
    }
}
