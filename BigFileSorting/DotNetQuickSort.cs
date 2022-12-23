using System;
using System.Text;

namespace BigFileSorting;

public interface IInMemorySorter
{
	byte[] Sort(byte[] bytes);
	int Compare(string? x, string? y);

}

public class DotNetQuickSort : IInMemorySorter
{
	private readonly IComparer<string> _comparer;
	private readonly IConfig _config;

	public DotNetQuickSort(IConfig config, IComparer<string> comparer)
	{
		_config = config;
		_comparer = comparer;
	}
	public byte[] Sort(byte[] bytes)
	{
		var lines = Encoding.Default.GetString(bytes).Split(_config.NewLine);
		var sorted = lines.OrderBy(s => s, _comparer).ToList();
		byte[] dataAsBytes = System.Text.Encoding.Default.GetBytes(string.Join(_config.NewLine, sorted));
		return dataAsBytes;
	}

	public int Compare(string? x, string? y)
	{
		return _comparer.Compare(x, y);
	}
}