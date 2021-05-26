using System;
using System.Globalization;
using Xamarin.Forms;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Converters
{
    public class MermaidColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.PaleGreen;
                    case PositionVM.CheckState.CanBeat: return Color.Tomato;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Navy;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.RoyalBlue;
                    case PositionVM.CheckState.CanBeat: return Color.DarkRed;

                }
            }
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
