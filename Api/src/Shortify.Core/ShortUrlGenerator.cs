namespace Shortify.Core;

public class ShortUrlGenerator(TokenProvider tokenProvider)
{
	public string GenerateShortUrl()
	{
		return tokenProvider.GetToken().EncodeToBase62();
	}
}