using LernTor.Core.Design;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Design-Wahl des Kindes (docs/NAECHSTES-LEVEL-3.md, Schritt 4) gegen eine echte
/// SQLite-Datei: sie überlebt einen Neustart, rührt keine andere Einstellung an und wird vom
/// Speichern im Eltern-Bereich nicht überschrieben.
/// </summary>
public sealed class ProfileDesignPersistenceTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-design-{Guid.NewGuid():N}.db");

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
    public async Task Neues_Profil_hat_das_Standard_Design()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);

        var profil = await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒");
        var gelesen = (await repo.GetAllAsync()).Single(p => p.Id == profil.Id);

        Assert.Equal(DesignPreferences.Default, gelesen.Design);
    }

    [Fact]
    public async Task Design_Wahl_wird_gespeichert_und_wieder_gelesen()
    {
        var wahl = new DesignPreferences("ozean", DesignFont.GutLesbar, 110, FollowWindows: true, DarkInEvening: true);
        string id;
        using (var db = CreateContext())
        {
            var repo = new StudentProfileRepository(db);
            id = (await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒")).Id;
            await repo.SetDesignAsync(id, wahl);
        }

        using (var db = CreateContext())
        {
            var gelesen = (await new StudentProfileRepository(db).GetAllAsync()).Single(p => p.Id == id);
            Assert.Equal(wahl, gelesen.Design);
        }
    }

    [Fact]
    public async Task Unbekannte_Werte_werden_zum_Standard()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);
        var id = (await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒")).Id;

        await repo.SetDesignAsync(id, new DesignPreferences("gibt-es-nicht", DesignFont.Verspielt, 175, false, false));
        var gelesen = (await repo.GetAllAsync()).Single(p => p.Id == id);

        Assert.Equal(DesignThemeCatalog.DefaultId, gelesen.Design.ThemeId);
        Assert.Equal(100, gelesen.Design.TextScalePercent);
        Assert.Equal(DesignFont.Verspielt, gelesen.Design.Font);
    }

    [Fact]
    public async Task Speichern_im_Eltern_Bereich_laesst_das_Design_des_Kindes_in_Ruhe()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);
        var profil = await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒");
        var wahl = DesignPreferences.Default with { ThemeId = "nacht", TextScalePercent = 120 };
        await repo.SetDesignAsync(profil.Id, wahl);

        await repo.UpdateSettingsAsync(profil.Id, ProfileSettings.From(profil) with { WeeklyGoalDays = 3 });
        var gelesen = (await repo.GetAllAsync()).Single(p => p.Id == profil.Id);

        Assert.Equal(wahl, gelesen.Design);
        Assert.Equal(3, gelesen.WeeklyGoalDays);
    }

    [Fact]
    public async Task Design_Wahl_aendert_keine_andere_Einstellung()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);
        var profil = await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒");
        await repo.UpdateSettingsAsync(profil.Id, ProfileSettings.From(profil) with
        {
            NewsFilterStrictness = NewsFilterStrictness.Streng,
            DrivingAreaEnabled = false,
            WeeklyGoalDays = 5
        });
        var vorher = ProfileSettings.From((await repo.GetAllAsync()).Single(p => p.Id == profil.Id));

        await repo.SetDesignAsync(profil.Id, DesignPreferences.Default with { ThemeId = "wald" });
        var nachher = ProfileSettings.From((await repo.GetAllAsync()).Single(p => p.Id == profil.Id));

        Assert.Equivalent(vorher, nachher);
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
            // Temp-Datei ggf. noch gesperrt (Connection-Pool) - das Temp-Verzeichnis räumt das OS auf.
        }
    }
}
