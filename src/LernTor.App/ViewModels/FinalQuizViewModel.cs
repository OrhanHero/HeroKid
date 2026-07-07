using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

public sealed partial class FinalQuizViewModel : ObservableObject
{
    private readonly IReadOnlyList<QuizQuestion> _questions;
    private readonly Action<IReadOnlyList<QuestionOutcome>> _onCompleted;
    private readonly List<QuestionOutcome> _outcomes = new();

    [ObservableProperty]
    private bool hasStarted;

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private QuestionAnswerViewModel? currentQuestion;

    public int TotalQuestions => _questions.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastQuestion => CurrentIndex >= _questions.Count - 1;

    public FinalQuizViewModel(IReadOnlyList<QuizQuestion> questions, Action<IReadOnlyList<QuestionOutcome>> onCompleted)
    {
        _questions = questions;
        _onCompleted = onCompleted;
    }

    [RelayCommand]
    private void Start()
    {
        HasStarted = true;
        LoadCurrent();
    }

    private void LoadCurrent()
    {
        if (CurrentIndex >= _questions.Count)
        {
            _onCompleted(_outcomes);
            return;
        }

        CurrentQuestion = new QuestionAnswerViewModel(_questions[CurrentIndex], OnAnswered);
        OnPropertyChanged(nameof(DisplayIndex));
        OnPropertyChanged(nameof(IsLastQuestion));
    }

    private void OnAnswered(QuestionAnswerViewModel answered)
    {
        _outcomes.Add(answered.ToOutcome());
    }

    [RelayCommand]
    private void Next()
    {
        CurrentIndex++;
        LoadCurrent();
    }
}
