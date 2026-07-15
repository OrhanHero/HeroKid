namespace LernTor.News;

/// <summary>
/// Vereinfacht Nachrichtentexte für Kinder (10-15 Jahre). Standardimplementierung ist
/// regelbasiert; ein optionales lokales LLM (z.B. Phi-3 über Ollama) kann später als
/// zweite Implementierung dieses Interfaces eingebunden werden, ohne den Rest anzufassen.
/// </summary>
public interface ITextSimplifier
{
    string Simplify(string rawText);
}
