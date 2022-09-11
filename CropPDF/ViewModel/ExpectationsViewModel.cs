using CropPDF.Classes.Helpers;
using System;
using System.Threading.Tasks;

namespace CropPDF.ViewModel
{
    public class ExpectationsViewModel : BaseViewModel
    {
        private double _progressValue;
        public event Action<bool> Completed;
        public string Error = string.Empty;
        public ExpectationsViewModel()
        {
            PDFHelper.Completed += PDFHelper_Completed;
            PDFHelper.Error += PDFHelper_Error;
        }

        private void PDFHelper_Error(string error)
        {
            this.Error = error;
        }

        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public void Start(string fileName, bool isOpenFile, int border)
        {
            Task.Run(()=> 
            {
                PDFHelper.CropPDF(fileName, isOpenFile, border);
            });
        }

        private void PDFHelper_Completed(bool isCompleted)
        {
            Completed?.Invoke(isCompleted);
        }
    }
}
