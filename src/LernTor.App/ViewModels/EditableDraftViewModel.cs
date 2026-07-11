using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.ContentGen.TeacherImport;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Bearbeitbarer Wrapper um einen KI-Fragenentwurf (<see cref="ExtractedQuestionDraft"/>) für das
/// Inline-Editing im Eltern-Bereich: Eltern können Frage, Optionen, richtige Antwort(en), Erklärung
/// und Tipp direkt in der Vorschlagskarte korrigieren, bevor sie übernehmen - vorher ging nur
/// ganz-übernehmen oder ganz-verwerfen. Listen werden als kommagetrennte Texte editiert (dieselbe
/// Konvention wie im manuellen Eigene-Aufgaben-Editor).
/// </summary>
public sealed partial class EditableDraftViewModel : ObservableObject
{
    private readonly ExtractedQuestionDraft _draft;

    [ObservableProperty]
    private string topic;

    [ObservableProperty]
    private string prompt;

    /// <summary>Antwortoptionen, kommagetrennt (bei OpenText leer/irrelevant).</summary>
    [ObservableProperty]
    private string optionsText;

    /// <summary>Akzeptierte richtige Antwort(en), kommagetrennt.</summary>
    [ObservableProperty]
    private string correctAnswersText;

    [ObservableProperty]
    private string explanation;

    [ObservableProperty]
    private string helpHint;

    [ObservableProperty]
    private string validationError = string.Empty;

    public QuestionType Type => _draft.Type;
    public bool NeedsOptions => Type != QuestionType.OpenText;

    /// <summary>Unverändert aus dem Dokument - die Belegstelle bleibt bewusst nicht editierbar.</summary>
    public string SourceExcerpt => _draft.SourceExcerpt;

    public EditableDraftViewModel(ExtractedQuestionDraft draft)
    {
        _draft = draft;
        topic = draft.Topic;
        prompt = draft.Prompt;
        optionsText = string.Join(", ", draft.Options);
        correctAnswersText = string.Join(", ", draft.CorrectAnswers);
        explanation = draft.Explanation;
        helpHint = draft.HelpHint ?? string.Empty;
    }

    /// <summary>
    /// Baut aus dem (ggf. korrigierten) Stand eine speicherbare Frage; null + gesetzte
    /// <see cref="ValidationError"/>, wenn Pflichtangaben fehlen oder die richtige Antwort bei
    /// Auswahlfragen keine der Optionen (mehr) ist.
    /// </summary>
    public QuizQuestion? TryBuildQuestion(Subject subject, GradeLevel gradeLevel)
    {
        ValidationError = string.Empty;

        if (string.IsNullOrWhiteSpace(Prompt))
        {
            ValidationError = Localization.LocalizationService.Instance["Parent_Import_ErrorPromptMissing"];
            return null;
        }

        var correctAnswers = Split(CorrectAnswersText);
        if (correctAnswers.Count == 0)
        {
            ValidationError = Localization.LocalizationService.Instance["Parent_Import_ErrorAnswerMissing"];
            return null;
        }

        var options = NeedsOptions ? Split(OptionsText) : Array.Empty<string>();
        if (NeedsOptions)
        {
            if (options.Count < 2)
            {
                ValidationError = Localization.LocalizationService.Instance["Parent_Import_ErrorOptionsMissing"];
                return null;
            }

            // Dieselbe Konsistenzregel, die auch der Import-Prompt vom Modell verlangt: bei
            // Auswahlfragen muss die richtige Antwort wörtlich unter den Optionen sein - sonst wäre
            // die Frage im Quiz unbeantwortbar.
            if (!correctAnswers.Any(a => options.Contains(a, StringComparer.OrdinalIgnoreCase)))
            {
                ValidationError = Localization.LocalizationService.Instance["Parent_Import_ErrorAnswerNotInOptions"];
                return null;
            }
        }

        return new QuizQuestion
        {
            Id = Guid.NewGuid().ToString("N"),
            Subject = _draft.SuggestedSubject ?? subject,
            GradeLevel = _draft.SuggestedGradeLevel ?? gradeLevel,
            Topic = string.IsNullOrWhiteSpace(Topic) ? "Import (KI)" : Topic.Trim(),
            Type = Type,
            Prompt = Prompt.Trim(),
            Options = options,
            CorrectAnswers = correctAnswers,
            Explanation = string.IsNullOrWhiteSpace(Explanation) ? "-" : Explanation.Trim(),
            HelpHint = string.IsNullOrWhiteSpace(HelpHint) ? null : HelpHint.Trim()
        };
    }

    private static IReadOnlyList<string> Split(string text) =>
        text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
