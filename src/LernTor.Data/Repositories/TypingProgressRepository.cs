using LernTor.Core.Enums;
using LernTor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LernTor.Data.Repositories;

/// <summary>
/// Repository für Tipp-Fortschritt (pro Profil + Lektion).
/// </summary>
public sealed class TypingProgressRepository
{
    private readonly LernTorDbContext _db;
    private readonly ILogger<TypingProgressRepository> _log;

    public TypingProgressRepository(LernTorDbContext db, ILogger<TypingProgressRepository> log)
    {
        _db = db;
        _log = log;
    }

    /// <summary>Lädt den kompletten Tipp-Fortschritt für ein Profil.</summary>
    public async Task<IReadOnlyDictionary<string, TypingLessonProgressEntity>> GetProgressAsync(string profileId)
    {
        var entries = await _db.TypingLessonProgress
            .Where(p => p.ProfileId == profileId)
            .ToListAsync();

        return entries.ToDictionary(
            e => e.LessonId,
            e => e,
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Lädt den Fortschritt für eine spezifische Lektion.</summary>
    public async Task<TypingLessonProgressEntity?> GetLessonProgressAsync(string profileId, string lessonId)
    {
        var id = $"{profileId}|{lessonId}".ToLowerInvariant();
        return await _db.TypingLessonProgress.FindAsync(id);
    }

    /// <summary>Lädt oder erstellt den Fortschritt für eine Lektion.</summary>
    public async Task<TypingLessonProgressEntity> GetOrCreateAsync(string profileId, string lessonId)
    {
        var id = $"{profileId}|{lessonId}".ToLowerInvariant();
        var entity = await _db.TypingLessonProgress.FindAsync(id);
        if (entity is null)
        {
            entity = new TypingLessonProgressEntity
            {
                Id = id,
                ProfileId = profileId,
                LessonId = lessonId,
                LessonType = 0,
                AttemptCount = 0,
                TotalCharactersTyped = 0,
                CorrectCharacters = 0,
                BestAccuracy = 0,
                BestWpm = 0,
                LastAttemptAt = DateTimeOffset.Now,
                LastAccuracy = 0,
                LastWpm = 0,
                IsCompleted = false,
                StarsEarned = 0
            };
            _db.TypingLessonProgress.Add(entity);
            await _db.SaveChangesAsync();
        }
        return entity;
    }

    /// <summary>Speichert den Fortschritt einer Lektion.</summary>
    public async Task SaveAsync(TypingLessonProgressEntity entity)
    {
        var existing = await _db.TypingLessonProgress.FindAsync(entity.Id);
        if (existing is null)
        {
            _db.TypingLessonProgress.Add(entity);
        }
        else
        {
            // Felder aktualisieren
            existing.BestAccuracy = entity.BestAccuracy;
            existing.BestWpm = entity.BestWpm;
            existing.TotalCharactersTyped = entity.TotalCharactersTyped;
            existing.CorrectCharacters = entity.CorrectCharacters;
            existing.AttemptCount = entity.AttemptCount;
            existing.IsCompleted = entity.IsCompleted;
            existing.LastAttemptAt = entity.LastAttemptAt;
            existing.CompletedAt = entity.CompletedAt;
            existing.StarsEarned = entity.StarsEarned;
            existing.LastWpm = entity.LastWpm;
            existing.LastAccuracy = entity.LastAccuracy;
        }

        await _db.SaveChangesAsync();
    }

    /// <summary>Speichert oder aktualisiert den Fortschritt einer Lektion.</summary>
    public async Task SaveLessonProgressAsync(TypingLessonProgressEntity entity)
    {
        var existing = await _db.TypingLessonProgress.FindAsync(entity.Id);
        if (existing is null)
        {
            _db.TypingLessonProgress.Add(entity);
        }
        else
        {
            // Felder aktualisieren
            existing.BestAccuracy = entity.BestAccuracy;
            existing.BestWpm = entity.BestWpm;
            existing.TotalCharactersTyped = entity.TotalCharactersTyped;
            existing.CorrectCharacters = entity.CorrectCharacters;
            existing.AttemptCount = entity.AttemptCount;
            existing.IsCompleted = entity.IsCompleted;
            existing.LastAttemptAt = entity.LastAttemptAt;
            existing.CompletedAt = entity.CompletedAt;
            existing.StarsEarned = entity.StarsEarned;
            existing.LastWpm = entity.LastWpm;
            existing.LastAccuracy = entity.LastAccuracy;
        }

        await _db.SaveChangesAsync();
    }

    /// <summary>Erhöht die Versuchszahl und aktualisiert Best-Werte falls besser.</summary>
    public async Task<TypingLessonProgressEntity> RecordAttemptAsync(
        string profileId,
        string lessonId,
        int lessonType,
        double accuracy,
        double wpm,
        int correctChars,
        int totalChars,
        double minAccuracy,
        int minChars,
        CancellationToken ct = default)
    {
        var id = $"{profileId}|{lessonId}".ToLowerInvariant();
        var entity = await _db.TypingLessonProgress.FindAsync(new object[] { id }, ct);

        if (entity is null)
        {
            entity = new TypingLessonProgressEntity
            {
                Id = id,
                ProfileId = profileId,
                LessonId = lessonId,
                LessonType = lessonType,
                BestAccuracy = accuracy,
                BestWpm = wpm,
                TotalCharactersTyped = totalChars,
                CorrectCharacters = correctChars,
                AttemptCount = 1,
                IsCompleted = IsPassed(accuracy, totalChars, minAccuracy, minChars),
                LastAttemptAt = DateTimeOffset.Now,
                LastWpm = wpm,
                LastAccuracy = accuracy
            };

            if (entity.IsCompleted)
            {
                entity.CompletedAt = DateTimeOffset.Now;
                entity.StarsEarned = CalculateStars(accuracy, wpm);
            }

            _db.TypingLessonProgress.Add(entity);
        }
        else
        {
            entity.AttemptCount++;
            entity.TotalCharactersTyped += totalChars;
            entity.CorrectCharacters = Math.Max(entity.CorrectCharacters, correctChars);
            entity.LastAttemptAt = DateTimeOffset.Now;
            entity.LastWpm = wpm;
            entity.LastAccuracy = accuracy;

            if (accuracy > entity.BestAccuracy)
            {
                entity.BestAccuracy = accuracy;
                entity.BestWpm = wpm;
            }

            if (wpm > entity.BestWpm)
            {
                entity.BestWpm = wpm;
            }

            var wasCompleted = entity.IsCompleted;
            entity.IsCompleted = IsPassed(accuracy, totalChars, 0.85, 20);

            if (!wasCompleted && entity.IsCompleted)
            {
                entity.CompletedAt = DateTimeOffset.Now;
                entity.StarsEarned = CalculateStars(accuracy, wpm);
                _log.LogInformation("Profil {ProfileId} hat Lektion {LessonId} abgeschlossen (Genauigkeit: {Accuracy:P1})", profileId, lessonId, accuracy);
            }
        }

        await _db.SaveChangesAsync(ct);
        return entity;
    }

    private static bool IsPassed(double accuracy, int charsTyped, double minAccuracy, int minChars)
        => accuracy >= minAccuracy && charsTyped >= minChars;

    private static int CalculateStars(double accuracy, double wpm)
    {
        // 1 Stern für Bestehen, +1 für >95% Genauigkeit, +1 für >30 WPM
        int stars = 1;
        if (accuracy >= 0.95) stars++;
        if (wpm >= 30) stars++;
        return Math.Min(stars, 3);
    }

    /// <summary>Setzt den gesamten Tipp-Fortschritt eines Profils zurück.</summary>
    public async Task ResetProfileProgressAsync(string profileId)
    {
        var entries = await _db.TypingLessonProgress
            .Where(p => p.ProfileId == profileId)
            .ToListAsync();

        _db.TypingLessonProgress.RemoveRange(entries);
        await _db.SaveChangesAsync();
    }

    /// <summary>Löscht alle Tipp-Daten (für 'Alle Daten zurücksetzen').</summary>
    public async Task ResetAllAsync()
    {
        await _db.Database.ExecuteSqlRawAsync("DELETE FROM \"TypingLessonProgress\"");
    }
}