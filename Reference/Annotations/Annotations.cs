using System;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Annotations;
using O2S.Components.PDF4NET.Actions;
using O2S.Components.PDF4NET.Destinations;
using System.IO;
using O2S.Components.PDF4NET.Forms;

namespace O2S.Components.PDF4NET.Samples
{
    /// <summary>
    /// Annotations sample.
    /// </summary>
    public class Annotations
    {
        /// <summary>
        /// Main method for running the sample.
        /// </summary>
        public static SampleOutputInfo[] Run(Stream flashStream, Stream u3dStream)
        {
            // Create a PDF document with 10 pages.
            PDFFixedDocument document = new PDFFixedDocument();
            PDFStandardFont helvetica = new PDFStandardFont(PDFStandardFontFace.Helvetica, 12);

            CreateTextAnnotations(document, helvetica);

            CreateSquareCircleAnnotations(document, helvetica);

            CreateFileAttachmentAnnotations(document, helvetica);

            CreateInkAnnotations(document, helvetica);

            CreateLineAnnotations(document, helvetica);

            CreatePolygonAnnotations(document, helvetica);

            CreatePolylineAnnotations(document, helvetica);

            CreateCloudAnnotations(document, helvetica);

            CreateRubberStampAnnotations(document, helvetica);

            CreateTextMarkupAnnotations(document, helvetica);

            CreateRichMediaAnnotations(document, helvetica, flashStream);

            Create3DAnnotations(document, helvetica, u3dStream);

            CreateRedactionAnnotations(document, helvetica, u3dStream);

            // Compress the page graphic content.
            for (int i = 0; i < document.Pages.Count; i++)
            {
                document.Pages[i].Canvas.CompressAndClose();
            }

            SampleOutputInfo[] output = new SampleOutputInfo[] { new SampleOutputInfo(document, "annotations.pdf") };
            return output;
        }

        private static void CreateTextAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            string[] textAnnotationNames = new string[] 
                { 
                    "Comment", "Check", "Circle", "Cross", "Help", "Insert", "Key", "NewParagraph", 
                    "Note", "Paragraph", "RightArrow", "RightPointer", "Star", "UpArrow", "UpLeftArrow" 
                };

            page.Canvas.DrawString("B/W text annotations", font, blackBrush, 50, 50);
            for (int i = 0; i < textAnnotationNames.Length; i++)
            {
                PDFTextAnnotation ta = new PDFTextAnnotation();
                ta.Author = "O2S.Components.PDF4NET";
                ta.Contents = "I am a " + textAnnotationNames[i] + " annotation.";
                ta.Icon = textAnnotationNames[i];
                page.Annotations.Add(ta);
                ta.Location = new PDFPoint(50, 100 + 40 * i);
                page.Canvas.DrawString(textAnnotationNames[i], font, blackBrush, 90, 100 + 40 * i);
            }

            Random rnd = new Random();
            byte[] rgb = new byte[3];
            page.Canvas.DrawString("Color text annotations", font, blackBrush, 300, 50);
            for (int i = 0; i < textAnnotationNames.Length; i++)
            {
                PDFTextAnnotation ta = new PDFTextAnnotation();
                ta.Author = "O2S.Components.PDF4NET";
                ta.Contents = "I am a " + textAnnotationNames[i] + " annotation.";
                ta.Icon = textAnnotationNames[i];
                rnd.NextBytes(rgb);
                ta.OutlineColor = new PDFRgbColor(rgb[0], rgb[1], rgb[2]);
                rnd.NextBytes(rgb);
                ta.InteriorColor = new PDFRgbColor(rgb[0], rgb[1], rgb[2]);
                page.Annotations.Add(ta);
                ta.Location = new PDFPoint(300, 100 + 40 * i);
                page.Canvas.DrawString(textAnnotationNames[i], font, blackBrush, 340, 100 + 40 * i);
            }

            page.Canvas.DrawString("Text annotations with custom icons", font, blackBrush, 50, 700);
            PDFTextAnnotation customTextAnnotation = new PDFTextAnnotation();
            customTextAnnotation.Author = "O2S.Components.PDF4NET";
            customTextAnnotation.Contents = "Text annotation with custom icon.";
            page.Annotations.Add(customTextAnnotation);
            customTextAnnotation.Icon = "Custom icon appearance";
            customTextAnnotation.Location = new PDFPoint(50, 720);

            PDFAnnotationAppearance customAppearance = new PDFAnnotationAppearance(50, 50);
            PDFPen redPen = new PDFPen(new PDFRgbColor(255, 0, 0), 10);
            PDFPen bluePen = new PDFPen(new PDFRgbColor(0, 0, 192), 10);
            customAppearance.Canvas.DrawRectangle(redPen, 5, 5, 40, 40);
            customAppearance.Canvas.DrawLine(bluePen, 0, 0, customAppearance.Width, customAppearance.Height);
            customAppearance.Canvas.DrawLine(bluePen, 0, customAppearance.Height, customAppearance.Width, 0);
            customAppearance.Canvas.CompressAndClose();
            customTextAnnotation.NormalAppearance = customAppearance;
        }

        private static void CreateSquareCircleAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Square annotations", font, blackBrush, 50, 50);

            PDFSquareAnnotation square1 = new PDFSquareAnnotation();
            page.Annotations.Add(square1);
            square1.Author = "PDF4NET";
            square1.Contents = "Square annotation with red border";
            square1.Border = new PDFAnnotationBorder();
            square1.Border.Color = new PDFRgbColor(255, 0, 0);
            square1.Border.Width = 3;
            square1.DisplayRectangle = new PDFDisplayRectangle(50, 70, 250, 150);

            PDFSquareAnnotation square2 = new PDFSquareAnnotation();
            page.Annotations.Add(square2);
            square2.Author = "PDF4NET";
            square2.Contents = "Square annotation with blue interior";
            square2.Border = new PDFAnnotationBorder();
            square2.Border.Color = null;
            square2.Border.Width = 0;
            square2.InteriorColor = new PDFRgbColor(0, 0, 192);
            square2.DisplayRectangle = new PDFDisplayRectangle(50, 270, 250, 150);

            PDFSquareAnnotation square3 = new PDFSquareAnnotation();
            page.Annotations.Add(square3);
            square3.Author = "PDF4NET";
            square3.Contents = "Square annotation with yellow border and green interior";
            square3.Border = new PDFAnnotationBorder();
            square3.Border.Color = new PDFRgbColor(255, 255, 0);
            square3.Border.Width = 3;
            square3.InteriorColor = new PDFRgbColor(0, 192, 0);
            square3.DisplayRectangle = new PDFDisplayRectangle(50, 470, 250, 150);

            page.Canvas.DrawString("Circle annotations", font, blackBrush, 50, 350);

            PDFCircleAnnotation circle1 = new PDFCircleAnnotation();
            page.Annotations.Add(circle1);
            circle1.Author = "PDF4NET";
            circle1.Contents = "Circle annotation with red border";
            circle1.Border = new PDFAnnotationBorder();
            circle1.Border.Color = new PDFRgbColor(255, 0, 0);
            circle1.Border.Width = 3;
            circle1.DisplayRectangle = new PDFDisplayRectangle(350, 70, 250, 150);

            PDFCircleAnnotation circle2 = new PDFCircleAnnotation();
            page.Annotations.Add(circle2);
            circle2.Author = "PDF4NET";
            circle2.Contents = "Circle annotation with blue interior";
            circle2.Border = new PDFAnnotationBorder();
            circle2.Border.Color = null;
            circle2.Border.Width = 0;
            circle2.InteriorColor = new PDFRgbColor(0, 0, 192);
            circle2.DisplayRectangle = new PDFDisplayRectangle(350, 270, 250, 150);

            PDFCircleAnnotation circle3 = new PDFCircleAnnotation();
            page.Annotations.Add(circle3);
            circle3.Author = "PDF4NET";
            circle3.Contents = "Circle annotation with yellow border and green interior";
            circle3.Border = new PDFAnnotationBorder();
            circle3.Border.Color = new PDFRgbColor(255, 255, 0);
            circle3.Border.Width = 3;
            circle3.InteriorColor = new PDFRgbColor(0, 192, 0);
            circle3.DisplayRectangle = new PDFDisplayRectangle(350, 470, 250, 150);
        }

        private static void CreateFileAttachmentAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();
            Random rnd = new Random();
            // Random binary data to be used a file content for file attachment annotations.
            byte[] fileData = new byte[256];

            PDFPage page = document.Pages.Add();

            string[] fileAttachmentAnnotationNames = new string[] 
                { 
                    "Graph", "Paperclip", "PushPin", "Tag"
                };

            page.Canvas.DrawString("B/W file attachment annotations", font, blackBrush, 50, 50);
            for (int i = 0; i < fileAttachmentAnnotationNames.Length; i++)
            {
                rnd.NextBytes(fileData);

                PDFFileAttachmentAnnotation faa = new PDFFileAttachmentAnnotation();
                faa.Author = "O2S.Components.PDF4NET";
                faa.Contents = "I am a " + fileAttachmentAnnotationNames[i] + " annotation.";
                faa.Icon = fileAttachmentAnnotationNames[i];
                faa.Payload = fileData;
                faa.FileName = "BlackAndWhite" + fileAttachmentAnnotationNames[i] + "dat";
                page.Annotations.Add(faa);
                faa.Location = new PDFPoint(50, 100 + 40 * i);
                page.Canvas.DrawString(fileAttachmentAnnotationNames[i], font, blackBrush, 90, 100 + 40 * i);
            }

            byte[] rgb = new byte[3];
            page.Canvas.DrawString("Color file attachment annotations", font, blackBrush, 300, 50);
            for (int i = 0; i < fileAttachmentAnnotationNames.Length; i++)
            {
                rnd.NextBytes(fileData);

                PDFFileAttachmentAnnotation faa = new PDFFileAttachmentAnnotation();
                faa.Author = "O2S.Components.PDF4NET";
                faa.Contents = "I am a " + fileAttachmentAnnotationNames[i] + " annotation.";
                faa.Icon = fileAttachmentAnnotationNames[i];
                faa.Payload = fileData;
                faa.FileName = "Color" + fileAttachmentAnnotationNames[i] + "dat";
                rnd.NextBytes(rgb);
                faa.OutlineColor = new PDFRgbColor(rgb[0], rgb[1], rgb[2]);
                rnd.NextBytes(rgb);
                faa.InteriorColor = new PDFRgbColor(rgb[0], rgb[1], rgb[2]);
                page.Annotations.Add(faa);
                faa.Location = new PDFPoint(300, 100 + 40 * i);
                page.Canvas.DrawString(fileAttachmentAnnotationNames[i], font, blackBrush, 340, 100 + 40 * i);
            }

            page.Canvas.DrawString("File attachment annotations with custom icons", font, blackBrush, 50, 700);
            PDFFileAttachmentAnnotation customFileAttachmentAnnotation = new PDFFileAttachmentAnnotation();
            customFileAttachmentAnnotation.Author = "O2S.Components.PDF4NET";
            customFileAttachmentAnnotation.Contents = "File attachment annotation with custom icon.";
            page.Annotations.Add(customFileAttachmentAnnotation);
            customFileAttachmentAnnotation.Icon = "Custom icon appearance";
            customFileAttachmentAnnotation.Location = new PDFPoint(50, 720);

            PDFAnnotationAppearance customAppearance = new PDFAnnotationAppearance(50, 50);
            PDFPen redPen = new PDFPen(new PDFRgbColor(255, 0, 0), 10);
            PDFPen bluePen = new PDFPen(new PDFRgbColor(0, 0, 192), 10);
            customAppearance.Canvas.DrawRectangle(redPen, 5, 5, 40, 40);
            customAppearance.Canvas.DrawLine(bluePen, 0, 0, customAppearance.Width, customAppearance.Height);
            customAppearance.Canvas.DrawLine(bluePen, 0, customAppearance.Height, customAppearance.Width, 0);
            customAppearance.Canvas.CompressAndClose();
            customFileAttachmentAnnotation.NormalAppearance = customAppearance;
        }

        private static void CreateInkAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();
            Random rnd = new Random();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Ink annotations", font, blackBrush, 50, 50);

            // The ink annotation will contain 3 lines, each one with 10 points.
            PDFPoint[][] points = new PDFPoint[3][];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PDFPoint[10];
                for (int j = 0; j < points[i].Length; j++)
                {
                    points[i][j] = new PDFPoint(rnd.NextDouble() * page.Width, rnd.NextDouble() * page.Height);
                }
            }

            PDFInkAnnotation ia = new PDFInkAnnotation();
            page.Annotations.Add(ia);
            ia.Contents = "I am an ink annotation.";
            ia.Ink = new PDFAnnotationBorder();
            ia.Ink.Color = new PDFRgbColor(255, 0, 255);
            ia.Ink.Width = 5;
            ia.Points = points;
        }

        private static void CreateLineAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Line annotations", font, blackBrush, 50, 50);

            PDFLineEndingStyle[] les = new PDFLineEndingStyle[] 
                {
                    PDFLineEndingStyle.Circle, PDFLineEndingStyle.ClosedArrow, PDFLineEndingStyle.None, PDFLineEndingStyle.OpenArrow
                };

            for (int i = 0; i < les.Length; i++)
            {
                PDFLineAnnotation la = new PDFLineAnnotation();
                page.Annotations.Add(la);
                la.Author = "O2S.Components.PDF4NET";
                la.Contents = "I am a line annotation with " + les[i].ToString() + " ending.";
                la.StartPoint = new PDFPoint(50, 100 + 40 * i);
                la.EndPoint = new PDFPoint(250, 100 + 40 * i);
                la.EndLineSymbol = les[i];
                page.Canvas.DrawString("Line end symbol: " + les[i].ToString(), font, blackBrush, 270, 100 + 40 * i);
            }
        }

        private static void CreatePolygonAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Polygon annotations", font, blackBrush, 50, 50);

            int[] vertices = new int[]{ 3, 4, 5, 6 };
            double centerY = 125, centerX = 150;
            double radius = 50;

            for (int i = 0; i < vertices.Length; i++)
            {
                PDFPoint[] points = new PDFPoint[vertices[i]];
                double angle = 90;
                double rotation = 360 / vertices[i];

                for (int j = 0; j < vertices[i]; j++)
                {
                    points[j] = new PDFPoint();
                    points[j].X = centerX + radius * Math.Cos(Math.PI * angle / 180);
                    points[j].Y = centerY - radius * Math.Sin(Math.PI * angle / 180);
                    angle = angle + rotation;
                }

                PDFPolygonAnnotation polygon = new PDFPolygonAnnotation();
                page.Annotations.Add(polygon);
                polygon.Author = "O2S.Components.PDF4NET";
                polygon.Contents = "Polygon annotation with " + vertices[i] + " vertices.";
                polygon.Points = points;
                polygon.Border = new PDFAnnotationBorder();
                polygon.Border.Color = new PDFRgbColor(192, 0, 0);
                polygon.Border.Width = 3;
                polygon.InteriorColor = new PDFRgbColor(0, 0, 192);

                centerY = centerY + 150;
            }
        }

        private static void CreatePolylineAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Polyline annotations", font, blackBrush, 50, 50);

            int[] vertices = new int[] { 3, 4, 5, 6 };
            double centerY = 125, centerX = 150;
            double radius = 50;

            for (int i = 0; i < vertices.Length; i++)
            {
                PDFPoint[] points = new PDFPoint[vertices[i]];
                double angle = 90;
                double rotation = 360 / vertices[i];

                for (int j = 0; j < vertices[i]; j++)
                {
                    points[j] = new PDFPoint();
                    points[j].X = centerX + radius * Math.Cos(Math.PI * angle / 180);
                    points[j].Y = centerY - radius * Math.Sin(Math.PI * angle / 180);
                    angle = angle + rotation;
                }

                PDFPolylineAnnotation polyline = new PDFPolylineAnnotation();
                page.Annotations.Add(polyline);
                polyline.Author = "O2S.Components.PDF4NET";
                polyline.Contents = "Polyline annotation with " + vertices[i] + " vertices.";
                polyline.Points = points;
                polyline.Border = new PDFAnnotationBorder();
                polyline.Border.Color = new PDFRgbColor(192, 0, 0);
                polyline.Border.Width = 3;

                centerY = centerY + 150;
            }
        }


        private static void CreateCloudAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Cloud square annotations", font, blackBrush, 50, 50);

            PDFCloudSquareAnnotation cloudSquare1 = new PDFCloudSquareAnnotation();
            page.Annotations.Add(cloudSquare1);
            cloudSquare1.Author = "PDF4NET";
            cloudSquare1.Contents = "Cloud square annotation with red border";
            cloudSquare1.Border = new PDFAnnotationBorder();
            cloudSquare1.Border.Color = new PDFRgbColor(255, 0, 0);
            cloudSquare1.Border.Width = 1;
            cloudSquare1.Intensity = 1;
            cloudSquare1.DisplayRectangle = new PDFDisplayRectangle(50, 70, 250, 150);

            PDFCloudSquareAnnotation cloudSquare2 = new PDFCloudSquareAnnotation();
            page.Annotations.Add(cloudSquare2);
            cloudSquare2.Author = "PDF4NET";
            cloudSquare2.Contents = "Cloud square annotation with blue interior";
            cloudSquare2.Border = new PDFAnnotationBorder();
            cloudSquare2.Border.Color = null;
            cloudSquare2.Border.Width = 0;
            cloudSquare2.Intensity = 2;
            cloudSquare2.InteriorColor = new PDFRgbColor(0, 0, 192);
            cloudSquare2.DisplayRectangle = new PDFDisplayRectangle(50, 270, 250, 150);

            PDFCloudSquareAnnotation cloudSquare3 = new PDFCloudSquareAnnotation();
            page.Annotations.Add(cloudSquare3);
            cloudSquare3.Author = "PDF4NET";
            cloudSquare3.Contents = "Cloud square annotation with yellow border and green interior";
            cloudSquare3.Border = new PDFAnnotationBorder();
            cloudSquare3.Border.Color = new PDFRgbColor(255, 255, 0);
            cloudSquare3.Border.Width = 1;
            cloudSquare3.InteriorColor = new PDFRgbColor(0, 192, 0);
            cloudSquare3.DisplayRectangle = new PDFDisplayRectangle(50, 470, 250, 150);

            page.Canvas.DrawString("Cloud polygon annotations", font, blackBrush, 350, 50);

            PDFCloudPolygonAnnotation cloudPolygon1 = new PDFCloudPolygonAnnotation();
            page.Annotations.Add(cloudPolygon1);
            cloudPolygon1.Author = "PDF4NET";
            cloudPolygon1.Contents = "Cloud polygon annotation with red border";
            cloudPolygon1.Border = new PDFAnnotationBorder();
            cloudPolygon1.Border.Color = new PDFRgbColor(255, 0, 0);
            cloudPolygon1.Border.Width = 3;
            cloudPolygon1.Points = new PDFPoint[] { new PDFPoint(350, 145), new PDFPoint(475, 70), new PDFPoint(600, 145), new PDFPoint(475, 220), new PDFPoint(350, 145) };

            PDFCloudPolygonAnnotation cloudPolygon2 = new PDFCloudPolygonAnnotation();
            page.Annotations.Add(cloudPolygon2);
            cloudPolygon2.Author = "PDF4NET";
            cloudPolygon2.Contents = "Cloud polygon annotation with blue interior";
            cloudPolygon2.Border = new PDFAnnotationBorder();
            cloudPolygon2.Border.Color = null;
            cloudPolygon2.Border.Width = 0;
            cloudPolygon2.InteriorColor = new PDFRgbColor(0, 0, 192);
            cloudPolygon2.Intensity = 1;
            cloudPolygon2.Points = new PDFPoint[] {
                new PDFPoint(350, 345), new PDFPoint(370, 290), new PDFPoint(475, 270), new PDFPoint(580, 290),
                new PDFPoint(600, 345), new PDFPoint(580, 400), new PDFPoint(475, 420), new PDFPoint(370, 400), new PDFPoint(350, 345)
            };

            PDFCloudPolygonAnnotation cloudPolygon3 = new PDFCloudPolygonAnnotation();
            page.Annotations.Add(cloudPolygon3);
            cloudPolygon3.Author = "PDF4NET";
            cloudPolygon3.Contents = "Cloud polygon annotation with yellow border and green interior";
            cloudPolygon3.Border = new PDFAnnotationBorder();
            cloudPolygon3.Border.Color = new PDFRgbColor(255, 255, 0);
            cloudPolygon3.Border.Width = 1;
            cloudPolygon3.InteriorColor = new PDFRgbColor(0, 192, 0);
            cloudPolygon3.Points = new PDFPoint[] {
                new PDFPoint(350, 545), new PDFPoint(455, 525), new PDFPoint(475, 470), new PDFPoint(495, 525),
                new PDFPoint(600, 545), new PDFPoint(495, 565), new PDFPoint(475, 620), new PDFPoint(455, 565), new PDFPoint(350, 545)
            };
        }

        private static void CreateRubberStampAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            string[] rubberStampAnnotationNames = new string[] 
                { 
                    "Approved", "AsIs", "Confidential", "Departmental", "Draft", "Experimental", "Expired", "Final", 
                    "ForComment", "ForPublicRelease", "NotApproved", "NotForPublicRelease", "Sold", "TopSecret" 
                };

            page.Canvas.DrawString("Rubber stamp annotations", font, blackBrush, 50, 50);
            for (int i = 0; i < rubberStampAnnotationNames.Length; i++)
            {
                PDFRubberStampAnnotation rsa = new PDFRubberStampAnnotation();
                rsa.Author = "O2S.Components.PDF4NET";
                rsa.Contents = "I am a " + rubberStampAnnotationNames[i] + " rubber stamp annotation.";
                rsa.StampName = rubberStampAnnotationNames[i];
                page.Annotations.Add(rsa);
                rsa.DisplayRectangle = new PDFDisplayRectangle(50, 70 + 50 * i, 200, 40);
                page.Canvas.DrawString(rubberStampAnnotationNames[i], font, blackBrush, 270, 85 + 50 * i);
            }

            page.Canvas.DrawString("Stamp annotations with custom appearance", font, blackBrush, 350, 50);
            PDFRubberStampAnnotation customRubberStampAnnotation = new PDFRubberStampAnnotation();
            customRubberStampAnnotation.Contents = "Rubber stamp annotation with custom appearance.";
            customRubberStampAnnotation.StampName = "Custom";
            page.Annotations.Add(customRubberStampAnnotation);
            customRubberStampAnnotation.DisplayRectangle = new PDFDisplayRectangle(350, 70, 200, 40);

            PDFAnnotationAppearance customAppearance = new PDFAnnotationAppearance(50, 50);
            PDFPen redPen = new PDFPen(new PDFRgbColor(255, 0, 0), 10);
            PDFPen bluePen = new PDFPen(new PDFRgbColor(0, 0, 192), 10);
            customAppearance.Canvas.DrawRectangle(redPen, 5, 5, 40, 40);
            customAppearance.Canvas.DrawLine(bluePen, 0, 0, customAppearance.Width, customAppearance.Height);
            customAppearance.Canvas.DrawLine(bluePen, 0, customAppearance.Height, customAppearance.Width, 0);
            customAppearance.Canvas.CompressAndClose();
            customRubberStampAnnotation.NormalAppearance = customAppearance;
        }

        private static void CreateTextMarkupAnnotations(PDFFixedDocument document, PDFFont font)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Text markup annotations", font, blackBrush, 50, 50);

            PDFTextMarkupAnnotationType[] tmat = new PDFTextMarkupAnnotationType[] 
                { 
                    PDFTextMarkupAnnotationType.Highlight, PDFTextMarkupAnnotationType.Squiggly, PDFTextMarkupAnnotationType.StrikeOut, PDFTextMarkupAnnotationType.Underline
                };

            PDFStringAppearanceOptions sao = new PDFStringAppearanceOptions();
            sao.Brush = blackBrush;
            sao.Font = font;

            PDFStringLayoutOptions slo = new PDFStringLayoutOptions();
            slo.HorizontalAlign = PDFStringHorizontalAlign.Center;
            slo.VerticalAlign = PDFStringVerticalAlign.Bottom;
            for (int i = 0; i < tmat.Length; i++)
            {
                PDFTextMarkupAnnotation tma = new PDFTextMarkupAnnotation();
                page.Annotations.Add(tma);
                tma.DisplayRectangle = new PDFDisplayRectangle(50, 100 + 50 * i, 200, font.Size + 2);
                tma.TextMarkupType = tmat[i];

                slo.X = 150;
                slo.Y = 100 + 50 * i + font.Size;
                
                page.Canvas.DrawString(tmat[i].ToString() + " annotation.", sao, slo);
            }
        }

        private static void CreateRichMediaAnnotations(PDFFixedDocument document, PDFFont font, Stream flashStream)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Rich media annotations", font, blackBrush, 50, 50);

            byte[] flashContent = new byte[flashStream.Length];
            flashStream.Read(flashContent, 0, flashContent.Length);

            PDFRichMediaAnnotation rma = new PDFRichMediaAnnotation();
            page.Annotations.Add(rma);
            rma.DisplayRectangle = new PDFDisplayRectangle(100, 100, 400, 400);
            rma.FlashPayload = flashContent;
            rma.FlashFile = "clock.swf";
            rma.ActivationCondition = PDFRichMediaActivationCondition.PageVisible;
        }

        private static void Create3DAnnotations(PDFFixedDocument document, PDFFont font, Stream u3dStream)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();
            page.Rotation = 90;

            page.Canvas.DrawString("3D annotations", font, blackBrush, 50, 50);

            byte[] u3dContent = new byte[u3dStream.Length];
            u3dStream.Read(u3dContent, 0, u3dContent.Length);

            PDF3DView view0 = new PDF3DView();
            view0.CameraToWorldMatrix = new double[] { 1, 0, 0, 0, 0, -1, 0, 1, 0, -0.417542, -0.881257, -0.125705 };
            view0.CenterOfOrbit = 0.123106;
            view0.ExternalName = "Default";
            view0.InternalName = "Default";
            view0.Projection = new PDF3DProjection();
            view0.Projection.FieldOfView = 30;

            PDF3DView view1 = new PDF3DView();
            view1.CameraToWorldMatrix = new double[] { -0.999888, 0.014949, 0, 0.014949, 0.999887, 0.00157084, 0.0000234825, 0.00157066, -0.999999, -0.416654, -0.761122, -0.00184508 };
            view1.CenterOfOrbit = 0.123106;
            view1.ExternalName = "Top";
            view1.InternalName = "Top";
            view1.Projection = new PDF3DProjection();
            view1.Projection.FieldOfView = 14.8096;

            PDF3DView view2 = new PDF3DView();
            view2.CameraToWorldMatrix = new double[] { -1.0, -0.0000411671, 0.0000000000509201, -0.00000101387, 0.0246288, 0.999697, -0.0000411546, 0.999697, -0.0246288, -0.417072, -0.881787, -0.121915 };
            view2.CenterOfOrbit = 0.123106;
            view2.ExternalName = "Side";
            view2.InternalName = "Side";
            view2.Projection = new PDF3DProjection();
            view2.Projection.FieldOfView = 12.3794;

            PDF3DView view3 = new PDF3DView();
            view3.CameraToWorldMatrix = new double[] { -0.797002, -0.603977, -0.0000000438577, -0.294384, 0.388467, 0.873173, -0.527376, 0.695921, -0.48741, -0.3518, -0.844506, -0.0675629 };
            view3.CenterOfOrbit = 0.123106;
            view3.ExternalName = "Isometric";
            view3.InternalName = "Isometric";
            view3.Projection = new PDF3DProjection();
            view3.Projection.FieldOfView = 15.1226;

            PDF3DView view4 = new PDF3DView();
            view4.CameraToWorldMatrix = new double[] { 0.00737633, -0.999973, -0.0000000000147744, -0.0656414, -0.000484206, 0.997843, -0.997816, -0.00736042, -0.0656432, -0.293887, -0.757928, -0.119485 };
            view4.CenterOfOrbit = 0.123106;
            view4.ExternalName = "Front";
            view4.InternalName = "Front";
            view4.Projection = new PDF3DProjection();
            view4.Projection.FieldOfView = 15.1226;

            PDF3DView view5 = new PDF3DView();
            view5.CameraToWorldMatrix = new double[] { 0.0151008, 0.999886, 0.0000000000261366, 0.0492408, -0.000743662, 0.998787, 0.998673, -0.0150825, -0.0492464, -0.540019, -0.756862, -0.118884 };
            view5.CenterOfOrbit = 0.123106;
            view5.ExternalName = "Back";
            view5.InternalName = "Back";
            view5.Projection = new PDF3DProjection();
            view5.Projection.FieldOfView = 12.3794;

            PDF3DStream _3dStream = new PDF3DStream();
            _3dStream.Views.Add(view0);
            _3dStream.Views.Add(view1);
            _3dStream.Views.Add(view2);
            _3dStream.Views.Add(view3);
            _3dStream.Views.Add(view4);
            _3dStream.Views.Add(view5);
            _3dStream.Content = u3dContent;
            _3dStream.DefaultViewIndex = 0;
            PDF3DAnnotation _3da = new PDF3DAnnotation();
            _3da.Stream = _3dStream;

            PDFAnnotationAppearance appearance = new PDFAnnotationAppearance(200, 200);
            appearance.Canvas.DrawString("Click to activate", font, blackBrush, 50, 50);
            _3da.NormalAppearance = appearance;

            page.Annotations.Add(_3da);
            _3da.DisplayRectangle = new PDFDisplayRectangle(36, 36, 720, 540);

            PDFStringAppearanceOptions sao = new PDFStringAppearanceOptions();
            sao.Font = font;
            sao.Brush = blackBrush;
            PDFStringLayoutOptions slo = new PDFStringLayoutOptions();
            slo.Y = 585 + 18 / 2;
            slo.HorizontalAlign = PDFStringHorizontalAlign.Center;
            slo.VerticalAlign = PDFStringVerticalAlign.Middle;

            PDFPen blackPen = new PDFPen(new PDFRgbColor(0, 0, 0), 1);

            page.Canvas.DrawRectangle(blackPen, 50, 585, 120, 18);
            slo.X = 50 + 120 / 2;
            page.Canvas.DrawString("Top", sao, slo);

            PDFGoTo3DViewAction gotoTopView = new PDFGoTo3DViewAction();
            gotoTopView.ViewIndex = 1;
            gotoTopView.TargetAnnotation = _3da;
            PDFLinkAnnotation linkGotoTopView = new PDFLinkAnnotation();
            page.Annotations.Add(linkGotoTopView);
            linkGotoTopView.DisplayRectangle = new PDFDisplayRectangle(50, 585, 120, 18);
            linkGotoTopView.Action = gotoTopView;

            page.Canvas.DrawRectangle(blackPen, 190, 585, 120, 18);
            slo.X = 190 + 120 / 2;
            page.Canvas.DrawString("Side", sao, slo);

            PDFGoTo3DViewAction gotoSideView = new PDFGoTo3DViewAction();
            gotoSideView.ViewIndex = 2;
            gotoSideView.TargetAnnotation = _3da;
            PDFLinkAnnotation linkGotoSideView = new PDFLinkAnnotation();
            page.Annotations.Add(linkGotoSideView);
            linkGotoSideView.DisplayRectangle = new PDFDisplayRectangle(190, 585, 120, 18);
            linkGotoSideView.Action = gotoSideView;

            page.Canvas.DrawRectangle(blackPen, 330, 585, 120, 18);
            slo.X = 330 + 120 / 2;
            page.Canvas.DrawString("Isometric", sao, slo);

            PDFGoTo3DViewAction gotoIsometricView = new PDFGoTo3DViewAction();
            gotoIsometricView.ViewIndex = 3;
            gotoIsometricView.TargetAnnotation = _3da;
            PDFLinkAnnotation linkGotoIsometricView = new PDFLinkAnnotation();
            page.Annotations.Add(linkGotoIsometricView);
            linkGotoIsometricView.DisplayRectangle = new PDFDisplayRectangle(330, 585, 120, 18);
            linkGotoIsometricView.Action = gotoIsometricView;

            page.Canvas.DrawRectangle(blackPen, 470, 585, 120, 18);
            slo.X = 470 + 120 / 2;
            page.Canvas.DrawString("Front", sao, slo);

            PDFGoTo3DViewAction gotoFrontView = new PDFGoTo3DViewAction();
            gotoFrontView.ViewIndex = 4;
            gotoFrontView.TargetAnnotation = _3da;
            PDFLinkAnnotation linkGotoFrontView = new PDFLinkAnnotation();
            page.Annotations.Add(linkGotoFrontView);
            linkGotoFrontView.DisplayRectangle = new PDFDisplayRectangle(470, 585, 120, 18);
            linkGotoFrontView.Action = gotoFrontView;

            page.Canvas.DrawRectangle(blackPen, 610, 585, 120, 18);
            slo.X = 610 + 120 / 2;
            page.Canvas.DrawString("Back", sao, slo);

            PDFGoTo3DViewAction gotoBackView = new PDFGoTo3DViewAction();
            gotoBackView.ViewIndex = 5;
            gotoBackView.TargetAnnotation = _3da;
            PDFLinkAnnotation linkGotoBackView = new PDFLinkAnnotation();
            page.Annotations.Add(linkGotoBackView);
            linkGotoBackView.DisplayRectangle = new PDFDisplayRectangle(610, 585, 120, 18);
            linkGotoBackView.Action = gotoBackView;
        }

        private static void CreateRedactionAnnotations(PDFFixedDocument document, PDFFont font, Stream flashStream)
        {
            PDFBrush blackBrush = new PDFBrush();

            PDFPage page = document.Pages.Add();

            page.Canvas.DrawString("Redaction annotations", font, blackBrush, 50, 50);

            PDFStandardFont helvetica = new PDFStandardFont(PDFStandardFontFace.Helvetica, 10);
            page.Canvas.DrawString(
                "Open the file with Adobe Acrobat, right click on the annotation and select \"Apply redactions\" The text under the annotation will be redacted.", 
                helvetica, blackBrush, 50, 110);

            PDFFormXObject redactionAppearance = new PDFFormXObject(250, 150);
            redactionAppearance.Canvas.DrawRectangle(new PDFBrush(PDFRgbColor.LightGreen),
                0, 0, redactionAppearance.Width, redactionAppearance.Height);
            PDFStringAppearanceOptions sao = new PDFStringAppearanceOptions();
            sao.Brush = new PDFBrush(PDFRgbColor.DarkRed);
            sao.Font = new PDFStandardFont(PDFStandardFontFace.HelveticaBold, 32);
            PDFStringLayoutOptions slo = new PDFStringLayoutOptions();
            slo.Width = redactionAppearance.Width;
            slo.Height = redactionAppearance.Height;
            slo.X = 0;
            slo.Y = 0;
            slo.HorizontalAlign = PDFStringHorizontalAlign.Center;
            slo.VerticalAlign = PDFStringVerticalAlign.Middle;
            redactionAppearance.Canvas.DrawString("This content has been redacted", sao, slo);

            PDFRedactionAnnotation redactionAnnotation = new PDFRedactionAnnotation();
            page.Annotations.Add(redactionAnnotation);
            redactionAnnotation.Author = "PDF4NET";
            redactionAnnotation.BorderColor = new PDFRgbColor(192, 0, 0);
            redactionAnnotation.BorderWidth = 1;
            redactionAnnotation.OverlayAppearance = redactionAppearance;
            redactionAnnotation.DisplayRectangle = new PDFDisplayRectangle(50, 100, 250, 150);
        }
    }
}