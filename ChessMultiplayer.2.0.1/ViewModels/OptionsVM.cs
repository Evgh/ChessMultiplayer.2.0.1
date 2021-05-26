using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ChessMultiplayer._2._0._1;
using ChessMultiplayer.Views;

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
            if (PurpleStyle)
            {
                if (ShowPrompt)
                {
                    App.Current.Resources["colorConverter"] = new ColorConverter();
                }
                else
                {
                    App.Current.Resources["colorConverter"] = new ProfessionalColorConverter();
                }
            }
            else
            {
                if (ShowPrompt)
                {
                    App.Current.Resources["colorConverter"] = new ColorConverterD();
                }
                else
                {
                    App.Current.Resources["colorConverter"] = new ProfessionalColorConverterD();
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
