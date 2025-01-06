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

    public Task<Core.Urls.List.ListUrlsResponse> GetAsync(string createdBy,
        CancellationToken cancellationToken)
    {
        var urls = Values
            .Where(u => u.CreatedBy == createdBy)
            .Select(u =>  new UrlItem(u.ShortUrl, u.LongUrl.ToString(), u.CreatedOn))
            .ToList();

        // var urls = data.Select(entry => entry.Item2);
        // var lastItemIndex = data.Last().index;

        return Task.FromResult(new Core.Urls.List.ListUrlsResponse(urls));
    }
}