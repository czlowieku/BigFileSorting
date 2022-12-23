using BigFileSorting;

namespace TestProject1;

[TestClass]
public class MergerTests : TestBase
{

	[TestMethod]
	public async Task MergereTest1()
	{
		var config = new Config();

		var dotNetQuickSort = new DotNetQuickSort(config, new SpanComparer());
		var merger = new Merger(dotNetQuickSort, config);

		var myString1 =
			Enumerable.Range(0, 10).Select(i => $"{i}. {1}").ToList();
		var myString2 =
			Enumerable.Range(0, 30).Select(i => $"{i}. {1}").ToList();
		var myString3=
			Enumerable.Range(20, 40).Select(i => $"{i}. {1}").ToList();


		await File.WriteAllLinesAsync("!1.txt", myString1);
		await File.WriteAllLinesAsync("!2.txt", myString2);
		await File.WriteAllLinesAsync("!3.txt", myString3);


		var sortedFile = await merger.MergeSortedFiles(new List<string>()
		{
			"!1.txt", "!2.txt", "!3.txt"
		});

		var sortedLines=await File.ReadAllLinesAsync(sortedFile);
		Assert.IsTrue(sortedLines.Length == myString1.Count+myString2.Count+myString3.Count);

		var allLinesSorted = myString1.Concat(myString2).Concat(myString3).ToList();
		allLinesSorted.Sort(new SpanComparer());

		for (int i = 0; i < allLinesSorted.Count; i++)
		{
			Assert.AreEqual(allLinesSorted[i], sortedLines[i],i.ToString());
		}

	}


}