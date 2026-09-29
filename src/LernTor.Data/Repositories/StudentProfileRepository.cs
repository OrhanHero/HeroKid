using System.Text.Json;
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

    /// <summary>
    /// Löscht ein Profil mitsamt allen zugehörigen Daten (Tagesfortschritt, Aktivitätsprotokoll,
    /// Quiz-Historie) - es gibt keine DB-seitigen Kaskaden-Regeln, daher explizit. Unumkehrbar;
    /// die Bestätigung holt der Eltern-Bereich vor dem Aufruf ein.
    /// </summary>
    public async Task DeleteAsync(string profileId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        // Jede Tabelle mit einer ProfileId-Spalte - aus dem EF-Modell, nicht aus einer Liste.
        // Hier standen lange nur Fortschritt, Protokoll und Quiz-Historie; Stundenplan,
        // Fehler-Kartei, gemeisterte Aufgaben, Vokabeln, Hausaufgaben, Klausuren, Tipptrainer-
        // und Fuehrerschein-Stand des geloeschten Kindes blieben verwaist in der Datenbank
        // liegen (gefunden am 28.09.2026) - entgegen dem, was die Rueckfrage verspricht.
        foreach (var (tabelle, spalte) in ProfileOwnedTables())
        {
            // Namen aus dem EF-Modell, Wert als Parameter - kein Injektionsweg.
            var sql = "DELETE FROM \"" + tabelle + "\" WHERE \"" + spalte + "\" = {0}";
            await _db.Database.ExecuteSqlRawAsync(sql, new object[] { profileId }, cancellationToken);
        }

        _db.Profiles.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Alle Tabellen, deren Zeilen einem Profil gehören, samt Name der ProfileId-Spalte.</summary>
    public IReadOnlyList<(string Tabelle, string Spalte)> ProfileOwnedTables()
    {
        var ergebnis = new List<(string Tabelle, string Spalte)>();

        foreach (var typ in _db.Model.GetEntityTypes())
        {
            var tabelle = typ.GetTableName();
            var eigenschaft = typ.FindProperty("ProfileId");
            if (tabelle is null || eigenschaft is null)
            {
                continue;
            }

            var spalte = eigenschaft.GetColumnName() ?? "ProfileId";
            if (!ergebnis.Any(eintrag => eintrag.Tabelle == tabelle))
            {
                ergebnis.Add((tabelle, spalte));
            }
        }

        return ergebnis;
    }

    /// <summary>
    /// Setzt Klassenstufe und Klassenbezeichnung eines Profils - und schreibt NUR diese beiden
    /// Spalten (Muster wie <see cref="SetPinnedReadingTextAsync"/>). Fuer den Schuljahreswechsel:
    /// vorher ging das nur ueber "Profil neu anlegen", und das kostete Sterne, gemeisterte Fragen,
    /// Fehler-Kartei, Stundenplan, Hausaufgaben und Klausurtermine.
    ///
    /// <para>Der Lernstand bleibt dabei stehen: gemeisterte Fragen und die Fehler-Kartei haengen
    /// am Fragetext, nicht an der Stufe, und der neue Pool bringt ohnehin andere Fragen.</para>
    /// </summary>
    public async Task SetGradeLevelAsync(
        string profileId, GradeLevel gradeLevel, string? classLabel, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.GradeLevel = (int)gradeLevel;
        entity.ClassLabel = string.IsNullOrWhiteSpace(classLabel) ? null : classLabel.Trim();
        await _db.SaveChangesAsync(cancellationToken);
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

    /// <summary>
    /// Speichert die von den Eltern im Eltern-Bereich pro Profil eingestellten Schwierigkeitsstufen
    /// (Tipptrainer-Mindestgenauigkeit, Abschlussquiz-Schwellenwerte für 1./2. Versuch).
    /// </summary>
    /// <summary>
    /// Setzt NUR den angehefteten Lesetext - und lässt jede andere Einstellung des Profils
    /// unangetastet.
    ///
    /// <para><b>Warum eine eigene Methode und nicht <see cref="UpdateSettingsAsync"/>:</b> die ist
    /// ein Voll-Überschreiber mit zwanzig Positionsparametern, von denen zehn optional sind. Der
    /// Aufruf im Eltern-Bereich übergab davon vierzehn - die restlichen sechs fielen still auf die
    /// Vorgabewerte der Signatur zurück. Einen Lesetext anzuheften hat damit die eingestellte
    /// Artikelzahl gelöscht, einen auf „Streng" gestellten Jugendschutzfilter auf „Normal"
    /// GELOCKERT, abgeschaltete Bereiche (Führerschein, Erste Hilfe) wieder eingeschaltet und alle
    /// abgewählten Schilderkategorien zurückgeholt. Kein Compilerfehler, kein fehlgeschlagener
    /// Test, keine Meldung - die Einstellungen waren einfach weg.</para>
    ///
    /// <para>Wer künftig ein einzelnes Feld ändern will, schreibt sich eine solche Methode dazu,
    /// statt <see cref="UpdateSettingsAsync"/> mit einer unvollständigen Argumentliste
    /// aufzurufen.</para>
    /// </summary>
    public async Task SetPinnedReadingTextAsync(
        string profileId, string? pinnedReadingTextKey, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.PinnedReadingTextKey = pinnedReadingTextKey;
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Schaltet die Fächerauswahl nach Stundenplan für ein Profil an oder aus - und schreibt
    /// dabei genau diese eine Spalte. Bewusst NICHT als weiterer Parameter von
    /// <see cref="UpdateSettingsAsync"/>: der Voll-Überschreiber mit seinen Positionsparametern
    /// hat schon einmal still Einstellungen zurückgesetzt (siehe
    /// <see cref="SetPinnedReadingTextAsync"/>).
    /// </summary>
    public async Task SetTimetableSubjectsEnabledAsync(
        string profileId, bool enabled, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        // Invertiert - siehe StudentProfileEntity.TimetableSubjectsDisabled.
        entity.TimetableSubjectsDisabled = !enabled;
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Schreibt ALLE Einstellungen eines Profils (siehe <see cref="ProfileSettings"/>). Nimmt ein
    /// Objekt mit lauter <c>required</c>-Eigenschaften statt der früheren zwanzig
    /// Positionsparameter - ein vergessenes Feld ist damit ein Compilerfehler statt einer still
    /// zurückgesetzten Einstellung. Wer nur ein Feld ändern will, nimmt eine der
    /// Ein-Spalten-Methoden oder <c>ProfileSettings.From(profil) with { … }</c>.
    /// </summary>
    public async Task UpdateSettingsAsync(
        string profileId, ProfileSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var entity = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        // Bereinigt speichern (Zeilenumbrüche raus, auf Maximallänge gekürzt), damit ein zu langer
        // oder zu kurzer Text gar nicht erst in die DB kommt.
        entity.CustomTypingSentenceText = TypingTextOverrides.Sanitize(settings.CustomTypingSentenceText);
        entity.CustomTypingFinalText = TypingTextOverrides.Sanitize(settings.CustomTypingFinalText);
        entity.WeeklyGoalDays = settings.WeeklyGoalDays;
        entity.PinnedReadingTextKey = settings.PinnedReadingTextKey;
        entity.TypingMinAccuracy = settings.TypingMinAccuracy;
        entity.QuizFirstAttemptThreshold = settings.QuizFirstAttemptThreshold;
        entity.QuizRetryThreshold = settings.QuizRetryThreshold;
        entity.ReadingMinutes = settings.ReadingMinutes;
        entity.NewsSecondsPerArticle = settings.NewsSecondsPerArticle;
        entity.NewsArticleCount = settings.NewsArticleCount;
        entity.NewsFilterStrictness = settings.NewsFilterStrictness.ToString();
        entity.ExerciseSecondsPerQuestion = settings.ExerciseSecondsPerQuestion;
        entity.ExercisesPerSubject = settings.ExercisesPerSubject;
        entity.QuizQuestionCount = settings.QuizQuestionCount;
        entity.QuizRetryQuestionCount = settings.QuizRetryQuestionCount;
        // Invertiert - siehe StudentProfileEntity.DrivingAreaDisabled.
        entity.DrivingAreaDisabled = !settings.DrivingAreaEnabled;
        entity.ErsteHilfeDisabled = !settings.ErsteHilfeEnabled;
        entity.DrivingChallengeSignCount = settings.DrivingChallengeSignCount;
        entity.DisabledSignCategoriesJson = JsonSerializer.Serialize(
            settings.DisabledSignCategories, JsonOptions.Default);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static StudentProfile ToModel(StudentProfileEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Age = entity.Age,
        ClassLabel = entity.ClassLabel,
        GradeLevel = (GradeLevel)entity.GradeLevel,
        AvatarEmoji = string.IsNullOrWhiteSpace(entity.AvatarEmoji) ? StudentProfile.DefaultAvatar : entity.AvatarEmoji,
        TotalStars = entity.TotalStars,
        TypingMinAccuracy = entity.TypingMinAccuracy,
        QuizFirstAttemptThreshold = entity.QuizFirstAttemptThreshold,
        QuizRetryThreshold = entity.QuizRetryThreshold,
        // Alt-Zeilen, deren Timer-Spalten erst per additivem Schema-Update (DEFAULT 0)
        // entstanden sind, laufen mit den bisherigen fest verdrahteten Standardwerten weiter.
        ReadingMinutes = entity.ReadingMinutes > 0 ? entity.ReadingMinutes : StudentProfile.DefaultReadingMinutes,
        NewsSecondsPerArticle = entity.NewsSecondsPerArticle > 0 ? entity.NewsSecondsPerArticle : StudentProfile.DefaultNewsSecondsPerArticle,
        NewsArticleCount = entity.NewsArticleCount > 0 ? entity.NewsArticleCount : StudentProfile.DefaultNewsArticleCount,
        // Alt-Zeilen haben hier den leeren String (additives Schema-Update) - der faellt auf
        // Normal zurueck, also auf das bisherige Verhalten.
        NewsFilterStrictness = Enum.TryParse<NewsFilterStrictness>(entity.NewsFilterStrictness, out var strictness)
            ? strictness
            : NewsFilterStrictness.Normal,
        ExerciseSecondsPerQuestion = entity.ExerciseSecondsPerQuestion > 0 ? entity.ExerciseSecondsPerQuestion : StudentProfile.DefaultExerciseSecondsPerQuestion,
        ExercisesPerSubject = entity.ExercisesPerSubject > 0 ? entity.ExercisesPerSubject : StudentProfile.DefaultExercisesPerSubject,
        QuizQuestionCount = entity.QuizQuestionCount > 0 ? entity.QuizQuestionCount : StudentProfile.DefaultQuizQuestionCount,
        QuizRetryQuestionCount = entity.QuizRetryQuestionCount > 0 ? entity.QuizRetryQuestionCount : StudentProfile.DefaultQuizRetryQuestionCount,
        WeeklyGoalDays = entity.WeeklyGoalDays,
        PinnedReadingTextKey = entity.PinnedReadingTextKey,
        CustomTypingSentenceText = entity.CustomTypingSentenceText,
        CustomTypingFinalText = entity.CustomTypingFinalText,
        DrivingAreaEnabled = !entity.DrivingAreaDisabled,
        ErsteHilfeEnabled = !entity.ErsteHilfeDisabled,
        TimetableSubjectsEnabled = !entity.TimetableSubjectsDisabled,
        DrivingChallengeSignCount = entity.DrivingChallengeSignCount > 0
            ? entity.DrivingChallengeSignCount
            : StudentProfile.DailySignChallengeDefaultCount,
        DisabledSignCategories = DeserializeCategories(entity.DisabledSignCategoriesJson)
    };

    /// <summary>Alt-Zeilen haben hier den leeren String (additives Schema-Update) - dann ist
    /// nichts ausgeblendet, also alle fuenf Gruppen aktiv.</summary>
    private static HashSet<TrafficSignCategory> DeserializeCategories(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new HashSet<TrafficSignCategory>();
        }

        try
        {
            return JsonSerializer.Deserialize<HashSet<TrafficSignCategory>>(json, JsonOptions.Default)
                   ?? new HashSet<TrafficSignCategory>();
        }
        catch (JsonException)
        {
            return new HashSet<TrafficSignCategory>();
        }
    }
}
