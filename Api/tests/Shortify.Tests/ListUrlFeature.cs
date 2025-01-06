using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;
using FluentAssertions;
using Shortify.Api.Core.Tests.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;

namespace Shortify.Tests;

[Collection("Api collection")]
public class ListUrlFeature
{
    private const string UrlsEndpoint = "/api/urls";
    private readonly HttpClient _client;

    public ListUrlFeature(ApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }
    
    [Fact]
    public async Task Should_return_200_ok_with_list_of_urls()
    {
        await AddUrl();

        var response = await _client.GetAsync(UrlsEndpoint);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var urls = await response.Content
            .ReadFromJsonAsync<ListUrlsResponse>();
        urls!.Urls.Should().NotBeEmpty();
    }

    private async Task<AddUrlResponse?> AddUrl(string? url = null)
    {
        url ??= $"https://{Guid.NewGuid()}.tests";

        var response = await _client.PostAsJsonAsync(UrlsEndpoint,
            new AddUrlRequest(new Uri(url), ""));
        return await response.Content.ReadFromJsonAsync<AddUrlResponse>();
    }

    [Fact]
    public async Task Should_return_url_when_created_first()
    {
        var urlCreated = await AddUrl("https://testing-in-list.tests");
        
        var getResponse = await _client.GetAsync(UrlsEndpoint);
        var urls = await getResponse.Content
            .ReadFromJsonAsync<ListUrlsResponse>();

        urls!.Urls.Should()
            .Contain(url => url.ShortUrl == urlCreated!.ShortUrl);
    }
    
}
