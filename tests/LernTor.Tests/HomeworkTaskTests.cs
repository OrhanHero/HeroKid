using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Hausaufgaben mit Stichtag - Dringlichkeit, Sichtbarkeit und Persistenz.</summary>
public sealed class HomeworkTaskTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-homework-{Guid.NewGuid():N}.db");

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

    private static HomeworkTask Aufgabe(DateOnly due, DateTimeOffset? completed = null) => new()
    {
        ProfileId = "p1",
        Subject = Subject.Mathematik,
        Description = "Seite 42",
        DueDate = due,
        CompletedAt = completed
    };

    [Fact]
    public void Ueberfaellig_heute_und_spaeter_werden_unterschieden()
    {
        Assert.True(Aufgabe(Heute.AddDays(-1)).IsOverdue(Heute));
        Assert.False(Aufgabe(Heute).IsOverdue(Heute));

        Assert.True(Aufgabe(Heute).IsDueToday(Heute));
        Assert.False(Aufgabe(Heute.AddDays(1)).IsDueToday(Heute));
    }

    [Fact]
    public void Erledigte_Aufgaben_sind_weder_ueberfaellig_noch_heute_faellig()
    {
        var erledigt = Aufgabe(Heute.AddDays(-3), DateTimeOffset.Now);

        Assert.False(erledigt.IsOverdue(Heute));
        Assert.False(erledigt.IsDueToday(Heute));
        Assert.True(erledigt.IsCompleted);
    }

    [Fact]
    public void Dringlichkeit_sortiert_ueberfaellig_vor_heute_vor_spaeter_vor_erledigt()
    {
        var ueberfaellig = Aufgabe(Heute.AddDays(-1)).UrgencyRank(Heute);
        var heute = Aufgabe(Heute).UrgencyRank(Heute);
        var spaeter = Aufgabe(Heute.AddDays(3)).UrgencyRank(Heute);
        var erledigt = Aufgabe(Heute, DateTimeOffset.Now).UrgencyRank(Heute);

        Assert.True(ueberfaellig < heute);
        Assert.True(heute < spaeter);
        Assert.True(spaeter < erledigt);
    }

    [Fact]
    public void Frisch_Erledigtes_bleibt_kurz_sichtbar_dann_nicht_mehr()
    {
        // Sofortiges Verschwinden nimmt dem Abhaken das Erfolgserlebnis.
        var mittags = new TimeOnly(12, 0);
        var geradeEben = Aufgabe(Heute, new DateTimeOffset(Heute.ToDateTime(mittags)));
        var laengstErledigt = Aufgabe(Heute, new DateTimeOffset(Heute.AddDays(-10).ToDateTime(mittags)));

        Assert.True(geradeEben.IsVisibleTo(Heute));
        Assert.False(laengstErledigt.IsVisibleTo(Heute));
    }

    [Fact]
    public void Uralte_offene_Aufgaben_verschwinden_aus_der_Kind_Ansicht()
    {
        // Eine Hausaufgabe von vor drei Wochen ist keine Erinnerung mehr, sondern ein Vorwurf.
        Assert.True(Aufgabe(Heute.AddDays(-3)).IsVisibleTo(Heute));
        Assert.False(Aufgabe(Heute.AddDays(-30)).IsVisibleTo(Heute));
    }

    [Fact]
    public async Task Hausaufgabe_ueberlebt_den_Neustart_mit_Datum_und_Fach()
    {
        string id;
        using (var db = CreateContext())
        {
            var task = await new HomeworkTaskRepository(db)
                .AddAsync("p1", Subject.Biologie, "  Blattquerschnitt zeichnen  ", new DateOnly(2026, 9, 1));
            id = task.Id;
        }

        using (var db = CreateContext())
        {
            var reloaded = (await new HomeworkTaskRepository(db).GetForProfileAsync("p1")).Single();

            Assert.Equal(id, reloaded.Id);
            Assert.Equal(Subject.Biologie, reloaded.Subject);
            Assert.Equal("Blattquerschnitt zeichnen", reloaded.Description);
            Assert.Equal(new DateOnly(2026, 9, 1), reloaded.DueDate);
            Assert.False(reloaded.IsCompleted);
        }
    }

    [Fact]
    public async Task Abhaken_und_wieder_oeffnen_wird_gespeichert()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var task = await repo.AddAsync("p1", Subject.Mathematik, "Seite 42", Heute);

        await repo.SetCompletedAsync(task.Id, completed: true);
        Assert.True((await repo.GetForProfileAsync("p1")).Single().IsCompleted);

        await repo.SetCompletedAsync(task.Id, completed: false);
        Assert.False((await repo.GetForProfileAsync("p1")).Single().IsCompleted);
    }

    [Fact]
    public async Task Profile_bleiben_getrennt()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        await repo.AddAsync("emirhan", Subject.Mathematik, "Seite 42", Heute);
        await repo.AddAsync("batuhan", Subject.Physik, "Versuch protokollieren", Heute);

        Assert.Equal("Seite 42", (await repo.GetForProfileAsync("emirhan")).Single().Description);
        Assert.Equal("Versuch protokollieren", (await repo.GetForProfileAsync("batuhan")).Single().Description);
    }

    [Fact]
    public async Task Liste_kommt_nach_Dringlichkeit_sortiert()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var heute = DateOnly.FromDateTime(DateTime.Today);

        await repo.AddAsync("p1", Subject.Deutsch, "spaeter", heute.AddDays(5));
        await repo.AddAsync("p1", Subject.Deutsch, "ueberfaellig", heute.AddDays(-2));
        await repo.AddAsync("p1", Subject.Deutsch, "heute", heute);

        var sortiert = (await repo.GetForProfileAsync("p1")).Select(h => h.Description).ToList();

        Assert.Equal(new[] { "ueberfaellig", "heute", "spaeter" }, sortiert);
    }

    [Fact]
    public async Task Aendern_und_Loeschen_wirken()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var task = await repo.AddAsync("p1", Subject.Mathematik, "alt", Heute);

        await repo.UpdateAsync(task.Id, Subject.Englisch, "neu", Heute.AddDays(2));
        var geaendert = (await repo.GetForProfileAsync("p1")).Single();
        Assert.Equal(Subject.Englisch, geaendert.Subject);
        Assert.Equal("neu", geaendert.Description);
        Assert.Equal(Heute.AddDays(2), geaendert.DueDate);

        await repo.DeleteAsync(task.Id);
        Assert.Empty(await repo.GetForProfileAsync("p1"));
    }

    [Fact]
    public async Task Kind_Ansicht_zeigt_nur_Relevantes()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var heute = DateOnly.FromDateTime(DateTime.Today);

        await repo.AddAsync("p1", Subject.Deutsch, "offen", heute.AddDays(1));
        await repo.AddAsync("p1", Subject.Deutsch, "uralt", heute.AddDays(-40));

        var sichtbar = (await repo.GetVisibleForProfileAsync("p1", heute)).Select(h => h.Description).ToList();

        Assert.Contains("offen", sichtbar);
        Assert.DoesNotContain("uralt", sichtbar);
    }

    [Fact]
    public async Task Kind_darf_eigene_Hausaufgaben_loeschen()
    {
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var eigene = await repo.AddAsync("p1", Subject.Mathematik, "meine", Heute, EntryAuthor.Kind);

        Assert.True(await repo.DeleteAsChildAsync(eigene.Id));
        Assert.Empty(await repo.GetForProfileAsync("p1"));
    }

    [Fact]
    public async Task Kind_darf_Eltern_Hausaufgaben_nicht_loeschen()
    {
        // Wegraeumen, was man nicht erledigt hat, waere ein Schlupfloch.
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var vonEltern = await repo.AddAsync("p1", Subject.Mathematik, "Seite 42", Heute, EntryAuthor.Eltern);

        Assert.False(await repo.DeleteAsChildAsync(vonEltern.Id));
        Assert.Single(await repo.GetForProfileAsync("p1"));
    }

    [Fact]
    public async Task Abhaken_darf_das_Kind_immer()
    {
        // Auch bei Eltern-Eintraegen - das ist ja der Sinn der Sache.
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var vonEltern = await repo.AddAsync("p1", Subject.Mathematik, "Seite 42", Heute, EntryAuthor.Eltern);

        await repo.SetCompletedAsync(vonEltern.Id, completed: true);

        Assert.True((await repo.GetForProfileAsync("p1")).Single().IsCompleted);
    }

    [Fact]
    public async Task Herkunft_ueberlebt_den_Neustart()
    {
        using (var db = CreateContext())
        {
            await new HomeworkTaskRepository(db)
                .AddAsync("p1", Subject.Deutsch, "Aufsatz", Heute, EntryAuthor.Kind);
        }

        using (var db = CreateContext())
        {
            var reloaded = (await new HomeworkTaskRepository(db).GetForProfileAsync("p1")).Single();
            Assert.Equal(EntryAuthor.Kind, reloaded.Author);
            Assert.True(reloaded.IsEditableByChild);
        }
    }

    [Fact]
    public async Task Alt_Zeilen_ohne_Herkunft_gelten_als_von_den_Eltern()
    {
        // Bestehende Datenbanken bekommen die Spalte per additivem Schema-Abgleich leer -
        // sie duerfen dadurch nicht ploetzlich fuer das Kind loeschbar werden.
        using var db = CreateContext();
        var repo = new HomeworkTaskRepository(db);
        var task = await repo.AddAsync("p1", Subject.Mathematik, "alt", Heute, EntryAuthor.Kind);

        db.HomeworkTasks.Single(h => h.Id == task.Id).Author = string.Empty;
        await db.SaveChangesAsync();

        Assert.Equal(EntryAuthor.Eltern, (await repo.GetForProfileAsync("p1")).Single().Author);
        Assert.False(await repo.DeleteAsChildAsync(task.Id));
    }

    [Fact]
    public async Task Werkseinstellungen_loeschen_auch_die_Hausaufgaben()
    {
        // ResetAllDataAsync liess lange acht Tabellen stehen - neue Tabellen muessen dort rein.
        using var db = CreateContext();
        await new HomeworkTaskRepository(db).AddAsync("p1", Subject.Mathematik, "Seite 42", Heute);

        await new DatabaseMaintenanceRepository(db).ResetAllDataAsync();

        Assert.Empty(await new HomeworkTaskRepository(db).GetForProfileAsync("p1"));
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
            // Sqlite haelt den Dateizeiger manchmal noch - xUnit wuerde das sonst als
            // Testfehler melden, obwohl der Test selbst durchgelaufen ist.
        }
    }
}
