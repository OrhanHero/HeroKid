namespace LernTor.ContentGen.Llm;

/// <summary>Ein im Eltern-Bereich wählbares, automatisch herunterladbares lokales Modell.
/// <paramref name="DownloadUrls"/> wird der Reihe nach probiert (Spiegel-Quellen), bis eine klappt.</summary>
public sealed record LocalLlmModelInfo(
    string Key,
    string DisplayName,
    string FileName,
    IReadOnlyList<string> DownloadUrls,
    double ApproxSizeGb);

/// <summary>
/// Kuratierte Auswahl lokaler GGUF-Modelle für den automatischen Download. Bewusst nur
/// Qwen-Instruct-Varianten (Apache-2.0): die Familie deckt als einzige im permissiv lizenzierten
/// CPU-tauglichen Bereich Deutsch UND Türkisch offiziell gut ab (29+ Sprachen), und Prompt-Format
/// samt AntiPrompts-Stoppwörtern ist in dieser Codebasis bereits gegen genau diese Familie erprobt -
/// ein Modellfamilien-Wechsel im Katalog hieße, beides neu testen zu müssen.
///
/// <para><b>Zur "Sehr stark"-Option Qwen3-30B-A3B:</b> ein Mixture-of-Experts-Modell - von den
/// 30 Mrd. Parametern sind pro Wort nur ~3 Mrd. aktiv, es antwortet auf CPU also ungefähr so
/// schnell wie ein 3B-Modell, mit deutlich besserer Qualität als das 7B-Standardmodell. Preis:
/// ~18,6 GB Download und ~24 GB freier RAM. Bewusst die allgemeine Instruct-Variante statt der
/// Coder-Variante (Qwen3-Coder-30B-A3B): Coder ist auf Programmieren spezialisiert und für
/// Deutsch/Türkisch-Lernhilfe die schwächere Wahl. Wer die Coder-Datei trotzdem nutzen will,
/// kann sie manuell herunterladen und im Eltern-Bereich als eigene Modelldatei auswählen.</para>
///
/// <para>Verworfene Alternativen (Stand der Trainingsdaten, huggingface.co ist aus der
/// Entwicklungsumgebung nicht erreichbar): Mistral-7B (Türkisch schwach), Llama-3.1-8B
/// (Llama-Lizenz, Türkisch mittelmäßig), Gemma-2-9B (Gemma-Lizenz), Qwen2.5-14B (Apache, aber ~9 GB
/// Q4 → auf Familien-CPUs nur noch 2-4 Token/s, zu langsam für den interaktiven Kinder-Chat).</para>
///
/// <para><b>Die Download-URLs sind aus der Entwicklungsumgebung heraus NICHT verifizierbar</b>
/// (huggingface.co blockiert, 403 bestätigt) und folgen dem offiziellen
/// Qwen/*-GGUF-Repo-Schema - beim ersten echten Test auf einem Windows-Rechner mit Internet
/// überprüfen. Schlägt ein Download fehl, bleibt der manuelle Datei-Dialog im Eltern-Bereich als
/// Ausweg (siehe LocalLlmModelHost).</para>
/// </summary>
public static class LocalLlmModelCatalog
{
    /// <summary>Standard: bestes Verhältnis aus Sprach-/Erklärqualität und CPU-Tauglichkeit.</summary>
    public const string DefaultKey = "qwen2.5-7b-q4";

    // Quellen-Reihenfolge: bartowski/*-GGUF zuerst - diese Community-Repos stellen verlässlich
    // EINZELDATEIEN bereit. Die offiziellen Qwen/*-GGUF-Repos teilen größere Dateien dagegen in
    // Teil-Dateien auf (qwen2.5-7b-...-00001-of-00002.gguf), dort existiert die Einzeldatei-URL
    // für 7B vermutlich gar nicht (beim Nutzer real fehlgeschlagen) - sie bleibt nur als
    // Zweitversuch für den 3B-Fall gelistet.
    public static readonly IReadOnlyList<LocalLlmModelInfo> Models = new[]
    {
        new LocalLlmModelInfo(
            Key: DefaultKey,
            DisplayName: "Standard – Qwen2.5 7B (beste Qualität, ~4,7 GB)",
            FileName: "qwen2.5-7b-instruct-q4_k_m.gguf",
            DownloadUrls: new[]
            {
                "https://huggingface.co/bartowski/Qwen2.5-7B-Instruct-GGUF/resolve/main/Qwen2.5-7B-Instruct-Q4_K_M.gguf"
            },
            ApproxSizeGb: 4.7),
        new LocalLlmModelInfo(
            Key: "qwen3-30b-a3b-q4",
            DisplayName: "Sehr stark – Qwen3 30B-A3B (MoE, ~18,6 GB; braucht ~24 GB RAM, antwortet trotzdem schnell)",
            FileName: "qwen3-30b-a3b-instruct-2507-q4_k_m.gguf",
            DownloadUrls: new[]
            {
                "https://huggingface.co/bartowski/Qwen_Qwen3-30B-A3B-Instruct-2507-GGUF/resolve/main/Qwen_Qwen3-30B-A3B-Instruct-2507-Q4_K_M.gguf",
                "https://huggingface.co/unsloth/Qwen3-30B-A3B-Instruct-2507-GGUF/resolve/main/Qwen3-30B-A3B-Instruct-2507-Q4_K_M.gguf"
            },
            ApproxSizeGb: 18.6),
        new LocalLlmModelInfo(
            Key: "qwen2.5-3b-q4",
            DisplayName: "Leicht & schnell – Qwen2.5 3B (~2 GB, für ältere PCs)",
            FileName: "qwen2.5-3b-instruct-q4_k_m.gguf",
            DownloadUrls: new[]
            {
                "https://huggingface.co/bartowski/Qwen2.5-3B-Instruct-GGUF/resolve/main/Qwen2.5-3B-Instruct-Q4_K_M.gguf",
                "https://huggingface.co/Qwen/Qwen2.5-3B-Instruct-GGUF/resolve/main/qwen2.5-3b-instruct-q4_k_m.gguf"
            },
            ApproxSizeGb: 2.0)
    };

    /// <summary>Liefert den Katalogeintrag zum Schlüssel, bei unbekanntem/leerem Schlüssel den Standard.</summary>
    public static LocalLlmModelInfo Resolve(string? key) =>
        Models.FirstOrDefault(m => m.Key == key) ?? Models[0];
}
