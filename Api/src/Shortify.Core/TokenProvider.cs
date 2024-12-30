using System.Collections.Concurrent;

namespace Shortify.Core;

public class TokenProvider
{
	private readonly object _tokenLock = new();
	private long _currentToken;
	private TokenRange? _currentTokenRange;
	private readonly ConcurrentQueue<TokenRange> _ranges = new();
	public void AssignRange(long start, long end)
	{
		AssignRange(new TokenRange(start, end));
	}

	public void AssignRange(TokenRange tokenRange)
	{
		_ranges.Enqueue(tokenRange);
	}

	public long GetToken()
	{
		lock (_tokenLock)
		{
			if (_currentTokenRange is null)
				MoveToNextRange();

			if (_currentToken > _currentTokenRange?.End)
				MoveToNextRange();

			if (IsReachingRangeLimit())
				OnRangeThresholdReached(new ReachingRangeLimitEventArgs()
				{
					RangeLimit = _currentTokenRange!.End,
					Token = _currentToken
				});

			return _currentToken++;
		}
	}
	
	private void MoveToNextRange()
	{
		if (!_ranges.TryDequeue(out _currentTokenRange))
			throw new IndexOutOfRangeException();
		_currentToken = _currentTokenRange.Start;
	}
	
	private bool IsReachingRangeLimit()
	{
		var currentIndex = _currentToken + 1 - _currentTokenRange!.Start;
		var total = _currentTokenRange.End - _currentTokenRange.Start;
		return currentIndex >= total * 0.8;
	}
	
	public event EventHandler? ReachingRangeLimit;
	
	protected virtual void OnRangeThresholdReached(EventArgs e)
	{
		ReachingRangeLimit?.Invoke(this, e);
	}
}

public class ReachingRangeLimitEventArgs : EventArgs
{
	public long Token { get; set; }
	public long RangeLimit { get; set; }
}