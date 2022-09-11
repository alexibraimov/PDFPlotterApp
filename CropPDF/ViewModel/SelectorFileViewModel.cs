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
        private int _borderMM ;
        private Visibility _isVisibleNextButton;

        public SelectorFileViewModel()
        {
            IsVisibleNextButton = Visibility.Hidden;
            BorderMM = 10;
        }

        public int BorderMM 
        { 
            get => _borderMM;
            set
            {
                if (value < 0)
                {
                    _borderMM = 0;
                }
                else if (value > 50)
                {
                    _borderMM = 50;
                }
                else
                {
                    _borderMM = value;
                }
                OnPropertyChanged(nameof(BorderMM));
            }
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
