using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;

namespace Shortify.Tests.TestDoubles;

public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore, IUserUrlsReader
{
    public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        Add(shortened.ShortUrl, shortened);
        return Task.CompletedTask;
    }

    public Task<Core.Urls.List.ListUrlsResponse> GetAsync(
        string createdBy,
        int pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        var data = Values
            .Where(u => u.CreatedBy == createdBy)
            .Select((u, index) => (index, new UrlItem(u.ShortUrl, u.LongUrl.ToString(), u.CreatedOn)))
            .Where(entry => continuationToken == null || entry.index > int.Parse(continuationToken))
            .Take(pageSize)
            .ToList();

        var urls = data.Select(entry => entry.Item2);
        var lastItemIndex = data.Last().index;

        return Task.FromResult(new ListUrlsResponse(urls,
            lastItemIndex.ToString()));
    }
}