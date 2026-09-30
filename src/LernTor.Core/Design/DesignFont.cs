namespace LernTor.Core.Design;

/// <summary>
/// Schriftwahl. Nur Schriften, die jedes Windows 10/11 mitbringt - kein Download, keine Lizenz.
/// Als Text gespeichert (nie Zahlen - siehe JsonOptions.Default in CLAUDE.md).
/// </summary>
public enum DesignFont
{
    /// <summary>Segoe UI - die Windows-Schrift, bisheriges Aussehen.</summary>
    Standard,

    /// <summary>Verdana - breite Buchstaben, großer Abstand; gut lesbar auch bei Leseschwäche.</summary>
    GutLesbar,

    /// <summary>Comic Sans MS - handschriftnah, verspielt.</summary>
    Verspielt
}

public static class DesignFonts
{
    public static string FamilyName(DesignFont font) => font switch
    {
        DesignFont.GutLesbar => "Verdana",
        DesignFont.Verspielt => "Comic Sans MS",
        _ => "Segoe UI"
    };
}
