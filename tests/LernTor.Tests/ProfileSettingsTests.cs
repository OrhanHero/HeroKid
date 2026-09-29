using System.Collections;
using System.Reflection;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using Xunit;

namespace LernTor.Tests;

public class ProfileSettingsTests
{
    /// <summary>Ein Profil, in dem JEDE Einstellung vom Vorgabewert abweicht.</summary>
    private static StudentProfile Abweichend() => new()
    {
        Id = "p1",
        Name = "Test",
        GradeLevel = GradeLevel.Klasse9,
        TypingMinAccuracy = 0.85,
        QuizFirstAttemptThreshold = 0.75,
        QuizRetryThreshold = 0.5,
        ReadingMinutes = 9,
        NewsSecondsPerArticle = 33,
        NewsArticleCount = 4,
        NewsFilterStrictness = NewsFilterStrictness.Streng,
        ExerciseSecondsPerQuestion = 11,
        ExercisesPerSubject = 7,
        QuizQuestionCount = 30,
        QuizRetryQuestionCount = 12,
        CustomTypingSentenceText = "Satz",
        CustomTypingFinalText = "Abschluss",
        WeeklyGoalDays = 5,
        PinnedReadingTextKey = "fest:Test",
        DrivingAreaEnabled = false,
        ErsteHilfeEnabled = false,
        DrivingChallengeSignCount = 9,
        DisabledSignCategories = new HashSet<TrafficSignCategory> { Enum.GetValues<TrafficSignCategory>()[0] },
    };

    [Fact]
    public void From_kopiert_jede_Einstellung()
    {
        // Wer ProfileSettings eine Eigenschaft hinzufuegt und sie in From() vergisst, speichert
        // beim naechsten Aendern einer ANDEREN Einstellung den Vorgabewert - genau die Fehlerklasse,
        // die das Objekt abschaffen soll. Deshalb per Reflection ueber ALLE Eigenschaften.
        var profil = Abweichend();
        var einstellungen = ProfileSettings.From(profil);

        foreach (var eigenschaft in typeof(ProfileSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(e => e.Name != "EqualityContract"))
        {
            var gegenstueck = typeof(StudentProfile).GetProperty(eigenschaft.Name);
            Assert.True(gegenstueck is not null, $"StudentProfile hat keine Eigenschaft {eigenschaft.Name}.");

            var erwartet = gegenstueck!.GetValue(profil);
            var tatsaechlich = eigenschaft.GetValue(einstellungen);

            if (erwartet is IEnumerable menge and not string)
            {
                Assert.Equal(menge.Cast<object>().ToHashSet(), ((IEnumerable)tatsaechlich!).Cast<object>().ToHashSet());
            }
            else
            {
                Assert.Equal(erwartet, tatsaechlich);
            }
        }
    }

    [Fact]
    public void Jede_Eigenschaft_ist_required()
    {
        // Eine Eigenschaft ohne "required" haette wieder einen stillen Vorgabewert.
        var ohne = typeof(ProfileSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(e => e.Name != "EqualityContract")
            .Where(e => !e.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredMemberAttribute"))
            .Select(e => e.Name)
            .ToList();

        Assert.Empty(ohne);
    }

    [Fact]
    public void Kopie_der_Schildergruppen_ist_unabhaengig_vom_Profil()
    {
        var profil = Abweichend();
        var einstellungen = ProfileSettings.From(profil);

        profil.DisabledSignCategories.Clear();

        Assert.Single(einstellungen.DisabledSignCategories);
    }
}
