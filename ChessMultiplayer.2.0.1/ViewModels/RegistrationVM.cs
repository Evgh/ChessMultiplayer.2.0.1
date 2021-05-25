using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Services;

namespace ChessMultiplayer.ViewModels
{
    public class RegistrationVM : INotifyPropertyChanged
    {
        public RegistrationVM()
        {
            InvalidValidation = false;
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

        bool invalidValidation;
        public bool InvalidValidation
        {
            get => invalidValidation;
            set
            {
                invalidValidation = value;
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
