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

namespace HelperProj
{
    public static class GhostWripper
    {
        public static void Check()
        {
            if (!GhostscriptVersionInfo.IsGhostscriptInstalled)
            {
                throw new Exception("You don't have Ghostscript installed on this machine!");
            }
        }

        public static void Start(string inputPdfPath)
        {
            int desired_dpi = 300;

            string outputPath = Path.GetDirectoryName(inputPdfPath);
        
            using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.CustomSwitches.Add("-dUseCropBox");
                rasterizer.CustomSwitches.Add("-c");
                rasterizer.CustomSwitches.Add("[/CropBox [24 72 559 794] /PAGES pdfmark");
                rasterizer.CustomSwitches.Add("-f");

                rasterizer.Open(inputPdfPath);

                for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    string pageFilePath = Path.Combine(outputPath, "Page-" + pageNumber.ToString() + "test.png");

                    Image img = rasterizer.GetPage(desired_dpi, pageNumber);
                    img.Save(pageFilePath, ImageFormat.Png);
                }
            }
        }
    }
}
