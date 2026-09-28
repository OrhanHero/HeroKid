# Stundenpläne 2026/27 – zum Einfügen im Eltern-Bereich

Abgeschrieben aus den eingescannten Plänen (Batuhan: Untis-Ausdruck vom 24.08.2026, Emirhan:
Stand 27.08.2026). **Eintragen:** Eltern-Bereich → Stundenplan → Profil wählen → Text einfügen →
Zeitraster wie unten anpassen → speichern.

Die Texte sind wortgleich in `tests/LernTor.Tests/TimetableSubjectPlannerTests.cs` hinterlegt.
Ein Test prüft dort, dass sie sich ohne Warnung einlesen lassen und die richtigen Fächer ergeben.

## Batuhan – Klasse 9a, Robert-Koch-Gymnasium

```
Mo: 1 Ch (A301), 2 Ch (A301), 3 De (A303), 4 Ku (O003), 5 Ku (O003), 6 Türkisch, 7 Ge (A201)
Di: 1 Türkisch, 2 Ma (A012), 3 De (A304), 4 E (A204), 5 PB (A201), 6 Wge, 7 Wge
Mi: 1 Ph (A109), 2 Ph (A109), 3 Türkisch, 4 E (A102), 5 Ma (A012), 6 Bi (A209), 7 Bi (A209)
Do: 1 Sp (TH1), 2 Sp (TH1), 3 Geo (A202), 4 De (A304), 5 De (A304), 6 Et (A102)
Fr: 1 Wge, 2 Geo (A202), 3 E (A204), 4 Ge (A103), 5 Sp (TH1), 6 Ma (A012), 7 Ma (A012)
```

| Stunde | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
|---|---|---|---|---|---|---|---|---|---|
| Beginn | 8:10 | 9:00 | 9:50 | 11:00 | 11:50 | 13:00 | 13:50 | 14:45 | 15:30 |
| Ende | 8:55 | 9:45 | 10:35 | 11:45 | 12:35 | 13:45 | 14:35 | 15:30 | 16:15 |

- „.F“ auf dem Plan ist die geteilte Gruppe Türkisch/Französisch. Batuhan ist in der
  **Türkisch**-Gruppe, deshalb steht dort „Türkisch“. Der Raum fehlt, weil der Plan für beide
  Gruppen zwei Räume nennt (A103/A105 bzw. A103/A207).
- „Wge“ ist ebenfalls ein geteilter Kurs (A206/A209). LernTor kennt dafür kein Fach, er wird nur
  angezeigt.
- **Bitte am Original nachprüfen:** Ethik steht nur in Do 6 (die Zelle darunter wirkt leer), und
  Mathe am Freitag ist als Doppelstunde 6/7 gelesen.

## Emirhan – Klasse 6c

```
Mo: 1 GeWi, 2 GeWi, 3 En, 4 En, 6 Sport, 7 Sport, 8 Orchester, 9 Orchester
Di: 1 NaWi, 2 NaWi, 3 Ma, 4 Mu, 6 De, 7 De, 8 MUBet INS
Mi: 1 Ku, 2 Ku, 3 Ma, 4 Mu, 6 NaWi, 7 NaWi, 8 WPU/JüM/Pop-Chor, 9 WPU/Pop-Chor
Do: 1 Ma, 2 Ma, 3 Klassenrat, 4 GeWi, 6 Sport, 7 En, 8 Religion
Fr: 2 En, 3 Ma, 4 De, 6 De, 7 En, 8 MUBet, 9 MUBet, 10 MUBet
```

| Stunde | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
|---|---|---|---|---|---|---|---|---|---|---|
| Beginn | 8:00 | 8:50 | 9:55 | 10:45 | 11:30 | 12:15 | 13:00 | 13:45 | 14:30 | 15:15 |
| Ende | 8:45 | 9:35 | 10:40 | 11:30 | 12:15 | 13:00 | 13:45 | 14:30 | 15:15 | 16:00 |

- Die 5. Stunde ist an allen Tagen frei (Mittagspause).
- Lehrkräfte stehen auf dem Plan (YaDu, ScRa, HaMa, SoNi, WuCh, PeCh). Die Texteingabe kennt nur
  Räume, Lehrkräfte lassen sich bei Bedarf im Raster nachtragen.
- „KR“ ist als Klassenrat gelesen.

## Was LernTor daraus macht

Geübt werden die Fächer des **nächsten Schultags**, dazu immer **Türkisch**
(`TimetableSubjectPlanner`). Zum Beispiel an einem Montag:

| Kind | Montag wird geübt für Dienstag |
|---|---|
| Batuhan | Mathematik, Deutsch, Türkisch, Englisch, Politik |
| Emirhan | Mathematik, Deutsch, Türkisch, Musik + ein NaWi-Fach (Bio → Chemie → Physik reihum) |

Nicht zugeordnet und deshalb nur angezeigt: Sport, Wge, Orchester, MUBet, WPU/Chor, Klassenrat,
Religion. Wer die Auswahl für ein Kind nicht will, schaltet sie im Eltern-Bereich ab („Fächer
des Tages nach dem Stundenplan auswählen“). Dann kommen wieder alle Fächer jeden Tag dran.
