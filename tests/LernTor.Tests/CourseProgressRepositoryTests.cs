using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LernTor.Tests;

/// <summary>Kurs-Lernstand gegen echte SQLite-Temp-Dateien.</summary>
public sealed class CourseProgressRepositoryTests : IDisposable
{
    private readonly string _dbPath =
        Path.Combine(Path.GetTempPath(), $"lerntor-kurs-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static CourseProgressRepository CreateRepo(LernTorDbContext db) =>
        new(db, NullLogger<CourseProgressRepository>.Instance);

    [Fact]
    public async Task Gelesen_und_geschafft_sind_zwei_verschiedene_Dinge()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.MarkReadAsync("p1", "kurs-vorfahrt");

        var eintrag = (await repo.GetAllAsync("p1"))["kurs-vorfahrt"];
        Assert.NotNull(eintrag.ReadAt);
        Assert.Null(eintrag.PassedAt);
    }

    [Fact]
    public async Task Eine_bestandene_Kontrolle_meldet_sich_nur_beim_ersten_Mal()
    {
        // Der Rückgabewert steuert die Sterne - sonst ließe sich dieselbe Kontrolle endlos
        // für Sterne wiederholen.
        using var db = CreateContext();
        var repo = CreateRepo(db);

        Assert.True(await repo.RecordCheckAsync("p1", "kurs-tempo", correct: 4, total: 5));
        Assert.False(await repo.RecordCheckAsync("p1", "kurs-tempo", correct: 5, total: 5));

        var eintrag = (await repo.GetAllAsync("p1"))["kurs-tempo"];
        Assert.NotNull(eintrag.PassedAt);
        Assert.Equal(2, eintrag.Attempts);
    }

    [Fact]
    public async Task Eine_verpatzte_Kontrolle_gilt_nicht_als_bestanden_zaehlt_aber_als_gelesen()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        Assert.False(await repo.RecordCheckAsync("p1", "kurs-technik", correct: 2, total: 5));

        var eintrag = (await repo.GetAllAsync("p1"))["kurs-technik"];
        Assert.Null(eintrag.PassedAt);
        Assert.NotNull(eintrag.ReadAt);      // wer die Kontrolle macht, hat gelesen
        Assert.Equal(40, eintrag.BestPercent);
    }

    [Fact]
    public async Task Der_beste_Versuch_zaehlt_nicht_der_letzte()
    {
        // Ein aus Neugier gestarteter und verpatzter zweiter Anlauf soll das erste Ergebnis
        // nicht verderben.
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordCheckAsync("p1", "kurs-parken", correct: 5, total: 5);
        await repo.RecordCheckAsync("p1", "kurs-parken", correct: 1, total: 5);

        var eintrag = (await repo.GetAllAsync("p1"))["kurs-parken"];
        Assert.Equal(100, eintrag.BestPercent);
        Assert.NotNull(eintrag.PassedAt);    // einmal bestanden bleibt bestanden
    }

    [Fact]
    public async Task Der_Lernstand_gehoert_je_einem_Profil()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordCheckAsync("emirhan", "kurs-zeichen", correct: 5, total: 5);

        Assert.Single(await repo.GetAllAsync("emirhan"));
        Assert.Empty(await repo.GetAllAsync("batuhan"));
    }

    [Fact]
    public async Task Zuruecksetzen_loescht_nur_dieses_Profil()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordCheckAsync("p1", "kurs-unfall", correct: 5, total: 5);
        await repo.RecordCheckAsync("p2", "kurs-unfall", correct: 5, total: 5);

        await repo.ResetAsync("p1");

        Assert.Empty(await repo.GetAllAsync("p1"));
        Assert.Single(await repo.GetAllAsync("p2"));
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
