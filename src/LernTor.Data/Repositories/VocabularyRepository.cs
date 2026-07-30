using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Vokabelpaare je Kind-Profil samt eigener Wiederholungs-Steuerung. Die Intervalle kommen aus
/// <see cref="SpacedRepetitionSchedule"/> - dieselben 7/30/90 Tage wie bei gemeisterten Aufgaben,
/// damit sich das Lernen für das Kind einheitlich anfühlt.
/// </summary>
public sealed class VocabularyRepository
{
    private readonly LernTorDbContext _db;

    public VocabularyRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Heute fällige Vokabeln eines Fachs: noch nie gemeisterte zuerst, danach die am häufigsten
    /// falsch beantworteten. Fälligkeit wird in-memory geprüft, weil SQLite
    /// <see cref="DateTimeOffset"/> nicht serverseitig vergleichen kann.
    /// </summary>
    public async Task<IReadOnlyList<VocabularyEntry>> GetDueAsync(
        string profileId, Subject subject, int maxCount, CancellationToken cancellationToken = default)
    {
        if (maxCount <= 0)
        {
            return Array.Empty<VocabularyEntry>();
        }

        var subjectName = subject.ToString();
        var entities = await _db.VocabularyEntries
            .Where(v => v.ProfileId == profileId && v.Subject == subjectName)
            .ToListAsync(cancellationToken);

        var now = DateTimeOffset.Now;
        return entities
            .Where(v => v.NextDueAt is null || v.NextDueAt <= now)
            .OrderBy(v => v.Stage)
            .ThenByDescending(v => v.WrongCount)
            .ThenBy(v => v.CreatedAt)
            .Take(maxCount)
            .Select(ToModel)
            .ToList();
    }

    /// <summary>Alle Vokabeln eines Profils (für die Liste im Eltern-Bereich), älteste zuerst.</summary>
    public async Task<IReadOnlyList<VocabularyEntryEntity>> GetAllForProfileAsync(
        string profileId, Subject? subject = null, CancellationToken cancellationToken = default)
    {
        var query = _db.VocabularyEntries.Where(v => v.ProfileId == profileId);
        if (subject is not null)
        {
            var subjectName = subject.Value.ToString();
            query = query.Where(v => v.Subject == subjectName);
        }

        var entities = await query.ToListAsync(cancellationToken);
        return entities.OrderBy(v => v.CreatedAt).ToList();
    }

    /// <summary>
    /// Fügt eingelesene Wortpaare hinzu. Bereits vorhandene deutsche Wörter desselben Fachs werden
    /// aktualisiert statt gedoppelt - Eltern fügen eine korrigierte Liste sonst ein zweites Mal ein
    /// und haben danach jede Vokabel zweimal. Gibt zurück, wie viele neu waren.
    /// </summary>
    public async Task<int> AddOrUpdateManyAsync(
        string profileId,
        Subject subject,
        IEnumerable<VocabularyParser.Pair> pairs,
        CancellationToken cancellationToken = default)
    {
        var subjectName = subject.ToString();
        var existing = await _db.VocabularyEntries
            .Where(v => v.ProfileId == profileId && v.Subject == subjectName)
            .ToListAsync(cancellationToken);

        int added = 0;
        foreach (var pair in pairs)
        {
            var match = existing.FirstOrDefault(
                v => string.Equals(v.German, pair.German, StringComparison.OrdinalIgnoreCase));

            if (match is not null)
            {
                match.Foreign = pair.Foreign;
                continue;
            }

            var entity = new VocabularyEntryEntity
            {
                Id = Guid.NewGuid().ToString("N"),
                ProfileId = profileId,
                Subject = subjectName,
                German = pair.German,
                Foreign = pair.Foreign,
                Stage = 0,
                NextDueAt = null,
                CreatedAt = DateTimeOffset.Now
            };

            _db.VocabularyEntries.Add(entity);
            existing.Add(entity);
            added++;
        }

        await _db.SaveChangesAsync(cancellationToken);
        return added;
    }

    /// <summary>
    /// Verbucht das Ergebnis einer Vokabelabfrage: richtig hebt die Stufe und schiebt die nächste
    /// Fälligkeit heraus, falsch setzt die Meisterung zurück auf "sofort wieder fällig". Die
    /// Vokabel wird nie gelöscht - anders als bei der Fehler-Kartei bleibt sie dauerhaft im
    /// Bestand, sie gehört ja zum Wortschatz.
    /// </summary>
    public async Task RecordOutcomeAsync(
        string profileId, string entryId, bool wasCorrect, CancellationToken cancellationToken = default)
    {
        var entity = await _db.VocabularyEntries
            .FirstOrDefaultAsync(v => v.Id == entryId && v.ProfileId == profileId, cancellationToken);

        if (entity is null)
        {
            return;
        }

        if (wasCorrect)
        {
            entity.Stage += 1;
            entity.NextDueAt = SpacedRepetitionSchedule.NextDueAt(entity.Stage, DateTimeOffset.Now);
        }
        else
        {
            entity.Stage = 0;
            entity.NextDueAt = null;
            entity.WrongCount += 1;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.VocabularyEntries.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.VocabularyEntries.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static VocabularyEntry ToModel(VocabularyEntryEntity entity) => new()
    {
        Id = entity.Id,
        Subject = Enum.Parse<Subject>(entity.Subject),
        German = entity.German,
        Foreign = entity.Foreign,
        Stage = entity.Stage,
        NextDueAt = entity.NextDueAt,
        WrongCount = entity.WrongCount
    };
}
