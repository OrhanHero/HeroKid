using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

public class MathGeneratorTests
{
    private readonly MathGenerator _generator = new();

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse7)]
    [InlineData(GradeLevel.Klasse9)]
    public void Generate_ReturnsRequestedCount(GradeLevel grade)
    {
        var questions = _generator.Generate(grade, 20, new Random(42));

        Assert.Equal(20, questions.Count);
        Assert.All(questions, q => Assert.False(string.IsNullOrWhiteSpace(q.Explanation)));
        Assert.All(questions, q => Assert.NotEmpty(q.CorrectAnswers));
    }

    [Fact]
    public void GeneratedAnswers_AreVerifiableAsCorrect()
    {
        // Für jede generierte Frage muss die eigene "richtige" Antwort auch als richtig erkannt werden.
        var random = new Random(1234);
        var questions = _generator.Generate(GradeLevel.Klasse6, 50, random)
            .Concat(_generator.Generate(GradeLevel.Klasse7, 50, random))
            .Concat(_generator.Generate(GradeLevel.Klasse9, 50, random));

        foreach (var question in questions)
        {
            var correctAnswer = question.CorrectAnswers[0];
            Assert.True(question.CheckAnswer(correctAnswer), $"Frage '{question.Prompt}' akzeptiert eigene Lösung '{correctAnswer}' nicht.");
        }
    }

    [Fact]
    public void GeneratedAnswers_RejectObviouslyWrongAnswer()
    {
        var random = new Random(999);
        var questions = _generator.Generate(GradeLevel.Klasse9, 30, random);

        foreach (var question in questions)
        {
            Assert.False(question.CheckAnswer("völlig-falsche-antwort-xyz"));
        }
    }

    [Fact]
    public void Generate_UnbekannteHoehereStufe_faellt_auf_die_naechstniedrigere_zurueck()
    {
        // Übergangsregel der Basisklasse: eine Stufe ohne eigenen Themenpool nutzt die
        // nächstniedrigere vorhandene Stufe, statt gar nichts zu liefern (siehe
        // ExerciseGeneratorBase.Generate) - hier fällt die Fantasie-Stufe auf Klasse 9 zurück.
        var questions = _generator.Generate((GradeLevel)999, 5, new Random(7));

        Assert.Equal(5, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse9, q.GradeLevel));
    }

    [Fact]
    public void Generate_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = _generator.Generate(GradeLevel.Klasse7, 9, new Random(7));

        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Theory]
    [InlineData(GradeLevel.Klasse8, GradeLevel.Klasse7)]
    [InlineData(GradeLevel.Klasse10, GradeLevel.Klasse9)]
    public void Doppeljahrgang_nutzt_den_Pool_der_unteren_Stufe(GradeLevel profil, GradeLevel erwartet)
    {
        // Der Berliner Rahmenlehrplan ist in Doppeljahrgängen gegliedert (7/8 und 9/10).
        // Klasse 8 und 10 haben deshalb bewusst keine eigenen Pools - die Übergangsregel in
        // ExerciseGeneratorBase greift auf die passende untere Stufe zu.
        var questions = new MusikGenerator().Generate(profil, 5, new Random(7));

        Assert.Equal(5, questions.Count);
        Assert.All(questions, q => Assert.Equal(erwartet, q.GradeLevel));
    }

    [Fact]
    public void Jede_Klassenstufe_liefert_in_jedem_Fach_Aufgaben()
    {
        // Sicherheitsnetz: kein Profil darf in irgendeinem Fach vor einer leeren Aufgabenliste
        // stehen - egal welche Klassenstufe eingetragen ist.
        var generators = new IExerciseGenerator[]
        {
            new MathGenerator(), new GermanGenerator(), new TurkishGenerator(), new EnglischGenerator(),
            new BiologieGenerator(), new ChemieGenerator(), new PhysikGenerator(), new GeschichteGenerator(),
            new GewiGenerator(), new PolitikGenerator(), new GeoGenerator(), new EthikGenerator(),
            new KunstGenerator(), new MusikGenerator(), new ItgGenerator(), new KiWissenGenerator()
        };

        foreach (var generator in generators)
        {
            foreach (var grade in Enum.GetValues<GradeLevel>())
            {
                var questions = generator.Generate(grade, 5, new Random(7));
                Assert.True(questions.Count > 0,
                    $"{generator.GetType().Name} liefert für {grade} keine Aufgaben.");
            }
        }
    }

    [Fact]
    public void Deutsch_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new GermanGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Englisch_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new EnglischGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Theory]
    [InlineData(typeof(PolitikGenerator))]
    [InlineData(typeof(GeoGenerator))]
    [InlineData(typeof(EthikGenerator))]
    [InlineData(typeof(KunstGenerator))]
    [InlineData(typeof(MusikGenerator))]
    [InlineData(typeof(ItgGenerator))]
    public void Fach_mit_Klasse7_Pool_faellt_nicht_zurueck(Type generatorType)
    {
        var generator = (IExerciseGenerator)Activator.CreateInstance(generatorType)!;

        var questions = generator.Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Gewi_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new GewiGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Geschichte_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new GeschichteGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Chemie_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new ChemieGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Physik_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new PhysikGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Biologie_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new BiologieGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }

    [Fact]
    public void Tuerkisch_Klasse7_liefert_eigene_Klasse7_Themen_ohne_Rueckfall()
    {
        var questions = new TurkishGenerator().Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.Equal(12, questions.Count);
        Assert.All(questions, q => Assert.Equal(GradeLevel.Klasse7, q.GradeLevel));
    }
}
