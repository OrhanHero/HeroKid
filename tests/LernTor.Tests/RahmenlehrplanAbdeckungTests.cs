using System.Reflection;
using System.Text;
using LernTor.ContentGen.Curriculum;
using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der Rahmenlehrplan-Katalog (docs/NAECHSTES-LEVEL-3-2.md, Schritt 1) und die Generatoren
/// dürfen nicht auseinanderlaufen: jede Zuordnung zeigt auf ein Thema, das es für diese
/// Klassenstufe wirklich gibt, und <c>docs/RAHMENLEHRPLAN.md</c> ist genau das, was der Katalog sagt.
/// </summary>
public sealed class RahmenlehrplanAbdeckungTests
{
    /// <summary>Offene Themenfelder. Darf nur sinken: wer eines schließt, senkt die Zahl.</summary>
    private const int OffeneThemenfelder = 1;

    private const string DokumentSchreiben = "LERNTOR_RLP_DOC";

    public static GradeLevel KlasseFuer(RlpStufe stufe) => stufe switch
    {
        RlpStufe.Stufe5_6 => GradeLevel.Klasse6,
        RlpStufe.Stufe7_8 => GradeLevel.Klasse7,
        _ => GradeLevel.Klasse9
    };

    /// <summary>Themen-Methoden je Fach und Klassenstufe, gelesen aus <c>TopicsByGrade</c>.</summary>
    private static readonly Lazy<Dictionary<(Subject, GradeLevel), List<string>>> Themen = new(() =>
    {
        var ergebnis = new Dictionary<(Subject, GradeLevel), List<string>>();
        var eigenschaft = typeof(ExerciseGeneratorBase).GetProperty(
            "TopicsByGrade", BindingFlags.Instance | BindingFlags.NonPublic)!;

        foreach (var typ in typeof(MathGenerator).Assembly.GetTypes()
                     .Where(t => !t.IsAbstract && typeof(ExerciseGeneratorBase).IsAssignableFrom(t)))
        {
            var generator = (ExerciseGeneratorBase)Activator.CreateInstance(typ)!;
            // TopicFactory ist ein geschützter Delegat - deshalb typneutral lesen.
            foreach (var paar in (System.Collections.IEnumerable)eigenschaft.GetValue(generator)!)
            {
                var stufe = (GradeLevel)paar.GetType().GetProperty("Key")!.GetValue(paar)!;
                var fabriken = (System.Collections.IEnumerable)paar.GetType().GetProperty("Value")!.GetValue(paar)!;
                ergebnis[(generator.Subject, stufe)] = fabriken.Cast<Delegate>().Select(f => f.Method.Name).ToList();
            }
        }

        return ergebnis;
    });

    [Fact]
    public void Jede_Zuordnung_zeigt_auf_ein_Thema_dieser_Klassenstufe()
    {
        var fehler = new List<string>();
        foreach (var feld in RahmenlehrplanKatalog.Alle)
        {
            var klasse = KlasseFuer(feld.Stufe);
            foreach (var zuordnung in feld.Themen)
            {
                if (!Themen.Value.TryGetValue((zuordnung.Fach, klasse), out var vorhanden) || !vorhanden.Contains(zuordnung.Thema))
                {
                    fehler.Add($"{feld.Fach} {feld.Titel}: {zuordnung.Fach}.{zuordnung.Thema} gibt es in {klasse} nicht");
                }
            }
        }

        Assert.True(fehler.Count == 0, string.Join("\n", fehler));
    }

    [Fact]
    public void Status_und_Zuordnung_passen_zusammen()
    {
        Assert.All(RahmenlehrplanKatalog.Alle, feld =>
        {
            var geuebt = feld.Status is RlpStatus.Abgedeckt or RlpStatus.Teilweise;
            Assert.True(geuebt == feld.Themen.Count > 0, $"{feld.Fach} {feld.Titel}: Status {feld.Status}, {feld.Themen.Count} Themen");
            if (feld.Status != RlpStatus.Abgedeckt)
            {
                Assert.False(string.IsNullOrWhiteSpace(feld.Hinweis), $"{feld.Fach} {feld.Titel}: {feld.Status} ohne Hinweis");
            }
        });
    }

    [Fact]
    public void Offene_Themenfelder_werden_nur_weniger()
    {
        var offen = RahmenlehrplanKatalog.Alle.Where(f => f.Status == RlpStatus.Offen).ToList();
        Assert.True(offen.Count <= OffeneThemenfelder,
            $"{offen.Count} offene Themenfelder statt höchstens {OffeneThemenfelder}:\n" +
            string.Join("\n", offen.Select(f => $"{f.Fach} {f.Titel}")));
        Assert.True(offen.Count == OffeneThemenfelder,
            $"Nur noch {offen.Count} offen - bitte {nameof(OffeneThemenfelder)} auf {offen.Count} senken.");
    }

    /// <summary>
    /// docs/RAHMENLEHRPLAN.md muss dem Katalog entsprechen. Nach einer Änderung am Katalog:
    /// <c>LERNTOR_RLP_DOC=1 dotnet test --filter RahmenlehrplanAbdeckungTests</c> schreibt sie neu.
    /// </summary>
    [Fact]
    public void Die_Uebersicht_in_docs_ist_aktuell()
    {
        var pfad = Path.Combine(Repowurzel(), "docs", "RAHMENLEHRPLAN.md");
        var soll = Uebersicht();

        if (Environment.GetEnvironmentVariable(DokumentSchreiben) is not null)
        {
            File.WriteAllText(pfad, soll, new UTF8Encoding(false));
        }

        var ist = File.Exists(pfad) ? File.ReadAllText(pfad).Replace("\r\n", "\n") : string.Empty;
        Assert.True(ist == soll,
            $"docs/RAHMENLEHRPLAN.md ist veraltet. Neu schreiben mit {DokumentSchreiben}=1 dotnet test --filter RahmenlehrplanAbdeckungTests");
    }

    private static string Repowurzel()
    {
        var ordner = new DirectoryInfo(AppContext.BaseDirectory);
        while (ordner is not null && !File.Exists(Path.Combine(ordner.FullName, "LernTor.sln")))
        {
            ordner = ordner.Parent;
        }

        return ordner?.FullName ?? throw new InvalidOperationException("LernTor.sln nicht gefunden");
    }

    private static readonly (RlpStufe Stufe, string Name)[] Stufen =
    {
        (RlpStufe.Stufe5_6, "5/6"), (RlpStufe.Stufe7_8, "7/8"), (RlpStufe.Stufe9_10, "9/10")
    };

    private static string Zeichen(RlpStatus status) => status switch
    {
        RlpStatus.Abgedeckt => "✅",
        RlpStatus.Teilweise => "◐ teilweise",
        RlpStatus.Offen => "⬜ offen",
        RlpStatus.NichtAlsQuiz => "– nicht als Quiz",
        _ => "– nicht in LernTor"
    };

    internal static string Uebersicht()
    {
        var alle = RahmenlehrplanKatalog.Alle;
        var s = new StringBuilder();
        s.Append("# Rahmenlehrplan in LernTor\n\n");
        s.Append("<!-- Erzeugt aus RahmenlehrplanKatalog (src/LernTor.ContentGen/Curriculum) durch\n");
        s.Append("     RahmenlehrplanAbdeckungTests. Nicht von Hand ändern: Katalog ändern, dann\n");
        s.Append("     LERNTOR_RLP_DOC=1 dotnet test --filter RahmenlehrplanAbdeckungTests -->\n\n");
        s.Append("Welche Themenfelder des Berliner Rahmenlehrplans 1–10 LernTor übt und mit welchen Themen. ");
        s.Append("Plan und Hintergrund: [`NAECHSTES-LEVEL-3-2.md`](NAECHSTES-LEVEL-3-2.md). ");
        s.Append("Ein Test prüft jede Zuordnung gegen die Generatoren.\n\n");

        s.Append("## Überblick\n\n");
        s.Append("Gezählt: abgedeckte (auch teilweise) von allen Themenfeldern, die sich als Quiz üben lassen.\n\n");
        s.Append("| Fach | 5/6 | 7/8 | 9/10 |\n|---|---|---|---|\n");
        foreach (var fach in alle.Select(f => f.Fach).Distinct().OrderBy(f => f, StringComparer.Ordinal))
        {
            s.Append($"| {fach} |");
            foreach (var (stufe, _) in Stufen)
            {
                var felder = alle.Where(f => f.Fach == fach && f.Stufe == stufe).ToList();
                var zaehlbar = felder.Where(f => f.Status is not (RlpStatus.NichtAlsQuiz or RlpStatus.NichtInLernTor)).ToList();
                s.Append(felder.Count == 0 ? " – |"
                    : zaehlbar.Count == 0 ? $" {Zeichen(felder[0].Status).TrimStart('–', ' ')} |"
                    : $" {zaehlbar.Count(f => f.Status is RlpStatus.Abgedeckt or RlpStatus.Teilweise)} von {zaehlbar.Count} |");
            }

            s.Append('\n');
        }

        foreach (var (stufe, name) in Stufen)
        {
            var felder = alle.Where(f => f.Stufe == stufe).ToList();
            s.Append($"\n## Doppeljahrgangsstufe {name}\n\n");
            if (felder.Count == 0)
            {
                s.Append("Noch nicht erfasst.\n");
                continue;
            }

            foreach (var fach in felder.Select(f => f.Fach).Distinct())
            {
                s.Append($"### {fach}\n\n| Themenfeld | Stand | Themen in LernTor |\n|---|---|---|\n");
                foreach (var feld in felder.Where(f => f.Fach == fach))
                {
                    var themen = string.Join(", ", feld.Themen.Select(t => $"`{t.Thema}`"));
                    var hinweis = feld.Hinweis is null ? string.Empty : (themen.Length > 0 ? " – " : string.Empty) + feld.Hinweis;
                    s.Append($"| {feld.Titel} | {Zeichen(feld.Status)} | {themen}{hinweis} |\n");
                }

                s.Append('\n');
            }
        }

        s.Append("## Quellen\n\n");
        foreach (var quelle in alle.Select(f => f.Quelle).Distinct())
        {
            s.Append($"- {quelle}: {alle.Count(f => f.Quelle == quelle)} Themenfelder\n");
        }

        return s.ToString();
    }
}
