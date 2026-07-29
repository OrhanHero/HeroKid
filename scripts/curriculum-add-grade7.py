#!/usr/bin/env python3
"""Ergänzt die Fach-Tabelle in docs/CURRICULUM.md um eine Klasse-7-Spalte.

Die Tabellen sind zweispaltig (Klasse 6 | Klasse 9) angelegt. Beim Klasse-7-Ausbau muss jede
Tabelle auf drei Spalten erweitert werden - das von Hand zu tun ist fehleranfällig, weil
Zeilenzahl und Trennzeile mitgezogen werden müssen.

Nutzung:
    python3 scripts/curriculum-add-grade7.py "Geschichte" "Thema 1" "Thema 2" ...

Der erste Parameter ist die Überschrift des Abschnitts (Teilstring reicht, z.B. "Gewi").
"""
from __future__ import annotations

import re
import sys
from pathlib import Path

DOC = Path(__file__).resolve().parent.parent / "docs" / "CURRICULUM.md"


def main() -> int:
    if len(sys.argv) < 3:
        print(__doc__)
        return 2

    section, topics = sys.argv[1], sys.argv[2:]
    text = DOC.read_text(encoding="utf-8")

    heading = re.search(rf"^##\s+.*{re.escape(section)}.*$", text, re.MULTILINE)
    if not heading:
        print(f"Abschnitt '{section}' nicht gefunden.")
        return 1

    rest = text[heading.end():]
    table = re.search(r"\n\n(\|[^\n]*\|)\n(\|[-| :]+\|)\n((?:\|[^\n]*\|\n)+)", rest)
    if not table:
        print(f"Keine Tabelle unter '{heading.group(0)}' gefunden.")
        return 1

    header_cells = [c.strip() for c in table.group(1).strip("|").split("|")]
    if len(header_cells) != 2:
        print(f"Tabelle hat {len(header_cells)} Spalten - erwartet werden 2 (bereits erweitert?).")
        return 1

    rows = [
        [c.strip() for c in line.strip().strip("|").split("|")]
        for line in table.group(3).strip("\n").split("\n")
    ]
    height = max(len(rows), len(topics))
    while len(rows) < height:
        rows.append(["", ""])
    while len(topics) < height:
        topics.append("")

    new_header = f"| {header_cells[0]} | Klasse 7 | {header_cells[1]} |"
    new_sep = "|---|---|---|"
    new_rows = "\n".join(
        f"| {row[0]} | {topic} | {row[1] if len(row) > 1 else ''} |"
        for row, topic in zip(rows, topics)
    )

    old_block = table.group(0)
    new_block = f"\n\n{new_header}\n{new_sep}\n{new_rows}\n"
    text = text[: heading.end()] + rest.replace(old_block, new_block, 1)
    DOC.write_text(text, encoding="utf-8")
    print(f"{heading.group(0).strip()}: Klasse-7-Spalte mit {len([t for t in topics if t])} Themen ergänzt.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
