using Shortify.Core;

namespace Shortify.Api.Core.Tests;

public class ShortUrlGeneratorScenarios
{
	//TEST LIST
	// Check if the end of range is gt than start
	// Unique tokens
	// Accept multiple changes
	
	[Fact]
	public void Should_return_short_url_for_10001()
	{
		var tokenProvider = new TokenProvider();
		tokenProvider.AssignRange(10001, 20000);
		var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
		var shortUrl = shortUrlGenerator.GenerateShortUrl();
		shortUrl.Should().Be("2bJ");
	}

	[Fact]
	public void Should_return_short_url_for_zero()
	{
		var tokenProvider = new TokenProvider();
		tokenProvider.AssignRange(0, 20);
		var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
		var shortUrl = shortUrlGenerator.GenerateShortUrl();
		shortUrl.Should().Be("0");
	}
}