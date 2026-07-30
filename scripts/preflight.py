#!/usr/bin/env python3
"""Lokale Vorab-Prüfung VOR jedem Commit/Push.

Hintergrund: Diese Codebasis wird aus einer Umgebung ohne .NET SDK entwickelt (siehe CLAUDE.md),
echtes Kompilieren ist nur in der GitHub-Action auf windows-latest möglich - pro Runde ~8 Minuten
Wartezeit. Dieses Skript fängt vorher genau die Fehlerklassen ab, die in dieser Codebasis
tatsächlich schon einmal aufgetreten sind ("Hard-won gotchas" in CLAUDE.md), und macht sie
in Sekunden statt Minuten sichtbar.

Es ersetzt KEINEN Compiler - es prüft nur bekannte, statisch erkennbare Fallen:

  1. Klammer-/Anführungszeichen-Balance in .cs-Dateien
  2. XAML-Wohlgeformtheit (fängt kaputte Tags/Attribute)
  3. `<Run Text="{Binding ...}"/>` ohne `Mode=OneWay`  -> XamlParseException zur Laufzeit
  4. `HttpClient` in LernTor.App ohne `using System.Net.Http;` -> nur dort kein implicit using
  5. `Application.Current.Shutdown()` ohne vorheriges `Unlock()` -> Kiosk lässt sich nicht beenden
  6. Vollständigkeit der Fach-Checkliste bei neuen `Subject`-Werten
  7. `OrderBy`/`OrderByDescending` auf DateTimeOffset-Spalten in Repositories (EF-Sqlite)
  8. JSON-/YAML-Gültigkeit der Konfigurations- und Zustandsdateien

Nutzung:  python3 scripts/preflight.py            (alles prüfen)
          python3 scripts/preflight.py --quick    (ohne die langsameren Repo-weiten Scans)
Exit-Code 1, sobald ein Befund vorliegt.
"""
from __future__ import annotations

import argparse
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
SRC = ROOT / "src"
TESTS = ROOT / "tests"

findings: list[str] = []


def report(check: str, path: Path, detail: str) -> None:
    rel = path.relative_to(ROOT) if path.is_absolute() else path
    findings.append(f"[{check}] {rel}: {detail}")


# --------------------------------------------------------------------------------------
# 1 + 2: Grundlegende Wohlgeformtheit
# --------------------------------------------------------------------------------------

def check_cs_balance() -> None:
    """Klammerbalance ausserhalb von Strings/Kommentaren - fängt abgeschnittene Edits."""
    for path in sorted(SRC.rglob("*.cs")) + sorted(TESTS.rglob("*.cs")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        text = path.read_text(encoding="utf-8")
        depth_curly = depth_paren = 0
        in_string = in_char = in_line_comment = in_block_comment = False
        in_verbatim = False
        i = 0
        while i < len(text):
            c = text[i]
            nxt = text[i + 1] if i + 1 < len(text) else ""

            if in_line_comment:
                if c == "\n":
                    in_line_comment = False
            elif in_block_comment:
                if c == "*" and nxt == "/":
                    in_block_comment = False
                    i += 1
            elif in_string:
                if in_verbatim:
                    if c == '"' and nxt == '"':
                        i += 1
                    elif c == '"':
                        in_string = in_verbatim = False
                else:
                    if c == "\\":
                        i += 1
                    elif c == '"':
                        in_string = False
            elif in_char:
                if c == "\\":
                    i += 1
                elif c == "'":
                    in_char = False
            else:
                if c == "/" and nxt == "/":
                    in_line_comment = True
                    i += 1
                elif c == "/" and nxt == "*":
                    in_block_comment = True
                    i += 1
                elif c == "@" and nxt == '"':
                    in_string = in_verbatim = True
                    i += 1
                elif c == '"':
                    in_string = True
                elif c == "'":
                    in_char = True
                elif c == "{":
                    depth_curly += 1
                elif c == "}":
                    depth_curly -= 1
                elif c == "(":
                    depth_paren += 1
                elif c == ")":
                    depth_paren -= 1
            i += 1

        if depth_curly != 0:
            report("cs-balance", path, f"geschweifte Klammern unausgeglichen (Differenz {depth_curly:+d})")
        if depth_paren != 0:
            report("cs-balance", path, f"runde Klammern unausgeglichen (Differenz {depth_paren:+d})")


def check_xaml_wellformed() -> None:
    import xml.dom.minidom

    for path in sorted(SRC.rglob("*.xaml")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        try:
            xml.dom.minidom.parse(str(path))
        except Exception as exc:  # noqa: BLE001 - jede Parse-Ursache ist ein Befund
            report("xaml-wellformed", path, str(exc))


# --------------------------------------------------------------------------------------
# 3-7: Bekannte Fallen dieser Codebasis (CLAUDE.md "Hard-won gotchas")
# --------------------------------------------------------------------------------------

RUN_TEXT_BINDING = re.compile(r"<Run\b[^>]*\bText\s*=\s*\"\{\s*Binding\b[^\"]*\"", re.DOTALL)


def check_run_text_oneway() -> None:
    """`Run.Text` hat BindsTwoWayByDefault - ohne Mode=OneWay wirft die View beim Rendern."""
    for path in sorted(SRC.rglob("*.xaml")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        for match in RUN_TEXT_BINDING.finditer(path.read_text(encoding="utf-8")):
            snippet = match.group(0)
            if "Mode=OneWay" in snippet or "Mode=OneTime" in snippet:
                continue
            line = path.read_text(encoding="utf-8")[: match.start()].count("\n") + 1
            report("run-text-oneway", path,
                   f"Zeile {line}: <Run Text=\"{{Binding ...}}\"> ohne Mode=OneWay "
                   f"-> XamlParseException beim Rendern")


def check_duplicate_style_assignment() -> None:
    """Style gleichzeitig als Attribut UND als <X.Style>-Element gesetzt.

    WPF erlaubt jede Eigenschaft nur einmal je Element. Passiert typischerweise beim
    Nachruesten eines Style-Triggers: das urspruengliche Style="{StaticResource ...}"
    bleibt stehen, und der Compiler meldet erst in der CI
    'MC3024: Style property has already been set and can be set only once'.
    """
    element_start = re.compile(r"<(\w+)\b((?:[^>\"]|\"[^\"]*\")*?)/?>", re.DOTALL)

    for path in sorted(SRC.rglob("*.xaml")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue

        text = path.read_text(encoding="utf-8")
        for match in element_start.finditer(text):
            tag, attributes = match.group(1), match.group(2)
            if not re.search(r"\bStyle\s*=", attributes):
                continue

            # Selbstschliessende Elemente (<X ... />) koennen gar kein Kind-Element enthalten -
            # ohne diese Abfrage schlug die Pruefung beim Style eines spaeteren Geschwisters an.
            if match.group(0).rstrip().endswith("/>"):
                continue

            # Setzt dasselbe Element weiter unten auch <Tag.Style>? Nur bis zum naechsten
            # gleichnamigen Start-Tag suchen, damit Geschwister nicht mitzaehlen.
            rest = text[match.end():]
            next_same_tag = rest.find(f"<{tag} ")
            scope = rest if next_same_tag < 0 else rest[:next_same_tag]
            if f"<{tag}.Style>" in scope:
                line = text[: match.start()].count("\n") + 1
                report("xaml-doppelter-style", path,
                       f"Zeile {line}: <{tag}> setzt Style als Attribut UND als <{tag}.Style>-Element "
                       f"-> MC3024 beim Kompilieren")


def check_httpclient_using() -> None:
    """net8.0-windows + UseWPF bekommt System.Net.Http NICHT als implicit using."""
    app_dir = SRC / "LernTor.App"
    if not app_dir.exists():
        return
    for path in sorted(app_dir.rglob("*.cs")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        text = path.read_text(encoding="utf-8")
        if "HttpClient" not in text:
            continue
        if "using System.Net.Http;" in text or "System.Net.Http.HttpClient" in text:
            continue
        report("httpclient-using", path,
               "nutzt HttpClient ohne 'using System.Net.Http;' (in LernTor.App kein implicit using)")


SHUTDOWN_CALL = re.compile(r"Application\.Current\.Shutdown\s*\(\s*\)")


def check_shutdown_unlocks() -> None:
    """Jeder Beenden-Pfad muss vorher entsperren, sonst blockt MainWindow.Closing ihn."""
    for path in sorted(SRC.rglob("*.cs")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        text = path.read_text(encoding="utf-8")
        for match in SHUTDOWN_CALL.finditer(text):
            # 12 Zeilen davor nach einem Unlock()-Aufruf oder einer erklärenden Notiz absuchen.
            start = text.rfind("\n", 0, match.start())
            preceding = text[max(0, start - 1200):match.start()]
            window = "\n".join(preceding.splitlines()[-12:])
            if "Unlock()" in window or "Unlock() ist" in window or "bereits gelaufen" in window:
                continue
            line = text[: match.start()].count("\n") + 1
            report("shutdown-unlock", path,
                   f"Zeile {line}: Application.Current.Shutdown() ohne erkennbares "
                   f"KioskLockService.Unlock() davor -> MainWindow.Closing blockiert das Beenden")


def check_subject_wiring() -> None:
    """Ein neues Subject muss an allen bekannten Stellen mitgepflegt werden."""
    subject_file = SRC / "LernTor.Core" / "Enums" / "Subject.cs"
    if not subject_file.exists():
        return
    raw_body = subject_file.read_text(encoding="utf-8")
    # Kommentare VOR dem Zerlegen entfernen - sonst landen Wörter aus <summary>-Blöcken
    # als vermeintliche Enum-Werte in der Liste.
    raw_body = re.sub(r"/\*.*?\*/", "", raw_body, flags=re.DOTALL)
    raw_body = re.sub(r"//.*", "", raw_body)
    body = raw_body[raw_body.index("{") + 1: raw_body.rindex("}")]
    subjects = [
        name for name in (part.strip().split("=")[0].strip() for part in body.split(","))
        if name.isidentifier()
    ]

    # Fächer ohne eigenen Generator/Stage - bewusste Ausnahmen.
    no_generator = {"News", "Tippen"}
    no_stage = {"News"}

    wiring = {
        "LearningStageSubjects.Map": SRC / "LernTor.Core" / "Services" / "LearningStageSubjects.cs",
        "SubjectToTitleConverter": SRC / "LernTor.App" / "Converters" / "SubjectToTitleConverter.cs",
        "ParentSettings-Toggle": SRC / "LernTor.App" / "ViewModels" / "ParentSettingsViewModel.cs",
    }
    for subject in subjects:
        if subject in no_stage:
            continue
        for label, path in wiring.items():
            if not path.exists():
                continue
            if f"Subject.{subject}" not in path.read_text(encoding="utf-8"):
                report("subject-wiring", path, f"Subject.{subject} fehlt in {label}")

    composer = SRC / "LernTor.ContentGen" / "QuizComposer.cs"
    if composer.exists():
        composer_text = composer.read_text(encoding="utf-8")
        for subject in subjects:
            if subject in no_generator:
                continue
            generators = list((SRC / "LernTor.ContentGen" / "Generators").glob("*.cs"))
            owns_generator = any(
                f"Subject => Subject.{subject}" in g.read_text(encoding="utf-8") for g in generators
            )
            if owns_generator:
                gen_name = next(
                    g.stem for g in generators
                    if f"Subject => Subject.{subject}" in g.read_text(encoding="utf-8")
                )
                if f"new {gen_name}()" not in composer_text:
                    report("subject-wiring", composer,
                           f"{gen_name} fehlt in der Standard-Generatorliste des QuizComposer")


TOPIC_FACTORY_ENTRY = re.compile(r"\[GradeLevel\.(\w+)\]\s*=\s*new List<TopicFactory>\s*\{(.*?)\}", re.DOTALL)
TOPIC_METHOD = re.compile(r"private\s+static\s+QuizQuestion\s+(\w+)\s*\(\s*Random\s+\w+\s*\)")
TUPLE_ARRAY = re.compile(
    r"private\s+static\s+readonly\s+\((?P<fields>[^)]*)\)\[\]\s+(?P<name>\w+)\s*=", re.DOTALL)


def check_generator_consistency() -> None:
    """Fängt die Compile-Fehler, die beim Anlegen neuer Themenpools entstehen.

    Ohne .NET SDK (Netzwerk-Policy dieser Umgebung lässt keine SDK-Installation zu) wäre der
    erste Hinweis sonst ein ~8-minütiger CI-Lauf. Geprüft wird:
      * jede in TopicsByGrade referenzierte TopicFactory existiert als Methode
      * jede Themen-Methode ist auch registriert (sonst toter Pool, der nie ausgespielt wird)
      * Tupel-Feldzugriffe (f.Frage, f.Optionen, ...) passen zur Array-Deklaration
    """
    gen_dir = SRC / "LernTor.ContentGen" / "Generators"
    if not gen_dir.exists():
        return

    for path in sorted(gen_dir.glob("*Generator.cs")):
        text = path.read_text(encoding="utf-8")
        defined = set(TOPIC_METHOD.findall(text))
        if not defined:
            continue

        referenced: set[str] = set()
        for _grade, block in TOPIC_FACTORY_ENTRY.findall(text):
            block = re.sub(r"//.*", "", block)
            for name in (part.strip() for part in block.split(",")):
                if name.isidentifier():
                    referenced.add(name)

        for name in sorted(referenced - defined):
            report("generator-topics", path,
                   f"TopicsByGrade verweist auf '{name}', aber keine Methode "
                   f"'private static QuizQuestion {name}(Random r)' gefunden")

        for name in sorted(defined - referenced):
            report("generator-topics", path,
                   f"Themen-Methode '{name}' ist in keiner TopicsByGrade-Liste registriert "
                   f"- der Pool wird nie ausgespielt")

        # Tupel-Felder: Deklaration mit den tatsächlich benutzten Zugriffen abgleichen.
        for match in TUPLE_ARRAY.finditer(text):
            array_name = match.group("name")
            fields = {
                part.strip().split()[-1]
                for part in match.group("fields").split(",")
                if len(part.strip().split()) >= 2
            }
            # Variable finden, die aus diesem Array zieht: var x = ArrayName[...]
            for var_match in re.finditer(
                    rf"var\s+(\w+)\s*=\s*{re.escape(array_name)}\s*\[", text):
                var = var_match.group(1)
                method_end = text.find("\n    }", var_match.end())
                body = text[var_match.end(): method_end if method_end > 0 else len(text)]
                for used in set(re.findall(rf"\b{re.escape(var)}\.(\w+)", body)):
                    if used not in fields:
                        line = text[: var_match.start()].count("\n") + 1
                        report("generator-tuple", path,
                               f"ab Zeile {line}: '{var}.{used}' passt nicht zu den Feldern von "
                               f"{array_name} ({', '.join(sorted(fields))})")


ORDER_CALL = re.compile(r"\.OrderBy(?:Descending)?\s*\(\s*\w+\s*=>\s*\w+\.(\w+)")
ENTITY_PROP = re.compile(r"public\s+(DateTimeOffset\??)\s+(\w+)\s*\{\s*get")


def _datetimeoffset_properties() -> set[str]:
    """Namen aller Entity-Properties, die wirklich DateTimeOffset sind.

    Wichtig für die Genauigkeit: `ArchivedArticleEntity.ArchivedDate` heißt zwar nach Datum,
    ist aber bewusst ein String ("yyyy-MM-dd"), damit SQLite serverseitig sortieren KANN -
    solche Spalten dürfen nicht gemeldet werden.
    """
    props: set[str] = set()
    entity_dir = SRC / "LernTor.Data" / "Entities"
    if entity_dir.exists():
        for path in entity_dir.glob("*.cs"):
            for match in ENTITY_PROP.finditer(path.read_text(encoding="utf-8")):
                props.add(match.group(2))
    return props


def check_ef_datetimeoffset_ordering() -> None:
    """EF Cores Sqlite-Provider kann OrderBy auf DateTimeOffset nicht übersetzen."""
    repo_dir = SRC / "LernTor.Data" / "Repositories"
    if not repo_dir.exists():
        return
    dto_props = _datetimeoffset_properties()
    if not dto_props:
        return

    for path in sorted(repo_dir.glob("*.cs")):
        text = path.read_text(encoding="utf-8")
        for match in ORDER_CALL.finditer(text):
            prop = match.group(1)
            if prop not in dto_props:
                continue  # z.B. String-Datumsspalten - serverseitig unproblematisch.

            # Umgebenden Methodenkörper bestimmen und darin nach einer Materialisierung suchen:
            # nach ToListAsync()/ToList()/AsEnumerable() liegt die Sortierung im Speicher und
            # ist damit erlaubt (das ist die dokumentierte Lösung dieses Gotchas).
            method_start = max(
                text.rfind("\n    public ", 0, match.start()),
                text.rfind("\n    private ", 0, match.start()),
                text.rfind("\n    internal ", 0, match.start()),
            )
            scope = text[method_start if method_start > 0 else 0: match.start()]
            if any(token in scope for token in ("ToListAsync(", "ToList()", "AsEnumerable()")):
                continue

            line = text[: match.start()].count("\n") + 1
            report("ef-datetimeoffset-order", path,
                   f"Zeile {line}: OrderBy auf DateTimeOffset-Spalte '{prop}' in einer DB-Query "
                   f"-> erst ToListAsync(), dann im Speicher sortieren")


# --------------------------------------------------------------------------------------
# 8: Konfigurationsdateien
# --------------------------------------------------------------------------------------

def check_config_files() -> None:
    state = ROOT / "docs" / "CLAUDE_STATE.json"
    if state.exists():
        try:
            json.loads(state.read_text(encoding="utf-8"))
        except json.JSONDecodeError as exc:
            report("json", state, str(exc))

    try:
        import yaml  # type: ignore
    except ImportError:
        return
    for path in sorted((ROOT / ".github" / "workflows").glob("*.yml")):
        try:
            yaml.safe_load(path.read_text(encoding="utf-8"))
        except Exception as exc:  # noqa: BLE001
            report("yaml", path, str(exc))


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--quick", action="store_true",
                        help="nur schnelle Prüfungen (ohne Repo-weite Verdrahtungs-Scans)")
    args = parser.parse_args()

    checks = [
        ("Klammerbalance (.cs)", check_cs_balance),
        ("XAML-Wohlgeformtheit", check_xaml_wellformed),
        ("Run.Text Mode=OneWay", check_run_text_oneway),
        ("XAML doppelter Style", check_duplicate_style_assignment),
        ("HttpClient-using", check_httpclient_using),
        ("Shutdown/Unlock", check_shutdown_unlocks),
        ("Konfigurationsdateien", check_config_files),
    ]
    if not args.quick:
        checks += [
            ("Subject-Verdrahtung", check_subject_wiring),
            ("Generator-Konsistenz", check_generator_consistency),
            ("EF DateTimeOffset-Sortierung", check_ef_datetimeoffset_ordering),
        ]

    for label, fn in checks:
        before = len(findings)
        fn()
        status = "OK" if len(findings) == before else f"{len(findings) - before} Befund(e)"
        print(f"  {label:32} {status}")

    if findings:
        print("\nBefunde:")
        for f in findings:
            print(f"  - {f}")
        print(f"\n{len(findings)} Befund(e) - bitte VOR dem Push beheben.")
        return 1

    print("\nAlle Vorab-Prüfungen bestanden.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
