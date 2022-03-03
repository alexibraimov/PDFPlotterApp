using Ghostscript.NET;
using System;
using System.IO;

namespace CropHackLib
{
    public static class GhostScriptLibraryHelper
    {
        public static void Check()
        {
            var systemPath = "C:\\Program Files\\gs\\gs9.55.0\\bin\\gsdll64.dll";
            if (File.Exists(systemPath))
            {
                if (!GhostscriptVersionInfo.IsGhostscriptInstalled)
                {
                    throw new Exception("You don't have Ghostscript installed on this machine!");
                }
            }
            else
            {
                Download();
            }
        }

        public static void Download()
        {
            var dllName = "gs9550w64.exe";
            var path = Path.Combine(Environment.CurrentDirectory, dllName);
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
