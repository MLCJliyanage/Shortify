namespace Shortify.Core.Urls.List;

public record ListUrlsRequest(string Author, int? PageSize, string ContinuationToken = null);