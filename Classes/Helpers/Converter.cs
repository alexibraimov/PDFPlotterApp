namespace CropPDF.Classes.Helpers
{
    public static class Converter
    {
        public static float GetMillimeters(float points)
        {
            return points * 25.4F / 72F;
        }

        public static float GetPoints(float mm)
        {
            return mm * 72F / 25.4F;
        }
    }
}
