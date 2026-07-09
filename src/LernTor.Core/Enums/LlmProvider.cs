namespace LernTor.Core.Enums;

/// <summary>
/// Welcher LLM-Anbieter eine bestimmte KI-Funktion durchführt - verwendet sowohl vom automatischen
/// Einlesen von Lehrer-Unterlagen (LernTor.ContentGen.TeacherImport) als auch vom KI-Lernchat für
/// Kinder (LernTor.ContentGen.HomeworkChat). Von Eltern je Funktion unabhängig wählbar.
/// </summary>
public enum LlmProvider
{
    /// <summary>Google NotebookLM Enterprise (Cloud, benötigt GCP-Projekt + Dienstkonto).</summary>
    NotebookLm,

    /// <summary>Lokal geladenes GGUF-Modell (LLamaSharp/llama.cpp) - kein Cloud-Konto, läuft auf dem Kiosk-PC.</summary>
    LocalLlm
}
