using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Generisches Übungs-ViewModel für jedes Fach (Mathematik, Deutsch, Türkisch, Englisch,
/// Biologie, Chemie, Physik, Gewi, Politik, Geo, Ethik, ITG): zeigt eine Frage nach der
/// anderen mit Erklärung, bis der Fachbereich abgeschlossen ist.
/// </summary>
public sealed partial class ExerciseViewModel : ObservableObject
{
    private readonly IReadOnlyList<QuizQuestion> _questions;
    private readonly Action<Subject, QuestionOutcome, QuizQuestion> _onQuestionAnswered;
    private readonly Action _onSubjectCompleted;

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private QuestionAnswerViewModel? currentQuestion;

    public Subject Subject { get; }
    public int TotalQuestions => _questions.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastQuestion => CurrentIndex >= _questions.Count - 1;

    public ExerciseViewModel(
        Subject subject,
        IReadOnlyList<QuizQuestion> questions,
        Action<Subject, QuestionOutcome, QuizQuestion> onQuestionAnswered,
        Action onSubjectCompleted)
    {
        Subject = subject;
        _questions = questions;
        _onQuestionAnswered = onQuestionAnswered;
        _onSubjectCompleted = onSubjectCompleted;

        LoadCurrent();
    }

    private void LoadCurrent()
    {
        if (CurrentIndex >= _questions.Count)
        {
            _onSubjectCompleted();
            return;
        }

        CurrentQuestion = new QuestionAnswerViewModel(_questions[CurrentIndex], OnAnswered);
        OnPropertyChanged(nameof(DisplayIndex));
        OnPropertyChanged(nameof(IsLastQuestion));
    }

    private void OnAnswered(QuestionAnswerViewModel answered)
    {
        _onQuestionAnswered(Subject, answered.ToOutcome(), answered.Question);
    }

    [RelayCommand]
    private void Next()
    {
        CurrentIndex++;
        LoadCurrent();
    }
}
