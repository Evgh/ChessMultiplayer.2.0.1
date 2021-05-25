using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ChessMultiplayer.Services;


namespace ChessMultiplayer.ViewModels
{
    public class AutorizationVM : INotifyPropertyChanged
    {
        public AutorizationVM()
        {
        }

        string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        bool invalidAutorization;
        public bool InvalidAutorization
        {
            get => invalidAutorization;
            set
            {
                invalidAutorization = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
