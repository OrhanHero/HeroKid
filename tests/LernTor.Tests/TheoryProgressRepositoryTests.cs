using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Theorie-Lernstand gegen echte SQLite-Temp-Dateien (Muster wie ReviewQuestionRepositoryTests).
/// </summary>
public sealed class TheoryProgressRepositoryTests : IDisposable
{
    private readonly string _dbPath =
        Path.Combine(Path.GetTempPath(), $"lerntor-theorie-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static TheoryProgressRepository CreateRepo(LernTorDbContext db) =>
        new(db, NullLogger<TheoryProgressRepository>.Instance);

    [Fact]
    public async Task Erst_die_zweite_richtige_Antwort_meldet_die_Frage_als_neu_gekonnt()
    {
        // Der Rückgabewert steuert die Sterne - beim ersten Treffer darf es keinen geben,
        // sonst wäre einmal Raten einen Stern wert.
        using var db = CreateContext();
        var repo = CreateRepo(db);

        Assert.False(await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true));
        Assert.True(await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true));

        // Und danach nicht noch einmal - sonst ließe sich dieselbe Frage endlos für Sterne
        // wiederholen.
        Assert.False(await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true));

        var gekonnt = await repo.GetMasteredQuestionIdsAsync("p1");
        Assert.Contains("vz-01", gekonnt);
    }

    [Fact]
    public async Task Ein_Fehler_nimmt_die_Frage_wieder_aus_dem_Gekonnt_Stapel()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true);
        await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true);
        await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: false);

        Assert.Empty(await repo.GetMasteredQuestionIdsAsync("p1"));

        var eintrag = (await repo.GetAnswersAsync("p1"))["vz-01"];
        Assert.Equal(0, eintrag.CorrectStreak);
        Assert.Equal(2, eintrag.CorrectTotal);
        Assert.Equal(1, eintrag.WrongTotal);
    }

    [Fact]
    public async Task Der_Lernstand_gehoert_je_einem_Profil()
    {
        // Zwei Kinder an einem PC - der Lernstand des einen darf beim anderen nicht auftauchen.
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordAnswerAsync("emirhan", "vz-01", wasCorrect: true);
        await repo.RecordAnswerAsync("emirhan", "vz-01", wasCorrect: true);

        Assert.Single(await repo.GetMasteredQuestionIdsAsync("emirhan"));
        Assert.Empty(await repo.GetMasteredQuestionIdsAsync("batuhan"));
    }

    [Fact]
    public async Task Pruefungsdurchlaeufe_werden_festgehalten_neueste_zuerst()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordExamAsync("p1", new TheoryExamResult(30, 14, 4, 1));

        // Der ältere Lauf wird ausdrücklich zurückdatiert. Zwei Aufrufe hintereinander können
        // denselben Zeitstempel bekommen, und dann würde der Test die Sortierung gar nicht
        // prüfen, sondern nur die Einfügereihenfolge.
        var alt = db.TheoryExamRuns.Single();
        alt.TakenAt = DateTimeOffset.Now.AddDays(-1);
        await db.SaveChangesAsync();

        await repo.RecordExamAsync("p1", new TheoryExamResult(30, 6, 2, 0));

        var laeufe = await repo.GetRecentExamsAsync("p1", 5);

        Assert.Equal(2, laeufe.Count);
        // Der zuletzt gespeicherte Lauf steht vorn.
        Assert.Equal(6, laeufe[0].WrongPoints);
        Assert.True(laeufe[0].ToResult().Passed);
        Assert.False(laeufe[1].ToResult().Passed);
    }

    [Fact]
    public async Task Zuruecksetzen_loescht_Fragen_und_Pruefungen_nur_dieses_Profils()
    {
        using var db = CreateContext();
        var repo = CreateRepo(db);

        await repo.RecordAnswerAsync("p1", "vz-01", wasCorrect: true);
        await repo.RecordExamAsync("p1", new TheoryExamResult(30, 6, 2, 0));
        await repo.RecordAnswerAsync("p2", "vz-01", wasCorrect: true);

        await repo.ResetAsync("p1");

        Assert.Empty(await repo.GetAnswersAsync("p1"));
        Assert.Empty(await repo.GetRecentExamsAsync("p1", 5));
        Assert.Single(await repo.GetAnswersAsync("p2"));
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
