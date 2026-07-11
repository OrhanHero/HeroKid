using System.Globalization;
using System.Windows.Data;

namespace LernTor.App.Converters;

/// <summary>
/// Vergleicht zwei gebundene Werte (z.B. das Avatar-Emoji eines Picker-Buttons mit dem aktuell
/// gewählten Avatar) und liefert "Selected" bei Gleichheit, sonst einen leeren String - als
/// Button.Tag gebunden steuert das den Auswahl-Rahmen im AvatarPickButton-Style, ohne dass die
/// simple Emoji-Reihe eine ganze ListBox mit Selektionslogik braucht.
/// </summary>
public sealed class EqualsToSelectedTagConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture) =>
        values.Length == 2 && Equals(values[0], values[1]) ? "Selected" : string.Empty;

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
