using System.Globalization;
using System.Windows.Data;

namespace LernTor.App.Converters;

/// <summary>MultiBinding-Konverter: values[0] ist ein Format-String ("Hallo, {0}!"), values[1..] die Argumente.</summary>
public sealed class FormatWithArgConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 0 || values[0] is not string format)
        {
            return string.Empty;
        }

        var args = values.Skip(1).ToArray();
        return string.Format(format, args);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
