using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Zwei Fehler, die Einstellungen bzw. Fortschritt still verloren haben. Beide waren am
/// laufenden Code nachweisbar, aber von keinem Test erfasst - deshalb stehen sie hier.
/// </summary>
public sealed class ProfileSettingsPersistenceTests : IDisposable
{
    private readonly string _dbPath =
        Path.Combine(Path.GetTempPath(), $"lerntor-einstellungen-{Guid.NewGuid():N}.db");

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
    public async Task Einen_Lesetext_anzuheften_laesst_alle_anderen_Einstellungen_in_Ruhe()
    {
        // DER Fehler: SetPinnedReadingTextAsync ging ueber UpdateSettingsAsync, einen
        // Voll-Ueberschreiber mit zwanzig Positionsparametern, und uebergab davon vierzehn. Die
        // restlichen sechs fielen still auf die Vorgabewerte der Signatur zurueck. Einen Lesetext
        // anzuheften hat damit den Jugendschutzfilter von "Streng" auf "Normal" GELOCKERT und
        // abgeschaltete Bereiche wieder eingeschaltet.
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);

        var profil = await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒");

        await repo.UpdateSettingsAsync(
            profil.Id,
            typingMinAccuracy: 0.8,
            quizFirstAttemptThreshold: 0.7,
            quizRetryThreshold: 0.4,
            readingMinutes: 7,
            newsSecondsPerArticle: 20,
            exerciseSecondsPerQuestion: 8,
            exercisesPerSubject: 9,
            quizQuestionCount: 25,
            quizRetryQuestionCount: 18,
            customTypingSentenceText: null,
            customTypingFinalText: null,
            weeklyGoalDays: 4,
            pinnedReadingTextKey: null,
            newsArticleCount: 6,
            newsFilterStrictness: NewsFilterStrictness.Streng,
            drivingAreaEnabled: false,
            ersteHilfeEnabled: false,
            drivingChallengeSignCount: 3);

        await repo.SetPinnedReadingTextAsync(profil.Id, "lesetext-42");

        var danach = (await repo.GetAllAsync()).Single(p => p.Id == profil.Id);

        Assert.Equal("lesetext-42", danach.PinnedReadingTextKey);

        // Und alles andere steht noch genau so da:
        Assert.Equal(6, danach.NewsArticleCount);
        Assert.Equal(NewsFilterStrictness.Streng, danach.NewsFilterStrictness);
        Assert.False(danach.DrivingAreaEnabled);
        Assert.False(danach.ErsteHilfeEnabled);
        Assert.Equal(3, danach.DrivingChallengeSignCount);
        Assert.Equal(4, danach.WeeklyGoalDays);
        Assert.Equal(25, danach.QuizQuestionCount);
    }

    [Fact]
    public async Task Das_Anheften_laesst_sich_auch_wieder_aufheben()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);

        var profil = await repo.CreateAsync("Testkind", 12, "6a", GradeLevel.Klasse6, "🧒");

        await repo.SetPinnedReadingTextAsync(profil.Id, "lesetext-42");
        await repo.SetPinnedReadingTextAsync(profil.Id, null);

        var danach = (await repo.GetAllAsync()).Single(p => p.Id == profil.Id);
        Assert.Null(danach.PinnedReadingTextKey);
    }

    [Fact]
    public async Task Tipptrainer_und_Schreiben_ueberleben_einen_Neustart()
    {
        // DER Fehler: StudentProgress hatte die Merker, ProgressGateService las
        // HasCompletedTyping - gespeichert wurde aber nur HasCompletedReading. Nach einem
        // Neustart stand der Tipptrainer wieder auf "nicht erledigt", und das Kind machte ihn
        // noch einmal.
        using var db = CreateContext();
        var repo = new ProgressRepository(db);

        var stand = await repo.LoadOrCreateTodayAsync("p1");

        stand.HasCompletedReading = true;
        stand.HasCompletedTyping = true;
        stand.HasCompletedWriting = true;
        await repo.SaveAsync(stand);

        // Zweiter Kontext = das, was ein Neustart der App tut.
        using var neu = CreateContext();
        var wieder = await new ProgressRepository(neu).LoadOrCreateTodayAsync("p1");

        Assert.True(wieder.HasCompletedReading);
        Assert.True(wieder.HasCompletedTyping);
        Assert.True(wieder.HasCompletedWriting);
    }

    [Fact]
    public async Task Ein_frischer_Tag_faengt_bei_nichts_erledigt_an()
    {
        // Die neuen Spalten bekommen beim additiven Schema-Abgleich DEFAULT 0 - fuer
        // "noch nicht erledigt" ist das genau richtig, deshalb stehen sie ausnahmsweise NICHT
        // invertiert (anders als DrivingAreaDisabled).
        using var db = CreateContext();
        var repo = new ProgressRepository(db);

        var stand = await repo.LoadOrCreateTodayAsync("p2");

        Assert.False(stand.HasCompletedReading);
        Assert.False(stand.HasCompletedTyping);
        Assert.False(stand.HasCompletedWriting);
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
