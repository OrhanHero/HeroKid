using System.Xml;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

public sealed class RssNewsServiceTests
{
    [Fact]
    public void CreateFeedRequest_sets_browser_like_user_agent_and_accept_headers()
    {
        var request = RssNewsService.CreateFeedRequest("https://example.com/feed.xml");

        Assert.True(request.Headers.UserAgent.Any());
        Assert.Contains(request.Headers.UserAgent, value => value.ToString().Contains("Mozilla/5.0"));
        Assert.Contains(request.Headers.Accept, value => value.MediaType == "application/rss+xml");
        Assert.Contains(request.Headers.Accept, value => value.MediaType == "application/atom+xml");
    }

    [Fact]
    public void ParseFeedReader_supports_standard_rss_and_dtd_entities()
    {
        const string xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <!DOCTYPE rss [
              <!ENTITY feedTitle "RSS Nachrichten">
            ]>
            <rss version="2.0">
              <channel>
                <title>&feedTitle;</title>
                <link>https://example.com</link>
                <description>Testfeed</description>
                <item>
                  <title>RSS Artikel</title>
                  <link>https://example.com/article</link>
                  <description>Inhalt mit Entität &feedTitle;</description>
                  <pubDate>Wed, 16 Jul 2026 10:15:00 GMT</pubDate>
                </item>
              </channel>
            </rss>
            """;

        var content = System.Text.Encoding.UTF8.GetBytes(xml);

        var items = RssNewsService.ParseFeedContent(content);

        Assert.Single(items);
        Assert.Equal("RSS Artikel", items[0].Title?.Text);
        Assert.Contains("RSS Nachrichten", items[0].Summary?.Text);
        Assert.Equal("https://example.com/article", items[0].Links.First().Uri.ToString());
    }

    [Fact]
    public void ParseFeedReader_supports_atom_feeds()
    {
        const string xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
              <title>Atom Feed</title>
              <id>https://example.com/feed</id>
              <updated>2026-07-16T10:15:00Z</updated>
              <entry>
                <title>Atom Artikel</title>
                <id>https://example.com/article-1</id>
                <updated>2026-07-16T10:15:00Z</updated>
                <summary>Atom Inhalt</summary>
                <link href="https://example.com/article-1" />
              </entry>
            </feed>
            """;

        var content = System.Text.Encoding.UTF8.GetBytes(xml);

        var items = RssNewsService.ParseFeedContent(content);

        Assert.Single(items);
        Assert.Equal("Atom Artikel", items[0].Title?.Text);
        Assert.Equal("Atom Inhalt", items[0].Summary?.Text);
        Assert.Equal("https://example.com/article-1", items[0].Links.First().Uri.ToString());
    }

    [Fact]
    public void ParseFeedReader_supports_rdf_feeds()
    {
        const string xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
                     xmlns="http://purl.org/rss/1.0/">
              <channel>
                <title>RDF Feed</title>
                <link>https://example.com</link>
                <description>Testfeed</description>
              </channel>
              <item>
                <title>RDF Artikel</title>
                <link>https://example.com/rdf-article</link>
                <description>RDF Inhalt</description>
                <pubDate>Wed, 16 Jul 2026 10:15:00 GMT</pubDate>
              </item>
            </rdf:RDF>
            """;

        var content = System.Text.Encoding.UTF8.GetBytes(xml);

        var items = RssNewsService.ParseFeedContent(content);

        Assert.Single(items);
        Assert.Equal("RDF Artikel", items[0].Title?.Text);
        Assert.Equal("RDF Inhalt", items[0].Summary?.Text);
        Assert.Equal("https://example.com/rdf-article", items[0].Links.First().Uri.ToString());
    }
}
