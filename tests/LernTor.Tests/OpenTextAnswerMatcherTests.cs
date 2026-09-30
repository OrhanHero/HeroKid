using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Offene Antworten: seit 30.09.2026 "entspricht" statt "enthält". Die Ablehnungs-Fälle sind
/// die echten Lücken der alten Prüfung - jede davon hat ein Kind raten lassen.
/// </summary>
public sealed class OpenTextAnswerMatcherTests
{
    [Theory]
    [InlineData("5", "25")]                       // Ziffer in einer anderen Zahl
    [InlineData("5", "-5")]
    [InlineData("5", "15")]
    [InlineData("5", "5 oder 6")]                  // zweite Zahl als Einheit getarnt
    [InlineData("gelse", "gelse gelir geliyor")]   // alle Formen auf einmal
    [InlineData("kitabım", "kitabımız")]           // längere Wortform: "unser" statt "mein"
    [InlineData("flew", "flew swam")]
    [InlineData("der", "die der")]                 // Lösung ist selbst ein Artikel
    [InlineData("-2,5", "-2.5")]                   // zwei Lösungen sind keine Dezimalzahl
    [InlineData("3/4", "3 4")]
    [InlineData("am wärmsten", "wärmsten")]
    public void Raten_und_falsche_Formen_zaehlen_nicht(string loesung, string antwort) =>
        Assert.False(OpenTextAnswerMatcher.Matches(loesung, antwort));

    [Theory]
    [InlineData("5", " 5 ")]
    [InlineData("42", "42 €")]
    [InlineData("42", "42€")]
    [InlineData("42", "42 Euro")]
    [InlineData("320", "320 cm³")]
    [InlineData("9", "x = 9")]
    [InlineData("-19", "- 19")]
    [InlineData("0.5", "0,5")]                      // Komma statt Punkt bei Dezimalzahlen
    [InlineData("-2,5", "-2, 5")]
    [InlineData("x=3,y=-6", "x = 3, y = -6")]
    [InlineData("(5,1)", "(5, 1)")]
    [InlineData("x² + 18x + 81", "x^2+18x+81")]
    [InlineData("36π", "36 pi")]
    [InlineData("Tea is drunk in England.", "tea is drunk in England")]
    [InlineData("isn't it", "isn’t it?")]
    [InlineData("isn't it", "isnt it")]
    [InlineData("walk / am taking", "walk, am taking")]
    [InlineData("Hund", "der Hund")]
    [InlineData("swim", "to swim")]
    [InlineData("kitabım", "kitabim")]               // ohne türkische Tastatur
    [InlineData("çiçekler", "CICEKLER")]
    [InlineData("gesehen hatten", "\"gesehen hatten\"")]
    public void Gleichwertige_Schreibweisen_zaehlen(string loesung, string antwort) =>
        Assert.True(OpenTextAnswerMatcher.Matches(loesung, antwort));

    [Fact]
    public void Der_ganze_Lueckensatz_zaehlt_aber_nur_genau_dieser()
    {
        const string prompt = "Setze die richtige Form ein: \"At the moment, I ___ (write) an email.\"";

        Assert.True(OpenTextAnswerMatcher.Matches("am writing", "At the moment, I am writing an email.", prompt));
        Assert.False(OpenTextAnswerMatcher.Matches("am writing", "At the moment, I write an email.", prompt));
        Assert.False(OpenTextAnswerMatcher.Matches("am writing", "At the moment, I am writing am writing an email.", prompt));
    }

    [Fact]
    public void Frageanhaengsel_lassen_sich_nicht_mit_beiden_Formen_raten()
    {
        const string prompt = "Ergänze das Frageanhängsel (question tag): \"Lena can't come tomorrow, ___?\"";

        Assert.True(OpenTextAnswerMatcher.Matches("can she", "Lena can't come tomorrow, can she?", prompt));
        Assert.False(OpenTextAnswerMatcher.Matches("can she", "can't she can she", prompt));
    }

    [Fact]
    public void Mehrere_Luecken_im_ganzen_Satz()
    {
        const string prompt = "Setze die richtige Form ein: \"I usually ___ (walk) to school, but today I ___ (take) the bus.\"";

        Assert.True(OpenTextAnswerMatcher.Matches("walk / am taking",
            "I usually walk to school, but today I am taking the bus.", prompt));
    }

    [Fact]
    public void Gross_und_Kleinschreibung_zaehlt_nur_wo_sie_geuebt_wird()
    {
        Assert.False(OpenTextAnswerMatcher.Matches("Auto", "auto", caseSensitive: true));
        Assert.True(OpenTextAnswerMatcher.Matches("Auto", "Auto", caseSensitive: true));
        Assert.True(OpenTextAnswerMatcher.Matches("Auto", "das Auto", caseSensitive: true));
        Assert.True(OpenTextAnswerMatcher.Matches("Auto", "auto"));
    }

    [Fact]
    public void Deutsch_Gross_Kleinschreibung_verlangt_die_richtige_Schreibung()
    {
        var fragen = new GermanGenerator().Generate(GradeLevel.Klasse6, 200, new Random(3))
            .Where(q => q.Topic == "Groß- und Kleinschreibung")
            .ToList();

        Assert.NotEmpty(fragen);
        Assert.All(fragen, q => Assert.True(q.CaseSensitive));

        var grossGeschrieben = fragen.First(q => char.IsUpper(q.CorrectAnswers[0][0]));
        Assert.True(grossGeschrieben.CheckAnswer(grossGeschrieben.CorrectAnswers[0]));
        Assert.False(grossGeschrieben.CheckAnswer(grossGeschrieben.CorrectAnswers[0].ToLowerInvariant()));
    }

    public static IEnumerable<object[]> Generatoren => typeof(MathGenerator).Assembly.GetTypes()
        .Where(t => !t.IsAbstract && typeof(ExerciseGeneratorBase).IsAssignableFrom(t))
        .Select(t => new object[] { t.Name });

    /// <summary>
    /// Jede offene Aufgabe jedes Generators nimmt ihre eigene Lösung an - und lehnt sie ab,
    /// sobald eine andere Lösung desselben Themas dahintersteht ("Schrotschuss").
    /// </summary>
    [Theory]
    [MemberData(nameof(Generatoren))]
    public void Offene_Aufgaben_nehmen_ihre_Loesung_an_und_keinen_Schrotschuss(string generatorName)
    {
        var typ = typeof(MathGenerator).Assembly.GetTypes().Single(t => t.Name == generatorName);
        var generator = (ExerciseGeneratorBase)Activator.CreateInstance(typ)!;
        var zufall = new Random(30);
        var fragen = new[] { GradeLevel.Klasse6, GradeLevel.Klasse7, GradeLevel.Klasse9 }
            .SelectMany(stufe => Enumerable.Range(0, 6).SelectMany(runde => generator.Generate(stufe, 40, zufall)))
            .Where(q => q.Type == QuestionType.OpenText)
            .ToList();

        foreach (var frage in fragen)
        {
            foreach (var loesung in frage.CorrectAnswers)
            {
                Assert.True(frage.CheckAnswer(loesung), $"'{frage.Prompt}' nimmt die eigene Lösung '{loesung}' nicht an.");
            }

            // Nur eine Lösung derselben Art: "16 Potsdam" ist kein Raten - hinter einer Zahl darf
            // ein Wort stehen (Einheit), eine zweite Zahl nie.
            var istZahl = frage.CorrectAnswers[0].Any(char.IsDigit);
            var andere = fragen
                .Where(q => q.Topic == frage.Topic)
                .Select(q => q.CorrectAnswers[0])
                .Where(a => a.Any(char.IsDigit) == istZahl)
                .FirstOrDefault(a => !frage.CorrectAnswers.Any(l => OpenTextAnswerMatcher.Matches(l, a)));
            if (andere is not null)
            {
                var schrotschuss = $"{frage.CorrectAnswers[0]} {andere}";
                Assert.False(frage.CheckAnswer(schrotschuss), $"'{frage.Prompt}' nimmt '{schrotschuss}' an.");
            }
        }
    }

    [Fact]
    public void Binomische_Formeln_haben_die_richtige_Loesung()
    {
        var fragen = new MathGenerator().Generate(GradeLevel.Klasse9, 400, new Random(5))
            .Where(q => q.Topic == BinomischeFormel.Thema)
            .ToList();

        Assert.NotEmpty(fragen);
        foreach (var frage in fragen)
        {
            var m = System.Text.RegularExpressions.Regex.Match(frage.Prompt, @"\(x (?<vz>[+-]) (?<b>\d+)\)²");
            Assert.True(m.Success, frage.Prompt);
            var b = int.Parse(m.Groups["b"].Value);
            Assert.Equal($"x² {m.Groups["vz"].Value} {2 * b}x + {b * b}", frage.CorrectAnswers[0]);
        }
    }

    [Theory]
    [InlineData("Multipliziere aus (1. bzw. 2. binomische Formel): (x + 9)² = ?", "x² + 18x + 81")]
    [InlineData("Multipliziere aus (1. bzw. 2. binomische Formel): (x - 1)² = ?", "x² - 2x + 1")]
    public void Alte_Karteikarten_lassen_sich_korrigieren(string prompt, string richtig)
    {
        Assert.True(BinomischeFormel.TryKorrigieren(prompt, out var loesung, out var erklaerung));
        Assert.Equal(richtig, loesung);
        Assert.EndsWith(richtig, erklaerung);
    }
}
