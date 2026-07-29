---
name: push-check
description: Änderungen absichern und veröffentlichen - lokale Vorab-Prüfung, Commit, Push und CI-Verifikation auf windows-latest. Nutzen, sobald Codeänderungen an LernTor fertig sind und veröffentlicht werden sollen, oder wenn nach einem Push das CI-Ergebnis geprüft werden muss.
---

# Änderungen absichern und veröffentlichen

Diese Codebasis lässt sich lokal **nicht kompilieren** (Linux-Sandbox ohne .NET SDK, WPF/Win32
brauchen Windows). Die GitHub-Action auf `windows-latest` ist die einzige echte Verifikation -
pro Runde ~8 Minuten. Deshalb gilt: erst lokal prüfen, was prüfbar ist, dann pushen.

## 1. Vorab-Prüfung (immer, vor jedem Commit)

```bash
python3 scripts/preflight.py
```

Prüft die real aufgetretenen Fehlerklassen dieser Codebasis (Klammerbalance, XAML-Wohlgeformtheit,
`Run.Text` ohne `Mode=OneWay`, fehlendes `using System.Net.Http;` in LernTor.App,
`Shutdown()` ohne vorheriges `Unlock()`, Subject-Verdrahtung, EF-Sortierung auf DateTimeOffset,
JSON/YAML-Gültigkeit). Exit-Code 1 = vor dem Push beheben.

Wurden Fragenpools verändert, zusätzlich:

```bash
python3 scripts/check-answer-length-bias.py    # CI-Gate: max. 60% je Generator
```

## 2. Commit und Push

`docs/CLAUDE_STATE.json` mit dem erledigten Schritt aktualisieren (oberster Eintrag in `done`),
dann committen. `scripts/__pycache__` nie mitcommitten:

```bash
git add -A ':!scripts/__pycache__'
git commit -m "<Aussage über das Verhalten, nicht über die Dateien>"
git push -u origin master
```

Commit-Nachrichten beschreiben, **was sich für Nutzer ändert und warum** - bei Bugfixes gehört
die Fehlerursache dazu, damit sie später auffindbar bleibt.

## 3. CI verifizieren

`mcp__github__actions_list` überschreitet bei diesem Repo **immer** das Token-Limit; das Ergebnis
landet automatisch in einer Datei. Deshalb direkt so vorgehen:

```
mcp__github__actions_list(
  method="list_workflow_runs", owner="OrhanHero", repo="HeroKid",
  resource_id="build.yml", per_page=3, workflow_runs_filter={"branch": "master"})
```

Danach die gemeldete Datei parsen:

```bash
python3 -c "
import json
data = json.load(open('<gemeldeter Pfad>'))
for r in data['workflow_runs'][:3]:
    print(r['head_sha'][:7], r['status'], r['conclusion'], r['id'])
"
```

Bei `failure`: `mcp__github__get_job_logs(owner, repo, run_id=<id>, failed_only=true,
return_content=true, tail_lines=200)`, Ursache beheben, erneut pushen und verifizieren.

Der Lauf dauert ~6-8 Minuten. Für die Wartezeit `ScheduleWakeup` mit ~480 Sekunden nutzen und im
Prompt festhalten, welcher Commit geprüft wird und was bei Fehlschlag die wahrscheinlichen
Ursachen sind.
