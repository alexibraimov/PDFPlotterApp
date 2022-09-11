using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CropHackLib
{
    public class GhostScriptFacade
    {
        static GhostScriptFacade()
        {

        }

        public static void GetImageFromPdf(string inputPdfPath, string outputImagePath, int dpi = 72, ImageFormat imageFormat = null)
        {
            if (imageFormat == null)
            {
                imageFormat = ImageFormat.Png;
            }
            int desired_dpi = dpi;

            using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.CustomSwitches.Add("-dUseCropBox");
                rasterizer.CustomSwitches.Add("-c");
                rasterizer.CustomSwitches.Add("[/CropBox [24 72 559 794] /PAGES pdfmark");
                rasterizer.CustomSwitches.Add("-f");

                rasterizer.Open(inputPdfPath);

                for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    var img = rasterizer.GetPage(desired_dpi, pageNumber);
                    img.Save(outputImagePath, imageFormat);
                }
            }
        }
    }
}
