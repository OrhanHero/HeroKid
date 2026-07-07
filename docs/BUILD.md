# LernTor bauen & installieren

## Voraussetzungen

- Windows 10 oder 11 (die App nutzt WPF + Win32-APIs und läuft nicht unter Linux/macOS)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional: Visual Studio 2022 (Workload ".NET Desktop Development")
- Optional, für den Installer: [Inno Setup 6](https://jrsoftware.org/isinfo.php)

> Hinweis: Dieses Projekt wurde in einer Linux-Cloud-Umgebung entwickelt, die WPF nicht ausführen/bauen
> kann. Der Code wurde sorgfältig geschrieben und die plattformunabhängigen Teile (Core/ContentGen/News)
> sind mit xUnit-Tests abgesichert, aber der erste echte Build auf einem Windows-Rechner sollte auf
> Restfehler geprüft werden. Der GitHub-Actions-Workflow `.github/workflows/build.yml` baut das gesamte
> Solution automatisch auf `windows-latest` und lädt ein Build-Artefakt hoch – das ist die einfachste Art,
> einen Build ohne eigenen Windows-Rechner zu bekommen.

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
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj
```

## 3. Self-contained Release-Build erzeugen

```powershell
dotnet publish src/LernTor.App/LernTor.App.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output publish/win-x64
```

Das Ergebnis in `publish/win-x64` ist eigenständig lauffähig (keine separate .NET-Installation auf dem
Ziel-PC nötig).

## 4. Installer bauen (optional)

```powershell
# Inno Setup Compiler (iscc.exe) muss im PATH sein oder mit vollem Pfad aufgerufen werden
iscc src\LernTor.Installer\setup.iss
```

Das fertige Setup landet in `dist\LernTor-Setup-1.0.0.exe`. Der Installer:

- kopiert die App nach `Program Files\LernTor`,
- registriert automatisch einen Autostart-Task (läuft direkt nach dem Windows-Login des Kindes),
- entfernt den Autostart-Task beim Deinstallieren wieder.

## 5. Manueller Autostart (ohne Installer)

```powershell
.\src\LernTor.Installer\install-autostart.ps1 -Action install -ExePath "C:\Pfad\zu\LernTor.exe"
# Entfernen:
.\src\LernTor.Installer\install-autostart.ps1 -Action uninstall -ExePath "C:\Pfad\zu\LernTor.exe"
```

## 6. Erststart / Eltern-Passwort

Beim allerersten Start ist noch kein Admin-Passwort gesetzt. Über das dezente Zahnrad-Symbol
(unten rechts im Kiosk-Fenster) gelangt man in den Eltern-Bereich und legt beim ersten Mal ein
Passwort fest (mind. 4 Zeichen, wird als PBKDF2-Hash gespeichert, nie im Klartext).

## 7. Deinstallation / Zurücksetzen des Fortschritts

- Deinstallation über "Programme hinzufügen/entfernen" entfernt App-Dateien und Autostart-Task.
- Der Lernfortschritt liegt in `%LOCALAPPDATA%\LernTor\lerntor.db` (SQLite) und wird beim
  Deinstallieren mit entfernt (siehe `[UninstallDelete]` in `setup.iss`). Zum manuellen Zurücksetzen
  einfach diese Datei löschen, während LernTor nicht läuft.
