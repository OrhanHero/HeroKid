using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Klausurkalender: Countdown, Lern-Gewichtung vor dem Termin und die Zugriffsregel für
/// selbst eingetragene Termine.
/// </summary>
public sealed class ExamEntryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-exams-{Guid.NewGuid():N}.db");

    private LernTorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LernTorDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        var db = new LernTorDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static readonly DateOnly Heute = new(2026, 8, 5);

    private static ExamEntry Klausur(int inTagen, Subject subject = Subject.Mathematik) => new()
    {
        ProfileId = "p1",
        Subject = subject,
        Title = "Klassenarbeit",
        ExamDate = Heute.AddDays(inTagen)
    };

    [Theory]
    [InlineData(0, "HEUTE")]
    [InlineData(1, "morgen")]
    [InlineData(2, "übermorgen")]
    [InlineData(5, "in 5 Tagen")]
    [InlineData(-1, "war gestern")]
    public void Countdown_spricht_wie_die_Familie(int inTagen, string erwartet)
    {
        Assert.Equal(erwartet, Klausur(inTagen).CountdownLabel(Heute));
    }

    [Fact]
    public void Ohne_anstehende_Klausur_bleibt_die_Gewichtung_neutral()
    {
        Assert.Equal(1.0, Klausur(30).LearningWeight(Heute));
        Assert.Equal(1.0, Klausur(-1).LearningWeight(Heute));
    }

    [Fact]
    public void Die_Gewichtung_steigt_zum_Termin_hin()
    {
        // Ein konstanter Aufschlag ueber die ganze Woche wuerde entweder zu frueh nerven oder
        // zu spaet zu wenig bringen.
        var eineWoche = Klausur(7).LearningWeight(Heute);
        var dreiTage = Klausur(3).LearningWeight(Heute);
        var amVortag = Klausur(1).LearningWeight(Heute);

        Assert.True(eineWoche > 1.0);
        Assert.True(dreiTage > eineWoche);
        Assert.True(amVortag > dreiTage);
        Assert.True(amVortag <= 2.0, $"Faktor {amVortag} ist zu hoch - der Tag soll straffer werden, nicht platzen.");
    }

    [Fact]
    public void Acht_Tage_vorher_passiert_noch_nichts()
    {
        Assert.Equal(1.0, Klausur(8).LearningWeight(Heute));
    }

    [Fact]
    public void Vergangene_Termine_verschwinden_kurz_danach()
    {
        Assert.True(Klausur(0).IsVisibleTo(Heute));
        Assert.True(Klausur(-1).IsVisibleTo(Heute));
        Assert.False(Klausur(-5).IsVisibleTo(Heute));
    }

    [Fact]
    public async Task Klausur_ueberlebt_den_Neustart()
    {
        using (var db = CreateContext())
        {
            await new ExamEntryRepository(db).AddAsync(
                "p1", Subject.Physik, " Test Nr. 2 ", " Optik, Linsen ",
                new DateOnly(2026, 9, 14), EntryAuthor.Kind);
        }

        using (var db = CreateContext())
        {
            var reloaded = (await new ExamEntryRepository(db).GetForProfileAsync("p1")).Single();

            Assert.Equal(Subject.Physik, reloaded.Subject);
            Assert.Equal("Test Nr. 2", reloaded.Title);
            Assert.Equal("Optik, Linsen", reloaded.Topics);
            Assert.Equal(new DateOnly(2026, 9, 14), reloaded.ExamDate);
            Assert.Equal(EntryAuthor.Kind, reloaded.Author);
        }
    }

    [Fact]
    public async Task Kind_darf_eigene_Termine_loeschen()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var eigener = await repo.AddAsync("p1", Subject.Mathematik, "meiner", "", Heute.AddDays(3), EntryAuthor.Kind);

        Assert.True(await repo.DeleteAsChildAsync(eigener.Id));
        Assert.Empty(await repo.GetForProfileAsync("p1"));
    }

    [Fact]
    public async Task Kind_darf_Eltern_Termine_nicht_loeschen()
    {
        // Sonst waere "Klausur weg, also nicht lernen" ein bequemer Ausweg.
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var elternTermin = await repo.AddAsync("p1", Subject.Mathematik, "von Mama", "", Heute.AddDays(3), EntryAuthor.Eltern);

        Assert.False(await repo.DeleteAsChildAsync(elternTermin.Id));
        Assert.Single(await repo.GetForProfileAsync("p1"));
    }

    [Fact]
    public async Task Kind_darf_Eltern_Termine_auch_nicht_verschieben()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var elternTermin = await repo.AddAsync("p1", Subject.Mathematik, "von Mama", "", Heute.AddDays(3), EntryAuthor.Eltern);

        var verschoben = await repo.UpdateAsChildAsync(
            elternTermin.Id, Subject.Mathematik, "von Mama", "", Heute.AddDays(300));

        Assert.False(verschoben);
        Assert.Equal(Heute.AddDays(3), (await repo.GetForProfileAsync("p1")).Single().ExamDate);
    }

    [Fact]
    public async Task Eltern_duerfen_alles_aendern()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var kindTermin = await repo.AddAsync("p1", Subject.Mathematik, "meiner", "", Heute.AddDays(3), EntryAuthor.Kind);

        await repo.UpdateAsync(kindTermin.Id, Subject.Chemie, "korrigiert", "Säuren", Heute.AddDays(5));

        var geaendert = (await repo.GetForProfileAsync("p1")).Single();
        Assert.Equal(Subject.Chemie, geaendert.Subject);
        Assert.Equal("korrigiert", geaendert.Title);
    }

    [Fact]
    public async Task Gewichtung_nennt_nur_Faecher_mit_anstehender_Klausur()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var heute = DateOnly.FromDateTime(DateTime.Today);

        await repo.AddAsync("p1", Subject.Mathematik, "bald", "", heute.AddDays(2), EntryAuthor.Eltern);
        await repo.AddAsync("p1", Subject.Chemie, "weit weg", "", heute.AddDays(60), EntryAuthor.Eltern);

        var weights = await repo.GetLearningWeightsAsync("p1", heute);

        Assert.True(weights[Subject.Mathematik] > 1.0);
        Assert.Equal(1.0, weights[Subject.Chemie]);
    }

    [Fact]
    public async Task Bei_zwei_Terminen_im_selben_Fach_zaehlt_der_naehere()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        var heute = DateOnly.FromDateTime(DateTime.Today);

        await repo.AddAsync("p1", Subject.Mathematik, "naeher", "", heute.AddDays(1), EntryAuthor.Eltern);
        await repo.AddAsync("p1", Subject.Mathematik, "spaeter", "", heute.AddDays(6), EntryAuthor.Eltern);

        var weights = await repo.GetLearningWeightsAsync("p1", heute);

        Assert.Equal(new ExamEntry { ExamDate = heute.AddDays(1) }.LearningWeight(heute), weights[Subject.Mathematik]);
    }

    [Fact]
    public async Task Profile_bleiben_getrennt()
    {
        using var db = CreateContext();
        var repo = new ExamEntryRepository(db);
        await repo.AddAsync("emirhan", Subject.Mathematik, "Emirhans", "", Heute.AddDays(3), EntryAuthor.Kind);
        await repo.AddAsync("batuhan", Subject.Physik, "Batuhans", "", Heute.AddDays(3), EntryAuthor.Kind);

        Assert.Equal("Emirhans", (await repo.GetForProfileAsync("emirhan")).Single().Title);
        Assert.Equal("Batuhans", (await repo.GetForProfileAsync("batuhan")).Single().Title);
    }

    [Fact]
    public async Task Werkseinstellungen_loeschen_auch_die_Klausuren()
    {
        using var db = CreateContext();
        await new ExamEntryRepository(db).AddAsync("p1", Subject.Mathematik, "x", "", Heute, EntryAuthor.Eltern);

        await new DatabaseMaintenanceRepository(db).ResetAllDataAsync();

        Assert.Empty(await new ExamEntryRepository(db).GetForProfileAsync("p1"));
    }

    public void Dispose()
    {
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        try
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }
        catch (IOException)
        {
            // Sqlite haelt den Dateizeiger manchmal noch - das Temp-Verzeichnis raeumt das OS auf.
        }
    }
}
