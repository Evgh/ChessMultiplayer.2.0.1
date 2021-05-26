using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChessMultiplayer.Views
{
    public class ViewFabric : IAbstractViewFabric
    {
        object bindingContext;
        public ViewFabric(object context = null)
        {
            bindingContext = context;
        }

        public void SetBindingContext(object context)
        {
            bindingContext = context;
        }

        public Page CreateMenuPage() => new MenuPage() { BindingContext = bindingContext};
        public Page CreateOptionsPage() => new OptionsPage() { BindingContext = bindingContext };
        public Page CreateStatisticsPage() => new StatisticsPage() { BindingContext = bindingContext };
        public Page CreateSavedGamesPage() => new SavedGamesPage() { BindingContext = bindingContext };
        public Page CreateWatchSavedGamePage() => new GameObserverPage() { BindingContext = bindingContext };

        public Page CreateAutorizationPage() => new AutorizationPage() { BindingContext = bindingContext };
        public Page CreateRegistrationPage() => new RegistrationPage() { BindingContext = bindingContext };

        async void Hi()
        {
            await Application.Current.MainPage.DisplayAlert("1", "1", "Ok");
        }


        public Page CreateGamePage()
        {
            return new GamePage() { BindingContext = bindingContext };
        }
    }
}
