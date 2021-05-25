using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChessMultiplayer.Models;
using ChessMultiplayer.ViewModels;
using ChessMultiplayer.Views.UserControls;

namespace ChessMultiplayer.Views.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckerBoard : ContentView
    {
        public CheckerBoard()
        {
            InitializeComponent();
        }

        private void Board_BindingContextChanged(object sender, EventArgs e)
        {
            var gameViewModel = BindingContext as GameVM;

            for (int i = 0; i < Board.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < Board.ColumnDefinitions.Count; j++)
                {
                    var check = new Check();
                    check.BindingContext = gameViewModel[i, j];
                    Board.Children.Add(check, j, i);
                }
            }
        }
    }
}