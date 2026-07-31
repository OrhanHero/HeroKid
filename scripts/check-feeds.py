#!/usr/bin/env python3
"""Feed-URL-Healthcheck für die kuratierten News-Quellen (siehe docs/STATUS-REPORT.md 3.1).

Liest alle RSS-URLs direkt aus src/LernTor.News/NewsFeedSource.cs (keine doppelte Pflege einer
URL-Liste) und prüft jede per HTTP-Abruf mit denselben Browser-Headern, die auch die App sendet
(RssNewsService.CreateFeedRequest). Ein Feed gilt als gesund, wenn er HTTP 200 liefert und die
Antwort nach RSS/Atom/RDF aussieht. Exit-Code 1, sobald mindestens ein Feed tot ist - der
wöchentliche GitHub-Actions-Lauf (feed-healthcheck.yml) wird dann rot und macht den schleichenden
News-Verfall sichtbar, den im Kiosk-Betrieb sonst niemand bemerkt (die App überspringt tote
Feeds bewusst geräuschlos).

Mit Argumenten prüft das Skript stattdessen genau die übergebenen URLs. Damit lässt sich eine
Ersatz-URL testen, BEVOR sie in den Katalog wandert - aus einer Umgebung ohne Zugriff auf die
Nachrichten-Domains ist der Actions-Lauf die einzige Möglichkeit, eine URL überhaupt zu prüfen:

    python3 scripts/check-feeds.py https://example.org/feed.xml https://example.org/rss
"""

import re
import sys
import urllib.error
import urllib.request
from pathlib import Path

SOURCE_FILE = Path(__file__).resolve().parent.parent / "src" / "LernTor.News" / "NewsFeedSource.cs"

HEADERS = {
    # Identisch zu RssNewsService.CreateFeedRequest - einige Anbieter blocken Default-UAs.
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
    "Accept": "application/rss+xml, application/atom+xml, application/xml, text/xml",
}

FEED_MARKERS = ("<rss", "<feed", "<rdf:RDF", "<RDF")

# HTTP-Codes, die NICHTS über die Feed-URL aussagen: die geteilten GitHub-Runner-IPs laufen bei
# manchen Anbietern in eine Drosselung oder Bot-Sperre, während derselbe Feed vom heimischen
# Anschluss einwandfrei kommt. Als "tot" gemeldet, würden sie den wöchentlichen Lauf dauerhaft
# rot färben - und ein Alarm, der immer an ist, wird ignoriert, also auch dann, wenn wirklich ein
# Feed stirbt. Deshalb eine eigene Kategorie: sichtbar, aber kein Fehlschlag.
THROTTLED_STATUS = (403, 429, 503)


class FeedProblem:
    """Ein Befund zu einer URL - tot oder nur vom Runner aus nicht abrufbar."""

    def __init__(self, message: str, throttled: bool = False):
        self.message = message
        self.throttled = throttled


def extract_feed_urls(source_text: str) -> list[tuple[str, str]]:
    """Liefert (Name, URL)-Paare aus den NewsFeedSource-Konstruktoraufrufen."""
    pattern = re.compile(r'new NewsFeedSource\(\s*"([^"]+)",\s*"([^"]+)"', re.DOTALL)
    return pattern.findall(source_text)


def check_feed(url: str) -> FeedProblem | None:
    """None = gesund, sonst ein Befund."""
    request = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(request, timeout=30) as response:
            if response.status != 200:
                return FeedProblem(f"HTTP {response.status}", response.status in THROTTLED_STATUS)
            body = response.read(65536).decode("utf-8", errors="replace")
            if not any(marker in body for marker in FEED_MARKERS):
                return FeedProblem("Antwort sieht nicht nach RSS/Atom/RDF aus")
            return None
    except urllib.error.HTTPError as ex:
        return FeedProblem(f"HTTPError: {ex}", ex.code in THROTTLED_STATUS)
    except Exception as ex:  # noqa: BLE001 - jede andere Fehlerart bedeutet hier "Feed tot"
        return FeedProblem(f"{type(ex).__name__}: {ex}")


def load_feeds(argv: list[str]) -> list[tuple[str, str]] | None:
    if argv:
        return [(url, url) for url in argv]

    feeds = extract_feed_urls(SOURCE_FILE.read_text(encoding="utf-8"))
    if not feeds:
        print(f"FEHLER: keine Feed-URLs in {SOURCE_FILE} gefunden - Regex/Quellcode geändert?")
        return None

    return feeds


def main(argv: list[str]) -> int:
    feeds = load_feeds(argv)
    if feeds is None:
        return 1

    dead: list[tuple[str, str, str]] = []
    throttled: list[tuple[str, str, str]] = []

    for name, url in feeds:
        problem = check_feed(url)
        if problem is None:
            status = "OK  "
        elif problem.throttled:
            status = "?   "
        else:
            status = "TOT "

        print(f"{status} {name}: {url}" + (f"  -> {problem.message}" if problem else ""))

        if problem is not None:
            (throttled if problem.throttled else dead).append((name, url, problem.message))

    healthy = len(feeds) - len(dead) - len(throttled)
    print(f"\n{healthy}/{len(feeds)} Feeds gesund, {len(throttled)} unklar, {len(dead)} tot.")

    if throttled:
        print("\nVom Runner aus gedrosselt/gesperrt - sagt nichts über die URL aus, kein Fehler:")
        for name, url, problem in throttled:
            print(f"  - {name}: {url} ({problem})")

    if dead:
        print("\nTote Feeds (URL in NewsFeedSource.cs prüfen/ersetzen):")
        for name, url, problem in dead:
            print(f"  - {name}: {url} ({problem})")
        return 1

    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
