using Shortify.TokenRangeService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(
    new TokenRangeManager(builder.Configuration["Postgres:ConnectionString"]!));

var app = builder.Build();

app.UseHttpsRedirection();


app.MapGet("/", () =>"TokenRanges Service");
app.MapPost("/assign", async (AssignTokenRangeRequest request, TokenRangeManager manager) =>
{
    var range = await manager.AssignRangeAsync(request.Key);
    return range;
});

app.Run();

public record AssignTokenRangeRequest(string Key);
public record TokenRangeResponse(long Start, long End);

