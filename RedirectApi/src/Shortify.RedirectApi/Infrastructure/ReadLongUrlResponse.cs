using Shortify.RedirectApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Redirect Api!");

app.MapGet("r/{shortUrl}", 
    async (string shortUrl, IShortenedUrlReader reader, CancellationToken cancellationToken) =>
    {
        var response = await reader.GetLongUrlAsync(shortUrl, cancellationToken);

        return response switch
        {
            { Found: true, LongUrl: not null } 
                => Results.Redirect(response.LongUrl, true),
            _ => Results.NotFound()
        };
    });

app.Run();

namespace Shortify.RedirectApi.Infrastructure
{
    public record ReadLongUrlResponse(bool Found, string? LongUrl);
}