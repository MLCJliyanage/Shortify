using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;

namespace Shortify.Infrastructure;

public class CosmosDBUrlDataStore : IUrlDataStore
{
    private readonly Container _container;

    public CosmosDBUrlDataStore(Container container)
    {
        _container = container;
    }
    public async Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        var document = (ShortenedUrlCosmos)shortened;
        await _container.CreateItemAsync(document,
            new PartitionKey(document.PartitionKey),
            cancellationToken: cancellationToken);
    }
    
    internal class ShortenedUrlCosmos
    {
        public string LongUrl { get; }
        [JsonProperty(PropertyName = "id")]
        public string ShortUrl { get; }
        public DateTimeOffset CreatedOn { get; }
        public string CreatedBy { get; }
        public string PartitionKey => ShortUrl[..1];

        public ShortenedUrlCosmos(string longurl, string shortUrl, DateTimeOffset createdOn, string createdBy)
        {
            LongUrl = longurl;
            ShortUrl = shortUrl;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }
        
        public static implicit operator ShortenedUrl(ShortenedUrlCosmos url) =>
            new(new Uri(url.LongUrl), url.ShortUrl, url.CreatedBy, url.CreatedOn);

        public static explicit operator ShortenedUrlCosmos(ShortenedUrl url) =>
            new(url.LongUrl.ToString(), url.ShortUrl, url.CreatedOn, url.CreatedBy);
    }
}