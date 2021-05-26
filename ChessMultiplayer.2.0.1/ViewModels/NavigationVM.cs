using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ChessMultiplayer.Views.ViewsServices;

namespace ChessMultiplayer.ViewModels
{
    public class NavigationVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; protected set; }
        public IAbstractViewFabric ViewFabric { get; protected set; }

        public NavigationVM(INavigation navigation, IAbstractViewFabric viewFabric)
        {
            Navigation = navigation;
            ViewFabric = viewFabric;
           
            ToMenuPage = new Command(async () => await GoToMenuPage());
            //ToStartGamePage = new Command(async () => await GotoStartGamePage());
            ToGamePage = new Command(async () => await GoToGamePage());
            ToOptionsPage = new Command(async () => await GoToOptionsPage());
            ToStatisticsPage = new Command(async () => await GoToStatisticsPage());
            ToSavedGamesPage = new Command(async () => await GoToSavedGamesPage());
            ToGameObserverPage = new Command(async () => await GoToGameObserverPage());
            ToPreviousPage = new Command(async () => await GotoPreviousPage());

            ToAutorization = new Command(async () => await GoToAutorization());
            ToRegistration = new Command(async () => await GoToRegistration());
        }
        
        public Command ToMenuPage { get; protected set; }
        //public Command ToStartGamePage { get; protected set; }
        public Command ToGamePage { get; protected set; }
        public Command ToOptionsPage { get; protected set; }
        public Command ToStatisticsPage { get; protected set; }
        public Command ToSavedGamesPage { get; protected set; }
        public Command ToGameObserverPage { get; protected set; }
        public Command ToPreviousPage { get; protected set; }


        public Command ToAutorization { get; protected set; }
        public Command ToRegistration { get; protected set; }


        async Task GoToMenuPage()
        {
            await Navigation.PushAsync(ViewFabric.CreateMenuPage());
        }

        // set some game options
        //async Task GotoStartGamePage()
        //{
        //    await Navigation.PushAsync(ViewFabric.CreateStartGamePage());
        //}

        // game
        async Task GoToGamePage()
        {
            await Navigation.PushAsync(ViewFabric.CreateGamePage());
        }

        // set app options
        async Task GoToOptionsPage()
        {
            await Navigation.PushAsync(ViewFabric.CreateOptionsPage());
        }

        async Task GoToStatisticsPage()
        {
            await Navigation.PushAsync(ViewFabric.CreateStatisticsPage());
        }

        async Task GoToSavedGamesPage()
        {
            await Navigation.PushAsync(ViewFabric.CreateSavedGamesPage());
        }

        async Task GoToGameObserverPage()
        {
            await Navigation.PushAsync(ViewFabric.CreateWatchSavedGamePage());
        }

        async Task GotoPreviousPage()
        {
            await Navigation.PopAsync();
        }


        async Task GoToAutorization()
        {
            await Navigation.PushAsync(ViewFabric.CreateAutorizationPage());
        }
        async Task GoToRegistration()
        {
            await Navigation.PushAsync(ViewFabric.CreateRegistrationPage());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
