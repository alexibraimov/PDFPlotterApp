using CropPDF.Classes.Helpers;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropPDF.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {

        }

        public int Border
        {
            get => GlobalSettings.Get(nameof(Border), 10, true);
            set
            {
                GlobalSettings.Set(nameof(Border), value);
                OnPropertyChanged(nameof(Border));
            }
        }
        public double Thickness
        {
            get => GlobalSettings.Get(nameof(Thickness), 0.5, true);
            set
            {
                GlobalSettings.Set(nameof(Thickness), value);
                OnPropertyChanged(nameof(Thickness));
            }
        }

        public Array Colors => Enum.GetValues(typeof(XKnownColor));

        public string[] Lines => new string[]
        {
            LineEnum.Solid.GetName(), LineEnum.Dotted.GetName()
        };

        public string Line
        {
            get => GlobalSettings.Get(nameof(Line), LineEnumEx.GetLineEnum(Lines[0]), true).GetName();
            set
            {
                GlobalSettings.Set(nameof(Line), LineEnumEx.GetLineEnum(value));
                OnPropertyChanged(nameof(Line));
            }
        }

        public XKnownColor Color
        {
            get => GlobalSettings.Get(nameof(Color), XKnownColor.Black, true);
            set => GlobalSettings.Set(nameof(Color), value);
        }
    }
}
