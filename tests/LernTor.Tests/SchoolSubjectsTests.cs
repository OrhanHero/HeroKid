using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Welche Bereiche der App wirklich Schulfächer sind. Entscheidet, was in den Auswahllisten für
/// Klausuren und Hausaufgaben steht.
/// </summary>
public sealed class SchoolSubjectsTests
{
    [Theory]
    [InlineData(Subject.News)]
    [InlineData(Subject.Tippen)]
    [InlineData(Subject.KiWissen)]
    [InlineData(Subject.Fuehrerschein)]
    [InlineData(Subject.ErsteHilfe)]
    public void LernTor_eigene_Bereiche_sind_keine_Schulfaecher(Subject bereich)
    {
        // Fuer diese gibt die Schule nichts auf, und "Klausur in Fuehrerschein" ist kein Termin,
        // den es gibt. In der Auswahlliste standen sie trotzdem.
        Assert.False(SchoolSubjects.IsSchoolSubject(bereich));
        Assert.DoesNotContain(bereich, SchoolSubjects.All);
    }

    [Theory]
    [InlineData(Subject.Mathematik)]
    [InlineData(Subject.Deutsch)]
    [InlineData(Subject.Tuerkisch)]
    [InlineData(Subject.Englisch)]
    [InlineData(Subject.Itg)]
    [InlineData(Subject.Gewi)]
    public void Echte_Schulfaecher_sind_dabei(Subject fach)
    {
        Assert.True(SchoolSubjects.IsSchoolSubject(fach));
        Assert.Contains(fach, SchoolSubjects.All);
    }

    [Fact]
    public void Ein_neues_Schulfach_ist_automatisch_dabei()
    {
        // Die Liste ist ueber die AUSNAHMEN definiert, nicht ueber eine Aufzaehlung - sonst waere
        // sie eine weitere Stelle, die man beim Anlegen eines Fachs zu pflegen vergisst. Dieser
        // Test haelt genau das fest: alles ausser den benannten Ausnahmen zaehlt als Schulfach.
        var erwartet = Enum.GetValues<Subject>().Length - SchoolSubjects.NonSchool.Count;

        Assert.Equal(erwartet, SchoolSubjects.All.Count);
    }

    [Fact]
    public void Die_Reihenfolge_bleibt_die_der_Aufzaehlung()
    {
        var ausListe = SchoolSubjects.All.ToList();
        var ausAufzaehlung = Enum.GetValues<Subject>()
            .Where(SchoolSubjects.IsSchoolSubject)
            .ToList();

        Assert.Equal(ausAufzaehlung, ausListe);
    }
}
