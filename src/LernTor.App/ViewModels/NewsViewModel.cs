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

    /// <summary>Verhindert Doppel-Einträge in <see cref="_allAskedQuestions"/>, wenn ein Artikel
    /// über die Sprung-Navigation (Marker/Suche) mehrfach besucht wird.</summary>
    private readonly HashSet<string> _askedQuestionIds = new();

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

    /// <summary>True, sobald die Fragen ALLER Artikel beantwortet sind - erst dann erscheint der
    /// Abschluss-Button. Nötig, weil die Sprung-Navigation (Marker/Suche) die frühere rein
    /// lineare Reihenfolge aufhebt: "letzter Artikel erreicht" heißt nicht mehr "alles erledigt".</summary>
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
            return;
        }

        CurrentArticle = _articles[CurrentIndex];
        foreach (var question in CurrentArticle.ComprehensionQuestions)
        {
            // Doppelbesuche über die Sprung-Navigation dürfen die Fragen nicht erneut in die
            // Abschluss-Sammlung legen (sonst tauchen sie doppelt in der News-Wertung auf).
            if (_askedQuestionIds.Add(question.Id))
            {
                _allAskedQuestions.Add(question);
            }

            CurrentQuestions.Add(new QuestionAnswerViewModel(question, _homeworkChat, OnQuestionSubmitted));
        }

        // "Weiter" ist frei, wenn es nichts zu beantworten gibt ODER dieser Artikel schon
        // erledigt ist (Wiederbesuch über Marker/Suche - erneutes Beantworten wäre unfair).
        CanProceed = CurrentQuestions.Count == 0 || _completedArticleIds.Contains(CurrentArticle.Id);

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
    /// durch die Sprung-Navigation ist "einfach Index+1" nicht mehr zwingend der nächste offene.</summary>
    [RelayCommand]
    private void NextArticle()
    {
        if (!CanProceed || AllArticlesCompleted)
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

    /// <summary>Schließt den News-Bereich ab - Button erscheint erst, wenn ALLE Artikel erledigt sind.</summary>
    [RelayCommand]
    private void FinishSection()
    {
        if (AllArticlesCompleted)
        {
            _onSectionCompleted(_allAskedQuestions);
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

    // --- Suche über die heutigen Artikel (Titel + Zusammenfassung) ---

    [ObservableProperty]
    private string searchText = string.Empty;

    public ObservableCollection<SessionStepViewModel> SearchResults { get; } = new();

    public bool HasSearchResults => SearchResults.Count > 0;

    partial void OnSearchTextChanged(string value)
    {
        SearchResults.Clear();

        var query = value.Trim();
        if (query.Length >= 2)
        {
            for (var i = 0; i < _articles.Count; i++)
            {
                var article = _articles[i];
                if (article.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    article.SimplifiedSummary.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    SearchResults.Add(new SessionStepViewModel
                    {
                        Label = $"{i + 1}. {Shorten(article.Title, 70)}",
                        IsDone = _completedArticleIds.Contains(article.Id),
                        IsCurrent = i == CurrentIndex,
                        Index = i
                    });
                }
            }
        }

        OnPropertyChanged(nameof(HasSearchResults));
    }

    /// <summary>Klick auf ein Suchergebnis: hinspringen und die Ergebnisliste schließen.</summary>
    [RelayCommand]
    private void JumpToSearchResult(SessionStepViewModel result)
    {
        JumpToArticle(result);
        SearchText = string.Empty;
    }

    private static string Shorten(string text, int maxLength) =>
        text.Length <= maxLength ? text : text[..maxLength].TrimEnd() + "…";
}
