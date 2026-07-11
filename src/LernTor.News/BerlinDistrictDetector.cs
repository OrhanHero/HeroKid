using System.Text.RegularExpressions;

namespace LernTor.News;

/// <summary>
/// Erkennt, welchen Berliner Bezirk ein Artikel betrifft - als bewusst leichtgewichtige
/// Alternative zu einer Kartenansicht (eine Offline-Karte ohne Cloud-Kartendienst wäre im
/// Kiosk unverhältnismäßig aufwendig): Berlin-Artikel bekommen stattdessen einen 📍-Chip mit
/// dem Bezirk, damit Kinder sofort sehen, ob eine Meldung ihren Kiez betrifft.
///
/// <para>Erkannt werden die 12 offiziellen Bezirke sowie bekannte Ortsteile/Kieze, die auf
/// ihren Bezirk abgebildet werden (z.B. "Wedding" → Mitte, "Rummelsburg" → Lichtenberg).
/// Ganze-Wort-Erkennung, damit "Mitte" nicht in "Familienmitte" o.ä. anschlägt; bei mehreren
/// Treffern gewinnt der zuerst im Text genannte.</para>
/// </summary>
public static class BerlinDistrictDetector
{
    /// <summary>Ortsname (Bezirk selbst oder Ortsteil) → offizieller Bezirksname. Public,
    /// damit die Tests (eigene Assembly) die Abdeckung aller 12 Bezirke prüfen können.</summary>
    public static readonly IReadOnlyDictionary<string, string> PlaceToDistrict =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Die 12 Bezirke selbst. "Mitte" allein ist ein Alltagswort ("in der Mitte") und wird
        // deshalb nur in eindeutigen Formen erkannt (siehe unten); dito entfällt der Ortsteil
        // "Buch" (Pankow) komplett - "ein Buch" käme ständig vor, ein ständig falscher
        // 📍-Chip wäre schlimmer als ein fehlender.
        ["Berlin-Mitte"] = "Mitte",
        ["Bezirk Mitte"] = "Mitte",
        ["Friedrichshain-Kreuzberg"] = "Friedrichshain-Kreuzberg",
        ["Pankow"] = "Pankow",
        ["Charlottenburg-Wilmersdorf"] = "Charlottenburg-Wilmersdorf",
        ["Spandau"] = "Spandau",
        ["Steglitz-Zehlendorf"] = "Steglitz-Zehlendorf",
        ["Tempelhof-Schöneberg"] = "Tempelhof-Schöneberg",
        ["Neukölln"] = "Neukölln",
        ["Treptow-Köpenick"] = "Treptow-Köpenick",
        ["Marzahn-Hellersdorf"] = "Marzahn-Hellersdorf",
        ["Lichtenberg"] = "Lichtenberg",
        ["Reinickendorf"] = "Reinickendorf",

        // Bekannte Ortsteile/Kieze → Bezirk
        ["Wedding"] = "Mitte",
        ["Moabit"] = "Mitte",
        ["Tiergarten"] = "Mitte",
        ["Gesundbrunnen"] = "Mitte",
        ["Kreuzberg"] = "Friedrichshain-Kreuzberg",
        ["Friedrichshain"] = "Friedrichshain-Kreuzberg",
        ["Prenzlauer Berg"] = "Pankow",
        ["Weißensee"] = "Pankow",
        ["Karow"] = "Pankow",
        ["Charlottenburg"] = "Charlottenburg-Wilmersdorf",
        ["Wilmersdorf"] = "Charlottenburg-Wilmersdorf",
        ["Grunewald"] = "Charlottenburg-Wilmersdorf",
        ["Westend"] = "Charlottenburg-Wilmersdorf",
        ["Haselhorst"] = "Spandau",
        ["Kladow"] = "Spandau",
        ["Staaken"] = "Spandau",
        ["Siemensstadt"] = "Spandau",
        ["Steglitz"] = "Steglitz-Zehlendorf",
        ["Zehlendorf"] = "Steglitz-Zehlendorf",
        ["Dahlem"] = "Steglitz-Zehlendorf",
        ["Lankwitz"] = "Steglitz-Zehlendorf",
        ["Wannsee"] = "Steglitz-Zehlendorf",
        ["Tempelhof"] = "Tempelhof-Schöneberg",
        ["Schöneberg"] = "Tempelhof-Schöneberg",
        ["Mariendorf"] = "Tempelhof-Schöneberg",
        ["Marienfelde"] = "Tempelhof-Schöneberg",
        ["Lichtenrade"] = "Tempelhof-Schöneberg",
        ["Rudow"] = "Neukölln",
        ["Britz"] = "Neukölln",
        ["Buckow"] = "Neukölln",
        ["Gropiusstadt"] = "Neukölln",
        ["Treptow"] = "Treptow-Köpenick",
        ["Köpenick"] = "Treptow-Köpenick",
        ["Adlershof"] = "Treptow-Köpenick",
        ["Schöneweide"] = "Treptow-Köpenick",
        ["Friedrichshagen"] = "Treptow-Köpenick",
        ["Grünau"] = "Treptow-Köpenick",
        ["Marzahn"] = "Marzahn-Hellersdorf",
        ["Hellersdorf"] = "Marzahn-Hellersdorf",
        ["Biesdorf"] = "Marzahn-Hellersdorf",
        ["Kaulsdorf"] = "Marzahn-Hellersdorf",
        ["Mahlsdorf"] = "Marzahn-Hellersdorf",
        ["Hohenschönhausen"] = "Lichtenberg",
        ["Karlshorst"] = "Lichtenberg",
        ["Rummelsburg"] = "Lichtenberg",
        ["Fennpfuhl"] = "Lichtenberg",
        ["Tegel"] = "Reinickendorf",
        ["Frohnau"] = "Reinickendorf",
        ["Hermsdorf"] = "Reinickendorf",
        ["Wittenau"] = "Reinickendorf",
        ["Märkisches Viertel"] = "Reinickendorf",
    };

    /// <summary>
    /// Liefert den zuerst im Text genannten Bezirk oder <c>null</c>, wenn kein Berliner
    /// Ortsname vorkommt (Ganze-Wort-Prüfung, case-insensitive; bei mehreren Treffern
    /// gewinnt der zuerst genannte).
    /// </summary>
    public static string? Detect(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        string? best = null;
        var bestPosition = int.MaxValue;

        foreach (var (place, district) in PlaceToDistrict)
        {
            var match = Regex.Match(text, $@"\b{Regex.Escape(place)}\b", RegexOptions.IgnoreCase);
            if (match.Success && match.Index < bestPosition)
            {
                bestPosition = match.Index;
                best = district;
            }
        }

        return best;
    }
}
