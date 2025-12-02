using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Content;
using O2S.Components.PDF4NET.Graphics;
using System;
using System.IO;

namespace O2S.Components.PDF4NET.Samples
{
    class SearchText
    {
        static void Main(string[] args)
        {
            string supportPath = "..\\..\\..\\..\\..\\SupportFiles\\";

            SimpleSearch(supportPath + "content.pdf");
            SearchResultsWithContext(supportPath + "content.pdf");

            Console.WriteLine("File(s) saved with success to current folder.");
        }

        private static void SimpleSearch(string fileName)
        {
            PDFFixedDocument document = new PDFFixedDocument(fileName);
            PDFContentExtractor ce = new PDFContentExtractor(document.Pages[0]);

            // Simple search.
            PDFTextSearchResultCollection searchResults = ce.SearchText("at");
            HighlightSearchResults(document.Pages[0], searchResults, PDFRgbColor.Red, false);

            // Whole words search.
            searchResults = ce.SearchText("at", PDFTextSearchOptions.WholeWordSearch);
            HighlightSearchResults(document.Pages[0], searchResults, PDFRgbColor.Green, false);

            // Regular expression search, find all words that start with uppercase.
            searchResults = ce.SearchText("[A-Z][a-z]*", PDFTextSearchOptions.RegExSearch);
            HighlightSearchResults(document.Pages[0], searchResults, PDFRgbColor.Blue, false);

            document.Save("searchtext.simple.pdf");
        }

        private static void SearchResultsWithContext(string fileName)
        {
            PDFFixedDocument document = new PDFFixedDocument(fileName);
            PDFContentExtractor ce = new PDFContentExtractor(document.Pages[0]);

            PDFContentExtractionContext context = new PDFContentExtractionContext();
            context.TextSeachContext = new PDFTextSearchContext();
            // How many characters we want before the search result
            context.TextSeachContext.TextSearchResultPrefixLength = 20;
            // How many characters we want after the search result
            context.TextSeachContext.TextSearchResultSuffixLength = 20;

            // Whole words search.
            PDFTextSearchResultCollection searchResults = ce.SearchText("at", PDFTextSearchOptions.WholeWordSearch, context);
            for (int i = 0; i < searchResults.Count; i++)
            {
                Console.WriteLine("Before: " + searchResults[i].PrefixText);
                Console.WriteLine("Result: " + searchResults[i].Text);
                Console.WriteLine("After: " + searchResults[i].SuffixText);
                Console.WriteLine();
            }
            HighlightSearchResults(document.Pages[0], searchResults, PDFRgbColor.Green, true);

            document.Save("searchtext.resultswithcontext.pdf");
        }

        private static void HighlightSearchResults(PDFPage page, PDFTextSearchResultCollection searchResults, PDFColor color, bool highlightPrefixSuffix)
        {
            PDFPen pen = new PDFPen(color, 0.7);
            PDFPen pen2 = new PDFPen(color, 0.2);

            for (int i = 0; i < searchResults.Count; i++)
            {
                HighlightTextRuns(page, searchResults[i].TextRuns, pen);

                if (highlightPrefixSuffix)
                {
                    if (searchResults[i].PrefixTextRuns != null)
                    {
                        HighlightTextRuns(page, searchResults[i].PrefixTextRuns, pen2);
                    }
                    if (searchResults[i].SuffixTextRuns != null)
                    {
                        HighlightTextRuns(page, searchResults[i].SuffixTextRuns, pen2);
                    }
                }
            }
        }

        private static void HighlightTextRuns(PDFPage page, PDFTextRunCollection textFragments, PDFPen pen)
        {
            for (int j = 0; j < textFragments.Count; j++)
            {
                PDFPath path = new PDFPath();

                path.StartSubpath(textFragments[j].Corners[0].X, textFragments[j].Corners[0].Y);
                path.AddPolygon(textFragments[j].Corners);

                page.Canvas.DrawPath(pen, path);
            }
        }

    }
}
