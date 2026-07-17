using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LernTor.Core.Logging;

namespace LernTor.News;

/// <summary>
/// Geräuschloser Offline-Cache für die ROHEN Feed-Inhalte (RSS/Atom/RDF-Bytes, eine Datei pro
/// Feed-URL): jeder erfolgreiche Abruf überschreibt den Cache-Eintrag, und schlägt ein Abruf
/// fehl (kein Internet, Feed down), liefert <see cref="TryLoad"/> den letzten Stand zurück,
/// sofern er nicht älter als <see cref="MaxAge"/> (48h) ist - so bleibt der Lernablauf des
/// Kindes auch offline intakt, statt mit leerer News-Stufe zu hängen.
///
/// <para>Bewusst werden die ROH-Bytes gecacht statt fertiger <c>NewsArticle</c>-Objekte: der
/// Fallback läuft dadurch durch exakt dieselbe Parse-/Vereinfachungs-/Fragen-Pipeline wie ein
/// Live-Abruf (<c>RssNewsService.ParseFeedContent</c> + <c>BuildArticle</c>), inklusive
/// Klassenstufen-abhängiger Vereinfachung - ein Cache fertiger Artikel würde die Vereinfachung
/// der falschen Klassenstufe einfrieren, wenn zwischenzeitlich das Profil wechselt.</para>
///
/// <para>Format: GZip-komprimierter JSON-Umschlag (URL, Zeitstempel, Inhalt) unter
/// <c>%LOCALAPPDATA%\LernTor\newscache\&lt;SHA256(url)&gt;.json.gz</c>. Alle IO-Fehler werden
/// verschluckt und nur ins Protokoll geschrieben - ein kaputter Cache darf weder den Live-Abruf
/// noch den Fallback crashen (gleiche Philosophie wie <see cref="AppLog"/> selbst).</para>
/// </summary>
public sealed class FeedCache
{
    /// <summary>Maximales Alter eines Cache-Eintrags für den Offline-Fallback.</summary>
    public static readonly TimeSpan MaxAge = TimeSpan.FromHours(48);

    private readonly string _cacheDirectory;

    public FeedCache()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LernTor", "newscache"))
    {
    }

    /// <summary>Testkonstruktor mit eigenem Cache-Verzeichnis (z.B. Temp-Ordner).</summary>
    public FeedCache(string cacheDirectory)
    {
        _cacheDirectory = cacheDirectory;
    }

    private sealed record CacheEnvelope(string RssUrl, DateTimeOffset SavedAtUtc, byte[] Content);

    public void Save(string rssUrl, byte[] content)
    {
        try
        {
            Directory.CreateDirectory(_cacheDirectory);
            var envelope = new CacheEnvelope(rssUrl, DateTimeOffset.UtcNow, content);

            using var file = File.Create(PathFor(rssUrl));
            using var gzip = new GZipStream(file, CompressionLevel.Fastest);
            JsonSerializer.Serialize(gzip, envelope);
        }
        catch (Exception ex)
        {
            AppLog.Warn("News", $"Feed-Cache konnte nicht geschrieben werden ({rssUrl}): {ex.Message}");
        }
    }

    /// <summary>
    /// Liefert die gecachten Roh-Bytes des Feeds oder null, wenn kein Eintrag existiert, der
    /// Eintrag älter als <see cref="MaxAge"/> ist oder die Datei nicht lesbar/kaputt ist.
    /// </summary>
    /// <param name="now">Nur für Tests: "Jetzt"-Zeitpunkt für die Altersprüfung.</param>
    public byte[]? TryLoad(string rssUrl, DateTimeOffset? now = null)
    {
        try
        {
            var path = PathFor(rssUrl);
            if (!File.Exists(path))
            {
                return null;
            }

            using var file = File.OpenRead(path);
            using var gzip = new GZipStream(file, CompressionMode.Decompress);
            var envelope = JsonSerializer.Deserialize<CacheEnvelope>(gzip);

            if (envelope is null || (now ?? DateTimeOffset.UtcNow) - envelope.SavedAtUtc > MaxAge)
            {
                return null;
            }

            return envelope.Content;
        }
        catch (Exception ex)
        {
            AppLog.Warn("News", $"Feed-Cache konnte nicht gelesen werden ({rssUrl}): {ex.Message}");
            return null;
        }
    }

    private string PathFor(string rssUrl) => Path.Combine(
        _cacheDirectory,
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rssUrl))) + ".json.gz");
}
