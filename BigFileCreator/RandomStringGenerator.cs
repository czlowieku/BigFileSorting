namespace BigFileCreator;

public interface IRandomStringGenerator
{
	string GenerateRandomAlphanumericString(int length = 10);
}

public class RandomStringGenerator : IRandomStringGenerator
{
	private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	private readonly Random _random;

	public RandomStringGenerator()
	{
		_random = new Random();
	}
	public string GenerateRandomAlphanumericString(int length = 10)
	{
		var randomString = new string(Enumerable.Repeat(Chars, length)
			.Select(s => s[_random.Next(s.Length)]).ToArray());
		return randomString;
	}

	

}