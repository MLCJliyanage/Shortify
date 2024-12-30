using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shortify.Api;
using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Tests.Extensions;
using Shortify.Tests.TestDoubles;

namespace Shortify.Api.Core.Tests.Urls;

public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(
			services =>
			{
				services.Remove<IUrlDataStore>();
				services
					.AddSingleton<IUrlDataStore>(
						new InMemoryUrlDataStore());
                
				services.Remove<ITokenRangeApiClient>();
				services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeApiClient>();
			}
		);
        
		base.ConfigureWebHost(builder);
	}
}

