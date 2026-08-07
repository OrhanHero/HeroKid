using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Die Zuordnung Stundenplan-Bezeichnung → LernTor-Fach. Sie entscheidet ausschließlich über das
/// Symbol vor der Zeile; angezeigt wird immer der eingetragene Text. Geprüft wird deshalb vor
/// allem, dass sie NICHT rät.
/// </summary>
public sealed class TimetableSubjectMapTests
{
    [Theory]
    [InlineData("Ma", Subject.Mathematik)]
    [InlineData("Mathe", Subject.Mathematik)]
    [InlineData("mathematik", Subject.Mathematik)]
    [InlineData("De", Subject.Deutsch)]
    [InlineData("E", Subject.Englisch)]
    [InlineData("Englisch", Subject.Englisch)]
    [InlineData("ITG", Subject.Itg)]
    public void Bekannte_Bezeichnungen_werden_zugeordnet(string bezeichnung, Subject erwartet)
    {
        Assert.Equal(erwartet, TimetableSubjectMap.TryMap(bezeichnung));
    }

    [Fact]
    public void Ge_Geo_und_GeWi_sind_drei_verschiedene_Faecher()
    {
        // Ein Praefix-Vergleich haette alle drei durcheinandergebracht - deshalb wird die
        // vollstaendige Bezeichnung verglichen.
        Assert.Equal(Subject.Geschichte, TimetableSubjectMap.TryMap("Ge"));
        Assert.Equal(Subject.Geo, TimetableSubjectMap.TryMap("Geo"));
        Assert.Equal(Subject.Gewi, TimetableSubjectMap.TryMap("GeWi"));
    }

    [Theory]
    [InlineData("NaWi")]        // Bio + Chemie + Physik zugleich - eines herauszupicken waere geraten
    [InlineData("Sport")]
    [InlineData("WPU Spanisch")]
    [InlineData("Klassenrat")]
    [InlineData("Französisch")]
    [InlineData("")]
    [InlineData(null)]
    public void Was_LernTor_nicht_kennt_wird_auch_nicht_erfunden(string? bezeichnung)
    {
        Assert.Null(TimetableSubjectMap.TryMap(bezeichnung));
        Assert.Equal("📓", TimetableSubjectMap.IconFor(bezeichnung));
    }

    [Fact]
    public void Jedes_erkannte_Fach_bekommt_ein_eigenes_Symbol()
    {
        Assert.Equal("🔢", TimetableSubjectMap.IconFor("Mathe"));
        Assert.Equal("📖", TimetableSubjectMap.IconFor("Deutsch"));
        Assert.NotEqual("📓", TimetableSubjectMap.IconFor("Musik"));
    }
}
