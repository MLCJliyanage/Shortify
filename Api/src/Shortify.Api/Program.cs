using Azure.Identity;
using Shortify.Api.Extensions;
using Shortify.Core.Urls.Add;
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
builder.Services.AddSingleton(TimeProvider.System);
builder.Services
	.AddUrlFeature()
	.AddCosmosUrlDataStore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/urls", async (
	AddUrlHandler handler,
	AddUrlRequest request,
	CancellationToken cancellationToken) =>
{
	var requestedWithUser = request with
	{
		CreatedBy = "Cj070@gmail.com"
	};

	var result = await handler.HandleAsync(requestedWithUser, cancellationToken);

	if (!result.Succeeded)
	{
		return Results.BadRequest(result.Error);
	}

	return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
});

app.Run();