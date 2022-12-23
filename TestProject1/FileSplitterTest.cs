using System.Text;
using BigFileSorting;

namespace TestProject1;

[TestClass]
public class FileSplitterTest:TestBase
{
	[DataTestMethod]
	[DataRow(128, 100)]
	[DataRow(1024, 1024)]
	[DataRow(10000000, 1)]
	[DataRow(1000000, 100000)]
	[DataRow(100000/30, 100000)]
	public async Task FileSplitter_ShouldSplit(int buffSize, int lines)
	{
		var config = new Config()
		{
			SplittedFileRuffSize = buffSize
		};
		var fs = new FileSplitterAndSorter(config);
		var myString = string.Join('\n', Enumerable.Range(0, lines).Select(i => $"{1}. someNotSplittableText"));
		File.WriteAllText("test.txt", myString);

		using (var stream = new FileStream("test.txt", FileMode.Open, FileAccess.Read))
		{
			var splitted = await fs.SplitAndSort(stream);
		}

		var fromFilesStr = string.Join('\n',
			new DirectoryInfo(config.SpliitedFilesTmpLocation)
				.GetFiles()
				.OrderBy(info => info.Name)
				.Select(info => string.Join('\n', File.ReadAllLines(info.FullName).Where(s => s!=string.Empty))));

		Assert.AreEqual(myString, fromFilesStr);
	}
	



}