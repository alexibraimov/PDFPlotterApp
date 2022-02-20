using NetVips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperProj
{
    public static class NetVipsWrapper
    {
        public static void Check()
        {
            if (ModuleInitializer.VipsInitialized)
            {
                var version = NetVips.NetVips.Version(0);

            }
            else
            {
                Console.WriteLine(ModuleInitializer.Exception.Message);
            }
        }
    }
}
