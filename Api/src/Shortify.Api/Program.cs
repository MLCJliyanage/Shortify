using System.Security.Authentication;
using System.Security.Claims;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Polly;
using Shortify.Api;
using Shortify.Api.Extensions;
using Shortify.Core.Urls.Add;
using Shortify.Core.Urls.List;
using Shortify.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["keyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
	builder.Configuration.AddAzureKeyVault(
		new Uri($"https://{keyVaultName}.vault.azure.net"),
		new DefaultAzureCredential()
	);
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
	.AddSingleton(TimeProvider.System)
	.AddSingleton<IEnvironmentManager, EnvironmentManager>();
builder.Services
	.AddUrlFeature()
	.AddListUrlsFeature()
	.AddCosmosUrlDataStore(builder.Configuration);

builder.Services.AddHttpClient("TokenRangeService",
	client =>
	{
		client.BaseAddress = new Uri(builder.Configuration["TokenRangeService:Endpoint"]!);
	});

builder.Services.AddSingleton<ITokenRangeApiClient, TokenRangeApiClient>();
builder.Services.AddHostedService<TokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(options =>
		{
			builder.Configuration.Bind("AzureAd", options);
			options.TokenValidationParameters.NameClaimType = "name";
		},
		options =>
		{
			builder.Configuration.Bind("AzureAd", options);

		});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AuthZPolicy", policyBuilder =>
		policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement()
		{
			RequiredScopesConfigurationKey = "AzureAd:Scopes"
		}));

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = 
		new AuthorizationPolicyBuilder(
				JwtBearerDefaults.AuthenticationScheme)
			.RequireAuthenticatedUser()
			.Build();
	// By default, all incoming requests will be authorized according to 
	// the default policy    
	options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () =>"API")
	.AllowAnonymous();

app.MapPost("/api/urls", async (
	AddUrlHandler handler,
	AddUrlRequest request,
	HttpContext context,
	CancellationToken cancellationToken) =>
{
	var email = context.User.GetUserEmail();
	
	var requestedWithUser = request with
	{
		CreatedBy = email
	};

	var result = await handler.HandleAsync(requestedWithUser, cancellationToken);

	if (!result.Succeeded)
	{
		return Results.BadRequest(result.Error);
	}

	return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
});

app.MapGet("/api/urls", async (HttpContext context, IUserUrlsReader reader, CancellationToken cancellationToken) =>
{
	var email = context.User.GetUserEmail();
	
	var urls = await reader.GetAsync(email, cancellationToken);
	return urls;
});

app.Run();
