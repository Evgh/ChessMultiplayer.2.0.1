using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.ViewModels.GameLogic
{
    public class HistoryManager : INotifyPropertyChanged
    {
        MoveParameters lastMove; // указатель на последний совершенный ход

        ObservableCollection<MoveParameters> moveHistory;
        ObservableCollection<MoveParameters> redoHistory;

        ICanDoUndo executer;

        public HistoryManager(ICanDoUndo executer)
        {
            lastMove = new MoveParameters() { ActionNum = 0 };

            moveHistory = new ObservableCollection<MoveParameters>() { lastMove };
            redoHistory = new ObservableCollection<MoveParameters>();

            this.executer = executer;
        }

        public MoveParameters LastMove
        {
            get => lastMove;
            set
            {
                SetLastMove(value);
                OnPropertyChanges();
            }
        }

        public ObservableCollection<MoveParameters> MoveHistory
        {
            get => moveHistory;
            set 
            {
                ResetHistory();

                foreach (var move in value)
                {
                    if (move.ActionNum != 0)
                        Do(move);
                }

                OnPropertyChanges();
            } 
        }

        //public void SetDoUndo(Action<MoveParameters> redo, Action<MoveParameters> undo)
        //{
        //    this.redo = redo;
        //    this.undo = undo;
        //}

        public void ResetHistory()
        {
            moveHistory.Clear();
            redoHistory.Clear();

            lastMove = new MoveParameters() { ActionNum = 0 };
            moveHistory.Add(lastMove);
        }


        #region GameLogic
        public void Do(MoveParameters moveParameters)
        {
            executer.Do(moveParameters);

            moveParameters.ActionNum = (lastMove.ActionNum + 1);

            moveHistory.Add(moveParameters);
            redoHistory.Clear();

            lastMove = moveParameters;
        }

        public void PerformUndo()
        {
            if (lastMove.ActionNum == 0)
            {
                return;
            }

            executer.Undo(lastMove);

            moveHistory.Remove(lastMove);
            redoHistory.Add(lastMove);

            if (moveHistory.Count > 0)
                lastMove = moveHistory[moveHistory.Count - 1];
        }

        public void PerformRedo()
        {
            if (redoHistory.Count == 0)
                return;

            var parameters = redoHistory[redoHistory.Count - 1];
            executer.Do(parameters);

            moveHistory.Add(parameters);
            redoHistory.Remove(parameters);
            lastMove = parameters;
        }

        void SetLastMove(MoveParameters moveParameters)
        {
            if (lastMove == null || moveParameters.ActionNum == -1)
            {
                Debug.WriteLine($"Invalid last move binding error");
                return;
            }

            while (moveParameters.ActionNum != lastMove.ActionNum)
            {
                if (moveParameters.ActionNum < lastMove.ActionNum)
                {
                    SafeUndo();
                }
                else
                {
                    SafeRedo();
                }
            }
        }
        void SafeUndo()
        {
            if (lastMove.ActionNum == 0)
                return;

            executer.Undo(lastMove);
            
            if (moveHistory.Count > 0)
                lastMove = moveHistory[lastMove.ActionNum - 1];
        }
        void SafeRedo()
        {
            if (lastMove.ActionNum + 1 >= moveHistory.Count)
                return;

            lastMove = moveHistory[lastMove.ActionNum + 1];
            executer.Do(lastMove);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
