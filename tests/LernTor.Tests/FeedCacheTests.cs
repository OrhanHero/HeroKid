using System.Text;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

public sealed class FeedCacheTests : IDisposable
{
    private readonly string _cacheDirectory =
        Path.Combine(Path.GetTempPath(), "lerntor-feedcache-tests", Guid.NewGuid().ToString("N"));

    private readonly FeedCache _cache;

    public FeedCacheTests()
    {
        _cache = new FeedCache(_cacheDirectory);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_cacheDirectory, recursive: true);
        }
        catch (IOException)
        {
            // Deckt auch DirectoryNotFoundException ab - Aufräumen ist Best-Effort.
        }
    }

    [Fact]
    public void Save_then_TryLoad_returns_identical_content()
    {
        var content = Encoding.UTF8.GetBytes("<rss><channel><title>Test</title></channel></rss>");

        _cache.Save("https://example.com/feed.xml", content);
        var loaded = _cache.TryLoad("https://example.com/feed.xml");

        Assert.NotNull(loaded);
        Assert.Equal(content, loaded);
    }

    [Fact]
    public void TryLoad_returns_null_for_unknown_url()
    {
        Assert.Null(_cache.TryLoad("https://example.com/never-saved.xml"));
    }

    [Fact]
    public void TryLoad_returns_null_once_entry_is_older_than_48_hours()
    {
        var content = Encoding.UTF8.GetBytes("<rss />");
        _cache.Save("https://example.com/feed.xml", content);

        var justInsideWindow = DateTimeOffset.UtcNow + FeedCache.MaxAge - TimeSpan.FromMinutes(5);
        var justOutsideWindow = DateTimeOffset.UtcNow + FeedCache.MaxAge + TimeSpan.FromMinutes(5);

        Assert.NotNull(_cache.TryLoad("https://example.com/feed.xml", justInsideWindow));
        Assert.Null(_cache.TryLoad("https://example.com/feed.xml", justOutsideWindow));
    }

    [Fact]
    public void TryLoad_returns_null_for_corrupted_cache_file_instead_of_throwing()
    {
        var content = Encoding.UTF8.GetBytes("<rss />");
        _cache.Save("https://example.com/feed.xml", content);

        // Einzige Datei im Cache-Verzeichnis ist der eben geschriebene Eintrag - kaputtschreiben.
        var cacheFile = Directory.GetFiles(_cacheDirectory).Single();
        File.WriteAllBytes(cacheFile, new byte[] { 0x00, 0x01, 0x02 });

        Assert.Null(_cache.TryLoad("https://example.com/feed.xml"));
    }

    [Fact]
    public void Save_overwrites_previous_entry_for_same_url()
    {
        _cache.Save("https://example.com/feed.xml", Encoding.UTF8.GetBytes("alt"));
        _cache.Save("https://example.com/feed.xml", Encoding.UTF8.GetBytes("neu"));

        var loaded = _cache.TryLoad("https://example.com/feed.xml");

        Assert.Equal("neu", Encoding.UTF8.GetString(loaded!));
    }

    [Fact]
    public void Entries_for_different_urls_do_not_collide()
    {
        _cache.Save("https://example.com/a.xml", Encoding.UTF8.GetBytes("Feed A"));
        _cache.Save("https://example.com/b.xml", Encoding.UTF8.GetBytes("Feed B"));

        Assert.Equal("Feed A", Encoding.UTF8.GetString(_cache.TryLoad("https://example.com/a.xml")!));
        Assert.Equal("Feed B", Encoding.UTF8.GetString(_cache.TryLoad("https://example.com/b.xml")!));
    }
}
