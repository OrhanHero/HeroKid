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
                CreatedAt = DateTimeOffset.Now
            },
            new StudentProfileEntity
            {
                Id = StudentProfile.NewId(),
                Name = "Emirhan Kahraman",
                Age = 12,
                ClassLabel = "6c",
                GradeLevel = (int)GradeLevel.Klasse6,
                CreatedAt = DateTimeOffset.Now
            });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _db.Profiles.OrderBy(p => p.CreatedAt).ToListAsync(cancellationToken);
        return entities.Select(ToModel).ToList();
    }

    public async Task<StudentProfile> CreateAsync(string name, int? age, string? classLabel, GradeLevel gradeLevel, CancellationToken cancellationToken = default)
    {
        var entity = new StudentProfileEntity
        {
            Id = StudentProfile.NewId(),
            Name = name,
            Age = age,
            ClassLabel = classLabel,
            GradeLevel = (int)gradeLevel,
            CreatedAt = DateTimeOffset.Now
        };

        _db.Profiles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return ToModel(entity);
    }

    private static StudentProfile ToModel(StudentProfileEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Age = entity.Age,
        ClassLabel = entity.ClassLabel,
        GradeLevel = (GradeLevel)entity.GradeLevel
    };
}
