using Shortify.Core;

namespace Shortify.Api.Core.Tests
{
	public class Base62EncodingScenarios
	{
		[Theory]
		[InlineData(1, "1")]
		[InlineData(0, "0")]
		[InlineData(20, "K")]
		public void Should_encode_number_to_base62(int number, string expectedNumber)
		{
			number.EncodeToBase62()
				.Should()
				.Be(expectedNumber);
		}
	}
}