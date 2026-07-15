using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Ein von <see cref="ITeacherQuestionSuggester"/> aus einem Lehrer-Dokument vorgeschlagener
/// Fragenentwurf. Bewusst kein <see cref="LernTor.Core.Models.QuizQuestion"/>: Ein Entwurf ist noch
/// unbestätigt, hat keine Id und alle Felder sind veränderlich, damit die Eltern ihn vor dem
/// Speichern im Eltern-Bereich korrigieren können (siehe README, Abschnitt "Automatisches Einlesen
/// von Lehrer-Unterlagen"). Erst nach expliziter Bestätigung wird daraus über
/// CustomQuestionRepository.AddAsync (LernTor.Data) eine echte
/// <see cref="LernTor.Core.Models.QuizQuestion"/>.
/// </summary>
public sealed class ExtractedQuestionDraft
{
    public Subject? SuggestedSubject { get; set; }
    public GradeLevel? SuggestedGradeLevel { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.OpenText;
    public List<string> Options { get; set; } = new();
    public List<string> CorrectAnswers { get; set; } = new();
    public string Explanation { get; set; } = string.Empty;
    public string? HelpHint { get; set; }

    /// <summary>
    /// Die Textstelle im Originaldokument, aus der dieser Entwurf abgeleitet wurde - wird den Eltern
    /// beim Review zusammen mit dem Entwurf angezeigt, damit sie die LLM-Interpretation gegen die
    /// Quelle prüfen können, statt dem Vorschlag blind vertrauen zu müssen.
    /// </summary>
    public string SourceExcerpt { get; set; } = string.Empty;
}
