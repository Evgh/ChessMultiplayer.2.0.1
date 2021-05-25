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
            if (ShowPrompt)
            {
                App.Current.Resources["colorConverter"] = new ColorConverter();
            }
            else
            {
                App.Current.Resources["colorConverter"] = new ProfessionalColorConverter();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
