using BigFileCreator;
using BigFileSorting;
using System;

namespace TestProject1
{
	[TestClass]
	public class IntegrationTest:TestBase
	{

		[TestMethod]
		public async Task ShouldNotMessData()
		{
			var config = new Config()
			{
			};

			var dotNetQuickSort = new DotNetQuickSort(config, new SpanComparer());

			var filesToSort = new FileSplitterAndSorter(config, dotNetQuickSort);
			var merger = new Merger(dotNetQuickSort, config);

			var lines = this.GetLinesStream(GetSameLines(100000));
			var files = await filesToSort.SplitAndSort(lines);


			var sortedFile = await merger.MergeSortedFiles(files);
			var sorted = await File.ReadAllLinesAsync(sortedFile);
			
			Assert.IsTrue(sorted.All(s => s == "1. a"));
			Assert.IsTrue(sorted.Length== 100000);
		}

	}
}