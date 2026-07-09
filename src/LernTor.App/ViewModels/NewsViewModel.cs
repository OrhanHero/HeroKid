using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

public sealed partial class NewsViewModel : ObservableObject
{
    private readonly IReadOnlyList<NewsArticle> _articles;
    private readonly Action<NewsArticle, QuestionOutcome, QuizQuestion> _onArticleAnswered;
    private readonly Action<IReadOnlyList<QuizQuestion>> _onSectionCompleted;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly List<QuizQuestion> _allAskedQuestions = new();

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private NewsArticle? currentArticle;

    [ObservableProperty]
    private bool canProceed;

    public ObservableCollection<QuestionAnswerViewModel> CurrentQuestions { get; } = new();

    public int TotalArticles => _articles.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastArticle => CurrentIndex >= _articles.Count - 1;
    public bool HasArticles => _articles.Count > 0;

    public NewsViewModel(
        IReadOnlyList<NewsArticle> articles,
        HashSet<string> alreadyCompletedIds,
        Action<NewsArticle, QuestionOutcome, QuizQuestion> onArticleAnswered,
        Action<IReadOnlyList<QuizQuestion>> onSectionCompleted,
        IHomeworkHelpChatService homeworkChat)
    {
        _articles = articles;
        _onArticleAnswered = onArticleAnswered;
        _onSectionCompleted = onSectionCompleted;
        _homeworkChat = homeworkChat;

        // Bereits an einem Vortag abgeschlossene Artikel dieser Session überspringen (Fortschritt aus Absturz/Neustart).
        while (CurrentIndex < _articles.Count && alreadyCompletedIds.Contains(_articles[CurrentIndex].Id))
        {
            CurrentIndex++;
        }

        LoadCurrentArticle();
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
    }

    private void OnQuestionSubmitted(QuestionAnswerViewModel answered)
    {
        if (CurrentArticle is null)
        {
            return;
        }

        _onArticleAnswered(CurrentArticle, answered.ToOutcome(), answered.Question);
        CanProceed = CurrentQuestions.All(q => q.IsSubmitted);
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
