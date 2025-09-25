using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace MergePDFLibrary
{
	public class PDFMerger
	{
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
