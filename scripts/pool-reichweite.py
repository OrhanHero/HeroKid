#!/usr/bin/env python3
"""Pool-Reichweite: Wie lange reichen die kuratierten Fragen je Fach und Klassenstufe?

Drei Mechanismen halten Fragen zurück (gemeisterte Fragen per Spaced Repetition, kürzlich
gesehene, Fehler-Kartei). Wann ein Fach nur noch Wiederholungen liefert, ist deshalb eine
Rechenfrage, keine Gefühlsfrage. Dieses Skript rechnet sie aus:

  * Poolgröße: gezählt aus den Generatoren (TopicsByGrade -> Themen-Methoden -> Fragenlisten),
    Klasse 8/10 greifen wie in der App auf die 7er/9er-Pools zurück.
  * Übungsrate: Aufgaben je Fach und Tag (Standard 6, siehe StudentProfile) mal Übungstage je
    Woche. Die Übungstage kommen aus den echten Stundenplänen in docs/STUNDENPLAENE-2026-27.md,
    ausgewertet nach derselben Regel wie TimetableSubjectPlanner: geübt wird am Vortag eines
    Schultags mit dem Fach, Türkisch immer, NaWi reihum auf Bio/Chemie/Physik verteilt.

Ergebnis je Kind: nach wie vielen Wochen jede Frage eines Fachs einmal dran war. Ab dann
kommen nur noch fällige Wiederholungen (7/30/90 Tage) und die Fehler-Kartei. Das ist nicht
falsch, aber es ist der Punkt, ab dem ein Fach sich "immer gleich" anfühlt.

Nutzung:  python3 scripts/pool-reichweite.py [--pro-tag 6] [--warnung 8] [--alle-stufen]
Rein informativ, Exit-Code immer 0.
"""
import argparse
import math
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
GENERATORS = ROOT / "src" / "LernTor.ContentGen" / "Generators"
SUBJECT_MAP = ROOT / "src" / "LernTor.Core" / "Services" / "TimetableSubjectMap.cs"
PLAENE = ROOT / "docs" / "STUNDENPLAENE-2026-27.md"

KINDER = {"Batuhan": 9, "Emirhan": 6}  # Klassenstufe laut Profil
WOCHENTAGE = ["Mo", "Di", "Mi", "Do", "Fr"]
NAWI = ["Biologie", "Chemie", "Physik"]
IMMER = ["Tuerkisch"]
# Doppeljahrgangs-Regel aus ExerciseGeneratorBase: 8 -> 7, 10 -> 9.
POOL_STUFE = {6: 6, 7: 7, 8: 7, 9: 9, 10: 9}


# ---------------------------------------------------------------- C#-Quelltext lesen

def skip_literal(text, i):
    """Gibt den Index hinter einem String-/Char-Literal ab Position i zurück, sonst None."""
    if text.startswith('"""', i):
        end = text.find('"""', i + 3)
        return len(text) if end < 0 else end + 3
    if text.startswith('@"', i) or text.startswith('$@"', i) or text.startswith('@$"', i):
        j = text.index('"', i) + 1
        while j < len(text):
            if text[j] == '"':
                if j + 1 < len(text) and text[j + 1] == '"':
                    j += 2
                    continue
                return j + 1
            j += 1
        return len(text)
    if text[i] == '"' or text.startswith('$"', i):
        j = text.index('"', i) + 1
        while j < len(text):
            if text[j] == "\\":
                j += 2
                continue
            if text[j] == '"':
                return j + 1
            j += 1
        return len(text)
    if text[i] == "'":
        m = re.match(r"'(\\.|[^\\'])'", text[i:])
        if m:
            return i + m.end()
    return None


def strip_comments(text):
    out, i = [], 0
    while i < len(text):
        lit = skip_literal(text, i)
        if lit is not None:
            out.append(text[i:lit])
            i = lit
        elif text.startswith("//", i):
            j = text.find("\n", i)
            i = len(text) if j < 0 else j
        elif text.startswith("/*", i):
            j = text.find("*/", i + 2)
            i = len(text) if j < 0 else j + 2
        else:
            out.append(text[i])
            i += 1
    return "".join(out)


def matching(text, i, open_ch, close_ch):
    """Index der schließenden Klammer zu text[i] == open_ch."""
    depth = 0
    while i < len(text):
        lit = skip_literal(text, i)
        if lit is not None:
            i = lit
            continue
        if text[i] == open_ch:
            depth += 1
        elif text[i] == close_ch:
            depth -= 1
            if depth == 0:
                return i
        i += 1
    return len(text) - 1


def count_entries(body):
    """Einträge eines Array-Initialisierers: Tupel auf oberster Ebene, sonst Elemente."""
    tuples, items, depth, i, has_content = 0, 0, 0, 0, False
    while i < len(body):
        lit = skip_literal(body, i)
        if lit is not None:
            has_content = True
            i = lit
            continue
        c = body[i]
        if c in "({[":
            if c == "(" and depth == 0:
                tuples += 1
            depth += 1
            has_content = True
        elif c in ")}]":
            depth -= 1
        elif c == "," and depth == 0:
            items += 1
        elif not c.isspace():
            has_content = True
        i += 1
    if tuples:
        return tuples
    return items + (1 if has_content and not body.rstrip().endswith(",") else 0)


class Generator:
    def __init__(self, path):
        self.name = path.stem
        self.text = strip_comments(path.read_text(encoding="utf-8"))
        m = re.search(r"Subject\s*=>\s*Subject\.(\w+)", self.text)
        self.subject = m.group(1) if m else None
        self.arrays = {}   # Name -> Anzahl Einträge
        self.bodies = {}   # Name -> Quelltext (Methode oder Feld-Initialisierer)
        self._scan()

    def _scan(self):
        t = self.text
        for m in re.finditer(r"(\w+)\s*=\s*(new[^{;=]*)?\{", t):
            name = m.group(1)
            start = m.end() - 1
            end = matching(t, start, "{", "}")
            decl = t[max(0, m.start() - 200):m.start()]
            if re.search(r"\[\]\s*$", decl.split("\n")[-1]) or re.search(r"\[\]\s+$", decl):
                self.arrays[name] = count_entries(t[start + 1:end])
            else:
                self.bodies.setdefault(name, t[start:end + 1])
        for m in re.finditer(r"(\w+)\s*\(\s*Random\b[^)]*\)\s*(=>|\{)", t):
            name = m.group(1)
            if m.group(2) == "{":
                start = m.end() - 1
                self.bodies[name] = t[start:matching(t, start, "{", "}") + 1]
            else:
                end = t.find(";", m.end())
                self.bodies[name] = t[m.end():end]
        # Hilfsmethoden mit anderen Parametern (z. B. Weltwunder(Random r, GradeLevel stufe)).
        for m in re.finditer(r"static\s+[\w<>\[\],\s]+?\s(\w+)\s*\([^)]*\)\s*(=>|\{)", t):
            name = m.group(1)
            if name in self.bodies:
                continue
            if m.group(2) == "{":
                start = m.end() - 1
                self.bodies[name] = t[start:matching(t, start, "{", "}") + 1]
            else:
                self.bodies[name] = t[m.end():t.find(";", m.end())]

    def arrays_of(self, name, seen=None):
        seen = seen if seen is not None else set()
        if name in seen:
            return set()
        seen.add(name)
        if name in self.arrays:
            return {name}
        found = set()
        for ident in set(re.findall(r"\b[A-Z]\w*\b", self.bodies.get(name, ""))):
            if ident in self.arrays or ident in self.bodies:
                found |= self.arrays_of(ident, seen)
        return found

    def topics_by_grade(self):
        m = re.search(r"TopicsByGrade\s*\{\s*get;\s*\}\s*=", self.text)
        if not m:
            return {}
        start = self.text.find("{", self.text.find("new Dictionary", m.end()))
        block = self.text[start:matching(self.text, start, "{", "}") + 1]
        result = {}
        for gm in re.finditer(r"\[GradeLevel\.Klasse(\d+)\]\s*=\s*", block):
            rest = block[gm.end():]
            if rest.startswith("new"):
                s = rest.find("{")
                inner = rest[s + 1:matching(rest, s, "{", "}")]
                names = re.findall(r"\b\w+\b", inner)
            else:
                ref = re.match(r"(\w+)", rest).group(1)
                names = re.findall(r"\b\w+\b", self.bodies.get(ref, ""))
                names = [n for n in names if n in self.bodies or n in self.arrays]
            result[int(gm.group(1))] = names
        return result

    def local_entries(self, name):
        """Fragen aus lokalen Arrays im Rumpf einer Themen-Methode (z. B. `var varianten =
        new (string Frage, ...)[] { ... }`). Gezählt werden nur Tupel mit Text darin -
        Zahlenlisten wie `new[] { 5, 10, 20 }` erzeugen Aufgaben, sie sind kein Fragenpool."""
        body = self.bodies.get(name, "")
        total = 0
        for m in re.finditer(r"\[\]\s*\{", body):
            start = m.end() - 1
            inner = body[start + 1:matching(body, start, "{", "}")]
            if re.match(r"\s*\(", inner) and '"' in inner:
                total += count_entries(inner)
        return total

    def pool(self, grade):
        """(kuratierte Fragen, Themen mit Fragenliste, Themen gesamt) für eine Klassenstufe.

        Themen ohne erkennbare Fragenliste erzeugen ihre Aufgaben (Mathe, Ohmsches Gesetz)
        oder holen sie von außerhalb des Generators (Diktat) - sie erschöpfen sich hier nicht
        und zählen nur beim Anteil mit."""
        topics = self.topics_by_grade().get(grade)
        if not topics:
            return 0, 0, 0
        arrays, lokal, kuratiert = set(), 0, 0
        for topic in topics:
            found = self.arrays_of(topic)
            eigene = self.local_entries(topic) if not found else 0
            if found or eigene:
                kuratiert += 1
            arrays |= found
            lokal += eigene
        return sum(self.arrays[a] for a in arrays) + lokal, kuratiert, len(topics)


# ---------------------------------------------------------------- Stundenpläne

def subject_map():
    text = SUBJECT_MAP.read_text(encoding="utf-8")
    block = text[text.index("Bekannt ="):text.index("};", text.index("Bekannt ="))]
    return {k.lower(): v for k, v in re.findall(r'\["([^"]+)"\]\s*=\s*Subject\.(\w+)', block)}


def timetables():
    text = PLAENE.read_text(encoding="utf-8")
    plans = {}
    for kind in KINDER:
        m = re.search(r"## " + kind + r".*?```\n(.*?)```", text, re.DOTALL)
        plans[kind] = {}
        for line in m.group(1).strip().splitlines():
            day, rest = line.split(":", 1)
            labels = []
            for entry in rest.split(","):
                label = re.sub(r"^\s*\d+\s+", "", entry).strip()
                label = re.sub(r"\s*[\(\[].*$", "", label)
                labels.append(label)
            plans[kind][day.strip()] = labels
    return plans


def practice_days_per_week(plan, mapping):
    """Übungstage je Fach und Woche nach der Regel von TimetableSubjectPlanner."""
    days = {}
    for day in WOCHENTAGE:
        labels = plan.get(day, [])
        subjects = {mapping[l.lower()] for l in labels if l.lower() in mapping}
        for s in subjects:
            days[s] = days.get(s, 0) + 1
        if any(l.lower() in ("nawi", "naturwissenschaften") for l in labels):
            for s in NAWI:
                days[s] = days.get(s, 0) + 1 / len(NAWI)
    for s in IMMER:
        days[s] = len(WOCHENTAGE)
    return days


# ---------------------------------------------------------------- Ausgabe

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--pro-tag", type=int, default=6, help="Aufgaben je Fach und Übungstag (Standard 6)")
    parser.add_argument("--warnung", type=float, default=8.0, help="Unter so vielen Wochen gilt ein Pool als knapp")
    parser.add_argument("--alle-stufen", action="store_true", help="Poolgrößen aller Stufen zeigen")
    args = parser.parse_args()

    gens = {}
    for path in sorted(GENERATORS.glob("*Generator.cs")):
        if path.stem in ("ExerciseGeneratorBase", "IExerciseGenerator"):
            continue
        g = Generator(path)
        if g.subject:
            gens[g.subject] = g

    print("Poolgrößen: kuratierte Fragen je Stufe. \"+2e\" = dazu 2 Themen, die ihre Aufgaben")
    print("erzeugen (Rechenaufgaben) oder von außerhalb holen (Diktat); ∞ = nur solche Themen.\n")
    print(f"{'Fach':14} {'K6':>9} {'K7':>9} {'K9':>9}")
    for subject, g in sorted(gens.items()):
        cells = []
        for grade in (6, 7, 9):
            n, kuratiert, gesamt = g.pool(grade)
            if gesamt == 0:
                cells.append("-")
            elif kuratiert == 0:
                cells.append("∞")
            else:
                cells.append(str(n) + (f"+{gesamt - kuratiert}e" if gesamt > kuratiert else ""))
        print(f"{subject:14} {cells[0]:>9} {cells[1]:>9} {cells[2]:>9}")

    mapping = subject_map()
    plans = timetables()
    knapp = []
    for kind, stufe in KINDER.items():
        days = practice_days_per_week(plans[kind], mapping)
        print(f"\n{kind} (Klasse {stufe}) - geübt nach Stundenplan, {args.pro_tag} Aufgaben je Fach und Tag\n")
        print(f"{'Fach':14} {'Pool':>6} {'Tage/Wo':>8} {'Fragen/Wo':>10} {'Wochen bis durch':>17}")
        for subject in sorted(days, key=lambda s: s):
            g = gens.get(subject)
            if g is None:
                continue
            n, kuratiert, gesamt = g.pool(POOL_STUFE[stufe])
            per_week = days[subject] * args.pro_tag
            if gesamt == 0:
                weeks = "kein Pool!"
            elif kuratiert == 0:
                weeks = "∞"
            else:
                # Die Themen werden etwa gleich oft gezogen; nur der Anteil der Themen mit
                # Fragenliste verbraucht den Pool.
                w = n / (per_week * kuratiert / gesamt)
                weeks = f"{w:.1f}"
                if w < args.warnung:
                    knapp.append((kind, subject, w, n))
            pool = "∞" if gesamt and kuratiert == 0 else str(n)
            print(f"{subject:14} {pool:>6} {days[subject]:>8.1f} {per_week:>10.1f} {weeks:>17}")

    print()
    if knapp:
        print(f"Knapp (unter {args.warnung:g} Wochen, danach nur noch Wiederholungen):")
        for kind, subject, w, n in sorted(knapp, key=lambda k: k[2]):
            print(f"  {kind:8} {subject:12} {w:4.1f} Wochen ({n} Fragen)")
    else:
        print(f"Kein Fach unter {args.warnung:g} Wochen.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
