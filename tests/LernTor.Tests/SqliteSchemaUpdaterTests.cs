using LernTor.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Tests gegen echte SQLite-Dateien im Temp-Verzeichnis: simuliert eine veraltete lerntor.db
/// (fehlende Spalten, fehlende Tabelle) und prüft, dass der Schema-Abgleich sie ergänzt,
/// OHNE vorhandene Daten zu verlieren - das ist das Kernversprechen des Features.
/// </summary>
public sealed class SqliteSchemaUpdaterTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-test-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        return new LernTorDbContext(options);
    }

    [Fact]
    public void Aktuelles_Schema_bleibt_unveraendert()
    {
        using var db = CreateContext();
        db.Database.EnsureCreated();

        var applied = SqliteSchemaUpdater.Update(db);

        Assert.Empty(applied);
    }

    [Fact]
    public void Fehlende_Spalten_und_Tabellen_werden_ergaenzt_ohne_Datenverlust()
    {
        // Veraltete DB von Hand anlegen: Profiles ohne die späteren Spalten AvatarEmoji/TotalStars,
        // Rewards fehlt komplett - genau der Zustand einer Installation von vor ein paar Updates.
        using (var db = CreateContext())
        {
            db.Database.ExecuteSqlRaw("""
                CREATE TABLE "Profiles" (
                    "Id" TEXT NOT NULL CONSTRAINT "PK_Profiles" PRIMARY KEY,
                    "Name" TEXT NOT NULL,
                    "Age" INTEGER NULL,
                    "ClassLabel" TEXT NULL,
                    "GradeLevel" INTEGER NOT NULL,
                    "CreatedAt" TEXT NOT NULL
                )
                """);
            db.Database.ExecuteSqlRaw(
                "INSERT INTO Profiles (Id, Name, Age, ClassLabel, GradeLevel, CreatedAt) " +
                "VALUES ('p1', 'Testkind', 12, '6c', 0, '2026-01-01 00:00:00+00:00')");
        }

        using (var db = CreateContext())
        {
            var applied = SqliteSchemaUpdater.Update(db);

            Assert.Contains(applied, c => c.Contains("Profiles") && c.Contains("AvatarEmoji"));
            Assert.Contains(applied, c => c.Contains("Profiles") && c.Contains("TotalStars"));
            Assert.Contains(applied, c => c.Contains("Rewards"));
        }

        // Die Altzeile muss die neuen Spalten mit Standardwerten bekommen haben und über EF lesbar sein.
        using (var db = CreateContext())
        {
            var profile = db.Profiles.Single(p => p.Id == "p1");
            Assert.Equal("Testkind", profile.Name);
            Assert.Equal(0, profile.TotalStars);

            Assert.Empty(db.Rewards.ToList()); // Neue Tabelle existiert und ist abfragbar.
        }
    }

    [Fact]
    public void Wiederholter_Abgleich_ist_idempotent()
    {
        using var db = CreateContext();
        db.Database.EnsureCreated();

        Assert.Empty(SqliteSchemaUpdater.Update(db));
        Assert.Empty(SqliteSchemaUpdater.Update(db));
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
