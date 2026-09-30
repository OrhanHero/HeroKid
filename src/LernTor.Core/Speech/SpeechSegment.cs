using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Speech;

/// <summary>Die drei Vorlesesprachen - genau die, für die es Piper-Stimmen gibt (siehe
/// <c>PiperTtsEngine.Voices</c> in der App).</summary>
public enum SpeechLanguage
{
    Deutsch,
    Englisch,
    Tuerkisch
}

/// <summary>Ein Stück Text, das in EINER Sprache vorgelesen wird.</summary>
public sealed record SpeechSegment(string Text, SpeechLanguage Language)
{
    /// <summary>Kulturname für die Stimmenwahl ("de-DE", "en-US", "tr-TR").</summary>
    public string CultureName => SpeechLanguages.CultureName(Language);
}

public static class SpeechLanguages
{
    public static string CultureName(SpeechLanguage language) => language switch
    {
        SpeechLanguage.Englisch => "en-US",
        SpeechLanguage.Tuerkisch => "tr-TR",
        _ => "de-DE"
    };

    /// <summary>Die Sprache, in der ein Fach „spricht“: Englisch und Türkisch in ihrer Sprache,
    /// alles andere auf Deutsch. Nachrichtenfragen richten sich nach ihrem Text - die Quellen
    /// sind deutsch, türkisch und englisch, und der Artikel merkt sich seine Sprache nicht.</summary>
    public static SpeechLanguage ForQuestion(QuizQuestion question) => question.Subject switch
    {
        Subject.Englisch => SpeechLanguage.Englisch,
        Subject.Tuerkisch => SpeechLanguage.Tuerkisch,
        Subject.News => SpeechSegmenter.Classify(
            question.Prompt,
            question.RequiresTurkishCharacters ? SpeechLanguage.Tuerkisch : SpeechLanguage.Deutsch),
        _ => question.RequiresTurkishCharacters ? SpeechLanguage.Tuerkisch : SpeechLanguage.Deutsch
    };
}
