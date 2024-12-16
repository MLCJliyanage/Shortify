namespace Shortify.Core.Urls.Add;

public class AddUrlHandler(ShortUrlGenerator shortUrlGenerator)
{
	public Task<AddUrlResponse> HandleAsync(AddUrlRequest request, CancellationToken cancellationToken)
	{
		return Task.FromResult(new AddUrlResponse(request.LongUrl, shortUrlGenerator.GenerateShortUrl()));
	}
}