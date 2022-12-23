namespace BigFileSorting;

public class SpanComparer : IComparer<string>
{
	public int Compare(string? x, string? y)
	{
		if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
			return 0;

		if (string.IsNullOrEmpty(x))
		{
			return 1;
		}

		if (string.IsNullOrEmpty(y))
		{
			return -1;
		}

		var str1 = GetStrVal(x, out var st1CommaIndex);
		var str2 = GetStrVal(y, out var st2CommaIndex);

		var strComp = str1.CompareTo(str2, StringComparison.OrdinalIgnoreCase);
		if (strComp != 0)
		{
			return strComp;
		}

		return GetIntVal(x, st1CommaIndex).CompareTo(GetIntVal(y, st2CommaIndex));
	}

	private int GetIntVal(string value, int commaIndex)
	{
		var span = value.AsSpan();
		return int.Parse(span[..commaIndex]);
	}

	//945. 51J4D1Q9HEQAIQ8JFHCDMJ
	private ReadOnlySpan<char> GetStrVal(string value, out int commaIndex)
	{
		var span = value.AsSpan();

		for (var i = 0; i < span.Length; i++)
		{
			if (span[i].Equals('.'))
			{
				commaIndex = i;
				return span[(i + 1)..];
			}
		}

		throw new Exception($"invalid line {value}");
	}
}
