using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class TypingContentProviderTests
{
    [Fact]
    public void Jede_Lektion_hat_fuer_jedes_Zeichen_einen_Finger()
    {
        // Das Finger-Mapping wird zeichenweise über den Zieltext gelegt. Waren beide Listen
        // unterschiedlich lang, zeigte die Fingeranzeige ab der Abweichung den falschen Finger -
        // hartkodierte Mapping-Listen waren genau deshalb in zwei Lektionen um drei Einträge zu kurz.
        foreach (var lesson in TypingContentProvider.GetAllLessons())
        {
            Assert.Equal(lesson.TargetText.Length, lesson.FingerMapping.Count);
        }

        var final = TypingContentProvider.GetFinalLessonForProfile("Emirhan");
        Assert.Equal(final.TargetText.Length, final.FingerMapping.Count);
    }

    [Fact]
    public void Keine_Lektion_verlangt_das_Semikolon_der_US_Tastatur()
    {
        // Auf QWERTZ liegt in der Grundreihe Ö, kein Semikolon - das gibt es nur über Umschalt+Komma.
        foreach (var lesson in TypingContentProvider.GetAllLessons())
        {
            Assert.False(lesson.TargetText.Contains(';'), $"'{lesson.Id}' verlangt ein Semikolon.");
        }
    }

    [Fact]
    public void Leerzeichen_gehoert_zum_Daumen()
    {
        var lesson = TypingContentProvider.GetLessonById("grundreihe_1");

        var spaceIndex = lesson.TargetText.IndexOf(' ');
        Assert.True(spaceIndex >= 0);
        Assert.Equal(TypingFinger.Thumb, lesson.FingerMapping[spaceIndex]);
    }

    [Fact]
    public void Grundreihe_uebt_die_deutsche_Belegung_asdf_jkloe()
    {
        var lesson = TypingContentProvider.GetLessonById("grundreihe_1");

        Assert.Contains("jklö", lesson.TargetText);
    }

    [Theory]
    // Auf QWERTZ liegt Y unten links (linker kleiner Finger) und Z oben rechts (rechter
    // Zeigefinger) - genau umgekehrt zu QWERTY. Komma und Punkt gehören zu Mittel- und Ringfinger.
    [InlineData('y', TypingFinger.LPinky)]
    [InlineData('z', TypingFinger.RIndex)]
    [InlineData(',', TypingFinger.RMiddle)]
    [InlineData('.', TypingFinger.RRing)]
    [InlineData('ö', TypingFinger.RPinky)]
    [InlineData('ä', TypingFinger.RPinky)]
    public void Finger_folgen_der_QWERTZ_Belegung(char zeichen, TypingFinger erwartet)
    {
        // Über einen eigenen Text prüfen, weil das Mapping nur als Teil einer Lektion öffentlich ist.
        var text = new string(zeichen, TypingTextOverrides.MinLength);
        var lesson = TypingContentProvider.GetFinalLessonForProfile("Emirhan", TypingTextOverrides.From(null, text));

        Assert.All(lesson.FingerMapping, finger => Assert.Equal(erwartet, finger));
    }

    [Fact]
    public void Eigener_Abschlusstext_ersetzt_den_eingebauten_Text()
    {
        const string eigener = "Ich heisse Mia und lerne jeden Tag ein bisschen mehr.";
        var overrides = TypingTextOverrides.From(null, eigener);

        var lesson = TypingContentProvider.GetFinalLessonForProfile("Emirhan", overrides);

        Assert.Equal(eigener, lesson.TargetText);
        Assert.Equal(eigener.Length, lesson.FingerMapping.Count);
        // Die Mindest-Zeichenzahl muss mitwandern, sonst haengt eine kurze eigene Uebung
        // an einer Vorgabe, die fuer den alten, laengeren Text galt.
        Assert.True(lesson.MinimumCharacters <= eigener.Length);
    }

    [Fact]
    public void Eigene_Texte_lassen_die_Aufbaulektionen_unangetastet()
    {
        // Grundreihe, Oberreihe, Unterreihe und Zahlen üben gezielt einzelne Tastenbereiche -
        // ein freier Elterntext würde diesen Zweck zerstören.
        var overrides = TypingTextOverrides.From(
            "Ein eigener Satz fuer die Satzlektion mit genug Zeichen.",
            "Ein eigener Abschlusstext mit ausreichend vielen Zeichen.");

        var original = TypingContentProvider.GetAllLessons();
        var angepasst = TypingContentProvider.GetAllLessons(overrides);

        foreach (var (alt, neu) in original.Zip(angepasst))
        {
            if (alt.Id == TypingContentProvider.SentenceLessonId)
            {
                Assert.NotEqual(alt.TargetText, neu.TargetText);
            }
            else
            {
                Assert.Equal(alt.TargetText, neu.TargetText);
            }
        }
    }

    [Fact]
    public void Ohne_eigene_Texte_bleibt_alles_beim_Alten()
    {
        Assert.Same(TypingContentProvider.GetAllLessons(), TypingContentProvider.GetAllLessons(TypingTextOverrides.None));
        Assert.Same(TypingContentProvider.GetAllLessons(), TypingContentProvider.GetAllLessons(null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("zu kurz")]
    public void Zu_kurze_Texte_werden_verworfen(string? eingabe)
    {
        // Sonst endet ein versehentlich halb geleertes Feld in einer Drei-Zeichen-Uebung.
        Assert.Null(TypingTextOverrides.Sanitize(eingabe));
    }

    [Fact]
    public void Zu_lange_Texte_werden_auf_das_Maximum_gekuerzt()
    {
        var zuLang = new string('a', TypingTextOverrides.MaxLength + 50);

        var bereinigt = TypingTextOverrides.Sanitize(zuLang);

        Assert.NotNull(bereinigt);
        Assert.Equal(TypingTextOverrides.MaxLength, bereinigt!.Length);
    }

    [Fact]
    public void Zeilenumbrueche_und_Mehrfach_Leerzeichen_werden_zusammengefasst()
    {
        // Ein mehrzeiliger Text wuerde in der Uebung als unsichtbare Zeichen auftauchen,
        // die das Kind gar nicht tippen kann.
        var bereinigt = TypingTextOverrides.Sanitize("Erste Zeile\n\nZweite   Zeile mit Abstand");

        Assert.Equal("Erste Zeile Zweite Zeile mit Abstand", bereinigt);
    }

    [Fact]
    public void Naechste_Lektion_nach_den_Saetzen_ist_der_Abschluss()
    {
        var next = TypingContentProvider.GetNextLesson(TypingContentProvider.SentenceLessonId, "Batuhan");

        Assert.NotNull(next);
        Assert.Equal(TypingLessonType.Abschluss, next!.LessonType);
    }
}
