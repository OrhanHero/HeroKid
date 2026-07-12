using LernTor.Core.Enums;
using LernTor.Data;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Belohnungs-Logik gegen echte SQLite-Temp-Dateien (Muster wie die übrigen Data-Tests).</summary>
public sealed class RewardRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-rewards-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static StudentProfileEntity Profile(int stars) => new()
    {
        Id = "p1",
        Name = "Testkind",
        GradeLevel = (int)GradeLevel.Klasse6,
        TotalStars = stars,
        CreatedAt = DateTimeOffset.Now
    };

    [Fact]
    public async Task Einloesen_zieht_Sterne_ab_und_protokolliert()
    {
        using var db = CreateContext();
        db.Profiles.Add(Profile(stars: 25));
        db.SaveChanges();
        var repo = new RewardRepository(db);
        var reward = await repo.AddAsync("🎮", "30 Minuten Spielzeit", 20);

        var (success, newTotal) = await repo.RedeemAsync("p1", reward.Id);

        Assert.True(success);
        Assert.Equal(5, newTotal);
        Assert.Equal(5, db.Profiles.Single().TotalStars);

        var redemption = Assert.Single(await repo.GetRedemptionsAsync("p1"));
        Assert.Equal("30 Minuten Spielzeit", redemption.RewardTitle);
        Assert.Equal(20, redemption.StarCost);
    }

    [Fact]
    public async Task Zu_wenig_Sterne_loest_nicht_ein_und_bucht_nichts_ab()
    {
        using var db = CreateContext();
        db.Profiles.Add(Profile(stars: 10));
        db.SaveChanges();
        var repo = new RewardRepository(db);
        var reward = await repo.AddAsync("🎬", "Kinobesuch", 100);

        var (success, total) = await repo.RedeemAsync("p1", reward.Id);

        Assert.False(success);
        Assert.Equal(10, total);
        Assert.Equal(10, db.Profiles.Single().TotalStars);
        Assert.Empty(await repo.GetRedemptionsAsync("p1"));
    }

    [Fact]
    public async Task Historie_bleibt_nach_Loeschen_der_Belohnung_erhalten()
    {
        using var db = CreateContext();
        db.Profiles.Add(Profile(stars: 30));
        db.SaveChanges();
        var repo = new RewardRepository(db);
        var reward = await repo.AddAsync("🎮", "Spielzeit", 20);
        await repo.RedeemAsync("p1", reward.Id);

        await repo.DeleteAsync(reward.Id);

        Assert.Empty(await repo.GetAllAsync());
        Assert.Single(await repo.GetRedemptionsAsync("p1")); // Schnappschuss überlebt.
    }

    [Fact]
    public async Task AddAsync_normalisiert_Emoji_und_Mindestkosten()
    {
        using var db = CreateContext();
        var repo = new RewardRepository(db);

        var reward = await repo.AddAsync("   ", "  Eis essen  ", 0);

        Assert.Equal("🎁", reward.Emoji);
        Assert.Equal("Eis essen", reward.Title);
        Assert.Equal(1, reward.StarCost);
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
