using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Eine falsch beantwortete Frage in der Nachbesprechung.</summary>
public sealed class TheoryMistakeViewModel
{
    public TheoryMistakeViewModel(TheoryAnswerRecord eintrag)
    {
        var frage = eintrag.Question;

        Prompt = frage.Question.Prompt;
        Explanation = frage.Question.Explanation;
        Topic = DrivingTheoryCatalog.TopicLabel(frage.Question.Topic);
        Points = frage.Question.Points;
        Sign = frage.Question.SignNumber is { } nummer ? TrafficSignCatalog.ByNumber(nummer) : null;

        CorrectAnswers = frage.CorrectIndices.Select(i => frage.Options[i]).ToList();
        ChosenAnswers = eintrag.Chosen.OrderBy(i => i).Select(i => frage.Options[i]).ToList();
    }

    public string Prompt { get; }

    public string Explanation { get; }

    public string Topic { get; }

    public int Points { get; }

    public TrafficSign? Sign { get; }

    public IReadOnlyList<string> CorrectAnswers { get; }

    public IReadOnlyList<string> ChosenAnswers { get; }

    public string CorrectDisplay => string.Join("  •  ", CorrectAnswers);

    public string ChosenDisplay => string.Join("  •  ", ChosenAnswers);

    public string PointsDisplay =>
        string.Format(LocalizationService.Instance["Fs_PointsWeight"], Points);
}

/// <summary>
/// Auswertung einer Prüfungssimulation.
///
/// <para><b>Die Zahl, die zählt, sind Fehlerpunkte - nicht Prozent.</b> Deshalb steht sie oben
/// und groß, und daneben die Grenze. Wer 28 von 30 richtig hat, aber zwei Fünf-Punkte-Fragen
/// verhauen hat, ist durchgefallen; eine Prozentanzeige würde das verschleiern.</para>
///
/// <para>Darunter kommt jede falsche Frage einzeln, mit der eigenen Antwort, der richtigen und
/// der Begründung. Das ist der eigentliche Ertrag des Durchlaufs - die Punktzahl allein bringt
/// niemanden weiter.</para>
/// </summary>
public sealed partial class TheoryResultViewModel : ObservableObject
{
    private readonly Action _onRepeat;
    private readonly Action _onBack;

    public TheoryResultViewModel(
        TheoryExamResult result,
        IReadOnlyList<TheoryAnswerRecord> records,
        Action onRepeat,
        Action onBack)
    {
        Result = result;
        _onRepeat = onRepeat;
        _onBack = onBack;

        foreach (var eintrag in records.Where(antwort => !antwort.WasCorrect))
        {
            Mistakes.Add(new TheoryMistakeViewModel(eintrag));
        }
    }

    public TheoryExamResult Result { get; }

    public ObservableCollection<TheoryMistakeViewModel> Mistakes { get; } = new();

    public bool Passed => Result.Passed;

    public bool HasMistakes => Mistakes.Count > 0;

    public string Headline => Passed
        ? LocalizationService.Instance["Fs_ExamPassed"]
        : LocalizationService.Instance["Fs_ExamFailed"];

    public string PointsDisplay => string.Format(
        LocalizationService.Instance["Fs_ExamPoints"],
        Result.WrongPoints, TheoryExamRules.MaxWrongPoints);

    public string CountDisplay => string.Format(
        LocalizationService.Instance["Fs_ExamCount"],
        Result.CorrectCount, Result.Questions);

    /// <summary>Leer, wenn bestanden - dann steht dort nichts statt einer beruhigenden Floskel.</summary>
    public string Reason => Result.FailureReason;

    public bool HasReason => !string.IsNullOrEmpty(Reason);

    [RelayCommand]
    private void Repeat() => _onRepeat();

    [RelayCommand]
    private void Back() => _onBack();
}
