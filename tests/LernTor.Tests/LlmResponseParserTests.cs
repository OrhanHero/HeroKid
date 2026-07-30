using LernTor.ContentGen.TeacherImport;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Antwort-Auswertung des Lehrer-Imports. Ein echter Modelllauf dauert Minuten und ist nicht
/// reproduzierbar - deshalb wird hier gegen feste Antworttexte geprüft.
/// </summary>
public class LlmResponseParserTests
{
    private const string EineVollstaendigeFrage =
        "{\"topic\":\"Mengen\",\"prompt\":\"Was ist eine Menge?\",\"type\":\"OpenText\"," +
        "\"options\":[],\"correctAnswers\":[\"Eine Zusammenfassung von Elementen\"]," +
        "\"explanation\":\"Steht so im Dokument.\"}";

    [Fact]
    public void Vollstaendige_Antwort_wird_gelesen()
    {
        var drafts = LlmResponseParser.ParseDrafts(
            "{\"questions\":[" + EineVollstaendigeFrage + "]}", "Dokumenttext");

        var draft = Assert.Single(drafts);
        Assert.Equal("Was ist eine Menge?", draft.Prompt);
        Assert.Equal("Mengen", draft.Topic);
    }

    [Fact]
    public void Antwort_im_Markdown_Codeblock_wird_gelesen()
    {
        // Instruct-Modelle packen JSON gern in ```json ... ``` - trotz gegenteiliger Anweisung.
        var drafts = LlmResponseParser.ParseDrafts(
            "Gerne! ```json\n{\"questions\":[" + EineVollstaendigeFrage + "]}\n``` Viel Erfolg!",
            "Dokumenttext");

        Assert.Single(drafts);
    }

    [Fact]
    public void Abgeschnittene_Antwort_wird_repariert_statt_verworfen()
    {
        // Das Token-Limit kann mitten in der Fragenliste zuschlagen. Nach mehreren Minuten
        // Rechenzeit sind brauchbare Vorschläge besser als eine Fehlermeldung.
        var abgeschnitten =
            "{\"questions\":[" + EineVollstaendigeFrage + "," +
            "{\"topic\":\"Mengen\",\"prompt\":\"Was ist ein Elem";

        var drafts = LlmResponseParser.ParseDrafts(abgeschnitten, "Dokumenttext");

        var draft = Assert.Single(drafts);
        Assert.Equal("Was ist eine Menge?", draft.Prompt);
    }

    [Fact]
    public void Geschweifte_Klammern_im_Fragetext_verwirren_die_Reparatur_nicht()
    {
        var mitKlammer =
            "{\"questions\":[" +
            "{\"topic\":\"Mengen\",\"prompt\":\"Was bedeutet {1,2,3}?\",\"type\":\"OpenText\"," +
            "\"options\":[],\"correctAnswers\":[\"Eine Menge\"],\"explanation\":\"Aufzählung.\"}," +
            "{\"topic\":\"Mengen\",\"prompt\":\"Unvollstän";

        var drafts = LlmResponseParser.ParseDrafts(mitKlammer, "Dokumenttext");

        Assert.Equal("Was bedeutet {1,2,3}?", Assert.Single(drafts).Prompt);
    }

    [Fact]
    public void Antwort_ohne_jedes_JSON_meldet_einen_verstaendlichen_Fehler()
    {
        // Genau der gemeldete Fall: das Modell hat den Text zusammengefasst statt Fragen zu bauen.
        var ex = Assert.Throws<InvalidOperationException>(() =>
            LlmResponseParser.ParseDrafts("Die Einheit Mengen ist eine wichtige Grundlage.", "Dok"));

        Assert.Contains("keine verwertbaren Fragen", ex.Message);
    }

    [Fact]
    public void Der_JSON_Anfang_wird_der_Antwort_vorangestellt()
    {
        // Der Prompt endet mitten im JSON, damit das Modell gar nicht anders kann als es
        // fortzusetzen - der vorgegebene Anfang muss beim Parsen wieder dabei sein.
        Assert.StartsWith("{", LlmResponseParser.AnswerPrefill);
        Assert.Contains("questions", LlmResponseParser.AnswerPrefill);
    }
}
