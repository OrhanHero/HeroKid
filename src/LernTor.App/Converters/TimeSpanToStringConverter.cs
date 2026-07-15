using System;
using System.Globalization;
using System.Windows.Data;

namespace LernTor.App.Converters;

/// <summary>Konvertiert TimeSpan zu formatiertem String (mm:ss).</summary>
public sealed class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
        {
            return ts.ToString(@"mm\:ss");
        }
        return "00:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}