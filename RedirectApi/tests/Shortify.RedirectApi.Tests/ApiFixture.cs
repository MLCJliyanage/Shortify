using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shortify.Libraries.Testing.Extensions;
using Shortify.RedirectApi.Infrastructure;
using Shortify.RedirectApi.Tests.TestDoubles;

namespace Shortify.RedirectApi.Tests;

public class ApiFixture : WebApplicationFactory<IRedirectApiAssemblyMarker>
{
    public InMemoryShortenedUrlReader ShortenedUrlReader { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                services.Remove<IShortenedUrlReader>();
                services.AddSingleton<IShortenedUrlReader>(ShortenedUrlReader);

            });
        base.ConfigureWebHost(builder);
    }
}