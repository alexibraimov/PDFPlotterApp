using CropPDF.Classes.Helpers;
using CropPDF.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CropPDF.View
{
    public partial class SelectorFileControl : UserControl
    {
        public SelectorFileViewModel ViewModel;
        public event EventHandler NextButtonClicked;
        public SelectorFileControl()
        {
            InitializeComponent();
            ViewModel = new SelectorFileViewModel();

            DataContext = ViewModel;
        }

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF file|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                if (FileHelper.CheckFileSize(openFileDialog.FileName))
                {
                    ViewModel.FileName = openFileDialog.FileName;
                    ViewModel.IsVisibleNextButton = Visibility.Visible;
                }
                else
                {
                    _ = MessageBox.Show("Размер файла не должен превышать 1 Гб", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            NextButtonClicked?.Invoke(this, new EventArgs());
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
