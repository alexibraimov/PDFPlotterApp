namespace CropPDF.Classes.Helpers
{
    public static class Converter
    {
        public static int GetDpi(float unit)
        {
            return 0;
        }

        public static float GetMillimeters(float points, float dpi = 72F)
        {
            return points * 25.4F / dpi;
        }

        public static float GetPoints(float mm, float dpi = 72F)
        {
            return mm * dpi / 25.4F;
        }
    }
}
