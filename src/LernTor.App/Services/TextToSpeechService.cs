using System.Globalization;
using System.Speech.Synthesis;

namespace LernTor.App.Services;

/// <summary>
/// Vorlesefunktion über die Windows-eigene Sprachausgabe (System.Speech/SAPI) - komplett offline,
/// keine Cloud, kein zusätzlicher Download; passt zum Kein-Cloud-Prinzip der App. Genutzt vom
/// Lesen-Abschnitt, um die Texte in der jeweils gewählten Sprache vorlesen zu lassen.
///
/// <para>Sprachauswahl per <c>SelectVoiceByHints(..., culture)</c> (Signatur gegen die echte
/// System.Speech-8.0.0-DLL verifiziert): Windows bringt standardmäßig deutsche und englische Stimmen
/// mit, eine türkische ist häufig NICHT vorinstalliert (nachinstallierbar über Windows-Einstellungen →
/// Zeit und Sprache → Sprache → Türkisch hinzufügen, mit Sprachausgabe). Ist keine passende Stimme
/// installiert, wird still mit der Standardstimme weitergelesen statt zu crashen - falsch betonte
/// Vorlesung ist besser als eine Fehlermeldung mitten im Pflicht-Leseteil.</para>
/// </summary>
public sealed class TextToSpeechService : IDisposable
{
    private readonly SpeechSynthesizer _synthesizer = new();

    /// <summary>Wird bei jedem Start/Stopp gefeuert, damit der Vorlesen-Button seinen Zustand
    /// (▶/⏹) nachführen kann. SpeechSynthesizer-Ereignisse kommen über den SynchronizationContext
    /// des erzeugenden Threads, in WPF also auf dem UI-Thread an.</summary>
    public event Action<bool>? SpeakingChanged;

    public bool IsSpeaking { get; private set; }

    public TextToSpeechService()
    {
        _synthesizer.SetOutputToDefaultAudioDevice();
        // Etwas langsamer als Standard (Skala -10..10): kindgerechtes Vorlesetempo.
        _synthesizer.Rate = -1;
        _synthesizer.SpeakCompleted += (_, _) => SetSpeaking(false);
    }

    /// <summary>Startet das Vorlesen von <paramref name="text"/> in der Sprache
    /// <paramref name="cultureName"/> (z.B. "de-DE", "tr-TR", "en-US"); bricht eine eventuell
    /// laufende Ausgabe vorher ab.</summary>
    public void Speak(string text, string cultureName)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        _synthesizer.SpeakAsyncCancelAll();

        try
        {
            _synthesizer.SelectVoiceByHints(
                VoiceGender.NotSet, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo(cultureName));
        }
        catch (Exception)
        {
            // Keine Stimme für diese Sprache installiert - Standardstimme weiterverwenden (s.o.).
        }

        _synthesizer.SpeakAsync(text);
        SetSpeaking(true);
    }

    public void Stop()
    {
        _synthesizer.SpeakAsyncCancelAll();
        SetSpeaking(false);
    }

    private void SetSpeaking(bool value)
    {
        if (IsSpeaking == value)
        {
            return;
        }

        IsSpeaking = value;
        SpeakingChanged?.Invoke(value);
    }

    public void Dispose() => _synthesizer.Dispose();
}
