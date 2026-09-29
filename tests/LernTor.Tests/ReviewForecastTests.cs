using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class ReviewForecastTests
{
    [Fact]
    public void Zaehlt_nur_Faecher_die_heute_geuebt_werden()
    {
        // Vorher stand hier "12", obwohl heute nur Mathe und Englisch dran waren und das Kind
        // vier Wiederholungen sah.
        var faellig = new Dictionary<Subject, int>
        {
            [Subject.Mathematik] = 1,
            [Subject.Englisch] = 3,
            [Subject.Biologie] = 5,
            [Subject.Geschichte] = 3,
        };
        var heute = new HashSet<Subject> { Subject.Mathematik, Subject.Englisch };

        Assert.Equal(4, ReviewForecast.CountForToday(faellig, heute.Contains));
    }

    [Fact]
    public void Je_Fach_hoechstens_so_viele_wie_der_Ablauf_ausgibt()
    {
        var faellig = new Dictionary<Subject, int> { [Subject.Mathematik] = 9, [Subject.Deutsch] = 2 };

        Assert.Equal(ReviewForecast.PerSubjectCap + 2, ReviewForecast.CountForToday(faellig, _ => true));
    }

    [Fact]
    public void Nichts_faellig_ergibt_null()
    {
        Assert.Equal(0, ReviewForecast.CountForToday(new Dictionary<Subject, int>(), _ => true));
        Assert.Equal(0, ReviewForecast.CountForToday(
            new Dictionary<Subject, int> { [Subject.Kunst] = 4 }, _ => false));
    }

    [Fact]
    public void Obergrenze_ist_die_des_Aufgabenablaufs()
    {
        // MainViewModel.BuildExerciseViewModelAsync liest dieselbe Konstante - aendert sich eine,
        // aendert sich die andere mit. Dieser Test haelt den Wert fest, damit eine Aenderung
        // bewusst passiert (die Startseite verspricht dem Kind genau diese Zahl).
        Assert.Equal(3, ReviewForecast.PerSubjectCap);
    }
}
