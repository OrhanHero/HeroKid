using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data;
using LernTor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LernTor.Tests;

/// <summary>Profil-Einstellungen (Schwierigkeits- und Timer-Werte) gegen echte SQLite-Temp-Dateien
/// (Muster wie MasteredPromptRepositoryTests).</summary>
public sealed class StudentProfileRepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lerntor-profile-{Guid.NewGuid():N}.db");

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
    public async Task Neues_Profil_hat_die_Standard_Timerwerte()
    {
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);

        var profile = await repo.CreateAsync("Test", 12, "7a", GradeLevel.Klasse7, "🧒");

        Assert.Equal(StudentProfile.DefaultReadingMinutes, profile.ReadingMinutes);
        Assert.Equal(StudentProfile.DefaultNewsSecondsPerArticle, profile.NewsSecondsPerArticle);
        Assert.Equal(StudentProfile.DefaultExerciseSecondsPerQuestion, profile.ExerciseSecondsPerQuestion);
        Assert.Equal(StudentProfile.DefaultExercisesPerSubject, profile.ExercisesPerSubject);
        Assert.Equal(StudentProfile.DefaultQuizQuestionCount, profile.QuizQuestionCount);
        Assert.Equal(StudentProfile.DefaultQuizRetryQuestionCount, profile.QuizRetryQuestionCount);
    }

    [Fact]
    public async Task UpdateSettings_speichert_Timerwerte_und_liefert_sie_beim_Neuladen()
    {
        using (var db = CreateContext())
        {
            var repo = new StudentProfileRepository(db);
            var profile = await repo.CreateAsync("Test", 12, "7a", GradeLevel.Klasse7, "🧒");

            await repo.UpdateSettingsAsync(profile.Id, 0.5, 0.75, 0.5,
                readingMinutes: 8, newsSecondsPerArticle: 20, exerciseSecondsPerQuestion: 10,
                exercisesPerSubject: 8, quizQuestionCount: 25, quizRetryQuestionCount: 20);
        }

        using (var db = CreateContext())
        {
            var repo = new StudentProfileRepository(db);
            var reloaded = (await repo.GetAllAsync()).Single();

            Assert.Equal(8, reloaded.ReadingMinutes);
            Assert.Equal(20, reloaded.NewsSecondsPerArticle);
            Assert.Equal(10, reloaded.ExerciseSecondsPerQuestion);
            Assert.Equal(8, reloaded.ExercisesPerSubject);
            Assert.Equal(25, reloaded.QuizQuestionCount);
            Assert.Equal(20, reloaded.QuizRetryQuestionCount);
            Assert.Equal(0.5, reloaded.TypingMinAccuracy);
            Assert.Equal(0.75, reloaded.QuizFirstAttemptThreshold);
        }
    }

    [Fact]
    public async Task Alt_Zeile_mit_Timerwert_0_faellt_auf_die_Standardwerte_zurueck()
    {
        // Bestehende Datenbanken bekommen die neuen Timer-Spalten per additivem Schema-Update
        // mit DEFAULT 0 (SqliteSchemaUpdater) - solche Zeilen müssen sich wie vor der Einführung
        // verhalten (5 min / 10 s / 5 s), nicht mit Timer 0 sofort freischalten.
        using var db = CreateContext();
        var repo = new StudentProfileRepository(db);
        var profile = await repo.CreateAsync("Alt", null, null, GradeLevel.Klasse6, "🧒");

        var entity = db.Profiles.Single(p => p.Id == profile.Id);
        entity.ReadingMinutes = 0;
        entity.NewsSecondsPerArticle = 0;
        entity.ExerciseSecondsPerQuestion = 0;
        entity.ExercisesPerSubject = 0;
        entity.QuizQuestionCount = 0;
        entity.QuizRetryQuestionCount = 0;
        await db.SaveChangesAsync();

        var reloaded = (await repo.GetAllAsync()).Single();

        Assert.Equal(StudentProfile.DefaultReadingMinutes, reloaded.ReadingMinutes);
        Assert.Equal(StudentProfile.DefaultNewsSecondsPerArticle, reloaded.NewsSecondsPerArticle);
        Assert.Equal(StudentProfile.DefaultExerciseSecondsPerQuestion, reloaded.ExerciseSecondsPerQuestion);
        Assert.Equal(StudentProfile.DefaultExercisesPerSubject, reloaded.ExercisesPerSubject);
        Assert.Equal(StudentProfile.DefaultQuizQuestionCount, reloaded.QuizQuestionCount);
        Assert.Equal(StudentProfile.DefaultQuizRetryQuestionCount, reloaded.QuizRetryQuestionCount);
    }

    public void Dispose()
    {
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }
}
