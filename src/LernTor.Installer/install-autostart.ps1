<#
.SYNOPSIS
    Registriert oder entfernt den LernTor-Autostart als geplante Windows-Aufgabe (Soft-Lock-Ansatz,
    kein Shell-Ersatz). Nützlich, wenn LernTor manuell per "dotnet publish" gebaut wurde, ohne den
    Inno-Setup-Installer zu benutzen.

.PARAMETER Action
    "install" registriert den Autostart, "uninstall" entfernt ihn wieder.

.PARAMETER ExePath
    Vollständiger Pfad zur LernTor.exe. Standard: .\publish\win-x64\LernTor.exe relativ zu diesem Skript.

.EXAMPLE
    .\install-autostart.ps1 -Action install -ExePath "C:\Program Files\LernTor\LernTor.exe"
#>
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("install", "uninstall")]
    [string]$Action,

    [string]$ExePath = (Join-Path $PSScriptRoot "..\..\publish\win-x64\LernTor.exe")
)

if (-not (Test-Path $ExePath) -and $Action -eq "install") {
    Write-Error "LernTor.exe wurde nicht gefunden unter: $ExePath. Bitte zuerst 'dotnet publish' ausführen (siehe docs/BUILD.md) oder -ExePath angeben."
    exit 1
}

if ($Action -eq "install") {
    & $ExePath --register-autostart
    Write-Host "Autostart registriert. LernTor startet ab dem nächsten Login automatisch."
}
else {
    & $ExePath --unregister-autostart
    Write-Host "Autostart entfernt."
}
