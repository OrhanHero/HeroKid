using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Ein Abzeichen in „Mein Fortschritt“ bzw. auf dem Geschafft-Bildschirm. Texte in der gerade
/// eingestellten Sprache - der Katalog führt beide (<see cref="AchievementCatalog"/>).
/// </summary>
public sealed class AchievementRowViewModel
{
    public AchievementRowViewModel(Achievement achievement, DateTimeOffset? unlockedAt, DateOnly today)
    {
        Achievement = achievement;
        UnlockedAt = unlockedAt;
        var tuerkisch = LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch;
        Title = tuerkisch ? achievement.TitleTr : achievement.TitleDe;
        HowTo = tuerkisch ? achievement.HowToTr : achievement.HowToDe;
        IsNew = unlockedAt is { } am && DateOnly.FromDateTime(am.LocalDateTime) == today;
        DetailDisplay = unlockedAt is { } datum
            ? string.Format(LocalizationService.Instance["Badges_UnlockedOn"], datum.LocalDateTime.ToString("dd.MM.yyyy"))
            : HowTo;
    }

    public Achievement Achievement { get; }

    public DateTimeOffset? UnlockedAt { get; }

    public string Emoji => Achievement.Emoji;

    public string Title { get; }

    public string HowTo { get; }

    public bool IsUnlocked => UnlockedAt is not null;

    /// <summary>Heute dazugekommen - wird hervorgehoben.</summary>
    public bool IsNew { get; }

    /// <summary>„am 29.09.2026“ für freigeschaltete, sonst wie man es bekommt.</summary>
    public string DetailDisplay { get; }

    /// <summary>
    /// Die anzuzeigende Liste: freigeschaltete zuerst (neueste oben), dann die offenen in
    /// Katalogreihenfolge. Offene Abzeichen eines Bereichs, den die Eltern für dieses Kind
    /// abgeschaltet haben, fehlen - ein Ziel, das man nicht erreichen darf, ist keins. Schon
    /// verdiente bleiben aber sichtbar, auch wenn der Bereich später abgeschaltet wird.
    /// </summary>
    public static IReadOnlyList<AchievementRowViewModel> BuildList(
        IReadOnlyDictionary<string, DateTimeOffset> unlocked,
        Func<Subject, bool> isAreaAvailable,
        DateOnly today)
    {
        var freigeschaltet = AchievementCatalog.All
            .Where(a => unlocked.ContainsKey(a.Id))
            .OrderByDescending(a => unlocked[a.Id])
            .Select(a => new AchievementRowViewModel(a, unlocked[a.Id], today));

        var offen = AchievementCatalog.All
            .Where(a => !unlocked.ContainsKey(a.Id))
            .Where(a => a.Area is not { } bereich || isAreaAvailable(bereich))
            .Select(a => new AchievementRowViewModel(a, null, today));

        return freigeschaltet.Concat(offen).ToList();
    }
}
