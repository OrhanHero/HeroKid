using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Belohnungen und Einlösungen (siehe <see cref="RewardEntity"/>/<see cref="RewardRedemptionEntity"/>).
/// </summary>
public sealed class RewardRepository
{
    private readonly LernTorDbContext _db;

    public RewardRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>Alle Belohnungen, günstigste zuerst (so sieht das Kind zuerst, was schon erreichbar ist).</summary>
    public async Task<IReadOnlyList<RewardEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Rewards.OrderBy(r => r.StarCost).ThenBy(r => r.Id).ToListAsync(cancellationToken);

    public async Task<RewardEntity> AddAsync(string emoji, string title, int starCost, CancellationToken cancellationToken = default)
    {
        var entity = new RewardEntity
        {
            Emoji = string.IsNullOrWhiteSpace(emoji) ? "🎁" : emoji.Trim(),
            Title = title.Trim(),
            StarCost = Math.Max(1, starCost),
            CreatedAt = DateTimeOffset.Now
        };

        _db.Rewards.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(int rewardId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Rewards.FirstOrDefaultAsync(r => r.Id == rewardId, cancellationToken);
        if (entity is not null)
        {
            _db.Rewards.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Löst eine Belohnung für ein Profil ein: prüft den Sterne-Stand, zieht die Kosten ab und
    /// protokolliert die Einlösung - alles in EINEM SaveChanges, damit nie Sterne ohne Eintrag
    /// (oder umgekehrt) abgebucht werden. Liefert (false, alterStand) bei zu wenig Sternen.
    /// </summary>
    public async Task<(bool Success, int NewTotalStars)> RedeemAsync(string profileId, int rewardId, CancellationToken cancellationToken = default)
    {
        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        var reward = await _db.Rewards.FirstOrDefaultAsync(r => r.Id == rewardId, cancellationToken);

        if (profile is null || reward is null || profile.TotalStars < reward.StarCost)
        {
            return (false, profile?.TotalStars ?? 0);
        }

        profile.TotalStars -= reward.StarCost;
        _db.RewardRedemptions.Add(new RewardRedemptionEntity
        {
            ProfileId = profileId,
            RewardTitle = reward.Title,
            RewardEmoji = reward.Emoji,
            StarCost = reward.StarCost,
            RedeemedAt = DateTimeOffset.Now
        });

        await _db.SaveChangesAsync(cancellationToken);
        return (true, profile.TotalStars);
    }

    /// <summary>Einlösungen eines Profils, neueste zuerst (Sortierung in-memory wegen
    /// DateTimeOffset, wie überall).</summary>
    public async Task<IReadOnlyList<RewardRedemptionEntity>> GetRedemptionsAsync(string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.RewardRedemptions
            .Where(r => r.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities.OrderByDescending(r => r.RedeemedAt).ToList();
    }
}
