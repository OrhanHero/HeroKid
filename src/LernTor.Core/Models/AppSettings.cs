using LernTor.Core.Enums;

namespace LernTor.Core.Models;

public sealed class AppSettings
{
    public string AdminPasswordHash { get; set; } = string.Empty;
    public string AdminPasswordSalt { get; set; } = string.Empty;

    public AppLanguage DefaultLanguage { get; set; } = AppLanguage.Deutsch;

    /// <summary>Fachbereiche, die Eltern temporär deaktivieren können (übersprungen, gelten als erledigt).</summary>
    public HashSet<Subject> DisabledSubjects { get; set; } = new();

    public bool HardLockShellReplacementEnabled { get; set; } = false;

    /// <summary>Pfad zu einer lokalen GGUF-Modelldatei für das lokale LLM (Lehrer-Import + KI-Lernchat,
    /// siehe LernTor.ContentGen.Llm.LocalLlmModelHost). Null/leer = Standardmodell wird beim ersten
    /// Gebrauch automatisch heruntergeladen.</summary>
    public string? LocalLlmModelPath { get; set; }
}
