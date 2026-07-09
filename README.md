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

- **Automatisches Einlesen von Lehrer-Unterlagen (PDF/Word)**: Architektur vorbereitet (siehe
  `src/LernTor.ContentGen/TeacherImport/`), aber noch nicht mit einer echten LLM-Anbindung verdrahtet
  und noch nicht an eine Eltern-Bereich-UI angebunden. Der manuelle Editor (siehe "Eltern-Features")
  deckt den Kernbedarf ("eigene Themen/Aufgaben einpflegen") in der Zwischenzeit bereits ab.
  - **Vorbereitete Schnittstellen**: `ITeacherDocumentTextExtractor` (Datei → Fließtext, pro
    Dateiformat austauschbar), `ITeacherQuestionSuggester` (Fließtext → Liste von
    `ExtractedQuestionDraft`-Vorschlägen, i.d.R. per LLM), `TeacherDocumentImportService`
    (orchestriert beides). Bewusst getrennt, damit Textextraktion (reine Bibliotheksarbeit) und
    LLM-Anbindung unabhängig voneinander implementiert/getestet werden können.
  - **`ExtractedQuestionDraft`** ist absichtlich kein `QuizQuestion`: alle Felder sind veränderlich
    und es gibt ein `SourceExcerpt`-Feld (Originaltextstelle), damit Eltern jeden Vorschlag im
    Eltern-Bereich gegen die Quelle prüfen/korrigieren können, bevor er über
    `CustomQuestionRepository.AddAsync` dauerhaft gespeichert wird - kein automatisches Übernehmen
    ohne menschliche Kontrolle, da eine falsch erkannte "richtige Antwort" schlimmer wäre als gar
    keine automatisch erzeugte Frage.
  - **`NotConfiguredTeacherQuestionSuggester`** ist der aktuelle Platzhalter für `ITeacherQuestionSuggester`
    und wirft absichtlich eine `NotSupportedException` mit Erklärung, statt still leere Vorschlagslisten
    zurückzugeben - eine versehentliche Verdrahtung würde sonst den Eindruck erwecken, das Feature
    funktioniere bereits, obwohl kein LLM-Anbieter konfiguriert ist.
  - **Noch zu klären, bevor eine echte Implementierung entsteht**:
    - Konkreter LLM-Anbieter: lokal via Ollama (Phi-3, Gemma 2, Llama 3.1 - kein Datenabfluss nach
      außen, braucht aber Ressourcen auf dem Kiosk-PC) vs. externe API (leistungsfähiger, aber
      Dokumente der Kinder/Lehrer verlassen den PC - Datenschutzabwägung, die die Eltern selbst
      treffen sollten, z.B. über eine Einstellung im Eltern-Bereich).
    - `PdfPig` (PDF) und `DocumentFormat.OpenXml` (.docx) als konkrete `ITeacherDocumentTextExtractor`-
      Implementierungen - beides reine .NET-Bibliotheken ohne Windows-Abhängigkeit, aber noch nicht
      als NuGet-Referenz eingebunden.
    - UI im Eltern-Bereich: Datei-Upload-Dialog, Vorschlagsliste mit Inline-Bearbeitung je Entwurf
      (analog zum bestehenden "Eigene Aufgaben"-Formular), Fach/Klassenstufe-Auswahl vor dem Senden
      an den Suggester.
- **News-Quellen**: kuratierte RSS-Feeds (siehe `LernTor.News/NewsFeedSource.cs`), inklusive einer
  KI-/Technik-Quelle (heise online) und einer Herabstufung (nicht Ausfilterung) von Artikeln mit
  verstörenden Themen (Krieg, Gewaltverbrechen, ...) über `SensitiveKeywords`. RSS-URLs von
  Nachrichtenseiten ändern sich gelegentlich – nicht erreichbare Feeds werden übersprungen, sollten aber
  gelegentlich geprüft werden. **Wichtig:** aus dieser Entwicklungsumgebung war der Netzwerkzugriff auf
  RSS-Hosts generell blockiert (auch auf bereits produktiv genutzte Feeds wie tagesschau.de), die neu
  hinzugefügte heise.de-URL konnte deshalb nicht getestet werden - bitte nach dem nächsten Start einmal
  prüfen, ob der News-Bereich weiterhin lädt.
- **Vereinfachung der Artikeltexte** ist aktuell regelbasiert (kein LLM). Ein lokales LLM (Phi-3, Gemma 2,
  Llama 3.1 über Ollama) ließe sich über das `ITextSimplifier`-Interface in `LernTor.News` als zweite
  Implementierung ergänzen, ohne den Rest der App anzufassen.
- Konnte in dieser Umgebung nicht auf einem echten Windows-Rechner gebaut/getestet werden (siehe
  [docs/BUILD.md](docs/BUILD.md)) – der CI-Workflow (`.github/workflows/build.yml`) baut und testet das
  Projekt automatisch auf `windows-latest`.

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
