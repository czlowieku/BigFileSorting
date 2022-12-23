using BigFileSorting;

namespace TestProject1;

[TestClass]
public class StringWithNumberComparerTests: TestBase
{

	

	[TestMethod]
	public void Span_ShouldCompare()
	{

		var myString =
			GetRandomLines(100);
		myString.Add("3. sameString");
		myString.Add("4. sameString");
		myString.Add("5. sameString");
		myString.Add("5. sameString");
		myString.Add("5. sameString");


		myString.Sort(new SpanComparer());

		var x = 1;
	}
}