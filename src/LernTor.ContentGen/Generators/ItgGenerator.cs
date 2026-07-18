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
        ("Warum sollte man volle Namen und Adressen nicht öffentlich in sozialen Netzwerken posten?", new[] { "Um Missbrauch der Daten durch Fremde zu vermeiden", "Es ist verboten, überhaupt etwas zu posten (was so in der Praxis nicht zutrifft)", "Es spielt keine Rolle" },
            "Um Missbrauch der Daten durch Fremde zu vermeiden", "Öffentlich geteilte persönliche Daten können von Fremden missbraucht werden, z.B. für Betrug oder Belästigung."),
        ("Ein Fremder im Internet möchte deine Adresse und Handynummer wissen. Was tust du am besten?", new[] { "Ich gebe nichts preis und erzähle es einem Erwachsenen", "Ich schicke ihm sofort alles", "Ich frage ihn erst nach seinem Namen" },
            "Ich gebe nichts preis und erzähle es einem Erwachsenen", "Persönliche Daten sollten nie an Fremde im Internet weitergegeben werden - sprich stattdessen mit einem Erwachsenen darüber."),
        ("Was ist ein Cookie im Internet?", new[] { "Eine kleine Datei, die Infos über deinen Webseiten-Besuch speichert", "Ein Keks, den man beim Surfen isst - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Ein Passwort" },
            "Eine kleine Datei, die Infos über deinen Webseiten-Besuch speichert", "Cookies speichern z.B. Einstellungen oder Log-in-Daten, damit eine Webseite sich an dich \"erinnert\"."),
        ("Warum sollte man bei Apps auf die angeforderten Berechtigungen (z.B. Zugriff auf Kontakte) achten?", new[] { "Weil manche Apps mehr Daten sammeln, als sie für ihre Funktion brauchen", "Weil das komplett egal ist", "Weil Apps sonst gar nicht starten, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Weil manche Apps mehr Daten sammeln, als sie für ihre Funktion brauchen", "Eine Taschenlampen-App braucht z.B. keinen Zugriff auf deine Kontakte - unnötige Berechtigungen sind ein Warnsignal."),
        ("Was ist Phishing?", new[] { "Der Versuch, über gefälschte Nachrichten/Webseiten an Passwörter oder Daten zu gelangen", "Ein Computerspiel über Angeln", "Ein besonders schnelles Internet" },
            "Der Versuch, über gefälschte Nachrichten/Webseiten an Passwörter oder Daten zu gelangen", "Phishing-Mails oder -Webseiten sehen echten Anbietern täuschend ähnlich, um Zugangsdaten zu stehlen."),
        ("Warum ist Online-Banking oder Einkaufen über ein öffentliches, ungesichertes WLAN riskant?", new[] { "Daten können in offenen Netzwerken leichter von Dritten mitgelesen werden", "Öffentliches WLAN ist immer schneller und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Es gibt dabei überhaupt kein Risiko" },
            "Daten können in offenen Netzwerken leichter von Dritten mitgelesen werden", "In ungesicherten öffentlichen WLANs können technisch versierte Personen leichter Datenverkehr mitschneiden."),
        ("Was bedeutet \"Datensparsamkeit\"?", new[] { "Nur so viele persönliche Daten preisgeben, wie wirklich nötig sind", "Möglichst viele Daten von sich veröffentlichen", "Daten nie speichern dürfen" },
            "Nur so viele persönliche Daten preisgeben, wie wirklich nötig sind", "Datensparsamkeit heißt: bei jeder Anfrage prüfen, ob diese Angabe wirklich notwendig ist."),
        ("Was ist eine IP-Adresse, einfach erklärt?", new[] { "Eine Art Adresse, über die ein Gerät im Internet erreichbar ist", "Ein anderes Wort für Passwort", "Der Name eines Computerprogramms - eine haeufige, aber unzutreffende Vorstellung" },
            "Eine Art Adresse, über die ein Gerät im Internet erreichbar ist", "Jedes Gerät im Internet bekommt eine IP-Adresse, ähnlich einer Postadresse für Datenpakete."),
        ("Was sollte man bei den Privatsphäre-Einstellungen in sozialen Netzwerken regelmäßig prüfen?", new[] { "Wer die eigenen Beiträge und Fotos sehen kann", "Nur die Lieblingsfarbe des Profils", "Die Anzahl der Emojis" },
            "Wer die eigenen Beiträge und Fotos sehen kann", "Privatsphäre-Einstellungen legen fest, ob Fremde oder nur bestätigte Kontakte die eigenen Inhalte sehen können."),
        ("Was ist ein \"Datenleck\" (Datenpanne)?", new[] { "Wenn persönliche Daten unabsichtlich an Unbefugte gelangen", "Ein technischer Fachbegriff für einen leeren Akku, auch wenn das manche zunaechst vermuten wuerden", "Ein Programm zum Daten-Aufräumen" },
            "Wenn persönliche Daten unabsichtlich an Unbefugte gelangen", "Bei einem Datenleck (z.B. durch einen Hackerangriff) gelangen gespeicherte Nutzerdaten ungewollt nach außen."),
        ("Warum sollte man die Standort-Freigabe von Apps kritisch prüfen?", new[] { "Weil Apps sonst genau nachverfolgen können, wo man sich aufhält", "Weil das den Akku überhaupt nicht beeinflusst, was bei genauerem Hinsehen nicht stimmt", "Weil Standortdaten gesetzlich verboten sind" },
            "Weil Apps sonst genau nachverfolgen können, wo man sich aufhält", "Nicht jede App braucht ständigen Zugriff auf den Standort, um ihre Funktion zu erfüllen."),
        ("Was bedeutet das \"Recht auf Löschung\" (auch \"Recht auf Vergessenwerden\")?", new[] { "Man kann verlangen, dass ein Unternehmen die eigenen gespeicherten Daten löscht", "Man darf keine eigenen Fotos mehr posten (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Firmen dürfen niemals Daten speichern" },
            "Man kann verlangen, dass ein Unternehmen die eigenen gespeicherten Daten löscht", "Nutzerinnen und Nutzer können von Unternehmen verlangen, ihre gespeicherten persönlichen Daten zu löschen."),
        ("Was sind biometrische Daten?", new[] { "Körperliche Merkmale wie Fingerabdruck oder Gesicht, die zur Identifikation genutzt werden", "Daten über Lieblingsspiele", "Die Anzahl der Kontakte im Handy, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt" },
            "Körperliche Merkmale wie Fingerabdruck oder Gesicht, die zur Identifikation genutzt werden", "Biometrische Daten wie Fingerabdruck- oder Gesichtserkennung gelten als besonders sensibel."),
        ("Warum ist ein gestohlener Fingerabdruck heikler als ein gestohlenes Passwort?", new[] { "Weil man einen Fingerabdruck nicht wie ein Passwort einfach ändern kann", "Weil Fingerabdrücke unwichtig sind", "Weil das genau gleich riskant ist" },
            "Weil man einen Fingerabdruck nicht wie ein Passwort einfach ändern kann", "Ein Passwort kann man nach einem Diebstahl ändern, ein Fingerabdruck bleibt für immer derselbe."),
        ("Was verfolgen viele Werbe-Tracker beim Surfen im Internet?", new[] { "Welche Webseiten man besucht, um passende Werbung zu zeigen", "Nur die Uhrzeit des letzten Neustarts", "Die Anzahl der installierten Schriftarten, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Welche Webseiten man besucht, um passende Werbung zu zeigen", "Tracker sammeln Surfverhalten über mehrere Webseiten hinweg, um personalisierte Werbung auszuspielen."),
        ("Was bedeutet der \"Inkognito-Modus\" im Browser wirklich?", new[] { "Der Verlauf wird lokal nicht gespeichert, Webseiten sehen den Besuch aber trotzdem", "Man ist damit komplett unsichtbar im gesamten Internet", "Der Modus verhindert jede Werbung für immer" },
            "Der Verlauf wird lokal nicht gespeichert, Webseiten sehen den Besuch aber trotzdem", "Inkognito verhindert nur, dass der Verlauf auf dem eigenen Gerät gespeichert wird - Webseiten und Internetanbieter können weiterhin Daten sehen."),
        ("Wieso sollten Kinder und Jugendliche besonders vorsichtig mit persönlichen Daten im Netz sein?", new[] { "Weil solche Daten oft sehr lange online bleiben und später Nachteile bringen können", "Weil das Internet Daten automatisch nach einem Tag löscht", "Weil es für sie gesetzlich verboten ist, das Internet zu nutzen und deshalb hier nicht zutrifft" },
            "Weil solche Daten oft sehr lange online bleiben und später Nachteile bringen können", "Einmal veröffentlichte Inhalte lassen sich oft nicht vollständig wieder entfernen und können Jahre später noch auffindbar sein."),
        ("Was kann eine Folge davon sein, wenn ein Unternehmen zu viele persönliche Daten sammelt und diese gestohlen werden?", new[] { "Die Daten können für Identitätsdiebstahl oder Betrug missbraucht werden", "Es passiert grundsätzlich gar nichts", "Das Unternehmen muss die Daten sofort veröffentlichen" },
            "Die Daten können für Identitätsdiebstahl oder Betrug missbraucht werden", "Gestohlene persönliche Daten werden häufig für Betrug, Identitätsdiebstahl oder Erpressung missbraucht."),
        ("Warum ist es sinnvoll, bei unwichtigen Spiele-Apps nicht automatisch das echte Geburtsdatum anzugeben?", new[] { "Weniger echte persönliche Daten im Umlauf verringern das Risiko bei einem Datenleck", "Weil Apps ohne Geburtsdatum gar nicht funktionieren, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Weil das gesetzlich vorgeschrieben ist" },
            "Weniger echte persönliche Daten im Umlauf verringern das Risiko bei einem Datenleck", "Je weniger echte persönliche Daten bei unwichtigen Diensten hinterlegt sind, desto geringer der mögliche Schaden bei einem Datenleck.")
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
        ("Welches Passwort ist am sichersten?", new[] { "Tr7$kLm9!qXz", "123456", "passwort, auch wenn das manche zunaechst vermuten wuerden" }, "Tr7$kLm9!qXz",
            "Ein sicheres Passwort ist lang und mischt Groß-/Kleinbuchstaben, Zahlen und Sonderzeichen - keine einfachen Wörter oder Zahlenreihen."),
        ("Was sollte man mit seinem Passwort NICHT tun?", new[] { "Es an Freunde weitergeben", "Es geheim halten", "Es regelmäßig ändern, was bei genauerem Hinsehen nicht stimmt" }, "Es an Freunde weitergeben",
            "Ein Passwort sollte man niemandem weitergeben, auch nicht guten Freunden - sonst ist es nicht mehr sicher."),
        ("Wie oft sollte man für unterschiedliche wichtige Accounts (z.B. E-Mail und Spiele) dasselbe Passwort verwenden?", new[] { "Nie - für jeden wichtigen Account ein eigenes Passwort", "Immer, das ist am einfachsten (was so in der Praxis nicht zutrifft)", "Nur bei E-Mail-Accounts" },
            "Nie - für jeden wichtigen Account ein eigenes Passwort", "Wird ein Passwort bei einem Dienst gestohlen, sind sonst sofort alle anderen Accounts mit demselben Passwort gefährdet."),
        ("Was ist die Zwei-Faktor-Authentifizierung (2FA) zusätzlich zum Passwort?", new[] { "Ein zweiter Nachweis, z.B. ein Code aufs Handy", "Ein zweites, identisches Passwort", "Eine zweite E-Mail-Adresse" },
            "Ein zweiter Nachweis, z.B. ein Code aufs Handy", "2FA schützt zusätzlich zum Passwort mit einem zweiten Faktor, z.B. einem per SMS gesendeten Code."),
        ("Was ist an einem Passwort wie \"MeinHund2024!\" besser als an \"12345678\"?", new[] { "Es ist länger und mischt Buchstaben, Zahlen und Sonderzeichen", "Es ist kürzer", "Nichts, beide sind gleich sicher - eine verbreitete, aber falsche Annahme" },
            "Es ist länger und mischt Buchstaben, Zahlen und Sonderzeichen", "Länge und eine Mischung verschiedener Zeichenarten machen ein Passwort deutlich schwerer zu erraten."),
        ("Was ist ein Passwort-Manager?", new[] { "Ein Programm, das sichere Passwörter erstellt und verschlüsselt speichert", "Ein Mitarbeiter, der Passwörter für andere errät, was einer genaueren Pruefung nicht standhaelt", "Ein Ordner auf dem Schreibtisch" },
            "Ein Programm, das sichere Passwörter erstellt und verschlüsselt speichert", "Passwort-Manager generieren zufällige, starke Passwörter und speichern sie verschlüsselt - man muss sich nur noch ein Master-Passwort merken."),
        ("Warum ist \"MeineKatzeMauzi1990\" riskant, wenn viele Menschen dein Haustier und Geburtsjahr kennen?", new[] { "Weil persönliche Infos leicht zu erraten sind, wenn man dich kennt", "Weil das Passwort zu lang ist", "Weil Katzennamen grundsätzlich verboten sind, obwohl das auf den ersten Blick plausibel klingt" },
            "Weil persönliche Infos leicht zu erraten sind, wenn man dich kennt", "Passwörter aus öffentlich bekannten persönlichen Infos (Haustiername, Geburtsjahr) sind für Bekannte oder aus Social-Media-Infos leicht zu erraten."),
        ("Was ist eine \"Passphrase\"?", new[] { "Ein langes Passwort aus mehreren zufälligen Wörtern, leicht zu merken, schwer zu erraten", "Ein Passwort, das man laut aussprechen muss, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein anderes Wort für PIN-Code" },
            "Ein langes Passwort aus mehreren zufälligen Wörtern, leicht zu merken, schwer zu erraten", "Eine Passphrase wie \"Elefant-Regenschirm-Gitarre-42\" ist oft leichter zu merken als ein kurzes, kryptisches Passwort und trotzdem sicher."),
        ("Was ist ein \"Brute-Force-Angriff\" auf ein Passwort?", new[] { "Automatisiertes Ausprobieren sehr vieler Passwort-Kombinationen", "Ein Angriff mit körperlicher Gewalt auf einen Computer", "Ein Virus, der Passwörter löscht" },
            "Automatisiertes Ausprobieren sehr vieler Passwort-Kombinationen", "Bei einem Brute-Force-Angriff probiert ein Programm automatisch sehr viele mögliche Passwörter durch - je länger das Passwort, desto länger dauert das."),
        ("Warum sollte man Sicherheitsfragen (z.B. \"Mädchenname der Mutter\") nicht wahrheitsgemäß öffentlich in sozialen Netzwerken teilen?", new[] { "Weil solche Infos Angreifern beim Zurücksetzen fremder Passwörter helfen können", "Weil das gesetzlich verboten ist", "Weil es niemanden interessiert" },
            "Weil solche Infos Angreifern beim Zurücksetzen fremder Passwörter helfen können", "Sicherheitsfragen dienen dem Passwort-Zurücksetzen - sind die Antworten öffentlich bekannt, wird dieser Schutz wertlos."),
        ("Was zeigt eine Passwort-Stärkeanzeige beim Registrieren typischerweise an?", new[] { "Wie schwer das gewählte Passwort zu erraten ist", "Wie viele Freunde man online hat", "Wie schnell das Internet ist" },
            "Wie schwer das gewählte Passwort zu erraten ist", "Die Anzeige bewertet Länge und Zeichenmischung, um grob abzuschätzen, wie sicher ein Passwort ist."),
        ("Sollte man das werkseitige Standardpasswort eines neuen WLAN-Routers behalten?", new[] { "Nein, es sollte sofort durch ein eigenes, sicheres Passwort ersetzt werden", "Ja, Standardpasswörter sind immer sicher genug", "Das spielt für die Sicherheit keine Rolle" },
            "Nein, es sollte sofort durch ein eigenes, sicheres Passwort ersetzt werden", "Standardpasswörter von Routern sind oft öffentlich bekannt oder leicht zu erraten und sollten direkt geändert werden."),
        ("Was bedeutet es, wenn eine Webseite meldet, ein Konto sei in einem bekannten \"Datenleck\" aufgetaucht?", new[] { "Das Passwort dieses Kontos sollte umgehend geändert werden", "Man kann die Meldung ignorieren", "Das Konto wird automatisch gelöscht und deshalb hier nicht zutrifft, was so nicht korrekt ist" },
            "Das Passwort dieses Kontos sollte umgehend geändert werden", "Ist ein Passwort durch ein Datenleck bekannt geworden, könnten Unbefugte es bereits kennen - schnelles Ändern verhindert Missbrauch."),
        ("Warum ist es riskant, ein Passwort im Browser auf einem fremden oder öffentlichen Computer zu speichern (Autofill)?", new[] { "Die nächste Person am Computer könnte sich damit einloggen", "Autofill macht Passwörter automatisch sicherer", "Das hat keinerlei Auswirkungen" },
            "Die nächste Person am Computer könnte sich damit einloggen", "Gespeicherte Zugangsdaten auf einem geteilten Gerät sind für die nächste Person nutzbar, wenn man sich nicht abmeldet."),
        ("Wo sollte man ein aufgeschriebenes Passwort auf keinen Fall aufbewahren?", new[] { "Auf einem Klebezettel direkt am Bildschirm", "In einem verschlossenen Passwort-Manager - eine haeufige, aber unzutreffende Vorstellung", "Nur im eigenen Kopf" },
            "Auf einem Klebezettel direkt am Bildschirm", "Ein sichtbar angebrachter Zettel macht das Passwort für jeden erkennbar, der Zugang zum Raum hat."),
        ("Was ist ein \"Wörterbuchangriff\" auf ein Passwort?", new[] { "Automatisches Ausprobieren gängiger Wörter und bekannter Passwörter", "Das Nachschlagen eines Wortes im echten Wörterbuch, auch wenn das manche zunaechst vermuten wuerden", "Ein Rechtschreibprogramm" },
            "Automatisches Ausprobieren gängiger Wörter und bekannter Passwörter", "Wörterbuchangriffe testen zuerst häufig genutzte Wörter und bekannte Passwortlisten - einfache Wörter sind daher besonders unsicher."),
        ("Wie schnell sollte man ein Passwort ändern, bei dem man einen Missbrauchsverdacht hat?", new[] { "Sofort", "Erst nach ein paar Wochen", "Gar nicht, das lohnt sich nicht" },
            "Sofort", "Je länger ein möglicherweise kompromittiertes Passwort gültig bleibt, desto größer das Risiko für Missbrauch."),
        ("Warum ist ein sehr kurzes Passwort (z.B. 4 Zeichen) fast immer unsicher?", new[] { "Weil es von einem Programm viel schneller durchprobiert werden kann als ein langes", "Weil kurze Passwörter gesetzlich verboten sind", "Weil kurze Passwörter automatisch bekannt werden" },
            "Weil es von einem Programm viel schneller durchprobiert werden kann als ein langes", "Mit jedem zusätzlichen Zeichen steigt die Anzahl möglicher Kombinationen stark an - kurze Passwörter sind entsprechend schneller zu knacken."),
        ("Was ist ein guter Trick, um sich ein langes, sicheres Passwort trotzdem merken zu können?", new[] { "Einen ungewöhnlichen Satz merken und daraus Anfangsbuchstaben plus Zahlen/Sonderzeichen bilden", "Das Passwort möglichst kurz halten", "Immer dasselbe einfache Wort verwenden, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" },
            "Einen ungewöhnlichen Satz merken und daraus Anfangsbuchstaben plus Zahlen/Sonderzeichen bilden", "Ein Merksatz wie \"Mein Hund bellt jeden Morgen um 7!\" lässt sich in ein starkes Passwort wie \"MHbjMu7!\" verwandeln."),
        ("Was ist ein \"Passkey\", eine modernere Alternative zum klassischen Passwort?", new[] { "Ein Anmeldeverfahren ohne Passwort, z.B. über Fingerabdruck oder Gerätefreigabe", "Ein noch längeres, klassisches Passwort", "Ein Passwort, das man mit Freunden teilt" },
            "Ein Anmeldeverfahren ohne Passwort, z.B. über Fingerabdruck oder Gerätefreigabe", "Passkeys ersetzen das klassische Passwort durch eine gerätegebundene, kryptografische Anmeldung - z.B. per Fingerabdruck freigegeben.")
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
        ("Darf man ein selbst aufgenommenes Foto von einem Freund einfach ohne Erlaubnis online stellen?", new[] { "Besser nicht - man sollte vorher fragen (Recht am eigenen Bild)", "Ja, immer, weil man das Foto gemacht hat - eine verbreitete, aber falsche Annahme", "Nur wenn das Foto unscharf ist" },
            "Besser nicht - man sollte vorher fragen (Recht am eigenen Bild)", "Das \"Recht am eigenen Bild\" bedeutet: Auch wer nur abgebildet ist, muss der Veröffentlichung meist zustimmen."),
        ("Was kann passieren, wenn man Musik ohne Erlaubnis herunterlädt und weiterverbreitet?", new[] { "Das kann eine Urheberrechtsverletzung sein und Folgen haben", "Das ist immer komplett erlaubt", "Musik ist nie urheberrechtlich geschützt, was einer genaueren Pruefung nicht standhaelt" },
            "Das kann eine Urheberrechtsverletzung sein und Folgen haben", "Musik ist urheberrechtlich geschützt - unerlaubtes Verbreiten kann rechtliche Konsequenzen haben."),
        ("Was bedeutet \"gemeinfrei\" (Public Domain) bei einem Werk?", new[] { "Das Werk darf von jedem frei genutzt werden, meist weil der Schutz abgelaufen ist", "Das Werk gehört einer einzigen Firma", "Das Werk darf niemand jemals anschauen, obwohl das auf den ersten Blick plausibel klingt" },
            "Das Werk darf von jedem frei genutzt werden, meist weil der Schutz abgelaufen ist", "Nach Ablauf der Schutzfrist wird ein Werk gemeinfrei und darf ohne Erlaubnis genutzt werden, z.B. viele alte Gemälde oder Musikstücke."),
        ("Wie lange gilt das Urheberrecht in Deutschland in der Regel nach dem Tod des Urhebers?", new[] { "70 Jahre", "5 Jahre", "Es gilt für immer" },
            "70 Jahre", "In Deutschland erlischt das Urheberrecht üblicherweise 70 Jahre nach dem Tod der Urheberin/des Urhebers."),
        ("Darf man ein kurzes Zitat aus einem Buch in einer Hausarbeit verwenden?", new[] { "Ja, unter Angabe der Quelle (Zitatrecht)", "Nein, niemals", "Nur wenn man das ganze Buch abschreibt" },
            "Ja, unter Angabe der Quelle (Zitatrecht)", "Das Zitatrecht erlaubt es, fremde Textstellen mit Quellenangabe zu zitieren, solange sie einem eigenen Zweck (z.B. Erläuterung) dienen."),
        ("Was bedeutet \"Plagiat\"?", new[] { "Fremde Werke oder Texte als eigene ausgeben, ohne Quelle zu nennen", "Ein besonders kreatives eigenes Werk", "Ein Fachbegriff für ein Musikinstrument, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Fremde Werke oder Texte als eigene ausgeben, ohne Quelle zu nennen", "Ein Plagiat liegt vor, wenn fremde geistige Leistungen ohne Kennzeichnung als eigene präsentiert werden."),
        ("Warum ist das Streamen eines Films über eine illegale, nicht lizenzierte Streaming-Seite problematisch?", new[] { "Es kann eine Urheberrechtsverletzung sein und schadet den Rechteinhabern finanziell", "Es ist völlig ohne rechtliche Bedeutung", "Filme unterliegen grundsätzlich keinem Urheberrecht und deshalb hier nicht zutrifft, was so nicht korrekt ist" },
            "Es kann eine Urheberrechtsverletzung sein und schadet den Rechteinhabern finanziell", "Illegale Streaming-Angebote nutzen Filme meist ohne Lizenz - das verletzt die Rechte der Produzenten und Kreativen."),
        ("Was ist ein \"Sample\" in der Musik, und warum ist es urheberrechtlich heikel?", new[] { "Ein kurzer Ausschnitt aus einem fremden Song, dessen Nutzung meist eine Erlaubnis braucht", "Ein neues, komplett eigenständiges Lied", "Ein technischer Begriff für Lautstärke" },
            "Ein kurzer Ausschnitt aus einem fremden Song, dessen Nutzung meist eine Erlaubnis braucht", "Wird ein fremder Musikausschnitt (Sample) in einem neuen Song verwendet, braucht das in der Regel die Zustimmung der Rechteinhaber."),
        ("Darf man ganze Schulbücher einscannen und frei im Internet teilen?", new[] { "Nein, das verletzt in der Regel das Urheberrecht des Verlags", "Ja, Schulbücher sind immer gemeinfrei - eine haeufige, aber unzutreffende Vorstellung", "Nur an Wochenenden erlaubt" },
            "Nein, das verletzt in der Regel das Urheberrecht des Verlags", "Schulbücher sind urheberrechtlich geschützte Werke - das komplette Kopieren und Verbreiten ohne Erlaubnis ist nicht erlaubt."),
        ("Was macht die GEMA in Deutschland (vereinfacht)?", new[] { "Sie sorgt dafür, dass Musikschaffende für die Nutzung ihrer Musik vergütet werden", "Sie komponiert selbst neue Musik", "Sie verbietet grundsätzlich alle Konzerte" },
            "Sie sorgt dafür, dass Musikschaffende für die Nutzung ihrer Musik vergütet werden", "Die GEMA sammelt Vergütungen ein, wenn geschützte Musik öffentlich gespielt oder verwendet wird, und leitet sie an die Urheber weiter."),
        ("Ist selbst geschriebener Programmcode urheberrechtlich geschützt?", new[] { "Ja, auch Software fällt unter das Urheberrecht", "Nein, Code ist grundsätzlich frei nutzbar, auch wenn das manche zunaechst vermuten wuerden", "Nur wenn er länger als 1000 Zeilen ist" },
            "Ja, auch Software fällt unter das Urheberrecht", "Auch Computerprogramme gelten als urheberrechtlich geschützte Werke - die Nutzung fremden Codes braucht eine passende Lizenz."),
        ("Was passiert oft, wenn man ein urheberrechtlich geschütztes Video ohne Erlaubnis auf YouTube hochlädt?", new[] { "Es kann gemeldet und gesperrt werden (Copyright-Strike)", "Nichts, YouTube prüft so etwas nie", "Man bekommt automatisch Geld dafür" },
            "Es kann gemeldet und gesperrt werden (Copyright-Strike)", "Plattformen wie YouTube erkennen geschütztes Material oft automatisch und können Uploads sperren oder melden."),
        ("Darf man ein fremdes Firmenlogo einfach für ein eigenes Schulprojekt-Plakat verwenden?", new[] { "Nur mit Erlaubnis oder passender Lizenz", "Ja, Logos sind grundsätzlich frei", "Nur wenn das Plakat größer als A4 ist, was bei genauerem Hinsehen nicht stimmt" },
            "Nur mit Erlaubnis oder passender Lizenz", "Logos sind meist sowohl urheberrechtlich als auch markenrechtlich geschützt - eine Nutzung ohne Erlaubnis kann problematisch sein."),
        ("Was ist der Unterschied zwischen Urheberrecht und Markenrecht (vereinfacht)?", new[] { "Urheberrecht schützt Werke wie Texte/Bilder, Markenrecht schützt Namen/Logos von Unternehmen", "Beide bedeuten exakt dasselbe", "Markenrecht gilt nur für Kleidung (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" },
            "Urheberrecht schützt Werke wie Texte/Bilder, Markenrecht schützt Namen/Logos von Unternehmen", "Das Urheberrecht schützt kreative Werke, das Markenrecht schützt Kennzeichen wie Firmennamen oder Logos vor Verwechslung."),
        ("Warum reicht \"Ich habe das Bild bei Google gefunden\" nicht als Erlaubnis, es zu nutzen?", new[] { "Google zeigt nur Bilder an, ist aber nicht der Rechteinhaber", "Google-Bilder sind immer automatisch gemeinfrei", "Bilder aus Suchmaschinen unterliegen keinem Urheberrecht, was einer genaueren Pruefung nicht standhaelt" },
            "Google zeigt nur Bilder an, ist aber nicht der Rechteinhaber", "Eine Suchmaschine ist nur ein Vermittler - die tatsächlichen Nutzungsrechte müssen bei der eigentlichen Quelle geprüft werden."),
        ("Was kann eine mögliche Folge einer schweren Urheberrechtsverletzung sein?", new[] { "Eine kostenpflichtige Abmahnung oder rechtliche Konsequenzen", "Ein Lob vom Urheber", "Gar keine Konsequenzen sind möglich, obwohl das auf den ersten Blick plausibel klingt" },
            "Eine kostenpflichtige Abmahnung oder rechtliche Konsequenzen", "Rechteinhaber können bei Verstößen rechtlich vorgehen, z.B. durch eine kostenpflichtige Abmahnung."),
        ("Was bedeutet die Lizenz \"CC0\" bei einem Bild oder Text?", new[] { "Der/die Urheber/in verzichtet komplett auf Rechte, das Werk darf frei genutzt werden", "Das Werk darf nur einmal angesehen werden", "CC0 bedeutet, dass eine Nutzung immer bezahlt werden muss" },
            "Der/die Urheber/in verzichtet komplett auf Rechte, das Werk darf frei genutzt werden", "Mit CC0 verzichtet die Urheberin/der Urheber freiwillig auf alle Rechte - das Werk darf ohne Einschränkung und ohne Namensnennung genutzt werden.")
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
        ("Was ist ein sinnvoller erster Schritt, wenn man Cybermobbing beobachtet (nicht selbst betroffen)?", new[] { "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen und nichts tun, was die eigentliche Bedeutung des Begriffs verfehlt", "Mitmachen" },
            "Der betroffenen Person beistehen und Hilfe holen", "Wegschauen verstärkt das Problem - Unterstützung und das Einschalten von Erwachsenen helfen der betroffenen Person."),
        ("Was kann ein Opfer von Cybermobbing als Beweis sichern?", new[] { "Screenshots der Nachrichten", "Nichts, Beweise sind unwichtig", "Nur das Datum, ohne Text" },
            "Screenshots der Nachrichten", "Screenshots dokumentieren beleidigende Nachrichten und helfen später beim Melden oder bei Erwachsenen/Schule."),
        ("Warum trauen sich manche Menschen im Internet zu Dingen, die sie im echten Leben nie sagen würden?", new[] { "Weil die scheinbare Anonymität die Hemmschwelle senkt", "Weil es im Internet keine Konsequenzen gibt", "Weil Cybermobbing gesetzlich erlaubt ist" },
            "Weil die scheinbare Anonymität die Hemmschwelle senkt", "Das Gefühl von Anonymität und fehlende direkte Reaktion senken bei manchen die Hemmschwelle für Beleidigungen."),
        ("Was ist KEIN sinnvoller Umgang mit Cybermobbing?", new[] { "Selbst zurückbeleidigen und mitmachen", "Nachrichten dokumentieren und Erwachsenen zeigen", "Den Absender blockieren und melden" },
            "Selbst zurückbeleidigen und mitmachen", "Zurückbeleidigen verschärft die Situation meist - besser sind Dokumentieren, Blockieren, Melden und Hilfe holen."),
        ("Was unterscheidet einen normalen Streit von echtem Cybermobbing?", new[] { "Cybermobbing ist wiederholt und oft mit einem Machtungleichgewicht verbunden", "Bei Cybermobbing lachen am Ende alle Beteiligten und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Es gibt keinen Unterschied"},
            "Cybermobbing ist wiederholt und oft mit einem Machtungleichgewicht verbunden", "Ein einmaliger Streit ist kein Mobbing - Cybermobbing zeichnet sich durch Wiederholung und ein Machtgefälle zwischen Täter/in und Opfer aus."),
        ("Welche deutsche Anlaufstelle können Kinder und Jugendliche bei Problemen wie Mobbing anonym und kostenlos kontaktieren?", new[] { "Die \"Nummer gegen Kummer\"", "Das Finanzamt", "Die Verkehrspolizei" },
            "Die \"Nummer gegen Kummer\"", "Die \"Nummer gegen Kummer\" bietet Kindern und Jugendlichen anonyme, kostenlose Beratung bei Problemen wie Mobbing."),
        ("Warum reicht das bloße Blockieren einer mobbenden Person manchmal nicht aus?", new[] { "Weil die Person mit einem anderen oder gefälschten Konto weitermachen könnte", "Weil Blockieren technisch gar nicht möglich ist", "Weil Blockieren die Situation immer sofort komplett löst - eine haeufige, aber unzutreffende Vorstellung" },
            "Weil die Person mit einem anderen oder gefälschten Konto weitermachen könnte", "Manche Täter/innen erstellen neue oder gefälschte Konten - deshalb hilft zusätzlich das Melden bei der Plattform und das Einbeziehen von Erwachsenen."),
        ("Ist das absichtliche, wiederholte Ausschließen einer Person aus einer wichtigen Gruppen-Chat auch eine Form von Cybermobbing?", new[] { "Ja", "Nein, das zählt nie dazu", "Nur wenn es in einer Fremdsprache passiert" },
            "Ja", "Auch gezieltes, wiederholtes soziales Ausschließen über digitale Kanäle gilt als eine Form von Cybermobbing."),
        ("Was können psychische Folgen von Cybermobbing für Betroffene sein?", new[] { "Angst, Traurigkeit und sozialer Rückzug", "Ausschließlich positive Gefühle, auch wenn das manche zunaechst vermuten wuerden", "Keinerlei Auswirkungen" },
            "Angst, Traurigkeit und sozialer Rückzug", "Cybermobbing kann ernsthafte psychische Belastungen wie Angst, depressive Verstimmungen oder sozialen Rückzug auslösen."),
        ("Warum ist es wichtig, beleidigende Nachrichten nicht sofort zu löschen, bevor man Hilfe holt?", new[] { "Damit Screenshots als Beweise für Erwachsene oder Meldestellen erhalten bleiben", "Weil gelöschte Nachrichten automatisch wieder auftauchen", "Weil das gesetzlich vorgeschrieben ist" },
            "Damit Screenshots als Beweise für Erwachsene oder Meldestellen erhalten bleiben", "Ohne Beweise ist es für Schule, Eltern oder Plattformen schwerer, gegen die Täter/innen vorzugehen."),
        ("Was bedeutet der \"Bystander-Effekt\" bei Cybermobbing?", new[] { "Beobachtende greifen oft nicht ein, weil sie denken, jemand anderes wird es schon tun", "Jeder Beobachter meldet automatisch den Vorfall", "Beobachter werden nie zu Mittätern" },
            "Beobachtende greifen oft nicht ein, weil sie denken, jemand anderes wird es schon tun", "In Gruppen fühlt sich oft niemand persönlich verantwortlich einzugreifen - genau das lässt Mobbing weiterlaufen."),
        ("Was kann eine beobachtende Person aktiv gegen Cybermobbing tun?", new[] { "Der betroffenen Person offen Unterstützung zeigen und Erwachsene informieren", "Einfach weiterscrollen und nichts sagen", "Selbst noch einen gemeinen Kommentar dazuschreiben, was bei genauerem Hinsehen nicht stimmt" },
            "Der betroffenen Person offen Unterstützung zeigen und Erwachsene informieren", "Aktives Eingreifen - Unterstützung zeigen, dokumentieren, Erwachsene informieren - kann Mobbing stoppen, bevor es eskaliert."),
        ("Dürfen peinliche Fotos einer Mitschülerin/eines Mitschülers ohne deren Erlaubnis in einer Chatgruppe geteilt werden?", new[] { "Nein, das kann rechtliche und emotionale Folgen haben", "Ja, wenn das Foto lustig genug ist", "Nur wenn mehr als 10 Personen in der Gruppe sind (was so in der Praxis nicht zutrifft)" },
            "Nein, das kann rechtliche und emotionale Folgen haben", "Das \"Recht am eigenen Bild\" gilt auch in privaten Chatgruppen - unerlaubtes Teilen kann Betroffene verletzen und Konsequenzen haben."),
        ("Was ist ein \"Hasskommentar\"?", new[] { "Eine beleidigende, oft anonyme Nachricht mit dem Ziel, jemanden herabzuwürdigen", "Ein besonders konstruktiver Verbesserungsvorschlag - eine verbreitete, aber falsche Annahme", "Ein Kommentar mit vielen Likes" },
            "Eine beleidigende, oft anonyme Nachricht mit dem Ziel, jemanden herabzuwürdigen", "Hasskommentare zielen bewusst darauf ab, eine Person oder Gruppe zu beleidigen oder herabzusetzen."),
        ("Warum fühlen sich manche Menschen im Internet 'sicherer', jemanden zu beleidigen, als sie es im echten Leben täten?", new[] { "Weil die scheinbare Anonymität und fehlende direkte Reaktion Hemmungen abbauen", "Weil das Internet Beleidigungen automatisch verbietet, was einer genaueren Pruefung nicht standhaelt", "Weil online geschriebene Worte keine Wirkung haben" },
            "Weil die scheinbare Anonymität und fehlende direkte Reaktion Hemmungen abbauen", "Ohne direkten Blickkontakt und mit vermeintlicher Anonymität sinkt für manche die Hemmschwelle deutlich."),
        ("Kann Cybermobbing auch strafrechtliche Folgen für die Täterinnen und Täter haben?", new[] { "Ja, z.B. wegen Beleidigung oder Verleumdung", "Nein, das Internet ist rechtsfrei", "Nur wenn die Täter über 30 Jahre alt sind" },
            "Ja, z.B. wegen Beleidigung oder Verleumdung", "Beleidigung, üble Nachrede oder Verleumdung sind auch online strafbar und können rechtliche Konsequenzen haben."),
        ("Was sollte man tun, wenn man selbst versehentlich an einer gemeinen Aktion gegen jemanden beteiligt war?", new[] { "Sich entschuldigen und das eigene Verhalten stoppen", "So tun, als sei nichts passiert", "Die Schuld komplett auf andere schieben, obwohl das auf den ersten Blick plausibel klingt" },
            "Sich entschuldigen und das eigene Verhalten stoppen", "Verantwortung zu übernehmen und sich zu entschuldigen ist ein wichtiger erster Schritt, um Schaden zu begrenzen."),
        ("Warum verschwindet ein einmal im Internet geteilter gemeiner Kommentar oft nicht wirklich, auch nach dem Löschen?", new[] { "Weil er von anderen kopiert, fotografiert oder weiterverbreitet worden sein kann", "Weil das Internet nichts jemals wirklich speichert", "Weil gelöschte Kommentare sich automatisch neu schreiben" },
            "Weil er von anderen kopiert, fotografiert oder weiterverbreitet worden sein kann", "Screenshots und Weiterleitungen sorgen dafür, dass Inhalte oft weiterexistieren, selbst wenn das Original gelöscht wird."),
        ("Was ist ein sinnvoller erster Schritt für Eltern oder Lehrkräfte, wenn ein Kind von Cybermobbing berichtet?", new[] { "Ernst nehmen, zuhören und gemeinsam nach Lösungen suchen", "Das Handy sofort und ohne Gespräch wegnehmen", "Die Sache herunterspielen, das wird schon vergehen, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Ernst nehmen, zuhören und gemeinsam nach Lösungen suchen", "Ernstes Zuhören und gemeinsames Vorgehen (z.B. Schule einbeziehen, Beweise sichern) helfen Betroffenen weit mehr als Bagatellisieren oder überzogene Reaktionen.")
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
        ("Ein Foto zu einer Nachricht sieht sehr extrem oder unglaubwürdig aus. Was ist ein guter nächster Schritt?", new[] { "Prüfen, ob das Bild schon einmal in einem ganz anderen Zusammenhang auftauchte", "Sofort glauben und teilen", "Das Bild ignorieren, aber den Text glauben und deshalb hier nicht zutrifft, was so nicht korrekt ist" },
            "Prüfen, ob das Bild schon einmal in einem ganz anderen Zusammenhang auftauchte", "Alte oder aus dem Zusammenhang gerissene Bilder werden oft zu neuen Falschmeldungen wiederverwendet."),
        ("Was bedeutet \"Faktencheck\"?", new[] { "Die Überprüfung, ob Behauptungen in einer Nachricht stimmen", "Das schnelle Überfliegen einer Überschrift - eine haeufige, aber unzutreffende Vorstellung", "Das Zählen von Kommentaren" },
            "Die Überprüfung, ob Behauptungen in einer Nachricht stimmen", "Ein Faktencheck vergleicht Behauptungen mit belegbaren Fakten und Quellen."),
        ("Was ist ein \"Deepfake\"?", new[] { "Ein mit KI erzeugtes, täuschend echtes gefälschtes Video oder Bild", "Ein besonders tiefes Unterwasserfoto", "Ein Fachbegriff für sehr laute Musik" },
            "Ein mit KI erzeugtes, täuschend echtes gefälschtes Video oder Bild", "Deepfakes nutzen künstliche Intelligenz, um Gesichter/Stimmen täuschend echt in Videos einzusetzen - ein Warnsignal für moderne Falschinformation."),
        ("Warum sollte man bei einem sehr emotionalen, aufwühlenden Nachrichtentitel besonders vorsichtig sein?", new[] { "Weil starke Emotionen oft absichtlich erzeugt werden, um kritisches Nachdenken zu verhindern", "Weil emotionale Titel immer automatisch wahr sind", "Weil solche Titel gesetzlich verboten sind" },
            "Weil starke Emotionen oft absichtlich erzeugt werden, um kritisches Nachdenken zu verhindern", "Wut oder Angst lassen Menschen schneller (und unkritischer) teilen - genau das nutzen Verbreiter von Falschmeldungen gezielt aus."),
        ("Was bedeutet \"Clickbait\"?", new[] { "Eine reißerische Überschrift, die vor allem zum Klicken verleiten soll", "Ein technischer Fachbegriff für schnelles Internet, auch wenn das manche zunaechst vermuten wuerden", "Ein Programm zum Sortieren von Nachrichten" },
            "Eine reißerische Überschrift, die vor allem zum Klicken verleiten soll", "Clickbait-Überschriften versprechen oft mehr, als der eigentliche Inhalt hält, um Klicks und Werbeeinnahmen zu erzeugen."),
        ("Was kann ein Hinweis auf eine gefälschte Nachrichten-Webseite sein?", new[] { "Eine der echten Seite ähnliche, aber leicht falsch geschriebene Internetadresse", "Ein sehr bekanntes, seriöses Impressum", "Viele unterschiedliche, unabhängige Quellenangaben" },
            "Eine der echten Seite ähnliche, aber leicht falsch geschriebene Internetadresse", "Betrügerische Seiten imitieren oft bekannte Nachrichtenportale mit minimal veränderten Adressen (z.B. einem zusätzlichen Buchstaben)."),
        ("Warum verbreiten sich Falschmeldungen im Internet oft schneller als spätere Richtigstellungen?", new[] { "Weil emotionale, aufregende Inhalte häufiger geteilt werden als nüchterne Korrekturen", "Weil Richtigstellungen gesetzlich verboten sind, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Weil Falschmeldungen immer kürzer sind" },
            "Weil emotionale, aufregende Inhalte häufiger geteilt werden als nüchterne Korrekturen", "Studien zeigen, dass aufregende Falschmeldungen sich oft schneller verbreiten als ruhige, sachliche Richtigstellungen."),
        ("Was ist \"Satire\" im Zusammenhang mit Nachrichten, und warum wird sie manchmal mit Fake News verwechselt?", new[] { "Satire übertreibt bewusst zur Unterhaltung/Kritik - aus dem Zusammenhang gerissen wirkt sie wie eine echte Falschmeldung", "Satire ist ein anderes Wort für Werbung", "Satire bedeutet, dass eine Nachricht garantiert wahr ist" },
            "Satire übertreibt bewusst zur Unterhaltung/Kritik - aus dem Zusammenhang gerissen wirkt sie wie eine echte Falschmeldung", "Satirische Beiträge sind bewusst übertrieben gemeint - werden sie ohne diesen Kontext geteilt, können sie fälschlich für echte Nachrichten gehalten werden."),
        ("Was kann man tun, um herauszufinden, ob ein Foto zu einer Nachricht schon einmal in einem anderen Zusammenhang genutzt wurde?", new[] { "Eine umgekehrte Bildersuche im Internet durchführen", "Das Bild einfach größer zoomen", "Die Helligkeit des Bildes verändern - eine verbreitete, aber falsche Annahme" },
            "Eine umgekehrte Bildersuche im Internet durchführen", "Eine umgekehrte Bildersuche zeigt, wo ein Bild bereits früher online aufgetaucht ist - so lassen sich wiederverwendete alte Fotos entlarven."),
        ("Warum sollte man eine wichtige Behauptung, die nur auf eine anonyme Quelle ohne Namen gestützt ist, kritisch sehen?", new[] { "Weil man die Glaubwürdigkeit einer namenlosen Quelle schwerer einschätzen kann", "Weil anonyme Quellen immer lügen", "Weil Namen bei Nachrichten grundsätzlich unwichtig sind, was einer genaueren Pruefung nicht standhaelt" },
            "Weil man die Glaubwürdigkeit einer namenlosen Quelle schwerer einschätzen kann", "Ohne nachprüfbare Quelle lässt sich eine Behauptung schwerer einordnen und überprüfen."),
        ("Was bedeutet \"Verifizierung\" einer Nachricht?", new[] { "Die Überprüfung, ob eine Information wirklich stimmt, z.B. durch mehrere unabhängige Quellen", "Das Löschen einer Nachricht", "Das Übersetzen einer Nachricht in eine andere Sprache, obwohl das auf den ersten Blick plausibel klingt" },
            "Die Überprüfung, ob eine Information wirklich stimmt, z.B. durch mehrere unabhängige Quellen", "Verifizierung bedeutet, eine Behauptung anhand unabhängiger, verlässlicher Quellen zu überprüfen, bevor man sie glaubt oder teilt."),
        ("Warum können auch gutgemeinte Weiterleitungen von Familienmitgliedern in Chatgruppen Falschinformationen verbreiten?", new[] { "Weil auch sie oft ungeprüfte Inhalte teilen, ohne die Quelle zu checken", "Weil Familienmitglieder nie Fehler machen", "Weil Chatgruppen automatisch alle Inhalte prüfen, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Weil auch sie oft ungeprüfte Inhalte teilen, ohne die Quelle zu checken", "Gute Absicht schützt nicht vor Fehlinformation - auch vertraute Personen teilen manchmal ungeprüfte Inhalte weiter."),
        ("Was ist ein \"Bot\" im Zusammenhang mit der Verbreitung von Falschnachrichten?", new[] { "Ein automatisiertes Programm, das massenhaft Inhalte postet oder teilt", "Ein besonders vertrauenswürdiger Nutzer-Account und deshalb hier nicht zutrifft", "Ein Fachbegriff für einen Zeitungsjournalisten" },
            "Ein automatisiertes Programm, das massenhaft Inhalte postet oder teilt", "Bots können automatisiert und in großer Zahl Inhalte verbreiten und so den Eindruck erwecken, viele echte Menschen würden etwas teilen."),
        ("Warum reicht eine hohe Anzahl an Likes oder Shares eines Beitrags NICHT als Beweis für dessen Wahrheit?", new[] { "Weil auch falsche Inhalte sich sehr schnell und weit verbreiten können", "Weil Likes automatisch nur bei wahren Inhalten vergeben werden, was so nicht korrekt ist", "Weil Likes von einer Regierungsbehörde geprüft werden" },
            "Weil auch falsche Inhalte sich sehr schnell und weit verbreiten können", "Beliebtheit sagt nichts über den Wahrheitsgehalt aus - auch Falschmeldungen können viral gehen."),
        ("Was ist ein guter Grundsatz beim Teilen von Nachrichten in sozialen Netzwerken?", new[] { "Erst prüfen, dann teilen", "Immer sofort teilen, um schnell zu sein", "Nur die Überschrift lesen und dann teilen" },
            "Erst prüfen, dann teilen", "Kurz innezuhalten und die Quelle zu prüfen, bevor man etwas weiterleitet, verhindert die unabsichtliche Verbreitung von Falschmeldungen."),
        ("Warum sind unabhängige Faktencheck-Organisationen (z.B. Correctiv) nützlich?", new[] { "Sie prüfen gezielt verdächtige Behauptungen und veröffentlichen die Ergebnisse", "Sie erfinden neue Nachrichten", "Sie ersetzen alle anderen Nachrichtenquellen" },
            "Sie prüfen gezielt verdächtige Behauptungen und veröffentlichen die Ergebnisse", "Faktencheck-Organisationen recherchieren gezielt umstrittene Behauptungen und veröffentlichen nachvollziehbare Einordnungen."),
        ("Was kann eine Folge davon sein, wenn man ungeprüft eine Falschmeldung weiterverbreitet?", new[] { "Man trägt selbst zur Verbreitung von Fehlinformationen bei", "Es hat garantiert keinerlei Auswirkungen", "Die Meldung wird dadurch automatisch wahr" },
            "Man trägt selbst zur Verbreitung von Fehlinformationen bei", "Jede Weiterleitung vergrößert die Reichweite einer Nachricht - bei ungeprüften Inhalten trägt man so aktiv zur Verbreitung von Fehlinformationen bei.")
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
        ("Was ist ein Algorithmus einfach erklärt?", new[] { "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein anderes Wort für Computer - eine haeufige, aber unzutreffende Vorstellung", "Ein Videospiel" },
            "Eine genaue Schritt-für-Schritt-Anleitung zur Lösung eines Problems", "Ein Algorithmus ist eine eindeutige Abfolge von Schritten, die z.B. ein Programm abarbeitet, um eine Aufgabe zu lösen."),
        ("Warum zeigen soziale Netzwerke jedem Nutzer unterschiedliche Inhalte im Feed an?", new[] { "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Der Zufall entscheidet komplett, auch wenn das manche zunaechst vermuten wuerden", "Alle sehen exakt dasselbe" },
            "Ein Algorithmus wählt Inhalte basierend auf dem bisherigen Verhalten aus", "Empfehlungsalgorithmen werten aus, was ein Nutzer bisher angeklickt/geliked hat, und passen den Feed entsprechend an."),
        ("Wie nennt man es, wenn ein Algorithmus dir bevorzugt Inhalte zeigt, die deine bisherige Meinung bestätigen?", new[] { "Filterblase / Echo-Kammer", "Firewall, was bei genauerem Hinsehen nicht stimmt", "Update" },
            "Filterblase / Echo-Kammer", "In einer Filterblase sieht man vor allem Inhalte, die zur eigenen Meinung passen - andere Sichtweisen werden seltener gezeigt."),
        ("Was macht ein Suchmaschinen-Algorithmus im Kern?", new[] { "Er sortiert Suchergebnisse nach Relevanz für die Suchanfrage", "Er löscht zufällig Webseiten", "Er erstellt neue Internetseiten (was so in der Praxis nicht zutrifft)" },
            "Er sortiert Suchergebnisse nach Relevanz für die Suchanfrage", "Suchalgorithmen bewerten und ordnen Webseiten danach, wie gut sie zur Suchanfrage passen."),
        ("Kann ein Algorithmus \"unfair\" sein?", new[] { "Ja, wenn er mit unfairen/verzerrten Daten trainiert oder programmiert wurde", "Nein, Algorithmen sind immer neutral", "Nein, weil Computer nicht denken können - eine verbreitete, aber falsche Annahme" },
            "Ja, wenn er mit unfairen/verzerrten Daten trainiert oder programmiert wurde", "Algorithmen übernehmen Verzerrungen (Bias) aus den Daten oder Entscheidungen, mit denen sie erstellt wurden."),
        ("Was ist ein \"Empfehlungsalgorithmus\"?", new[] { "Ein Programm, das basierend auf früherem Verhalten passende Inhalte vorschlägt", "Ein Programm, das zufällig irgendwelche Inhalte zeigt", "Ein Fachbegriff für eine Suchmaschine ohne Ergebnisse" },
            "Ein Programm, das basierend auf früherem Verhalten passende Inhalte vorschlägt", "Empfehlungsalgorithmen werten frühere Klicks, Likes oder Käufe aus, um passende neue Vorschläge zu machen."),
        ("Was bedeutet \"künstliche Intelligenz\" im Unterschied zu einem einfachen, festen Algorithmus (vereinfacht)?", new[] { "KI kann aus Daten lernen und sich anpassen, ein einfacher Algorithmus folgt immer denselben festen Schritten", "Beide Begriffe bedeuten exakt dasselbe, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "KI ist ein anderes Wort für Hardware" },
            "KI kann aus Daten lernen und sich anpassen, ein einfacher Algorithmus folgt immer denselben festen Schritten", "Klassische Algorithmen arbeiten feste Regeln ab, während KI-Systeme aus Beispieldaten selbst Muster lernen können."),
        ("Warum zeigt eine Videoplattform nach einem Video oft ein sehr ähnliches als Nächstes an?", new[] { "Der Algorithmus versucht, Zuschauende möglichst lange auf der Plattform zu halten", "Es ist reiner Zufall", "Jedes Video wird zufällig neu sortiert, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Der Algorithmus versucht, Zuschauende möglichst lange auf der Plattform zu halten", "Empfehlungsalgorithmen sind oft darauf ausgelegt, die Nutzungsdauer zu erhöhen, indem sie ähnliche, ansprechende Inhalte vorschlagen."),
        ("Was ist ein \"Trainingsdatensatz\" beim maschinellen Lernen (vereinfacht)?", new[] { "Beispieldaten, mit denen ein KI-System lernt, Muster zu erkennen", "Ein Fitnessprogramm für Computer", "Ein Ordner mit gelöschten Dateien" },
            "Beispieldaten, mit denen ein KI-System lernt, Muster zu erkennen", "Ein KI-System wird mit vielen Beispieldaten trainiert, um daraus Muster für spätere Vorhersagen abzuleiten."),
        ("Warum kann ein Algorithmus, der mit unfairen Trainingsdaten trainiert wurde, diskriminierende Ergebnisse liefern?", new[] { "Weil er die Muster und Vorurteile aus den Trainingsdaten übernimmt", "Weil Algorithmen absichtlich böse programmiert werden und deshalb hier nicht zutrifft", "Weil das technisch unmöglich ist" },
            "Weil er die Muster und Vorurteile aus den Trainingsdaten übernimmt", "Enthalten Trainingsdaten Verzerrungen (z.B. einseitige Beispiele), übernimmt das System diese Verzerrungen in seine Entscheidungen."),
        ("Was ist ein einfaches Beispiel für einen Algorithmus im Alltag, ganz ohne Computer?", new[] { "Ein Kochrezept mit genauen Schritten", "Ein zufälliges Gefühl", "Eine Emotion wie Freude, was so nicht korrekt ist" },
            "Ein Kochrezept mit genauen Schritten", "Ein Rezept ist eine klare Schritt-für-Schritt-Anleitung - genau das ist die Grundidee eines Algorithmus."),
        ("Was bedeutet \"Personalisierung\" bei Online-Diensten?", new[] { "Inhalte werden individuell an das bisherige Verhalten einer Person angepasst", "Alle Nutzer sehen exakt dieselben Inhalte", "Eine Webseite wird für jeden zufällig neu programmiert - eine haeufige, aber unzutreffende Vorstellung" },
            "Inhalte werden individuell an das bisherige Verhalten einer Person angepasst", "Personalisierte Feeds/Angebote basieren auf ausgewertetem individuellem Nutzungsverhalten."),
        ("Warum kann starke Personalisierung von Inhalten problematisch sein?", new[] { "Weil man dadurch seltener andere Sichtweisen oder Themen zu sehen bekommt", "Weil personalisierte Inhalte immer falsch sind, auch wenn das manche zunaechst vermuten wuerden", "Weil das technisch gar nicht funktioniert" },
            "Weil man dadurch seltener andere Sichtweisen oder Themen zu sehen bekommt", "Starke Personalisierung kann zu einer \"Filterblase\" führen, in der man vor allem Bestätigendes statt neuer Perspektiven sieht."),
        ("Was ist ein \"Chatbot\"?", new[] { "Ein Programm, das mithilfe von Algorithmen automatisch auf Textnachrichten antwortet", "Ein menschlicher Kundenservice-Mitarbeiter", "Ein Computerspiel-Charakter ohne Programmierung, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" },
            "Ein Programm, das mithilfe von Algorithmen automatisch auf Textnachrichten antwortet", "Chatbots nutzen Algorithmen bzw. KI, um automatisiert auf Anfragen in Textform zu reagieren."),
        ("Was können Programmiererinnen und Programmierer tun, um einen Algorithmus fairer zu machen?", new[] { "Die Trainingsdaten und Entscheidungsregeln bewusst auf Verzerrungen prüfen", "Den Algorithmus einfach schneller laufen lassen", "Gar nichts, Fairness lässt sich nicht beeinflussen" },
            "Die Trainingsdaten und Entscheidungsregeln bewusst auf Verzerrungen prüfen", "Durch gezielte Prüfung und Anpassung von Daten/Regeln lassen sich unfaire Muster in Algorithmen verringern."),
        ("Wofür wird ein Algorithmus bei einer Navigations-App genutzt?", new[] { "Um aus vielen möglichen Wegen den schnellsten/besten zu berechnen", "Um das Wetter vorherzusagen - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Um Musik abzuspielen" },
            "Um aus vielen möglichen Wegen den schnellsten/besten zu berechnen", "Navigations-Algorithmen berechnen aus Straßendaten und Verkehrsinfos die vermutlich schnellste Route."),
        ("Was bedeutet es, wenn eine App \"lernt\", welche Inhalte dir gefallen?", new[] { "Sie wertet dein bisheriges Verhalten (Klicks, Likes) aus, um Vorschläge anzupassen", "Die App liest heimlich deine Gedanken", "Die App fragt dich jeden Tag persönlich, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" },
            "Sie wertet dein bisheriges Verhalten (Klicks, Likes) aus, um Vorschläge anzupassen", "Das \"Lernen\" besteht aus der Auswertung von Verhaltensdaten, nicht aus echtem menschlichem Verstehen."),
        ("Warum ist völlige Neutralität bei jedem Algorithmus schwer zu garantieren?", new[] { "Weil Menschen die Regeln/Daten festlegen und dabei eigene Annahmen einfließen", "Weil Algorithmen von Natur aus perfekt objektiv sind und deshalb hier nicht zutrifft", "Weil Computer keine Fehler machen können" },
            "Weil Menschen die Regeln/Daten festlegen und dabei eigene Annahmen einfließen", "Da Menschen Algorithmen entwickeln und Trainingsdaten auswählen, können unbewusste Annahmen und Verzerrungen einfließen."),
        ("Was ist ein Vorteil davon, zu wissen, dass ein Feed von einem Algorithmus sortiert wird?", new[] { "Man hinterfragt eher kritisch, warum genau diese Inhalte gezeigt werden", "Man muss sich dann keine Gedanken mehr machen", "Es ändert nichts an der eigenen Nutzung" },
            "Man hinterfragt eher kritisch, warum genau diese Inhalte gezeigt werden", "Wer weiß, dass ein Algorithmus die Auswahl steuert, hinterfragt eher, warum bestimmte Inhalte bevorzugt angezeigt werden."),
        ("Was passiert technisch, wenn ein Computerprogramm eine Reihe von \"Wenn-Dann\"-Regeln abarbeitet?", new[] { "Es führt einen Algorithmus aus", "Es speichert nur Bilder", "Es schaltet sich automatisch aus" },
            "Es führt einen Algorithmus aus", "Eine klar festgelegte Abfolge von Schritten oder Regeln zur Lösung einer Aufgabe ist die Grundidee eines Algorithmus.")
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
