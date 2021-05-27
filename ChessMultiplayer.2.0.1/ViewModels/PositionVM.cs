using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ChessMultiplayer.Models;


namespace ChessMultiplayer.ViewModels
{
    public class PositionVM : INotifyPropertyChanged
    {
        public enum CheckType { White, Black }
        public enum CheckState { Simple, Selected, CanMove, CanBeat, Check, Checkmate, Evolutionate, Сastling}

        public class CheckColor 
        { 
            public CheckType Type { get; set; } 
            public CheckState State { get; set; }
        }

        CheckType type;
        public CheckType Type 
        {
            get => type; 
            set
            {
                type = value;
                if (color != null) color.Type = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("Color");
            }
        }

        CheckState state;
        public CheckState State
        {
            get => state;
            set
            {
                state = value;
                if (color != null) color.State = value;
                OnPropertyChanged("State");
                OnPropertyChanged("Color");
            }
        }

        CheckColor color;
        public CheckColor Color
        {
            get => color ?? (color = new CheckColor() { Type = this.Type, State = this.State });
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }


        IFigure figure;
        public IFigure Figure
        {
            get => figure;
            set
            {
                figure = value;
                OnPropertyChanged("Figure");
                OnPropertyChanged("ImagePath");
            }
        }

        public string ImagePath
        {
            get
            {
                if (figure != null)
                    return figure.ImagePath;
                else
                    return "";
            }
        }

        int row;
        public int Row
        {
            get => row;
            set
            {
                row = value;
                OnPropertyChanged("Row");
            }
        }

        int column;
        public int Column
        {
            get => column;
            set
            {
                column = value;
                OnPropertyChanged("Column");
            }
        }

        public Command PressCommand { get; }
        public PositionVM(Command pressCommand)
        {
            PressCommand = pressCommand;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

