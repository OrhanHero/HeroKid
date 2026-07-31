using CommunityToolkit.Mvvm.ComponentModel;

namespace LernTor.App.ViewModels;

/// <summary>
/// Ein-/Ausschalter für eine Nachrichtenquelle im Eltern-Bereich. Änderungen greifen sofort
/// (werden direkt gespeichert) - eine Einstellung, die erst beim Schließen des Fensters wirkt,
/// lädt zum Vergessen ein.
/// </summary>
public sealed partial class NewsFeedToggle : ObservableObject
{
    private readonly Action<NewsFeedToggle> _onChanged;

    public NewsFeedToggle(
        string name,
        string description,
        bool isEnabled,
        Action<NewsFeedToggle> onChanged,
        string healthLabel = "",
        bool isUnhealthy = false)
    {
        Name = name;
        Description = description;
        this.isEnabled = isEnabled;
        _onChanged = onChanged;
        HealthLabel = healthLabel;
        IsUnhealthy = isUnhealthy;
    }

    /// <summary>Zustand beim letzten tatsaechlichen Abruf, als Klartext (siehe FeedHealthLog).</summary>
    public string HealthLabel { get; }

    /// <summary>Ob der letzte Abruf fehlschlug - faerbt die Zeile im Eltern-Bereich.</summary>
    public bool IsUnhealthy { get; }

    public bool HasHealthLabel => HealthLabel.Length > 0;

    /// <summary>Name der Quelle - dient zugleich als Schlüssel in AppSettings.DisabledNewsFeeds.</summary>
    public string Name { get; }

    /// <summary>Region und Sprache als Klartext, damit Eltern die Quelle einordnen können.</summary>
    public string Description { get; }

    [ObservableProperty]
    private bool isEnabled;

    /// <summary>
    /// Setzt den Schalter, ohne den Einzel-Callback auszuloesen - fuer "alle an/aus", das sonst
    /// je Zeile einmal speichern wuerde.
    /// </summary>
    public void SetEnabledSilently(bool value)
    {
        // Das Backing-Field ist hier genau der Punkt: ueber die generierte Eigenschaft zu gehen
        // wuerde OnIsEnabledChanged und damit den Einzel-Callback ausloesen - "alle an/aus" wuerde
        // dann je Zeile einmal speichern. Deshalb MVVMTK0034 bewusst unterdrueckt.
#pragma warning disable MVVMTK0034
        if (isEnabled == value) return;
        isEnabled = value;
#pragma warning restore MVVMTK0034
        OnPropertyChanged(nameof(IsEnabled));
    }

    partial void OnIsEnabledChanged(bool value) => _onChanged(this);
}
