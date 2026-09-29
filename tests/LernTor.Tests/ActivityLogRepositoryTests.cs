using LernTor.Core.Enums;
using LernTor.Data;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Aktivitätsprotokoll gegen echte SQLite-Temp-Dateien (Muster wie ReviewQuestionRepositoryTests).</summary>
public sealed class ActivityLogRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-activity-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static ActivityLogEntity Eintrag(string profil, string fach, string thema, bool richtig, int tageZurueck) => new()
    {
        ProfileId = profil,
        Subject = fach,
        Topic = thema,
        Prompt = $"{fach}/{thema}/{tageZurueck}",
        GivenAnswer = "x",
        WasCorrect = richtig,
        Timestamp = DateTimeOffset.Now.AddDays(-tageZurueck)
    };

    [Fact]
    public async Task Alle_Antworten_ohne_Zeitfenster_und_nur_fuer_das_eigene_Profil()
    {
        using var db = CreateContext();
        db.ActivityLog.AddRange(
            Eintrag("p1", "Mathematik", "Bruchrechnen", true, tageZurueck: 200),
            Eintrag("p1", "Englisch", "Question Words", false, tageZurueck: 1),
            Eintrag("p2", "Mathematik", "Bruchrechnen", true, tageZurueck: 1));
        await db.SaveChangesAsync();

        var antworten = await new ActivityLogRepository(db).GetAllAnswersAsync("p1");

        Assert.Equal(2, antworten.Count);
        Assert.Contains(antworten, a => a.Subject == Subject.Mathematik && a.WasCorrect);
        Assert.Contains(antworten, a => a.Subject == Subject.Englisch && !a.WasCorrect);
    }

    [Fact]
    public async Task Unbekannter_Fachname_wird_uebergangen_statt_zu_werfen()
    {
        using var db = CreateContext();
        db.ActivityLog.AddRange(
            Eintrag("p1", "Sachkunde", "Alt", true, tageZurueck: 400),
            Eintrag("p1", "Musik", "Stimme, Gesang und Chor", true, tageZurueck: 0));
        await db.SaveChangesAsync();

        var antwort = Assert.Single(await new ActivityLogRepository(db).GetAllAnswersAsync("p1"));

        Assert.Equal(Subject.Musik, antwort.Subject);
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
            // Microsoft.Data.Sqlite haelt gepoolte Verbindungen - unter Windows ist die Datei dann
            // noch gesperrt. Das Temp-Verzeichnis raeumt das Betriebssystem auf (siehe CLAUDE.md).
        }
    }
}
