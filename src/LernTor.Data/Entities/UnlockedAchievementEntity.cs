namespace LernTor.Data.Entities;

/// <summary>
/// Ein freigeschaltetes Abzeichen (siehe <c>AchievementCatalog</c>). Einmal gespeichert, wird es
/// nie wieder gelöscht - außer mit dem ganzen Profil oder beim Zurücksetzen. Das Datum steht
/// in „Mein Fortschritt“ unter dem Abzeichen.
/// </summary>
public sealed class UnlockedAchievementEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;

    /// <summary>Stabile Kennung aus dem Katalog, z. B. "richtig-100".</summary>
    public string AchievementId { get; set; } = string.Empty;

    public DateTimeOffset UnlockedAt { get; set; }
}
