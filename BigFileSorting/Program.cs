
using System.Diagnostics;


namespace BigFileSorting
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var s = new Stopwatch();
		
			var file = args[0];
			var config = new Config()
			{
				SplittedFileRuffSize = 200 * 1024 * 1024
			};
			if (args.Length > 1)
				config.SplittedFileRuffSize = int.Parse(args[1]);

			s.Start();
			var dotNetQuickSort = new DotNetQuickSort(config, new SpanComparer());
			var splitter = new FileSplitterAndSorter(config, dotNetQuickSort);
			var splitedFiles = await splitter.SplitAndSort(new FileStream(file, FileMode.Open, FileAccess.Read));
			var merger = new Merger(dotNetQuickSort, config);
			var sortedFile = await merger.MergeSortedFiles(splitedFiles);
			s.Stop();

			Console.WriteLine(sortedFile);
			Console.WriteLine($"sorted file {file} in {s.Elapsed}");
		}
	}

	public interface IConfig
	{
		string NewLine { get; }
		string SpliitedFilesTmpLocation { get; }
		int SplittedFileRuffSize { get; }
	}

	public record Config : IConfig
	{
		public string NewLine { get; set; } = Environment.NewLine;
		public string SpliitedFilesTmpLocation { get; set; } = "splitted";
		public int SplittedFileRuffSize { get; set; } = 5 * 1024 * 1024;
	}
}