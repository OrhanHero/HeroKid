# LernTor bauen & installieren

## Voraussetzungen

- Windows 10 oder 11 zum **Ausführen** (die App nutzt WPF + Win32-APIs)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Langzeitversion, unterstützt bis
  November 2028; die genaue Mindestversion steht in `global.json`)
- Optional: Visual Studio 2022 17.14+ oder 2026 (Workload ".NET Desktop Development")
- Optional, für den Installer: [Inno Setup 6](https://jrsoftware.org/isinfo.php)

> **Seit 29.09.2026 auf .NET 10.** .NET 8 bekommt ab dem 10.11.2026 keine Sicherheitsupdates mehr.
> Die Datenbank ist davon nicht betroffen, ein Update von einer .NET-8-Version braucht keinen
> Zwischenschritt.

Der GitHub-Actions-Workflow `.github/workflows/build.yml` baut die Solution bei jedem Push zweimal:
schnell unter Linux (Bauen + Unit-Tests, etwa zwei Minuten) und vollständig unter `windows-latest`
(zusätzlich UI-Tests, Publish, Prüfung der KI-Bibliotheken) und lädt das fertige Programm als
Artefakt `LernTor-win-x64` hoch. Das ist der einfachste Weg zu einem Build ohne eigenen
Windows-Rechner.

### Bauen unter Linux (Entwicklungsumgebung ohne Windows)

**Bauen geht überall, ausführen nur unter Windows.** `Directory.Build.props` setzt
`EnableWindowsTargeting`, damit holt sich das SDK die Windows-Referenz-Assemblys als NuGet-Paket.
Compiler und XAML-Übersetzung laufen dann auch für die WPF-App vollständig durch:

```bash
sudo apt-get install -y dotnet-sdk-10.0      # Ubuntu 24.04: aus dem normalen Ubuntu-Archiv
dotnet build LernTor.sln -c Release           # ganze Solution, auch LernTor.App (WPF)
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj -c Release --no-build
```

Das fängt alle Fehler, die der Compiler sieht (auch `MC3024` und andere XAML-Übersetzungsfehler), in
Sekunden statt nach einer CI-Runde. **Nicht** gefangen werden Laufzeitfehler beim Laden der XAML
(`XamlParseException`) - die finden nur die UI-Tests, und die laufen nur unter Windows.

### Fertige Umgebungen (seit 30.09.2026)

Damit niemand das SDK von Hand installieren muss:

| Wo | Was passiert | Datei |
|---|---|---|
| **Claude Code im Web** | Beim Start jeder Sitzung installiert ein Hook `dotnet-sdk-10.0` (falls es fehlt), stellt die Pakete wieder her und setzt `DOTNET_NOLOGO`. Läuft synchron, dauert mit fertigem Container etwa 3 Sekunden. | `.claude/hooks/session-start.sh`, `.claude/settings.json` |
| **VS Code (Dev Containers) / GitHub Codespaces** | Container mit .NET 10 und Python; beim Anlegen einmal `restore`, `build` und `preflight.py --quick`. | `.devcontainer/devcontainer.json` |
| **Jeder Editor** | Einrückung (4 Leerzeichen, JSON/YAML 2), LF, UTF-8 ohne BOM, Klammern auf eigener Zeile, `_feld` für private Felder. | `.editorconfig` |

### Paketversionen

Jede NuGet-Paketversion steht genau einmal in `Directory.Packages.props` (zentrale
Paketverwaltung); die `.csproj`-Dateien nennen nur den Paketnamen. Gemeinsame Einstellungen
(Nullable, ImplicitUsings, NuGet-Sicherheitsprüfung) stehen in `Directory.Build.props`. Dependabot
schlägt monatlich Updates vor (`.github/dependabot.yml`); große Versionssprünge (z. B. .NET 11)
bleiben eine bewusste Entscheidung.

## 1. Entwicklung / Debuggen (ohne Kiosk-Sperre!)

Die App aktiviert standardmäßig die Kiosk-Sperre (Vollbild, Tastatur-Hook, Task-Manager-Sperre) –
das würde bei jedem Testlauf den eigenen Entwicklungs-PC sperren. Für die Entwicklung daher immer:

```powershell
$env:LERNTOR_SKIP_LOCK = "1"
dotnet run --project src/LernTor.App
```

Die Sperre wird auch automatisch übersprungen, wenn ein Debugger angehängt ist (`F5` in Visual Studio).

## 2. Tests ausführen

```powershell
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj     # überall (auch Linux)
dotnet test tests/LernTor.UiTests/LernTor.UiTests.csproj # nur Windows: XAML-Laden, echter App-Start
```

## 3. Self-contained Release-Build erzeugen

```powershell
dotnet publish src/LernTor.App/LernTor.App.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output publish/win-x64 `
  -p:PublishSingleFile=true `
  -p:PublishReadyToRun=true
```

Das Ergebnis in `publish/win-x64` ist eigenständig lauffähig (keine separate .NET-Installation auf dem
Ziel-PC nötig) und liegt für den Kiosk-Einsatz als eine einzelne `LernTor.exe`
(`PublishSingleFile`) vor, die dank `PublishReadyToRun` (vorab-JIT-kompiliert für win-x64) auch
nach dem automatischen Autostart-Login schneller hochfährt.

**Bewusst nicht gesetzt: `IncludeNativeLibrariesForSelfExtract`.** Mit dieser Option landen native
Bibliotheken in der exe und werden beim Start in einen Temp-Ordner entpackt. LLamaSharp sucht seine
Bibliotheken (`llama.dll`, `ggml-*.dll`) aber selbst per Dateipfad neben der exe und findet sie
dort nicht - die App startet dann normal, nur KI-Chat und Lehrer-Import scheitern mit
`The type initializer for 'LLama.Native.NativeApi' threw an exception`. Genau dieser Fehler ist im
Familienbetrieb zweimal aufgetreten; ein erster Reparaturversuch über ein eigenes MSBuild-Target
lief still ins Leere, weil das Publish-Kommando die Option gleichzeitig wieder aktivierte.

Ohne die Option bleiben native Bibliotheken als lose Dateien liegen (Standard seit .NET 6). Der
CI-Lauf prüft nach jedem Publish, ob `llama.dll` und mindestens eine `ggml-*.dll` wirklich lose
vorliegen, damit dieser Fehler nicht ein drittes Mal erst beim Kind auffällt.

**Wichtig für die Weitergabe:** Das Publish-Ergebnis ist deshalb *nicht* nur `LernTor.exe`. Neben
der exe liegt ein Ordner `runtimes/` mit je einem Unterordner pro CPU-Variante:

```
runtimes/win-x64/native/noavx/    llama.dll, ggml.dll, ggml-base.dll, ggml-cpu.dll, mtmd.dll
runtimes/win-x64/native/avx/      (dieselben Dateien)
runtimes/win-x64/native/avx2/     (dieselben Dateien)
runtimes/win-x64/native/avx512/   (dieselben Dateien)
runtimes/win-arm64/native/        (für ARM-Geräte)
```

LLamaSharp wählt beim Start selbst die zur CPU passende Variante. Wird nur die exe kopiert oder der
`runtimes`-Ordner "aufgeräumt", startet die App zwar normal, aber KI-Chat und Lehrer-Import fallen
aus. Beim Aktualisieren einer bestehenden Installation deshalb immer das komplette ZIP entpacken.

**Bewusst nicht aktiviert: `PublishTrimmed`.** WPF nutzt an vielen Stellen Reflection (Binding,
`DataTemplate`-Auflösung, Converter), die der Trimmer nicht zuverlässig erkennt - ohne umfangreiche
`TrimmerRootAssembly`-Ausnahmen und anschließenden vollständigen manuellen GUI-Test auf echtem
Windows würde Trimmen zur Laufzeit unsichtbar Funktionalität entfernen (kein Compile-Fehler, nur
kaputtes UI). Der Startzeit-Gewinn kommt ohnehin größtenteils schon aus ReadyToRun.

### Release über GitHub (seit 29.09.2026)

Ein Git-Tag `v<Version>` baut die App mit genau diesen Flags, führt Unit- und UI-Tests aus, prüft
die KI-Bibliotheken und legt das Ergebnis als ZIP samt SHA-256-Prüfsumme an ein GitHub-Release
(`.github/workflows/release.yml`):

```bash
git tag v2.0.1
git push origin v2.0.1
```

**Ohne lokales Git** geht es auch im Browser: auf GitHub unter **Actions → Release → Run workflow**
den Branch (`master`) wählen und die Version ohne „v“ eintragen (z. B. `2.0.1`). Der Workflow baut
dann genau diesen Stand und legt Tag und Release in einem Schritt an. In beiden Fällen **kein**
Release vorher über die GitHub-Oberfläche anlegen – sonst scheitert der letzte Schritt, weil es
das Release schon gibt.

Die Versionsnummer kommt aus dem Tag und erscheint in der **Systeminfo** im Eltern-Bereich
(„LernTor 2.0.1 (245a544)“ – dahinter der Git-Stand, aus dem gebaut wurde). Ohne Tag gilt die
`<Version>` aus `Directory.Build.props`. Das Release schaltet nichts ein: LernTor fragt nirgends
nach neuen Versionen, ein Auto-Update wäre eine eigene Entscheidung (siehe
[`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md)).

## 4. Installer bauen (optional)

```powershell
# Inno Setup Compiler (iscc.exe) muss im PATH sein oder mit vollem Pfad aufgerufen werden
iscc src\LernTor.Installer\setup.iss
```

Das fertige Setup landet in `dist\LernTor-Setup-2.0.0.exe` (Version aus `MyAppVersion` in
`setup.iss`). Der Installer:

- kopiert die App nach `Program Files\LernTor`,
- registriert automatisch einen Autostart-Task (läuft direkt nach dem Windows-Login des Kindes),
- entfernt den Autostart-Task beim Deinstallieren wieder.

## 5. Manueller Autostart (ohne Installer)

```powershell
.\src\LernTor.Installer\install-autostart.ps1 -Action install -ExePath "C:\Pfad\zu\LernTor.exe"
# Entfernen:
.\src\LernTor.Installer\install-autostart.ps1 -Action uninstall -ExePath "C:\Pfad\zu\LernTor.exe"
```

## 6. Datenbankschema-Updates: automatisch, kein DB-Löschen mehr nötig

Seit dem automatischen Schema-Abgleich (`LernTor.Data.SqliteSchemaUpdater`, läuft bei jedem
App-Start nach `EnsureCreated`) werden **additive** Schema-Änderungen - neue Tabellen, neue
Spalten, neue Indizes - automatisch auf eine bestehende `lerntor.db` angewendet. Profile, Sterne
und Fortschritte überleben App-Updates; das früher hier beschriebene manuelle Löschen der
Datenbank entfällt. Angewendete Schema-Updates werden im Fehlerprotokoll
(`%LOCALAPPDATA%\LernTor\logs`) vermerkt.

Nur in zwei seltenen Fällen ist weiterhin ein manuelles Löschen von
`%LOCALAPPDATA%\LernTor\lerntor.db` nötig (die App legt danach automatisch eine frische
Datenbank inkl. der beiden Beispielprofile an):

- eine Spalte wurde **entfernt oder umbenannt** und die tote Alt-Spalte stört tatsächlich
  (normalerweise bleibt sie einfach harmlos stehen), oder
- vorhandene **Werte müssen umgedeutet** werden - z.B. wenn ein numerisch gespeichertes Enum
  umsortiert wurde (deshalb gilt weiterhin die Regel: Enums als Strings persistieren).

## 7. Erststart / Eltern-Passwort

Beim allerersten Start ist noch kein Admin-Passwort gesetzt. Über das dezente Zahnrad-Symbol
(unten rechts im Kiosk-Fenster) gelangt man in den Eltern-Bereich und legt beim ersten Mal ein
Passwort fest (mind. 4 Zeichen, wird als PBKDF2-SHA256-Hash mit 600.000 Durchläufen gespeichert,
nie im Klartext). Passwörter aus der Zeit vor dem 29.09.2026 (210.000 Durchläufe) bleiben gültig
und werden beim nächsten Anmelden unbemerkt mit der neuen Stärke gespeichert.

## 7. Deinstallation / Zurücksetzen des Fortschritts

- Deinstallation über "Programme hinzufügen/entfernen" entfernt App-Dateien und Autostart-Task.
- Der Lernfortschritt liegt in `%LOCALAPPDATA%\LernTor\lerntor.db` (SQLite) und wird beim
  Deinstallieren mit entfernt (siehe `[UninstallDelete]` in `setup.iss`). Zum manuellen Zurücksetzen
  einfach diese Datei löschen, während LernTor nicht läuft.

## Verwandte Dokumentation

- [README.md](../README.md) als Einstieg und Gesamtüberblick
- [TIPPTRAINER.md](TIPPTRAINER.md) für den Typing-Flow und die behobenen WPF-Binding-Fallen
- [FAECHER-SYSTEM.md](FAECHER-SYSTEM.md) für Stage-Reihenfolge und Fachzuordnung

## 7. Sicherung, Wiederherstellung, Notfall

Wo die Daten liegen, wie man eine Sicherung einspielt und was zu tun ist, wenn LernTor nicht
mehr startet, steht in [`WIEDERHERSTELLUNG.md`](WIEDERHERSTELLUNG.md). Sie ist für den Fall
geschrieben, dass niemand da ist, der es aus dem Code lesen kann.
