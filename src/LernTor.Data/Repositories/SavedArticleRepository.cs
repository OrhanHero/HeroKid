using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Gemerkte News-Artikel (🔖 Lesezeichen, pro Profil) - siehe <see cref="SavedArticleEntity"/>.
/// </summary>
public sealed class SavedArticleRepository
{
    private readonly LernTorDbContext _db;

    public SavedArticleRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>Neueste zuerst. Sortierung in-memory: EF Core/SQLite kann ORDER BY auf
    /// DateTimeOffset-Spalten nicht serverseitig übersetzen.</summary>
    public async Task<IReadOnlyList<SavedArticleEntity>> GetForProfileAsync(string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.SavedArticles
            .Where(a => a.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        return entities.OrderByDescending(a => a.SavedAt).ToList();
    }

    /// <summary>Merkt den Artikel bzw. entfernt das Lesezeichen wieder; liefert den neuen
    /// Zustand (true = jetzt gemerkt).</summary>
    public async Task<bool> ToggleAsync(string profileId, NewsArticle article, CancellationToken cancellationToken = default)
    {
        var existing = await _db.SavedArticles.FirstOrDefaultAsync(
            a => a.ProfileId == profileId && a.ArticleId == article.Id, cancellationToken);

        if (existing is not null)
        {
            _db.SavedArticles.Remove(existing);
            await _db.SaveChangesAsync(cancellationToken);
            return false;
        }

        _db.SavedArticles.Add(new SavedArticleEntity
        {
            ProfileId = profileId,
            ArticleId = article.Id,
            Title = article.Title,
            SimplifiedSummary = article.SimplifiedSummary,
            SourceName = article.SourceName,
            SourceUrl = article.SourceUrl,
            CategoryEmoji = article.CategoryEmoji,
            Category = article.Category.ToString(),
            BerlinDistrict = article.BerlinDistrict,
            PublishedAt = article.PublishedAt,
            SavedAt = DateTimeOffset.Now
        });

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.SavedArticles.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (entity is not null)
        {
            _db.SavedArticles.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
