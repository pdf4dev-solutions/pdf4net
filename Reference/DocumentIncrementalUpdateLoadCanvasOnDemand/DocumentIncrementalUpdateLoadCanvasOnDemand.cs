using System;
using System.Diagnostics;
using System.IO;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Content;
using O2S.Components.PDF4NET.Graphics;

namespace O2S.Components.PDF4NET.Samples
{
    class DocumentIncrementalUpdateLoadGraphicsOnDemand
    {
        static void Main(string[] args)
        {
            PDFDocumentFeatures features = new PDFDocumentFeatures();
            // Load only the page information but not the actual page content
            features.EnablePages = true;
            features.EnablePageGraphics = false;

            // Make a copy of the input file because the changes are appended to the source
            File.Copy("..\\..\\..\\..\\..\\SupportFiles\\content.pdf", "content.pdf", true);
            using (FileStream fs = new FileStream("Content.pdf", FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                PDFFixedDocument document = new PDFFixedDocument(fs, features);

                // The page graphics has not been loaded so no content can be added to the page and
                // page processing operations (text extraction and search, redaction, etc) will fail.
                for (int i = 0; i < document.Pages.Count; i++)
                {
                    Debug.Assert(document.Pages[i].Canvas == null);
                }

                // Load the page graphics so that new content can be added to it.
                // If no page processing operation are required then there is no need to load existing page content.
                document.Pages[0].LoadCanvas(false);
                document.Pages[0].Canvas.DrawLine(new PDFPen(PDFRgbColor.Red, 1), 0, 0, document.Pages[0].Width, document.Pages[0].Height);


                // Load the page graphics so that page processing operations are possible.
                // For this we need to load the existing page content.
                document.Pages[1].LoadCanvas(true);
                PDFContentExtractor ce = new PDFContentExtractor(document.Pages[1]);
                PDFTextRunCollection tfc = ce.ExtractTextRuns();

                PDFPath path = new PDFPath();
                path.AddPolygon(tfc[0].TypographicCorners);
                document.Pages[1].Canvas.DrawPath(new PDFPen(PDFRgbColor.Red, 1), path);

                // Save the document changes.
                document.Save(fs);
                fs.Flush();

                Console.WriteLine("File saved with success to current folder.");
            }
        }
    }
}
