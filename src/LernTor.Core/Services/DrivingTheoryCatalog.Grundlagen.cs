using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class DrivingTheoryCatalog
{
    /// <summary>
    /// Sachgebiete 1-7: von den persönlichen Voraussetzungen bis zu den Fahrmanövern.
    ///
    /// <para>Die Fragen sind selbst geschrieben. Der amtliche Fragenkatalog gehört der
    /// TÜV|DEKRA arge tp 21 und darf hier nicht hinein - inhaltlich decken diese Fragen dieselben
    /// Sachgebiete ab, wortgleich mit der Prüfung sind sie aber nicht.</para>
    ///
    /// <para><b>Fehlerpunkte</b> nach Gefährlichkeit: 5 für alles, wo ein Irrtum Menschen in
    /// Lebensgefahr bringt (Vorfahrt, Alkohol, Bahnübergang, Geschwindigkeit), 2-3 für
    /// Formalien.</para>
    /// </summary>
    private static readonly TheoryQuestion[] Grundlagen =
    {
        // ---------------------------------------------- Persönliche Voraussetzungen
        new()
        {
            Id = "pv-01", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 5,
            Prompt = "Du bist 18 und in der Probezeit. Wie viel Alkohol darfst du im Blut haben, wenn du fährst?",
            Options = new[] { "Gar keinen - 0,0 Promille", "Bis 0,3 Promille", "Bis 0,5 Promille", "Bis 0,8 Promille" },
            CorrectIndices = new[] { 0 },
            Explanation = "Für Fahranfänger in der Probezeit und für alle unter 21 gilt ein absolutes Alkoholverbot. Die 0,5-Promille-Grenze gilt erst danach - und auch dann ist sie eine Grenze, keine Empfehlung."
        },
        new()
        {
            Id = "pv-02", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 5,
            Prompt = "Du merkst auf der Autobahn, dass du müde wirst. Was tust du?",
            Options = new[]
            {
                "Bei der nächsten Gelegenheit anhalten und schlafen",
                "Fenster öffnen und Musik lauter drehen, dann geht es schon",
                "Kaffee trinken und weiterfahren",
                "Schneller fahren, damit du früher ankommst"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Gegen Müdigkeit hilft nur Schlaf. Frische Luft und Kaffee wirken kurz und täuschen darüber hinweg, dass die Reaktionszeit längst zu lang ist. Sekundenschlaf bei 100 km/h heißt 28 Meter blind."
        },
        new()
        {
            Id = "pv-03", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 4,
            Prompt = "Warum ist Telefonieren am Steuer auch mit Freisprecheinrichtung gefährlich?",
            Options = new[]
            {
                "Weil das Gespräch die Aufmerksamkeit bindet, nicht die Hand",
                "Weil die Freisprecheinrichtung das Radio stört",
                "Weil man dabei automatisch schneller fährt",
                "Weil die Verbindung im Auto schlechter ist"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Nicht die Hand am Ohr ist das Problem, sondern der Kopf beim Gespräch. Wer telefoniert, übersieht nachweislich mehr - deshalb ist Freisprechen zwar erlaubt, aber nicht harmlos."
        },
        new()
        {
            Id = "pv-04", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 4,
            Prompt = "Du nimmst ein Medikament, auf dessen Beipackzettel vor Müdigkeit gewarnt wird. Was gilt?",
            Options = new[]
            {
                "Du bist selbst dafür verantwortlich, ob du fahrtüchtig bist",
                "Medikamente sind egal, solange sie ärztlich verordnet sind",
                "Nur verschreibungspflichtige Medikamente können die Fahrtüchtigkeit beeinträchtigen",
                "Solange du dich fit fühlst, darfst du fahren"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Wer sich ans Steuer setzt, muss fahrtüchtig sein - das gilt unabhängig davon, woher die Beeinträchtigung kommt. Auch frei verkäufliche Mittel (Erkältung, Allergie) können müde machen."
        },
        new()
        {
            Id = "pv-05", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 3,
            Prompt = "Wie lange dauert die Probezeit für Fahranfänger?",
            Options = new[] { "Zwei Jahre", "Ein Jahr", "Drei Jahre", "Bis zum 21. Geburtstag" },
            CorrectIndices = new[] { 0 },
            Explanation = "Zwei Jahre ab Ausstellung der Fahrerlaubnis. Bei einem schweren Verstoß verlängert sie sich auf vier Jahre, dazu kommt ein Aufbauseminar."
        },
        new()
        {
            Id = "pv-06", Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen, Points = 4,
            Prompt = "Welche Folgen kann Alkohol am Steuer haben? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Das Sehvermögen lässt nach, besonders in der Dunkelheit",
                "Die Reaktionszeit wird länger",
                "Man überschätzt die eigene Leistungsfähigkeit",
                "Die Sehschärfe verbessert sich kurzzeitig"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Alkohol verschlechtert Sehen und Reaktion und lässt einen das zugleich für unproblematisch halten - diese Kombination macht ihn so gefährlich. Besser wird davon nichts."
        },

        // ---------------------------------------------- Rechtliche Rahmenbedingungen
        new()
        {
            Id = "rr-01", Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen, Points = 3,
            Prompt = "Welche Papiere musst du beim Fahren dabeihaben? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Den Führerschein",
                "Die Zulassungsbescheinigung Teil I (Fahrzeugschein)",
                "Die Zulassungsbescheinigung Teil II (Fahrzeugbrief)",
                "Den Kaufvertrag des Fahrzeugs"
            },
            CorrectIndices = new[] { 0, 1 },
            Explanation = "Führerschein und Fahrzeugschein gehören ins Auto. Der Fahrzeugbrief ist der Eigentumsnachweis und bleibt zu Hause - im Handschuhfach wäre er bei einem Diebstahl gleich mit weg."
        },
        new()
        {
            Id = "rr-02", Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen, Points = 3,
            Prompt = "Ab wie vielen Punkten in Flensburg wird die Fahrerlaubnis entzogen?",
            Options = new[] { "Ab 8 Punkten", "Ab 5 Punkten", "Ab 12 Punkten", "Ab 18 Punkten" },
            CorrectIndices = new[] { 0 },
            Explanation = "Bei 8 Punkten ist Schluss. Davor gibt es Stufen: ab 4 Punkten eine Ermahnung, ab 6 eine Verwarnung."
        },
        new()
        {
            Id = "rr-03", Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen, Points = 3,
            Prompt = "Wofür ist der Halter eines Fahrzeugs verantwortlich? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Dass das Fahrzeug verkehrssicher ist",
                "Dass es zugelassen und versichert ist",
                "Dass nur Personen mit gültiger Fahrerlaubnis damit fahren",
                "Für jeden Parkverstoß des Fahrers"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Der Halter haftet für Zustand, Zulassung und dafür, wem er das Auto gibt. Für das Fahrverhalten haftet dagegen der Fahrer selbst."
        },
        new()
        {
            Id = "rr-04", Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen, Points = 2,
            Prompt = "Was deckt die Kfz-Haftpflichtversicherung ab?",
            Options = new[]
            {
                "Schäden, die du anderen zufügst",
                "Schäden an deinem eigenen Fahrzeug",
                "Beides gleichermaßen",
                "Nur Personenschäden, keine Sachschäden"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Haftpflicht ist Pflicht und zahlt den Schaden der anderen. Für das eigene Auto braucht man Teil- oder Vollkasko - die sind freiwillig."
        },
        new()
        {
            Id = "rr-05", Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen, Points = 3,
            Prompt = "Was passiert, wenn du in der Probezeit einen schweren Verstoß begehst (z.B. deutlich zu schnell)?",
            Options = new[]
            {
                "Die Probezeit verlängert sich auf vier Jahre und du musst zu einem Aufbauseminar",
                "Es passiert nichts Besonderes, es gilt allein der normale Bußgeldkatalog",
                "Die Fahrerlaubnis wird sofort und ohne weiteres Verfahren entzogen",
                "Die Probezeit endet vorzeitig, dafür steigt das Bußgeld deutlich"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Ein A-Verstoß in der Probezeit kostet mehr als das Bußgeld: vier statt zwei Jahre Probezeit plus verpflichtendes Aufbauseminar auf eigene Kosten."
        },

        // ---------------------------------------------- Straßenverkehrssystem
        new()
        {
            Id = "sv-01", Topic = DrivingTheoryTopic.Strassenverkehrssystem, Points = 4,
            Prompt = "Was bedeutet die Grundregel des § 1 StVO?",
            Options = new[]
            {
                "Ständige Vorsicht und gegenseitige Rücksicht",
                "Wer schneller fährt, hat Vorrang",
                "Wer im Recht ist, darf auf seinem Recht bestehen",
                "Rechts vor links gilt immer"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "§ 1 steht bewusst vor allen anderen Regeln: niemand darf geschädigt oder mehr als unvermeidbar behindert werden. Recht haben und Recht bekommen nützt nach einem Unfall wenig."
        },
        new()
        {
            Id = "sv-02", Topic = DrivingTheoryTopic.Strassenverkehrssystem, Points = 3,
            Prompt = "Wer darf die Autobahn benutzen?",
            Options = new[]
            {
                "Nur Kraftfahrzeuge, die schneller als 60 km/h fahren können",
                "Alle Kraftfahrzeuge",
                "Alle Fahrzeuge außer Fahrrädern",
                "Kraftfahrzeuge ab 50 km/h Höchstgeschwindigkeit"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Entscheidend ist die bauartbedingte Höchstgeschwindigkeit über 60 km/h. Mofas, Fahrräder und Fußgänger sind ausgeschlossen - dort ist der Geschwindigkeitsunterschied lebensgefährlich."
        },
        new()
        {
            Id = "sv-03", Topic = DrivingTheoryTopic.Strassenverkehrssystem, Points = 3,
            Prompt = "In einem verkehrsberuhigten Bereich (Spielstraße) - was gilt dort? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Schrittgeschwindigkeit",
                "Fußgänger dürfen die ganze Straße benutzen",
                "Parken nur auf gekennzeichneten Flächen",
                "Kinder haben Vorfahrt vor allen anderen"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Schrittgeschwindigkeit sind etwa 7 km/h - langsamer als die meisten denken. Der Begriff \"Vorfahrt\" passt hier nicht: Fahrzeuge dürfen Fußgänger weder gefährden noch behindern, das ist mehr als Vorfahrt."
        },

        // ---------------------------------------------- Vorfahrt und Regelung
        new()
        {
            Id = "vf-01", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 5,
            Prompt = "An einer Kreuzung ohne Verkehrszeichen und ohne Ampel - wer darf zuerst?",
            Options = new[]
            {
                "Wer von rechts kommt",
                "Wer zuerst da war",
                "Wer geradeaus fährt",
                "Das größere Fahrzeug"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Rechts vor links ist die Grundregel, wenn nichts anderes geregelt ist. \"Wer zuerst da war\" gibt es im Straßenverkehr nicht - das ist Alltagslogik, keine Rechtsregel."
        },
        new()
        {
            Id = "vf-02", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 5,
            Prompt = "Ein Polizist regelt den Verkehr, gleichzeitig zeigt die Ampel Grün. Was gilt?",
            Options = new[]
            {
                "Die Zeichen des Polizisten",
                "Die Ampel",
                "Was zuerst kam",
                "Man darf sich aussuchen, welchem man folgt"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Rangfolge lautet: Polizei vor Lichtzeichen vor Verkehrszeichen vor allgemeinen Regeln. Ein Mensch kann auf eine Lage reagieren, eine Ampel nicht."
        },
        new()
        {
            Id = "vf-03", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 5,
            SignNumber = "206",
            Prompt = "Du kommst an ein Stoppschild und siehst, dass weit und breit niemand kommt. Was musst du tun?",
            Options = new[]
            {
                "Vollständig anhalten, auch wenn niemand kommt",
                "Langsam heranrollen und weiterfahren, wenn frei ist",
                "Nur bremsen, anhalten ist nicht nötig",
                "Blinken und weiterfahren"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Beim Stoppschild ist das Anhalten selbst die Anordnung - unabhängig davon, ob jemand kommt. Genau das unterscheidet es von \"Vorfahrt gewähren\" (VZ 205)."
        },
        new()
        {
            Id = "vf-04", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 5,
            Prompt = "Du willst aus einer Grundstücksausfahrt auf die Straße einfahren. Wer hat Vorrang?",
            Options = new[]
            {
                "Alle anderen - du musst dich einordnen, ohne jemanden zu gefährden",
                "Du hast Vorrang, sofern du von rechts auf die Straße einfährst",
                "Es gilt rechts vor links wie an jeder gewöhnlichen Kreuzung",
                "Vorrang hat, wer zuerst an der Einmündung angekommen ist"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Wer aus einem Grundstück, einem verkehrsberuhigten Bereich oder vom Fahrbahnrand anfährt, ist immer der Wartepflichtige. Rechts vor links gilt nur zwischen gleichrangigen Straßen."
        },
        new()
        {
            Id = "vf-05", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 4,
            Prompt = "Die Ampel zeigt Gelb, du bist noch ein Stück entfernt. Was tust du?",
            Options = new[]
            {
                "Anhalten, wenn das ohne scharfes Bremsen möglich ist",
                "Beschleunigen, um noch durchzukommen",
                "Auf jeden Fall anhalten, egal wie scharf du bremsen musst",
                "Weiterfahren, Gelb ist noch erlaubt"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Gelb heißt: vor der Kreuzung warten. Wer aber so nah dran ist, dass nur eine Vollbremsung hilft, fährt weiter - sonst fährt der Hintermann auf."
        },
        new()
        {
            Id = "vf-06", Topic = DrivingTheoryTopic.VorfahrtUndRegelung, Points = 5,
            SignNumber = "215",
            Prompt = "Du fährst in einen Kreisverkehr ein, an dem VZ 215 zusammen mit \"Vorfahrt gewähren\" steht. Was gilt?",
            Options = new[]
            {
                "Der Verkehr im Kreis hat Vorfahrt",
                "Du hast Vorfahrt, weil du von rechts kommst",
                "Rechts vor links gilt auch im Kreisverkehr",
                "Wer schneller ist, fährt zuerst"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Kombination aus beiden Zeichen ist der Normalfall: im Kreis hat Vorfahrt, wer schon drin ist. Beim Hineinfahren wird nicht geblinkt, beim Hinausfahren schon."
        },

        // ---------------------------------------------- Geschwindigkeit und Abstand
        new()
        {
            Id = "ga-01", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 5,
            Prompt = "Wie schnell darfst du innerorts fahren, wenn kein Schild etwas anderes sagt?",
            Options = new[] { "50 km/h", "30 km/h", "60 km/h", "So schnell es die Lage erlaubt" },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Ortstafel selbst ist das Tempolimit - ab ihr gelten 50, ohne dass ein Zahlenschild kommt. Wer darauf wartet, wartet vergeblich."
        },
        new()
        {
            Id = "ga-02", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 5,
            Prompt = "Du fährst außerorts 100 km/h. Wie groß sollte dein Abstand zum Vordermann mindestens sein?",
            Options = new[]
            {
                "Etwa 50 Meter - der halbe Tachowert in Metern",
                "Etwa 25 Meter - ein Viertel des Tachowerts in Metern",
                "Etwa 100 Meter - der volle Tachowert in Metern",
                "Zwei Fahrzeuglängen, unabhängig vom Tempo"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Halber Tacho in Metern entspricht etwa dem Weg in 1,8 Sekunden. Das ist die Faustregel, kein Idealwert - bei Nässe oder Nebel gehört mehr dazu."
        },
        new()
        {
            Id = "ga-03", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 4,
            Prompt = "Du fährst 60 km/h. Wie lang ist dein Anhalteweg ungefähr (Reaktion plus Bremsung)?",
            Options = new[] { "Etwa 54 Meter", "Etwa 36 Meter", "Etwa 18 Meter", "Etwa 90 Meter" },
            CorrectIndices = new[] { 0 },
            Explanation = "Reaktionsweg = (60÷10)×3 = 18 m, Bremsweg = (60÷10)² = 36 m, zusammen 54 m. Das ist mehr als eine halbe Fußballfeldbreite - und der Grund, warum Abstand kein Luxus ist."
        },
        new()
        {
            Id = "ga-04", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 5,
            Prompt = "Was passiert mit dem Bremsweg, wenn du die Geschwindigkeit verdoppelst?",
            Options = new[]
            {
                "Er wird viermal so lang",
                "Er wird doppelt so lang",
                "Er wird dreimal so lang",
                "Er bleibt gleich, nur die Zeit ändert sich"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Der Bremsweg wächst im Quadrat: aus 30 m bei 50 km/h werden 120 m bei 100 km/h. Deshalb ist ein bisschen schneller nie nur ein bisschen gefährlicher."
        },
        new()
        {
            Id = "ga-05", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 4,
            Prompt = "Bei Nebel siehst du nur etwa 50 Meter weit. Wie schnell darfst du höchstens fahren?",
            Options = new[] { "50 km/h", "80 km/h", "Wie ausgeschildert", "30 km/h" },
            CorrectIndices = new[] { 0 },
            Explanation = "Bei Sichtweite unter 50 m gilt höchstens 50 km/h. Dahinter steckt die Grundregel: man muss innerhalb der übersehbaren Strecke anhalten können."
        },
        new()
        {
            Id = "ga-06", Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand, Points = 4,
            Prompt = "Was gilt für die Richtgeschwindigkeit von 130 km/h auf der Autobahn?",
            Options = new[]
            {
                "Sie ist eine Empfehlung, aber bei einem Unfall kann eine Mitschuld entstehen",
                "Sie ist ein verbindliches Tempolimit und wird wie ein Schild geahndet",
                "Sie gilt ausschließlich bei Nässe und schlechter Sicht",
                "Sie ist eine reine Empfehlung ganz ohne rechtliche Bedeutung"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Schneller zu fahren ist erlaubt, aber nicht folgenlos: wer deutlich darüber liegt, kann bei einem Unfall anteilig haften, selbst wenn er den Unfall nicht verursacht hat."
        },

        // ---------------------------------------------- Fahrmanöver und Überholen
        new()
        {
            Id = "fm-01", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 5,
            Prompt = "Du überholst eine Radfahrerin innerorts. Welchen seitlichen Abstand musst du mindestens halten?",
            Options = new[] { "1,5 Meter", "1 Meter", "2 Meter", "Es gibt keinen festen Mindestabstand" },
            CorrectIndices = new[] { 0 },
            Explanation = "Innerorts 1,5 m, außerorts 2 m - seit 2020 ausdrücklich in der StVO. Wer den Abstand nicht hat, darf nicht überholen, auch wenn es eng zugeht."
        },
        new()
        {
            Id = "fm-02", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 5,
            SignNumber = "350-10",
            Prompt = "Vor einem Fußgängerüberweg (Zebrastreifen) - was ist verboten?",
            Options = new[]
            {
                "Überholen",
                "Bremsen",
                "Blinken",
                "Langsamer als 30 km/h zu fahren"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Am Zebrastreifen darf nicht überholt werden - auch nicht, wenn gerade niemand dasteht. Der Überholende verdeckt sonst genau die Sicht, auf die es ankommt."
        },
        new()
        {
            Id = "fm-03", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 4,
            Prompt = "Du willst nach links abbiegen. Was gehört dazu? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Rechtzeitig blinken",
                "In den Rückspiegel und über die Schulter schauen",
                "Sich zur Fahrbahnmitte einordnen",
                "Vor dem Abbiegen kräftig beschleunigen"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Blinken, Schulterblick, Einordnen - in dieser Reihenfolge. Der Schulterblick fängt den toten Winkel ab, in dem gerade das Fahrrad fährt, das der Spiegel nicht zeigt."
        },
        new()
        {
            Id = "fm-04", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 4,
            Prompt = "Zwei Fahrstreifen laufen zusammen. Wie wird eingefädelt?",
            Options = new[]
            {
                "Im Reißverschlussverfahren, unmittelbar an der Engstelle abwechselnd",
                "Wer zuerst kommt, fährt zuerst",
                "Der rechte Fahrstreifen hat immer Vorrang",
                "Möglichst früh auf den durchgehenden Fahrstreifen wechseln"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Der Reißverschluss funktioniert nur, wenn man den endenden Fahrstreifen bis zum Schluss nutzt. Frühes Einfädeln verschenkt Platz und staut mehr - höflich gemeint, aber falsch."
        },
        new()
        {
            Id = "fm-05", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 5,
            Prompt = "Wann darfst du nicht überholen? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Wenn du nicht sicher bist, dass die Strecke frei ist",
                "Bei unklarer Verkehrslage",
                "Wenn der Vordermann links blinkt",
                "Wenn der Vordermann langsamer fährt als erlaubt"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Überholen darf nur, wer eine wesentlich höhere Geschwindigkeit hat UND die Strecke überblickt. Dass jemand langsam fährt, ist für sich genommen kein Grund - und wer links blinkt, will selbst gerade abbiegen oder überholen."
        },
        new()
        {
            Id = "fm-06", Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen, Points = 4,
            Prompt = "Du musst rückwärts aus einer Parklücke. Was gilt?",
            Options = new[]
            {
                "Du musst jede Gefährdung ausschließen - notfalls einweisen lassen",
                "Der fließende Verkehr muss dich einfädeln lassen und dafür abbremsen",
                "Rückwärtsfahren aus einer Parklücke ist grundsätzlich verboten",
                "Es genügt, den Warnblinker einzuschalten und langsam zu fahren"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Beim Rückwärtsfahren trägt man die alleinige Verantwortung. Der Warnblinker macht sichtbar, ersetzt aber das Schauen nicht - und niemand muss einen einfädeln lassen."
        }
    };
}
