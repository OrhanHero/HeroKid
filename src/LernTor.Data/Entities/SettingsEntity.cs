namespace LernTor.Data.Entities;

/// <summary>Einzeilige Settings-Tabelle (immer Id=1) für Eltern-Konfiguration.</summary>
public sealed class SettingsEntity
{
    public int Id { get; set; } = 1;
    public string AdminPasswordHash { get; set; } = string.Empty;
    public string AdminPasswordSalt { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = "Deutsch";
    public string DisabledSubjectsJson { get; set; } = "[]";
    public bool HardLockShellReplacementEnabled { get; set; }
    public string? NotebookLmProjectId { get; set; }
    public string? NotebookLmLocation { get; set; } = "global";
    public string? NotebookLmServiceAccountKeyPath { get; set; }
    public string TeacherImportProvider { get; set; } = "NotebookLm";
    public string? LocalLlmModelPath { get; set; }
}
