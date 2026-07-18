using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Globale Settings (u.a. Ferien-/Pausenmodus) gegen echte SQLite-Temp-Dateien.</summary>
public sealed class SettingsRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-settings-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task PauseUntilDate_wird_gespeichert_und_wieder_geladen()
    {
        var pauseUntil = new DateOnly(2026, 8, 30);

        using (var db = CreateContext())
        {
            var repo = new SettingsRepository(db);
            await repo.SaveAsync(new AppSettings { PauseUntilDate = pauseUntil });
        }

        using (var db = CreateContext())
        {
            var reloaded = await new SettingsRepository(db).LoadAsync();
            Assert.Equal(pauseUntil, reloaded.PauseUntilDate);
        }
    }

    [Fact]
    public async Task PauseUntilDate_null_beendet_den_Pausenmodus()
    {
        using (var db = CreateContext())
        {
            var repo = new SettingsRepository(db);
            await repo.SaveAsync(new AppSettings { PauseUntilDate = new DateOnly(2026, 8, 30) });
            await repo.SaveAsync(new AppSettings { PauseUntilDate = null });
        }

        using (var db = CreateContext())
        {
            var reloaded = await new SettingsRepository(db).LoadAsync();
            Assert.Null(reloaded.PauseUntilDate);
        }
    }

    [Fact]
    public async Task Unlesbarer_Datumswert_wird_wie_kein_Pausenmodus_behandelt()
    {
        // Defensive: eine von Hand editierte/beschädigte DB darf den App-Start nicht kippen -
        // ungültige Strings zählen schlicht als "kein Pausenmodus".
        using (var db = CreateContext())
        {
            await new SettingsRepository(db).SaveAsync(new AppSettings());
            var entity = db.Settings.Single();
            entity.PauseUntilDate = "kein-datum";
            await db.SaveChangesAsync();
        }

        using (var db = CreateContext())
        {
            var reloaded = await new SettingsRepository(db).LoadAsync();
            Assert.Null(reloaded.PauseUntilDate);
        }
    }

    public void Dispose()
    {
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }
}
