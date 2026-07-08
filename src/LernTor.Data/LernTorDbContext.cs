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
}
