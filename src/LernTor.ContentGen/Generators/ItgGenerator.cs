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
            "Um Missbrauch der Daten durch Fremde zu vermeiden", "Öffentlich geteilte persönliche Daten können von Fremden missbraucht werden, z.B. für Betrug oder Belästigung."),
        ("Ein Fremder im Internet möchte deine Adresse und Handynummer wissen. Was tust du am besten?", new[] { "Ich gebe nichts preis und erzähle es einem Erwachsenen", "Ich schicke ihm sofort alles", "Ich frage ihn erst nach seinem Namen" },
            "Ich gebe nichts preis und erzähle es einem Erwachsenen", "Persönliche Daten sollten nie an Fremde im Internet weitergegeben werden - sprich stattdessen mit einem Erwachsenen darüber."),
        ("Was ist ein Cookie im Internet?", new[] { "Eine kleine Datei, die Infos über deinen Webseiten-Besuch speichert", "Ein Keks, den man beim Surfen isst", "Ein Passwort" },
            "Eine kleine Datei, die Infos über deinen Webseiten-Besuch speichert", "Cookies speichern z.B. Einstellungen oder Log-in-Daten, damit eine Webseite sich an dich \"erinnert\"."),
        ("Warum sollte man bei Apps auf die angeforderten Berechtigungen (z.B. Zugriff auf Kontakte) achten?", new[] { "Weil manche Apps mehr Daten sammeln, als sie für ihre Funktion brauchen", "Weil das komplett egal ist", "Weil Apps sonst gar nicht starten" },
            "Weil manche Apps mehr Daten sammeln, als sie für ihre Funktion brauchen", "Eine Taschenlampen-App braucht z.B. keinen Zugriff auf deine Kontakte - unnötige Berechtigungen sind ein Warnsignal.")
    };

    private static QuizQuestion Datenschutz(Random r)
    {
        var f = DatenschutzListe[r.Next(DatenschutzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Datenschutz-Grundlagen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Persönliche Daten (Name, Adresse, Fotos) machen dich identifizierbar - öffentlich geteilt können sie missbraucht werden."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] PasswortListe =
    {
        ("Welches Passwort ist am sichersten?", new[] { "Tr7$kLm9!qXz", "123456", "passwort" }, "Tr7$kLm9!qXz",
            "Ein sicheres Passwort ist lang und mischt Groß-/Kleinbuchstaben, Zahlen und Sonderzeichen - keine einfachen Wörter oder Zahlenreihen."),
        ("Was sollte man mit seinem Passwort NICHT tun?", new[] { "Es an Freunde weitergeben", "Es geheim halten", "Es regelmäßig ändern" }, "Es an Freunde weitergeben",
            "Ein Passwort sollte man niemandem weitergeben, auch nicht guten Freunden - sonst ist es nicht mehr sicher."),
        ("Wie oft sollte man für unterschiedliche wichtige Accounts (z.B. E-Mail und Spiele) dasselbe Passwort verwenden?", new[] { "Nie - für jeden wichtigen Account ein eigenes Passwort", "Immer, das ist am einfachsten", "Nur bei E-Mail-Accounts" },
            "Nie - für jeden wichtigen Account ein eigenes Passwort", "Wird ein Passwort bei einem Dienst gestohlen, sind sonst sofort alle anderen Accounts mit demselben Passwort gefährdet."),
        ("Was ist die Zwei-Faktor-Authentifizierung (2FA) zusätzlich zum Passwort?", new[] { "Ein zweiter Nachweis, z.B. ein Code aufs Handy", "Ein zweites, identisches Passwort", "Eine zweite E-Mail-Adresse" },
            "Ein zweiter Nachweis, z.B. ein Code aufs Handy", "2FA schützt zusätzlich zum Passwort mit einem zweiten Faktor, z.B. einem per SMS gesendeten Code."),
        ("Was ist an einem Passwort wie \"MeinHund2024!\" besser als an \"12345678\"?", new[] { "Es ist länger und mischt Buchstaben, Zahlen und Sonderzeichen", "Es ist kürzer", "Nichts, beide sind gleich sicher" },
            "Es ist länger und mischt Buchstaben, Zahlen und Sonderzeichen", "Länge und eine Mischung verschiedener Zeichenarten machen ein Passwort deutlich schwerer zu erraten.")
    };

    private static QuizQuestion SicherePasswoerter(Random r)
    {
        var f = PasswortListe[r.Next(PasswortListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Sichere Passwörter", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein sicheres Passwort ist lang und mischt Groß-/Kleinbuchstaben, Zahlen und Sonderzeichen - und wird niemandem weitergegeben."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] UrheberrechtListe =
    {
        ("Darf man jedes Bild aus dem Internet einfach für die eigene Schulpräsentation nutzen?", new[] { "Nein, man muss auf Nutzungsrechte/Lizenz achten", "Ja, immer und ohne Einschränkung", "Nur wenn das Bild bunt ist" },
            "Nein, man muss auf Nutzungsrechte/Lizenz achten", "Bilder sind meist urheberrechtlich geschützt - man braucht eine passende Lizenz (z.B. Creative Commons) oder Erlaubnis."),
        ("Was bedeutet \"Urheberrecht\" ganz einfach?", new[] { "Der/die Ersteller/in eines Werks entscheidet über dessen Nutzung", "Jeder darf alles frei kopieren", "Es betrifft nur Musik" },
            "Der/die Ersteller/in eines Werks entscheidet über dessen Nutzung", "Das Urheberrecht schützt geistige Schöpfungen (Bilder, Texte, Musik) und gibt der/dem Urheber/in Kontrolle über die Nutzung."),
        ("Was bedeutet eine \"Creative Commons\"-Lizenz bei einem Bild im Internet?", new[] { "Das Bild darf unter bestimmten Bedingungen frei genutzt werden", "Das Bild darf niemals verwendet werden", "Das Bild gehört niemandem mehr" },
            "Das Bild darf unter bestimmten Bedingungen frei genutzt werden", "Creative-Commons-Lizenzen erlauben eine Nutzung unter bestimmten Bedingungen, z.B. mit Namensnennung des Urhebers."),
        ("Darf man ein selbst aufgenommenes Foto von einem Freund einfach ohne Erlaubnis online stellen?", new[] { "Besser nicht - man sollte vorher fragen (Recht am eigenen Bild)", "Ja, immer, weil man das Foto gemacht hat", "Nur wenn das Foto unscharf ist" },
            "Besser nicht - man sollte vorher fragen (Recht am eigenen Bild)", "Das \"Recht am eigenen Bild\" bedeutet: Auch wer nur abgebildet ist, muss der Veröffentlichung meist zustimmen."),
        ("Was kann passieren, wenn man Musik ohne Erlaubnis herunterlädt und weiterverbreitet?", new[] { "Das kann eine Urheberrechtsverletzung sein und Folgen haben", "Das ist immer komplett erlaubt", "Musik ist nie urheberrechtlich geschützt" },
            "Das kann eine Urheberrechtsverletzung sein und Folgen haben", "Musik ist urheberrechtlich geschützt - unerlaubtes Verbreiten kann rechtliche Konsequenzen haben.")
    };

    private static QuizQuestion Urheberrecht(Random r)
    {
        var f = UrheberrechtListe[r.Next(UrheberrechtListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse6,
            Topic = "Urheberrecht im Internet", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Urheberrecht: der/die Ersteller/in eines Werks entscheidet über die Nutzung - Bilder aus dem Internet brauchen Lizenz/Erlaubnis."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] CybermobbingListe =
    {
        ("Was ist Cybermobbing?", new[] { "Wiederholtes Belästigen/Bloßstellen einer Person über digitale Medien", "Ein einmaliger Streit im Chat", "Ein Computerspiel" },
            "Wiederholtes Belästigen/Bloßstellen einer Person über digitale Medien", "Cybermobbing ist absichtliches, wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über Internet/Handy."),
        ("Was ist ein sinnvoller erster Schritt, wenn man Cybermobbing beobachtet (nicht selbst betroffen)?", new[] { "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen und nichts tun", "Mitmachen" },
            "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen verstärkt das Problem - Unterstützung und das Einschalten von Erwachsenen helfen der betroffenen Person."),
        ("Was kann ein Opfer von Cybermobbing als Beweis sichern?", new[] { "Screenshots der Nachrichten", "Nichts, Beweise sind unwichtig", "Nur das Datum, ohne Text" },
            "Screenshots der Nachrichten", "Screenshots dokumentieren beleidigende Nachrichten und helfen später beim Melden oder bei Erwachsenen/Schule."),
        ("Warum trauen sich manche Menschen im Internet zu Dingen, die sie im echten Leben nie sagen würden?", new[] { "Weil die scheinbare Anonymität die Hemmschwelle senkt", "Weil es im Internet keine Konsequenzen gibt", "Weil Cybermobbing gesetzlich erlaubt ist" },
            "Weil die scheinbare Anonymität die Hemmschwelle senkt", "Das Gefühl von Anonymität und fehlende direkte Reaktion senken bei manchen die Hemmschwelle für Beleidigungen."),
        ("Was ist KEIN sinnvoller Umgang mit Cybermobbing?", new[] { "Selbst zurückbeleidigen und mitmachen", "Nachrichten dokumentieren und Erwachsenen zeigen", "Den Absender blockieren und melden" },
            "Selbst zurückbeleidigen und mitmachen", "Zurückbeleidigen verschärft die Situation meist - besser sind Dokumentieren, Blockieren, Melden und Hilfe holen.")
    };

    private static QuizQuestion Cybermobbing(Random r)
    {
        var f = CybermobbingListe[r.Next(CybermobbingListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Cybermobbing", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Cybermobbing ist wiederholtes, absichtliches Belästigen über digitale Medien - Hilfe holen statt wegschauen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FakeNewsListe =
    {
        ("Was ist ein guter erster Schritt, um eine verdächtige Nachricht zu prüfen?", new[] { "Die Quelle/den Absender genau prüfen", "Sofort teilen", "Die Überschrift glauben, ohne weiterzulesen" },
            "Die Quelle/den Absender genau prüfen", "Seriöse Nachrichten nennen eine überprüfbare Quelle - bei unbekannten oder fragwürdigen Absendern ist Vorsicht geboten."),
        ("Woran erkennt man oft (nicht immer) Fake News?", new[] { "Reißerische Überschriften ohne Belege, starke Emotionen", "Ruhige, sachliche Sprache mit Quellenangaben", "Viele Rechtschreibfehler bedeuten automatisch, es ist wahr" },
            "Reißerische Überschriften ohne Belege, starke Emotionen", "Übertriebene, emotionalisierende Sprache ohne nachprüfbare Belege ist ein Warnsignal für Falschmeldungen."),
        ("Was kann helfen, um eine verdächtige Nachricht zu überprüfen?", new[] { "Nachschauen, ob seriöse Nachrichtenseiten dasselbe berichten", "Nur auf das Bild schauen", "Die Anzahl der Likes zählen" },
            "Nachschauen, ob seriöse Nachrichtenseiten dasselbe berichten", "Berichten mehrere unabhängige, seriöse Quellen dasselbe, ist die Nachricht wahrscheinlicher wahr."),
        ("Ein Foto zu einer Nachricht sieht sehr extrem oder unglaubwürdig aus. Was ist ein guter nächster Schritt?", new[] { "Prüfen, ob das Bild schon einmal in einem ganz anderen Zusammenhang auftauchte", "Sofort glauben und teilen", "Das Bild ignorieren, aber den Text glauben" },
            "Prüfen, ob das Bild schon einmal in einem ganz anderen Zusammenhang auftauchte", "Alte oder aus dem Zusammenhang gerissene Bilder werden oft zu neuen Falschmeldungen wiederverwendet."),
        ("Was bedeutet \"Faktencheck\"?", new[] { "Die Überprüfung, ob Behauptungen in einer Nachricht stimmen", "Das schnelle Überfliegen einer Überschrift", "Das Zählen von Kommentaren" },
            "Die Überprüfung, ob Behauptungen in einer Nachricht stimmen", "Ein Faktencheck vergleicht Behauptungen mit belegbaren Fakten und Quellen.")
    };

    private static QuizQuestion FakeNewsErkennen(Random r)
    {
        var f = FakeNewsListe[r.Next(FakeNewsListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Fake News erkennen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Quelle/Absender genau prüfen; reißerische Überschriften ohne Belege und starke Emotionen sind ein Warnsignal."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AlgorithmenListe =
    {
        ("Was ist ein Algorithmus einfach erklärt?", new[] { "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein anderes Wort für Computer", "Ein Videospiel" },
            "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein Algorithmus ist eine eindeutige Abfolge von Schritten, die z.B. ein Programm abarbeitet, um eine Aufgabe zu lösen."),
        ("Warum zeigen soziale Netzwerke jedem Nutzer unterschiedliche Inhalte im Feed an?", new[] { "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Der Zufall entscheidet komplett", "Alle sehen exakt dasselbe" },
            "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Empfehlungsalgorithmen werten aus, was ein Nutzer bisher angeklickt/geliked hat, und passen den Feed entsprechend an."),
        ("Wie nennt man es, wenn ein Algorithmus dir bevorzugt Inhalte zeigt, die deine bisherige Meinung bestätigen?", new[] { "Filterblase / Echo-Kammer", "Firewall", "Update" },
            "Filterblase / Echo-Kammer", "In einer Filterblase sieht man vor allem Inhalte, die zur eigenen Meinung passen - andere Sichtweisen werden seltener gezeigt."),
        ("Was macht ein Suchmaschinen-Algorithmus im Kern?", new[] { "Er sortiert Suchergebnisse nach Relevanz für die Suchanfrage", "Er löscht zufällig Webseiten", "Er erstellt neue Internetseiten" },
            "Er sortiert Suchergebnisse nach Relevanz für die Suchanfrage", "Suchalgorithmen bewerten und ordnen Webseiten danach, wie gut sie zur Suchanfrage passen."),
        ("Kann ein Algorithmus \"unfair\" sein?", new[] { "Ja, wenn er mit unfairen/verzerrten Daten trainiert oder programmiert wurde", "Nein, Algorithmen sind immer neutral", "Nein, weil Computer nicht denken können" },
            "Ja, wenn er mit unfairen/verzerrten Daten trainiert oder programmiert wurde", "Algorithmen übernehmen Verzerrungen (Bias) aus den Daten oder Entscheidungen, mit denen sie erstellt wurden.")
    };

    private static QuizQuestion Algorithmen(Random r)
    {
        var f = AlgorithmenListe[r.Next(AlgorithmenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Itg, GradeLevel = GradeLevel.Klasse9,
            Topic = "Algorithmen-Grundbegriff", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ein Algorithmus ist eine genaue Schritt-für-Schritt-Anleitung - Empfehlungsalgorithmen wählen Inhalte basierend auf deinem bisherigen Verhalten aus."
        };
    }
}
