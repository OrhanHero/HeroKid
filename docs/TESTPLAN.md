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

---

## Neu seit 28.09.2026

Acht Punkte für die Funktionen aus Pull Request #1. Sie laufen alle durch die CI, gesehen hat
sie aber noch niemand. **Vorher eine Sicherung auf USB.**

| # | Rolle | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|---|
| N.1 | Elternteil | Stundenplan beider Kinder aus `STUNDENPLAENE-2026-27.md` einfügen, Zeitraster anpassen, speichern | Keine Warnung beim Einlesen; die Startseite zeigt den richtigen Tag mit den richtigen Uhrzeiten | |
| N.2 | Kind | Startseite ansehen | Unter dem Stundenplan steht „Heute übst du für <Tag>: …“ mit den Fächern des nächsten Schultags plus Türkisch | |
| N.3 | Kind | Tag durchspielen | Es kommen **nur** diese Fächer; der Zähler „Fächer x/y“ geht bis zum Ende auf; das Abschlussquiz fragt nur diese Fächer | |
| N.4 | Emirhan | An zwei NaWi-Tagen hintereinander (Mo für Di, Di für Mi) | Zwei **verschiedene** Fächer aus Bio/Chemie/Physik | |
| N.5 | Elternteil | Schalter „Fächer des Tages nach dem Stundenplan auswählen“ aus, Eltern-Bereich schließen | Ab der nächsten Etappe kommen wieder alle Fächer | |
| N.6 | Elternteil | „🔍 Datenbank prüfen“ | ✅ mit Datum und Uhrzeit | |
| N.7 | Elternteil | Klassenstufe eines Kindes testweise ändern und zurückstellen | Rückfrage, dann „✅ Gespeichert“; Sterne und Stundenplan sind danach noch da | |
| N.8 | Notfall | Eine automatische Sicherung aus `…\LernTor\sicherungen\` über „Sicherung wiederherstellen…“ einspielen | App beendet sich; nach dem Neustart ist alles da, was vor der Sicherung da war | |

## Neu seit 29.09.2026 (Version 2.0)

Zehn Punkte für .NET 10 und die neuen Funktionen aus [`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md).
Alles läuft durch die CI (auch die neuen Ansichten werden dort mit Beispieldaten gerendert),
**gesehen hat es noch niemand**. **Vorher eine Sicherung auf USB** – das Update legt eine neue
Tabelle an (die automatische `-schema.db`-Sicherung entsteht trotzdem von selbst).

| # | Rolle | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|---|
| V.1 | Elternteil | Neue Version über die alte entpacken, starten | Startet wie vorher; Profile, Sterne, Stundenpläne sind da | |
| V.2 | Elternteil | **Eltern-Bereich mit dem bisherigen Passwort öffnen** | Geht beim ersten Mal (dauert kaum merklich länger), beim zweiten Mal ebenso – das Passwort wurde still auf die neue Stärke umgestellt | |
| V.3 | Elternteil | Eltern-Bereich → Systeminfo | „LernTor 2.0.0 (…)“, „.NET 10.0.…“, Datenbankgröße, letzte Sicherung; „📋 Kopieren“ legt den Text in die Zwischenablage | |
| V.4 | Kind | Startseite: Zeile „🔁 Aus deiner Fehler-Kartei heute dran: N“ | N stimmt mit den 🔁-Fragen überein, die heute wirklich kommen (höchstens 3 je Fach, nur Fächer des Tages) | |
| V.5 | Kind | „🏆 Mein Fortschritt“ auf der Startseite | Abzeichen oben, darunter die Fächer mit Themen und Stufe (🌱/📘/✅/🏆); **alles lesbar, nichts abgeschnitten** | |
| V.6 | Kind | Mitten am Tag Planer öffnen → „🏆 Mein Fortschritt“ → Zurück → „Zurück zum Lernen“ | Man landet in derselben, halb beantworteten Aufgabe; die Mindestzeit ist nicht weitergelaufen | |
| V.7 | Kind | Abschlussquiz bestehen | Falls ein Abzeichen dazugekommen ist: goldene Zeile „🏅 Neues Abzeichen: …“; der Knopf „PC jetzt benutzen“ ist **ohne Scrollen** sichtbar (auch auf einem kleinen Bildschirm) | |
| V.8 | Elternteil | Führerschein-Bereich für ein Kind abschalten, „Mein Fortschritt“ ansehen | Die offenen Führerschein-Abzeichen sind verschwunden; schon verdiente bleiben | |
| V.9 | Elternteil | Bericht → „🏆 Meisterschaft je Fach“ und HTML-Export | Dieselben Zahlen wie beim Kind in „Mein Fortschritt“ | |
| V.10 | Emirhan / Batuhan | Englisch- bzw. Musikaufgaben der neuen Themen (Simple Past, Pronomen, in/on/at, Notenwerte, Stimme; Second Conditional, Relative Clauses) | Fragen und Erklärungen stimmen – **bitte von jemandem mit gutem Englisch gegenlesen lassen** | |

## Neu seit 30.09.2026 (Version 2.1 und 3.0)

Version 2.1 prüft offene Antworten strenger und korrigiert die binomischen Formeln. Version 3.0
bringt wählbare Designs ([`DESIGN.md`](DESIGN.md)). Die CI rendert jede Ansicht in jedem Design als
Bild (Artefakt „Design-Vorschau“), **am echten Bildschirm gesehen hat es noch niemand**.
**Vorher eine Sicherung auf USB** – 3.0 ergänzt fünf Spalten in der Profiltabelle.

| # | Rolle | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|---|
| V.2-1 | Batuhan | Mathe: „(x + 3)² = ?“ mit „x² + 6x + 9“ beantworten (auch als „x^2+6x+9“) | Richtig. Eine alte 🔁-Karte zu den binomischen Formeln zeigt jetzt die richtige Lösung | |
| V.2-2 | Kind | Offene Aufgabe: zwei Formen hintereinander tippen („gelse gelir“) | Falsch – Raten mit mehreren Antworten zählt nicht mehr | |
| V.2-3 | Kind | Offene Aufgabe mit Zahl: „42 €“ oder „x = 9“ eintippen | Richtig; „142“ für „42“ dagegen falsch | |
| V.2-4 | Emirhan | Deutsch „Groß- und Kleinschreibung“: „auto“ statt „Auto“ | Falsch – genau das wird dort geübt | |
| V.3-1 | Kind | Startseite → „🎨 Mein Design“ → „🌊 Ozean“ | Die ganze App wird sofort blau-türkis; „✔ Fertig“ führt zur Startseite zurück | |
| V.3-2 | Kind | In der Galerie mehrmals zwischen hellen und dunklen Designs wechseln | **Jede** Stelle der Galerie wechselt mit (Überschriften, Karten, Knöpfe, Häkchen-Beschriftungen); nichts bleibt im alten Design stehen | |
| V.3-3 | Kind | „🌙 Nacht“ wählen, einmal durch Lesen, News, eine Übung und das Abschlussquiz | Alles lesbar; keine weiße Fläche mit weißer Schrift, keine schwarze Schrift auf Dunkel | |
| V.3-4 | Kind | Schrift „Verspielt“ und Textgröße 120 % | Größer und in Comic-Schrift; auf dem kleinsten Bildschirm ist nichts abgeschnitten, Scrollen ist möglich | |
| V.3-5 | Kind | „🌌 Galaxie“ vor zehn Lerntagen | Blass mit 🔒 und dem Hinweis auf das Abzeichen „📅 Zehn Lerntage“; lässt sich nicht wählen | |
| V.3-6 | Kind | „Abends ab 19 Uhr dunkel“ einschalten, nach 19 Uhr eine Etappe weiter | Wechselt zu „Nacht“ (spätestens beim nächsten Etappenwechsel) | |
| V.3-7 | Elternteil | Eltern-Bereich öffnen, während ein Kind „Nacht“ gewählt hat | Eltern-Bereich ist hell (Standard); beim Profil steht „🌙 Nacht · …“; „Auf Standard zurücksetzen“ stellt nach dem Schließen wieder Lavendel her | |
| V.3-8 | Beide Kinder | Profilwahl nach dem Abmelden eines Kindes mit eigenem Design | Profilwahl ist wieder im Standard-Design | |

## Neu seit 30.09.2026 (Version 3.1)

Version 3.1 bringt Funktionen, die man hören und sehen muss ([`NAECHSTES-LEVEL-3-1.md`](NAECHSTES-LEVEL-3-1.md)).
Für das Vorlesen sollten im Eltern-Bereich die **natürlichen Vorlesestimmen** installiert sein –
ohne sie gibt es den 🔊-Knopf nur für Sprachen, für die Windows eine Stimme hat.

| # | Rolle | Was | Erwartung | ✓ / Beobachtung |
|---|---|---|---|---|
| V.31-1 | Elternteil | Eltern-Bereich öffnen, Passwort-Fenster ansehen | Beide Knöpfe ganz sichtbar, keiner abgeschnitten | |
| V.31-2 | Elternteil | Im Eltern-Bereich links im Inhaltsverzeichnis „Bericht“, dann „Gefahrenzone“ anklicken | Die Seite springt jeweils zur Überschrift | |
| V.31-3 | Elternteil | „Datenbank prüfen“, „Erste Hilfe“, Klassenstufe, Belohnungen ansehen | 🔍 und 🚑 statt Kästchen, „Klasse 10“, beschriftete Felder | |
| V.31-4 | Emirhan | Englisch-Übung: 🔊 neben der Frage drücken | Die deutsche Anweisung klingt deutsch, der englische Satz englisch, dann die Möglichkeiten | |
| V.31-5 | Emirhan | Nach dem Antworten „🔊 Richtige Antwort anhören“ | Die Lösung wird englisch vorgelesen | |
| V.31-6 | Batuhan | Türkisch-Übung: 🔊 drücken, dann gleich „Weiter“ | Türkische Stimme; beim Weiterblättern hört das Vorlesen auf | |
| V.31-7 | Batuhan | Mathe-Aufgabe mit Bruch vorlesen lassen | „drei durch vier“, kein Datum | |
| V.31-8 | Beide Kinder | „🏆 Mein Fortschritt“ öffnen | Oben „📅 Deine Lerntage“: gelernte Tage grün mit Häkchen, Wochenenden und Ferien nur umrandet, heute markiert; die Zahl der Lerntage passt zum Abzeichen „Zehn Lerntage“ | |
| V.31-9 | Beide Kinder | „🎨 Mein Design“ öffnen | Zehn Designs; Galaxie, Gletscher und Vulkan tragen ein 🔒 mit dem Abzeichen, das sie freischaltet (sofern noch nicht verdient) | |

