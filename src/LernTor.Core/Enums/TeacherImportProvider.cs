namespace LernTor.Core.Enums;

/// <summary>
/// Welcher LLM-Anbieter das automatische Einlesen von Lehrer-Unterlagen (siehe
/// LernTor.ContentGen.TeacherImport) durchführt. Von Eltern im Eltern-Bereich wählbar.
/// </summary>
public enum TeacherImportProvider
{
    /// <summary>Google NotebookLM Enterprise (Cloud, benötigt GCP-Projekt + Dienstkonto).</summary>
    NotebookLm,

    /// <summary>Lokal geladenes GGUF-Modell (LLamaSharp/llama.cpp) - kein Cloud-Konto, läuft auf dem Kiosk-PC.</summary>
    LocalLlm
}
