using System.Collections.Immutable;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record KnowledgeDocumentLink(string Url, string CanonicalUrl);

public sealed class KnowledgeCrawler(HttpClient client, TimeSpan? minimumDelay = null)
{
    private readonly HttpClient client = client ?? throw new ArgumentNullException(nameof(client));
    private readonly TimeSpan minimumDelay = minimumDelay ?? TimeSpan.FromMilliseconds(250);
    private DateTimeOffset lastRequest = DateTimeOffset.MinValue;

    public async Task<ImmutableArray<KnowledgeDocumentLink>> FetchLinksAsync(Uri feed,
        CancellationToken cancellationToken = default)
    {
        var wait = minimumDelay - (DateTimeOffset.UtcNow - lastRequest);
        if (wait > TimeSpan.Zero) await Task.Delay(wait, cancellationToken);
        lastRequest = DateTimeOffset.UtcNow;
        using var response = await client.GetAsync(feed, cancellationToken);
        response.EnsureSuccessStatusCode();
        return ParseLinks(await response.Content.ReadAsStringAsync(cancellationToken), feed);
    }

    public static ImmutableArray<KnowledgeDocumentLink> ParseLinks(string xml, Uri baseUri)
    {
        ArgumentNullException.ThrowIfNull(xml);
        ArgumentNullException.ThrowIfNull(baseUri);
        var document = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
        var links = document.Descendants().Where(element => element.Name.LocalName is "loc" or "link")
            .Select(element => (string?)element.Attribute("href") ?? element.Value.Trim())
            .Where(value => Uri.TryCreate(baseUri, value, out _))
            .Select(value => new Uri(baseUri, value!)).Where(uri => uri.Scheme is "http" or "https")
            .Select(uri => new KnowledgeDocumentLink(uri.AbsoluteUri, Canonical(uri)))
            .GroupBy(link => link.CanonicalUrl, StringComparer.OrdinalIgnoreCase).Select(group => group.First())
            .ToImmutableArray();
        return links;
    }

    private static string Canonical(Uri uri)
        => new UriBuilder(uri) { Fragment = "", Query = uri.Query }.Uri.AbsoluteUri.TrimEnd('/');
}
