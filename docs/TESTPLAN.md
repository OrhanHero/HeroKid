# Testprotokoll LernTor — nach Rollen

Stand: 07.08.2026. **Reine Planung.** Dieses Dokument wird beim Testen abgehakt, nicht vorher.

Der wichtigste Satz über den Stand der App: **kein Mensch hat sie je von Anfang bis Ende
durchgespielt.** Alles, was bisher gefunden wurde — das falsche STOP-Schild, das doppelte „STOP",
die unbrauchbaren vektorisierten Zeichen, das „TR" statt der türkischen Flagge — kam aus dem
Hinsehen, nicht aus Tests. 601 Testmethoden und 16 statische Prüfungen haben **keinen einzigen**
davon gefunden. Das ist keine Kritik an den Tests; es ist der Grund für dieses Dokument.

## Wie es benutzt wird

Nach **Rollen** geordnet, nicht nach Bildschirmen. Jede Rolle hat eine andere Absicht, und Fehler
zeigen sich an der Absicht, nicht am Knopf. Wer als „Kind, das durch will" testet, findet andere
Dinge als wer als „Elternteil, das etwas einstellen will" testet — und beide finden nichts von
dem, was auffällt, wenn man als „Kind, das rauswill" testet.

**Regel für alle Rollen:** Wenn etwas komisch ist, wird es aufgeschrieben — auch wenn es „bestimmt
so gedacht" ist. Genau die Beobachtungen sind wertvoll, bei denen man unsicher ist.

**Vorbereitung** (einmal, vor allem anderen):
- [ ] `%LOCALAPPDATA%\LernTor\lerntor.db` an einen sicheren Ort kopieren
- [ ] Notieren: Datum, Uhrzeit, welche Version (Commit aus dem ZIP-Namen)
- [ ] Ein Blatt Papier oder eine Datei für Beobachtungen — nicht im Kopf behalten

---

## Rolle 1 — Emirhan (Klasse 6, ~12)

**Absicht:** Ich will meinen Tag machen und dann an den PC.

| # | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|
| 1.1 | Profil auswählen | Beide Namen stehen da, meiner ist anklickbar | |
| 1.2 | Startseite ansehen | Links Stundenplan, darunter Hausaufgaben; rechts Ferien und Klausuren | |
| 1.3 | **Stundenplan lesen** | Der Tag stimmt. Die Uhrzeiten sind MEINE Schulzeiten, nicht die meines Bruders | |
| 1.4 | Lesen | Text erscheint, „Weiter" ist erst nach der Mindestzeit da | |
| 1.5 | Tippen | Tastatur wird angezeigt, meine Genauigkeit wird gemessen | |
| 1.6 | Diktat | Die Stimme spricht wirklich (Piper oder Windows) | |
| 1.7 | News | Artikel laden. **Ohne Internet:** kommt der Vorrat von gestern? | |
| 1.8 | Je Fach 6 Aufgaben | Nach einer falschen Antwort kommt die Erklärung — und ich muss sie wegklicken | |
| 1.9 | Erste Hilfe | **Nie von einem Menschen gesehen.** Alle acht Themen einmal durch | |
| 1.10 | Abschlussquiz | 20 Fragen, Auswertung, Freischaltung | |
| 1.11 | Nach dem Bestehen | Sperre weg, Desktop da | |

**Fragen an Emirhan hinterher** (wichtiger als jedes Häkchen):
- Wie lange kam dir das vor? (Und dann: wie lange war es wirklich?)
- Was war das Langweiligste?
- Gab es etwas, das du nicht verstanden hast — nicht die Aufgabe, sondern die App?
- Hast du gemerkt, warum heute welche Fächer dran waren?

---

## Rolle 2 — Batuhan (Klasse 9, ~15)

**Absicht:** Dasselbe, plus Führerschein — und ich bin alt genug, um Schwächen zu bemerken.

| # | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|
| 2.1 | Stundenplan | **Meine** Zeiten, nicht die von Emirhan. Beide Profile nacheinander prüfen | |
| 2.2 | Verkehrszeichen-Challenge | Startet, Schilder werden erkennbar gezeichnet | |
| 2.3 | **Alle 77 Schilder ansehen** | Keins ist falsch, keins doppelt, keins leer | |
| 2.4 | Karteikarten | Vor/zurück, Sterne werden vergeben | |
| 2.5 | **Theoriefragen** | **Nie gesehen.** Mehrfachauswahl bedienbar? Wird „teilweise richtig" richtig gewertet? | |
| 2.6 | **Prüfungssimulation** | **Nie gesehen.** Läuft vollständig durch? Kommt eine Auswertung? | |
| 2.7 | **Theorie-Kurs, alle 14 Lektionen** | **Nie gesehen.** Lesbar? Kontrolle startbar? Sinnvolle Länge? | |
| 2.8 | Fach Türkisch | Reichen die Fragen, oder wiederholt sich schnell etwas? | |

**Zu 2.8 mit Zahl dahinter:** Türkisch hat **228 Frage-Tupel auf 24 Themen — rund 10 je Thema**,
Englisch 223 auf 22. Das sind die beiden dünnsten Fächer (Chemie: 446, Deutsch: 538). Wenn sich
irgendwo etwas wiederholt anfühlt, dann hier zuerst. Bitte gezielt darauf achten.

---

## Rolle 3 — Elternteil, das etwas einstellen will

**Absicht:** Ich will den Tag anpassen, ohne ihn kaputtzumachen.

| # | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|
| 3.1 | Passwort | Greift. Falsches Passwort wird abgewiesen | |
| 3.2 | **Stundenplan eintragen** | Raster ausfüllen, speichern, App neu starten → steht noch da | |
| 3.3 | Stundenplan-Zeiten | Für beide Kinder VERSCHIEDEN eintragen → beide Startseiten prüfen | |
| 3.4 | Stundenplan-Textimport | `Mo: 1 Deutsch, 2 Mathe` → füllt Raster. **Unsinn eintippen → wird gemeldet?** | |
| 3.5 | „Fächer leeren" | Leert Fächer, **lässt Uhrzeiten stehen** | |
| 3.6 | Fach abschalten | Beim nächsten Start ist die Etappe wirklich weg | |
| 3.7 | Führerschein/Erste Hilfe abschalten | Dito — und die Reihenfolge der anderen bleibt gleich | |
| 3.8 | Klausur eintragen | Liste enthält **nur Schulfächer**, „Türkisch" mit Umlaut | |
| 3.9 | Schwellen ändern | Quiz-Schwelle auf 90 % → Quiz mit 80 % wird abgewiesen | |
| 3.10 | Wochenbericht | Zeigt die Daten von heute | |
| 3.11 | Profil wechseln und zurück | Ungespeicherte Änderungen werden abgefragt, nicht verschluckt | |
| 3.12 | Ferienliste | Vollständig, richtig sortiert, „reicht bis"-Hinweis stimmt | |

---

## Rolle 4 — Kind, das rauswill

**Absicht:** Ich will an den PC, ohne zu lernen. **Nur mit einer zweiten Person am Rechner testen.**

Diese Rolle findet die Fehler, die niemand sonst findet — jeder bekannte Kiosk-Fehler dieses
Projekts wurde so gefunden, nicht durch Nachdenken.

| # | Versuch | Erwartung | ✓ / Beobachtung |
|---|---|---|---|
| 4.1 | Alt+Tab | Fenster kommt zurück | |
| 4.2 | **In der Alt+Tab-Vorschau auf das X klicken** | Fenster bleibt offen | |
| 4.3 | Windows-Taste allein | Nichts passiert | |
| 4.4 | **Win+Tab, dann neuer Desktop** | Kommt man auf einen leeren Desktop? | |
| 4.5 | Win+D, Win+E, Win+R, Win+X, Win+I | Nichts passiert | |
| 4.6 | Win+Strg+Links/Rechts | Kein Desktopwechsel | |
| 4.7 | Strg+Esc, Alt+Esc, Alt+F4 | Nichts passiert | |
| 4.8 | Strg+Alt+Entf → Task-Manager | Gesperrt | |
| 4.9 | Auf die Taskleiste klicken | Fenster holt sich den Fokus zurück (max. 300 ms) | |
| 4.10 | Zweiten Bildschirm anstecken | Was passiert? **Ungeklärt** | |
| 4.11 | Eltern-Bereich → Passwort raten | Kein Hinweis auf das richtige Passwort | |
| 4.12 | Mitten in einer Aufgabe: PC ausschalten, neu starten | Fortschritt sinnvoll, keine Doppelanrechnung | |

---

## Rolle 5 — Der Notfall

**Absicht:** Etwas ist kaputt, ich muss retten. **Das ist der einzige Test, der wirklich weh tun
kann — deshalb steht die Sicherung ganz oben in der Vorbereitung.**

| # | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|
| 5.1 | Sicherung exportieren | Datei entsteht, größer als 0 Byte | |
| 5.2 | **Sicherung wieder einspielen** | Alles ist wieder da. **Dieser Pfad ist durch KEINEN Test abgedeckt** | |
| 5.3 | Datenbank löschen, App starten | Startet neu, keine Fehlermeldung, Profile neu anlegbar | |
| 5.4 | Alte Sicherung (älteres Schema) einspielen | Schema wird nachgezogen, App startet | |
| 5.5 | App bei laufendem Kiosk abstürzen lassen | Startet höchstens 3× in 10 min neu, dann Ruhe | |

---

## Was hinterher passiert

1. **Alle Beobachtungen sammeln** — auch die unsicheren.
2. **Sortieren nach:** „falsches Ergebnis" > „geht gar nicht" > „unschön".
3. Erst dann entscheiden, was gebaut wird. Nicht vorher.

Die Liste konkreter Beobachtungen aus einem echten Durchlauf ist mehr wert als jedes weitere
statische Audit — und mehr wert als jedes Modul, das noch dazukommen könnte.
