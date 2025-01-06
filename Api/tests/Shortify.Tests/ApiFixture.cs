using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shortify.Api;
using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;
using Shortify.Libraries.Testing.Extensions;
using Shortify.Tests.TestDoubles;

namespace Shortify.Api.Core.Tests.Urls;

public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(
			services =>
			{
				var inMemoryStore = new InMemoryUrlDataStore();
				services.Remove<IUrlDataStore>();
				services
					.AddSingleton<IUrlDataStore>(
						inMemoryStore);
				
				services.Remove<IUserUrlsReader>();
				services
					.AddSingleton<IUserUrlsReader>(
						inMemoryStore);
                
				services.Remove<ITokenRangeApiClient>();
				services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeApiClient>();
				
				services.AddAuthentication(defaultScheme: "TestScheme")
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
						"TestScheme", options => { });
				
				services.AddAuthorization(options =>
				{
					options.DefaultPolicy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					options.FallbackPolicy = null;
				});
			}
		);
        
		base.ConfigureWebHost(builder);
	}
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger, UrlEncoder encoder)
		: base(options, logger, encoder)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new[]
		{
			new Claim(ClaimTypes.Name, "Test user"),
			new Claim("preferred_username", "testuser@test.com"),
		};
		var identity = new ClaimsIdentity(claims, "Test");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, 
			"TestScheme");

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}
}

