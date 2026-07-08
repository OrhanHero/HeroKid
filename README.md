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
Start werden automatisch zwei Beispielprofile angelegt.

## Ablauf für das Kind

1. **Profil wählen**
2. **Lesen** (Pflicht, nicht überspringbar) – täglich wechselndes Gedicht/wichtiges Werk (Mix aus
   Deutsch/Türkisch/Englisch/Allgemeinwissen, siehe `LernTor.Core/Services/ReadingContentProvider.cs`),
   soll **laut vorgelesen** werden. Ein 5-Minuten-Timer läuft; "Weiter" ist erst danach nutzbar - es gibt
   bewusst keine Überspringen-Funktion. Ob tatsächlich (laut) gelesen wurde, kann technisch nicht geprüft
   werden – das ist eine bekannte, akzeptierte Grenze dieser Funktion.
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
  LernTor.Data         SQLite-Persistenz (EF Core): Fortschritt, Aktivitätsprotokoll, Einstellungen
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
- Fachbereiche einzeln deaktivieren, Klassenstufe (6/9) einstellen, tägliches Zeitlimit (Grundgerüst).
- Aktivitätsprotokoll: alle beantworteten Aufgaben + Quiz-Ergebnisse einsehbar.
- "Sofort freischalten": Notfall-Override, überspringt den restlichen Ablauf.
- "Alle Daten zurücksetzen…" (Gefahrenzone): löscht alle Profile/Fortschritte/Einstellungen aus der
  App heraus, mit Ja/Nein-Bestätigung. Vorher ging das nur manuell über das Löschen von `lerntor.db`.

## Bekannte Grenzen / nächste Schritte

- **Individuelle Themen/Aufgaben von Lehrkräften einpflegen**: aktuell noch nicht möglich – die
  Generatoren liefern feste, rahmenlehrplan-orientierte Beispielaufgaben. Ein Editor im Eltern-Bereich
  für eigene Fragen (gespeichert in der lokalen DB, ergänzend zu den generierten Fragen) ist als
  nächster Schritt sinnvoll; automatisches Einlesen von Lehrer-Unterlagen (PDF/Word) bräuchte zusätzlich
  ein LLM zur Extraktion und ist ein größeres Folgeprojekt.
- **News-Quellen**: kuratierte RSS-Feeds (siehe `LernTor.News/NewsFeedSource.cs`). RSS-URLs von
  Nachrichtenseiten ändern sich gelegentlich – nicht erreichbare Feeds werden übersprungen, sollten aber
  gelegentlich geprüft werden.
- **Vereinfachung der Artikeltexte** ist aktuell regelbasiert (kein LLM). Ein lokales LLM (Phi-3, Gemma 2,
  Llama 3.1 über Ollama) ließe sich über das `ITextSimplifier`-Interface in `LernTor.News` als zweite
  Implementierung ergänzen, ohne den Rest der App anzufassen.
- **Zeitlimit-Durchsetzung**: Das Datenmodell/UI dafür existiert (`AppSettings.DailyTimeLimitMinutes`),
  die aktive Erzwingung (App nach Ablauf automatisch schließen) ist noch nicht verdrahtet.
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
