using Shortify.Core;
using Shortify.Core.Urls;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;

namespace Shortify.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddUrlFeature(this IServiceCollection services)
		{
			services.AddScoped<AddUrlHandler>();
			services.AddSingleton<TokenProvider>();
			services.AddScoped<ShortUrlGenerator>();
			return services;
		}
		
		public static IServiceCollection AddListUrlsFeature(this IServiceCollection services)
		{
			services.AddScoped<ListUrlsHandler>();

			return services;
		}
	}
}
