using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Shortify.Api.Core.Tests.Urls;
using Shortify.Core.Urls.Add;

namespace Shortify.Tests
{
	[Collection("Api collection")]
	public class AddUrlFeature
	{
		private readonly HttpClient _client;

		public AddUrlFeature(ApiFixture fixture)
		{
			_client = fixture.CreateClient();
			_client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("TestScheme");
		}

		[Fact]
		public async Task Given_long_url_Should_return_short_url()
		{
			var response = await _client.PostAsJsonAsync("/api/urls",
				new AddUrlRequest(new Uri("https://dometrain.com"), ""));

			response.StatusCode.Should().Be(HttpStatusCode.Created);
			var addUrlResponse = await response.Content.ReadFromJsonAsync<AddUrlResponse>();
			addUrlResponse!.ShortUrl.Should().NotBeNull();
		}
	}
}