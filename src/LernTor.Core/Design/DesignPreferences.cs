namespace LernTor.Core.Design;

/// <summary>
/// Was ein Kind in der Design-Galerie eingestellt hat. Gespeichert beim Profil
/// (<c>StudentProfileRepository.SetDesignAsync</c>) - bewusst nicht in <c>ProfileSettings</c>,
/// weil das Kind es selbst einstellt und der Eltern-Bereich es nicht bei jedem Speichern
/// überschreiben darf.
/// </summary>
/// <param name="TextScalePercent">100, 110 oder 120 - siehe <see cref="TextScales"/>.</param>
/// <param name="FollowWindows">Dunkles Design, wenn Windows auf dunkel steht.</param>
/// <param name="DarkInEvening">Dunkles Design von 19 bis 6 Uhr.</param>
public sealed record DesignPreferences(
    string ThemeId,
    DesignFont Font,
    int TextScalePercent,
    bool FollowWindows,
    bool DarkInEvening)
{
    public static DesignPreferences Default { get; } =
        new(DesignThemeCatalog.DefaultId, DesignFont.Standard, 100, FollowWindows: false, DarkInEvening: false);

    /// <summary>Mehr als 120 % passt auf 1366 × 768 nicht mehr sicher (Bildschirmfotos in der CI).</summary>
    public static IReadOnlyList<int> TextScales { get; } = new[] { 100, 110, 120 };
}

/// <summary>
/// Entscheidet, welches Design gerade gilt. Reine Funktion (Uhrzeit, Windows-Modus und
/// Abzeichen werden hereingegeben), damit sie ohne Windows testbar ist.
/// </summary>
public static class DesignSelection
{
    public static readonly TimeSpan EveningStart = TimeSpan.FromHours(19);
    public static readonly TimeSpan MorningEnd = TimeSpan.FromHours(6);

    public static bool IsUnlocked(DesignTheme theme, IReadOnlySet<string> unlockedAchievementIds) =>
        theme.IsFree || unlockedAchievementIds.Contains(theme.UnlockAchievementId!);

    public static bool IsEvening(DateTime now) =>
        now.TimeOfDay >= EveningStart || now.TimeOfDay < MorningEnd;

    /// <summary>
    /// Das Design, das angezeigt wird. Ein gesperrtes Design (etwa nach dem Zurückspielen einer
    /// Sicherung ohne das Abzeichen) fällt auf Lavendel zurück. Ist das gewählte Design schon
    /// dunkel, bleibt es auch abends - wer "Galaxie" gewählt hat, soll nicht "Nacht" bekommen.
    /// </summary>
    public static DesignTheme ResolveTheme(
        DesignPreferences preferences, DateTime now, bool windowsIsDark, IReadOnlySet<string> unlockedAchievementIds)
    {
        var gewaehlt = DesignThemeCatalog.Find(preferences.ThemeId);
        if (!IsUnlocked(gewaehlt, unlockedAchievementIds))
        {
            gewaehlt = DesignThemeCatalog.Default;
        }

        if (gewaehlt.IsDark)
        {
            return gewaehlt;
        }

        if ((preferences.FollowWindows && windowsIsDark) || (preferences.DarkInEvening && IsEvening(now)))
        {
            return DesignThemeCatalog.Find(DesignThemeCatalog.DarkId);
        }

        return gewaehlt;
    }

    /// <summary>Skalierungsfaktor; unbekannte Werte werden zu 100 %.</summary>
    public static double TextScale(int percent) =>
        DesignPreferences.TextScales.Contains(percent) ? percent / 100.0 : 1.0;
}
