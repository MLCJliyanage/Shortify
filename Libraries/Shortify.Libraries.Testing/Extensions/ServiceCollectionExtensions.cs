using Microsoft.Extensions.DependencyInjection;

namespace Shortify.Libraries.Testing.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Remove<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if(descriptor is not null) services.Remove(descriptor);
    }
}