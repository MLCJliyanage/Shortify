namespace Shortify.Core.Urls.Add;

public class AddUrlHandler(ShortUrlGenerator shortUrlGenerator, IUrlDataStore urlDataStore, TimeProvider timeProvider)
{
	public async Task<Result<AddUrlResponse>> HandleAsync(AddUrlRequest request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(request.CreatedBy))
		{
			return Errors.MissingCreatedBy;
		}
		var shortened = new ShortenedUrl(request.LongUrl, shortUrlGenerator.GenerateShortUrl(), request.CreatedBy, timeProvider.GetUtcNow());

		await urlDataStore.AddAsync(shortened, cancellationToken);
		return new AddUrlResponse(request.LongUrl, shortened.ShortUrl);
	}
}