using System.ComponentModel;
using LernTor.Core.Enums;

namespace LernTor.App.Localization;

/// <summary>
/// Einfacher, laufzeitumschaltbarer Übersetzungsdienst (Deutsch/Türkisch). XAML bindet über den
/// Indexer, z.B. Text="{Binding Source={x:Static loc:LocalizationService.Instance}, Path=[Welcome_Title]}".
/// Beim Sprachwechsel wird "Item[]" invalidiert, wodurch WPF alle Indexer-Bindings neu auswertet.
/// </summary>
public sealed class LocalizationService : INotifyPropertyChanged
{
    public static LocalizationService Instance { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    private AppLanguage _currentLanguage = AppLanguage.Deutsch;

    public AppLanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage == value)
            {
                return;
            }

            _currentLanguage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
    }

    public string this[string key]
    {
        get
        {
            if (Translations.Map.TryGetValue(key, out var perLanguage) &&
                perLanguage.TryGetValue(CurrentLanguage, out var text))
            {
                return text;
            }

            return $"[{key}]";
        }
    }
}
