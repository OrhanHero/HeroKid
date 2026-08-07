using System.Globalization;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Der Stundenplan je Profil: das Zeitraster der Schule und die eingetragenen Stunden.
///
/// <para>Gespeichert wird immer der GANZE Plan auf einmal (<see cref="ReplaceAsync"/>). Ein neuer
/// Stundenplan zum Halbjahr ist kein Satz einzelner Änderungen, sondern ein Austausch - und ein
/// zeilenweiser Abgleich hätte gestrichene Stunden aus dem alten Plan stehen lassen, was am Ende
/// niemandem auffällt außer dem Kind, das umsonst in einem Raum sitzt.</para>
/// </summary>
public sealed class TimetableRepository
{
    private const string TimeFormat = "HH\\:mm";

    private readonly LernTorDbContext _db;

    public TimetableRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Der Plan eines Profils. Ohne eigenes Raster gilt das Ausgangsraster aus
    /// <see cref="Timetable.DefaultPeriods"/> - so steht im Eltern-Bereich von Anfang an etwas
    /// Sinnvolles zum Überschreiben da, statt zehn leerer Zeitfelder.
    /// </summary>
    public async Task<Timetable> GetForProfileAsync(
        string profileId, CancellationToken cancellationToken = default)
    {
        var stunden = await _db.TimetableLessons
            .Where(eintrag => eintrag.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        var raster = await _db.TimetablePeriods
            .Where(eintrag => eintrag.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        // Ausdrueckliche Lambdas statt Methodengruppen: ToModel ist ueberladen, und darauf zu
        // bauen, dass die Ueberladungsaufloesung die gemeinte trifft, hat diese Codebasis schon
        // einen CI-Durchlauf gekostet.
        return new Timetable(
            raster.Count == 0
                ? Timetable.DefaultPeriods
                : raster.Select(eintrag => ToModel(eintrag)).OfType<TimetablePeriod>(),
            stunden.Select(eintrag => ToModel(eintrag)).OfType<TimetableLesson>());
    }

    /// <summary>
    /// Ersetzt Raster und Stunden eines Profils vollständig. Leere Fächer fallen dabei weg -
    /// so räumt ein geleertes Feld im Eingaberaster die Stunde wirklich ab.
    /// </summary>
    public async Task ReplaceAsync(
        string profileId,
        IEnumerable<TimetablePeriod> periods,
        IEnumerable<TimetableLesson> lessons,
        CancellationToken cancellationToken = default)
    {
        var alteStunden = await _db.TimetableLessons
            .Where(eintrag => eintrag.ProfileId == profileId)
            .ToListAsync(cancellationToken);
        _db.TimetableLessons.RemoveRange(alteStunden);

        var altesRaster = await _db.TimetablePeriods
            .Where(eintrag => eintrag.ProfileId == profileId)
            .ToListAsync(cancellationToken);
        _db.TimetablePeriods.RemoveRange(altesRaster);

        // Ueber das Modell gehen: es wirft ungueltige Stundennummern und leere Faecher heraus und
        // sortiert - dieselbe Bereinigung wie beim Lesen, an genau einer Stelle.
        var geprueft = new Timetable(periods, lessons);

        foreach (var raster in geprueft.Periods)
        {
            _db.TimetablePeriods.Add(new TimetablePeriodEntity
            {
                Id = Guid.NewGuid().ToString("N"),
                ProfileId = profileId,
                Period = raster.Number,
                Start = raster.Start.ToString(TimeFormat, CultureInfo.InvariantCulture),
                End = raster.End.ToString(TimeFormat, CultureInfo.InvariantCulture)
            });
        }

        foreach (var stunde in geprueft.Lessons)
        {
            _db.TimetableLessons.Add(new TimetableLessonEntity
            {
                Id = Guid.NewGuid().ToString("N"),
                ProfileId = profileId,
                Day = stunde.Day.ToString(),
                Period = stunde.Period,
                Subject = stunde.Subject.Trim(),
                Teacher = Leer(stunde.Teacher),
                Room = Leer(stunde.Room)
            });
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Löscht den Stundenplan eines Profils - das Raster eingeschlossen.</summary>
    public async Task ClearAsync(string profileId, CancellationToken cancellationToken = default)
    {
        await ReplaceAsync(profileId, Array.Empty<TimetablePeriod>(), Array.Empty<TimetableLesson>(), cancellationToken);
    }

    private static string? Leer(string? wert) =>
        string.IsNullOrWhiteSpace(wert) ? null : wert.Trim();

    /// <summary>Unlesbare Zeilen (von Hand editierte Datenbank) werden übersprungen statt den
    /// ganzen Plan zu kippen - eine fehlende Stunde faellt auf, eine leere Startseite auch,
    /// eine Ausnahme beim Start dagegen nimmt dem Kind den Zugang zum PC.</summary>
    private static TimetableLesson? ToModel(TimetableLessonEntity entity) =>
        Enum.TryParse<DayOfWeek>(entity.Day, out var tag)
            ? new TimetableLesson(tag, entity.Period, entity.Subject, entity.Teacher, entity.Room)
            : null;

    private static TimetablePeriod? ToModel(TimetablePeriodEntity entity) =>
        TimeOnly.TryParseExact(entity.Start, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var von) &&
        TimeOnly.TryParseExact(entity.End, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var bis)
            ? new TimetablePeriod(entity.Period, von, bis)
            : null;
}
