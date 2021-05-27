using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Services;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        public UserVM User { get; protected set; }
        public NavigationVM Navigation { get; protected set; }
        public AutorizationVM Autorization { get; protected set; }
        public RegistrationVM Registration { get; protected set; }
        public OptionsVM Options { get; protected set; }

        public MainVM(NavigationVM navigation = null, UserVM currentUser = null)
        {
            Navigation = navigation;
            User = currentUser;

            Autorization = new AutorizationVM();
            Registration = new RegistrationVM();
            Options = new OptionsVM();

            AutorizationCommand = new Command(Autorize);
            LogoutCommand = new Command(Logout);
            SaveCommand = new Command(async() => await SaveCurrentGame(), () => !User.CurrentUser.IsNull());
            RegistrationCommand = new Command(async() => await Register());
            ObserveGameCommand = new Command(ObserveGame);
            ContinueGameCommand = new Command(ContinueGame);

            Statistics = DatabaseWrap.GetStatistics();

            Autorization.Login = "";
            Autorization.Password = "";
            Autorization.InvalidAutorization = false;
            Registration.Login = "";
            Registration.Password = "";
            Registration.InvalidValidation = false;
        }

        ObservableCollection<UserStatistics> statistics;
        public ObservableCollection<UserStatistics> Statistics 
        {
            get => statistics;
            set
            {
                statistics = value;
                OnPropertyChanged();
            }
        }

        public Command RegistrationCommand { get; protected set; }
        async Task Register()
        {

            if (Registration.Login == "" || Registration.Login == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка регистрации", "Введите логин", "Ok");
                return;
            }
            if (Registration.Login.Length > 50)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка регистрации", "Логин слишком длинный", "Ok");
                return;
            }

            if (Registration.Password == "" || Registration.Password == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка регистрации", "Введите пароль", "Ok");
                return;
            }
            if (Registration.Password.Length > 30)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка регистрации", "Пароль слишком длинный", "Ok");
                return;
            }

            try
            {
                Registration.InvalidValidation = !await DatabaseWrap.RegisterUserAsync(Registration.Login, Registration.Password);

                if (!Registration.InvalidValidation)
                {
                    var user = DatabaseWrap.AutorizeUser(Registration.Login, Registration.Password);
                    User.CurrentUser = user;
                    Navigation.ToPreviousPage.Execute(null);
                }

                Statistics = DatabaseWrap.GetStatistics();
            }
            catch(Exception e)
            {
                Registration.InvalidValidation = true;
                Debug.WriteLine(e.Message);
            }

        }

        public Command AutorizationCommand { get; protected set; }
        void Autorize()
        {
            try
            {
                var user = DatabaseWrap.AutorizeUser(Autorization.Login, Autorization.Password);
                if (user != null)
                {
                    User.CurrentUser = user;
                    Autorization.InvalidAutorization = false;
                    Navigation.ToPreviousPage.Execute(null);

                    Autorization.Login = "";
                    Autorization.Password = "";
                    Autorization.InvalidAutorization = false;
                    Registration.Login = "";
                    Registration.Password = "";
                    Registration.InvalidValidation = false;
                }
                else
                {
                    Autorization.InvalidAutorization = true;
                }
            }
            catch (Exception e)
            {
                Autorization.InvalidAutorization = true;
                Debug.WriteLine(e.Message);
            }
        }

        public Command LogoutCommand { get; protected set; }
        private void Logout()
        {
            User.CurrentUser = new Models.User();

            Autorization.Login = "";
            Autorization.Password = "";
            Autorization.InvalidAutorization = false;
            Registration.Login = "";
            Registration.Password = "";
            Registration.InvalidValidation = false;

            Debug.WriteLine("Logout");
        }

        public Command SaveCommand { get; }
        async Task SaveCurrentGame()
        {
            if (User.CurrentUser.IsNull())
                return;

            try
            {
                Debug.WriteLine("SaveCommand Perform");
                User.SaveCurrentGame();

                await DatabaseWrap.UpdateGame(User.GameViewModel.CurrentGame);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            Statistics = DatabaseWrap.GetStatistics();
        }

        public Command ObserveGameCommand { get; }
        void ObserveGame(object parameter)
        {
            try
            {
                User.GameObserverViewModel.CurrentGame = parameter as Game;
                Navigation.ToGameObserverPage.Execute(null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Command ContinueGameCommand { get; }
        void ContinueGame()
        {
            try
            {
                User.GameViewModel.CurrentGame = User.GameObserverViewModel.CurrentGame;
                Navigation.ToPreviousPage.Execute(null);
                Navigation.ToPreviousPage.Execute(null);
                Navigation.ToGamePage.Execute(null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
