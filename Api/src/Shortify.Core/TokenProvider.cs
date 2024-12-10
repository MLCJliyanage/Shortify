namespace Shortify.Core;

public class TokenProvider
{
	private int _start;
	public void AssignRange(int start, int i1)
	{
		_start = start;
	}

	public int GetToken()
	{
		return _start;
	}
}