using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperProj
{
    public class ITextSharp
    {
        public static void CropDocument(string file, string newFile,  float x, float y, float width, float height)
        {
            int pageNumber = 1;
            PdfReader reader = new PdfReader(file);
            iTextSharp.text.Rectangle size = new iTextSharp.text.Rectangle(
            x,
            y,
            width,
            height);
            Document document = new Document(size);
            PdfWriter writer = PdfWriter.GetInstance(document,
            new FileStream(newFile,
            FileMode.Create, FileAccess.Write));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            document.NewPage();
            PdfImportedPage page = writer.GetImportedPage(reader,
            pageNumber);
            cb.AddTemplate(page, 0, 0);
            document.Close();
        }

        public static void TrimLeftandRightFoall(string sourceFilePath, string outputFilePath, float cropwidth)
        {

            PdfReader pdfReader = new PdfReader(sourceFilePath);
            float width = pdfReader.GetPageSize(1).Width;
            float height = pdfReader.GetPageSize(1).Height;
            float widthTo_Trim = iTextSharp.text.Utilities.MillimetersToPoints(cropwidth);


            PdfRectangle rectLeftside = new PdfRectangle(widthTo_Trim, widthTo_Trim, width - widthTo_Trim, height - widthTo_Trim);


            using (var output = new FileStream(outputFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                //Create a new document
                Document doc = new Document();

                //Make a copy of the document
                PdfSmartCopy smartCopy = new PdfSmartCopy(doc, output);

                //Open the newly created document
                doc.Open();

                //Loop through all pages of the source document
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    //Get a page
                    var page = pdfReader.GetPageN(i);
                    page.Put(PdfName.MEDIABOX, rectLeftside);
                    var copiedPage = smartCopy.GetImportedPage(pdfReader, i);
                    smartCopy.AddPage(copiedPage);
                }

                doc.Close();

            }
        }
    }
}
