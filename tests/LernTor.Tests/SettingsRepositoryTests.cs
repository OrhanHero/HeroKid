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

    [Fact]
    public async Task Ausgeblendete_Lesetexte_und_abgeschaltete_Quellen_ueberleben_den_Neustart()
    {
        // Genau das ging schief: die Felder standen nur im Modell, SettingsEntity hatte gar keine
        // Spalten dafuer. Die Einstellungen waren nach dem Schliessen der App weg.
        using (var db = CreateContext())
        {
            var repo = new SettingsRepository(db);
            var settings = await repo.LoadAsync();

            settings.HiddenReadingTextKeys.Add("fest:Wandrers Nachtlied");
            settings.HiddenReadingTextKeys.Add("eigen:abc123");
            settings.DisabledNewsFeeds.Add("heise online");

            await repo.SaveAsync(settings);
        }

        using (var db = CreateContext())
        {
            var reloaded = await new SettingsRepository(db).LoadAsync();

            Assert.Contains("fest:Wandrers Nachtlied", reloaded.HiddenReadingTextKeys);
            Assert.Contains("eigen:abc123", reloaded.HiddenReadingTextKeys);
            Assert.Contains("heise online", reloaded.DisabledNewsFeeds);
        }
    }

    [Fact]
    public async Task Frische_Einstellungen_haben_leere_Listen()
    {
        using var db = CreateContext();

        var settings = await new SettingsRepository(db).LoadAsync();

        Assert.Empty(settings.HiddenReadingTextKeys);
        Assert.Empty(settings.DisabledNewsFeeds);
    }
}
