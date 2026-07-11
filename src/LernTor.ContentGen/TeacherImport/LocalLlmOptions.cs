namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Konfiguration für das lokale LLM (LLamaSharp/llama.cpp, siehe <see cref="Llm.LocalLlmModelHost"/>),
/// die einzige KI-Anbindung in LernTor - komplett lokal, keine Cloud, kein Konto, keine laufenden
/// Kosten, das Dokument/Gespräch verlässt nie den PC. Gilt sowohl für den Lehrer-Import
/// (<see cref="LocalLlmQuestionSuggester"/>) als auch für den KI-Lernchat
/// (<c>LocalLlmHomeworkHelpChatService</c>). <see cref="ModelPath"/> ist optional: ist er leer, lädt
/// <see cref="Llm.LocalLlmModelHost"/> beim ersten Gebrauch automatisch ein Standardmodell herunter,
/// damit Eltern nicht selbst eine Modelldatei suchen/herunterladen müssen. Eigene, mutable
/// Options-Klasse statt direkt <c>AppSettings</c> (LernTor.Data) zu referenzieren: LernTor.ContentGen
/// darf laut Architektur nicht von LernTor.Data abhängen.
/// </summary>
public sealed class LocalLlmOptions
{
    /// <summary>Schlüssel des gewählten Katalog-Modells (siehe <see cref="Llm.LocalLlmModelCatalog"/>),
    /// von den Eltern per Dropdown wählbar. Unbekannt/leer fällt auf das Standardmodell zurück.</summary>
    public string? ModelKey { get; set; } = Llm.LocalLlmModelCatalog.DefaultKey;

    /// <summary>Pfad zu einer lokalen GGUF-Modelldatei, falls die Eltern statt eines Katalog-Modells
    /// ein eigenes verwenden möchten. Gesetzt hat er Vorrang vor <see cref="ModelKey"/>.</summary>
    public string? ModelPath { get; set; }

    /// <summary>Kontextfenstergröße in Token - je größer, desto mehr Text passt in Aufgabenkontext/Chatverlauf, aber desto mehr RAM wird gebraucht.</summary>
    public uint ContextSize { get; set; } = 4096;
}
