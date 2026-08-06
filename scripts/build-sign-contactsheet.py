#!/usr/bin/env python3
"""Ordnet die extrahierten Zeichen-Vektoren ihren Namen zu und baut einen Kontaktbogen.

Zweck: aus dieser Entwicklungsumgebung heraus laesst sich nichts ansehen. Ein Zeichen, das
falsch zugeordnet ist, faellt in keinem Test auf - es rendert ja tadellos, es zeigt nur das
falsche Schild. Deshalb entsteht hier eine HTML-Seite mit allen extrahierten Zeichen samt
zugeordnetem Namen, die ein Mensch in zwei Minuten durchsehen kann.

Zuordnung: die Broschuere setzt links ein Bildraster und rechts die Namensliste - beide in
derselben Lesereihenfolge (oben nach unten, links nach rechts). Die VZ-Nummern selbst stehen in
einer Font-Untermenge mit eigener Kodierung und sind nicht ohne Weiteres lesbar; die Namen
dagegen schon. Also wird ueber die REIHENFOLGE gepaart, nicht ueber die Nummer.

Nutzung:
  python3 scripts/extract-signs-from-pdf.py <pdf> -o zeichen.json
  python3 scripts/build-sign-contactsheet.py <pdf> zeichen.json -o kontaktbogen.html
"""
from __future__ import annotations

import argparse
import html
import json
import re
import sys
import zlib
from pathlib import Path

UMLAUTE = {"\\337": "ß", "\\374": "ü", "\\344": "ä", "\\366": "ö",
           "\\304": "Ä", "\\326": "Ö", "\\334": "Ü", "\\351": "é"}


def load_objects(data: bytes) -> dict[int, bytes]:
    return {int(m.group(1)): m.group(2)
            for m in re.finditer(rb"(\d+)\s+0\s+obj(.*?)endobj", data, re.DOTALL)}


def stream_bytes(body: bytes) -> bytes:
    m = re.search(rb"stream\r?\n", body)
    if not m:
        return b""
    try:
        return zlib.decompress(body[m.end():body.find(b"endstream", m.end())])
    except zlib.error:
        return b""


def page_contents(objs: dict[int, bytes], body: bytes) -> bytes:
    s = re.search(rb"/Contents\s+(\d+)\s+0\s+R", body)
    return stream_bytes(objs.get(int(s.group(1)), b"")) if s else b""


def clean(text: str) -> str:
    """PDF-Stringliteral in lesbaren Text verwandeln."""
    for k, v in UMLAUTE.items():
        text = text.replace(k, v)
    text = re.sub(r"\\[0-7]{3}", "#", text)           # Ziffern aus der Font-Untermenge
    text = re.sub(r"\\([()\\])", r"\1", text)
    text = re.sub(r"\s+", " ", text).strip()
    # "V erkehr" -> "Verkehr": die Broschuere setzt mit Sperrsatz, was im Stream als
    # Leerzeichen mitten im Wort ankommt.
    text = re.sub(r"\b([A-ZÄÖÜ]) ([a-zäöüß])", r"\1\2", text)
    text = re.sub(r"([a-zäöüß]) ([a-zäöüß]{1,3})\b(?=[A-ZÄÖÜ])", r"\1\2", text)
    return text


# Positionierung, Textausgabe und alles dazwischen.
TEXT_OP = re.compile(rb"""
      (?P<str>\((?:\\.|[^\\()])*\))
    | (?P<zeile>\bT\*|\bTd|\bTD|\bTm|\bTJ|\bTj|\bBT|\bET)
""", re.VERBOSE)


def page_lines(stream: bytes) -> list[str]:
    """Die Textzeilen einer Seite in Stromreihenfolge.

    Wichtig: die Broschuere setzt mit Kerning, ein Wort steht deshalb als mehrere
    Stringstuecke mit Zahlen dazwischen im Strom ("Fu" -30 "ssgaenger"). Wer an jedem Stueck
    trennt, bekommt Bruchstuecke wie "erkehr" statt "Verkehr" - deshalb wird erst bei einem
    ZEILENWECHSEL (Td/TD/T*/Tm) getrennt, nicht bei jedem Textstueck.
    """
    zeilen: list[str] = []
    puffer = ""

    for m in TEXT_OP.finditer(stream):
        if m.lastgroup == "str":
            puffer += m.group()[1:-1].decode("latin-1")
            continue

        op = m.group("zeile")
        if op in (b"T*", b"Td", b"TD", b"Tm", b"BT", b"ET"):
            if puffer.strip():
                zeilen.append(puffer)
            puffer = ""

    if puffer.strip():
        zeilen.append(puffer)

    return zeilen


def sign_names(stream: bytes) -> list[str]:
    """Aus den Zeilen einer Namensseite die eigentlichen Zeichenbezeichnungen filtern."""
    namen: list[str] = []
    for zeile in page_lines(stream):
        text = clean(zeile)
        # Fußzeile, reine Nummernzeilen und Zierrat fallen weg.
        if "Verkehrszeichen in Deutsch" in text:
            continue
        text = text.strip(" #.-*")
        if len(text) > 2 and re.search(r"[A-Za-zÄÖÜäöüß]{3}", text):
            namen.append(text)
    return namen


def svg_for(gruppe: dict, groesse: int = 92) -> str:
    ebenen = []
    for e in gruppe["ebenen"]:
        if e["gefuellt"]:
            ebenen.append(f'<path d="{e["d"]}" fill="{e["farbe"]}"/>')
        else:
            ebenen.append(f'<path d="{e["d"]}" fill="none" stroke="{e["farbe"]}" '
                          f'stroke-width="0.7"/>')
    return (f'<svg viewBox="0 0 100 100" width="{groesse}" height="{groesse}">'
            + "".join(ebenen) + "</svg>")


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("pdf", type=Path)
    ap.add_argument("zeichen", type=Path, help="Ausgabe von extract-signs-from-pdf.py")
    ap.add_argument("-o", "--out", type=Path, default=Path("kontaktbogen.html"))
    args = ap.parse_args()

    data = args.pdf.read_bytes()
    objs = load_objects(data)
    seiten = sorted(n for n, b in objs.items() if re.search(rb"/Type\s*/Page[^s]", b))

    gruppen = json.loads(args.zeichen.read_text(encoding="utf-8"))

    # Namen je Seite einsammeln. Die Namensliste steht auf der Seite NACH dem Bildraster.
    namen_je_seite: dict[int, list[str]] = {}
    for nr, obj in enumerate(seiten, 1):
        namen_je_seite[nr] = sign_names(page_contents(objs, objs[obj]))

    # Bildgruppen je Seite in Lesereihenfolge: oben nach unten, links nach rechts.
    # Die Zeilenhoehe wird grob gerastert, damit leicht versetzte Schilder nicht die
    # Reihenfolge zerreissen.
    zeilen_raster = 30
    je_seite: dict[int, list[dict]] = {}
    for g in gruppen:
        je_seite.setdefault(g["seite"], []).append(g)
    for nr in je_seite:
        je_seite[nr].sort(key=lambda g: (-round(g["mitte"][1] / zeilen_raster),
                                         g["mitte"][0]))

    teile = ["""<!doctype html><meta charset="utf-8">
<title>Verkehrszeichen aus der PDF - Kontaktbogen</title>
<style>
 body{font:15px/1.5 system-ui,sans-serif;margin:24px;background:#f5f6fa;color:#2d3436}
 h1{font-size:22px} h2{font-size:16px;margin:28px 0 8px;color:#636e72}
 .raster{display:flex;flex-wrap:wrap;gap:12px}
 .karte{background:#fff;border-radius:10px;padding:10px;width:150px;text-align:center;
        box-shadow:0 1px 3px rgba(0,0,0,.12)}
 .karte svg{display:block;margin:0 auto 6px}
 .name{font-size:11px;line-height:1.3;color:#2d3436;min-height:32px}
 .meta{font-size:10px;color:#b2bec3;margin-top:4px}
 .hinweis{background:#fff3cd;border-radius:8px;padding:12px 16px;margin:16px 0;font-size:13px}
</style>
<h1>Verkehrszeichen aus der ADAC-PDF &ndash; Original-Vektoren</h1>
<div class="hinweis"><b>Bitte durchsehen:</b> stimmt unter jedem Bild der Name? Die Zuordnung
laeuft ueber die Lesereihenfolge (Bildraster links, Namensliste rechts), nicht ueber die
VZ-Nummer &ndash; die steht in einer Font-Untermenge und ist nicht zuverlaessig lesbar. Wenn
irgendwo Bild und Name auseinanderlaufen, sag mir bei welchem, dann korrigiere ich den
Versatz.</div>"""]

    zugeordnet = 0
    for nr in sorted(je_seite):
        bilder = je_seite[nr]
        namen = namen_je_seite.get(nr + 1, []) or namen_je_seite.get(nr, [])
        if len(bilder) < 4:
            continue

        teile.append(f'<h2>Seite {nr} &ndash; {len(bilder)} Zeichen, '
                     f'{len(namen)} Namen auf Seite {nr + 1}</h2><div class="raster">')
        for i, g in enumerate(bilder):
            name = namen[i] if i < len(namen) else "&mdash;"
            if i < len(namen):
                zugeordnet += 1
            teile.append(
                f'<div class="karte">{svg_for(g)}'
                f'<div class="name">{html.escape(name)}</div>'
                f'<div class="meta">S{nr} #{i + 1} &middot; {len(g["ebenen"])} Ebenen</div></div>')
        teile.append("</div>")

    args.out.write_text("\n".join(teile), encoding="utf-8")
    print(f"{zugeordnet} Zeichen mit Namen -> {args.out}", file=sys.stderr)
    return 0


if __name__ == "__main__":
    sys.exit(main())
