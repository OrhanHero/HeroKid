using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Eigene Lesetexte der Eltern - Speicherung und Einmischung in die Tagesauswahl.</summary>
public sealed class CustomReadingTextRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-reading-{Guid.NewGuid():N}.db");

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
    public async Task Text_wird_pro_Profil_gespeichert_und_gelesen()
    {
        using var db = CreateContext();
        var repo = new CustomReadingTextRepository(db);

        await repo.AddAsync("p1", "Der Erlkönig (Auszug)", "Goethe", "Wer reitet so spät durch Nacht und Wind?", "", "");
        await repo.AddAsync("anderes-profil", "Nur für das andere Kind", "", "Ein anderer Text.", "", "");

        var pieces = await repo.GetForProfileAsync("p1");

        var piece = Assert.Single(pieces);
        Assert.Equal("Der Erlkönig (Auszug)", piece.Title);
        Assert.Equal("Goethe", piece.Author);
        Assert.True(piece.IsCustom);
        Assert.True(piece.HasText("De"));
        // Eltern haben ein Gedicht meist nur in einer Sprache - die anderen bleiben leer.
        Assert.False(piece.HasText("Tr"));
        Assert.False(piece.HasText("En"));
    }

    [Fact]
    public async Task Reihenfolge_bleibt_nach_Anlagedatum_stabil()
    {
        // Sonst würde ein neu ergänzter Text die Tagesrotation der bestehenden durcheinanderbringen.
        using var db = CreateContext();
        var repo = new CustomReadingTextRepository(db);

        await repo.AddAsync("p1", "Erster", "", "A", "", "");
        await repo.AddAsync("p1", "Zweiter", "", "B", "", "");
        await repo.AddAsync("p1", "Dritter", "", "C", "", "");

        var titles = (await repo.GetForProfileAsync("p1")).Select(p => p.Title).ToList();

        Assert.Equal(new[] { "Erster", "Zweiter", "Dritter" }, titles);
    }

    [Fact]
    public async Task Loeschen_entfernt_nur_den_gewaehlten_Text()
    {
        using var db = CreateContext();
        var repo = new CustomReadingTextRepository(db);

        var id = await repo.AddAsync("p1", "Weg damit", "", "A", "", "");
        await repo.AddAsync("p1", "Bleibt", "", "B", "", "");

        await repo.DeleteAsync(id);

        var remaining = await repo.GetForProfileAsync("p1");
        Assert.Equal("Bleibt", Assert.Single(remaining).Title);
    }

    [Fact]
    public void Ohne_eigene_Texte_bleibt_die_Tagesauswahl_unveraendert()
    {
        var date = new DateOnly(2026, 3, 14);

        var (first, second) = ReadingContentProvider.GetPairForDate(date);

        Assert.Equal(ReadingContentProvider.GetForDate(date).Title, first.Title);
        Assert.Equal(ReadingContentProvider.GetSecondForDate(date).Title, second.Title);
    }

    [Fact]
    public void Eigener_Text_belegt_den_ersten_Tagesplatz()
    {
        // Der springende Punkt: Eltern tragen einen Text ein, weil er JETZT gebraucht wird.
        // Unter 63 eingebauten Stücken käme er im Schnitt alle zwei Monate dran.
        var eigene = new[] { Custom("Mein Text") };

        var (first, second) = ReadingContentProvider.GetPairForDate(new DateOnly(2026, 3, 14), eigene);

        Assert.Equal("Mein Text", first.Title);
        Assert.True(first.IsCustom);
        Assert.False(second.IsCustom);
    }

    [Fact]
    public void Mehrere_eigene_Texte_wechseln_sich_taeglich_ab()
    {
        var eigene = new[] { Custom("A"), Custom("B"), Custom("C") };

        var titel = Enumerable.Range(0, 6)
            .Select(offset => ReadingContentProvider.GetPairForDate(new DateOnly(2026, 1, 1).AddDays(offset), eigene).First.Title)
            .ToList();

        // Über sechs Tage müssen alle drei Texte vorkommen - keiner darf hängenbleiben.
        Assert.Equal(3, titel.Distinct().Count());
    }

    [Fact]
    public void Der_zweite_Text_bleibt_ein_eingebautes_Stueck()
    {
        // Die Mischung aus eigenem Text und Pool-Stück soll erhalten bleiben, damit die Kinder
        // nicht nur noch Eltern-Texte lesen.
        var eigene = new[] { Custom("Nur meiner") };

        for (int offset = 0; offset < 10; offset++)
        {
            var (_, second) = ReadingContentProvider.GetPairForDate(new DateOnly(2026, 1, 1).AddDays(offset), eigene);
            Assert.False(second.IsCustom);
            Assert.False(string.IsNullOrWhiteSpace(second.TextDe));
        }
    }

    [Fact]
    public void Angehefteter_Text_steht_jeden_Tag_an_erster_Stelle()
    {
        // Loest das gemeldete Problem: einen frisch eingetragenen Text konnte man nicht gezielt
        // aufrufen und damit auch nicht pruefen, ob er ueberhaupt ankommt.
        var eigene = new[] { Custom("A"), Custom("Mein Gedicht"), Custom("C") };
        var pin = eigene[1].Key;

        for (int offset = 0; offset < 10; offset++)
        {
            var (first, _) = ReadingContentProvider.GetPairForDate(
                new DateOnly(2026, 1, 1).AddDays(offset), eigene, pinnedKey: pin);

            Assert.Equal("Mein Gedicht", first.Title);
        }
    }

    [Fact]
    public void Auch_ein_eingebauter_Text_laesst_sich_anheften()
    {
        var eingebaut = ReadingContentProvider.GetAllBuiltIn()[5];

        var (first, second) = ReadingContentProvider.GetPairForDate(
            new DateOnly(2026, 4, 2), customPieces: null, hiddenKeys: null, pinnedKey: eingebaut.Key);

        Assert.Equal(eingebaut.Title, first.Title);
        Assert.NotEqual(first.Key, second.Key);
    }

    [Fact]
    public void Ausgeblendete_Texte_kommen_nicht_mehr_vor()
    {
        var alle = ReadingContentProvider.GetAllBuiltIn();
        var ausgeblendet = alle.Take(20).Select(p => p.Key).ToHashSet();

        for (int offset = 0; offset < 30; offset++)
        {
            var (first, second) = ReadingContentProvider.GetPairForDate(
                new DateOnly(2026, 1, 1).AddDays(offset), customPieces: null, hiddenKeys: ausgeblendet);

            Assert.DoesNotContain(first.Key, ausgeblendet);
            Assert.DoesNotContain(second.Key, ausgeblendet);
        }
    }

    [Fact]
    public void Die_beiden_Tagestexte_sind_nie_derselbe()
    {
        for (int offset = 0; offset < 40; offset++)
        {
            var (first, second) = ReadingContentProvider.GetPairForDate(new DateOnly(2026, 1, 1).AddDays(offset));
            Assert.NotEqual(first.Key, second.Key);
        }
    }

    [Fact]
    public void Fast_alles_ausgeblendet_liefert_trotzdem_einen_Text()
    {
        // Ein leerer Lesebereich waere schlimmer als ein doppelter Text.
        var alle = ReadingContentProvider.GetAllBuiltIn();
        var ausgeblendet = alle.Skip(1).Select(p => p.Key).ToHashSet();

        var (first, second) = ReadingContentProvider.GetPairForDate(
            new DateOnly(2026, 6, 6), customPieces: null, hiddenKeys: ausgeblendet);

        Assert.False(string.IsNullOrWhiteSpace(first.Title));
        Assert.False(string.IsNullOrWhiteSpace(second.Title));
    }

    [Fact]
    public async Task Eigene_Texte_haben_einen_stabilen_Schluessel()
    {
        // Der Schluessel wandert in die Einstellungen (ausblenden/anheften) und darf sich beim
        // Bearbeiten des Titels NICHT aendern - sonst zeigt der Lesebereich still wieder etwas
        // anderes an.
        using var db = CreateContext();
        var repo = new CustomReadingTextRepository(db);

        var id = await repo.AddAsync("p1", "Alter Titel", "", "Ein Text zum Vorlesen.", "", "");
        var vorher = (await repo.GetForProfileAsync("p1")).Single().Key;

        await repo.UpdateAsync(id, "Neuer Titel", "Neuer Autor", "Ein geänderter Text.", "", "");
        var nachher = (await repo.GetForProfileAsync("p1")).Single();

        Assert.Equal(vorher, nachher.Key);
        Assert.Equal("Neuer Titel", nachher.Title);
        Assert.Equal("Ein geänderter Text.", nachher.TextDe);
    }

    private static ReadingPiece Custom(string title) => new()
    {
        SourceId = title,
        Title = title,
        Author = "Eltern",
        TextDe = "Ein eigener Text zum Vorlesen.",
        TextTr = string.Empty,
        TextEn = string.Empty,
        IsCustom = true
    };

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
            // Der Sqlite-Provider poolt Verbindungen, die Datei kann nach dem Dispose des
            // DbContext noch kurz gesperrt sein - dann räumt das OS das Temp-Verzeichnis auf.
            // Gleiches Muster wie in ReviewQuestionRepositoryTests.
        }
    }
}
