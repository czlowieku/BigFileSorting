namespace BigFileCreator;

public class Seeder
{
	private readonly Random _r;
	private readonly IRandomStringGenerator _stringGenerator;

	public Seeder(IRandomStringGenerator randomStringGenerator)
	{
		_r = new Random();
		_stringGenerator = randomStringGenerator;
	}

	// 1. A utility for creating a test file of a given size.The result of the work should be a text file
	//  of the type described above. There must be some number of lines with the same String
	// part.
	public string CreateFile(ulong sizeInBytes, int someNumberOfLines = 1000, string? fullPath = null)
	{
		var maxNumber = 1000;
		var fileSize = sizeInBytes;
		var fileName = fullPath ?? $"{Guid.NewGuid()}.txt";

		var repeatedStrings = GetRepeatingLines(someNumberOfLines, fileSize);

		using var f = File.Create(fileName);
		using StreamWriter file = new StreamWriter(f);

		WriteRepeatingLines(repeatedStrings, file, maxNumber);

		WriteRestLines(f, fileSize, file, maxNumber);

		return fileName;
	}

	private List<string> GetRepeatingLines(int someNumberOfLines, ulong fileSize)
	{
		var repeatedStrings = Enumerable.Range(0, someNumberOfLines)
			.Select(i => _stringGenerator.GenerateRandomAlphanumericString(_r.Next(15) + 5)).ToList();

		while ((ulong)repeatedStrings.Sum(s => s.Length + 2) * 2 > fileSize)
		{
			repeatedStrings = repeatedStrings.Take(repeatedStrings.Count / 2).ToList();
		}

		if (someNumberOfLines > repeatedStrings.Count)
			Console.WriteLine(
				$"someNumberOfLines {someNumberOfLines} was changed to {repeatedStrings.Count} as it would not fit the file of size{fileSize}");

		return repeatedStrings;
	}

	private void WriteRepeatingLines(List<string> repeatedStrings, StreamWriter file, int maxNumber)
	{
		for (int j = 0; j < 2; j++)
		for (int i = 0; i < repeatedStrings.Count; i++)
		{
			WriteList(file, maxNumber, repeatedStrings[i]);
		}
	}

	private void WriteRestLines(FileStream f, ulong fileSize, StreamWriter file, int maxNumber)
	{
		while ((ulong)f.Length < fileSize)
		{
			WriteList(file, maxNumber, _stringGenerator.GenerateRandomAlphanumericString(_r.Next(30)));
		}
	}

	private void WriteList(StreamWriter file, int maxNumber, string str)
	{
		file.WriteLine(
			$"{_r.NextInt64(maxNumber)}. {str}");
	}
}