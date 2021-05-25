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
        }


        public Command RegistrationCommand { get; protected set; }
        async Task Register()
        {
            Registration.InvalidValidation = ! await DatabaseWrap.RegisterUserAsync(Registration.Login, Registration.Password);

            if (!Registration.InvalidValidation)
            {
                var user = DatabaseWrap.AutorizeUser(Registration.Login, Registration.Password);
                User.CurrentUser = user;
                Navigation.ToMenuPage?.Execute(null);
            } 
        }

        public Command AutorizationCommand { get; protected set; }
        void Autorize()
        {
            var user = DatabaseWrap.AutorizeUser(Autorization.Login, Autorization.Password);
            if (user != null)
            {
                User.CurrentUser = user;
                Autorization.InvalidAutorization = false;
                Navigation.ToMenuPage?.Execute(null);

                //User.SelectedGame = (user.Games as ObservableCollection<Game>)[0];
                //User.SelectedObserveGame = (user.Games as ObservableCollection<Game>)[0];

            }
            else
            {
                Autorization.InvalidAutorization = true;
            }
        }

        public Command LogoutCommand { get; protected set; }
        private void Logout()
        {
            User.CurrentUser = new Models.User();
            Debug.WriteLine("Logout");
        }

        public Command SaveCommand { get; }
        async Task SaveCurrentGame()
        {
            if (User.CurrentUser.IsNull())
                return;

            Debug.WriteLine("SaveCommand Perform");

            if (!User.Games.Contains(User.GameViewModel.CurrentGame))
            {
                User.GameViewModel.CurrentGame.UserID = User.CurrentUser.Id;
                User.Games.Add(User.GameViewModel.CurrentGame);
            }

            await DatabaseWrap.UpdateGame(User.GameViewModel.CurrentGame);
        }

        public Command ObserveGameCommand { get; }
        void ObserveGame(object parameter)
        {
            User.GameObserverViewModel.CurrentGame = parameter as Game;
            Navigation.ToGameObserverPage.Execute(null);
        }

        public Command ContinueGameCommand { get; }
        void ContinueGame()
        {
            User.GameViewModel.CurrentGame = User.GameObserverViewModel.CurrentGame;
            Navigation.ToMenuPage.Execute(null);
            Navigation.ToGamePage.Execute(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
