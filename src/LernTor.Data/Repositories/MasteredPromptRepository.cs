using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data.Repositories;

/// <summary>
/// Verwaltet dauerhaft ausgeschlossene Aufgaben (siehe <see cref="MasteredPromptEntity"/>):
/// einmal richtig beantwortet, taucht der exakte Fragetext für dieses Profil nie wieder auf -
/// weder in Übungen noch im Abschlussquiz. Ergänzt (statt ersetzt) das bestehende 21-Tage-Fenster
/// aus <see cref="ActivityLogRepository.GetRecentPromptsAsync"/>, das nur kurzfristige Frische
/// sicherstellt.
/// </summary>
public sealed class MasteredPromptRepository
{
    private readonly LernTorDbContext _db;

    public MasteredPromptRepository(LernTorDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Verbucht eine richtig beantwortete Aufgabe als dauerhaft gemeistert. Falsch beantwortete
    /// Aufgaben werden ignoriert (die Fehler-Kartei kümmert sich um deren Wiederholung). News-Fragen
    /// werden wie in der Fehler-Kartei ausgenommen: sie beziehen sich auf Tagesartikel, die exakt so
    /// ohnehin nicht wiederkehren. Idempotent - ein bereits gemeisterter Prompt wird nicht doppelt
    /// eingetragen.
    /// </summary>
    public async Task RecordIfCorrectAsync(string profileId, QuizQuestion question, bool wasCorrect, CancellationToken cancellationToken = default)
    {
        if (!wasCorrect || question.Subject == Subject.News)
        {
            return;
        }

        var alreadyMastered = await _db.MasteredPrompts.AnyAsync(
            m => m.ProfileId == profileId && m.Prompt == question.Prompt, cancellationToken);

        if (alreadyMastered)
        {
            return;
        }

        _db.MasteredPrompts.Add(new MasteredPromptEntity
        {
            ProfileId = profileId,
            Prompt = question.Prompt,
            Subject = question.Subject.ToString(),
            MasteredAt = DateTimeOffset.Now
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Alle Fragetexte, die dieses Profil je richtig beantwortet hat - zum Ausschluss bei
    /// der Aufgabenauswahl (siehe ExerciseGeneratorBase.Generate).</summary>
    public async Task<IReadOnlySet<string>> GetMasteredPromptsAsync(string profileId, CancellationToken cancellationToken = default)
    {
        return (await _db.MasteredPrompts
            .Where(m => m.ProfileId == profileId)
            .Select(m => m.Prompt)
            .ToListAsync(cancellationToken))
            .ToHashSet();
    }
}
