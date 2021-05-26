using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChessMultiplayer.Views.ViewsServices
{
    public interface IAbstractViewFabric
    {
        Page CreateMenuPage();
        Page CreateGamePage();
        Page CreateOptionsPage();
        Page CreateStatisticsPage();
        Page CreateSavedGamesPage();
        Page CreateWatchSavedGamePage();
        Page CreateRegistrationPage();
        Page CreateAutorizationPage();
    }
}
