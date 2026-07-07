using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Wiederverwendbares ViewModel für eine einzelne beantwortbare Frage – genutzt von News,
/// Übungsaufgaben und dem Abschlussquiz. Bei Multiple-Choice/Wahr-Falsch wird beim Klick auf
/// eine Option sofort ausgewertet, bei offenen Fragen über einen Prüfen-Button.
/// </summary>
public sealed partial class QuestionAnswerViewModel : ObservableObject
{
    private readonly Action<QuestionAnswerViewModel>? _onSubmitted;

    public QuizQuestion Question { get; }

    public bool IsOpenText => Question.Type == QuestionType.OpenText;
    public bool IsChoice => !IsOpenText;

    /// <summary>
    /// Zeigt bei offenen Türkisch-Fragen eine Reihe von Sonderzeichen-Buttons (ç ğ ı ş) an,
    /// da diese auf einer deutschen Tastatur nicht direkt eingebbar sind.
    /// </summary>
    public bool ShowTurkishCharacterHelper => IsOpenText && Question.Subject == Subject.Tuerkisch;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitOpenTextCommand))]
    private string givenAnswerText = string.Empty;

    [ObservableProperty]
    private bool isSubmitted;

    [ObservableProperty]
    private bool isCorrect;

    [ObservableProperty]
    private string? selectedOption;

    public string FinalGivenAnswer { get; private set; } = string.Empty;

    public QuestionAnswerViewModel(QuizQuestion question, Action<QuestionAnswerViewModel>? onSubmitted = null)
    {
        Question = question;
        _onSubmitted = onSubmitted;
    }

    [RelayCommand]
    private void SelectOption(string option)
    {
        if (IsSubmitted)
        {
            return;
        }

        SelectedOption = option;
        Submit(option);
    }

    [RelayCommand(CanExecute = nameof(CanSubmitOpenText))]
    private void SubmitOpenText() => Submit(GivenAnswerText);

    private bool CanSubmitOpenText() => !IsSubmitted && !string.IsNullOrWhiteSpace(GivenAnswerText);

    private void Submit(string answer)
    {
        if (IsSubmitted)
        {
            return;
        }

        FinalGivenAnswer = answer;
        IsCorrect = Question.CheckAnswer(answer);
        IsSubmitted = true;
        _onSubmitted?.Invoke(this);
    }

    public QuestionOutcome ToOutcome() => new()
    {
        QuestionId = Question.Id,
        Subject = Question.Subject,
        GivenAnswer = FinalGivenAnswer,
        WasCorrect = IsCorrect
    };
}
