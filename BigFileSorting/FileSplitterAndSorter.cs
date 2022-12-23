namespace BigFileSorting;

public interface IFileSplitterAndSorter
{
	Task<List<string>> SplitAndSort(string path);
	Task<List<string>> SplitAndSort(Stream source);
}

public class FileSplitterAndSorter : IFileSplitterAndSorter
{
	private readonly Config _conf;
	private readonly IInMemorySorter? _inMemorySorter;

	public FileSplitterAndSorter(Config conf, IInMemorySorter? inMemorySorter = null)
	{
		_conf = conf;
		_inMemorySorter = inMemorySorter;
	}

	public async Task<List<string>> SplitAndSort(string path)
	{
		await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
		return await SplitAndSort(stream);
	}

	public async Task<List<string>> SplitAndSort(Stream source)
	{
		PrepareDir();
		var fileNames = new List<string>();
		var size = _conf.SplittedFileRuffSize;
		int part = 0;
		var extraBuffer = new List<byte>();
		var buff = new byte[size];
		var read = 0;

		while ((read = source.Read(buff)) != 0)
		{
			if (buff.Last() != '\n')
			{
				var extra = source.ReadByte();
				do
				{
					if (extra == -1)
					{
						break;
					}

					extraBuffer.Add((byte)extra);
					extra = source.ReadByte();
				} while ((extra != '\n'));
			}

			var fileName = Path.Combine(_conf.SpliitedFilesTmpLocation, $"{part}.splited.txt");
			fileNames.Add(fileName);
			await SortAndSavePartialFile(fileName, extraBuffer, buff, read);
			extraBuffer.Clear();
			buff = new byte[size];
			part++;
		}

		return fileNames;
	}

	private async Task SortAndSavePartialFile(string fileName, List<byte> extraBuffer, byte[] buff, int read)
	{
		await using var outputStream = new FileStream(fileName, FileMode.Create);
		if (_inMemorySorter != null)
		{
			if (buff.Length > read)
				buff = buff.Take(read).ToArray();

			var allData = extraBuffer.Any() ? Combine(buff, extraBuffer.ToArray()) : buff;
			var sorted = _inMemorySorter.Sort(allData);
			await outputStream.WriteAsync(sorted, 0, sorted.Length);
		}
		else
		{
			await outputStream.WriteAsync(buff, 0, read);
			if (extraBuffer.Any())
				await outputStream.WriteAsync(extraBuffer.ToArray());
		}
	}

	public static byte[] Combine(byte[] first, byte[] second)
	{
		byte[] ret = new byte[first.Length + second.Length];
		Buffer.BlockCopy(first, 0, ret, 0, first.Length);
		Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
		return ret;
	}

	private void PrepareDir()
	{
		var dir = Directory.CreateDirectory(_conf.SpliitedFilesTmpLocation);
		foreach (var f in dir.GetFiles())
		{
			f.Delete();
		}
	}
}