; LernTor Windows-Installer (Inno Setup 6.x, https://jrsoftware.org/isinfo.php)
; Erwartet, dass die App vorher mit "dotnet publish" self-contained für win-x64 gebaut wurde,
; siehe docs/BUILD.md. Der Publish-Ordner wird hier eingebunden.

#define MyAppName "LernTor"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "LernTor"
#define MyAppExeName "LernTor.exe"
#define PublishDir "..\..\publish\win-x64"

[Setup]
AppId={{7E2B6B7B-6F1B-4A8B-9C1E-4B7A0A2D9F11}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\..\dist
OutputBaseFilename=LernTor-Setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Desktop-Symbol erstellen"; GroupDescription: "Zusätzliche Symbole:"; Flags: unchecked

[Run]
; Registriert den Autostart als geplante Aufgabe (läuft direkt nach dem Login des Kindes).
Filename: "{app}\{#MyAppExeName}"; Parameters: "--register-autostart"; Flags: runascurrentuser waituntilterminated

[UninstallRun]
; Entfernt den Autostart-Task vor der Deinstallation wieder.
Filename: "{app}\{#MyAppExeName}"; Parameters: "--unregister-autostart"; Flags: runascurrentuser waituntilterminated; RunOnceId: "RemoveAutostart"

[UninstallDelete]
Type: filesandordirs; Name: "{localappdata}\LernTor"
