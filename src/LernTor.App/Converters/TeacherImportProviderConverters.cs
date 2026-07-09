using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

/// <summary>
/// Für die Provider-Auswahl (RadioButtons) im Eltern-Bereich: vergleicht den gebundenen
/// <see cref="TeacherImportProvider"/>-Wert mit dem als ConverterParameter übergebenen Enum-Namen
/// (z.B. "NotebookLm"/"LocalLlm") und liefert/parst ihn als Bool für RadioButton.IsChecked.
/// </summary>
public sealed class TeacherImportProviderToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is TeacherImportProvider provider && parameter is string name && provider.ToString() == name;

    public object? ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true && parameter is string name && Enum.TryParse<TeacherImportProvider>(name, out var provider)
            ? provider
            : Binding.DoNothing;
}

/// <summary>Wie <see cref="TeacherImportProviderToBooleanConverter"/>, aber für Sichtbarkeit von
/// provider-spezifischen Konfigurationsbereichen (nur der aktive Anbieter zeigt seine Felder).</summary>
public sealed class TeacherImportProviderToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is TeacherImportProvider provider && parameter is string name && provider.ToString() == name
            ? Visibility.Visible
            : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
