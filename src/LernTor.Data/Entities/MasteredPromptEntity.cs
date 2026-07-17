namespace LernTor.Data.Entities;

/// <summary>
/// Gemeisterte Aufgabe mit Spaced-Repetition-Zeitplan: eine Aufgabe, die dieses Profil richtig
/// beantwortet hat - in Übungen oder im Abschlussquiz. Anders als das 21-Tage-Fenster in
/// <see cref="ActivityLogEntity"/> (nur "kürzlich gestellt", bevorzugt Frische) und anders als die
/// Fehler-Kartei (<see cref="ReviewQuestionEntity"/>, nur FALSCH beantwortete Aufgaben) schließt
/// dieser Eintrag den Fragetext von der Aufgabenauswahl aus - aber nicht mehr für immer: nach
/// wachsenden Intervallen (7/30/90 Tage, siehe SpacedRepetitionSchedule in Core) wird die Aufgabe
/// wieder fällig und kann zur Auffrischung erneut drankommen. Nur der Prompt-Text wird gespeichert
/// (kein Schnappschuss der ganzen Frage nötig) - er dient ausschließlich als Ausschlusskriterium
/// bei der Aufgabenauswahl (siehe ExerciseGeneratorBase.Generate).
/// </summary>
public sealed class MasteredPromptEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTimeOffset MasteredAt { get; set; }

    /// <summary>Meisterungs-Stufe (1 = einmal richtig, steigt mit jeder richtigen Auffrischung).
    /// 0 = Alt-Eintrag von vor der Spaced-Repetition-Umstellung (wird beim nächsten richtigen
    /// Beantworten auf eine echte Stufe gehoben).</summary>
    public int ReviewStage { get; set; }

    /// <summary>Zeitpunkt, ab dem die Aufgabe wieder gestellt werden darf. NULL = sofort fällig
    /// (Alt-Einträge von vor der Umstellung - dadurch werden ehemals "für immer" ausgeschlossene
    /// Aufgaben genau einmal wieder aufgefrischt und bekommen dann einen echten Zeitplan).</summary>
    public DateTimeOffset? NextDueAt { get; set; }
}
