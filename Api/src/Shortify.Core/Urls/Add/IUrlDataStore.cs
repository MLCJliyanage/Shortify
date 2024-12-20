namespace Shortify.Core.Urls.Add;

public interface IUrlDataStore
{
	Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken);
}