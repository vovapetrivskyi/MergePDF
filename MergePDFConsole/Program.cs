/*
	VPPdfMerger

    Copyright (C) 2026  Volodymyr Petrivskyi vovapetrivskyi@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

*/

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