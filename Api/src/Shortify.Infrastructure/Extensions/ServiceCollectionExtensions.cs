using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;

namespace Shortify.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosUrlDataStore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<CosmosClient>(s =>
            new CosmosClient(configuration["CosmosDB:ConnectionString"]!));

        services.AddSingleton<IUrlDataStore>(s =>
        {
            var cosmosClient = s.GetRequiredService<CosmosClient>();
            var container = cosmosClient.GetContainer(
                configuration["DatabaseName"]!,
                configuration["ContainerName"]!);
            return new CosmosDBUrlDataStore(container);
        });
        
        services.AddSingleton<IUserUrlsReader>(s =>
        {
            var cosmosClient = s.GetRequiredService<CosmosClient>();
            var container = cosmosClient.GetContainer(
                configuration["ByUserDatabaseName"]!,
                configuration["ByUserContainerName"]!);
            
            return new CosmosUserUrlsReader(container);
        });
        
        return services;
    }
}