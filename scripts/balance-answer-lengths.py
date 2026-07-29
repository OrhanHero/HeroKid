#!/usr/bin/env python3
"""Senkt den Anteil 'richtige Antwort = strikt laengste Option' pro Generator auf einen
Zielwert (~Zufalls-Erwartung), NICHT auf 0% - sonst entsteht ein umgekehrt ausnutzbares
Muster ('nimm nie die laengste'). Positionsbasiert (kein Text-Suche/Ersetzen), daher robust
gegen doppelte Distraktor-Texte im selben Generator. Nur Muster A
(Frage, Optionen[], Antwort, Erklaerung)."""
import hashlib
import re
import sys

CSTRING = r'"((?:[^"\\]|\\.)*)"'
PATTERN_A = re.compile(
    r'(\(\s*"(?:[^"\\]|\\.)*"\s*,\s*new\[\]\s*\{)([^}]*)(\}\s*,\s*"(?:[^"\\]|\\.)*"\s*,)',
    re.DOTALL,
)
STRINGS = re.compile(CSTRING)

SUFFIXES = [
    " (was so in der Praxis nicht zutrifft)",
    " - eine verbreitete, aber falsche Annahme",
    ", was einer genaueren Pruefung nicht standhaelt",
    ", obwohl das auf den ersten Blick plausibel klingt",
    ", was die eigentliche Bedeutung des Begriffs verfehlt",
    " und deshalb hier nicht zutrifft",
    ", was so nicht korrekt ist",
    " - eine haeufige, aber unzutreffende Vorstellung",
    ", auch wenn das manche zunaechst vermuten wuerden",
    ", was bei genauerem Hinsehen nicht stimmt",
]


def unescape(s: str) -> str:
    return s.replace('\\"', '"').replace('\\\\', '\\')


def scan(text: str):
    """Liefert (alle_matches_mit_gap, gesamt_anzahl)."""
    entries = []
    total = 0
    for m in PATTERN_A.finditer(text):
        tail = m.group(3)
        ans_m = STRINGS.search(tail)
        if ans_m is None:
            continue
        answer_raw = ans_m.group(1)
        answer_len = len(unescape(answer_raw))

        opts = list(STRINGS.finditer(m.group(2)))
        if len(opts) < 2:
            continue
        texts_raw = [o.group(1) for o in opts]
        if answer_raw not in texts_raw:
            continue

        total += 1
        wrong = [(o, len(unescape(o.group(1)))) for o in opts if o.group(1) != answer_raw]
        if not wrong:
            continue
        target_opt, target_len = max(wrong, key=lambda x: x[1])
        gap = answer_len - target_len
        if gap > 0:
            entries.append((m.start(), m, target_opt, gap, answer_raw))
    return entries, total


def process(path: str, target_pct: float = 35.0) -> None:
    text = open(path, encoding="utf-8").read()
    entries, total = scan(text)
    biased = len(entries)
    if total == 0:
        print(f"{path}: keine passenden Fragen gefunden")
        return
    current_pct = 100.0 * biased / total
    if current_pct <= target_pct:
        print(f"{path}: bereits bei {current_pct:.1f}% (Ziel {target_pct:.0f}%) - nichts zu tun")
        return

    target_biased = round(target_pct / 100 * total)
    fix_count = max(0, biased - target_biased)

    # Deterministische, aber "zufällig" wirkende Auswahl, welche Einträge NICHT korrigiert
    # werden - ein Rest nahe der Zufallserwartung bleibt bewusst stehen, damit "korrigiert
    # = nie am längsten" seinerseits kein neues Muster wird.
    entries_sorted = sorted(entries, key=lambda e: hashlib.md5(e[1].group(0).encode()).hexdigest())
    to_fix_starts = {e[0] for e in entries_sorted[:fix_count]}

    out = []
    last_end = 0
    suffix_idx = 0
    changed = 0

    for start, m, target_opt, gap, _answer_raw in entries:
        if start not in to_fix_starts:
            continue
        prefix, optsblock, tail = m.group(1), m.group(2), m.group(3)

        # Leichtes, variierendes Ueberschiessen statt fixem Betrag - sonst waere "der
        # nachtraeglich verlaengerte Distraktor" selbst an einem festen Muster erkennbar.
        needed = gap + 3 + (suffix_idx * 7) % 11
        combo = ""
        while len(combo) < needed:
            combo += SUFFIXES[suffix_idx % len(SUFFIXES)]
            suffix_idx += 1

        s, e = target_opt.span(1)
        new_optsblock = optsblock[:s] + target_opt.group(1) + combo + optsblock[e:]
        changed += 1

        out.append(text[last_end:m.start()])
        out.append(prefix + new_optsblock + tail)
        last_end = m.end()

    out.append(text[last_end:])
    new_text = "".join(out)
    if changed:
        open(path, "w", encoding="utf-8").write(new_text)
    print(f"{path}: {changed}/{biased} biased Eintraege korrigiert "
          f"(Ziel {target_pct:.0f}%, vorher {current_pct:.1f}%)")


if __name__ == "__main__":
    target = 35.0
    args = sys.argv[1:]
    if args and args[0].startswith("--target="):
        target = float(args[0].split("=", 1)[1])
        args = args[1:]
    for path in args:
        process(path, target_pct=target)
