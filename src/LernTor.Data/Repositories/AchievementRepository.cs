using LernTor.Core.Services;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Freigeschaltete Abzeichen je Profil. Es gibt bewusst kein Entziehen: <see cref="UnlockAsync"/>
/// fügt nur hinzu, und ein Abzeichen, dessen Bedingung später nicht mehr erfüllt ist (etwa ein
/// Thema, das von „gemeistert“ zurückfällt), bleibt stehen.
/// </summary>
public sealed class AchievementRepository
{
    private readonly LernTorDbContext _db;

    public AchievementRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>Freigeschaltete Abzeichen mit Datum, nach Kennung.</summary>
    public async Task<IReadOnlyDictionary<string, DateTimeOffset>> GetUnlockedAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.UnlockedAchievements
            .Where(a => a.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        // Doppelte Zeilen kann der eindeutige Index nicht mehr erzeugen; eine aeltere Datei ohne
        // Index koennte sie haben - dann gilt das frueheste Datum.
        return entities
            .GroupBy(a => a.AchievementId, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.Min(a => a.UnlockedAt), StringComparer.Ordinal);
    }

    /// <summary>
    /// Speichert, was an Abzeichen neu erfüllt ist, und gibt genau diese zurück (für „Neues
    /// Abzeichen!“ auf dem Geschafft-Bildschirm). Bereits gespeicherte bleiben unverändert.
    /// </summary>
    public async Task<IReadOnlyList<Achievement>> UnlockAsync(
        string profileId, AchievementFacts facts, CancellationToken cancellationToken = default)
    {
        var bekannt = (await GetUnlockedAsync(profileId, cancellationToken)).Keys.ToHashSet(StringComparer.Ordinal);
        var neu = AchievementCatalog.NewlyEarned(facts, bekannt);
        if (neu.Count == 0)
        {
            return neu;
        }

        var jetzt = DateTimeOffset.Now;
        foreach (var abzeichen in neu)
        {
            _db.UnlockedAchievements.Add(new UnlockedAchievementEntity
            {
                ProfileId = profileId,
                AchievementId = abzeichen.Id,
                UnlockedAt = jetzt
            });
        }

        await _db.SaveChangesAsync(cancellationToken);
        return neu;
    }
}
