using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Klausur-Zeile - genutzt vom Willkommensbildschirm (Kind sieht Countdown, darf eigene
/// Einträge löschen) und von der Verwaltungsliste im Eltern-Bereich.
/// </summary>
public sealed class ExamItemViewModel
{
    private readonly DateOnly _today;

    public ExamItemViewModel(ExamEntry exam, DateOnly today)
    {
        Exam = exam;
        _today = today;
    }

    public ExamEntry Exam { get; }

    public string Id => Exam.Id;

    /// <summary>Ob ein eigener Titel eingetragen wurde. Ohne ihn wuerde in der Kind-Ansicht
    /// sonst "Mathematik" doppelt untereinander stehen (Fach und Ersatztitel).</summary>
    public bool HasOwnTitle => !string.IsNullOrWhiteSpace(Exam.Title);

    public string Title => HasOwnTitle ? Exam.Title : $"{Exam.Subject}-Klausur";

    public string SubjectName => Exam.Subject.ToString();

    public string Topics => Exam.Topics;

    public bool HasTopics => !string.IsNullOrWhiteSpace(Exam.Topics);

    /// <summary>"in 3 Tagen", "morgen", "HEUTE" - ein Datum muss man erst umrechnen, und genau
    /// dieses Umrechnen ist der Grund, warum Klausuren überraschen.</summary>
    public string Countdown => Exam.CountdownLabel(_today);

    public string DateLabel => Exam.ExamDate.ToString("dd.MM.yyyy");

    /// <summary>Herkunft im Klartext - im Eltern-Bereich sichtbar, was das Kind selbst gemeldet hat.</summary>
    public string AuthorLabel => Exam.Author == ExamAuthor.Kind ? "selbst eingetragen" : "von den Eltern";

    /// <summary>Steht unmittelbar bevor - Anlass, die Zeile hervorzuheben.</summary>
    public bool IsImminent => Exam.DaysUntil(_today) is >= 0 and <= 2;

    public bool IsPast => Exam.DaysUntil(_today) < 0;

    /// <summary>Ob das Kind diese Zeile löschen darf (nur eigene Einträge).</summary>
    public bool CanChildEdit => Exam.IsEditableByChild;

    /// <summary>Hinweis, dass in diesem Fach gerade mehr geübt wird.</summary>
    public bool IsBoostingLearning => Exam.LearningWeight(_today) > 1.0;
}
