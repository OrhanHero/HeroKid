namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Konfiguration für die lokale LLM-Alternative zu NotebookLM (siehe <see cref="LocalLlmQuestionSuggester"/>):
/// ein GGUF-Modell wird direkt im App-Prozess geladen (LLamaSharp/llama.cpp), keine Cloud, kein Konto,
/// keine laufenden Kosten - dafür muss der Elternteil eine Modelldatei selbst herunterladen (nicht Teil
/// des Installers, typischerweise mehrere Gigabyte) und die App braucht mehr RAM/CPU-Zeit pro Antwort.
/// Eigene, mutable Options-Klasse aus demselben Architekturgrund wie <see cref="NotebookLmOptions"/>:
/// LernTor.ContentGen darf nicht von LernTor.Data abhängen.
/// </summary>
public sealed class LocalLlmOptions
{
    /// <summary>Pfad zu einer lokalen GGUF-Modelldatei (z.B. ein quantisiertes Llama-3- oder Mistral-Modell).</summary>
    public string? ModelPath { get; set; }

    /// <summary>Kontextfenstergröße in Token - je größer, desto mehr Dokumenttext passt hinein, aber desto mehr RAM wird gebraucht.</summary>
    public uint ContextSize { get; set; } = 4096;

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ModelPath) && File.Exists(ModelPath);
}
