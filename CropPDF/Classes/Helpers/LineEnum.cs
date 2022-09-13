using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropPDF.Classes.Helpers
{
    public enum LineEnum
    {
        Solid = 0,
        Dotted = 1
    }

    public static class LineEnumEx 
    {
        public static string GetName(this LineEnum line)
        {
            switch (line)
            {
                case LineEnum.Solid:
                    return "Сплошная";
                case LineEnum.Dotted:
                    return "Пунктирная";
            }
            return string.Empty;
        }

        public static LineEnum GetLineEnum(string name)
        {
            switch (name)
            {
                case "Сплошная":
                    return LineEnum.Solid;
                case "Пунктирная":
                    return LineEnum.Dotted;
            }
            return LineEnum.Solid;
        }
    }
}
