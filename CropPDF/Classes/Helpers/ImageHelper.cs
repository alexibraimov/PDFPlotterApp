using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropPDF.Classes.Helpers
{
    public static class ImageHelper
    {
        public static Image Crop(this Image image, RectangleF selection)
        {
            var bmp = image as Bitmap;

            // Check if it is a bitmap:
            if (bmp == null)
                throw new ArgumentException("No valid bitmap");

            // Crop the image:
            var cropBmp = bmp.Clone(selection, bmp.PixelFormat);

            // Release the resources:
            image.Dispose();

            return cropBmp;
        }

        public static void Split(this Image img, int column, int row, string folder)
        {
            var big = new Bitmap(img);
            var smallWidth = big.Width / column + (big.Width % column == 0 ? 0 : 1);
            var smallHeight = big.Height / row + (big.Height % row == 0 ? 0 : 1);

            var small = new Bitmap(smallWidth, smallHeight);

            column = 0;
            using (var g = Graphics.FromImage(small))
            {
                for (int i = 0; i < big.Height; i += small.Height)
                {
                    row = 0;
                    for (int j = 0; j < big.Width; j += small.Width) 
                    {
                        g.DrawImage(big, new Rectangle(0, 0, small.Width, small.Height), new Rectangle(j, i, small.Width, small.Height), GraphicsUnit.Pixel);
                        small.Save(Path.Combine(folder, $"{column}_{row}.png"), System.Drawing.Imaging.ImageFormat.Png);
                        row++;
                    }
                    column++;
                }
            }

            big.Dispose();
            small.Dispose();
        }
    }
}
