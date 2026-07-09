namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Konfiguration für die Anbindung an die NotebookLM-Enterprise-API (Google Cloud). Wird vom
/// Eltern-Bereich befüllt (siehe ParentSettingsViewModel) und in <see cref="NotebookLmQuestionSuggester"/>
/// gelesen. Absichtlich eine eigene, mutable Options-Klasse statt direkt <c>AppSettings</c> (LernTor.Data)
/// zu referenzieren: LernTor.ContentGen darf laut Architektur nicht von LernTor.Data abhängen.
/// </summary>
public sealed class NotebookLmOptions
{
    public string? ProjectId { get; set; }

    /// <summary>GCP-Region/Standort, z.B. "global" oder "us". Siehe NotebookLM-Enterprise-Doku.</summary>
    public string? Location { get; set; } = "global";

    /// <summary>Pfad zur JSON-Schlüsseldatei eines GCP-Dienstkontos (Service Account) auf der lokalen Festplatte.</summary>
    public string? ServiceAccountKeyPath { get; set; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ProjectId) &&
        !string.IsNullOrWhiteSpace(ServiceAccountKeyPath) &&
        File.Exists(ServiceAccountKeyPath);
}
