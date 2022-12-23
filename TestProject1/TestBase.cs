using BigFileCreator;
using BigFileSorting;

namespace TestProject1;

public class TestBase
{
	public List<string> GetRandomLines(int count)
	{
		var r = new Random();
		var sg = new RandomStringGenerator();
		var myString =
			Enumerable.Range(0, count).Select(i => $"{i}. {sg.GenerateRandomAlphanumericString(r.Next(20))}").ToList();
		return myString;
	}

	public Stream GetLinesStream(List<string> lines)
	{
	
		var allLines = string.Join(new Config().NewLine, lines);
		byte[] dataAsBytes = System.Text.Encoding.Default.GetBytes(allLines);

		return new MemoryStream(dataAsBytes);
	}


	public List<string> GetSameLines(int count)
	{
		var r = new Random();
		var sg = new RandomStringGenerator();
		var myString =
			Enumerable.Range(0, count).Select(i => $"1. a").ToList();
		return myString;
	}


}