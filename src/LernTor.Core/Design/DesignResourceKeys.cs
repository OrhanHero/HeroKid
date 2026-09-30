namespace LernTor.Core.Design;

/// <summary>
/// Unter welchen Schlüsseln die App die Farbrollen als WPF-Ressourcen anlegt. Die Schlüssel sind
/// die alten Namen aus <c>Colors.xaml</c> (<c>CardBrush</c> für die Fläche usw.), damit keine
/// Ansicht umbenannt werden musste. Liegt in Core, damit ein Unit-Test prüfen kann, dass
/// <c>Colors.xaml</c> jeden Schlüssel mit dem Wert des Standard-Designs enthält.
/// </summary>
public static class DesignResourceKeys
{
    public sealed record Keys(string Role, string ColorKey, string BrushKey);

    public const string FontFamilyKey = "AppFontFamily";

    public static IReadOnlyList<Keys> All { get; } = new[]
    {
        new Keys("Primary", "PrimaryColor", "PrimaryBrush"),
        new Keys("PrimaryDark", "PrimaryDarkColor", "PrimaryDarkBrush"),
        new Keys("Accent", "AccentColor", "AccentBrush"),
        new Keys("Success", "SuccessColor", "SuccessBrush"),
        new Keys("Error", "ErrorColor", "ErrorBrush"),
        new Keys("Background", "BackgroundColor", "BackgroundBrush"),
        new Keys("Surface", "CardColor", "CardBrush"),
        new Keys("TextPrimary", "TextPrimaryColor", "TextPrimaryBrush"),
        new Keys("TextSecondary", "TextSecondaryColor", "TextSecondaryBrush"),
        new Keys("OnColor", "OnColorColor", "OnColorBrush"),
        new Keys("Focus", "FocusColor", "FocusBrush"),
        new Keys("Math", "MathColor", "MathBrush"),
        new Keys("German", "GermanColor", "GermanBrush"),
        new Keys("Turkish", "TurkishColor", "TurkishBrush"),
        new Keys("Science", "ScienceColor", "ScienceBrush"),
        new Keys("News", "NewsColor", "NewsBrush"),
        new Keys("TileLavender", "TileLavenderColor", "TileLavenderBrush"),
        new Keys("TileSand", "TileSandColor", "TileSandBrush"),
        new Keys("TileMint", "TileMintColor", "TileMintBrush"),
        new Keys("TileRose", "TileRoseColor", "TileRoseBrush"),
        new Keys("ProgressTrack", "ProgressTrackColor", "ProgressTrackBrush"),
        new Keys("InputBorder", "InputBorderColor", "InputBorderBrush"),
    };
}
