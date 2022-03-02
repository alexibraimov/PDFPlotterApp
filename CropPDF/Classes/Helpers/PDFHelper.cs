using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace CropPDF.Classes.Helpers
{
    public static class PDFHelper
    {
        public static event Action<bool> Completed;

        public static void CropPDF(string pathFile, bool isOpenFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(pathFile);
            var folder = Path.GetDirectoryName(pathFile);

            var tempFolder = FileHelper.Create("temp");
            var patternFile = Path.Combine(folder, $"{fileName}_pattern.pdf");
            var resultFile = Path.Combine(folder, $"{fileName}_result.pdf");
            var pngPathFile = Path.Combine(tempFolder, $"{Guid.NewGuid()}.png");
            var compressPdfPathFile = Path.Combine(tempFolder, $"{Guid.NewGuid()}.pdf");
            var newFileName = $"{Guid.NewGuid()}.pdf";
            var newFilePath = Path.Combine(tempFolder, newFileName);
            File.Copy(pathFile, Path.Combine(tempFolder, newFileName));
            try
            {
                CropHackLib.GhostScriptFacade.GetImageFromPdf(newFilePath, pngPathFile);
                var form = XImage.FromFile(newFilePath);
                CropHackLib.iTextSharpFacade.GetPdfFromImage(pngPathFile, compressPdfPathFile, (float)form.PointWidth, (float)form.PointHeight);
                form.Dispose();

                form = XImage.FromFile(compressPdfPathFile);
                var border = Converter.GetPoints(10);
                var array = A4.GetCountPage(form.PixelWidth, form.PixelHeight);
                var doc = new PdfDocument();
                var width = Converter.GetPoints(A4.Width);
                var height = Converter.GetPoints(A4.Height);
                var page = doc.AddPage();
                CropIntoPieces(form, width - border, height - border, tempFolder);
                page.Width = new XUnit(width * array[0]);
                page.Height = new XUnit(height * array[1]);
                var graphics = XGraphics.FromPdfPage(page);

                for (int x = 0; x < array[0]; x++)
                {
                    for (int y = 0; y < array[1]; y++)
                    {
                        var font = new XFont("Arial", 10, XFontStyle.Bold);
                        graphics.DrawString($"Страница ({x}, {y})", font, XBrushes.Black, x * width + width / 2 - border / 2, y * height + height - border / 2);
                        graphics.DrawRectangle(new XPen(XColors.Black, 1)
                        {
                            DashStyle = XDashStyle.DashDot,
                            DashPattern = new double[] { 10, 20, 10 }
                        }, new XRect(x * (width), y * (height), width - border, height - border));
                        graphics.DrawRectangle(new XPen(XColors.Red, 2)
                        {
                            DashStyle = XDashStyle.Solid
                        }, new XRect(x * width, y * height, width, height));
                        var f = XImage.FromFile($"{tempFolder}\\{x}{array[1] - 1 - y}.pdf");

                        graphics.DrawImage(f, new XRect(x * width, y * height, width - border, height - border));
                        f.Dispose();
                    }
                }

                doc.Save(patternFile);

                GluePieces(XImage.FromFile(patternFile), Converter.GetPoints(A4.Width), Converter.GetPoints(A4.Height), resultFile);

                Completed?.Invoke(true);
            }
            catch (Exception ex)
            {
                Completed?.Invoke(false);
            }

            Directory.Delete(tempFolder, true);

            if (isOpenFile && File.Exists(resultFile))
            {
                System.Diagnostics.Process.Start(resultFile);
            }
        }

        private static void CropIntoPieces(XImage form, float width, float height, string folder)
        {
            var index = 0;
            var array = A4.GetCountPage(form.PixelWidth, form.PixelHeight);

            for (int y = 0; y < array[1]; y++)
            {
                for (int x = 0; x < array[0]; x++)
                {
                    var doc = new PdfDocument();
                    var page = doc.AddPage();
                    var graphics = XGraphics.FromPdfPage(page);
                    page.Width = new XUnit(form.PixelWidth);
                    page.Height = new XUnit(form.PointHeight);
                    graphics.DrawImage(form, 0, 0, form.PixelWidth, form.PointHeight);

                    page.MediaBox = new PdfRectangle(new XRect(x * width, y * height, width, height));
                    doc.Save($"{folder}\\{x}{y}.pdf");

                    CompressPdf($"{folder}\\{x}{y}.pdf");
                    index++;
                    doc.Dispose();
                }
            }
        }

        private static void GluePieces(XImage form, float width, float height, string fileName)
        {
            var index = 0;
            var array = A4.GetCountPage(form.PixelWidth, form.PixelHeight, false);

            var doc = new PdfDocument();
            for (int y = 0; y < array[1]; y++)
            {
                for (int x = 0; x < array[0]; x++)
                {
                    var page = doc.AddPage();
                    var graphics = XGraphics.FromPdfPage(page);
                    page.Width = new XUnit(form.PixelWidth);
                    page.Height = new XUnit(form.PointHeight);
                    graphics.DrawImage(form, 0, 0, form.PixelWidth, form.PointHeight);
                    page.MediaBox = new PdfRectangle(new XRect(x * width, y * height, width, height));
                    index++;
                }
            }

            doc.Save(fileName);
        }

        public static void CompressPdf(string outputFile, string inputFile)
        {

        }

        public static void CompressPdf(string targetPath)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(targetPath)) { Position = 0 })
            using (var source = PdfReader.Open(stream, PdfDocumentOpenMode.Import))
            using (var document = new PdfDocument())
            {
                var options = document.Options;
                options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
                options.CompressContentStreams = true;
                options.NoCompression = false;
                foreach (var page in source.Pages)
                {
                    document.AddPage(page);
                }

                document.Save(targetPath);
            }
        }
    }
}
