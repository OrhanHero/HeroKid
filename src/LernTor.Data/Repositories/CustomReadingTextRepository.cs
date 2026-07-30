using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Eigene Lesetexte der Eltern, pro Kind-Profil. Ergänzen den festen Pool aus
/// <c>ReadingContentProvider</c> - siehe <c>ReadingPiece.IsCustom</c> für die Auswahllogik.
/// </summary>
public sealed class CustomReadingTextRepository
{
    private readonly LernTorDbContext _db;

    public CustomReadingTextRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Alle eigenen Texte eines Profils, älteste zuerst - so bleibt die Tagesrotation stabil,
    /// wenn hinten ein Text ergänzt wird. Sortierung in-memory, weil SQLite
    /// <see cref="DateTimeOffset"/> nicht serverseitig ordnen kann.
    /// </summary>
    public async Task<IReadOnlyList<ReadingPiece>> GetForProfileAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.CustomReadingTexts
            .Where(t => t.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities
            .OrderBy(t => t.CreatedAt)
            .Select(ToModel)
            .ToList();
    }

    /// <summary>Rohdaten für die Eltern-Liste (mit Id, damit einzelne Texte löschbar sind).</summary>
    public async Task<IReadOnlyList<CustomReadingTextEntity>> GetEntitiesForProfileAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.CustomReadingTexts
            .Where(t => t.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities.OrderBy(t => t.CreatedAt).ToList();
    }

    public async Task<string> AddAsync(
        string profileId,
        string title,
        string author,
        string textDe,
        string textTr,
        string textEn,
        CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString("N");
        _db.CustomReadingTexts.Add(new CustomReadingTextEntity
        {
            Id = id,
            ProfileId = profileId,
            Title = (title ?? string.Empty).Trim(),
            Author = (author ?? string.Empty).Trim(),
            TextDe = (textDe ?? string.Empty).Trim(),
            TextTr = (textTr ?? string.Empty).Trim(),
            TextEn = (textEn ?? string.Empty).Trim(),
            CreatedAt = DateTimeOffset.Now
        });

        await _db.SaveChangesAsync(cancellationToken);
        return id;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CustomReadingTexts.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.CustomReadingTexts.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static ReadingPiece ToModel(CustomReadingTextEntity entity) => new()
    {
        Title = entity.Title,
        Author = entity.Author,
        TextDe = entity.TextDe,
        TextTr = entity.TextTr,
        TextEn = entity.TextEn,
        IsCustom = true
    };
}
