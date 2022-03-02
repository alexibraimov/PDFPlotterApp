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
            if (!ProcessHelper.IsAdmin())
            {
                MessageBox.Show("Программа должна быть запущена от имени администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Process.GetCurrentProcess().Kill();
            }
            base.OnStartup(e);
        }
    }
}
