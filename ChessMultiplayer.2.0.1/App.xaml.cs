using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessMultiplayer._2._0._1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyInjection();
        }

        protected virtual void DependencyInjection()
        {
            var viewFabric = new Views.ViewFabric();
            var mainPage = viewFabric.CreateMenuPage();

            var navigation = new ViewModels.NavigationVM(mainPage.Navigation, viewFabric); // создаем контекст данных и задаем для него систему навигации


            var viewModel = new ViewModels.MainVM(navigation, new ViewModels.UserVM());

            // задаем созданную ВМ для фабрики, чтобы впоследствии все страницы создавались с именно этой ВМ
            viewFabric.SetBindingContext(viewModel);
            mainPage.BindingContext = viewModel;

            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
