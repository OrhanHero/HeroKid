using LernTor.Core.Enums;

namespace LernTor.News;

/// <summary>
/// Vereinfacht Nachrichtentexte für Kinder (10-15 Jahre). Standardimplementierung ist
/// regelbasiert; ein optionales lokales LLM (z.B. Phi-3 über Ollama) kann später als
/// zweite Implementierung dieses Interfaces eingebunden werden, ohne den Rest anzufassen.
/// </summary>
public interface ITextSimplifier
{
    /// <param name="rawText">Rohtext aus dem RSS-Feed.</param>
    /// <param name="gradeLevel">Klassenstufe des Kindes (6 oder 9) für altersgerechte Anpassung.</param>
    string Simplify(string rawText, GradeLevel gradeLevel);
}
