using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Welche Bereiche für ein Kind ausfallen. Die Regel gab es einmal zu oft: die Etappen-Steuerung
/// kannte beide Schalter, das Abschlussquiz und der Etappen-Zähler nur den globalen. Deshalb
/// steht sie jetzt an einer Stelle - und deshalb wird sie hier geprüft.
/// </summary>
public sealed class SubjectAvailabilityTests
{
    private static readonly HashSet<Subject> Nichts = new();

    [Fact]
    public void Ohne_jeden_Schalter_faellt_nichts_aus()
    {
        var aus = SubjectAvailability.EffectiveDisabled(Nichts, drivingAreaEnabled: true, ersteHilfeEnabled: true);

        Assert.Empty(aus);
    }

    [Fact]
    public void Der_globale_Schalter_gilt_fuer_jeden_Bereich()
    {
        var global = new HashSet<Subject> { Subject.Musik, Subject.Kunst };

        var aus = SubjectAvailability.EffectiveDisabled(global, true, true);

        Assert.Equal(global, aus);
        Assert.True(SubjectAvailability.IsDisabled(Subject.Musik, global, true, true));
        Assert.False(SubjectAvailability.IsDisabled(Subject.Mathematik, global, true, true));
    }

    [Fact]
    public void Der_Profilschalter_fuer_den_Fuehrerschein_zaehlt_mit()
    {
        // GENAU DER FEHLER: das Abschlussquiz sah nur die globale Menge und stellte deshalb
        // Fuehrerschein-Fragen an ein Kind, fuer das der Bereich abgeschaltet war - in einem
        // Bereich also, den es an dem Tag nie gesehen hatte.
        var aus = SubjectAvailability.EffectiveDisabled(Nichts, drivingAreaEnabled: false, ersteHilfeEnabled: true);

        Assert.Contains(Subject.Fuehrerschein, aus);
        Assert.DoesNotContain(Subject.ErsteHilfe, aus);
        Assert.Single(aus);
    }

    [Fact]
    public void Der_Profilschalter_fuer_Erste_Hilfe_zaehlt_mit()
    {
        var aus = SubjectAvailability.EffectiveDisabled(Nichts, drivingAreaEnabled: true, ersteHilfeEnabled: false);

        Assert.Contains(Subject.ErsteHilfe, aus);
        Assert.Single(aus);
    }

    [Fact]
    public void Beide_Schalterarten_zusammen()
    {
        var global = new HashSet<Subject> { Subject.Physik };

        var aus = SubjectAvailability.EffectiveDisabled(global, drivingAreaEnabled: false, ersteHilfeEnabled: false);

        Assert.Equal(
            new HashSet<Subject> { Subject.Physik, Subject.Fuehrerschein, Subject.ErsteHilfe },
            aus);
    }

    [Fact]
    public void Die_Menge_sagt_immer_dasselbe_wie_die_Einzelabfrage()
    {
        // Das ist der eigentliche Punkt: die Menge wird AUS der Abfrage abgeleitet, damit die
        // beiden Formen nicht auseinanderlaufen koennen. Genau das war vorher der Fehler.
        var global = new HashSet<Subject> { Subject.Geo };

        foreach (var fahren in new[] { true, false })
        {
            foreach (var ersteHilfe in new[] { true, false })
            {
                var menge = SubjectAvailability.EffectiveDisabled(global, fahren, ersteHilfe);

                foreach (var fach in Enum.GetValues<Subject>())
                {
                    Assert.Equal(
                        SubjectAvailability.IsDisabled(fach, global, fahren, ersteHilfe),
                        menge.Contains(fach));
                }
            }
        }
    }

    [Fact]
    public void Ein_globaler_Schalter_auf_Fuehrerschein_wirkt_auch_bei_eingeschaltetem_Profil()
    {
        var global = new HashSet<Subject> { Subject.Fuehrerschein };

        Assert.True(SubjectAvailability.IsDisabled(Subject.Fuehrerschein, global, drivingAreaEnabled: true, ersteHilfeEnabled: true));
    }

    [Fact]
    public void Ohne_globale_Menge_bleibt_die_Auskunft_gueltig()
    {
        // Ein null-Wert darf nicht in eine Ausnahme laufen - der Aufrufer im ViewModel steht
        // zeitweise ohne geladenes Profil da.
        Assert.False(SubjectAvailability.IsDisabled(Subject.Mathematik, null, true, true));
        Assert.True(SubjectAvailability.IsDisabled(Subject.ErsteHilfe, null, true, ersteHilfeEnabled: false));
    }
}
