using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Zeile der Lernzeit-Auswertung im Eltern-Bericht: wie viel Zeit ein Fach gekostet hat und
/// was das im Verhältnis zur Trefferquote bedeutet.
/// </summary>
public sealed class SubjectTimeRowViewModel
{
    public SubjectTimeRowViewModel(SubjectTimeStat stat, string subjectLabel)
    {
        Stat = stat;
        SubjectLabel = subjectLabel;
    }

    public SubjectTimeStat Stat { get; }

    public string SubjectLabel { get; }

    /// <summary>Gesamtzeit in der Einheit, in der Eltern denken - Minuten, ab einer Stunde mit Stunde.</summary>
    public string TotalTimeDisplay
    {
        get
        {
            var time = Stat.TotalTime;
            return time.TotalHours >= 1
                ? $"{(int)time.TotalHours} h {time.Minutes} min"
                : $"{(int)time.TotalMinutes} min";
        }
    }

    /// <summary>Typische Dauer je Aufgabe (Median).</summary>
    public string MedianDisplay => $"{Stat.MedianTime.TotalSeconds:0.#} s pro Aufgabe";

    public string AccuracyDisplay => $"{Stat.Accuracy:P0} richtig ({Stat.CorrectCount}/{Stat.Answered})";

    /// <summary>
    /// Die Einordnung im Klartext. Bewusst als ganzer Satz statt als Etikett: "HierHaktEs" sagt
    /// niemandem etwas, "braucht lange und liegt trotzdem oft falsch" schon.
    /// </summary>
    public string EffortDisplay => Stat.Effort switch
    {
        SubjectEffort.SitztSicher => "✅ Geht schnell und sitzt",
        SubjectEffort.VermutlichGeraten => "⚡ Sehr schnell und oft falsch – hier wird vermutlich geraten",
        SubjectEffort.GruendlichAberLangsam => "🐢 Braucht länger, liegt aber richtig – gründlich, kein Problem",
        SubjectEffort.HierHaktEs => "❗ Braucht lange und liegt trotzdem oft falsch – hier hakt es wirklich",
        SubjectEffort.KeineDaten => "– noch zu wenige Aufgaben für eine Aussage",
        _ => "unauffällig"
    };

    /// <summary>Die beiden Fälle, die eine Reaktion nahelegen - der Rest ist reine Information.</summary>
    public bool NeedsAttention =>
        Stat.Effort is SubjectEffort.HierHaktEs or SubjectEffort.VermutlichGeraten;
}
