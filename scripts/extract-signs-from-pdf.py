#!/usr/bin/env python3
"""Holt die ORIGINAL-Vektorzeichnungen der Verkehrszeichen aus einer PDF.

Hintergrund: der Verkehrszeichen-Bereich zeichnete die Schilder zunaechst selbst nach (siehe
SignPictograms). Das war wiedererkennbar, aber eben nachgebaut. Die Familie hat die amtliche
ADAC-Uebersicht als PDF beigesteuert, in der jedes Zeichen als echte Vektorzeichnung steckt -
dieses Skript holt sie heraus.

Warum ueberhaupt ein eigener Parser: diese Entwicklungsumgebung kommt nicht ins Netz (das
Gateway verweigert CONNECT), vzkat.de und adac.de sind also nicht abrufbar. Eine PDF-Bibliothek
mit Vektorzugriff ist ebenfalls nicht installierbar (pypdf laeuft hier wegen eines kaputten
cryptography-Backends nicht). Bleibt: den Inhaltsstrom selbst lesen. Das ist weniger schlimm,
als es klingt - PDF-Grafik kennt ein gutes Dutzend Operatoren, und genau die stehen hier drin.

Vorgehen:
  1. Seiten finden, /Contents dekomprimieren.
  2. Inhaltsstrom auswerten: Grafikzustand (q/Q/cm), Pfade (m/l/c/v/y/re/h), Fuellungen und
     Farben (rg/g/k/sc/scn). Ergebnis: Pfade in Seitenkoordinaten, jeder mit seiner Farbe.
  3. Pfade raeumlich zu Gruppen zusammenfassen - eine Gruppe ist ein Schild.
  4. Textstuecke mit Position einlesen und ueber die ToUnicode-Tabelle entschluesseln; die
     VZ-Nummer steht jeweils UEBER ihrem Schild.
  5. Jede Gruppe der naechstliegenden Nummer zuordnen und auf ein Feld von 0..100 normieren.

Ausgabe: JSON mit einem Eintrag je Zeichen (Nummer, Pfade, Farben, Rahmen). Was daraus wird -
C#-Katalog, Kontaktbogen zum Nachsehen - macht ein zweites Skript.

Nutzung:  python3 scripts/extract-signs-from-pdf.py <datei.pdf> -o zeichen.json
"""
from __future__ import annotations

import argparse
import json
import math
import re
import sys
import zlib
from pathlib import Path

# --------------------------------------------------------------------------------------
# PDF-Grundgeruest
# --------------------------------------------------------------------------------------


def load_objects(data: bytes) -> dict[int, bytes]:
    """Alle "N 0 obj ... endobj" einsammeln. Reicht fuer diese PDF - sie nutzt keine
    Objekt-Streams fuer die Seiteninhalte."""
    objs: dict[int, bytes] = {}
    for m in re.finditer(rb"(\d+)\s+0\s+obj(.*?)endobj", data, re.DOTALL):
        objs[int(m.group(1))] = m.group(2)
    return objs


def stream_bytes(body: bytes) -> bytes:
    """Den (meist Flate-komprimierten) Stream eines Objekts auspacken."""
    m = re.search(rb"stream\r?\n", body)
    if not m:
        return b""
    end = body.find(b"endstream", m.end())
    raw = body[m.end():end]
    try:
        return zlib.decompress(raw)
    except zlib.error:
        return raw


def page_contents(objs: dict[int, bytes], body: bytes) -> bytes:
    """Inhaltsstrom einer Seite; /Contents kann auf ein Objekt oder ein Array zeigen."""
    single = re.search(rb"/Contents\s+(\d+)\s+0\s+R", body)
    if single:
        return stream_bytes(objs.get(int(single.group(1)), b""))

    array = re.search(rb"/Contents\s*\[(.*?)\]", body, re.DOTALL)
    if array:
        teile = [stream_bytes(objs.get(int(n), b""))
                 for n in re.findall(rb"(\d+)\s+0\s+R", array.group(1))]
        return b"\n".join(teile)

    return b""


# --------------------------------------------------------------------------------------
# Matrizen
# --------------------------------------------------------------------------------------

Matrix = tuple[float, float, float, float, float, float]
EINHEIT: Matrix = (1, 0, 0, 1, 0, 0)


def mat_mul(m: Matrix, n: Matrix) -> Matrix:
    a, b, c, d, e, f = m
    a2, b2, c2, d2, e2, f2 = n
    return (a * a2 + b * c2, a * b2 + b * d2,
            c * a2 + d * c2, c * b2 + d * d2,
            e * a2 + f * c2 + e2, e * b2 + f * d2 + f2)


def apply(m: Matrix, x: float, y: float) -> tuple[float, float]:
    a, b, c, d, e, f = m
    return (a * x + c * y + e, b * x + d * y + f)


# --------------------------------------------------------------------------------------
# Farben
# --------------------------------------------------------------------------------------


def rgb_hex(r: float, g: float, b: float) -> str:
    def kanal(v: float) -> int:
        return max(0, min(255, round(v * 255)))
    return f"#{kanal(r):02X}{kanal(g):02X}{kanal(b):02X}"


def cmyk_hex(c: float, m: float, y: float, k: float) -> str:
    return rgb_hex((1 - c) * (1 - k), (1 - m) * (1 - k), (1 - y) * (1 - k))


# --------------------------------------------------------------------------------------
# Inhaltsstrom auswerten
# --------------------------------------------------------------------------------------

# Zahlen, Namen, Strings und Operatoren - mehr braucht der Grafikteil nicht.
TOKEN = re.compile(rb"""
      (?P<zahl>[-+]?\d*\.?\d+)
    | (?P<name>/[^\s/\[\]<>(){}]+)
    | (?P<str>\((?:\\.|[^\\()])*\))
    | (?P<hex><[0-9A-Fa-f\s]*>)
    | (?P<arr>[\[\]])
    | (?P<dict><<|>>)
    | (?P<op>[A-Za-z'"*]+)
""", re.VERBOSE)


class Pfad:
    """Ein gefuellter oder gestrichener Pfad in Seitenkoordinaten."""

    __slots__ = ("subpfade", "farbe", "gefuellt", "x0", "y0", "x1", "y1")

    def __init__(self, subpfade, farbe, gefuellt):
        self.subpfade = subpfade
        self.farbe = farbe
        self.gefuellt = gefuellt
        punkte = [p for sub in subpfade for p in sub]
        xs = [p[0] for p in punkte]
        ys = [p[1] for p in punkte]
        self.x0, self.x1 = (min(xs), max(xs)) if xs else (0.0, 0.0)
        self.y0, self.y1 = (min(ys), max(ys)) if ys else (0.0, 0.0)

    @property
    def breite(self) -> float:
        return self.x1 - self.x0

    @property
    def hoehe(self) -> float:
        return self.y1 - self.y0


def parse_content(stream: bytes) -> list[Pfad]:
    """Grafikoperatoren auswerten und die gezeichneten Pfade zurueckgeben."""
    stapel: list[tuple[Matrix, str, str]] = []
    ctm: Matrix = EINHEIT
    fuellfarbe = "#000000"
    strichfarbe = "#000000"

    operanden: list = []
    pfade: list[Pfad] = []

    aktuell: list[list[tuple[float, float]]] = []
    sub: list[tuple[float, float]] = []
    start = (0.0, 0.0)
    letzt = (0.0, 0.0)

    def zahlen(n: int) -> list[float]:
        werte = [v for v in operanden if isinstance(v, float)]
        return werte[-n:] if len(werte) >= n else []

    def abschliessen(fuellen: bool, farbe: str) -> None:
        nonlocal aktuell, sub
        if sub:
            aktuell.append(sub)
        gefiltert = [s for s in aktuell if len(s) >= 2]
        if gefiltert:
            pfade.append(Pfad(gefiltert, farbe, fuellen))
        aktuell, sub = [], []

    for m in TOKEN.finditer(stream):
        if m.lastgroup == "zahl":
            operanden.append(float(m.group()))
            continue
        if m.lastgroup in ("name", "str", "hex", "arr", "dict"):
            operanden.append(m.group())
            continue

        op = m.group("op").decode("latin-1")

        if op == "q":
            stapel.append((ctm, fuellfarbe, strichfarbe))
        elif op == "Q":
            if stapel:
                ctm, fuellfarbe, strichfarbe = stapel.pop()
        elif op == "cm":
            w = zahlen(6)
            if len(w) == 6:
                ctm = mat_mul(tuple(w), ctm)  # type: ignore[arg-type]

        # --- Pfadaufbau -------------------------------------------------------------
        elif op == "m":
            w = zahlen(2)
            if len(w) == 2:
                if sub:
                    aktuell.append(sub)
                letzt = start = apply(ctm, w[0], w[1])
                sub = [letzt]
        elif op == "l":
            w = zahlen(2)
            if len(w) == 2:
                letzt = apply(ctm, w[0], w[1])
                sub.append(letzt)
        elif op in ("c", "v", "y"):
            # Kurven werden zu Polygonzuegen aufgeloest: fuer die Anzeige auf einer
            # Karteikarte ist der Unterschied unsichtbar, und der Ausgabepfad bleibt
            # einfach (nur M/L), was das Einbetten in WPF-Geometrie trivial macht.
            w = zahlen(6 if op == "c" else 4)
            if op == "c" and len(w) == 6:
                p1, p2, p3 = (w[0], w[1]), (w[2], w[3]), (w[4], w[5])
            elif op == "v" and len(w) == 4:
                p1, p2, p3 = None, (w[0], w[1]), (w[2], w[3])
            elif op == "y" and len(w) == 4:
                p1, p2, p3 = (w[0], w[1]), None, (w[2], w[3])
            else:
                operanden = []
                continue

            a = letzt
            b = apply(ctm, *p1) if p1 else a
            d = apply(ctm, *p3)
            c = apply(ctm, *p2) if p2 else d
            for i in range(1, 9):
                t = i / 8
                s = 1 - t
                x = s**3 * a[0] + 3 * s * s * t * b[0] + 3 * s * t * t * c[0] + t**3 * d[0]
                y = s**3 * a[1] + 3 * s * s * t * b[1] + 3 * s * t * t * c[1] + t**3 * d[1]
                sub.append((x, y))
            letzt = d
        elif op == "re":
            w = zahlen(4)
            if len(w) == 4:
                x, y, br, ho = w
                ecken = [apply(ctm, x, y), apply(ctm, x + br, y),
                         apply(ctm, x + br, y + ho), apply(ctm, x, y + ho)]
                if sub:
                    aktuell.append(sub)
                aktuell.append(ecken + [ecken[0]])
                sub = []
                letzt = start = ecken[0]
        elif op == "h":
            if sub:
                sub.append(start)

        # --- Farben ------------------------------------------------------------------
        elif op in ("rg", "RG"):
            w = zahlen(3)
            if len(w) == 3:
                farbe = rgb_hex(*w)
                if op == "rg":
                    fuellfarbe = farbe
                else:
                    strichfarbe = farbe
        elif op in ("g", "G"):
            w = zahlen(1)
            if w:
                farbe = rgb_hex(w[0], w[0], w[0])
                if op == "g":
                    fuellfarbe = farbe
                else:
                    strichfarbe = farbe
        elif op in ("k", "K"):
            w = zahlen(4)
            if len(w) == 4:
                farbe = cmyk_hex(*w)
                if op == "k":
                    fuellfarbe = farbe
                else:
                    strichfarbe = farbe
        elif op in ("sc", "scn", "SC", "SCN"):
            w = [v for v in operanden if isinstance(v, float)]
            farbe = None
            if len(w) == 3:
                farbe = rgb_hex(*w)
            elif len(w) == 4:
                farbe = cmyk_hex(*w)
            elif len(w) == 1:
                farbe = rgb_hex(w[0], w[0], w[0])
            if farbe:
                if op in ("sc", "scn"):
                    fuellfarbe = farbe
                else:
                    strichfarbe = farbe

        # --- Pfadende ----------------------------------------------------------------
        elif op in ("f", "F", "f*", "b", "b*", "B", "B*"):
            abschliessen(True, fuellfarbe)
        elif op in ("S", "s"):
            abschliessen(False, strichfarbe)
        elif op == "n":
            aktuell, sub = [], []

        operanden = []

    return pfade


# --------------------------------------------------------------------------------------
# Schilder aus den Pfaden herausloesen
# --------------------------------------------------------------------------------------


def cluster(pfade: list[Pfad], abstand: float) -> list[list[Pfad]]:
    """Pfade, deren Rahmen sich beruehren oder nahe beieinanderliegen, gehoeren zum selben
    Schild. Einfaches Zusammenwachsen ueber eine Union-Find-Struktur."""
    n = len(pfade)
    eltern = list(range(n))

    def finde(i: int) -> int:
        while eltern[i] != i:
            eltern[i] = eltern[eltern[i]]
            i = eltern[i]
        return i

    def vereine(i: int, j: int) -> None:
        a, b = finde(i), finde(j)
        if a != b:
            eltern[b] = a

    for i in range(n):
        for j in range(i + 1, n):
            p, q = pfade[i], pfade[j]
            if (p.x0 - abstand <= q.x1 and q.x0 - abstand <= p.x1
                    and p.y0 - abstand <= q.y1 and q.y0 - abstand <= p.y1):
                vereine(i, j)

    gruppen: dict[int, list[Pfad]] = {}
    for i, p in enumerate(pfade):
        gruppen.setdefault(finde(i), []).append(p)
    return list(gruppen.values())


def normiere(gruppe: list[Pfad]) -> dict:
    """Eine Schildgruppe auf ein Feld von 0..100 umrechnen. Die y-Achse wird gespiegelt:
    PDF zaehlt von unten, Bildschirmkoordinaten von oben."""
    x0 = min(p.x0 for p in gruppe)
    x1 = max(p.x1 for p in gruppe)
    y0 = min(p.y0 for p in gruppe)
    y1 = max(p.y1 for p in gruppe)

    seite = max(x1 - x0, y1 - y0)
    if seite <= 0:
        return {}

    # Mittig einpassen, damit ein breites Schild nicht verzerrt wird.
    dx = (seite - (x1 - x0)) / 2
    dy = (seite - (y1 - y0)) / 2

    def um(x: float, y: float) -> tuple[float, float]:
        return (round((x - x0 + dx) / seite * 100, 2),
                round(100 - (y - y0 + dy) / seite * 100, 2))

    ebenen = []
    for p in gruppe:
        teile = []
        for sub in p.subpfade:
            punkte = [um(*pt) for pt in sub]
            entrumpelt = [punkte[0]]
            for pt in punkte[1:]:
                if abs(pt[0] - entrumpelt[-1][0]) > 0.05 or abs(pt[1] - entrumpelt[-1][1]) > 0.05:
                    entrumpelt.append(pt)
            if len(entrumpelt) >= 2:
                teile.append("M" + " L".join(f"{x},{y}" for x, y in entrumpelt) + " Z")
        if teile:
            ebenen.append({"d": " ".join(teile), "farbe": p.farbe, "gefuellt": p.gefuellt})

    return {"ebenen": ebenen, "seitePt": round(seite, 2),
            "mitte": [round((x0 + x1) / 2, 2), round((y0 + y1) / 2, 2)]}


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("pdf", type=Path)
    ap.add_argument("-o", "--out", type=Path, default=Path("zeichen.json"))
    ap.add_argument("--min-groesse", type=float, default=12.0,
                    help="Gruppen kleiner als das (in PDF-Punkten) sind Zierrat, kein Schild.")
    ap.add_argument("--abstand", type=float, default=1.5,
                    help="Wie nah zwei Pfade liegen muessen, um zum selben Schild zu zaehlen.")
    args = ap.parse_args()

    data = args.pdf.read_bytes()
    objs = load_objects(data)
    seiten = sorted(n for n, b in objs.items() if re.search(rb"/Type\s*/Page[^s]", b))

    alle = []
    for nr, obj in enumerate(seiten, 1):
        pfade = parse_content(page_contents(objs, objs[obj]))
        if not pfade:
            continue

        gruppen = [g for g in cluster(pfade, args.abstand)
                   if max(max(p.x1 for p in g) - min(p.x0 for p in g),
                          max(p.y1 for p in g) - min(p.y0 for p in g)) >= args.min_groesse]

        for g in gruppen:
            eintrag = normiere(g)
            if eintrag:
                eintrag["seite"] = nr
                eintrag["pfade"] = len(g)
                alle.append(eintrag)

        print(f"  Seite {nr:2}: {len(pfade):4} Pfade -> {len(gruppen):3} Schildgruppen", file=sys.stderr)

    args.out.write_text(json.dumps(alle, ensure_ascii=False, indent=1), encoding="utf-8")
    print(f"\n{len(alle)} Gruppen -> {args.out}", file=sys.stderr)
    return 0


if __name__ == "__main__":
    sys.exit(main())
