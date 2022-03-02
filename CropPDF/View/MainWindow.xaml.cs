using CropHackLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CropPDF.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            ctlExpect.ViewModel.Completed += ViewModel_Completed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void ViewModel_Completed(bool isCompleted)
        {
            this.Dispatcher.Invoke(() => 
            {
                if (isCompleted)
                {
                    MessageBox.Show("Файл обработан успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось обработать файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                ctlSelectorFile.Visibility = Visibility.Visible;
                ctlSelectorFile.ViewModel.Reset();
                ctlExpect.Visibility = Visibility.Hidden;
            });
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            if (sender is SelectorFileControl control)
            {
                ctlSelectorFile.Visibility = Visibility.Hidden;
                ctlExpect.Visibility = Visibility.Visible;
                ctlExpect.ViewModel.Start(ctlSelectorFile.ViewModel.FileName, ctlSelectorFile.ViewModel.IsOpenFile);
            }
        }
    }
}
