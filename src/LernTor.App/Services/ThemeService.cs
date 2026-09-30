using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using LernTor.Core.Design;
using Microsoft.Win32;

namespace LernTor.App.Services;

/// <summary>
/// Legt ein Design als Ressourcen über die App (docs/DESIGN.md). Das Design-Wörterbuch wird als
/// LETZTES in <c>Application.Resources.MergedDictionaries</c> eingehängt - WPF sucht dort von
/// hinten nach vorn, also gewinnt es gegen <c>Colors.xaml</c>. Weil die Ansichten die Pinsel per
/// <c>DynamicResource</c> holen, wechseln auch offene Ansichten sofort die Farbe.
///
/// <para>Die Textgröße ist eine Skalierung des ganzen Inhalts (MainWindow bindet daran), statt
/// 505 einzelner Schriftgrößen.</para>
/// </summary>
public sealed class ThemeService : INotifyPropertyChanged
{
    public static ThemeService Instance { get; } = new();

    private ResourceDictionary? _aktiv;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DesignTheme CurrentTheme { get; private set; } = DesignThemeCatalog.Default;

    public DesignFont CurrentFont { get; private set; } = DesignFont.Standard;

    /// <summary>1.0, 1.1 oder 1.2 - Skalierungsfaktor des Fensterinhalts.</summary>
    public double TextScale { get; private set; } = 1.0;

    /// <summary>Wendet die Einstellungen eines Kindes an (Uhrzeit, Windows-Modus und Abzeichen
    /// entscheiden mit, siehe <see cref="DesignSelection"/>).</summary>
    public void ApplyPreferences(DesignPreferences preferences, IReadOnlySet<string> unlockedAchievementIds)
    {
        var theme = DesignSelection.ResolveTheme(preferences, DateTime.Now, WindowsUsesDarkMode(), unlockedAchievementIds);
        var skalierung = DesignSelection.TextScale(preferences.TextScalePercent);

        // Nichts geändert (häufig: bei jedem Etappenwechsel wird neu entschieden) - dann auch
        // nichts tauschen, sonst liefe jedes Mal eine Neuauflösung aller Ressourcen.
        if (_aktiv is not null && theme == CurrentTheme && preferences.Font == CurrentFont && skalierung == TextScale)
        {
            return;
        }

        Apply(theme, preferences.Font, skalierung);
    }

    /// <summary>Zurück zum Standard - für die Profilwahl, die allen Kindern gehört.</summary>
    public void ApplyDefault() => Apply(DesignThemeCatalog.Default, DesignFont.Standard, 1.0);

    public void Apply(DesignTheme theme, DesignFont font, double textScale, ResourceDictionary? target = null)
    {
        var resources = target ?? Application.Current?.Resources;
        if (resources is null)
        {
            return;
        }

        var neu = BuildResources(theme.Palette, font);
        if (_aktiv is not null)
        {
            resources.MergedDictionaries.Remove(_aktiv);
        }

        resources.MergedDictionaries.Add(neu);
        _aktiv = neu;

        CurrentTheme = theme;
        CurrentFont = font;
        TextScale = textScale;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTheme)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFont)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextScale)));
    }

    /// <summary>Farben und Pinsel unter den Schlüsseln aus <see cref="DesignResourceKeys"/>.</summary>
    public static ResourceDictionary BuildResources(DesignPalette palette, DesignFont font)
    {
        var rollen = palette.Roles;
        var dict = new ResourceDictionary();
        foreach (var keys in DesignResourceKeys.All)
        {
            var farbe = ToColor(rollen[keys.Role]);
            var pinsel = new SolidColorBrush(farbe);
            pinsel.Freeze();
            dict[keys.ColorKey] = farbe;
            dict[keys.BrushKey] = pinsel;
        }

        dict[DesignResourceKeys.FontFamilyKey] = new FontFamily(DesignFonts.FamilyName(font));
        return dict;
    }

    public static Color ToColor(string hex)
    {
        var (r, g, b) = ColorContrast.Parse(hex);
        return Color.FromRgb(r, g, b);
    }

    /// <summary>
    /// Ob Windows für Apps den dunklen Modus eingestellt hat (Einstellungen → Personalisierung →
    /// Farben). Fehlt der Wert oder ist die Registry gesperrt, gilt hell.
    /// </summary>
    public static bool WindowsUsesDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return key?.GetValue("AppsUseLightTheme") is int hell && hell == 0;
        }
        catch
        {
            return false;
        }
    }
}
