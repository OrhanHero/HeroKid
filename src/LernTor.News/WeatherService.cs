using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace LernTor.News;

/// <summary>Kindgerecht aufbereiteter Wetterbericht für das Widget im News-Bereich. Beschreibung
/// und Tipp liegen in beiden App-Sprachen vor; die Oberfläche wählt je nach Spracheinstellung.</summary>
public sealed record KidWeatherReport(
    string Emoji,
    int CurrentTemp,
    int TodayMax,
    int TodayMin,
    int PrecipitationProbability,
    string DescriptionDe,
    string DescriptionTr,
    string TipDe,
    string TipTr);

/// <summary>
/// Aktuelles Berlin-Wetter für das Wetter-Widget im News-Bereich, über die Open-Meteo-API
/// (https://open-meteo.com): kostenlos, ohne API-Schlüssel, ohne Konto - im Rahmen des
/// Kein-Cloud-KI-Prinzips vertretbar, denn es werden (wie beim RSS-Laden) nur öffentliche Daten
/// abgerufen und keinerlei Nutzerdaten gesendet. Schlägt der Abruf fehl (kein Internet, API
/// geändert), liefert der Dienst <c>null</c> und das Widget bleibt einfach ausgeblendet -
/// Wetter ist Beiwerk und darf den Pflicht-News-Teil nie blockieren.
/// </summary>
public sealed class WeatherService
{
    // Feste Berlin-Koordinaten: die App ist ein Kiosk für Berliner Kinder, keine Geolokalisierung.
    private const string BerlinForecastUrl =
        "https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41" +
        "&current=temperature_2m,weather_code" +
        "&daily=temperature_2m_max,temperature_2m_min,precipitation_probability_max" +
        "&timezone=Europe%2FBerlin&forecast_days=1";

    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<KidWeatherReport?> LoadBerlinWeatherAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var json = await _httpClient.GetFromJsonAsync<JsonDocument>(BerlinForecastUrl, cancellationToken);
            if (json is null)
            {
                return null;
            }

            var current = json.RootElement.GetProperty("current");
            var daily = json.RootElement.GetProperty("daily");

            var temp = (int)Math.Round(current.GetProperty("temperature_2m").GetDouble());
            var code = current.GetProperty("weather_code").GetInt32();
            var max = (int)Math.Round(daily.GetProperty("temperature_2m_max")[0].GetDouble());
            var min = (int)Math.Round(daily.GetProperty("temperature_2m_min")[0].GetDouble());
            var rainProbability = daily.GetProperty("precipitation_probability_max")[0].ValueKind == JsonValueKind.Number
                ? daily.GetProperty("precipitation_probability_max")[0].GetInt32()
                : 0;

            var (emoji, descriptionDe, descriptionTr) = DescribeWeatherCode(code);
            var (tipDe, tipTr) = BuildKidTip(max, rainProbability);

            return new KidWeatherReport(
                emoji, temp, max, min, rainProbability, descriptionDe, descriptionTr, tipDe, tipTr);
        }
        catch (Exception ex)
        {
            // Kein Internet/API geändert: Widget still ausblenden statt den News-Teil zu stören.
            LernTor.Core.Logging.AppLog.Warn("Wetter", $"Berlin-Wetter nicht ladbar - {ex.Message}");
            return null;
        }
    }

    /// <summary>WMO-Wettercode (Standard der Open-Meteo-API) → Emoji + einfache Beschreibung.</summary>
    public static (string Emoji, string De, string Tr) DescribeWeatherCode(int code) => code switch
    {
        0 => ("☀️", "Sonnig und klar", "Güneşli ve açık"),
        1 or 2 => ("🌤️", "Meist sonnig, ein paar Wolken", "Çoğunlukla güneşli, biraz bulutlu"),
        3 => ("☁️", "Bewölkt", "Bulutlu"),
        45 or 48 => ("🌫️", "Neblig - alles etwas grau", "Sisli - her yer biraz gri"),
        51 or 53 or 55 or 56 or 57 => ("🌦️", "Leichter Nieselregen", "Hafif çiseleyen yağmur"),
        61 or 63 or 65 or 66 or 67 => ("🌧️", "Regen", "Yağmurlu"),
        71 or 73 or 75 or 77 => ("❄️", "Schneefall", "Kar yağışlı"),
        80 or 81 or 82 => ("🌦️", "Regenschauer zwischendurch", "Ara ara sağanak yağmur"),
        85 or 86 => ("🌨️", "Schneeschauer", "Kar sağanağı"),
        95 or 96 or 99 => ("⛈️", "Gewitter - besser drinnen bleiben", "Gök gürültülü fırtına - içeride kalmak daha iyi"),
        _ => ("🌡️", "Wechselhaftes Wetter", "Değişken hava")
    };

    /// <summary>Kindgerechter Tages-Tipp aus Höchsttemperatur und Regenwahrscheinlichkeit.</summary>
    public static (string De, string Tr) BuildKidTip(int todayMax, int precipitationProbability)
    {
        if (precipitationProbability >= 50)
        {
            return ("Pack lieber eine Regenjacke oder einen Schirm ein!",
                    "Yanına yağmurluk veya şemsiye almayı unutma!");
        }

        return todayMax switch
        {
            >= 28 => ("Richtig heiß heute - viel Wasser trinken und an Sonnencreme denken!",
                      "Bugün çok sıcak - bol su iç ve güneş kremini unutma!"),
            <= 5 => ("Ganz schön kalt - zieh dich warm an mit Mütze und Jacke!",
                     "Epey soğuk - bere ve montunla sıkı giyin!"),
            _ => ("Gutes Wetter, um nach dem Lernen rauszugehen!",
                  "Ders bittikten sonra dışarı çıkmak için güzel bir hava!")
        };
    }
}
