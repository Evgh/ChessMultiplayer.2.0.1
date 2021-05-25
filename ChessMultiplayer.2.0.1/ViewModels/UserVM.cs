using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ChessMultiplayer.Models;
using ChessMultiplayer.Services;

namespace ChessMultiplayer.ViewModels
{
    public class UserVM : INotifyPropertyChanged
    {
        User currentUser;
        public GameVM GameViewModel { get; protected set; }
        public GameObserverVM GameObserverViewModel { get; protected set; }


        public UserVM()
        {
            currentUser = new User();
            GameViewModel = new GameVM();
            GameObserverViewModel = new GameObserverVM();

            NewGameCommand = new Command(NewGame);
        }

        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                NewGame();

                OnPropertyChanged("IsGuest");
                OnPropertyChanged("IsAuthorized");
                OnPropertyChanged("UserId");

                OnPropertyChanged("Games");
                OnPropertyChanged("SelectedGame");

                OnPropertyChanged();
            }
        }

        public bool IsGuest => currentUser.IsNull();
        public bool IsAuthorized => !currentUser.IsNull();
        public string UserId => currentUser.Id ?? "Гость";


        public ObservableCollection<Game> Games
        {
            get => currentUser.Games as ObservableCollection<Game>;
            set
            {
                currentUser.Games = value;
                OnPropertyChanged();
            }
        }

        public Game SelectedGame
        {
            get 
            {
                GameViewModel.CurrentGame.UserID = currentUser.Id;
                return GameViewModel.CurrentGame;
            } 
            set
            {
                GameViewModel.CurrentGame = value;
                OnPropertyChanged();
            }
        }

        public Game SelectedObserveGame
        {
            get
            {
                GameObserverViewModel.CurrentGame.UserID = currentUser.Id;
                return GameObserverViewModel.CurrentGame;
            }
            set
            {
                GameObserverViewModel.CurrentGame = value;
                OnPropertyChanged();
            }
        }

        public Command NewGameCommand { get; }
        void NewGame()
        {
            SelectedGame = new Game() { User = currentUser };
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
