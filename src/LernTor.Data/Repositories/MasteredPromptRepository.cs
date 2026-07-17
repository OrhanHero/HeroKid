using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Verwaltet gemeisterte Aufgaben mit Spaced-Repetition-Zeitplan (siehe
/// <see cref="MasteredPromptEntity"/> und SpacedRepetitionSchedule in Core): einmal richtig
/// beantwortet, ist der exakte Fragetext für dieses Profil ausgeschlossen, bis er nach 7/30/90
/// Tagen zur Auffrischung wieder fällig wird - Wissen zerfällt, "für immer gemeistert" gibt es
/// nicht. Eine falsch beantwortete Auffrischung löscht die Meisterung wieder (die Fehler-Kartei
/// übernimmt dann, bis die Aufgabe erneut gemeistert ist). Ergänzt (statt ersetzt) das bestehende
/// 21-Tage-Fenster aus <see cref="ActivityLogRepository.GetRecentPromptsAsync"/>, das nur
/// kurzfristige Frische sicherstellt.
/// </summary>
public sealed class MasteredPromptRepository
{
    private readonly LernTorDbContext _db;

    public MasteredPromptRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Verbucht das Ergebnis einer beantworteten Aufgabe für den Spaced-Repetition-Zeitplan:
    /// richtig + noch nicht gemeistert → Stufe 1 (fällig in 7 Tagen); richtig + bereits gemeistert
    /// (Auffrischung) → Stufe hoch, nächste Fälligkeit nach 30/90 Tagen; falsch + bereits
    /// gemeistert → Meisterung verfällt (Eintrag wird gelöscht, die Fehler-Kartei kümmert sich um
    /// die Wiederholung, bis die Aufgabe neu gemeistert wird). News-Fragen werden wie in der
    /// Fehler-Kartei ausgenommen: sie beziehen sich auf Tagesartikel, die exakt so ohnehin nicht
    /// wiederkehren.
    /// </summary>
    public async Task RecordOutcomeAsync(string profileId, QuizQuestion question, bool wasCorrect, CancellationToken cancellationToken = default)
    {
        if (question.Subject == Subject.News)
        {
            return;
        }

        var entity = await _db.MasteredPrompts.FirstOrDefaultAsync(
            m => m.ProfileId == profileId && m.Prompt == question.Prompt, cancellationToken);

        if (!wasCorrect)
        {
            if (entity is not null)
            {
                _db.MasteredPrompts.Remove(entity);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return;
        }

        var now = DateTimeOffset.Now;
        if (entity is null)
        {
            _db.MasteredPrompts.Add(new MasteredPromptEntity
            {
                ProfileId = profileId,
                Prompt = question.Prompt,
                Subject = question.Subject.ToString(),
                MasteredAt = now,
                ReviewStage = 1,
                NextDueAt = SpacedRepetitionSchedule.NextDueAt(1, now)
            });
        }
        else
        {
            // Auffrischung bestanden (bzw. Alt-Eintrag mit Stufe 0 von vor der Umstellung):
            // Stufe hoch, nächstes Intervall länger.
            entity.ReviewStage = Math.Max(entity.ReviewStage, 0) + 1;
            entity.NextDueAt = SpacedRepetitionSchedule.NextDueAt(entity.ReviewStage, now);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Alle Fragetexte, die für dieses Profil AKTUELL ausgeschlossen sind: gemeistert und noch
    /// nicht wieder fällig. Fällige Einträge (NextDueAt erreicht oder NULL bei Alt-Einträgen von
    /// vor der Spaced-Repetition-Umstellung) fehlen bewusst im Ergebnis - sie dürfen zur
    /// Auffrischung wieder gestellt werden (siehe ExerciseGeneratorBase.Generate).
    /// </summary>
    public async Task<IReadOnlySet<string>> GetMasteredPromptsAsync(string profileId, CancellationToken cancellationToken = default)
    {
        // Erst laden, dann filtern (in-memory): SQLite/EF Core kann Vergleiche auf
        // DateTimeOffset-Spalten nicht zuverlässig serverseitig übersetzen.
        var entities = await _db.MasteredPrompts
            .Where(m => m.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        var now = DateTimeOffset.Now;
        return entities
            .Where(m => m.NextDueAt is not null && m.NextDueAt > now)
            .Select(m => m.Prompt)
            .ToHashSet();
    }
}
