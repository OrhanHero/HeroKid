using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.Core.Design;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// „🎨 Mein Design“ (docs/NAECHSTES-LEVEL-3.md, Schritt 4): das Kind wählt Design, Schrift und
/// Textgröße und schaltet die Automatik („wie Windows“, „abends dunkel“). Jede Änderung wirkt
/// sofort - <paramref name="onChanged"/> speichert und wendet an - und bleibt auf diesem
/// Bildschirm sichtbar, damit das Kind vergleichen kann.
///
/// <para>Die Vorschaukarten zeigen die Farben IHRES Designs, nicht die des gerade aktiven:
/// deshalb eigene Pinsel aus der Palette statt DynamicResource.</para>
/// </summary>
public sealed partial class DesignPickerViewModel : ObservableObject
{
    private readonly Action<DesignPreferences> _onChanged;
    private readonly Action _onBack;

    public DesignPickerViewModel(
        string profileName,
        DesignPreferences current,
        IReadOnlySet<string> unlockedAchievementIds,
        Action<DesignPreferences> onChanged,
        Action onBack)
    {
        ProfileName = profileName;
        _onChanged = onChanged;
        _onBack = onBack;
        _current = current;

        var tuerkisch = LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch;
        foreach (var theme in DesignThemeCatalog.All)
        {
            Themes.Add(new DesignCardViewModel(theme, DesignSelection.IsUnlocked(theme, unlockedAchievementIds), tuerkisch));
        }

        Fonts = Enum.GetValues<DesignFont>()
            .Select(schrift => new DesignOptionViewModel(
                schrift.ToString(),
                LocalizationService.Instance[$"Design_Font_{schrift}"],
                DesignFonts.FamilyName(schrift)))
            .ToList();

        TextScales = DesignPreferences.TextScales
            .Select(prozent => new DesignOptionViewModel(prozent.ToString(), $"{prozent} %", DesignFonts.FamilyName(DesignFont.Standard)))
            .ToList();

        MarkSelection();
    }

    public string ProfileName { get; }

    public ObservableCollection<DesignCardViewModel> Themes { get; } = new();

    public IReadOnlyList<DesignOptionViewModel> Fonts { get; }

    public IReadOnlyList<DesignOptionViewModel> TextScales { get; }

    private DesignPreferences _current;

    public DesignPreferences Current
    {
        get => _current;
        private set => SetProperty(ref _current, value);
    }

    public bool FollowWindows
    {
        get => Current.FollowWindows;
        set => Change(Current with { FollowWindows = value });
    }

    public bool DarkInEvening
    {
        get => Current.DarkInEvening;
        set => Change(Current with { DarkInEvening = value });
    }

    [RelayCommand]
    private void SelectTheme(DesignCardViewModel? card)
    {
        if (card is null || card.IsLocked)
        {
            return;
        }

        Change(Current with { ThemeId = card.Id });
    }

    [RelayCommand]
    private void SelectFont(DesignOptionViewModel? option)
    {
        if (option is not null && Enum.TryParse<DesignFont>(option.Key, out var schrift))
        {
            Change(Current with { Font = schrift });
        }
    }

    [RelayCommand]
    private void SelectTextScale(DesignOptionViewModel? option)
    {
        if (option is not null && int.TryParse(option.Key, out var prozent))
        {
            Change(Current with { TextScalePercent = prozent });
        }
    }

    [RelayCommand]
    private void Back() => _onBack();

    private void Change(DesignPreferences neu)
    {
        if (neu == Current)
        {
            return;
        }

        Current = neu;
        OnPropertyChanged(nameof(FollowWindows));
        OnPropertyChanged(nameof(DarkInEvening));
        MarkSelection();
        _onChanged(neu);
    }

    private void MarkSelection()
    {
        foreach (var card in Themes)
        {
            card.IsSelected = card.Id == Current.ThemeId;
        }

        foreach (var option in Fonts)
        {
            option.IsSelected = option.Key == Current.Font.ToString();
        }

        foreach (var option in TextScales)
        {
            option.IsSelected = option.Key == Current.TextScalePercent.ToString();
        }
    }
}

/// <summary>Eine Karte in der Design-Galerie: Name, Beschreibung und eine kleine Vorschau in
/// den Farben dieses Designs.</summary>
public sealed partial class DesignCardViewModel : ObservableObject
{
    public DesignCardViewModel(DesignTheme theme, bool unlocked, bool tuerkisch)
    {
        Id = theme.Id;
        Emoji = theme.Emoji;
        Name = tuerkisch ? theme.NameTr : theme.NameDe;
        Description = tuerkisch ? theme.DescriptionTr : theme.DescriptionDe;
        IsLocked = !unlocked;

        var p = theme.Palette;
        PreviewBackground = Pinsel(p.Background);
        PreviewSurface = Pinsel(p.Surface);
        PreviewText = Pinsel(p.TextPrimary);
        PreviewTextSecondary = Pinsel(p.TextSecondary);
        PreviewPrimary = Pinsel(p.Primary);
        PreviewOnColor = Pinsel(p.OnColor);
        PreviewAccent = Pinsel(p.Accent);
        PreviewSuccess = Pinsel(p.Success);

        if (IsLocked)
        {
            var abzeichen = AchievementCatalog.All.FirstOrDefault(a => a.Id == theme.UnlockAchievementId);
            var titel = abzeichen is null ? string.Empty : $"{abzeichen.Emoji} {(tuerkisch ? abzeichen.TitleTr : abzeichen.TitleDe)}";
            LockHint = string.Format(LocalizationService.Instance["Design_LockedHint"], titel);
        }
    }

    public string Id { get; }

    public string Emoji { get; }

    public string Name { get; }

    public string Description { get; }

    public bool IsLocked { get; }

    public string LockHint { get; } = string.Empty;

    public Brush PreviewBackground { get; }

    public Brush PreviewSurface { get; }

    public Brush PreviewText { get; }

    public Brush PreviewTextSecondary { get; }

    public Brush PreviewPrimary { get; }

    public Brush PreviewOnColor { get; }

    public Brush PreviewAccent { get; }

    public Brush PreviewSuccess { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectionTag))]
    private bool isSelected;

    /// <summary>"Selected" für die Button-Stile mit Tag-Trigger (wie TabPillButton).</summary>
    public string SelectionTag => IsSelected ? "Selected" : string.Empty;

    private static Brush Pinsel(string hex)
    {
        var pinsel = new SolidColorBrush(ThemeService.ToColor(hex));
        pinsel.Freeze();
        return pinsel;
    }
}

/// <summary>Eine Wahlmöglichkeit (Schrift oder Textgröße) als Pillen-Knopf.</summary>
public sealed partial class DesignOptionViewModel : ObservableObject
{
    public DesignOptionViewModel(string key, string label, string fontFamily)
    {
        Key = key;
        Label = label;
        FontFamily = new FontFamily(fontFamily);
    }

    public string Key { get; }

    public string Label { get; }

    /// <summary>Die Schrift, in der die Beschriftung erscheint - bei der Schriftwahl die
    /// jeweilige Schrift selbst, damit das Kind sie vergleichen kann.</summary>
    public FontFamily FontFamily { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectionTag))]
    private bool isSelected;

    public string SelectionTag => IsSelected ? "Selected" : string.Empty;
}
