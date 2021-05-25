using System;
using System.Collections.Generic;
using System.Text;
using ChessMultiplayer.Services;
using ChessMultiplayer.ViewModels.GameLogic;
using Xamarin.Forms;

namespace ChessMultiplayer.ViewModels
{
    public class GameObserverVM : GameVM
    {
        Command pressCommand;
        public override Command PressCommand => pressCommand ?? (pressCommand = new Command(() => { }));
    }
}
