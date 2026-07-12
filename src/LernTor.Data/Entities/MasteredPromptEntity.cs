namespace LernTor.Data.Entities;

/// <summary>
/// Dauerhafter Ausschluss: eine Aufgabe, die dieses Profil schon einmal richtig beantwortet hat -
/// in Übungen oder im Abschlussquiz. Anders als das 21-Tage-Fenster in
/// <see cref="ActivityLogEntity"/> (nur "kürzlich gestellt", bevorzugt Frische) und anders als die
/// Fehler-Kartei (<see cref="ReviewQuestionEntity"/>, nur FALSCH beantwortete Aufgaben) verhindert
/// dieser Eintrag, dass exakt derselbe Fragetext einem Profil JEMALS wieder gestellt wird, sobald
/// er einmal richtig beantwortet wurde. Nur der Prompt-Text wird gespeichert (kein Schnappschuss
/// der ganzen Frage nötig) - er dient ausschließlich als Ausschlusskriterium bei der
/// Aufgabenauswahl (siehe ExerciseGeneratorBase.Generate).
/// </summary>
public sealed class MasteredPromptEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTimeOffset MasteredAt { get; set; }
}
