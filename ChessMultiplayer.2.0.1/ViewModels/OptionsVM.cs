using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ChessMultiplayer._2._0._1;
using ChessMultiplayer.Converters;

namespace ChessMultiplayer.ViewModels
{
    public class OptionsVM : INotifyPropertyChanged
    {
        public OptionsVM()
        {
            ShowPrompt = true;
            PurpleStyle = true;
        }


        bool purpleStyle;
        public bool PurpleStyle
        {
            get => purpleStyle;
            set
            {
                purpleStyle = value;
                OnPropertyChanged();
                SetConverter();
            }
        }

        bool seaStyle;
        public bool SeaStyle
        {
            get => seaStyle;
            set
            {
                seaStyle = value;
                OnPropertyChanged();
                SetConverter();
            }
        }

        bool showPrompt;
        public bool ShowPrompt
        {
            get => showPrompt;
            set
            {
                showPrompt = value;
                OnPropertyChanged();
                SetConverter();
            }
        }

        void SetConverter()
        {
            var currentStatus = GetCurrentStatus();
            App.Current.Resources["colorConverter"] = ColorConverterFabric.GetColorConverter(currentStatus);
        }

        ColorConverterFabric.ColorConverter GetCurrentStatus()
        {
            if (PurpleStyle)
            {
                if (ShowPrompt)
                {
                    return ColorConverterFabric.ColorConverter.Capcake;
                }
                else
                {
                    return ColorConverterFabric.ColorConverter.CapcakePro;
                }
            }
            else
            {
                if (ShowPrompt)
                {
                    return ColorConverterFabric.ColorConverter.Mermaid;
                }
                else
                {
                    return ColorConverterFabric.ColorConverter.MermaidPro;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
