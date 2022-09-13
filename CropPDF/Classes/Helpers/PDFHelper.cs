using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using static CropPDF.Classes.A4;

namespace CropPDF.Classes.Helpers
{
    public static class PDFHelper
    {
        private static string GetLetter(int index)
        {
            var str = @"АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            if (index < str.Length)
            {
                return str[index].ToString();
            }

            return $"{str[index % str.Length]}{index / str.Length}";
        }

        public static event Action<bool> Completed;

        public static event Action<string> Error;

        public static void CropPDF(string pathFile, bool isOpenFile)
        {
            try
            {
                var crop = new Crop(pathFile, GlobalSettings.Get("Border", 5)).Clone().Setup().Split().Join();

                crop.Dispose();
                Completed?.Invoke(true);

                if (isOpenFile && File.Exists(crop.FileResultPath))
                {
                    System.Diagnostics.Process.Start(crop.FileResultPath);
                }
            }
            catch (Exception ex)
            {
               Completed?.Invoke(false);
            }

            return;
        }

        public class Crop
        {
            private int _borderMM;
            private string _filePath;
            private string _folder;
            private string _fileName;
            private string _tempFolder;

            public string FileResultPath => Path.Combine(_folder, $"{_fileName}_result.pdf");
            public string FilePatternPath => Path.Combine(_folder, $"{_fileName}_pattern.pdf");


            public Crop(string filePath, int borderMM)
            {
                _filePath = filePath;
                _borderMM = borderMM;
                _folder = Path.GetDirectoryName(_filePath);
                _fileName = Path.GetFileNameWithoutExtension(_filePath);
                _tempFolder = FileHelper.Create("temp");
            }

            private float _borderUnit;
            private float _widthUnit;
            private float _heightUnit;
            private int _dpi;
            private int _column;
            private int _row;
            private XImage _form;

            public Crop Setup()
            {
                _form = XImage.FromFile(_filePath);
                _dpi = (int)_form.HorizontalResolution;
                _heightUnit = _form.PixelHeight;
                _widthUnit = _form.PixelWidth;
                _borderUnit = Converter.GetPoints(_borderMM, _dpi);

                return this;
            }

            public Crop Clone()
            {
                var newFileName = $"{Guid.NewGuid()}.pdf";

                var clonePathFile = Path.Combine(_tempFolder, newFileName);
                File.Copy(_filePath, clonePathFile);
                _filePath = clonePathFile;

                return this;
            }

            public Crop Split()
            {
                var pngFilePath = Path.Combine(_tempFolder, $"{Guid.NewGuid()}.png");
                CropHackLib.GhostScriptFacade.GetImageFromPdf(_filePath, pngFilePath, _dpi);

                var image = Image.FromFile(pngFilePath);

                var width = Converter.GetPoints(A4.Width - 2 * _borderMM, _dpi);
                var height = Converter.GetPoints(A4.Height - 2 * _borderMM, _dpi);

                _column = (int)Math.Ceiling((double)(_widthUnit / width));
                _row = (int)Math.Ceiling((double)(_heightUnit / height));

                var flag = new Bitmap((int)(_column * width), (int)(_row * height));

                var leftRightOffset = (flag.Width - _widthUnit) / 2;
                var topBottomOffset = (flag.Height - _heightUnit) / 2;

                var flagGraphics = Graphics.FromImage(flag);
                flagGraphics.Clear(Color.White);
                flagGraphics.DrawImage(image, new PointF(leftRightOffset, topBottomOffset));
                pngFilePath = Path.Combine(_tempFolder, $"{Guid.NewGuid()}.png");
                flag.Save(pngFilePath, ImageFormat.Png);


                Image.FromFile(pngFilePath).Split(_column, _row, _tempFolder);

                return this;
            }

            public Crop Join()
            {
                var doc = new PdfDocument();

                var width = Converter.GetPoints(A4.Width, _dpi);
                var height = Converter.GetPoints(A4.Height, _dpi);

                for (int x = 0; x < _column; x++)
                {
                    for (int y = 0; y < _row; y++)
                    {
                        var imagePath = Path.Combine(_tempFolder, $"{y}_{x}.png");
                        var image = XImage.FromFile(imagePath);
                        var page = doc.AddPage();
                        page.Size = PdfSharp.PageSize.A4;
                        var graphics = XGraphics.FromPdfPage(page);
                        graphics.DrawImage(image, _borderUnit, _borderUnit, width - 2 * _borderUnit, height - 2 * _borderUnit);
                        var font = new XFont("Arial", 10, XFontStyle.Regular);
                        graphics.DrawString($"Ряд {GetLetter(y)} стр {x + 1}", font, XBrushes.Black, width / 2 - 30 , height - _borderUnit + 10);

                        var thickness = GlobalSettings.Get("Thickness", 0.5);
                        var pen = new XPen(XColor.FromKnownColor(GlobalSettings.Get("Color", XKnownColor.Black)), thickness);

                        if (GlobalSettings.Get("Line", LineEnum.Solid) == LineEnum.Solid)
                        {
                            pen.DashStyle = XDashStyle.Solid;
                        }
                        else
                        {
                            pen.DashStyle = XDashStyle.DashDot;
                            pen.DashPattern = new double[] { 10, 20, 10 };
                        }

                        graphics.DrawRectangle(pen, new XRect(_borderUnit, _borderUnit, width - 2 * _borderUnit, height - 2 * _borderUnit));

                        image.Dispose();
                    }
                }
                doc.Save(FileResultPath);

                return this;
            }

            public void Dispose()
            {
                Directory.Delete(_tempFolder, true);
            }
        }

    }
}
