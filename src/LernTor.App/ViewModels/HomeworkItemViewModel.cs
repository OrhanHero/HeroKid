using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Hausaufgaben-Zeile - genutzt vom Willkommensbildschirm (Kind hakt ab) und von der
/// Verwaltungsliste im Eltern-Bereich.
/// </summary>
public sealed partial class HomeworkItemViewModel : ObservableObject
{
    private readonly Action<HomeworkItemViewModel>? _onCompletedChanged;
    private readonly DateOnly _today;

    public HomeworkItemViewModel(
        HomeworkTask task,
        DateOnly today,
        Action<HomeworkItemViewModel>? onCompletedChanged = null)
    {
        Task = task;
        _today = today;
        _onCompletedChanged = onCompletedChanged;
        isCompleted = task.IsCompleted;
    }

    public HomeworkTask Task { get; }

    public string Id => Task.Id;

    public string Description => Task.Description;

    /// <summary>Fach als Klartext - die Übersetzung liegt im SubjectToTitleConverter, hier reicht der Name.</summary>
    public string SubjectName => Task.Subject.ToString();

    /// <summary>
    /// Stichtag in der Sprache, in der Familien darüber reden: "heute", "morgen", "seit 2 Tagen
    /// überfällig" - ein nacktes Datum muss man erst umrechnen.
    /// </summary>
    public string DueLabel
    {
        get
        {
            var days = Task.DueDate.DayNumber - _today.DayNumber;
            return days switch
            {
                0 => "heute fällig",
                1 => "morgen fällig",
                -1 => "seit gestern überfällig",
                < -1 => $"seit {-days} Tagen überfällig",
                _ => $"in {days} Tagen fällig ({Task.DueDate:dd.MM.})"
            };
        }
    }

    public bool IsOverdue => Task.IsOverdue(_today);

    public bool IsDueToday => Task.IsDueToday(_today);

    /// <summary>Ob das Kind diese Zeile loeschen darf (nur selbst eingetragene).</summary>
    public bool CanChildEdit => Task.IsEditableByChild;

    /// <summary>Herkunft als Klartext - im Eltern-Bereich sichtbar, was das Kind selbst eingetragen hat.</summary>
    public string AuthorLabel => Task.Author == LernTor.Core.Models.EntryAuthor.Kind
        ? "selbst eingetragen"
        : "von den Eltern";

    /// <summary>Abgehakt. Änderungen melden sich sofort an das Repository - eine Hausaufgabe,
    /// die man abhakt und die beim nächsten Start wieder offen ist, wäre schlimmer als keine.</summary>
    [ObservableProperty]
    private bool isCompleted;

    partial void OnIsCompletedChanged(bool value)
    {
        Task.CompletedAt = value ? DateTimeOffset.Now : null;
        _onCompletedChanged?.Invoke(this);
        OnPropertyChanged(nameof(IsOverdue));
        OnPropertyChanged(nameof(IsDueToday));
    }
}
