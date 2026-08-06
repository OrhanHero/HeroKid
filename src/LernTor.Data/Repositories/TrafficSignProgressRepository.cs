using LernTor.Core.Models;
using LernTor.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LernTor.Data.Repositories;

/// <summary>
/// Lernstand der Verkehrszeichen pro Profil. Die Regeln, wann ein Zeichen als gekonnt gilt,
/// stehen in <see cref="TrafficSignProgress"/> (Core) - hier wird nur gespeichert.
/// </summary>
public sealed class TrafficSignProgressRepository
{
    private readonly LernTorDbContext _db;
    private readonly ILogger<TrafficSignProgressRepository> _log;

    public TrafficSignProgressRepository(LernTorDbContext db, ILogger<TrafficSignProgressRepository> log)
    {
        _db = db;
        _log = log;
    }

    private static string KeyFor(string profileId, string signNumber) => $"{profileId}|{signNumber}";

    /// <summary>Alle Einträge eines Profils, nach Zeichennummer.</summary>
    public async Task<IReadOnlyDictionary<string, TrafficSignProgressEntity>> GetAllAsync(string profileId)
    {
        var entries = await _db.TrafficSignProgress
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        return entries.ToDictionary(entry => entry.SignNumber, entry => entry, StringComparer.Ordinal);
    }

    /// <summary>Nummern der Zeichen, die das Kind sicher kann.</summary>
    public async Task<IReadOnlySet<string>> GetMasteredNumbersAsync(string profileId)
    {
        var entries = await _db.TrafficSignProgress
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        return entries
            .Where(entry => TrafficSignProgress.IsMastered(entry.CorrectStreak))
            .Select(entry => entry.SignNumber)
            .ToHashSet(StringComparer.Ordinal);
    }

    /// <summary>
    /// Schreibt eine Antwort fort. Legt den Eintrag beim ersten Mal an.
    /// </summary>
    /// <returns>Ob das Zeichen durch diese Antwort neu als gekonnt gilt - dafür gibt es einen Stern.</returns>
    public async Task<bool> RecordAnswerAsync(string profileId, string signNumber, bool wasCorrect)
    {
        var id = KeyFor(profileId, signNumber);
        var entry = await _db.TrafficSignProgress.FindAsync(id);

        if (entry is null)
        {
            entry = new TrafficSignProgressEntity
            {
                Id = id,
                ProfileId = profileId,
                SignNumber = signNumber
            };
            _db.TrafficSignProgress.Add(entry);
        }

        var warVorherGekonnt = TrafficSignProgress.IsMastered(entry.CorrectStreak);

        entry.CorrectStreak = TrafficSignProgress.NextStreak(entry.CorrectStreak, wasCorrect);
        entry.LastSeenAt = DateTimeOffset.Now;

        if (wasCorrect)
        {
            entry.CorrectTotal++;
        }
        else
        {
            entry.WrongTotal++;
        }

        await _db.SaveChangesAsync();

        return !warVorherGekonnt && TrafficSignProgress.IsMastered(entry.CorrectStreak);
    }

    /// <summary>
    /// Setzt den Lernstand eines Profils zurück - im Eltern-Bereich, wenn ein Kind von vorn
    /// anfangen möchte. Betrifft nur die Verkehrszeichen, nichts anderes.
    /// </summary>
    public async Task ResetAsync(string profileId)
    {
        var entries = await _db.TrafficSignProgress
            .Where(entry => entry.ProfileId == profileId)
            .ToListAsync();

        _db.TrafficSignProgress.RemoveRange(entries);
        await _db.SaveChangesAsync();

        _log.LogInformation("Verkehrszeichen-Lernstand für Profil {ProfileId} zurückgesetzt ({Count} Einträge).",
            profileId, entries.Count);
    }
}
