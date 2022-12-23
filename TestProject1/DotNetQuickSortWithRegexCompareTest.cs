using BigFileSorting;

namespace TestProject1;

[TestClass]
public class DotNetQuickSortWithRegexCompareTest : TestBase
{

	[TestMethod]
	public void StringWithNumberComparer_ShouldCompare()
	{
		var myString =
			GetRandomLines(100);
		var config = new Config();

		var allLines = string.Join(config.NewLine, myString);
		byte[] dataAsBytes = System.Text.Encoding.Default.GetBytes(allLines);

		var sortedBytes = new DotNetQuickSort(config,new SpanComparer()).Sort(dataAsBytes);

		string fromBytes = System.Text.Encoding.Default.GetString(sortedBytes.ToArray());

	}
}