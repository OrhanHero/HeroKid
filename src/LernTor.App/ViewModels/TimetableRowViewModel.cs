using CommunityToolkit.Mvvm.ComponentModel;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Zeile des Stundenplan-Rasters im Eltern-Bereich: eine Schulstunde mit ihrer Uhrzeit und
/// den fünf Wochentagen.
///
/// <para>Die fünf Tage sind bewusst fünf einzelne Eigenschaften und keine Liste: WPF bindet
/// TextBoxen an Namen, und eine Liste hätte einen Index-Konverter gebraucht, nur um dasselbe
/// zu erreichen.</para>
/// </summary>
public sealed partial class TimetableRowViewModel : ObservableObject
{
    public TimetableRowViewModel(int period)
    {
        Period = period;
    }

    public int Period { get; }

    /// <summary>"3." - so steht die Stunde auch auf dem Plan der Schule.</summary>
    public string PeriodDisplay => $"{Period}.";

    /// <summary>Beginn als "HH:mm". Freies Textfeld: eine Uhrzeit einzutippen geht schneller,
    /// als sie in zwei Drehfeldern zusammenzuklicken.</summary>
    [ObservableProperty]
    private string startText = string.Empty;

    [ObservableProperty]
    private string endText = string.Empty;

    [ObservableProperty]
    private string monday = string.Empty;

    [ObservableProperty]
    private string tuesday = string.Empty;

    [ObservableProperty]
    private string wednesday = string.Empty;

    [ObservableProperty]
    private string thursday = string.Empty;

    [ObservableProperty]
    private string friday = string.Empty;

    public string TextFor(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => Monday,
        DayOfWeek.Tuesday => Tuesday,
        DayOfWeek.Wednesday => Wednesday,
        DayOfWeek.Thursday => Thursday,
        DayOfWeek.Friday => Friday,
        _ => string.Empty
    };

    public void SetText(DayOfWeek day, string value)
    {
        switch (day)
        {
            case DayOfWeek.Monday:
                Monday = value;
                break;
            case DayOfWeek.Tuesday:
                Tuesday = value;
                break;
            case DayOfWeek.Wednesday:
                Wednesday = value;
                break;
            case DayOfWeek.Thursday:
                Thursday = value;
                break;
            case DayOfWeek.Friday:
                Friday = value;
                break;
        }
    }

    /// <summary>Leert die fünf Fächer-Felder, lässt die Uhrzeiten aber stehen - das Zeitraster
    /// der Schule ändert sich nicht, wenn ein neuer Stundenplan kommt.</summary>
    public void ClearSubjects()
    {
        Monday = string.Empty;
        Tuesday = string.Empty;
        Wednesday = string.Empty;
        Thursday = string.Empty;
        Friday = string.Empty;
    }
}
