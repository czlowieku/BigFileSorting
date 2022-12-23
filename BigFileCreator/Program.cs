using System.Diagnostics;
using BigFileCreator;

var s = new Stopwatch();
var sizeInBytes = args.Length == 1 ? ulong.Parse(args[0]) : 1073741824;
s.Start();
var fn= new Seeder(new RandomStringGenerator()).CreateFile(sizeInBytes);
s.Stop();
Console.WriteLine($"file {fn} of size {sizeInBytes} seeded with data in {s.Elapsed}");
