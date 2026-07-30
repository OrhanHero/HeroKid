using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

public class VocabularyParserTests
{
    [Theory]
    [InlineData("Haus = house")]
    [InlineData("Haus=house")]
    [InlineData("Haus ; house")]
    [InlineData("Haus\thouse")]
    [InlineData("Haus - house")]
    [InlineData("Haus – house")]
    [InlineData("Haus : house")]
    public void Alle_gaengigen_Trennzeichen_werden_erkannt(string line)
    {
        // Eltern haben ihre Liste schon irgendwo stehen - der Parser soll sich anpassen,
        // nicht ein bestimmtes Format erzwingen.
        var pair = Assert.Single(VocabularyParser.Parse(line));

        Assert.Equal("Haus", pair.German);
        Assert.Equal("house", pair.Foreign);
    }

    [Fact]
    public void Bindestrich_im_Wort_trennt_nicht()
    {
        // "Fußball-Verein" darf nicht in "Fußball" und "Verein = football club" zerfallen.
        var pair = Assert.Single(VocabularyParser.Parse("Fußball-Verein = football club"));

        Assert.Equal("Fußball-Verein", pair.German);
        Assert.Equal("football club", pair.Foreign);
    }

    [Fact]
    public void Bindestrich_mit_Leerzeichen_trennt_auch_bei_Bindestrich_Woertern()
    {
        var pair = Assert.Single(VocabularyParser.Parse("E-Mail - email"));

        Assert.Equal("E-Mail", pair.German);
        Assert.Equal("email", pair.Foreign);
    }

    [Fact]
    public void Mehrere_Zeilen_werden_eingelesen()
    {
        var pairs = VocabularyParser.Parse("Haus = house\nBaum = tree\r\nAuto = car");

        Assert.Equal(3, pairs.Count);
        Assert.Equal("tree", pairs[1].Foreign);
        Assert.Equal("Auto", pairs[2].German);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("nur ein Wort ohne Trennzeichen")]
    [InlineData("Haus = ")]
    [InlineData(" = house")]
    public void Unbrauchbare_Zeilen_liefern_nichts(string? text)
    {
        Assert.Empty(VocabularyParser.Parse(text));
    }

    [Fact]
    public void Eine_kaputte_Zeile_verwirft_nicht_die_ganze_Liste()
    {
        // Eine halb abgeschriebene Liste soll die brauchbaren Zeilen trotzdem übernehmen.
        var pairs = VocabularyParser.Parse("Haus = house\nhier fehlt was\nBaum = tree");

        Assert.Equal(2, pairs.Count);
    }

    [Fact]
    public void Doppelte_deutsche_Woerter_kommen_nur_einmal()
    {
        var pairs = VocabularyParser.Parse("Haus = house\nhaus = building");

        Assert.Single(pairs);
    }
}

public class VocabularyQuestionFactoryTests
{
    private static VocabularyEntry Entry(string id = "v1") => new()
    {
        Id = id,
        Subject = Subject.Englisch,
        German = "Haus",
        Foreign = "house"
    };

    [Fact]
    public void Frage_ist_beantwortbar_und_erklaert_das_Wortpaar()
    {
        var question = VocabularyQuestionFactory.Create(Entry(), GradeLevel.Klasse7, new DateOnly(2026, 5, 4));

        Assert.Equal(Subject.Englisch, question.Subject);
        Assert.Equal(VocabularyQuestionFactory.Topic, question.Topic);
        Assert.Equal(QuestionType.OpenText, question.Type);
        Assert.True(question.CheckAnswer(question.CorrectAnswers[0]));
        Assert.False(question.CheckAnswer("voellig-falsch-xyz"));
        Assert.Contains("Haus", question.Explanation);
        Assert.Contains("house", question.Explanation);
    }

    [Fact]
    public void Richtung_bleibt_am_selben_Tag_stabil()
    {
        // Sonst würde ein Neustart der App die laufende Aufgabe verändern.
        var date = new DateOnly(2026, 5, 4);

        var first = VocabularyQuestionFactory.Create(Entry(), GradeLevel.Klasse7, date);
        var second = VocabularyQuestionFactory.Create(Entry(), GradeLevel.Klasse7, date);

        Assert.Equal(first.Prompt, second.Prompt);
    }

    [Fact]
    public void Beide_Abfragerichtungen_kommen_vor()
    {
        // Wer nur Fremdsprache→Deutsch übt, erkennt das Wort, kann es aber nicht selbst benutzen.
        var prompts = Enumerable.Range(0, 14)
            .Select(offset => VocabularyQuestionFactory.Create(
                Entry(), GradeLevel.Klasse7, new DateOnly(2026, 5, 4).AddDays(offset)).Prompt)
            .Distinct()
            .ToList();

        Assert.Equal(2, prompts.Count);
    }

    [Fact]
    public void Tuerkische_Vokabeln_bekommen_die_Sonderzeichen_Hilfe()
    {
        // ç ğ ı ş sind auf einer deutschen Tastatur nicht direkt eingebbar - aber nur, wenn das
        // Kind auch tatsächlich türkisch schreiben soll.
        var entry = new VocabularyEntry { Id = "t1", Subject = Subject.Tuerkisch, German = "Haus", Foreign = "ev" };

        var mitHilfe = Enumerable.Range(0, 14)
            .Select(offset => VocabularyQuestionFactory.Create(entry, GradeLevel.Klasse9, new DateOnly(2026, 5, 4).AddDays(offset)))
            .Where(q => q.RequiresTurkishCharacters)
            .ToList();

        Assert.NotEmpty(mitHilfe);
        Assert.All(mitHilfe, q => Assert.Contains("Haus", q.Prompt));
    }
}

/// <summary>Vokabel-Speicherung und eigene Wiederholungs-Steuerung gegen echtes SQLite.</summary>
public sealed class VocabularyRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-vocab-{Guid.NewGuid():N}.db");

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
    public async Task Neue_Vokabeln_sind_sofort_faellig()
    {
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);

        var added = await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house\nBaum = tree"));

        Assert.Equal(2, added);
        Assert.Equal(2, (await repo.GetDueAsync("p1", Subject.Englisch, 10)).Count);
    }

    [Fact]
    public async Task Erneuter_Import_aktualisiert_statt_zu_doppeln()
    {
        // Eltern fügen eine korrigierte Liste ein zweites Mal ein - danach darf nicht jede
        // Vokabel doppelt im Bestand stehen.
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);

        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = hause"));
        var added = await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house"));

        Assert.Equal(0, added);
        var entry = Assert.Single(await repo.GetAllForProfileAsync("p1", Subject.Englisch));
        Assert.Equal("house", entry.Foreign);
    }

    [Fact]
    public async Task Faecher_und_Profile_bleiben_getrennt()
    {
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);

        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house"));
        await repo.AddOrUpdateManyAsync("p1", Subject.Tuerkisch, VocabularyParser.Parse("Haus = ev"));
        await repo.AddOrUpdateManyAsync("p2", Subject.Englisch, VocabularyParser.Parse("Auto = car"));

        Assert.Single(await repo.GetDueAsync("p1", Subject.Englisch, 10));
        Assert.Single(await repo.GetDueAsync("p1", Subject.Tuerkisch, 10));
        Assert.Empty(await repo.GetDueAsync("p3", Subject.Englisch, 10));
    }

    [Fact]
    public async Task Richtig_beantwortet_pausiert_die_Vokabel()
    {
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);
        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house"));

        var entry = (await repo.GetDueAsync("p1", Subject.Englisch, 10)).Single();
        await repo.RecordOutcomeAsync("p1", entry.Id, wasCorrect: true);

        // Erste Stufe: sieben Tage Pause - heute also nicht mehr fällig.
        Assert.Empty(await repo.GetDueAsync("p1", Subject.Englisch, 10));

        var stored = Assert.Single(await repo.GetAllForProfileAsync("p1"));
        Assert.Equal(1, stored.Stage);
        Assert.NotNull(stored.NextDueAt);
    }

    [Fact]
    public async Task Falsch_beantwortet_macht_sofort_wieder_faellig_ohne_zu_loeschen()
    {
        // Anders als bei der Fehler-Kartei verschwindet eine Vokabel nie - sie gehört zum Wortschatz.
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);
        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house"));

        var entry = (await repo.GetDueAsync("p1", Subject.Englisch, 10)).Single();
        await repo.RecordOutcomeAsync("p1", entry.Id, wasCorrect: true);
        await repo.RecordOutcomeAsync("p1", entry.Id, wasCorrect: false);

        Assert.Single(await repo.GetDueAsync("p1", Subject.Englisch, 10));

        var stored = Assert.Single(await repo.GetAllForProfileAsync("p1"));
        Assert.Equal(0, stored.Stage);
        Assert.Null(stored.NextDueAt);
        Assert.Equal(1, stored.WrongCount);
    }

    [Fact]
    public async Task Schwierige_Vokabeln_kommen_zuerst()
    {
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);
        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house\nBaum = tree"));

        var baum = (await repo.GetAllForProfileAsync("p1")).Single(v => v.German == "Baum");
        await repo.RecordOutcomeAsync("p1", baum.Id, wasCorrect: false);
        await repo.RecordOutcomeAsync("p1", baum.Id, wasCorrect: false);

        var due = await repo.GetDueAsync("p1", Subject.Englisch, 10);

        Assert.Equal("Baum", due[0].German);
    }

    [Fact]
    public async Task Loeschen_entfernt_nur_die_gewaehlte_Vokabel()
    {
        using var db = CreateContext();
        var repo = new VocabularyRepository(db);
        await repo.AddOrUpdateManyAsync("p1", Subject.Englisch, VocabularyParser.Parse("Haus = house\nBaum = tree"));

        var haus = (await repo.GetAllForProfileAsync("p1")).Single(v => v.German == "Haus");
        await repo.DeleteAsync(haus.Id);

        var remaining = Assert.Single(await repo.GetAllForProfileAsync("p1"));
        Assert.Equal("Baum", remaining.German);
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
            // Sqlite poolt Verbindungen, die Datei kann noch gesperrt sein - siehe CLAUDE.md.
        }
    }
}
