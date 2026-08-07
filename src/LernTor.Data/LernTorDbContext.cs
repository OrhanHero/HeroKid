using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data;

public sealed class LernTorDbContext : DbContext
{
    public DbSet<ProgressEntity> Progress => Set<ProgressEntity>();
    public DbSet<ActivityLogEntity> ActivityLog => Set<ActivityLogEntity>();
    public DbSet<QuizAttemptEntity> QuizAttempts => Set<QuizAttemptEntity>();
    public DbSet<SettingsEntity> Settings => Set<SettingsEntity>();
    public DbSet<StudentProfileEntity> Profiles => Set<StudentProfileEntity>();
    public DbSet<CustomQuestionEntity> CustomQuestions => Set<CustomQuestionEntity>();
    public DbSet<ReviewQuestionEntity> ReviewQuestions => Set<ReviewQuestionEntity>();
    public DbSet<MasteredPromptEntity> MasteredPrompts => Set<MasteredPromptEntity>();
    public DbSet<ArchivedArticleEntity> ArchivedArticles => Set<ArchivedArticleEntity>();
    public DbSet<RewardEntity> Rewards => Set<RewardEntity>();
    public DbSet<RewardRedemptionEntity> RewardRedemptions => Set<RewardRedemptionEntity>();
    public DbSet<TypingLessonProgressEntity> TypingLessonProgress => Set<TypingLessonProgressEntity>();
    public DbSet<CustomReadingTextEntity> CustomReadingTexts => Set<CustomReadingTextEntity>();
    public DbSet<VocabularyEntryEntity> VocabularyEntries => Set<VocabularyEntryEntity>();
    public DbSet<HomeworkTaskEntity> HomeworkTasks => Set<HomeworkTaskEntity>();
    public DbSet<ExamEntryEntity> Exams => Set<ExamEntryEntity>();
    public DbSet<TrafficSignProgressEntity> TrafficSignProgress => Set<TrafficSignProgressEntity>();
    public DbSet<TheoryAnswerEntity> TheoryAnswers => Set<TheoryAnswerEntity>();
    public DbSet<TheoryExamRunEntity> TheoryExamRuns => Set<TheoryExamRunEntity>();
    public DbSet<CourseLessonProgressEntity> CourseLessonProgress => Set<CourseLessonProgressEntity>();
    public DbSet<TimetableLessonEntity> TimetableLessons => Set<TimetableLessonEntity>();
    public DbSet<TimetablePeriodEntity> TimetablePeriods => Set<TimetablePeriodEntity>();

    public LernTorDbContext(DbContextOptions<LernTorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgressEntity>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => new { p.ProfileId, p.SessionDate });
        });

        modelBuilder.Entity<StudentProfileEntity>(e =>
        {
            e.HasKey(p => p.Id);
        });

        modelBuilder.Entity<ActivityLogEntity>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasIndex(a => a.Timestamp);
        });

        modelBuilder.Entity<QuizAttemptEntity>(e =>
        {
            e.HasKey(q => q.Id);
            e.HasIndex(q => q.Timestamp);
        });

        modelBuilder.Entity<SettingsEntity>(e =>
        {
            e.HasKey(s => s.Id);
        });

        modelBuilder.Entity<CustomQuestionEntity>(e =>
        {
            e.HasKey(c => c.Id);
        });

        modelBuilder.Entity<CustomReadingTextEntity>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.ProfileId);
        });

        modelBuilder.Entity<VocabularyEntryEntity>(e =>
        {
            e.HasKey(v => v.Id);
            e.HasIndex(v => new { v.ProfileId, v.Subject });
        });

        modelBuilder.Entity<HomeworkTaskEntity>(e =>
        {
            e.HasKey(h => h.Id);
            // Nach Profil UND Stichtag: die Kind-Ansicht fragt genau so ab ("was ist fuer mich
            // offen, zeitlich sortiert"), und DueDate ist als "yyyy-MM-dd" sortierbar.
            e.HasIndex(h => new { h.ProfileId, h.DueDate });
        });

        modelBuilder.Entity<ExamEntryEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.ProfileId, x.ExamDate });
        });

        modelBuilder.Entity<ReviewQuestionEntity>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => new { r.ProfileId, r.QuestionId });
        });

        modelBuilder.Entity<ArchivedArticleEntity>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasIndex(a => a.ArchivedDate);
        });

        modelBuilder.Entity<MasteredPromptEntity>(e =>
        {
            e.HasKey(m => m.Id);
            e.HasIndex(m => new { m.ProfileId, m.Prompt }).IsUnique();
        });

        modelBuilder.Entity<RewardEntity>(e =>
        {
            e.HasKey(r => r.Id);
        });

        modelBuilder.Entity<RewardRedemptionEntity>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.ProfileId);
        });

        modelBuilder.Entity<TypingLessonProgressEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => new { t.ProfileId, t.LessonId }).IsUnique();
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<TrafficSignProgressEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => new { t.ProfileId, t.SignNumber }).IsUnique();
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<TheoryAnswerEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => new { t.ProfileId, t.QuestionId }).IsUnique();
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<TheoryExamRunEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<CourseLessonProgressEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => new { t.ProfileId, t.LessonId }).IsUnique();
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<TimetableLessonEntity>(e =>
        {
            e.HasKey(t => t.Id);
            // Reiner Nachschlage-Index, bewusst ohne UNIQUE: die Eindeutigkeit je (Tag, Stunde)
            // stellt schon das Modell her (Timetable-Konstruktor), und ein zusaetzlicher
            // Datenbankzwang wuerde eine kuenftige Doppelstunde mit einer Ausnahme beim
            // Speichern quittieren statt mit einer Anzeige.
            e.HasIndex(t => new { t.ProfileId, t.Day });
            e.HasIndex(t => t.ProfileId);
        });

        modelBuilder.Entity<TimetablePeriodEntity>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => new { t.ProfileId, t.Period }).IsUnique();
        });
    }

    /// <summary>Pfad zur lokalen SQLite-Datenbank unter %LOCALAPPDATA%\LernTor\lerntor.db.</summary>
    public static string GetDefaultDbPath()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LernTor");
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, "lerntor.db");
    }

    /// <summary>Ordner für die automatischen Sicherungen (siehe <c>AutoBackupService</c>) -
    /// bewusst ein Unterordner, damit sie nicht neben der aktiven Datenbank liegen und beim
    /// Aufräumen von Hand nicht mit ihr verwechselt werden.</summary>
    public static string GetAutoBackupDirectory() =>
        Path.Combine(Path.GetDirectoryName(GetDefaultDbPath())!, "sicherungen");

    /// <summary>Datei mit dem zuletzt bekannten Schema-Fingerabdruck (siehe
    /// <c>SchemaFingerprint</c>).</summary>
    public static string GetSchemaFingerprintPath() =>
        Path.Combine(Path.GetDirectoryName(GetDefaultDbPath())!, "schema.fingerprint");
}
