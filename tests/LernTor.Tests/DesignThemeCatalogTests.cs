using System.Text.RegularExpressions;
using LernTor.Core.Design;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Designs (docs/NAECHSTES-LEVEL-3.md, Schritt 1-3): jedes Design muss die Kontrastregeln
/// nach WCAG 2.2 erfüllen - ein unlesbares Design kommt nicht durch die CI.
/// </summary>
public sealed class DesignThemeCatalogTests
{
    public static TheoryData<string> DesignIds
    {
        get
        {
            var daten = new TheoryData<string>();
            foreach (var theme in DesignThemeCatalog.All)
            {
                daten.Add(theme.Id);
            }

            return daten;
        }
    }

    [Theory]
    [MemberData(nameof(DesignIds))]
    public void Design_erfuellt_die_Kontrastregeln(string id)
    {
        var verstoesse = DesignContrastRules.Violations(DesignThemeCatalog.Find(id).Palette);

        Assert.True(verstoesse.Count == 0, $"{id}:\n" + string.Join("\n", verstoesse));
    }

    [Theory]
    [MemberData(nameof(DesignIds))]
    public void Hell_oder_dunkel_passt_zum_Hintergrund(string id)
    {
        var theme = DesignThemeCatalog.Find(id);
        var dunkel = ColorContrast.RelativeLuminance(theme.Palette.Background) < 0.2;

        Assert.Equal(dunkel, theme.IsDark);
    }

    [Fact]
    public void Katalog_hat_zehn_Designs_mit_eindeutigen_Ids_und_Namen()
    {
        Assert.Equal(10, DesignThemeCatalog.All.Count);

        // Drei davon sind Belohnungen (3.0: Galaxie, 3.1: Gletscher und Vulkan).
        Assert.Equal(new[] { "galaxie", "gletscher", "vulkan" },
            DesignThemeCatalog.All.Where(t => !t.IsFree).Select(t => t.Id));
        Assert.Equal(DesignThemeCatalog.All.Count, DesignThemeCatalog.All.Select(t => t.Id).Distinct().Count());
        Assert.All(DesignThemeCatalog.All, t =>
        {
            Assert.Matches("^[a-z]+$", t.Id);
            Assert.False(string.IsNullOrWhiteSpace(t.NameDe));
            Assert.False(string.IsNullOrWhiteSpace(t.NameTr));
            Assert.False(string.IsNullOrWhiteSpace(t.DescriptionDe));
            Assert.False(string.IsNullOrWhiteSpace(t.DescriptionTr));
        });
        Assert.Equal("lavendel", DesignThemeCatalog.Default.Id);
        Assert.True(DesignThemeCatalog.Find(DesignThemeCatalog.DarkId).IsDark);
    }

    [Fact]
    public void Unbekannte_Id_faellt_auf_Lavendel_zurueck()
    {
        Assert.Same(DesignThemeCatalog.Default, DesignThemeCatalog.Find("gibt-es-nicht"));
        Assert.Same(DesignThemeCatalog.Default, DesignThemeCatalog.Find(null));
    }

    [Fact]
    public void Freischalt_Abzeichen_gibt_es_wirklich()
    {
        var abzeichen = AchievementCatalog.All.Select(a => a.Id).ToHashSet();

        Assert.All(DesignThemeCatalog.All.Where(t => !t.IsFree),
            t => Assert.Contains(t.UnlockAchievementId!, abzeichen));
        Assert.Contains(DesignThemeCatalog.All, t => !t.IsFree);
    }

    /// <summary>
    /// Colors.xaml ist der Stand beim Start, bevor ThemeService etwas anlegt - er muss genau
    /// dem Standard-Design entsprechen, sonst "springt" die Farbe beim ersten Umschalten.
    /// </summary>
    [Fact]
    public void Colors_xaml_entspricht_dem_Standard_Design()
    {
        var pfad = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "src", "LernTor.App", "Resources", "Colors.xaml"));
        var xaml = File.ReadAllText(pfad);
        var rollen = DesignThemeCatalog.Default.Palette.Roles;

        foreach (var keys in DesignResourceKeys.All)
        {
            var m = Regex.Match(xaml, $"<Color x:Key=\"{keys.ColorKey}\">(#[0-9A-Fa-f]{{6}})</Color>");
            Assert.True(m.Success, $"{keys.ColorKey} fehlt in Colors.xaml");
            Assert.Equal(rollen[keys.Role], m.Groups[1].Value, ignoreCase: true);
            Assert.Contains($"<SolidColorBrush x:Key=\"{keys.BrushKey}\" Color=\"{{StaticResource {keys.ColorKey}}}\" />", xaml);
        }

        Assert.Contains($"<FontFamily x:Key=\"{DesignResourceKeys.FontFamilyKey}\">Segoe UI</FontFamily>", xaml);
    }

    [Fact]
    public void Jede_Rolle_hat_einen_Ressourcenschluessel()
    {
        Assert.Equal(
            DesignThemeCatalog.Default.Palette.Roles.Keys.OrderBy(k => k),
            DesignResourceKeys.All.Select(k => k.Role).OrderBy(k => k));
    }

    [Theory]
    [InlineData("#FFFFFF", "#000000", 21.0)]
    [InlineData("#000000", "#FFFFFF", 21.0)]
    [InlineData("#777777", "#777777", 1.0)]
    [InlineData("#767676", "#FFFFFF", 4.54)]   // das bekannte "gerade noch AA"-Grau
    public void Kontrast_rechnet_wie_WCAG(string a, string b, double erwartet) =>
        Assert.Equal(erwartet, ColorContrast.Ratio(a, b), 2);
}

public sealed class DesignSelectionTests
{
    private static readonly IReadOnlySet<string> KeineAbzeichen = new HashSet<string>();
    private static readonly DateTime Mittag = new(2026, 10, 5, 12, 0, 0);
    private static readonly DateTime Abend = new(2026, 10, 5, 20, 30, 0);
    private static readonly DateTime FruehMorgens = new(2026, 10, 5, 5, 30, 0);

    private static DesignPreferences Wahl(string id, bool windows = false, bool abends = false) =>
        DesignPreferences.Default with { ThemeId = id, FollowWindows = windows, DarkInEvening = abends };

    [Fact]
    public void Gewaehltes_Design_gilt()
    {
        Assert.Equal("ozean", DesignSelection.ResolveTheme(Wahl("ozean"), Mittag, windowsIsDark: false, KeineAbzeichen).Id);
    }

    [Theory]
    [InlineData(20, 30, "nacht")]
    [InlineData(5, 30, "nacht")]
    [InlineData(12, 0, "wald")]
    [InlineData(19, 0, "nacht")]
    [InlineData(6, 0, "wald")]
    public void Abends_automatisch_dunkel(int stunde, int minute, string erwartet)
    {
        var zeit = new DateTime(2026, 10, 5, stunde, minute, 0);

        Assert.Equal(erwartet, DesignSelection.ResolveTheme(Wahl("wald", abends: true), zeit, false, KeineAbzeichen).Id);
    }

    [Fact]
    public void Ohne_Schalter_bleibt_es_abends_hell()
    {
        Assert.Equal("wald", DesignSelection.ResolveTheme(Wahl("wald"), Abend, false, KeineAbzeichen).Id);
    }

    [Fact]
    public void Wie_Windows_folgt_dem_dunklen_Modus()
    {
        Assert.Equal("nacht", DesignSelection.ResolveTheme(Wahl("bonbon", windows: true), Mittag, windowsIsDark: true, KeineAbzeichen).Id);
        Assert.Equal("bonbon", DesignSelection.ResolveTheme(Wahl("bonbon", windows: true), Mittag, windowsIsDark: false, KeineAbzeichen).Id);
    }

    [Fact]
    public void Ein_dunkles_Design_bleibt_auch_abends()
    {
        var abzeichen = new HashSet<string> { "lerntage-10" };

        Assert.Equal("galaxie", DesignSelection.ResolveTheme(Wahl("galaxie", windows: true, abends: true), FruehMorgens, true, abzeichen).Id);
    }

    [Fact]
    public void Gesperrtes_Design_faellt_auf_Lavendel_zurueck()
    {
        Assert.Equal("lavendel", DesignSelection.ResolveTheme(Wahl("galaxie"), Mittag, false, KeineAbzeichen).Id);
        Assert.Equal("galaxie", DesignSelection.ResolveTheme(Wahl("galaxie"), Mittag, false, new HashSet<string> { "lerntage-10" }).Id);
    }

    [Theory]
    [InlineData(100, 1.0)]
    [InlineData(110, 1.1)]
    [InlineData(120, 1.2)]
    [InlineData(150, 1.0)]
    [InlineData(0, 1.0)]
    public void Textgroesse_nur_in_erlaubten_Stufen(int prozent, double faktor) =>
        Assert.Equal(faktor, DesignSelection.TextScale(prozent), 3);

    [Theory]
    [InlineData(DesignFont.Standard, "Segoe UI")]
    [InlineData(DesignFont.GutLesbar, "Verdana")]
    [InlineData(DesignFont.Verspielt, "Comic Sans MS")]
    public void Schriften_sind_Windows_Schriften(DesignFont schrift, string name) =>
        Assert.Equal(name, DesignFonts.FamilyName(schrift));
}
