using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Zeile der Stundenplan-Kachel auf der Startseite: "3. 🔢 Mathe 9:55 – 10:40".
///
/// <para>Eigene Datei, nicht ans Ende einer anderen ViewModel-Datei gehaengt - genau so ist in
/// dieser Codebasis schon einmal eine Methode in der falschen (weil letzten) Klasse gelandet
/// (siehe CLAUDE.md).</para>
/// </summary>
public sealed class TimetableLessonViewModel
{
    public TimetableLessonViewModel(TimetableLesson lesson, TimetablePeriod? period, bool isCurrent, bool isNext)
    {
        Period = lesson.Period;
        Subject = lesson.Subject;
        Details = lesson.Details;
        Icon = TimetableSubjectMap.IconFor(lesson.Subject);
        // Ohne Raster-Eintrag bleibt die Zeitspalte leer statt eine erfundene Zeit zu zeigen.
        Time = period?.Range ?? string.Empty;
        IsCurrent = isCurrent;
        IsNext = isNext;
    }

    public int Period { get; }

    /// <summary>"3." - die Stundennummer, wie sie auf dem Plan steht.</summary>
    public string PeriodDisplay => $"{Period}.";

    /// <summary>Das Fach, so wie es die Eltern eingetragen haben - also so, wie es auf dem
    /// Stundenplan der Schule steht.</summary>
    public string Subject { get; }

    /// <summary>Lehrkraft und/oder Raum, sofern eingetragen.</summary>
    public string Details { get; }

    public bool HasDetails => Details.Length > 0;

    public string Icon { get; }

    /// <summary>"9:55 – 10:40" aus dem Zeitraster der jeweiligen Schule.</summary>
    public string Time { get; }

    /// <summary>Diese Stunde laeuft gerade - nur an einem heutigen Schultag.</summary>
    public bool IsCurrent { get; }

    /// <summary>Die naechste Stunde, die heute noch kommt.</summary>
    public bool IsNext { get; }
}
