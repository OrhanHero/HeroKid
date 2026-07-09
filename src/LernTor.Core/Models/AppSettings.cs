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

    /// <summary>GCP-Projekt-ID für die NotebookLM-Enterprise-Anbindung (automatisches Einlesen von
    /// Lehrer-Unterlagen). Null/leer = Funktion nicht konfiguriert.</summary>
    public string? NotebookLmProjectId { get; set; }

    /// <summary>GCP-Region/Standort für NotebookLM Enterprise, z.B. "global" oder "us".</summary>
    public string? NotebookLmLocation { get; set; } = "global";

    /// <summary>Pfad zur JSON-Schlüsseldatei eines GCP-Dienstkontos auf der lokalen Festplatte.</summary>
    public string? NotebookLmServiceAccountKeyPath { get; set; }

    /// <summary>Welcher LLM-Anbieter das automatische Einlesen von Lehrer-Unterlagen durchführt.</summary>
    public LlmProvider TeacherImportProvider { get; set; } = LlmProvider.NotebookLm;

    /// <summary>Pfad zu einer lokalen GGUF-Modelldatei für die lokale LLM-Alternative (LLamaSharp).
    /// Null/leer = Funktion nicht konfiguriert.</summary>
    public string? LocalLlmModelPath { get; set; }

    /// <summary>Welcher LLM-Anbieter den KI-Lernchat für Kinder durchführt (siehe Eltern-Features).
    /// Standard: lokal, damit Kinder-Fragen/Dokumenttexte im Standardfall nie den PC verlassen.</summary>
    public LlmProvider HomeworkChatProvider { get; set; } = LlmProvider.LocalLlm;
}
