# Pilotwoche & Bypass-Härtetest (Checkliste für den echten Kiosk-PC)

Der Engpass der App ist nicht mehr Code, sondern Realitätskontakt: die App wurde in einer
Linux-Sandbox entwickelt und via CI gebaut, aber nie über längere Zeit im Realbetrieb erprobt.
Diese Checkliste deckt die beiden Schritte ab, die nur auf dem Ziel-PC möglich sind.

## A. Installation

- [ ] CI-Artefakt oder lokalen `dotnet publish`-Build (siehe [BUILD.md](BUILD.md)) mit
      `iscc src\LernTor.Installer\setup.iss` zum Installer bauen und installieren.
- [ ] Nach der Installation einmal ab- und wieder anmelden: startet LernTor automatisch?
- [ ] Eltern-Passwort setzen (Zahnrad unten rechts), Fächer/Schwierigkeitsstufen je Profil prüfen.
- [ ] **Sofort eine Sicherung erstellen** (Eltern-Bereich → Sicherung) und extern ablegen.

## B. Pilotwoche (täglich ~2 Minuten)

- [ ] Fehlerprotokoll im Eltern-Bereich lesen: übersprungene Feeds? TTS-Rückfälle? Abstürze?
- [ ] Lief der komplette Ablauf (Lesen → Tippen → News → Fächer → Quiz) ohne Hänger durch?
- [ ] Wie lange braucht der Kaltstart nach dem Login? (ReadyToRun sollte spürbar helfen)
- [ ] RAM im Blick behalten, sobald der KI-Chat/das LLM benutzt wurde (Task-Manager als Admin).
- [ ] Fragen die Kinder Dinge wie "warum kommt die Frage schon wieder"? (Pool-Ermüdung notieren)
- [ ] Ende der Woche: `%LOCALAPPDATA%\LernTor\crash-restarts.txt` vorhanden? Wenn ja, gab es
      stille Auto-Neustarts - Ursache im Fehlerprotokoll suchen.

## C. Bypass-Härtetest (bewusst aus Kindersicht denken, 10-15 Jahre)

Soft-Kiosk heißt: Absturz ⇒ Desktop bleibt erreichbar. Diese Wege sollten trotzdem einmal
durchgespielt werden:

| Angriffsweg | Erwartetes Verhalten | Geprüft |
|---|---|---|
| Windows-Taste, Alt+Tab, Alt+Esc, Strg+Esc, Alt+F4 | verschluckt (Keyboard-Hook) | [ ] |
| Strg+Shift+Esc (Task-Manager direkt) | Task-Manager blockiert (`DisableTaskMgr`) - **prüfen, ob der Registry-Write auf diesem PC durchging** (Fehlerprotokoll) | [ ] |
| Strg+Alt+Entf → Task-Manager | blockiert via Policy; Sperren/Abmelden bleibt möglich (OS-Schutz, nicht abfangbar) | [ ] |
| Strg+Alt+Entf → Abmelden → neu anmelden | LernTor startet per Autostart erneut | [ ] |
| **Zweites Windows-Konto / "Benutzer wechseln"** | größtes Scheunentor - es darf nur EIN Kind-Standardkonto ohne Adminrechte existieren | [ ] |
| Abgesicherter Modus (Shift+Neustart) | nicht verhinderbar ohne Hard-Lock; braucht Admin-/BIOS-Hürden, bewusst akzeptiertes Restrisiko | [ ] |
| Hartes Ausschalten + Neustart | LernTor startet wieder, Fortschritt des Tages ist erhalten | [ ] |
| App zum Absturz bringen (falls gelungen: wie?) | stiller Auto-Neustart, max. 3× in 10 min, danach Desktop (Soft-Lock-Prinzip) | [ ] |

**Flankierende Windows-Bordmittel (kosten nichts, schließen die größten Wege):**

- Kind-Konto als Standardbenutzer (nie Administrator), keine weiteren Konten sichtbar.
- BIOS/UEFI-Passwort + Bootreihenfolge fixieren (kein Boot vom USB-Stick).
- Optional als zweite Schicht: Microsoft Family Safety für Bildschirmzeit außerhalb von LernTor.

## D. Nach der Pilotwoche

- [ ] Gefundene Probleme als Liste sammeln (Fehlerprotokoll-Auszüge genügen) und in die
      Entwicklung zurückspielen.
- [ ] Wenn stabil: Entscheidung über die letzten offenen Punkte aus dem
      [STATUS-REPORT](STATUS-REPORT.md) - Installer-Signing (EV-Zertifikat) und ggf. eine
      Update-Strategie (Auto-Update wäre der erste Cloud-Call der App → bewusste Entscheidung).
