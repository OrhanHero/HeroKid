using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

public sealed class StudentProfileRepository
{
    private readonly LernTorDbContext _db;

    public StudentProfileRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>Legt beim allerersten Start die beiden Standardprofile an, falls noch keine existieren.</summary>
    public async Task SeedDefaultProfilesIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Profiles.AnyAsync(cancellationToken))
        {
            return;
        }

        _db.Profiles.AddRange(
            new StudentProfileEntity
            {
                Id = StudentProfile.NewId(),
                Name = "Batuhan Kahraman",
                Age = 15,
                ClassLabel = "9a",
                GradeLevel = (int)GradeLevel.Klasse9,
                AvatarEmoji = "🚀",
                CreatedAt = DateTimeOffset.Now
            },
            new StudentProfileEntity
            {
                Id = StudentProfile.NewId(),
                Name = "Emirhan Kahraman",
                Age = 12,
                ClassLabel = "6c",
                GradeLevel = (int)GradeLevel.Klasse6,
                AvatarEmoji = "⚽",
                CreatedAt = DateTimeOffset.Now
            });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Sortierung erst nach dem Laden (in-memory): SQLite/EF Core kann ORDER BY auf
        // DateTimeOffset-Spalten nicht serverseitig übersetzen.
        var entities = await _db.Profiles.ToListAsync(cancellationToken);
        return entities.OrderBy(p => p.CreatedAt).Select(ToModel).ToList();
    }

    public async Task<StudentProfile> CreateAsync(string name, int? age, string? classLabel, GradeLevel gradeLevel, string avatarEmoji, CancellationToken cancellationToken = default)
    {
        var entity = new StudentProfileEntity
        {
            Id = StudentProfile.NewId(),
            Name = name,
            Age = age,
            ClassLabel = classLabel,
            GradeLevel = (int)gradeLevel,
            AvatarEmoji = string.IsNullOrWhiteSpace(avatarEmoji) ? StudentProfile.DefaultAvatar : avatarEmoji,
            CreatedAt = DateTimeOffset.Now
        };

        _db.Profiles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return ToModel(entity);
    }

    /// <summary>Schreibt verdiente Belohnungs-Sterne auf das Profil gut und liefert den neuen Gesamtstand.</summary>
    public async Task<int> AddStarsAsync(string profileId, int amount, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return 0;
        }

        entity.TotalStars += amount;
        await _db.SaveChangesAsync(cancellationToken);
        return entity.TotalStars;
    }

    private static StudentProfile ToModel(StudentProfileEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Age = entity.Age,
        ClassLabel = entity.ClassLabel,
        GradeLevel = (GradeLevel)entity.GradeLevel,
        AvatarEmoji = string.IsNullOrWhiteSpace(entity.AvatarEmoji) ? StudentProfile.DefaultAvatar : entity.AvatarEmoji,
        TotalStars = entity.TotalStars
    };
}
