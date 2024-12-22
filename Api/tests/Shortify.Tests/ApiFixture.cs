using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shortify.Api;
using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Tests.Extensions;

namespace Shortify.Tests;

public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			services.Remove<IUrlDataStore>();
			services.AddSingleton<IUrlDataStore>(new InMemoryUrlDataStore());
		});
	}
}

public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
	public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
	{
		Add(shortened.ShortUrl, shortened);
		return Task.CompletedTask;
	}
}