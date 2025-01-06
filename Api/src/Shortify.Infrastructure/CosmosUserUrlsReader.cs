using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Shortify.Core.Urls.List;

namespace Shortify.Infrastructure;

public class CosmosUserUrlsReader: IUserUrlsReader
{
    private readonly Container _container;

    private const int PageSize = 10;
    public CosmosUserUrlsReader(Container container)
    {
        _container = container;
    }
    public async Task<ListUrlsResponse> GetAsync(string createdBy, CancellationToken cancellationToken)
    {
        var query = 
            new QueryDefinition("SELECT * FROM c  WHERE c.PartitionKey = @partitionKey")
                .WithParameter("@partitionKey", createdBy);
        
        var iterator = _container.GetItemQueryIterator<ShortenedUrlEntity>(query,
            requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(createdBy),
                MaxItemCount = PageSize
            });
        
        var results = new List<ShortenedUrlEntity>();
        string? resultContinuationToken = null;
        var readItemsCount = 0;

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }
        
        return new ListUrlsResponse(
            results.Select(e => 
                    new UrlItem(e.ShortUrl, e.LongUrl, e.CreatedOn))
                .ToList());
    }
}

public class ShortenedUrlEntity
{
    public string LongUrl { get; }

    [JsonProperty(PropertyName = "id")] // Cosmos DB Unique Identifier
    public string ShortUrl { get; }
    public DateTimeOffset CreatedOn { get; }
    
    public ShortenedUrlEntity(string longUrl, string shortUrl, 
        DateTimeOffset createdOn)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedOn = createdOn;
    }
}