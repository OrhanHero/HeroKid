namespace LernTor.Core.Design;

/// <summary>
/// Ein Design, wie es die Kinder in der Galerie sehen.
/// </summary>
/// <param name="Id">Gespeichert beim Profil - nie umbenennen (wie Abzeichen-Ids).</param>
/// <param name="UnlockAchievementId">Abzeichen, das dieses Design freischaltet; <c>null</c> =
/// von Anfang an frei. Freigeschaltet bleibt freigeschaltet: Abzeichen verfallen nie.</param>
public sealed record DesignTheme(
    string Id,
    string Emoji,
    string NameDe,
    string NameTr,
    string DescriptionDe,
    string DescriptionTr,
    bool IsDark,
    DesignPalette Palette,
    string? UnlockAchievementId = null)
{
    public bool IsFree => UnlockAchievementId is null;
}
