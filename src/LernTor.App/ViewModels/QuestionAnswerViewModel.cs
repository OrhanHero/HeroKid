using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Services;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Wiederverwendbares ViewModel für eine einzelne beantwortbare Frage – genutzt von News,
/// Übungsaufgaben und dem Abschlussquiz. Bei Multiple-Choice/Wahr-Falsch wird beim Klick auf
/// eine Option sofort ausgewertet, bei offenen Fragen über einen Prüfen-Button.
/// </summary>
public sealed partial class QuestionAnswerViewModel : ObservableObject
{
    private readonly Action<QuestionAnswerViewModel>? _onSubmitted;
    private readonly Action<QuestionAnswerViewModel>? _onExplanationAcknowledged;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly TextToSpeechService? _speech;
    private readonly bool _requireExplanationAcknowledgment;

    /// <summary>
    /// Wann diese Karte aufgebaut wurde. Alle drei Aufrufer (Übungen, Abschlussquiz, News) bauen
    /// das ViewModel genau dann, wenn die Frage sichtbar wird - der Aufbauzeitpunkt ist damit der
    /// Anzeigezeitpunkt. Bei News stehen mehrere Fragen eines Artikels gleichzeitig auf dem
    /// Bildschirm; dort enthält die zweite Frage auch die Zeit der ersten. Das überschätzt die
    /// Dauer, macht die Auswertung also höchstens vorsichtiger und erzeugt keine Fehlalarme.
    /// Monoton steigende Uhr statt Systemzeit, damit eine Zeitumstellung keine negativen
    /// Dauern erzeugt.
    /// </summary>
    private readonly long _shownAtTicks = System.Diagnostics.Stopwatch.GetTimestamp();

    /// <summary>Bearbeitungsdauer in Millisekunden, gesetzt beim Absenden.</summary>
    public int DurationMs { get; private set; }

    public QuizQuestion Question { get; }

    public bool IsOpenText => Question.Type == QuestionType.OpenText;

    /// <summary>
    /// Diktat: der Satz wird vorgelesen statt angezeigt. Die Anzeige muss den Prompt deshalb
    /// verbergen - stünde er da, wäre es Abschreiben und kein Diktat.
    /// </summary>
    public bool IsDictation => Question.Type == QuestionType.Diktat;

    /// <summary>Beide Freitext-Arten teilen sich das Eingabefeld und den Prüfen-Knopf.</summary>
    public bool IsTypedAnswer => IsOpenText || IsDictation;

    public bool IsChoice => !IsTypedAnswer;

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

    // --- Anti-Durchklick (nur Übungsteil, siehe ExerciseViewModel): nach einer falschen Antwort
    // muss die Erklärung aktiv bestätigt werden, bevor "Weiter" freigegeben wird - sonst wird
    // das Feedback (der eigentliche Lernmoment) einfach weggeklickt. ---

    /// <summary>Vom Kind bestätigt: "Erklärung gelesen". Gibt im Übungsteil "Weiter" frei.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NeedsExplanationAcknowledgment))]
    private bool explanationAcknowledged;

    /// <summary>Bestätigungs-Button nur zeigen, wenn der Host ihn verlangt (Übungsteil), die
    /// Antwort falsch war und noch nicht bestätigt wurde.</summary>
    public bool NeedsExplanationAcknowledgment =>
        _requireExplanationAcknowledgment && IsSubmitted && !IsCorrect && !ExplanationAcknowledged;

    [RelayCommand]
    private void AcknowledgeExplanation()
    {
        if (ExplanationAcknowledged)
        {
            return;
        }

        ExplanationAcknowledged = true;
        _onExplanationAcknowledged?.Invoke(this);
    }

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
        Action<QuestionAnswerViewModel>? onSubmitted = null,
        bool requireExplanationAcknowledgment = false,
        Action<QuestionAnswerViewModel>? onExplanationAcknowledged = null,
        TextToSpeechService? speech = null)
    {
        Question = question;
        _homeworkChat = homeworkChat;
        _speech = speech;
        _onSubmitted = onSubmitted;
        _requireExplanationAcknowledgment = requireExplanationAcknowledgment;
        _onExplanationAcknowledged = onExplanationAcknowledged;
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

    // --- Diktat ---

    /// <summary>
    /// Liest den Diktatsatz vor. Beliebig oft aufrufbar: eine Lehrkraft liest ein Diktat auch
    /// zwei- bis dreimal, und ein Kind, das einen Satz nicht verstanden hat, soll ihn hören
    /// dürfen - der Fehler soll aus der Rechtschreibung kommen, nicht aus dem Hören.
    /// </summary>
    [RelayCommand]
    private void SpeakDictation()
    {
        if (!IsDictation || _speech is null)
        {
            return;
        }

        // Erst stoppen: sonst reihen sich bei mehrmaligem Klicken die Wiedergaben hintereinander.
        _speech.Stop();
        _speech.Speak(Question.Prompt, "de-DE");
    }

    /// <summary>Wort-für-Wort-Rückmeldung nach dem Absenden (leer, solange nicht abgesendet).</summary>
    [ObservableProperty]
    private string dictationFeedback = string.Empty;

    /// <summary>Der richtige Satz - erst NACH dem Absenden sichtbar.</summary>
    public string DictationSolution => IsSubmitted && IsDictation ? Question.Prompt : string.Empty;

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
        DurationMs = (int)Math.Clamp(
            System.Diagnostics.Stopwatch.GetElapsedTime(_shownAtTicks).TotalMilliseconds,
            1, int.MaxValue);
        IsSubmitted = true;

        if (IsDictation)
        {
            // Nicht nur "falsch", sondern WELCHES Wort - sonst weiß das Kind nach einem
            // 10-Wort-Satz nicht, woran es lag.
            DictationFeedback = DictationEvaluator.Feedback(
                DictationEvaluator.Evaluate(Question.Prompt, answer));
            OnPropertyChanged(nameof(DictationSolution));
        }

        OnPropertyChanged(nameof(NeedsExplanationAcknowledgment));
        _onSubmitted?.Invoke(this);
    }

    public QuestionOutcome ToOutcome() => new()
    {
        QuestionId = Question.Id,
        Subject = Question.Subject,
        GivenAnswer = FinalGivenAnswer,
        WasCorrect = IsCorrect,
        DurationMs = DurationMs
    };
}
