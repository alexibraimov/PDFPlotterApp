using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CropPDF.ViewModel
{
    public class SelectorFileViewModel : BaseViewModel
    {
        private string _fileName;
        private bool _isOpenFile;
        private Visibility _isVisibleNextButton;

        public SelectorFileViewModel()
        {
            IsVisibleNextButton = Visibility.Hidden;
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public bool IsOpenFile
        {
            get => _isOpenFile;
            set
            {
                _isOpenFile = value;
                OnPropertyChanged(nameof(IsOpenFile));
            }
        }

        public Visibility IsVisibleNextButton
        {
            get => _isVisibleNextButton;
            set
            {
                _isVisibleNextButton = value;
                OnPropertyChanged(nameof(IsVisibleNextButton));
            }
        }

        public void Reset()
        {
            FileName = string.Empty;
            IsVisibleNextButton = Visibility.Hidden;
        }
    }
}
