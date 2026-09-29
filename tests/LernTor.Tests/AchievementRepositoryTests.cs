using LernTor.Core.Services;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Abzeichen gegen echte SQLite-Temp-Dateien (Muster wie ReviewQuestionRepositoryTests).</summary>
public sealed class AchievementRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-abzeichen-{Guid.NewGuid():N}.db");

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
    public async Task Neue_Abzeichen_werden_einmal_gespeichert_und_nur_einmal_gemeldet()
    {
        using var db = CreateContext();
        var repo = new AchievementRepository(db);
        var fakten = new AchievementFacts { CorrectAnswers = 120 };

        var erstesMal = await repo.UnlockAsync("p1", fakten);
        var zweitesMal = await repo.UnlockAsync("p1", fakten);

        Assert.Equal(new[] { "richtig-10", "richtig-100" }, erstesMal.Select(a => a.Id).ToArray());
        Assert.Empty(zweitesMal);
        Assert.Equal(2, db.UnlockedAchievements.Count());
    }

    [Fact]
    public async Task Einmal_verdient_bleibt_verdient()
    {
        // Grundsatz: nichts verfaellt. Faellt eine Bedingung spaeter weg (Thema nicht mehr
        // gemeistert), bleibt das Abzeichen.
        using var db = CreateContext();
        var repo = new AchievementRepository(db);

        await repo.UnlockAsync("p1", new AchievementFacts { MasteredTopics = 1 });
        await repo.UnlockAsync("p1", new AchievementFacts { MasteredTopics = 0 });

        Assert.Contains("meister-1", (await repo.GetUnlockedAsync("p1")).Keys);
    }

    [Fact]
    public async Task Abzeichen_gehoeren_zum_Profil()
    {
        using var db = CreateContext();
        var repo = new AchievementRepository(db);

        await repo.UnlockAsync("p1", new AchievementFacts { CorrectAnswers = 10 });

        Assert.Single(await repo.GetUnlockedAsync("p1"));
        Assert.Empty(await repo.GetUnlockedAsync("p2"));
    }

    [Fact]
    public async Task Tabelle_gehoert_zu_den_Profil_Tabellen_und_wird_beim_Loeschen_mitgenommen()
    {
        using var db = CreateContext();
        var profile = new StudentProfileRepository(db);

        Assert.Contains(profile.ProfileOwnedTables(), eintrag => eintrag.Tabelle == "UnlockedAchievements");
        Assert.Contains("UnlockedAchievements", new DatabaseMaintenanceRepository(db).AllTableNames());
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
            // Gepoolte SQLite-Verbindung haelt die Datei unter Windows noch - das Temp-Verzeichnis
            // raeumt das Betriebssystem auf (siehe CLAUDE.md).
        }
    }
}
