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
                OnPropertyChanges();
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanges();
            }
        }

        bool invalidAutorization;
        public bool InvalidAutorization
        {
            get => invalidAutorization;
            set
            {
                invalidAutorization = value;
                OnPropertyChanges();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
