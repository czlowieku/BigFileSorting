namespace BigFileSorting;

public class Merger :IMerger
{
	private readonly IInMemorySorter _inMemorySorter;
	private readonly IConfig _config;

	public Merger(IInMemorySorter inMemorySorter,IConfig config)
	{
		_inMemorySorter = inMemorySorter;
		_config = config;
	}

	public async Task<string> MergeSortedFiles(List<string> paths)
	{
		var q = new Queue<string>(paths);

		while (q.Count > 1)
		{
			if (q.Count == 1)
			{
				break;
			}

			var f1 = q.Dequeue();
			var f2 = q.Dequeue();
			var path =await Merge(f1, f2);
			q.Enqueue(path);
		}

		var sorted = q.Dequeue();
		return sorted;
	}

	private async Task<string> Merge(string p1, string p2)
	{
		var path = Path.Combine(_config.SpliitedFilesTmpLocation,$"{Guid.NewGuid()}.merged.txt");
		using (var s1 = new StreamReader(p1))
		using (var s2 = new StreamReader(p2))
		{
			await using var write = new StreamWriter(path);
			var l1 = await s1.ReadLineSkipEmptyAsync();
			var l2 = await s2.ReadLineSkipEmptyAsync();

			while (true)
			{
				if (l1 == null && l2 == null)
				{
					break;
				}

				var c = _inMemorySorter.Compare(l1, l2);
				switch (c)
				{
					case <0:
						await write.WriteLineAsync(l1);
						l1 = await s1.ReadLineSkipEmptyAsync();
						break;
					case >0:
						await write.WriteLineAsync(l2);
						l2 = await s2.ReadLineSkipEmptyAsync();
						break;
					case 0:
						await write.WriteLineAsync(l1);
						await write.WriteLineAsync(l2);
						l1 = await s1.ReadLineSkipEmptyAsync();
						l2 = await s2.ReadLineSkipEmptyAsync();
						break;
				}
			}
		}

		File.Delete(p1);
		File.Delete(p2);
		return path;
	}
}

public interface IMerger
{
	Task<string> MergeSortedFiles(List<string> paths);
}

public static class StreamReaderExt
{
	public static async Task<string?> ReadLineSkipEmptyAsync(this StreamReader sr)
	{
		var s= await sr.ReadLineAsync();
		while (s == string.Empty)
		{
			s = await sr.ReadLineAsync();
		}
		return s;
	}
}
