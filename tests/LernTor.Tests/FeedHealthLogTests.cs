using LernTor.News;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Feed-Zustandsprotokoll: macht im Eltern-Bereich sichtbar, welche der 44 Quellen zuletzt nicht
/// erreichbar waren. Die App überspringt tote Feeds bewusst geräuschlos - ohne diese Anzeige
/// könnte die Hälfte kaputt sein, ohne dass es jemandem auffällt.
/// </summary>
public sealed class FeedHealthLogTests : IDisposable
{
    private readonly string _path = Path.Combine(
        Path.GetTempPath(), $"lerntor-feedhealth-{Guid.NewGuid():N}", "feedhealth.json");

    private FeedHealthLog Create() => new(_path);

    [Fact]
    public void Ohne_Datei_ist_die_Liste_leer()
    {
        Assert.Empty(Create().LoadAll());
    }

    [Fact]
    public void Erfolg_und_Fehler_werden_festgehalten()
    {
        var log = Create();
        log.Record("tagesschau.de", isHealthy: true);
        log.Record("BBC Newsround", isHealthy: false, "HTTP 404");

        var all = log.LoadAll();

        Assert.True(all["tagesschau.de"].IsHealthy);
        Assert.Null(all["tagesschau.de"].Error);
        Assert.False(all["BBC Newsround"].IsHealthy);
        Assert.Equal("HTTP 404", all["BBC Newsround"].Error);
    }

    [Fact]
    public void Neuer_Stand_ersetzt_den_alten()
    {
        var log = Create();
        log.Record("heise online", isHealthy: false, "Timeout");
        log.Record("heise online", isHealthy: true);

        var entry = log.LoadAll()["heise online"];

        Assert.True(entry.IsHealthy);
        Assert.Null(entry.Error);
    }

    [Fact]
    public void Zustand_ueberlebt_den_Neustart()
    {
        Create().Record("DW Türkçe", isHealthy: false, "Feed nicht erreichbar");

        // Frische Instanz = wie nach einem App-Neustart.
        Assert.False(Create().LoadAll()["DW Türkçe"].IsHealthy);
    }

    [Fact]
    public void Lange_Fehlertexte_werden_gekuerzt()
    {
        // Eine mehrzeilige .NET-Ausnahme gehoert nicht ungekuerzt in den Eltern-Bereich.
        var log = Create();
        log.Record("Steam News", isHealthy: false, new string('x', 500));

        var error = log.LoadAll()["Steam News"].Error!;

        Assert.True(error.Length <= 161, $"Fehlertext ist {error.Length} Zeichen lang.");
    }

    [Fact]
    public void Zeilenumbrueche_im_Fehlertext_werden_geglaettet()
    {
        Assert.DoesNotContain("\n", FeedHealthLog.Shorten("Zeile 1\nZeile 2\r\nZeile 3"));
    }

    [Fact]
    public void Beschaedigte_Datei_kippt_den_Eltern_Bereich_nicht()
    {
        // Eine Diagnose-Hilfe darf niemals das kaputtmachen, was sie diagnostizieren soll.
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, "{kein json");

        Assert.Empty(Create().LoadAll());
    }

    public void Dispose()
    {
        try
        {
            var directory = Path.GetDirectoryName(_path);
            if (directory is not null && Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
        catch (IOException)
        {
            // Das Temp-Verzeichnis raeumt das Betriebssystem ohnehin auf.
        }
    }
}
