using Shortify.Core;
using Shortify.Core.Urls.Add;

namespace Shortify.Api.Core.Tests;

public class AddUrlScenarios
{
	private readonly AddUrlHandler _handler;

	public AddUrlScenarios()
	{
		var tokenProvider = new TokenProvider();
		tokenProvider.AssignRange(1, 5);
		var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
		_handler = new AddUrlHandler(shortUrlGenerator);
	}

	[Fact]
	public async Task Should_return_shortened_url()
	{
		var request = CreateAddUrlRequest();
		var response = await _handler.HandleAsync(request, default);

		response.ShortUrl.Should().NotBeEmpty();
		response.ShortUrl.Should().Be("1");
	}

	private static AddUrlRequest CreateAddUrlRequest()
	{
		return new AddUrlRequest(new Uri("https://chathuraliyanage.info"));
	}
}