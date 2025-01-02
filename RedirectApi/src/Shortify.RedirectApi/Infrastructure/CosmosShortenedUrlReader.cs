using Microsoft.Azure.Cosmos;

namespace Shortify.RedirectApi.Infrastructure;

public class CosmosShortenedUrlReader: IShortenedUrlReader
{
    private readonly Container _container;

    public CosmosShortenedUrlReader(Container container)
    {
        _container = container;
    }
    public async Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            var record = await _container.ReadItemAsync<CosmosUrlRecord>(
                shortUrl,
                new PartitionKey(shortUrl[..1]),
                cancellationToken: cancellationToken);

            return record switch
            {
                { Resource: not null } => new ReadLongUrlResponse(true, record.Resource.LongUrl),
                _ => new ReadLongUrlResponse(false, null)
            };
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new ReadLongUrlResponse(false, null);
        }
    }
}

public record CosmosUrlRecord(string LongUrl);