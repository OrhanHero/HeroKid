using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>Eine Zeile der Ferien-/Feiertagsliste im Eltern-Bereich.</summary>
public sealed class CalendarRowViewModel
{
    public CalendarRowViewModel(SchoolCalendarEntry entry, DateOnly today)
    {
        Name = entry.Name;
        IsRunning = entry.Contains(today);
        IsHoliday = entry.Kind == CalendarEntryKind.Feiertag;

        DateDisplay = entry.IsSingleDay
            ? entry.Start.ToString("dd.MM.yyyy")
            : $"{entry.Start:dd.MM.yyyy} - {entry.End:dd.MM.yyyy}";

        if (IsRunning)
        {
            var uebrig = entry.RemainingDays(today);
            Note = uebrig == 1 ? "laeuft, letzter Tag" : $"laeuft, noch {uebrig} Tage";
        }
        else
        {
            var tage = entry.DaysUntilStart(today);
            Note = tage == 1 ? "morgen" : $"in {tage} Tagen";

            // Ein Feiertag am Wochenende bringt keinen freien Tag - dazusagen statt
            // verschweigen. Bewusst ohne Wochentagsnamen: "dddd" haengt an der Kultur des
            // laufenden Threads und stuende auf einem englischen Windows als "Saturday" da.
            if (entry.FallsOnWeekend)
            {
                Note += " (am Wochenende)";
            }
        }
    }

    public string Name { get; }

    public string DateDisplay { get; }

    public string Note { get; }

    public bool IsRunning { get; }

    public bool IsHoliday { get; }
}
