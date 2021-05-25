using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChessMultiplayer.Models;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Views.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Check : ContentView
    {
        public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position",
                                                                                     typeof(PositionVM),
                                                                                     typeof(Check),
                                                                                     null);

        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color",
                                                                             typeof(ColorConverter),
                                                                             typeof(Check),
                                                                             null);

        public Check()
        {
            InitializeComponent();
            check.BindingContext = Position;
        }

        public PositionVM Position
        {
            set
            {
                SetValue(PositionProperty, value);
                check.BindingContext = Position;
            }
            get
            {
                return (PositionVM)GetValue(PositionProperty);
            }
        }

        public ColorConverter Color
        {
            set
            {
                SetValue(ColorProperty, value);
            }
            get
            {
                return (ColorConverter)GetValue(ColorProperty);
            }
        }
    }
}