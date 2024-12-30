using Shortify.Core;

namespace Shortify.Api;

public interface ITokenRangeApiClient
{
    Task<TokenRange?> AssignRangeAsync(string machinekey, CancellationToken cancellationToken);
}