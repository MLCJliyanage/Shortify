using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shortify.Api.Core.Tests.Urls;
using Shortify.Core.Urls.Add;

namespace Shortify.Tests
{
	public class AddUrlFeature(ApiFixture fixture) : IClassFixture<ApiFixture>
	{
		private readonly HttpClient _client = fixture.CreateClient();

		[Fact]
		public async Task Given_long_url_should_return_short_url()
		{
			var response = await _client.PostAsJsonAsync<AddUrlRequest>("/api/urls",
				new AddUrlRequest(new Uri("https://stackoverflow.com/questions/27465851/how-should-i-handle-very-very-long-url"),
					"cj@gmail.com"));

			response.StatusCode.Should().Be(HttpStatusCode.Created);
			var addUrlResponse = await response.Content.ReadFromJsonAsync<AddUrlResponse>();
			addUrlResponse!.ShortUrl.Should().NotBeNull();
		}
	}
}