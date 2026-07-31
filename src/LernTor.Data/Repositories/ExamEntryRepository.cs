using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Klausurtermine pro Kind (siehe <see cref="ExamEntry"/>). Kinder dürfen selbst eintragen -
/// die Zugriffsregel ("eigene ja, Eltern-Einträge nein") setzt <see cref="DeleteAsChildAsync"/>
/// bzw. <see cref="UpdateAsChildAsync"/> durch, damit sie nicht allein an der Oberfläche hängt.
/// </summary>
public sealed class ExamEntryRepository
{
    private readonly LernTorDbContext _db;

    public ExamEntryRepository(LernTorDbContext db)
    {
        _db = db;
    }

    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>Alle Termine eines Profils, nächster zuerst - für den Eltern-Bereich.</summary>
    public async Task<IReadOnlyList<ExamEntry>> GetForProfileAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var entities = await _db.Exams
            .Where(e => e.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        // In-memory sortiert: EF Core/SQLite kann DateTimeOffset nicht serverseitig sortieren.
        return entities
            .Select(ToModel)
            .OrderBy(e => e.ExamDate)
            .ThenBy(e => e.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    /// <summary>Die Termine, die dem Kind gezeigt werden: bevorstehende und der von gestern.</summary>
    public async Task<IReadOnlyList<ExamEntry>> GetVisibleForProfileAsync(
        string profileId, DateOnly today, CancellationToken cancellationToken = default)
    {
        var all = await GetForProfileAsync(profileId, cancellationToken);
        return all.Where(e => e.IsVisibleTo(today)).ToList();
    }

    /// <summary>
    /// Alle Fächer mit bevorstehender Klausur samt Gewichtungsfaktor - Grundlage dafür, dass das
    /// betroffene Fach in den Tagen davor mehr Aufgaben bekommt. Stehen für ein Fach mehrere
    /// Termine an, zählt der stärkste Faktor (also der nächstgelegene).
    /// </summary>
    public async Task<IReadOnlyDictionary<Subject, double>> GetLearningWeightsAsync(
        string profileId, DateOnly today, CancellationToken cancellationToken = default)
    {
        var upcoming = await GetForProfileAsync(profileId, cancellationToken);

        return upcoming
            .Where(e => e.IsUpcoming(today))
            .GroupBy(e => e.Subject)
            .ToDictionary(
                group => group.Key,
                group => group.Max(e => e.LearningWeight(today)));
    }

    public async Task<ExamEntry> AddAsync(
        string profileId,
        Subject subject,
        string title,
        string topics,
        DateOnly examDate,
        EntryAuthor author,
        CancellationToken cancellationToken = default)
    {
        var exam = new ExamEntry
        {
            ProfileId = profileId,
            Subject = subject,
            Title = title.Trim(),
            Topics = topics.Trim(),
            ExamDate = examDate,
            Author = author
        };

        _db.Exams.Add(new ExamEntryEntity
        {
            Id = exam.Id,
            ProfileId = exam.ProfileId,
            Subject = exam.Subject.ToString(),
            Title = exam.Title,
            Topics = exam.Topics,
            ExamDate = exam.ExamDate.ToString(DateFormat),
            Author = exam.Author.ToString(),
            CreatedAt = exam.CreatedAt
        });

        await _db.SaveChangesAsync(cancellationToken);
        return exam;
    }

    /// <summary>Ändern aus dem Eltern-Bereich - darf alles.</summary>
    public async Task UpdateAsync(
        string examId,
        Subject subject,
        string title,
        string topics,
        DateOnly examDate,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.Exams.FirstOrDefaultAsync(e => e.Id == examId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.Subject = subject.ToString();
        entity.Title = title.Trim();
        entity.Topics = topics.Trim();
        entity.ExamDate = examDate.ToString(DateFormat);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Ändern durch das Kind. Eltern-Einträge bleiben unangetastet und die Methode meldet
    /// <c>false</c> - sonst wäre "Klausur auf nächstes Jahr schieben" ein bequemer Weg, dem
    /// Üben zu entgehen.
    /// </summary>
    public async Task<bool> UpdateAsChildAsync(
        string examId,
        Subject subject,
        string title,
        string topics,
        DateOnly examDate,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.Exams.FirstOrDefaultAsync(e => e.Id == examId, cancellationToken);
        if (entity is null || entity.Author != EntryAuthor.Kind.ToString())
        {
            return false;
        }

        await UpdateAsync(examId, subject, title, topics, examDate, cancellationToken);
        return true;
    }

    /// <summary>Löschen aus dem Eltern-Bereich - darf alles.</summary>
    public async Task DeleteAsync(string examId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Exams.FirstOrDefaultAsync(e => e.Id == examId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.Exams.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Löschen durch das Kind - nur eigene Einträge, sonst <c>false</c>.</summary>
    public async Task<bool> DeleteAsChildAsync(string examId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Exams.FirstOrDefaultAsync(e => e.Id == examId, cancellationToken);
        if (entity is null || entity.Author != EntryAuthor.Kind.ToString())
        {
            return false;
        }

        _db.Exams.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>Räumt Termine ab, die lange vorbei sind.</summary>
    public async Task<int> DeletePastOlderThanAsync(
        DateOnly cutoff, CancellationToken cancellationToken = default)
    {
        var cutoffValue = cutoff.ToString(DateFormat);

        // Als "yyyy-MM-dd" ist der Textvergleich zugleich der Datumsvergleich.
        var stale = await _db.Exams
            .Where(e => string.Compare(e.ExamDate, cutoffValue) < 0)
            .ToListAsync(cancellationToken);

        if (stale.Count == 0)
        {
            return 0;
        }

        _db.Exams.RemoveRange(stale);
        await _db.SaveChangesAsync(cancellationToken);
        return stale.Count;
    }

    private static ExamEntry ToModel(ExamEntryEntity entity) => new()
    {
        Id = entity.Id,
        ProfileId = entity.ProfileId,
        Subject = Enum.TryParse<Subject>(entity.Subject, out var subject) ? subject : Subject.Mathematik,
        Title = entity.Title,
        Topics = entity.Topics,
        // Ein unlesbares Datum (von Hand editierte DB) zaehlt als "heute" - so faellt es auf,
        // statt still zu verschwinden.
        ExamDate = DateOnly.TryParseExact(entity.ExamDate, DateFormat, out var date)
            ? date
            : DateOnly.FromDateTime(DateTime.Today),
        Author = Enum.TryParse<EntryAuthor>(entity.Author, out var author) ? author : EntryAuthor.Eltern,
        CreatedAt = entity.CreatedAt
    };
}
