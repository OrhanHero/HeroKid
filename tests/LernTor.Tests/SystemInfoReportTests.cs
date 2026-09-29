using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class SystemInfoReportTests
{
    [Theory]
    [InlineData("2.0.0+245a544b3c9d0e1f", "2.0.0 (245a544)")]
    [InlineData("2.0.0+abc", "2.0.0 (abc)")]
    [InlineData("2.0.0", "2.0.0")]
    [InlineData("2.0.0+", "2.0.0")]
    [InlineData(null, "(unbekannt)")]
    [InlineData("  ", "(unbekannt)")]
    public void Version_mit_kurzem_Git_Stand(string? eingabe, string erwartet)
    {
        Assert.Equal(erwartet, SystemInfoReport.AppVersion(eingabe));
    }

    [Theory]
    [InlineData(0, "0 B")]
    [InlineData(1023, "1023 B")]
    [InlineData(1024 * 812, "812 KB")]
    [InlineData(3_355_443, "3,2 MB")]
    [InlineData(5L * 1024 * 1024 * 1024, "5,0 GB")]
    [InlineData(-5, "0 B")]
    public void Groessen_in_deutscher_Schreibweise(long bytes, string erwartet)
    {
        Assert.Equal(erwartet, SystemInfoReport.FormatBytes(bytes));
    }

    [Fact]
    public void Fehlende_Datenbank_und_Sicherung_werden_benannt_statt_leer_zu_bleiben()
    {
        var zeilen = SystemInfoReport.Lines(new SystemInfoSnapshot(
            "2.0.0+1234567890", "10.0.12", "Windows 11", null, null, 2, @"C:\Users\Kind\AppData\Local\LernTor"));

        Assert.Equal("LernTor 2.0.0 (1234567)", zeilen[0]);
        Assert.Contains("nicht gefunden", zeilen[2]);
        Assert.Contains("2 Profil(e)", zeilen[2]);
        Assert.Contains("noch keine", zeilen[3]);
    }
}
