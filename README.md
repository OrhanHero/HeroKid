# LernTor

Eine kindersichere Windows-Kiosk-Lern-App: Nach dem Login startet LernTor im Vollbild und
sperrt den PC, bis das Kind News gelesen, Übungsaufgaben in den freigeschalteten Fächern
bearbeitet und das Abschlussquiz mit mindestens 50% bestanden hat. Erst dann wird der PC
freigegeben. Eltern legen im Eltern-Bereich fest, welche der 12 verfügbaren Fächer für den
Tag/das Profil überhaupt aktiv sind (siehe "Bereiche deaktivieren").

Zielgruppe: deutsch-türkische Kinder (ca. 10–15 Jahre) in Berlin, Lehrplan-Themen orientiert
am Berliner Rahmenlehrplan, Klasse 6 und 9 (siehe [docs/CURRICULUM.md](docs/CURRICULUM.md)).

## Profile

Mehrere Kinder am selben PC wählen beim Start ihr eigenes Profil (Name, Alter, Klasse) aus einer
Kachel-Auswahl, oder legen über "Neues Profil ➕" ein weiteres Profil mit eigener Klassenstufe
(6 oder 9) an. Jedes Profil hat seinen eigenen Fortschritt, sein eigenes Abschlussquiz-Ergebnis
und sein eigenes Aktivitätsprotokoll (im Eltern-Bereich per Dropdown auswählbar). Beim ersten
Start werden automatisch zwei Beispielprofile angelegt. Nach Auswahl/Erstellung wird der
Profilname dauerhaft oben rechts im Kiosk-Fenster angezeigt (👤 Name), damit jederzeit erkennbar
ist, welches Kind gerade angemeldet ist.

## Ablauf für das Kind

1. **Profil wählen**
2. **Lesen** (Pflicht, nicht überspringbar) – täglich wechselndes Stück aus einem festen Pool von
   60 Texten (siehe `LernTor.Core/Services/ReadingContentProvider.cs`), das **gleichzeitig auf
   Deutsch, Türkisch und Englisch nebeneinander** angezeigt wird, damit dasselbe Stück in allen drei
   Sprachen gelesen werden kann. Soll **laut vorgelesen** werden. Ein 5-Minuten-Timer läuft; "Weiter"
   ist erst danach nutzbar - es gibt bewusst keine Überspringen-Funktion. Ob tatsächlich (laut)
   gelesen wurde, kann technisch nicht geprüft werden – das ist eine bekannte, akzeptierte Grenze
   dieser Funktion. Der Pool besteht aus zwei Hälften:
   - **30 literarische/Allgemeinwissen-Stücke**: gemeinfreie Gedichte/Texte (Goethe, Heine, Yunus
     Emre, Schiller, Rilke, Robert Frost, William Blake, Lewis Carroll, u.a.) sowie kurze
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
3. **News** (Pflicht) – kuratierte RSS-Artikel mit Fokus Berlin/Deutschland und Istanbul/Samsun/Ünye/Türkei
   (Berlin-Lokalnachrichten werden garantiert einbezogen), je 1-2 Verständnisfragen pro Artikel.
4. **Fachbereiche** (alle nicht von den Eltern deaktivierten, Klasse 6/9): Mathematik, Deutsch, Türkisch,
   Englisch, Biologie, Chemie, Physik, Gesellschaftswissenschaften (Gewi), Politik, Geografie, Ethik,
   Medienbildung (ITG) – siehe [docs/CURRICULUM.md](docs/CURRICULUM.md) für die genauen Themen je Fach.
   Bei offenen Mathematik-Aufgaben steht ein Taschenrechner zur Verfügung; Aufgaben mit hinterlegtem
   Tipp zeigen einen abrufbaren Formel-/Vorgehens-Hinweis (verrät nicht die Lösung).
5. **Abschlussquiz** – gemischte Fragen aus allen aktiven Fächern (Anzahl passt sich automatisch an, wie
   viele Fächer aktiv sind), ≥50% richtig → PC wird freigeschaltet. Bei Nichtbestehen werden gezielt mehr
   Fragen aus den schwachen Fächern gestellt. Antwortoptionen bei Multiple-Choice-Fragen werden bei jeder
   Anzeige neu gemischt, damit die richtige Antwort nicht immer an derselben Stelle steht.

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

## Eltern-Features

- Zahnrad-Symbol (unten rechts, dezent) öffnet den passwortgeschützten Eltern-Bereich.
- Erststart: Admin-Passwort selbst festlegen (PBKDF2-Hash, kein Klartext gespeichert).
- Fachbereiche einzeln deaktivieren, Klassenstufe (6/9) einstellen.
- Aktivitätsprotokoll: alle beantworteten Aufgaben + Quiz-Ergebnisse einsehbar.
- "Sofort freischalten": Notfall-Override, überspringt den restlichen Ablauf.
- "Alle Daten zurücksetzen…" (Gefahrenzone): löscht alle Profile/Fortschritte/Einstellungen aus der
  App heraus, mit Ja/Nein-Bestätigung. Vorher ging das nur manuell über das Löschen von `lerntor.db`.
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

- **Automatisches Einlesen von Lehrer-Unterlagen (PDF/Word) über NotebookLM Enterprise**: implementiert
  (siehe `src/LernTor.ContentGen/TeacherImport/`) und im Eltern-Bereich nutzbar - **größtenteils gegen
  die echte Doku verifiziert, ein Teil bleibt Annahme**. Der manuelle Editor (siehe "Eltern-Features")
  funktioniert unabhängig davon weiterhin normal.
  - **Verifizierungs-Historie**: `NotebookLmQuestionSuggester.cs` wurde zunächst ohne Zugriff auf die
    offizielle API-Dokumentation geschrieben (docs.cloud.google.com war durch die Organisations-Policy
    dieser Entwicklungsumgebung blockiert, 403 per curl UND WebFetch bestätigt). Zwei Nachbesserungen
    haben seither die Unsicherheit reduziert:
    1. **api.nuget.org war aus derselben Sandbox erreichbar** - die drei NuGet-Pakete (`UglyToad.PdfPig`,
       `DocumentFormat.OpenXml`, `Google.Apis.Auth`) wurden real heruntergeladen und ihre Methoden-
       signaturen per Metadaten-Analyse verifiziert (dabei einen echten Bug gefunden und behoben:
       `GetAccessTokenForRequestAsync` ist eine explizite Interface-Implementierung, nur über eine
       `ITokenAccess`-Referenz aufrufbar).
    2. **Der Nutzer hat die offizielle NotebookLM-Enterprise-Dokumentation als PDF exportiert und
       bereitgestellt**, wodurch die Basis-URL (mit Regions-Präfix vor dem Hostnamen, z.B.
       `us-discoveryengine.googleapis.com` statt nur `discoveryengine.googleapis.com`), das
       Ressourcen-Pfadschema, sowie `notebooks.create`, `notebooks.sources.batchCreate` (Text-Upload)
       und `notebooks.batchDelete` (ein POST mit `names`-Array, **kein** HTTP-DELETE) jetzt nach
       dokumentierten Beispielen implementiert sind.
  - **⚠️ Weiterhin unverifiziert bleibt nur `QueryNotebookAsync`** (die Frage-/Antwort-Funktion): Die
    bereitgestellte Doku enthielt dafür keine eigene REST-Anleitung, nur Feldnamen aus einer
    Audit-Log-Tabelle (RPC-Methode `NotebookService.InteractSources`, Anfragefelder `name`/
    `input_sources`/`free_form_action`, Antwortfeld `response.response`). Der REST-Pfad
    (`:interactSources`) und die genaue innere Form von `free_form_action` sind eine begründete
    Annahme und müssen beim ersten echten Testlauf mit echten Zugangsdaten überprüft werden - siehe
    den `// NICHT verifiziert`-Kommentar direkt am Methodenkommentar im Code. Fehlermeldungen bei
    HTTP-Fehlern (404 o.ä.) weisen im Eltern-Bereich explizit darauf hin, wenn genau dieser Schritt
    scheitert.
  - **Funktionsweise**: PDF (`PdfPigTextExtractor`) bzw. .docx (`OpenXmlWordTextExtractor`, kein altes
    binäres .doc) → Fließtext → `NotebookLmQuestionSuggester` legt ein Wegwerf-Notebook an, lädt den
    Text als Quelle hoch, stellt eine Frage nach strukturiertem JSON mit Quizfragen und löscht das
    Notebook danach wieder. Ergebnisse sind immer nur Vorschläge (`ExtractedQuestionDraft`, mit
    `SourceExcerpt` = Textstelle im Original) - Eltern müssen im Eltern-Bereich jeden Vorschlag einzeln
    über "Übernehmen" bestätigen oder "Verwerfen", bevor er via `CustomQuestionRepository.AddAsync`
    gespeichert wird. Keine automatische Übernahme ohne menschliche Kontrolle.
  - **Konfiguration** (Eltern-Bereich, Abschnitt "Automatisches Einlesen…"): Google-Cloud-Projekt-
    **Nummer** (laut Doku wird im Ressourcenpfad wörtlich die Projekt-Nummer erwartet, nicht die
    textuelle Projekt-ID - die UI weist explizit darauf hin), Region (Standard "global"; laut Doku
    wird sie als Präfix vor den API-Hostnamen gesetzt, z.B. "us"/"eu"), Pfad zur JSON-Schlüsseldatei
    eines GCP-Dienstkontos. Ohne diese drei Angaben bleibt die Funktion inaktiv und wirft beim Versuch,
    sie zu nutzen, eine klare Fehlermeldung statt stillschweigend nichts zu tun.
  - **Nutzungslimits laut Doku** (falls beim Testen 429/Quota-Fehler auftreten): 500 Notebooks pro
    Nutzer, 300 Quellen pro Notebook, 500 MB/500.000 Wörter pro Quelle, 500 Abfragen pro Nutzer und Tag.
  - **Datenschutz-Hinweis**: Dokumente werden zur Verarbeitung an Google Cloud übertragen - das ist
    bewusst eine bewusste Entscheidung der Eltern (Konfiguration ist optional/leer per Standard), keine
    versteckte Standardeinstellung.
  - **Noch nicht umgesetzt / nice-to-have**: Inline-Bearbeitung einzelner Felder eines Vorschlags vor
    dem Übernehmen (aktuell nur ganz übernehmen oder ganz verwerfen - Korrekturen sind über den
    bestehenden manuellen "Eigene Aufgaben"-Editor möglich); lokale LLM-Alternative (Ollama) als
    zweite `ITeacherQuestionSuggester`-Implementierung für Familien, die keine Cloud-Verarbeitung
    möchten.
- **News-Quellen**: kuratierte RSS-Feeds (siehe `LernTor.News/NewsFeedSource.cs`), inklusive einer
  KI-/Technik-Quelle (heise online) und einer Herabstufung (nicht Ausfilterung) von Artikeln mit
  verstörenden Themen (Krieg, Gewaltverbrechen, ...) über `SensitiveKeywords`. RSS-URLs von
  Nachrichtenseiten ändern sich gelegentlich – nicht erreichbare Feeds werden übersprungen, sollten aber
  gelegentlich geprüft werden. Inzwischen auf einem echten Windows-Rechner getestet und bestätigt
  funktionsfähig (inkl. der neuen heise.de-Quelle).
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
