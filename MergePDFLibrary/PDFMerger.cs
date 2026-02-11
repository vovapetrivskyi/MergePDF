/*
	VP_PdfMerger

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

using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace MergePDFLibrary
{
	/// <summary>
	/// Class responsible for merging PDF files and optionally adding page numbers.
	/// </summary>
	public class PDFMerger
	{
		/// <summary>
		/// Method  for merging PDF files from a specified directory into a single PDF file. It also has options to enumerate pages and to skip enumeration on the first page.
		/// </summary>
		/// <param name="inputDirectoryPath">Path of thedirectorywithfiles</param>
		/// <param name="outputFilePath">Result file path</param>
		/// <param name="enumeratePages">Is page enumetation needed</param>
		/// <param name="exceptFirstPage">Is there need to enumerate first page</param>
		/// <returns></returns>
		public string MergePdfFilesWithPageNumbers(string inputDirectoryPath, string outputFilePath, bool enumeratePages, bool exceptFirstPage)
		{
			string result = "Ok";

			try
			{
				// Collect all PDF files from directory
				var pdfFiles = Directory.GetFiles(inputDirectoryPath, "*.pdf").OrderBy(f => f).ToList();

				if (pdfFiles.Count == 0)
				{
					result = "No PDF files found in the directory.";

					return result;
				}

				// Create output PDF
				using (var outputStream = new FileStream(outputFilePath, FileMode.Create))
				{
					var pdfWriter = new PdfWriter(outputStream);
					var pdfDocument = new PdfDocument(pdfWriter);
					var merger = new PdfMerger(pdfDocument);
					// Merge PDFs
					foreach (var file in pdfFiles)
					{
						if (file != outputFilePath)
						{
							var reader = new PdfReader(file);
							var srcPdf = new PdfDocument(reader);
							merger.Merge(srcPdf, 1, srcPdf.GetNumberOfPages());
							srcPdf.Close();
						}
					}

					if (enumeratePages)
					{
						int totalPages = pdfDocument.GetNumberOfPages();

						int startPage = exceptFirstPage ? 2 : 1;

						// Add page numbers
						for (int i = startPage; i <= totalPages; i++)
						{
							var page = pdfDocument.GetPage(i);
							var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDocument);

							var document = new Document(pdfDocument);
							document.ShowTextAligned(
								new Paragraph($"{i}")
									.SetFontSize(10),
								550, // X coordinate
								20,  // Y coordinate (bottom of the page)
								i,
								TextAlignment.RIGHT,
								VerticalAlignment.BOTTOM,
								0
							);
						}

						pdfDocument.Close();
					}
				}
			}
			catch (Exception ex)
			{
				result = "Error occurred while processing files. Try again.";
			}

			return result;
		}
	}
}
