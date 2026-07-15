namespace LernTor.Data.Entities;

/// <summary>
/// Eine von den Eltern selbst eingetragene Aufgabe (z.B. aktuelle Hausaufgabe der Lehrkraft) -
/// ergänzt die generierten Aufgaben aus LernTor.ContentGen, ersetzt sie aber nicht.
/// Subject/GradeLevel/Type werden als String gespeichert (nicht als int), damit ein künftiges
/// Umsortieren/Erweitern der Enums bestehende Einträge nicht stillschweigend verfälscht.
/// </summary>
public sealed class CustomQuestionEntity
{
    public string Id { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;

    /// <summary>JSON-serialisierte Antwortoptionen (nur bei MultipleChoice/TrueFalse, sonst "[]").</summary>
    public string OptionsJson { get; set; } = "[]";

    /// <summary>JSON-serialisierte Liste akzeptierter Antworten.</summary>
    public string CorrectAnswersJson { get; set; } = "[]";

    public string Explanation { get; set; } = string.Empty;

    /// <summary>Optionaler, vorab abrufbarer Tipp (Formel/Vorgehen, keine Lösung).</summary>
    public string? HelpHint { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
