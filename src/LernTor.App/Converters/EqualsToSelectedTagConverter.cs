using System.Globalization;
using System.Windows.Data;

namespace LernTor.App.Converters;

/// <summary>
/// Vergleicht zwei gebundene Werte (z.B. das Avatar-Emoji eines Picker-Buttons mit dem aktuell
/// gewählten Avatar, oder eine Tab-/Zeitraum-Kennung mit der aktiven) und liefert "Selected" bei
/// Gleichheit, sonst einen leeren String - als Button.Tag gebunden steuert das den Auswahl-Zustand
/// in AvatarPickButton/TabPillButton, ohne dass eine ganze ListBox mit Selektionslogik nötig wäre.
/// Verglichen wird über ToString(), damit auch XAML-String-Literale gegen int-Properties
/// funktionieren (z.B. "7" gegen ReportDays=7).
/// </summary>
public sealed class EqualsToSelectedTagConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture) =>
        values.Length == 2 && string.Equals(values[0]?.ToString(), values[1]?.ToString(), StringComparison.Ordinal)
            ? "Selected"
            : string.Empty;

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
