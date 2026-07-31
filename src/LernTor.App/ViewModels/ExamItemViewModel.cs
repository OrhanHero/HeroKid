using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Klausur-Zeile - genutzt vom Willkommensbildschirm (Kind sieht Countdown, darf eigene
/// Einträge löschen) und von der Verwaltungsliste im Eltern-Bereich.
/// </summary>
public sealed class ExamItemViewModel
{
    private readonly DateOnly _today;

    /// <param name="subjectLabel">Uebersetzte Fachbezeichnung fuer den Lernplan-Satz. Ohne
    /// Angabe der Enum-Name - lesbar genug, und ein leerer Standardwert haette im Satz eine
    /// Luecke hinterlassen ("mehr -Aufgaben").</param>
    public ExamItemViewModel(ExamEntry exam, DateOnly today, string? subjectLabel = null)
    {
        Exam = exam;
        _today = today;
        SubjectLabel = string.IsNullOrWhiteSpace(subjectLabel) ? exam.Subject.ToString() : subjectLabel;
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
    public string AuthorLabel => Exam.Author == EntryAuthor.Kind ? "selbst eingetragen" : "von den Eltern";

    /// <summary>Steht unmittelbar bevor - Anlass, die Zeile hervorzuheben.</summary>
    public bool IsImminent => Exam.DaysUntil(_today) is >= 0 and <= 2;

    public bool IsPast => Exam.DaysUntil(_today) < 0;

    /// <summary>Ob das Kind diese Zeile löschen darf (nur eigene Einträge).</summary>
    public bool CanChildEdit => Exam.IsEditableByChild;

    /// <summary>Hinweis, dass in diesem Fach gerade mehr geübt wird.</summary>
    public bool IsBoostingLearning => Exam.LearningWeight(_today) > 1.0;

    /// <summary>
    /// Was heute wegen dieser Klausur dran ist - der Lernplan in einem Satz (siehe
    /// <see cref="ExamStudyPlanner"/>). Bisher erhoehte die Gewichtung still die Aufgabenzahl;
    /// das Kind sah nur mehr Mathe, ohne den Zusammenhang. Leer, solange die Klausur weiter weg
    /// ist als der Vorlauf - dann gibt es auch nichts zu planen.
    /// </summary>
    public string TodayPlan => ExamStudyPlanner.TodayHint(Exam, _today, SubjectLabel);

    public bool HasTodayPlan => TodayPlan.Length > 0;

    /// <summary>Das Fach so, wie es im Lernplan-Satz steht (siehe Konstruktor).</summary>
    public string SubjectLabel { get; }
}
