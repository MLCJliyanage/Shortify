using Shortify.Core.Urls;
using Shortify.Core.Urls.List;

namespace Shortify.Core.Extensions;

public static class UrlResponseExtensions
{
    public static ListUrlsResponse MapToResponse(this UserUrls urls, RedirectLinkBuilder redirectLinkBuilder)
    {
        var urlItems = urls.Urls.Select(url => url.MapToResponse(redirectLinkBuilder)).ToArray();
        return new ListUrlsResponse(urlItems, urls.ContinuationToken);
    }

    private static UrlItem MapToResponse(this UserUrlItem url, RedirectLinkBuilder redirectLinkBuilder)
    {
        return new UrlItem(
            url.ShortUrl,
            redirectLinkBuilder.LinkTo(url.ShortUrl),
            new Uri(url.LongUrl),
            url.CreatedOn);
    }
}