using Ghostscript.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropHackLib
{
    public static class GhostScriptLibraryHelper
    {
        public static void Check()
        {
            var dllName = "gsdll64.dll";
            var path = Path.Combine(Environment.CurrentDirectory, dllName);
            var systemPath = "C:\\Program Files\\gs\\gs9.55.0\\bin";
            var systemDllPath = Path.Combine(systemPath, dllName);
            if (File.Exists(path))
            {
                if (!Directory.Exists(systemPath))
                {
                    Directory.CreateDirectory(systemPath);
                }

                if (!File.Exists(systemDllPath))
                {
                    File.Copy(path, systemDllPath);
                }
            }
            else
            {
                throw new Exception("You don't have Ghostscript installed on this machine!");
            }
            if (!GhostscriptVersionInfo.IsGhostscriptInstalled)
            {
                throw new Exception("You don't have Ghostscript installed on this machine!");
            }
        }

    }
}
