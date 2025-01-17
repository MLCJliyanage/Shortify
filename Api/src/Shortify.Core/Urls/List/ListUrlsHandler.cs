using Shortify.Core.Extensions;

namespace Shortify.Core.Urls.List;

public class ListUrlsHandler
{
    private readonly IUserUrlsReader _userUrlsReader;
    private const int MaxPageSize = 20;
    private readonly RedirectLinkBuilder _redirectLinkBuilder;

    public ListUrlsHandler(IUserUrlsReader userUrlsReader, RedirectLinkBuilder redirectLinkBuilder)
    {
        _userUrlsReader = userUrlsReader;
        _redirectLinkBuilder = redirectLinkBuilder;
    }

    public async Task<ListUrlsResponse> HandleAsync(ListUrlsRequest request, CancellationToken cancellationToken)
    {
        var urls = await _userUrlsReader.GetAsync(request.Author,
            int.Min(request.PageSize ?? MaxPageSize, MaxPageSize),
            request.ContinuationToken,
            cancellationToken);
        
        return urls.MapToResponse(_redirectLinkBuilder);
    }
}