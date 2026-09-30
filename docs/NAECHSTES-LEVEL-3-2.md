# LernTor 3.2: Rahmenlehrplan – Plan

Stand: 30.09.2026. Auftrag der Familie: **„Wichtig ist die Übernahme aus dem Rahmenlehrplan für
die Klassen“** (Einstieg: <https://www.berlin.de/sen/bildung/unterricht/faecher-rahmenlehrplaene/rahmenlehrplaene/>).
Vorher bestätigt: Testplan V.31 am echten PC geprüft, **alles richtig** (Vorlesen, Ferien
2027/28).

Es gelten die Regeln wie bei 3.0 und 3.1: jeder Schritt einzeln, lokal gebaut und getestet, von
der CI auf `windows-latest` bestätigt, der Plan liegt vor der Umsetzung auf GitHub und wird nach
jedem Schritt nachgeführt.

### Stand der Umsetzung

| Schritt | Stand |
|---|---|
| 1 — Rahmenlehrplan-Katalog im Code | ✅ 30.09.2026: `RahmenlehrplanKatalog` mit 186 Themenfeldern für 5/6 und 9/10 aus den bisherigen Checklisten, alle Zuordnungen per Test gegen die Generatoren geprüft; 3 offen, 2 teilweise, 25 bewusst nicht (Sport, Wahlpflicht, Praxis); Übersicht [`RAHMENLEHRPLAN.md`](RAHMENLEHRPLAN.md) wird erzeugt. **Für 7/8 und die Klasse-10-Prüfung fehlt der Zugriff auf Teil C** (Umgebung sperrt die Quelle) |
| 2 — Klasse 7/8 vollständig (Emirhan ab Sommer 2027) | ⏳ geplant |
| 3 — Klasse 9/10 vollständig (Batuhan, 2027/28 Klasse 10 mit MSA) | ⏳ geplant |
| 4 — Klasse 5/6: letzte Lücken | ⏳ geplant |
| 5 — Eltern-Bericht: Rahmenlehrplan je Fach | ⏳ geplant |
| 6 — Dokumentation, Version 3.2.0, Release | ⏳ geplant |

---

## Ausgangslage

- **`docs/CURRICULUM.md`** hat Checklisten für **Klasse 6 und 9**. Die Häkchen wurden damals von
  Hand aus dem Code abgeleitet, und kein Test prüft, ob sie noch stimmen.
- **Klasse 7/8 hat keine Checkliste.** Die Klasse-7-Pools wurden am 30.09.2026 ausgebaut, aber nicht
  Themenfeld für Themenfeld gegen den Rahmenlehrplan abgeglichen. Emirhan kommt im Sommer 2027 in
  Klasse 7.
- **Klasse 10 nutzt die Klasse-9-Pools** (Doppeljahrgang 9/10). Batuhan kommt im Sommer 2027 in
  Klasse 10 und schreibt am Ende den MSA. Themenfelder, die der Rahmenlehrplan erst für Klasse 10
  vorsieht, sind nicht gesondert geprüft.
- Offen aus Klasse 5/6: NaWi 3.3 „Sonne als Energiequelle“ (Wasserkreislauf), 3.5 „Pflanzen – Tiere –
  Lebensräume“ (seit 3.1 teilweise), 3.7 „Körper und Gesundheit“ (seit 3.1 teilweise).

## Quelle

Rahmenlehrplan 1–10 Berlin/Brandenburg, **Teil C** je Fach (Themenfelder und Standards je
Doppeljahrgangsstufe), veröffentlicht auf dem Bildungsserver Berlin-Brandenburg und verlinkt von
der Seite der Senatsverwaltung.

**Einschränkung:** `www.berlin.de` und `bildungsserver.berlin-brandenburg.de` sind aus der
Entwicklungsumgebung gesperrt (Netzwerk-Einstellung der Umgebung). Bis sie freigegeben sind,
stammen die Themenfelder aus den Suchergebnissen genau dieser Seiten. Jeder Eintrag im Katalog
nennt seine Quelle; ein Eintrag, der noch nicht am Original geprüft ist, ist als solcher
markiert. Nach der Freigabe wird am Original abgeglichen.

Übernommen werden **Themenfelder und Inhalte**, keine Textpassagen (der Rahmenlehrplan ist
urheberrechtlich geschützt). Die Fragen formuliert LernTor selbst.

## Schritt 1 — Rahmenlehrplan-Katalog im Code

- **`RahmenlehrplanKatalog`** (ContentGen): Pro Fach und Doppeljahrgangsstufe (5/6, 7/8, 9/10)
  gibt es einen Eintrag für jedes Themenfeld. Jeder Eintrag hat eine Nummer, einen Titel, seine
  Quelle und seine Zuordnung zu den Themen der Generatoren. Hat ein Themenfeld noch keine
  Zuordnung, trägt es einen Grund („noch offen“ oder „nicht als Quiz prüfbar“ wie Sport).
- **Test `RahmenlehrplanAbdeckungTests`** mit drei Prüfungen:
  - Jede Zuordnung zeigt auf ein Thema, das der Generator für diese Klassenstufe wirklich
    liefert. Ein umbenanntes oder gelöschtes Thema macht die CI rot.
  - Jedes Fach mit Generator hat für jede Doppeljahrgangsstufe Einträge.
  - Die Zahl der offenen Themenfelder ist festgehalten und darf nur sinken.
- **`docs/RAHMENLEHRPLAN.md`**: die Übersicht je Fach und Stufe mit Abdeckung in Prozent. Sie
  ersetzt die Checklisten in `CURRICULUM.md`, die nur noch darauf verweist.

## Schritt 2 — Klasse 7/8 vollständig

Alle Fächer, die Emirhan in Klasse 7/8 haben wird: Deutsch, Mathematik, Englisch, Türkisch,
Geschichte, Geografie, Politische Bildung, Biologie, Chemie, Physik, Musik, Kunst und Ethik.
Jedes offene Themenfeld bekommt ein Thema mit etwa 20 Fragen; Mathe bekommt Aufgaben mit frischen
Zahlen. Es gelten dieselben Qualitätsregeln wie immer: Längen-Bias unter 60 % (Ziel etwa 35 %), der
Schrotflinten-Test für offene Antworten und Klassenstufe in der Frage (`themen-stufe`).

## Schritt 3 — Klasse 9/10 vollständig

Wie Schritt 2 für die Themenfelder der Doppeljahrgangsstufe 9/10. Themenfelder, die erst in
Klasse 10 vorkommen und MSA-relevant sind, kommen zuerst. Klasse 10 bleibt bei den 9er-Pools
(Übergangsregel); der Katalog zeigt, dass sie vollständig sind.

## Schritt 4 — Klasse 5/6: letzte Lücken

- NaWi 3.3 „Sonne als Energiequelle“ mit Wasserkreislauf.
- Die Reste von 3.5 und 3.7: Winterschlaf und Frühblüher sowie Suchtprävention ausdrücklich für 5/6.

## Schritt 5 — Eltern-Bericht: Rahmenlehrplan je Fach

Im Eltern-Bericht steht je Fach, wie viele Themenfelder der Klassenstufe des Kindes LernTor abdeckt
und wie viele davon das Kind schon geübt hat, zum Beispiel „Biologie 7/8: 6 von 6 Themenfeldern,
davon 4 geübt“. Damit sehen Eltern, ob LernTor zum Unterricht passt.

## Schritt 6 — Dokumentation, Version 3.2.0, Release

`README.md`, `CLAUDE.md` (der Katalog ist Pflicht bei neuen Themen), `CURRICULUM.md`,
`NAECHSTE-SCHRITTE.md`, `TESTPLAN.md`, dieser Plan. Version 3.2.0, Release nach grüner CI.

## Bewusst nicht

- **Sport** und rein praktische Inhalte (Kunst „Verfahren und Werkzeuge“, Informatik
  „Standardsoftware“ am Rechner) lassen sich nicht als Quizfrage prüfen. Der Katalog führt sie
  mit diesem Grund, damit sie nicht als vergessen gelten.
- **Wahlpflichtfächer** (WAT, NaWi-WP) stehen auf keinem der beiden Stundenpläne. Sie bleiben
  außen vor, bis die Familie sie braucht.
- **Keine Textübernahme** aus dem Rahmenlehrplan.
