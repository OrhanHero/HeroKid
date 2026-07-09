namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Konfiguration für die Anbindung an die NotebookLM-Enterprise-API (Google Cloud). Wird vom
/// Eltern-Bereich befüllt (siehe ParentSettingsViewModel) und über <see cref="Llm.NotebookLmClient"/>
/// sowohl vom Lehrer-Import (<see cref="NotebookLmQuestionSuggester"/>) als auch vom KI-Lernchat
/// (<c>NotebookLmHomeworkHelpChatService</c>) gelesen - dieselben Zugangsdaten gelten für beide
/// Features. Absichtlich eine eigene, mutable Options-Klasse statt direkt <c>AppSettings</c>
/// (LernTor.Data) zu referenzieren: LernTor.ContentGen darf laut Architektur nicht von LernTor.Data
/// abhängen.
/// </summary>
public sealed class NotebookLmOptions
{
    /// <summary>
    /// Trotz des Namens laut offizieller Doku die GCP-Projekt-NUMMER (nicht die textuelle Projekt-ID) -
    /// der Ressourcenpfad eines Notebooks lautet "projects/PROJECT_NUMBER/locations/.../notebooks/...".
    /// Der Name blieb "ProjectId" für Konsistenz mit AppSettings/SettingsEntity; die UI-Beschriftung im
    /// Eltern-Bereich weist explizit auf die Projekt-Nummer hin.
    /// </summary>
    public string? ProjectId { get; set; }

    /// <summary>GCP-Region/Standort, z.B. "us" oder "eu" - wird als Präfix vor den Hostnamen gesetzt
    /// (z.B. "us-discoveryengine.googleapis.com"), siehe NotebookLM-Enterprise-Doku.</summary>
    public string? Location { get; set; } = "global";

    /// <summary>Pfad zur JSON-Schlüsseldatei eines GCP-Dienstkontos (Service Account) auf der lokalen Festplatte.</summary>
    public string? ServiceAccountKeyPath { get; set; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ProjectId) &&
        !string.IsNullOrWhiteSpace(ServiceAccountKeyPath) &&
        File.Exists(ServiceAccountKeyPath);
}
