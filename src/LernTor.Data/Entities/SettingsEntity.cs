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
    public bool StreaksEnabled { get; set; }

    /// <summary>Ferienmodus-Enddatum als ISO-String (yyyy-MM-dd), null = kein Pausenmodus -
    /// bewusst kulturneutraler String statt DateOnly (additive Spalte, sortier-/lesbar in SQLite).</summary>
    public string? PauseUntilDate { get; set; }
    public string? LocalLlmModelPath { get; set; }
    public string? LocalLlmModelKey { get; set; }
}
