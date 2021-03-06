using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Models;
using ChessMultiplayer.Services;
using ChessMultiplayer.ViewModels.GameLogic;
using Xamarin.Essentials;

namespace ChessMultiplayer.ViewModels
{
    public class GameVM : INotifyPropertyChanged
    {
        Game game;
        protected ChessPositionsManager positionsManager;
        protected HistoryManager historyManager;

        Command pressCommand;

        public GameVM()
        {
            InitializeManagers();
            UndoCommand = new Command(historyManager.PerformUndo);
            RedoCommand = new Command(historyManager.PerformRedo);
        }

        void InitializeManagers()
        {
            positionsManager = new ChessPositionsManager(PressCommand);
            positionsManager.GameEnds += OnGameEnds;

            historyManager = new HistoryManager(positionsManager);
        }

        public PositionVM this[int i, int j]
        {
            get => positionsManager[i, j];
        }

        async protected virtual void OnGameEnds(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate();
            }
            catch
            {
                Debug.WriteLine("Uwp cant vibrate");
            }
            await Application.Current.MainPage.DisplayAlert("Король повержен", "Игра окончена", "Ok");
        }

        #region Properties
        public Game CurrentGame // т.е. когда кто-то извне задает игру, мы сбрасываем поле 
        {
            get
            {
                game.Moves.Clear();
                foreach (var move in historyManager.MoveHistory)
                {
                    if (!game.Moves.Contains(move))
                    {
                        game.Moves.Add(move);
                        move.GameID = game.Id;
                    }
                }

                game.HappenedAt = DateTime.Now;
                return game;
            }
            set
            {
                game = value;

                ResetManagers();
                MoveHistory = new ObservableCollection<MoveParameters>(game.Moves);

                OnPropertyChanged("LastMove");
                OnPropertyChanged("MoveHistory");
                OnPropertyChanged();
            }
        }

        // логика истории
        public MoveParameters LastMove
        {
            get => historyManager.LastMove;
            set
            {
                historyManager.LastMove = value;
                OnPropertyChanged("LastMove");
            }
        }

        public ObservableCollection<MoveParameters> MoveHistory
        {
            get => historyManager.MoveHistory;
            set
            {
                historyManager.MoveHistory = value;
                OnPropertyChanged("MoveHistory");
            }
        }

        // логика перемещения по доске
        public ObservableCollection<IFigure> DeadWhites
        {
            get => positionsManager.DeadWhites;
            set
            {
                positionsManager.DeadWhites = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IFigure> DeadBlacks
        {
            get => positionsManager.DeadBlacks;
            set
            {
                positionsManager.DeadBlacks = value;
                OnPropertyChanged();
            }
        }

        protected PositionVM SelectedPosition
        {
            get => positionsManager.SelectedPosition;
            set
            {
                positionsManager.SelectedPosition = value;
                OnPropertyChanged("SelectedPosition");
            }
        }
        #endregion


        #region Commands
        public Command UndoCommand { get; }
        public Command RedoCommand { get; }
        public virtual Command PressCommand => pressCommand ?? (pressCommand = new Command((obj) => Press(obj)));
        #endregion


        #region Press Logic
        enum GameState { NewSelection, RemoveSelection, Move, DefaultState }
        GameState GetCurrentState(PositionVM pressedPosition)
        {
            if (SelectedPosition == null && pressedPosition.Figure != null)
                return GameState.NewSelection;

            else if (SelectedPosition != null && (pressedPosition.State == PositionVM.CheckState.Simple || pressedPosition.State == PositionVM.CheckState.Selected))
                return GameState.RemoveSelection;

            else if (SelectedPosition != pressedPosition && (pressedPosition.State == PositionVM.CheckState.CanMove || pressedPosition.State == PositionVM.CheckState.CanBeat))
                return GameState.Move;
            else
                return GameState.DefaultState;
        }

        void Press(object parameter)
        {
            var pressedPosition = parameter as PositionVM;
            GameState gameState = GetCurrentState(pressedPosition);

            switch (gameState)
            {
                case GameState.NewSelection :
                    {
                        SelectedPosition = pressedPosition;
                        positionsManager.DrawMoveOpportunities();
                    } break;
                case GameState.RemoveSelection:
                    {
                        SelectedPosition = null;
                        positionsManager.RemoveSelection();

                    } break;
                case GameState.Move:
                    {
                        bool didSomeoneDied = pressedPosition.State == PositionVM.CheckState.CanBeat;

                        MoveParameters moveParameters = new MoveParameters()
                        {
                            StartRow = SelectedPosition.Row,
                            StartColumn = SelectedPosition.Column,
                            TargetRow = pressedPosition.Row,
                            TargetColumn = pressedPosition.Column,
                            DidSomeoneDied = didSomeoneDied
                        };

                        historyManager.Do(moveParameters);                       
                    } break;
            }
        }

        #endregion

        void ResetManagers()
        {
            positionsManager.StartNewGame();
            historyManager.ResetHistory();

            OnPropertyChanged("DeadWhites");
            OnPropertyChanged("DeadBlacks");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public ChessPositionsManager ChessPositionsManager
        {
            get => default;
            set
            {
            }
        }

        public HistoryManager HistoryManager
        {
            get => default;
            set
            {
            }
        }
    }
}
