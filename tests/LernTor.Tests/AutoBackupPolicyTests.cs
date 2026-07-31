using LernTor.Core.Services;
using LernTor.Data;
using Xunit;

namespace LernTor.Tests;

/// <summary>Automatische Sicherung: wann gezogen und was aufgehoben wird.</summary>
public sealed class AutoBackupPolicyTests
{
    private static readonly DateTimeOffset Heute = new(2026, 8, 5, 7, 30, 0, TimeSpan.FromHours(2));

    private static AutoBackupFile Datei(DateTimeOffset erstellt, AutoBackupReason grund = AutoBackupReason.Taeglich) =>
        new(AutoBackupPolicy.FileNameFor(erstellt, grund), erstellt);

    [Fact]
    public void Dateiname_traegt_Zeitpunkt_und_Grund()
    {
        Assert.Equal(
            "lerntor-auto-2026-08-05-073000-taeglich.db",
            AutoBackupPolicy.FileNameFor(Heute, AutoBackupReason.Taeglich));

        Assert.Equal(
            "lerntor-auto-2026-08-05-073000-schema.db",
            AutoBackupPolicy.FileNameFor(Heute, AutoBackupReason.Schemaaenderung));
    }

    [Fact]
    public void Nur_eigene_Dateien_werden_erkannt()
    {
        // Wichtig fuers Aufraeumen: was die App nicht selbst angelegt hat, faesst sie nicht an.
        Assert.True(AutoBackupPolicy.IsOwnFile(AutoBackupPolicy.FileNameFor(Heute, AutoBackupReason.Taeglich)));
        Assert.False(AutoBackupPolicy.IsOwnFile("lerntor.db"));
        Assert.False(AutoBackupPolicy.IsOwnFile("lerntor-backup-2026-08-05.db"));
        Assert.False(AutoBackupPolicy.IsOwnFile("urlaubsfotos.zip"));
    }

    [Fact]
    public void Taeglich_wird_nur_einmal_pro_Tag_gezogen()
    {
        // Sonst zoege jeder Neustart eine weitere Kopie - an einem Tag mit fuenf Starts waere der
        // Vorrat der letzten Tage sofort weggeraeumt.
        var vonHeuteFrueh = Datei(Heute.AddHours(-6));

        Assert.False(AutoBackupPolicy.IsDue(new[] { vonHeuteFrueh }, Heute, AutoBackupReason.Taeglich));
        Assert.True(AutoBackupPolicy.IsDue(new[] { Datei(Heute.AddDays(-1)) }, Heute, AutoBackupReason.Taeglich));
        Assert.True(AutoBackupPolicy.IsDue(Array.Empty<AutoBackupFile>(), Heute, AutoBackupReason.Taeglich));
    }

    [Fact]
    public void Schema_Sicherung_ist_immer_faellig()
    {
        // Sie ist der eigentliche Zweck: ohne sie bliebe bei einer nicht-additiven Aenderung nur
        // "Datenbank loeschen".
        var schonHeuteGesichert = new[] { Datei(Heute.AddHours(-1)) };

        Assert.True(AutoBackupPolicy.IsDue(schonHeuteGesichert, Heute, AutoBackupReason.Schemaaenderung));
    }

    [Fact]
    public void Nur_die_neuesten_bleiben_liegen()
    {
        var dateien = Enumerable.Range(0, 8).Select(tag => Datei(Heute.AddDays(-tag))).ToList();

        var weg = AutoBackupPolicy.Obsolete(dateien, keep: 5);

        Assert.Equal(3, weg.Count);
        Assert.Contains(dateien[7].FileName, weg);
        Assert.DoesNotContain(dateien[0].FileName, weg);
        Assert.DoesNotContain(dateien[4].FileName, weg);
    }

    [Fact]
    public void Die_neueste_Schema_Sicherung_bleibt_auch_wenn_sie_alt_ist()
    {
        // Das ist der Kern: nach einer Schemaaenderung lernt ein Kind eine Woche weiter, und die
        // taeglichen Kopien haetten die eine Sicherung verdraengt, die den Umbau rueckgaengig
        // machen koennte.
        var schema = Datei(Heute.AddDays(-30), AutoBackupReason.Schemaaenderung);
        var taeglich = Enumerable.Range(0, 6).Select(tag => Datei(Heute.AddDays(-tag))).ToList();

        var weg = AutoBackupPolicy.Obsolete(taeglich.Append(schema), keep: 5);

        Assert.DoesNotContain(schema.FileName, weg);
        Assert.Contains(taeglich[5].FileName, weg);
    }

    [Fact]
    public void Von_mehreren_Schema_Sicherungen_bleibt_die_neueste()
    {
        // Aeltere Schema-Staende sind wertlos: das Schema von vorletzter Woche passt zu keiner
        // App-Version, die noch jemand startet.
        var alt = Datei(Heute.AddDays(-60), AutoBackupReason.Schemaaenderung);
        var neu = Datei(Heute.AddDays(-40), AutoBackupReason.Schemaaenderung);
        var taeglich = Enumerable.Range(0, 5).Select(tag => Datei(Heute.AddDays(-tag))).ToList();

        var weg = AutoBackupPolicy.Obsolete(taeglich.Concat(new[] { alt, neu }), keep: 5);

        Assert.Contains(alt.FileName, weg);
        Assert.DoesNotContain(neu.FileName, weg);
    }

    [Fact]
    public void Bei_wenigen_Dateien_wird_nichts_geloescht()
    {
        var dateien = Enumerable.Range(0, 3).Select(tag => Datei(Heute.AddDays(-tag))).ToList();

        Assert.Empty(AutoBackupPolicy.Obsolete(dateien, keep: 5));
        Assert.Empty(AutoBackupPolicy.Obsolete(Array.Empty<AutoBackupFile>(), keep: 5));
    }

    [Fact]
    public void Ein_Fingerabdruck_bleibt_gleich_wenn_sich_nur_die_Formatierung_aendert()
    {
        // Sonst zoege jede EF-Version, die ihr CREATE-Skript anders einrueckt, eine ueberfluessige
        // Sicherung nach sich.
        var a = SchemaFingerprint.Compute("CREATE TABLE \"Profiles\" (\n  \"Id\" TEXT NOT NULL\n);");
        var b = SchemaFingerprint.Compute("CREATE TABLE \"Profiles\" (   \"Id\" TEXT NOT NULL );");

        Assert.Equal(a, b);
        Assert.Equal(SchemaFingerprint.Length, a.Length);
    }

    [Fact]
    public void Eine_echte_Schemaaenderung_aendert_den_Fingerabdruck()
    {
        var vorher = SchemaFingerprint.Compute("CREATE TABLE \"Profiles\" (\"Id\" TEXT NOT NULL);");
        var nachher = SchemaFingerprint.Compute("CREATE TABLE \"Profiles\" (\"Id\" TEXT NOT NULL, \"Name\" TEXT);");

        Assert.NotEqual(vorher, nachher);
    }

    [Fact]
    public void Fehlender_Fingerabdruck_gilt_als_unbekannt()
    {
        // Unbekannt fuehrt zu einer Sicherung - die harmlose Richtung.
        var pfad = Path.Combine(Path.GetTempPath(), $"lerntor-fp-{Guid.NewGuid():N}.txt");

        Assert.Null(SchemaFingerprint.Read(pfad));

        try
        {
            SchemaFingerprint.Write(pfad, "abc123");
            Assert.Equal("abc123", SchemaFingerprint.Read(pfad));
        }
        finally
        {
            try
            {
                File.Delete(pfad);
            }
            catch (IOException)
            {
                // Aufraeumen ist Nebensache - das Temp-Verzeichnis raeumt Windows selbst.
            }
        }
    }
}
