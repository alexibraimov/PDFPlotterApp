using System;
using System.IO;

namespace CropPDF.Classes.Helpers
{
    public static class FileHelper
    {
        public static bool CheckFileSize(string fileName, long maxLength = 1000000000)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            return file.Length <= maxLength;
        }

        public static string Create(string folder)
        {
            var path = Path.Combine(Environment.CurrentDirectory, folder); 
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            return path;
        }
    }
}
