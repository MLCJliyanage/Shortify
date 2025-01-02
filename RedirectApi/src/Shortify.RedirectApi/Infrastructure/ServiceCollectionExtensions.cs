using Microsoft.Azure.Cosmos;

namespace Shortify.RedirectApi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlReader(this IServiceCollection services, 
        string cosmosConnectionString,
        string databaseName, string containerName,
        string redisConnectionString)
    {
        services.AddSingleton<CosmosClient>(s => 
            new CosmosClient(connectionString: cosmosConnectionString));
        
        
        services.AddSingleton<IShortenedUrlReader>(s =>
        {
            var cosmosClient = s.GetRequiredService<CosmosClient>();
            var container = cosmosClient.GetContainer(databaseName, containerName);

            return
                new CosmosShortenedUrlReader(container);
        });

        return services;
    }
}