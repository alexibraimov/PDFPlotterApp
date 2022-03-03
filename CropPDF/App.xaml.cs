using CropHackLib;
using System.Diagnostics;
using System.Windows;

namespace CropPDF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            GhostScriptLibraryHelper.Check(); 
            base.OnStartup(e);
        }
    }
}
