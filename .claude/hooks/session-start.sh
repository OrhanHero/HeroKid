#!/bin/bash
# SessionStart-Hook fuer Claude Code im Web (docs/NAECHSTES-LEVEL-3.md, Schritt 6).
#
# Stellt sicher, dass jede neue Cloud-Sitzung sofort bauen und testen kann:
#   - .NET 10 SDK aus dem normalen Ubuntu-Archiv (der Microsoft-Download-Host ist im Proxy
#     gesperrt, nuget.org nicht - siehe CLAUDE.md, "Environment constraint")
#   - NuGet-Pakete der ganzen Solution
#   - python3 fuer scripts/preflight.py und die Inhaltspruefungen
# Idempotent: ist alles schon da, laeuft nur ein schnelles "dotnet restore".
set -euo pipefail

# Nur in Cloud-Sitzungen - auf einem Entwickler-Rechner nichts installieren.
if [ "${CLAUDE_CODE_REMOTE:-}" != "true" ]; then
  exit 0
fi

cd "${CLAUDE_PROJECT_DIR:-$(dirname "$0")/../..}"

als_root() {
  if [ "$(id -u)" -eq 0 ]; then "$@"; else sudo "$@"; fi
}

apt_aktualisiert=false
paket_installieren() {
  if [ "$apt_aktualisiert" = false ]; then
    als_root apt-get update -qq
    apt_aktualisiert=true
  fi
  als_root env DEBIAN_FRONTEND=noninteractive apt-get install -y -qq "$@"
}

if ! command -v dotnet >/dev/null 2>&1 || ! dotnet --list-sdks | grep -q '^10\.'; then
  echo "LernTor: .NET 10 SDK fehlt - installiere dotnet-sdk-10.0 aus dem Ubuntu-Archiv."
  paket_installieren dotnet-sdk-10.0
fi

if ! command -v python3 >/dev/null 2>&1; then
  paket_installieren python3
fi

export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
if [ -n "${CLAUDE_ENV_FILE:-}" ]; then
  {
    echo 'export DOTNET_NOLOGO=1'
    echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1'
    echo 'export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1'
  } >> "$CLAUDE_ENV_FILE"
fi

dotnet restore LernTor.sln --verbosity quiet
echo "LernTor: .NET $(dotnet --version), Pakete wiederhergestellt - bauen mit 'dotnet build LernTor.sln -c Release'."
