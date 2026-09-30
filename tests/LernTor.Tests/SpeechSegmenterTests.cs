using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Speech;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Vorlesen jeder Frage (docs/NAECHSTES-LEVEL-3-1.md, Schritt 1): welcher Teil einer Frage in
/// welcher Stimme gelesen wird. Die Beispiele stammen aus einem Probelauf über alle Generatoren -
/// jede Regel im <see cref="SpeechSegmenter"/> hat hier den Fall, an dem sie nötig wurde.
/// </summary>
public sealed class SpeechSegmenterTests
{
    private const SpeechLanguage De = SpeechLanguage.Deutsch;
    private const SpeechLanguage En = SpeechLanguage.Englisch;
    private const SpeechLanguage Tr = SpeechLanguage.Tuerkisch;

    private static QuizQuestion Frage(Subject fach, string prompt, params string[] optionen) => new()
    {
        Id = "t", Subject = fach, GradeLevel = GradeLevel.Klasse6, Topic = "Test", Prompt = prompt,
        Type = optionen.Length == 0 ? QuestionType.OpenText : QuestionType.MultipleChoice,
        Options = optionen, CorrectAnswers = optionen.Length == 0 ? new[] { "x" } : new[] { optionen[0] },
        Explanation = "-"
    };

    private static List<(SpeechLanguage Sprache, string Text)> Abschnitte(QuizQuestion frage) =>
        SpeechSegmenter.ForQuestion(frage, frage.Options).Select(a => (a.Language, a.Text)).ToList();

    [Fact]
    public void Englischer_Luckensatz_hat_deutsche_Anweisung_und_englischen_Satz()
    {
        var abschnitte = Abschnitte(Frage(Subject.Englisch,
            "Setze die richtige Form ein: \"She ___ (go) to school every day.\""));

        Assert.Equal(new[] { De, En }, abschnitte.Select(a => a.Sprache));
        Assert.Equal("Setze die richtige Form ein:", abschnitte[0].Text);
        Assert.Equal("She … (go) to school every day.", abschnitte[1].Text);
    }

    [Fact]
    public void Uebersetzung_ins_Englische_liest_das_deutsche_Wort_deutsch()
    {
        var abschnitte = Abschnitte(Frage(Subject.Englisch,
            "Wie sagt man auf Englisch \"Umweltschutz\"?", "environmental protection", "nature shop"));

        Assert.Equal(new[] { De, En }, abschnitte.Select(a => a.Sprache));
        Assert.Contains("Umweltschutz", abschnitte[0].Text);
        Assert.Equal("environmental protection. nature shop.", abschnitte[1].Text);
    }

    [Fact]
    public void Bedeutung_auf_Deutsch_liest_die_Antworten_deutsch()
    {
        var abschnitte = Abschnitte(Frage(Subject.Englisch,
            "Was bedeutet \"to get dressed\" auf Deutsch?", "sich anziehen", "aufstehen"));

        Assert.Equal(new[] { De, En, De }, abschnitte.Select(a => a.Sprache));
        Assert.Equal("to get dressed", abschnitte[1].Text);
        Assert.EndsWith("sich anziehen. aufstehen.", abschnitte[2].Text);
    }

    [Fact]
    public void Englische_Frage_ohne_Anfuehrungszeichen_bleibt_englisch()
    {
        var abschnitte = Abschnitte(Frage(Subject.Englisch, "___ did you go on holiday?", "Where", "Why"));

        Assert.Equal(new[] { En }, abschnitte.Select(a => a.Sprache));
        Assert.Equal("… did you go on holiday? Where. Why.", abschnitte[0].Text);
    }

    [Fact]
    public void Antwortmoeglichkeiten_werden_nicht_an_einzelnen_Woertern_eingeordnet()
    {
        // "ben" ist türkisch ("ich"), "her" englisch - beides darf die Stimme nicht wechseln.
        var englisch = Abschnitte(Frage(Subject.Englisch,
            "Welches berühmte Bauwerk steht in Südengland?", "Stonehenge", "Big Ben"));
        Assert.Equal(new[] { De, En }, englisch.Select(a => a.Sprache));

        var tuerkisch = Abschnitte(Frage(Subject.Tuerkisch,
            "Metin: \"Elif her cumartesi babaannesini ziyaret eder.\" Soru: Elif ne zaman gelir?",
            "Her cumartesi", "Her pazar"));
        Assert.Equal(new[] { Tr }, tuerkisch.Select(a => a.Sprache));
    }

    [Fact]
    public void Almancasi_verlangt_deutsche_Antworten()
    {
        var abschnitte = Abschnitte(Frage(Subject.Tuerkisch,
            "\"komşuluk\" kelimesinin Almancası hangisidir?", "Nachbarschaft", "Freundschaft"));

        Assert.Equal(new[] { Tr, De }, abschnitte.Select(a => a.Sprache));
        Assert.Equal("komşuluk kelimesinin Almancası hangisidir?", abschnitte[0].Text);
    }

    [Fact]
    public void Eine_Sprache_im_Lesetext_ist_keine_Uebersetzungsaufgabe()
    {
        var abschnitte = Abschnitte(Frage(Subject.Tuerkisch,
            "Metin: \"Murat Türkçe ve Almanca konuşuyor.\" Soru: Murat kaç dil konuşuyor?", "İki", "Üç"));

        Assert.Equal(new[] { Tr }, abschnitte.Select(a => a.Sprache));
    }

    [Fact]
    public void Beispiel_in_Klammern_liest_nur_das_Wort_Beispiel_deutsch()
    {
        var abschnitte = Abschnitte(Frage(Subject.Tuerkisch,
            "İyelik ekini ekle ve kelimeyi yaz: \"sizin kedi___\" (Beispiel: benim ev___ -> evim)"));

        Assert.Equal(new[] { Tr, De, Tr }, abschnitte.Select(a => a.Sprache));
        Assert.Equal("Beispiel:", abschnitte[1].Text);
        Assert.Equal("benim ev … , evim", abschnitte[2].Text);
    }

    [Fact]
    public void Wahr_und_Falsch_sind_immer_deutsch()
    {
        var frage = Frage(Subject.Englisch, "\"She have a dog.\" is correct.", "Wahr", "Falsch");

        Assert.Equal(new[] { En, De }, Abschnitte(frage).Select(a => a.Sprache));
    }

    [Fact]
    public void Deutsche_Faecher_bleiben_ganz_deutsch_auch_mit_Zitaten()
    {
        var abschnitte = Abschnitte(Frage(Subject.Deutsch,
            "Welches Wort passt: \"das\" oder \"dass\"? Ich hoffe, ___ du kommst.", "dass", "das"));

        Assert.Single(abschnitte);
        Assert.Equal(De, abschnitte[0].Sprache);
    }

    [Fact]
    public void Nachrichtenfragen_folgen_ihrem_Text()
    {
        Assert.Equal(En, SpeechLanguages.ForQuestion(Frage(Subject.News, "What did the scientists find on Mars?")));
        Assert.Equal(Tr, SpeechLanguages.ForQuestion(Frage(Subject.News, "Bu haberde hangi şehir anlatılıyor?")));
        Assert.Equal(De, SpeechLanguages.ForQuestion(Frage(Subject.News, "Welche Stadt kommt in der Nachricht vor?")));
    }

    [Theory]
    [InlineData("Berechne: 3/4 + 1/8 = ?", "Berechne: 3 durch 4 plus 1 durch 8 gleich ?")]
    [InlineData("Berechne: 18 + (-6) = ?", "Berechne: 18 plus (minus 6) gleich ?")]
    [InlineData("Löse: x - 3 = 5", "Löse: x minus 3 gleich 5")]
    [InlineData("Wie groß ist die Fläche in cm²?", "Wie groß ist die Fläche in cm hoch 2?")]
    [InlineData("Maßstab 1:1000", "Maßstab 1 zu 1000")]
    [InlineData("Wie viel sind 30% von 90?", "Wie viel sind 30 Prozent von 90?")]
    [InlineData("Welche Aussage stimmt - die erste oder die zweite?", "Welche Aussage stimmt - die erste oder die zweite?")]
    [InlineData("Um 8:30 Uhr beginnt die Schule.", "Um 8:30 Uhr beginnt die Schule.")]
    public void Rechenzeichen_werden_im_Deutschen_zu_Woertern(string text, string erwartet) =>
        Assert.Equal(erwartet, SpeechSegmenter.Speakable(text, De));

    [Fact]
    public void Loesung_zum_Anhoeren_hat_die_Sprache_der_Antwort()
    {
        var englisch = SpeechSegmenter.ForSolution(Frage(Subject.Englisch, "Setze ein: \"He ___ (eat) meat.\""));
        Assert.Equal(En, Assert.Single(englisch).Language);

        var deutsch = SpeechSegmenter.ForSolution(Frage(Subject.Englisch,
            "Was bedeutet \"rules\" auf Deutsch?", "Regeln", "Regale"));
        Assert.Equal(De, Assert.Single(deutsch).Language);
    }

    public static TheoryData<string> Generatoren()
    {
        var daten = new TheoryData<string>();
        foreach (var typ in typeof(MathGenerator).Assembly.GetTypes()
                     .Where(t => !t.IsAbstract && typeof(ExerciseGeneratorBase).IsAssignableFrom(t))
                     .OrderBy(t => t.Name))
        {
            daten.Add(typ.Name);
        }

        return daten;
    }

    [Theory]
    [MemberData(nameof(Generatoren))]
    public void Jede_Frage_jedes_Generators_laesst_sich_sauber_vorlesen(string generatorName)
    {
        var typ = typeof(MathGenerator).Assembly.GetTypes().Single(t => t.Name == generatorName);
        var generator = (ExerciseGeneratorBase)Activator.CreateInstance(typ)!;
        var zufall = new Random(31);
        var fragen = new[] { GradeLevel.Klasse6, GradeLevel.Klasse7, GradeLevel.Klasse9 }
            .SelectMany(stufe => generator.Generate(stufe, 60, zufall))
            .Where(q => q.Type != QuestionType.Diktat)
            .ToList();

        var mitFachsprache = 0;
        foreach (var frage in fragen)
        {
            var abschnitte = SpeechSegmenter.ForQuestion(frage, frage.Options);
            Assert.NotEmpty(abschnitte);

            foreach (var abschnitt in abschnitte)
            {
                Assert.True(abschnitt.Text.Any(char.IsLetterOrDigit), $"'{frage.Prompt}': leerer Abschnitt");
                Assert.DoesNotContain("__", abschnitt.Text);
                Assert.True(abschnitt.Text.IndexOfAny(['"', '„', '“', '”']) < 0,
                    $"'{frage.Prompt}': Anführungszeichen im Abschnitt '{abschnitt.Text}'");
            }

            var fach = SpeechLanguages.ForQuestion(frage);
            if (fach == De)
            {
                Assert.All(abschnitte, a => Assert.Equal(De, a.Language));
            }
            else if (abschnitte.Any(a => a.Language == fach))
            {
                mitFachsprache++;
            }
        }

        // Englisch- und Türkischfragen: fast jede hat einen Teil in der Fachsprache - sonst liest
        // der Zerleger die Sätze, um die es geht, mit der deutschen Stimme.
        var fremdsprachig = fragen.Count(q => SpeechLanguages.ForQuestion(q) != De);
        if (fremdsprachig > 0)
        {
            Assert.True(mitFachsprache >= fremdsprachig * 0.9,
                $"{generatorName}: nur {mitFachsprache} von {fremdsprachig} Fragen mit Fachsprache");
        }
    }
}
