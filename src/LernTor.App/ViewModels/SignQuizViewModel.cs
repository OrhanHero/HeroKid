using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Eine Antwortmöglichkeit im Zeichen-Quiz.</summary>
public sealed partial class SignOptionViewModel : ObservableObject
{
    public SignOptionViewModel(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public int Index { get; }

    public string Text { get; }

    /// <summary>Nach der Antwort: diese Option war die richtige.</summary>
    [ObservableProperty]
    private bool isCorrectAnswer;

    /// <summary>Nach der Antwort: diese Option hat das Kind gewählt (und sie war falsch).</summary>
    [ObservableProperty]
    private bool isWrongChoice;

    [ObservableProperty]
    private bool isEnabled = true;
}

/// <summary>
/// Zeichen-Quiz: Bild zeigen, aus vier Namen den richtigen wählen.
///
/// <para><b>Nach einer falschen Antwort geht es nicht sofort weiter.</b> Die richtige Antwort
/// wird markiert und die Bedeutung eingeblendet, das Kind muss "Weiter" drücken. Dieselbe
/// Anti-Durchklick-Regel wie in den Fächern: wer nur klickt, lernt nichts, und ein Quiz, das
/// den Fehler sofort wegblendet, ist reine Beschäftigung.</para>
///
/// <para>Wird für zwei Zwecke benutzt: die tägliche Challenge (feste fünf Zeichen des Tages)
/// und das Üben einer einzelnen Gruppe. Der Unterschied liegt nur in der Frageliste.</para>
/// </summary>
public sealed partial class SignQuizViewModel : ObservableObject
{
    private readonly IReadOnlyList<SignQuizQuestion> _questions;
    private readonly Func<TrafficSign, bool, Task> _onAnswered;
    private readonly Action<int, int> _onFinished;
    private readonly Action _onBack;

    private int _index;
    private int _correctCount;

    public SignQuizViewModel(
        IReadOnlyList<SignQuizQuestion> questions,
        string title,
        Func<TrafficSign, bool, Task> onAnswered,
        Action<int, int> onFinished,
        Action onBack)
    {
        _questions = questions;
        _onAnswered = onAnswered;
        _onFinished = onFinished;
        _onBack = onBack;
        Title = title;

        LoadCurrent();
    }

    public string Title { get; }

    public int TotalQuestions => _questions.Count;

    [ObservableProperty]
    private TrafficSign? currentSign;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PositionDisplay))]
    private int position = 1;

    [ObservableProperty]
    private bool isAnswered;

    [ObservableProperty]
    private bool wasCorrect;

    [ObservableProperty]
    private string feedback = string.Empty;

    /// <summary>Bedeutung und Merksatz - erscheinen erst NACH der Antwort, sonst wären sie die Lösung.</summary>
    [ObservableProperty]
    private string meaning = string.Empty;

    [ObservableProperty]
    private string hint = string.Empty;

    [ObservableProperty]
    private bool isFinished;

    public ObservableCollection<SignOptionViewModel> Options { get; } = new();

    public string PositionDisplay => $"{Position} / {TotalQuestions}";

    public string ResultDisplay => $"{_correctCount} / {TotalQuestions}";

    private void LoadCurrent()
    {
        if (_index >= _questions.Count)
        {
            IsFinished = true;
            _onFinished(_correctCount, TotalQuestions);
            return;
        }

        var frage = _questions[_index];

        CurrentSign = frage.Sign;
        Position = _index + 1;
        IsAnswered = false;
        Feedback = string.Empty;
        Meaning = string.Empty;
        Hint = string.Empty;

        Options.Clear();
        for (var i = 0; i < frage.Options.Count; i++)
        {
            Options.Add(new SignOptionViewModel(i, frage.Options[i]));
        }
    }

    [RelayCommand]
    private async Task ChooseAsync(SignOptionViewModel? option)
    {
        if (option is null || IsAnswered || _index >= _questions.Count)
        {
            return;
        }

        var frage = _questions[_index];
        var richtig = frage.IsCorrect(option.Index);

        IsAnswered = true;
        WasCorrect = richtig;

        if (richtig)
        {
            _correctCount++;
            Feedback = LocalizationService.Instance["Fs_Correct"];
        }
        else
        {
            Feedback = string.Format(LocalizationService.Instance["Fs_Wrong"], frage.CorrectAnswer);
        }

        Meaning = frage.Sign.Meaning;
        Hint = frage.Sign.Hint;

        foreach (var eintrag in Options)
        {
            eintrag.IsEnabled = false;
            eintrag.IsCorrectAnswer = eintrag.Index == frage.CorrectIndex;
            eintrag.IsWrongChoice = !richtig && eintrag.Index == option.Index;
        }

        await _onAnswered(frage.Sign, richtig);
    }

    [RelayCommand]
    private void Next()
    {
        if (!IsAnswered)
        {
            return;
        }

        _index++;
        LoadCurrent();
    }

    [RelayCommand]
    private void Back() => _onBack();
}
