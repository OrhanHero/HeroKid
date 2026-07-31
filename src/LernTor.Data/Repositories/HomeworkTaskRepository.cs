using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Verwaltet die von den Eltern eingetragenen Hausaufgaben mit Stichtag
/// (siehe <see cref="HomeworkTask"/>). Alles profilbezogen - zwei Kinder an einem PC haben
/// getrennte Hausaufgaben.
/// </summary>
public sealed class HomeworkTaskRepository
{
    private readonly LernTorDbContext _db;

    public HomeworkTaskRepository(LernTorDbContext db)
    {
        _db = db;
    }

    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>Alle Hausaufgaben eines Profils - für die Verwaltungsliste im Eltern-Bereich.</summary>
    public async Task<IReadOnlyList<HomeworkTask>> GetForProfileAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.HomeworkTasks
            .Where(h => h.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        var today = DateOnly.FromDateTime(DateTime.Today);

        // Sortierung in-memory: EF Core/SQLite kann DateTimeOffset-Spalten nicht serverseitig
        // sortieren (siehe CLAUDE.md), und die Dringlichkeit ist ohnehin Modell-Logik.
        return entities
            .Select(ToModel)
            .OrderBy(h => h.UrgencyRank(today))
            .ThenBy(h => h.DueDate)
            .ThenBy(h => h.Description, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// Die Hausaufgaben, die dem Kind gezeigt werden: offene (bis zwei Wochen nach Stichtag) und
    /// gerade erst abgehakte. Dringendste zuerst.
    /// </summary>
    public async Task<IReadOnlyList<HomeworkTask>> GetVisibleForProfileAsync(
        string profileId, DateOnly today, CancellationToken cancellationToken = default)
    {
        var all = await GetForProfileAsync(profileId, cancellationToken);
        return all.Where(h => h.IsVisibleTo(today)).ToList();
    }

    public async Task<HomeworkTask> AddAsync(
        string profileId,
        Subject subject,
        string description,
        DateOnly dueDate,
        EntryAuthor author = EntryAuthor.Eltern,
        CancellationToken cancellationToken = default)
    {
        var task = new HomeworkTask
        {
            ProfileId = profileId,
            Subject = subject,
            Description = description.Trim(),
            DueDate = dueDate,
            Author = author
        };

        _db.HomeworkTasks.Add(new HomeworkTaskEntity
        {
            Id = task.Id,
            ProfileId = task.ProfileId,
            Subject = task.Subject.ToString(),
            Description = task.Description,
            DueDate = task.DueDate.ToString(DateFormat),
            CompletedAt = null,
            Author = task.Author.ToString(),
            CreatedAt = task.CreatedAt
        });

        await _db.SaveChangesAsync(cancellationToken);
        return task;
    }

    /// <summary>Hakt eine Hausaufgabe ab bzw. macht das Abhaken rückgängig.</summary>
    public async Task SetCompletedAsync(
        string taskId, bool completed, CancellationToken cancellationToken = default)
    {
        var entity = await _db.HomeworkTasks.FirstOrDefaultAsync(h => h.Id == taskId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.CompletedAt = completed ? DateTimeOffset.Now : null;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        string taskId,
        Subject subject,
        string description,
        DateOnly dueDate,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.HomeworkTasks.FirstOrDefaultAsync(h => h.Id == taskId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.Subject = subject.ToString();
        entity.Description = description.Trim();
        entity.DueDate = dueDate.ToString(DateFormat);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Loeschen durch das Kind - nur eigene Eintraege, sonst <c>false</c>. Die Pruefung sitzt hier
    /// und nicht nur an der Oberflaeche: ein ausgeblendeter Knopf ist keine Zugriffskontrolle.
    /// </summary>
    public async Task<bool> DeleteAsChildAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.HomeworkTasks.FirstOrDefaultAsync(h => h.Id == taskId, cancellationToken);
        if (entity is null || entity.Author != EntryAuthor.Kind.ToString())
        {
            return false;
        }

        _db.HomeworkTasks.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task DeleteAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.HomeworkTasks.FirstOrDefaultAsync(h => h.Id == taskId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.HomeworkTasks.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Räumt erledigte Hausaufgaben ab, die lange genug erledigt sind.</summary>
    public async Task<int> DeleteCompletedOlderThanAsync(
        DateOnly cutoff, CancellationToken cancellationToken = default)
    {
        var entities = await _db.HomeworkTasks.Where(h => h.CompletedAt != null).ToListAsync(cancellationToken);

        var stale = entities
            .Where(e => e.CompletedAt is { } completed
                        && DateOnly.FromDateTime(completed.LocalDateTime) < cutoff)
            .ToList();

        if (stale.Count == 0)
        {
            return 0;
        }

        _db.HomeworkTasks.RemoveRange(stale);
        await _db.SaveChangesAsync(cancellationToken);
        return stale.Count;
    }

    private static HomeworkTask ToModel(HomeworkTaskEntity entity) => new()
    {
        Id = entity.Id,
        ProfileId = entity.ProfileId,
        Subject = Enum.TryParse<Subject>(entity.Subject, out var subject) ? subject : Subject.Deutsch,
        Description = entity.Description,
        // Unlesbare Datumswerte (von Hand editierte DB) duerfen den Eltern-Bereich nicht kippen -
        // sie zaehlen als "heute faellig" und fallen damit auf, statt still zu verschwinden.
        DueDate = DateOnly.TryParseExact(entity.DueDate, DateFormat, out var due)
            ? due
            : DateOnly.FromDateTime(DateTime.Today),
        CompletedAt = entity.CompletedAt,
        // Alt-Zeilen ohne Wert gelten als von den Eltern eingetragen - das bisherige Verhalten.
        Author = Enum.TryParse<EntryAuthor>(entity.Author, out var author) ? author : EntryAuthor.Eltern,
        CreatedAt = entity.CreatedAt
    };
}
