using CropPDF.Classes.Helpers;
using System;
using System.Threading.Tasks;

namespace CropPDF.ViewModel
{
    public class ExpectationsViewModel : BaseViewModel
    {
        private double _progressValue;
        public event Action<bool> Completed;

        public ExpectationsViewModel()
        {
            PDFHelper.Completed += PDFHelper_Completed;
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

        public void Start(string fileName, bool isOpenFile)
        {
            Task.Run(()=> 
            {
                PDFHelper.CropPDF(fileName, isOpenFile);
            });
        }

        private void PDFHelper_Completed(bool isCompleted)
        {
            Completed?.Invoke(isCompleted);
        }
    }
}
