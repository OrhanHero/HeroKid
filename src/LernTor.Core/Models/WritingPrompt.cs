namespace LernTor.Core.Models;

/// <summary>
/// Eine Schreibaufgabe für den täglichen Pflicht-Schreibabschnitt (5 Minuten Timer).
/// Jede Aufgabe hat einen Titel und einen Prompt-Text in drei Sprachen (DE/TR/EN).
/// Der Prompt ist ein offener Story-Anfang (Jugend-Abenteuer-Thema), der zum Weitererzählen einlädt.
/// </summary>
public sealed class WritingPrompt
{
    public required string Title { get; init; }

    public required string PromptDe { get; init; }
    public required string PromptTr { get; init; }
    public required string PromptEn { get; init; }
}