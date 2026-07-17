# LernTor

Eine kindersichere Windows-Kiosk-Lern-App: Nach dem Login startet LernTor im Vollbild und
sperrt den PC, bis das Kind Lesen, den Tipptrainer, News, Übungsaufgaben in den freigeschalteten
Fächern und das Abschlussquiz durchlaufen hat. Erst dann wird der PC freigegeben. Eltern legen
im Eltern-Bereich fest, welche der 15 verfügbaren Fächer für den Tag/das Profil überhaupt aktiv
sind (siehe "Bereiche deaktivieren").

Zielgruppe: deutsch-türkische Kinder (ca. 10–15 Jahre) in Berlin, Lehrplan-Themen orientiert
am Berliner Rahmenlehrplan, Klasse 6 und 9 (siehe [docs/CURRICULUM.md](docs/CURRICULUM.md)).

## Profile

Mehrere Kinder am selben PC wählen beim Start ihr eigenes Profil aus einem Kachel-Dashboard: jede
Kachel zeigt den selbst gewählten Avatar (Emoji, beim Anlegen aus einer Auswahl wählbar - bewusst
Emoji statt Bilddateien: keine Assets zu pflegen, rendert nativ), einen Fortschrittsring für die
heutige Lernsession (grün + Häkchen, wenn heute schon freigeschaltet wurde) und Name/Alter/Klasse
auf sanft rotierenden Pastell-Hintergründen. Darüber steht eine tageszeitabhängige Begrüßung
(morgens/nachmittags/abends, DE/TR). Über "Neues Profil ➕" wird ein weiteres Profil mit eigener
Klassenstufe (6 oder 9) und Avatar angelegt. Jedes Profil hat seinen eigenen Fortschritt, sein
eigenes Abschlussquiz-Ergebnis und sein eigenes Aktivitätsprotokoll (im Eltern-Bereich per Dropdown
auswählbar; dort lassen sich Profile samt aller zugehörigen Daten auch löschen - mit Rückfrage, das
letzte verbleibende Profil ist geschützt). Beim ersten Start werden automatisch zwei
Beispielprofile angelegt. Nach
Auswahl/Erstellung wird der Profilname dauerhaft oben rechts im Kiosk-Fenster angezeigt (👤 Name),
damit jederzeit erkennbar ist, welches Kind gerade angemeldet ist.

## Ablauf für das Kind

1. **Profil wählen**
2. **Lesen** (Pflicht, nicht überspringbar) – täglich **zwei** wechselnde Stücke aus einem festen Pool
   von 63 Texten (siehe `LernTor.Core/Services/ReadingContentProvider.cs`; die Tagespaarung kombiniert
   je einen literarischen/Allgemeinwissen-Text mit einer Pop-Kultur-Geschichte - zwei ganze Texte statt
   künstlich verlängerter Einzeltexte, denn den echten, gemeinfreien Gedichten eine zweite Strophe
   anzudichten würde die Werke verfälschen). Beide werden **gleichzeitig auf Deutsch, Türkisch und
   Englisch nebeneinander** angezeigt, damit dasselbe Stück in allen drei Sprachen gelesen werden kann. Über Sprach-Tabs lässt sich alternativ eine einzelne Sprache größer
   und mit mehr Zeilenabstand anzeigen; ein **Vorlesen-Button** liest den Text der gewählten Sprache
   vor - komplett offline, zweistufig: Haben die Eltern im Eltern-Bereich einmalig die **natürlichen
   Piper-Stimmen** heruntergeladen (~230 MB: piper.exe + je eine neuronale "medium"-Stimme für
   Deutsch/Türkisch/Englisch, auch die türkische Aussprache stimmt), wird satzweise per `piper.exe`
   zu WAV synthetisiert und abgespielt - satzweise, damit die Wiedergabe sofort startet, während der
   nächste Satz schon im Hintergrund erzeugt wird (`PiperTtsEngine`/`TextToSpeechService`). Ohne
   Piper-Stimmen liest als Rückfall die Windows-eigene Sprachausgabe (`System.Speech` - eine
   türkische SAPI-Stimme ist auf Windows oft nicht vorinstalliert; ohne passende Stimme liest die
   Standardstimme). Ein LLM wäre hierfür das falsche Werkzeug: Sprachmodelle erzeugen Text, keine
   Audiodaten - Piper ist das passende (und viel kleinere) dedizierte TTS-Modell. Soll **laut
   vorgelesen** werden. Ein 5-Minuten-Timer läuft; "Weiter" ist erst danach nutzbar - es gibt bewusst
   keine Überspringen-Funktion. Ob tatsächlich (laut) gelesen wurde, kann technisch nicht geprüft
   werden – das ist eine bekannte, akzeptierte Grenze dieser Funktion. Der Pool gliedert sich in
   zwei Kategorien:
   - **33 literarische/Allgemeinwissen-Stücke**: gemeinfreie Gedichte/Texte (Goethe, Heine, Yunus
     Emre, Schiller, Rilke, Fontane, Robert Frost, William Blake, Lewis Carroll, u.a.) sowie kurze
     "kurz erklärt"-Sachtexte (Berlin, Sonnensystem, Menschenrechte, EU, ...). Fremdsprachige
     Originale werden mit eigenen, freien Übertragungen (keine fremden Übersetzungen) begleitet.
   - **30 Pop-Kultur-Kurzgeschichten**: eigens verfasste, kindgerechte Abenteuergeschichten rund um
     Minecraft, Minecraft Legends, Super Mario, AntonCraft, Geometry Dash, Rock'n'Roll, Pac-Man,
     Kirby, Batman, One Punch Man, Dragon Ball, One Piece und Backrooms. Diese Texte referenzieren
     die Marken/Figuren nur namentlich als Thema; Handlung, Dialoge und Wortlaut sind komplett neu
     und zitieren kein geschütztes Spiel-/Comic-/Anime-Material.
   (Eine Anbindung an lyrikline.org wurde bewusst nicht umgesetzt: aus dieser Entwicklungsumgebung war
   der Zugriff auf die Seite nicht möglich, und ihre zeitgenössischen Gedichte/Übersetzungen sind
   urheberrechtlich geschützt und dürften ohne Erlaubnis nicht in die App übernommen werden.)
3. **Tipptrainer** – der 10-Finger-Trainer mit mehreren aufeinander aufbauenden Lektionen, virtueller
  Tastatur, Live-Feedback zu Eingaben, Genauigkeit und Geschwindigkeit. **Nur deutsche Sprache und
  QWERTZ-Tastaturlayout** (keine türkischen/englischen Wörter mehr). Nach jeder abgeschlossenen
  Lektion erscheint der gewohnte Weiter-/Wiederholen-Dialog; ist wirklich alles erledigt, zeigt das
  Dashboard unten ein Glückwunsch-Panel mit einem Button weiter zu den News. **Profil-spezifische
  Abschluss-Lektion**: Emirhan (Klasse 6) tippt einen persönlichen Steckbrief-Text (Name, Geburtsdatum,
  Adresse, Telefon, Eltern, Schule 6c, Lieblingsessen, Hobbys), Batuhan (Klasse 9) seinen eigenen
  Steckbrief (Geburtsdatum, Adresse, Telefon, Eltern, Gymnasium 9a, Lieblingsessen, Hobbys).
  Der Tipptrainer ist eine eigene feste Stage und gehört nicht zum Fachcurriculum.
4. **News für Kinder** (Pflicht) – kuratierte RSS-Artikel, verständlich, neutral und ohne Angstmache
   aufbereitet (sprachliches Vorbild: logo!/Checker-Sendungen). **Rubriken**: 🐻 Berlin (wichtigste
  regionale Rubrik, garantierte Plätze), 🇩🇪 Deutschland, 🌍 Welt, 🇹🇷 Türkei (täglich garantierte
  Plätze; seriöse Quellen: Anadolu Ajansı, TRT Haber, DW Türkçe – bewusst keine Boulevardquellen),
  🤖 KI & Technik, 🎮 Spiele, ⛅ Wetter.
  Themen-Rubriken werden zusätzlich per Schlüsselwort-Klassifikation quer über alle Quellen erkannt
  (`NewsCategoryClassifier`) - eine Minecraft-Meldung von tagesschau.de landet trotzdem in 🎮 Spiele.
  Im News-Teil wird von jedem Feed genau der neueste Artikel übernommen; dadurch bleibt der Block
  bewusst klein und landet typischerweise bei etwa 22 Artikeln statt bei 71.
   **Jeder Artikel erhält**: Rubrik-Chip mit Emoji, geschätzte Lesedauer, Schwierigkeitsgrad
   (🟢/🟡/🔴, Satz-/Wortlängen-Heuristik), sofort erklärte schwierige Wörter (kuratiertes
  `KidTermGlossary`, ~55 Begriffe von Inflation bis Deepfake) und EINE Verständnisfrage, die
   echtes Lesen verlangt: ein Lückentext aus der Zusammenfassung mit ausgeblendetem Schlüsselwort
   (frühere Fragetypen wurden auf Nutzerwunsch entfernt: die „Nenne ein wichtiges Wort aus der
   Überschrift"-Frage war ohne Lesen lösbar, und eine zusätzliche Rubrik-Frage erwies sich als
   unnötig und bei Fehlklassifikation sogar unfair; ebenso die Einordnungs-Boxen „Warum ist das
   wichtig?"/„Was bedeutet das für dich?" - der Fokus liegt auf der Nachricht selbst). Gibt die
   Zusammenfassung keinen Lückentext her, bleibt der Artikel ohne Frage. Diese Verständnisfragen
   zählen NICHT ins Abschlussquiz - sie werden ausschließlich hier im News-Bereich gestellt.
   **Mindest-Lesezeit**: wie
   bei den Fach-Übungen wird „Weiter" erst frei, wenn die Frage(n) beantwortet sind UND ein
   20-Sekunden-Countdown abgelaufen ist - Nachrichten sollen gelesen, nicht weggeklickt werden.
   Verstörende Themen (Krieg, Gewalt – auch türkischsprachige Schlüsselwörter) werden im Ranking
   stark heruntergestuft. **Finanzwissen**: weil es zu Finanzthemen kaum kindtaugliche Tagesmeldungen
   gibt, wird täglich ein rotierendes, kuratiertes Erklärstück angehängt (Was ist Geld? Inflation,
   Sparen, Börse, Taschengeld, Berufswelt – mit handgeschriebenen Verständnisfragen, siehe
   `FinanceKnowledgeArticles`). **Wetter-Widget**: oben rechts zeigt eine kleine Karte das heutige
   Berlin-Wetter (Open-Meteo-API, kostenlos/ohne Schlüssel/ohne Konto) mit Emoji, Temperaturen und
   einem kindgerechten Tages-Tipp („Pack einen Schirm ein!"); schlägt der Abruf fehl, bleibt das
   Widget einfach ausgeblendet. **Bezirks-Chip statt Karte**: Artikel mit Berliner Ortsbezug
   bekommen einen 📍-Chip mit dem Bezirk (`BerlinDistrictDetector`: alle 12 Bezirke plus ~45
   bekannte Ortsteile/Kieze wie Wedding → Mitte; mehrdeutige Alltagswörter wie „Mitte" allein
   werden bewusst nur in eindeutigen Formen wie „Berlin-Mitte" erkannt) - eine Offline-Karte ohne
   Cloud-Kartendienst wäre unverhältnismäßig, der Kiez-Bezug ist die eigentliche Information.
   Eine Marker-Leiste im Kopf zeigt je Artikel einen Kreis (grün mit ✓ =
   Fragen beantwortet, auch aus einer früheren Session desselben Tages nach Absturz/Neustart;
   lila = aktueller Artikel; grau = offen). **Die Marker sind klickbar** (direkt zum Artikel
   springen). „Weiter" führt zum nächsten noch offenen Artikel; der Abschluss-Button
   erscheint erst, wenn wirklich ALLE Artikel beantwortet sind. **Automatischer Altersfilter**:
   bis einschließlich 9 Jahren (Profil-Alter) werden Artikel mit verstörenden Schlüsselwörtern
   komplett ausgefiltert statt nur herabgestuft. **Tages-Archiv & Offline-Rückfall**: die
   aufbereiteten Tagesartikel werden automatisch ~7 Tage archiviert (`ArchivedArticleRepository`,
   inkl. Verständnisfragen); sind morgens alle Feeds unerreichbar, liest das Kind die Artikel des
   letzten erfolgreichen Tages statt vor einem fast leeren News-Teil zu stehen (im
   Fehlerprotokoll vermerkt). (Eine frühere Lesezeichen-/Suchfunktion im News-Teil wurde auf
   Nutzerwunsch wieder entfernt - im geführten Pflicht-Ablauf brachte sie keinen Mehrwert und
   verschob das Layout.)
5. **Fachbereiche** (alle nicht von den Eltern deaktivierten, Klasse 6/9): Mathematik, Deutsch, Türkisch,
   Englisch, Biologie, Chemie, Physik, Gesellschaftswissenschaften (Gewi), Politik, Geografie, Ethik,
   Medienbildung (ITG) – siehe [docs/CURRICULUM.md](docs/CURRICULUM.md) für die genauen Themen je Fach.
   Bei offenen Mathematik-Aufgaben steht ein Taschenrechner zur Verfügung; Aufgaben mit hinterlegtem
   Tipp zeigen einen abrufbaren Formel-/Vorgehens-Hinweis (verrät nicht die Lösung).
   **Fehler-Kartei** (vereinfachtes Spaced-Repetition): im Übungsteil falsch beantwortete Aufgaben
   werden als Schnappschuss gespeichert (generierte Aufgaben wären am Folgetag nicht reproduzierbar)
   und erscheinen an FOLGETAGEN im selben Fach zuerst wieder (mit 🔁 markiert, am häufigsten falsch
   beantwortete zuerst, nie am selben Tag erneut) - bis sie zweimal in Folge richtig beantwortet
   wurden, dann gelten sie als gelernt und verschwinden. News-Fragen sind ausgenommen (deren
   Tagesartikel gibt es später nicht mehr). Siehe `ReviewQuestionRepository`/`ReviewQuestionEntity`;
   die neue Tabelle legt der automatische Schema-Abgleich selbst an.
   **Dauerhafter Ausschluss richtig beantworteter Aufgaben**: einmal richtig beantwortet (egal ob im
   Übungsteil oder im Abschlussquiz), taucht der exakte Fragetext für dieses Profil nie wieder auf -
   unabhängig vom 21-Tage-Fenster der reinen Frische-Bevorzugung. Deshalb halten alle Fächer mit
   festem Themen-Pool inzwischen 20 kuratierte Beispiele je Thema vor (siehe
   [docs/CURRICULUM.md](docs/CURRICULUM.md)) statt nur 5 - sonst wäre der Pool schnell erschöpft.
   Ist ein Themen-Pool tatsächlich komplett gemeistert, greift die Aufgabenauswahl als Rückfall auf
   Wiederholungen zurück, statt weniger Aufgaben als angefordert zu liefern. Siehe
   `MasteredPromptRepository`/`MasteredPromptEntity`.
   **Mindest-Lernzeit pro Aufgabe**: „Weiter" wird erst frei, wenn die Frage beantwortet ist UND ein
   20-Sekunden-Countdown abgelaufen ist (sichtbar unter dem Button) - gegen das beobachtete wilde
   Durchklicken, nur um schnell zum Quiz zu kommen. Das Abschlussquiz hat bewusst keinen Countdown:
   dort bestraft sich Raten von selbst (unter der eingestellten Schwelle beim ersten Versuch gibt es
   eine Wiederholung, siehe unten).
6. **Abschlussquiz** – gemischte Fragen aus allen aktiven Fächern (Anzahl je Fach passt sich automatisch
   an, wie viele Fächer aktiv sind), Nachrichten-Verständnisfragen zählen NICHT mit (die werden nur im
   News-Bereich selbst gestellt). Erster Versuch am Tag: exakt **20 Fragen**, Bestehens-Schwelle ist im
   Eltern-Bereich pro Profil einstellbar (Presets 50/75/85%, Standard 50%) → ab dieser Trefferquote wird
   der PC freigeschaltet. Bei Nichtbestehen ein kürzeres Wiederholungsquiz mit exakt **15 Fragen**:
   schwache Fächer bekommen konzentriert Fragen (mind. 2 je Fach), der Rest füllt ein allgemeines
   Mini-Quiz über alle aktiven Fächer auf - für diesen zweiten Anlauf gilt eine eigene, niedrigere
   Bestehens-Schwelle (Presets 25/50%, Standard 25%, ebenfalls pro Profil einstellbar); wird auch diese
   nicht erreicht, gibt es einen weiteren Wiederholungsanlauf. Antwortoptionen bei Multiple-Choice-Fragen
   werden bei jeder Anzeige neu gemischt, damit die richtige Antwort nicht immer an derselben Stelle steht.

Fortschritt wird laufend in einer lokalen SQLite-Datenbank gespeichert (`%LOCALAPPDATA%\LernTor\lerntor.db`),
ein Absturz oder Neustart verliert also keinen Fortschritt.

## Architektur & Technologie-Entscheidung

**.NET 8 + WPF**, kein WinUI 3, kein Electron. Begründung: Die sicherheitskritischste Anforderung
(Kiosk-Sperre: Task-Manager deaktivieren, Windows-Taste/Alt+Tab abfangen) ist mit klassischen
Win32-APIs (P/Invoke) am zuverlässigsten umsetzbar, und WPF hat dafür die geringste Reibung bei
gleichzeitig moderner, gut stylebarer UI. Details siehe Projekt-Chatverlauf/Architektur-Entscheidung.

```
src/
  LernTor.Core         Domänenmodelle, Fortschritts-/Scoring-Logik (plattformunabhängig)
  LernTor.ContentGen   Regelbasierte Aufgabengeneratoren (Mathe/Deutsch/Türkisch/NaWi) + Quiz-Composer
  LernTor.News         RSS-Ingestion, Vereinfachung, Verständnisfragen-Generierung
  LernTor.Data         SQLite-Persistenz (EF Core): Fortschritt, Aktivitätsprotokoll, Einstellungen,
                       eigene (elternerstellte) Aufgaben
  LernTor.Security     Kiosk-Lock (Keyboard-Hook), Task-Manager-Policy, Autostart, Admin-Auth
  LernTor.App          WPF-UI (Kiosk-Shell, alle Bildschirme, DE/TR-Lokalisierung)
  LernTor.Installer     Inno-Setup-Skript + PowerShell-Autostart-Helfer
tests/
  LernTor.Tests        xUnit-Tests für Core/ContentGen/News
```

## Kiosk-Sperre: was sie tut und was sie NICHT tut

Umgesetzt ist ein **Soft-Lock**: Vollbild-Topmost-Fenster ohne Rahmen, globaler Keyboard-Hook
(blockiert Windows-Taste, Alt+Tab, Strg+Esc, Alt+Esc, Alt+F4) und eine Registry-Richtlinie, die den
Task-Manager deaktiviert, solange LernTor läuft. Start erfolgt über einen Windows-Autostart-Task
(Anmeldung-Trigger), **kein** Shell-Ersatz (`explorer.exe` bleibt unangetastet).

**Bewusste Grenze:** Strg+Alt+Entf (Secure Attention Sequence) kann von keiner Windows-Anwendung im
User-Modus abgefangen werden – das ist ein OS-Schutzmechanismus. Die Task-Manager-Registry-Sperre
greift aber auch dann, wenn ein Kind versucht, darüber den Task-Manager zu öffnen (es erscheint eine
"vom Administrator deaktiviert"-Meldung). Ein sehr technisch versiertes Kind mit Admin-Wissen könnte
dies dennoch umgehen – für den Einsatzzweck (Motivation/Struktur für 10-15-Jährige) ist das eine
angemessene, dokumentierte Grenze. Ein härterer Shell-Ersatz-Modus wäre als spätere Erweiterung möglich,
birgt aber das Risiko, den Desktop bei einem App-Absturz komplett unbenutzbar zu machen, und wurde
deshalb bewusst nicht umgesetzt.

## Motivation & Gamification

- **Belohnungs-Sterne**: +2 fürs Lesen, +2 für News, +1 je abgeschlossenem Fach, +5 fürs bestandene
  Abschlussquiz - nur echte Abschlüsse zählen, von Eltern deaktivierte (übersprungene) Fächer geben
  keine Sterne. Heute verdiente Sterne und der Gesamtstand erscheinen auf dem Abschluss-Bildschirm;
  der über alle Tage gesammelte Gesamtstand (⭐) steht auf der Profil-Kachel.
- **Etappen-Leiste**: oben im Kiosk-Fenster zeigen fünf Chips (Lesen → Tippen → News → Fächer x/y → Quiz)
  jederzeit, wo das Kind in der heutigen Session steht - erledigt = grün mit Häkchen, aktuell = lila.
- **Konfettiregen**: beim bestandenen Abschlussquiz fällt einmalig Konfetti in den App-Farben über
  den Ergebnis-Bildschirm (einmalige Animation, keine Dauerschleife - keine bleibende CPU-Last im
  Kiosk-Betrieb).
- **🎁 Belohnungsliste**: Eltern legen im Eltern-Bereich Belohnungen mit Sterne-Kosten an
  (z.B. „30 Minuten extra Spielzeit" für 20 ⭐). Nach dem bestandenen Abschlussquiz sieht das Kind
  die Liste mit Fortschritt („12 / 20 ⭐") und kann erreichbare Belohnungen einlösen (mit
  Rückfrage; Abbuchung + Protokolleintrag passieren in einem Schritt). Eingelöste Belohnungen
  erscheinen je Profil im Eltern-Bereich - eingelöst wird in der echten Welt von den Eltern.
  Die Historie bleibt als Schnappschuss erhalten, auch wenn die Belohnung später gelöscht wird.
- Bewusst KEINE Tages-Streaks: ein verpasster Tag soll kein schlechtes Gewissen erzeugen - Sterne
  können nur wachsen, nie verfallen (einlösen ja, verfallen nein).

## Eltern-Features

- Zahnrad-Symbol (unten rechts, dezent) öffnet den passwortgeschützten Eltern-Bereich.
- Erststart: Admin-Passwort selbst festlegen (PBKDF2-Hash, kein Klartext gespeichert).
- Fachbereiche einzeln deaktivieren, Klassenstufe (6/9) einstellen.
- **Schwierigkeitsstufen pro Profil** (Presets statt Freitext, kein neuer Build nötig): Tipptrainer-
  Mindestgenauigkeit (25/50/75/85%, Standard 25%), Abschlussquiz-Bestehensschwelle für den 1. Versuch
  (50/75/85%, Standard 50%) und für den 2. Versuch/die Wiederholung (25/50%, Standard 25%).
- Aktivitätsprotokoll: alle beantworteten Aufgaben + Quiz-Ergebnisse einsehbar.
- **Bericht "Stärken & Schwächen"** (pro Profil, umschaltbar 7/30 Tage): Richtig-Quote je Fach als
  Ampel-Balken (grün ≥75 %, gelb, rot <50 %), schwächste Fächer zuerst; dazu Lerntage im Zeitraum
  und der Abschlussquiz-Verlauf. Rechnet komplett aus dem vorhandenen Aktivitätsprotokoll - keine
  neuen Tabellen.
- "Sofort freischalten": Notfall-Override, überspringt den restlichen Ablauf.
- "Alle Daten zurücksetzen…" (Gefahrenzone): löscht alle Profile/Fortschritte/Einstellungen aus der
  App heraus, mit Ja/Nein-Bestätigung. Vorher ging das nur manuell über das Löschen von `lerntor.db`.
- **Fehlerprotokoll** (`LernTor.Core.Logging.AppLog`): eine Log-Datei pro Tag unter
  `%LOCALAPPDATA%\LernTor\logs\lerntor-JJJJ-MM-TT.log` (älter als 14 Tage wird automatisch
  gelöscht; komplett lokal, keine Telemetrie). Protokolliert wird genau das, was die App dem Kind
  gegenüber bewusst verschluckt: übersprungene News-Feeds **mit Quelle und Grund**, fehlgeschlagene
  Modell-/Piper-Downloads je Spiegel-URL, Wetter-Ausfälle, TTS-Rückfälle (fehlende SAPI-Stimme,
  Piper-Fehler) sowie unbehandelte Abstürze mit vollem Stacktrace (zusätzlich zur MessageBox, die
  nach dem Wegklicken verloren wäre). Der Eltern-Bereich zeigt die letzten Zeilen des heutigen
  Protokolls direkt an ("Heute wurden keine Probleme protokolliert 👍", wenn leer) und öffnet den
  Log-Ordner per Button im Explorer. Das Protokollieren selbst wirft nie Exceptions - ein Logger,
  der die App crasht, wäre schlimmer als keiner.
- **Eigene Aufgaben** ("Eigene Aufgaben (z.B. von der Lehrkraft)"): Formular zum manuellen Anlegen
  eigener Fragen (Fach, Klassenstufe, Fragetyp, Frage, Antwortoptionen, richtige Antwort(en),
  Erklärung, optionaler Tipp) - z.B. für aktuelle Hausaufgaben oder Themen, die die Lehrkraft gerade
  durchnimmt. Gespeichert in der lokalen DB (`CustomQuestionRepository`/`CustomQuestionEntity`,
  Tabelle `CustomQuestions`), unabhängig von den generierten Aufgaben. Diese Aufgaben werden **additiv**
  zu den generierten Übungsaufgaben (passendes Fach + Klassenstufe) und zum Abschlussquiz
  hinzugefügt - sie ersetzen die Generatoren nicht. Löschbar über den "Löschen"-Button je Eintrag.

**Bewusst keine Zeitlimit-Funktion**: Es gibt kein tägliches Zeitlimit für die Lernsession - die
Kinder sollen so viel Zeit erhalten, wie sie zum Durcharbeiten von Lesen/Fächern/Abschlussquiz
benötigen. Das ist eine bewusste Design-Entscheidung, kein technisches Versäumnis.

## Bekannte Grenzen / nächste Schritte

- **KI-Funktionen: komplett lokal, keine Cloud-Anbindung.** LernTor nutzt an keiner Stelle einen
  Cloud-KI-Dienst - das lokal geladene Sprachmodell ist die einzige KI-Anbindung, für zwei Features:
  automatisches Einlesen von Lehrer-Unterlagen (PDF/Word → Fragenvorschläge) und den KI-Lernchat für
  Kinder (siehe "Eltern-Features" bzw. weiter unten). Beide teilen sich dieselbe Infrastruktur:
  - **`LocalLlmModelHost`** (`src/LernTor.ContentGen/Llm/LocalLlmModelHost.cs`) lädt das GGUF-Modell
    über [LLamaSharp](https://github.com/SciSharp/LLamaSharp) (llama.cpp-Bindings, Pakete `LLamaSharp` +
    `LLamaSharp.Backend.Cpu`, reines CPU-Backend ohne CUDA-Abhängigkeit) genau einmal und hält es im
    Speicher, statt es bei jedem Aufruf neu zu laden.
  - **Modell-Katalog + automatischer Download** (`LocalLlmModelCatalog.cs`): Eltern wählen im
    Eltern-Bereich per Dropdown eines der kuratierten Modelle; die Datei wird beim ersten Gebrauch
    automatisch nach `%LOCALAPPDATA%\LernTor\models\` heruntergeladen (einmalig Internet nötig, danach
    komplett offline; jedes Modell hat seine eigene Datei, ein Wechsel lädt also nur einmal herunter).
    - **Standard: [Qwen2.5-7B-Instruct](https://huggingface.co/Qwen/Qwen2.5-7B-Instruct-GGUF)**
      (Q4_K_M, ~4,7 GB, Apache-2.0): als einzige Familie im permissiv lizenzierten, CPU-tauglichen
      7B-Bereich offiziell stark in Deutsch UND Türkisch (29+ Sprachen), deutlich besseres Reasoning
      und Erklärqualität als das 3B - und dieselbe Modellfamilie, gegen die Prompt-Format und
      AntiPrompts-Stoppwörter in dieser Codebasis bereits erprobt sind. Verworfen: Mistral-7B
      (Türkisch schwach), Llama-3.1-8B (Lizenz + Türkisch mittelmäßig), Gemma-2-9B (Lizenz),
      Qwen2.5-14B (~9 GB Q4 → auf Familien-CPUs nur 2-4 Token/s, zu langsam für den Kinder-Chat).
    - **Alternative: Qwen2.5-3B-Instruct** (Q4_K_M, ~2 GB) als "Leicht & schnell"-Option für ältere
      PCs, auf denen das 7B zu langsam antwortet.
    - **Download-Robustheit** (nach real fehlgeschlagenem Erst-Download beim Nutzer korrigiert):
      der Download nutzt einen eigenen HttpClient ohne Timeout - der geteilte App-Client hätte mit
      seinem 100-Sekunden-Standard-Timeout jeden GB-Download über Heim-Internet mitten im Stream
      abgebrochen (das war die eigentliche Fehlerursache). Als Quellen dienen die
      bartowski/*-GGUF-Repos (verlässlich Einzeldateien; die offiziellen Qwen/*-GGUF-Repos teilen
      größere Modelle in `-00001-of-00002`-Teildateien, dort existiert die 7B-Einzeldatei-URL nicht),
      mit Spiegel-Liste pro Modell - schlägt eine Quelle fehl, wird die nächste probiert. Modell- und
      Dateiauswahl im Eltern-Bereich werden sofort gespeichert (nicht erst über den
      "Speichern"-Button - das war ein gemeldeter Bug).
    **Die Hugging-Face-URLs bleiben aus der Entwicklungsumgebung heraus nicht verifizierbar**
    (huggingface.co dort blockiert); schlägt der Download trotz Spiegel-Liste fehl, zeigt die App eine
    klare Fehlermeldung (inkl. Modellname, Größe und technischem Grund) und Eltern können ein anderes
    Modell wählen oder manuell eine `.gguf`-Datei über den Datei-Dialog hinterlegen (eigene Datei hat
    Vorrang vor dem Katalog, "✕" entfernt sie wieder).
  - **API-Verifizierung**: `LLamaSharp` 0.27.0 wurde real von nuget.org heruntergeladen und die
    kompilierte DLL per CLR-Metadaten-Analyse geprüft (nicht nur aus Dokumentation/Training geraten):
    `ModelParams(string modelPath)`-Konstruktor mit settable `ContextSize`/`GpuLayerCount`-Properties,
    statisches `LLamaWeights.LoadFromFile(ModelParams)`,
    `StatelessExecutor(LLamaWeights, IContextParams, ILogger?)`-Konstruktor (baut sich seinen
    Inferenz-Kontext laut Metadaten-Analyse intern selbst auf - ein separat per
    `LLamaWeights.CreateContext(...)` erzeugter Kontext wird gar nicht entgegengenommen) sowie
    `InferAsync(string, InferenceParams?, CancellationToken)`, das laut TypeRef-Analyse
    `IAsyncEnumerable<string>` liefert (per `await foreach` zum vollständigen Antworttext
    zusammengesetzt). `LLamaWeights` implementiert `IDisposable`.
- **Automatisches Einlesen von Lehrer-Unterlagen** (`src/LernTor.ContentGen/TeacherImport/`): PDF
  (`PdfPigTextExtractor`) bzw. .docx (`OpenXmlWordTextExtractor`, kein altes binäres .doc) → Fließtext →
  `LocalLlmQuestionSuggester` stellt eine Frage nach strukturiertem JSON mit Quizfragen. Ergebnisse sind
  immer nur Vorschläge (`ExtractedQuestionDraft`, mit `SourceExcerpt` = Textstelle im Original) - Eltern
  müssen im Eltern-Bereich jeden Vorschlag einzeln über "Übernehmen" bestätigen oder "Verwerfen", bevor
  er via `CustomQuestionRepository.AddAsync` gespeichert wird. Keine automatische Übernahme ohne
  menschliche Kontrolle. Der manuelle Editor (siehe "Eltern-Features") funktioniert unabhängig davon
  weiterhin normal.
  - **Inline-Editing**: alle Felder eines Vorschlags (Thema, Frage, Optionen, richtige Antworten,
    Erklärung, Tipp) sind direkt in der Vorschlagskarte korrigierbar, bevor übernommen wird -
    "Übernehmen" validiert (Frage/Antwort vorhanden, bei Auswahlfragen mindestens 2 Optionen und die
    richtige Antwort wörtlich darunter) und zeigt Fehler direkt an der Karte. Die Belegstelle aus dem
    Dokument bleibt bewusst nicht editierbar - sie ist die Prüf-Referenz, kein Inhalt.
- **KI-Lernchat für Kinder** (`src/LernTor.App/Controls/QuestionCard.xaml`, "🤖 KI fragen"-Button):
  Kinder können zu jeder Aufgabe - in News, Übungen und im Abschlussquiz gleichermaßen, da alle drei
  dieselbe `QuestionCard` verwenden - dem KI-Assistenten Rückfragen stellen, so wie sie einen
  Taschenrechner benutzen würden: als Werkzeug, mit dem man sich auseinandersetzt, nicht als reinen
  Lösungsautomaten. Der Prompt (`HomeworkChatPromptBuilder`, `src/LernTor.ContentGen/HomeworkChat/`)
  bekommt bewusst die richtige Antwort und die Erklärung der Aufgabe MIT in den Kontext - ohne sie zu
  kennen, könnte die KI nur raten statt wirklich zu helfen. Die Leitplanke ist eine reine
  Prompt-Anweisung: die fertige Antwort nicht schon in der ersten Nachricht preiszugeben, sondern zuerst
  durch Rückfragen und Denkanstöße zu helfen - erst wenn das Kind es mehrfach probiert hat oder
  ausdrücklich danach fragt, soll die KI die Lösung erklären.
- **News-Quellen**: kuratierte RSS-Feeds (siehe `LernTor.News/NewsFeedSource.cs`), inklusive einer
  KI-/Technik-Quelle (heise online, IT Boltwise), einer dedizierten Spiele-Quelle (GameStar) sowie
  offizieller Bundesregierungs-Quellen (Bundesregierung
  kompakt, Bundesregierung Pressemitteilungen, BMBFSFJ für Bildungs-/Familienthemen), dazu einer
  Herabstufung (nicht Ausfilterung) von Artikeln mit verstörenden Themen (Krieg, Gewaltverbrechen, ...)
  über `SensitiveKeywords`. RSS-URLs von Nachrichtenseiten ändern sich gelegentlich – nicht erreichbare
  Feeds werden übersprungen, sollten aber gelegentlich geprüft werden. Inzwischen auf einem echten
  Windows-Rechner getestet und bestätigt funktionsfähig (inkl. der neuen heise.de-Quelle).
- **Vereinfachung der Artikeltexte** ist aktuell regelbasiert (kein LLM). Ein lokales LLM (Phi-3, Gemma 2,
  Llama 3.1 über Ollama) ließe sich über das `ITextSimplifier`-Interface in `LernTor.News` als zweite
  Implementierung ergänzen, ohne den Rest der App anzufassen.
- Wird inzwischen auch auf einem echten Windows-Rechner getestet (nicht nur über den CI-Workflow
  `.github/workflows/build.yml` auf `windows-latest`), da diese Entwicklungsumgebung selbst kein
  Windows bereitstellt.

## Schnellstart (Entwicklung)

```powershell
git clone <repo>
cd HeroKid
dotnet restore LernTor.sln
dotnet build LernTor.sln
$env:LERNTOR_SKIP_LOCK = "1"   # WICHTIG für die Entwicklung: verhindert die Kiosk-Sperre!
dotnet run --project src/LernTor.App
```

Vollständige Build-/Installationsanleitung: [docs/BUILD.md](docs/BUILD.md).

Weitere technische Details:

- Tipptrainer und die behobenen WPF-Binding-Fallen: [docs/TIPPTRAINER.md](docs/TIPPTRAINER.md)
- Fach- und Stage-System: [docs/FAECHER-SYSTEM.md](docs/FAECHER-SYSTEM.md)
