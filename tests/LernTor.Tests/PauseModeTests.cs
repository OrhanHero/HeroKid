using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Ferien-/Pausenmodus. Realer Fehler: der Modus leuchtete im Eltern-Bereich gruen, das Kind
/// musste aber trotzdem bis zum Abschlussquiz durch - die Regel galt nur fuer die Kiosk-Sperre.
/// </summary>
public sealed class PauseModeTests
{
    private static readonly DateOnly Ferienende = new(2026, 8, 23);

    [Fact]
    public void Ohne_gesetztes_Datum_laeuft_keine_Pause()
    {
        Assert.False(PauseMode.IsActive(null, new DateOnly(2026, 8, 1)));
        Assert.Equal(0, PauseMode.RemainingDays(null, new DateOnly(2026, 8, 1)));
    }

    [Fact]
    public void Waehrend_der_Ferien_ist_die_Pause_aktiv()
    {
        Assert.True(PauseMode.IsActive(Ferienende, new DateOnly(2026, 8, 1)));
    }

    [Fact]
    public void Der_letzte_Tag_zaehlt_noch_dazu()
    {
        // "bis 23.08." heisst bis einschliesslich - wer das Datum eintraegt, meint diesen Tag mit.
        Assert.True(PauseMode.IsActive(Ferienende, Ferienende));
        Assert.Equal(1, PauseMode.RemainingDays(Ferienende, Ferienende));
    }

    [Fact]
    public void Am_Tag_danach_greift_die_Sperre_wieder_von_selbst()
    {
        // Eltern muessen nichts abschalten - genau das ist der Sinn des Enddatums.
        Assert.False(PauseMode.IsActive(Ferienende, Ferienende.AddDays(1)));
        Assert.Equal(0, PauseMode.RemainingDays(Ferienende, Ferienende.AddDays(1)));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(22, 23)]
    public void Restliche_Tage_schliessen_heute_mit_ein(int tageVorEnde, int erwartet)
    {
        var heute = Ferienende.AddDays(-tageVorEnde);

        Assert.Equal(erwartet, PauseMode.RemainingDays(Ferienende, heute));
    }

    [Fact]
    public void Ein_lange_abgelaufenes_Datum_haelt_niemanden_mehr_auf()
    {
        var altesDatum = new DateOnly(2024, 1, 1);

        Assert.False(PauseMode.IsActive(altesDatum, new DateOnly(2026, 8, 1)));
    }

    [Fact]
    public void Der_Berliner_Ferienkalender_entscheidet_NICHT_ueber_die_Sperre()
    {
        // Im Eltern-Bereich heissen ZWEI verschiedene Dinge "Ferien...": dieser Pausenmodus (ein
        // von Hand gesetztes Datum, das die Kiosk-Sperre aussetzt) und die reine Anzeige der
        // Berliner Schulferien (SchoolCalendar). Beim Testen wurde genau das verwechselt - die
        // App liess sich ueber das X in der Alt+Tab-Vorschau schliessen, und der Verdacht fiel
        // zuerst auf den Kalender.
        //
        // Der 07.08.2026 liegt mitten in den Berliner Sommerferien. Fuer den Pausenmodus ist das
        // ohne Bedeutung, und das soll so bleiben: die Sperre stillschweigend ueber sechs Wochen
        // Sommerferien auszusetzen, weil ein Kalendereintrag das sagt, waere eine Entscheidung -
        // keine Nebenwirkung. Waeren die beiden je gekoppelt, meldet dieser Test es.
        var mittenInDenSommerferien = new DateOnly(2026, 8, 7);

        Assert.NotNull(SchoolCalendar.CurrentVacation(mittenInDenSommerferien));
        Assert.False(PauseMode.IsActive(null, mittenInDenSommerferien));
    }
}
