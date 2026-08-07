namespace LernTor.Data.Entities;

public sealed class ProgressEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public DateOnly SessionDate { get; set; }
    public string CurrentStage { get; set; } = string.Empty;

    public bool HasCompletedReading { get; set; }

    /// <summary>
    /// Tipptrainer und Schreiben/Diktat des Tages erledigt.
    ///
    /// <para>Diese beiden Spalten fehlten lange. <c>StudentProgress</c> hatte die Merker,
    /// <c>ProgressGateService</c> las <c>HasCompletedTyping</c>, gespeichert wurde aber nur
    /// <c>HasCompletedReading</c> - nach einem Neustart (oder einem Absturz mitten in der Sitzung)
    /// stand der Tipptrainer wieder auf "nicht erledigt", und das Kind musste ihn noch einmal
    /// machen. Kein Fehler, keine Meldung, nur ein Tag, der laenger wurde.</para>
    ///
    /// <para>Beide sind <c>bool</c> mit dem gewuenschten Standardwert <c>false</c> - deshalb
    /// stehen sie hier ausnahmsweise NICHT invertiert (siehe DrivingAreaDisabled): der
    /// SqliteSchemaUpdater haengt an neue NOT-NULL-Spalten DEFAULT 0, und "noch nicht erledigt"
    /// ist fuer Bestandszeilen genau die richtige Auskunft.</para>
    /// </summary>
    public bool HasCompletedTyping { get; set; }

    public bool HasCompletedWriting { get; set; }

    /// <summary>JSON-serialisierte Liste abgeschlossener News-Artikel-IDs.</summary>
    public string CompletedNewsArticleIdsJson { get; set; } = "[]";

    /// <summary>JSON-serialisierte Liste abgeschlossener Fachbereiche.</summary>
    public string CompletedSubjectsJson { get; set; } = "[]";

    public int FinalQuizAttempts { get; set; }
    public double? LastQuizScore { get; set; }
    public bool IsUnlocked { get; set; }

    /// <summary>JSON-serialisierte Liste der zu wiederholenden Fachbereiche.</summary>
    public string SubjectsToRetryJson { get; set; } = "[]";

    /// <summary>Heute verdiente Belohnungs-Sterne (siehe MainViewModel.AwardStarsAsync).</summary>
    public int EarnedStarsToday { get; set; }

    public DateTimeOffset LastUpdatedAt { get; set; }
}
