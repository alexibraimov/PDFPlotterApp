using CropPDF.Classes.Helpers;
using System;
namespace CropPDF.Classes
{
    public static class A4
    {
        public static float Width = 210;
        public static float Height = 297;

        public static int[] GetCountPage(float width, float height, bool isBorder = true)
        {
            var array = new int[2];
            var border = isBorder ? 10 : 0;
            array[0] = (int)Math.Ceiling(width / Converter.GetPoints(Width - border));
            array[1] = (int)Math.Ceiling(height / Converter.GetPoints(Height - border));

            return array;
        }
    }
}
