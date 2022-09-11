using CropPDF.Classes.Helpers;
using System;
namespace CropPDF.Classes
{
    public static class A4
    {
        public static float Width = 210;
        public static float Height = 297;

        public static int[] GetCountPage(float width, float height, float border = 0)
        {
            var array = new int[2];
            array[0] = (int)Math.Ceiling(width / Converter.GetPoints(Width - border));
            array[1] = (int)Math.Ceiling(height / Converter.GetPoints(Height - border));

            return array;
        }


        public static Cell GetCell(float widthMM, float heigthMM, float borderMM)
        {
            return new Cell((int)Math.Ceiling(widthMM / (Width - borderMM)), (int)Math.Ceiling(heigthMM / (Height - borderMM)));
        }

        public class Cell
        {
            public Cell(int column, int row)
            {
                Column = column;
                Row = row;
            }
            public int Column { get; }
            public int Row { get; }
        }
    }
}
