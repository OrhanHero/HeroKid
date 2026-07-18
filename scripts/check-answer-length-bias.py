#!/usr/bin/env python3
"""Prüft die kuratierten Multiple-Choice-Pools auf Längen-Bias: Wie oft ist die richtige
Antwort STRIKT die längste Option? Kinder lernen dieses Muster schnell ("nimm die längste") -
Ziel ist ein Wert nahe der Zufallserwartung (~33% bei 3 Optionen, 25% bei 4).

Nutzung:  python3 scripts/check-answer-length-bias.py [--min-questions 5] [--threshold 60]
Exit-Code 1, wenn ein Generator über dem Schwellwert liegt (für CI nutzbar).
"""
import argparse
import re
import sys
from pathlib import Path

GENERATORS_DIR = Path(__file__).resolve().parent.parent / "src" / "LernTor.ContentGen" / "Generators"

CSTRING = r'"((?:[^"\\]|\\.)*)"'

# Muster A: ("Frage", new[] { "o1", "o2", ... }, "Antwort", "Erklärung")
PATTERN_A = re.compile(
    r"\(\s*" + CSTRING + r"\s*,\s*new\[\]\s*\{([^}]*)\}\s*,\s*" + CSTRING + r"\s*,",
    re.DOTALL,
)

# Muster B: ("wort", "richtig", new[] { "falsch1", ... })  (Vokabel-Stil)
PATTERN_B = re.compile(
    r"\(\s*" + CSTRING + r"\s*,\s*" + CSTRING + r"\s*,\s*new\[\]\s*\{([^}]*)\}\s*\)",
    re.DOTALL,
)

STRINGS = re.compile(CSTRING)


def unescape(s: str) -> str:
    return s.replace('\\"', '"').replace("\\\\", "\\")


def analyze(path: Path):
    text = path.read_text(encoding="utf-8")
    total = 0
    longest = 0

    for match in PATTERN_A.finditer(text):
        options = [unescape(m.group(1)) for m in STRINGS.finditer(match.group(2))]
        answer = unescape(match.group(3))
        if answer not in options or len(options) < 2:
            continue
        total += 1
        if len(answer) > max(len(o) for o in options if o != answer):
            longest += 1

    for match in PATTERN_B.finditer(text):
        answer = unescape(match.group(2))
        wrongs = [unescape(m.group(1)) for m in STRINGS.finditer(match.group(3))]
        if not wrongs:
            continue
        total += 1
        if len(answer) > max(len(w) for w in wrongs):
            longest += 1

    return total, longest


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--min-questions", type=int, default=5)
    parser.add_argument("--threshold", type=float, default=60.0,
                        help="Maximal erlaubter Prozentsatz 'richtig = längste Option' pro Generator")
    args = parser.parse_args()

    failed = False
    rows = []
    for path in sorted(GENERATORS_DIR.glob("*Generator.cs")):
        total, longest = analyze(path)
        if total < args.min_questions:
            continue
        pct = 100.0 * longest / total
        rows.append((pct, path.name, longest, total))

    rows.sort(reverse=True)
    print(f"{'Generator':35} {'längste=richtig':>16} {'Anteil':>8}")
    for pct, name, longest, total in rows:
        marker = "  <-- BIAS" if pct > args.threshold else ""
        print(f"{name:35} {longest:>7}/{total:<8} {pct:>6.1f}%{marker}")
        if pct > args.threshold:
            failed = True

    if failed:
        print(f"\nMindestens ein Generator liegt über {args.threshold:.0f}% - Distraktoren angleichen "
              "(richtige Antwort kürzen oder falsche Optionen ähnlich lang formulieren).")
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
