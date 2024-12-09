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

	public static class Base62EncodingExtentions
	{
		private const string Alphanumeric = "0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz";
		public static string EncodeToBase62(this int number)
		{
			if(number == 0) return Alphanumeric[0].ToString();

			var result = new Stack<char>();
			while (number > 0)
			{
				result.Push(Alphanumeric[(int)(number % 62)]);
				number /= 62;
			}

			return new string(result.ToArray());
		}
	}
}