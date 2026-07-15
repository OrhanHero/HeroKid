using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Ordnet jeden Artikel einer Rubrik zu (siehe <see cref="NewsCategory"/>). Themen-Rubriken
/// (KI, Spiele, Finanzen, Wetter) werden per Schlüsselwort in Titel+Zusammenfassung erkannt und
/// haben Vorrang - eine Minecraft-Meldung von tagesschau.de gehört in "Spiele", nicht in
/// "Deutschland". Greift kein Themen-Schlüsselwort, entscheidet die Region der Quelle
/// (Berlin-Feed → Berlin, Türkei-Feed → Türkei, internationale Quelle → Welt, sonst Deutschland).
/// Rein regelbasiert und damit offline, deterministisch und testbar - bewusst kein LLM pro
/// Artikel (zu langsam auf CPU für ein Nachrichten-Laden mit Dutzenden Artikeln).
/// </summary>
public static class NewsCategoryClassifier
{
    // Reihenfolge = Priorität: das erste Thema mit Treffer gewinnt. Spiele vor KI, damit
    // "KI-Gegner in Mario Kart" bei Spielen landet; Wetter zuletzt der Themen-Rubriken, weil
    // Wetter-Wörter ("Sturm") oft nur Beiwerk anderer Meldungen sind.
    private static readonly (NewsCategory Category, string[] Keywords)[] TopicKeywords =
    {
        (NewsCategory.Spiele, new[]
        {
            "Nintendo", "Minecraft", "Roblox", "Fortnite", "Pokémon", "Pokemon", "PlayStation",
            "Playstation", "Xbox", "Videospiel", "Computerspiel", "Gaming", "Spielkonsole",
            "Konsole", "Zelda", "Super Mario", "E-Sport", "Esport", "Steam Deck", "Gamescom"
        }),
        (NewsCategory.Ki, new[]
        {
            "Künstliche Intelligenz", "KI-", " KI ", "ChatGPT", "OpenAI", "Anthropic", "Claude",
            "Gemini", "Chatbot", "Sprachmodell", "Roboter", "Robotik", "Machine Learning",
            "maschinelles Lernen", "neuronale", "yapay zeka", "Deepfake", "Algorithmus"
        }),
        (NewsCategory.Finanzen, new[]
        {
            "Inflation", "Börse", "Aktie", "Aktien", "Zinsen", "Leitzins", "Taschengeld",
            "Preise steigen", "Preiserhöhung", "Wirtschaftswachstum", "Rezession", "Gehalt",
            "Mindestlohn", "Sparen", "Sparkonto", "Steuern", "Haushalt der Regierung", "enflasyon",
            "Kryptowährung", "Bitcoin"
        }),
        (NewsCategory.Wetter, new[]
        {
            "Unwetter", "Sturm", "Orkan", "Hitzewelle", "Hitzerekord", "Schneefall", "Schneechaos",
            "Gewitter", "Hochwasser", "Überschwemmung", "Glatteis", "Wetterdienst", "Dürre",
            "Waldbrand", "Starkregen", "Hagel"
        }),
    };

    /// <summary>Emoji je Rubrik - konstant gehalten, damit Kinder die Symbole wiedererkennen.</summary>
    public static string EmojiFor(NewsCategory category) => category switch
    {
        NewsCategory.Berlin => "🐻",
        NewsCategory.Deutschland => "🇩🇪",
        NewsCategory.Welt => "🌍",
        NewsCategory.Tuerkei => "🇹🇷",
        NewsCategory.Ki => "🤖",
        NewsCategory.Spiele => "🎮",
        NewsCategory.Finanzen => "💰",
        NewsCategory.Wetter => "⛅",
        _ => "📰"
    };

    public static NewsCategory Classify(string? title, string? summary, NewsCategory sourceDefault)
    {
        var text = $"{title} {summary}";

        foreach (var (category, keywords) in TopicKeywords)
        {
            if (keywords.Any(k => text.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                return category;
            }
        }

        return sourceDefault;
    }
}
