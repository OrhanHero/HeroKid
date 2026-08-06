using LernTor.Core.Models;
using LernTor.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LernTor.Data.Repositories;

/// <summary>
/// Lernstand des Theorie-Kurses pro Profil. Die Regel, ab wann eine Kontrolle bestanden ist,
/// steht in <see cref="DrivingCourseRules"/> (Core) - hier wird nur gespeichert.
/// </summary>
public sealed class CourseProgressRepository
{
    private readonly LernTorDbContext _db;
    private readonly ILogger<CourseProgressRepository> _log;

    public CourseProgressRepository(LernTorDbContext db, ILogger<CourseProgressRepository> log)
    {
        _db = db;
        _log = log;
    }

    private static string KeyFor(string profileId, string lessonId) => $"{profileId}|{lessonId}";

    /// <summary>Alle Lektions-Einträge eines Profils, nach Lektions-Kennung.</summary>
    public async Task<IReadOnlyDictionary<string, CourseLessonProgressEntity>> GetAllAsync(string profileId)
    {
        var entries = await _db.CourseLessonProgress
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        return entries.ToDictionary(entry => entry.LessonId, entry => entry, StringComparer.Ordinal);
    }

    /// <summary>Hält fest, dass eine Lektion gelesen wurde. Bestehendes bleibt unberührt.</summary>
    public async Task MarkReadAsync(string profileId, string lessonId)
    {
        var entry = await GetOrCreateAsync(profileId, lessonId);

        entry.ReadAt = DateTimeOffset.Now;

        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Schreibt das Ergebnis einer Lernstandskontrolle fort.
    /// </summary>
    /// <returns>Ob die Lektion durch diesen Versuch <b>zum ersten Mal</b> bestanden wurde - nur
    /// dafür gibt es Sterne, sonst ließe sich dieselbe Kontrolle endlos wiederholen.</returns>
    public async Task<bool> RecordCheckAsync(string profileId, string lessonId, int correct, int total)
    {
        var entry = await GetOrCreateAsync(profileId, lessonId);

        var prozent = DrivingCourseRules.Percent(correct, total);
        var warVorherBestanden = entry.PassedAt is not null;

        entry.Attempts++;
        entry.ReadAt ??= DateTimeOffset.Now;

        // Der beste Versuch zählt, nicht der letzte - ein aus Neugier abgebrochener zweiter
        // Anlauf soll das erste Ergebnis nicht verderben.
        if (prozent > entry.BestPercent)
        {
            entry.BestPercent = prozent;
        }

        if (!warVorherBestanden && DrivingCourseRules.HasPassed(correct, total))
        {
            entry.PassedAt = DateTimeOffset.Now;
        }

        await _db.SaveChangesAsync();

        return !warVorherBestanden && entry.PassedAt is not null;
    }

    private async Task<CourseLessonProgressEntity> GetOrCreateAsync(string profileId, string lessonId)
    {
        var id = KeyFor(profileId, lessonId);
        var entry = await _db.CourseLessonProgress.FindAsync(id);

        if (entry is null)
        {
            entry = new CourseLessonProgressEntity
            {
                Id = id,
                ProfileId = profileId,
                LessonId = lessonId
            };
            _db.CourseLessonProgress.Add(entry);
        }

        return entry;
    }

    /// <summary>Setzt den Kurs-Lernstand eines Profils zurück - nur den Kurs, nichts anderes.</summary>
    public async Task ResetAsync(string profileId)
    {
        var entries = await _db.CourseLessonProgress
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        _db.CourseLessonProgress.RemoveRange(entries);
        await _db.SaveChangesAsync();

        _log.LogInformation("Kurs-Lernstand für Profil {ProfileId} zurückgesetzt ({Count} Lektionen).",
            profileId, entries.Count);
    }
}
