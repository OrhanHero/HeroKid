# Notfall & Wiederherstellung

Diese Seite ist für den Fall gedacht, dass etwas schiefgeht und niemand da ist, der es aus dem
Code herauslesen kann. Sie beschreibt, wo die Daten liegen, wie man eine Sicherung einspielt und
was zu tun ist, wenn LernTor gar nicht mehr startet.

## Wo liegt was?

Alles liegt im Benutzerprofil des **Kind-Kontos** unter `%LOCALAPPDATA%\LernTor\`, also meist
`C:\Users\<Kind>\AppData\Local\LernTor\`:

| Pfad | Inhalt |
|---|---|
| `lerntor.db` | **Die** Datenbank: Profile, Sterne, Fortschritt, Fehler-Kartei, Stundenpläne, Einstellungen |
| `sicherungen\lerntor-auto-JJJJ-MM-TT-HHMMSS-taeglich.db` | automatische Sicherung, einmal täglich beim ersten Start |
| `sicherungen\lerntor-auto-…-schema.db` | automatische Sicherung **vor** einem Datenbank-Umbau nach einem Update |
| `logs\lerntor-JJJJ-MM-TT.log` | Fehlerprotokoll (auch im Eltern-Bereich lesbar) |
| `crash-restarts.txt` | nur vorhanden, wenn die App abgestürzt ist und sich selbst neu gestartet hat |
| `models\` | heruntergeladenes KI-Modell (mehrere GB; darf gelöscht werden, lädt neu) |

Das Programm selbst liegt unter `C:\Program Files\LernTor\`. Der Autostart ist eine geplante
Aufgabe namens **„LernTor Autostart“** (Aufgabenplanung).

Von den automatischen Sicherungen bleiben die letzten **fünf** liegen, dazu immer die neueste
Schema-Sicherung. **Sie liegen auf derselben Festplatte.** Einen Plattendefekt überleben sie
nicht, dafür ist die Sicherung von Hand auf einen USB-Stick da (siehe unten).

## Regelmäßig: Sicherung auf USB-Stick

Eltern-Bereich (Zahnrad unten rechts) → Abschnitt mit dem Fehlerprotokoll → **„Sicherung erstellen…“**
→ Datei auf den USB-Stick speichern. Die Datei ist eine vollständige Kopie, auch wenn die App
gerade läuft. Empfehlung: einmal im Monat und vor jedem größeren Update.

## Datenbank prüfen

Gleicher Abschnitt → **„🔍 Datenbank prüfen“**. Das Ergebnis steht direkt darunter:

- **✅ in Ordnung**: nichts zu tun.
- **⚠️ beschädigt**: die Datei repariert sich nicht von selbst. Weiter mit „Sicherung einspielen“.

Typische Anzeichen einer beschädigten Datenbank sind scheinbar zufällige Abstürze, Profile, die
fehlen, oder eine Meldung wie „database disk image is malformed“ im Fehlerprotokoll.

## Sicherung einspielen (die App startet noch)

1. Eltern-Bereich → **„Sicherung wiederherstellen…“**.
2. Datei wählen. Das ist entweder die vom USB-Stick oder eine automatische aus
   `…\LernTor\sicherungen\` (der Knopf **„Ordner öffnen“** führt direkt dorthin). Für eine
   beschädigte Datenbank nimmt man die **neueste** Sicherung, nach einem missglückten Update die
   neueste **`-schema.db`**.
3. Bestätigen. LernTor beendet sich und startet beim nächsten Login mit den Daten aus der
   Sicherung. Alles, was **nach** der Sicherung entstanden ist, geht dabei verloren.

Eine Sicherung aus einer älteren LernTor-Version ist kein Problem: fehlende Tabellen und Spalten
ergänzt die App beim Start selbst. Das ist durch `BackupRestoreTests` abgesichert.

## Die App startet gar nicht mehr

LernTor hat dafür einen eingebauten Rückfall: stürzt es beim Start **dreimal innerhalb von zehn
Minuten** ab, startet es sich nicht mehr neu, und der normale Desktop bleibt erreichbar. Ein
Kind ist also nie in einem schwarzen Bildschirm gefangen. Dann:

1. **Mit dem Kind-Konto anmelden** (oder dort bleiben, wenn der Desktop schon da ist).
2. Den Explorer öffnen und `%LOCALAPPDATA%\LernTor\` in die Adresszeile eingeben.
3. `lerntor.db` **umbenennen** (nicht löschen!), z. B. in `lerntor-kaputt.db`.
4. Die neueste Datei aus `sicherungen\` **kopieren** und die Kopie in `lerntor.db` umbenennen.
5. Abmelden und wieder anmelden. LernTor startet per Autostart.

Hilft das nicht, weil die App auch mit einer frischen Datenbank abstürzt, liegt es am Programm
und nicht an den Daten. Die Datenbank bleibt dann liegen, und es hilft, das Programm neu zu
installieren oder eine ältere Version einzuspielen. Der Grund steht im neuesten Log unter
`logs\`, und `crash-restarts.txt` zeigt, wann die Abstürze waren.

## LernTor vorübergehend aus dem Weg räumen

- **Für heute:** Eltern-Bereich → **„Sofort freischalten (überspringen)“**.
- **Für mehrere Tage (Urlaub, Krankheit):** Eltern-Bereich → **„Ferien- / Pausenmodus“** mit Enddatum.
- **Ganz abschalten, ohne zu deinstallieren:** Aufgabenplanung öffnen → Aufgabe
  „LernTor Autostart“ → Deaktivieren.
- **Task-Manager oder Windows-Taste bleiben gesperrt**, obwohl LernTor nicht läuft (sollte nicht
  vorkommen, die App gibt beides beim Entsperren frei): Registrierungs-Editor → unter
  `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System` den Wert
  `DisableTaskMgr` löschen, unter `…\Policies\Explorer` den Wert `NoWinKeys` löschen, dann
  ab- und wieder anmelden.

## Werkseinstellungen

Eltern-Bereich → Gefahrenzone → **„Alle Daten zurücksetzen…“** löscht **alles**: beide Profile,
Sterne, Fortschritt, Stundenpläne, Führerschein-Stand und Einstellungen. Vorher also unbedingt
eine Sicherung erstellen. (Bis zum 28.09.2026 blieben dabei sechs Tabellen versehentlich stehen,
unter anderem die Stundenpläne und der Führerschein-Stand. Das ist behoben.)

## Für Entwickler

- App starten, ohne dass sie den Rechner sperrt: `LERNTOR_SKIP_LOCK=1` setzen (siehe
  [`BUILD.md`](BUILD.md)). Fehler erscheinen dann als Meldungsfenster statt still im Log.
- Das Datenbankschema wird beim Start nur **ergänzt** (`SqliteSchemaUpdater`). Umbenennen oder
  Löschen von Spalten kann es nicht; in dem Fall ist die `-schema.db`-Sicherung der Rückweg.
