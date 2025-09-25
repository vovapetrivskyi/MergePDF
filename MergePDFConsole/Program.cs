using MergePDFLibrary;
using System.Diagnostics;

string inputDirectoryPath = "";

while (!Directory.Exists(inputDirectoryPath))
{
	Console.WriteLine("Input directory with pdf files to merge:");

	inputDirectoryPath = Console.ReadLine();
}

string outputFilePath = "";

while (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
{
	Console.WriteLine("Input result file full path with file name (for example C:\\result.pdf):");

	outputFilePath = Console.ReadLine();
}

string enumeratePages = "-1";

while (enumeratePages != "1" && enumeratePages != "0")
{
	Console.WriteLine("Enumerate pages (1 - yes, 0 - no):");

	enumeratePages = Console.ReadLine();
}

string exceptFirstPage = "-1";

while (exceptFirstPage != "1" && exceptFirstPage != "0")
{
	Console.WriteLine("Don't enumetare first page (1 - yes, 0 - no):");

	exceptFirstPage = Console.ReadLine();
}

PDFMerger pdfMerger = new PDFMerger();

bool enumratePagesBool = enumeratePages == "1" ? true : false;
bool exceptFirstPageBool = exceptFirstPage == "1" ? true : false;

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();

string result = pdfMerger.MergePdfFilesWithPageNumbers(inputDirectoryPath, 
	outputFilePath,
	enumratePagesBool,
	exceptFirstPageBool
	);

stopwatch.Stop();

// Get the elapsed time as a TimeSpan object
TimeSpan ts = stopwatch.Elapsed;

// Format and display the elapsed time
string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

Console.WriteLine("Task completed in: " + elapsedTime);

Console.WriteLine($"Result: {result}");

Console.ReadLine();