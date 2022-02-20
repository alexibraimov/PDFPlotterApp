using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;

namespace CropHackLib
{
    public class iTextSharpFacade
    {
        public static void GetPdfFromImage(string inputImage, string outputPdf, float width, float height)
        {
            try
            {
                Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);
                using (var stream = new FileStream(outputPdf, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    foreach (var i in Enumerable.Range(0, 1))
                    {
                        var image = Image.GetInstance(new Uri(inputImage));
                        image.ScaleAbsolute(width, height);
                        document.Add(image);
                    }
                    document.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
