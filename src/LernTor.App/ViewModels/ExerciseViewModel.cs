using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Generisches Übungs-ViewModel für jedes Fach (Mathematik, Deutsch, Türkisch, Englisch,
/// Biologie, Chemie, Physik, Gewi, Politik, Geo, Ethik, ITG): zeigt eine Frage nach der
/// anderen mit Erklärung, bis der Fachbereich abgeschlossen ist.
///
/// <para><b>Mindest-Lernzeit pro Aufgabe:</b> "Weiter" wird erst frei, wenn die Frage
/// beantwortet ist UND ein kurzer Countdown abgelaufen ist. Ohne diese Bremse wurde in der
/// Praxis wild durchgeklickt (irgendeine Antwort + sofort Weiter), nur um schnell zum
/// Abschlussquiz zu kommen - so bleibt genug Zeit, Frage und Erklärung wirklich zu lesen.
/// Das Abschlussquiz selbst bekommt bewusst keinen Countdown: dort bestraft sich Raten von
/// selbst, weil unter 50 % der PC gesperrt bleibt.</para>
/// </summary>
public sealed partial class ExerciseViewModel : ObservableObject
{
    /// <summary>Mindestzeit pro Aufgabe in Sekunden - grob die Zeit, um eine kurze Frage samt
    /// Erklärung tatsächlich zu lesen.</summary>
    private const int MinSecondsPerQuestion = 5;

    private readonly IReadOnlyList<QuizQuestion> _questions;
    private readonly Action<Subject, QuestionOutcome, QuizQuestion> _onQuestionAnswered;
    private readonly Action _onSubjectCompleted;
    private readonly IHomeworkHelpChatService _homeworkChat;
    private readonly DispatcherTimer _minTimeTimer;

    private bool _currentAnswered;

    [ObservableProperty]
    private int currentIndex;

    [ObservableProperty]
    private QuestionAnswerViewModel? currentQuestion;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoNext))]
    [NotifyPropertyChangedFor(nameof(ShowLockCountdown))]
    [NotifyCanExecuteChangedFor(nameof(NextCommand))]
    private int lockSecondsRemaining;

    public Subject Subject { get; }
    public int TotalQuestions => _questions.Count;
    public int DisplayIndex => CurrentIndex + 1;
    public bool IsLastQuestion => CurrentIndex >= _questions.Count - 1;

    /// <summary>"Weiter" erst nach Antwort UND abgelaufener Mindest-Lernzeit.</summary>
    public bool CanGoNext => _currentAnswered && LockSecondsRemaining <= 0;

    /// <summary>Countdown-Hinweis nur zeigen, solange die Mindestzeit noch läuft.</summary>
    public bool ShowLockCountdown => LockSecondsRemaining > 0;

    public ExerciseViewModel(
        Subject subject,
        IReadOnlyList<QuizQuestion> questions,
        Action<Subject, QuestionOutcome, QuizQuestion> onQuestionAnswered,
        Action onSubjectCompleted,
        IHomeworkHelpChatService homeworkChat)
    {
        Subject = subject;
        _questions = questions;
        _onQuestionAnswered = onQuestionAnswered;
        _onSubjectCompleted = onSubjectCompleted;
        _homeworkChat = homeworkChat;

        _minTimeTimer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _minTimeTimer.Tick += (_, _) => Tick();

        LoadCurrent();
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

    private void LoadCurrent()
    {
        if (CurrentIndex >= _questions.Count)
        {
            _minTimeTimer.Stop();
            _onSubjectCompleted();
            return;
        }

        _currentAnswered = false;
        LockSecondsRemaining = MinSecondsPerQuestion;
        _minTimeTimer.Start();

        CurrentQuestion = new QuestionAnswerViewModel(_questions[CurrentIndex], _homeworkChat, OnAnswered);
        OnPropertyChanged(nameof(DisplayIndex));
        OnPropertyChanged(nameof(IsLastQuestion));
        OnPropertyChanged(nameof(CanGoNext));
    }

    private void OnAnswered(QuestionAnswerViewModel answered)
    {
        _onQuestionAnswered(Subject, answered.ToOutcome(), answered.Question);
        _currentAnswered = true;
        OnPropertyChanged(nameof(CanGoNext));
        NextCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanGoNext))]
    private void Next()
    {
        CurrentIndex++;
        LoadCurrent();
    }
}
