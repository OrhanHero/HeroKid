using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Ordnet dem Fachnamen auf einem Stundenplan - Kürzel wie "Ma" oder ausgeschrieben wie
/// "Mathe" - ein LernTor-Fach zu, soweit es eines gibt.
///
/// <para><b>Die Zuordnung darf danebenliegen.</b> Sie entscheidet ausschließlich über das
/// Symbol vor der Zeile. Angezeigt wird immer der Text, den die Eltern eingetragen haben, also
/// genau das, was auf dem Plan der Schule steht. Deshalb ist "NaWi" hier bewusst KEIN Fach:
/// Naturwissenschaften umfassen Biologie, Chemie und Physik zugleich, und eines davon
/// herauszupicken wäre geraten. Solche Fächer bekommen das neutrale Buch-Symbol.</para>
///
/// <para>Abgeglichen wird über die vollständige, kleingeschriebene Bezeichnung - kein
/// Anfangsbuchstaben-Raten. "Ge" ist Geschichte, "Geo" ist Geografie, "GeWi" ist
/// Gesellschaftswissenschaften; ein Präfixvergleich hätte alle drei durcheinandergebracht.</para>
/// </summary>
public static class TimetableSubjectMap
{
    private static readonly IReadOnlyDictionary<string, Subject> Bekannt =
        new Dictionary<string, Subject>(StringComparer.OrdinalIgnoreCase)
        {
            ["ma"] = Subject.Mathematik,
            ["mat"] = Subject.Mathematik,
            ["mathe"] = Subject.Mathematik,
            ["mathematik"] = Subject.Mathematik,

            ["de"] = Subject.Deutsch,
            ["deu"] = Subject.Deutsch,
            ["deutsch"] = Subject.Deutsch,

            ["e"] = Subject.Englisch,
            ["en"] = Subject.Englisch,
            ["eng"] = Subject.Englisch,
            ["englisch"] = Subject.Englisch,

            ["tü"] = Subject.Tuerkisch,
            ["tue"] = Subject.Tuerkisch,
            ["tür"] = Subject.Tuerkisch,
            ["türkisch"] = Subject.Tuerkisch,
            ["tuerkisch"] = Subject.Tuerkisch,

            ["bi"] = Subject.Biologie,
            ["bio"] = Subject.Biologie,
            ["biologie"] = Subject.Biologie,

            ["ch"] = Subject.Chemie,
            ["che"] = Subject.Chemie,
            ["chemie"] = Subject.Chemie,

            ["ph"] = Subject.Physik,
            ["phy"] = Subject.Physik,
            ["physik"] = Subject.Physik,

            ["ge"] = Subject.Geschichte,
            ["gesch"] = Subject.Geschichte,
            ["geschichte"] = Subject.Geschichte,

            ["gewi"] = Subject.Gewi,
            ["gesellschaftswissenschaften"] = Subject.Gewi,

            ["pb"] = Subject.Politik,
            ["pol"] = Subject.Politik,
            ["politik"] = Subject.Politik,
            ["politische bildung"] = Subject.Politik,
            ["sowi"] = Subject.Politik,

            ["geo"] = Subject.Geo,
            ["geografie"] = Subject.Geo,
            ["geographie"] = Subject.Geo,
            ["erdkunde"] = Subject.Geo,

            ["et"] = Subject.Ethik,
            ["eth"] = Subject.Ethik,
            ["ethik"] = Subject.Ethik,

            ["ku"] = Subject.Kunst,
            ["kun"] = Subject.Kunst,
            ["kunst"] = Subject.Kunst,

            ["mu"] = Subject.Musik,
            ["mus"] = Subject.Musik,
            ["musik"] = Subject.Musik,

            ["itg"] = Subject.Itg,
            ["inf"] = Subject.Itg,
            ["informatik"] = Subject.Itg
        };

    /// <summary>Das LernTor-Fach zu einer Stundenplan-Bezeichnung, oder <c>null</c>, wenn es
    /// keines gibt (Sport, NaWi, WPU Spanisch, Klassenrat …).</summary>
    public static Subject? TryMap(string? label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return null;
        }

        var schluessel = label.Trim();
        return Bekannt.TryGetValue(schluessel, out var fach) ? fach : null;
    }

    /// <summary>Symbol für die Zeile im Stundenplan. Ohne erkanntes Fach ein neutrales Buch -
    /// nie ein geratenes Symbol.</summary>
    public static string IconFor(string? label) => TryMap(label) switch
    {
        Subject.Mathematik => "🔢",
        Subject.Deutsch => "📖",
        Subject.Tuerkisch => "🇹🇷",
        Subject.Englisch => "🇬🇧",
        Subject.Biologie => "🌿",
        Subject.Chemie => "🧪",
        Subject.Physik => "🔬",
        Subject.Geschichte => "🏛️",
        Subject.Gewi => "🌍",
        Subject.Politik => "🏛️",
        Subject.Geo => "🗺️",
        Subject.Ethik => "🤝",
        Subject.Kunst => "🎨",
        Subject.Musik => "🎵",
        Subject.Itg => "💻",
        _ => "📓"
    };
}
