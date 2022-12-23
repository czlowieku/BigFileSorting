Info:

	BigFilesCreatpr 
	Run to generate file.
	Output file may be few bytes larger ( one line size) 
	params:
	1- file size (max is 18446744073709551615)

	BigFileSorting
	Run to sort the big file. output file is in splitted folder.
	params:
	1-path to a file to sort
	2-(optional) size of splitted sub files in bytes default is 200mb

	assumptions:
	for a program to run correctly you need to have at least input file size free space on hdd 
	program uses case invartiant sorting  (StringComparison.OrdinalIgnoreCase)

	todo:
	merge improvments when one file ends can read other to end
	tests are not finished 
	spliting can be done in paralell with merging?
	try some other in memory sorting

Desc:

	The input is a large text file, where each line is a Number.String
	For example:
	415. Apple
	30432. Something something something
	1. Apple
	32. Cherry is the best
	2. Banana is yellow

	Both parts can be repeated within the file.You need to get another file as output, where all
	the lines are sorted. Sorting criteria: String part is compared first, if it matches then
	Number.
	Those in the example above, it should be:
	1. Apple
	415. Apple
	2. Banana is yellow
	32. Cherry is the best
	30432. Something something something

	You need to write two programs:
	1. A utility for creating a test file of a given size. The result of the work should be a text file
	of the type described above.There must be some number of lines with the same String
	part.
	2. The actual sorter.An important point, the file can be very large. The size of ~100Gb will
	be used for testing.
	When evaluating the completed task, we will first look at the result (correctness of
	generation / sorting and running time), and secondly, at how the candidate writes the code.
	Programming language: C#.