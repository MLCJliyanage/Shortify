namespace Shortify.Core.Urls.List;

public record ListUrlsResponse(IEnumerable<UrlItem> Urls);

public record UrlItem(string ShortUrl, string LongUrl, DateTimeOffset CreatedOn);