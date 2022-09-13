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
            get =>((LineEnum)GlobalSettings.Get(nameof(Line), (int)LineEnumEx.GetLineEnum(Lines[0]), true)).GetName();
            set
            {
                GlobalSettings.Set(nameof(Line), (int)LineEnumEx.GetLineEnum(value));
                OnPropertyChanged(nameof(Line));
            }
        }

        public XKnownColor Color
        {
            get => (XKnownColor) GlobalSettings.Get(nameof(Color), (int)XKnownColor.Black, true);
            set => GlobalSettings.Set(nameof(Color), (int)value);
        }
    }
}
