using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Stundenplan gegen echte SQLite-Temp-Dateien.</summary>
public sealed class TimetableRepositoryTests : IDisposable
{
    private readonly string _dbPath =
        Path.Combine(Path.GetTempPath(), $"lerntor-stundenplan-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static readonly TimetablePeriod[] Raster =
    {
        new(1, new TimeOnly(8, 0), new TimeOnly(8, 45)),
        new(2, new TimeOnly(8, 50), new TimeOnly(9, 35))
    };

    [Fact]
    public async Task Ohne_Eintrag_gilt_das_Ausgangsraster()
    {
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        var plan = await repo.GetForProfileAsync("p1");

        Assert.True(plan.IsEmpty);
        // Nicht leer: sonst muesste der Eltern-Bereich bei zehn leeren Zeitfeldern anfangen.
        Assert.Equal(Timetable.DefaultPeriods.Count, plan.Periods.Count);
    }

    [Fact]
    public async Task Gespeichertes_kommt_unveraendert_zurueck()
    {
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        await repo.ReplaceAsync("p1", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Mathe", null, "A012a"),
            new TimetableLesson(DayOfWeek.Friday, 2, "WPU Spanisch")
        });

        var plan = await repo.GetForProfileAsync("p1");

        Assert.Equal(2, plan.Lessons.Count);
        Assert.Equal(2, plan.Periods.Count);

        var montag = Assert.Single(plan.ForDay(DayOfWeek.Monday));
        Assert.Equal("Mathe", montag.Subject);
        Assert.Equal("A012a", montag.Room);
        Assert.Equal(new TimeOnly(8, 0), plan.PeriodOf(1)!.Start);

        // Ein Fach, das LernTor als Lernbereich nicht kennt, bleibt genau so stehen.
        Assert.Equal("WPU Spanisch", plan.ForDay(DayOfWeek.Friday)[0].Subject);
    }

    [Fact]
    public async Task Speichern_ersetzt_den_alten_Plan_vollstaendig()
    {
        // Ein neuer Stundenplan zum Halbjahr ist ein Austausch, kein Satz Einzeländerungen -
        // eine gestrichene Stunde darf nicht aus dem alten Plan stehen bleiben.
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        await repo.ReplaceAsync("p1", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Alt"),
            new TimetableLesson(DayOfWeek.Tuesday, 1, "Fällt weg")
        });

        await repo.ReplaceAsync("p1", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Neu")
        });

        var plan = await repo.GetForProfileAsync("p1");

        var stunde = Assert.Single(plan.Lessons);
        Assert.Equal("Neu", stunde.Subject);
        Assert.Empty(plan.ForDay(DayOfWeek.Tuesday));
    }

    [Fact]
    public async Task Die_beiden_Kinder_haben_getrennte_Plaene_und_getrennte_Zeiten()
    {
        // Genau der Grund, warum das Zeitraster am Profil haengt und nicht an der App: zwei
        // Schulen, zwei Anfangszeiten.
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        await repo.ReplaceAsync("emirhan", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Deutsch")
        });

        await repo.ReplaceAsync("batuhan", new[]
        {
            new TimetablePeriod(1, new TimeOnly(7, 45), new TimeOnly(8, 30))
        }, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Englisch")
        });

        var einer = await repo.GetForProfileAsync("emirhan");
        var anderer = await repo.GetForProfileAsync("batuhan");

        Assert.Equal("Deutsch", einer.ForDay(DayOfWeek.Monday)[0].Subject);
        Assert.Equal("Englisch", anderer.ForDay(DayOfWeek.Monday)[0].Subject);
        Assert.Equal(new TimeOnly(8, 0), einer.PeriodOf(1)!.Start);
        Assert.Equal(new TimeOnly(7, 45), anderer.PeriodOf(1)!.Start);
    }

    [Fact]
    public async Task Leere_Faecher_raeumen_die_Stunde_wirklich_ab()
    {
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        await repo.ReplaceAsync("p1", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Mathe"),
            new TimetableLesson(DayOfWeek.Monday, 2, "   ")
        });

        var plan = await repo.GetForProfileAsync("p1");

        Assert.Single(plan.Lessons);
    }

    [Fact]
    public async Task Loeschen_nimmt_auch_das_Raster_mit()
    {
        using var db = CreateContext();
        var repo = new TimetableRepository(db);

        await repo.ReplaceAsync("p1", Raster, new[]
        {
            new TimetableLesson(DayOfWeek.Monday, 1, "Mathe")
        });

        await repo.ClearAsync("p1");

        var plan = await repo.GetForProfileAsync("p1");

        Assert.True(plan.IsEmpty);
        // Ohne eigenes Raster gilt wieder das Ausgangsraster.
        Assert.Equal(Timetable.DefaultPeriods.Count, plan.Periods.Count);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }
        catch
        {
            // Temp-Datei ggf. noch gesperrt - das Temp-Verzeichnis räumt das OS auf.
        }
    }
}
