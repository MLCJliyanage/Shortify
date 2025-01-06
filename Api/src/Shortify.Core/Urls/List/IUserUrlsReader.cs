namespace Shortify.Core.Urls.List;

public interface IUserUrlsReader
{
    Task<ListUrlsResponse> GetAsync(string createdBy,
        CancellationToken cancellationToken);
}