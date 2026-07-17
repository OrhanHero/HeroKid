#!/usr/bin/env python3
"""Feed-URL-Healthcheck für die kuratierten News-Quellen (siehe docs/STATUS-REPORT.md 3.1).

Liest alle RSS-URLs direkt aus src/LernTor.News/NewsFeedSource.cs (keine doppelte Pflege einer
URL-Liste) und prüft jede per HTTP-Abruf mit denselben Browser-Headern, die auch die App sendet
(RssNewsService.CreateFeedRequest). Ein Feed gilt als gesund, wenn er HTTP 200 liefert und die
Antwort nach RSS/Atom/RDF aussieht. Exit-Code 1, sobald mindestens ein Feed tot ist - der
wöchentliche GitHub-Actions-Lauf (feed-healthcheck.yml) wird dann rot und macht den schleichenden
News-Verfall sichtbar, den im Kiosk-Betrieb sonst niemand bemerkt (die App überspringt tote
Feeds bewusst geräuschlos).
"""

import re
import sys
import urllib.request
from pathlib import Path

SOURCE_FILE = Path(__file__).resolve().parent.parent / "src" / "LernTor.News" / "NewsFeedSource.cs"

HEADERS = {
    # Identisch zu RssNewsService.CreateFeedRequest - einige Anbieter blocken Default-UAs.
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
    "Accept": "application/rss+xml, application/atom+xml, application/xml, text/xml",
}

FEED_MARKERS = ("<rss", "<feed", "<rdf:RDF", "<RDF")


def extract_feed_urls(source_text: str) -> list[tuple[str, str]]:
    """Liefert (Name, URL)-Paare aus den NewsFeedSource-Konstruktoraufrufen."""
    pattern = re.compile(r'new NewsFeedSource\(\s*"([^"]+)",\s*"([^"]+)"', re.DOTALL)
    return pattern.findall(source_text)


def check_feed(url: str) -> str | None:
    """None = gesund, sonst eine kurze Fehlerbeschreibung."""
    request = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(request, timeout=30) as response:
            if response.status != 200:
                return f"HTTP {response.status}"
            body = response.read(65536).decode("utf-8", errors="replace")
            if not any(marker in body for marker in FEED_MARKERS):
                return "Antwort sieht nicht nach RSS/Atom/RDF aus"
            return None
    except Exception as ex:  # noqa: BLE001 - jede Fehlerart bedeutet hier "Feed tot"
        return f"{type(ex).__name__}: {ex}"


def main() -> int:
    feeds = extract_feed_urls(SOURCE_FILE.read_text(encoding="utf-8"))
    if not feeds:
        print(f"FEHLER: keine Feed-URLs in {SOURCE_FILE} gefunden - Regex/Quellcode geändert?")
        return 1

    dead = []
    for name, url in feeds:
        problem = check_feed(url)
        status = "OK  " if problem is None else "TOT "
        print(f"{status} {name}: {url}" + (f"  -> {problem}" if problem else ""))
        if problem is not None:
            dead.append((name, url, problem))

    print(f"\n{len(feeds) - len(dead)}/{len(feeds)} Feeds gesund.")
    if dead:
        print("\nTote Feeds (URL in NewsFeedSource.cs prüfen/ersetzen):")
        for name, url, problem in dead:
            print(f"  - {name}: {url} ({problem})")
        return 1

    return 0


if __name__ == "__main__":
    sys.exit(main())
