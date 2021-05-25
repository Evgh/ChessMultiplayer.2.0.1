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
        public enum CheckState { Simple, Selected, CanMove, CanBeat, Check, Checkmate}

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
                OnPropertyChanges("Type");
                OnPropertyChanges("Color");
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
                OnPropertyChanges("State");
                OnPropertyChanges("Color");
            }
        }

        CheckColor color;
        public CheckColor Color
        {
            get => color ?? (color = new CheckColor() { Type = this.Type, State = this.State });
            set
            {
                color = value;
                OnPropertyChanges("Color");
            }
        }


        IFigure figure;
        public IFigure Figure
        {
            get => figure;
            set
            {
                figure = value;
                OnPropertyChanges("Figure");
                OnPropertyChanges("ImagePath");
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
                OnPropertyChanges("Row");
            }
        }

        int column;
        public int Column
        {
            get => column;
            set
            {
                column = value;
                OnPropertyChanges("Column");
            }
        }

        public Command PressCommand { get; }
        public PositionVM(Command pressCommand)
        {
            PressCommand = pressCommand;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

