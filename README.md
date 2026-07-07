# LernTor

Eine kindersichere Windows-Kiosk-Lern-App: Nach dem Login startet LernTor im Vollbild und
sperrt den PC, bis das Kind News gelesen, Übungsaufgaben in vier Fächern bearbeitet und das
Abschlussquiz mit mindestens 50% bestanden hat. Erst dann wird der PC freigegeben.

Zielgruppe: deutsch-türkische Kinder (ca. 10–15 Jahre) in Berlin, Lehrplan-Themen orientiert
am Berliner Rahmenlehrplan, Klasse 6 und 9 (siehe [docs/CURRICULUM.md](docs/CURRICULUM.md)).

## Ablauf für das Kind

1. **News** (Pflicht) – kuratierte RSS-Artikel mit Fokus Berlin/Deutschland und Istanbul/Samsun/Ünye/Türkei,
   je 1-2 Verständnisfragen pro Artikel.
2. **Mathematik** – automatisch generierte Aufgaben (Klasse 6: Bruchrechnung, Prozentrechnung, Flächen,
   Maßstab; Klasse 9: lineare/quadratische Gleichungen, Pythagoras, Zinsrechnung, Binomische Formeln).
3. **Deutsch** – Grammatik, Zeitformen, Rechtschreibung, Satzgefüge (Klasse 6/9).
4. **Türkisch** – Zeitformen, Wortschatz, Satzglieder, Fiilimsi (Klasse 6/9).
5. **Naturwissenschaften** – Physik/Chemie/Biologie kombiniert.
6. **Abschlussquiz** – 20-25 gemischte Fragen, ≥50% richtig → PC wird freigeschaltet. Bei Nichtbestehen
   werden gezielt mehr Fragen aus den schwachen Fächern gestellt.

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

## Bekannte Grenzen / nächste Schritte

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
