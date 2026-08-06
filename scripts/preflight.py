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
  9. Zwei Typen gleichen Namens im selben Namensraum (CS0101)
 10. Vollstaendigkeit der Einordnungstexte bei neuen NewsCategory-Werten
 11. Uebersetzungsschluessel, die benutzt, aber nirgends definiert sind -> "[Stage_News]" auf dem Schirm
 12. Lambda in einem struct-Member, die auf ein eigenes Feld/Property zugreift (CS1673)

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


LAMBDA_DISCARD = re.compile(
    r"\(\s*(?:\w+\s*,\s*)*_\s*(?:,\s*\w+\s*)*\)\s*=>\s*_\s*=[^=]"
)


def check_lambda_discard_shadowing() -> None:
    """`_` als Lambda-Parameter UND als Verwerfen-Platzhalter im selben Ausdruck.

    In dieser Codebasis ist `_ = IrgendwasAsync()` das uebliche Muster fuer bewusst nicht
    abgewartete Tasks. Heisst ein Lambda-Parameter ebenfalls `_`, ist `_ =` im Rumpf keine
    Verwerfung mehr, sondern eine ZUWEISUNG an diesen Parameter - und der Compiler meldet
    "CS0029: Cannot implicitly convert type 'Task' to 'int'". Das ist genau einmal passiert
    (Fuehrerschein-Bereich, Callback der Challenge) und kostete eine volle CI-Runde.
    """
    for path in sorted(SRC.rglob("*.cs")) + sorted(TESTS.rglob("*.cs")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        text = path.read_text(encoding="utf-8")
        for match in LAMBDA_DISCARD.finditer(text):
            line = text[: match.start()].count("\n") + 1
            report("lambda-discard", path,
                   f"Zeile {line}: '_' ist hier Lambda-Parameter, '_ =' im Rumpf ist deshalb "
                   f"eine Zuweisung an ihn statt ein Verwerfen -> CS0029. Parameter umbenennen.")


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
    no_generator = {"News", "Tippen", "Fuehrerschein"}
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

def check_duplicate_type_names() -> None:
    """Zwei Typen gleichen Namens im selben Namensraum (CS0101).

    Kostet sonst einen kompletten CI-Durchlauf: der Fehler faellt erst beim Kompilieren auf, und
    er entsteht leicht, weil Testklassen nicht zwingend in einer gleichnamigen Datei stehen -
    NewsCategoryClassifierTests lag in KidNewsEnrichmentTests.cs, eine Dateisuche fand sie also
    nicht.
    """
    namespace_pattern = re.compile(r"^\s*namespace\s+([\w.]+)\s*[;{]", re.MULTILINE)
    type_pattern = re.compile(
        r"^\s*(?:public|internal)\s+(?:sealed\s+|static\s+|abstract\s+|partial\s+)*"
        r"(?:class|record|struct|interface|enum)\s+(\w+)",
        re.MULTILINE)

    seen: dict[tuple[str, str], list[str]] = {}

    for path in sorted(SRC.rglob("*.cs")) + sorted(TESTS.rglob("*.cs")):
        if "/obj/" in str(path) or "/bin/" in str(path):
            continue
        text = path.read_text(encoding="utf-8")
        namespace_match = namespace_pattern.search(text)
        namespace = namespace_match.group(1) if namespace_match else "<global>"

        for name in set(type_pattern.findall(text)):
            # partial: dieselbe Klasse darf bewusst auf mehrere Dateien verteilt sein.
            if re.search(rf"partial\s+(?:class|record|struct)\s+{re.escape(name)}\b", text):
                continue
            seen.setdefault((namespace, name), []).append(str(path.relative_to(ROOT)))

    for (namespace, name), paths in sorted(seen.items()):
        if len(paths) > 1:
            findings.append(
                f"CS0101: '{name}' ist in '{namespace}' mehrfach definiert - "
                + ", ".join(sorted(paths)))


def check_news_category_coverage() -> None:
    """Jede NewsCategory braucht Einordnungstexte fuer beide Textvarianten (Klasse 6 / Klasse 9).

    Beim Ergaenzen der Rubriken Wissen und Sport blieben KidNewsMetadata.WhyImportantFor und
    MeaningForKidsFor unveraendert - der Fallback "_ => string.Empty" schluckt das lautlos, und
    erst ein bestehender Test in der CI meldete es. Dieselbe Falle wie bei den Faechern (siehe
    check_subject_wiring): ein neuer Enum-Wert ist nie nur ein Enum-Wert.
    """
    enum_file = SRC / "LernTor.Core" / "Models" / "NewsArticle.cs"
    metadata_file = SRC / "LernTor.News" / "KidNewsMetadata.cs"
    if not enum_file.exists() or not metadata_file.exists():
        return

    enum_body = enum_file.read_text(encoding="utf-8").split("enum NewsCategory")[1].split("}")[0]
    categories = [
        name for name in re.findall(r"^\s+(\w+),?\s*$", enum_body, re.MULTILINE)
        if not name.startswith("//")
    ]

    metadata = metadata_file.read_text(encoding="utf-8")
    for method in ("WhyImportantFor", "MeaningForKidsFor"):
        parts = metadata.split(f"public static string {method}")
        if len(parts) < 2:
            findings.append(f"KidNewsMetadata.{method} nicht gefunden - Methode umbenannt?")
            continue

        block = parts[1].split("_ => string.Empty")[0]
        pairs = set(re.findall(r"\(NewsCategory\.(\w+), GradeLevel\.(\w+)\)", block))

        for category in categories:
            for variant in ("Klasse6", "Klasse9"):
                if (category, variant) not in pairs:
                    findings.append(
                        f"KidNewsMetadata.{method}: Rubrik '{category}' hat keinen Text "
                        f"fuer {variant} - faellt still auf string.Empty zurueck.")


STRUCT_DECL = re.compile(
    r"^\s*(?:public|internal|private|protected)?[\w\s]*?\b(?:readonly\s+)?(?:record\s+)?struct\s+"
    r"(\w+)\s*(?:\(([^)]*)\))?", re.M)


def _struct_bodies(text: str):
    """(Name, Positionsparameter, Rumpftext) je struct-Deklaration mit Rumpf."""
    for match in STRUCT_DECL.finditer(text):
        start = text.find("{", match.end())
        if start == -1:
            continue

        # Positionsbasierte structs ohne Rumpf enden mit ';'. Ohne diese Pruefung wuerde der
        # naechste Block der Datei - meist eine ganz andere Klasse - als Rumpf gelesen, und
        # die Pruefung meldete reihenweise Unsinn.
        if ";" in text[match.end():start]:
            continue

        depth = 0
        for i in range(start, len(text)):
            if text[i] == "{":
                depth += 1
            elif text[i] == "}":
                depth -= 1
                if depth == 0:
                    yield match.group(1), match.group(2) or "", text[start:i]
                    break


def check_struct_lambda_capture() -> None:
    """Lambdas in struct-Membern duerfen nicht auf eigene Felder/Properties zugreifen.

    CS1673: "Anonymous methods, lambda expressions ... inside structs cannot access instance
    members of 'this'." Ein Compilerfehler, also nur in der CI sichtbar - und genau daran ist
    der Build einmal gescheitert (PresentedQuestion.CorrectAnswers in TheoryQuestionPresenter).
    Die Loesung ist immer dieselbe: das Member vorher in eine lokale Variable kopieren.
    """
    for path in sorted(SRC.rglob("*.cs")):
        text = path.read_text(encoding="utf-8")

        for name, positional, body in _struct_bodies(text):
            members = set(re.findall(r"\b(\w+)\s*(?:,|$)", positional))
            members |= set(re.findall(
                r"^\s*public\s+(?!static\b)[\w<>?\[\],\s]+?\b(\w+)\s*(?:=>|\{\s*get)", body, re.M))
            members = {m for m in members if m and m[0].isupper()}

            if not members:
                continue

            # Jedes "=>" einzeln ansehen. Nicht mit einem Regex ueber die ganze Zeile: bei
            # "public X Y => Liste.Select(i => Member[i]);" verschluckt der aeussere Pfeil
            # sonst den inneren, und genau der innere ist der Fehler.
            for pfeil in re.finditer(r"=>", body):
                davor = body[:pfeil.start()].rstrip()

                # Parameterliste der Lambda ueberspringen: "x" oder "(x, y)".
                if davor.endswith(")"):
                    tiefe, i = 0, len(davor) - 1
                    while i >= 0:
                        if davor[i] == ")":
                            tiefe += 1
                        elif davor[i] == "(":
                            tiefe -= 1
                            if tiefe == 0:
                                break
                        i -= 1
                    davor = davor[:i].rstrip()
                else:
                    davor = re.sub(r"\w+$", "", davor).rstrip()

                # Eine Lambda steht als Argument da, also hinter "(" oder ",". Ein
                # ausdrucksbasiertes Member (public string X => ...) tut das nicht.
                if not davor.endswith(("(", ",")):
                    continue

                rumpf = body[pfeil.end():].split("\n", 1)[0]

                for member in sorted(members):
                    if re.search(rf"\b{re.escape(member)}\b", rumpf):
                        report("struct-lambda", path,
                               f"struct {name}: Lambda greift auf '{member}' zu -> CS1673. "
                               "Member vorher in eine lokale Variable kopieren.")
                        break


def check_translation_keys() -> None:
    """Benutzte, aber nicht definierte Uebersetzungsschluessel.

    Der Indexer von LocalizationService gibt fuer einen unbekannten Schluessel "[Schluessel]"
    zurueck - kein Absturz, keine Warnung, nur ein eckig geklammerter Bezeichner mitten in der
    Oberflaeche. Genau so stand "[Stage_News]" als Ueberschrift ueber der Nachrichtenansicht,
    ohne dass es jemandem auffiel.
    """
    translations = SRC / "LernTor.App" / "Localization" / "Translations.cs"
    if not translations.exists():
        return

    defined = set(re.findall(r'\["([A-Za-z0-9_]+)"\]\s*=\s*L\(',
                            translations.read_text(encoding="utf-8")))
    if not defined:
        return

    app = SRC / "LernTor.App"

    for path in sorted(app.rglob("*.xaml")):
        used = set(re.findall(r"Path=\[([A-Za-z0-9_]+)\]", path.read_text(encoding="utf-8")))
        for key in sorted(used - defined):
            report("uebersetzung-fehlt", path, f"'{key}' wird gebunden, steht aber nicht in Translations.Map")

    for path in sorted(app.rglob("*.cs")):
        if path.name == "Translations.cs":
            continue
        text = path.read_text(encoding="utf-8")
        used = set(re.findall(r'LocalizationService\.Instance\["([A-Za-z0-9_]+)"\]', text))
        for key in sorted(used - defined):
            report("uebersetzung-fehlt", path, f"'{key}' wird abgefragt, steht aber nicht in Translations.Map")


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
        ("Lambda-Verwerfen", check_lambda_discard_shadowing),
        ("Konfigurationsdateien", check_config_files),
        ("Doppelte Typnamen", check_duplicate_type_names),
        ("News-Rubrik-Texte", check_news_category_coverage),
        ("Uebersetzungsschluessel", check_translation_keys),
        ("struct-Lambda (CS1673)", check_struct_lambda_capture),
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
