using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>In welcher Betriebsart ein Fragensatz läuft.</summary>
public enum TheoryRunMode
{
    /// <summary>Üben: nach jeder Frage sofort Auflösung und Erklärung.</summary>
    Uebung,

    /// <summary>Prüfungssimulation: keine Rückmeldung zwischendurch, Auswertung erst am Ende.</summary>
    Pruefung
}

/// <summary>Eine Antwortmöglichkeit einer Theoriefrage.</summary>
public sealed partial class TheoryOptionViewModel : ObservableObject
{
    public TheoryOptionViewModel(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public int Index { get; }

    public string Text { get; }

    /// <summary>Vom Kind angekreuzt.</summary>
    [ObservableProperty]
    private bool isChosen;

    /// <summary>Nach der Auflösung: diese Antwort wäre richtig gewesen.</summary>
    [ObservableProperty]
    private bool isCorrectAnswer;

    /// <summary>Nach der Auflösung: angekreuzt, obwohl falsch.</summary>
    [ObservableProperty]
    private bool isWrongChoice;

    /// <summary>Nach der Auflösung: richtig, aber nicht angekreuzt - der häufigste Fehler bei
    /// Mehrfachantworten.</summary>
    [ObservableProperty]
    private bool isMissed;

    [ObservableProperty]
    private bool isEnabled = true;
}

/// <summary>Wie eine Frage tatsächlich beantwortet wurde - für die Auswertung am Ende.</summary>
/// <param name="Question">Die gestellte Frage in Anzeigereihenfolge.</param>
/// <param name="Chosen">Angekreuzte Positionen.</param>
/// <param name="WasCorrect">Ob vollständig richtig.</param>
public readonly record struct TheoryAnswerRecord(
    PresentedQuestion Question,
    IReadOnlyList<int> Chosen,
    bool WasCorrect);

/// <summary>
/// Ein Satz Theoriefragen, Frage für Frage.
///
/// <para><b>Mehrere Antworten können richtig sein.</b> Bei Mehrfachfragen sind es Kästchen, bei
/// Einfachfragen wirkt dieselbe Liste wie Knöpfe (eine Auswahl schaltet die andere ab). Es steht
/// nicht dran, wie viele richtig sind - das wüsste man in der Prüfung auch nicht, und es ist
/// genau der Punkt, an dem Ein-aus-vier-Übende scheitern.</para>
///
/// <para><b>Üben und Prüfung verhalten sich verschieden</b>, und das mit Absicht: beim Üben kommt
/// die Auflösung samt Erklärung sofort und das Kind muss "Weiter" drücken (dieselbe
/// Anti-Durchklick-Regel wie in den Fächern). In der Prüfungssimulation gibt es zwischendurch
/// keine Rückmeldung, sonst wäre es keine Simulation - die Auswertung kommt am Ende, dort dann
/// vollständig mit allen Erklärungen.</para>
/// </summary>
public sealed partial class TheoryQuestionViewModel : ObservableObject
{
    private readonly IReadOnlyList<PresentedQuestion> _questions;
    private readonly Func<TheoryQuestion, bool, Task> _onAnswered;
    private readonly Action<IReadOnlyList<TheoryAnswerRecord>> _onFinished;
    private readonly Action _onBack;
    private readonly List<TheoryAnswerRecord> _records = new();

    private int _index;

    public TheoryQuestionViewModel(
        IReadOnlyList<PresentedQuestion> questions,
        string title,
        TheoryRunMode mode,
        Func<TheoryQuestion, bool, Task> onAnswered,
        Action<IReadOnlyList<TheoryAnswerRecord>> onFinished,
        Action onBack)
    {
        _questions = questions;
        _onAnswered = onAnswered;
        _onFinished = onFinished;
        _onBack = onBack;

        Title = title;
        Mode = mode;

        LoadCurrent();
    }

    public string Title { get; }

    public TheoryRunMode Mode { get; }

    /// <summary>In der Prüfung gibt es zwischendurch keine Auflösung.</summary>
    public bool ShowsFeedback => Mode == TheoryRunMode.Uebung;

    public int TotalQuestions => _questions.Count;

    public ObservableCollection<TheoryOptionViewModel> Options { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PositionDisplay))]
    private int position = 1;

    [ObservableProperty]
    private string prompt = string.Empty;

    /// <summary>Das Verkehrszeichen zur Frage, falls eines dazugehört.</summary>
    [ObservableProperty]
    private TrafficSign? sign;

    [ObservableProperty]
    private bool isMultipleChoice;

    [ObservableProperty]
    private bool isAnswered;

    [ObservableProperty]
    private bool wasCorrect;

    [ObservableProperty]
    private string feedback = string.Empty;

    /// <summary>Die Begründung - erscheint erst nach der Antwort, nie vorher.</summary>
    [ObservableProperty]
    private string explanation = string.Empty;

    [ObservableProperty]
    private string points = string.Empty;

    public string PositionDisplay => $"{Position} / {TotalQuestions}";

    /// <summary>
    /// Antworten darf man erst, wenn überhaupt etwas angekreuzt ist. Eine leere Abgabe wäre
    /// sonst der bequemste Weg durch dreißig Fragen.
    /// </summary>
    public bool CanAnswer => !IsAnswered && Options.Any(option => option.IsChosen);

    private PresentedQuestion Current => _questions[_index];

    private void LoadCurrent()
    {
        if (_index >= _questions.Count)
        {
            _onFinished(_records);
            return;
        }

        var frage = Current;

        Position = _index + 1;
        Prompt = frage.Question.Prompt;
        IsMultipleChoice = frage.IsMultipleChoice;
        IsAnswered = false;
        Feedback = string.Empty;
        Explanation = string.Empty;
        Points = string.Format(LocalizationService.Instance["Fs_PointsWeight"], frage.Question.Points);

        Sign = frage.Question.SignNumber is { } nummer
            ? TrafficSignCatalog.ByNumber(nummer)
            : null;

        Options.Clear();
        for (var i = 0; i < frage.Options.Count; i++)
        {
            Options.Add(new TheoryOptionViewModel(i, frage.Options[i]));
        }

        OnPropertyChanged(nameof(CanAnswer));
    }

    /// <summary>
    /// Kreuzt eine Antwort an. Bei Einfachfragen ersetzt die neue Wahl die alte - dieselbe Liste
    /// verhält sich dann wie eine Knopfreihe, ohne dass es dafür eine zweite Ansicht braucht.
    /// </summary>
    [RelayCommand]
    private void Toggle(TheoryOptionViewModel? option)
    {
        if (option is null || IsAnswered)
        {
            return;
        }

        if (IsMultipleChoice)
        {
            option.IsChosen = !option.IsChosen;
        }
        else
        {
            foreach (var eintrag in Options)
            {
                eintrag.IsChosen = ReferenceEquals(eintrag, option);
            }
        }

        OnPropertyChanged(nameof(CanAnswer));
    }

    [RelayCommand]
    private async Task AnswerAsync()
    {
        if (IsAnswered || _index >= _questions.Count)
        {
            return;
        }

        var gewaehlt = Options.Where(option => option.IsChosen).Select(option => option.Index).ToList();

        if (gewaehlt.Count == 0)
        {
            return;
        }

        var frage = Current;
        var richtig = frage.IsCorrect(gewaehlt);

        _records.Add(new TheoryAnswerRecord(frage, gewaehlt, richtig));

        IsAnswered = true;
        WasCorrect = richtig;
        OnPropertyChanged(nameof(CanAnswer));

        await _onAnswered(frage.Question, richtig);

        if (!ShowsFeedback)
        {
            // Prüfung: keine Auflösung, direkt zur nächsten Frage.
            Advance();
            return;
        }

        Feedback = richtig
            ? LocalizationService.Instance["Fs_Correct"]
            : LocalizationService.Instance["Fs_TheoryWrong"];
        Explanation = frage.Question.Explanation;

        foreach (var eintrag in Options)
        {
            var istRichtig = frage.CorrectIndices.Contains(eintrag.Index);

            eintrag.IsEnabled = false;
            eintrag.IsCorrectAnswer = istRichtig;
            eintrag.IsWrongChoice = eintrag.IsChosen && !istRichtig;
            eintrag.IsMissed = istRichtig && !eintrag.IsChosen;
        }
    }

    [RelayCommand]
    private void Next()
    {
        if (!IsAnswered)
        {
            return;
        }

        Advance();
    }

    private void Advance()
    {
        _index++;
        LoadCurrent();
    }

    [RelayCommand]
    private void Back() => _onBack();
}
