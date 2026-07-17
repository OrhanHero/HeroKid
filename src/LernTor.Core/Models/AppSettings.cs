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

    /// <summary>Zeigt dem Kind die 🔥-Lernserie (aufeinanderfolgende Lerntage) auf dem
    /// Willkommensbildschirm. Standard AUS - bewusste Design-Entscheidung: kein Streak-Druck,
    /// Eltern schalten es explizit ein (siehe StreakCalculator).</summary>
    public bool StreaksEnabled { get; set; } = false;

    /// <summary>Pfad zu einer lokalen GGUF-Modelldatei für das lokale LLM (Lehrer-Import + KI-Lernchat,
    /// siehe LernTor.ContentGen.Llm.LocalLlmModelHost). Null/leer = das gewählte Katalog-Modell wird
    /// beim ersten Gebrauch automatisch heruntergeladen; gesetzt hat der Pfad Vorrang.</summary>
    public string? LocalLlmModelPath { get; set; }

    /// <summary>Schlüssel des im Eltern-Bereich gewählten Katalog-Modells (siehe
    /// LernTor.ContentGen.Llm.LocalLlmModelCatalog). Null/unbekannt = Standardmodell.</summary>
    public string? LocalLlmModelKey { get; set; }
}
