using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.ContentGen.HomeworkChat;
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
    private readonly IHomeworkHelpChatService _homeworkChat;

    public QuizQuestion Question { get; }

    public bool IsOpenText => Question.Type == QuestionType.OpenText;
    public bool IsChoice => !IsOpenText;

    /// <summary>
    /// Antwortoptionen in zufälliger Reihenfolge fürs Anzeigen. Die Generatoren legen die richtige
    /// Antwort meist als erstes Element in <see cref="QuizQuestion.Options"/> an - ohne Mischen wäre
    /// "immer die erste Zeile" die richtige Antwort, was Kinder sehr schnell durchschauen.
    /// Einmal pro Frageninstanz gemischt (nicht bei jedem Rebind neu), damit die Reihenfolge beim
    /// Neuzeichnen der Karte stabil bleibt.
    /// </summary>
    public IReadOnlyList<string> DisplayOptions { get; }

    /// <summary>
    /// Zeigt bei offenen Türkisch-Fragen (Übungen, News, Quiz) eine Reihe von Sonderzeichen-Buttons
    /// (ç ğ ı ş) an, da diese auf einer deutschen Tastatur nicht direkt eingebbar sind. Nicht nur bei
    /// Subject.Tuerkisch, da auch türkische Nachrichtenartikel-Fragen offene Antworten mit türkischen
    /// Wörtern verlangen können.
    /// </summary>
    public bool ShowTurkishCharacterHelper => IsOpenText &&
        (Question.Subject == Subject.Tuerkisch || Question.RequiresTurkishCharacters);

    /// <summary>Zeigt einen einfachen Taschenrechner bei offenen Mathematik-Aufgaben.</summary>
    public bool ShowCalculator => IsOpenText && Question.Subject == Subject.Mathematik;

    /// <summary>Ob diese Frage einen vorab abrufbaren Tipp hat (Formel/Vorgehen, keine Lösung).</summary>
    public bool HasHelpHint => !string.IsNullOrWhiteSpace(Question.HelpHint);

    [ObservableProperty]
    private bool isHelpHintVisible;

    [RelayCommand]
    private void ToggleHelpHint() => IsHelpHintVisible = !IsHelpHintVisible;

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

    private static readonly Random ShuffleRandom = new();

    // --- KI-Lernchat: Kinder können wie mit einem Taschenrechner zu jeder Aufgabe nachfragen ---

    [ObservableProperty]
    private bool isChatOpen;

    [ObservableProperty]
    private string chatInputText = string.Empty;

    [ObservableProperty]
    private bool isChatLoading;

    [ObservableProperty]
    private string chatErrorMessage = string.Empty;

    public ObservableCollection<ChatMessage> ChatMessages { get; } = new();

    public QuestionAnswerViewModel(
        QuizQuestion question,
        IHomeworkHelpChatService homeworkChat,
        Action<QuestionAnswerViewModel>? onSubmitted = null)
    {
        Question = question;
        _homeworkChat = homeworkChat;
        _onSubmitted = onSubmitted;
        DisplayOptions = question.Options.Count == 0
            ? question.Options
            : question.Options.OrderBy(_ => ShuffleRandom.Next()).ToList();
    }

    [RelayCommand]
    private void ToggleChat() => IsChatOpen = !IsChatOpen;

    [RelayCommand(CanExecute = nameof(CanSendChatMessage))]
    private async Task SendChatMessageAsync()
    {
        var text = ChatInputText.Trim();
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        ChatErrorMessage = string.Empty;
        ChatMessages.Add(new ChatMessage { Role = ChatRole.Kind, Text = text });
        ChatInputText = string.Empty;
        IsChatLoading = true;

        try
        {
            var reply = await _homeworkChat.AskAsync(Question, ChatMessages.ToList());
            ChatMessages.Add(new ChatMessage { Role = ChatRole.Assistent, Text = reply });
        }
        catch (Exception ex)
        {
            ChatErrorMessage = ex.Message;
        }
        finally
        {
            IsChatLoading = false;
        }
    }

    private bool CanSendChatMessage() => !IsChatLoading && !string.IsNullOrWhiteSpace(ChatInputText);

    partial void OnChatInputTextChanged(string value) => SendChatMessageCommand.NotifyCanExecuteChanged();

    partial void OnIsChatLoadingChanged(bool value) => SendChatMessageCommand.NotifyCanExecuteChanged();

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
