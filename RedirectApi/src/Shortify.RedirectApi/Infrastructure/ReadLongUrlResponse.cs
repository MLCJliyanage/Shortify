namespace Shortify.RedirectApi.Infrastructure
{
    public record ReadLongUrlResponse(bool Found, string? LongUrl);
}