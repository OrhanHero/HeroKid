using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// ITG (Informationstechnische Grundbildung) - Medienkompetenz/Datenschutz-Grundlagen, wie im
/// Berliner Rahmenlehrplan als eigenes Fach (v.a. Klasse 5/6) sowie vertiefend für Klasse 9.
/// </summary>
public sealed class ItgGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Itg;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Datenschutz, SicherePasswoerter, Urheberrecht },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Cybermobbing, FakeNewsErkennen, Algorithmen }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DatenschutzListe =
    {
        ("Was zählt zu \"persönlichen Daten\", die besonders geschützt werden sollten?", new[] { "Name, Adresse und Fotos von dir", "Nur die Lieblingsfarbe", "Nichts davon" }, "Name, Adresse und Fotos von dir",
            "Persönliche Daten sind alle Informationen, die dich identifizierbar machen, z.B. Name, Adresse, Telefonnummer oder Fotos."),
        ("Warum sollte man volle Namen und Adressen nicht öffentlich in sozialen Netzwerken posten?", new[] { "Um Missbrauch der Daten durch Fremde zu vermeiden", "Es ist verboten, überhaupt etwas zu posten", "Es spielt keine Rolle" },
            "Um Missbrauch der Daten durch Fremde zu vermeiden", "Öffentlich geteilte persönliche Daten können von Fremden missbraucht werden, z.B. für Betrug oder Belästigung.")
    };

    private static QuizQuestion Datenschutz(Random r)
    {
        var f = DatenschutzListe[r.Next(DatenschutzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Datenschutz-Grundlagen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PasswortListe =
    {
        ("Welches Passwort ist am sichersten?", new[] { "Tr7$kLm9!qXz", "123456", "passwort" }, "Tr7$kLm9!qXz",
            "Ein sicheres Passwort ist lang und mischt Groß-/Kleinbuchstaben, Zahlen und Sonderzeichen - keine einfachen Wörter oder Zahlenreihen."),
        ("Was sollte man mit seinem Passwort NICHT tun?", new[] { "Es an Freunde weitergeben", "Es geheim halten", "Es regelmäßig ändern" }, "Es an Freunde weitergeben",
            "Ein Passwort sollte man niemandem weitergeben, auch nicht guten Freunden - sonst ist es nicht mehr sicher.")
    };

    private static QuizQuestion SicherePasswoerter(Random r)
    {
        var f = PasswortListe[r.Next(PasswortListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Sichere Passwörter", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] UrheberrechtListe =
    {
        ("Darf man jedes Bild aus dem Internet einfach für die eigene Schulpräsentation nutzen?", new[] { "Nein, man muss auf Nutzungsrechte/Lizenz achten", "Ja, immer und ohne Einschränkung", "Nur wenn das Bild bunt ist" },
            "Nein, man muss auf Nutzungsrechte/Lizenz achten", "Bilder sind meist urheberrechtlich geschützt - man braucht eine passende Lizenz (z.B. Creative Commons) oder Erlaubnis."),
        ("Was bedeutet \"Urheberrecht\" ganz einfach?", new[] { "Der/die Ersteller/in eines Werks entscheidet über dessen Nutzung", "Jeder darf alles frei kopieren", "Es betrifft nur Musik" },
            "Der/die Ersteller/in eines Werks entscheidet über dessen Nutzung", "Das Urheberrecht schützt geistige Schöpfungen (Bilder, Texte, Musik) und gibt der/dem Urheber/in Kontrolle über die Nutzung.")
    };

    private static QuizQuestion Urheberrecht(Random r)
    {
        var f = UrheberrechtListe[r.Next(UrheberrechtListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Urheberrecht im Internet", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] CybermobbingListe =
    {
        ("Was ist Cybermobbing?", new[] { "Wiederholtes Belästigen/Bloßstellen einer Person über digitale Medien", "Ein einmaliger Streit im Chat", "Ein Computerspiel" },
            "Wiederholtes Belästigen/Bloßstellen einer Person über digitale Medien", "Cybermobbing ist absichtliches, wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über Internet/Handy."),
        ("Was ist ein sinnvoller erster Schritt, wenn man Cybermobbing beobachtet (nicht selbst betroffen)?", new[] { "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen und nichts tun", "Mitmachen" },
            "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen verstärkt das Problem - Unterstützung und das Einschalten von Erwachsenen helfen der betroffenen Person.")
    };

    private static QuizQuestion Cybermobbing(Random r)
    {
        var f = CybermobbingListe[r.Next(CybermobbingListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Cybermobbing", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FakeNewsListe =
    {
        ("Was ist ein guter erster Schritt, um eine verdächtige Nachricht zu prüfen?", new[] { "Die Quelle/den Absender genau prüfen", "Sofort teilen", "Die Überschrift glauben, ohne weiterzulesen" },
            "Die Quelle/den Absender genau prüfen", "Seriöse Nachrichten nennen eine überprüfbare Quelle - bei unbekannten oder fragwürdigen Absendern ist Vorsicht geboten."),
        ("Woran erkennt man oft (nicht immer) Fake News?", new[] { "Reißerische Überschriften ohne Belege, starke Emotionen", "Ruhige, sachliche Sprache mit Quellenangaben", "Viele Rechtschreibfehler bedeuten automatisch, es ist wahr" },
            "Reißerische Überschriften ohne Belege, starke Emotionen", "Übertriebene, emotionalisierende Sprache ohne nachprüfbare Belege ist ein Warnsignal für Falschmeldungen.")
    };

    private static QuizQuestion FakeNewsErkennen(Random r)
    {
        var f = FakeNewsListe[r.Next(FakeNewsListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Fake News erkennen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AlgorithmenListe =
    {
        ("Was ist ein Algorithmus einfach erklärt?", new[] { "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein anderes Wort für Computer", "Ein Videospiel" },
            "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein Algorithmus ist eine eindeutige Abfolge von Schritten, die z.B. ein Programm abarbeitet, um eine Aufgabe zu lösen."),
        ("Warum zeigen soziale Netzwerke jedem Nutzer unterschiedliche Inhalte im Feed an?", new[] { "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Der Zufall entscheidet komplett", "Alle sehen exakt dasselbe" },
            "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Empfehlungsalgorithmen werten aus, was ein Nutzer bisher angeklickt/geliked hat, und passen den Feed entsprechend an.")
    };

    private static QuizQuestion Algorithmen(Random r)
    {
        var f = AlgorithmenListe[r.Next(AlgorithmenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Algorithmen-Grundbegriff", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung
        };
    }
}
